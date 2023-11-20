import { Pipe, PipeTransform } from '@angular/core';
import { parsePhoneNumber } from 'libphonenumber-js';

@Pipe({name: 'politeDatePipe'})
export class PoliteDatePipe implements PipeTransform {

  private monthNames = ["Unknown", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];


  transform(possibleDate: any): string {

    if (!possibleDate || possibleDate.toString() === '') {
      return '';
    }

    var formatted = this.formatFullUSDate(possibleDate);
    if (formatted) {
      return formatted;
    }

    formatted = this.formatISODate(possibleDate);
    if (formatted) {
      return formatted;
    }

    formatted = this.formatShortUSDate(possibleDate);
    if (formatted) {
      return formatted;
    }
    return '';
  }

  private formatFullUSDate(possibleDate: any): string | null {

    // is this a date formatted MM/DD/YYYY?
    const pattern = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;
    const match = possibleDate.toString().match(pattern);
    if (match.length == 4) {
      return this.getMonthFor(match[1]) + ' ' + match[2];
    }
    return null;
  }

  private formatISODate(possibleDate: any): string | null {

    // is this a date formatted YYYY-MM-DD
    let pattern = /^(\d{4})-(\d{1,2})-(\d{1,2})$/;
    const match = possibleDate.toString().match(pattern);
    if (match.length == 4) {
      return this.getMonthFor(match[2]) + ' ' + match[3];
    }
    return null;
  }

  private formatShortUSDate(possibleDate: any): string | null {

    // is this a date formatted MM/DD?
    let pattern = /^(\d{1,2})\/(\d{1,2})$/;
    const match = possibleDate.toString().match(pattern);
    if (match.length == 3) {
      return this.getMonthFor(match[1]) + ' ' + match[2];
    }
    return null;
  }

  private getMonthFor(monthNumber: string): string {
    let month = parseInt(monthNumber, 10);
    if (month > 12) {
      month = 0;
    }
    return this.monthNames[month];
  }
}
