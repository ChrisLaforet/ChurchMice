import { Injectable } from '@angular/core';
import { ConfigurationService } from '@service/configuration/configuration.service';
import { Configuration, LocalConfigurationDto } from '@data/index';
import { UserContentListDto } from '@data/configuration/user-content-list.dto';
import { HttpEventType, HttpResponse } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class ConfigurationLoader {

  private configuration: Configuration;

  constructor(configurationService: ConfigurationService) {
    this.configuration = new Configuration();

    configurationService.getConfiguration().pipe()
      .subscribe({
        next: (localConfiguration : LocalConfigurationDto) => {
          // console.log(localConfiguration);
          this.configuration.ministryName = localConfiguration.ministryName;
          return;
        },
        error: (err: any) => {
          console.log('Error while fetching configuration ' + err);
          return;
        }
      });

    configurationService.getUserContentList().pipe()
      .subscribe({
        next: (userContentList: UserContentListDto) => {
          if (userContentList.about) {
            this.loadHtmlContentFor('about', configurationService);
          }
          if (userContentList.index) {
            this.loadHtmlContentFor('index', configurationService);
          }
          if (userContentList.beliefs) {
            this.loadHtmlContentFor('beliefs', configurationService);
          }
          if (userContentList.services) {
            this.loadHtmlContentFor('services', configurationService);
          }
          if (userContentList.logo) {
            this.loadPngContentFor('logo', configurationService);
          }
        }
      });
  }

  private loadHtmlContentFor(key: string, configurationService: ConfigurationService) {
    configurationService.getUserHtmlContent(key)
      .subscribe((event) => {
        if (event.type === HttpEventType.Response) {
          switch (key) {
            case 'index':
              this.configuration.index = this.downloadTextFile(event);
              break;
            case 'about':
              this.configuration.about = this.downloadTextFile(event);
              break;
            case 'beliefs':
              this.configuration.beliefs = this.downloadTextFile(event);
              break;
            case 'services':
              this.configuration.services = this.downloadTextFile(event);
              break;
          }
        }
      });
  }

  private downloadTextFile(data: any): string {
    return '' + [data.body];
  }

  private loadPngContentFor(key: string, configurationService: ConfigurationService) {
    configurationService.getUserPngContent(key)
      .subscribe((event) => {
        if (event.type === HttpEventType.Response) {
          switch (key) {
            case 'logo':
              this.configuration.logo = this.downloadBlobFile(event);
              // console.log(this.configuration);
              break;
          }
        }
      });
  }

  private downloadBlobFile(data: any): Blob {
    return new Blob([data.body], { type: data.body.type });
  }

  public GetConfiguration(): Configuration {
    return this.configuration;
  }
}
