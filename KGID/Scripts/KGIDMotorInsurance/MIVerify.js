var ColOneLoaded = false;
var ColTwoLoaded = false;
var ColThreeLoaded = false;
var ColFourLoaded = false;
var ColFiveLoaded = false;
var ColSIXLoaded = false;
var ColSevenLoaded = false;
$(document).ready(function () {

  
    $("#btncollapseOne,#a_col1").click(function () {
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
    $("#btncollapseTwo, #a_col2").click(function () {
       
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
    $("#btncollapseThree ,#a_col3").click(function () {
        if (!ColThreeLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/OtherDetailsToView',
                data: JSON.stringify({}),
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divMIOD").html(data);
                    var vtype = $('#hdnVehicleTypeId').val();
                    if (vtype == '1') {
                        var list = document.getElementsByClassName("slnoid1");
                        for (var i = 1; i <= list.length - 1; i++) {
                            list[i].innerHTML = i;
                        }
                    }
                    else {
                        var list = document.getElementsByClassName("slnoid2");
                        for (var i = 1; i <= list.length - 1; i++) {
                            list[i].innerHTML = i;
                        }
                    }
                }
            });

            ColThreeLoaded = true;
        }
    })
    $("#btncollapseFour ,#a_col4").click(function () {
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
    $("#btncollapseFive ,#a_col5").click(function () {
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
    $("#btncollapseSIX, #a_col6").click(function () {
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

    $("#btncollapseSeven,#a_col7").click(function () {
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
