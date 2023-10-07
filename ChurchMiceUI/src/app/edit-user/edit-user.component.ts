import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import {
  AuthService,
  NotificationService, Roles,
  UserManagementService
} from '@service/index';
import { ReCaptchaV3Service } from 'ngx-captcha';
import { UserDataDto } from '@data/dto/user-data.dto';
import { AuthenticatedUser } from '@data/auth/authenticated-user';
import { first } from 'rxjs/operators';
import { SelectOption } from '@ui/container/select-option';


@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {

  submitted = false;
  roleLevels: SelectOption[] = [];

  userId: string = '';
  user?: UserDataDto;
  existingUserName = '';

  newFullName = '';
  newEmail = '';
  newRoleLevel = Roles.NOACCESS;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private userManagementService: UserManagementService,
    private notifyService: NotificationService) {

    this.createRoleLevels();
  }

  private createRoleLevels() {
    this.roleLevels.push(new SelectOption(Roles.NOACCESS, Roles.NOACCESS));
    this.roleLevels.push(new SelectOption(Roles.ATTENDER, Roles.ATTENDER));
    this.roleLevels.push(new SelectOption(Roles.MEMBER, Roles.MEMBER));
    this.roleLevels.push(new SelectOption(Roles.ADMINISTRATOR, Roles.ADMINISTRATOR + ' (Warning)'));
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      // https://indepth.dev/tutorials/angular/indepth-guide-to-passing-parameters-via-routing
      const userId = params['userId'];
      this.loadUserFor(userId);
    });
  }

  private loadUserFor(userId: string) {
    this.userId = userId;
    this.userManagementService.getUser(userId)
      .pipe(first())
      .subscribe({
        next: (user: UserDataDto | null) => {
          if (user === null) {
            this.notifyService.showError('Empty record while loading user record by user\'s Id', 'Error loading user record');
          } else {
            this.user = user;
            this.existingUserName = user.userName;
            this.newEmail = user.email;
            this.newFullName = user.fullName;
            this.newRoleLevel = user.roleLevel;
          }
        },
        error: (err: any) => {
          this.notifyService.showError('Error while attempting to load user record by user\'s Id', 'Error loading user record');
        },
        complete: () => {}
      });
  }

  private noChanges(): boolean {
    if (this.user === null) {
      return true;
    }
    return this.user?.fullName === this.newFullName &&
      this.user.email === this.newEmail &&
      this.user.roleLevel === this.newRoleLevel;
  }

  onSubmit() {
    if (this.noChanges()) {
      this.notifyService.showWarning('There are no changes to be saved to this user record', 'Update not done');
      return;
    }

    // saveUserData();
    // saveRoleLevel();

    // this.submitted = true;
    //
    // // reset alerts on submit
    // this.alertService.clear();
    //
    // // stop here if form is invalid
    // if (this.form.invalid) {
    //   return;
    // }
    //
    // this.loading = true;
    // if (this.isAddMode) {
    //   this.createUser();
    // } else {
    //   this.updateUser();
    // }
  }

}
