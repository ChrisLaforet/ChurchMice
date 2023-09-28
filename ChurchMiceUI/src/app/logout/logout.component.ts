import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@service/auth/auth.service';
import { NotificationService } from '@service/angular/notification.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
})
export class LogoutComponent implements OnInit {

  constructor(private authService: AuthService,
              private notifyService: NotificationService,
              private router: Router) {

  }

  ngOnInit(): void {
    this.notifyService.showInfo('Logging you out as your requested', 'Log out');
    this.authService.logout();
    this.router.navigate(['/home']);
  }
}
