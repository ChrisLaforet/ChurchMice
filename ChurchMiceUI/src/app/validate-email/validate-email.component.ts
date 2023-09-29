import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { UserService, NotificationService } from '@service/index';

@Component({
  selector: 'app-validate-email',
  templateUrl: './validate-email.component.html',
})
export class ValidateEmailComponent implements OnInit {
  submitted = false;
  loginName = '';
  loginPassword = '';

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
    this.notifyService.showInfo('Attempting to validate email for ' + this.loginName, 'Validate Email')

    this.userService.validateEmailFor(this.loginName, this.loginPassword)
      .pipe()
      .subscribe({
        error: (err: any) => {
          this.notifyService.showError('An error has occurred while validating the Email address. Are your username and password correct?', 'Error');
          this.submitted = false;
          return;
        },
        complete: () => {
          this.notifyService.showSuccess('Email for new login user has been validated.  Basic access has been granted.', 'Success');
          this.submitted = false;
          this.router.navigate(['/main']);
          return;
        }
      });
  }
}

