export class MessageResponseDto {
  message: string;
  other: string | null;

  constructor(message: string, other: string | null) {
    this.message = message;
    this.other = other;
  }
}
