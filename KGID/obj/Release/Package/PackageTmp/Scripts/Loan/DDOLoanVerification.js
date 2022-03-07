﻿
    $(document).ready(function () {
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
                    $("#lblBackToEmp").hide();
                    $("#lblFrwdToCW").show();
                } else {
                    $("#lblBackToEmp").show();
                    $("#lblFrwdToCW").hide();
                }
            }
        });
    });

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
                    $("#lblBackToEmp").hide();
                    $("#lblFrwdToCW").show();
                } else {
                    $("#lblBackToEmp").show();
                    $("#lblFrwdToCW").hide();
                }
            }
        });
    })

    function Forward() {
        debugger;
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
        if ($(".err:visible").length == 0) {
            var loanApplicationWorkflowModel = {
                law_loan_application_id: $("#hdnLoanAppID").val(),
                law_remarks: $('#ddlRemarks').val(),
                law_comments: $('#txtComments').val(),
                law_application_status: $("input[name='ApplicationStatus']:checked").val(),
                law_checklist_verification_status: true,
            }
        //var msg = "";
        //var successmsg = "";
        //var errormsg = "";
            //if ($("input[name='ApplicationStatus']:checked").val() == 2) {
        //    msg = "Do you want to send back the loan application to employee?";
        //    successmsg = "Loan Application Sended Back Successfully.";
        //    errormsg = "There is a problem in loan application sending back.";
        //}
        //else {
        //    msg = "Do you want to forward the loan application?";
        //    successmsg = "Loan Application Forwarded Successfully.";
        //    errormsg = "There is a problem in forwarding loan application.";
        //}
        alertify.confirm("Are you sure you want to submit the changes?", function () {
            $.ajax({
                url: "/loan/LoanApplicationForward",
                type: "POST",
                data: { loanApplicationWorkflowModel: loanApplicationWorkflowModel },
                datatype: "json",
                success: function (result) {
                    if (result) {
                        window.location.href = "/kgid-ddo-loan";
                    }
                }
            });
        }).setHeader("Confirm changes?");
    }
        else {
            return false;
}
}

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
$("#btnClear").click(function () {
    $("#ddlRemarks").val("");
    $("#txtComments").val("");
    $(".chk-req").prop("checked", false);
});