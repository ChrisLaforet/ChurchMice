import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { AuthService, IRecaptchaKeyReaderService, NotificationService, UserService } from '@service/index';
import { ReCaptchaV3Service } from 'ngx-captcha';
import { first } from 'rxjs/operators';
import { MessageResponseDto } from '@data/dto/message-response.dto';


@Component({
  selector: 'app-new-login',
  templateUrl: './new-login.component.html',
  styleUrls: ['./new-login.component.css']
})
export class NewLoginComponent implements OnInit {

  submitted = false;
  newUserName = '';
  newFullName = '';
  newEmail = '';
  newPassword = '';
  newConfirmPassword = '';

  siteKey: any;

  // https://stackoverflow.com/questions/46180915/angular-2-check-if-email-exists
  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private userService: UserService,
    private notifyService: NotificationService,
    private reCaptchaV3Service: ReCaptchaV3Service,
    private recaptchaKeyReaderService: IRecaptchaKeyReaderService) {

    this.siteKey = recaptchaKeyReaderService.getSiteKey();
  }

  ngOnInit(): void {
    this.authService.logout();
  }

  onSubmit(): void {
    this.submitted = true;
    this.notifyService.showInfo('Requesting to create a new login', 'Create new login');

    this.userService.createUserFor(this.newUserName, this.newPassword, this.newEmail, this.newFullName)
      .pipe()
      .subscribe({
        error: (err: any) => {
          this.notifyService.showError('An error has occurred while creating new login.', 'Error');
          this.submitted = false;
          return;
        },
        complete: () => {
          this.notifyService.showSuccess('New login user has been created.  Check your Email for instructions.', 'Success');
          this.submitted = false;
          this.router.navigate(['/main']);
          return;
        }
      });

    // this.authService.login(this.loginName, this.loginPassword)
    //   .pipe(first())
    //   .subscribe({
    //     next: (user: AuthenticatedUser) => {
    //       // TODO: update the header with correct user information!
    //       console.log(user);
    //       this.notifyService.showSuccess('Welcome, ' + user.fullName + ', you are successfully logged in', 'Success');
    //       //this.router.navigate(['TODO_SET_THIS_PATH_AND_UNCOMMENT']);
    //     },
    //     error: (err: any) => {
    //       this.submitted = false;
    //       return;
    //     },
    //     complete: () => {
    //       this.submitted = false;
    //       return;
    //     }
    //   });
  }
}

