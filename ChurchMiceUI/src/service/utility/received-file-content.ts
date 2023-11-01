export class ReceivedFileContent {
  fileName: string;
  fileSize: number;
  fileType: string;
  fileContentBase64: string;

  constructor(fileName: string, fileSize: number, fileType: string, fileContentBase64: string) {
    this.fileName = fileName;
    this.fileSize = fileSize;
    this.fileType = fileType;
    this.fileContentBase64 = fileContentBase64;
  }
}
