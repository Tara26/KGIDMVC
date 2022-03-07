$(document).ready(function () {
    $('#divHealthDetails input:radio').each(function () {
        var name = $(this).attr('name');
        if ($(".cs" + name + "Y").prop("checked") == false && $(".cs" + name + "N").prop("checked") == true) {
            $('#txt' + name).attr("readonly", true);
            $('#txt' + name + 'Doc').attr("disabled", true);
            if (name != "GoodLifeCycle")
                $('#txt' + name).removeClass("validateHDTxtAr");
            else
                $('#txt' + name).removeClass("validateHHDTxtAr");
        } else {
            $('#txt' + name).removeAttr("readonly");
            $('#txt' + name + 'Doc').removeAttr("disabled");
            if (name != "GoodLifeCycle")
                $('#txt' + name).addClass("validateHDTxtAr");
            else
                $('#txt' + name).addClass("validateHHDTxtAr");

        }
    });
   
  
    if ($(".csGoodLifeCycleYes").prop("checked") == true) {
        $('#txtGoodLifeCycle').attr("readonly", true);
        $('#txtGoodLifeCycleDoc').attr("disabled", true);
        $('#txtGoodLifeCycle').removeClass("validateHHDTxtAr");
        $('#txtGoodLifeCycle').removeClass("validateHDTxtAr");
    } else {
        $('#txtGoodLifeCycle').removeAttr("readonly");
        $('#txtGoodLifeCycleDoc').removeAttr("disabled");
        $('#txtGoodLifeCycle').addClass("validateHHDTxtAr");

    }
    
    if ($("#hdnGen").val() == "Female") {
        $("#divMedicalFemaleDetails").show();
    }
    else {
        $("#divMedicalFemaleDetails").hide();
    }
});

function HDcheckboxchange(chkId, txtId) {
    if ($('#' + chkId).val() == "True") {
        $('#' + txtId).removeAttr("readonly");
        $('#' + txtId + 'Doc').removeAttr("disabled");
        $('#' + txtId).addClass("validateHDTxtAr");
     
    } else {
        $('#' + txtId).val("");
        $('#' + txtId).attr("readonly", "true");
        $('#' + txtId + 'Doc').attr("disabled", true);
        $('#' + txtId).removeClass("validateHDTxtAr");
       
        $('#err' + txtId + 'Req').hide();
        $('#err' + txtId + 'Doc').hide();
    }
}

function HDHealthcheckboxchange(chkId, txtId) {
    if ($('#' + chkId).val() == "False") {
        $('#' + txtId).removeAttr("readonly");
        $('#' + txtId + 'Doc').removeAttr("disabled");
        $('#' + txtId).addClass("validateHHDTxtAr");
        $('#err' + txtId + 'Req').hide();
        $('#err' + txtId + 'Doc').hide();
    } else {
        $('#' + txtId).val("");
        $('#' + txtId).attr("readonly", "true");
        $('#' + txtId + 'Doc').attr("disabled", true);
        $('#' + txtId).removeClass("validateHHDTxtAr");
    }
}

function ValidateHealthDetails() {
    $('.err').attr('hidden', true);

    $(".validateHDTxtAr").each(function () {
        if ($(this).val() === '') {
            $(this).next("label.err").removeAttr("hidden");
            $(this).next("label.err").css("display", "block");

        }
    });

    $(".validateHHDTxtAr").each(function () {
        if ($(this).val() === '' && $(".csGoodLifeCycleYes").prop("checked") == false) {
            $(this).next("label.err").removeAttr("hidden");
            $(this).next("label.err").css("display", "block");

        }
    });
    
    $(".txtAr-doc-health-req").each(function () {
        var name = $(this).attr('name').replace('Doc', '');
        if ($(".cs" + name + "Y").prop("checked") == true && $(".cs" + name + "N").prop("checked") == false) {
            if ($(this).val() === '' && $(this).siblings(".doc-uploaded-HD").length === 0) {
                $(this).next("label.err").removeAttr("hidden");
                $(this).next("label.err").css("display", "block");
            }
        }
    });

}

function HDocFileChange(id, errLbl) {
    if ($("#" + id).get(0).files[0] != undefined) {
        var fileType = $("#" + id).get(0).files[0].type;
        if (fileType == 'application/pdf') {
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