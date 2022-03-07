$(document).ready(function () {
    var _knLan = $('.knlan');
    var _EnLan = $('.Enlan');
    if (localStorage.length == 2) {
        if (localStorage.ChangeLang == '0') {
            _EnLan.hide();
            _knLan.show();
            $('#changeLan').val('A');
        }
        else {
            _knLan.hide();
            _EnLan.show();
            $('#changeLan').val('ಕ');
        }
    } else {
        _knLan.hide();
        _EnLan.show();
        $('#changeLan').val('ಕ');
    }



    //var _knLan = $('.knlan');
    //var _EnLan = $('.Enlan');
    //if (sessionStorage.getItem('Lang') == '0') {
    //    _EnLan.hide();
    //    _knLan.show();
    //    $('#changeLan').val('A');

    //    //$('.bkan').attr("style", "display:block;");
    //    //$('.bEng').attr("style", "display:none;");
    //}
    //else {
    //    _knLan.hide();
    //    _EnLan.show();
    //    $('#changeLan').val('ಕ');
    //    //$('.bkan').attr("style", "display:none;");
    //    //$('.bEng').attr("style", "display:block;");
    //}

    $('#changeLan').click(function () {
        //_knLan.toggle();
        //_EnLan.toggle();
        //sessionStorage.getItem('Lang');

        //if ($('.knlan').is(":visible")) {
        //    $('#changeLan').val('A');
        //    sessionStorage.setItem('Lang', '0');

        //    //$('.bkan').attr("style", "display:block;");
        //    //$('.bEng').attr("style", "display:none;");
        //}
        //else {
        //    $('#changeLan').val('ಕ');
        //    //$('.bkan').attr("style", "display:none;");
        //    //$('.bEng').attr("style", "display:block;");
        //    sessionStorage.setItem('Lang', '1');
        //}
        //location.reload();

        _knLan.toggle();
        _EnLan.toggle();

        if ($('.knlan').is(":visible")) {
            localStorage['ChangeLang'] = 0;
            $('#changeLan').val('A');
        }
        else {
            localStorage['ChangeLang'] = 1;
            $('#changeLan').val('ಕ');
        }
        //location.reload();
    });
});