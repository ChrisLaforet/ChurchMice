import { Pipe, PipeTransform } from "@angular/core";
import { parsePhoneNumber } from 'libphonenumber-js';

// Borrowed from: https://stackoverflow.com/questions/36895431/angular-2-pipes-how-to-format-a-phone-number

@Pipe({
  name: "usPhonePipe"
})
export class USPhonePipe implements PipeTransform {
  transform(valueToTransform: any) : string {
    let stringPhone = USPhonePipe.unformatPhoneNumber(valueToTransform + '');
    if (stringPhone.length < 2) {
      return stringPhone;
    }

    const phoneNumber = parsePhoneNumber(stringPhone, 'US');
    const formatted = phoneNumber.formatNational();
    return formatted;
  }

  private static unformatPhoneNumber(phoneNumber: string): string {
    return phoneNumber.replace(/^\D+/g, '');
  }
}
