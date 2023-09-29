export class MessageResponseDto {
  message: string;

  constructor(message: string) {
    this.message = message;
  }

  public toString(): string {
    return this.getMessage();
  }

  public getMessage(): string {
  return this.message;
  }
}
