import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import {
  NotificationService, Roles,
  UserManagementService, UserService
} from '@service/index';
import { RoleValidator } from '@app/helper';
import { MessageResponseDto } from '@data/dto/message-response.dto';
import { first } from 'rxjs/operators';
import { faArrowRotateLeft } from '@fortawesome/free-solid-svg-icons';


@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.css']
})
export class CreateUserComponent implements OnInit {

  submitted= false;
  newUserName = '';
  newFullName = '';
  newEmail = '';

  nameIsNotAvailable = false;


  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private roleValidator: RoleValidator,
    private userManagementService: UserManagementService,
    private userService: UserService,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.roleValidator.validateUserAuthorizedFor([Roles.ADMINISTRATOR]);
  }

  navigateBack(): void {
    this.router.navigate(['admin/manageUsers']);
  }

  isUserNameNotAvailable(): boolean {

    if (this.newUserName === null || this.newUserName.length == 0) {
      this.nameIsNotAvailable = false;
    }

    this.userService.checkUsernameIsAvailable(this.newUserName)
      .pipe()
      .subscribe({
        error: (err: any) => {
          this.nameIsNotAvailable = true;
          return;
        },
        complete: () => {
          this.nameIsNotAvailable = false;
          return ;
        }
      })
    return false;
  }

  onSubmit() {
    this.submitted = true;
    this.notifyService.showInfo('Requesting to create a new user', 'Create new user');

    this.userManagementService.createUser(this.newUserName, this.newFullName, this.newEmail)
      .pipe(first())
      .subscribe({
        next: (message: MessageResponseDto) => {
          const userId = message.other;
          this.notifyService.showSuccess('New user has been created.  Selecting edit mode for setting of role and password.', 'Success');
          this.submitted = false;
          this.router.navigate(['admin/editUser', userId]);
        },
        error: () => {
          this.notifyService.showError('An error has occurred while creating new login.', 'Error');
          this.submitted = false;
        },
        complete: () => {
        }
      });
  }

  protected readonly faArrowRotateLeft = faArrowRotateLeft;
}
