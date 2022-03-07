var medSupportingDocs = [];
var today = '';
var minDate = '';
var dates = [];
var rowno = 0;
$(document).ready(function () {
    debugger;
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
    disabledate(0);
    $('#txtMedFromDate').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        autoclose: true,
        minDate: minDate,
        maxDate: today,
        yearStart: today.getFullYear() - 80,
        yearEnd: today.getFullYear(),
        beforeShowDay: function (date) {
            var string = jQuery.datepicker.formatDate('dd-mm-yy', date);
            return [dates.indexOf(string) == -1]
        },
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
        beforeShowDay: function (date) {
            var string = jQuery.datepicker.formatDate('dd-mm-yy', date);
            return [dates.indexOf(string) == -1]
        },
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
        beforeShowDay: function (date) {
            var string = jQuery.datepicker.formatDate('dd-mm-yy', date);
            return [dates.indexOf(string) == -1]
        },
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
        beforeShowDay: function (date) {
            var string = jQuery.datepicker.formatDate('dd-mm-yy', date);
            return [dates.indexOf(string) == -1]
        },
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

$("#txtMedFromDate, #txtMedToDate").change(function () {
    $(".err").attr("hidden", true);
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
    $(".err").attr("hidden", true);
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
    debugger;
    $('.err').attr("hidden", false);
    ValidateMedLeaveFields();
    if ($('.err:visible').length === 0) {
        var fromDate = $("#txtMedFromDate").val();
        var toDate = $("#txtMedToDate").val();
        var checkexistdate = false;
        $("#tblMedicalLeaveDetails TBODY TR").each(function () {
            var row = $(this);
            if (fromDate == row.find("TD").eq(0).html() && toDate == row.find("TD").eq(1).html()) {
                $('#errToDate').text('Already leave applied for the same dates');
                $('#errToDate').removeAttr("hidden");
                checkexistdate = true;
                return;
            }
        });

        if (!checkexistdate) {
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
                    if ($("#divMedLeave").find(".dataTables_empty").length === 1) {
                        $("#divMedLeave").find(".dataTables_empty").parent("tr").remove();
                    }
                    if (rowno == 0)
                        rowno = $("#divMedLeave tr[data-row-number]").length + 1;
                    else
                        rowno = rowno + 1;
                    var newClass;
                    if ($("tr[data-row-number=" + $("#divMedLeave tr[data-row-number]").length + "]").hasClass("odd")) {
                        newClass = "even";
                    } else {
                        newClass = "odd";
                    }
                    
                    $("#hdnRowNumber").val(rowno);
                    var rowNumber = parseInt($("#hdnRowNumber").val());
                    var AddRow = "";
                    if (typeof supportingDoc == "undefined" && typeof reimburseDoc == "undefined") {
                        AddRow = "<tr data-row-number='" + rowno + "' role='row' class='" + newClass + "'><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td></td><td></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimbursed + "</td><td hidden>" + result.split("~")[0] + "</td><td hidden>" + result.split("~")[1] + "</td></tr>";
                    }
                    else if (typeof supportingDoc != "undefined" && typeof reimburseDoc == "undefined") {
                        AddRow = "<tr data-row-number='" + rowno + "' role='row' class='" + newClass + "'><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td>" + supportingDoc.name + "</td><td></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimbursed + "</td><td hidden>" + result.split("~")[0] + "</td><td hidden>" + result.split("~")[1] + "</td></tr>";
                    }
                    else if (typeof supportingDoc == "undefined" && typeof reimburseDoc != "undefined") {
                        AddRow = "<tr data-row-number='" + rowno + "' role='row' class='" + newClass + "'><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td></td><td>" + reimburseDoc.name + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimbursed + "</td><td hidden>" + result.split("~")[0] + "</td><td hidden>" + result.split("~")[1] + "</td></tr>";
                    }
                    else {
                        AddRow = "<tr data-row-number='" + rowno + "' role='row' class='" + newClass + "'><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td>" + supportingDoc.name + "</td><td>" + reimburseDoc.name + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimbursed + "</td><td hidden>" + result.split("~")[0] + "</td><td hidden>" + result.split("~")[1] + "</td></tr>";
                    }
                    $("#tblMedicalLeaveDetails tbody").append(AddRow);
                    disabledate(rowno);
                    alertify.success("Medical leave details added successfully");
                }
            });

            ResetMedLeaveFields();
        }
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
    $('.err').attr("hidden", true);
    $("#rbtnYMed").prop("checked", false);
    $("#rbtnNMed").prop("checked", true);
    $(".rd-btn-reimburse:checked").val("False");
}

function ValidateMedLeaveFields() {
    $('.err').attr("hidden", true);
    var dateFromString = $("#txtMedFromDate").val();
    var dateToString = $("#txtMedToDate").val();

    if (dateFromString === "") {
        $('#errFromDate').text('Please enter leave start date');
        $('#errFromDate').removeAttr("hidden");
    }

    if (dateToString === "") {
        $('#errToDate').text('Please enter leave end date');
        $('#errToDate').removeAttr("hidden");
    }
    var isreimbursed = $(".rd-btn-reimburse:checked").val();

    if (isreimbursed == "False") {
        reimburseDoc = undefined;
    }
    if (isreimbursed == "True") {
        if ($("#flReimburseSupportingDoc").val().length == 0) {
            $("#errReimbursedoc").text("Please upload document for medical reimbursed");
            $("#errReimbursedoc").removeAttr("hidden");
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

//Delete Medical Leave
function DeleteMedLeaveRow(rowNumber) {

    if ($("#divMedLeave").find("tbody").find("tr").length === 0) {
        var AddRow = "<tr class='odd'><td valign='top' colspan='8' class='dataTables_empty'>No data available in table</td></tr>";
        $("#tblMedicalLeaveDetails tbody").append(AddRow);
    }
    //enable deleted dates 
    var $row = $("#divMedLeave tr[data-row-number='" + rowNumber + "']");
    var startdate = $row.find("TD").eq(0).html();
    var enddate = $row.find("TD").eq(1).html();
    var a = dates.indexOf(startdate);
    var b = dates.indexOf(enddate);
    dates.splice(a, b - a + 1);
    $("#divMedLeave tr[data-row-number='" + rowNumber + "']").remove();

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
    $('.err').attr("hidden", true);
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
    if ($row.find("TD").eq(4).html() != null && $row.find("TD").eq(4).html() != undefined && $row.find("TD").eq(4).html() != "") {
        var b = new ClipboardEvent("").clipboardData || new DataTransfer();
        var files = [
            new File(['content'], $row.find("TD").eq(4).html())
        ];
        $("#flEditMedSupportingDoc").get(0).files = new FileListItems(files)
    }
    if ($row.find("TD").eq(5).html() != null && $row.find("TD").eq(5).html() != undefined && $row.find("TD").eq(5).html() != "") {
        var b = new ClipboardEvent("").clipboardData || new DataTransfer();
        var files = [
            new File(['content'], $row.find("TD").eq(5).html())
        ];
        $("#flEditReimburseSupportingDoc").get(0).files = new FileListItems(files)
    }
    $("#mdUpdateMedicalLeave").modal("show");
}

function FileListItems(files) {
    var b = new ClipboardEvent("").clipboardData || new DataTransfer()
    for (var i = 0, len = files.length; i < len; i++) b.items.add(files[i])
    return b.files
}

function UpdateMedicalLeave() {
    $('.err').attr("hidden", true);
    if ($("#divMedLeave").find(".dataTables_empty").length === 1) {
        $("#divMedLeave").find(".dataTables_empty").parent("tr").remove();
    }
    ValidateEditMedLeaveFields();
    if ($('.err-edit:visible').length === 0) {
        var fromDate = $("#txtEditMedFromDate").val();
        var toDate = $("#txtEditMedToDate").val();
        var rowNum = $("#hdnMedLeaveRowNumber").val();
        var checkexistdate = false; var rowcount = 0;
        $("#tblMedicalLeaveDetails TBODY TR").each(function () {
            var row = $(this); rowcount = $(this)[0].getAttribute("data-row-number");
            var leaveid = row.find("TD").eq(10).html();
            if (rowcount != rowNum) {
                if (fromDate == row.find("TD").eq(0).html() && toDate == row.find("TD").eq(1).html()) {
                    $('#errEditToDate').text('Already leave applied for the same dates');
                    $('#errEditToDate').removeAttr("hidden");
                    checkexistdate = true;
                    return false;
                }
            }
        });
        if (!checkexistdate) {
            var leaves = $("#txtEditMedTotalLeaves").val();
            var remarks = $("#txtEditMedRemarks").val();
            var supportingDoc = $("#flEditMedSupportingDoc").get(0).files[0];
            var editreimburseDoc = $("#flEditReimburseSupportingDoc").get(0).files[0];
            var isreimburse = $(".rd-btn-editreimburse:checked").val();

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
                    var rowNumber = $("#hdnMedLeaveRowNumber").val();
                    var newClass = $("#divMedLeave tr[data-row-number=" + rowNumber + "]").attr("class");
                    var updatedRow = "";
                    if (typeof supportingDoc == "undefined" && typeof editreimburseDoc == "undefined") {
                        updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td></td><td></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimburse + "</td><td hidden>" + result.split("~")[0] + "</td><td hidden>" + result.split("~")[1] + "</td></tr>";
                    }
                    else if (typeof supportingDoc != "undefined" && typeof editreimburseDoc == "undefined") {
                        updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td>" + supportingDoc.name + "</td><td></td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimburse + "</td><td hidden>" + result.split("~")[0] + "</td><td hidden>" + result.split("~")[1] + "</td></tr>";
                    }
                    else if (typeof supportingDoc == "undefined" && typeof editreimburseDoc != "undefined") {
                        updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td></td><td>" + editreimburseDoc.name + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimburse + "</td><td hidden>" + result.split("~")[0] + "</td><td hidden>" + result.split("~")[1] + "</td></tr>";
                    }
                    else {
                        updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td>" + supportingDoc.name + "</td><td>" + editreimburseDoc.name + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimburse + "</td><td hidden>" + result.split("~")[0] + "</td><td hidden>" + result.split("~")[1] + "</td></tr>";
                    }

                    $("#divMedLeave tr[data-row-number=" + rowNumber + "]").replaceWith(updatedRow);
                    $("#mdUpdateMedicalLeave").modal("hide");
                    alertify.success("Medical leave details updated successfully");
                    disabledate(rowNumber);
                }, error: function (result) {
                    alertify.error("Could not save medical leave details");
                }
            });
        }
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
    $('.err').attr("hidden", true);
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
        if ($("#flEditReimburseSupportingDoc").val().length == 0) {
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
        beforeShowDay: function (date) {
            var string = jQuery.datepicker.formatDate('dd-mm-yy', date);
            return [dates.indexOf(string) == -1]
        },
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
        beforeShowDay: function (date) {
            var string = jQuery.datepicker.formatDate('dd-mm-yy', date);
            return [dates.indexOf(string) == -1]
        },
        closeOnDateSelect: true
    });
}
