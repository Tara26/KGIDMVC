
$(document).ready(function () {
    var RefNo = $('#PrevRefNo').val();
    var category = $('#Category').val();
    $("#btncollapseOne").click(function () {
        if (!ColOneLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/RenewalProposerDetailsToView',
                data: JSON.stringify({ "PageType": "ViewRenewal", "refNo": RefNo, "Category": category}),
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divMIPD").html(data);
                }
            });
            ColOneLoaded = true;
        }
    })
    $("#btncollapseTwo").click(function () {
        if (!ColTwoLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/RenewalVehicleDetailsToView',
                data: JSON.stringify({ "PageType": "ViewRenewal", "refNo": RefNo }),
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divMIVD").html(data);
                }
            });
            ColTwoLoaded = true;
        }
    })
    $("#btncollapseThree").click(function () {
        if (!ColThreeLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/RenewalOtherDetailsToView',
                data: JSON.stringify({ "PageType": "ViewRenewal", "refNo": RefNo }),
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divMIOD").html(data);
                }
            });
            ColThreeLoaded = true;
        }
    })
    $("#btncollapseFour").click(function () {
        if (!ColFourLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/RenewalIDVDetailsToView',
                data: JSON.stringify({ "PageType": "ViewRenewal", "refNo": RefNo }),
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divMIIDV").html(data);
                }
            });

            ColFourLoaded = true;
        }
    })
    $("#btncollapseFive").click(function () {
        if (!ColFiveLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/RenewalPreviousHistoryToView',
                data: JSON.stringify({ "PageType": "ViewRenewal", "refNo": RefNo }),
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divMIPHD").html(data);
                }
            });

            ColThreeLoaded = true;
        }
    })
    $("#btncollapseSIX").click(function () {
        var empId = $('#EmpCode').val();
        var applicationId = $('#ApplicationRefNo').val();
        if (!ColSIXLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/MIPaymentDetailsToView',
                data: JSON.stringify({
                    "PageType": "ViewRenewal", "empId": empId, "applicationId": applicationId
                }),
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divMIPAD").html(data);
                }
            });

            ColSIXLoaded = true;
        }
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
});