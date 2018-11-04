function toMoneyFormat(input) {
    var cost = input.value.replace(/[^\d,.]/g, '');
    cost = cost.replace(/,/g, '');
    var num = parseFloat(cost).toFixed(2);
    num += '';
    var x = num.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    if (x1 == 'NaN') {
        x1 = 0.00;
    }
    input.value =(x1 + x2);
}

function onInputTextNumber(input) {
    input.value = input.value.replace(/[^0-9]/g, '');
    input.value = input.value.replace(/(\..*)\./g, '$1');
}


function toThousandSeparatorFormat(input) {
    if (input) {
        var cost = input.value.replace(/[^\d,]/g, '');
        cost = cost.replace(/,/g, '');
        var num = parseInt(cost);
        num += '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(num)) {
            num = num.replace(rgx, '$1' + ',' + '$2');
        }
        if (num == 'NaN') {
            num = 0;
        }
        input.value = num;
    }
}

function formatDate(date) {
  if (date) {
    date = new Date(parseInt(date.substr(6)));

    var dt = addLeadingZeros((date.getMonth() + 1), 2) + '/' + addLeadingZeros(date.getDate(), 2) + '/' + date.getFullYear();
    var time = date.toLocaleTimeString().toLowerCase().replace(/([\d]+\d]+)\d]+(\s\w+)/g, "$1$2");

    return (dt);
  }
  return "";
}

function formatDateWithTime(date) {
  if (date) {
    date = new Date(parseInt(date.substr(6)));

    var dt = addLeadingZeros((date.getMonth() + 1), 2) + '/' + addLeadingZeros(date.getDate(), 2) + '/' + date.getFullYear() + " " + addLeadingZeros((date.getHours()), 2) +":" +  addLeadingZeros((date.getMinutes()), 2) ;
    var time = date.toLocaleTimeString().toLowerCase().replace(/([\d]+\d]+)\d]+(\s\w+)/g, "$1$2");

    return (dt);
  }
  return "";
}

function addLeadingZeros(n, length) {
  var str = (n > 0 ? n : -n) + "";
  var zeros = "";
  for (var i = length - str.length; i > 0; i--)
    zeros += "0";
  zeros += str;
  return n >= 0 ? zeros : "-" + zeros;
}

