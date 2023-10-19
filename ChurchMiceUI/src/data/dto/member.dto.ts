export class MemberDto {
  id: number;
  firstName: string;
  lastName: string;
  email?: string;
  homePhone?: string;
  mobilePhone?: string;
  mailingAddress1?: string;
  mailingAddress2?: string;
  city?: string;
  state?: string;
  zip?: string;
  birthday?: string;
  anniversary?: string;
  memberSince?: string;
  userId?: string;

  constructor(id: number, firstName: string, lastName: string, email?: string,
              homePhone?: string, mobilePhone?: string, mailingAddress1?: string,
              mailingAddress2?: string, city?: string, state?: string, zip?: string,
              birthday?: string, anniversary?: string, memberSince?: string, userId?: string) {
    this.id = id;
    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;
    this.homePhone = homePhone;
    this.mobilePhone = mobilePhone;
    this.mailingAddress1 = mailingAddress1;
    this.mailingAddress2 = mailingAddress2;
    this.city = city;
    this.state = state;
    this.zip = zip;
    this.birthday = birthday;
    this.anniversary = anniversary;
    this.memberSince = memberSince;
    this.userId = userId;
  }
}

