import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AuthService, NotificationService } from '@service/index';
import { AuthenticatedUser } from '@data/index';
import { TopBarComponent } from '@app/top-bar/top-bar.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  submitted = false;
  loginName = '';
  loginPassword = '';

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private notifyService: NotificationService) {
  }

  ngOnInit(): void {
    this.authService.logout();
    TopBarComponent
  }

  onSubmit(): void {
    this.submitted = true;
    this.notifyService.showInfo('Attempting to login ' + this.loginName, 'Login')

    this.authService.login(this.loginName, this.loginPassword)
      .pipe(first())
      .subscribe({
        next: (user: AuthenticatedUser) => {
          // TODO: update the header with correct user information!
          console.log(user);
          this.notifyService.showSuccess('Welcome, ' + user.fullName + ', you are successfully logged in', 'Success');
          //this.router.navigate(['TODO_SET_THIS_PATH_AND_UNCOMMENT']);
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

