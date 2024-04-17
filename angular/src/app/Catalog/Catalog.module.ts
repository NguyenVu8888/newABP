import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { CatalogRoutingModule } from './Catalog-routing.module';
import { ProductComponent } from './product/product.component';
import { PanelModule } from 'primeng/panel';
import { TableModule } from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { BlockUIModule } from 'primeng/blockui';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ReactiveFormsModule } from '@angular/forms';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { ProductDetailsComponent } from './product/product-details.component';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { CheckboxModule } from 'primeng/checkbox';
import { EditorModule } from 'primeng/editor';
import { ImageModule } from 'primeng/image';
import { ProductAttributeComponent } from './product/product-attribute.component';
import { AttributeDetailsComponent } from './product-attribute/attribute-details.component';
import { AttributeComponent } from './product-attribute/attribute.component';

@NgModule({
  declarations: [
    ProductComponent,
    ProductDetailsComponent,
    ProductAttributeComponent,
    AttributeComponent,
    AttributeDetailsComponent,
  ],
  imports: [
    SharedModule,
    CatalogRoutingModule,
    PanelModule,
    TableModule,
    PaginatorModule,
    BlockUIModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    ProgressSpinnerModule,
    ReactiveFormsModule,
    DynamicDialogModule,
    InputNumberModule,
    InputTextareaModule,
    CheckboxModule,
    EditorModule,
    ImageModule,
  ],
  bootstrap: [],
})
export class CatalogModule {}
