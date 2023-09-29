import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { UserService, NotificationService } from '@service/index';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
})
export class ChangePasswordComponent implements OnInit {
  submitted = false;
  loginName = '';
  resetKey = '';
  newPassword = '';
  newConfirmPassword = '';

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private notifyService: NotificationService) {
  }

  ngOnInit(): void {
  }

  onSubmit(): void {
    this.submitted = true;
    this.notifyService.showInfo('Attempting to change password for ' + this.loginName, 'Change password')

    this.userService.completeChangingPasswordFor(this.loginName, this.resetKey, this.newPassword)
      .pipe()
      .subscribe({
        error: (err: any) => {
          this.notifyService.showError('An error has occurred while changing password. Are your username and reset code correct?', 'Error');
          this.submitted = false;
          return;
        },
        complete: () => {
          this.notifyService.showSuccess('Password has been successfully changed.', 'Success');
          this.submitted = false;
          this.router.navigate(['/main']);
          return;
        }
      });
  }
}

