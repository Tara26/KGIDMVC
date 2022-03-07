
$("#btnSubmit").click(function () {
    var isDocUploaded = true;
    if ($("#flupldCWHealthOpinion").val() === "" && $("#flupldedCWHealthOpinion").length === 0) {
        isDocUploaded = false;
        $("#lblCWHealthReq").removeAttr("hidden");
        return false;
    } else {
        $("#lblCWHealthReq").attr("hidden",true);
    }

    if (isDocUploaded) {
        if ($("#hdnHOApplicationStatus").val() === "16") {
            $("#hdnHOApplicationStatus").val(11);
        }
        else {
            $("#hdnHOApplicationStatus").val(13);
        }
        $("#hdnHealthOpinion").val(true);
        alertify.confirm("Are you sure you want to submit the DHS opinion?", function () {
            //$("#frmHOVerDetails").submit();
            var caseWorkerVerifiedDetails = new FormData($("#frmHOVerDetails").get(0));
            $.ajax({
                type: 'POST',
                url: '/SaveAVGCWVData/',
                data: caseWorkerVerifiedDetails,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                success: function (result) {
                    if (result == "1")
                        window.location.href = "/kgid-avg-cw/";
                }
            });
        }).setHeader("Confirm changes?");
    }
});
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

$("#btnSNext").click(function (e) {
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").addClass("show active");

    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").addClass("active");
});

$("#btnUPrevious").click(function (e) {
    $("#scrutiny").addClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Scrutiny").addClass("active");
    $("#nav-tab-Documents").removeClass("active");
})