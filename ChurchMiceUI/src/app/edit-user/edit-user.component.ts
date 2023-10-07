import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import {
  AuthService,
  NotificationService,
  UserManagementService
} from '@service/index';
import { ReCaptchaV3Service } from 'ngx-captcha';
import { UserDataDto } from '@data/dto/user-data.dto';
import { AuthenticatedUser } from '@data/auth/authenticated-user';
import { first } from 'rxjs/operators';


@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {

  submitted = false;
  userId: string = '';
  user?: UserDataDto;
  existingUserName = '';

  newFullName = '';
  newEmail = '';

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private userManagementService: UserManagementService,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
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
          }
        },
        error: (err: any) => {
          this.notifyService.showError('Error while attempting to load user record by user\'s Id', 'Error loading user record');
        },
        complete: () => {}
      });
  }

  onSubmit() {
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
