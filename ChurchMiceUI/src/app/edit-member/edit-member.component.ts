import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { AuthService, Roles, DatepickerUtilities, MemberService, NotificationService } from '@service/index';
import { first } from 'rxjs/operators';
import { MemberDto } from '@data/dto/member.dto';
import { faCalendarDays, faArrowRotateLeft } from '@fortawesome/free-solid-svg-icons';
import { NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { RoleValidator } from '@app/helper';
import { SelectOption } from '@ui/container/select-option';
import { MemberContainer } from '@tool/index';
import { DomSanitizer } from '@angular/platform-browser';


@Component({
  selector: 'app-edit-member',
  templateUrl: './edit-member.component.html',
  styleUrls: ['./edit-member.component.css']
})
export class EditMemberComponent implements OnInit {

  faArrowRotateLeft = faArrowRotateLeft;
  faCalendarDays = faCalendarDays;

  submitted = false;

  memberId: string = '';
  memberContainer?: MemberContainer;

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

  years: SelectOption[] = [];

  userId: string | undefined;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private memberService: MemberService,
    private authService: AuthService,
    private roleValidator: RoleValidator,
    private notifyService: NotificationService,
    private domSanitizer: DomSanitizer) {

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

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      // https://indepth.dev/tutorials/angular/indepth-guide-to-passing-parameters-via-routing
      const memberId = params['memberId'];
      this.loadMemberFor(memberId);
    });
  }

  private loadMemberFor(memberId: string) {
    this.memberId = memberId;
    this.memberService.getMember(memberId)
      .pipe(first())
      .subscribe({
        next: (member: MemberDto | null) => {
          if (member === null) {
            this.notifyService.showError('Empty record while loading member record by member\'s Id', 'Error loading member record');
          } else {
            this.memberContainer = new MemberContainer(this.memberService, this.domSanitizer, this.notifyService, member);
            this.mapNewFieldsWith(member);
          }
        },
        error: (err: any) => {
          this.notifyService.showError('Error while attempting to load user record by user\'s Id', 'Error loading user record');
        },
        complete: () => {}
      });
  }

  private mapNewFieldsWith(member: MemberDto) {
    this.newFirstName = member.firstName;
    this.newLastName = member.lastName;
    this.newEmail = member.email;
    this.newHomePhone = member.homePhone;
    this.newMobilePhone = member.mobilePhone;
    this.newAddress1 = member.mailingAddress1;
    this.newAddress2 = member.mailingAddress2;
    this.newCity = member.city;
    this.newZip = member.zip;
    this.newBirthDate = DatepickerUtilities.createNgbDateFromUSDateString(member.birthday);
    this.newAnniversary = DatepickerUtilities.createNgbDateFromUSDateString(member.anniversary);
    this.newMemberSince = member.memberSince;
    this.newUserId = member.userId;
  }

  isMemberForCurrentUser(): boolean {
    if (this.userId === undefined) {
      const user = this.authService.getAuthenticatedUser();
      if (user !== null) {
        this.userId = user.id;
      }
    }
    return this.memberContainer !== undefined && this.userId !== undefined && this.memberContainer.member.userId === this.userId;
  }

  navigateBack(): void {
    this.router.navigate(['manageMembers']);
  }

  isMemberUserIdOrAdministrator(): boolean {
    return (this.newUserId !== undefined && this.newUserId === this.userId) ||
          this.roleValidator.isUserAuthorizedFor([Roles.ADMINISTRATOR]);
  }

  onSubmit(): void {
    this.submitted = true;
    this.notifyService.showInfo('Requesting to update member details', 'Update existing member')

    this.memberService.updateMember(this.mapToMemberDto())
      .pipe(first())
      .subscribe({
        next: (member: MemberDto) => {
          this.notifyService.showSuccess('Member details have been updated.', 'Success');
          this.submitted = false;
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

  private mapToMemberDto(): MemberDto {
    // @ts-ignore
    return new MemberDto(this.member.id, this.newFirstName, this.newLastName,
      this.newEmail, this.newHomePhone,
      this.newMobilePhone, this.newAddress1,
      this.newAddress2, this.newCity, this.newState,
      this.newZip, DatepickerUtilities.createUSDateStringFromNgbDate(this.newBirthDate),
      DatepickerUtilities.createUSDateStringFromNgbDate(this.newAnniversary),
      this.newMemberSince, this.newUserId);
  }
}

