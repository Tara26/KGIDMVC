
$(document).ready(function () {
    if ($("input[name='ApplicationStatus']:checked").val() == "15") {
        $("#btnDIOSubmit").text("Accept");
    }
    else if ($("input[name='ApplicationStatus']:checked").val() == "18") {
        $("#btnDIOSubmit").text("Reject");
    }
    else {
        $("#btnDIOSubmit").text("Send");
    }
    $.ajax({
        type: 'POST',
        url: '/VerifyDetails/GetRemarkComments',
        data: JSON.stringify({ 'RemarkID': $("#ddlRemarks").val() }),
        async: false,
        contentType: 'application/json; charset=utf-8',
        processData: false,
        cache: false,
        success: function (data) {
            $("#txtComments").val(data);
            if ($("#ddlRemarks option:selected").text() == "No Correction Found") {
                $("#lblSendToEmp").hide();
                $("#divApprove").show();
                $("#divReject").show();
            } else {
                $("#lblSendToEmp").show();
                $("#divApprove").hide();
                $("#divReject").show();
                //$("#btnDIOSubmit").text("Send");
            }
        }
    })
});

function ChangeAppStatus() {
    if ($("input[name='ApplicationStatus']:checked").val() == "15") {
        $("#btnDIOSubmit").text("Accept");
    }
    else if ($("input[name='ApplicationStatus']:checked").val() == "18") {
        $("#btnDIOSubmit").text("Reject");
    }
    else {
        $("#btnDIOSubmit").text("Send");
    }
}

function IntimationToInsured() {
    $(".err").attr("hidden", true);
    $(".chk-req").each(function () {
        if (!$(this).is(":checked")) {
            $(this).siblings(".err").removeAttr("hidden");
        }
    });
    if ($('#txtComments').val() == "" || $('#txtComments').val() == null || $('#txtComments').val() == undefined) {
        $("#errComments").removeAttr("hidden");
    }
    if ($('#ddlRemarks').val() == 0 || $('#ddlRemarks').val() == "0" || $('#ddlRemarks').val() == null || $('#ddlRemarks').val() == "") {
        $("#errRemarks").removeAttr("hidden");
    }
    if ($("input[name='ApplicationStatus']:checked").val() == "" || $("input[name='ApplicationStatus']:checked").val() == null || $("input[name='ApplicationStatus']:checked").val() == undefined) {
        $("#errAction").removeAttr("hidden");
    }
    if ($(".err:visible").length == 0) {
        alertify.confirm("Are you sure you want to submit the changes?", function () {
            if ($("input[name='ApplicationStatus']:checked").val() == "15") {
                $("#mdlDA").modal("show");
                $("#lbldisburseamtvalue").text($("#spnNetAmt").text());
            }
            else {
                UpdateApplicationStatus();
            }
        }).setHeader("Confirm changes?");
    }
    else {
        return false;
    }
}

$("#ddlRemarks").change(function () {
    $.ajax({
        type: 'POST',
        url: '/VerifyDetails/GetRemarkComments',
        data: JSON.stringify({ 'RemarkID': this.selectedOptions[0].value }),
        async: false,
        contentType: 'application/json; charset=utf-8',
        processData: false,
        cache: false,
        success: function (data) {
            $("#txtComments").val(data);
            if ($("#ddlRemarks option:selected").text() == "No Correction Found") {
                $("#lblSendToEmp").hide();
                $("#divApprove").show();
                $("#divReject").show();
            } else {
                $("#lblSendToEmp").show();
                $("#divApprove").hide();
                $("#divReject").show();
            }
        }
    });
});

$("#btnWFNext").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").addClass("show active");
    $("#scrutiny").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").addClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
});

$("#btnAFNext").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").addClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").addClass("active");
});

$("#btnSPrevious").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").addClass("show active");
    $("#scrutiny").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").addClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
});

$("#btnAFPrevious").click(function (e) {
    $("#workflow").addClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").removeClass("show active");

    $("#nav-tab-Workflow").addClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
});

$("#btnDisburse").click(function () {
    $.ajax({
        url: "/Loan/LoanApplicationDisburse",
        type: "POST",
        data: { 'LoanAppID': $("#hdnLoanAppID").val(), 'Type' : 'DIO' },
        datatype: "json",
        success: function (result) {
            if (result == "1") {
                UpdateApplicationStatus();
            }
            else {
                alertify.error("Error occured while disbursing loan amount.");
            }
        }
    });
})

function UpdateApplicationStatus() {
    var loanApplicationWorkflowModel = {
        law_loan_application_id: $("#hdnLoanAppID").val(),
        law_remarks: $('#ddlRemarks').val(),
        law_comments: $('#txtComments').val(),
        law_application_status: $("input[name='ApplicationStatus']:checked").val(),
        law_checklist_verification_status: true,
    }
    $.ajax({
        url: "/loan/LoanApplicationForward",
        type: "POST",
        data: { loanApplicationWorkflowModel: loanApplicationWorkflowModel },
        datatype: "json",
        success: function (result) {
            if (result) {
                window.location.href = "/kgid-dio-loan/";
            }
        }, error: function () {
            alertify.error("Error occured while accepting loan application.");
        }
    });
}

$("#btnDIOClear").click(function () {
    $("#ddlRemarks").val("");
    $("#txtComments").val("");
    $(".chk-req").prop("checked", false);
});