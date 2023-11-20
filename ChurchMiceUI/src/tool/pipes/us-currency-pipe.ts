import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'usCurrencyPipe'})
export class USCurrencyPipe implements PipeTransform {

  transform(valueToTransform: any): string {

    if (valueToTransform == null) {
      return '';
    }

    let result = this.processTransform(valueToTransform.toString());
    //console.log("returns => " + result);
    return result;
  }

  private processTransform(value: string): string {
    if (value == null || value === '') {
      return '';
    }

    if (value.startsWith('0')) {
      if (value.length == 1) {
        return value;
      }

      if (/^0.[0-9]{1,2}$/.test(value)) {
        return value;
      }

      let decimalPosition = value.indexOf('.');
      return value.slice(0, decimalPosition + 3);
    }

    if (/^([1-9][0-9]*)$|^([1-9][0-9]*.[0-9]{1,2})$/.test(value)) {
      return value;
    }

    if (/^[1-9][0-9]*.[0-9]*$/.test(value)) {
      let decimalPosition = value.indexOf('.');
      return value.slice(0, decimalPosition + 3);
    }

    return '';
  }
}
