
const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];
const words = new Array();
words[0] = '';
words[1] = 'One';
words[2] = 'Two';
words[3] = 'Three';
words[4] = 'Four';
words[5] = 'Five';
words[6] = 'Six';
words[7] = 'Seven';
words[8] = 'Eight';
words[9] = 'Nine';
words[10] = 'Ten';
words[11] = 'Eleven';
words[12] = 'Twelve';
words[13] = 'Thirteen';
words[14] = 'Fourteen';
words[15] = 'Fifteen';
words[16] = 'Sixteen';
words[17] = 'Seventeen';
words[18] = 'Eighteen';
words[19] = 'Nineteen';
words[20] = 'Twenty';
words[30] = 'Thirty';
words[40] = 'Forty';
words[50] = 'Fifty';
words[60] = 'Sixty';
words[70] = 'Seventy';
words[80] = 'Eighty';
words[90] = 'Ninety';
$(document).ready(function () {
    getAge();
    var amount = $("#hdnAmount").val();
    var amt = convertNumberToWords(amount);
    $("#spnAmount").text(amt);
    var dueyearsplit = $("#hdnSanctiondate").val().split('-')[2].substring(0, 4);
    var year = convertNumberToWords(dueyearsplit);
    $("#spnCurrYear").text(year);
    var duedatesplit = Math.round($("#hdnSanctiondate").val().split('-')[0]);
    var day = convertNumberToWords(duedatesplit);
    $("#spnCurrDay").text(day);
    getDueMonth();
});
function printNBBond() {
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2></h2></div><div class="form-group col-4"></div></div>' + $("#divPrintNBBond").html();
    //contents = contents + '<div class="row" style="page-break-before:always;"><div class="form-group col-5"></div><div class="form-group col-3"><h2></h2></div><div class="form-group col-4"></div></div>' + $("#divPrintNBBond2").html();
    //contents = contents + '<div class="row" style="page-break-before:always;"><div class="form-group col-5"></div><div class="form-group col-3"><h2></h2></div><div class="form-group col-4"></div></div>' + $("#divPrintNBBond3").html();
    //contents = contents + '<div class="row" style="page-break-before:always;"><div class="form-group col-5"></div><div class="form-group col-3"><h2></h2></div><div class="form-group col-4"></div></div>' + $("#divPrintNBBond4").html();
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPersonal").html() + "<hr />";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html();
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>NB Bond</title>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.
    //frameDoc.document.write('<link href="/Content/Custom/sb-admin-2.min.css" rel="stylesheet" />');
    frameDoc.document.write('<link href="/Scripts/DataTables/Bootstrap-4-4.1.1/css/bootstrap.min.css" rel="stylesheet" />');
    //Append the DIV contents.
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);

}
function getAge() {
    var DOB = $("#hdnDOB").val().split("-");
    var dFrom = DOB[1] + "-" + DOB[0] + "-" + DOB[2];
    var birthDate = new Date(dFrom);
    var today = new Date();
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age = age - 1;
    }
    $("#spnDOB").text(age);
    //Calc final month & year
    var finalyear = birthDate.getFullYear() + 55;
    var finalmonth = monthNames[birthDate.getMonth()];
    $("#spnfinalpayment").text(finalmonth + ' ' + finalyear);
}
function convertNumberToWords(amount) {
    amount = amount.toString();
    var atemp = amount.split(".");
    var number = atemp[0].split(",").join("");
    var n_length = number.length;
    var words_string = "";
    if (n_length <= 9) {
        var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
        var received_n_array = new Array();
        for (var i = 0; i < n_length; i++) {
            received_n_array[i] = number.substr(i, 1);
        }
        for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
            n_array[i] = received_n_array[j];
        }
        for (var i = 0, j = 1; i < 9; i++ , j++) {
            if (i == 0 || i == 2 || i == 4 || i == 7) {
                if (n_array[i] == 1) {
                    n_array[j] = 10 + parseInt(n_array[j]);
                    n_array[i] = 0;
                }
            }
        }
        value = "";
        for (var i = 0; i < 9; i++) {
            if (i == 0 || i == 2 || i == 4 || i == 7) {
                value = n_array[i] * 10;
            } else {
                value = n_array[i];
            }
            if (value != 0) {
                words_string += words[value] + " ";
            }
            if ((i == 1 && value != 0) || (i == 0 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Crores ";
            }
            if ((i == 3 && value != 0) || (i == 2 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Lakhs ";
            }
            if ((i == 5 && value != 0) || (i == 4 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Thousand ";
            }
            if (i == 6 && value != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                words_string += "Hundred and ";
            } else if (i == 6 && value != 0) {
                words_string += "Hundred ";
            }
        }
        words_string = words_string.split("  ").join(" ");
    }
    return words_string;
}
function getDueMonth() {
    var duedate = $("#hdnSanctiondate").val();
    if ($("#hdnSanctiondate").val().indexOf('-') > 0) {
        var duemonthsplit = Math.round($("#hdnSanctiondate").val().split('-')[1]);
        $("#spnduemonth").text(monthNames[duemonthsplit]);
        $("#spnCurrMonth").text(monthNames[duemonthsplit - 1]);
    }
}