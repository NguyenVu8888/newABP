import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoleComponent } from './Role/role.component';
import { UserComponent } from './User/user.component';
import { permissionGuard } from '@abp/ng.core';

const routes: Routes = [
  {
    path: 'Role',
    component: RoleComponent,
    canActivate: [permissionGuard],
    data: {
      requiredPolicy: 'AbpIdentity.Roles',
    },
  },
  {
    path: 'User',
    component: UserComponent,
    canActivate: [permissionGuard],
    data: {
      requiredPolicy: 'AbpIdentity.Users',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SystemRoutingModule {}
