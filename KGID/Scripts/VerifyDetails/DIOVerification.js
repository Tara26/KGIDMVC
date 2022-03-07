var ColOneLoaded = false;
var ColTwoLoaded = false;
var ColThreeLoaded = false;
var ColFourLoaded = false;
var ColFiveLoaded = false;
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
$(document).ready(function () {
    $("#tblCWApprovedData").DataTable({
        paging: false,
        info: false,
        searching: false,
        "ordering": false
    });
    $("#spnEName").text($("#spnProposerName").text());
    $("#spnRNo").text($("#hdnBReferanceNo").val());
    var RefNumber = $("#spnBReferanceNo").text();
    if (RefNumber != null) {
        $("#spnKReferanceNo").text(RefNumber);
        $("#spnFReferanceNo").text(RefNumber);
        $("#spnNReferanceNo").text(RefNumber);
        $("#spnPReferanceNo").text(RefNumber);
        $("#spnDReferanceNo").text(RefNumber);

        $("#spnHBReferanceNo").text(RefNumber);
        $("#spnHHPReferanceNo").text(RefNumber);
        $("#spnHOReferanceNo").text(RefNumber);
        $("#spnHHDReferanceNo").text(RefNumber);
        $("#spnHDDReferanceNo").text(RefNumber);
        $("#spnHDReferanceNo").text(RefNumber);

        $("#spnPayscale").text($("#txtPayscleCode").val());
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
                $("#lblBackToEmployee").hide();
                $("#lblSendBackToDDO").hide();
                $("#lblSendBackToCW").hide();
                $("#lblSendBackToSup").hide();
            } else {
                $("#lblBackToEmployee").show();
                $("#lblSendBackToDDO").show();
                $("#lblSendBackToCW").show();
                $("#lblSendBackToSup").show();
                $("#divForwardToDD").hide();
                //$("#divHealthOpinion1").hide();
            }
            showHealthOpinion();
        }
    })
    if ($("#txtSumAssured").val() === "") {
        $("#btnDIOSubmit").text("Send");
        $("#divApprove").hide();
    }
    else {
        if ((parseFloat($("#txtSumAssured").val()) > parseFloat(1500000.00))) {
            $("#btnDIOSubmit").text("Send");
            $("#divApprove").hide();
        }

        else if ($("input[name='ApplicationStatus']:checked").val() === "15") {
            $("#btnDIOSubmit").text("Accept");
            $("#divApprove").show();
        }
        else {
            $("#btnDIOSubmit").text("Send");
            $("#divApprove").show();
        }
    }

    $(".nav-item").click(function (e) {
        $("#viewSidebar")[0].style.width = "0";
        $("#applicationFormTab")[0].style.marginRight = "0";
    });

    //if ($(".ho-doc").length > 0) {
    //    $("#divHealthOpinion1").hide();
    //}
    showHealthOpinion();
    $("#divMedLeaveDetails").hide();
    $("#divMedLeave").show();
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

$("#btnDIOSubmit").click(function (e) {
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
    if ($("#ddlRemarks").val() === "" || $("#ddlRemarks").val() == null) {
        $("#errRemarksReq").show();
        isSendBack = false;
    }
    var qnText = '';
    if ($("input[name='ApplicationStatus']:checked").val() === "2")
        qnText = "Are you sure you want to send back application to Employee?"
    else if ($("input[name='ApplicationStatus']:checked").val() === "4")
        qnText = "Are you sure you want to send back application to DDO?"
    else if ($("input[name='ApplicationStatus']:checked").val() === "6")
        qnText = "Are you sure you want to send back application to Caseworker?"
    else if ($("input[name='ApplicationStatus']:checked").val() === "8")
        qnText = "Are you sure you want to send back application to Superintendent?"
    else if ($("input[name='ApplicationStatus']:checked").val() === "11")
        qnText = "Are you sure you want to forward application to Deputy Director?"
    else if ($("input[name='ApplicationStatus']:checked").val() === "14")
        qnText = "Are you sure you want to take health opinion?"
    else if ($("input[name='ApplicationStatus']:checked").val() === "15")
        qnText = "Are you sure you want to approve the application?"

    var respText = '';
    if ($("input[name='ApplicationStatus']:checked").val() === "2")
        respText = "Application send back to employee"
    else if ($("input[name='ApplicationStatus']:checked").val() === "4")
        respText = "Application send back to DDO"
    else if ($("input[name='ApplicationStatus']:checked").val() === "6")
        respText = "Application send back to Caseworker"
    else if ($("input[name='ApplicationStatus']:checked").val() === "8")
        respText = "Application send back to Superintendent"
    else if ($("input[name='ApplicationStatus']:checked").val() === "11")
        respText = "Application forwarded to Deputy Director"
    else if ($("input[name='ApplicationStatus']:checked").val() === "14")
        respText = "Application sent for health opinion"

    if (isStatusSelected && isCheckboxchecked && isSendBack) {
        debugger
        e.preventDefault();
        bootbox.confirm(qnText, function (btnresult) {
            if (btnresult == true) {
                var formData = new FormData($("#frmDIOVerify").get(0));
                //e.preventDefault();
                //$(e.target).css('pointer-events', 'none');
                //$('.ajs-button ajs-ok').attr('readonly', true);
                //$('.ajs-button ajs-ok').css("pointer-events", "none");
                //$('.ajs-footer').css("pointer-events", "none");
                //$('.ajs-primary ajs-buttons').css("pointer-events", "none");
                $.ajax({
                    url: '/SaveDIOVData/',
                    data: formData,
                    async: false,
                    type: 'POST',
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        if (result.PolicyNumber === "" && result.Result != "7") {
                            alertify.alert(respText, function () {
                                window.location.href = result.RedirectUrl;
                            }).setHeader("Attention");
                        }
                        else if (result.Result == "7") {
                            alertify.alert("There is no Deputy Director mapped for this district.", function () {
                                window.location.href = "/kgid-dio/";
                            }).setHeader("Warning");
                        }
                        else {
                            ///////////////////////////////////
                            alertify.alert("Policy Generated", "Policy has been generated successfully and policy number is " + result.PolicyNumber, function () {
                                window.location.href = result.RedirectUrl;
                            }).setHeader("Attention");
                        }
                    }, error: function (result) {
                        alertify.error("Could not save verified details");
                    }
                });
            }
        });
    }

    e.preventDefault();
});

$("input[name='ApplicationStatus']").change(function () {
    $("#hdnApplicationStatus").val($("input[name='ApplicationStatus']:checked").val());
    if ($("input[name='ApplicationStatus']:checked").val() === "2" || $("input[name='ApplicationStatus']:checked").val() === "4" || $("input[name='ApplicationStatus']:checked").val() === "6" || $("input[name='ApplicationStatus']:checked").val() === "8" || $("input[name='ApplicationStatus']:checked").val() === "11") {
        $("#btnDIOSubmit").text("Send");
    }
    else if ($("input[name='ApplicationStatus']:checked").val() === "14") {
        $("#btnDIOSubmit").text("Send");
    }
    else {
        $("#btnDIOSubmit").text("Accept");
    }
});

//$("#btnDIOApprove").click(function () {
//    alertify.success("Application approved successfully");
//});



function printApplicationForm() {
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Basic Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintBasicDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPersonal").html() + "<hr/>";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html();
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
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Payment Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPaymentDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Payment Status</h2></div><div class="form-group col-4"></div></div>' + $("#divPaymentStatus").html() + "<hr/>";
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

    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Basic Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHBasicDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Physical Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHPhysicalDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Other Details</h3></div><div class="form-group col-4"></div></div>' + $("#divOtherDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Health Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHealthDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Doctor Details</h3></div><div class="form-group col-4"></div></div>' + $("#divDoctorDetails").html(); + "<hr/>";
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
function ChangeVerificationStatus(destid, srcid) {
    var verificationstatus = $("#" + srcid).prop("checked");
    $("#" + destid).prop("checked", verificationstatus);
}
$("#btnDIOClear").click(function (e) {
    $("#ddlRemarks").val("");
    $("#txtComments").val("");
    $("#txtDLoadFactor").val("");
    $("#rdBtnSendBackToDDO").prop("checked", false);
    $("#rdBtnSendBackToSup").prop("checked", false);
    $("#rdBtnForwardToDD").prop("checked", false);
    $(".verify").prop("checked", false);
    e.preventDefault();
})
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
        //$.ajax({
        //    url: '/ViewDataToVerify/BasicDetailsToView',
        //    data: JSON.stringify({}),
        //    type: 'POST',
        //    async: false,
        //    contentType: 'application/json; charset=utf-8',
        //    success: function (data) {
        //        $("#divBD").html(data);
        //    }
        //});
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
        });
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
        });
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
            data: JSON.stringify({ EmpId: empid, AppId: appid }),
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
        });
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
        });
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
        });
        ColFiveLoaded = true;
    }
})
$("#ddlRemarks").change(function () {
    debugger;
    $.ajax({
        type: 'POST',
        url: '/VerifyDetails/GetRemarkComments',
        data: JSON.stringify({ 'RemarkID': this.selectedOptions[0].value }),
        async: false,
        contentType: 'application/json; charset=utf-8',
        processData: false,
        cache: false,
        success: function (data) {
            debugger
            $("#txtComments").val(data);
            if ($("#ddlRemarks option:selected").text() == "No Correction Found") {
                $("#lblBackToEmployee").hide();
                $("#lblSendBackToDDO").hide();
                $("#lblSendBackToCW").hide();
                $("#lblSendBackToSup").hide();
                $("#divForwardToDD").show();

                //$("#divHealthOpinion1").show();
                if ((parseFloat($("#txtSumAssured").val()) <= parseFloat(1500000.00)))
                    $("#divApprove").show();
            } else {
                $("#lblBackToEmployee").show();
                $("#lblSendBackToDDO").show();
                $("#lblSendBackToCW").show();
                $("#lblSendBackToSup").show();
                $("#divApprove").hide();
                $("#divForwardToDD").hide();
                //$("#divHealthOpinion1").hide();
            }
            //if ($(".ho-doc").length > 0) {
            //    $("#divHealthOpinion1").hide();
            //}
            showHealthOpinion();
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
});
$("#btnAFNext").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").addClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").addClass("active");
    $("#nav-tab-Documents").removeClass("active");
});
$("#btnSPrevious").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").addClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").addClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").removeClass("active");
});
$("#btnAFPrevious").click(function (e) {
    $("#workflow").addClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").addClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").removeClass("active");
});
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

function showHealthOpinion() {
    if ($("div:contains('Previously uploaded Document(DHO).')").length > 0) {
        $("#divHealthOpinion1").hide();
    } else {
        $("#divHealthOpinion1").show();
    }
}