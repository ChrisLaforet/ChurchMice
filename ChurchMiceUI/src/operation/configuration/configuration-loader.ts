import { Injectable } from '@angular/core';
import { ConfigurationService } from '@service/configuration/configuration.service';
import { Configuration, LocalConfigurationDto } from '@data/index';
import { UserContentListDto } from '@data/configuration/user-content-list.dto';
import { HttpEventType } from '@angular/common/http';
import { Observable, interval } from 'rxjs';
import { ignoreElements, timeout, startWith, takeWhile, tap } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ConfigurationLoader {

  private static POLL_INTERVAL_MSEC = 250;
  private static MAXWAIT_MSEC = 5 * 1000;

  private readonly configuration: Configuration;

  private localConfigurationDone: boolean = false;
  private localAboutDone: boolean = false;
  private localIndexDone: boolean = false;
  private localBeliefsDone: boolean = false;
  private localServicesDone: boolean = false;
  private localLogoDone: boolean = false;
  private totalWaitMsec = 0;

  constructor(configurationService: ConfigurationService) {
    this.configuration = new Configuration();

    configurationService.getConfiguration().pipe()
      .subscribe({
        next: (localConfiguration : LocalConfigurationDto) => {
          // console.log(localConfiguration);
          this.configuration.ministryName = localConfiguration.ministryName;

          this.localConfigurationDone = true;
          return;
        },
        error: (err: any) => {
          console.log('Error while fetching configuration ' + err);
          this.localConfigurationDone = true;
          return;
        }
      });

    configurationService.getUserContentList().pipe()
      .subscribe({
        next: (userContentList: UserContentListDto) => {
          if (userContentList.about) {
            this.loadHtmlContentFor('about', configurationService);
          } else {
            this.localAboutDone = true;
          }

          if (userContentList.index) {
            this.loadHtmlContentFor('index', configurationService);
          } else {
            this.localIndexDone = true;
          }

          if (userContentList.beliefs) {
            this.loadHtmlContentFor('beliefs', configurationService);
          } else {
            this.localBeliefsDone = true;
          }

          if (userContentList.services) {
            this.loadHtmlContentFor('services', configurationService);
          } else {
            this.localServicesDone = true;
          }

          if (userContentList.logo) {
            this.loadPngContentFor('logo', configurationService);
          } else {
            this.localLogoDone = true;
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
              this.localIndexDone = true;
              break;

            case 'about':
              this.configuration.about = this.downloadTextFile(event);
              this.localAboutDone = true;
              break;

            case 'beliefs':
              this.configuration.beliefs = this.downloadTextFile(event);
              this.localBeliefsDone = true;
              break;

            case 'services':
              this.configuration.services = this.downloadTextFile(event);
              this.localServicesDone = true;
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
              this.localLogoDone = true;
              break;
          }
        }
      });
  }

  private downloadBlobFile(data: any): Blob {
    return new Blob([data.body], { type: data.body.type });
  }

  public getConfiguration(): Configuration {
    return this.configuration;
  }

  private isFinishedLoading(): boolean {
    this.totalWaitMsec += ConfigurationLoader.POLL_INTERVAL_MSEC;
    if (this.totalWaitMsec >= ConfigurationLoader.MAXWAIT_MSEC) {
      return true;    // force our way out - done waiting
    }
//    console.log(this.localLogoDone + '|' + this.localBeliefsDone + '|' + this.localServicesDone + '|' + this.localConfigurationDone + '|' + this.localAboutDone + '|' + this.localIndexDone);
    return this.localLogoDone && this.localBeliefsDone && this.localServicesDone && this.localConfigurationDone && this.localAboutDone && this.localIndexDone;
  }

  public waitForConfigurationToLoad(): Observable<boolean> {
    return interval(ConfigurationLoader.POLL_INTERVAL_MSEC).pipe(
      tap(() => console.log('Waiting for configuration load...')),
      takeWhile(() => !this.isFinishedLoading()),
      ignoreElements()
    );
  }
}
