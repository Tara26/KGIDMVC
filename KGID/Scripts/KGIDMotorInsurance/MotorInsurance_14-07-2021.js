$(document).ready(function () {
    var ddlTypeofCover = $('#ddlTypeofCover').val();
    var ddlCategory = $('#ddlVehCat').val();
    var ddlTypeofCoverName = $('select#ddlTypeofCover option:selected').text();
    if (ddlTypeofCover == "2") {
        //alertify.success("P")
        var txttotamntValue = document.getElementById('txttotamnt').value || 0;
        var txtODValue = document.getElementById('txtowndamage').value || 0;
        var txtPLValue = document.getElementById('txtpremiumliability').value || 0;
        var DepreciationValue = $("#txtVDDepreciation").val();
        //PrevoiusHistoryData
        var PHNCBValue = 0;
        var PHMalusValue = 0;
        if (document.getElementById('rbtnVPurchaseTypYes').checked) {
            PHNCBValue = document.getElementById('txtPHncb').value || 0;
            PHMalusValue = document.getElementById('txtPHmalus').value || 0;
            //alertify.alert(PHNCBValue + '-' + PHMalusValue);
            //if ($("#txtDonatedEmissionCertificateDoc").val().length == 0) {
            //    alertify.alert("Please upload Emission Certificate").setHeader("Warning!!!");
            //    return false;
            //}
        }
        //
        var txtODAdditionAmtValue = document.getElementById('txtAdditionalamt').value || 0;
        var txtODgovDiscntValue = document.getElementById('txtGovDiscount').value || 0;
        var txtPLgovDiscountValue = document.getElementById('txtPLGovDiscount').value || 0;
        var txtPLDriverAmtValue = document.getElementById('txtPLDriverAmt').value || 0;
        var txtPLPassengerAmtValue = document.getElementById('txtPLPassengerAmt').value || 0;

        //IDV Calculation
        //
        var txtidvovamntValue = document.getElementById('txtidvovamnt').value || 0;
        var txtnoneleaccamntValue = document.getElementById('txtnoneleaccamnt').value || 0;
        var txteleaccamntValue = document.getElementById('txteleaccamnt').value || 0;
        var txtsidecaramntValue = document.getElementById('txtsidecaramnt').value || 0;
        var txtcngamntrValue = document.getElementById('txtcngamnt').value || 0;
        var resultA = parseFloat(txtidvovamntValue) + parseFloat(txtnoneleaccamntValue) + parseFloat(txteleaccamntValue) + parseFloat(txtsidecaramntValue) + parseFloat(txtcngamntrValue);
        //var resultround = Math.round(result).toFixed(2);
        //alert(result)
        //var DepreB = Math.round(DepreciationValue).toFixed(2);
        var DepreB = ((parseFloat(resultA)) / 100) * parseFloat(DepreciationValue);
        var ValueC = parseFloat(resultA) - parseFloat(DepreB);
        var TotalPVV = Math.round(ValueC).toFixed(2);
        document.getElementById('txttotamnt').value = ReplaceNumberWithCommas(TotalPVV);
        //PVVAmount = TotalPVV;
        //IDV Calculation End
        //txtbpidv
        var VCategory = $('#ddlVehCatType').val();
        console.log(VCategory)
        if (VCategory == '11' || VCategory == '12') {
            var txtbpidvValue = ((parseFloat(TotalPVV)) / 100) * (parseFloat(txtODValue) + parseFloat(txtODAdditionAmtValue));
            console.log(txtbpidvValue)
        }
        else {
            var txtbpidvValue = ((parseFloat(TotalPVV)) / 100) * (parseFloat(txtODValue));
            console.log(txtbpidvValue)
        }

        if (!isNaN(txtbpidvValue)) {
            var res = Math.round(txtbpidvValue).toFixed(2);
        }
        //txtlgrod
        var txtlgrodValue = ((parseFloat(res)) / 100) * (parseFloat(txtODgovDiscntValue));
        if (!isNaN(txtlgrodValue)) {
            var res1 = Math.round(txtlgrodValue).toFixed(2);
        }
        //txtrebatetotod
        var txtrebatetotodvalue = parseFloat(res) - parseFloat(res1);
        var res2 = Math.round(txtrebatetotodvalue).toFixed(2);

        //txtsubtotlpgod
        var txtsubtotlpgodValue = Math.round(txtrebatetotodvalue).toFixed(2);

        //txth
        var txthValue = ((parseFloat(txtsubtotlpgodValue)) / 100) * 1;

        //txtsubtotextra
        var txtsubtotextraVlaue = Math.round(txtsubtotlpgodValue).toFixed(2);

        //txtaddmalus
        var txtaddmalusValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * PHMalusValue;
        if (!isNaN(txtaddmalusValue)) {
            var resmalus = Math.round(txtaddmalusValue).toFixed(2)
            //alert(res1)
            //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
            //document.getElementById('txtrebatetotod').value = res1;
        }
        //var txttotaftetmalus = parseFloat(txtsubtotextraVlaue) + parseFloat(resmalus);
        //txtlessncb
        var txtlessncbValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * PHNCBValue;
        if (!isNaN(txtlessncbValue)) {
            var res3 = Math.round(txtlessncbValue).toFixed(2)
            //alert(res1)
            //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
            //document.getElementById('txtrebatetotod').value = res1;
        }

        //txtodtot
        var txtodtotValue = parseFloat(txtsubtotextraVlaue) + parseFloat(parseFloat(resmalus)) - parseFloat(res3);
        var res4 = Math.round(txtodtotValue).toFixed(2);

        //B. LIABILITY TO PUBLIC RISK
        //txtlgrlpr 
        var txtlprValue = parseFloat(txtPLValue)
        var res5 = Math.round(txtlprValue).toFixed(2)
        //txtbpidv
        var txtlgrlprValue = ((parseFloat(res5)) / 100) * (parseFloat(txtPLgovDiscountValue));
        var res6 = Math.round(txtlgrlprValue).toFixed(2);
        //txtsubtotlpr
        var txtsubtotlprValue = parseFloat(res5) - parseFloat(res6)
        var res7 = Math.round(txtsubtotlprValue).toFixed(2);

        //txtlpgkitlpr
        var txtcngamntrValue = document.getElementById('txtcngamnt').value || 0;
        var txtlpgkitlprValue = ((parseFloat(txtcngamntrValue)) / 100) * 60;
        var res8 = Math.round(txtlpgkitlprValue).toFixed(2);

        //txtsubtotlpglpr
        var txtsubtotlpglprValue = parseFloat(res7) + parseFloat(res8)
        var res9 = Math.round(txtsubtotlpglprValue).toFixed(2);

        //txtdrlpr
        var res10 = parseFloat(txtPLDriverAmtValue);
        //txtprlpr
        var totpassengeramt = 0;
        if (ddlCategory == 1 || ddlCategory == 2 || ddlCategory == 3 || ddlCategory == 4) {
            var totpassengeramt = parseFloat(txtPLPassengerAmtValue);
        }
        else {
            var noofseats = $('#txtVDSeating').val();
            var totpassengeramt = (parseFloat(noofseats) - 1) * parseFloat(txtPLPassengerAmtValue);
        }
        var res11 = parseFloat(totpassengeramt);

        //txtlprtot
        var txtlprtotValue = (parseFloat(res9) + parseFloat(res10) + parseFloat(res11));
        var res12 = Math.round(txtlprtotValue).toFixed(2);

        //txttotAB
        var txttotABValue = parseFloat(res4) + parseFloat(res12);
        var res13 = Math.round(txttotABValue).toFixed(2);

        //txtgstamt
        var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
        var res14 = Math.round(txtgstamtValue).toFixed(2);

        //txttotalcrpremium
        var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14);
        var res15 = Math.round(txttotalcrpremiumValue).toFixed(2);
        document.getElementById('txtPV').value = ReplaceNumberWithCommas(res15);
        PremiumPayableAmount = res15;
        document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
        $('#txtTotalPremium').change(function () {
            var MDateValue = $("#txtTotalPremium").val();
            document.getElementById('txtPV').value = MDateValue;
            PremiumPayableAmount = res15;

        });
        //Addition Popup For View Premium Details
        $('#idvpercent').text(txtODValue);
        $('#txtbpidv').val(res);
        $('#txtidvsubtot').val(res);
        $('#txtodp').val(res);
        $('#txtlgrod').val(res1);
        $('#txtrebatetotod').val(res4);
        $('#txtsubtotlpgod').val(res4);
        $('#txtsubtotextra').val(res4);
        $('#txtamod').val(resmalus);
        $('#idvpercent').text(idvmaluspercent);
        $('#txtlessncb').val(res3);
        $('#idvpercent').text(idvncbpercent);
        $('#txtodtot').val(res4);

        $('#txtlpr').val(txtPLValue);
        $('#txtlgrlpr').val(res6);
        $('#txtsubtotlpr').val(res7);
        $('#txtsubtotlpglpr').val(res9);
        $('#txtdrlpr').val(txtPLDriverAmtValue);
        if (ddlCategory == 1 || ddlCategory == 2 || ddlCategory == 3 || ddlCategory == 4) {
            $('#txtprrlpr').val(res11);
            $('#txtprlpr').val('');
        }
        else {
            $('#txtprlpr').val(res11);
            $('#txtprrlpr').val('');
        }
        // $('#txtprlpr').val(txtPLPassengerAmtValue);
        $('#txtlprtot').val(res12);
        $('#txtodtotA').val(res4);
        $('#txtlprtotB').val(res12);
        $('#txttotAB').val(res13);

        $('#txtgstamt').val(res14);
        $('#txtTotalPremiumAmt').val(res15);

        //
    }
    else if (ddlTypeofCover == "1") {
        //alertify.success("L")
        var txttotamntValue = document.getElementById('txttotamnt').value || 0;
        var txtODValue = document.getElementById('txtowndamage').value || 0;
        var txtPLValue = document.getElementById('txtpremiumliability').value || 0;
        var DepreciationValue = $("#txtVDDepreciation").val();
        //
        var txtODAdditionAmtValue = document.getElementById('txtAdditionalamt').value || 0;
        var txtODgovDiscntValue = document.getElementById('txtGovDiscount').value || 0;
        var txtPLgovDiscountValue = document.getElementById('txtPLGovDiscount').value || 0;
        var txtPLDriverAmtValue = document.getElementById('txtPLDriverAmt').value || 0;
        var txtPLPassengerAmtValue = document.getElementById('txtPLPassengerAmt').value || 0;

        //IDV Calculation
        //
        var txtidvovamntValue = document.getElementById('txtidvovamnt').value || 0;
        var txtnoneleaccamntValue = document.getElementById('txtnoneleaccamnt').value || 0;
        var txteleaccamntValue = document.getElementById('txteleaccamnt').value || 0;
        var txtsidecaramntValue = document.getElementById('txtsidecaramnt').value || 0;
        var txtcngamntrValue = document.getElementById('txtcngamnt').value || 0;
        var resultA = parseFloat(txtidvovamntValue) + parseFloat(txtnoneleaccamntValue) + parseFloat(txteleaccamntValue) + parseFloat(txtsidecaramntValue) + parseFloat(txtcngamntrValue);
        //var resultround = Math.round(result).toFixed(2);
        //alert(result)
        //var DepreB = Math.round(DepreciationValue).toFixed(2);
        var DepreB = ((parseFloat(resultA)) / 100) * parseFloat(DepreciationValue);
        var ValueC = parseFloat(resultA) - parseFloat(DepreB);
        var TotalPVV = Math.round(ValueC).toFixed(2);
        document.getElementById('txttotamnt').value = ReplaceNumberWithCommas(TotalPVV);
        //PVVAmount = TotalPVV;
        //IDV Calculation End
        //txtbpidv


        //txtodtot
        var res4 = 0;

        //B. LIABILITY TO PUBLIC RISK
        //txtlgrlpr 
        var txtlprValue = parseFloat(txtPLValue)
        var res5 = Math.round(txtlprValue).toFixed(2)
        //txtbpidv
        var txtlgrlprValue = ((parseFloat(res5)) / 100) * (parseFloat(txtPLgovDiscountValue));
        var res6 = Math.round(txtlgrlprValue).toFixed(2);
        //txtsubtotlpr
        var txtsubtotlprValue = parseFloat(res5) - parseFloat(res6)
        var res7 = Math.round(txtsubtotlprValue).toFixed(2);

        //txtlpgkitlpr
        var txtcngamntrValue = document.getElementById('txtcngamnt').value || 0;
        var txtlpgkitlprValue = ((parseFloat(txtcngamntrValue)) / 100) * 60;
        var res8 = Math.round(txtlpgkitlprValue).toFixed(2);

        //txtsubtotlpglpr
        var txtsubtotlpglprValue = parseFloat(res7) + parseFloat(res8)
        var res9 = Math.round(txtsubtotlpglprValue).toFixed(2);

        //txtdrlpr
        var res10 = parseFloat(txtPLDriverAmtValue);
        //txtprlpr
        var totpassengeramt = 0;
        if (ddlCategory == 1 || ddlCategory == 2 || ddlCategory == 3 || ddlCategory == 4) {
            var totpassengeramt = parseFloat(txtPLPassengerAmtValue);
        }
        else {
            var noofseats = $('#txtVDSeating').val();
            var totpassengeramt = (parseFloat(noofseats) - 1) * parseFloat(txtPLPassengerAmtValue);
        }
        var res11 = parseFloat(totpassengeramt);

        //txtlprtot
        var txtlprtotValue = (parseFloat(res9) + parseFloat(res10) + parseFloat(res11));
        var res12 = Math.round(txtlprtotValue).toFixed(2);

        //txttotAB
        var txttotABValue = parseFloat(res4) + parseFloat(res12);
        var res13 = Math.round(txttotABValue).toFixed(2);

        //txtgstamt
        var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
        var res14 = Math.round(txtgstamtValue).toFixed(2);

        //txttotalcrpremium
        var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14);
        var res15 = Math.round(txttotalcrpremiumValue).toFixed(2);
        document.getElementById('txtPV').value = ReplaceNumberWithCommas(res15);
        PremiumPayableAmount = res15;
        document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
        $('#txtTotalPremium').change(function () {
            var MDateValue = $("#txtTotalPremium").val();
            document.getElementById('txtPV').value = MDateValue;
            PremiumPayableAmount = res15;

        });
        //Addition Popup For View Premium Details
        //$('#txtbpidv').val(res);
        //$('#txtidvsubtot').val(res);
        //$('#txtlgrod').val(res1);
        //$('#txtrebatetotod').val(res4);
        //$('#txtsubtotlpgod').val(res4);
        //$('#txtsubtotextra').val(res4);
        //$('#txtodtot').val(res4);

        $('#txtlpr').val(txtPLValue);
        $('#txtlgrlpr').val(res6);
        $('#txtsubtotlpr').val(res7);
        $('#txtsubtotlpglpr').val(res9);
        $('#txtdrlpr').val(txtPLDriverAmtValue);
        if (ddlCategory == 1 || ddlCategory == 2 || ddlCategory == 3 || ddlCategory == 4) {
            $('#txtprrlpr').val(res11);
            $('#txtprlpr').val('');
        }
        else {
            $('#txtprlpr').val(res11);
            $('#txtprrlpr').val('');
        }
        //$('#txtprlpr').val(txtPLPassengerAmtValue);
        $('#txtlprtot').val(res12);
        $('#txtodtotA').val(res4);
        $('#txtlprtotB').val(res12);
        $('#txttotAB').val(res13);

        $('#txtgstamt').val(res14);
        $('#txtTotalPremiumAmt').val(res15);

        //
    }
    else if (ddlTypeofCover == "3") {
        //alertify.success("O")
        //alertify.success("P")
        var txttotamntValue = document.getElementById('txttotamnt').value || 0;
        var txtODValue = document.getElementById('txtowndamage').value || 0;
        var txtPLValue = document.getElementById('txtpremiumliability').value || 0;
        var DepreciationValue = $("#txtVDDepreciation").val();
        //
        var txtODAdditionAmtValue = document.getElementById('txtAdditionalamt').value || 0;
        var txtODgovDiscntValue = document.getElementById('txtGovDiscount').value || 0;
        var txtPLgovDiscountValue = document.getElementById('txtPLGovDiscount').value || 0;
        var txtPLDriverAmtValue = document.getElementById('txtPLDriverAmt').value || 0;
        var txtPLPassengerAmtValue = document.getElementById('txtPLPassengerAmt').value || 0;

        //IDV Calculation
        //
        var txtidvovamntValue = document.getElementById('txtidvovamnt').value || 0;
        var txtnoneleaccamntValue = document.getElementById('txtnoneleaccamnt').value || 0;
        var txteleaccamntValue = document.getElementById('txteleaccamnt').value || 0;
        var txtsidecaramntValue = document.getElementById('txtsidecaramnt').value || 0;
        var txtcngamntrValue = document.getElementById('txtcngamnt').value || 0;
        var resultA = parseFloat(txtidvovamntValue) + parseFloat(txtnoneleaccamntValue) + parseFloat(txteleaccamntValue) + parseFloat(txtsidecaramntValue) + parseFloat(txtcngamntrValue);
        //var resultround = Math.round(result).toFixed(2);
        //alert(result)
        //var DepreB = Math.round(DepreciationValue).toFixed(2);
        var DepreB = ((parseFloat(resultA)) / 100) * parseFloat(DepreciationValue);
        var ValueC = parseFloat(resultA) - parseFloat(DepreB);
        var TotalPVV = Math.round(ValueC).toFixed(2);
        document.getElementById('txttotamnt').value = ReplaceNumberWithCommas(TotalPVV);
        //PVVAmount = TotalPVV;
        //IDV Calculation End
        //txtbpidv
        var VCategory = $('#ddlVehCatType').val();
        console.log(VCategory)
        if (VCategory == '11' || VCategory == '12') {
            var txtbpidvValue = ((parseFloat(TotalPVV)) / 100) * (parseFloat(txtODValue) + parseFloat(txtODAdditionAmtValue));
            console.log(txtbpidvValue)
        }
        else {
            var txtbpidvValue = ((parseFloat(TotalPVV)) / 100) * (parseFloat(txtODValue));
            console.log(txtbpidvValue)
        }

        if (!isNaN(txtbpidvValue)) {
            var res = Math.round(txtbpidvValue).toFixed(2);
        }
        //txtlgrod
        var txtlgrodValue = ((parseFloat(res)) / 100) * (parseFloat(txtODgovDiscntValue));
        if (!isNaN(txtlgrodValue)) {
            var res1 = Math.round(txtlgrodValue).toFixed(2);
        }
        //txtrebatetotod
        var txtrebatetotodvalue = parseFloat(res) - parseFloat(res1);
        var res2 = Math.round(txtrebatetotodvalue).toFixed(2);

        //txtsubtotlpgod
        var txtsubtotlpgodValue = Math.round(txtrebatetotodvalue).toFixed(2);

        //txth
        var txthValue = ((parseFloat(txtsubtotlpgodValue)) / 100) * 1;

        //txtsubtotextra
        var txtsubtotextraVlaue = Math.round(txtsubtotlpgodValue).toFixed(2);

        //txtlessncb
        var txtlessncbValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * 0;
        if (!isNaN(txtlessncbValue)) {
            var res3 = Math.round(txtlessncbValue).toFixed(2);
        }

        //txtodtot
        var txtodtotValue = parseFloat(txtsubtotextraVlaue) - parseFloat(res3);
        var res4 = Math.round(txtodtotValue).toFixed(2);

        //B. LIABILITY TO PUBLIC RISK

        var res12 = 0;

        //txttotAB
        var txttotABValue = parseFloat(res4) + parseFloat(res12);
        var res13 = Math.round(txttotABValue).toFixed(2);

        //txtgstamt
        var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
        var res14 = Math.round(txtgstamtValue).toFixed(2);

        //txttotalcrpremium
        var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14);
        var res15 = Math.round(txttotalcrpremiumValue).toFixed(2);
        document.getElementById('txtPV').value = ReplaceNumberWithCommas(res15);
        PremiumPayableAmount = res15;
        document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
        $('#txtTotalPremium').change(function () {
            var MDateValue = $("#txtTotalPremium").val();
            document.getElementById('txtPV').value = MDateValue;
            PremiumPayableAmount = res15;

        });
        //Addition Popup For View Premium Details
        $('#idvpercent').text(txtODValue);
        $('#txtbpidv').val(res);
        $('#txtidvsubtot').val(res);
        $('#txtodp').val(res);
        $('#txtlgrod').val(res1);
        $('#txtrebatetotod').val(res4);
        $('#txtsubtotlpgod').val(res4);
        $('#txtsubtotextra').val(res4);
        $('#txtodtot').val(res4);

        //$('#txtlpr').val(txtPLValue);
        //$('#txtlgrlpr').val(res6);
        //$('#txtsubtotlpr').val(res7);
        //$('#txtsubtotlpglpr').val(res9);
        //$('#txtdrlpr').val(txtPLDriverAmtValue);
        //$('#txtprlpr').val(txtPLPassengerAmtValue);
        //$('#txtlprtot').val(res12);
        $('#txtodtotA').val(res4);
        $('#txtlprtotB').val(res12);
        $('#txttotAB').val(res13);

        $('#txtgstamt').val(res14);
        $('#txtTotalPremiumAmt').val(res15);

        //
    }

    $("#ApplicationTab-t-5").on("click", function (event) {
        GetMIDeclaration();
    });

});

//Motor insurance Vehicle details validations rules


function EnableDisableNexrButton() {
    if ($("#spnMIReferanceNo").text() == "") {
        $("a[href*='next']").parent("li").hide();
    }
    else {
        $("a[href*='next']").parent("li").show();
    }
}

function ValidateSeatingCubicWeightCapacity(id) {    debugger;    var seatId = $('#txtVDSeating').val();    var cubicId = $('#txtVDCubicCapacity').val();    var weightId = $('#txtVDWeight').val();    var errCount = 0;    var seatId = parseInt(seatId);    var cubicId = parseInt($('#txtVDCubicCapacity').val());    var weightId = parseFloat($('#txtVDWeight').val());    var categoryId = $("#ddlVehCat").val();    //categoryId = parseInt($(categoryId).val());      if (categoryId == 1 || categoryId == 2 || categoryId == 3 || categoryId == 4) {        if (cubicId == 0) {            $('#errVDCubicCapacityReq').removeAttr('hidden');            $('#errVDCubicCapacityReq').text("Cubic capacity cannot be 0.")            //return false;            errorCount++;        }        else if (cubicId >= 999) {            //alert("cannot exceed 999 CC");            $('#errVDCubicCapacityReq').removeAttr('hidden');            $('#errVDCubicCapacityReq').text("Cubic capacity cannot exceed 999 CC");            $('#txtVDCubicCapacity').attr("readonly", false);            // return false;            errCount++;        }        else {            $('#errVDCubicCapacityReq').attr('hidden', true);        }        if (categoryId == 1 || categoryId == 2) {            if (seatId > 3) {                $('#errVDSeatingReq').removeAttr('hidden');                $('#errVDSeatingReq').text("Seating capacity cannot be greater than 3");                $('#txtVDSeating').attr("readonly", false);                //return false;                errCount++;            }            else {                $('#errVDSeatingReq').attr('hidden', true);                //return false;                errCount++;            }        }        else {            if (seatId > 5) {                $('#errVDSeatingReq').removeAttr('hidden');                $('#errVDSeatingReq').text("Seating capacity cannot be greater than 5");                $('#txtVDSeating').attr("readonly", false);                //return false;                errCount++;            }            else {                $('#errVDSeatingReq').attr('hidden', true);            }        }    }    if (categoryId == 5 || categoryId == 6) {        if (weightId >= 99999) {            //alert("cannot exceed 99999 kgs");            $('#errVDVWeightReq').removeAttr('hidden');            $('#errVDVWeightReq').text("Weight cannot exceed 99999 kgs")            //return false;            errCount++;        }        else {            $('#errVDVWeightReq').attr('hidden', true);        }    }    if (categoryId == 7 || categoryId == 8) {        if (weightId > 1200) {            //alert("cannot exceed 1200 kgs");            $('#errVDVWeightReq').removeAttr('hidden');            $('#errVDVWeightReq').text("Weight cannot exceed 1200 kgs")            //return false;            errCount++;        }        else {            $('#errVDVWeightReq').attr('hidden', true);        }    }    if (categoryId == 9 || categoryId == 10) {        if (seatId > 6) {            //alert("cannot exceed 6 passengers");            $('#errVDSeatingReq').removeAttr('hidden');            $('#errVDSeatingReq').text(" cannot exceed 6 passengers")            $('#txtVDSeating').attr("readonly", false);            // return false;            errCount++;        }        else {            $('#errVDSeatingReq').attr('hidden', true);        }        if (cubicId > 999) {            //alert("cannot exceed 999 CC");            $('#errVDCubicCapacityReq').removeAttr('hidden');            $('#errVDCubicCapacityReq').text("Cubic capacity cannot exceed 999 CC");            $('#txtVDCubicCapacity').attr("readonly", false);            // return false;            errCount++;        }        else {            $('#errVDCubicCapacityReq').attr('hidden', true);        }    }    if (categoryId == 11 || categoryId == 12) {        if (seatId > 6) {            // alert("cannot exceed 6 passengers");            $('#errVDSeatingReq').removeAttr('hidden');            $('#errVDSeatingReq').text(" cannot exceed 6 passengers")            $('#txtVDSeating').attr("readonly", false);            //return false;            errCount++;        }        else {            $('#errVDSeatingReq').attr('hidden', true);        }        if (cubicId > 99999) {            //alert("cannot exceed 99999 CC");            $('#errVDCubicCapacityReq').removeAttr('hidden');            $('#errVDCubicCapacityReq').text("Cubic capacity cannot exceed 99999 CC");            $('#txtVDCubicCapacity').attr("readonly", false);            //return false;            errCount++;        }        else {            $('#errVDCubicCapacityReq').attr('hidden', true);        }    }    if (categoryId == 13) {        if (seatId <= 6) {            //alert("cannot be less than 6 passengers and more than 999");            $('#txtVDSeating').attr("readonly", false);            $('#errVDSeatingReq').removeAttr('hidden');            $('#errVDSeatingReq').text("cannot be less than 7 passengers and more than 999")            //return false;            errCount++;        }        else {            $('#errVDSeatingReq').attr('hidden', true);        }    }    if (categoryId == 14) {        if ((seatId - 18) * (seatId - 36) > 0)        // if ((seatId < 17 && !(seatId > 36)) || (!(seatId < 17) && seatId > 36))        {            //alert("cannot be less than 6 passengers and more than 999");            $('#errVDSeatingReq').removeAttr('hidden');            $('#errVDSeatingReq').text("cannot be less than 18  and greater than 36 passengers")            $('#txtVDSeating').attr("readonly", false);            //return false;            errCount++;        }        else {            $('#errVDSeatingReq').attr('hidden', true);        }    }    if (categoryId == 15) {        if ((seatId - 6) * (seatId - 17) > 0) {            //alert("cannot be less than 6 passengers and more than 17");            $('#errVDSeatingReq').removeAttr('hidden');            $('#errVDSeatingReq').text("cannot be less than 6  and more than 17 passengers")            $('#txtVDSeating').attr("readonly", false);            //return false;            errCount++;        }        else {            $('#errVDSeatingReq').attr('hidden', true);        }    }    if (errCount == 0) return true;    else return false;}

function ValidateVehicleDetails() {    var currentyear = new Date().getFullYear();    var currentmonth = new Date().getMonth() + 1;    $('.err').attr('hidden', true);    var ddlVehRTO = $('#ddlVDRTO').val();    var txtChasisNo = $('#txtVDChasisNo').val();    var txtEngine = $('#txtVDEngine').val();    var ddlVehicleMake = $('#ddlVDVehicleMake').val();    var ddlVehicleManufacture = $('#ddlVDVehicleManufacture').val();    var ddlVehicleCatType = $('#ddlVehicleCatType').val();    var ddlVehClassType = $('#ddlVehClassType').val();    var ddlManufacturerYear = $('#ddlVDManufacturerYear').val();    var ddlFuelType = $('#ddlVDFuelType').val();    var ddlVehicleType = $('#ddlVehType').val();    var ddlVehilceSubType = $('#ddlVehSubType').val();    var ddlvehivleCatergory = $('#ddlVehCat').val();    var txtSeat = $('#txtVDSeating').val();    var txtModel = $('#txtVDTypeOfModel').val();    var ddlmonth = $('#ddlManufacturMonth').val();    var cubicId = $('#txtVDCubicCapacity').val();    var weightId = $('#txtVDWeight').val();    var errorCount = 0;    if (txtChasisNo.trim() === null || txtChasisNo.trim() === "") {        $("#errVDChassisNoReq").removeAttr("hidden");        //return false;        errorCount++;    }    else {        if (txtChasisNo.length < 8 || txtChasisNo.length > 17) {            $("#errVDChassisNoReq").removeAttr("hidden");            //return false;            errorCount++;        }        else {            var isNumber = /^\d+$/.test(txtChasisNo);            var isString = (!/[^a-zA-Z]/.test(txtChasisNo))            if (isString == true || isNumber == true) {                $("#errVDChassisNoReq").removeAttr("hidden");                //return false;                errorCount++;            }        }    }    if (ddlVehRTO.trim() === "" || ddlVehRTO.trim() === 'undefined') {        $("#errVDRTOReq").removeAttr("hidden");        //return false;        errorCount++;    }    if (txtEngine.trim() === null || txtEngine.trim() === "") {        $("#errVDEngineNOReq").removeAttr("hidden");        //return false;        errorCount++;    }    else {        if (txtEngine.length < 10 && txtEngine.length >= 15) {            $("#errVDEngineNOReq").removeAttr("hidden");            // return false;            errorCount++;        }        else {            var isNumber = /^\d+$/.test(txtEngine);            var isString = (!/[^a-zA-Z]/.test(txtEngine))            if (isString == true || isNumber == true) {                $("#errVDEngineNOReq").removeAttr("hidden");                //return false;                errorCount++;            }        }    }    //if (cubicId == 0 ) {    //    $('#errVDCubicCapacityReq').removeAttr('hidden');    //    $('#errVDCubicCapacityReq').text("Cubic capacity cannot be 0.")    //    //return false;    //    errorCount++;    //}    if (txtSeat.trim() === null || txtSeat.trim() === "") {        $("#errVDSeatingReq").removeAttr("hidden");        $('#errVDSeatingReq').text("Please enter seating capacity.")        //return false;        errorCount++;    }    else if (txtSeat == 0) {        $('#errVDSeatingReq').removeAttr('hidden');        $('#errVDSeatingReq').text("Seating capacity cannot be 0.")        // return false;        errorCount++;    }    if (weightId == 0) {        $('#errVDVWeightReq').removeAttr('hidden');        $('#errVDVWeightReq').text("Weight cannot be 0.")        //return false;        errorCount++;    }    if (ddlVehicleCatType === "" || ddlVehicleCatType === 'undefined') {        $("#errVDMakeReq").removeAttr("hidden");        //return false;        errorCount++;    }    if (ddlVehicleManufacture.trim() === "-1" || ddlVehicleManufacture.trim() === "") {        $("#errVDManuReq").removeAttr("hidden");        // return false;        errorCount++;    }    if (txtModel.trim() === null || txtModel.trim() === "") {        $("#errVDModelTypeReq").removeAttr("hidden");        $('#errVDModelTypeReq').text("Please enter type of model.")        // return false;        errorCount++;    }    else {        var isNumber = /^\d+$/.test(txtModel);        var isString = (!/[^a-zA-Z]/.test(txtModel))        if (isString == true || isNumber == true) {            $("#errVDModelTypeReq").removeAttr("hidden");            //return false;            errorCount++;        }    }    if (ddlmonth.trim() === 0 || ddlmonth.trim() === "") {        $("#errVDManufactureReq").removeAttr("hidden");        //return false;        errorCount++;    }    if (ddlManufacturerYear.trim() === 0 || ddlManufacturerYear.trim() === "") {        $("#errVDManufactureryearReq").removeAttr("hidden");        //return false;        errorCount++;    }    else if ($('#ddlVDManufacturerYear option:selected').text() == currentyear) {        debugger;        if ($('#ddlManufacturMonth option:selected').val() > currentmonth) {            $('#errVDManufactureReq').removeAttr("hidden");            $('#errVDManufactureReq').text("Please select valid month.");            //return false;            errorCount++;        }        else {            $('#errVDManufactureReq').attr("hidden", true);        }    }    if (ddlFuelType.trim() === 0 || ddlFuelType.trim() === "") {        $("#errVDFuelTypeReq").removeAttr("hidden");        //return false;        errorCount++;    }    if (ddlVehicleType.trim() === "" || ddlVehicleType.trim() === "") {        $("#errVDVTypeReq").removeAttr("hidden");        //return false;        errorCount++;    }    if (ddlVehilceSubType.trim() === "-1" || ddlVehilceSubType.trim() === "") {        $("#errVDVSTypeReq").removeAttr("hidden");        // return false;        errorCount++;    }    if (ddlvehivleCatergory === "-1" || ddlvehivleCatergory === "") {        $("#errVDVCTypeReq").removeAttr("hidden");        //return false;        errorCount++;    }    if (ddlVehClassType === "-1" || ddlVehClassType === "") {        $("#errVDVClassTypeReq").removeAttr("hidden");        //return false;        errorCount++;    }    if (errorCount == 0) return true;    else return false;}
function checkEngineNoExists() {
    debugger;
    var vehiclechassisNo = {
        'mivd_engine_no': $("#txtVDEngine").val(),
        'mivd_chasis_no': null,
    };
    $.ajax({
        url: '/MotorInsurance/CheckVehicleExists',
        //data:JSON.stringify({ vehicleDetails: $("#txtVDChasisNo").val() }),
        data: JSON.stringify(vehiclechassisNo),

        type: 'POST',
        async: false,
        cache: false,
        contentType: 'application/json; charset=utf-8',
        processData: false,
        success: function (response) {
            if (response == 1) {
                alertify.confirm("Engine No. already exists.", function () {
                }).setHeader("Attention");
                $("#txtVDEngine").val("");
            }
            else if (response == 0) {

            }
        },
        error: function (response) {

        }
    });
}

function ValidateIDVDetails() {
    $('.err').attr('hidden', true);
    if ($('#txtidvovamnt').val() === "" || $('#txtidvovamnt').val() === 0) {
        $("#errIsidvvalue").removeAttr("hidden");
        return false;
    }
    else {
        $("#errIsidvvalue").attr("hidden");
        return true;
    }
}
function ApplicationDetailsSave() {
    debugger;
    var errcount = ValidateProposerDetails();
    if (errcount == 0) {
        var StepNo = $("#ApplicationTab ul li.current").find('a').attr('id');

        if (StepNo == "ApplicationTab-t-0") {

            if ($('.err:visible').length === 0) {
                return SaveMIProposerDetails();
            }
            else {
                return false;
            }
        }
        if (StepNo == "ApplicationTab-t-1") {
            $("a[href*='next']").parent("li").hide();
            ValidateVehicleDetails();
            ValidateSeatingCubicWeightCapacity("ddlVehCat");
            if ($('.err:visible').length === 0) {
                return SaveVehicleDetails();
            }
            else {
                return false;
            }

        }
        if (StepNo == "ApplicationTab-t-2") {

            //ValidateVehicleDetails();

            if ($('.err:visible').length === 0) {
                return SaveOtherDetails();
            }
            else {
                return false;
            }

        }
        if (StepNo == "ApplicationTab-t-3") {

            //ValidateVehicleDetails();

            if ($('.err:visible').length === 0) {

                return SavePreviousHistoryDetails();
            }
            else {
                return false;
            }

        }
        if (StepNo == "ApplicationTab-t-4") {

            //ValidateVehicleDetails();

            if ($('.err:visible').length === 0) {
                return SaveIDVDetails();
            }
            else {
                return false;
            }
        }
        if (StepNo == "ApplicationTab-t-5") {

            var isChecked = $("#miDeclaration").is(":checked");
            if (isChecked) {
                $('#ApplicationTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
                alertify.success("Declartion details saved successful ")
                return true;
            }
            else {
                alertify.alert("Please agree to the terms and condition.").setHeader("Warning !!!");
                return false;
            }

        }
    }
}

function ValidateProposerDetails() {
    var ddlTypeofCoverId = $('#ddlTypeofCover').val();
    var errCount = 0;
    $('.err').attr('hidden', true);

    if ($("#txtPEmail").val() == "") {
        $("#errVDEmailReq").removeAttr("hidden");
        errCount++;
    }
    else {

        var input_eval = $("#txtPEmail").val();
        var inputeRGEX = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        var inputeResult = inputeRGEX.test(input_eval);
        if (!(inputeResult)) {
            $('#errVDEmailReq').text("Email id is not in correct format.")
            $('#errVDEmailReq').removeAttr('hidden');
            errCount++;
        } else {
            $('#errVDEmailReq').attr('hidden', true);
            $('#txtPAddress').prop('readonly', true);
        }


    }
    if ($("#txtPAddress").val() == "") {
        $("#errVDAddressReq").removeAttr("hidden");
        $('#txtPAddress').prop('readonly', false);
        // return false;
        errCount++;
    }
    else {
        $('#txtPAddress').prop('readonly', true);
    }
    if ($("#txtPPinCode").val() == "0") {
        $("#errVDPinCodeReq").removeAttr("hidden");
        // return false;
        errCount++;
    }
    else {
        if ($("#txtPPinCode").val().length < 6) {
            $("#errVDPinCodeReq").removeAttr("hidden");
            $("#errVDPinCodeReq").text("Pincode should be 6 digits.")
            $('#txtPPinCode').prop('readonly', false);
            //return false;
            errCount++;
        }
        else {
            if ($("#txtPPinCode").val().indexOf('0') == "0") {
                $("#errVDPinCodeReq").removeAttr("hidden");
                $("#errVDPinCodeReq").text("Pincode cannot start with 0.")
                $('#txtPPinCode').prop('readonly', false);
                //return false;
                errCount++;
            }
            else {
                $('#txtPPinCode').prop('readonly', true);
            }

        }

    }
    //if ($("#txtPFaxNo").val() == "0") {
    //    $("#errVDFaxReq").removeAttr("hidden");
    //    $("#txtPFaxNo").prop('readonly', false);
    //    //return false;
    //    errCount++;
    //}
    //else {

    //    if ($("#txtPFaxNo").val().length < 9) {
    //        $("#errVDFaxReq").removeAttr("hidden");
    //        $("#errVDFaxReq").text("Fax No. should be 10 digitss.")
    //        $("#txtPFaxNo").prop('readonly', false);
    //        //return false;
    //        errCount++;
    //    }
    //    else {
    //        if ($("#txtPFaxNo").val().indexOf('0') == "0") {
    //            $("#errVDFaxReq").removeAttr("hidden");
    //            $("#errVDFaxReq").text("Fax No. cannot start with 0.")
    //            $('#txtPFaxNo').prop('readonly', false);
    //            //return false;
    //            errCount++;
    //        }
    //        else {
    //            $('#txtPFaxNo').prop('readonly', true);
    //        }
    //    }
    //}
    if (ddlTypeofCoverId === 0 || ddlTypeofCoverId === "") {
        $("#errddlTypeofCoverReq").removeAttr("hidden");
        // return false;
        errCount++;
    }
    return errCount;
}
function SaveMIProposerDetails() {
    debugger;
    // ValidateProposerDetails();
    var eid = $("#divApplicationMI").data("eid");
    var PageType = $('#hdnPagetype').val();
    if ($('.err:visible').length === 0) {
        var ViewModel = {
            "mipd_pagetype": PageType, "mipd_address": $("#txtPAddress").val(), "mipd_fax_no": $("#txtPFaxNo").val(), "mipd_email": $("#txtPEmail").val(),
            "mipd_pincode": $("#txtPPinCode").val(), "mipd_old_application_Ref_number": $("#txtPrevRefNo").val(), "mipd_type_of_cover_id": $('#ddlTypeofCover').val()
        }
        var submitUrl = $("#divMIProposerDetails").data("submit-url");
        $.ajax({
            url: submitUrl,
            data:
                JSON.stringify({ "objMIPDetails": ViewModel }),

            type: 'POST',
            async: false,
            cache: false,
            contentType: 'application/json; charset=utf-8',
            processData: false,
            success: function (response) {
                if (response.IsSuccess) {
                    if (response.ReferenceNo != 0)
                        $('#spnMIReferanceNo').text(response.ReferenceNo)
                    //$('#imgBQRCode').attr('src', response.QRCodeImage);
                    document.getElementById("imgBQRCode").src = response.QRCodeImage
                    alertify.success("Proposer details saved successful");
                    PDserverResponse = true;
                }
                else {
                    alertify.error("Could not save Proposer Details");
                }
                EnableDisableNexrButton();
            }, error: function (response) {
                alertify.error("Could not save Proposer Details");
            }
        });
    }

    return PDserverResponse;
}
function SaveVehicleDetails() {
    ValidateVehicleDetails();
    ValidateSeatingCubicWeightCapacity("ddlVehCat");
    if ($('.err:visible').length === 0) {
        var submitUrl = $("#divMIVehicleDetails").data("submit-url");

        var viewModel = {
            'mivd_employee_id': $("#hdnmivdEmpID").val(), 'mivd_registration_no': $("#txtVDRegistrationNo").val(), 'mivd_year_of_manufacturer': $("#ddlVDManufacturerYear").val(),
            'mivd_date_of_registration': $("#txtVDDateOfRegistration").val(), 'mivd_vehicle_category_type_id': $("#ddlVehicleCatType").val(), 'mivd_make_of_vehicle': $("#ddlVDVehicleManufacture").val(), 'mivd_registration_authority_and_location': $("#txtVDRegistering").val(),
            'mivd_type_of_model': $("#txtVDTypeOfModel").val(), 'mivd_chasis_no': $("#txtVDChasisNo").val(), 'mivd_vehicle_fuel_type': $("#ddlVDFuelType").val(),
            'mivd_engine_no': $("#txtVDEngine").val(), 'mivd_seating_capacity_including_driver': $("#txtVDSeating").val(), 'mivd_cubic_capacity': $("#txtVDCubicCapacity").val(), 'mivd_date_of_manufacture': $("#txtVDDateOfManufacture").val(),
            'mi_referenceno': $("#spnMIReferanceNo").text(), 'mivd_vehicle_weight': $("#txtVDWeight").val(), 'mivd_vehicle_category_id': $("#ddlVehCat").val(),
            'mivd_vehicle_type_id': $("#ddlVehType").val(), 'mivd_vehicle_subtype_id': $("#ddlVehSubType").val(), 'mivd_pagetype': $("#hdnPagetype").val(),
            'mivd_own_damage_value': $("#txtowndamage").val(), 'mivd_premium_liability_value': $("#txtpremiumliability").val(), 'mivd_vehicle_min_value': $("#txtVDminvalue").val(),
            'mivd_Depreciation_value': $("#txtVDDepreciation").val(), 'mivd_malus_value': $("#txtMalus").val(), 'mivd_ncb_value': $("#txtNcb").val(),
            'mivd_own_damage_id': $("#txtowndamageid").val(), 'mivd_premium_liability_id': $("#txtpremiumliabilityid").val(), 'mivd_vehicle_min_id': $("#txtVDminvalueid").val(),
            'mivd_Depreciation_id': $("#txtVDDepreciationid").val(), 'mivd_malus_id': $("#txtMalusid").val(), 'mivd_ncb_id': $("#txtNcbid").val(),
            'mivd_Additionalamt': $("#txtAdditionalamt").val(), 'mivd_govDiscount': $("#txtGovDiscount").val(),
            'mivd_PLgovDiscount': $("#txtPLGovDiscount").val(), 'mivd_PLDriverAmt': $("#txtPLDriverAmt").val(), 'mivd_PLPassengerAmt': $("#txtPLPassengerAmt").val(),
            'mivd_vehicle_rto_id': $("#ddlVDRTO").val(), 'mivd_vehicle_class_id': $("#ddlVehClassType").val(),
            'mivd_manufacturer_month': $("#ddlManufacturMonth").val(),

        };
        $.ajax({
            url: submitUrl,
            data: JSON.stringify({ "objMVDetails": viewModel }),
            type: 'POST',
            async: false,
            cache: false,
            contentType: 'application/json; charset=utf-8',
            processData: false,
            success: function (response) {
                if (response.IsSuccess) {
                    if (response.Pagetype == "Renewal" || response.Pagetype == "EditRenewal") {
                        GetRenewalVehicleDetails();
                        $('#txtowndamage').val(response.OwnDamageValue);
                        $('#txtpremiumliability').val(response.PremiumLiabilityValue);
                        $('#txtVDminvalue').val(response.VehMinValue);
                        //document.getElementById('txtODV').value = response.OwnDamageValue;
                        //document.getElementById('txtPLV').value = response.PremiumLiabilityValue;
                        //document.getElementById('txtDPV').value = response.DepreciationValue;
                        $('#txtVDDepreciation').val(response.DepreciationValue);
                        $('#txtVDZone').val(response.Zonetype);
                        $('#txtAdditionalamt').val(response.AdditionAmt);
                        $('#txtGovDiscount').val(response.govDiscnt);
                        $('#txtPLGovDiscount').val(response.PLgovDiscountValue);
                        $('#txtPLDriverAmt').val(response.PLDriverAmtValue);
                        $('#txtPLPassengerAmt').val(response.PLPassengerAmtValue);
                    }
                    else {
                        $('#txtowndamage').val(response.OwnDamageValue);
                        $('#txtpremiumliability').val(response.PremiumLiabilityValue);
                        $('#txtVDminvalue').val(response.VehMinValue);
                        //document.getElementById('txtODV').value = response.OwnDamageValue;
                        //document.getElementById('txtPLV').value = response.PremiumLiabilityValue;
                        //document.getElementById('txtDPV').value = response.DepreciationValue;
                        $('#txtVDDepreciation').val(response.DepreciationValue);
                        $('#txtVDZone').val(response.Zonetype);
                        $('#txtAdditionalamt').val(response.AdditionAmt);
                        $('#txtGovDiscount').val(response.govDiscnt);
                        $('#txtPLGovDiscount').val(response.PLgovDiscountValue);
                        $('#txtPLDriverAmt').val(response.PLDriverAmtValue);
                        $('#txtPLPassengerAmt').val(response.PLPassengerAmtValue);
                        GetVehicleDetails();
                    }

                    alertify.success("Vehicle details saved successful");
                    $("a[href*='next']").parent("li").show();
                    VDserverResponse = true;

                }
                else {
                    if (response.IsSuccess == false) {
                        // alertify.alert(response.Message);
                        VDserverResponse = true;
                    }
                    else
                        alertify.error("Could not save");
                }
            }, error: function (response) {
                alertify.error("Could not save");
            }
        });
        debugger;
        //if ($('#hdnSentBackApp').val() == "1") {
        //    DisableFeilds();
        //    $("#ddlVDFuelType").attr('readonly', true);
        //    $("#ddlVehType").attr('readonly', true);
        //    $("#txtVDChasisNo").attr('readonly', true);
        //    $("#btnFetchDetails").attr('readonly', true);
        //    $("#ddlVehSubType").attr('readonly', true);
        //    $("#ddlVehCatType").attr('readonly', true);

        //    $("#ddlVDFuelType").css("pointer-events", "none");;
        //    $("#ddlVehType").css("pointer-events", "none");
        //    $("#txtVDChasisNo").css("pointer-events", "none");
        //    $("#btnFetchDetails").css("pointer-events", "none");
        //    $("#ddlVehSubType").css("pointer-events", "none");
        //    $("#ddlVehCatType").css("pointer-events", "none");

        //    $("#ddlVDFuelType").attr("tabindex", "-1");
        //    $("#ddlVehType").attr("tabindex", "-1");
        //    $("#txtVDChasisNo").attr("tabindex", "-1");
        //    $("#btnFetchDetails").attr("tabindex", "-1");
        //    $("#ddlVehSubType").attr("tabindex", "-1");
        //    $("#ddlVehCatType").attr("tabindex", "-1");
        //}

    }

    return VDserverResponse;

}
function GetVehicleDetails() {
    $.ajax({
        url: '/MotorInsurance/VehicleDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-1").html(data);
        }
    });
}
function SaveOtherDetails() {
    var manufactureryear = $('#ddlVDManufacturerYear option:selected').text();
    var manufacturermonth = $('#ddlManufacturMonth option:selected').val();



    debugger;
    var today = new Date();
    jQuery('#txtPHDateOfPurchase').datetimepicker({
        defaultDate: new Date(manufactureryear, manufacturermonth - 1, 01),
        timepicker: false,
        format: 'd-m-Y',
        minDate: new Date(manufactureryear, manufacturermonth - 1, 01),
        maxDate: new Date(),
        yearStart: manufactureryear,
        scrollMonth: false,
        scrollInput: false,
        closeOnDateSelect: true,
        useCurrent: false,
    });

    jQuery('#txtPHfromDt').datetimepicker({
        defaultDate: new Date(manufactureryear, manufacturermonth - 1, 01),
        timepicker: false,
        format: 'd-m-Y',
        minDate: new Date(manufactureryear, manufacturermonth - 1, 01),
        maxDate: new Date(),
        yearStart: manufactureryear,
        scrollMonth: false,
        scrollInput: false,
        closeOnDateSelect: true,
        useCurrent: false,
    });

    $("#txtPHDateOfPurchase").on("keydown keypress keyup", false);
    $("#txtPHVToDate").on("keydown keypress keyup", false);
    $("#txtPHfromDt").on("keydown keypress keyup", false);


    ValidateOtherDetails();
    //var PserverResponse = false;
    var eid = $("#divApplicationMI").data("eid");
    var formData = new FormData($("#FormOtherDetails").get(0));
    var refid = $("#hdnPHReferenceNo").val();
    formData.append("miod_app_ref_id", refid);
    if ($('.err:visible').length === 0) {
        $.ajax({
            url: '/MotorInsurance/InsertOtherDetails',
            data: formData,
            async: false,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.IsSuccess) {
                    GetOtherDetails()
                    alertify.success("Other details saved successfully");
                    ODserverResponse = true;
                }
                else {
                    alertify.error("Could not save Other Details");
                    ODserverResponse = false;
                }
            }, error: function (result) {
                alertify.error("Could not save Other details");
                ODserverResponse = false;
            }
        });
    }

    return ODserverResponse;
}
function GetOtherDetails() {
    $.ajax({
        url: '/MotorInsurance/OtherDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-2").html(data);
        }
    });
}
function ValidateOtherDetails() {
    $('.err').attr('hidden', true);
    if ($('[name="miod_is_non_conventioanal_source"]:checked').val() == "True") {
        if ($("#txtIsNonConSourceDetails").val() == "") {
            $("#errIsNonConSourceDetails").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="miod_is_geographical"]:checked').val() == "True") {
        if ($("#ddlGeoExt1").val() == "") {
            $("#errddlGeoExt1Details").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="miod_is_bi_fuel_system"]:checked').val() == "True") {
        if ($("#txtIsBiFuelAmount").val() == "") {
            $("#errbifuelamount").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="miod_is_higher_deductible"]:checked').val() == "True") {
        if ($("#txtIsHigherDedctAmount").val() == "") {
            $("#errIsHigherDedctAmount").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="miod_is_automobile_association_of_india"]:checked').val() == "True") {
        if ($("#txtIsNameOfAssociation").val() == "" && $("#txtIsMembershipNo").val() == "" && $("#txtIsDateofExpiry").val() == "") {
            $("#errIsNameOfAssociation").removeAttr("hidden");
            $("#errIsMembershipNo").removeAttr("hidden");
            $("#errIsDateofExpiry").removeAttr("hidden");
            return false;
        }
        else if ($("#txtIsNameOfAssociation").val() == "" && $("#txtIsMembershipNo").val() == "") {
            $("#errIsNameOfAssociation").removeAttr("hidden");
            $("#errIsMembershipNo").removeAttr("hidden");
            return false;
        }
        else if ($("#txtIsMembershipNo").val() == "" && $("#txtIsDateofExpiry").val() == "") {
            $("#errIsMembershipNo").removeAttr("hidden");
            $("#errIsDateofExpiry").removeAttr("hidden");
            return false;
        }
        else if ($("#txtIsNameOfAssociation").val() == "" && $("#txtIsDateofExpiry").val() == "") {
            $("#errIsNameOfAssociation").removeAttr("hidden");
            $("#errIsDateofExpiry").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="miod_is_cover_legal_liability"]:checked').val() == "True") {
        if ($("#txtIsCLLNoOfDrivers").val() == "") {
            $("#errIsCLLNoOfDrivers").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="miod_is_no_claim_bonus"]:checked').val() == "True") {
        if ($("#txtIsPlaceChangeDoc").val() == "") {
            if ($("#txtnoclmbonusdoc").val() === undefined) {
                $("#errIsPlaceChangeDoc").removeAttr("hidden");
                return false;
            }
        }
    }
    if ($('[name="miod_is_higher_towing_charges"]:checked').val() == "True") {
        if ($("#txtIsHighTowingCharges").val() == "") {
            $("#errIsHighTowingCharges").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="miod_is_include_personal_accident"]:checked').val() == "True") {
        if ($("#txtIsPAcover").val() == "") {
            $("#errIsPAcover").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="miod_is_include_personal_accident_for_persons"]:checked').val() == "True") {
        if ($("#txtIsIPAname1").val() == "") {
            $("#errIsIPAname1").removeAttr("hidden");
            $("#errIsIPAnameamt1").removeAttr("hidden");
            return false;
        }
        else if ($("#txtIsIPAname1CSI").val() == "") {
            $("#errIsIPAnameamt1").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="miod_is_include_pa_cover_for_unnamed_persons"]:checked').val() == "True") {
        if ($("#txtIsunIPAname1").val() == "") {
            $("#errIsunIPAname1").removeAttr("hidden");
            $("#errIsunIPAname1CSI").removeAttr("hidden");
            return false;
        }
        else if ($("#txtIsunIPAname1CSI").val() == "") {
            $("#errIsunIPAname1CSI").removeAttr("hidden");
            return false;
        }
    }
    //if ($('[name="ph_Hire"]:checked').val() == "True" || $('[name="ph_Lease"]:checked').val() == "True" || $('[name="ph_Hypothecation"]:checked').val() == "True") {
    //    if ($("#txtPVHReason").val() == "") {
    //        $("#errorPVHireReq").removeAttr("hidden");
    //        return false;
    //    }
    //}
}
function SaveIDVDetails() {
    debugger;


    ValidateIDVDetails();
    var eid = $("#divApplicationMI").data("eid");
    var formData = new FormData($("#FormIDVDetails").get(0));
    var refid = $("#hdnMIReferanceNo").val();
    formData.append("miidv_app_ref_id", refid);
    formData.append('premium_amount', PremiumPayableAmount);
    if ($('.err:visible').length === 0) {
        $.ajax({
            url: '/MotorInsurance/InsertIDVDetails',
            data: formData,
            async: false,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.IsSuccess) {
                    GetIDVDetails()
                    alertify.success("IDV details saved successfully");
                    IDVserverResponse = true;
                    if ($('#hdnIdvPageType').val() == "Renewal" || $('#hdnIdvPageType').val() == "EditRenewal") {
                        Renewalsum();
                    }
                    else {
                        sum();
                    }

                }
                else {
                    alertify.error("Could not save IDV Details");
                    IDVserverResponse = false;
                }
            }, error: function (result) {
                alertify.error("Could not save IDV details");
                IDVserverResponse = false;
            }
        });
        //if ($('#hdnSentBackApp').val() == "1") {
        //    $('#divIDVDetails input').attr("readonly", true);
        //    $('#divIDVDetails select').attr("readonly", true);
        //    $('#divIDVDetails textarea').attr("readonly", true);

        //    $('#divIDVDetails input').css("pointer-events", "none");
        //    $('#divIDVDetails select').css("pointer-events", "none");
        //    $('#divIDVDetails textarea').css("pointer-events", "none");

        //    $('#divIDVDetails input').attr("tabindex", "-1");
        //    $('#divIDVDetails select').attr("tabindex", "-1");
        //    $('#divIDVDetails textarea').attr("tabindex", "-1");
        //}

    }
    return IDVserverResponse;
}
function GetIDVDetails() {
    $.ajax({
        url: '/MotorInsurance/IDVDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-4").html(data);
            document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(PremiumPayableAmount);
        }
    });
}
function SavePreviousHistoryDetails() {

    ValidatePreviousHistoryDetails();
    if ($('.err:visible').length === 0) {
        var formData = new FormData($("#formPreviousHistoryMI").get(0));
        //formData.append('premium_amount', PremiumPayableAmount);
        var viewModel = {
            'objMIPreviousDetails': formData, 'ph_EmployeeCode': $('#hdnmipdEmpID').val(), 'ph_reference': $('#spnMIReferanceNo').text()
        }
        $.ajax({
            url: '/MotorInsurance/InsertMIPreviousHistoryDetails',
            //data: JSON.stringify({ 'objMIPreviousDetails': formData, 'EmpID': $('#hdnmipdEmpID').val(), 'ReferenceId': $('#spnMIReferanceNo').text()}),
            data: formData,
            type: 'POST',
            async: false,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.IsSuccess) {

                    $('#txtPHmalus').val(response.mcb);
                    $('#txtPHncb').val(response.ncb);

                    if (response.PageType == "Renewal" || response.PageType == "EditRenewal") {
                        GetRenewalPreviousHistoryDetails();
                    }
                    else {
                        GetPreviousDetails();
                    }
                    sum();
                    alertify.success("Previous history details saved successful");
                    PHDserverResponse = true;
                }
                else {
                    alertify.error("Could not save Previous history details");
                    PHDserverResponse = false;
                }
            }, error: function (response) {
                alertify.error("Could not save Previous history details");
                PHDserverResponse = false;
            }
        });
    }
    return PHDserverResponse;
}
function GetPreviousDetails() {
    $.ajax({
        url: '/MotorInsurance/PreviousHistoryToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-3").html(data);
        }
    });
}

function GetRenewalPreviousHistoryDetails() {

    var ref = $("#hdnPHReferenceNo").val();
    var pt = $("#hdnIdvPageType").val();
    $.ajax({
        url: '/MotorInsurance/RenewalPreviousHistoryToView',
        data: JSON.stringify({ "PageType": pt, "refno": ref }),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $("#ApplicationTab-p-3").html(data);
        }
    });
}
function ValidatePreviousHistoryDetails() {
    $('.err').attr('hidden', true);
    if ($('[name="ph_vehicleCondition"]:checked').val() == "True") {
        if ($("#txtPHReason").val() == "") {
            $("#errorPVConditionReq").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="ph_InsuranceCancelled"]:checked').val() == "True") {
        if ($("#txtphCancelled").val() == "") {
            $("#errorPVCancelledReq").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="ph_InsuranceImposed"]:checked').val() == "True") {
        if ($("#txtphImposed").val() == "") {
            $("#errorPVImposedReq").removeAttr("hidden");
            return false;
        }
    }
    if ($('[name="ph_Hire"]:checked').val() == "True" || $('[name="ph_Lease"]:checked').val() == "True" || $('[name="ph_Hypothecation"]:checked').val() == "True") {
        if ($("#txtPVHReason").val() == "") {
            $("#errorPVHireReq").removeAttr("hidden");
            return false;
        }
    }

    if ($('#txtPHfromDt').val() != "") {

        if ($('#txtPHVToDate').val() == "") {
            $('#errorPVinsuranceToDtReq').removeAttr("hidden");
            $('#errorPVinsuranceToDtReq').text("Please enter To date.")

            return false;
        }
    }
}
function SaveMIDocumentDetails(sentbackid) {
    // alert("sentbackid" + sentbackid);

    var eid = $("#divApplicationMI").data("eid");
    var formData = new FormData($("#FormMIDocumentDetails").get(0));
    var refid = $("#hdnMIReferanceNo").val();
    formData.append("MI_App_Reference_ID", refid);
    //
    var DocType = $('input[name=IsDocType]:checked').val();
    formData.append("IsDocType", DocType);
    //
    if ($('.err:visible').length === 0) {
        $.ajax({
            url: '/MotorInsurance/InsertMIDocumentDetails',
            data: formData,
            async: false,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.IsSuccess) {
                    //alertify.confirm("Document details saved successfully");
                    MIDOCserverResponse = true;
                    alertify.confirm("Document details saved successfully", function () {
                        if ($('#hdnDocPage').val() == "Renewal" || $('#hdnDocPage').val() == "EditRenewal") {

                            if (sentbackid == 1) {
                                window.location.href = "/mi-dpt-r-rsoa";
                                alertify.alert("Forwarded to Caseworker").setHeader("Submit Changes ?");
                            }
                            else {
                                window.location.href = "/mi-dpt-r-rmp";
                            }

                        }
                        else {
                            if (sentbackid == 1) {
                                alertify.alert("Forwarded to Caseworker").setHeader("Submit Changes ?");
                                window.location.href = "/mi-e-soa";
                                alertify.alert("Forwarded to Caseworker").setHeader("Submit Changes ?");
                            }
                            else {
                                window.location.href = "/mi-e-mp";
                            }
                        }

                    }).setHeader("Submit Changes ?");
                    //setTimeout(function () {
                    //    window.location.href = "/mi-e-mp";
                    //}, 1000);
                    //window.location.href = "/MotorInsurance/MotorInsuranceListOfApplications";
                }
                else {
                    alertify.error("Could not save Document Details");
                    MIDOCserverResponse = false;
                }
            }, error: function (result) {
                alertify.error("Could not save Document details");
            }
        });
    }
    return MIDOCserverResponse;
}
function ValidateDocumentDetails(id) {
    if (id == 1) {
        //if ($("#txtProposalDocNewPurchase").val().length == 0) {
        //    alertify.alert("Please upload Proposal Form").setHeader("Warning!!!");
        //    return false;
        //}
        if ($("#txtGovtSanctionDoc").val().length == 0) {
            alertify.alert("Please upload Sanction Letter from Government").setHeader("Warning!!!");
            return false;
        }
        if ($("#txtProformaInvoiceDoc").val().length == 0) {
            alertify.alert("Please upload Tax Invoice").setHeader("Warning!!!");
            return false;
        }
        if ($("#txtPSaleCertificateDoc").val().length == 0) {
            alertify.alert("Please upload Sale Certificate").setHeader("Warning!!!");
            return false;
        }
        return true;
    }
    else if (id == 2) {
        if ($("#txtDonationDoc").val().length == 0) {
            alertify.alert("Please upload  Letter of Donation").setHeader("Warning!!!");
            return false;
        }
        //if ($("#txtProposalDocDonatedVehicle").val().length == 0) {
        //    alertify.alert("Please upload Proposal Form").setHeader("Warning!!!");
        //    return false;
        //}

        if ($("#txtSaleCertificateDoc").val().length == 0) {
            alertify.alert("Please upload Sale Certificate").setHeader("Warning!!!");
            return false;
        }
        if ($("#txtTaxInvoiceDoc").val().length == 0) {
            alertify.alert("Please upload Tax Invoice").setHeader("Warning!!!");
            return false;
        }

        if (document.getElementById('rbtnVPurchaseTypYes').checked) {
            if ($("#txtDonatedEmissionCertificateDoc").val().length == 0) {
                alertify.alert("Please upload Emission Certificate").setHeader("Warning!!!");
                return false;
            }
        }

        return true;
    }
    else if (id == 3) {
        if ($("#txtcCertificateDoc").val().length == 0) {
            alertify.alert("Please upload “B” Extract from RTO- Certificate").setHeader("Warning!!!");
            return false;
        }
        //if ($("#txtProposalDocSeizedVehicle").val().length == 0) {
        //    alertify.alert("Please upload Proposal Form").setHeader("Warning!!!");
        //    return false;
        //}

        if ($("#txtRTOcertificateDoc").val().length == 0) {
            alertify.alert("Please upload Tax Invoice from Showroom / Vehicle present Value from RTO").setHeader("Warning!!!");
            return false;
        }
        if ($("#txtfitnesscertificateDoc").val().length == 0) {
            alertify.alert("Please upload Fitness Certificate from RTO").setHeader("Warning!!!");
            return false;
        }
        if ($("#txtSeizedEmissionCertificateDoc").val().length == 0) {
            alertify.alert("Please upload Emission Certificate").setHeader("Warning!!!");
            return false;
        }
        return true;
    }
}
function GetRTODetailsData(id) {
    var cid = '#' + id;
    var chassisno = $(cid).val();
    if (chassisno === null || chassisno === "" || chassisno == 'undefined') {

        $("#errVDChassisNoReq").removeAttr("hidden");
        return false;
    }
    else {
        var isNumber = /^\d+$/.test(chassisno);
        var isString = (!/[^a-zA-Z]/.test(chassisno))

        if (chassisno.length < 8 || chassisno.length > 17) {

            $("#errVDChassisNoReq").removeAttr("hidden");
            return false;
        }
        else {


            if (isString == true || isNumber == true) {

                $("#errVDChassisNoReq").removeAttr("hidden");
                return false;
            }

            else {
                $('.err').attr('hidden', true);
                if ($('.err:visible').length === 0) {
                    $.ajax({
                        url: "/MotorInsurance/GetRTODetails",
                        data: JSON.stringify({ "ChasisNo": chassisno }),
                        type: 'POST',
                        async: false,
                        cache: false,
                        contentType: 'application/json; charset=utf-8',
                        processData: false,
                        success: function (response) {
                            if (response != null) {
                                if (response.mivd_date_of_registration == null) {
                                    $('#txtVDRegistrationNo').val(null);
                                }
                                else {
                                    var newFormattedDate = moment(response.mivd_date_of_registration).utcOffset(330).format('DD-MM-YYYY');

                                    $('#txtVDRegistrationNo').val(newFormattedDate);
                                }


                                $('#txtVDRegistrationNo').val(response.mivd_registration_no);
                                $('#txtVDDateOfRegistration').val(newFormattedDate);
                                $('#txtVDRegistering').val(response.mivd_registration_authority_and_location);
                                $('#txtVDSeating').val(response.mivd_seating_capacity_including_driver);
                                $('#txtVDCubicCapacity').val(response.mivd_cubic_capacity);
                            }
                            else {
                                alertify.error("Please retry again.");
                            }
                        }, error: function (response) {
                            alertify.error("Please retry again.");
                        }
                    });
                }

            }
        }
    }

}

//PVV Calculation and Premium Amount Start
var PremiumPayableAmount;
//alert(PremiumPayableAmount)
function sum() {
    debugger
    var PVVAmount;
    //
    //var MFdate = document.getElementById('txtDOM').value || 0;
    //var ODValue = document.getElementById('txtODV').value || 0;
    //var PLValue = document.getElementById('txtPLV').value || 0;
    //var DepreciationValue = document.getElementById('txtDPV').value || 0;
    //var MFdate = $("#txtVDDateOfManufacture").val();
    //
    var ddlTypeofCover = $('#ddlTypeofCover').val();
    var ddlTypeofCoverName = $('select#ddlTypeofCover option:selected').text();
    var ddlCategory = $('#ddlVehCat').val();
    //
    var ODValue = $("#txtowndamage").val();
    var PLValue = $("#txtpremiumliability").val();
    var DepreciationValue = $("#txtVDDepreciation").val();
    //PrevoiusHistoryData
    var PHNCBValue = 0;
    var PHMalusValue = 0;
    if (document.getElementById('rbtnVPurchaseTypYes').checked) {
        PHNCBValue = document.getElementById('txtPHncb').value || 0;
        PHMalusValue = document.getElementById('txtPHmalus').value || 0;
        // alertity.alert(PHNCBValue + '-' + PHMalusValue);
        //if ($("#txtDonatedEmissionCertificateDoc").val().length == 0) {
        //    alertify.alert("Please upload Emission Certificate").setHeader("Warning!!!");
        //    return false;
        //}
    }


    //
    var txtAdditionAmtValue = document.getElementById('txtAdditionalamt').value || 0;
    var txtODgovDiscntValue = document.getElementById('txtGovDiscount').value || 0;
    var txtPLgovDiscountValue = document.getElementById('txtPLGovDiscount').value || 0;
    var txtPLDriverAmtValue = document.getElementById('txtPLDriverAmt').value || 0;
    var txtPLPassengerAmtValue = document.getElementById('txtPLPassengerAmt').value || 0;
    //
    //var MDate = new Date(MFdate.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"))

    //var today = new Date();
    // var formattedtoday = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
    // var TDate = new Date(formattedtoday.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"))

    //var getmonthDiff = monthDiff(MDate, TDate)
    //console.log(getmonthDiff)

    //
    var txtidvovamntValue = document.getElementById('txtidvovamnt').value || 0;
    var txtnoneleaccamntValue = document.getElementById('txtnoneleaccamnt').value || 0;
    var txteleaccamntValue = document.getElementById('txteleaccamnt').value || 0;
    var txtsidecaramntValue = document.getElementById('txtsidecaramnt').value || 0;
    var txtcngamntrValue = document.getElementById('txtcngamnt').value || 0;
    var resultA = parseFloat(txtidvovamntValue) + parseFloat(txtnoneleaccamntValue) + parseFloat(txteleaccamntValue) + parseFloat(txtsidecaramntValue) + parseFloat(txtcngamntrValue);
    //var resultround = Math.round(result).toFixed(2);
    //alert(result)
    //var DepreB = Math.round(DepreciationValue).toFixed(2);
    var DepreB = ((parseFloat(resultA)) / 100) * parseFloat(DepreciationValue);
    var ValueC = parseFloat(resultA) - parseFloat(DepreB);
    var TotalPVV = Math.round(ValueC).toFixed(2);
    document.getElementById('txttotamnt').value = ReplaceNumberWithCommas(TotalPVV);
    PVVAmount = TotalPVV;
    //if (getmonthDiff <= 6) {
    //    var IDVTot = ((parseFloat(result)) / 100) * 5;
    //    //alert(IDVTot)
    //    if (!isNaN(IDVTot)) {
    //        var res1 = Math.round(IDVTot).toFixed(2)
    //        var res2 = parseFloat(result) - parseFloat(res1)
    //        var totres = Math.round(res2).toFixed(2)
    //        document.getElementById('txttotamnt').value = ReplaceNumberWithCommas(totres);
    //        PVVAmount = totres;
    //        //alert(getmonthDiff)
    //    }
    //}

    // Premium Calculation Start
    if (ddlTypeofCover == "2") {
        //alertify.success("P")
        var txttotamntValue = PVVAmount;
        //txtbpidv
        var VCategory = $('#ddlVehCatType').val();
        console.log(VCategory)
        if (VCategory == '11' || VCategory == '12') {
            var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue) + parseFloat(txtAdditionAmtValue));
            console.log(txtbpidvValue)
        }
        else {
            var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));
            console.log(txtbpidvValue)
        }
        //var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));

        if (!isNaN(txtbpidvValue)) {
            var res = Math.round(txtbpidvValue).toFixed(2)
            //alert(res)
            //document.getElementById('txtbpidv').value = ReplaceNumberWithCommas(res);
            //document.getElementById('txtidvsubtot').value = ReplaceNumberWithCommas(res);
            //document.getElementById('txtodp').value = ReplaceNumberWithCommas(res);
        }
        //txtlgrod
        var txtlgrodValue = ((parseFloat(res)) / 100) * (parseFloat(txtODgovDiscntValue));
        if (!isNaN(txtlgrodValue)) {
            var res1 = Math.round(txtlgrodValue).toFixed(2)
            //alert(res1)
            //document.getElementById('txtlgrod').value = ReplaceNumberWithCommas(res1);
            //document.getElementById('txtrebatetotod').value = res1;
        }
        //txtrebatetotod
        var txtrebatetotodvalue = parseFloat(res) - parseFloat(res1)
        var res2 = Math.round(txtrebatetotodvalue).toFixed(2)
        //document.getElementById('txtrebatetotod').value = ReplaceNumberWithCommas(res2);

        //txtsubtotlpgod
        var txtsubtotlpgodValue = Math.round(txtrebatetotodvalue).toFixed(2);
        //document.getElementById('txtsubtotlpgod').value = ReplaceNumberWithCommas(txtsubtotlpgodValue);

        //txth
        var txthValue = ((parseFloat(txtsubtotlpgodValue)) / 100) * 1;
        //document.getElementById('txth').value = ReplaceNumberWithCommas(txthValue);

        //txtsubtotextra
        var txtsubtotextraVlaue = Math.round(txtsubtotlpgodValue).toFixed(2);
        //document.getElementById('txtsubtotextra').value = ReplaceNumberWithCommas(txtsubtotextraVlaue);

        //txtaddmalus
        var txtaddmalusValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * PHMalusValue;
        if (!isNaN(txtaddmalusValue)) {
            var resmalus = Math.round(txtaddmalusValue).toFixed(2)
            //alert(res1)
            //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
            //document.getElementById('txtrebatetotod').value = res1;
        }
        //var txttotaftetmalus = parseFloat(txtsubtotextraVlaue) + parseFloat(resmalus);
        //txtlessncb
        var txtlessncbValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * PHNCBValue;
        if (!isNaN(txtlessncbValue)) {
            var res3 = Math.round(txtlessncbValue).toFixed(2)
            //alert(res1)
            //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
            //document.getElementById('txtrebatetotod').value = res1;
        }

        //txtodtot
        var txtodtotValue = parseFloat(txtsubtotextraVlaue) + parseFloat(parseFloat(resmalus)) - parseFloat(res3);
        var res4 = Math.round(txtodtotValue).toFixed(2);
        //document.getElementById('txtodtot').value = ReplaceNumberWithCommas(res4);
        //document.getElementById('txtodtotA').value = ReplaceNumberWithCommas(res4);

        //B. LIABILITY TO PUBLIC RISK
        //txtlgrlpr
        //var txtlprValue = document.getElementById('txtlpr').value || 0;
        var txtlprValue = parseFloat(PLValue);
        var res5 = Math.round(txtlprValue).toFixed(2)
        //txtbpidv
        var txtlgrlprValue = ((parseFloat(res5)) / 100) * (parseFloat(txtPLgovDiscountValue));
        var res6 = Math.round(txtlgrlprValue).toFixed(2)
        //document.getElementById('txtlgrlpr').value = ReplaceNumberWithCommas(res6);
        //txtsubtotlpr
        var txtsubtotlprValue = parseFloat(res5) - parseFloat(res6)
        var res7 = Math.round(txtsubtotlprValue).toFixed(2)
        //document.getElementById('txtsubtotlpr').value = ReplaceNumberWithCommas(res7);

        //txtlpgkitlpr
        var txtcngamntrValue = 0;
        var txtlpgkitlprValue = ((parseFloat(txtcngamntrValue)) / 100) * 60;
        var res8 = Math.round(txtlpgkitlprValue).toFixed(2)
        //document.getElementById('txtlpgkitlpr').value = ReplaceNumberWithCommas(res8);

        //txtsubtotlpglpr
        var txtsubtotlpglprValue = parseFloat(res7) + parseFloat(res8)
        var res9 = Math.round(txtsubtotlpglprValue).toFixed(2)
        //document.getElementById('txtsubtotlpglpr').value = ReplaceNumberWithCommas(res9);

        //txtdrlpr
        var res10 = (parseFloat(txtPLDriverAmtValue));
        //document.getElementById('txtdrlpr').value = ReplaceNumberWithCommas(res10);
        //txtprlpr
        var totpassengeramt = 0;
        if (ddlCategory == 1 || ddlCategory == 2 || ddlCategory == 3 || ddlCategory == 4) {
            var totpassengeramt = parseFloat(txtPLPassengerAmtValue);
        }
        else {
            var noofseats = $('#txtVDSeating').val();
            var totpassengeramt = (parseFloat(noofseats) - 1) * parseFloat(txtPLPassengerAmtValue);
        }
        var res11 = parseFloat(totpassengeramt);
        //document.getElementById('txtprlpr').value = ReplaceNumberWithCommas(res11);

        //txtlprtot
        var txtlprtotValue = (parseFloat(res9) + parseFloat(res10) + parseFloat(res11))
        var res12 = Math.round(txtlprtotValue).toFixed(2)
        //document.getElementById('txtlprtot').value = ReplaceNumberWithCommas(res12);
        //document.getElementById('txtlprtotB').value = ReplaceNumberWithCommas(res12);

        //txttotAB
        var txttotABValue = parseFloat(res4) + parseFloat(res12)
        var res13 = Math.round(txttotABValue).toFixed(2)
        //document.getElementById('txttotAB').value = ReplaceNumberWithCommas(res13);
        //document.getElementById('txtpremium').value = ReplaceNumberWithCommas(res13);

        //txtgstamt
        var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
        var res14 = Math.round(txtgstamtValue).toFixed(2)
        //document.getElementById('txtgstamt').value = ReplaceNumberWithCommas(res14);

        //txttotalcrpremium
        var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14)
        var res15 = Math.round(txttotalcrpremiumValue).toFixed(2)
        //document.getElementById('txttotalcrpremium').value = ReplaceNumberWithCommas(res15);
        //txtTotalPremium
        document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
        PremiumPayableAmount = res15;

        // Premium Calculation End
        //if (!isNaN(result)) {
        //    var res = parseFloat(result).toFixed(2)
        //    document.getElementById('txttotamnt').value = res;
        //    alert(getmonthDiff)
        //}
        //Addition Popup For View Premium Details
        $('#idvpercent').text(ODValue);
        $('#txtbpidv').val(res);
        $('#txtidvsubtot').val(res);
        $('#txtodp').val(res);
        $('#txtlgrod').val(res1);
        $('#txtrebatetotod').val(txtsubtotextraVlaue);
        $('#txtsubtotlpgod').val(txtsubtotextraVlaue);
        $('#txtsubtotextra').val(txtsubtotextraVlaue);
        $('#txtamod').val(resmalus);
        $('#idvmaluspercent').text(PHMalusValue);
        $('#txtlessncb').val(res3);
        $('#idvncbpercent').text(PHNCBValue);
        $('#txtodtot').val(res4);


        $('#txtlpr').val(PLValue);
        $('#txtlgrlpr').val(res6);
        $('#txtsubtotlpr').val(res7);
        $('#txtsubtotlpglpr').val(res9);
        $('#txtdrlpr').val(txtPLDriverAmtValue);
        if (ddlCategory == 1 || ddlCategory == 2 || ddlCategory == 3 || ddlCategory == 4) {
            $('#txtprrlpr').val(res11);
            $('#txtprlpr').val('');
        }
        else {
            $('#txtprlpr').val(res11);
            $('#txtprrlpr').val('');
        }
        //$('#txtprlpr').val(txtPLPassengerAmtValue);
        $('#txtlprtot').val(res12);
        $('#txtodtotA').val(res4);
        $('#txtlprtotB').val(res12);
        $('#txttotAB').val(res13);

        $('#txtgstamt').val(res14);
        $('#txtTotalPremiumAmt').val(res15);
    }
    else if (ddlTypeofCover == "1") {
        //alertify.success("L")
        var txttotamntValue = PVVAmount;

        //txtodtot
        var res4 = 0;
        //document.getElementById('txtodtot').value = ReplaceNumberWithCommas(res4);
        //document.getElementById('txtodtotA').value = ReplaceNumberWithCommas(res4);

        //B. LIABILITY TO PUBLIC RISK
        //txtlgrlpr
        //var txtlprValue = document.getElementById('txtlpr').value || 0;
        var txtlprValue = parseFloat(PLValue);
        var res5 = Math.round(txtlprValue).toFixed(2)
        //txtbpidv
        var txtlgrlprValue = ((parseFloat(res5)) / 100) * (parseFloat(txtPLgovDiscountValue));
        var res6 = Math.round(txtlgrlprValue).toFixed(2)
        //document.getElementById('txtlgrlpr').value = ReplaceNumberWithCommas(res6);
        //txtsubtotlpr
        var txtsubtotlprValue = parseFloat(res5) - parseFloat(res6)
        var res7 = Math.round(txtsubtotlprValue).toFixed(2)
        //document.getElementById('txtsubtotlpr').value = ReplaceNumberWithCommas(res7);

        //txtlpgkitlpr
        var txtcngamntrValue = 0;
        var txtlpgkitlprValue = ((parseFloat(txtcngamntrValue)) / 100) * 60;
        var res8 = Math.round(txtlpgkitlprValue).toFixed(2)
        //document.getElementById('txtlpgkitlpr').value = ReplaceNumberWithCommas(res8);

        //txtsubtotlpglpr
        var txtsubtotlpglprValue = parseFloat(res7) + parseFloat(res8)
        var res9 = Math.round(txtsubtotlpglprValue).toFixed(2)
        //document.getElementById('txtsubtotlpglpr').value = ReplaceNumberWithCommas(res9);

        //txtdrlpr
        var res10 = (parseFloat(txtPLDriverAmtValue));
        //document.getElementById('txtdrlpr').value = ReplaceNumberWithCommas(res10);
        //txtprlpr
        var totpassengeramt = 0;
        if (ddlCategory == 1 || ddlCategory == 2 || ddlCategory == 3 || ddlCategory == 4) {
            var totpassengeramt = parseFloat(txtPLPassengerAmtValue);
        }
        else {
            var noofseats = $('#txtVDSeating').val();
            var totpassengeramt = (parseFloat(noofseats) - 1) * parseFloat(txtPLPassengerAmtValue);
        }
        var res11 = parseFloat(totpassengeramt);
        //document.getElementById('txtprlpr').value = ReplaceNumberWithCommas(res11);

        //txtlprtot
        var txtlprtotValue = (parseFloat(res9) + parseFloat(res10) + parseFloat(res11))
        var res12 = Math.round(txtlprtotValue).toFixed(2)
        //document.getElementById('txtlprtot').value = ReplaceNumberWithCommas(res12);
        //document.getElementById('txtlprtotB').value = ReplaceNumberWithCommas(res12);

        //txttotAB
        var txttotABValue = parseFloat(res4) + parseFloat(res12)
        var res13 = Math.round(txttotABValue).toFixed(2)
        //document.getElementById('txttotAB').value = ReplaceNumberWithCommas(res13);
        //document.getElementById('txtpremium').value = ReplaceNumberWithCommas(res13);

        //txtgstamt
        var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
        var res14 = Math.round(txtgstamtValue).toFixed(2)
        //document.getElementById('txtgstamt').value = ReplaceNumberWithCommas(res14);

        //txttotalcrpremium
        var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14)
        var res15 = Math.round(txttotalcrpremiumValue).toFixed(2)
        //document.getElementById('txttotalcrpremium').value = ReplaceNumberWithCommas(res15);
        //txtTotalPremium
        document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
        PremiumPayableAmount = res15;

        // Premium Calculation End
        //if (!isNaN(result)) {
        //    var res = parseFloat(result).toFixed(2)
        //    document.getElementById('txttotamnt').value = res;
        //    alert(getmonthDiff)
        //}
        //Addition Popup For View Premium Details
        $('#idvpercent').text("");
        $('#txtbpidv').val("");
        $('#txtidvsubtot').val("");
        $('#txtlgrod').val("");
        $('#txtrebatetotod').val("");
        $('#txtsubtotlpgod').val("");
        $('#txtsubtotextra').val("");
        $('#txtodtot').val("");

        $('#txtlpr').val(PLValue);
        $('#txtlgrlpr').val(res6);
        $('#txtsubtotlpr').val(res7);
        $('#txtsubtotlpglpr').val(res9);
        $('#txtdrlpr').val(txtPLDriverAmtValue);
        if (ddlCategory == 1 || ddlCategory == 2 || ddlCategory == 3 || ddlCategory == 4) {
            $('#txtprrlpr').val(res11);
            $('#txtprlpr').val('');
        }
        else {
            $('#txtprlpr').val(res11);
            $('#txtprrlpr').val('');
        }
        //$('#txtprlpr').val(txtPLPassengerAmtValue);
        $('#txtlprtot').val(res12);
        $('#txtodtotA').val(res4);
        $('#txtlprtotB').val(res12);
        $('#txttotAB').val(res13);

        $('#txtgstamt').val(res14);
        $('#txtTotalPremiumAmt').val(res15);
    }
    else if (ddlTypeofCover == "3") {
        //alertify.success("O")
        var txttotamntValue = PVVAmount;
        //txtbpidv
        var VCategory = $('#ddlVehCatType').val();
        console.log(VCategory)
        if (VCategory == '11' || VCategory == '12') {
            var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue) + parseFloat(txtAdditionAmtValue));
            console.log(txtbpidvValue)
        }
        else {
            var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));
            console.log(txtbpidvValue)
        }
        //var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));

        if (!isNaN(txtbpidvValue)) {
            var res = Math.round(txtbpidvValue).toFixed(2)
            //alert(res)
            //document.getElementById('txtbpidv').value = ReplaceNumberWithCommas(res);
            //document.getElementById('txtidvsubtot').value = ReplaceNumberWithCommas(res);
            //document.getElementById('txtodp').value = ReplaceNumberWithCommas(res);
        }
        //txtlgrod
        var txtlgrodValue = ((parseFloat(res)) / 100) * (parseFloat(txtODgovDiscntValue));
        if (!isNaN(txtlgrodValue)) {
            var res1 = Math.round(txtlgrodValue).toFixed(2)
            //alert(res1)
            //document.getElementById('txtlgrod').value = ReplaceNumberWithCommas(res1);
            //document.getElementById('txtrebatetotod').value = res1;
        }
        //txtrebatetotod
        var txtrebatetotodvalue = parseFloat(res) - parseFloat(res1)
        var res2 = Math.round(txtrebatetotodvalue).toFixed(2)
        //document.getElementById('txtrebatetotod').value = ReplaceNumberWithCommas(res2);

        //txtsubtotlpgod
        var txtsubtotlpgodValue = Math.round(txtrebatetotodvalue).toFixed(2);
        //document.getElementById('txtsubtotlpgod').value = ReplaceNumberWithCommas(txtsubtotlpgodValue);

        //txth
        var txthValue = ((parseFloat(txtsubtotlpgodValue)) / 100) * 1;
        //document.getElementById('txth').value = ReplaceNumberWithCommas(txthValue);

        //txtsubtotextra
        var txtsubtotextraVlaue = Math.round(txtsubtotlpgodValue).toFixed(2);
        //document.getElementById('txtsubtotextra').value = ReplaceNumberWithCommas(txtsubtotextraVlaue);

        //txtlessncb
        var txtlessncbValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * 0;
        if (!isNaN(txtlessncbValue)) {
            var res3 = Math.round(txtlessncbValue).toFixed(2)
            //alert(res1)
            //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
            //document.getElementById('txtrebatetotod').value = res1;
        }

        //txtodtot
        var txtodtotValue = parseFloat(txtsubtotextraVlaue) - parseFloat(res3)
        var res4 = Math.round(txtodtotValue).toFixed(2)
        //document.getElementById('txtodtot').value = ReplaceNumberWithCommas(res4);
        //document.getElementById('txtodtotA').value = ReplaceNumberWithCommas(res4);

        //B. LIABILITY TO PUBLIC RISK

        //txtlprtot
        var res12 = 0;
        //document.getElementById('txtlprtot').value = ReplaceNumberWithCommas(res12);
        //document.getElementById('txtlprtotB').value = ReplaceNumberWithCommas(res12);

        //txttotAB
        var txttotABValue = parseFloat(res4) + parseFloat(res12)
        var res13 = Math.round(txttotABValue).toFixed(2)
        //document.getElementById('txttotAB').value = ReplaceNumberWithCommas(res13);
        //document.getElementById('txtpremium').value = ReplaceNumberWithCommas(res13);

        //txtgstamt
        var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
        var res14 = Math.round(txtgstamtValue).toFixed(2)
        //document.getElementById('txtgstamt').value = ReplaceNumberWithCommas(res14);

        //txttotalcrpremium
        var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14)
        var res15 = Math.round(txttotalcrpremiumValue).toFixed(2)
        //document.getElementById('txttotalcrpremium').value = ReplaceNumberWithCommas(res15);
        //txtTotalPremium
        document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
        PremiumPayableAmount = res15;

        // Premium Calculation End
        //if (!isNaN(result)) {
        //    var res = parseFloat(result).toFixed(2)
        //    document.getElementById('txttotamnt').value = res;
        //    alert(getmonthDiff)
        //}
        //Addition Popup For View Premium Details
        $('#idvpercent').text(ODValue);
        $('#txtbpidv').val(res);
        $('#txtidvsubtot').val(res);
        $('#txtodp').val(res);
        $('#txtlgrod').val(res1);
        $('#txtrebatetotod').val(res4);
        $('#txtsubtotlpgod').val(res4);
        $('#txtsubtotextra').val(res4);
        $('#txtodtot').val(res4);

        $('#txtlpr').val("");
        $('#txtlgrlpr').val("");
        $('#txtsubtotlpr').val("");
        $('#txtsubtotlpglpr').val("");
        $('#txtdrlpr').val("");
        $('#txtprlpr').val("");
        $('#txtlprtot').val("");
        $('#txtodtotA').val(res4);
        $('#txtlprtotB').val(res12);
        $('#txttotAB').val(res13);

        $('#txtgstamt').val(res14);
        $('#txtTotalPremiumAmt').val(res15);
    }

}
function monthDiff(d1, d2) {
    var months;
    months = (d2.getFullYear() - d1.getFullYear()) * 12;
    months -= d1.getMonth();
    months += d2.getMonth();
    return months <= 0 ? 0 : months;
}
function ReplaceNumberWithCommas(yourNumber) {
    //Seperates the components of the number
    var components = yourNumber.toString().split(".");
    //Comma-fies the first part
    components[0] = components[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //Combines the two sections
    return components.join(".");
}
//PVV Calculation and Premium Amount End



function ToggleVehicletype() {
    $("#txtPHInsurerDetails").val('');
    $("#txtPHVInsurerNo").val('');
    $("#txtPHfromDt").val('');
    $("#txtPHVToDate").val('');
    $("#txtPHDateOfPurchase").val('');
    debugger
    if ($('[name="ph_PurchaseType"]:checked').val() == "False") {
        $("#divSecondhand").hide();

    }
    else {
        $("#divSecondhand").show();
    }
}

function ToggleVehicleCondition() {
    if ($('[name="ph_vehicleCondition"]:checked').val() == "False") {
        $("#divVehicleCondition").hide();
    }
    else {
        $("#divVehicleCondition").show();

        $("#txtPHReason").val('');
    }
}

function ToggleCancelledInsured() {

    if ($('[name="ph_InsuranceCancelled"]:checked').val() == "True") {
        $("#divCancellation").show();
        $("#txtphCancelled").addClass("txtar-personal-req");

    }
    else {
        $("#divCancellation").hide();
        $("#txtphCancelled").removeClass("txtar-personal-req");
        $("#txtphCancelled").val('');
    }
}

function ToggleImposedInsured() {

    if ($('[name="ph_InsuranceImposed"]:checked').val() == "True") {
        $("#divImposed").show();
        $("#txtphImposed").addClass("txtar-personal-req");

    }
    else {
        $("#divImposed").hide();
        $("#txtphImposed").removeClass("txtar-personal-req");
        $("#txtphImposed").val('');

    }
}

function ToggleHireorLease() {

    if ($('[name="ph_Hire"]:checked').val() == "True" || $('[name="ph_Lease"]:checked').val() == "True" || $('[name="ph_Hypothecation"]:checked').val() == "True") {
        $(".divHire").show();
        $("#txtPHReason").addClass("txtar-personal-req");
    }
    else {
        $(".divHire").hide();
        $("#txtPHReason").removeClass("txtar-personal-req");
        $("#txtPHReason").val('');

    }
}

function SelectModelBasedonManufacture(id) {

    var mid = '#' + id;
    var makeid = $(mid).val();
    if (makeid === null || makeid === "" || makeid == 'undefined') {

        $("#errVDMakeReq").removeAttr("hidden");
        return false;
    }
    else {

        $('.err').attr('hidden', true);
        if ($('.err:visible').length === 0) {
            $.ajax({
                url: "/MotorInsurance/GetModelListBasedonMake",
                data: JSON.stringify({ "makeid": makeid }),
                type: 'POST',
                async: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                processData: false,
                success: function (response) {
                    if (response != null) {
                        var s = '<option value="-1">-- Select Model --</option>';
                        for (var i = 0; i < response.VehicleTypeOfModelList.length; i++) {
                            s += '<option value="' + response.VehicleTypeOfModelList[i].Value + '">' + response.VehicleTypeOfModelList[i].Text + '</option>';
                        }
                        $("#ddlVDTypeOfModel").html(s);

                    }
                    else {
                        alertify.error("Please retry again.");
                    }
                }, error: function (response) {
                    alertify.error("Please retry again.");
                }
            });
        }

    }

}

function SelectManufactureBasedonMake(id) {

    var mid = '#' + id;
    var makeid = $(mid).val();
    if (makeid === null || makeid === "" || makeid == 'undefined') {

        $("#errVDMakeReq").removeAttr("hidden");
        return false;
    }
    else {

        $('.err').attr('hidden', true);
        if ($('.err:visible').length === 0) {
            $.ajax({
                url: "/MotorInsurance/GetManufactureListBasedonMake",
                data: JSON.stringify({ "makeid": makeid }),
                type: 'POST',
                async: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                processData: false,
                success: function (response) {


                    var d = '<option value="-1">-- Select Vehicle Subtype --</option>';
                    if (response.VehicleSubTypeList.length > 0) {
                        for (var i = 0; i < response.VehicleSubTypeList.length; i++) {
                            d += '<option value="' + response.VehicleSubTypeList[i].Value + '">' + response.VehicleSubTypeList[i].Text + '</option>';
                        }
                    }

                    $("#ddlVehSubType").html(d);

                }, error: function (response) {
                    alertify.error("Please retry again.");
                }
            });
        }

    }

}

function DDLGetCategoryList() {
    var vehTypeid = $('#ddlVehType').val();
    var vehSubTypeid = $('#ddlVehSubType').val();

    if (vehTypeid == null || vehTypeid == 0) {
        $('#ddlVehSubType').prop("disabled", true);
        //  $('#errVDVTypeReq').removeAttr("hidden");
        return false;
    }
    else {
        $('#ddlVehSubType').prop("disabled", false);
        //  $('#errVDVTypeReq').attr('hidden', true);
        if (vehSubTypeid == null || vehSubTypeid == 0 || vehSubTypeid == -1) {
            $('#ddlVehCatType').prop("disabled", true);
            // $('#errVDVSTypeReq').removeAttr("hidden");
            return false;
        }
        else {
            $.ajax({
                url: "/MotorInsurance/GetVehCatergoryList",
                data: JSON.stringify({ "VehTypeid": vehTypeid, "VehSubTypeid": vehSubTypeid }),
                type: 'POST',
                async: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                processData: false,
                success: function (response) {
                    if (response.length > 0) {

                        $('#ddlVehCatType').prop("disabled", false);
                        $('#errVDVCTypeReq').attr('hidden', true);

                        var s = '<option value="-1">-- Select Vehicle Category --</option>';
                        for (var i = 0; i < response.length; i++) {
                            s += '<option value="' + response[i].vc_vehicle_category_id + '">' + response[i].vc_vehicle_category_desc + '</option>';
                        }
                        $("#ddlVehCat").html(s);
                        $('#errVDVSTypeReq').attr('hidden', true);

                    }
                    else {
                        if (vehSubTypeid != -1) {
                            var s = '<option value="-1">-- Select Vehicle Category --</option>';
                            $('#ddlVehCatType').val('')
                            $("#ddlVehCatType").html(s);
                            $('#ddlVehCatType').prop("disabled", true);
                            $('#errVDVSTypeReq').removeAttr('hidden', true);
                            $('#errVDVSTypeReq').text("No category exits, Please select other subtype");
                            alertify.error("No category exits, Please select other subtype");
                        }

                    }
                }, error: function (response) {
                    alertify.error("Could not save");
                }
            });
        }
    }
}

function GetRenewalVehicleDetails() {
    $.ajax({
        url: '/MotorInsurance/RenewalVehicleDetailsToView',
        data: JSON.stringify({
            "PageType": "Renewal", "refNo": $('#txtPrevRefNo').val(), "RenewalRefNo": $('#txtPrevRefNo').val()
        }),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#divMIVehicleDetails").html(data);
        }
    });

}

function Renewalsum() {
    var PVVAmount;
    var NCBClaim;
    //
    //var MFdate = document.getElementById('txtDOM').value || 0;
    //var ODValue = document.getElementById('txtODV').value || 0;
    //var PLValue = document.getElementById('txtPLV').value || 0;
    //var DepreciationValue = document.getElementById('txtDPV').value || 0;
    //var NCBValue = document.getElementById('txtNCBV').value || 0;
    //var MalusValue = document.getElementById('txtMALUSV').value || 0;

    // var MFdate = $("#txtVDDateOfManufacture").val();
    //
    var ddlTypeofCover = $('#ddlTypeofCover').val();
    var ddlTypeofCoverName = $('select#ddlTypeofCover option:selected').text();
    //
    var ODValue = $("#txtowndamage").val();
    var PLValue = $("#txtpremiumliability").val();
    var DepreciationValue = $("#txtVDDepreciation").val();
    var NCBValue = $("#txtNcb").val();
    var MalusValue = $("#txtMalus").val();
    var PolicyMonths = $("#txtPolicyMonths").val();
    //
    var txtAdditionAmtValue = document.getElementById('txtAdditionalamt').value || 0;
    var txtODgovDiscntValue = document.getElementById('txtGovDiscount').value || 0;
    var txtPLgovDiscountValue = document.getElementById('txtPLGovDiscount').value || 0;
    var txtPLDriverAmtValue = document.getElementById('txtPLDriverAmt').value || 0;
    var txtPLPassengerAmtValue = document.getElementById('txtPLPassengerAmt').value || 0;
    //
    // var MDate = new Date(MFdate.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"))

    //var today = new Date();
    //var formattedtoday = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
    //var TDate = new Date(formattedtoday.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"))

    //var getmonthDiff = monthDiff(MDate, TDate)
    //console.log(getmonthDiff)

    //
    var txtidvovamntValue = document.getElementById('txtidvovamnt').value || 0;
    var txtnoneleaccamntValue = document.getElementById('txtnoneleaccamnt').value || 0;
    var txteleaccamntValue = document.getElementById('txteleaccamnt').value || 0;
    var txtsidecaramntValue = document.getElementById('txtsidecaramnt').value || 0;
    var txtcngamntrValue = document.getElementById('txtcngamnt').value || 0;
    var resultA = parseFloat(txtidvovamntValue) + parseFloat(txtnoneleaccamntValue) + parseFloat(txteleaccamntValue) + parseFloat(txtsidecaramntValue) + parseFloat(txtcngamntrValue);
    //var resultround = Math.round(result).toFixed(2);
    //alert(result)
    //var DepreB = Math.round(DepreciationValue).toFixed(2);
    var DepreB = ((parseFloat(resultA)) / 100) * parseFloat(DepreciationValue);
    var ValueC = parseFloat(resultA) - parseFloat(DepreB);
    var TotalPVV = Math.round(ValueC).toFixed(2);
    document.getElementById('txttotamnt').value = ReplaceNumberWithCommas(TotalPVV);
    PVVAmount = TotalPVV;
    //if (getmonthDiff <= 6) {
    //    var IDVTot = ((parseFloat(result)) / 100) * 5;
    //    //alert(IDVTot)
    //    if (!isNaN(IDVTot)) {
    //        var res1 = Math.round(IDVTot).toFixed(2)
    //        var res2 = parseFloat(result) - parseFloat(res1)
    //        var totres = Math.round(res2).toFixed(2)
    //        document.getElementById('txttotamnt').value = ReplaceNumberWithCommas(totres);
    //        PVVAmount = totres;
    //        //alert(getmonthDiff)
    //    }
    //}

    // Premium Calculation Start
    if (PolicyMonths >= 33) {
        if (ddlTypeofCover == "2") {
            //alertify.success("P");
            var txttotamntValue = PVVAmount;
            //txtbpidv
            var VCategory = $('#ddlVehCatType').val();
            console.log(VCategory)
            if (VCategory == '11' || VCategory == '12') {
                var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue) + parseFloat(txtAdditionAmtValue));
                console.log(txtbpidvValue)
            }
            else {
                var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));
                console.log(txtbpidvValue)
            }
            //var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));

            if (!isNaN(txtbpidvValue)) {
                var res = Math.round(txtbpidvValue).toFixed(2)
                //alert(res)
                //document.getElementById('txtbpidv').value = ReplaceNumberWithCommas(res);
                //document.getElementById('txtidvsubtot').value = ReplaceNumberWithCommas(res);
                //document.getElementById('txtodp').value = ReplaceNumberWithCommas(res);
            }
            //txtlgrod
            var txtlgrodValue = ((parseFloat(res)) / 100) * (parseFloat(txtODgovDiscntValue));
            if (!isNaN(txtlgrodValue)) {
                var res1 = Math.round(txtlgrodValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlgrod').value = ReplaceNumberWithCommas(res1);
                //document.getElementById('txtrebatetotod').value = res1;
            }
            //txtrebatetotod
            var txtrebatetotodvalue = parseFloat(res) - parseFloat(res1)
            var res2 = Math.round(txtrebatetotodvalue).toFixed(2)
            //document.getElementById('txtrebatetotod').value = ReplaceNumberWithCommas(res2);

            //txtsubtotlpgod
            var txtsubtotlpgodValue = Math.round(txtrebatetotodvalue).toFixed(2);
            //document.getElementById('txtsubtotlpgod').value = ReplaceNumberWithCommas(txtsubtotlpgodValue);

            //txth
            var txthValue = ((parseFloat(txtsubtotlpgodValue)) / 100) * 1;
            //document.getElementById('txth').value = ReplaceNumberWithCommas(txthValue);

            //txtsubtotextra
            var txtsubtotextraVlaue = Math.round(txtsubtotlpgodValue).toFixed(2);
            //document.getElementById('txtsubtotextra').value = ReplaceNumberWithCommas(txtsubtotextraVlaue);

            //txtAddMalus
            var txtaddmalusValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * parseFloat(MalusValue);
            if (!isNaN(txtaddmalusValue)) {
                var res3malus = Math.round(txtaddmalusValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
                //document.getElementById('txtrebatetotod').value = res1;
            }


            //txtlessncb
            var txtlessncbValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * parseFloat(NCBValue);
            if (!isNaN(txtlessncbValue)) {
                var res3 = Math.round(txtlessncbValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
                //document.getElementById('txtrebatetotod').value = res1;
            }

            //txtodtot
            var txtodtotValue = parseFloat(res3malus) + parseFloat(txtsubtotextraVlaue) - parseFloat(res3)
            var res4 = Math.round(txtodtotValue).toFixed(2)
            //document.getElementById('txtodtot').value = ReplaceNumberWithCommas(res4);
            //document.getElementById('txtodtotA').value = ReplaceNumberWithCommas(res4);

            //B. LIABILITY TO PUBLIC RISK
            //txtlgrlpr
            //var txtlprValue = document.getElementById('txtlpr').value || 0;
            var txtlprValue = parseFloat(PLValue);
            var res5 = Math.round(txtlprValue).toFixed(2)
            //txtbpidv
            var txtlgrlprValue = ((parseFloat(res5)) / 100) * (parseFloat(txtPLgovDiscountValue));
            var res6 = Math.round(txtlgrlprValue).toFixed(2)
            //document.getElementById('txtlgrlpr').value = ReplaceNumberWithCommas(res6);
            //txtsubtotlpr
            var txtsubtotlprValue = parseFloat(res5) - parseFloat(res6)
            var res7 = Math.round(txtsubtotlprValue).toFixed(2)
            //document.getElementById('txtsubtotlpr').value = ReplaceNumberWithCommas(res7);

            //txtlpgkitlpr
            var txtcngamntrValue = 0;
            var txtlpgkitlprValue = ((parseFloat(txtcngamntrValue)) / 100) * 60;
            var res8 = Math.round(txtlpgkitlprValue).toFixed(2)
            //document.getElementById('txtlpgkitlpr').value = ReplaceNumberWithCommas(res8);

            //txtsubtotlpglpr
            var txtsubtotlpglprValue = parseFloat(res7) + parseFloat(res8)
            var res9 = Math.round(txtsubtotlpglprValue).toFixed(2)
            //document.getElementById('txtsubtotlpglpr').value = ReplaceNumberWithCommas(res9);

            //txtdrlpr
            var res10 = (parseFloat(txtPLDriverAmtValue));
            //document.getElementById('txtdrlpr').value = ReplaceNumberWithCommas(res10);
            //txtprlpr
            var res11 = (parseFloat(txtPLPassengerAmtValue));
            //document.getElementById('txtprlpr').value = ReplaceNumberWithCommas(res11);

            //txtlprtot
            var txtlprtotValue = parseFloat(res9) + parseFloat(res10) + parseFloat(res11)
            var res12 = Math.round(txtlprtotValue).toFixed(2)
            //document.getElementById('txtlprtot').value = ReplaceNumberWithCommas(res12);
            //document.getElementById('txtlprtotB').value = ReplaceNumberWithCommas(res12);

            //txttotAB
            var txttotABValue = parseFloat(res4) + parseFloat(res12)
            var res13 = Math.round(txttotABValue).toFixed(2)
            //document.getElementById('txttotAB').value = ReplaceNumberWithCommas(res13);
            //document.getElementById('txtpremium').value = ReplaceNumberWithCommas(res13);

            //txtgstamt
            var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
            var res14 = Math.round(txtgstamtValue).toFixed(2)
            //document.getElementById('txtgstamt').value = ReplaceNumberWithCommas(res14);

            //txttotalcrpremium
            var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14)
            var res15 = Math.round(txttotalcrpremiumValue).toFixed(2)
            //document.getElementById('txttotalcrpremium').value = ReplaceNumberWithCommas(res15);
            //txtTotalPremium
            document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
            PremiumPayableAmount = res15;

            // Premium Calculation End
            //if (!isNaN(result)) {
            //    var res = parseFloat(result).toFixed(2)
            //    document.getElementById('txttotamnt').value = res;
            //    alert(getmonthDiff)
            //}
            //Addition Popup For View Premium Details
            $('#idvpercent').text(ODValue);
            $('#txtbpidv').val(res);
            $('#txtidvsubtot').val(res);
            $('#txtodp').val(res);
            $('#txtlgrod').val(res1);
            $('#txtrebatetotod').val(res4);
            $('#txtsubtotlpgod').val(res4);
            $('#txtsubtotextra').val(res4);
            $('#txtodtot').val(res4);

            $('#txtlpr').val(PLValue);
            $('#txtlgrlpr').val(res6);
            $('#txtsubtotlpr').val(res7);
            $('#txtsubtotlpglpr').val(res9);
            $('#txtdrlpr').val(txtPLDriverAmtValue);
            $('#txtprlpr').val(txtPLPassengerAmtValue);
            $('#txtlprtot').val(res12);
            $('#txtodtotA').val(res4);
            $('#txtlprtotB').val(res12);
            $('#txttotAB').val(res13);

            $('#txtgstamt').val(res14);
            $('#txtTotalPremiumAmt').val(res15);
        }
        else if (ddlTypeofCover == "1") {
            //alertify.success("L");
            var txttotamntValue = PVVAmount;

            //txtodtot
            var res4 = 0;

            //B. LIABILITY TO PUBLIC RISK
            //txtlgrlpr
            //var txtlprValue = document.getElementById('txtlpr').value || 0;
            var txtlprValue = parseFloat(PLValue);
            var res5 = Math.round(txtlprValue).toFixed(2)
            //txtbpidv
            var txtlgrlprValue = ((parseFloat(res5)) / 100) * (parseFloat(txtPLgovDiscountValue));
            var res6 = Math.round(txtlgrlprValue).toFixed(2)
            //document.getElementById('txtlgrlpr').value = ReplaceNumberWithCommas(res6);
            //txtsubtotlpr
            var txtsubtotlprValue = parseFloat(res5) - parseFloat(res6)
            var res7 = Math.round(txtsubtotlprValue).toFixed(2)
            //document.getElementById('txtsubtotlpr').value = ReplaceNumberWithCommas(res7);

            //txtlpgkitlpr
            var txtcngamntrValue = 0;
            var txtlpgkitlprValue = ((parseFloat(txtcngamntrValue)) / 100) * 60;
            var res8 = Math.round(txtlpgkitlprValue).toFixed(2)
            //document.getElementById('txtlpgkitlpr').value = ReplaceNumberWithCommas(res8);

            //txtsubtotlpglpr
            var txtsubtotlpglprValue = parseFloat(res7) + parseFloat(res8)
            var res9 = Math.round(txtsubtotlpglprValue).toFixed(2)
            //document.getElementById('txtsubtotlpglpr').value = ReplaceNumberWithCommas(res9);

            //txtdrlpr
            var res10 = (parseFloat(txtPLDriverAmtValue));
            //document.getElementById('txtdrlpr').value = ReplaceNumberWithCommas(res10);
            //txtprlpr
            var res11 = (parseFloat(txtPLPassengerAmtValue));
            //document.getElementById('txtprlpr').value = ReplaceNumberWithCommas(res11);

            //txtlprtot
            var txtlprtotValue = parseFloat(res9) + parseFloat(res10) + parseFloat(res11)
            var res12 = Math.round(txtlprtotValue).toFixed(2)
            //document.getElementById('txtlprtot').value = ReplaceNumberWithCommas(res12);
            //document.getElementById('txtlprtotB').value = ReplaceNumberWithCommas(res12);

            //txttotAB
            var txttotABValue = parseFloat(res4) + parseFloat(res12)
            var res13 = Math.round(txttotABValue).toFixed(2)
            //document.getElementById('txttotAB').value = ReplaceNumberWithCommas(res13);
            //document.getElementById('txtpremium').value = ReplaceNumberWithCommas(res13);

            //txtgstamt
            var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
            var res14 = Math.round(txtgstamtValue).toFixed(2)
            //document.getElementById('txtgstamt').value = ReplaceNumberWithCommas(res14);

            //txttotalcrpremium
            var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14)
            var res15 = Math.round(txttotalcrpremiumValue).toFixed(2)
            //document.getElementById('txttotalcrpremium').value = ReplaceNumberWithCommas(res15);
            //txtTotalPremium
            document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
            PremiumPayableAmount = res15;

            // Premium Calculation End
            //if (!isNaN(result)) {
            //    var res = parseFloat(result).toFixed(2)
            //    document.getElementById('txttotamnt').value = res;
            //    alert(getmonthDiff)
            //}
            //Addition Popup For View Premium Details
            $('#idvpercent').text("");
            $('#txtbpidv').val("");
            $('#txtidvsubtot').val("");
            $('#txtlgrod').val("");
            $('#txtrebatetotod').val("");
            $('#txtsubtotlpgod').val("");
            $('#txtsubtotextra').val("");
            $('#txtodtot').val("");

            $('#txtlpr').val(PLValue);
            $('#txtlgrlpr').val(res6);
            $('#txtsubtotlpr').val(res7);
            $('#txtsubtotlpglpr').val(res9);
            $('#txtdrlpr').val(txtPLDriverAmtValue);
            $('#txtprlpr').val(txtPLPassengerAmtValue);
            $('#txtlprtot').val(res12);
            $('#txtodtotA').val(res4);
            $('#txtlprtotB').val(res12);
            $('#txttotAB').val(res13);

            $('#txtgstamt').val(res14);
            $('#txtTotalPremiumAmt').val(res15);
        }
        else if (ddlTypeofCover == "3") {
            //alertify.success("O");
            var txttotamntValue = PVVAmount;
            //txtbpidv
            var VCategory = $('#ddlVehCatType').val();
            console.log(VCategory)
            if (VCategory == '11' || VCategory == '12') {
                var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue) + parseFloat(txtAdditionAmtValue));
                console.log(txtbpidvValue)
            }
            else {
                var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));
                console.log(txtbpidvValue)
            }
            //var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));

            if (!isNaN(txtbpidvValue)) {
                var res = Math.round(txtbpidvValue).toFixed(2)
                //alert(res)
                //document.getElementById('txtbpidv').value = ReplaceNumberWithCommas(res);
                //document.getElementById('txtidvsubtot').value = ReplaceNumberWithCommas(res);
                //document.getElementById('txtodp').value = ReplaceNumberWithCommas(res);
            }
            //txtlgrod
            var txtlgrodValue = ((parseFloat(res)) / 100) * (parseFloat(txtODgovDiscntValue));
            if (!isNaN(txtlgrodValue)) {
                var res1 = Math.round(txtlgrodValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlgrod').value = ReplaceNumberWithCommas(res1);
                //document.getElementById('txtrebatetotod').value = res1;
            }
            //txtrebatetotod
            var txtrebatetotodvalue = parseFloat(res) - parseFloat(res1)
            var res2 = Math.round(txtrebatetotodvalue).toFixed(2)
            //document.getElementById('txtrebatetotod').value = ReplaceNumberWithCommas(res2);

            //txtsubtotlpgod
            var txtsubtotlpgodValue = Math.round(txtrebatetotodvalue).toFixed(2);
            //document.getElementById('txtsubtotlpgod').value = ReplaceNumberWithCommas(txtsubtotlpgodValue);

            //txth
            var txthValue = ((parseFloat(txtsubtotlpgodValue)) / 100) * 1;
            //document.getElementById('txth').value = ReplaceNumberWithCommas(txthValue);

            //txtsubtotextra
            var txtsubtotextraVlaue = Math.round(txtsubtotlpgodValue).toFixed(2);
            //document.getElementById('txtsubtotextra').value = ReplaceNumberWithCommas(txtsubtotextraVlaue);

            //txtAddMalus
            var txtaddmalusValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * parseFloat(MalusValue);
            if (!isNaN(txtaddmalusValue)) {
                var res3malus = Math.round(txtaddmalusValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
                //document.getElementById('txtrebatetotod').value = res1;
            }


            //txtlessncb
            var txtlessncbValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * parseFloat(NCBValue);
            if (!isNaN(txtlessncbValue)) {
                var res3 = Math.round(txtlessncbValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
                //document.getElementById('txtrebatetotod').value = res1;
            }

            //txtodtot
            var txtodtotValue = parseFloat(res3malus) + parseFloat(txtsubtotextraVlaue) - parseFloat(res3)
            var res4 = Math.round(txtodtotValue).toFixed(2)
            //document.getElementById('txtodtot').value = ReplaceNumberWithCommas(res4);
            //document.getElementById('txtodtotA').value = ReplaceNumberWithCommas(res4);

            //B. LIABILITY TO PUBLIC RISK

            var res12 = 0;
            //document.getElementById('txtlprtot').value = ReplaceNumberWithCommas(res12);
            //document.getElementById('txtlprtotB').value = ReplaceNumberWithCommas(res12);

            //txttotAB
            var txttotABValue = parseFloat(res4) + parseFloat(res12)
            var res13 = Math.round(txttotABValue).toFixed(2)
            //document.getElementById('txttotAB').value = ReplaceNumberWithCommas(res13);
            //document.getElementById('txtpremium').value = ReplaceNumberWithCommas(res13);

            //txtgstamt
            var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
            var res14 = Math.round(txtgstamtValue).toFixed(2)
            //document.getElementById('txtgstamt').value = ReplaceNumberWithCommas(res14);

            //txttotalcrpremium
            var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14)
            var res15 = Math.round(txttotalcrpremiumValue).toFixed(2)
            //document.getElementById('txttotalcrpremium').value = ReplaceNumberWithCommas(res15);
            //txtTotalPremium
            document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
            PremiumPayableAmount = res15;

            // Premium Calculation End
            //if (!isNaN(result)) {
            //    var res = parseFloat(result).toFixed(2)
            //    document.getElementById('txttotamnt').value = res;
            //    alert(getmonthDiff)
            //}
            //Addition Popup For View Premium Details
            $('#idvpercent').text(ODValue);
            $('#txtbpidv').val(res);
            $('#txtidvsubtot').val(res);
            $('#txtodp').val(res);
            $('#txtlgrod').val(res1);
            $('#txtrebatetotod').val(res4);
            $('#txtsubtotlpgod').val(res4);
            $('#txtsubtotextra').val(res4);
            $('#txtodtot').val(res4);

            $('#txtlpr').val("");
            $('#txtlgrlpr').val("");
            $('#txtsubtotlpr').val("");
            $('#txtsubtotlpglpr').val("");
            $('#txtdrlpr').val("");
            $('#txtprlpr').val("");
            $('#txtlprtot').val("");
            $('#txtodtotA').val(res4);
            $('#txtlprtotB').val(res12);
            $('#txttotAB').val(res13);

            $('#txtgstamt').val(res14);
            $('#txtTotalPremiumAmt').val(res15);
        }
    }
    else {
        if (ddlTypeofCover == "2") {
            //alertify.success("P");
            var txttotamntValue = PVVAmount;
            //txtbpidv
            var VCategory = $('#ddlVehCatType').val();
            console.log(VCategory)
            if (VCategory == '11' || VCategory == '12') {
                var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue) + parseFloat(txtAdditionAmtValue));
                console.log(txtbpidvValue)
            }
            else {
                var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));
                console.log(txtbpidvValue)
            }
            //var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));

            if (!isNaN(txtbpidvValue)) {
                var res = Math.round(txtbpidvValue).toFixed(2)
                //alert(res)
                //document.getElementById('txtbpidv').value = ReplaceNumberWithCommas(res);
                //document.getElementById('txtidvsubtot').value = ReplaceNumberWithCommas(res);
                //document.getElementById('txtodp').value = ReplaceNumberWithCommas(res);
            }
            //txtlgrod
            var txtlgrodValue = ((parseFloat(res)) / 100) * (parseFloat(txtODgovDiscntValue));
            if (!isNaN(txtlgrodValue)) {
                var res1 = Math.round(txtlgrodValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlgrod').value = ReplaceNumberWithCommas(res1);
                //document.getElementById('txtrebatetotod').value = res1;
            }
            //txtrebatetotod
            var txtrebatetotodvalue = parseFloat(res) - parseFloat(res1)
            var res2 = Math.round(txtrebatetotodvalue).toFixed(2)
            //document.getElementById('txtrebatetotod').value = ReplaceNumberWithCommas(res2);

            //txtsubtotlpgod
            var txtsubtotlpgodValue = Math.round(txtrebatetotodvalue).toFixed(2);
            //document.getElementById('txtsubtotlpgod').value = ReplaceNumberWithCommas(txtsubtotlpgodValue);

            //txth
            var txthValue = ((parseFloat(txtsubtotlpgodValue)) / 100) * 1;
            //document.getElementById('txth').value = ReplaceNumberWithCommas(txthValue);

            //txtsubtotextra
            var txtsubtotextraVlaue = Math.round(txtsubtotlpgodValue).toFixed(2);
            //document.getElementById('txtsubtotextra').value = ReplaceNumberWithCommas(txtsubtotextraVlaue);

            //txtAddMalus
            var txtaddmalusValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * parseFloat(MalusValue);
            if (!isNaN(txtaddmalusValue)) {
                var res3malus = Math.round(txtaddmalusValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
                //document.getElementById('txtrebatetotod').value = res1;
            }


            //txtlessncb
            var txtlessncbValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * parseFloat(NCBValue);
            if (!isNaN(txtlessncbValue)) {
                var res3 = Math.round(txtlessncbValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
                //document.getElementById('txtrebatetotod').value = res1;
            }

            //txtodtot
            var txtodtotValue = parseFloat(res3malus) + parseFloat(txtsubtotextraVlaue) - parseFloat(res3)
            var res4 = Math.round(txtodtotValue).toFixed(2)
            //document.getElementById('txtodtot').value = ReplaceNumberWithCommas(res4);
            //document.getElementById('txtodtotA').value = ReplaceNumberWithCommas(res4);

            //B. LIABILITY TO PUBLIC RISK

            var res12 = 0;
            //document.getElementById('txtlprtot').value = ReplaceNumberWithCommas(res12);
            //document.getElementById('txtlprtotB').value = ReplaceNumberWithCommas(res12);

            //txttotAB
            var txttotABValue = parseFloat(res4) + parseFloat(res12)
            var res13 = Math.round(txttotABValue).toFixed(2)
            //document.getElementById('txttotAB').value = ReplaceNumberWithCommas(res13);
            //document.getElementById('txtpremium').value = ReplaceNumberWithCommas(res13);

            //txtgstamt
            var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
            var res14 = Math.round(txtgstamtValue).toFixed(2)
            //document.getElementById('txtgstamt').value = ReplaceNumberWithCommas(res14);

            //txttotalcrpremium
            var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14)
            var res15 = Math.round(txttotalcrpremiumValue).toFixed(2)
            //document.getElementById('txttotalcrpremium').value = ReplaceNumberWithCommas(res15);
            //txtTotalPremium
            document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
            PremiumPayableAmount = res15;

            // Premium Calculation End
            //if (!isNaN(result)) {
            //    var res = parseFloat(result).toFixed(2)
            //    document.getElementById('txttotamnt').value = res;
            //    alert(getmonthDiff)
            //}
            //Addition Popup For View Premium Details
            $('#idvpercent').text(ODValue);
            $('#txtbpidv').val(res);
            $('#txtidvsubtot').val(res);
            $('#txtlgrod').val(res1);
            $('#txtrebatetotod').val(res4);
            $('#txtsubtotlpgod').val(res4);
            $('#txtsubtotextra').val(res4);
            $('#txtodtot').val(res4);

            $('#txtlpr').val("");
            $('#txtlgrlpr').val("");
            $('#txtsubtotlpr').val("");
            $('#txtsubtotlpglpr').val("");
            $('#txtdrlpr').val("");
            $('#txtprlpr').val("");
            $('#txtlprtot').val("");
            $('#txtodtotA').val(res4);
            $('#txtlprtotB').val(res12);
            $('#txttotAB').val(res13);

            $('#txtgstamt').val(res14);
            $('#txtTotalPremiumAmt').val(res15);
        }
        else if (ddlTypeofCover == "1") {
            //alertify.success("L");
            var txttotamntValue = PVVAmount;

            //txtodtot
            var res4 = 0;

            //B. LIABILITY TO PUBLIC RISK
            //txtlgrlpr
            //var txtlprValue = document.getElementById('txtlpr').value || 0;
            var txtlprValue = parseFloat(PLValue);
            var res5 = Math.round(txtlprValue).toFixed(2)
            //txtbpidv
            var txtlgrlprValue = ((parseFloat(res5)) / 100) * (parseFloat(txtPLgovDiscountValue));
            var res6 = Math.round(txtlgrlprValue).toFixed(2)
            //document.getElementById('txtlgrlpr').value = ReplaceNumberWithCommas(res6);
            //txtsubtotlpr
            var txtsubtotlprValue = parseFloat(res5) - parseFloat(res6)
            var res7 = Math.round(txtsubtotlprValue).toFixed(2)
            //document.getElementById('txtsubtotlpr').value = ReplaceNumberWithCommas(res7);

            //txtlpgkitlpr
            var txtcngamntrValue = 0;
            var txtlpgkitlprValue = ((parseFloat(txtcngamntrValue)) / 100) * 60;
            var res8 = Math.round(txtlpgkitlprValue).toFixed(2)
            //document.getElementById('txtlpgkitlpr').value = ReplaceNumberWithCommas(res8);

            //txtsubtotlpglpr
            var txtsubtotlpglprValue = parseFloat(res7) + parseFloat(res8)
            var res9 = Math.round(txtsubtotlpglprValue).toFixed(2)
            //document.getElementById('txtsubtotlpglpr').value = ReplaceNumberWithCommas(res9);

            //txtdrlpr
            var res10 = (parseFloat(txtPLDriverAmtValue));
            //document.getElementById('txtdrlpr').value = ReplaceNumberWithCommas(res10);
            //txtprlpr
            var res11 = (parseFloat(txtPLPassengerAmtValue));
            //document.getElementById('txtprlpr').value = ReplaceNumberWithCommas(res11);

            //txtlprtot
            var txtlprtotValue = parseFloat(res9) + parseFloat(res10) + parseFloat(res11)
            var res12 = Math.round(txtlprtotValue).toFixed(2)
            //document.getElementById('txtlprtot').value = ReplaceNumberWithCommas(res12);
            //document.getElementById('txtlprtotB').value = ReplaceNumberWithCommas(res12);

            //txttotAB
            var txttotABValue = parseFloat(res4) + parseFloat(res12)
            var res13 = Math.round(txttotABValue).toFixed(2)
            //document.getElementById('txttotAB').value = ReplaceNumberWithCommas(res13);
            //document.getElementById('txtpremium').value = ReplaceNumberWithCommas(res13);

            //txtgstamt
            var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
            var res14 = Math.round(txtgstamtValue).toFixed(2)
            //document.getElementById('txtgstamt').value = ReplaceNumberWithCommas(res14);

            //txttotalcrpremium
            var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14)
            var res15 = Math.round(txttotalcrpremiumValue).toFixed(2)
            //document.getElementById('txttotalcrpremium').value = ReplaceNumberWithCommas(res15);
            //txtTotalPremium
            document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
            PremiumPayableAmount = res15;

            // Premium Calculation End
            //if (!isNaN(result)) {
            //    var res = parseFloat(result).toFixed(2)
            //    document.getElementById('txttotamnt').value = res;
            //    alert(getmonthDiff)
            //}
            //Addition Popup For View Premium Details
            $('#idvpercent').text("");
            $('#txtbpidv').val("");
            $('#txtidvsubtot').val("");
            $('#txtlgrod').val("");
            $('#txtrebatetotod').val("");
            $('#txtsubtotlpgod').val("");
            $('#txtsubtotextra').val("");
            $('#txtodtot').val("");

            $('#txtlpr').val(PLValue);
            $('#txtlgrlpr').val(res6);
            $('#txtsubtotlpr').val(res7);
            $('#txtsubtotlpglpr').val(res9);
            $('#txtdrlpr').val(txtPLDriverAmtValue);
            $('#txtprlpr').val(txtPLPassengerAmtValue);
            $('#txtlprtot').val(res12);
            $('#txtodtotA').val(res4);
            $('#txtlprtotB').val(res12);
            $('#txttotAB').val(res13);

            $('#txtgstamt').val(res14);
            $('#txtTotalPremiumAmt').val(res15);
        }
        else if (ddlTypeofCover == "3") {
            //alertify.success("O");
            var txttotamntValue = PVVAmount;
            //txtbpidv
            var VCategory = $('#ddlVehCatType').val();
            console.log(VCategory)
            if (VCategory == '11' || VCategory == '12') {
                var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue) + parseFloat(txtAdditionAmtValue));
                console.log(txtbpidvValue)
            }
            else {
                var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));
                console.log(txtbpidvValue)
            }
            //var txtbpidvValue = ((parseFloat(txttotamntValue)) / 100) * (parseFloat(ODValue));

            if (!isNaN(txtbpidvValue)) {
                var res = Math.round(txtbpidvValue).toFixed(2)
                //alert(res)
                //document.getElementById('txtbpidv').value = ReplaceNumberWithCommas(res);
                //document.getElementById('txtidvsubtot').value = ReplaceNumberWithCommas(res);
                //document.getElementById('txtodp').value = ReplaceNumberWithCommas(res);
            }
            //txtlgrod
            var txtlgrodValue = ((parseFloat(res)) / 100) * (parseFloat(txtODgovDiscntValue));
            if (!isNaN(txtlgrodValue)) {
                var res1 = Math.round(txtlgrodValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlgrod').value = ReplaceNumberWithCommas(res1);
                //document.getElementById('txtrebatetotod').value = res1;
            }
            //txtrebatetotod
            var txtrebatetotodvalue = parseFloat(res) - parseFloat(res1)
            var res2 = Math.round(txtrebatetotodvalue).toFixed(2)
            //document.getElementById('txtrebatetotod').value = ReplaceNumberWithCommas(res2);

            //txtsubtotlpgod
            var txtsubtotlpgodValue = Math.round(txtrebatetotodvalue).toFixed(2);
            //document.getElementById('txtsubtotlpgod').value = ReplaceNumberWithCommas(txtsubtotlpgodValue);

            //txth
            var txthValue = ((parseFloat(txtsubtotlpgodValue)) / 100) * 1;
            //document.getElementById('txth').value = ReplaceNumberWithCommas(txthValue);

            //txtsubtotextra
            var txtsubtotextraVlaue = Math.round(txtsubtotlpgodValue).toFixed(2);
            //document.getElementById('txtsubtotextra').value = ReplaceNumberWithCommas(txtsubtotextraVlaue);

            //txtAddMalus
            var txtaddmalusValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * parseFloat(MalusValue);
            if (!isNaN(txtaddmalusValue)) {
                var res3malus = Math.round(txtaddmalusValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
                //document.getElementById('txtrebatetotod').value = res1;
            }


            //txtlessncb
            var txtlessncbValue = ((parseFloat(txtsubtotextraVlaue)) / 100) * parseFloat(NCBValue);
            if (!isNaN(txtlessncbValue)) {
                var res3 = Math.round(txtlessncbValue).toFixed(2)
                //alert(res1)
                //document.getElementById('txtlessncb').value = ReplaceNumberWithCommas(res3);
                //document.getElementById('txtrebatetotod').value = res1;
            }

            //txtodtot
            var txtodtotValue = parseFloat(res3malus) + parseFloat(txtsubtotextraVlaue) - parseFloat(res3)
            var res4 = Math.round(txtodtotValue).toFixed(2)
            //document.getElementById('txtodtot').value = ReplaceNumberWithCommas(res4);
            //document.getElementById('txtodtotA').value = ReplaceNumberWithCommas(res4);

            //B. LIABILITY TO PUBLIC RISK

            var res12 = 0;
            //document.getElementById('txtlprtot').value = ReplaceNumberWithCommas(res12);
            //document.getElementById('txtlprtotB').value = ReplaceNumberWithCommas(res12);

            //txttotAB
            var txttotABValue = parseFloat(res4) + parseFloat(res12)
            var res13 = Math.round(txttotABValue).toFixed(2)
            //document.getElementById('txttotAB').value = ReplaceNumberWithCommas(res13);
            //document.getElementById('txtpremium').value = ReplaceNumberWithCommas(res13);

            //txtgstamt
            var txtgstamtValue = ((parseFloat(res13)) / 100) * 18;
            var res14 = Math.round(txtgstamtValue).toFixed(2)
            //document.getElementById('txtgstamt').value = ReplaceNumberWithCommas(res14);

            //txttotalcrpremium
            var txttotalcrpremiumValue = parseFloat(res13) + parseFloat(res14)
            var res15 = Math.round(txttotalcrpremiumValue).toFixed(2)
            //document.getElementById('txttotalcrpremium').value = ReplaceNumberWithCommas(res15);
            //txtTotalPremium
            document.getElementById('txtTotalPremium').value = ReplaceNumberWithCommas(res15);
            PremiumPayableAmount = res15;

            // Premium Calculation End
            //if (!isNaN(result)) {
            //    var res = parseFloat(result).toFixed(2)
            //    document.getElementById('txttotamnt').value = res;
            //    alert(getmonthDiff)
            //}
            //Addition Popup For View Premium Details
            $('#idvpercent').text(ODValue);
            $('#txtbpidv').val(res);
            $('#txtidvsubtot').val(res);
            $('#txtlgrod').val(res1);
            $('#txtrebatetotod').val(res4);
            $('#txtsubtotlpgod').val(res4);
            $('#txtsubtotextra').val(res4);
            $('#txtodtot').val(res4);

            $('#txtlpr').val("");
            $('#txtlgrlpr').val("");
            $('#txtsubtotlpr').val("");
            $('#txtsubtotlpglpr').val("");
            $('#txtdrlpr').val("");
            $('#txtprlpr').val("");
            $('#txtlprtot').val("");
            $('#txtodtotA').val(res4);
            $('#txtlprtotB').val(res12);
            $('#txttotAB').val(res13);

            $('#txtgstamt').val(res14);
            $('#txtTotalPremiumAmt').val(res15);
        }
    }


}
function SaveLoanDetailsMI() {
    var LDserverResponse = false;
    ValidateLoanDetails();
    var eid = $("#divMILoanDetails").data("eid");
    if ($('.err:visible').length === 0) {
        var ViewModel = {
            "vehicleLoanId": $("#txtVehicleLoanID").val(), "EmployeeId": eid, "MI_Loan_start_date": $("#txtVLSD").val(),
            "MI_Loan_end_date": $("#txtVLED").val(), "MI_Loan_Amount": $("#txtLoanAmount").val()
        }

        var submitUrl = $("#divMILoanDetails").data("submit-url");
        $.ajax({
            url: submitUrl,
            data:
                JSON.stringify({ "objMILoanDetails": ViewModel }),

            type: 'POST',
            async: false,
            cache: false,
            contentType: 'application/json; charset=utf-8',
            processData: false,
            success: function (response) {
                if (response.IsSuccess) {

                    alertify.success("Loan details saved successful");
                    LDserverResponse = true;
                    setTimeout(function () {
                        window.location.href = "/mi-e-fa";
                    }, 1000);

                }
                else {
                    alertify.error("Could not save Loan Details");
                }
            }, error: function (response) {
                alertify.error("Could not save Loan Details");
            }
        });
    }

    return LDserverResponse;
}


function ValidateLoanDetails() {
    $('.err').attr('hidden', true);

    if ($("#txtVehicleLoanID").val() == "") {
        $("#errVDLoanIDReq").removeAttr("hidden");
        return false;
    }
    if ($("#txtLoanAmount").val() == "0") {
        $("#errVDLoanAmountReq").removeAttr("hidden");
        return false;
    }
    if ($("#txtVLSD").val() == "") {
        $("#errVDLEDsReq").removeAttr("hidden");
        return false;
    }

    else if ($("#txtVLSD").val() != "") {
        var today = new Date();
        var value = dateDiff($("#txtVLSD").val(), today);
        if (value.years > 3 && value.days >= 0) {
            $("#errerrorReq").removeAttr("hidden");
            $("#errerrorReq").text("Not Eligible for applying for fresh application, Loan tenure cannot exceed 3 years.")
            return false;
        }
    }
}

function dateDiff(startdate, enddate) {
    //define moments for the startdate and enddate
    var startdateMoment = moment(startdate, 'DD-MM-YYYY');
    var enddateMoment = moment(enddate, 'DD-MM-YYYY');

    if (
        startdateMoment.isValid() === true &&
        enddateMoment.isValid() === true
    ) {
        //getting the difference in years
        var years = enddateMoment.diff(startdateMoment, 'years');

        //moment returns the total months between the two dates, subtracting the years
        var months =
            enddateMoment.diff(startdateMoment, 'months') - years * 12;

        //to calculate the days, first get the previous month and then subtract it
        startdateMoment.add(years, 'years').add(months, 'months');
        var days = enddateMoment.diff(startdateMoment, 'days');

        return {
            years: years,
            months: months,
            days: days,
        };
    } else {
        return undefined;
    }
}


function GetVahanDetails() {
    debugger;
    ChasisNOValidation();

    var vehiclechassisNo = {
        'mivd_chasis_no': $("#txtVDChasisNo").val(),
        'mivd_engine_no': null,
    };
    $.ajax({
        url: '/MotorInsurance/CheckVehicleExists',
        //data:JSON.stringify({ vehicleDetails: $("#txtVDChasisNo").val() }),
        data: JSON.stringify(vehiclechassisNo),

        type: 'POST',
        async: false,
        cache: false,
        contentType: 'application/json; charset=utf-8',
        processData: false,
        success: function (response) {
            if (response == 1) {
                alertify.confirm("Chasis No. already exists. ", function () {
                }).setHeader("Attention");
                $("#txtVDChasisNo").val("");
            }
            else if (response == 0) {
                if ($('.err:visible').length === 0) {
                    $.ajax({
                        url: "/Service/GetVehicleDetails",
                        data:
                            JSON.stringify({ chasis: $("#txtVDChasisNo").val() }),

                        type: 'POST',
                        async: false,
                        cache: false,
                        contentType: 'application/json; charset=utf-8',
                        processData: false,
                        success: function (response) {
                            if (response.IsSuccess) {
                                debugger;
                                $("#vaahanidvamount").val(response.VahanDetails.VehicleIDVAmount);
                                $("#txtVDRegistrationNo").val(response.VahanDetails.mivd_vehicle_reg_no);
                                $('#txtVDDateOfRegistration').val(response.VahanDetails.mivd_date_of_registration);
                                $("#ddlVDRTO").val(response.VahanDetails.mivd_vehicle_rto_id);
                                // $('#txtVDDateOfRegistration').val(response.VahanDetails.mivd_chasis_no);
                                $('#txtVDEngine').val(response.VahanDetails.mivd_engine_no);
                                $('#txtVDCubicCapacity').val(response.VahanDetails.mivd_cubic_capacity);
                                $('#txtVDSeating').val(response.VahanDetails.mivd_seating_capacity_including_driver);
                                $('#txtVDWeight').val(response.VahanDetails.mivd_vehicle_weight);
                                $("#ddlVehicleCatType").val($("#ddlVehicleCatType option:contains(" + response.VahanDetails.VehicleCategoryTypedesc + ")").val());

                                $("#ddlVDVehicleManufacture").val($("#ddlVDVehicleManufacture option:contains(" + response.VahanDetails.VehicleMakedesc + ")").val());

                                // $('#ddlVDVehicleManufacture').val($("#ddlVDVehicleManufacture option:contains(" + response.VahanDetails.VehicleManufacturedesc + ")").val());
                                $("#txtVDTypeOfModel").val(response.VahanDetails.mivd_type_of_model);
                                $("#ddlManufacturMonth").val(response.VahanDetails.mivd_manufacturer_month);
                                $("#ddlVDManufacturerYear").val($("#ddlVDManufacturerYear option:contains(" + response.VahanDetails.yeardesc + ")").val());

                                $('#ddlVDFuelType').val($("#ddlVDFuelType option:contains(" + response.VahanDetails.VehicleFueldesc + ")").val());
                                $('#ddlVehType').val($("#ddlVehType option:contains(" + response.VahanDetails.VehicleTypedesc + ")").val());
                                $('#ddlVehSubType').val($("#ddlVehSubType option:contains(" + response.VahanDetails.VehicleSubTypedesc + ")").val());
                                $('#ddlVehCat').val($("#ddlVehCat option:contains(" + response.VahanDetails.VehicleCategorydesc + ")").val());
                                $('#ddlVehClassType').val($("#ddlVehClassType option:contains(" + response.VahanDetails.VehicleClassdesc + ")").val());
                                var d = new Date();
                                var year = response.VahanDetails.yeardesc;
                                var month = parseInt(response.VahanDetails.mivd_manufacturer_month) - 1;
                                //$('#manu1').hide();
                                //$('#manu2').show();
                                //$('#txtVDDateOfManufacture1').datepicker({
                                //    dateFormat: 'dd-mm-yy',

                                //    changeYear: false,
                                //    changeMonth: false,
                                //    yearRange: year + ":0",  
                                //    defaultDate: new Date(year, month, 1),
                                //    onSelect: function (dateText, inst) {
                                //        alert(dateText);
                                //    }
                                //});

                                DisableFeilds();
                            }
                            else if (!response.IsSuccess) {
                                if (response.VahanDetails == "") {
                                    alertify.alert("Sorry the requested chasis_no already exists.").setHeader("Warning !!!");;
                                } else {
                                    alertify.confirm("Sorry the requested chasis_no could not be found.Do you want to enter the details manually?", function () {
                                        Clearfeilds();
                                        EnableFeilds();
                                    }).setHeader("Attention");
                                }


                                $("txtVDChasisNo").val("");

                            }
                        }, error: function (response) {
                            alertify.error("Could not save Loan Details");
                        }
                    });
                }
            }
        },
        error: function (response) {

        }
    });
}

function EnableDisableAllControls(controlID, Enable) {

    EnableAllButtons(controlID, Enable);
    var divControls = $("#" + controlID + " input[type=text]").attr("readonly", !Enable);
    var divControls = $("#" + controlID + " input[type=text]").attr("disabled", !Enable);
    var divControls = $("#" + controlID + " input[type=checkbox]").attr("disabled", !Enable);
    $("#" + controlID + " input[type=radio]").attr("disabled", !Enable);
    $("#" + controlID + " select").attr("disabled", !Enable);
    $("#" + controlID).attr("disabled", !Enable);
    //Disables datepicker
    $("#" + controlID + " span").attr("disabled", !Enable);

    $("#" + controlID + " [data-role=grid]").each(function (index) {
        // $(this).css("backgroundColor", "#EEE");
        var id = $(this).attr('id');
        if (Enable) {
            enableKendoGrid(id);
        }
        else {
            disableKendoGrid(id);
        }
    });

    $("#" + controlID + " textarea").attr("disabled", "disabled");
    $("#" + controlID + "[data-role=listview]").each(function (index) {
        var id = $(this).attr('id');
        // $("#" + id).data("kendoListView").enable(Enable);
    });
}

function DisableFeilds() {    $('#txtVDEngine').attr('readonly', true);    $('#txtVDSeating').attr('readonly', true);    $('#txtVDCubicCapacity').attr('readonly', true);    $('#txtVDWeight').attr('readonly', true);    $('#ddlVDTypeOfModel').attr('readonly', true);    $('#ddlVDVehicleManufacture').attr('readonly', true);    $('#ddlVDManufacturerYear').attr('readonly', true);    $('#ddlManufacturMonth').attr('readonly', true);    $('#ddlVDVehicleMake').attr('readonly', true);    $('#ddlVDRTO').attr('readonly', true);    $('#ddlVehicleCatType').attr('readonly', true);    $('#txtVDTypeOfModel').attr('readonly', true);    $('#ddlVDFuelType').attr('readonly', true);    $('#ddlVehType').attr('readonly', true);    $('#ddlVehSubType').attr('readonly', true);    $('#ddlVehCat').attr('readonly', true);    $('#ddlVehClassType').attr('readonly', true);    $('#txtVDEngine').css("pointer-events", "none");    $('#txtVDSeating').css("pointer-events", "none");    $('#txtVDCubicCapacity').css("pointer-events", "none");    $('#txtVDWeight').css("pointer-events", "none");    $('#ddlVDVehicleMake').css("pointer-events", "none");    $('#ddlVDVehicleManufacture').css("pointer-events", "none");    $('#ddlVDManufacturerYear').css("pointer-events", "none");    $('#ddlVDTypeOfModel').css("pointer-events", "none");    $('#ddlManufacturMonth').css("pointer-events", "none");    $('#ddlVDRTO').css("pointer-events", "none");    $('#ddlVehicleCatType').css("pointer-events", "none");    $('#txtVDTypeOfModel').css("pointer-events", "none");    $('#ddlVDFuelType').css("pointer-events", "none");    $('#ddlVehType').css("pointer-events", "none");    $('#ddlVehSubType').css("pointer-events", "none");    $('#ddlVehCat').css("pointer-events", "none");    $('#ddlVehClassType').css("pointer-events", "none");    $('#ddlVehicleCatType').attr("tabindex", "-1");    $('#txtVDTypeOfModel').attr("tabindex", "-1");    $('#ddlVDFuelType').attr("tabindex", "-1");    $('#ddlVehType').attr("tabindex", "-1");    $('#ddlVehSubType').attr("tabindex", "-1");    $('#ddlVehCat').attr("tabindex", "-1");    $('#ddlVehClassType').attr("tabindex", "-1")    $('#txtVDEngine').attr("tabindex", "-1");    $('#txtVDSeating').attr("tabindex", "-1");    $('#txtVDCubicCapacity').attr("tabindex", "-1");    $('#txtVDWeight').attr("tabindex", "-1");    $('#ddlVDVehicleMake').attr("tabindex", "-1");    $('#ddlVDVehicleManufacture').attr("tabindex", "-1");    $('#ddlVDManufacturerYear').attr("tabindex", "-1");    $('#ddlVDTypeOfModel').attr("tabindex", "-1");    $('#ddlManufacturMonth').attr("tabindex", "-1");    $('#ddlVDRTO').attr("tabindex", "-1");}
function EnableFeilds() {    $('#ddlVDRTO').attr('readonly', false);    $('#txtVDEngine').attr('readonly', false);    $('#ddlVDVehicleMake').attr('readonly', false);    $('#ddlVDVehicleManufacture').attr('readonly', false);    $('#ddlVDManufacturerYear').attr('readonly', false);    $('#txtVDSeating').attr('readonly', false);    $('#txtVDCubicCapacity').attr('readonly', false);    $('#txtVDWeight').attr('readonly', false);    $('#ddlVDTypeOfModel').attr('readonly', false);    $('#ddlVehicleCatType').attr('readonly', false);    $('#txtVDTypeOfModel').attr('readonly', false);    $('#ddlVDFuelType').attr('readonly', false);    $('#ddlVehType').attr('readonly', false);    $('#ddlVehSubType').attr('readonly', false);    $('#ddlVehCat').attr('readonly', false);    $('#ddlVehClassType').attr('readonly', false);    $('#ddlVehicleCatType').css("pointer-events", "auto");    $('#txtVDTypeOfModel').css("pointer-events", "auto");    $('#ddlVDFuelType').css("pointer-events", "auto");    $('#ddlVehType').css("pointer-events", "auto");    $('#ddlVehSubType').css("pointer-events", "auto");    $('#ddlVehCat').css("pointer-events", "auto");    $('#ddlVehClassType').css("pointer-events", "auto");    $('#ddlVDVehicleMake').css("pointer-events", "auto");    $('#ddlVDVehicleManufacture').css("pointer-events", "auto");    $('#ddlVDManufacturerYear').css("pointer-events", "auto");    $('#ddlVDTypeOfModel').css("pointer-events", "auto");    $('#ddlManufacturMonth').attr('readonly', false);    $('#ddlManufacturMonth').css("pointer-events", "auto");    $('#ddlVDRTO').attr('readonly', false);    $('#ddlVDRTO').css("pointer-events", "auto");    $('#txtVDSeating').css("pointer-events", "auto");    $('#txtVDCubicCapacity').css("pointer-events", "auto");    $('#txtVDWeight').css("pointer-events", "auto");    $('#txtVDEngine').css("pointer-events", "auto");}
function ChasisNOValidation() {
    var chasis = $("#txtVDChasisNo").val();

    if (chasis.trim() === null || chasis.trim() === "") {

        $("#errVDChassisNoReq").removeAttr("hidden");
        return false;
    }
    else {

        if (chasis.length < 8 || chasis.length > 17)  {

            $("#errVDChassisNoReq").removeAttr("hidden");
            return false;
        }


        else {
            var isNumber = /^\d+$/.test(chasis);
            var isString = (!/[^a-zA-Z]/.test(chasis))

            if (isString == true || isNumber == true) {

                $("#errVDChassisNoReq").removeAttr("hidden");
                return false;
            }


        }
    }
}

function Clearfeilds() {
    $('#ddlVDRTO').val('');
    $('#txtVDEngine').val('');
    $('#txtVDDateOfRegistration').val('');
    $('#ddlVDVehicleManufacture').val('');
    $('#txtVDTypeOfModel').val('');
    $('#ddlVDManufacturerYear').val('');
    $('#txtVDSeating').val('');
    $('#txtVDCubicCapacity').val(0);
    $('#txtVDWeight').val(0);
    $('#ddlVDFuelType').val('');
    $('#ddlVehType').val('');
    $('#ddlVehSubType').val('');
    $('#ddlVehCat').val('');
    $('#ddlManufacturMonth').val('');
    $('#ddlVehicleCatType').val('');
    $('#ddlVehClassType').val('');
}
$("#txtPHVToDate").on("keydown keypress keyup", false);
$("#txtPHfromDt").on("keydown keypress keyup", false);

//$('#txtPHfromDt').prop("readonly", true);
//$('#txtPHVToDate').prop("readonly", true);
//$('#txtPHVToDate').css("pointer-events", "none");
//$('#txtPHfromDt').css("pointer-events", "none");
//function AutopopulateFromToDate(id) {
//    debugger;
//    //$('#txtPHfromDt').val('');
//    //$('#txtPHVToDate').val('');
//    debugger;
//    var purchaseDt = $('#' + id).val();

//    var splitdate = purchaseDt.split('-');
//    var date = splitdate[0];
//    var month = splitdate[1] - 1;
//    var year = splitdate[2];
//    var endyear = parseInt(year) + 5;


//    jQuery('#txtPHfromDt').datetimepicker({
//        defaultDate: new Date(year, month, date),
//        timepicker: false,
//        format: 'd-m-Y',
//        minDate: new Date(year, month, date),
//        maxDate: new Date(),
//        yearStart: year,
//        yearEnd: endyear,
//        scrollMonth: false,
//        scrollInput: false,
//        closeOnDateSelect: true,
//        useCurrent: false,
//    });

//    //$('#txtPHfromDt').prop("readonly", false);
//    //$('#txtPHVToDate').prop("readonly", false);
//    //$('#txtPHVToDate').css("pointer-events", "auto");
//    //$('#txtPHfromDt').css("pointer-events", "auto");
//}

function SetEndDate() {


    //if ($('#txtPHDateOfPurchase').val() != "") {
    $('#txtPHVToDate').prop('readonly', false);
    var fromDT = $('#txtPHfromDt').val();

    var splitdate = fromDT.split('-');
    var date = splitdate[0];
    var month = splitdate[1] - 1;
    var year = splitdate[2];
    var endyear = parseInt(year) + 5;

    jQuery('#txtPHVToDate').datetimepicker({
        defaultDate: new Date(parseInt(year), month, date),
        timepicker: false,
        format: 'd-m-Y',
        minDate: new Date(parseInt(year), month, date),
        maxDate: new Date(endyear, month, date),
        yearStart: parseInt(year),
        yearEnd: endyear,
        closeOnDateSelect: true,
        scrollMonth: false,
        scrollInput: false
    });
    $('#errorPVinsuranceFromDtReq').attr("hidden", true);
    $('#txtPHVToDate').css("pointer-events", "auto");
}
//else {

//    $('#errorPVinsuranceFromDtReq').removeAttr("hidden");
//    $('#errorPVinsuranceFromDtReq').text("Please enter Date of purchase.");
//    $('#txtPHVToDate').prop('readonly', true);
//    $('#txtPHVToDate').css("pointer-events", "none");
//    $('#txtPHfromDt').val('');
//    return false;
//}


function CheckValidVehicleNo() {

    var catergory = $('#category').val();
    //alertify.success($('#category').val());
    $('.err').attr('hidden', true);

    var vehicleNo = $('#txtVehicleNo').val();
    if (vehicleNo != "") {
        $("#errVehcileNo").attr("hidden");
        debugger
        if ($('.err:visible').length === 0) {
            debugger
            $.ajax({
                url: '/MotorInsurance/MICheckValidVehicleNo',
                data: JSON.stringify({ VehicleNo: vehicleNo, Category: catergory }),
                type: 'POST',
                async: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                processData: false,
                success: function (response) {
                    debugger
                    if (response.Empid != '' && response.Appid != '') {
                        $('#hdnmicnclappid').val(response.Appid);
                        $('#hdnmicnclempid').val(response.Empid);
                        //alertify.success($('#hdnmicnclappid').val());
                        //alertify.success($('#hdnmicnclempid').val());
                        alertify.success("Vehicle no. exits");
                        $.ajax({
                            url: '/MotorInsurance/VehicleDetailsForCancellation',
                            data: JSON.stringify({ VehicleNo: vehicleNo }),
                            type: 'POST',
                            async: false,
                            contentType: 'application/json; charset=utf-8',
                            success: function (data) {
                                debugger;
                                var rowData = '';
                                //var datalength = Object.keys(data).length;
                                //iterating through objects
                                if (data != null) {
                                    if (data.listVehicleDetails.length > 0) {
                                        for (var i = 0; i < data.listVehicleDetails.length; i++) {
                                            //var name = data.listVehicleDetails[i].vehicle_application_id;
                                            //if (html.indexOf(name) == -1)
                                            //    html.push(name);
                                            var application_id = data.listVehicleDetails[i].vehicle_application_id;
                                            var vehicle_details_id = data.listVehicleDetails[i].vehicle_details_id;
                                            rowData += "<tr>";
                                            rowData += "<td>" + data.listVehicleDetails[i].vehicle_application_id + "</td>";
                                            rowData += "<td>" + data.listVehicleDetails[i].vehicle_details_id + "</td>";
                                            rowData += "<td>" + data.listVehicleDetails[i].vehicle_chassis_no + "</td>";
                                            rowData += "<td>" + data.listVehicleDetails[i].vehicle_registration_no + "</td>";
                                            rowData += "<td>" + data.listVehicleDetails[i].policy_from_date + "</td>";
                                            rowData += "<td>" + data.listVehicleDetails[i].policy_to_date + "</td>";
                                            rowData += "<td>" + data.listVehicleDetails[i].status + "</td>";
                                            rowData += "<td><a href='javascript: void (0);' class='btn-sm btn-danger' onclick='cancelVehiclePolicy(" + application_id + ", " + vehicle_details_id + ");'>Cancel Policy</a></td>"
                                            rowData += "</tr>";
                                        }
                                        $("#tblVehicleDetails tbody").append(rowData);
                                        $("#tblVehicleDetails").show();
                                    }
                                    else {
                                        alertify.alert("Cancellation request cannot be processed,as policy is not created.").setHeader('Warning..!!');
                                        $("#txtVehicleNo").val('');
                                    }

                                }
                            }
                        });
                    }
                    else if (response.Sucess == "1") {
                        alertify.alert("Vehicle no. is already processed for cancellation").setHeader('Warning..!!');
                        $('#txtVehicleNo').val('');
                        $('#ddlCancelReasonList').val('');
                        $('#txtAuctionDetailsDoc').val('');
                    }
                    else {
                        alertify.alert("Please enter valid vehicle no.").setHeader('Error..!!');
                        $('#txtVehicleNo').val('');
                        $('#ddlCancelReasonList').val('');
                        $('#txtAuctionDetailsDoc').val('');
                    }

                }, error: function (response) {
                    alertify.error("Could not fetch the details.").setHeader('Error..!!');
                    $('#txtVehicleNo').val('');
                    $('#ddlCancelReasonList').val('');
                    $('#txtAuctionDetailsDoc').val('');
                }
            });

        }

    }
    else {
        $("#errVehcileNo").removeAttr("hidden");
        return false;

    }
}
function cancelVehiclePolicy(application_id, vehicle_details_id) {
    debugger;
    $("#divDoctab").show();
}
function MICancelApplication() {

    ValidateCancelPolicy();
    if ($('.err:visible').length === 0) {
        var formData = new FormData($("#FormCancelDetails").get(0));
        var refid = $("#hdnmicnclappid").val();
        var empid = $("#hdnmicnclempid").val();
        formData.append("MI_cncl_App_Reference_ID", refid);
        formData.append("App_Proposer_ID", empid);
        debugger;
        alertify.confirm("Are you sure you want to cancel the policy?", function () {
            // alertify.alert('Warning', 'Are you sure you want to cancel the policy?', function () {
            $.ajax({
                url: '/MotorInsurance/MIAppCancelRequestAction',
                data: formData,
                async: false,
                type: 'POST',
                cache: false,
                contentType: false,
                processData: false,
                success: function (data) {
                    debugger
                    if (data == 1) {
                        alertify.alert('Success', 'Policy cancellation request forwarded to KGID office.', function () {
                            var catergory = $('#category').val();
                            window.location.href = "/kgid-mi-c-req/" + catergory;
                        });
                        $('#txtVehicleNo').val('');
                        $('#ddlCancelReasonList').val('');
                        $('#txtAuctionDetailsDoc').val('');
                    }
                    else if (data == 2) {
                        alertify.alert("Cannot process the request,Either Application is not submitted or Vehicle No. is invalid.").setHeader('Warning..!!');
                        $('#txtVehicleNo').val('');
                        $('#ddlCancelReasonList').val('');
                        $('#txtAuctionDetailsDoc').val('');
                    }

                    else {
                        alertify.alert("Error occured in sending cancellation request.").setHeader('Warning..!!');
                        $('#txtVehicleNo').val('');
                        $('#ddlCancelReasonList').val('');
                        $('#txtAuctionDetailsDoc').val('');
                    }

                }
            });
        }).setHeader("Warning!!!");
    }
}

function ShowUploadDocTab(id) {
    debugger
    $("#btnCancelMIPolicy").show();
    //$("#divDoctab").show();
    $("#AuctionUpload").hide();
    $("#NOCUpload").hide();
    $("#VIRTOUpload").hide();
    var ddlid = $('#' + id).val();
    if (ddlid == "1") {
        $("#AuctionUpload").show();

    }
    else if (ddlid == "2") {
        $("#VIRTOUpload").show();

    }
    else if (ddlid == "3") {
        $("#NOCUpload").show();

    }
}


function ValidateCancelPolicy() {
    debugger
    $('.err').attr('hidden', true);

    var ddlid = $('#ddlCancelReasonList').val();
    if (ddlid == "") {
        $('#errReasonList').removeAttr("hidden");
        return false
    }
    var vehicleNo = $('#txtVehicleNo').val();
    if (vehicleNo == "") {
        $('#errVehcileNo').removeAttr("hidden");
        return false
    }
    if (ddlid == '1') {
        if ($("#txtAuctionDetailsDoc").val().length == 0) {
            $('#errAuctionDetails').removeAttr("hidden");
            // alertify.alert("Please upload Auction Details form").setHeader("warning!!!");
            return false;
        }
    }

    if (ddlid == '2') {
        if ($("#txVIReprtDoc").val().length == 0) {
            $('#errVIRptDoc').removeAttr("hidden");
            //  alertify.alert("Please upload Vehicle Inspection Report by RTO").setHeader("warning!!!");
            return false;
        }
    }

    if (ddlid == '3') {
        if ($("#txtNOCDoc").val().length == 0) {
            $('#errNOCDoc').removeAttr("hidden");
            //  alertify.alert("Please upload NOC stating clearance of vehicle purchase loan (MCA) by DDO").setHeader("warning!!!");
            return false;
        }
    }

    else {
        return true;
    }
}

//function DownloadApplicationForm() {
//    debugger
//    var Applicationcontents = "";

//    Applicationcontents = $('#divMIPrintForm').html();

//    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();

//    let mywindow = window.open('', 'PRINT', 'height=650,width=900,top=100,left=150');

//    mywindow.document.write(`<html><head><title>Print Application</title>`);
//    mywindow.document.write('</head><body >');
//    mywindow.document.write(Applicationcontents);
//    mywindow.document.write('</body></html>');

//    mywindow.document.close(); // necessary for IE >= 10
//    mywindow.focus(); // necessary for IE >= 10*/

//    mywindow.print();
//    mywindow.close();

//    return true;

//}
function printform() {
    //GetNomineeDetails();
    var Applicationcontents = "";

    Applicationcontents = $('#divMIPrintForm').html();
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>KGID Details</h2></div><div class="form-group col-4"></div></div>' + $("#divKGID").html() ;
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html();
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + '<div class="html2pdf__page-break"></div>';
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintPD").html();
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html() + '<input type="checkbox" value="true" checked /><label>I agree to the terms and condition mentioned above.</label>' + "<hr />";

    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();

    html2pdf(Applicationcontents, {
        margin: 0.3,
        filename: "ApplicationForm" + "_" + date,
        image: { type: 'jpeg', quality: 1 },
        html2canvas: { scale: 0, letterRendering: true },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'Portrait' },
        pagebreak: { mode: 'legacy' }
    });

    //$("#divHeightChart").show();
    $("#divShowQRCode").hide();
    $(".action").show();
}
$('.ClAlpha').keyup(function () {
    var input_val = $(this).val();
    var inputRGEX = /^[a-zA-Z]*$/;
    var inputResult = inputRGEX.test(input_val);
    if (!(inputResult)) {
        this.value = this.value.replace(/[^a-z\s]/gi, '');
    }
});

function DocFileChange(id, errLbl) {
    debugger;

    var checkpdf = $("#" + id).val().split('.').includes('pdf')
    if (!checkpdf) {
        $("#" + errLbl).removeAttr("hidden");
        $("#" + errLbl).text("Please upload document in pdf format");
        $("#" + id).val("");
    }
    else {

        $("#" + errLbl).attr("hidden", true);
    }

    const FileSize = $("#" + id).get(0).files[0].size;
    const maxAllowedSize = 5 * 1024 * 1024;

    if (FileSize > maxAllowedSize) {

        $("#" + errLbl).removeAttr("hidden");
        $("#" + errLbl).text("File size exceeds 5 MB");
        $("#" + id).val("");
    }
    else {
        $("#" + errLbl).attr("hidden", true);
    }


}

function GetMIDeclaration() {
    $.ajax({
        url: '/MotorInsurance/DeclarationToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-5").html(data);
            ////EnglishToKannada();
            //if ($('.knlan').is(":visible")) {
            //    $("#lblEmployeeNameKN").text($("#txtEmployeeFullNameKN").val());
            //}
            //else {
            //    $("#lblEmployeeNameEN").text($("#txtEmployeeFullName").val());
            //}
        }
    });
}