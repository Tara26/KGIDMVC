var table;
$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const myParam = urlParams.get('excelUploaded');
    var uploadData = "fail";
    uploadData = myParam;
    console.log(uploadData);
    var uri = window.location.toString();
    if (uri.indexOf("?") > 0) {
        var clean_uri = uri.substring(0, uri.indexOf("?"));
        window.history.replaceState({}, document.title, clean_uri);
    }
    
    if ($("#hdnUploadMsg").val() != "" && $("#hdnUploadMsg").val() != null && $("#hdnUploadMsg").val() != undefined) {
        ShowUploadOutput($("#hdnUploadMsg").val());
    }

    table = $('#tblUploadedEmployeeDetails').DataTable({
        "ajax": {
            "LengthMenu": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11],
            "url": $('#tblUploadedEmployeeDetails').data('url'),
            "type": "GET",
            "datatype": "json",
            "dataSrc": "",
            "serverprocessing": false,
            "paging": true,
            "retrieve": true,
        },
        "columns": [
            { "data": "creation_datetime", "name": "creation_datetime", "autoWidth": true },
            { "data": "employee_id", "name": "employee_id", "autoWidth": true },
            { "data": "employee_name", "name": "employee_name", "autoWidth": true },
            {
                "data": "date_of_birth", "name": "dateofbirth", "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return date.getDate() + "-" + (month.toString().length > 1 ? month : "0" + month) + "-" + date.getFullYear();
                }
            },
            { "data": "gender_desc", "name": "gender", "autoWidth": true },
            { "data": "mobile_number", "name": "mobile_number", "autoWidth": true },
            { "data": "email_id", "name": "email_id", "autoWidth": true, },
            { "data": "father_name", "name": "father_name", "autoWidth": true },
            { "data": "department", "name": "DeptName", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return '<a href="javascript:void(0);" class="btn-sm btn-secondary" onclick="EditEmployee(' + data.employee_id + ');">Edit</a>&nbsp<a href="javascript:void(0);" class="btn-sm btn-danger" onclick="DeleteEmployee(' + data.employee_id + ');">Delete</a>';
                },
            }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        'columnDefs': [{
            'targets': [0],
            'visible': false,
        }]
    });
});

$("#btnupload").click(function () {
    $(".err").attr("hidden", true);
    if ($("#postedFile").val().length == 0) {
        $("#lblUploadErr").removeAttr("hidden");
    }
    if ($(".err:visible").length == 0) {
        $("#frmUploadExcelData").submit();
    }
})

$("#btnUploadClose,#btnmodalUploadClose").click(function () {
    if ($('#hUploadResult')[0].innerHTML == "Employee data deleted successfully")
        window.location.href = '/Upload/UploadEmployeeDetails';
})

function EditEmployee(empId) {
    debugger;
    $(".err").attr('hidden');
    $("#ddlewd_permanent_temporary").val("1");
    var url = $("#tblUploadedEmployeeDetails").data("edit-url");
    url = url + "?empId=" + empId;
    $.get(url).done(function (data) {
        $("#modalPopup").find(".modal-body").html(data);
        $("#modalPopup").find(".modal-header").find("h6").html("Edit employee details");
        $("body").addClass("modal-open");
        $("#modalPopup").show();
        EnglishToKannada();
    });
    
}

function DeleteEmployee(empId, e) {
    alertify.confirm("Are you sure you want to remove this employee data?", function () {
        var url = $("#tblUploadedEmployeeDetails").data("delete-url");
        $.post(url, { empId: empId }).done(function (response) {
            if (response == "1") {
                $('#myUploadModal').modal('show');
                $('#hUploadResult')[0].innerHTML = "Employee data deleted successfully";
                $("#btnUploadClose")[0].className = "btn btn-default btn-danger";
            } else {
                alertify.error("Error occured while deleting employee data");
            }
        });

    }).setHeader("Warning");
    
}
//$(".disable-keyboard").on("keypress", function (e) {
//    debugger;
//    return false;
//});
$(".modal-save").click(function () {
    debugger;
    $(".err").attr('hidden', true);
    //var dob = new Date($("#txtDateOfBirth").val());
    //var today = new Date();
    //var age = Math.floor((today - dob) / (365.25 * 24 * 60 * 60 * 1000));
    //if (age <17) {
    //    $('#errDOB').text('Age of employee should be more than 18 years.');
    //    $('#errDOB').removeAttr("hidden");
    //    return;
    //}
    ValidateEmpDOB();
    ValidateEmpDOA();

    var url = $("#divEditEmpPopup").data("save-url");
    //var dateofbir ="2021/04/23" //$("#txtDateOfBirth").val();
    //var dateofapp = $("#txtDateOfAppointment").val();
    //var dateofjoin = $("#txtewd_date_of_joining_post").val();
	    var url = $("#divEditEmpPopup").data("save-url");
	var dob=$("#txtDateOfBirth").val().split("-");
	var doa=$("#txtDateOfAppointment").val().split("-");
	var doj=$("#txtewd_date_of_joining_post").val().split("-");

    if ($("#txtewd_date_of_joining_post").val() == "") {
        $("#errDOJ").removeAttr('hidden'); return false;
    }

     var dateofbir =dob[2]+"/"+dob[1]+"/"+dob[0]; //$("#txtDateOfBirth").val();
     var dateofapp =doa[2]+"/"+doa[1]+"/"+doa[0]; //$("#txtDateOfAppointment").val();
    var dateofjoin = doj[2]+"/"+doj[1]+"/"+doj[0];//$("#txtewd_date_of_joining_post").val();
    var viewModel = {
        'employee_name': $("#txtName").val(), 'father_name': $("#txtFatherName").val(), 'spouse_name': $("#txtSpouseName").val(),
        'gender': $("#ddlGender").val(), 'dateofbirth': dateofbir, 'place_of_birth': $("#txtPlaceOfBirth").val(),
        'pan_number': $("#txtPANNumber").val(), 'mobile_number': $("#txtMobileNumber").val(), 'email_id': $("#txtEmailId").val(),
        'dateofappointment': dateofapp, 'active_status': $("#chkIsActive").is(":checked"), 'ddocode': $("#ddlDDOCode").val(),
        'department': $("#ddlDepartmentCode").val(), 'employee_id': $("#hdnEmpID").val(),
        'ewddateofjoining': dateofjoin, 'payscalecode': $("#ddlewd_payscle_code").val(), 'emptype': $("#ddlewd_permanent_temporary").val(),
        'designation': $("#ddlewd_designation").val(), 'group': $("#ddlewd_group").val(), 'ewd_place_of_posting': $("#txtewd_place_of_posting").val()
        , 'employee_name_kannada': $("#txtkanName").val(), 'father_name_kannada': $("#txtkanFatherName").val(), 'spouse_name_kannada': $("#txtKanSpouseName").val()
    };

    var dateFromString = $("#txtewd_date_of_joining_post").val();
    var dateToString = $("#txtDateOfAppointment").val();
    if (dateFromString !== "" && dateToString !== "") {
        var datFromArray = dateFromString.split("-");
        var dFrom = datFromArray[1] + "-" + datFromArray[0] + "-" + datFromArray[2];
        var dateFrom = new Date(dFrom);
        var datToArray = dateToString.split("-");
        var dTo = datToArray[1] + "-" + datToArray[0] + "-" + datToArray[2];
        var dateTo = new Date(dTo);
        if (dateFrom == "Invalid Date") {
            $('#errDOJ').text('Please enter date in DD/MM/YYYY format');
            $('#errDOJ').removeAttr("hidden");
            return;
        }
        if (dateTo == "Invalid Date") {
            $('#errDOA').text('Please enter date in DD/MM/YYYY format');
            $('#errDOA').removeAttr("hidden");
            return;
        }
        if (dateFrom < dateTo) {
            $('#errDOADOJ').removeAttr("hidden");
            return;
        }
    }
    if (ValidateEmpDetails(viewModel)) {
        if ($('.err:visible').length === 0) {
            $.ajax({
                url: url,
                data: JSON.stringify(viewModel),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    if (response.IsSuccess) {
                        ClosePopup();
                        table.ajax.reload();
                        alertify.success(response.Message);
                    }
                    else {
                        alertify.error(response.Message);
                    }
                }, error: function (result) {
                    alertify.error(result);
                }
            });
        }
    }
});

function ValidateEmpDetails(viewModel) {
    debugger;
    if (viewModel.employee_name == "" || viewModel.employee_name == null) {
        $("#errEmpName").removeAttr('hidden'); return false;
    }
    else if (viewModel.employee_name_kannada == "" || viewModel.employee_name_kannada == null) {
        $("#errKEmpName").removeAttr('hidden'); return false;
    }
    if (viewModel.father_name != "") {
        if (viewModel.father_name_kannada == "") {
            $('#asteriskkF').show();
            $("#errFatherKannada").removeAttr('hidden'); return false;
        }
    }
    else if (viewModel.father_name_kannada != "") {
        if (viewModel.father_name == "" || viewModel.father_name == null) {
            $('#asteriskEF').show();
            $("#errFatherName").removeAttr('hidden'); return false;
        }
    }
    else {
        $('#asteriskkF').hide();
        $('#asteriskEF').hide();
        $("#errFatherKannada").attr('hidden', true);
        $("#errFatherName").attr('hidden', true); //return true;
    }
    if (viewModel.spouse_name != "") {
        if (viewModel.spouse_name_kannada == "" || viewModel.spouse_name_kannada == null) {
            $('#asteriskS').show();
            $("#errSpouseKName").removeAttr('hidden'); return false;
        }
    }
    else if (viewModel.spouse_name_kannada != "") {
        if (viewModel.spouse_name == "" || viewModel.spouse_name == null) {
            $('#asteriskES').show();
            $("#errSpouseEName").removeAttr('hidden'); return false;
        }
    }
    else {
        $('#asteriskS').hide();
        $("#errSpouseKName").attr('hidden', true);
        $('#asteriskES').hide();
        $("#errSpouseEName").attr('hidden', true); //return true;
    }

    if (viewModel.gender == "" || viewModel.gender == null) {
        $("#errGender").removeAttr('hidden'); return false;
    }
    if (viewModel.mobile_number == "" || viewModel.mobile_number == null) {
        $("#errMblNum").removeAttr('hidden'); return false;
    }
    else if (viewModel.mobile_number != "" || viewModel.mobile_number != null) {
        //$("#errEmpName").removeAttr('hidden');
        var input_val = viewModel.mobile_number;
        var inputRGEX = /^(\d{10})$/;
        var inputResult = inputRGEX.test(input_val);
        if (!(inputResult)) {
            $('#errMNumber').show();
            $('#errMNumber').text("Please enter correct mobile number.")
            return false;
        } else {
            if (input_val.charAt(0) == "0") {
                $('#errMNumber').show();
                $('#errMNumber').text("Mobile number cannot start with 0.")
                return false;
            }
            else {
                $('#errMNumber').hide();
            }
        }
        //return true;
    }
    if (viewModel.email_id == "" || viewModel.email_id == null) {
        $("#errMailID").removeAttr('hidden'); return false;
    }
    else if (viewModel.email_id != "" || viewModel.email_id != null) {
        var input_eval = $("#txtEmailId").val();
        var inputeRGEX = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        var inputeResult = inputeRGEX.test(input_eval);
        if (!(inputeResult)) {
            $('#errEMail').show();
            return false;
        } else {
            $('#errEMail').hide();
        }
        //return true;
    }
    if (viewModel.dateofbirth == "" || viewModel.dateofbirth == null) {
        $("#errDOB").removeAttr('hidden'); return false;
    }
    if (viewModel.place_of_birth == "" || viewModel.place_of_birth == null) {
        $("#errPOB").removeAttr('hidden'); return false;
    }
    if (viewModel.department == "" || viewModel.department == null) {
        $("#errDeptCode").removeAttr('hidden'); return false;
    }
    if (viewModel.ddocode == "" || viewModel.ddocode == null) {
        $("#errDDOCode").removeAttr('hidden'); return false;
    }
    if (viewModel.dateofappointment == "" || viewModel.dateofappointment == null) {
        $("#errDOA").removeAttr('hidden'); return false;
    }
    if (viewModel.pan_number == "" || viewModel.pan_number == null) {
        $("#errPANNum").removeAttr('hidden');return false;
    }
    else if (viewModel.pan_number != "" || viewModel.pan_number != null) {
        debugger;
        var input_pval = viewModel.pan_number;
        var inputpRGEX = /[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
        var inputpResult = inputpRGEX.test(input_pval);
        if (!(inputpResult)) {
            $('#errPNum').show();
            return false;
        } else {
            $('#errPNum').hide();
        }
        //return true;
    }
    if (viewModel.ewddateofjoining == "" || viewModel.ewddateofjoining == null) {
        $("#errDOJ").removeAttr('hidden'); return false;
    }
    if (viewModel.payscalecode == "" || viewModel.payscalecode == null) {
        $("#errPSCode").removeAttr('hidden');return false;
    }
    if (viewModel.emptype == "" || viewModel.emptype == null) {
        $("#errEmpType").removeAttr('hidden'); return false;
    }
    if (viewModel.designation == "" || viewModel.designation == null) {
        $("#errDesg").removeAttr('hidden'); return false;
    }
    if (viewModel.group == "" || viewModel.group == null) {
        $("#errEmpGrp").removeAttr('hidden'); return false;
    }
    if (viewModel.ewd_place_of_posting == "" || viewModel.ewd_place_of_posting == null) {
        $("#errPOP").removeAttr('hidden'); return false;
    }
    return true;
}

function ValidateEmpDOA(id) {
    debugger;
    var txtDOB = $('#txtDateOfBirth').val();
    var splitdatetxtDOB = txtDOB.split('-');
    var txtDOBdate = splitdatetxtDOB[0];
    var txtDOBmonth = splitdatetxtDOB[1];
    var txtDOByear = splitdatetxtDOB[2];

    var txtDOA = $('#txtDateOfAppointment').val();
    var txtDOAsplitdate = txtDOA.split('-');
    var txtDOAdate = txtDOAsplitdate[0];
    var txtDOAmonth = txtDOAsplitdate[1];
    var txtDOAyear = txtDOAsplitdate[2];

    if ($('#txtDateOfAppointment').val() == "") {
        $('#errDOA').removeAttr("hidden");
        return false;
    }

    if (txtDOB != "") {
        var From_date = new Date(txtDOByear + '-' + txtDOBmonth + '-' + txtDOBdate);
        var To_date = new Date(txtDOAyear + '-' + txtDOAmonth + '-' + txtDOAdate);
        var diff_date = To_date - From_date;

        var years = Math.floor(diff_date / 31536000000);
        var months = Math.floor((diff_date % 31536000000) / 2628000000);
        var days = Math.floor(((diff_date % 31536000000) % 2628000000) / 86400000);
        //        $("#Result").html(years + " year(s) " + months + " month(s) " + days + " and day(s)");
        // alert(years + " year(s) " + months + " month(s) " + days + " and day(s)");
        if (years > 17) {
            if (months > 0) {
                if (days > 0) {
                    $('#errDOA').attr("hidden", true);
                    return true;
                }
            }
            $('#errDOA').attr("hidden", true);
            return true;
        }
        else {
            $('#errDOA').removeAttr("hidden");
            $('#errDOA').text("Appointment cannot be given if age is less than 18.");
            return false;
        }
    }

}

function ValidateEmpDOB(id) {
    var txtDOB = $('#txtDateOfBirth').val();
    var splitdate = txtDOB.split('-');
    var date = splitdate[0];
    var month = splitdate[1] - 1;
    var year = splitdate[2];
    if (txtDOB != "") {
        txtDOB = new Date(year, month, date);
        var today = new Date();
        var age = Math.floor((today - txtDOB) / (365.25 * 24 * 60 * 60 * 1000));
        //  alert(age);
        if (age >= 18) {
            $('#errDOB').attr("hidden", true);
            return true;
        }
        else {
            $('#errDOB').removeAttr("hidden");
            $('#errDOB').text("Age cannot be less than 18.");
            return false;
        }
    }
    else {
        $('#errDOB').removeAttr("hidden");
        return false;
    }
}

$("#a_ddoupload").click(function () {
    var a = $("#a_ddoupload");
    const filename = new Date().getDate() + "_" + new Date().getMonth() + "_" + new Date().getFullYear() + "_" + new Date().getHours() + "_" + new Date().getMinutes() + "_" + new Date().getSeconds();
    a.attr("download", "DDO FILE UPLOAD.xlsx")
    $("#a_ddoupload").click();
})

function ShowUploadOutput(message) {
    $('#myUploadModal').modal('show');
    if (message == "Employee data uploaded successfully") {
        $('#hUploadResult')[0].innerHTML = message;
        $("#btnUploadClose")[0].className = "btn btn-default btn-success";
    }
    else {
        $('#hUploadResult')[0].innerHTML = message;
        $("#btnUploadClose")[0].className = "btn btn-default btn-danger";
    }
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