import { Component, OnInit } from '@angular/core';
import { AuthService, NotificationService, Roles, UserManagementService } from '@service/index';
import { HeaderSorterDriver, HeaderSortable, HeaderFilterable, ConfirmationDialogService } from '@ui/index';
import {
  faFilePen,
  faFilter,
  faXmark,
  faPlus,
  faKey,
  faFilterCircleXmark
} from '@fortawesome/free-solid-svg-icons';
import { RoleValidator } from '@app/helper';
import { UserDataDto } from '@data/dto/user-data.dto';
import { AuthenticatedUser } from '@data/auth/authenticated-user';

@Component({
  selector: 'app-manage-users',
  templateUrl: './manage-users.component.html',
  styleUrls: ['./manage-users.component.css']
})
export class ManageUsersComponent implements OnInit, HeaderSortable, HeaderFilterable {

  USERNAME_HEADER = 'User name';
  FULLNAME_HEADER = 'Full name';
  EMAIL_HEADER = 'Email';
  ROLE_HEADER = 'Role';
  CREATE_HEADER = 'Created';
  LASTLOGIN_HEADER = 'Last login';

  faFilePen = faFilePen;
  faFilter = faFilter;
  faPlus = faPlus;
  faKey = faKey;
  faXmark = faXmark;
  faFilterCircleXmark = faFilterCircleXmark;

  authenticatedUser: AuthenticatedUser | null = null;
  public users = new Array<UserDataDto>();

  filterText: string | null = null;
  headerSorter = new HeaderSorterDriver(this);

  protected readonly Roles = Roles;

  constructor(private userManagementService: UserManagementService,
              private authService: AuthService,
              private roleValidator: RoleValidator,
              private notifyService: NotificationService,
              private confirmationDialogService: ConfirmationDialogService) {

    this.authenticatedUser = this.authService.getAuthenticatedUser();
    this.userManagementService.getAllUsers().subscribe(data => {
      this.users = data;
    });
  }

  ngOnInit(): void {
    this.roleValidator.validateUserAuthorizedFor([Roles.ADMINISTRATOR]);
  }

  showUsers(): UserDataDto[] {
    let visible = this.users;
    if (this.filterText == null || this.filterText === '') {
      return visible;
    }

    let filtered = new Array<UserDataDto>();
    visible.forEach(user => {
      // @ts-ignore
      let filterText: string = this.filterText?.toLowerCase();
      if (user.userName.toLowerCase().indexOf(filterText) >= 0 ||
        user.fullName.toLowerCase().indexOf(filterText) >= 0 ||
        user.email.toLowerCase().indexOf(filterText) >= 0) {
        filtered.push(user);
      }
    });
    return filtered;
  }

  sortBy(headerName: string, isAscendingOrder: boolean): void {
    if (headerName === this.USERNAME_HEADER) {
      this.users.sort((a, b) => isAscendingOrder ? a.userName.localeCompare(b.userName) : b.userName.localeCompare(a.userName));
    } else if (headerName === this.FULLNAME_HEADER) {
      this.users.sort((a, b) => isAscendingOrder ? a.fullName.localeCompare(b.fullName) : b.fullName.localeCompare(a.fullName));
    } else if (headerName === this.EMAIL_HEADER) {
      this.users.sort((a, b) => isAscendingOrder ? a.email.localeCompare(b.email) : b.email.localeCompare(a.email));
    } else if (headerName === this.ROLE_HEADER) {
      this.users.sort((a, b) => isAscendingOrder ? a.roleLevel.localeCompare(b.roleLevel) : b.roleLevel.localeCompare(a.roleLevel));
    } else if (headerName === this.CREATE_HEADER) {
      this.users.sort((a, b) => isAscendingOrder ? a.createdDate.localeCompare(b.createdDate) : b.createdDate.localeCompare(a.createdDate));
    } else if (headerName === this.LASTLOGIN_HEADER) {
      this.users.sort((a, b) => {
        if (a.lastLoginDate === null && b.lastLoginDate === null) {
          return 0;
        } else if (a.lastLoginDate === null) {
          return isAscendingOrder ? -1 : 1;
        } else if (b.lastLoginDate === null) {
          return isAscendingOrder ? 1 : -1;
        } else {
          return isAscendingOrder ?
            a.lastLoginDate.localeCompare(b.lastLoginDate) :
            b.lastLoginDate.localeCompare(a.lastLoginDate);
        }
      });
    }
  }

  headerClicked(headerName: string): void {
    this.headerSorter.headerClicked(headerName);
  }

  filter(filterText: string | null): void {
    if (filterText === '') {
      this.filterText = null;
    } else  {
      this.filterText = filterText;
    }
  }

  canBeDisabled(user: UserDataDto): boolean {
    return user.roleLevel !== Roles.NOACCESS &&
      user.id !== this.authenticatedUser?.id;
  }

  disableUser(user: UserDataDto): void {
    this.confirmationDialogService.askYesNo('Confirm disable operation', 'Do you really want to disable user ' + user.userName + '?')
      .then((confirmed) => {
        if (confirmed) {
          console.log('Confirmed disabling of user ' + user.userName);
          this.userManagementService.setUserRole(user.id, Roles.NOACCESS)      .pipe()
            .subscribe({
              error: (err: any) => {
                this.notifyService.showError(`An error has occurred while disabling user ${user.userName}`, 'Error');
              },
              complete: () => {
                user.roleLevel = Roles.NOACCESS;
                this.notifyService.showSuccess(`Successfully disabled user ${user.userName}`, 'Success');
              }
            });
        }
      })
      .catch(() => {
      });
  }
}
