import { Component, OnInit } from '@angular/core';
import { NotificationService, Roles, UserManagementService } from '@service/index';
import { HeaderSorterDriver, HeaderSortable, HeaderFilterable } from '@ui/index';
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

  public users = new Array<UserDataDto>();

  filterText: string | null = null;
  headerSorter = new HeaderSorterDriver(this);

  constructor(private userManagementService: UserManagementService,
    private roleValidator: RoleValidator,
    private notifyService: NotificationService) {

    this.userManagementService.getAllUsers().subscribe(data => {
      this.users = data;
    });
  }

  ngOnInit(): void {
    this.roleValidator.validateUserAuthorizedFor([Roles.ADMINISTRATOR]);
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

  protected readonly Roles = Roles;
}
