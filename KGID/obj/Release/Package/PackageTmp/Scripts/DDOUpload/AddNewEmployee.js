$(document).ready(function () {
    $("#ddlewd_permanent_temporary").val("1");
})


$("#btnSave").click(function () {
    debugger;
    $(".err").attr('hidden', true);
    ValidateEmpDOB();
    ValidateEmpDOA();

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
        'employee_name_kannada': $("#txtKName").val(), 'father_name_kannada': $("#txtKFatherName").val(), 'spouse_name_kannada': $("#txtKSpouseName").val(),
        'gender_id': $("#ddlGender").val(), 'dateofbirth': dateofbir , 'place_of_birth': $("#txtPlaceOfBirth").val(),
        'pan_number': $("#txtPANNumber").val(), 'mobile_number': $("#txtMobileNumber").val(), 'email_id': $("#txtEmailId").val(),
        'dateofappointment': dateofapp, 'ewd_ddo_id': $("#ddlDDOCode").val(),
        'dept_employee_code': $("#ddlDepartmentCode").val(), 'employee_id': $("#hdnEmpID").val(),
        'ewddateofjoining': dateofjoin , 'ewd_payscale_id': $("#ddlewd_payscle_code").val(), 'et_employee_type_id': $("#ddlewd_permanent_temporary").val(),
        'd_designation_id': $("#ddlewd_designation").val(), 'ewd_group_id': $("#ddlewd_group").val(), 'ewd_place_of_posting': $("#txtewd_place_of_posting").val()
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
    if (ValidateNewEmpDetails(viewModel)) {
        if ($('.err:visible').length === 0) {
            $.ajax({
                url: "/Employee/AddEmployeeBasicDetails",
                data: JSON.stringify(viewModel),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    if (response.IsSuccess) {
                        alertify.success(response.Message);
                        window.location.href = "/kgid-ddo-upload/";
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



function ValidateNewEmpDetails(viewModel) {
    if (viewModel.employee_name == "" || viewModel.employee_name == null) {
        $("#errEmpName").removeAttr('hidden'); return false;
    }
    else if (viewModel.employee_name_kannada == "" || viewModel.employee_name_kannada == null) {
        $("#errEmpKName").removeAttr('hidden'); return false;
    }
    if (viewModel.father_name != "") {
        if (viewModel.father_name_kannada == "" || viewModel.father_name_kannada == null) {
            $('#asteriskF').show();
            $("#errFatherNameKannada").removeAttr('hidden'); return false;
        }
    }
    else if (viewModel.father_name_kannada != "") {
        if (viewModel.father_name == "" || viewModel.father_name == null) {
            $('#asteriskEF').show();
            $("#errFatherName").removeAttr('hidden'); return false;
        }
    }
    else {
        $('#asteriskF').hide();
        $('#asteriskEF').hide();
        $("#errFatherNameKannada").attr('hidden', true);
        $("#errFatherName").attr('hidden', true); //return true;
    }
    if (viewModel.spouse_name != "") {
        if (viewModel.spouse_name_kannada == "" || viewModel.spouse_name_kannada == null) {
            $('#asteriskS').show();
            $("#errspouseNameKannada").removeAttr('hidden'); return false;
        }
    }
    else if (viewModel.spouse_name_kannada != "") {
        if (viewModel.spouse_name == "" || viewModel.spouse_name == null) {
            $('#asteriskES').show();
            $("#errspouseName").removeAttr('hidden'); return false;
        }
    }
    else {
        $('#asteriskS').hide();
        $("#errspouseNameKannada").attr('hidden', true);
        $('#asteriskES').hide();
        $("#errspouseName").attr('hidden', true);//return true;
    }

    if (viewModel.gender_id == "" || viewModel.gender_id == null) {
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
     if (viewModel.dept_employee_code == "" || viewModel.dept_employee_code == null) {
        $("#errDeptCode").removeAttr('hidden'); return false;
    }
     if (viewModel.ewd_ddo_id == "" || viewModel.ewd_ddo_id == null) {
        $("#errDDOCode").removeAttr('hidden'); return false;
    }
     if (viewModel.dateofappointment == "" || viewModel.dateofappointment == null) {
        $("#errDOA").removeAttr('hidden'); return false;
    }
    if (viewModel.pan_number == "" || viewModel.pan_number == null) {
        $("#errPANNum").removeAttr('hidden'); return false;
    }
    else if(viewModel.pan_number != "" || viewModel.pan_number != null) {
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
     if (viewModel.ewd_payscale_id == "" || viewModel.ewd_payscale_id == null) {
        $("#errPSCode").removeAttr('hidden'); return false;
    }
     if (viewModel.et_employee_type_id == "" || viewModel.et_employee_type_id == null) {
        $("#errEmpType").removeAttr('hidden'); return false;
    }
     if (viewModel.d_designation_id == "" || viewModel.d_designation_id == null) {
        $("#errDesg").removeAttr('hidden'); return false;
    }
     if (viewModel.ewd_group_id == "" || viewModel.ewd_group_id == null) {
        $("#errEmpGrp").removeAttr('hidden'); return false;
    }
     if (viewModel.ewd_place_of_posting == "" || viewModel.ewd_place_of_posting == null) {
        $("#errPOP").removeAttr('hidden'); return false;
    }
    return true;
}

function ValidateEmpDOA(id) {
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
    else {
        $('#errDOA').removeAttr("hidden");
        return false;
    }
}

function ValidateEmpDOB(id) {
    var endingDate = "";
    var txtDOB = $('#txtDateOfBirth').val();
    var splitdate = txtDOB.split('-');
    var date = splitdate[0];
    var month = splitdate[1] - 1;
    var year = splitdate[2];
    if (txtDOB != "") {
        var startDate = new Date(new Date(year + "-" + splitdate[1] + "-" + date).toISOString().substr(0, 10));
        if (!endingDate) {
            endingDate = new Date().toISOString().substr(0, 10);    // need date in YYYY-MM-DD format
        }
        var endDate = new Date(endingDate);
        if (startDate > endDate) {
            var swap = startDate;
            startDate = endDate;
            endDate = swap;
        }
        var startYear = startDate.getFullYear();
        var february = (startYear % 4 === 0 && startYear % 100 !== 0) || startYear % 400 === 0 ? 29 : 28;
        var daysInMonth = [31, february, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        var yearDiff = endDate.getFullYear() - startYear;
        var monthDiff = endDate.getMonth() - startDate.getMonth();
        if (monthDiff < 0) {
            yearDiff--;
            monthDiff += 12;
        }
        var dayDiff = endDate.getDate() - startDate.getDate();
        if (dayDiff < 0) {
            if (monthDiff > 0) {
                monthDiff--;
            } else {
                yearDiff--;
                monthDiff = 11;
            }
            dayDiff += daysInMonth[startDate.getMonth()];
        }

        // return yearDiff + 'Y ' + monthDiff + 'M ' + dayDiff + 'D';


        if (yearDiff >= 18) {
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
$(".disable-keyboard").on("keypress", function (e) {
    return false;
});