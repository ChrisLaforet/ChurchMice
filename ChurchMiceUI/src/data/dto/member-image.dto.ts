export class MemberImageDto {
  id: number;
  memberId: number;
  uploadUserId: string;
  fileType: string;
  fileContentBase64: string;
  isApproved: boolean;
  uploadDate?: string;

  constructor(id: number, memberId: number, uploadUserId: string, fileContentBase64: string,
              fileType: string, isApproved: boolean, uploadDate?: string) {
    this.id = id;
    this.memberId = memberId;
    this.uploadUserId = uploadUserId;
    this.fileContentBase64 = fileContentBase64;
    this.fileType = fileType;
    this.isApproved = isApproved;
    this.uploadDate = uploadDate;
  }
}
