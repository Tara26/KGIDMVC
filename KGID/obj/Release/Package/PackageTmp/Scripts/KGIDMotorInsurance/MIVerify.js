var ColOneLoaded = false;
var ColTwoLoaded = false;
var ColThreeLoaded = false;
var ColFourLoaded = false;
var ColFiveLoaded = false;
var ColSIXLoaded = false;
var ColSevenLoaded = false;
$(document).ready(function () {
    $("#btncollapseOne").click(function () {
        // alert("HI");
        var category = $('#Category').val();
        if (!ColOneLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/ProposerDetailsToView',
                data: JSON.stringify({ "Category": category, }),
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
                url: '/ViewDataToVerify/VehicleDetailsToView',
                data: JSON.stringify({}),
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
                url: '/ViewDataToVerify/OtherDetailsToView',
                data: JSON.stringify({}),
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
                url: '/ViewDataToVerify/IDVDetailsToView',
                data: JSON.stringify({}),
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
                url: '/ViewDataToVerify/PreviousHistoryToView',
                data: JSON.stringify({}),
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
        if (!ColSIXLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/UploadedDocumentsToView',
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divMIDoc").html(data);
                }
            });

            ColSIXLoaded = true;
        }
    })

    $("#btncollapseSeven").click(function () {
        var empId = $('#EmpCode').val();
        var applicationId = $('#ApplicationRefNo').val();
        if (!ColSevenLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/MIPaymentDetailsToView',
                data: JSON.stringify({
                    "empId": empId, "applicationId": applicationId
                }),
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divMIPAD").html(data);
                }
            });

            ColSevenLoaded = true;
        }
    })
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
