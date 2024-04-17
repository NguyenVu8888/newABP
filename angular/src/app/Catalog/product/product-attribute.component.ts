import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject, forkJoin, takeUntil } from 'rxjs';

import { AttributeType } from '@proxy/ecommerce/attributies';
import { ProductAttributeService } from '@proxy/catalogs/product-attributes';
import { ProductsService } from '@proxy/catalogs/products';
import { ProductAttributeInListDto } from '@proxy/catalog/product-attributes';
import {
  ProductAttributeValueDto,
  addUpdateProductAttributeDto,
} from '@proxy/catalog/products/attributes';

@Component({
  selector: 'app-product-attribute',
  templateUrl: './product-attribute.component.html',
  styleUrls: [],
})
export class ProductAttributeComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  btnDisabled = false;
  public form: FormGroup;

  attributes: any[] = [];
  fullAttributes: any[] = [];
  productAttributes: any[] = [];
  showDateTimeControl: boolean = false;
  showDecimalControl: boolean = false;
  showIntControl: boolean = false;
  showVarcharControl: boolean = false;
  showTextControl: boolean = false;

  constructor(
    private _productAttributeService: ProductAttributeService,
    private productService: ProductsService,
    private fb: FormBuilder,
    private config: DynamicDialogConfig,
    private ref: DynamicDialogRef
  ) {}

  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    this.buildForm();
    this.initFormData();
  }

  initFormData() {
    //Load data to form
    this.toggleBlockUI(true);
    this._productAttributeService
      .getListAll()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: any) => {
          //Push data to dropdown
          this.fullAttributes = response;
          var attributesList = response as ProductAttributeInListDto[];
          attributesList.forEach(element => {
            this.attributes.push({
              value: element.id,
              label: element.label,
            });
          });
          this.loadFormDetails(this.config.data?.id);
        },
        error: err => {
          console.log(err);
          this.toggleBlockUI(false);
        },
      });
  }

  loadFormDetails(id: string) {
    this.toggleBlockUI(true);
    this.productService
      .getListProductAttributeAll(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: ProductAttributeValueDto[]) => {
          this.productAttributes = response;
          console.log(response);
          this.buildForm();
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  saveChange() {
    this.toggleBlockUI(true);
    var datasend: addUpdateProductAttributeDto = {
      productId: this.form.value.productId,
      attributeId: this.form.value.attributeId,
      dateTimeValue: this.form.value.dateTimeValue,
      decimalValue: this.form.value.decimalValue,
      intValue: this.form.value.intValue,
      varcharValue: this.form.value.varcharValue,
      textValue: this.form.value.textValue,
    };
    console.log(datasend);
    this.productService
      .addAttibute(datasend)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.toggleBlockUI(false);
          this.loadFormDetails(this.config.data.id);
        },
        error: err => {
          this.toggleBlockUI(false);
        },
      });
  }

  private buildForm() {
    this.form = this.fb.group({
      productId: new FormControl(this.config.data.id),
      attributeId: new FormControl(null, Validators.required),
      dateTimeValue: new FormControl(null),
      decimalValue: new FormControl(null),
      intValue: new FormControl(null),
      varcharValue: new FormControl(null),
      textValue: new FormControl(null),
    });
  }
  removeItem(attribute: ProductAttributeValueDto) {
    var id = '';
    if (attribute.type == AttributeType.Date) {
      id = attribute.dateTimeId;
    } else if (attribute.type == AttributeType.Decimal) {
      id = attribute.decimalId;
    } else if (attribute.type == AttributeType.Int) {
      id = attribute.intId;
    } else if (attribute.type == AttributeType.Text) {
      id = attribute.textId;
    } else if (attribute.type == AttributeType.Varchar) {
      id = attribute.varcharId;
    }
    this.deleteItemsConfirmed(attribute, id);
  }

  selectAttribute(event: any) {
    var dataType = this.fullAttributes.filter(x => x.id == event.value)[0].type;
    this.showDateTimeControl = false;
    this.showDecimalControl = false;
    this.showIntControl = false;
    this.showTextControl = false;
    this.showVarcharControl = false;
    if (dataType == AttributeType.Date) {
      this.showDateTimeControl = true;
    } else if (dataType == AttributeType.Decimal) {
      this.showDecimalControl = true;
    } else if (dataType == AttributeType.Int) {
      this.showIntControl = true;
    } else if (dataType == AttributeType.Text) {
      this.showTextControl = true;
    } else if (dataType == AttributeType.Varchar) {
      this.showVarcharControl = true;
    }
  }
  deleteItemsConfirmed(attribute: ProductAttributeValueDto, id: string) {
    this.toggleBlockUI(true);
    this.productService
      .removeAttribute(attribute.attributeId, id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.loadFormDetails(this.config.data?.id);
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }
  getDataTypeName(value: number) {
    return AttributeType[value];
  }
  getValueByType(attribute: ProductAttributeValueDto, value: number) {
    if (attribute.type == AttributeType.Date) {
      return attribute.dateTimeValue;
    } else if (attribute.type == AttributeType.Decimal) {
      return attribute.decimalValue;
    } else if (attribute.type == AttributeType.Int) {
      return attribute.intValue;
    } else if (attribute.type == AttributeType.Text) {
      return attribute.textValue;
    } else if (attribute.type == AttributeType.Varchar) {
      return attribute.varcharValue;
    }
  }

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
