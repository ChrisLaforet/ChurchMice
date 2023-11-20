import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MemberService } from '@service/member/member.service';
import { AuthService } from '@service/auth/auth.service';
import { RoleValidator } from '@app/helper';
import { NotificationService } from '@service/angular/notification.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { faArrowRotateLeft } from '@fortawesome/free-solid-svg-icons';
import { MemberDto } from '@data/dto/member.dto';
import { Roles } from '@service/user/roles';
import { MemberContainer } from '@tool/index';


@Component({
  selector: 'app-browse-members',
  templateUrl: './browse-members.component.html',
  styleUrls: ['./browse-members.component.css']
})
export class BrowseMembersComponent implements OnInit {

  faArrowRotateLeft = faArrowRotateLeft;

  public members = new Array<MemberContainer>();

  readonly isUserMember: boolean;

  constructor( private route: ActivatedRoute,
               private router: Router,
               private memberService: MemberService,
               private authService: AuthService,
               private roleValidator: RoleValidator,
               private notifyService: NotificationService,
               private domSanitizer: DomSanitizer) {
    this.memberService.getAllEditableMembers().subscribe(data => {
      this.sortMembers(data).forEach(member => {
        this.members.push(new MemberContainer(this.memberService, this.domSanitizer, this.notifyService, member));
      });
    });

    this.isUserMember = roleValidator.isUserAuthorizedFor([Roles.MEMBER, Roles.ADMINISTRATOR]);
  }

  ngOnInit(): void {
  }

  private sortMembers(members: MemberDto[]): MemberDto[] {
    return members.sort((a,b) => {
      let comparison = a.lastName.localeCompare(b.lastName);
      if (comparison != 0) {
        return comparison;
      }
      return a.firstName.localeCompare(b.firstName);
    })
  }
}
