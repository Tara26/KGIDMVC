

$(document).ready(function () {


    $(".preventSpace").keypress(function (e) {
        if (e.which === 32)
            return false;
    });
    $(".numbersonlywithDecimal").keypress(function (e) {

        var inputValue = e.charCode;
        if (e.which != 8 && e.which != 0 && (e.which <= 45 || e.which > 57)) {
            return false;
        }
    });

    //called when key is pressed in textbox
    $(".numbersonly").keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
  

    $(".txtOnly").keypress(function (event) {

        var inputValue = event.charCode;
        if (!(inputValue >= 65 && inputValue <= 122) && (inputValue != 32 && inputValue != 0)) {
            event.preventDefault();
        }
        if ((inputValue == 94 || inputValue == 95 || inputValue == 91 || inputValue == 93 || inputValue == 92 || inputValue == 96)) {
            event.preventDefault();
        }
    });
    $(".txtOnlyWithDot").keypress(function (event) {
        var inputValue = event.charCode;

        if (!(inputValue >= 65 && inputValue <= 122) && (inputValue != 32 && inputValue != 0) && (inputValue != 46)) {
            event.preventDefault();
        }

        if ((inputValue == 94 || inputValue == 95 || inputValue == 91 || inputValue == 93 || inputValue == 92 || inputValue == 96)) {
            event.preventDefault();
        }
    });

    $(".txtOnly").bind("cut copy paste", function (event) {
        var inputValue = event.charCode;
        if (!(inputValue >= 65 && inputValue <= 120) && (inputValue != 32 && inputValue != 0)) {
            event.preventDefault();
        }
    });


    $.fn.regexMask = function (mask) {
        $(this).keypress(function (event) {
            if (!event.charCode) return true;
            var part1 = this.value.substring(0, this.selectionStart);
            var part2 = this.value.substring(this.selectionEnd, this.value.length);
            if (!mask.test(part1 + String.fromCharCode(event.charCode) + part2))
                return false;
        });
    };


    var mask = new RegExp('^[A-Za-z0-9 ]*$')
    $(".alphanumericOnly").regexMask(mask)



    check = function (e, value) {
        if (!e.target.validity.valid) {
            e.target.value = value.substring(0, value.length - 1);
            return false;
        }
        var idx = value.indexOf('.');
        if (idx >= 0) {
            if (value.length - idx > 3) {
                e.target.value = value.substring(0, value.length - 1);
                return false;
            }
        }
        return true;
    }




});