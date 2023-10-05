import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import {
  AuthService,
  NotificationService,
  UserManagementService
} from '@service/index';
import { ReCaptchaV3Service } from 'ngx-captcha';


@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent {

  isAddMode: boolean;
  loading = false;
  submitted = false;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private userManagementService: UserManagementService,
    private notifyService: NotificationService) {

    this.isAddMode = false;
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
