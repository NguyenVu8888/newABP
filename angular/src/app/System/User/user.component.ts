import { PagedResultDto } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { FormControl, FormGroup } from '@angular/forms';
import { UserDetailsComponent } from './user-details.component';
import { MessageConstants } from '../../shared/constants/masage.constant';
import { UserDto, UserInListDto, UserService } from '@proxy/system/users';
import { RoleAssignComponent } from './role-assign.component';
import { SetPasswordUserComponent } from './set-password.component';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
})
export class UserComponent implements OnInit, OnDestroy {
  //System variables
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;

  //Paging variables
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;

  //Business variables
  public items: UserInListDto[];
  public selectedItems: UserInListDto[] = [];
  public keyword: string = '';

  constructor(private userService: UserService, public dialogService: DialogService) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    this.loadData();
  }

  loadData(selectionId = null) {
    this.toggleBlockUI(true);
    if (this.keyword == ' ' || this.keyword.length == 0) {
      this.userService
        .getListAll()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: UserInListDto[]) => {
            this.items = response;
            if (selectionId != null && this.items.length > 0) {
              this.selectedItems = this.items.filter(x => x.id == selectionId);
            }

            this.toggleBlockUI(false);
          },
          error: () => {
            this.toggleBlockUI(false);
          },
        });
    } else {
      this.userService
        .getListWithFilter({
          maxResultCount: this.maxResultCount,
          skipCount: this.skipCount,
          keyword: this.keyword,
        })
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: PagedResultDto<UserInListDto>) => {
            this.items = response.items;
            this.totalCount = response.totalCount;
            if (selectionId != null && this.items.length > 0) {
              this.selectedItems = this.items.filter(x => x.id == selectionId);
            }

            this.toggleBlockUI(false);
          },
          error: () => {
            this.toggleBlockUI(false);
          },
        });
    }
  }

  showAddModal() {
    const ref = this.dialogService.open(UserDetailsComponent, {
      header: 'Thêm mới người dùng',
      width: '70%',
    });

    ref.onClose.subscribe((data: UserDto) => {
      if (data) {
        alert(MessageConstants.CREATED_OK_MSG);
        this.selectedItems = [];
        this.loadData();
      }
    });
  }

  pageChanged(event: any): void {
    this.skipCount = (event.page - 1) * this.maxResultCount;
    this.maxResultCount = event.rows;
    this.loadData();
  }

  showEditModal() {
    if (this.selectedItems.length == 0) {
      alert(MessageConstants.NOT_CHOOSE_ANY_RECORD);
      return;
    }
    var id = this.selectedItems[0].id;
    const ref = this.dialogService.open(UserDetailsComponent, {
      data: {
        id: id,
      },
      header: 'Cập nhật người dùng',
      width: '70%',
    });

    ref.onClose.subscribe((data: UserDto) => {
      if (data) {
        alert(MessageConstants.UPDATED_OK_MSG);
        this.selectedItems = [];
        this.loadData(data.id);
      }
    });
  }

  deleteItems() {
    if (this.selectedItems.length == 0) {
      alert(MessageConstants.NOT_CHOOSE_ANY_RECORD);
      return;
    }
    var ids = [];
    this.selectedItems.forEach(element => {
      ids.push(element.id);
    });
    this.deleteItemsConfirm(ids);
  }

  deleteItemsConfirm(ids: any[]) {
    this.toggleBlockUI(true);
    this.userService.deleteMultiple(ids).subscribe({
      next: () => {
        alert(MessageConstants.DELETED_OK_MSG);
        this.loadData();
        this.selectedItems = [];
        this.toggleBlockUI(false);
      },
      error: () => {
        this.toggleBlockUI(false);
      },
    });
  }

  setPassword(id: string) {
    const ref = this.dialogService.open(SetPasswordUserComponent, {
      data: {
        id: id,
      },
      header: 'Đặt lại mật khẩu',
      width: '70%',
    });

    ref.onClose.subscribe((result: boolean) => {
      if (result) {
        alert(MessageConstants.CHANGE_PASSWORD_SUCCCESS_MSG);
        this.selectedItems = [];
        this.loadData();
      }
    });
  }

  assignRole(id: string) {
    const ref = this.dialogService.open(RoleAssignComponent, {
      data: {
        id: id,
      },
      header: 'Gán quyền',
      width: '70%',
    });

    ref.onClose.subscribe((result: boolean) => {
      if (result) {
        alert(MessageConstants.ROLE_ASSIGN_SUCCESS_MSG);
        this.loadData();
      }
    });
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }
}
