$(document).ready(function () {
    if (medicalRequiredStatus) {
        $(".field-required").addClass("required");
    }
    $(".Num").keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
        $(".Num").attr('maxlength', '3');
    });
});

function ValidatePhysicalFields() {
    debugger;
    $('.err').attr('hidden', true);


    var txtHeight = $("#txtPHeight").val();
    var txtWeight = $("#txtPWEIGHT").val();
    var txtBreathPM = $("#txtBreathRatePerMin").val();
    var txtPulsePM = $("#txtPulseRatePerMin").val();
    var txtBP = $("#txtPBP").val();
    var txtLow = $("#txtLowDystolic").val();
    var txtHigh = $("#txtHighSystolic").val();

    if (txtHeight.trim() === '') {
        $("#errPHeightReq").removeAttr("hidden");
        return false;
    }
    else if (txtHeight.trim() === '0' || txtHeight.trim() === '00' || txtHeight.trim() === '000') {
        $("#errPHHeightReq").removeAttr("hidden");
        $("#errPHHeightReq").text("Value cannot be 0");
        return false;
    }

    if (txtWeight.trim() === '') {
        $("#errPWeightReq").removeAttr("hidden");
        return false;
    }
    else if (txtWeight.trim() === '0' || txtWeight.trim() === '00' || txtWeight.trim() === '000') {
        $("#errPHWeightReq").removeAttr("hidden");
        $("#errPHWeightReq").text("Value cannot be 0");
        return false;
    }

    if (txtPulsePM.trim() === '') {
        $("#errPulseRatePMReq").removeAttr("hidden");
        return false;
    }
    else if (txtPulsePM.trim() === '0' || txtPulsePM.trim() === '00' || txtPulsePM.trim() === '000') {
        $("#errPulseRatePHMReq").removeAttr("hidden");
        $("#errPulseRatePHMReq").text("Value cannot be 0");
        return false;
    }

    if (txtBreathPM.trim() === '') {
        $("#errBreathRatePMReq").removeAttr("hidden");
        return false;
    }
    else if (txtBreathPM.trim() === '0' || txtBreathPM.trim() === '00' || txtBreathPM.trim() === '000') {
        $("#errBreathRatePHMReq").removeAttr("hidden");
        $("#errBreathRatePHMReq").text("Value cannot be 0");
        return false;
    }

    if (txtBP.trim() === '') {
        $("#errBPReq").removeAttr("hidden");
        return false;
    }
    else if (txtBP.trim() === '0' || txtBP.trim() === '00' || txtBP.trim() === '000') {
        $("#errBPHReq").removeAttr("hidden");
        $("#errBPHReq").text("Value cannot be 0");
        return false;
    }

    if (txtLow.trim() === '') {
        $("#errfDystolicReq").removeAttr("hidden");
        return false;
    }
    else if (txtLow.trim() === '0' || txtLow.trim() === '00' || txtLow.trim() === '000') {
        $("#errfDystolicHReq").removeAttr("hidden");
        $("#errfDystolicHReq").text("Value cannot be 0");
        return false;
    }

    if (txtHigh.trim() === '') {
        $("#errSystolicReq").removeAttr("hidden");
        return false;
    }
    else if (txtHigh.trim() === '0' || txtHigh.trim() === '00' || txtHigh.trim() === '000') {
        $("#errSystolicReq").removeAttr("hidden");
        $("#errSystolicReq").text("Value cannot be 0");
        return false;
    }
}