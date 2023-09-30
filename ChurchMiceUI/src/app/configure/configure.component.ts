import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { ConfigurationService } from '@service/configuration/configuration.service';
import { NotificationService } from '@service/angular/notification.service';

@Component({
  selector: 'app-configure',
  templateUrl: './configure.component.html',
  styleUrls: ['./configure.component.css']
})
export class ConfigureComponent {
  submitted = false;
  configMinistryName = '';
  configBasUrl = '';

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private configurationService: ConfigurationService,
    private notifyService: NotificationService) {
  }

  onSubmit(): void {
    this.submitted = true;
    this.notifyService.showInfo('Attempting to save configuration', 'Configuration')

    // this.authService.login(this.loginName, this.loginPassword)
    //   .pipe()
    //   .subscribe({
    //     next: (user: AuthenticatedUser) => {
    //       // TODO: update the header with correct user information!
    //       console.log(user);
    //       this.notifyService.showSuccess('Welcome, ' + user.fullName + ', you are successfully logged in', 'Success');
    //     },
    //     error: (err: any) => {
    //       this.submitted = false;
    //       return;
    //     },
    //     complete: () => {
    //       this.submitted = false;
    //       this.router.navigate(['/main']);
    //       return;
    //     }
    //   });
  }
}

