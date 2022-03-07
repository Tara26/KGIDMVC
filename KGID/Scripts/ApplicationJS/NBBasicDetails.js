$(document).ready(function () {
    $('#txtEAddress').keypress(function (e) {
        var key = e.keyCode;
        if (key == 13)
            e.preventDefault();
    });

    if ($(".rd-btn-mstatus:checked").val() === "True") {
        $("#divMarriedOptions").show();
        $(".rd-btn-orphan:checked").prop("checked", false);
        $("#divOrphanOptions").hide();
        $(".rd-btn-mstatus").attr("disabled", true);
        $("#ddlDRType").removeAttr("disabled");
    }
    else {
        $(".rd-btn-govt:checked").prop("checked", false);
        $("#txtSpouseKGIDNumber").val("");
        $("#divSpouseKGID").hide();
        $("#divIDNumber").hide();
        $("#divMarriedOptions").hide();
        $("#divOrphanOptions").show();
        $(".rd-btn-mstatus").attr("disabled", false);
        $("#ddlDRType").attr("disabled",true);
    }

    var isMarried = $("#rbtnMarried").is(":checked");
    if (!isMarried && $("#father_name").val() != "") {
        $("#hdnOrphan").val("false");
        $("#rbtnNotOrphan").prop("checked", true)
        $(".rd-btn-orphan").attr("disabled", true);
    }
    else if (!isMarried && $("#father_name").val() == "") {
        $("#hdnOrphan").val("true");
        $("#rbtnOrphan").prop("checked", true)
        $(".rd-btn-orphan").attr("disabled", true);
    }
    if ($(".rd-btn-govt:checked").val() === "True") {
        $("#divIDNumber").show();
        $("#divSpouseKGID").show();
    }
    else {
        $("#divSpouseKGID").hide();
        $("#divIDNumber").hide();
    }
    if ($(".rd-btn-kgidpan:checked").val() === "True") {
        $("#divPANNumber").show();
        $("#divKGIDNumber").hide();
    }
    else {
        $("#divKGIDNumber").show();
        $("#divPANNumber").hide();
    }
    if ($("#ddlDRType").val() == 2) {
        $("#divCSpouse").show();
        $("#divdrSuppDoc").hide();
        $("input[name='eod_spouse_govt_emp']").removeAttr("disabled");
    } else if ($("#ddlDRType").val() == 1) {
        $("#divCSpouse").hide();
        $("#divdrSuppDoc").show();
        $("input[name='eod_spouse_govt_emp']").attr("disabled", true);
    } else {
        $("#divCSpouse").hide();
        $("#divdrSuppDoc").hide();
        $("input[name='eod_spouse_govt_emp']").removeAttr("disabled");
    }
    jQuery('#txtEPin').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
        $("#txtEPin").attr('maxlength', '6');
    });
    //if ($("#hdnfirstpolno").val() == null || $("#hdnfirstpolno").val() == "") {
    //    $("#ddlDRType option:contains('N/A')").prop("selected", true);
    //    $("#ddlDRType option:contains('N/A')").attr("disabled", true);
    //}
});
$('input[type=radio][name=eod_emp_orphan]').change(function () {
    if (this.value == "True") {
        $("#hdnOrphan").val(true);
    }
    else {
        $("#hdnOrphan").val(false);
    }
});
$(".rd-btn-mstatus").change(function () {
        if ($(this).val() === "True") {
            $("#divMarriedOptions").show();
            $(".rd-btn-orphan:checked").prop("checked", false);
            $("#divOrphanOptions").hide();
            $(".rd-btn-govt").removeAttr("disabled");
            $("#txtSpouseName").removeAttr("readonly");
            $("#txtSpouseNameKN").removeAttr("readonly");
            if ($("#txtSpouseName").val() != "" || $("#txtSpouseNameKN").val() != "") {
                $("#ddlDRType").removeAttr("disabled");
            }
            $.ajax({
                url: '/Employee/GetNomineeList',
                data: JSON.stringify({ AppID: $("#hdnAppID").val(), IsMarried: "Married" }),
                async: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    if (result == "0") {
                        $(".rd-btn-mstatus").prop("checked", false);
                        alertify.alert("Please remove member from nominee list before editing.").setHeader("Warning!!!");
                        return false;
                    }
                }, error: function (result) {
                    alertify.error("Could not edit family member");
                }
            });
        }
        else {
            $(".rd-btn-govt:checked").prop("checked", false);
            $("#txtSpouseKGIDNumber").val("");
            $("#divSpouseKGID").hide();
            $("#divMarriedOptions").hide();
            $("#divOrphanOptions").show();
            $("#divIDNumber").hide();
            $("#txtSpouseName").attr("readonly", "readonly").val("");
            $("#txtSpouseNameKN").attr("readonly", "readonly").val("");
            $("#ddlDRType").attr("disabled", true);
            $("#ddlDRType").val("");
            $("#divCSpouse").hide();
            $("#divSuppDoc").hide();
        }

    var isMarried = $("#rbtnMarried").is(":checked");
    if (!isMarried && $("#father_name").val() != "") {
        $("#hdnOrphan").val("false");
        $("#rbtnNotOrphan").prop("checked", true);
        $(".rd-btn-orphan").attr("disabled", true);
    }
    else if (!isMarried && $("#father_name").val() == ""){
        $("#hdnOrphan").val("true");
        $("#rbtnOrphan").prop("checked", true);
        $(".rd-btn-orphan").attr("disabled", true);
    }
});

$("#txtSpouseName,#txtSpouseNameKN").keyup(function () {
    if ($("#txtSpouseName").val() != "") {
        $("#ddlDRType").removeAttr("disabled");
    }
    if ($("#txtSpouseNameKN").val() != "") {
        $("#ddlDRType").removeAttr("disabled");
    }
})

$(".rd-btn-govt").change(function () {
    if ($(this).val() === "True") {
        $("#divSpouseKGID").show();
        $("#divIDNumber").show();
        $('#txtSpousePanNumber').val("");
        $('#txtSpouseKGIDNumber').val("");
        $('#errSpousePANNumberReq').attr("hidden",true);
    }
    else {
        $("#divSpouseKGID").hide();
        $("#divIDNumber").hide();
    }
});

$(".rd-btn-kgidpan").change(function () {
    if ($(this).val() === "True") {
        $("#divKGIDNumber").hide();
        $("#divPANNumber").show();
    }
    else {
        $("#divPANNumber").hide();
        $("#divKGIDNumber").show();
    }
});

function ValidateBasicDetails() {
    $('.err').attr('hidden', true);
    var isMarried = $("#rbtnMarried").is(":checked");
    var IsMarriedRadioSelected = $("input[name='eod_emp_married']:checked").length;
    var IsGovtEmpRadioSelected = $("input[name='eod_spouse_govt_emp']:checked").length;
    var IsOrphanSelected = $("input[name='eod_emp_orphan']:checked").length;
    var IsKGIDPANNum = $("input[name='isKGIDPAN']:checked").length;

    if (IsMarriedRadioSelected === 0) {
        $("#errMarritalStatusReq").removeAttr("hidden");
    }

    if (isMarried && IsGovtEmpRadioSelected === 0) {
        $("#errGovtEmpStatusReq").removeAttr("hidden");
    }

    if (isMarried && ($("#txtSpouseName").val() == "" && $("#txtSpouseNameKN").val() == "")) {
        $("#errSpouseName").removeAttr("hidden");
    }

    if ($("#rbtnGovtEmp").is(":checked") && $("#txtSpouseKGIDNumber").val() === "") {
        $("#errSpouseKGIDNumberReq").removeAttr("hidden");
    }

    if (!isMarried && IsOrphanSelected === 0) {
        $("#errOrphanStatusReq").removeAttr("hidden");
    }
    if ($("#rbtnGovtEmp").is(":checked")) {
        if ($("#rbtnPAN").is(":checked")) {
            if ($("#txtSpousePanNumber").val() == "") {
                $("#errSpousePANNumberReq").removeAttr("hidden");
            }
            var panVal = $('#txtSpousePanNumber').val();
            var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
            if (regpan.test(panVal)) {
                $("#errSpousePANNumberReq").attr("hidden");
            } else {
                $("#errSpousePANNumberReq").text("Please enter valid PAN Number.");
                $("#errSpousePANNumberReq").removeAttr("hidden");
            }
        } else {
            if ($("#txtSpouseKGIDNumber").val() == "") {
                $("#errSpouseKGIDNumberReq").removeAttr("hidden");
            }
        }
    }
    if ($("#ddlDRType").val() == 2) {
        if ($("#txtCS").val() == "") {
            $("#errCurrMarritalStatusReq").removeAttr("hidden");
        }
    }

    if ($("#ddlDRType").val() == 1) {
        if ($("#flDivSupportingDoc").get(0).files[0] == "" || $("#flDivSupportingDoc").get(0).files[0] == undefined) {
            if ($("#divdrDocUpload:visible").length === 0) {
                $("#errCurrMarritalStatusReq").text("Please upload supporting document");
                $("#errCurrMarritalStatusReq").removeAttr("hidden");
            }
        }
    }

    var msg = "";

    if ($('#txtEmployeeFullNameKN').val() == "") {

        msg += "Proposer Name Kannada, ";
    }

    if ($('#father_name').val() != "") {
        if ($('#txtFatherNameKN').val() == "") {
            msg += "Father Name (Kannada), "; 
        }
    }
   
    if ($('#txtSpouseName').val() != "") {
        if ($('#txtSpouseNameKN').val() == "") {
            msg += "Spouse Name (Kannada), ";     
        }
    }

    if (msg != "") {

        var lastChar = msg.slice(-1);
        if (lastChar == ',') {
            msg = msg.slice(0, -1);
        }
        alertify.alert("Please contact DDO to update the following feilds " + msg)
        return false;
    }
    else {
        return true;
    }
 
}

function ValidatePancardNumber(panVal) {
    var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
    if (regpan.test(panVal)) {
        $("#errSpousePANNumberReq").attr("hidden");
    } else {
        $("#errSpousePANNumberReq").removeAttr("hidden");
    }
}   

function MShandleClick(sts) {
    if (sts == 1) {
        $("#hdnMStatus").val(true);
    } else {
        $("#hdnMStatus").val(false);
    }
}
function ChangeCurrentMarriedSts() {
    if ($("#hdnmarriedstatus").val() == 1 || $("#hdnmarriedstatus").val() == 2) {
        if ($("#ddlDRType").val() == 0 || $("#ddlDRType").val() == "") {
            alertify.error("You cannot select this option");
            $("#ddlDRType").val($("#hdnmarriedstatus").val());
            return false;
        }
    }
    $("#hdnDRStatus").val($("#ddlDRType").val());
    if ($("#ddlDRType").val() == 2) {
        $.ajax({
            url: '/Employee/GetNomineeList',
            data: JSON.stringify({ AppID: $("#hdnAppID").val(), IsMarried: "Remarried" }),
            async: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                debugger;
                if (result == "0") {
                    $("#ddlDRType").val("");
                    alertify.alert("Please remove member from nominee list before editing.").setHeader("Warning!!!");
                    $("#txtCS").val("");
                    $("#divCSpouse").hide();
                    $("#divdrSuppDoc").hide();
                    $("#divdrDocUpload").hide();
                    return false;
                }
                else {
                    $("#txtCS").val("");
                    $("#divCSpouse").show();
                    $("#divdrSuppDoc").hide();
                    $("#divdrDocUpload").hide();
                    $("input[name='eod_spouse_govt_emp']").removeAttr("disabled");
                }
            }, error: function (result) {
                alertify.error("Could not edit family member");
            }
        });
    }
    else if ($("#ddlDRType").val() == 1) {
        $.ajax({
            url: '/Employee/GetNomineeList',
            data: JSON.stringify({ AppID: $("#hdnAppID").val(), IsMarried: "Divorced" }),
            async: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                debugger;
                if (result == "0") {
                    $("#ddlDRType").val("");
                    alertify.alert("Please remove member from nominee list before editing.").setHeader("Warning!!!");
                    $("#txtCS").val("");
                    $("#divCSpouse").hide();
                    $("#divdrSuppDoc").hide();
                    $("#divdrDocUpload").hide();
                    return false;
                }
                else {
                    $("#divCSpouse").hide();
                    $("#divdrSuppDoc").show();
                    //$("#divdrDocUpload").show();
                    $("input[name='eod_spouse_govt_emp']").attr("disabled", true);
                }
            }, error: function (result) {
                alertify.error("Could not edit family member");
            }
        });
    }
    else {
        $("#txtCS").val("");
        $("#divCSpouse").hide();
        $("#divdrSuppDoc").hide();
        $("#divdrDocUpload").hide();
        $("input[name='eod_spouse_govt_emp']").removeAttr("disabled");
    }
}

function DocFileChange(id, errLbl) {
    if ($("#" + id).get(0).files[0] != undefined) {
        var fileType = $("#" + id).get(0).files[0].type;
        if (fileType == 'application/pdf') {
            $("#" + errLbl).attr("hidden", true);
        }
        else {
            alert('Wrong type!! Upload Pdf file only..')
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

$('.ClAlphaNum').keyup(function () {
    var input_val = $(this).val();
    var inputRGEX = /^[a-zA-Z0-9]*$/;
    var inputResult = inputRGEX.test(input_val);
    if (!(inputResult)) {
        this.value = this.value.replace(/[^a-z0-9\s]/gi, '');
    }
});
$('.ClAlpha').keyup(function () {
    var input_val = $(this).val();
    var inputRGEX = /^[a-zA-Z]*$/;
    var inputResult = inputRGEX.test(input_val);
    if (!(inputResult)) {
        this.value = this.value.replace(/[^a-zA-Z\s]/gi, '');
    }
});
$('.ClNum').keyup(function () {
    var input_val = $(this).val();
    var inputRGEX = /^[0-9]*$/;
    var inputResult = inputRGEX.test(input_val);
    if (!(inputResult)) {
        this.value = this.value.replace(/[^0-9\s]/gi, '');
    }

});
$('.tocaps').keyup(function (e) {


    var input = $(this);
    var start = input[0].selectionStart;
    $(this).val(function (_, val) {
        return val.toUpperCase();
    });
    input[0].selectionStart = input[0].selectionEnd = start;

});

$(".preventSpace").keypress(function (e) {
    if (e.which === 32)
        return false;
});
