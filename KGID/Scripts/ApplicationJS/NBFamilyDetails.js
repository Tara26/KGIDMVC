var table;
var today = new Date();
var tblrowNum = 0;
$(document).ready(function () {
    if (!$.fn.dataTable.isDataTable('#tblFamilyDetails')) {
        table = $('#tblFamilyDetails').DataTable({
            paging: false,
            info: false,
            searching: false,
            "ordering": false,
        });
    }
    $("div#tblFamilyDetails_wrapper").addClass("pb-3");
    UpdateRelationCount();
    var isSpouseAdded = $(".tdRelation:contains('Spouse')").length;
    if ($("#ddlDRType").val() == 2 && isSpouseAdded > 0) {
        $("#tblFamilyDetails tbody tdRelation:contains('Spouse')").addClass("spousealert");
    }
    if ($("#hdnIsOrphan").val() == "True" && $("#ddlDRType").val() == 1)
        $("#divNote").show();
    else
        $("#divNote").hide();

    $("#rbtnAlive").prop("checked", true);
    AliveDead("Alive");
});

$('#ddlRelation').change(function (e) {
    $('#txtMemberName').prop("readonly", false);
    if ($("#errMember:visible").length > 0) {
        $("#errMember").attr("hidden", true);
    }
    $("#chkSiblingMarried").prop("checked", false);
    var relation = $(this)[0].selectedOptions[0].innerHTML;
    var isFatherAdded = $(".tdRelation:contains('Father')").length;
    var isMotherAdded = $(".tdRelation:contains('Mother')").length;
    var isSpouseAdded = $(".tdRelation:contains('Spouse')").length;
    var isNoneAdded = $(".tdRelation:contains('None')").length;
    var isOrphan = $("#hdnIsOrphan").val();
    var isMarried = $("#hdnIsMarried").val();
    var empdob = $("#txtBasicDateOfBirth").val();
    var dat = empdob.split("-");
    var d = dat[1] + "-" + dat[0] + "-" + dat[2];
    var dob = new Date(d);
    $('#txtMemberName').val("");
    if (relation == "None") {
        var isNoneAdded = $(".tdRelation:contains('None')").length;
        if (isNoneAdded > 0) {
            $('#ddlRelation').val("");
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
            return false;
        }
        if ($("tr[data-row-number]").length > 0) {
            $('#ddlRelation').val("");
            alertify.alert("Please remove the family member you have added previously.").setHeader("Warning!!!");
            return false;
        }
        $("#txtMemberName").val("").attr("disabled", true);
        $("#txtDOB").val("").attr("disabled", true);
        $("#txtAge").val("").attr("disabled", true);
        $("#txtHealth").val("").attr("disabled", true);
        $("#txtDead").val("").attr("disabled", true);
        $("#txtDeathDate").val("").attr("disabled", true);
        $(".chkAlive").attr("disabled", true);
    }
    else {
        if (isNoneAdded > 0) {
            $('#ddlRelation').val("");
            alertify.alert("You cannot add other family members as you have added None in family details.").setHeader("Warning!!!");
            return false;
        }
        $("#txtMemberName").val("").removeAttr("disabled");
        $("#txtDOB").val("").removeAttr("disabled");
        $("#txtAge").val("").removeAttr("disabled");
        $(".chkAlive").removeAttr("disabled");
        if ($("#rbtnAlive").is(":checked")) {
            $("#txtHealth").val("").removeAttr("disabled");
        }
        else {
            $("#txtDead").val("").removeAttr("disabled");
            $("#txtDeathDate").val("").removeAttr("disabled");
        }
    }

    if (relation === "Father") {
        if ($('#father_name').val() != "") {
            $('#txtMemberName').val($('#father_name').val());
            $('#txtMemberName').prop("readonly", true);
        }
        if (isFatherAdded > 0) {
            $('#ddlRelation').val("");
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
        }
        else {
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                maxDate: new Date((dob.getFullYear() - 10), 11, 31),
                minDate: new Date(today.getFullYear() - 100, 0, 1),
                defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
                yearStart: today.getFullYear() - 100,
                yearEnd: dob.getFullYear() - 10,
                scrollMonth: false,
                scrollInput: false,
                autoClose: true,
                closeOnDateSelect: true
            };

            $('#txtDOB').datetimepicker("destroy");
            $('#txtDOB').datetimepicker(options);
        }
    }
    if (relation === "Mother") {
        if (isMotherAdded > 0) {
            $('#ddlRelation').val("");
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
        }
        else if (isFatherAdded == 0) {
            $('#ddlRelation').val("");
            $("#errMember").text("Please add father before adding other family members");
            $("#errMember").removeAttr("hidden");
        }
        else {
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoClose: true,
                maxDate: new Date((dob.getFullYear() - 10), 11, 31),
                minDate: new Date(today.getFullYear() - 100, 0, 1),
                defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
                yearStart: today.getFullYear() - 100,
                yearEnd: dob.getFullYear() - 10,
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };
            $('#txtDOB').datetimepicker("destroy");
            $('#txtDOB').datetimepicker(options);
        }
    }
    if (relation === "Spouse") {
        if ($('#txtSpouseName').val() != "") {
            if ($("#hdnDRStatus").val() == "2") {
                $('#txtMemberName').val($('#txtCS').val());
                $('#txtMemberName').prop("readonly", true);
            }
            else {
                $('#txtMemberName').val($('#txtSpouseName').val());
                $('#txtMemberName').prop("readonly", true);
            }
        }
        if (isSpouseAdded > 0) {
            $('#ddlRelation').val("");
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
        }
        else if (isFatherAdded == 0 && isOrphan != "True" && $('#father_name').val() != '') {
            $('#ddlRelation').val("");
            $("#errMember").text("Please add father before adding other family members");
            $("#errMember").removeAttr("hidden");
        }
        else if (isMotherAdded == 0 && isOrphan != "True" && $('#father_name').val() != '') {
            $('#ddlRelation').val("");
            $("#errMember").text("Please add mother before adding other family members");
            $("#errMember").removeAttr("hidden");
        }
        else {
            if ($(".tdRelation:contains('Mother')").length > 0) {
                var dateOfBirthMother = $(".tdRelation:contains('Mother')").siblings(".tdDateOfBirth").text();
                var dateArray = dateOfBirthMother.split('-');
                var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
                var dateOfMotherBirth = new Date(date);

                var options = {
                    timepicker: false,
                    format: 'd-m-Y',
                    autoClose: true,
                    yearStart: today.getFullYear() - 100,
                    yearEnd: today.getFullYear() - 18,
                    defaultDate: new Date(dateOfMotherBirth.getFullYear(), dateOfMotherBirth.getMonth(), dateOfMotherBirth.getDate()),
                    maxDate: new Date((today.getFullYear() - 18), today.getMonth(), today.getDate()),
                    minDate: new Date(today.getFullYear() - 100, 0, 1),
                    scrollMonth: false,
                    scrollInput: false,
                    closeOnDateSelect: true
                };

                $('#txtDOB').datetimepicker("destroy");
                $('#txtDOB').datetimepicker(options);
            } else {
                var options = {
                    timepicker: false,
                    format: 'd-m-Y',
                    autoClose: true,
                    yearStart: today.getFullYear() - 100,
                    yearEnd: today.getFullYear() - 18,
                    defaultDate: new Date(today.getFullYear() - 18, today.getMonth(), today.getDate()),
                    maxDate: new Date((today.getFullYear() - 18), today.getMonth(), today.getDate()),
                    minDate: new Date(today.getFullYear() - 100, 0, 1),
                    scrollMonth: false,
                    scrollInput: false,
                    closeOnDateSelect: true
                };
                $('#txtDOB').datetimepicker("destroy");
                $('#txtDOB').datetimepicker(options);
            }
        }
    }
    if (relation === "Brother" || relation === "Sister") {
        if (isFatherAdded == 0) {
            $('#ddlRelation').val("");
            $("#errMember").text("Please add father before adding other family members");
            $("#errMember").removeAttr("hidden");
        }
        else if (isMotherAdded == 0) {
            $('#ddlRelation').val("");
            $("#errMember").text("Please add mother before adding other family members");
            $("#errMember").removeAttr("hidden");
        }
        else {
            if ($(".tdRelation:contains('Mother')").length > 0) {
                var dateOfBirthMother = $(".tdRelation:contains('Mother')").siblings(".tdDateOfBirth").text();
                var dateArray = dateOfBirthMother.split('-');
                var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
                var dateOfMotherBirth = new Date(date);
                var dateOfBirthFather = $(".tdRelation:contains('Father')").siblings(".tdDateOfBirth").text();
                var dateArray = dateOfBirthFather.split('-');
                var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
                var dateOfFatherBirth = new Date(date);
                var startDate = '';
                if (dateOfMotherBirth > dateOfFatherBirth)
                    startDate = dateOfMotherBirth;
                else
                    startDate = dateOfFatherBirth;

                var options = {
                    timepicker: false,
                    format: 'd-m-Y',
                    autoClose: true,
                    yearStart: startDate.getFullYear() + 10,
                    yearEnd: today.getFullYear(),
                    minDate: new Date(startDate.getFullYear() + 10, 0, 1),
                    maxDate: today,
                    defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
                    scrollMonth: false,
                    scrollInput: false,
                    closeOnDateSelect: true
                };

                $('#txtDOB').datetimepicker("destroy");
                $('#txtDOB').datetimepicker(options);
            }
        }
    }
    if (relation === "Daughter" || relation === "Son") {
        var divStatus = $("#hdnDRStatus").val();
        if (isSpouseAdded == 0 && divStatus != "1") {
            $('#ddlRelation').val("");
            $("#errMember").text("Please add spouse before adding children");
            $("#errMember").removeAttr("hidden");
        }
        else if (divStatus == "1" && isFatherAdded == 0 && isOrphan != "True") {
            $('#ddlRelation').val("");
            $("#errMember").text("Please add father before adding children");
            $("#errMember").removeAttr("hidden");
        }
        else if (divStatus == "1" && isMotherAdded == 0 && isOrphan != "True") {
            $('#ddlRelation').val("");
            $("#errMember").text("Please add mother before adding children");
            $("#errMember").removeAttr("hidden");
        }
        else {
            if ($(".tdRelation:contains('Spouse')").length > 0) {
                var dateOfBirthSpouse = $(".tdRelation:contains('Spouse')").siblings(".tdDateOfBirth").text();
                var dateArray = dateOfBirthSpouse.split('-');
                var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
                var dateOfSpouseBirth = new Date(date);
                var dateOfBirth = $("#txtBasicDateOfBirth").val();
                var dateArray = dateOfBirth.split('-');
                var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
                var dateOfBirth = new Date(date);
                var startdate = '';
                if (dateOfBirth > dateOfSpouseBirth)
                    startdate = dateOfBirth;
                else
                    startdate = dateOfSpouseBirth;

                var options = {
                    timepicker: false,
                    format: 'd-m-Y',
                    autoClose: true,
                    yearStart: startdate.getFullYear() + 10,
                    yearEnd: today.getFullYear(),
                    minDate: new Date(startdate.getFullYear() + 10, 0, 1),
                    maxDate: today,
                    defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
                    scrollMonth: false,
                    scrollInput: false,
                    closeOnDateSelect: true
                };

                $('#txtDOB').datetimepicker("destroy");
                $('#txtDOB').datetimepicker(options);
            }
            else {
                var dateOfBirth = $("#txtBasicDateOfBirth").val();
                var dateArray = dateOfBirth.split('-');
                var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
                var dateOfBirth = new Date(date);
                var options = {
                    timepicker: false,
                    format: 'd-m-Y',
                    autoClose: true,
                    yearStart: dateOfBirth.getFullYear() + 10,
                    yearEnd: today.getFullYear(),
                    minDate: new Date(dateOfBirth.getFullYear() + 10, 0, 1),
                    maxDate: today,
                    defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
                    scrollMonth: false,
                    scrollInput: false,
                    closeOnDateSelect: true
                };

                $('#txtDOB').datetimepicker("destroy");
                $('#txtDOB').datetimepicker(options);
            }
        }
    }
    if (relation === "Brother" || relation === "Sister") {
        if (age != "" || age != null) {
            var age = Math.round($("#txtDAge").val());
            if (age >= 18) {
                $("#divSiblingMarried").show();
                if (relation === "Brother" && $('.knlan').is(":visible")) {
                    $("#lblBrotherMarriedKN").show();
                    $("#lblBrotherMarriedEN").hide();
                    $("#lblSisterMarriedKN").hide();
                    $("#lblSisterMarriedEN").hide();
                }
                else if (relation === "Brother" && $('.Enlan').is(":visible")) {
                    $("#lblBrotherMarriedEN").show();
                    $("#lblBrotherMarriedKN").hide();
                    $("#lblSisterMarriedKN").hide();
                    $("#lblSisterMarriedEN").hide();
                }
                else if (relation === "Sister" && $('.Enlan').is(":visible")) {
                    $("#lblSisterMarriedEN").show();
                    $("#lblSisterMarriedKN").hide();
                    $("#lblBrotherMarriedEN").hide();
                    $("#lblBrotherMarriedKN").hide();
                }
                else {
                    $("#lblSisterMarriedKN").show();
                    $("#lblSisterMarriedEN").hide();
                    $("#lblBrotherMarriedEN").hide();
                    $("#lblBrotherMarriedKN").hide();
                }
            }
        }
        else {
            $("#divSiblingMarried").hide();
        }
    }
});

$(".disable-keyboard").on("keypress", function (e) {
    return false;
});

function SaveFamilyMember(savetype) {
    if ($("#divFamily").find(".dataTables_empty").length === 1) {
        $("#divFamily").find(".dataTables_empty").parent("tr").remove();
    }
    var sibMarried = false; var err = false;
    if (savetype == "Add") {
        if ($("#ddlRelation option:selected")[0].innerHTML == "Brother" || $("#ddlRelation option:selected")[0].innerHTML == "Sister") {
            sibMarried = $("#chkSiblingMarried").is(":checked");
        }
    }
    else {
        if ($("#ddlEditRelation option:selected")[0].innerHTML == "Brother" || $("#ddlEditRelation option:selected")[0].innerHTML == "Sister") {
            sibMarried = $("#chkEditSiblingMarried").is(":checked")
        }
    }
    (savetype == "Add") ? validateFamilyFields() : validateEditFamilyFields();
    if (savetype == "Add") {
        if ($('.err:visible').length > 0)
            err = true;
    }
    else {
        if ($('.err-edit:visible').length > 0)
            err = true;
    }
    if (!err) {
        var isMarried = $(".rd-btn-mstatus:checked").val();
        var isOrphan = $(".rd-btn-orphan:checked").val();
        if (isMarried === "True" || ($(".rd-btn-orphan:checked").length > 0 && isOrphan !== "True")) {
            var viewModel = {
                EmpID: $("#hdnEmployeeId").val(),
                NameOfMember: (savetype == "Add") ? $("#txtMemberName").val() : $("#txtEditMemberName").val(),
                Relation: (savetype == "Add") ? $("#ddlRelation option:selected")[0].innerHTML : $("#ddlEditRelation option:selected")[0].innerHTML,
                DateOfBirth: (savetype == "Add") ? $("#txtDOB").val() : $("#txtEditDOB").val(),
                AliveDead: (savetype == "Add") ? $("#rbtnAlive").is(":checked") : $("#rbtnEditAlive").is(":checked"),
                HealthCondition: (savetype == "Add") ? $("#txtHealth").val() : $("#txtEditHealth").val(),
                Age: (savetype == "Add") ? $("#txtDAge").val() : $("#txtEditDAge").val(),
                IsSiblingMarried: sibMarried,
                DateOfDeath: (savetype == "Add") ? $("#txtDeathDate").val() : $("#txtEditDeathDate").val(),
                ReasonOfDeath: (savetype == "Add") ? $("#txtDead").val() : $("#txtEditDead").val(),
                ApplicationId: $("#spnReferanceNo").text(),
                Id: (savetype == "Add") ? 0 : $("#hdnFamilyRowNumber").val()
            };
            addrow(viewModel, 0, savetype);
            UpdateRelationCount();
            (savetype == "Add") ? resetFamilyFields() : resetEditFamilyFields();
            if (savetype != "Add") {
                $("#mdUpdateFamily").modal("hide");
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }
        }
    }
}

function addrow(viewmodel, leaveid, type) {
    if (rowNumber == 0)
        rowNumber = $("#tblFamilyDetails TBODY TR").length + 1;
    if ($("#tblFamilyDetails TBODY TR").find("td").hasClass("dataTables_empty") || $("#tblFamilyDetails TBODY TR").length == 0) {
        leaveid = 0;
    }
    else {
        leaveid = tblrowNum + 1;
    }
    tblrowNum = leaveid;
    var HealthCond;
    var SibMStatus = "";

    if (viewmodel.Relation === "Brother" || viewmodel.Relation === "Sister") {
        if (viewmodel.IsSiblingMarried) {
            SibMStatus = "Married";
        }
        else {
            SibMStatus = "Unmarried"
        }
    }
    else {
        SibMStatus = "N/A";
    }
    if (type == "Add") {
        if ($("#rbtnAlive").is(":checked") == true) {
            HealthCond = "Alive";
        } else if ($("#rbtnDead").is(":checked") == true) {
            HealthCond = "Dead";
        } else {
            HealthCond = "";
        }
        var newClass;
        if ($("tr[data-row-number=" + $("tr[data-row-number]").length + "]").hasClass("odd")) {
            newClass = "even";
        } else {
            newClass = "odd";
        }
        if (viewmodel.Relation === "None") {
            var AddRow = "<tr data-row-number='" + leaveid + "' role='row' class='" + newClass + "'><td class='tdName'>" + viewmodel.NameOfMember + "</td><td class='tdRelation'>" + viewmodel.Relation + "</td><td class='tdDateOfBirth'>" + viewmodel.DateOfBirth + "</td><td class='tdAge'>" + viewmodel.Age + "</td><td class='tdLivingStatus'>" + HealthCond + "</td><td>" + SibMStatus + "</td><td>" + viewmodel.HealthCondition + "</td><td>" + viewmodel.DateOfDeath + "</td><td>" + viewmodel.ReasonOfDeath + "</td><td><a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteFamilyRow(" + leaveid + ");'>Delete</a></td>&nbsp;<td class='tdInsert' hidden>" + false + "</td><td hidden>" + 'None' + "</td></tr>";
            $("#tblFamilyDetails tbody").append(AddRow);
        }
        else {
            var AddRow = "<tr data-row-number='" + leaveid + "' role='row' class='" + newClass + "'><td class='tdName'>" + viewmodel.NameOfMember + "</td><td class='tdRelation'>" + viewmodel.Relation + "</td><td class='tdDateOfBirth'>" + viewmodel.DateOfBirth + "</td><td class='tdAge'>" + viewmodel.Age + "</td><td class='tdLivingStatus'>" + HealthCond + "</td><td>" + SibMStatus + "</td><td>" + viewmodel.HealthCondition + "</td><td>" + viewmodel.DateOfDeath + "</td><td>" + viewmodel.ReasonOfDeath + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary edit' onclick='EditFamilyDetail(" + leaveid + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteFamilyRow(" + leaveid + ");'>Delete</a></td><td class='tdInsert' hidden>" + false + "</td><td hidden>" + 'New' + "</td></tr>";
            $("#tblFamilyDetails tbody").append(AddRow);
        }
    }
    else {
        if ($("#rbtnEditAlive").is(":checked") == true) {
            HealthCond = "Alive";
        } else if ($("#rbtnEditDead").is(":checked") == true) {
            HealthCond = "Dead";
        } else {
            HealthCond = "";
        }

        var rowNumber = $("#hdnFamilyRowNumber").val();
        var newClass = $("#divFamily tr[data-row-number=" + rowNumber + "]").attr("class");
        var row = $("#divFamily tr[data-row-number=" + rowNumber + "]");
        var newaddedrow = "";
        if (row.find("TD").eq(10).html() == "true")
            newaddedrow = true;
        else
            newaddedrow = false;
        var chkEditDeleteStatus = row.find("TD").eq(11).html();
        if (viewmodel.Relation === "None") {
            var updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td class='tdName'>" + viewmodel.NameOfMember + "</td><td class='tdRelation'>" + viewmodel.Relation + "</td><td class='tdDateOfBirth'>" + viewmodel.DateOfBirth + "</td><td class='tdAge'>" + viewmodel.Age + "</td><td class='tdLivingStatus'>" + HealthCond + "</td><td>" + SibMStatus + "</td><td>" + viewmodel.HealthCondition + "</td><td>" + viewmodel.DateOfDeath + "</td><td>" + viewmodel.ReasonOfDeath + "</td><td><a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteFamilyRow(" + leaveid + ");'>Delete</a></td>&nbsp;<td class='tdInsert' hidden>" + newaddedrow + "</td><td hidden>" + chkEditDeleteStatus + "</td></tr>";
            $("#divFamily tr[data-row-number=" + rowNumber + "]").replaceWith(updatedRow);
        }
        else if (chkEditDeleteStatus == "None") {
            var updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td class='tdName'>" + viewmodel.NameOfMember + "</td><td class='tdRelation'>" + viewmodel.Relation + "</td><td class='tdDateOfBirth'>" + viewmodel.DateOfBirth + "</td><td class='tdAge'>" + viewmodel.Age + "</td><td class='tdLivingStatus'>" + HealthCond + "</td><td>" + SibMStatus + "</td><td>" + viewmodel.HealthCondition + "</td><td>" + viewmodel.DateOfDeath + "</td><td>" + viewmodel.ReasonOfDeath + "</td><td></td>&nbsp;<td class='tdInsert' hidden>" + newaddedrow + "</td><td hidden>" + chkEditDeleteStatus + "</td></tr>";
            $("#divFamily tr[data-row-number=" + rowNumber + "]").replaceWith(updatedRow);
        }
        else if (chkEditDeleteStatus == "New") {
            var updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td class='tdName'>" + viewmodel.NameOfMember + "</td><td class='tdRelation'>" + viewmodel.Relation + "</td><td class='tdDateOfBirth'>" + viewmodel.DateOfBirth + "</td><td class='tdAge'>" + viewmodel.Age + "</td><td class='tdLivingStatus'>" + HealthCond + "</td><td>" + SibMStatus + "</td><td>" + viewmodel.HealthCondition + "</td><td>" + viewmodel.DateOfDeath + "</td><td>" + viewmodel.ReasonOfDeath + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditFamilyDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteFamilyRow(" + rowNumber + ");'>Delete</a></td><td class='tdInsert' hidden>" + newaddedrow + "</td><td hidden>" + chkEditDeleteStatus + "</td></tr>";
            $("#divFamily tr[data-row-number=" + rowNumber + "]").replaceWith(updatedRow);
        }
        else {
            var updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td class='tdName'>" + viewmodel.NameOfMember + "</td><td class='tdRelation'>" + viewmodel.Relation + "</td><td class='tdDateOfBirth'>" + viewmodel.DateOfBirth + "</td><td class='tdAge'>" + viewmodel.Age + "</td><td class='tdLivingStatus'>" + HealthCond + "</td><td>" + SibMStatus + "</td><td>" + viewmodel.HealthCondition + "</td><td>" + viewmodel.DateOfDeath + "</td><td>" + viewmodel.ReasonOfDeath + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditFamilyDetail(" + rowNumber + ");'>Edit</a>&nbsp;<td class='tdInsert' hidden>" + newaddedrow + "</td><td hidden>" + chkEditDeleteStatus + "</td></tr>";
            $("#divFamily tr[data-row-number=" + rowNumber + "]").replaceWith(updatedRow);
        }
    }
}

$("#txtDOB").change(function () {
    var dateString = $("#txtDOB").val();
    if (dateString !== "") {
        var dat = dateString.split("-");
        var d = dat[1] + "-" + dat[0] + "-" + dat[2];
        var myDate = new Date(d);
        var today = new Date();

        var age = Math.floor((today - myDate) / (365.25 * 24 * 60 * 60 * 1000));
        $("#txtDAge").val(age);
        //$(".xdsoft_datetimepicker ")[0].style.display = "none";
        var options = {
            timepicker: false,
            format: 'd-m-Y',
            autoclose: true,
            yearStart: myDate.getFullYear(),
            yearEnd: today.getFullYear(),
            minDate: new Date(myDate.getFullYear(), myDate.getMonth(), myDate.getDate()),
            maxDate: today,
            defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
            scrollMonth: false,
            scrollInput: false,
            closeOnDateSelect: true
        };

        $('#txtDeathDate').datetimepicker("destroy");
        $('#txtDeathDate').datetimepicker(options).on('change', function () {
           // $('#txtDeathDate').Close();
        });
        var relation = $("#ddlRelation option:selected").text();
        if (relation != "") {
            if (age >= 18 && (relation == "Brother" || relation == "Sister")) {
                $("#divSiblingMarried").show();
                if (relation === "Brother" && $('.knlan').is(":visible")) {
                    $("#lblBrotherMarriedKN").show();
                    $("#lblBrotherMarriedEN").hide();
                    $("#lblSisterMarriedKN").hide();
                    $("#lblSisterMarriedEN").hide();
                }
                else if (relation === "Brother" && $('.Enlan').is(":visible")) {
                    $("#lblBrotherMarriedEN").show();
                    $("#lblBrotherMarriedKN").hide();
                    $("#lblSisterMarriedKN").hide();
                    $("#lblSisterMarriedEN").hide();
                }
                else if (relation === "Sister" && $('.Enlan').is(":visible")) {
                    $("#lblSisterMarriedEN").show();
                    $("#lblSisterMarriedKN").hide();
                    $("#lblBrotherMarriedEN").hide();
                    $("#lblBrotherMarriedKN").hide();
                }
                else {
                    $("#lblSisterMarriedKN").show();
                    $("#lblSisterMarriedEN").hide();
                    $("#lblBrotherMarriedEN").hide();
                    $("#lblBrotherMarriedKN").hide();
                }
            }
            else if (relation == "Father" || relation == "Mother") {
                var empdob = $("#txtBasicDateOfBirth").val();
                var dat = empdob.split("-");
                var d = dat[1] + "-" + dat[0] + "-" + dat[2];
                var myDate = new Date(d);
                var today = new Date();

                var empage = Math.floor((today - myDate) / (365.25 * 24 * 60 * 60 * 1000));
                if (relation == "Father") {
                    if (age < parseInt(empage) + 10) {
                        alertify.alert("Father's age cannot be less than employee's age.").setHeader("Warning!!!");
                    }
                } else {
                    if (age < parseInt(empage) + 10) {
                        alertify.alert("Mother's age cannot be less than employee's age.").setHeader("Warning!!!");
                    }
                }
            }
            else {
                $("#divSiblingMarried").hide();
            }
        }
    }
    else {
        $("#txtDAge").val("");
    }
});

$("#txtDeathDate").change(function () {
    debugger;
    var dateString = $("#txtDeathDate").val();
    var bdateString = $("#txtDOB").val();
    if (dateString !== "" && bdateString != "") {
        var ddat = dateString.split("-");
        var dd = ddat[1] + "-" + ddat[0] + "-" + ddat[2];
        var dDate = new Date(dd);
        var bdat = bdateString.split("-");
        var bd = bdat[1] + "-" + bdat[0] + "-" + bdat[2];
        var bDate = new Date(bd);
        var age = Math.floor((dDate - bDate) / (365.25 * 24 * 60 * 60 * 1000));
        $("#txtDAge").val(age);
    }
    else if (dateString == "") {
        $("#txtDeathDate").val("");
    }
    else if (bdateString == "") {
        $("#txtDOB").val("");
    }
})

function validateFamilyFields() {
    $('.err').attr('hidden', true);

    var txtMemberName = $("#txtMemberName").val();
    var drpdwnRelation = $("#ddlRelation option:selected").text();
    if (drpdwnRelation != "None") {
        var txtBirthDate = $("#txtDOB").val();
        var txtLivingStatus = $("input[name='txtLiving']:checked").val();
        var txtHealth = $("#txtHealth").val();
        var txtDeathReason = $("#txtDead").val();
        var txtDateOfDeath = $("#txtDeathDate").val();

        if ($("#ddlRelation").val() == "") {
            $("#errRelationReq").removeAttr("hidden");
        }

        if (txtMemberName === "") {
            $("#errMemberNameReq").removeAttr("hidden");
        }

        if (drpdwnRelation === "") {
            $("#errRelationReq").removeAttr('hidden');
        }

        if (txtBirthDate.trim() === '') {
            $("#errDOBReq").text("Please enter family member's date of birth.");
            $("#errDOBReq").removeAttr('hidden');
        }

        if ($("input[name='txtLiving']:checked").length === 0 || txtLivingStatus.trim() === '') {
            $("#errLivingReq").removeAttr('hidden');
        }

        if ($("#txtHealth").prop("disabled") === false && txtHealth.trim() === '') {
            $("#errHealthReq").removeAttr('hidden');
        }

        if ($("#txtDead").prop("disabled") === false && txtDeathReason.trim() === '') {
            $("#errDeathReasonReq").removeAttr('hidden');
        }

        if ($("#txtDeathDate").prop("disabled") === false > 0 && txtDateOfDeath.trim() === '') {
            $("#errDateDeathReq").removeAttr('hidden');
        }
        var dateParts = $("#txtDOB").val().split("-");
        var dateObject = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);

        var dateParts1 = $("#txtDeathDate").val().split("-");
        var dateObject1 = new Date(+dateParts1[2], dateParts1[1] - 1, +dateParts1[0]);

        var dtDOB = new Date(dateObject);
        var dtDOD = new Date(dateObject1);
        if (dtDOB > dtDOD) {
            $("#errDOBReq").text("Date of birth Should be greater than Date of death.");
            $("#errDOBReq").removeAttr('hidden');
        }
    }
}

function UpdateRelationCount() {
    $("#txtNoOfBrother").val($(".tdRelation:contains('Brother')").length);
    $("#txtNoOfSister").val($(".tdRelation:contains('Sister')").length);
    var numberOfChildren = $(".tdRelation:contains('Daughter')").length + $(".tdRelation:contains('Son')").length;
    $("#txtChildren").val(numberOfChildren);
}
function resetFamilyFields() {
    $("#ddlRelation option[value='']").prop("selected", true);
    $("#txtMemberName").val('');
    $("#txtDOB").val('');
    $("input[name='txtLiving']:checked").prop("checked", false);
    $("#txtHealth").val('');
    $("#txtDead").val('');
    $("#txtDAge").val('');
    $("#txtDeathDate").val("");
    $("#txtHealth").attr("disabled", true);
    $("#txtDead").attr("disabled", true);
    $("#txtDeathDate").attr("disabled", true);
    $("#chkSiblingMarried").prop("checked", false);
    $("#divSiblingMarried").hide();
}

function AliveDead(type) {
    if (type == "Alive") {
        $("#txtDead").val("");
        $("#txtDeathDate").val("");
        $("#txtHealth").removeAttr("disabled");
        $("#txtDead").attr("disabled", true);
        $("#txtDeathDate").attr("disabled", true);
    } else {
        $("#txtHealth").val("");
        $("#txtHealth").attr("disabled", true);
        $("#txtDead").removeAttr("disabled");
        $("#txtDeathDate").removeAttr("disabled");

        var today = new Date();
        var dateOfBirthString = $("#txtDOB").val();
        var dateArray = dateOfBirthString.split('-');
        var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
        var dateOfBirth = new Date(date);

        var options = {
            timepicker: false,
            format: 'd-m-Y',
            autoclose: true,
            yearStart: dateOfBirth.getFullYear(),
            yearEnd: today.getFullYear(),
            minDate: new Date(dateOfBirth.getFullYear(), dateOfBirth.getMonth(), dateOfBirth.getDate()),
            maxDate: today,
            defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
            scrollMonth: false,
            scrollInput: false,
            closeOnDateSelect: true
        };

        $('#txtDeathDate').datetimepicker("destroy");
        $('#txtDeathDate').datetimepicker(options);
    }
}

$("#txtMemberName,#txtEditMemberName").keypress(function (e) {
    var isValid = false;
    var regex = /^[A-Za-z.\s]*$/;
    isValid = regex.test(e.originalEvent.key);
    if (!isValid)
        e.preventDefault();
});

//// Edit Family detail
$("#txtEditDOB").change(function () {
    var dateString = $("#txtEditDOB").val();
    if (dateString !== "") {
        var dat = dateString.split("-");
        var d = dat[1] + "-" + dat[0] + "-" + dat[2];
        var myDate = new Date(d);
        var today = new Date();

        var age = Math.floor((today - myDate) / (365.25 * 24 * 60 * 60 * 1000));
        $("#txtEditDAge").val(age);
        $(".xdsoft_datetimepicker ")[0].style.display = "none";
        var options = {
            timepicker: false,
            format: 'd-m-Y',
            autoClose: true,
            yearStart: myDate.getFullYear(),
            yearEnd: today.getFullYear(),
            minDate: new Date(myDate.getFullYear(), myDate.getMonth(), myDate.getDate()),
            maxDate: today,
            defaultDate: new Date(today.getFullYear(), myDate.getMonth(), myDate.getDate()),
            scrollMonth: false,
            scrollInput: false,
            closeOnDateSelect: true

        };

        $('#txtEditDeathDate').datetimepicker("destroy");
        $('#txtEditDeathDate').datetimepicker(options);

        var relation = $("#ddlEditRelation option:selected").text();
        if (age >= 18 && (relation == "Brother" || relation == "Sister")) {
            $("#divEditSiblingMarried").show();
            //if (relation === "Brother") {
            //    $("#lblEditBrotherMarried").show();
            //    $("#lblEditSisterMarried").hide();
            //}
            //else {
            //    $("#lblEditBrotherMarried").hide();
            //    $("#lblEditSisterMarried").show();
            //}
            if (relation === "Brother" && $('.knlan').is(":visible")) {
                $("#lblEditBrotherMarriedKN").show();
                $("#lblEditBrotherMarriedEN").hide();
                $("#lblEditSisterMarriedEN").hide();
                $("#lblEditSisterMarriedKN").hide();
            }
            else if (relation === "Brother" && $('.Enlan').is(":visible")) {
                $("#lblEditBrotherMarriedEN").show();
                $("#lblEditBrotherMarriedKN").hide();
                $("#lblEditSisterMarriedEN").hide();
                $("#lblEditSisterMarriedKN").hide();
            }
            else if (relation === "Sister" && $('.Enlan').is(":visible")) {
                $("#lblEditSisterMarriedEN").show();
                $("#lblEditSisterMarriedKN").hide();
                $("#lblEditBrotherMarriedKN").hide();
                $("#lblEditBrotherMarriedEN").hide();
            }
            else {
                $("#lblEditSisterMarriedKN").show();
                $("#lblEditSisterMarriedEN").hide();
                $("#lblEditBrotherMarriedEN").hide();
                $("#lblEditBrotherMarriedKN").hide();
            }
        }
        else {
            $("#divEditSiblingMarried").hide();
        }
    }
    else {
        $("#txtEditDAge").val("");
    }
    $(".xdsoft_datetimepicker ")[0].style.display = "none";
});

$("#txtEditDeathDate").change(function () {
    debugger
    var dateString = $("#txtEditDeathDate").val();
    var bdateString = $("#txtEditDOB").val();
    if (dateString !== "" && bdateString != "") {
        var ddat = dateString.split("-");
        var dd = ddat[1] + "-" + ddat[0] + "-" + ddat[2];
        var dDate = new Date(dd);
        var bdat = bdateString.split("-");
        var bd = bdat[1] + "-" + bdat[0] + "-" + bdat[2];
        var bDate = new Date(bd);
        var age = Math.floor((dDate - bDate) / (365.25 * 24 * 60 * 60 * 1000));
        $("#txtEditDAge").val(age);
    }
    else if (dateString == "") {
        $("#txtEditDeathDate").val("");
    }
    else if (bdateString == "") {
        $("#txtEditDOB").val("");
    }
})

function EditFamilyDetail(rowNumber) {
    var retuenresult = 1;
    $("#hdnFamilyRowNumber").val(rowNumber);
    var $row = $("#divFamily tr[data-row-number=" + rowNumber + "]");
    if ($row.find("TD").eq(10).html().toLocaleLowerCase() == "true") {
        $.ajax({
            url: '/Employee/CheckFamilyMemberDetails',
            data: JSON.stringify({ rowNum: rowNumber }),
            async: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result == 0) {
                    retuenresult = result;
                    alertify.alert("Please remove member from nominee list before editing.").setHeader("Warning!!!");
                    return false;
                }
            }, error: function (result) {
                alertify.error("Could not edit family member");
            }
        });
    }
    if (retuenresult == 0) {
        return false;
    }
    $("#txtEditMemberName").prop("readonly", true);
    $('.err-edit').attr('hidden', true);

    $("#txtEditMemberName").val($row.find("TD").eq(0).html());
    var relation = $row.find("TD").eq(1).html();
    var relval = $("#ddlEditRelation option:contains(" + relation + ")").val();
    $("#ddlEditRelation").val(relval);
    $("#ddlEditRelation").attr("disabled", true);
    $("#txtEditDOB").val($row.find("TD").eq(2).html());
    $("#txtEditDAge").val($row.find("TD").eq(3).html());
    var livingStatus = $row.find("TD").eq(4).html();
    if (livingStatus === "Alive") {
        $("input#rbtnEditAlive[value='Alive']").prop("checked", true);
        AliveEditDead('Alive');
    }
    else {
        $("input#rbtnEditDead[value='Dead']").prop("checked", true);
        AliveEditDead('Dead');
    }
    var age = $("#txtEditDAge").val();
    if (relation === "Brother") {
        if (Math.round(age) >= 18) {
            $("#divEditSiblingMarried").show();
            //$("#lblEditBrotherMarried").show();
            //$("#lblEditSisterMarried").hide();
            if ($('.knlan').is(":visible")) {
                $("#lblEditBrotherMarriedKN").show();
                $("#lblEditBrotherMarriedEN").hide();
                $("#lblEditSisterMarriedEN").hide();
                $("#lblEditSisterMarriedKN").hide();
            }
            else if ($('.Enlan').is(":visible")) {
                $("#lblEditBrotherMarriedEN").show();
                $("#lblEditBrotherMarriedKN").hide();
                $("#lblEditSisterMarriedEN").hide();
                $("#lblEditSisterMarriedKN").hide();
            }
            if ($row.find("TD").eq(5).html() === "Married") {
                $("#chkEditSiblingMarried").prop("checked", true);
            }
            else {
                $("#chkEditSiblingMarried").prop("checked", false);
            }
        }
        else {
            $("#divEditSiblingMarried").hide();
        }
    }
    else if (relation === "Sister") {
        if (Math.round(age) >= 18) {
            $("#divEditSiblingMarried").show();
            //$("#lblEditSisterMarried").show();
            //$("#lblEditBrotherMarried").hide();
            if ($('.Enlan').is(":visible")) {
                $("#lblEditSisterMarriedEN").show();
                $("#lblEditSisterMarriedKN").hide();
                $("#lblEditBrotherMarriedKN").hide();
                $("#lblEditBrotherMarriedEN").hide();
            }
            else {
                $("#lblEditSisterMarriedKN").show();
                $("#lblEditSisterMarriedEN").hide();
                $("#lblEditBrotherMarriedEN").hide();
                $("#lblEditBrotherMarriedKN").hide();
            }
            if ($row.find("TD").eq(5).html() === "Married") {
                $("#chkEditSiblingMarried").prop("checked", true);
            }
            else {
                $("#chkEditSiblingMarried").prop("checked", false);
            }
        }
        else {
            $("#divEditSiblingMarried").hide();
        }

    }
    else {
        $("#divEditSiblingMarried").hide();
        $("#lblEditBrotherMarried").hide();
        $("#lblEditSisterMarried").hide();
        $("#chkEditSiblingMarried").prop("checked", false);
    }
    if ($row.find("TD").eq(9)[0].children.length == 1)
        $("#txtEditDOB").attr("disabled", true);
    else
        $("#txtEditDOB").removeAttr("disabled");
    var today = new Date();
    var empdob = $("#txtBasicDateOfBirth").val();
    var dat = empdob.split("-");
    var d = dat[1] + "-" + dat[0] + "-" + dat[2];
    var dob = new Date(d);
    if (relation === "Father") {
        var options = {
            maxDate: new Date((dob.getFullYear() - 10), 11, 31),
            minDate: new Date(today.getFullYear() - 100, 0, 1),
            defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
            yearStart: today.getFullYear() - 100,
            yearEnd: dob.getFullYear() - 10,
            timepicker: false,
            format: 'd-m-Y',
            autoclose: true,
            scrollMonth: false,
            scrollInput: false,
            closeOnDateSelect: true
        };

        $('#txtEditDOB').datetimepicker("destroy");
        $('#txtEditDOB').datetimepicker(options);
    }

    if (relation === "Mother") {
        var options = {
            timepicker: false,
            format: 'd-m-Y',
            autoclose: true,
            maxDate: new Date((dob.getFullYear() - 10), 11, 31),
            minDate: new Date(today.getFullYear() - 100, 0, 1),
            defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
            yearStart: today.getFullYear() - 100,
            yearEnd: dob.getFullYear() - 10,
            scrollMonth: false,
            scrollInput: false,
            closeOnDateSelect: true
        };

        $('#txtEditDOB').datetimepicker("destroy");
        $('#txtEditDOB').datetimepicker(options);
    }

    if (relation === "Spouse") {
        if ($(".tdRelation:contains('Mother')").length > 0) {
            var dateOfBirthMother = $(".tdRelation:contains('Mother')").siblings(".tdDateOfBirth").text();
            var dateArray = dateOfBirthMother.split('-');
            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
            var dateOfMotherBirth = new Date(date);
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoclose: true,
                yearStart: today.getFullYear() - 100,
                yearEnd: today.getFullYear() - 18,
                defaultDate: new Date(dateOfMotherBirth.getFullYear(), dateOfMotherBirth.getMonth(), dateOfMotherBirth.getDate()),
                maxDate: new Date((today.getFullYear() - 18), today.getMonth(), today.getDate()),
                minDate: new Date(today.getFullYear() - 100, 0, 1),
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };

            $('#txtEditDOB').datetimepicker("destroy");
            $('#txtEditDOB').datetimepicker(options);
        }
        else {
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoclose: true,
                yearStart: today.getFullYear() - 100,
                yearEnd: today.getFullYear() - 18,
                defaultDate: new Date(today.getFullYear() - 18, 0, 1),
                maxDate: new Date((today.getFullYear() - 18), today.getMonth(), today.getDate()),
                minDate: new Date(today.getFullYear() - 100, 0, 1),
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };

            $('#txtEditDOB').datetimepicker("destroy");
            $('#txtEditDOB').datetimepicker(options);
        }
    }

    if (relation === "Brother" || relation === "Sister") {
        var dateOfBirthMother = $(".tdRelation:contains('Mother')").siblings(".tdDateOfBirth").text();
        var dateArray = dateOfBirthMother.split('-');
        var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
        var dateOfMotherBirth = new Date(date);
        var dateOfBirthFather = $(".tdRelation:contains('Father')").siblings(".tdDateOfBirth").text();
        var dateArray = dateOfBirthFather.split('-');
        var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
        var dateOfFatherBirth = new Date(date);
        var startDate = '';
        if (dateOfMotherBirth > dateOfFatherBirth)
            startDate = dateOfMotherBirth;
        else
            startDate = dateOfFatherBirth;

        var options = {
            timepicker: false,
            format: 'd-m-Y',
            autoclose: true,
            yearStart: startDate.getFullYear() + 10,
            yearEnd: today.getFullYear(),
            defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
            maxDate: today,
            minDate: new Date(startDate.getFullYear() + 10, 0, 1),
            scrollMonth: false,
            scrollInput: false,
            closeOnDateSelect: true
        };

        $('#txtEditDOB').datetimepicker("destroy");
        $('#txtEditDOB').datetimepicker(options);
    }

    if (relation === "Daughter" || relation === "Son") {
        var divStatus = $("#hdnDRStatus").val();
        if (divStatus != "1") {
            var dateOfBirthSpouse = $(".tdRelation:contains('Spouse')").siblings(".tdDateOfBirth").text();
            var dateArray = dateOfBirthSpouse.split('-');
            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
            var dateOfSpouseBirth = new Date(date);
            var dateOfBirth = $("#txtBasicDateOfBirth").val();
            var dateArray = dateOfBirth.split('-');
            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
            var dateOfBirth = new Date(date);
            var startdate = '';
            if (dateOfBirth > dateOfSpouseBirth)
                startdate = dateOfBirth;
            else
                startdate = dateOfSpouseBirth;

            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoclose: true,
                yearStart: startdate.getFullYear() + 10,
                yearEnd: today.getFullYear(),
                minDate: new Date(startdate.getFullYear() + 10, 0, 1),
                maxDate: today,
                defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };
            $('#txtEditDOB').datetimepicker("destroy");
            $('#txtEditDOB').datetimepicker(options);
        }
        else {
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoclose: true,
                yearStart: dob.getFullYear() + 10,
                yearEnd: today.getFullYear(),
                defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
                minDate: new Date(dob.getFullYear() + 10, 0, 1),
                maxDate: today,
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };
            $('#txtEditDOB').datetimepicker("destroy");
            $('#txtEditDOB').datetimepicker(options);
        }
    }

    $("#txtEditHealth").val($row.find("TD").eq(6).html());
    $("#txtEditDead").val($row.find("TD").eq(8).html());
    $("#txtEditDeathDate").val($row.find("TD").eq(7).html());
    $("#mdUpdateFamily").modal("show");
}

function validateEditFamilyFields() {
    $('.err-edit').attr('hidden', true);
    var drpdwnRelation = $("#ddlEditRelation option:selected").text();
    if (drpdwnRelation != "None") {
        var txtBirthDate = $("#txtEditDOB").val();
        var txtLivingStatus = $("input[name='txtEditLiving']:checked").val();
        var txtHealth = $("#txtEditHealth").val();
        var txtDeathReason = $("#txtEditDead").val();
        var txtAgeOfDeath = $("#txtEditDAge").val();
        var txtDateOfDeath = $("#txtEditDeathDate").val();

        if ($("#ddlEditRelation").val() == "") {
            $("#errEditRelationReq").removeAttr('hidden');
        }

        if (drpdwnRelation.toLowerCase() === 'select' || drpdwnRelation.toLowerCase() === "") {
            $("#errEditRelationReq").removeAttr('hidden');
        }

        if (txtBirthDate.trim() === '') {
            $("#errEditDOBReq").removeAttr('hidden');
            $("#errEditDOBReq").text("Please enter family member's date of birth.");
        }
        if ($("input[name='txtEditLiving']:checked").length === 0 || txtLivingStatus.trim() === '') {
            $("#errEditLivingReq").removeAttr('hidden');
        }

        if ($("#txtEditHealth").prop("disabled") === false && txtHealth.trim() === '') {
            $("#errEditHealthReq").removeAttr('hidden');
        }

        if ($("#txtEditDead").prop("disabled") === false && txtDeathReason.trim() === '') {
            $("#errEditDeathReasonReq").removeAttr('hidden');
        }

        if ($("#txtEditDAge").prop("readonly") === false > 0 && txtAgeOfDeath.trim() === '') {
            $("#errEditDeathAgeReq").removeAttr('hidden');
        }

        if ($("#txtEditDeathDate").prop("disabled") === false > 0 && txtDateOfDeath.trim() === '') {
            $("#errEditDateDeathReq").removeAttr('hidden');
        }
        var dateParts = $("#txtEditDOB").val().split("-");
        var dateObject = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);

        var dateParts1 = $("#txtEditDeathDate").val().split("-");
        var dateObject1 = new Date(+dateParts1[2], dateParts1[1] - 1, +dateParts1[0]);


        var dtEditDOB = new Date(dateObject);
        var dtEditDOD = new Date(dateObject1);
        if (dtEditDOB > dtEditDOD) {
            $("#errEditDOBReq").text("Date of birth Should be greater than Date of death.");
            $("#errEditDOBReq").removeAttr('hidden');
        }
    }
}

function resetEditFamilyFields() {
    $("#ddlEditRelation option[value='']").prop("selected", true);
    $("#txtEditMemberName").val('');
    $("#txtEditDOB").val('');
    $("input[name='txtEditLiving']:checked").prop("checked", false);
    $("#txtEditHealth").val('');
    $("#txtEditDead").val('');
    $("#txtEditDAge").val('');
    $("#txtEditDeathDate").val("");
    $("#txtEditDead").attr("disabled", true);
    $("#txtEditDeathDate").attr("disabled", true);
    $("#chkSiblingMarried").prop("checked", false);
    $("#divSiblingMarried").hide();
}

function AliveEditDead(type) {
    if (type == "Alive") {
        $("#txtEditDead").val("");
        $("#txtEditDeathDate").val("");
        $("#txtEditHealth").removeAttr("disabled");
        $("#txtEditDead").attr("disabled", true);
        $("#txtEditDeathDate").attr("disabled", true);
    } else {
        $("#txtEditHealth").val("");
        $("#txtEditHealth").attr("disabled", true);
        $("#txtEditDead").removeAttr("disabled");
        $("#txtEditDeathDate").removeAttr("disabled");

        var today = new Date();
        var dateOfBirthString = $("#txtEditDOB").val();

        var dateArray = dateOfBirthString.split('-');
        var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
        var dateOfBirth = new Date(date);

        var options = {
            timepicker: false,
            format: 'd-m-Y',
            autoclose: true,
            yearStart: dateOfBirth.getFullYear(),
            yearEnd: today.getFullYear(),
            minDate: new Date(dateOfBirth.getFullYear(), dateOfBirth.getMonth(), dateOfBirth.getDate()),
            maxDate: today,
            defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
            scrollMonth: false,
            scrollInput: false,
            closeOnDateSelect: true
        };

        $('#txtEditDeathDate').datetimepicker("destroy");
        $('#txtEditDeathDate').datetimepicker(options);
    }
}

////Delete Family detail
function DeleteFamilyRow(rowNumber) {

    alertify.confirm("Are you sure want to delete this family member?", function () {

        var $row = $("#divFamily tr[data-row-number=" + rowNumber + "]");
        var checkinserted = $row.find("TD").eq(10).html();
        if (checkinserted == "false") {
            $row.remove();
            if ($("#divFamily").find("tbody").find("tr").length === 0) {
                var AddRow = "<tr class='odd'><td valign='top' colspan='8' class='dataTables_empty'>No data available in table</td></tr>";
                $("#tblFamilyDetails tbody").append(AddRow);
            }
            UpdateRelationCount();
            alertify.success("Family member deleted successfully");
        }
        else {
            var $row = $("#divFamily tr[data-row-number=" + rowNumber + "]");
            var membername = $row.find("TD").eq(0).html();
            $.ajax({
                url: '/Employee/DeleteFamilyDetails',
                data: JSON.stringify({ 'LeaveID': rowNumber }),
                async: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    if (result == 1) {
                        $("#divFamily tr[data-row-number='" + rowNumber + "']").remove();
                        if ($("#divFamily").find("tbody").find("tr").length === 0) {
                            var AddRow = "<tr class='odd'><td valign='top' colspan='8' class='dataTables_empty'>No data available in table</td></tr>";
                            $("#tblFamilyDetails tbody").append(AddRow);
                        }
                        alertify.success("Family member deleted successfully");
                        UpdateRelationCount();
                    }
                    else {
                        alertify.alert(membername + " assigned as nominee. Please remove member from nominee list before deleting.").setHeader("Warning!!!");
                    }
                }, error: function (result) {
                    alertify.error("Could not delete family member");
                }
            });
        }
    }).setHeader("Warning!!!");
};