import { PagedResultDto } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { AttributeDetailsComponent } from './attribute-details.component';
import { FormControl, FormGroup } from '@angular/forms';
import { AttributeType } from '@proxy/ecommerce/attributies';
import { ProductAttributeDto, ProductAttributeInListDto } from '@proxy/catalog/product-attributes';
import { ProductAttributeService } from '@proxy/catalogs/product-attributes';

interface cate {
  name: string;
  value: string;
}
@Component({
  selector: 'app-Attribute',
  templateUrl: './attribute.component.html',
  styleUrls: ['./attribute.component.scss'],
})
export class AttributeComponent implements OnInit, OnDestroy {
  private ngUnsubcribe = new Subject<void>();
  blockedPanel: boolean = false;

  items: ProductAttributeInListDto[] = [];

  //xoa nhieu sp
  public selectedItems: ProductAttributeInListDto[] = [];

  //paging variable
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;

  // filter Attribute
  keyword: string = '';

  formSearch: FormGroup;

  constructor(
    private _AttributeService: ProductAttributeService,
    private dialogService: DialogService
  ) {}
  ngOnDestroy(): void {
    this.ngUnsubcribe.next();
    this.ngUnsubcribe.complete();
  }

  ngOnInit(): void {
    this.InitData();

    this.formSearch = new FormGroup({
      selectedCity: new FormControl<cate | null>(null),
    });
  }

  // end load data section

  InitData() {
    this.toggleBlockUI(true);
    this._AttributeService
      .getListAll()
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: (response: ProductAttributeInListDto[]) => {
          this.items = response;
          this.toggleBlockUI(false);
        },
        error: error => {
          alert('Loi load Attribute init data list');
          console.log(error);
          this.toggleBlockUI(false);
        },
      });
  }

  loadData() {
    this.toggleBlockUI(true);
    if (this.keyword.length == 0 || this.keyword == ' ') {
      this.InitData();
    } else {
      this.toggleBlockUI(true);
      this._AttributeService
        .getListFilter({
          keyword: this.keyword,
          skipCount: this.skipCount,
          maxResultCount: this.maxResultCount,
        })
        .pipe(takeUntil(this.ngUnsubcribe))
        .subscribe({
          next: (response: PagedResultDto<ProductAttributeInListDto>) => {
            this.items = response.items;
            this.totalCount = response.totalCount;
            this.toggleBlockUI(false);
          },
          error: error => {
            alert('Loi load Attribute');
            console.log(error);
            this.toggleBlockUI(false);
          },
        });
    }
  }

  // end load data section

  // show model section

  showAddModel() {
    const ref = this.dialogService.open(AttributeDetailsComponent, {
      header: 'Add new Attribute',
      width: '70%',
    });

    ref.onClose.subscribe((data: ProductAttributeDto) => {
      if (data) {
        this.loadData();
      }
    });
  }

  showEditModel(id: string) {
    alert('showmodel');
    const ref = this.dialogService.open(AttributeDetailsComponent, {
      header: 'Add new Attribute',
      width: '70%',
      data: {
        id: id,
      },
    });

    ref.onClose.subscribe((data: ProductAttributeDto) => {
      if (data) {
        this.loadData();
      }
    });
  }

  // end show model section

  // delete section

  DeleteListItems() {
    if (this.selectedItems.length == 0) {
      alert('Phải chọn ít nhất một bản ghi');
      return;
    }
    var ids = [];
    this.selectedItems.forEach(element => {
      ids.push(element.id);
    });

    this.deleteItemsConfirmed(ids);
  }

  deleteItemsConfirmed(ids: string[]) {
    this.toggleBlockUI(true);
    this._AttributeService
      .deleteMultiple(ids)
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: () => {
          alert('Xóa thành công');
          this.loadData();
          this.selectedItems = [];
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  DeleteAttribute(id: string) {
    this.toggleBlockUI(true);
    this._AttributeService
      .delete(id)
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: () => {
          alert('Xóa thành công');
          this.loadData();
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  getAttributeTypeName(value: number) {
    return AttributeType[value];
  }

  // end delete section

  pageChanged(event: any): void {
    this.skipCount = (event.paging - 1) * this.maxResultCount;
    this.maxResultCount = event.rows;
    this.loadData();
  }

  toggleBlockUI(enable: boolean) {
    if (enable == true) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }
}
