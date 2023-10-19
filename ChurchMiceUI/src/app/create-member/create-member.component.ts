import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { AuthService, Roles, DatepickerUtilities, MemberService, NotificationService } from '@service/index';
import { first } from 'rxjs/operators';
import { MemberDto } from '@data/dto/member.dto';
import { faCalendarDays, faArrowRotateLeft } from '@fortawesome/free-solid-svg-icons';
import { NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { RoleValidator } from '@app/helper';
import { SelectOption } from '@ui/container/select-option';


@Component({
  selector: 'app-create-member',
  templateUrl: './create-member.component.html',
  styleUrls: ['./create-member.component.css']
})
export class CreateMemberComponent {

  faArrowRotateLeft = faArrowRotateLeft;
  faCalendarDays = faCalendarDays;

  submitted = false;

  minDate: NgbDateStruct;
  today: NgbDateStruct = DatepickerUtilities.currentNgbDate();

  newFirstName = '';
  newLastName = '';
  newEmail: string | undefined = undefined;
  newHomePhone: string | undefined = undefined;
  newMobilePhone: string | undefined = undefined;
  newAddress1: string | undefined = undefined;
  newAddress2: string | undefined = undefined;
  newCity: string | undefined = undefined;
  newState: string | undefined = undefined;
  newZip: string | undefined = undefined;
  newBirthDate: NgbDate | null = null;
  newAnniversary: NgbDate | null = null;
  newMemberSince: string | undefined = undefined;
  newUserId: string | undefined = undefined;

  members = new Array<MemberDto>();
  years: SelectOption[] = [];

  userId: string | undefined = undefined;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private memberService: MemberService,
    private authService: AuthService,
    private roleValidator: RoleValidator,
    private notifyService: NotificationService) {

    this.memberService.getAllEditableMembers().subscribe(data => {
      this.members = data;
    });

    let min= DatepickerUtilities.createNgbDateFromUSDateString("1/1/1900");
    if (min == null) {
      this.minDate = this.today;
    } else {
      this.minDate = min;
    }

    for (var year = this.today.year; year > (this.today.year - 100); year--) {
      this.years.push(new SelectOption(year.toString(), year.toString()));
    }
  }

  isFirstMemberForUser(): boolean {
    if (this.userId === undefined) {
      const user = this.authService.getAuthenticatedUser();
      if (user !== null) {
        this.userId = user.id;
      }
    }
    if (this.members.length === 0 &&
      this.roleValidator.isUserAuthorizedFor([Roles.ADMINISTRATOR]) !== true) {
      this.newUserId = this.userId;
      return true;
    }
    return false;
  }

  navigateBack(): void {
    this.router.navigate(['manageMembers']);
  }

  isUserOrAdministrator(): boolean {
    return this.newUserId === this.userId || this.roleValidator.isUserAuthorizedFor([Roles.ADMINISTRATOR]);
  }

  onSubmit(): void {
    this.submitted = true;
    this.notifyService.showInfo('Requesting to create a new member', 'Create new member')

    this.memberService.createMember(this.newFirstName, this.newLastName,
                                    this.newEmail, this.newHomePhone,
                                    this.newMobilePhone, this.newAddress1,
                                    this.newAddress2, this.newCity, this.newState,
                                    this.newZip, DatepickerUtilities.createUSDateStringFromNgbDate(this.newBirthDate),
                                    DatepickerUtilities.createUSDateStringFromNgbDate(this.newAnniversary),
                                    this.newMemberSince, this.newUserId)
      .pipe(first())
      .subscribe({
        next: (member: MemberDto) => {
          this.notifyService.showSuccess('New member has been created.  Selecting edit mode for additional editing.', 'Success');
          this.submitted = false;
          this.router.navigate(['editMember', member.id]);
        },
        error: (err: any) => {
          this.submitted = false;
          return;
        },
        complete: () => {
          this.submitted = false;
          return;
        }
      });
  }
}

