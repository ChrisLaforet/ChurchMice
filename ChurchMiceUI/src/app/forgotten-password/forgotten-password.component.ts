import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { UserService, NotificationService } from '@service/index';
import { TopBarComponent } from '@app/top-bar/top-bar.component';


@Component({
  selector: 'app-forgotten-password',
  templateUrl: './forgotten-password.component.html',
  styleUrls: ['./forgotten-password.component.css']
})
export class ForgottenPasswordComponent implements OnInit {
  submitted = false;
  loginName = '';

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private notifyService: NotificationService) {
  }

  ngOnInit(): void {
    TopBarComponent
  }

  onSubmit(): void {
    this.submitted = true;
    this.notifyService.showInfo('Attempting to request password reset for  ' + this.loginName, 'Password Reset')

    this.userService.requestPasswordChangeFor(this.loginName)
      .pipe()
      .subscribe({
        error: (err: any) => {
          this.notifyService.showError('An error has occurred while requesting password change. Is your username correct?', 'Error');
          this.submitted = false;
          return;
        },
        complete: () => {
          this.notifyService.showSuccess('Password request has been successfully started.  Check your Email for reset instructions.', 'Success');
          this.submitted = false;
          this.router.navigate(['/changePassword']);
          return;
        }
      });
  }
}

