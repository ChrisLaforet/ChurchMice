import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { AuthService, Roles, DatepickerUtilities, MemberService, NotificationService } from '@service/index';
import { first } from 'rxjs/operators';
import { MemberDto } from '@data/dto/member.dto';
import { faCalendarDays } from '@fortawesome/free-solid-svg-icons';
import { NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { RoleValidator } from '@app/helper';


@Component({
  selector: 'app-create-member',
  templateUrl: './create-member.component.html',
  styleUrls: ['./create-member.component.css']
})
export class CreateMemberComponent {

  faCalendarDays = faCalendarDays;

  submitted = false;

  minDate: NgbDateStruct;
  today: NgbDateStruct = DatepickerUtilities.currentNgbDate();

  newFirstName = '';
  newLastName = '';
  newEmail = '';
  newHomePhone = '';
  newMobilePhone = '';
  newAddress1 = '';
  newAddress2 = '';
  newCity = '';
  newState = '';
  newZip = '';
  newBirthDate: NgbDate | null = null;
  newAnniversary: NgbDate | null = null;
  newUserId: string | undefined = undefined;

  members = new Array<MemberDto>();

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
  }

  isFirstMemberForUser(): boolean {
    if (this.members.length === 0 &&
      this.roleValidator.isUserAuthorizedFor([Roles.ADMINISTRATOR]) !== true) {
      const user = this.authService.getAuthenticatedUser();
      if (user !== null) {
        this.newUserId = user.id;
      }
      return true;
    }
    return false;
  }

  onSubmit(): void {
    this.submitted = true;
    this.notifyService.showInfo('Requesting to create a new member', 'Create new member')

// TODO: Administrator set/clear MemberSince

    this.memberService.createMember(this.newFirstName, this.newLastName,
                                    this.newEmail, this.newHomePhone,
                                    this.newMobilePhone, this.newAddress1,
                                    this.newAddress2, this.newCity, this.newState,
                                    this.newZip, DatepickerUtilities.createUSDateStringFromNgbDate(this.newBirthDate),
                                    DatepickerUtilities.createUSDateStringFromNgbDate(this.newAnniversary),
                                    undefined, this.newUserId)
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

