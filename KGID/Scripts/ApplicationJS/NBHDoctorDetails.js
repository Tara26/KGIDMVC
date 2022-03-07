$(document).ready(function () {
    $("#hdnDocName").val($("#ddlDoctors option:selected").text());
    if ($("#txtKMCCode").val() == "") 
        $("#rbtnKMCDr").prop("checked", true);
    
    if ($(".rd-btn-hodd:checked").val() == "False") {
        $("#divDctrBnkDetails").show();
        $("#divKgid").hide();
        $("#divDsgtn").insertBefore("#divHsptl");
        $(".ddele").removeAttr("disabled");
    }
    else {
        $("#divDctrBnkDetails").hide();
        $("#divDsgtn").insertBefore("#divHsptl");
    }
    if (medicalRequiredStatus) {
        $(".field-required").addClass("required");
    }
    
    //handleClick('Other');
});

function DRhandleClick(ddtype) {
    if (ddtype == "KMC") {
        $("#txtKMCCode").removeAttr("disabled");
        $(".ddele").attr("disabled", true);
        $(".ddele").val("");
        $("#txtKMCCode").val("");
        $("#divDctrBnkDetails").hide();
        $("#divKgid").show();
        $("#divDsgtn").insertBefore("#divHsptl");
    }
    else {
        $(".ddele").removeAttr("disabled");
        $(".ddele").val("");
        $("#txtKgid,#txtKMCCode").val("");
        $("#hdnDocId").val("");
        $("#divDctrBnkDetails").show();
        $(".bnkdata").val("");
        $("#divKgid").hide();
        $("#divDsgtn").insertBefore("#divHsptl");
    }
}

$("#txtKMCCode").change(function () {
    if ($(".rd-btn-hodd").prop("checked") == true) {
        //var url = $(this).data("data_fetch_detail_url");
        var kmcCode = $(this).val();

        $.ajax({
            url: "/Employee/GetDoctorDetailByKMCCode",
            async: false,
            data: JSON.stringify({ "docKMCCode": kmcCode }),
            type: 'POST',
            cache: false,
            contentType: 'application/json; charset=utf-8',
            processData: false,
            success: function (response) {
                if (response.DoctorId == 0) {
                    if (kmcCode != "") {
                        $("#errCORRECTKMCCode").removeAttr("hidden");
                        $("#errKMCCodeReq").attr("hidden", true);
                    }

                    else {
                        $("#errCORRECTKMCCode").attr("hidden", true);
                        $("#errKMCCodeReq").removeAttr("hidden");
                    }
                    
                    $("#txtDocName").val("");
                    $("#txtKgid").val("");
                    $("#txtDesignation").val("");
                    $("#txtNameOfOffice").val("");
                    $("#txtPlaceOfOffice").val("");
                    return false;
                } else {
                    $("#txtDocName").val(response.DoctorName);
                    $("#txtKgid").val(response.KGIDOfDoctor);
                    $("#txtDesignation").val(response.Designation);
                    $("#txtNameOfOffice").val(response.NameOfHospital);
                    $("#txtPlaceOfOffice").val(response.PlaceOfHospital);
                    $("#hdnDocId").val(response.DoctorId);
                    $("#errCORRECTKMCCode").attr("hidden",true);
                    $("#errKMCCodeReq").attr("hidden",true);
                }
            }, error: function (response) {
                alertify.error("Could not get doctor details");
            }
        });
    }
});

function ValidateDoctorDetails() {
    $('.err').attr('hidden', true);
    var ddlKMCCodeVal = $("#txtKMCCode").val();
    var returnval = 0;
    if ($(".rd-btn-hodd").prop("checked") == true) {
        if (ddlKMCCodeVal === '' || ddlKMCCodeVal === "") {
            $("#errKMCCodeReq").removeAttr("hidden");
            return false;
        }
        else {
            $.ajax({
                url: "/Employee/GetDoctorDetailByKMCCode",
                async: false,
                data: JSON.stringify({ "docKMCCode": ddlKMCCodeVal }),
                type: 'POST',
                cache: false,
                contentType: 'application/json; charset=utf-8',
                processData: false,
                success: function (response) {
                    if (response.DoctorId == 0) {
                        if (ddlKMCCodeVal != "") {
                            $("#errCORRECTKMCCode").removeAttr("hidden");
                            $("#errKMCCodeReq").attr("hidden", true);
                        }

                        else {
                            $("#errCORRECTKMCCode").attr("hidden", true);
                            $("#errKMCCodeReq").removeAttr("hidden");
                        }

                        $("#txtDocName").val("");
                        $("#txtKgid").val("");
                        $("#txtDesignation").val("");
                        $("#txtNameOfOffice").val("");
                        $("#txtPlaceOfOffice").val("");
                        return false;
                    } else {
                        debugger;
                        $("#txtDocName").val(response.DoctorName);
                        $("#txtKgid").val(response.KGIDOfDoctor);
                        $("#txtDesignation").val(response.Designation);
                        $("#txtNameOfOffice").val(response.NameOfHospital);
                        $("#txtPlaceOfOffice").val(response.PlaceOfHospital);
                        $("#hdnDocId").val(response.DoctorId);
                        $("#errCORRECTKMCCode").attr("hidden", true);
                        $("#errKMCCodeReq").attr("hidden", true);
                    }
                }, error: function (response) {
                    alertify.error("Could not get doctor details");
                }
            });
        }
       
       
        return true;
    }
    else {
        if (ddlKMCCodeVal === '' || ddlKMCCodeVal === "") {
            $("#errKMCCodeReq").removeAttr("hidden");
            returnval = 1;
        }
        if ($("#txtDocName").val() === "") {
            $("#errtxtDocName").removeAttr("hidden");
            returnval = 1;
        }
        if ($("#txtDesignation").val() === "") {
            $("#errtxtDesignation").removeAttr("hidden");
            returnval = 1;
        }
        if ($("#txtNameOfOffice").val() === "") {
            $("#errtxtNameOfOffice").removeAttr("hidden");
            returnval = 1;
        }
    }
    if (returnval == 1)
        return false;
    else
        return true;
}