import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject, forkJoin, takeUntil } from 'rxjs';
import { UtilityService } from '../../shared/services/utility.service';
import { DomSanitizer } from '@angular/platform-browser';
import { attributeTypeOptions } from '@proxy/ecommerce/attributies';
import { ProductAttributeService } from '@proxy/catalogs/product-attributes';
import {
  CreateUpdateProductAttributeDto,
  ProductAttributeDto,
} from '@proxy/catalog/product-attributes';

@Component({
  selector: 'app-attribute-details',
  templateUrl: './attribute-details.component.html',
  styleUrls: [],
})
export class AttributeDetailsComponent implements OnInit, OnDestroy {
  isSubmited: boolean = false;
  private ngUnsubcribe = new Subject<void>();
  blockedPanel: boolean = false;
  btnDisabled: boolean = false;
  btnEdit: boolean = this.utilService.isEmpty(this.config.data?.id);

  //form
  public formAttributeProduct: FormGroup;

  // dropdown product
  dataTypes: any[] = [];

  selectedEntity = {} as ProductAttributeDto;

  constructor(
    private _attributeServie: ProductAttributeService,
    private fb: FormBuilder,
    private config: DynamicDialogConfig,
    private ref: DynamicDialogRef,
    private utilService: UtilityService,

    private cd: ChangeDetectorRef,
    private sanitizer: DomSanitizer
  ) {}

  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
    this.ngUnsubcribe.next();
    this.ngUnsubcribe.complete();
  }

  ngOnInit(): void {
    this.buildForm();
    this.loadAttributeTypes();
    this.addDataform();
  }

  //Load data to form if user chose edit
  addDataform() {
    if (this.utilService.isEmpty(this.config.data?.id) == true) {
      this.toggleBlockUI(false);
    } else {
      this.loadFromDetails(this.config.data?.id);
    }
  }

  loadFromDetails(id: string) {
    this.toggleBlockUI(true);
    this._attributeServie
      .get(id)
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: (response: ProductAttributeDto) => {
          this.selectedEntity = response;
          this.buildForm();
          this.toggleBlockUI(false);
        },
        error: error => {
          this.toggleBlockUI(false);
        },
      });
  }

  private buildForm() {
    this.formAttributeProduct = this.fb.group({
      label: new FormControl(
        this.selectedEntity.label || null,
        Validators.compose([Validators.required, Validators.maxLength(250)])
      ),
      code: new FormControl(this.selectedEntity.code || null, [Validators.required]),
      dataType: new FormControl(this.selectedEntity.type || null, Validators.required),
      sortOrder: new FormControl(this.selectedEntity.sortOrder || null, Validators.required),
      visibility: new FormControl(this.selectedEntity.vissibility || true),
      isActive: new FormControl(this.selectedEntity.isActive || true),
      note: new FormControl(this.selectedEntity.note || null),
      isRequired: new FormControl(this.selectedEntity.isRequired || true),
      isUnique: new FormControl(this.selectedEntity.isUnique || false),
    });
  }

  //end load du lieu

  //luu du lieu xuong

  saveChange() {
    var dataSend: CreateUpdateProductAttributeDto = {
      label: this.formAttributeProduct.value.label,
      code: this.formAttributeProduct.value.code,
      type: this.formAttributeProduct.value.dataType,
      sortOrder: this.formAttributeProduct.value.sortOrder,
      vissibility: this.formAttributeProduct.value.visibility,
      isActive: this.formAttributeProduct.value.isActive,
      isRequired: this.formAttributeProduct.value.isRequired,
      isUnique: this.formAttributeProduct.value.isUnique,
      note: this.formAttributeProduct.value.note,
    };

    this.isSubmited = true;
    if (!this.formAttributeProduct.invalid && this.isSubmited == true) {
      if (this.utilService.isEmpty(this.config.data?.id) == true) {
        this._attributeServie
          .create(dataSend)
          .pipe(takeUntil(this.ngUnsubcribe))
          .subscribe({
            next: () => {
              this.ref.close(this.formAttributeProduct.value);
            },
            error: err => {
              alert(err.error.error.message);
              this.toggleBlockUI(false);
            },
          });
      } else {
        this._attributeServie
          .update(this.config.data?.id, dataSend)
          .pipe(takeUntil(this.ngUnsubcribe))
          .subscribe({
            next: () => {
              this.ref.close(this.formAttributeProduct.value);
            },
            error: () => {
              this.toggleBlockUI(false);
            },
          });
      }
    } else {
      alert('not now');
    }
  }

  loadAttributeTypes() {
    attributeTypeOptions.forEach(element => {
      this.dataTypes.push({
        value: element.value,
        label: element.key,
      });
    });
  }
  // end luu du lieu

  toggleBlockUI(enable: boolean) {
    if (enable == true) {
      this.blockedPanel = true;
      this.btnDisabled = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
        this.btnDisabled = false;
      }, 1000);
    }
  }
}
