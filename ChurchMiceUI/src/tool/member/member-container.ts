import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { MemberDto } from '@data/dto/member.dto';
import { first } from 'rxjs/operators';
import { MemberImagesDto } from '@data/dto/member-images.dto';
import { MemberService } from '@service/member/member.service';
import { NotificationService } from '@service/angular/notification.service';

export class MemberContainer {

  public member: MemberDto;
  public memberImageUrl: SafeUrl = this.domSanitizer.bypassSecurityTrustUrl("assets/images/ImagePlaceHolder.png");
  public memberImageUrls = new Array<SafeUrl>();

  constructor(private memberService: MemberService,
              private domSanitizer: DomSanitizer,
              private notifyService: NotificationService,
              member: MemberDto) {
    this.member = member;

    this.memberService.getMemberImages(member.id.toString(10))
      .pipe(first())
      .subscribe({
        next: (memberImages: MemberImagesDto) => {
          if (memberImages.images.length > 0) {
            memberImages.images.forEach(image => {
              let objectURL = `data:${image.fileType};base64,` + image.fileContentBase64;
              this.memberImageUrls.push(this.domSanitizer.bypassSecurityTrustUrl(objectURL));
            });

            this.memberImageUrl = this.memberImageUrls[0] ;
          }
        },
        error: (err: any) => {
          this.notifyService.showError(`Error while attempting to load member images for ${member.id}`, 'Error loading member images');
        },
        complete: () => {}
      });
  }
}
