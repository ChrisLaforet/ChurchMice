import { DatepickerUtilities } from '@app/utility/datepicker-utilities';

export class DateOperations {

  public static ConvertUSDateStringToDate(usDate: string): Date | null {
    const parts = usDate.split('/');
    if (parts.length === 3 &&
      DatepickerUtilities.isNumber(parts[0]) &&
      DatepickerUtilities.isNumber(parts[1]) &&
      DatepickerUtilities.isNumber(parts[2])) {
      return new Date(parts[2], parts[0], parts[1], 0, 0, 0, 0);
    }
    return null;
  }

  public static ConvertUSDateStringToISODateString(usDate: string): string {
    const parts = usDate.split('/');
    if (parts.length === 3 &&
      DatepickerUtilities.isNumber(parts[0]) &&
      DatepickerUtilities.isNumber(parts[1]) &&
      DatepickerUtilities.isNumber(parts[2])) {
      return parts[2] + '-' + parts[0] + '-' + parts[1] + 'T00:00:00';
    }
    return '';
  }

  public static YearsBetweenDates(date1: Date, date2: Date): number {
    if (date1 == null || date2 == null) {
      return 0;
    }
    if (date1.getTime() > date2.getTime()) {
      return DateOperations.CalculateYearsBetweenDates(date2, date1);
    }
    return DateOperations.CalculateYearsBetweenDates(date1, date2);
  }

  private static CalculateYearsBetweenDates(oldestDate: Date, recentDate: Date): number {
    // using the Sleek foundation javascript function from https://stackoverflow.com/questions/8152426/how-can-i-calculate-the-number-of-years-between-two-dates
    let diffMs = recentDate.getTime() - oldestDate.getTime();
    let ageDate = new Date(diffMs); // milliseconds from epoch
    return Math.abs(ageDate.getUTCFullYear() - 1970);
  }
}
