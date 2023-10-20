import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { ConfigurationService } from '@service/configuration/configuration.service';
import { NotificationService } from '@service/angular/notification.service';
import { LocalConfigurationDto } from '@data/configuration/local-configuration.dto';
import { first } from 'rxjs/operators';
import { RoleValidator } from '@app/helper';
import { Roles } from '@service/user/roles';
import { faFacebook, faYoutube, faInstagram, faVimeo } from '@fortawesome/free-brands-svg-icons';

@Component({
  selector: 'app-configure',
  templateUrl: './configure.component.html',
  styleUrls: ['./configure.component.css']
})
export class ConfigureComponent implements OnInit {

  faFacebook = faFacebook;
  faYoutube = faYoutube;
  faInstagram = faInstagram;
  faVimeo = faVimeo;

  submitted = false;

  configMinistryName = '';
  configBaseUrl = '';
  configMinistryAddress1 = '';
  configMinistryAddress2 = '';
  configMinistryAddress3 = '';
  configMinistryPhone = '';
  configFacebookUrl = '';
  configYouTubeUrl = '';
  configVimeoUrl = '';
  configInstagramUrl = '';

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private roleValidator: RoleValidator,
    private configurationService: ConfigurationService,
    private notifyService: NotificationService) {

    configurationService.getConfiguration()
      .pipe(first())
      .subscribe({
        next: (config: LocalConfigurationDto) => {
          this.configBaseUrl = config.baseUrl;
          this.configMinistryName = config.ministryName;
          this.configMinistryAddress1 = config.ministryAddress1;
          this.configMinistryAddress2 = config.ministryAddress2;
          this.configMinistryAddress3 = config.ministryAddress3;
          this.configMinistryPhone = config.ministryPhone;
          this.configFacebookUrl = config.facebookUrl;
          this.configYouTubeUrl = config.youTubeUrl;
          this.configVimeoUrl = config.vimeoUrl;
          this.configInstagramUrl = config.instagramUrl;
        },
        error: (err) => {
          console.error(`Error loading local configuration for editing {err}`);
        },
        complete:() => {
        }
      })
  }

  ngOnInit(): void {
      this.roleValidator.validateUserAuthorizedFor([Roles.ADMINISTRATOR]);
  }

  onSubmit(): void {
    this.submitted = true;
    this.notifyService.showInfo('Attempting to save configuration', 'Configuration')

    this.configurationService.saveConfiguration(this.configMinistryName, this.configBaseUrl, this.configMinistryAddress1,
              this.configMinistryAddress2, this.configMinistryAddress3, this.configMinistryPhone, this.configFacebookUrl,
              this.configYouTubeUrl, this.configVimeoUrl, this.configInstagramUrl)
      .pipe()
      .subscribe({
        error: (err: any) => {
          this.submitted = false;
          this.notifyService.showError("Error while saving configuration", "Error saving")
        },
        complete: () => {
          this.submitted = false;
          this.notifyService.showSuccess("Saved configuration", "Success")
        }
      });
  }
}

