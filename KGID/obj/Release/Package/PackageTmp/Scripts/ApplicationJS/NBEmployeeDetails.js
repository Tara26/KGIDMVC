$(document).ready(function () {
    var today = new Date();
    $('#txtDateOfBirth').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        maxDate: 0,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        closeOnDateSelect: true,
        scrollMonth: false,
        scrollInput: false,
    });
    $('#txtDateOfAppointment').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        maxDate: 0,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        closeOnDateSelect: true,
        scrollMonth: false,
        scrollInput: false,
    });
    $('#txtewd_date_of_joining_post').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        maxDate: 0,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        closeOnDateSelect: true,
        scrollMonth: false,
        scrollInput: false,
    });
});
$(function () {
    //$('.ClAlphaNum').keyup(function () {
    //    var input_val = $(this).val();
    //    var inputRGEX = /^[a-zA-Z0-9]*$/;
    //    var inputResult = inputRGEX.test(input_val);
    //    if (!(inputResult)) {
    //        this.value = this.value.replace(/[^a-z0-9\s]/gi, '');
    //    }
    //});
    //$('#txtMobileNumber').change(function () {
    //    var input_val = $(this).val();
    //    var inputRGEX = /^(\d{10})$/;
    //    var inputResult = inputRGEX.test(input_val);
    //    if (!(inputResult)) {
    //        $('#errMNumber').show();
    //        return false;
    //    } else {
    //        $('#errMNumber').hide();
    //    }
    //});
    //$('#txtEmailId').change(function () {
    //    var input_val = $(this).val();
    //    var inputRGEX = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/;
    //    var inputResult = inputRGEX.test(input_val);
    //    if (!(inputResult)) {
    //        $('#errEMail').show();
    //        return false;
    //    } else {
    //        $('#errEMail').hide();
    //    }

    //});
    //$('#txtPANNumber').change(function () {
    //    var input_val = $(this).val();
    //    var inputRGEX = /[A-Z]{5}\d{4}[A-Z]{1}/;
    //    var inputResult = inputRGEX.test(input_val);
    //    if (!(inputResult)) {
    //        $('#errPNum').show();
    //        return false;
    //    } else {
    //        $('#errPNum').hide();
    //    }
    //});
});
$(".disable-keyboard").on("keypress", function (e) {
    return false;
});