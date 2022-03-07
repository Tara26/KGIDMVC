$(document).ready(function () {
    GetModuleList();
    GetProblemList();
});

function GetModuleList() {
    $.ajax({
        url: "/TicketingTool/LoadModuleList",
        // data: JSON.stringify({ "VehTypeid": vehTypeid, "VehSubTypeid": vehSubTypeid }),
        type: 'Get',
        async: false,
        cache: false,
        //contentType: 'application/json; charset=utf-8',
        processData: false,
        //success: function (data) {
        //    debugger;
        //    var optionhtml1 = '<option value="' +
        //        0 + '">' + "--Select Module--" + '</option>';
        //    $(".ddlModuletype").append(optionhtml1);

        //    $.each(data, function (i) {
        //        debugger;
        //        var optionhtml = '<option value="' +
        //            data[i].mt_module_type + '">' + data[i].mt_desc + '</option>';
        //        $(".ddlModuletype").append(optionhtml);
        //    });
        //    $('.ddlModuletype').prop('disabled', false);
        //},
        success: function (response) {
            if (response.length > 0) {
                //$('#ddlVehCatType').prop("disabled", false);
                //$('#errVDVCTypeReq').attr('hidden', true);

                var s = '<option value="-1">-- Select Module --</option>';
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].mt_module_type + '">' + response[i].mt_desc + '</option>';
                }
                $("#ddlModuletype").html(s);

            }

        },
        error: function (response) {
            alertify.error("Could not save");
        }
    });
}

function GetProblemList() {
    $.ajax({
        url: "/TicketingTool/LoadProblemList",
        type: 'Get',
        async: false,
        cache: false,
        processData: false,
        success: function (response) {
            if (response.length > 0) {
                var s = '<option value="-1">-- Select Problem --</option>';
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].pt_id + '">' + response[i].pr_description + '</option>';
                }
                $("#ddlProblemtype").html(s);
            }
        },
        error: function (response) {
            alertify.error("Could not save");
        }
    });
}


function savereportedproblem() {
    var result = true;
    $(".err").hide();
    var Moduletype = $('select#ddlModuletype option:selected').val();
    var Problemtype = $('select#ddlProblemtype option:selected').val();
    var complaintdesc = $('#txtcomplaintdesc').val();
    var postedFile = $('#postedFile').val();
    if (Moduletype == -1) {
        $(".div_Moduletype").closest('div.row_BasicDetails').find('.err').show();
        //$(".div_Moduletype").each(function () {
        //    $(".div_Moduletype").find(".err").show();
        //});
        result = false;
    }

    if (Problemtype == -1) {
        $(".ddlProblemtype").closest('div.row_BasicDetails').find('.err').show();


        result = false;

    }
    else {
        $(".ddlProblemtype").closest('div.row_BasicDetails').find('.err').hide();

    }
    if (complaintdesc.toString().length == 0) {

        $("#txtcomplaintdesc").closest('div.row_BasicDetails').find('.err').show();


        result = false;
    }

    //if (postedFile.toString().length == 0) {
    //    $("#postedFile").closest('div.row_BasicDetails').find('.err').show();
    //    result = false;
    //}
    if (result == true) {
        var model = new FormData();

        if (postedFile.toString().length > 0) {
            var files = $("#postedFile").get(0).files;
            model.append("rp_upload_document", files[0]);
        }
        model.append("rp_module_id", $('select#ddlModuletype option:selected').val());
        model.append("rp_problem_type_id", $('select#ddlProblemtype option:selected').val());
        model.append("rp_complaint_description", $("#txtcomplaintdesc").val());
        model.append("rp_complaint_description", $("#txtcomplaintdesc").val());
        //model.append("rp_upload_document", files[0]);

        //for (var i = 0; i < files.length; i++) {
        //    model.append(files[i].name, files[i]);
        //}

        $.ajax({
            url: '/TicketingTool/SaveReportedProblem',
            //data: JSON.stringify(viewModel),
            data: model,

            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                /* var r = GetFamilyDetails();
                 if(!(r<0))*/
                alertify.success("Problem Reported successfully");
                //FserverResponse = true;
            }, error: function (result) {
                //alertify.error("Problem Reported successfully");
            }
        });
    }
    return result;
}

function GetreportedproblemsBasedonFilter() {
    var mid = $('select#ddlModuletype option:selected').val();
    var fstatus = $('select#ddlfStatus option:selected').text();
    var fdate = $("#txtFromDate").val();
    var tdate = $("#txtToDate").val();
    $.ajax({
        url: "/TicketingTool/GetReportProblemsBasedonFilters?mid=" + mid + "&fdate=" + fdate + "&tdate=" + tdate,
        dataType: "json",
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {

        }, error: function (result) {
            alertify.error("Problem Reported Failed to load ");
        }
    });
}

var TeamDetailPostBackURL = '/TicketingTool/GetDetailsById';
//function UpdateReportedproblem() {
$(function () {
    $(".anchorDetail").click(function () {
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        //var TSTS = $("#hdnProblemStatus").val();
        $.ajax({
            type: "GET",
            url: TeamDetailPostBackURL,
            contentType: "application/json; charset=utf-8",
            data: { "Id": id },
            datatype: "json",
            success: function (data) {
                $('#mdl_content').html(data);
                EnglishToKannada();
                //$('#myModal').modal('show');

            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });

    $(".GetAllanchorDetail").click(function () {
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        $.ajax({
            type: "GET",
            url: TeamDetailPostBackURL,
            contentType: "application/json; charset=utf-8",
            data: { "Id": id, "STS": 2 },
            datatype: "json",
            success: function (data) {
                $('#mdl_content').html(data);
                EnglishToKannada();
                //$('#myModal').modal('show');

            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
/* Super Admin Assign Tickets To Helpdesk */
    $(".csAssignTicket").click(function () {
        debugger;

        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        $.ajax({
            type: "GET",
            url: "/sa-assign-tickets",
            contentType: "application/json; charset=utf-8",
            data: { "Id": id, "STS": 2 },
            datatype: "json",
            success: function (data) {
                debugger;
                $('#mdl_content').html(data);
                EnglishToKannada();
                //$('#myModal').modal('show');

            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
});

function UpdateReportedproblem1() {
    var viewModel = {
        'rp_report_problem_status': $('#ddlTicketStsList').val(),
        'rp_remarks': $("#txtComments").val(),
        'rp_id': $("#hdnrp_id").val()
    };
    $.ajax({
        url: '/hd-issue-update',
        data: JSON.stringify(viewModel),
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            /* var r = GetFamilyDetails();
             if(!(r<0))*/
            alertify.success("Updated  successfully");
            //FserverResponse = true;
        }
    });
}
function file_validation(file) {
    $(".err-postedFile").hide();
    var count = 0;
    myfile = $(file).val().toLowerCase();

    var ext = myfile.split('.').pop();

    if (ext == "png" || ext == "jpg" || ext == "jpeg" || ext == "gif") {
        //const fi = document.getElementById(file);
        // Check if any file is selected.
        //var FileSize = file.files[0].size / 1024 
        //{
        //    alert(FileSize);

        //}

        if (file.files.length > 0) {
            var FileSize = file.files[0].size
            {
                const file = Math.round((FileSize / 1024));
                // The size of the file.

                if (FileSize > 4766573) {
                    count = 1;
                }
            }
            if (count == 1) {
                $(file).val('');
                $("#postedFile").closest('div.row_BasicDetails').find('.err-postedFile').show();

            }
        }
    }
    else {
        $("#postedFile").closest('div.row_BasicDetails').find('.err-postedFile').show();
        $(file).val('');

    }
}
$(document).ready(function () {
    $("#txtToDate").change(function () {
        var startDate = document.getElementById("txtFromDate").value;
        var endDate = document.getElementById("txtToDate").value;

        if ((Date.parse(startDate) > Date.parse(endDate))) {
            document.getElementById('errddToDate').style.display = "";
            $("#errddToDate").addClass("alert alert-danger");
            document.getElementById('errddToDate').innerHTML = "End date should be greater than Start date";

            document.getElementById("txtToDate").value = "";
        }
        else {
            $("#errddToDate").addClass("alert alert-danger");
            $("#errddToDate").removeClass("alert alert-danger");
            document.getElementById('errddToDate').innerHTML = "";

        }
    });
});
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

function AssignTicket() {
    debugger;
    $(".err").hide();
    if ($('#ddlAssignedto').val() == "") {
        $("#errddlAssignedto").show();
        return false;
    }
    else {
        var viewModel = {
            'rp_assignedto': $('#ddlAssignedto').val(),
            'rp_id': $("#hdnrp_id").val()
        };
        $.ajax({
            url: '/sa-assign-to-helpdesk',
            data: JSON.stringify(viewModel),
            async: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                /* var r = GetFamilyDetails();
                 if(!(r<0))*/
                alertify.success("Updated  successfully");
                //FserverResponse = true;
            }
        });
    }
}