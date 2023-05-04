import { Injectable } from '@angular/core';
import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DatepickerUtilities } from '@app/utility/datepicker-utilities';

// From the article: https://bradleycarey.com/posts/ng-bootstrap-us-date-formatter/
// and: https://seegatesite.com/tutorial-to-change-date-format-ng-bootstrap-datepicker-angular-5/

@Injectable()
export class UnitedStatesDateParserFormatter extends NgbDateParserFormatter {

  parse(value: string): NgbDateStruct | null {
    return DatepickerUtilities.createNgbDateFromUSDateString(value);
  }

  format(date: NgbDateStruct | null): string {
    return DatepickerUtilities.createUSDateStringFromNgbDate(date);
  }
}
