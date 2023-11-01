import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import {
  AuthService,
  NotificationService, Roles,
  UserManagementService
} from '@service/index';
import { UserDataDto } from '@data/dto/user-data.dto';
import { first } from 'rxjs/operators';
import { v4 as uuidv4 } from 'uuid';
import { MessageResponseDto } from '@data/index';


@Component({
  selector: 'app-edit-password',
  templateUrl: './edit-password.component.html',
  styleUrls: ['./edit-password.component.css']
})
export class EditPasswordComponent implements OnInit {

  submitted = false;

  userId: string = '';
  user?: UserDataDto;
  existingUserName = '';
  existingFullName = '';

  newPassword = '';
  newConfirmPassword = '';

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private userManagementService: UserManagementService,
    private notifyService: NotificationService)
  { }

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
            this.existingFullName = user.fullName;
          }
        },
        error: (err: any) => {
          this.notifyService.showError('Error while attempting to load user record by user\'s Id', 'Error loading user record');
        },
        complete: () => {}
      });
  }

  generateLockout() {
    const password = uuidv4();
    this.newPassword = password;
    this.newConfirmPassword = password;
  }

  onSubmit() {
    this.submitted = true;
    this.notifyService.showInfo('Requesting to change user\'s password', 'Change password');

    this.userManagementService.setUserPassword(this.userId, this.newPassword)
      .pipe(first())
      .subscribe({
        next: (message: MessageResponseDto) => {
          this.notifyService.showSuccess('User\'s password has been successfully changed.', 'Success');
          this.newPassword = '';
          this.newConfirmPassword = '';
          this.submitted = false;
        },
        error: () => {
          this.notifyService.showError('An error has occurred while changing password.', 'Error');
          this.submitted = false;
        },
        complete: () => {
        }
      });
  }
}
