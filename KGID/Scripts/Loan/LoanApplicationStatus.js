$(document).ready(function () {
    if ($("#tblViewLoanApplicationList tbody tr").length > 0) {
        $("#tblViewLoanApplicationList").DataTable();
        $("#p_noloanapp").hide();
    }
    else {
        $("#tblViewLoanApplicationList").hide();
        $("#p_noloanapp").show();
    }
});

function CancelLoan(EmpID, LoanID) {
    var loanApplicationCancel = {
        LoanID: LoanID,
        EmpID: EmpID,
    }
    alertify.confirm("Do you want to cancel the application?", function () {
        $.ajax({
            url: "/Loan/LoanApplicationCancel",
            type: "POST",
            data: { loanApplicationCancel: loanApplicationCancel },
            datatype: "json",
            success: function (result) {
                if (result == "1") {
                    alertify.success("Loan Application cancelled successfully.");
                    setTimeout(function () {
                        window.location.href = '/kgid-loan-app-status/';
                    }, 2000);
                }
                else {
                    alertify.error("Error occured while cancelling application.");
                }
            }
        });
    }, function () {
    }).set('labels', { ok: 'Yes', cancel: 'No' }).setHeader("Confirmation");
}