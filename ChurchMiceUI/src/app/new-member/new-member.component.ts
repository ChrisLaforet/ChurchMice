import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { AuthService, NotificationService } from '@service/index';
import { TopBarComponent } from '@app/top-bar/top-bar.component';

@Component({
  selector: 'app-new-member',
  templateUrl: './new-member.component.html',
  styleUrls: ['./new-member.component.css']
})
export class NewMemberComponent implements OnInit {
  submitted = false;
  newFirstName = '';
  newLastName = '';
  newEmail = '';


  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private notifyService: NotificationService) {
  }

  ngOnInit(): void {
    this.authService.logout();
  }

  onSubmit(): void {
    this.submitted = true;
    this.notifyService.showInfo('Requesting to create a new member', 'Create new member')

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

