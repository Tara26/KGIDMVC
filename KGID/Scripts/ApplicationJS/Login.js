
$(document).ready(function () {
    DrawCaptcha($("#txtInputCaptchaDept"));
   // CheckLogin();
});
function CheckLogin() {
    $.ajax({
        url: '/Login/CheckValidLogin',
        data: JSON.stringify(),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            var _valid = result;
            if (_valid == 1) {
                window.location.href = "/Login/Dashboard";
            }
        }
    });
}
// Server Side Captcha Code Generation
function DrawCaptcha($control) {
    $.ajax({
        url: "/Login/GetCaptcha",
        dataType: 'json',
        success: function (d) {
            $control.val(d);
        }
    });
}

