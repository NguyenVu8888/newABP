import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject, forkJoin, takeUntil } from 'rxjs';
import { UtilityService } from '../../shared/services/utility.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ProductsService } from '@proxy/catalogs/products';
import { ProductCategoriesService } from '@proxy/catalogs/product-categories';
import { CreateUpdateProductDto, ProductDto } from '@proxy/catalog/products';
import { ProductCategoryInListDto } from '@proxy/catalog/product-categories';
import { productTypeOptions } from '@proxy/ecommerce/products';
import { ManufactureService } from '@proxy/catalogs/manufactures';
import { ManufactureInListDto } from '@proxy/catalog/manufactures';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: [],
})
export class ProductDetailsComponent implements OnInit, OnDestroy {
  isSubmited: boolean = false;
  private ngUnsubcribe = new Subject<void>();
  blockedPanel: boolean = false;
  btnDisabled: boolean = false;
  btnEdit: boolean = this.utilService.isEmpty(this.config.data?.id);
  public ThumbnailImage;

  //form
  public formProduct: FormGroup;

  // dropdown product
  productCategorise: any[] = [];
  productTypes: any[] = [];
  manufacturers: any[] = [];
  selectedEntity = {} as ProductDto;

  constructor(
    private _productServie: ProductsService,
    private _productCategoryService: ProductCategoriesService,
    private fb: FormBuilder,
    private config: DynamicDialogConfig,
    private ref: DynamicDialogRef,
    private utilService: UtilityService,
    private manufacturerService: ManufactureService,

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
    this.loadProductCategories();
    this.loadProductTypes();
    this.loadManufacture();
    this.buildForm();

    this.addDataform();
  }

  //Load data to form if user chose edit
  addDataform() {
    var productCategories = this._productCategoryService.getListAll();
    this.toggleBlockUI(true);
    forkJoin({
      productCategories,
    })
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: (response: any) => {
          //Push data to dropdown
          var productCategories = response.productCategories as ProductCategoryInListDto[];
          productCategories.forEach(element => {
            this.productCategorise.push({
              value: element.id,
              label: element.name,
            });
          });

          //Load edit data to form
          if (this.utilService.isEmpty(this.config.data?.id) == true) {
            this.GetNewSuggestionCode();
            this.toggleBlockUI(false);
          } else {
            this.loadFromDetails(this.config.data?.id);
          }
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  generateSlug() {
    this.formProduct.controls['SKU'].setValue(
      this.utilService.MakeSeoTitle(this.formProduct.get('name').value)
    );
  }
  loadFromDetails(id: string) {
    this.toggleBlockUI(true);
    this._productServie
      .get(id)
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: (response: ProductDto) => {
          this.selectedEntity = response;
          this.loadThumbnail(this.selectedEntity.thumbnailPicture);
          this.buildForm();
          this.toggleBlockUI(false);
        },
        error: error => {
          this.toggleBlockUI(false);
        },
      });
  }

  private buildForm() {
    this.formProduct = this.fb.group({
      name: new FormControl(
        this.selectedEntity.name || null,
        Validators.compose([Validators.required, Validators.maxLength(250)])
      ),
      code: new FormControl(this.selectedEntity.code || null, [Validators.required]),
      manufictureId: new FormControl(this.selectedEntity.manufictureId || null, [
        Validators.required,
      ]),
      ProductType: new FormControl(this.selectedEntity.productType || null, Validators.required),
      SKU: new FormControl(this.selectedEntity.sku || null, Validators.required),
      sortOrder: new FormControl(this.selectedEntity.sortOrder || null, Validators.required),
      visibility: new FormControl(this.selectedEntity.visibility || false),
      isActive: new FormControl(this.selectedEntity.isActive || false),
      categoryId: new FormControl(this.selectedEntity.categoryId || null, Validators.required),
      seoMetaDescription: new FormControl(this.selectedEntity.seoMetaDescription || null),
      descreption: new FormControl(this.selectedEntity.descreption || null),
      thumbnailPictureName: new FormControl(this.selectedEntity.thumbnailPicture || null),
      thumbnailPictureContent: new FormControl(null),
    });
  }

  GetNewSuggestionCode() {
    this._productServie
      .getSuggestNewCode()
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: (response: string) => {
          this.formProduct.patchValue({
            code: response,
          });
        },
        error: error => {
          console.log(error);
        },
      });
  }

  loadProductCategories() {
    this._productCategoryService
      .getListAll()
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: (response: ProductCategoryInListDto[]) => {
          response.forEach(element => {
            this.productCategorise.push({
              value: element.id,
              name: element.name,
            });
          });
        },
        error: error => {
          alert('loi load category-product-detail');
          console.log(error);
        },
      });
  }

  loadManufacture() {
    //Load data to form
    this.manufacturerService
      .getListAll()
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: (response: ManufactureInListDto[]) => {
          //Push data to dropdown
          response.forEach(element => {
            this.manufacturers.push({
              value: element.id,
              label: element.name,
            });
          });
        },
        error: error => {
          console.log(error);
        },
      });
  }

  loadProductTypes() {
    productTypeOptions.forEach(element => {
      this.productTypes.push({
        value: element.value,
        label: element.key,
      });
    });
  }
  //end load du lieu

  //luu du lieu xuong

  saveChange() {
    // console.log(this.formProduct.value.categoryId.value);
    // console.log(this.config.data);

    var dataSend: CreateUpdateProductDto = {
      name: this.formProduct.value.name,
      code: this.formProduct.value.code,
      manufictureId: this.formProduct.value.manufictureId,
      productType: this.formProduct.value.ProductType,
      sku: this.formProduct.value.SKU,
      sortOrder: this.formProduct.value.sortOrder,
      visibility: this.formProduct.value.visibility,
      isActive: this.formProduct.value.isActive,
      categoryId: this.formProduct.value.categoryId.value,
      seoMetaDescription: this.formProduct.value.seoMetaDescription,
      descreption: this.formProduct.value.descreption,
      thumbnailPictureName: this.formProduct.value.thumbnailPictureName,
      thumbnailPictureContent: this.formProduct.value.thumbnailPictureContent,
    };

    this.isSubmited = true;
    console.log(dataSend);

    if (!this.formProduct.invalid && this.isSubmited == true) {
      if (this.utilService.isEmpty(this.config.data?.id) == true) {
        this._productServie
          .create(dataSend)
          .pipe(takeUntil(this.ngUnsubcribe))
          .subscribe({
            next: () => {
              this.ref.close(this.formProduct.value);
            },
            error: err => {
              alert(err.error.error.message);
              this.toggleBlockUI(false);
            },
          });
      } else {
        this._productServie
          .update(this.config.data?.id, dataSend)
          .pipe(takeUntil(this.ngUnsubcribe))
          .subscribe({
            next: () => {
              this.ref.close(this.formProduct.value);
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

  // end luu du lieu

  // xu ly anh
  onFileChange(event) {
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.formProduct.patchValue({
          thumbnailPictureName: file.name,
          thumbnailPictureContent: reader.result,
        });

        // need to run CD since file load runs outside of zone
        this.cd.markForCheck();
      };
    }
  }

  loadThumbnail(filename: string) {
    this._productServie
      .getThumnailImage(filename)
      .pipe(takeUntil(this.ngUnsubcribe))
      .subscribe({
        next: (response: string) => {
          var fileExt = this.selectedEntity.thumbnailPicture?.split('.').pop();
          this.ThumbnailImage = this.sanitizer.bypassSecurityTrustResourceUrl(
            `data:image/${fileExt};base64, ${response}`
          );
        },
      });
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
