import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { first } from 'rxjs/operators';
import {
  AuthService, IUploadService,
  MemberService,
  NotificationService,
  PhotoUploadService,
  Roles,
  UploadService
} from '@service/index';
import { MemberDto } from '@data/index';
import { RoleValidator } from '@app/helper';

@Component({
  selector: 'app-upload-member-image',
  templateUrl: './upload-member-image.component.html',
  styleUrls: ['./upload-member-image.component.css']
})
export class UploadMemberImageComponent implements OnInit {

  userId: string | undefined;
  memberId: string = '';
  member?: MemberDto;

  constructor(private formBuilder: UntypedFormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private memberService: MemberService,
              private authService: AuthService,
              private roleValidator: RoleValidator,
              private notifyService: NotificationService,
              private photoUploadService: PhotoUploadService) {
  }
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      // https://indepth.dev/tutorials/angular/indepth-guide-to-passing-parameters-via-routing
      const memberId = params['memberId'];
      this.loadMemberFor(memberId);
    });
  }

  getUploadService(): IUploadService {
    return this.photoUploadService;
  }

  private loadMemberFor(memberId: string) {
    this.memberId = memberId;
    this.memberService.getMember(memberId)
      .pipe(first())
      .subscribe({
        next: (member: MemberDto | null) => {
          if (member === null) {
            this.notifyService.showError('Empty record while loading member record by member\'s Id', 'Error loading member record');
          } else {
            this.member = member;
          }
        },
        error: (err: any) => {
          this.notifyService.showError('Error while attempting to load user record by user\'s Id', 'Error loading user record');
        },
        complete: () => {}
      });
  }

  isMemberUserIdOrAdministrator(): boolean {
    if (this.userId === undefined) {
      const user = this.authService.getAuthenticatedUser();
      if (user !== null) {
        this.userId = user.id;
      }
    }

    return (this.member?.userId !== undefined && this.member?.userId === this.userId) ||
      this.roleValidator.isUserAuthorizedFor([Roles.ADMINISTRATOR]);
  }

  onSubmit(): void {

  }

 // Based upon https://stackoverflow.com/questions/40214772/file-upload-in-angular

  // At the drag drop area
  // (drop)="onDropFile($event)"
  // onDropFile(event: DragEvent) {
  //   event.preventDefault();
  //   this.uploadFile(event.dataTransfer.files);
  // }

  // At the drag drop area
  // (dragover)="onDragOverFile($event)"
  // onDragOverFile(event) {
  //   event.stopPropagation();
  //   event.preventDefault();
  // }

  // At the file input element
  // (change)="selectFile($event)"
  // selectFile(event) {
  //   this.uploadFile(event.target.files);
  // }

  uploadFile(files: FileList) {
    if (files.length == 0) {
      console.log("No file selected!");
      return

    }
    let file: File = files[0];

  //
  //     .subscribe(
  //       event => {
  //         if (event.type == HttpEventType.UploadProgress) {
  //           const percentDone = Math.round(100 * event.loaded / event.total);
  //           console.log(`File is ${percentDone}% loaded.`);
  //         } else if (event instanceof HttpResponse) {
  //           console.log('File is completely loaded!');
  //         }
  //       },
  //       (err) => {
  //         console.log("Upload Error:", err);
  //       }, () => {
  //         console.log("Upload done");
  //       }
  //     )
   }

    protected readonly undefined = undefined;
}

