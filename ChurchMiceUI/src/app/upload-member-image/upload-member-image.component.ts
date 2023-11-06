import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { first } from 'rxjs/operators';
import {
  AuthService, IUploadService,
  MemberService,
  NotificationService,
  PhotoUploadService,
  Roles
} from '@service/index';
import { faArrowRotateLeft } from '@fortawesome/free-solid-svg-icons';
import { MemberDto, MemberImageDto } from '@data/index';
import { RoleValidator } from '@app/helper';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { MemberImagesDto } from '@data/dto/member-images.dto';

@Component({
  selector: 'app-upload-member-image',
  templateUrl: './upload-member-image.component.html',
  styleUrls: ['./upload-member-image.component.css']
})
export class UploadMemberImageComponent implements OnInit {

  faArrowRotateLeft = faArrowRotateLeft;

  userId: string | undefined;
  memberId: string = '';
  member?: MemberDto;
  memberImages?: MemberImageDto[];

  constructor(private formBuilder: UntypedFormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private memberService: MemberService,
              private authService: AuthService,
              private roleValidator: RoleValidator,
              private notifyService: NotificationService,
              private photoUploadService: PhotoUploadService,
              private domSanitizer: DomSanitizer) {
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      // https://indepth.dev/tutorials/angular/indepth-guide-to-passing-parameters-via-routing
      const memberId = params['memberId'];
      this.loadMemberFor(memberId);
      this.loadMemberImagesFor(memberId);
    });
  }

  getUploadService(): IUploadService {
    return this.photoUploadService;
  }

  getMemberId(): number {
    return Number(this.memberId);
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

  private loadMemberImagesFor(memberId: string) {
    this.memberService.getMemberImages(memberId)
      .pipe(first())
      .subscribe({
        next: (memberImages: MemberImagesDto) => {
          this.memberImages = memberImages.images;
        },
        error: (err: any) => {
          this.notifyService.showError('Error while attempting to load user images by user\'s Id', 'Error loading user images');
        },
        complete: () => {}
      });
  }

  reloadImages() {
    this.loadMemberImagesFor(this.memberId);
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


  navigateBack(): void {
    this.router.navigate(['manageMembers']);
  }


// Show images: https://stackoverflow.com/questions/55967908/angular-display-byte-array-as-image

  hasImages(): boolean {
    return this.memberImages !== undefined && this.memberImages?.length > 0
  }

  getImageFrom(memberImage: MemberImageDto): SafeUrl {
    let objectURL = `data:${memberImage.fileType};base64,` + memberImage.fileContentBase64;
    return this.domSanitizer.bypassSecurityTrustUrl(objectURL);
  }
}
