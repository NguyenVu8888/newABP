import { PagedResultDto } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { DialogService } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { ProductDetailsComponent } from './product-details.component';
import { FormControl, FormGroup } from '@angular/forms';
import { ProductAttributeComponent } from './product-attribute.component';
import { ProductDto, ProductInListDto } from '@proxy/catalog/products';
import { ProductsService } from '@proxy/catalogs/products';
import { ProductCategoriesService } from '@proxy/catalogs/product-categories';
import { ProductCategoryInListDto } from '@proxy/catalog/product-categories';
import { ProductType } from '@proxy/ecommerce/products';

interface cate {
  name: string;
  value: string;
}
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
})
export class ProductComponent implements OnInit, OnDestroy {
  private ngUnsubcribe = new Subject<void>();
  blockedPanel: boolean = false;

  items: ProductInListDto[] = [];

  //xoa nhieu sp
  public selectedItems: ProductInListDto[] = [];

  //paging variable
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;

  // filter product
  productCategorise: any[] = [];
  keyword: string = '';
  categoryId: string = '';

  formSearch: FormGroup;

  constructor(
    private _productServie: ProductsService,
    private _productCategoryService: ProductCategoriesService,
    private dialogService: DialogService
  ) {}
  ngOnDestroy(): void {
    this.ngUnsubcribe.next();
    this.ngUnsubcribe.complete();
  }

  ngOnInit(): void {
    this.loadProductCategories();
    this.InitData();

    this.formSearch = new FormGroup({
      selectedCity: new FormControl<cate | null>(null),
    });
  }

  // end load data section

  InitData() {
    this.toggleBlockUI(true);
    this._productServie
      .getListAll()
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: (response: ProductInListDto[]) => {
          this.items = response;
          this.totalCount = response.length;
          this.toggleBlockUI(false);
        },
        error: error => {
          alert('Loi load product init data list');
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
      this._productServie
        .getListFilter({
          keyword: this.keyword,
          categoryId: this.categoryId,
          skipCount: this.skipCount,
          maxResultCount: this.maxResultCount,
        })
        .pipe(takeUntil(this.ngUnsubcribe))
        .subscribe({
          next: (response: PagedResultDto<ProductInListDto>) => {
            this.items = response.items;
            this.totalCount = response.totalCount;
            this.toggleBlockUI(false);
          },
          error: error => {
            alert('Loi load product');
            console.log(error);
            this.toggleBlockUI(false);
          },
        });
    }
  }

  loadProductCategories() {
    this._productCategoryService.getListAll().subscribe({
      next: (response: ProductCategoryInListDto[]) => {
        response.forEach(element => {
          this.productCategorise.push({
            value: element.id,
            name: element.name,
          });
        });
      },
      error: error => {
        console.log(error);
      },
    });
  }

  getProductTypeName(value: number) {
    return ProductType[value];
  }

  // end load data section

  // show model section

  showAddModel() {
    const ref = this.dialogService.open(ProductDetailsComponent, {
      header: 'Add new product',
      width: '70%',
    });

    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
      }
    });
  }

  showEditModel(id: string) {
    const ref = this.dialogService.open(ProductDetailsComponent, {
      header: 'Add new product',
      width: '70%',
      data: {
        id: id,
      },
    });

    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
      }
    });
  }

  manageProductAttribute(id: string) {
    const ref = this.dialogService.open(ProductAttributeComponent, {
      header: 'Quản lý thuộc tính sản phẩm',
      width: '70%',
      data: {
        id: id,
      },
    });

    ref.onClose.subscribe((data: ProductDto) => {
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
    this._productServie
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

  DeleteProduct(id: string) {
    this.toggleBlockUI(true);
    this._productServie
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
