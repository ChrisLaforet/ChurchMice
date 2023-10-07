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
import { forkJoin, ReplaySubject } from 'rxjs';


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
    return !this.isUserChanged() && !this.isRoleLevelChanged();
  }

  private isUserChanged(): boolean {
    if (this.user === null) {
      return false;
    }
    return this.user?.fullName !== this.newFullName ||
      this.user.email !== this.newEmail;
  }

  private isRoleLevelChanged(): boolean {
    if (this.user === null) {
      return false;
    }
    return this.user?.roleLevel !== this.newRoleLevel;
  }

  onSubmit() {
    if (this.noChanges()) {
      this.notifyService.showWarning('There are no changes to be saved to this user record', 'Update not done');
      return;
    }

    this.submitted = true;
    if (this.isUserChanged() && this.isRoleLevelChanged()) {
      let saveUserReplay = new ReplaySubject(1);
      this.userManagementService.updateUser(this.userId, this.newFullName, this.newEmail)
        .pipe(first())
        .subscribe({
          error: err => {
            this.notifyService.showError('Error encountered while saving user changes', 'Error saving');
            saveUserReplay.next(null);
            saveUserReplay.complete();
          },
          complete: () => {
            this.notifyService.showSuccess('Successfully saved user changes', 'User changes saved');
            saveUserReplay.next(null);
            saveUserReplay.complete();
          }
        });

      let saveRoleReplay = new ReplaySubject(1);
      this.userManagementService.setUserRole(this.userId, this.newRoleLevel)
        .pipe(first())
        .subscribe({
          error: err => {
            this.notifyService.showError('Error encountered while saving role change for user', 'Error saving');
            saveRoleReplay.next(null);
            saveRoleReplay.complete();
          },
          complete: () => {
            this.notifyService.showSuccess('Successfully saved user role change', 'Role change saved');
            saveRoleReplay.next(null);
            saveRoleReplay.complete();
          }
        });

      forkJoin([saveUserReplay, saveRoleReplay]).subscribe(results => {
        this.submitted = false;
      });
    } else if (this.isUserChanged()) {
      this.userManagementService.updateUser(this.userId, this.newFullName, this.newEmail)
        .pipe(first())
        .subscribe({
          error: err => {
            this.notifyService.showError('Error encountered while saving user changes', 'Error saving');
            this.submitted = false;
          },
          complete: () => {
            this.notifyService.showSuccess('Successfully saved user changes', 'User changes saved');
            this.submitted = false;
          }
        });
    } else {
      this.userManagementService.setUserRole(this.userId, this.newRoleLevel)
        .pipe(first())
        .subscribe({
          error: err => {
            this.notifyService.showError('Error encountered while saving role change for user', 'Error saving');
            this.submitted = false;
          },
          complete: () => {
            this.notifyService.showSuccess('Successfully saved user role change', 'Role change saved');
            this.submitted = false;
          }
        });
    }
  }

  private saveUser(isOnlyChange: boolean) {

  }

}
