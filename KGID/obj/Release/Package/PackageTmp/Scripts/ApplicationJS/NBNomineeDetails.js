//Variable Declarations
var alldead = true;
var nomineeDetails = new Array();
var guardianRelation = "";
var table;
var previoussharevalue = 0;
var guardianList = new Array();
var rowNumber = 0;
$(document).ready(function () {
    debugger;
    if ($.fn.dataTable.isDataTable('#tblNomineeDetails')) {
        table = $('#tblNomineeDetails').DataTable();
    }
    else {
        table = $('#tblNomineeDetails').DataTable({
            paging: false,
            info: false,
            searching: false,
            "ordering": false
        });
    }

    $("#tblFamilyDetails TBODY TR").each(function () {
       var $row = $(this);
       var livingStatus = $row.find(".tdLivingStatus").text();
       var memberName = $row.find(".tdName").text();
       var relation = $row.find(".tdRelation").text();
       var famAge = $row.find(".tdAge").text();
       var memberId = $row.data("fd-id");

        if (livingStatus === "Alive") {
            isEmpOrphan = false;
            alldead = false;
            var familyDetails = {};
            familyDetails.memberId = memberId;
            familyDetails.name = memberName;
            familyDetails.relation = relation;
            familyDetails.age = famAge;
            nomineeDetails.push(familyDetails);
        }
    });

    var eachSumShare = 0;
    $('.val-share').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });

    if (eachSumShare < 100) {
        $("#btnNomAdd").removeAttr("disabled");
    }
    else {
        $("#btnNomAdd").attr("disabled", true);
    }

    var type = "";
    var isOrphan = $(".rd-btn-orphan:checked").val();
    if (isOrphan == "True") {
        type = "Orphan";
    }
    else if ((isOrphan != "True" || isOrphan == undefined) && nomineeDetails.length == 0) {
        type = "Guardian";
    }
    else if (nomineeDetails.find(x => x.relation == "None") != undefined) {
        type = "Orphan";
    }
    else {
        type = "Nominee";
    }
    GetNomineeNameList(type);
    ShowFields(type); 
});

//// Add nominee details
function ShowFields(type) {
    if (type == "Orphan" || type == "Guardian") {
        $("#txtNomineeName").show();
        $("#txtEditNomineeName").show();
        $("#ddlNomineeName").hide();
        $("#ddlEditNomineeName").hide();
        $("#ddlNomineeRelation").removeAttr("disabled");
        $("#ddlEditGuardianRelation").removeAttr("disabled");
        $("#divAge").hide();
    }
    else {
        $("#txtNomineeName").hide();
        $("#txtEditNomineeName").hide();
        $("#ddlNomineeName").show();
        $("#ddlEditNomineeName").show();
        $("#ddlNomineeRelation").attr("disabled", "true");
        $("#ddlEditGuardianRelation").attr("disabled", "true");
        $("#divAge").show();
    }
}

function GetNomineeNameList(type) {
    var empid = $("#hdnPReferanceNo").val();
    $.ajax({
        type: "POST",
        url: "/Employee/GetNomineeNameList?empId=" + empid + "&type=" + type,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (type != "Guardian" && type != "Orphan") {
                $("#ddlNomineeName").empty();
                $("#ddlEditNomineeName").empty();
                $("#ddlEditGuardianRelation").empty();
                $("#ddlGuardianRelation").empty();
                $('#ddlNomineeName').append(new Option("-- Select Nominee --", ""));
                $('#ddlEditNomineeName').append(new Option("-- Select Nominee --", ""));
                $('#ddlEditGuardianRelation').append(new Option("-- Select Guardian Relation --", ""));
                $('#ddlGuardianRelation').append(new Option("-- Select Guardian Relation --", ""));
                
                $.each(data.NomineeList, function () {
                    $("#ddlNomineeName").append($("<option />").val(this.Id).text(this.Value));
                    $("#ddlEditNomineeName").append($("<option />").val(this.Id).text(this.Value));
                })
                $.each(data.GuardianList, function () {
                    $("#ddlGuardianRelation").append($("<option />").val(this.Id).text(this.Value));
                    $("#ddlEditGuardianRelation").append($("<option />").val(this.Id).text(this.Value));
                })
            }
            else {
                $("#ddlNomineeRelation").empty();
                $("#ddlEditNomineeRelation").empty();
                $("#ddlEditGuardianRelation").empty();
                $("#ddlGuardianRelation").empty();
                $('#ddlNomineeRelation').append(new Option("-- Select Relation --", ""));
                $('#ddlEditNomineeRelation').append(new Option("-- Select Relation --", ""));
                $('#ddlEditGuardianRelation').append(new Option("-- Select Guardian Relation --", ""));
                $('#ddlGuardianRelation').append(new Option("-- Select Guardian Relation --", ""));
                $.each(data.NomineeList, function () {
                    $("#ddlNomineeRelation").append($("<option />").val(this.Id).text(this.Value));
                    $("#ddlEditNomineeRelation").append($("<option />").val(this.Id).text(this.Value));
                })
                $.each(data.GuardianList, function () {
                    $("#ddlGuardianRelation").append($("<option />").val(this.Id).text(this.Value));
                    $("#ddlEditGuardianRelation").append($("<option />").val(this.Id).text(this.Value));
                })
                $("#divTwo").insertBefore("#divOne");
            }
        }
    });
}

$(document).on('change', '#ddlNomineeName', function () {
    memberName = $("#ddlNomineeName option:selected").text();
    memberId = $("#ddlNomineeName").val();
    var member = nomineeDetails.find(x => x.memberId == memberId);
    $("#txtGuardianName").removeAttr("disabled");
    if (typeof member !== "undefined") {
        $('#ddlNomineeRelation').empty();
        $('#ddlNomineeRelation').append(new Option(member.relation, member.relation));
        $("#txtNomineeAge").val(member.age);
        $("#txtNomineeAge").trigger("change");
    }
    else {
        $("#ddlNomineeRelation").val("");
        $("#txtNomineeAge").val("");
        $("#txtNomineeAge").trigger("change");
    }
    var isRelationAdded = $(".nRelation:contains(" + member.relation + ")").length;
    var isMemberAdded = $(".nName:contains(" + member.name + ")").length;
    if (isRelationAdded > 0 && isMemberAdded > 0) {
        $("#txtNomineeAge").val("");
        $("#txtNomineeShare").val("");
        $("#txtGuardianName").val("");
        $("#ddlGuardianRelation").val("");
        $('#ddlNomineeRelation').val("");
        $('#ddlNomineeName').val("");
        alertify.alert("Member Already Added.").setHeader("Warning!!!");
        return false;
    }
});

$("#txtNomineeAge").on("change", function () {
    memberName = $("#ddlNomineeRelation option:selected").text();
    var NAge = $("#txtNomineeAge").val();
    if (NAge <= 18 || memberName === "Guardian") {
        $("#txtGuardianName").val("");
        $("#ddlGuardianRelation").val("");
        $("#txtGuardianName").removeAttr("readonly");
        $("#ddlGuardianRelation").removeAttr("disabled");
    }
    else {
        $("#txtGuardianName").val("");
        $("#ddlGuardianRelation").val("");
        $("#txtGuardianName").attr("readonly", "readonly");
        $("#ddlGuardianRelation").attr("disabled", true);
    }
});

$(document).on('change', '#ddlNomineeRelation', function () {
    $('.err').attr("hidden", true);
    memberName = $("#ddlNomineeRelation option:selected").text();
    var isOrphan = $(".rd-btn-orphan:checked").val();
    $("#txtGuardianName").removeAttr("disabled");
    if ($("#ddlNomineeRelation").val() != "") {
        if (memberName === "Guardian") {
            if (alldead == true || isOrphan === "True") {
                $("#divAge").show();
                $("#txtNomineeAge").removeAttr("readonly");
                $("#errNomineeAgeReq").attr("hidden", true);
            }
            $("#labelNomineeName").text("Name of Nominee");
            $("#txtGuardianName").removeAttr("readonly", "readonly");
            $("#ddlGuardianRelation").removeAttr("disabled");
            $("#txtNomineeName").attr("disabled", true);
            $("#section-minor").show();
            $("#txtNomineeName").val("");
            $("#lblGuardianRelation").text("Relation With Guardian");
        }
        else if (memberName === "Trust/Organisation") {
            $("#divAge").hide();
            $("#section-minor").hide();
            $("#labelNomineeName").text("Name of Organization");
            $("#txtNomineeName").removeAttr("disabled");
            $("#txtGuardianName").val("");
            $("#ddlGuardianRelation").val("");
            $("#txtGuardianName").attr("readonly", "readonly");
            $("#ddlGuardianRelation").attr("disabled", "true");
            $("#txtNomineeName").val("");
        }
        else {
            $("#labelNomineeName").text("Name of Nominee");
            $("#txtGuardianName").val("");
            $("#labelAge").text("Age");
            $("#ddlGuardianRelation").val("");
            $("#txtGuardianName").attr("readonly", "readonly");
            $("#txtNomineeAge").attr("readonly", "readonly");
            $("#ddlGuardianRelation").attr("disabled", "true");
            $("#txtNomineeName").val("");
            $("#lblGuardianRelation").text("Guardian Relation With Nominee");
        }
    }
});

$(document).on('change', '#ddlGuardianRelation', function () {
    $("#txtGuardianName").val('');
    var GuardianRelationText = $("#ddlGuardianRelation option:selected").text();
    if (($(".rd-btn-orphan:checked").val() == "True" && $("#ddlDRType").val() == 0) || nomineeDetails.length == 0) {
        var isGuardianAdded = $(".gRelation:contains(" + GuardianRelationText + ")").length;
        if (isGuardianAdded > 0) {
            alertify.alert("Guardian Already Added.").setHeader("Warning!!!");
            $("#ddlGuardianRelation").val("");
            return false;
        }
    }
    else {
        if (GuardianRelationText == "Father" || GuardianRelationText == "Mother") {
            var spousedetails = nomineeDetails.find(x => x.relation == "Spouse")
            $("#txtGuardianName").val(spousedetails.name);
            $("#txtGuardianName").attr("disabled", true);
        } else {
            $("#txtGuardianName").removeAttr("disabled");
        }
    }
});

function resetNomineeFields() {
    $("#txtNomineeName").val('');
    $("#txtNomineeAge").val('');
    $("#txtGuardianName").val('');
    $("#ddlGuardianRelation").val('')
    $("#txtNomineeShare").val('');
    $("#labelAge").text("Age");
    $("#ddlNomineeName").val("");
    $('#ddlNomineeRelation').val("");
    var eachSumShare = 0;
    $('.val-share').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });

    if (eachSumShare < 100) {
        $("#btnNomAdd").removeAttr("disabled");
    }
    else {
        $("#btnNomAdd").attr("disabled", true);
    }
}

//// Edit nominee details
$(document).on('change', '#ddlEditNomineeRelation', function () {
    memberName = $("#ddlEditNomineeRelation option:selected").text();
    if (memberName === "Guardian") {
        $("#txtEditGuardianName").removeAttr("readonly", "readonly");
        $("#ddlEditGuardianRelation").removeAttr("disabled");
    }
    else {
        $("#txtEditGuardianName").val("");
        $("#txtEditGuardianName").attr("readonly", "readonly");
        $("#ddlEditGuardianRelation").attr("disabled", "true");
    }
});

$(document).on('change', '#ddlEditNomineeName', function () {
    memberName = $("#ddlEditNomineeName option:selected").text();
    memberId = $("#ddlEditNomineeName option:selected").val();
    var member = nomineeDetails.find(x => x.memberId == memberId);
    if (typeof member !== "undefined") {
        $("#ddlEditNomineeRelation").val(member.relation);
        $("#txtEditNomineeAge").val(member.age);
        $("#txtEditNomineeAge").trigger("change");
    }
    else {
        $("#ddlEditNomineeRelation").val("");
        $("#txtEditNomineeAge").val("");
        $("#txtEditNomineeAge").trigger("change");
    }
});

$("#txtEditNomineeAge").on("change", function () {
    memberName = $("#ddlEditNomineeRelation option:selected").text();
    var NAge = $("#txtEditNomineeAge").val();
    if (NAge <= 18) {
        $("#txtEditGuardianName").val("");
        $("#ddlEditGuardianRelation").val("");
        $("#txtEditGuardianName").removeAttr("readonly");
        $("#ddlEditGuardianRelation").removeAttr("disabled");

    }
    else if (memberName === "Guardian") {
        return true;
    }
    else {
        $("#txtEditGuardianName").val("");
        $("#ddlEditGuardianRelation").val("");
        $("#txtEditGuardianName").attr("readonly", "readonly");
        $("#ddlEditGuardianRelation").attr("disabled", true);
    }
});

function resetEditNomineeFields() {
    $('.err-edit').attr('hidden', true);
    $("#txtEditNomineeName").val('');
    $("#txtEditNomineeAge").val('');
    $("#txtEditNomineeShare").val('');
    $("#ddlNomineeName").val('');
    $("#ddlNomineeRelation").val('');
    $("#txtEditGuardianName").val('');
    $("#ddlEditGuardianRelation").val('');

    var eachSumShare = 0;
    $('.val-share').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });

    if (eachSumShare < 100) {
        $("#btnNomAdd").removeAttr("disabled");
    }
    else {
        $("#btnNomAdd").attr("disabled", true);
    }
}

function EditNomineeDetails(rowNumber, Id) {
    var isOrphan = $(".rd-btn-orphan:checked").val();
    $("#hdnRowNumber").val(rowNumber);
    $("#hdnId").val(Id);
    var $row = $("#divNominee tr[data-row-number=" + rowNumber + "]");
    var relation = $row.find("TD").eq(3).html();
    var guardianRelation = $row.find("TD").eq(6).html();
    var nomineeAge = parseInt($row.find("TD").eq(2).html());
    var relationName = $row.find("TD").eq(1).html();
    $("#txtEditNomineeName").val("");
    $("#ddlEditNomineeName option").removeAttr("selected");
    $("#txtEditNomineeAge").val("");
    $("#txtEditGuardianName").val("");
    $("#ddlEditGuardianRelation option").removeAttr("selected");
    $("#txtEditNomineeShare").val("");
    $("#ddlEditNomineeRelation option").removeAttr("selected");
    
    if (relation === "Trust/Organisation") {
        $("#labelEditNomineeName").text("Name of Organization");
        $("#txtEditNomineeName").removeAttr("disabled");
        $("#section-minor-edit").hide();
        $(".divEditAge").hide();
    }
    else if ((relation !== 'Trust/Organisation' && nomineeAge < 18) || (isOrphan === "True" && relation === 'Guardian') || (alldead == true && relation === "Guardian")) {
        if ((isOrphan === "True" && relation === 'Guardian') || (alldead == true && relation === "Guardian")) {
            $("#txtEditNomineeAge").removeAttr("disabled");
            $("#txtEditNomineeAge").removeAttr("readonly");
            $(".divEditAge").show();
        }
        $("#section-minor-edit").show();
        $("#txtEditNomineeName").attr("disabled", true);
        $("#labelEditNomineeName").text("Name of Nominee");
        $("#txtEditGuardianName").removeAttr("readonly");
        $("#ddlEditGuardianRelation").removeAttr("disabled");
    }
    else if (relation != "Guardian") {  // if (nomineeAge > 18) {
        $("#section-minor-edit").hide();
        $("#txtEditGuardianName").val("");
        $("#ddlEditGuardianRelation").val("");
    }
    else if (relation == "Guardian") {
        $("#txtEditNomineeAge").removeAttr("disabled");
        $("#txtEditNomineeAge").removeAttr("readonly");
        $(".divEditAge").show();
        $("#section-minor-edit").show();
        $("#txtEditNomineeName").attr("disabled", true);
        $("#lblGuardianRelation").text("Relation With Guardian");
    }
    
    $("#txtEditNomineeName").val(relationName);
    $("#ddlEditNomineeName option:contains(" + relationName + ")").attr("selected", "selected");
    $("#txtEditNomineeAge").val(nomineeAge);
    $("#txtEditNomineeShare").val($row.find("TD").eq(4).html());
    $("#txtEditGuardianName").val($row.find("TD").eq(5).html());
    var gRelation = $("#ddlEditGuardianRelation option:contains(" + guardianRelation + ")").val();
    $("#ddlEditGuardianRelation").val(gRelation);
    previoussharevalue = $("#txtEditNomineeShare").val();
    if (relation != "" && relation != undefined && nomineeDetails.length > 0) {
        var member = nomineeDetails.find(x => x.relation == relation);
        $('#ddlEditNomineeRelation').empty();
        $('#ddlEditNomineeRelation').append(new Option(member.relation, member.relation));
        $("#ddlEditNomineeRelation option:contains(" + member.relation + ")").attr("selected", "selected");
    }
    else {
        var rel = $("#ddlEditNomineeRelation option:contains(" + relation + ")").val();
        $("#ddlEditNomineeRelation").val(rel);
    }
    $("#mdUpdateNominee").modal("show");
}

$(document).on('change', '#ddlEditGuardianRelation', function () {
    $("#txtEditGuardianName").val("");
    var GuardianRelationText = $("#ddlEditGuardianRelation option:selected").text();
    if (($(".rd-btn-orphan:checked").val() == "True" && $("#ddlDRType").val() == 0) || nomineeDetails.length == 0) {
        var isGuardianAdded = $(".gRelation:contains(" + GuardianRelationText + ")").length;
        if (isGuardianAdded > 0) {
            alertify.alert("Guardian Already Added.").setHeader("Warning!!!");
            $("#ddlEditGuardianRelation").val();
            return false;
        }
    }
    else {
        if (GuardianRelationText == "Father" || GuardianRelationText == "Mother") {
            var spousedetails = nomineeDetails.find(x => x.relation == "Spouse")
            $("#txtEditGuardianName").val(spousedetails.name);
            $("#txtEditGuardianName").attr("disabled", true);
        } else {
            $("#txtEditGuardianName").removeAttr("disabled");
        }
    }
});

//// Add & Update in nominee grid
function addNomineeDetails() {
    debugger;
    $('.err').attr("hidden", true);
    var i = 0;
    validateNomineeFields();
    var guardianAge = "";
    var nomineeName = "";
    if ($("#txtNomineeName").val() != "" && $("#txtNomineeName").val() != undefined && $("#txtNomineeName").val() != null) {
        nomineeName = $("#txtNomineeName").val();
    }
    else {
        nomineeName = $("#ddlNomineeName option:selected").text();
    }
    if ((isOrphan === "True" && $("#ddlNomineeRelation option:selected").text() === 'Guardian') || (alldead == true && $("#ddlNomineeRelation option:selected").text() === 'Guardian')) {
        guardianAge = $("#txtNomineeAge").val();
    }

    var viewModelNominee = {
        'EmpId': $("#hdnPReferanceNo").val(),
        'NameOfNominee': nomineeName,//$("#ddlNomineeName option:selected").text(),
        'Relation': ($("#ddlNomineeRelation").val() == "") ? $("#ddlNomineeRelation").val() : $("#ddlNomineeRelation option:selected").text(), 
        'PercentageShare': $("#txtNomineeShare").val(),
        'NameOfGaurdian': $("#txtGuardianName").val(),
        'GaurdianRelation': ($("#ddlGuardianRelation").val() == "") ? $("#ddlGuardianRelation").val() : $("#ddlGuardianRelation option:selected").text(), 
        'Age': guardianAge
    };
    if ($('.err:visible').length === 0) {
        if (i != 2) {
            $.ajax({
                url: '/Employee/InsertNomineeDetails',
                data: JSON.stringify(viewModelNominee),
                async: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    alertify.success("Nominee details saved successfully");
                    NserverResponse = true;
                    addRow(result);
                    resetNomineeFields();
                }, error: function (result) {
                    alertify.error("Could not save nominee details");
                }
            });
        }
    }
    return NserverResponse;
}

function addRow(result) {
    if ($("#divNominee").find(".dataTables_empty").length === 1) {
        $("#divNominee").find(".dataTables_empty").parent("tr").remove();
    }
    else {
        skipSumShareValidation = false;
    }
    var nomineeName = "";
    if ($("#txtNomineeName").val() != "" && $("#txtNomineeName").val() != undefined && $("#txtNomineeName").val() != null) {
        nomineeName = $("#txtNomineeName").val();
    }
    else {
        nomineeName = $("#ddlNomineeName option:selected").text();
    }
    var NName = nomineeName;
    var NAge = $("#txtNomineeAge").val();
    var Relation = '';
    if ($("#ddlNomineeRelation").val() == "")
        Relation = "";
    else
        Relation = $("#ddlNomineeRelation option:selected").text();
    var NShare = $("#txtNomineeShare").val();
    var GName = $("#txtGuardianName").val();
    var GRelation = "";
    if ($("#ddlGuardianRelation").val() != null && $("#ddlGuardianRelation").val() != "") {
        GRelation = $("#ddlGuardianRelation option:selected")[0].innerHTML;
    }
    var newClass;
    if ($("#divNominee tr[data-row-number=" + $("#divNominee tr[data-row-number]").length + "]").hasClass("even")) {
        newClass = "odd";
    } else {
        newClass = "even";
    }
    if (rowNumber == 0) 
        rowNumber = $("#tblNomineeDetails TBODY TR").length + 1;
    else
        rowNumber = rowNumber + 1;

    var AddRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'> <td hidden>" + result + "</td> <td class='nName'>" + NName + "</td> <td>" + NAge + "</td> <td class='nRelation'>" + Relation + "</td><td class='val-share'>" + NShare + "</td><td>" + GName + "</td><td class='gRelation'>" + GRelation + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditNomineeDetails(" + rowNumber + "," + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteNomineeDetails(" + result + "," + rowNumber + ");'>Delete</a></td></tr>";
    $("#tblNomineeDetails tbody").append(AddRow);
}

function UpdateNomineeDetails() {
    var i = 0;
    var theTotal = 0;
    var share = 0;
    var diff = 0;
    var guardianAge = "";
    share = $("#txtEditNomineeShare").val();
    $("#tblNomineeDetails tr td:nth-child(5)").each(function () {
        var val = this.innerText;
        theTotal += parseInt(val);
    });

    theTotal -= previoussharevalue;
    diff = 100 - parseInt(theTotal);
    if (share > diff && diff > 0) {
        i = 2;
        alertify.alert("Percentage share can not be more than " + diff + "%.").setHeader("warning!!!");;
        return false;
    }
    validateEditNomineeFields();
    
    var nomineeName = "";
    if ($("#txtEditNomineeName").val() != "" && $("#txtEditNomineeName").val() != undefined && $("#txtEditNomineeName").val() != null) {
        nomineeName = $("#txtEditNomineeName").val();
    }
    else {
        nomineeName = $("#ddlEditNomineeName option:selected").text();
    }
    if (($(".rd-btn-orphan:checked").val() === "True" && $("#ddlEditNomineeRelation option:selected").text() === 'Guardian') || (alldead == true && $("#ddlEditNomineeRelation option:selected").text() === 'Guardian')) {
        guardianAge = $("#txtEditNomineeAge").val();
    }
    var viewModelNominee = {
        'Id': $("#hdnId").val(),
        'EmpId': $("#hdnPReferanceNo").val(),
        'NameOfNominee': nomineeName,
        'Relation': ($("#ddlEditNomineeRelation").val() == "") ? $("#ddlEditNomineeRelation").val() : $("#ddlEditNomineeRelation option:selected").text(),
        'PercentageShare': $("#txtEditNomineeShare").val(),
        'NameOfGaurdian': $("#txtEditGuardianName").val(),
        'GaurdianRelation': ($("#ddlEditGuardianRelation").val() == "") ? $("#ddlEditGuardianRelation").val() :$("#ddlEditGuardianRelation option:selected").text(),
        'Age': guardianAge
    };
    if ($('.err-edit:visible').length === 0) {
        if (i != 2) {
            $.ajax({
                url: '/Employee/InsertNomineeDetails',
                data: JSON.stringify(viewModelNominee),
                async: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    alertify.success("Nominee details updated successfully");
                    NserverResponse = true;
                    updateRow(result);
                }, error: function (result) {
                    alertify.error("Could not updated nominee details");
                }
            });
        }
    }
    return NserverResponse;
}

function updateRow(result) {
    if ($("#divNominee").find(".dataTables_empty").length === 1) {
        $("#divNominee").find(".dataTables_empty").parent("tr").remove();
    }
    else {
        skipSumShareValidation = false;
    }
    if ($("#txtEditNomineeName").val() != "" && $("#txtEditNomineeName").val() != undefined && $("#txtEditNomineeName").val() != null) {
        nomineeName = $("#txtEditNomineeName").val();
    }
    else {
        nomineeName = $("#ddlEditNomineeName option:selected").text();
    }
    var NName = nomineeName;
    var NAge = $("#txtEditNomineeAge").val();
    var Relation = ($("#ddlEditNomineeRelation").val() == "") ? "" : $("#ddlEditNomineeRelation option:selected").text();
    var NShare = $("#txtEditNomineeShare").val();
    var GName = $("#txtEditGuardianName").val();
    var GRelation = "";
    if ($("#ddlEditGuardianRelation").val() != null && $("#ddlEditGuardianRelation").val() != "") {
        GRelation = $("#ddlEditGuardianRelation option:selected").text();
    }
    var rowNumber = $("#hdnRowNumber").val();
    var newClass = $("#divNominee tr[data-row-number=" + rowNumber + "]").attr("class");
    var updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'> <td hidden>" + result + "</td>  <td class='nName'>" + NName + "</td> <td>" + NAge + "</td> <td class='nRelation'>" + Relation + "</td><td class='val-share'>" + NShare + "</td><td>" + GName + "</td><td class='gRelation'>" + GRelation + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditNomineeDetails(" + rowNumber + "," + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteNomineeDetails(" + result + "," + rowNumber + ");'>Delete</a></td></tr>";
    $("#divNominee tr[data-row-number=" + rowNumber + "]").replaceWith(updatedRow);
    resetEditNomineeFields();
    $("#mdUpdateNominee").modal("hide");
}

//// Delete Nominee
function DeleteNomineeRow(rowNumber) {
    $("#divNominee tr[data-row-number='" + rowNumber + "']").remove();

    if ($("#divNominee").find("tbody").find("tr").length === 0) {
        var AddRow = "<tr class='odd'><td valign='top' colspan='8' class='dataTables_empty'>No data available in table</td></tr>";
        $("#tblNomineeDetails tbody").append(AddRow);
    }

    var eachSumShare = 0;
    $('.val-share').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });

    if (eachSumShare < 100) {
        $("#btnNomAdd").removeAttr("disabled");
    }
    else {
        $("#btnNomAdd").attr("disabled", true);
    }
}

function DeleteNomineeDetails(Id, rowNumber) {
    var viewModelNominee = {
        'nd_id': Id,
    };
    $.ajax({
        url: '/Employee/DeleteNomineeDetails',
        data: JSON.stringify(viewModelNominee),
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result == 1) {
                alertify.success("Nominee details deleted successfully");
                NserverResponse = true;
                DeleteNomineeRow(rowNumber);
            }
            else {
                alertify.error("Could not delete nominee details");
            }

        }, error: function (result) {
            alertify.error("Could not delete nominee details");
        }
    });
    return NserverResponse;
}

//Validations
$(".alphaonly").on("input", function () {
    var regexp = /[^a-z A-Z]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});

$('.disable-first-zero').keypress(function (e) {
    if (this.value.length == 0 && e.which == 48) {
        return false;
    }
});

function setMaxNumberLength(event, control, numberOfCharacters) {
    if ((((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) && $(control).val().length < numberOfCharacters)
        || event.keyCode === 8 || event.keyCode === 9 || event.keyCode === 13 || event.keyCode === 37 || event.keyCode === 39 || event.keyCode === 46) {
        return true;
    }
    return false;
}

function ValidateSumShare() {
    var sumShare = 0;
    var eachSumShare = 0;
    $('.val-share').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });

    if (eachSumShare === 100) {
        return true;
    }
    return false;
}

function validateNomineeFields() {
    $('.err').attr('hidden', true);
    var txtNomName = "";
    var isOrphan = $(".rd-btn-orphan:checked").val();
    if (isOrphan === "True" || alldead == true) {
        txtNomName = $("#txtNomineeName").val();
    }
    else {
        txtNomName = $("#ddlNomineeName  option:selected").text();
    }

    var txtAge = $("#txtNomineeAge").val();
    var drpdwnRelation = $("#ddlNomineeRelation option:selected").text();
    var txtShare = $("#txtNomineeShare").val();
    var txtGardName = $("#txtGuardianName").val();
    var txtGardRelation = $("#ddlGuardianRelation option:selected").val();
    
    //if (txtNomName.trim() === '') {
    //    $("#errNomineeAgeReq").attr('hidden', true);
    //    $("#errNomNameReq").removeAttr('hidden');
    //}
    if (drpdwnRelation !== 'Guardian') {
        if (txtNomName.trim() === '') {
            $("#errNomNameReq").removeAttr('hidden');
        }
    }

    if ((isOrphan === "True" && drpdwnRelation === 'Guardian') || (alldead == true && drpdwnRelation === "Guardian")) {
        if (txtAge < 18) {
            $("#errNomineeAgeReq").text('Gaurdian cannot be minor');
            $("#errNomineeAgeReq").removeAttr('hidden');
            return false;
        }
    }
    if (!$.isNumeric(txtAge) || txtAge === '' || txtAge === 0) {
        $("#errNomineeAgeReq").removeAttr('hidden');
    }

    if (drpdwnRelation.toLowerCase() === 'select') {
        $("#errNomRelationReq").removeAttr('hidden');
    }

    var sumShare = 0;
    var eachSumShare = 0;
    $('.val-share').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });

    sumShare = eachSumShare + parseInt(txtShare);

    if (txtShare.trim() === '' || parseInt(txtShare) == 0) {
        $("#errNomineeShareReq").removeAttr('hidden');
    }

    if (sumShare > 100) {
        $("#errNomineeSharePercent").removeAttr('hidden');
    }
    
    if ((parseInt($("#txtNomineeAge").val()) < 18) || (isOrphan === "true" && drpdwnRelation === 'Guardian') || (alldead == true && drpdwnRelation === "Guardian")) {
        if (txtGardName.trim() === '') {
            $("#errGardNomineeNameReq").removeAttr('hidden');
        }

        if (txtGardRelation.trim() === '') {
            $("#errGuardianRelationReq").removeAttr('hidden');
        }
    }
    if ($(".rd-btn-mstatus:checked").val() == "True" && (drpdwnRelation == "Son" || drpdwnRelation == "Daughter")) {
        if ($("#ddlDRType").val() != "1") {
            if ($("#txtGender").val() == "Male") {
                if (nomineeDetails.find(x => x.relation == "Spouse") != undefined) {
                    if ($("#ddlGuardianRelation option:selected").text() != "Mother") {
                        $("#errGuardianRelationReq").removeAttr('hidden');
                        $("#errGuardianRelationReq").text("Please select Mother as Guardian");
                        $("#ddlGuardianRelation").val("");
                    }
                } 
            } else {
                if (nomineeDetails.find(x => x.relation == "Spouse") != undefined) {
                    if ($("#ddlGuardianRelation option:selected").text() != "Father") {
                        $("#errGuardianRelationReq").removeAttr('hidden');
                        $("#errGuardianRelationReq").text("Please select Father as Guardian");
                        $("#ddlGuardianRelation").val("");
                    }
                } 
            }
        }
    }
}

function validateEditNomineeFields() {
    $('.err-edit').attr('hidden', true);
    var txtNomName = "";
    var isOrphan = $(".rd-btn-orphan:checked").val();
    if (isOrphan === "True" || alldead == true) {
        txtNomName = $("#txtEditNomineeName").val();
    }
    else {
        txtNomName = $("#ddlEditNomineeName option:selected").text();
    }

    var txtAge = $("#txtEditNomineeAge").val();
    var drpdwnRelation = $("#ddlEditNomineeRelation option:selected").text();
    var txtShare = $("#txtEditNomineeShare").val();
    var txtGardName = $("#txtEditGuardianName").val();
    var txtGardRelation = $("#ddlEditGuardianRelation option:selected").text();
    if (txtShare == '' || parseInt(txtShare) == 0) {
        $("#errEditNomineeShareReq").removeAttr('hidden');
    }
    if ((isOrphan === "True" && drpdwnRelation === 'Guardian') || (alldead == true && drpdwnRelation === "Guardian")) {
        if (txtAge < 18) {
            $("#errEditNomineeAgeReq").text('Gaurdian cannot be minor');
            $("#errEditNomineeAgeReq").removeAttr('hidden');
            return false;
        }
    }

    if (parseInt($("#txtEditNomineeAge").val()) < 18) {
        if (txtGardName.trim() === '') {
            $("#errEditGardNomineeNameReq").removeAttr('hidden');
        }

        if (txtGardRelation.trim() === '') {
            $("#errEditGardNomineeRelationReq").removeAttr('hidden');
        }
    }
    if (drpdwnRelation !== 'Guardian') {
        if (txtNomName.trim() === '') {
            $("#errEditNomNameReq").removeAttr('hidden');
        }
    }
    if ($(".rd-btn-mstatus:checked").val() == "True" && (drpdwnRelation == "Son" || drpdwnRelation == "Daughter")) {
        if ($("#ddlDRType").val() != "1") {
            if ($("#txtGender").val() == "Male") {
                if (nomineeDetails.find(x => x.relation == "Spouse") != undefined) {
                    if ($("#ddlEditGuardianRelation option:selected").text() != "Mother") {
                        $("#errEditGardNomineeRelationReq").removeAttr('hidden');
                        $("#errEditGardNomineeRelationReq").text("Please select Mother as Guardian");
                        $("#ddlEditGuardianRelation").val("");
                    }
                } 
            } else {
                if (nomineeDetails.find(x => x.relation == "Spouse") != undefined) {
                    if ($("#ddlEditGuardianRelation option:selected").text() != "Father") {
                        $("#errEditGardNomineeRelationReq").removeAttr('hidden');
                        $("#errEditGardNomineeRelationReq").text("Please select Father as Guardian");
                        $("#ddlEditGuardianRelation").val("");
                    }
                } 
            }
        }
    }
}

function Nominee() {
    memberName = $("#ddlNomineeRelation option:selected").text();
    var NAge = $("#txtNomineeAge").val();
    if (NAge >= 18 && memberName !== "Guardian") {
        $("#section-minor").hide()
        $("#txtGuardianName").val("");
        $("#ddlGuardianRelation").val("");
        $("#txtGuardianName").attr("readonly", "readonly");
        $("#ddlGuardianRelation").attr("disabled", "true");
    } else {
        $("#section-minor").show()
        $("#txtGuardianName").removeAttr("readonly", "readonly");
        $("#ddlGuardianRelation").removeAttr("disabled");
    }
}