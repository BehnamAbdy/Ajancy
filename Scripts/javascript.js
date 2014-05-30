/*=========== public alerts ================*/
var codeNotFound = 'کد مورد نظر یافت نشد';
var preDeleteMessagse = 'آیا رکورد انتخاب شده حذف گردد؟';

function getGender(gender) {
    switch (gender) {
        case 'F':
            return 'زن';

        case 'M':
            return 'مرد';

        case 'B':
            return 'مختلط';
    }
}

function objectPosition(obj) {
    var curleft = 0;
    var curtop = 0;
    if (obj.offsetParent) {
        do {
            curleft += obj.offsetLeft;
            curtop += obj.offsetTop;
        } while (obj = obj.offsetParent);
    }
    return [curleft, curtop];
}

function getTime(classTime) {
    var time = classTime.split(':');
    return time[0] + ':' + time[1];
}

function isDate(date) {
    if (date.search(/\d{4}(\/|-)\d{2}(\/|-)\d{2}/g) == -1)
        return false;
    var str = date.split(date.charAt(4));
    var day = parseFloat(str[2]);
    var month = parseFloat(str[1]);
    var year = parseFloat(str[0]);
    if (day > 31 || day < 1 || month > 12 || month < 1 || year > 1500 || year < 1300)
        return false;
    return true;
}

function txt_exit(e) {
    e.className = 'textbox';
}

function txt_focus(e) {
    e.className = 'textbox-focused';
}

function txtmiddle_exit(e) {
    e.className = 'textbox-middle';
}

function txtmiddle_focus(e) {
    e.className = 'textbox-middle-focused';
}

function txtlarge_focus(e) {
    e.className = 'textbox-large-focused';
}

function txtlarge_exit(e) {
    e.className = 'textbox-large';
}

function getPlateNumber(plateNumber) {
    if (plateNumber != null) {
        var num = plateNumber.split(':');
        return num[0] + getAlphabet(num[1]) + num[2] + ' ایران ' + num[3];
    }
}

function getAlphabet(code) {
    switch (code) {
        case '0': return 'الف';
        case '1': return 'ب';
        case '2': return 'پ';
        case '3': return 'ت';
        case '4': return 'ث';
        case '5': return 'ج';
        case '6': return 'چ';
        case '7': return 'ح';
        case '8': return 'خ';
        case '9': return 'د';
        case '10': return 'ذ';
        case '11': return 'ر';
        case '12': return 'ز';
        case '13': return 'ژ';
        case '14': return 'س';
        case '15': return 'ش';
        case '16': return 'ص';
        case '17': return 'ض';
        case '18': return 'ط';
        case '19': return 'ظ';
        case '20': return 'ع';
        case '21': return 'غ';
        case '22': return 'ف';
        case '23': return 'ق';
        case '24': return 'ک';
        case '25': return 'گ';
        case '26': return 'ل';
        case '27': return 'م';
        case '28': return 'ن';
        case '29': return 'و';
        case '30': return 'ه';
        case '31': return 'ی';
    }
}

function getQueryString(param, key) {
    var values = new Array(), keys = new Array();
    param = param.substring(1, param.length)
    param = param.split('&');
    for (i = 0; i < param.length; i++) {
        keys[i] = param[i].split('=')[0];
        values[i] = param[i].split('=')[1];
    }
    for (i = 0; i < keys.length; i++) {
        if (keys[i] == key)
            return values[i];
    }
    return null;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function isFloatKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode > 31 && (charCode < 47 || charCode > 57)))
        return false;

    return true;
}

function nationalCodeValidate(source, arguments) {
    if (arguments.Value.length == 10) {
        arguments.IsValid = true;
    }
    else {
        arguments.IsValid = false;
    }
}

function VINValidate(source, arguments) {
    if (arguments.Value.length == 17) {
        arguments.IsValid = true;
    }
    else {
        arguments.IsValid = false;
    }
}

function checkMelliCode(varmellicode) {
    var meli_code;
    meli_code = varmellicode.value;
    if (meli_code.length == 10) {
        if (meli_code == '1111111111' ||
            meli_code == '0000000000' ||
            meli_code == '2222222222' ||
            meli_code == '3333333333' ||
            meli_code == '4444444444' ||
            meli_code == '5555555555' ||
            meli_code == '6666666666' ||
            meli_code == '7777777777' ||
            meli_code == '8888888888' ||
            meli_code == '9999999999') {
            alert('کد ملي صحيح نمي باشد');
            objcode.focus();
            return false;
        }
        c = parseInt(meli_code.charAt(9));
        n = parseInt(meli_code.charAt(0)) * 10 +
              parseInt(meli_code.charAt(1)) * 9 +
              parseInt(meli_code.charAt(2)) * 8 +
              parseInt(meli_code.charAt(3)) * 7 +
              parseInt(meli_code.charAt(4)) * 6 +
              parseInt(meli_code.charAt(5)) * 5 +
              parseInt(meli_code.charAt(6)) * 4 +
              parseInt(meli_code.charAt(7)) * 3 +
              parseInt(meli_code.charAt(8)) * 2;
        r = n - parseInt(n / 11) * 11;
        if ((r == 0 && r == c) || (r == 1 && c == 1) || (r > 1 && c == 11 - r)) {
            return true;
        }
        else {
            alert('کد ملي صحيح نمي باشد');
            objcode.focus();
            return true;
        }
    }
    else {
        return true;
    }
}