import { NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { formatDate } from '@angular/common';

export class DatepickerUtilities {

  public static createNgbDateFromUSDateString(usDate?: string): NgbDate | null {
    if (usDate != null) {
      const parts = usDate.split('/');
      if (parts.length === 3 &&
        DatepickerUtilities.isNumber(parts[0]) &&
        DatepickerUtilities.isNumber(parts[1]) &&
        DatepickerUtilities.isNumber(parts[2])) {
        return NgbDate.from({year: parseInt(parts[2]), month: parseInt(parts[0]), day: parseInt(parts[1])});
      }
    }

    return null;
  }

  public static createUSDateStringFromNgbDate(date: NgbDateStruct | null): string {
    return date &&
    DatepickerUtilities.isNumber(date.day) &&
    DatepickerUtilities.isNumber(date.month) &&
    DatepickerUtilities.isNumber(date.year) ?
      `${DatepickerUtilities.padNumber(date.month)}/${DatepickerUtilities.padNumber(date.day)}/${date.year}` : '';
  }

  public static isNumber(value: any): value is number {
    return !isNaN(parseInt(value));
  }

  public static padNumber(value: number) {
    if (DatepickerUtilities.isNumber(value)) {
      return `0${value}`.slice(-2);
    } else {
      return '';
    }
  }

  public static currentUSDateString(): string {
    return formatDate(new Date(), 'MM/dd/yyyy', 'en');
  }

  public static currentNgbDate(): NgbDateStruct {
    let today = new Date();
    return {month: today.getMonth() + 1, day: today.getDate(), year: today.getFullYear()};
  }

  public static createNgbDateStructFromUSDateString(usDate: string): NgbDateStruct | null {
    if (usDate != null) {
      const parts = usDate.split('/');
      if (parts.length === 3 &&
        DatepickerUtilities.isNumber(parts[0]) &&
        DatepickerUtilities.isNumber(parts[1]) &&
        DatepickerUtilities.isNumber(parts[2])) {
        return {month: parseInt(parts[0]), day: parseInt(parts[1]), year: parseInt(parts[2])};
      }
    }
    return null;
  }
}
