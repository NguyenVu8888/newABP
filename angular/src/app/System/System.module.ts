import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
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
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { CheckboxModule } from 'primeng/checkbox';
import { EditorModule } from 'primeng/editor';
import { ImageModule } from 'primeng/image';
import { BadgeModule } from 'primeng/badge';
import { RoleComponent } from './Role/role.component';
import { RoleDetailsComponent } from './Role/role-details.component';
import { SystemRoutingModule } from './System-routing.module';
import { PermissionsComponent } from './Role/permission-grant.component';
import { UserComponent } from './User/user.component';
import { UserDetailsComponent } from './User/user-details.component';
import { RoleAssignComponent } from './User/role-assign.component';
import { PickListModule } from 'primeng/picklist';
import { SetPasswordUserComponent } from './User/set-password.component';

@NgModule({
  declarations: [
    RoleComponent,
    RoleDetailsComponent,
    PermissionsComponent,
    UserComponent,
    UserDetailsComponent,
    RoleAssignComponent,
    SetPasswordUserComponent,
  ],
  imports: [
    SharedModule,
    SystemRoutingModule,
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
    BadgeModule,
    PickListModule,
  ],
  bootstrap: [],
})
export class SystemModule {}
