import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductComponent } from './product/product.component';
import { ProductAttributeComponent } from './product/product-attribute.component';
import { AttributeComponent } from './product-attribute/attribute.component';
import { permissionGuard } from '@abp/ng.core';

const routes: Routes = [
  {
    path: 'Product',
    component: ProductComponent,
    canActivate: [permissionGuard],
    data: {
      requiredPolicy: 'ecomAdminCatalog.Product',
    },
  },
  {
    path: 'Attribute',
    component: AttributeComponent,
    canActivate: [permissionGuard],
    data: {
      requiredPolicy: 'ecomAdminCatalog.Attribute',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CatalogRoutingModule {}
