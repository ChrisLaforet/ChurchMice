import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MemberService } from '@service/member/member.service';
import { AuthService } from '@service/auth/auth.service';
import { RoleValidator } from '@app/helper';
import { NotificationService } from '@service/angular/notification.service';
import { PhotoUploadService } from '@service/utility/photo-upload.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { faArrowRotateLeft } from '@fortawesome/free-solid-svg-icons';
import { MemberDto } from '@data/dto/member.dto';
import { MemberImageDto } from '@data/dto/member-image.dto';
import { first } from 'rxjs/operators';
import { MemberImagesDto } from '@data/dto/member-images.dto';

@Component({
  selector: 'app-browse-members',
  templateUrl: './browse-members.component.html',
  styleUrls: ['./browse-members.component.css']
})
export class BrowseMembersComponent implements OnInit {

  faArrowRotateLeft = faArrowRotateLeft;

  public members = new Array<MemberDto>();

  constructor( private route: ActivatedRoute,
               private router: Router,
               private memberService: MemberService,
               private authService: AuthService,
               private roleValidator: RoleValidator,
               private notifyService: NotificationService,
               private photoUploadService: PhotoUploadService,
               private domSanitizer: DomSanitizer) {
    this.memberService.getAllEditableMembers().subscribe(data => {
      this.members = this.sortMembers(data);
      this.members = data;

    });
  }

  ngOnInit(): void {
  }

  sortMembers(members: MemberDto[]): MemberDto[] {
    return members.sort((a,b) => {
      let comparison = a.lastName.localeCompare(b.lastName);
      if (comparison != 0) {
        return comparison;
      }
      return a.firstName.localeCompare(b.firstName);
    })
  }

  getImageFor(memberId: number): SafeUrl {
    // this.memberService.getMemberImages(memberId.toString(10))
    //   .pipe(first())
    //   .subscribe({
    //     next: (memberImages: MemberImagesDto) => {
    //       if (memberImages.images.length > 0) {
    //         const memberImage = memberImages.images[0];
    //         let objectURL = `data:${memberImage.fileType};base64,` + memberImage.fileContentBase64;
    //         return this.domSanitizer.bypassSecurityTrustUrl(objectURL);
    //       }
    //       return this.domSanitizer.bypassSecurityTrustUrl("assets/images/ImagePlaceHolder.png")
    //     },
    //     error: (err: any) => {
    //       this.notifyService.showError('Error while attempting to load user images by user\'s Id', 'Error loading user images');
    //     },
    //     complete: () => {}
    //   });

    return this.domSanitizer.bypassSecurityTrustUrl("assets/images/ImagePlaceHolder.png");
  }
}
