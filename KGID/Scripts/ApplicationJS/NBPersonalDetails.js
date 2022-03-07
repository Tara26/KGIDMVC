var initialProposalDiv;
var today;
$(document).ready(function () {
    today = new Date();
    //$('#txtRPeriodDate').datetimepicker({
    //    timepicker: false,
    //    format: 'd-m-Y',
    //    autoclose: true,
    //    maxDate: 0,
    //    yearStart: today.getFullYear() - 80,
    //    yearEnd: today.getFullYear(),
    //    scrollMonth: false,
    //    scrollInput: false,
    //    closeOnDateSelect: true
    //});
    $('#txtlastmensuration').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        maxDate: 0,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        scrollMonth: false,
        scrollInput: false,
        closeOnDateSelect: true
    });
    //$('#txtdateoflastdelivery').datetimepicker({
    //    timepicker: false,
    //    format: 'd-m-Y',
    //    autoclose: true,
    //    maxDate: 0,
    //    yearStart: today.getFullYear() - 80,
    //    yearEnd: today.getFullYear(),
    //    scrollMonth: false,
    //    scrollInput: false,
    //    closeOnDateSelect: true
    //});
    //if ($('[name="ephd_menstruating"]:checked').val() == "False") {
    //    $("#txtlastmensuration").removeAttr("readonly");
    //}
    //if ($('[name="ephd_is_pregnant"]:checked').val() == "True") {
    //    $("#txtRPeriodDate").removeAttr("readonly");
    //}
});

//$('input[type=radio][name=ephd_health_condition]').change(function () {
//    if (this.value == "True") {
//        $('label.err').attr('hidden', true);
//    }
//});
//function PStatusChange(isChange) {
//    var $rdButton = $('[name="ProposalStatus"]:checked');
//    var $divId = $("#" + $rdButton.data("target-div"));
//    var $control = $("#" + $rdButton.data("target-control"));
//    if ($rdButton.val() == "True") {
//        $(".div-txt-ar-proposal").hide();
//        $(".hdnProSta").val(false);
//        $control.val(true);
//        $divId.show();
//        $("#txtProposalMadeDoc").removeAttr("disabled");
//        //$("#txtProposalMadeDoc").addClass("txtAr-doc-personal-req");
//        if (isChange) {
//            $("#prevPropUpld").hide();
//            $("#fupldProposal").hide();
//        }

//        if (initialProposalDiv === $rdButton.data("target-div")) {
//            $("#prevPropUpld").show();
//            $("#fupldProposal").show();
//        }
//    }
//}

//function ProposalChange() {
//    if ($('[name="IsProposalMade"]:checked').val() == "True") {
//        $(".div-choices").show();
//        $("#txtProposalMadeDoc").removeAttr("disabled");
//        //$("#txtProposalMadeDoc").addClass("txtAr-doc-personal-req");
//        $("#txtOrganizationOrPolicyNumber").addClass("txtar-personal-req");
//        $("#txtPolicyOrProposalNumber").addClass("txtar-personal-req");
//    }
//    else {
//        $(".div-choices").hide();
//        $("#errdivDummyTxtAr").show();
//        $("#errtxtProposalAccepted").show();
//        $("#errtxtProposalPostponed").show();
//        $("#errtxtProposalDeclined").show();
//        $("#errProposalOtp").show();
      
//    }
//}

//function ToggleMarriageDetails() {
//    if ($('[name="ephd_is_married"]:checked').val() == "True") {
//        $(".marriage-details").show();
//        $("#txtMarriedTenureDetailsDIseases").addClass("txtar-personal-req");
//        $("#txtOccupationAndAddress").addClass("txtar-personal-req");
//        $("#txtFinsuredOfficialBrch").addClass("txtar-personal-req");
//        $("#txtFhusbandLIC").addClass("txtar-personal-req");
//        $("#txtlastmensuration").addClass("txtar-personal-req");
//        $("#txtFnopregnancies").addClass("txtar-personal-req");
//        $("#txtFgonefulltime").addClass("txtar-personal-req");
//        $("#txtdateoflastdelivery").addClass("txtar-personal-req");
//        $("#txtFmiscarriages").addClass("txtar-personal-req");
//    }
//    else {
//        $(".marriage-details").hide();
//        $("#txtMarriedTenureDetailsDIseases").removeClass("txtar-personal-req");
//        $("#txtOccupationAndAddress").removeClass("txtar-personal-req");
//        $("#txtFinsuredOfficialBrch").removeClass("txtar-personal-req");
//        $("#txtFhusbandLIC").removeClass("txtar-personal-req");
//        $("#txtlastmensuration").removeClass("txtar-personal-req");
//        $("#txtFnopregnancies").removeClass("txtar-personal-req");
//        $("#txtFgonefulltime").removeClass("txtar-personal-req");
//        $("#txtdateoflastdelivery").removeClass("txtar-personal-req");
//        $("#txtFmiscarriages").removeClass("txtar-personal-req");
//    }
//}

//function MDKeypress(textId) {
//    debugger;
//    if ($("#" + textId).val("") != "") {
//        var isValid = false;
//        var regex = /^[0-9.\s]*$/;
//        isValid = regex.test($("#" + textId).val());
//        if (!isValid) {
//            $("#" + textId).val("");
//        }
//        else {
//            var dob = $('#txtBasicDateOfBirth').val().split("-");
//            var dobdate = new Date(dob[2], dob[1] - 1, dob[0]);
//            var today = new Date();
//            var age = Math.floor((today - dobdate) / (365.25 * 24 * 60 * 60 * 1000));

//            if ((parseFloat(age) - 10) < parseFloat($('#txtMarriedTenureDetails').val())) {
//                $('#errMarriedTenure').show();
//                return false;
//            } else {
//                $('#errMarriedTenure').hide();
//            }
//        }
           
//    }
//    else {

//    }
        
    
//}
function MDKeypress(textId,errorid) {
    debugger;
    if ($("#" + textId).val() != "0") {
        var isValid = false;
        var regex = /^[0-9.\s]*$/;
        isValid = regex.test($("#" + textId).val());
        if (!isValid)
            $("#" + textId).val("");
    }
    else {
        $('#' + errorid).removeAttr("hidden");
        $('#' + errorid).text("Value cannot be 0.")
        return false;

    }
    
}

function PDcheckboxchange(chkId, txtId, prevDocID) {
    if ($('#' + chkId).val() == "True") {
        $('#' + txtId).removeAttr("readonly");
        $('#' + txtId + 'Doc').removeAttr("disabled");
        $('#' + txtId).addClass("txtar-personal-req");
        //$('#' + txtId + 'Doc').addClass("txtAr-doc-personal-req");
        $('#' + txtId + 'Doc').val("");
    } else {
        $('#' + txtId).val("");
        $('#' + txtId + 'Doc').val("")
        $('#' + txtId).attr("readonly", "true");
        $('#' + txtId + 'Doc').attr("disabled", true);
        $('#' + txtId).removeClass("txtar-personal-req");
        //$('#' + txtId + 'Doc').removeClass("txtAr-doc-personal-req");
        $('#err' + txtId + 'Req').hide();
        $('#err' + txtId + 'Doc').hide();
        $("#" + prevDocID).hide();
    }
}

//function PGcheckboxchange(chkId, txtId) {
//    if ($('#' + chkId).val() == "True") {
//        $('#' + txtId).removeAttr("readonly");
//    } else {
//        $('#' + txtId).attr("readonly", "true");
//    }
//}

//function MTcheckboxchange(chkId, txtId) {
//    if ($('#' + chkId).val() == "True") {
//        $('#' + txtId).attr("readonly", "true");
//    } else {
//        $('#' + txtId).removeAttr("readonly");
//    }
//}

var isMarried = $(".rd-btn-mstatus:checked").val();
var isOrphan = $(".rd-btn-orphan:checked").val();
var isGovtEmp = $(".rd-btn-govt:checked").val();
$(document).ready(function () {
    $(".NUM").keyup(function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
    //initialProposalDiv = $('[name="ProposalStatus"]:checked').data("target-div");
    $('#divPersonal input:radio').each(function () {
        var name = $(this).attr('name');
        if ($(".cs" + name + "Y").prop("checked") == false && $(".cs" + name + "N").prop("checked") == true) {
            $('#txt' + name).attr("readonly", true);
            $('#txt' + name + 'Doc').attr("disabled", true);
            $('#txt' + name).removeClass("txtar-personal-req");
        } else {
            $('#txt' + name).removeAttr("readonly");
            $('#txt' + name + 'Doc').removeAttr("disabled");
            $('#txt' + name).addClass("txtar-personal-req");
            //$('.fu' + name).addClass("txtAr-doc-personal-req");
        }
    });
    //ProposalChange();
    //PStatusChange(false);
    if ($("#txtGender").val().toUpperCase() === "FEMALE") {
        $("#divFemaleDetails").show();
    }
    else {
        $("#divFemaleDetails").hide();
    }
    //if (isMarried == "True" && isGovtEmp == "True") {
    //    $(".marriage-details").show();
    //    $(".divMargYrs").show();
    //    $(".divOccuAddr").show();
    //    $("#txtMarriedTenureDetailsDIseases").addClass("txtar-personal-req");
    //    $("#txtOccupationAndAddress").addClass("txtar-personal-req");

    //    $("#rbtnMarriedYes").prop("checked", true);
    //    $("#rbtnMarriedNo").prop("checked", false);
    //    $(".csIsMarriedY:checked").val("True");
    //    $("#rbtnMarriedNo").attr("disabled", true);

    //    $("#rbtnHGYes").prop("checked", true);
    //    $("#rbtnHGNo").prop("checked", false);
    //    $(".csephd_husbandGovEmpY:checked").val("True");
    //    $("#rbtnHGNo").attr("disabled", true);

      
       
    //    //$("#txtFinsuredOfficialBrch").addClass("txtar-personal-req");
    //    //$("#txtFhusbandLIC").addClass("txtar-personal-req");
    //    //$("#txtlastmensuration").addClass("txtar-personal-req");
    //    //$("#txtFnopregnancies").addClass("txtar-personal-req");
    //    //$("#txtFgonefulltime").addClass("txtar-personal-req");
    //    //$("#txtdateoflastdelivery").addClass("txtar-personal-req");
    //    //$("#txtFmiscarriages").addClass("txtar-personal-req");
    //}
    //else if (isMarried == "True" && isGovtEmp == "False") {
    //    $(".marriage-details").show();
    //    $(".divMargYrs").show();
    //    $(".divOccuAddr").hide();
    //    $("#txtMarriedTenureDetailsDIseases").addClass("txtar-personal-req");
    //    $("#txtOccupationAndAddress").addClass("txtar-personal-req");
    //    $("#rbtnMarriedYes").prop("checked", true); $("#rbtnMarriedNo").prop("checked", false)
    //    $(".csIsMarriedY:checked").val("True");
    //    $("#rbtnMarriedNo").attr("disabled", true);  
    //    //$("#txtFinsuredOfficialBrch").addClass("txtar-personal-req");
    //    //$("#txtFhusbandLIC").addClass("txtar-personal-req");
    //    //$("#txtlastmensuration").addClass("txtar-personal-req");
    //    //$("#txtFnopregnancies").addClass("txtar-personal-req");
    //    //$("#txtFgonefulltime").addClass("txtar-personal-req");
    //    //$("#txtdateoflastdelivery").addClass("txtar-personal-req");
    //    //$("#txtFmiscarriages").addClass("txtar-personal-req");

    //    $("#rbtnHGYes").prop("checked", false);
    //    $("#rbtnHGNo").prop("checked", true);
    //    $(".csephd_husbandGovEmpY:checked").val("False");
    //    $("#rbtnHGNo").attr("disabled", true);
    //    $("#rbtnHGYes").attr("disabled", true);

    //}
    //else if ($('[name="ephd_is_married"]:checked').val() == "True" && isGovtEmp == "False") {
    //    $(".marriage-details").show();
    //    $(".divMargYrs").show();
    //    $(".divOccuAddr").hide();    
    //    $("#txtMarriedTenureDetailsDIseases").addClass("txtar-personal-req");
    //    $("#rbtnMarriedYes").prop("checked", true); $("#rbtnMarriedNo").prop("checked", false)
    //    $(".csIsMarriedY:checked").val("True");
    //    $("#rbtnMarriedNo").attr("disabled", true); 
    //    //$("#txtFinsuredOfficialBrch").addClass("txtar-personal-req");
    //    //$("#txtFhusbandLIC").addClass("txtar-personal-req");
    //    //$("#txtlastmensuration").addClass("txtar-personal-req");
    //    //$("#txtFnopregnancies").addClass("txtar-personal-req");
    //    //$("#txtFgonefulltime").addClass("txtar-personal-req");
    //    //$("#txtdateoflastdelivery").addClass("txtar-personal-req");
    //    //$("#txtFmiscarriages").addClass("txtar-personal-req");
    //}
    //else {
    //    $(".marriage-details").hide();
    //    $("#txtMarriedTenureDetailsDIseases").removeClass("txtar-personal-req");
    //    $("#txtOccupationAndAddress").removeClass("txtar-personal-req");
    //    $("#rbtnMarriedYes").prop("checked", false);
    //    $("#rbtnMarriedNo").prop("checked", true)
    //    $("#rbtnMarriedYes").attr("disabled", true);   
    //    $(".csIsMarriedY:checked").val("False");

    //    //$("#txtFinsuredOfficialBrch").removeClass("txtar-personal-req");
    //    //$("#txtFhusbandLIC").removeClass("txtar-personal-req");
    //    //$("#txtlastmensuration").removeClass("txtar-personal-req");
    //    //$("#txtFnopregnancies").removeClass("txtar-personal-req");
    //    //$("#txtFgonefulltime").removeClass("txtar-personal-req");
    //    //$("#txtdateoflastdelivery").removeClass("txtar-personal-req");
    //    //$("#txtFmiscarriages").removeClass("txtar-personal-req");
    //}
});

function validatePersonalFields() {
    debugger;
    var chkPersonalHealth = false;
    $('.err').attr('hidden', true);

    var txtHeight = $("#txtHeight").val();
    var txtWeight = $("#txtWeight").val();

    if ($("#rbtnBad").is(":checked")) {
        $('#divPerHealthDetails input:radio:checked').each(function () {
            if (this.value == "True" && !chkPersonalHealth)
                chkPersonalHealth = true;
        });
    }
    else {
        chkPersonalHealth = true;
    }
    if (!$("#rbtnNo").is(":checked") && !$("#rbtnYes").is(":checked")) {
        $("#errIsPregnant").removeAttr("hidden");
    }
    //if (!$("#rbtnHGYes").is(":checked") && !$("#rbtnHGNo").is(":checked")) {
    //    $("#errIsHG").removeAttr("hidden");
    //}
    //if (!$("#rbtnMYes").is(":checked") && !$("#rbtnMNo").is(":checked")) {
    //    $("#errMens").removeAttr("hidden");
    //}
    //if (!$("#rbtnHLICYes").is(":checked") && !$("#rbtnHLICNo").is(":checked")) {
    //    $("#errHLIC").removeAttr("hidden");
    //}
    //if (!$("#rbtnIOBYes").is(":checked") && !$("#rbtnIOBNo").is(":checked")) {
    //    $("#errIOB").removeAttr("hidden");
    //}

    if (!chkPersonalHealth) {
        $("#errPersonal").removeAttr("hidden");
    }
    if (txtHeight.trim() === '' || (txtHeight.trim() == "0" || txtHeight.trim() == "00" || txtHeight.trim() == "000" )) {
        $("#errHeightReq").removeAttr("hidden");
    }

    if (txtWeight.trim() === '' || (txtWeight.trim() == "0" || txtWeight.trim() == "00" || txtWeight.trim() == "000")) {
        $("#errWeightReq").removeAttr("hidden");
    }
    //if ($('[name="ephd_menstruating"]:checked').val() == "False") {
    //    if ($("#txtlastmensuration").val() == "") {
    //        $("#errLastmensuration").removeAttr("hidden");
    //    } else {
    //        $("#errLastmensuration").attr("hidden",true);
    //    }
    //}
    //if ($('[name="ephd_is_married"]:checked').val() == "True") {
    //    if ($("#txtMarriedTenureDetails").val() == "") {
    //        $("#errMarriedTenureDetails").removeAttr("hidden");
    //    }
    //    else if ($("#txtMarriedTenureDetails").val() != "") {
    //        debugger;
    //        var dob = $('#txtBasicDateOfBirth').val().split("-");
    //        var dobdate = new Date(dob[2], dob[1] - 1, dob[0]);
    //        var today = new Date();
    //        var age = Math.floor((today - dobdate) / (365.25 * 24 * 60 * 60 * 1000));

    //        if ((parseFloat(age) - 10) < parseFloat($('#txtMarriedTenureDetails').val())) {
    //            $('#errMarriedTenure').show();
    //            $('#errMarriedTenure').removeAttr("hidden");
    //            return false;
    //        } else {
    //            $('#errMarriedTenure').hide();
    //            $('#errMarriedTenure').attr("hidden",true);
    //        }
    //    }
    //}
    //if ($('[name="ephd_is_pregnant"]:checked').val() == "True") {
    //    if ($("#txtRPeriodDate").val() == "") {
    //        $("#errDOP").removeAttr("hidden");
    //    }
    //    if ($("#txtFnopregnancies").val() == "0") {
    //        $("#errNP").removeAttr("hidden");
    //    }
    //    if ($("#txtFgonefulltime").val() == "") {
    //        $("#errGFT").removeAttr("hidden");
    //    }
    //    if ($("#txtdateoflastdelivery").val() == "") {
    //        $("#errDOLD").removeAttr("hidden");
    //    }
    //    if ($("#txtFmiscarriages").val() == "") {
    //        $("#errmiscarriages").removeAttr("hidden");
    //    }
    //}
    //if ($('[name="IsinsuredOfficialBrch"]:checked').val() == "false") {
    //    $('#txtFinsuredOfficialBrch').css("readonly", "true");
    //}
    $(".txtar-personal-req").each(function () {
        if ($(this).val() === '') {
            $(this).next("label.err").removeAttr("hidden");
            $(this).next("label.err").css("display", "block");
        }
    });

    if ($("#txtlastmensuration").val() == "") {
        $("#errLastmensuration").removeAttr("hidden");
    } else {
        $("#errLastmensuration").attr("hidden", true);
    }

    //$(".txtAr-doc-personal-req").each(function () {
    //    if ($(this).val() === '' && $(this).prev(".doc-uploaded:visible").length === 0) {
    //        $(this).next("label.err").removeAttr("hidden");
    //    }
    //});

    //if ($('[name="IsProposalMade"]:checked').val() == "True" && $(".proposal-txt-ar:visible").val() === '') {
    //    $(".proposal-txt-ar").next("label.err").removeAttr("hidden");
    //}

    //if ($('[name="IsProposalMade"]:checked').val() == "True" && $('[name="ProposalStatus"]:checked').length === 0) {
    //    $("#errProposalOtp").removeAttr("hidden");
    //}
    if ($('[name="ephd_health_condition"]:checked').length === 0) {
        $("#errHealthConditionReq").removeAttr("hidden");
    }
}

function PerDocFileChange(id, errLbl) {
    if ($("#" + id).get(0).files[0] != undefined) {
        var fileType = $("#" + id).get(0).files[0].type;
        if (fileType == 'application/pdf') {
                $("#" + errLbl).attr("hidden", true);
            }
            else {
                $("#" + errLbl).removeAttr("hidden");
                $("#" + errLbl).text("Please upload document in pdf format");
                $("#" + id).val("");
                return false;
            }
        const fsize = $("#" + id).get(0).files[0].size;
        const maxAllowedSize = 5 * 1024 * 1024;
        // The size of the file. 
        if (fsize > maxAllowedSize) {
            $("#" + errLbl).removeAttr("hidden");
            $("#" + errLbl).text("File too Big, please select a file less than 5 MB");
            $("#" + id).val("");
        }
    }
}

//function ToggleHusbandGovDetails() {
//    if ($('[name="ephd_husbandGovEmp"]:checked').val() == "True") {
        
//        $("#txtMarriedTenureDetailsDIseases").addClass("txtar-personal-req");
//        $("#txtOccupationAndAddress").addClass("txtar-personal-req");
//    }
//    else {
//        $(".marriage-details").hide();
//        $("#txtMarriedTenureDetailsDIseases").removeClass("txtar-personal-req");
//        $("#txtOccupationAndAddress").removeClass("txtar-personal-req");
//    }
//}