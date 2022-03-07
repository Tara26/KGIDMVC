$(document).ready(function () {
    $("#txtKGIDNO").val("");
    $("#txtMBLNO").val("");
    $("#txtEmailID").val("");
    $("#divUserDetails").hide();
})
$(".preventSpace").keypress(function (e) {
    if (e.which === 32)
        return false;
});
$("#btnShowDetails").click(function () {
    $(".err").attr("hidden", true);
    $("#txtMBLNO").val("");
    $("#txtEmailID").val("");
    var kgidnoLength = $("#txtKGIDNO").val().length;
    if ($("#txtKGIDNO").val() == "" || $("#txtKGIDNO").val() == null) {
        $("#errKGIDNo").removeAttr('hidden');
    }
    if (kgidnoLength <= 4) {
        $('#errKGIDNo').removeAttr('hidden');
        $('#errKGIDNo').text("Please enter correct KGID number.")
        
    }
    else {
        $("#errKGIDNo").attr('hidden', true);
    }
    if ($(".err:visible").length == 0) {
        $.ajax({
            type: 'get',
            url: '/get-user-details/',
            data: { KGIDNumber: $("#txtKGIDNO").val() },
            success: function (result) {
                if (result.MobileNumber == "0" && result.EmilID == "0") {
                    $("#divUserDetails").hide();
                    alertify.error("KGID Number does not exists in table");
                }
                else {
                    $("#txtMBLNO").val(result.MobileNumber);
                    $("#txtEmailID").val(result.EmilID);
                    $("#divUserDetails").show();
                }
            }
        })
    }
    else {
        $("#divUserDetails").hide();
        return false;
    }
});
$("#btnUpdateDetails").click(function () {
    $(".err").attr("hidden", true);
    if ($("#txtMBLNO").val() == "" || $("#txtMBLNO").val() == null) {
        $("#errMBLNo").removeAttr('hidden'); 
    }
    if ($("#txtMBLNO").val() == "" || $("#txtMBLNO").val() == null) {
        $('#errMBLNo').removeAttr('hidden');
        $('#errMBLNo').text("Please enter mobile number.")
    }
    else if ($("#txtMBLNO").val() != "" || $("#txtMBLNO").val() != null) {
        //$("#errEmpName").removeAttr('hidden');
        var input_val = $("#txtMBLNO").val();
        var inputRGEX = /^(\d{10})$/;
        var inputResult = inputRGEX.test(input_val);
        if (!(inputResult)) {
            $('#errMBLNo').removeAttr('hidden'); 
            $('#errMBLNo').text("Please enter correct mobile number.")
           
        } else {
            if (input_val.charAt(0) == "0") {
                $('#errMBLNo').removeAttr('hidden'); 
                $('#errMBLNo').text("Mobile number cannot start with 0.")
                
            }
            else {
                $('#errMBLNo').attr('hidden',true); 
            }
        }
        //return true;
    }
    if ($("#txtEmailID").val() == "" || $("#txtEmailID").val() == null) {
        $("#errEmailID").removeAttr('hidden'); 
    }
    if ($("#txtEmailID").val() == "" || $("#txtEmailID").val()== null) {
    $('#errEmailID').text("Please enter Email id.")
    $('#errEmailID').removeAttr('hidden'); 
    }
    else if ($("#txtEmailID").val() != "" || $("#txtEmailID").val() != null) {
        var input_eval = $("#txtEmailID").val();
        var inputeRGEX = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        var inputeResult = inputeRGEX.test(input_eval);
        if (!(inputeResult)) {
            $('#errEmailID').text("Email id is not in correct format.")
            $('#errEmailID').removeAttr('hidden'); 
            
        } else {
            $('#errEmailID').attr('hidden',true); 
        }
        //return true;
    }
    if ($(".err:visible").length == 0) {
        $.ajax({
            type: 'post',
            url: '/update-user-details/',
            data: { KGIDNumber: $("#txtKGIDNO").val(), MobileNumber: $("#txtMBLNO").val(), EMailID: $("#txtEmailID").val() },
            success: function (result) {
                if(result == "2") {
                    alertify.warning("Provided Email ID and Mobile number already exists");
                }
                else if (result == "3") {
                    alertify.warning("Provided Mobile number already exists");
                }
                else if (result == "4") {
                    alertify.warning("Provided Email ID already exists");
                }
                else if (result == "-1") {
                    alertify.error("KGID Number does not exists in table");
                }
                else if (result == "1") {
                    alertify.success("Employee details updated successfully");
                    $("#divUserDetails").hide();
                    $("#txtKGIDNO").val("");
                }
                else {
                    alertify.error("Error occured while updating data");
                }
            },
            error: function () {
                alertify.error("Error occured while updating data");
            }
        })
    }
    else {
        return false;
    }
})

$('.ClNum').keyup(function () {
    var input_val = $(this).val();
    var inputRGEX = /^[0-9]*$/;
    var inputResult = inputRGEX.test(input_val);
    if (!(inputResult)) {
        this.value = this.value.replace(/[^0-9\s]/gi, '');
    }

});