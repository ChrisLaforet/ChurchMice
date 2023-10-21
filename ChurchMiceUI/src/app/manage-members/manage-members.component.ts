import { Component, OnInit } from '@angular/core';
import { NotificationService, Roles, UserManagementService } from '@service/index';
import { HeaderSorterDriver, HeaderSortable, HeaderFilterable, ConfirmationDialogService } from '@ui/index';
import {
  faFilePen,
  faFilter,
  faXmark,
  faPlus
} from '@fortawesome/free-solid-svg-icons';
import { RoleValidator } from '@app/helper';
import { MemberService } from '@service/member/member.service';
import { MemberDto } from '@data/index';

@Component({
  selector: 'app-manage-members',
  templateUrl: './manage-members.component.html',
  styleUrls: ['./manage-members.component.css']
})
export class ManageMembersComponent implements OnInit, HeaderSortable, HeaderFilterable {

  LASTNAME_HEADER = 'Last name';
  FIRSTNAME_HEADER = 'First name';
  HOMEPHONE_HEADER = 'Home #';
  MOBILEPHONE_HEADER = 'Mobile #';
  EMAIL_HEADER = 'Email';
  CITY_HEADER = 'City';
  MEMBERSINCE_HEADER = 'Member since';

  faFilePen = faFilePen;
  faFilter = faFilter;
  faPlus = faPlus;
  faXmark = faXmark;

  public members = new Array<MemberDto>();

  filterText: string | null = null;
  headerSorter = new HeaderSorterDriver(this);

  protected readonly Roles = Roles;

  constructor(private userManagementService: UserManagementService,
              private memberService: MemberService,
              private roleValidator: RoleValidator,
              private notifyService: NotificationService,
              private confirmationDialogService: ConfirmationDialogService) {
    this.memberService.getAllEditableMembers().subscribe(data => {
      this.members = data;
    });
  }

  ngOnInit(): void {
    this.roleValidator.validateUserAuthorizedFor([Roles.ADMINISTRATOR, Roles.MEMBER, Roles.ATTENDER]);
  }

  isFirstMemberForUser(): boolean {
    return this.members.length === 0 &&
      this.roleValidator.isUserAuthorizedFor([Roles.ADMINISTRATOR]) !== true;
  }

  showMembers(): MemberDto[] {
    let visible = this.members;
    if (this.filterText == null || this.filterText === '') {
      return visible;
    }

    let filtered = new Array<MemberDto>();
    visible.forEach(member => {
      // @ts-ignore
      let filterText: string = this.filterText?.toLowerCase();

      if (member.firstName.toLowerCase().indexOf(filterText) >= 0 ||
        member.lastName.toLowerCase().indexOf(filterText) >= 0 ||
        (member.email !== undefined && member.email.toLowerCase().indexOf(filterText) >= 0)) {
        filtered.push(member);
      }
    });
    return filtered;
  }

  sortBy(headerName: string, isAscendingOrder: boolean): void {
    if (headerName === this.LASTNAME_HEADER) {
      this.members.sort((a, b) => isAscendingOrder ? a.lastName.localeCompare(b.lastName) : b.lastName.localeCompare(a.lastName));
    } else if (headerName === this.FIRSTNAME_HEADER) {
      this.members.sort((a, b) => isAscendingOrder ? a.firstName.localeCompare(b.firstName) : b.firstName.localeCompare(a.firstName));
    } else if (headerName === this.EMAIL_HEADER) {
      this.members.sort((a, b) => {
        const aEmail = a.email === undefined ? '' : a.email;
        const bEmail = b.email === undefined ? '' : b.email;
        return isAscendingOrder ? aEmail.localeCompare(bEmail) : bEmail.localeCompare(aEmail)
      });
    } else if (headerName === this.CITY_HEADER) {
      this.members.sort((a, b) => {
        const aCity = a.city === undefined ? '' : a.city;
        const bCity = b.city === undefined ? '' : b.city;
        return isAscendingOrder ? aCity.localeCompare(bCity) : bCity.localeCompare(aCity)
      });
    } else if (headerName === this.HOMEPHONE_HEADER) {
      this.members.sort((a, b) => {
        const aPhone = a.homePhone === undefined ? '' : a.homePhone;
        const bPhone = b.homePhone === undefined ? '' : b.homePhone;
        return isAscendingOrder ? aPhone.localeCompare(bPhone) : bPhone.localeCompare(aPhone)
      });
    } else if (headerName === this.MOBILEPHONE_HEADER) {
      this.members.sort((a, b) => {
        const aPhone = a.mobilePhone === undefined ? '' : a.mobilePhone;
        const bPhone = b.mobilePhone === undefined ? '' : b.mobilePhone;
        return isAscendingOrder ? aPhone.localeCompare(bPhone) : bPhone.localeCompare(aPhone)
      });
    } else if (headerName === this.MEMBERSINCE_HEADER) {
      this.members.sort((a, b) => {
        const aSince = a.memberSince === undefined ? '' : a.memberSince;
        const bSince = b.memberSince === undefined ? '' : b.memberSince;
        return isAscendingOrder ? aSince.localeCompare(bSince) : bSince.localeCompare(aSince)
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

  removeMember(member: MemberDto) {
    // TODO: finalize this - ask if ok, then make it happen
  }
}