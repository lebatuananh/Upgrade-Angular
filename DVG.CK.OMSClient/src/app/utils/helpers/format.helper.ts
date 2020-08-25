import * as $ from 'jquery';

export class FormatHelper {

    constructor() { }

    public static formatCurrency(inputVal: string): string {
        const regexNumber = /^[0-9\-]+$/i;
        const decimal_separator = '.';
        const thousands_separator = ',';

        let val = inputVal;
        if (val.length > 1) {
            val = val.replace(/^0/, '').toString();
        }
        val = val.replace(/[,]|[^\d.-]/g, "");
        val += '';
        let x = val.split(decimal_separator);
        let x1 = x[0];
        let x2 = x.length > 1 ? decimal_separator + x[1] : '';

        let rgx = /(\d+)(\d{3})/;
        if (!regexNumber.test(x1) || (x2 !== decimal_separator && x2.replace(/./g, "") !== "" && !regexNumber.test(x2.replace(/./g, "")))) {
            return;
        }
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + thousands_separator + '$2');
        }
        return x1 + x2;
    }
}