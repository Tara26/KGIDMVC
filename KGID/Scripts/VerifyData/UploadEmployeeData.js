
$(document).ready(function () {
    $("#btnSubmit").on('click', function () {
        var IsApplicationRequired = ($("#txtApplicationFormDoc").val() != undefined) ? true : false;
        var IsMedicalRequired = ($("#txtMedicalFormDoc").val() != undefined) ? true : false;

        if (IsApplicationRequired) {
            if ($("#txtApplicationFormDoc").val().length == 0) {
                alertify.alert("Please upload application form").setHeader("warning!!!");
                return false;
            }
        }
       
        if (IsMedicalRequired) {
            if ($("#txtMedicalFormDoc").val().length == 0) {
                alertify.alert("Please upload medical form").setHeader("warning!!!");
                return false;
            }
        }
        alertify.confirm("Are you sure you want to upload documents ?", function () {
            var objUF = new FormData($("#frmUploadDocs").get(0));
            $.ajax({
                type: 'POST',
                url: '/kgid-upload-form-data/',
                data: objUF,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                success: function (result) {
                    debugger;
                    if (result.Result == 0) {

                        alertify.alert("There is no DDO available for this DDO code!", function () {
                            //window.location.href = result.RedirectUrl;
                            $("#txtApplicationFormDoc").val("");
                            $("#txtApplicationFormDoc").text("");
                            $("#txtMedicalFormDoc").val("");
                            $("#txtMedicalFormDoc").text("");
                        }).setHeader("Attention");
                    }
                    else {
                        window.location.href = result.RedirectUrl;
                    }
                }
            });
        }).setHeader("Confirm changes?");
    });
   
    });
    function ApplicationFormUpload() {
        var status = false;
        alertify.confirm('Application Upload !!!', 'Are you sure want to upload documents?', function () {alertify.success('Uploaded'); status = true; }
            , function () {alertify.error('Canceled'); status = false; }); return status;
}
function DocFileChange(id, errLbl) {
    if ($("#" + id).get(0).files[0] != undefined) {
        var fileType = $("#" + id).get(0).files[0].type;
        if (fileType == 'application/pdf') {
            $("#" + errLbl).attr("hidden", true);
        }
        else {
            $("#" + errLbl).removeAttr("hidden");
            $("#" + errLbl).text("Please upload document in pdf format");
            $("#" + id).val("");
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