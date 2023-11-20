import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { MemberDto } from '@data/dto/member.dto';
import { first } from 'rxjs/operators';
import { MemberImagesDto } from '@data/dto/member-images.dto';
import { MemberService } from '@service/member/member.service';
import { NotificationService } from '@service/angular/notification.service';

export class MemberContainer {

  public member: MemberDto;
  public memberImageUrl: SafeUrl = this.domSanitizer.bypassSecurityTrustUrl("assets/images/ImagePlaceHolder.png");

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
            const memberImage = memberImages.images[0];
            let objectURL = `data:${memberImage.fileType};base64,` + memberImage.fileContentBase64;
            this.memberImageUrl = this.domSanitizer.bypassSecurityTrustUrl(objectURL);
          }
        },
        error: (err: any) => {
          this.notifyService.showError(`Error while attempting to load member images for ${member.id}`, 'Error loading member images');
        },
        complete: () => {}
      });
  }
}
