$(document).ready(function () {
    var amt = 0;
    $("#tblExistingLoanDetails").DataTable();
    $("#rbtnNIsGovtEmp").prop("checked", true);
    $("#rbtnYIsGovtEmp").prop("checked", false);
    $("#divspKGID").hide();
    $("#divSpouseAuthentication").hide();
    $("#btn_submit").attr("disabled", true);
})

function loanFuntionCheckbox(id) {
    var $row = $("#tblExistingLoanDetails tr[data-row-number=" + id + "]");
    var chkrow = $row.find("TD").eq(1)[0].children[0].checked;
    if (chkrow) {
        if ($("#tblExistingLoanDetails tr[data-row-number=" + id + "] td.clsLoanAmt")[0].innerText != "") {
            alertify.alert("Cannot apply loan for this policy").setHeader("Warning!!!");
            $row.find("TD").eq(1)[0].children[0].checked = false;
            return;
        }
        $("#txtLoan_" + id).removeAttr("disabled");
    }
    else {
        $("#txtLoan_" + id).attr("disabled", true);
        $("#txtLoan_" + id).val("");
        $("#txtTotLoanAmt").val("");
        $("#txtNetAmount").val("");
        $("#ddlPrincipleInstallments").val("");
        $("#ddlInterestInstallments").val("");
        $("#PrincipleAmountInstallment").val("");
        $("#InterestAmountInstallment").val("");        
    }
    if ($("#tblExistingLoanDetails tbody tr input[type = checkbox]:checked").length > 0)
        $("#btn_submit").removeAttr("disabled");
    else
        $("#btn_submit").attr("disabled", true);
}

function LoanAmtChange(id, rowid) {
    debugger;
    var $row = $("#tblExistingLoanDetails tr[data-row-number=" + rowid + "]");
    var enteredamt = parseFloat($("#" + id).val());
    var eligibleamt = parseFloat($row.find("TD").eq(6).html());
    if (enteredamt > eligibleamt) {
        alertify.error("Entered amount is greater than eligible loan amount for this policy " + rowid);
        $("#" + id).val("");
    }
    else if (enteredamt < 100) {
        alertify.error("Loan amount should be greater than 100 " + rowid);
        $("#" + id).val("");
        $("#txtTotLoanAmt").val("");
        $("#txtNetAmount").val("");
    }
    else {
        var amt = 0;
        $(".txtLA").each(function () {
            if (this.value == '')
                amt = amt;
            else
                amt = amt + parseFloat(this.value);
        })
        var ba = parseFloat($("#txtBranchAdjustments").val());
        $("#txtTotLoanAmt").val(amt);
        var na = 0;
        if (amt > ba)
            na = amt - ba;
        else
            na = ba - amt;
        $("#txtNetAmount").val(na);
    }
}

$("input[name=rbtnKGIDNum]:radio").change(function () {
    if (this.value == "true") {
        $.getJSON("/Loan/GetSpouseKgidNumber",
            function (result) {
                if (result == "-1") {
                    alertify.alert('Spouse KGID number is not updated.').setHeader("Warning !!!");
                    $('#txtSpouseKGIDNo').val('');
                    $("#rbtnYIsGovtEmp").prop("checked", false);
                    $("#rbtnNIsGovtEmp").prop("checked", true);
                    $("#divspKGID").hide();
                }
                else if (result == "0") {
                    alertify.alert('Spouse is not an Govt Employee.').setHeader("Warning !!!");
                    $('#txtSpouseKGIDNo').val('');
                    $("#rbtnYIsGovtEmp").prop("checked", false);
                    $("#rbtnNIsGovtEmp").prop("checked", true);
                    $("#divspKGID").hide();
                }
                else if (result == "Fail") {
                    alertify.alert('Spouse salary deductions more than 50% of gross salary is not allowed.').setHeader("Warning !!!");
                    $("#divspKGID").hide();
                    $("#rbtnNIsGovtEmp").prop("checked", false);
                    $("#rbtnYIsGovtEmp").prop("checked", true);
                }
                else {
                    $('#txtSpouseKGIDNo').val(result);
                    $("#divspKGID").show();
                }
            });
    }
    else if (this.value == "false") {
        $("#divspKGID").hide();
        $('#divSpouseAuthentication').hide();
    }
});

function AuthenticateSpouseKGID() {
    if ($('#txtSpouseKGIDNo').val() == 0 || $('#txtSpouseKGIDNo').val() == null || $('#txtSpouseKGIDNo').val() == undefined) {
        alertify.error("Please enter valid kgid number.");
    }
    else {
        $('#divSpouseAuthentication').show();
    }
}

function AuthenticateSpouseOTP() {
    if ($('#txtSpouseOTP').val() == 0 || $('#txtSpouseOTP').val() == null || $('#txtSpouseOTP').val() == undefined) {
        alertify.alert("Please enter valid OTP.").setHeader("Warning");
    }
    else {
        $('#txtSpouseKGIDNo').attr("disabled", "disabled");
        $('#txtSpouseOTP').attr("disabled", "disabled");
        $('#btnAuthenticateSubmit').hide();
        $("#btnAuthenticateSpouseKGID").hide();
    }
}

$("#ddlPrincipleInstallments").change(function () {
    var principalmon = $("#ddlPrincipleInstallments").val();
    var enteredloanamt = $("#txtTotLoanAmt").val();
    var monpplins = parseFloat(enteredloanamt) / parseFloat(principalmon);
    $("#PrincipleAmountInstallment").val(monpplins.toFixed(2));
})

$("#ddlInterestInstallments").change(function () {
    var principalmon = $("#ddlInterestInstallments").val();
    var enteredloanamt = $("#txtTotLoanAmt").val();
    var monintins = (parseFloat(enteredloanamt) * 0.09) / parseFloat(principalmon);
    $("#InterestAmountInstallment").val(monintins.toFixed(2));
})

$("input[name=inlineRadioOptions]:radio").change(function () {
    if (this.value == 'false')
        $(".FamilLoanPurpose").show();
    else
        $(".FamilLoanPurpose").hide();
});

function ViewLoanBA() {
    $.ajax({
        url: '/Loan/GetLoanBADetails',
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            debugger;
            $('#tblLoanBA tbody').empty();
            for (var r = 0; r < result.length; r++) {
                $('#tblLoanBA tbody').append('<tr><td>' + result[r].Policy_No + '</td><td>' + result[r].Month + '</td><td>' + result[r].Year + '</td><td>' + result[r].LoanDue + '</td><td>' + result[r].LoanInterestDue + '</td><td>' + result[r].PremiumDue + '</td><td>' + result[r].PremiumInterestDue + '</td></tr>');
            }
            if (!$.fn.dataTable.isDataTable('#tblLoanBA')) {
                table = $('#tblLoanBA').DataTable({
                    paging: true,
                    info: false,
                    searching: false,
                    "ordering": false,
                });
            }
        }
    });
}

function SubmitApplication() {
    $(".err").attr("hidden", true);

    if ($("#tblExistingLoanDetails tbody tr input[type = checkbox]:checked").length <= 0) {
        alertify.error("Please select policy for applying loan");
        return false;
    }
    else {
        var checkAmt = false;
        $("#tblExistingLoanDetails tbody tr").each(function () {
            debugger;
            var $row = $(this);
            var chk = $row.find("TD").eq(1)[0].children[0].checked;
            var policyno = $row.find("TD").eq(2).html();
            if (!$("#txtLoan_" + policyno).attr("disabled")) {
                if (chk && ($("#txtLoan_" + policyno).val() == "0" || $("#txtLoan_" + policyno).val() == null || $("#txtLoan_" + policyno).val() == '' || $("#txtLoan_" + policyno).val() == "")) {
                    alertify.alert("Please enter loan amount for policy - " + policyno);
                    checkAmt = true;
                }
            }
        })
        if (checkAmt)
            return false;
        if ($("#tblHRMSPayScaleList tbody tr").length == 0) {
            if ($("#docHrmsPaySlip").val() == "") {
                $("#errDocHrmsPaySlip").removeAttr("hidden");
            }
        }
        if ($("#btnAuthenticateSpouseKGID")[0].style.display == "block") {
            $("#errAuthenticateSpouseKGID").removeAttr("hidden");
        }
        else if ($("#btnAuthenticateSubmit")[0].style.display == "block") {
            $("#errAuthenticateSpouseOTP").removeAttr("hidden");
        }
        else if ($('#ddlPrincipleInstallments').val() == 0 || $('#ddlPrincipleInstallments').val() == null || $('#ddlPrincipleInstallments').val() == undefined) {
            $("#errddlPrincipleInstallments").removeAttr("hidden");
        }
        else if ($('#ddlInterestInstallments').val() == 0 || $('#ddlInterestInstallments').val() == null || $('#ddlInterestInstallments').val() == undefined) {
            $("#errddlInterestInstallments").removeAttr("hidden");
        }
        else if ($("#ddlLoanPurposeList").val() == "0") {
            $("#errddlLoanPurposeList").removeAttr("hidden");
        }
        else if ($("#ddlFamilyList").val() == "0") {
            $("#errddlFamilyList").removeAttr("hidden");
        }
        if ($("#rbtn_others").val() == "false") {
            if ($("#ddlFamilyList").val() == "") {
                $("#errddlFamilyList").removeAttr("hidden");
            }
            else if ($("#txtRelationName").val() == "") {
                $("#errtxtRelationName").removeAttr("hidden");
            }
            else if ($("#txtRelationAge").val() == "") {
                $("#errtxtRelationAge").removeAttr("hidden");
            }
        }
        if ($("#chk_Acknowledgement").prop("checked") == false) {
            alertify.alert("Please agree to the terms and condition.").setHeader("Warning !!!");
            return false;
        }
        if ($('.err:visible').length == 0) {
            var loanPolicyDetails = new Array();
            $("#tblExistingLoanDetails tbody tr").each(function () {
                if (this.children[1].children[0].checked) {
                    var policyModel = {};
                    policyModel.PolicyNo = this.children[2].innerHTML;
                    policyModel.LoanAmount = $("#txtLoan_" + policyModel.PolicyNo).val();
                    policyModel.EligibleLoanAmount = this.children[6].innerHTML;
                    loanPolicyDetails.push(policyModel);
                }
            });
            var model = {};
            model.PrincipalInstallments = $("#ddlPrincipleInstallments").val();
            model.InterestInstallments = $("#ddlInterestInstallments").val();
            model.PurposeID = $("#ddlLoanPurposeList").val();
            model.FamilyRelationID = $("#ddlFamilyList").val();
            model.RelationName = $("#txtRelationName").val();
            model.RelationAge = $("#txtRelationAge").val();
            model.NetAmount = $("#txtNetAmount").val();
            model.AppliedAmount = $("#txtTotLoanAmt").val();
            model.Deductions = $("#txtBranchAdjustments").val();
            model.MonthWisePInstallments = $("#PrincipleAmountInstallment").val();
            model.MonthWiseIInstallments = $("#InterestAmountInstallment").val();
            model.listPolicyDetails = loanPolicyDetails;

            $.ajax({
                url: '/Loan/SaveLoanApplication',
                data: JSON.stringify({
                    model: model
                }),
                async: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    if ($("#tblHRMSPayScaleList tbody tr").length == 0) {
                        var model = new FormData();
                        model.append('Document', $("#docHrmsPaySlip").get(0).files[0]);
                        $.ajax({
                            url: '/Loan/UploadPayslip',
                            data: model,
                            async: false,
                            type: 'POST',
                            cache: false,
                            contentType: false,
                            processData: false,
                            success: function (result) {
                                $("#btn_submit").attr("disabled", true);
                                alertify.success("You applied for loan successfully.");
                                window.location.href = "/kgid-loan-app-status/";
                            }
                        });
                    }
                    else {
                        alertify.success("You applied for loan successfully.");
                    }
                },
                error: function (result) {
                    alertify.error("Error occured. While saving loan application.")
                }
            })
        }
    }
}

$("#docHrmsPaySlip").change(function () {
    var checkpdf = $("#docHrmsPaySlip").val().split('.').includes('pdf')
    if (!checkpdf) {
        alertify.error("Please upload pdf file");
        $("#docHrmsPaySlip").val("");
    }
})

$(function () {
    $('.ClAlpha').keyup(function () {
        var input_val = $(this).val();
        var inputRGEX = /^[a-zA-Z]*$/;
        var inputResult = inputRGEX.test(input_val);
        if (!(inputResult)) {
            this.value = this.value.replace(/[^a-z\s]/gi, '');
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
    $('.ClAgeNum').keyup(function () {
        var input_val = $(this).val();
        var inputRGEX = /^[0-9]*$/;
        var inputResult = inputRGEX.test(input_val);
        if (!(inputResult)) {
            this.value = this.value.replace(/[^0-9\s]/gi, '');
        }
        if (parseInt(input_val) > 150) {
            this.value = "";
        }
    });
});

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