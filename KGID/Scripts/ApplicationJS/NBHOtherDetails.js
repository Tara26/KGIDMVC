$(document).ready(function () {
    $('#divOtherDetails input:radio').each(function () {

        var name = $(this).attr('name');
        if ($(".cs" + name + "Y").prop("checked") == false && $(".cs" + name + "N").prop("checked") == true) {
            $('#txt' + name).attr("readonly", true);
            $('#txt' + name + 'Doc').attr("disabled", true);
           $('#txt' + name).removeClass("validateODTxtAr");
        } else {
            $('#txt' + name).removeAttr("readonly");
            $('#txt' + name + 'Doc').removeAttr("disabled");
            $('#txt' + name).addClass("validateODTxtAr");
          
        }
    });
});

function Hcheckboxchange(chkId, txtId) {

    if ($('#' + chkId).val() == "True") {
        $('#' + txtId).removeAttr("readonly");
        $('#' + txtId + 'Doc').removeAttr("disabled");
        $('#' + txtId).addClass("validateODTxtAr");
     
    } else {
        $('#' + txtId).val("");
        $('#' + txtId).attr("readonly", "true");
        $('#' + txtId + 'Doc').attr("disabled", true);
        $('#' + txtId).removeClass("validateODTxtAr");
      
        $('#err' + txtId + 'Req').hide();
        $('#err' + txtId + 'Doc').hide();
    }
}

function ValidateOtherDetails() {
    
    $('.err').attr('hidden', true);

    $(".validateODTxtAr").each(function () {
        if ($(this).val() === '') {
            $(this).next("label.err").removeAttr("hidden");
            $(this).next("label.err").css("display", "block");
        }
    });

   
}
function ODocFileChange(id, errLbl) {
    $('#'+id).on('change', function () {
        Object.values(this.files).forEach(function (file) {
            console.log(`Type: ${file.type}`);
            if (file.type == 'application/pdf') {
                $("#" + errLbl).attr("hidden", true);
            }
            else {
                //alert('Wrong type!! Upload Pdf file only..')
                $("#" + errLbl).removeAttr("hidden");
                $("#" + errLbl).text("Please upload document in pdf format");
                $("#" + id).val("");
                return false;
            }
        })
    })
    //var checkpdf = $("#" + id).val().split('.').includes('pdf')
    //if (!checkpdf) {
    //    $("#" + errLbl).removeAttr("hidden");
    //    $("#" + errLbl).text("Please upload document in pdf format");
    //    $("#" + id).val("");
    //    return false;
    //}
    //else {
    //    $("#" + errLbl).attr("hidden", true);
    //}
    if ($("#" + id).get(0).files[0] != undefined) {
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


function ODocFileSize(id, errLbl) {
    debugger;
    //$('#' + id).on('change', function () {
    //    Object.values(this.files).forEach(function (file) {
    //        console.log(`Type: ${file.type}`);
    //        if (file.type == 'application/pdf') {
    //            $("#" + errLbl).attr("hidden", true);
    //        }
    //        else {
    //            //alert('Wrong type!! Upload Pdf file only..')
    //            $("#" + errLbl).removeAttr("hidden");
    //            $("#" + errLbl).text("Please upload document in pdf format");
    //            $("#" + id).val("");
    //            return false;
    //        }
    //    })
    //})
    //var checkpdf = $("#" + id).val().split('.').includes('pdf')
    //if (!checkpdf) {
    //    $("#" + errLbl).removeAttr("hidden");
    //    $("#" + errLbl).text("Please upload document in pdf format");
    //    $("#" + id).val("");
    //    return false;
    //}
    //else {
    //    $("#" + errLbl).attr("hidden", true);
    //}
    if ($("#" + id).get(0).files[0] != undefined) {
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