$(document).ready(function () {
    $("#ddlPurposeType").val("1");
    $("#ddlSubPurposeType").val("1");
    $("#ddlReceiptType").val("1");
})

function PrintChallanDetails() {
    var empid = $('#hdnEmployeeID').val();
    var appid = $('#hdnAppID').val();
    //console.log($('#txtpaymentrefno1').val());
    $.ajax({
        url: '/Employee/PrintChallanDetails',
        data: { EmpId: empid, AppId: appid, challanNo: $('#txtChallanRefNo').val() },
        type: 'POST',
        success: function (result) {
            window.location = "/Employee/PrintChallanDetails?challanNo=" + $('#txtpaymentrefno1').val();
            $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
        }
    });
}