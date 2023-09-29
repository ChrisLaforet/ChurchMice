import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from '@app/nav-menu/nav-menu.component';
import { AppInitializer, AuthGuard } from '@app/helper';
import { HomeComponent } from './home/home.component';
import { IApiKeyReaderService } from '@service/key-support/api-key-reader.service.interface';
import { ApiKeyReaderService } from '@service/key-support/api-key-reader.service';
import { IRecaptchaKeyReaderService } from '@service/key-support/recaptcha-key-reader.service.interface';
import { RecaptchaKeyReaderService } from '@service/key-support/recaptcha-key-reader.service';
import { TopBarComponent } from '@app/top-bar/top-bar.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome'
import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { CurrentUserService } from '@service/auth/current-user.service';
import { NgbDateParserFormatter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { InputDirectiveModule } from '@app/directive/input-directive.module';
import { CommonModule } from '@angular/common';
import { UnitedStatesDateParserFormatter } from '@app/formatter';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { LoginComponent } from '@app/login/login.component';
import { LogoutComponent } from '@app/logout/logout.component';
import { ForgottenPasswordComponent } from '@app/forgotten-password/forgotten-password.component';
import { USCurrencyPipe } from '@app/pipes';
import { NewLoginComponent } from '@app/new-login/new-login.component';
import { NewMemberComponent } from '@app/new-member/new-member.component';
import { NgxCaptchaModule } from 'ngx-captcha';
import { UploadMemberImageComponent } from '@app/upload-member-image/upload-member-image.component';
import { UserContentComponent } from '@app/user-content/user-content.component';
import { ConfigurationLoader } from '../operation';
import { ConfigurationService } from '@service/configuration/configuration.service';
import { ValidateEmailComponent } from '@app/validate-email/validate-email.component';
import { ChangePasswordComponent } from '@app/change-password/change-password.component';

library.add(fas, far);

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    TopBarComponent,
    LoginComponent,
    LogoutComponent,
    ForgottenPasswordComponent,
    NewLoginComponent,
    NewMemberComponent,
    UploadMemberImageComponent,
    USCurrencyPipe,
    UserContentComponent,
    ValidateEmailComponent,
    ChangePasswordComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FontAwesomeModule,
    CommonModule,
    FormsModule,
    NgbModule,
    ReactiveFormsModule,
    NgxCaptchaModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'home', component: HomeComponent},

      {path: 'login', component: LoginComponent},
      {path: 'logout', component: LogoutComponent},

      {path: 'forgotten', component: ForgottenPasswordComponent},
      {path: 'newLogin', component: NewLoginComponent},
      {path: 'validateUserEmail', component: ValidateEmailComponent},
      {path: 'changePassword', component: ChangePasswordComponent},

      {path: 'newMember', component: NewMemberComponent, canActivate: [AuthGuard]},

      {path: 'about', component: UserContentComponent, data: {page: 'about'}},
      {path: 'main', component: UserContentComponent, data: {page: 'index'}},
      {path: 'services', component: UserContentComponent, data: {page: 'services'}},
      {path: 'beliefs', component: UserContentComponent, data: {page: 'beliefs'}}

    ]),
    InputDirectiveModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [
    {
      provide: IApiKeyReaderService,
      useClass: ApiKeyReaderService
    },
    {
      provide: IRecaptchaKeyReaderService,
      useClass: RecaptchaKeyReaderService
    },
    {
      provide: CurrentUserService,
      useClass: CurrentUserService
    },
    {
      provide: NgbDateParserFormatter,
      useClass: UnitedStatesDateParserFormatter
    },
    { provide: APP_INITIALIZER, useFactory: AppInitializer, multi: true, deps: [ConfigurationLoader, ConfigurationService] },
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor() {
    library.add(fas, far);
  }
}

