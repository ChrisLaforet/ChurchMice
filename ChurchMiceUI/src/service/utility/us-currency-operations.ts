export class USCurrencyOperations {

  public static isCurrencyAmountValid(amount?: string): boolean {
    if (amount != null) {
      if (amount.endsWith('.')) {
        amount = amount.slice(0, amount.length - 1);
      }
      if (USCurrencyOperations.testCurrencyPattern(amount)) {
        return true;
      }
    }
    return false;
  }

  public static testCurrencyPattern(amount: string): boolean {
    return /^[0-9]+$|^[0-9]+(.[0-9]{1,2})$/.test(amount);
  }

  public static normalizeCurrency(amount: string): string {
    if (amount.endsWith('.')) {
      amount = amount.slice(0, amount.length - 1);
    }

    if (!USCurrencyOperations.testCurrencyPattern(amount)) {
      return '0.00';
    }

    let decimalPosition = amount.indexOf('.');
    if (decimalPosition < 0 || amount.endsWith('.')) {
      return amount + '.00';
    }
    if (/^[0-9]+.[0-9]{3,}$/.test(amount)) {
      return amount.slice(0, decimalPosition + 3);
    } else if (decimalPosition != amount.length - 1) {
      return amount + '0';
    }

    return amount;
  }
}
