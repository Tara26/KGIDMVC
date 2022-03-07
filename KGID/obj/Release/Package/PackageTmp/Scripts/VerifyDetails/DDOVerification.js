
    var ColOneLoaded = false;
    var ColTwoLoaded = false;
    var ColThreeLoaded = false;
    var ColFourLoaded = false;
    var ColFiveLoaded = false;
    var medSupportingDocs = [];
    var today = '';
    var minDate = '';
    var dates = [];
    var rowno = 0;
$(document).ready(function () {
    // Add minus icon for collapse element which is open by default
    $(".collapse.show").each(function () {
        $(this).prev(".card-header").find(".fa").addClass("fa-minus").removeClass("fa-plus");
    });

    // Toggle plus minus icon on show hide of collapse element
    $(".collapse").on('show.bs.collapse', function () {
        $(this).prev(".card-header").find(".fa").removeClass("fa-plus").addClass("fa-minus");
    }).on('hide.bs.collapse', function () {
        $(this).prev(".card-header").find(".fa").removeClass("fa-minus").addClass("fa-plus");
    });

    $("#spnEName").text($("#spnProposerName").text());
    $("#spnRNo").text($("#hdnBReferanceNo").val());
    $('#tblDDOWorkflow').DataTable({
        paging: false,
        info: false,
        searching: false,
        "ordering": false
    });

    $(".nav-item").click(function (e) {
        $("#viewSidebar")[0].style.width = "0";
        $("#applicationFormTab")[0].style.marginRight = "0";
    });

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
            } else {
                $("#lblBackToEmp").show();
            }
        }
    });
    $("#hdnRowNumber").val($('#tblMedicalLeaveDetails tbody tr').length);
    $(".divreimbursedoc").attr("hidden");
    $("#rbtnYMed").prop("checked", false);
    $("#rbtnNMed").prop("checked", true);
    $(".rd-btn-reimburse:checked").val("False");
    today = new Date();
    if ($("#hdnDOJ").val() != "") {
        var datFromArray = $("#hdnDOJ").val().split("-");
        var dFrom = datFromArray[1] + "-" + datFromArray[0] + "-" + datFromArray[2];
        minDate = new Date(dFrom);
    }

    $('#txtMedFromDate').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        minDate: minDate,
        maxDate: today,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        //beforeShowDay: function (date) {
        //    var string = jQuery.datetimepicker.format('d-m-Y', date);
        //    return [dates.indexOf(string) == -1]
        //},
        scrollMonth: false,
        scrollInput: false,
        keepOpen: false,
        closeOnDateSelect: true
    });

    $('#txtMedToDate').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        minDate: minDate,
        maxDate: today,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        //beforeShowDay: function (date) {
        //    var string = jQuery.datetimepicker.format('d-m-Y', date);
        //    return [dates.indexOf(string) == -1]
        //},
        scrollMonth: false,
        scrollInput: false,
        keepOpen: false,
        closeOnDateSelect: true

    });
    $('#txtEditMedFromDate').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        minDate: minDate,
        maxDate: today,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        //beforeShowDay: function (date) {
        //    var string = jQuery.datetimepicker.format('d-m-Y', date);
        //    return [dates.indexOf(string) == -1]
        //},
        scrollMonth: false,
        scrollInput: false,
        closeOnDateSelect: true
    });
    $('#txtEditMedToDate').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        minDate: minDate,
        maxDate: today,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        //beforeShowDay: function (date) {
        //    var string = jQuery.datetimepicker.format('d-m-Y', date);
        //    return [dates.indexOf(string) == -1]
        //},
        scrollMonth: false,
        scrollInput: false,
        closeOnDateSelect: true
    });

    if ($.fn.dataTable.isDataTable('#tblMedicalLeaveDetails')) {
        table = $('#tblMedicalLeaveDetails').DataTable();
    }
    else {
        table = $('#tblMedicalLeaveDetails').DataTable({
            paging: false,
            info: false,
            searching: false,
            "columnDefs": [
                {
                    "orderable": false,
                    "targets": [0, 1, 2, 3, 4, 5]
                }
            ]
        });
    }
});

function ShowApplicationDetails(id) {
    $("#divBDetails").hide();
    $("#divFNDetails").hide();
    $("#divPMLDetails").hide();
    if (id == 1) {
        $("#divBDetails").show();
    } else if (id == 2) {
        $("#divFNDetails").show();
    } else if (id == 3) {
        $("#divPMLDetails").show();
    }
    $("#mdBasicDetails").modal("show");
}
$("#btnSubmit").click(function (e) {
    $(".err").hide();

    $(".chk-req").each(function () {
        if (!$(this).is(":checked")) {
            $(this).siblings(".err").show();
        }
    });

    var isStatusSelected = true;
    if ($("input[name='ApplicationStatus']:checked").length === 0) {
        $("input[name='ApplicationStatus']").parent().siblings(".err").show();
        isStatusSelected = false;
    }

    var isCheckboxchecked = true;
    if ($(".err:visible").length > 0) {
        isCheckboxchecked = false;
    }

    var isSendBack = true;
    if ($("#ddlRemarks").val() == null || $("#ddlRemarks").val() == null) {
        $("#errRemarksReq").show();
        isSendBack = false;
    }

    if (isStatusSelected && isCheckboxchecked && isSendBack) {
        alertify.confirm(($("input[name='ApplicationStatus']:checked").val() === "2") ? "Are you sure you want to send back application to employee?" : "Are you sure you want to forward application to KGID office?", function () {
            $(".mederr").hide();
            // $("#frmDDOVerDetails").submit();
            var objVerifyDetails = new FormData($("#frmDDOVerDetails").get(0));
            $.ajax({
                type: 'POST',
                url: '/SaveDDOVData/',
                data: objVerifyDetails,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                success: function (result) {
                    if (result == "1") {
                        if ($("input[name='ApplicationStatus']:checked").val() === "2") {
                            window.location.href = "/kgid-ddo/";
                        }
                        else {
                            alertify.alert("Forwarded to KGID department.", function () {
                                window.location.href = "/kgid-ddo/";
                            }).setHeader("Attention");
                        }
                    }
                    else if (result == "7") {
                        alertify.alert("There is no Caseworker mapped for this district.", function () {
                            window.location.href = "/kgid-ddo/";
                        }).setHeader("Warning");
                    }

                }
            });
        }).setHeader("Confirm changes?");
    }

    e.preventDefault();
});

function printApplicationForm() {
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Basic Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintBasicDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPersonal").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html();
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>Application Form</title>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.
    //frameDoc.document.write('<link href="/Content/Custom/sb-admin-2.min.css" rel="stylesheet" />');
    frameDoc.document.write('<link href="/Scripts/DataTables/Bootstrap-4-4.1.1/css/bootstrap.min.css" rel="stylesheet" />');
    //Append the DIV contents.
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);

}
function printPaymentDetails() {
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Payment Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPaymentDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Payment Status</h2></div><div class="form-group col-4"></div></div>' + $("#divPaymentStatus").html() + "<hr />";
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>Payment Details</title>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.
    //frameDoc.document.write('<link href="/Content/Custom/sb-admin-2.min.css" rel="stylesheet" />');
    frameDoc.document.write('<link href="/Scripts/DataTables/Bootstrap-4-4.1.1/css/bootstrap.min.css" rel="stylesheet" />');
    //Append the DIV contents.
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);

}
function printMedicalReport() {

    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Basic Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHBasicDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Physical Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHPhysicalDetails").html() + "<hr />";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Other Details</h3></div><div class="form-group col-4"></div></div>' + $("#divOtherDetails").html() + "<hr />";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Health Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHealthDetails").html() + "<hr />";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Doctor Details</h3></div><div class="form-group col-4"></div></div>' + $("#divDoctorDetails").html(); + "<hr />";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Doctor Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHDeclaration").html();
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>Medical Report</title>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.
    frameDoc.document.write('<link href="/Scripts/DataTables/Bootstrap-4-4.1.1/css/bootstrap.min.css" rel="stylesheet" />');
    //Append the DIV contents.
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);
}
function ChangeVerificationStatus(destid, srcid) {
    var verificationstatus = $("#" + srcid).prop("checked");
    $("#" + destid).prop("checked", verificationstatus);
}
$("#btnClear").click(function (e) {
    $("#ddlRemarks").val("");
    $("#txtComments").val("");
    $("#rbtnBackToEmployee").prop("checked", false);
    $("#rbtnForwardToCW").prop("checked", false);
    $(".verify").prop("checked", false);
    e.preventDefault();
});
function showtab(btnid, divid) {
    $("#applicationForm").removeClass("fade").addClass("active show");
    $("#nav-tab-Form").attr("aria-selected", "true");
    $("#scrutiny").removeClass("active show").addClass("fade");
    $("#nav-tab-Scrutiny").attr("aria-selected", "false");
    $("#nav-tab-Form").addClass("active show");
    $("#nav-tab-Scrutiny").removeClass("active show");
    $(".collapse").removeClass("show");
    $(".card-header > h2 > button").attr("aria-expanded", "false");
    $(".card-header > h2 > button > i").removeClass("fa-minus").addClass("fa-plus");
    $("#" + btnid > button).attr("aria-expanded", "true").removeClass("collapsed");
    $("#divid").addClass("show");
}
function closeNav() {
    $("#viewSidebar")[0].style.width = "0";
    $("#applicationFormTab")[0].style.marginRight = "0";
}
$("#btncollapseOne,#a_colone").click(function () {
    if (!ColOneLoaded) {
        $.ajax({
            url: '/ViewDataToVerify/KGIDDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divKD").html(data);
                EnglishToKannada();
            }
        });
        ColOneLoaded = true;
    }
})
    $("#btncollapseTwo,#a_coltwo").click(function () {
        if (!ColTwoLoaded) {
            $.ajax({
                url: '/ViewDataToVerify/FamilyDetailsToView',
                data: JSON.stringify({}),
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divFD").html(data);
                    EnglishToKannada();
                }
            })
            $.ajax({
                url: '/ViewDataToVerify/NomineeDetailsToView',
                data: JSON.stringify({}),
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#divND").html(data);
                    EnglishToKannada();
                }
            })
            ColTwoLoaded = true;
        }
    })
$("#btncollapseThree,#a_colthree").click(function () {
    if (!ColThreeLoaded) {
        $.ajax({
            url: '/ViewDataToVerify/PersonalDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divPD").html(data);
                EnglishToKannada();
            }
        });
        ColThreeLoaded = true;
    }
})
$("#btncollapseFour,#a_colfour").click(function () {
   
    var empid = $('#hdnAddEmpCode').val();
    var appid = $('#hdnAppCode').val();
    if (!ColFourLoaded) {
        $.ajax({
            url: '/ViewDataToVerify/PaymentDetailsToView',
            data: JSON.stringify({ EmpId: empid, AppId: appid}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divPAD").html(data);
                EnglishToKannada();
            }
        });
        ColFourLoaded = true;
    }
})
$("#btncollapseFive,#a_colfive").click(function () {
    if (!ColFiveLoaded) {
        $.ajax({
            url: '/ViewDataToVerify/HPhysicalDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divHPD").html(data);
                EnglishToKannada();
            }
        });
        $.ajax({
            url: '/ViewDataToVerify/HOtherDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divHOD").html(data);
                EnglishToKannada();
            }
        })
        $.ajax({
            url: '/ViewDataToVerify/HHealthDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divHHD").html(data);
                EnglishToKannada();
            }
        })
        $.ajax({
            url: '/ViewDataToVerify/HDoctorDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divHDD").html(data);
                EnglishToKannada();
                if (!$("#rbtnKMCDr").prop("checked"))
                    $("#divDctrBnkDetails").show();
                else
                    $("#divDctrBnkDetails").hide();
            }
        })
        ColFiveLoaded = true;
    }
})
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
                    $("#lblfrwftoddo").show();
                } else {
                    $("#lblBackToEmp").show();
                    $("#lblfrwftoddo").hide();
                }
            }
        });
    });

$("#btnWFNext").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").addClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").addClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").removeClass("active");
})
$("#btnAFNext").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").addClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").addClass("active");
    $("#nav-tab-Documents").removeClass("active");
})
$("#btnSPrevious").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").addClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").addClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").removeClass("active");
})
$("#btnAFPrevious").click(function (e) {
    $("#workflow").addClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").addClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").removeClass("active");
})
$("#btnSNext").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").addClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").addClass("active");

    //LoadUploadedDocuments();
})

$("#btnUPrevious").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").addClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").addClass("active");
    $("#nav-tab-Documents").removeClass("active");
})

$("#nav-tab-Documents").click(function () {
    //LoadUploadedDocuments();
})

function LoadUploadedDocuments() {
    $.ajax({
        url: '/VerifyDetails/LoadUploadedDocuments',
        async: false,
        type: 'POST',
        contentType: false,
        processData: false,
        success: function (result) {
            var htmlData = '';
            for (var i = 0; i < result.length; i++) {
                htmlData = '<div class="form-group col-6"><label class="control-label text-justify col-6">' + result[i].UploaddocType + '</label><a class="viewuploadeddoc col-6" href="javascript:;" data-path='+result[i].UploaddocPath+'>Click Here</a></div>'
            }
            $("#divLoadUploadedDocuments").html(htmlData);
        }
    });
}

$("#txtMedFromDate, #txtMedToDate").change(function () {
    $(".mederr").attr("hidden", true);
    var dateFromString = $("#txtMedFromDate").val();
    var dateToString = $("#txtMedToDate").val();
    if (dateFromString !== "" && dateToString !== "") {
        var datFromArray = dateFromString.split("-");
        var dFrom = datFromArray[1] + "-" + datFromArray[0] + "-" + datFromArray[2];
        var dateFrom = new Date(dFrom);
        var datToArray = dateToString.split("-");
        var dTo = datToArray[1] + "-" + datToArray[0] + "-" + datToArray[2];
        var dateTo = new Date(dTo);
        if (dateFrom == "Invalid Date") {
            $('#errFromDate').text('Please enter date in DD/MM/YYYY format');
            $('#errFromDate').removeAttr("hidden");
            return;
        }
        if (dateTo == "Invalid Date") {
            $('#errToDate').text('Please enter date in DD/MM/YYYY format');
            $('#errToDate').removeAttr("hidden");
            return;
        }
        if (dateFrom < minDate || dateTo < minDate) {
            $('#errToDate').text('Leave dates should not be prior to joining date');
            $('#errToDate').removeAttr("hidden");
            return;
        }
        if (dateFrom > today || dateTo > today) {
            $('#errToDate').text('You cannot enter future dates');
            $('#errToDate').removeAttr("hidden");
            return;
        }
        if (dateTo < dateFrom) {
            $('#errToDate').text('Leave end date sholud be greater than Leave start date');
            $('#errToDate').removeAttr("hidden");
            return;
        }
        var difference = new Date(dateTo - dateFrom);
        var totalLeaves = Math.round(difference / (1000 * 60 * 60 * 24)) + 1;
        $("#txtMedTotalLeaves").val(totalLeaves);
    }
});

$("#txtEditMedFromDate, #txtEditMedToDate").change(function () {
    $(".mederr").attr("hidden", true);
    var dateFromString = $("#txtEditMedFromDate").val();
    var dateToString = $("#txtEditMedToDate").val();
    if (dateFromString !== "" && dateToString !== "") {
        var datFromArray = dateFromString.split("-");
        var dFrom = datFromArray[1] + "-" + datFromArray[0] + "-" + datFromArray[2];
        var dateFrom = new Date(dFrom);

        var datToArray = dateToString.split("-");
        var dTo = datToArray[1] + "-" + datToArray[0] + "-" + datToArray[2];
        var dateTo = new Date(dTo);

        if (dateFrom == "Invalid Date") {
            $('#errEditFromDate').text('Please enter date in DD/MM/YYYY format');
            $('#errEditFromDate').removeAttr("hidden");
            return;
        }
        if (dateTo == "Invalid Date") {
            $('#errEditToDate').text('Please enter date in DD/MM/YYYY format');
            $('#errEditToDate').removeAttr("hidden");
            return;
        }
        if (dateFrom < minDate || dateTo < minDate) {
            $('#errEditToDate').text('Leave dates should not be prior to joining date');
            $('#errEditToDate').removeAttr("hidden");
            return;
        }
        if (dateFrom > today || dateTo > today) {
            $('#errEditToDate').text('You cannot enter future dates');
            $('#errEditToDate').removeAttr("hidden");
            return;
        }
        if (dateTo < dateFrom) {
            $('#errEditToDate').text('Leave end date sholud be greater than Leave start date');
            $('#errEditToDate').removeAttr("hidden");
            return;
        }
        var difference = new Date(dateTo - dateFrom);
        var totalLeaves = Math.round(difference / (1000 * 60 * 60 * 24)) + 1;
        $("#txtEditMedTotalLeaves").val(totalLeaves);
    }
});

function AddMedicalLeave() {
    $('.mederr').attr("hidden", true);
    ValidateMedLeaveFields();
    if ($('.mederr:visible').length === 0) {
        //var fromDate = $("#txtMedFromDate").val();
        //var toDate = $("#txtMedToDate").val();

        //var dob = $("#txtDateOfBirth").val().split("-");
        //var doa = $("#txtDateOfAppointment").val().split("-");
        //var doj = $("#txtewd_date_of_joining_post").val().split("-");

        //var dateofbir = dob[2] + "/" + dob[1] + "/" + dob[0]; //$("#txtDateOfBirth").val();
        //var dateofapp = doa[2] + "/" + doa[1] + "/" + doa[0]; //$("#txtDateOfAppointment").val();
        //var dateofjoin = doj[2] + "/" + doj[1] + "/" + doj[0];//$("#txtewd_date_of_joining_post").val();

        var df = $("#txtMedFromDate").val().split("-");
        var fromDate = df[2] + "/" + df[1] + "/" + df[0];
        var gfromDate = df[0] + "-" + df[1] + "-" + df[2];

        var dt = $("#txtMedToDate").val().split("-");
        var toDate = dt[2] + "/" + dt[1] + "/" + dt[0];
        var gtoDate = dt[0] + "-" + dt[1] + "-" + dt[2];

        var leaves = $("#txtMedTotalLeaves").val();
        var remarks = $("#txtMedRemarks").val();
        var supportingDoc = $("#flMedSupportingDoc").get(0).files[0];
        var isreimbursed = $(".rd-btn-reimburse:checked").val();
        var reimburseDoc = $("#flReimburseSupportingDoc").get(0).files[0];
        if (isreimbursed == "False") {
            reimburseDoc = undefined;
        }
        if (isreimbursed == "True") {
            if (typeof (reimburseDoc) == undefined) {
                $("#errReimbursedoc").text("error");
                $("#errReimbursedoc").removeAttr("hidden");
                return;
            }
            if ($("#flReimburseSupportingDoc").length == 0) {
                $("#errReimbursedoc").text("error");
                $("#errReimbursedoc").removeAttr("hidden");
                return false;
            }
        }
        var supportingdocpath = ""; var reimbursedocpath = "";
        var model = new FormData();
        model.append('doc', supportingDoc);
        model.append('reimbursedoc', reimburseDoc);
        $.ajax({
            url: '/Employee/UploadMedicalLeaveDocument',
            data: model,
            async: false,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (result) {
                supportingdocpath = result.split("~")[0];
                reimbursedocpath = result.split("~")[1];
                var MedicalLeave = {};
                MedicalLeave.emld_application_id = $("#hdnAppCode").val();
                MedicalLeave.startdate = fromDate;
                MedicalLeave.enddate = toDate;
                MedicalLeave.emld_no_of_days = leaves;
                MedicalLeave.emld_leave_reason = remarks;
                MedicalLeave.emld_upload_document_path = supportingdocpath;
                MedicalLeave.emld_medical_reimbursement = isreimbursed;
                MedicalLeave.emld_medical_reimbursement_doc = reimbursedocpath;
                MedicalLeave.emld_emp_id = $("#hdnEmployeeCode").val();
                MedicalLeave.Type = "Insert";

                $.ajax({
                    url: '/Employee/SaveDDOMedicalLeave',
                    data: JSON.stringify({ "MedicalLeaveData": MedicalLeave }),//JSON.stringify(),
                    async: false,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    success: function (result) {
                        if ($("#divMedLeave").find(".dataTables_empty").length === 1) {
                            $("#divMedLeave").find(".dataTables_empty").parent("tr").remove();
                        }
                        var newClass; var AddRow = "";
                        if ($("tr[data-row-number=" + $("#divMedLeave tr[data-row-number]").length + "]").hasClass("odd")) {
                            newClass = "even";
                        } else {
                            newClass = "odd";
                        }
                        
                        if (typeof supportingDoc != "undefined" && typeof reimburseDoc != "undefined")
                            AddRow = "<tr data-row-number='" + result + "' role='row' class='" + newClass + "'><td>" + gfromDate + "</td><td>" + gtoDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td><a href='/Home/ViewFilePath?FilePath=" + supportingdocpath + "' target='_blank' style='text-decoration: underline; color: #0056b3;'>Click Here</a></td><td><a href='/Home/ViewFilePath?FilePath=" + reimbursedocpath + "' target='_blank' style='text-decoration: underline; color: #0056b3;'>Click Here</a></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + result + ");'>Delete</a></td><td hidden>" + isreimbursed + "</td><td hidden>" + supportingdocpath + "</td><td hidden>" + reimbursedocpath + "</td><td hidden>" + result + "</td></tr>";
                        else if (typeof supportingDoc == "undefined" && typeof reimburseDoc != "undefined")                                      
                            AddRow = "<tr data-row-number='" + result + "' role='row' class='" + newClass + "'><td>" + gfromDate + "</td><td>" + gtoDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td></td><td><a href='/Home/ViewFilePath?FilePath=" + reimbursedocpath + "' target='_blank' style='text-decoration: underline; color: #0056b3;'>Click Here</a></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + result + ");'>Delete</a></td><td hidden>" + isreimbursed + "</td><td hidden>" + supportingdocpath + "</td><td hidden>" + reimbursedocpath + "</td><td hidden>" + result + "</td></tr>";
                        else if (typeof supportingDoc != "undefined" && typeof reimburseDoc == "undefined")                                      
                            AddRow = "<tr data-row-number='" + result + "' role='row' class='" + newClass + "'><td>" + gfromDate + "</td><td>" + gtoDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td><a href='/Home/ViewFilePath?FilePath=" + supportingdocpath + "' target='_blank' style='text-decoration: underline; color: #0056b3;'>Click Here</a></td><td></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + result + ");'>Delete</a></td><td hidden>" + isreimbursed + "</td><td hidden>" + supportingdocpath + "</td><td hidden>" + reimbursedocpath + "</td><td hidden>" + result + "</td></tr>";
                        else                                                                                                                     
                            AddRow = "<tr data-row-number='" + result + "' role='row' class='" + newClass + "'><td>" + gfromDate + "</td><td>" + gtoDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td></td><td></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + result + ");'>Delete</a></td><td hidden>" + isreimbursed + "</td><td hidden>" +'"'+ supportingdocpath + '"'+"</td><td hidden>" +'"'+ reimbursedocpath +'"'+ "</td><td hidden>" + result + "</td></tr>";

                        $("#tblMedicalLeaveDetails tbody").append(AddRow);
                        alertify.success("Medical Leave details saved successfully");
                        CalculateMedicalLeave();
                    }, error: function (result) {
                        alertify.error("Could not save medical leave details");
                    }
                });
            }, error: function (result) {
                alertify.error("Could not upload medical leave document");
            }
        });
        ResetMedLeaveFields();
    }
}



function ResetMedLeaveFields() {
    $("#txtMedFromDate").val("");
    $("#txtMedToDate").val("");
    $("#txtMedTotalLeaves").val("");
    $("#txtMedRemarks").val("");
    $("#flMedSupportingDoc").val("");
    $("#flReimburseSupportingDoc").val("");
    $(".divreimbursedoc").attr("hidden", true);
    $('.mederr').attr("hidden", true);
    $("#rbtnYMed").prop("checked", false);
    $("#rbtnNMed").prop("checked", true);
    $(".rd-btn-reimburse:checked").val("False");
}

function ValidateMedLeaveFields() {
   $('.mederr').attr("hidden", true);
    var dateFromString = $("#txtMedFromDate").val();
    var dateToString = $("#txtMedToDate").val();

    //if (dateFromString === "") {
    //    $('#errFromDate').text('Please enter leave start date');
    //    $('#errFromDate').removeAttr("hidden");
    //    $('#errFromDate').css("display", "block")

    //}

    //if (dateToString === "") {
    //    $('#errToDate').text('Please enter leave end date');
    //    $('#errToDate').removeAttr("hidden");
    //    $('#errToDate').css("display", "block")
    //}
    var isDate = true;
    if (dateFromString === "") {
        $('#errFromDate').text('Please enter leave start date');
        $('#errFromDate').removeAttr("hidden");
        $('#errFromDate').css("display", "block");
        isDate = false;
    }

    if (dateToString === "") {
        $('#errToDate').text('Please enter leave end date');
        $('#errToDate').removeAttr("hidden");
        $('#errToDate').css("display", "block");
        isDate = false;
    }
    if (isDate == false) {
        return false;
    }
    var isreimbursed = $(".rd-btn-reimburse:checked").val();

    if (isreimbursed == "False") {
        reimburseDoc = undefined;
    }
    if (isreimbursed == "True") {
        if ($("#flReimburseSupportingDoc").val().length == 0) {
            $("#errReimbursedoc").text("Please upload document for medical reimbursed");
            $("#errReimbursedoc").removeAttr("hidden");
            $('#errReimbursedoc').css("display", "block")
            return false;
        }
    }
    return true;
}

function ReimburseChange() {
    if ($(".rd-btn-reimburse:checked").val() === "True") {
        $(".divreimbursedoc").removeAttr("hidden");
        $("#flReimburseSupportingDoc").val("");
    }
    else {
        $(".divreimbursedoc").attr('hidden', true);
    }
}

function CalculateMedicalLeave() {
    var mLeave = 0;
    $("#tblMedicalLeaveDetails TBODY TR").each(function () {
        var row = $(this);
        mLeave = parseInt(mLeave) + parseInt(row.find("TD").eq(2).html());
    });
    $("#txtMLeave").val(mLeave);
}

//Delete Medical Leave
function DeleteMedLeaveRow(rowNumber) {
    $.ajax({
        url: '/Employee/DeleteMedicalLeaveDetails',
        data: JSON.stringify({ 'EmpCode': rowNumber }),
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        processData: false,
        cache: false,
        success: function (result) {
            $("#divMedLeave tr[data-row-number='" + rowNumber + "']").remove();
            if ($("#divMedLeave").find("tbody").find("tr").length === 0) {
                var AddRow = "<tr class='odd'><td valign='top' colspan='8' class='dataTables_empty'>No data available in table</td></tr>";
                $("#tblMedicalLeaveDetails tbody").append(AddRow);
                $("#txtMLeave").val("0");
            }
            else
                CalculateMedicalLeave();
            alertify.success("Medical leave details deleted successfully");
        }, error: function (result) {
            alertify.error("Could not delete medical leave details");
        }
    });
}

//// Edit Medical Leave
function EditToggleReimburseDetails() {
    if ($(".rd-btn-editreimburse:checked").val() === "True") {
        $(".diveditreimbursedoc").removeAttr("hidden");
        $("#flEditReimburseSupportingDoc").val("");
    }
    else {
        $(".diveditreimbursedoc").attr("hidden", true);
    }
}

function EditMedLeaveDetail(rowNumber) {
    debugger;
    ResetEditMedLeaveFields();
    $("#hdnMedLeaveRowNumber").val(rowNumber);
    var $row = $("#divMedLeave tr[data-row-number=" + rowNumber + "]");

    var startdate = $row.find("TD").eq(0).html();
    var enddate = $row.find("TD").eq(1).html();
    var a = dates.indexOf(startdate);
    var b = dates.indexOf(enddate);
    dates.splice(a, b - a + 1);
    $("#txtEditMedFromDate").val($row.find("TD").eq(0).html());
    $("#txtEditMedToDate").val($row.find("TD").eq(1).html());
    $("#txtEditMedTotalLeaves").val($row.find("TD").eq(2).html());
    $("#txtEditMedRemarks").val($row.find("TD").eq(3).html());
    $("#flEditMedSupportingDoc").val("");
    $("#flEditReimburseSupportingDoc").val("");
    $('.mederr').attr("hidden", true);
    if ($row.find("TD").eq(7).html() == "True") {
        $("#rbtnEditNMed").prop("checked", false);
        $("#rbtnEditYMed").prop("checked", true);
        $(".diveditreimbursedoc").removeAttr("hidden");
        $(".rd-btn-editreimburse:checked").val("True");
    }
    else {
        $("#rbtnEditNMed").prop("checked", true);
        $("#rbtnEditYMed").prop("checked", false);
        $(".diveditreimbursedoc").attr("hidden", true);
        $(".rd-btn-editreimburse:checked").val("False");
    }
    if ($row.find("TD").eq(8).html() != null && $row.find("TD").eq(8).html() != undefined && $row.find("TD").eq(8).html() != "") {
        var b = new ClipboardEvent("").clipboardData || new DataTransfer();
        var files = [
            new File(['content'], $row.find("TD").eq(8).html())
        ];
        $("#flEditMedSupportingDoc").get(0).files = new FileListItems(files);
        $("#hdnMedSupportingDoc").val(files[0].name);
    }
    if ($row.find("TD").eq(9).html() != null && $row.find("TD").eq(9).html() != undefined && $row.find("TD").eq(9).html() != "") {
        var b = new ClipboardEvent("").clipboardData || new DataTransfer();
        var rfiles = [
            new File(['content'], $row.find("TD").eq(9).html())
        ];
        $("#flEditReimburseSupportingDoc").get(0).files = new FileListItems(files)
        $("#hdnReimbursedDocument").val(rfiles[0].name);
    }
    $("#mdUpdateMedicalLeave").modal("show");
}

function FileListItems(files) {
    var b = new ClipboardEvent("").clipboardData || new DataTransfer()
    for (var i = 0, len = files.length; i < len; i++) b.items.add(files[i])
    return b.files
}

function UpdateMedicalLeave() {
    $('.mederr').attr("hidden", true);
    ValidateEditMedLeaveFields();
    if ($('.err-edit:visible').length === 0) {
       // var fromDate = $("#txtEditMedFromDate").val();
        //var toDate = $("#txtEditMedToDate").val();
        var leaves = $("#txtEditMedTotalLeaves").val();
        var remarks = $("#txtEditMedRemarks").val();
        var supportingDoc = $("#flEditMedSupportingDoc").get(0).files[0];
        var editreimburseDoc = $("#flEditReimburseSupportingDoc").get(0).files[0];

        var df = $("#txtEditMedFromDate").val().split("-");
        var fromDate = df[2] + "/" + df[1] + "/" + df[0];
        var gfromDate = df[0] + "-" + df[1] + "-" + df[2];

        var dt = $("#txtEditMedToDate").val().split("-");
        var toDate = dt[2] + "/" + dt[1] + "/" + dt[0];
        var gtoDate = dt[0] + "-" + dt[1] + "-" + dt[2];


        //var supportingDoc = $("#hdnMedSupportingDoc").val();
        //var editreimburseDoc = $("#hdnReimbursedDocument").val();
        var isreimburse = $(".rd-btn-editreimburse:checked").val();
        var supportingdocpath = ""; var reimbursedocpath = "";
        if (isreimburse == "False") {
            editreimburseDoc = undefined;
        }
        if (isreimburse == "True") {
            if ($("#flEditReimburseSupportingDoc").val().length == 0) {
                $("#errEditReimbursedoc").removeAttr("hidden");
                $("#errEditReimbursedoc").text("Please upload document for medical reimbursed");
                return;
            }
        }

        var model = new FormData();
        model.append('doc', supportingDoc);
        model.append('reimbursedoc', editreimburseDoc);
        $.ajax({
            url: '/Employee/UploadMedicalLeaveDocument',
            data: model,
            async: false,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (result) {
                supportingdocpath = result.split("~")[0];
                reimbursedocpath = result.split("~")[1];
                var MedicalLeave = {};
                MedicalLeave.emld_application_id = $("#hdnAppCode").val();
                MedicalLeave.startdate = fromDate;
                MedicalLeave.enddate = toDate;
                MedicalLeave.emld_no_of_days = leaves;
                MedicalLeave.emld_leave_reason = remarks;
                MedicalLeave.emld_upload_document_path = supportingdocpath;
                MedicalLeave.emld_medical_reimbursement = isreimburse;
                MedicalLeave.emld_medical_reimbursement_doc = reimbursedocpath;
                MedicalLeave.emld_emp_id = $("#hdnEmployeeCode").val();
                MedicalLeave.Type = "Update";

                $.ajax({
                    url: '/Employee/SaveDDOMedicalLeave',
                    data: JSON.stringify({ "MedicalLeaveData": MedicalLeave }),//JSON.stringify(),
                    async: false,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    success: function (result) {
                        debugger;
                        var rowNumber = $("#hdnMedLeaveRowNumber").val();
                        var newClass = $("#divMedLeave tr[data-row-number=" + rowNumber + "]").attr("class");
                        var updatedRow = "";
                        if (typeof supportingDoc == "undefined" && typeof editreimburseDoc == "undefined") {
                            updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td>" + gfromDate + "</td><td>" + gtoDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td></td><td></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimburse + "</td><td hidden>'" + supportingdocpath + "'</td><td hidden>'" + reimbursedocpath + "'</td></tr>";
                        }
                        else if (typeof supportingDoc != "undefined" && typeof editreimburseDoc == "undefined") {
                            updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td>" + gfromDate + "</td><td>" + gtoDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td><a href='/Home/ViewFilePath?FilePath=" + supportingdocpath + "' target='_blank' style='text-decoration: underline; color: #0056b3;'>Click Here</a></td><td></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimburse + "</td><td hidden>'" + supportingdocpath + "'</td><td hidden>'" + reimbursedocpath + "'</td></tr>";
                        }
                        else if (typeof supportingDoc == "undefined" && typeof editreimburseDoc != "undefined") {
                            updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td>" + gfromDate + "</td><td>" + gtoDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td></td><td><a href='/Home/ViewFilePath?FilePath=" + reimbursedocpath + "' target='_blank' style='text-decoration: underline; color: #0056b3;'>Click Here</a></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimburse + "</td><td hidden>'" + supportingdocpath + "'</td><td hidden>'" + reimbursedocpath + "'</td></tr>";
                        }
                        else {
                            updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td>" + gfromDate + "</td><td>" + gtoDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td><a href='/Home/ViewFilePath?FilePath=" + supportingdocpath + "' target='_blank' style='text-decoration: underline; color: #0056b3;'>Click Here</a></td><td><a href='/Home/ViewFilePath?FilePath=" + reimbursedocpath + "' target='_blank' style='text-decoration: underline; color: #0056b3;'>Click Here</a></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimburse + "</td><td hidden>'" + supportingdocpath + "'</td><td hidden>'" + reimbursedocpath + "'</td></tr>";
                        }

                        $("#divMedLeave tr[data-row-number=" + rowNumber + "]").replaceWith(updatedRow);
                        $("#mdUpdateMedicalLeave").modal("hide");

                        CalculateMedicalLeave();
                        alertify.success("Medical leave details updated successfully");
                    }, error: function (result) {
                        alertify.error("Could not update medical leave details");
                    }
                });
            }, error: function (result) {
                alertify.error("Could not upload medical leave document");
            }
        });
    }
}

function ResetEditMedLeaveFields() {
    $("#txtEditMedFromDate").val("");
    $("#txtEditMedToDate").val("");
    $("#txtEditMedTotalLeaves").val("");
    $("#txtEditMedRemarks").val("");
    $("#flEditMedSupportingDoc").val("");
    $("#flEditReimburseSupportingDoc").val("");
    $(".diveditreimbursedoc").attr("hidden", true);
    $('.mederr').attr("hidden", true);
}

function ValidateEditMedLeaveFields() {
    var dateFromString = $("#txtEditMedFromDate").val();
    var dateToString = $("#txtEditMedToDate").val();
    if (dateFromString === "") {
        $('#errEditFromDate').text('Please enter leave start date');
        $('#errEditFromDate').removeAttr("hidden");
        return false;
    }
    if (dateToString === "") {
        $('#errEditToDate').text('Please enter leave end date');
        $('#errEditToDate').removeAttr("hidden");
        return false;
    }
    var isreimbursed = $(".rd-btn-editreimburse:checked").val();

    if (isreimbursed == "False") {
        reimburseDoc = undefined;
    }
    if (isreimbursed === "True") {
        //if ($("#flEditReimburseSupportingDoc").val().length == 0) {
        if ($("#hdnMedSupportingDoc").val().length == 0) {
            $("#errEditReimbursedoc").text("Please upload document for medical reimbursed");
            $("#errEditReimbursedoc").removeAttr("hidden");
            return false;
        }
        else {
            $("#errEditReimbursedoc").text("");
            $("#errEditReimbursedoc").attr("hidden", true);
        }

    }
}

function DocFileChange(id, errLbl) {
    debugger;
    if ($("#" + id).get(0).files[0] != undefined) {
        var fileType = $("#" + id).get(0).files[0].type;
        if (fileType == 'application/pdf') {
            if (id == "flEditMedSupportingDoc") {
                $("#hdnMedSupportingDoc").val($("#" + id).get(0).files[0].name);
            } else {
                $("#hdnReimbursedDocument").val($("#" + id).get(0).files[0].name);
            }
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

function disabledate(rowNumber) {
    if (rowNumber !== 0) {
        var $row = $("#divMedLeave tr[data-row-number='" + rowNumber + "']");
        var startdate = $row.find("TD").eq(0).html();
        var enddate = $row.find("TD").eq(1).html();
        var fromdaterow = startdate.split('-');
        var fdate = (fromdaterow[1] + '-' + fromdaterow[0] + '-' + fromdaterow[2]);
        var FromDate = new Date(fdate);
        var todaterow = enddate.split('-');
        var tdate = (todaterow[1] + '-' + todaterow[0] + '-' + todaterow[2]);
        var ToDate = new Date(tdate);
        while (FromDate <= ToDate) {
            var fd = FromDate.getDate();
            var fm = FromDate.getMonth() + 1;
            var fy = FromDate.getFullYear();
            var fromdateString = (fd <= 9 ? '0' + fd : fd) + '-' + (fm <= 9 ? '0' + fm : fm) + '-' + fy;
            dates.push(fromdateString);

            fd = FromDate.getDate() + 1
            fromdateString = (fd <= 9 ? '0' + fd : fd) + '-' + (fm <= 9 ? '0' + fm : fm) + '-' + fy;
            var g = (fm <= 9 ? '0' + fm : fm) + '-' + (fd <= 9 ? '0' + fd : fd) + '-' + fy;
            FromDate = new Date(g);
        }

    }

    else {
        if ($("#divMedLeave").find(".dataTables_empty").length !== 1) {
            $("#tblMedicalLeaveDetails tbody tr").each(function () {
                var row = $(this);
                var fromdaterow = row.find("TD").eq(0).html().split('-');
                var fdate = (fromdaterow[1] + '-' + fromdaterow[0] + '-' + fromdaterow[2]);
                var FromDate = new Date(fdate);
                var todaterow = row.find("TD").eq(1).html().split('-');
                var tdate = (todaterow[1] + '-' + todaterow[0] + '-' + todaterow[2]);
                var ToDate = new Date(tdate);
                while (FromDate <= ToDate) {
                    var fd = FromDate.getDate();
                    var fm = FromDate.getMonth() + 1;
                    var fy = FromDate.getFullYear();
                    var fromdateString = (fd <= 9 ? '0' + fd : fd) + '-' + (fm <= 9 ? '0' + fm : fm) + '-' + fy;
                    dates.push(fromdateString);

                    fd = FromDate.getDate() + 1
                    fromdateString = (fd <= 9 ? '0' + fd : fd) + '-' + (fm <= 9 ? '0' + fm : fm) + '-' + fy;
                    var g = (fm <= 9 ? '0' + fm : fm) + '-' + (fd <= 9 ? '0' + fd : fd) + '-' + fy;
                    FromDate = new Date(g);
                }
            });
        }
    }


}

function setfromandtodate() {
    $('#txtMedFromDate').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        minDate: minDate,
        maxDate: today,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        //beforeShowDay: function (date) {
        //    var string = jQuery.datepicker.formatDate('dd-mm-yy', date);
        //    return [dates.indexOf(string) == -1]
        //},
        closeOnDateSelect: true
    });

    $('#txtMedToDate').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        minDate: minDate,
        maxDate: today,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        //beforeShowDay: function (date) {
        //    var string = jQuery.datepicker.formatDate('dd-mm-yy', date);
        //    return [dates.indexOf(string) == -1]
        //},
        closeOnDateSelect: true
    });
}

function EnglishToKannada() {
    var _knLan = $('.knlan');
    var _EnLan = $('.Enlan');
    if (localStorage.ChangeLang == '0') {
        _EnLan.hide();
        _knLan.show();
        $('#changeLan').val('A');
    }
    else {
        _knLan.hide();
        _EnLan.show();
        $('#changeLan').val('ಕ');
    }
}