var BserverResponse = false;
var KserverResponse = false;
var NserverResponse = false;
var FserverResponse = false;
var PserverResponse = false;
var DserverResponse = false;
var MserverResponse = false;
var CserverResponse = false;
var HPserverResponse = false;
var HOserverResponse = false;
var HHserverResponse = false;
var HDserverResponse = false;
var HDCserverResponse = false;
var medicalRequiredStatus = true;
var MrtStatus;
var isFamilyDetailsAdded = false;
var isNomineeDetailsAdded = false;

$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const myParam = urlParams.get('pay');
    if (myParam != null) {
        if (myParam.toString() == "true") {
debugger;
            $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
            $('#nav-Payment-tab').removeClass("disabled");
            $("#nav-Payment-tab").click();
        }
        else {
            $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "none");
        }
    }
    else {
        $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "none");
    }

    var uri = window.location.toString();
    if (uri.indexOf("?") > 0) {
        var clean_uri = uri.substring(0, uri.indexOf("?"));
        window.history.replaceState({}, document.title, clean_uri);
    }
    $(".number").hide();
    $("#imgQRCode").attr('src', $("#hdnQRCode").val());
    alertify.set('notifier', 'delay', 2);
    var eid = $("#divApplication").data("eid");
    $('#btnAllPrint').remove();
    $("#ApplicationTab").steps({
        headerTag: "h2",
        bodyTag: "section",
        transitionEffect: "slideLeft",
        onStepChanging: function (event, currentIndex, newIndex) {
            //var form = $(this);
            //var aaa = $('li').closest('ul');
            //if ($("#hdnSentBackAppliaction").val() == 1) {
            //    $(".steps > ul > li").removeClass("disabled").addClass("done");
            //}
            
            $('#btnAllPrint').remove();
            if (currentIndex > newIndex) {
                if (newIndex == 2) {
                    GetFamilyDetails();
                }
                return true;
            }
            else {
                if (currentIndex == 0) {
                    var status = ValidateBasicDetails();
                    if (status == true) {
                        if ($('.err:visible').length === 0) {
                            $("#lblEmployeeName").html($("#txtEmployeeFullName").val());
                            var RefNumber = $("#spnReferanceNo").text();
                            if (RefNumber != null) {
                                $("#spnPayscale").text($("#txtPayscleCode").val());
                                $('#ddlNomineeRelation').empty();
                                $('#ddlEditNomineeRelation').empty();
                            } else {
                                alertify.error("Please Save and continue");
                                return false;
                            }
                            SaveBasicDetails(eid);
                            GetKGIDDetails();
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                            return false;
                        }
                   
                }
                else if (currentIndex == 1) {
                    if (parseInt($("#txtAmount").val()) != 0) {
                        if (parseInt($("#txtAmount").val()) < 100) {
                            $("#txtAmount").val($("#txtMPremium").val());
                            $('#errTotalAmount').text('Entered amount should be greater than 100(Rupees).');
                            $('#errTotalAmount').removeAttr("hidden");
                            return false;
                        }
                    }
                    var KGIDPremium = $("#txtMPremium").val();
                    var EnteredAmount = $("#txtAmount").val();
                    $("#txtPayAmount").val(EnteredAmount);
                    $("#hdnKgidPremiumAmount").val(EnteredAmount);
                    var grossSalary = $("#hdnGrossPay").val();
                    var GrossHalfSal = parseFloat(grossSalary) / 2;//50% of Gross Salary
                    var AlreadyPolicy = $(".csTotal").text();
                    var NewPolicyAmount = 0;
                    var ExistingPolicyAmount = 0;
                    $("#hdnInsuredEmployee").val(0);
                    $("#tblKgidPremium tbody tr td:nth-child(2)").each(function () {
                        var value = $(this);
                        if (value[0].innerHTML == "") {
                            if (EnteredAmount == 0) {
                                NewPolicyAmount = NewPolicyAmount + parseFloat(value[0].nextElementSibling.innerHTML);
                            } else {
                                NewPolicyAmount = NewPolicyAmount + parseFloat(EnteredAmount);
                            }
                        } else {
                            $("#hdnInsuredEmployee").val(1);
                            if (ExistingPolicyAmount == 0) {
                                ExistingPolicyAmount = parseFloat(value[0].nextElementSibling.innerHTML);
                            } else {
                                ExistingPolicyAmount = ExistingPolicyAmount + parseFloat(value[0].nextElementSibling.innerHTML);
                            }
                        }
                    });
                    var TotalAmount = 0;
                    TotalAmount = parseFloat(ExistingPolicyAmount) + parseFloat(EnteredAmount);
                    if (EnteredAmount == 0) {
                        TotalAmount = parseFloat(ExistingPolicyAmount) + parseFloat(NewPolicyAmount);
                    }
                    if (GrossHalfSal > TotalAmount) {
                        $("#hdnNewPolicyAmount").val(NewPolicyAmount);
                    } else {
                        $('#errTotalAmount').text('Total amount should be lesser than 50% of Gross Salary. Minimum amount is ' + KGIDPremium);
                        $('#errTotalAmount').removeAttr("hidden");
                        $("#txtAmount").val(KGIDPremium);
                        return false;
                    }
                    if ($("#txtAmount").val() != "0" && parseFloat(EnteredAmount) < parseFloat(KGIDPremium)) {
                        $('#errTotalAmount').text('Premium Amount should be greater than or equal to minimum monthly premium.');
                        $("#txtAmount").val($("#txtMPremium").val());
                        $('#errTotalAmount').removeAttr("hidden");                        
                        return false;
                    }
                    if (parseInt(TotalAmount) == 0) {
                        //$("#txtAmount").val($("#txtMPremium").val());
                        $('#errTotalAmount').text('Entered amount should be greater than 100(Rupees).');
                        $('#errTotalAmount').removeAttr("hidden");
                        return false;
                    }
                    var SubPolicyCondition = true;
                    var UnProgPolicy = false;
                    if ($("#tblKgidPremium tbody tr").length > 0) {
                        $("#tblKgidPremium tr td:nth-child(2)").each(function () {
                            var value = $(this);
                            if (value[0].innerHTML == "") {
                                UnProgPolicy = true;
                                SubPolicyCondition = true;
                                $("#hdnNewPolicyAmount").val(NewPolicyAmount);
                                //$("#txtPayAmount").val(NewPolicyAmount);
                                //$("#txtPayAmount").val(0);
                            }
                            else {
                                if (($("#txtAmount").val() == 0 || $("#txtAmount").val() == "") && UnProgPolicy == false) {
                                    $('#errTotalAmount').text('Premium Amount should not be zero for subsequent policy.');
                                    $('#errTotalAmount').removeAttr("hidden");
                                    SubPolicyCondition = false;
                                }
                            }
                        });
                        if (SubPolicyCondition == true) {
                            SaveKGIDDetails(eid);
                            GetFamilyDetails();
                        }
                        //if (NewPolicyAmount == 0) {
                        //    $("#hdnNewPolicyAmount").val(EnteredAmount);
                        //}
                        //else {
                        //    $("#hdnNewPolicyAmount").val(NewPolicyAmount);
                        //}
                        
                        return SubPolicyCondition;
                    }
                    else {
                        return false;
                    }
                }
                else if (currentIndex == 2) {
                    var isOrphan = $(".rd-btn-orphan:checked").val();
                    var isMarried = $(".rd-btn-mstatus:checked").val();
                    if (isMarried !== "True" && isOrphan === "True")
                        GetNomineeDetails();
                    else {

                        if ($("#tblFamilyDetails TBODY TR").find("td").hasClass("dataTables_empty")) {
                            alertify.error("Add Family Details.");
                            return false;
                        } else if ($("#tblFamilyDetails TBODY TR").length === 0) {
                            alertify.error('Add Family Details.');
                            return false;
                        }
                        else {
                            var isMarriedNomineeAdded = $("#tblFamilyDetails TBODY TR TD.tdRelation:contains('Spouse')").length;
                            var divStatus = $("#hdnDRStatus").val();
                            if (isMarriedNomineeAdded === 0 && isMarried == "True" && divStatus != "1") {
                                alertify.alert("Please Add Spouse in Family Details.").setHeader("Warning");
                                return false;
                            }
                            var isParentsAdded = $("#tblFamilyDetails TBODY TR TD.tdRelation:contains('Father'), TD.tdRelation:contains('Mother')").length;
                            if ((isMarried == "False" || isMarried == "True") && (isOrphan == "False" || (isOrphan == undefined && $('#father_name').val() != "")) && isParentsAdded < 2) {
                                alertify.alert("Please Add Father & Mother in Family Details.").setHeader("Warning");
                                return false;
                            }
                        }
                    }
                    SaveFamilyDetails(eid);
                    //GetNomineeDetails();
                    return true;
                }
                else if (currentIndex == 3) {
                    var Result = ValidateNominee();
                    //if ($("#tblNomineeDetails TBODY TR").find("td").hasClass("dataTables_empty")) {
                    //    alertify.error("Add Nominee Details.");
                    //    return false;
                    //} else if ($("#tblNomineeDetails TBODY TR").length == 0) {
                    //    alertify.error("Add Nominee Details.");
                    //    return false;
                    //} else if (!ValidateSumShare()) {
                    //    alertify.error("Percentage share for nominees should be 100 %.");
                    //    return false;
                    //}
                    //else {
                    if (Result == true) {
                        GetPersonalDetails();
                    }
                    
                    //    return true;
                    //}
                    return Result;
                }
                else if (currentIndex == 4) {
                    validatePersonalFields();
                    if ($('.err:visible').length === 0) {
                        var result = SavePersonalDetails(eid);
                        if (result == true) {
                            GetPersonalDetails();

                            GetDeclaration();
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                        return false;
                    }
                    //GetMedicalLeaveDetails();
                    return true;
                }
                else if (currentIndex == 5) {
                    $('#ApplicationTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
                    return true;
                }
                return true;
            }
        },
        onStepChanged: function (event, currentIndex, priorIndex) {
            // Used to skip the "Warning" step if the user is old enough.
            var isOrphan = $(".rd-btn-orphan:checked").val();
            var isMarried = $(".rd-btn-mstatus:checked").val();
            if (currentIndex === 2 && priorIndex === 1 && (isMarried !== "True" && isOrphan === "True")) {
                $("#ApplicationTab").steps("next");
            }
            if (currentIndex === 2 && priorIndex === 3 && (isMarried !== "True" && isOrphan === "True")) {
                $("#ApplicationTab").steps("previous");
            }
            EnglishToKannada();
        },
        onFinished: function (event, currentIndex) {

            if ($("#chkDeclaration").prop("checked") == false) {
                alertify.alert("Please agree to the terms and condition.").setHeader("Warning !!!");
                return false;
            }
            GetPaymentDetails();
            var NResult = ValidateNominee();
            if (NResult == 'flase') {
                alertify.alert("Please add nominee.").setHeader("Warning !!!");
                return false;
            }
            if (currentIndex == 5) {
                alertify.confirm("Are you sure you want to finish the application?", function () {
                    if (BserverResponse == false) {
                        SaveBasicDetails(eid);
                    }
                    if (KserverResponse == false) {
                        SaveKGIDDetails(eid);
                    }
                    if (NserverResponse == false) {
                        
                    }
                    if (FserverResponse == false) {
                        SaveFamilyDetails(eid);
                    }
                    if (PserverResponse == false) {
                        var result = SavePersonalDetails(eid);
                        if (result == true) {
                            GetPersonalDetails();
                        }
                        else {
                            return false;
                        }
                    }
                    //if (MserverResponse == false) {
                    //    SaveMedicalLeaveDetails(eid);
                    //}

                    SaveUploadStatus(eid);
                    //debugger;
                    //GetNomineeDetails();
                    if ($("#hdnSentBackAppliaction").val() == 0) {
                        debugger;
                        $("#txtPDAmount").val($("#hdnNewPolicyAmount").val());
                        $('#nav-Payment-tab').removeClass("disabled");
                        $("#nav-Payment-tab").click();
                        //$('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "none");
                    }
					
                    else if ($("#hdnSentBackAppliaction").val() == 1) {
                        if ($("#hdnRmrks").val() == 2) {
                            $("#nav-Medical-tab").addClass("disabled");
                            //printNBApplication();
                            printform();
                            setTimeout(function () {
                                window.location.href = '/kgid-upload-emp-data/';
                            }, 2000);
                        }
                        else if ($("#hdnRmrks").val() == 4) {

                            SwitchMedicalRequired();
                            if (medicalRequiredStatus) {
                                $("#nav-Medical-tab").removeClass("disabled");
                                $('#nav-Medical-tab').click();
                                $('#nav-Medical').addClass("show active");
                                $('#nav-Application').removeClass("show active");
                                $("#divShowQRCode").show();
                                $("#spnBReferanceNo").text($("#spnReferanceNo").text());
                                $("#divHeightChart").hide();
                                
                                printform();
                                //var Applicationcontents = "";
                                //var burl = "ViewDataToVerify/PersonalDetailsToView"
                                //$.get(burl, function (data) {
                                //    $('#divPrintPD').html(data);

                                //    $(".action").hide();
                                //    $('#divPrintPD').find('.knlan').hide();
                                //    $('#divPrintPD').find('.clsdoc').hide();

                                //    Applicationcontents = $('#divPrintForm').html();
                                //    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>KGID Details</h2></div><div class="form-group col-4"></div></div>' + $("#divKGID").html() + '<div class="html2pdf__page-break"></div>';
                                //    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html();
                                //    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + '<div class="html2pdf__page-break"></div>';
                                //    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintPD").html();
                                //    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html() + '<input type="checkbox" value="true" checked /><label>I agree to the terms and condition mentioned above.</label>' + "<hr />";

                                //    var date = new Date().getDate() + '_' + new Date().getMonth() + '_' + new Date().getFullYear();

                                //    html2pdf(Applicationcontents, {
                                //        margin: 1,
                                //        filename: "ApplicationForm" + "_" + date,
                                //        image: { type: 'jpeg', quality: 0.98 },
                                //        html2canvas: { scale: 1, letterRendering: true },
                                //        jsPDF: { unit: 'in', format: 'a4', orientation: 'landscape' },
                                //        pagebreak: { mode: 'legacy' }
                                //    });
                                //});
                                //$("#divHeightChart").show();
                                //$("#divShowQRCode").hide();
                                //$(".action").show();

                                //Medical Form
                                if (medicalRequiredStatus) {
                                    //var apprefno = $('#spnReferanceNo').text();
                                    //var proposername = $('#txtEmployeeFullName').val();
                                    //var address = $('#txtEAddress').val();
                                    //var pincode = $('#txtEPin').val();
                                    //var phone = $('#txtMobileNumber').val();
                                    //var joiningdate = $('#txtDateOfJoining').val();
                                    //var designation = $('#d_designation_desc').val();
                                    //var workingplace = $('#ewd_place_of_posting').val();

                                    //$.ajax({
                                    //    url: '/Employee/PrintMedicalDetails',
                                    //    data: JSON.stringify({ "AppRefNo": apprefno, "ProposerName": proposername, "Address": address, "Pincode": pincode, "Phone": phone, "JoiningDate": joiningdate, "Designation": designation, "WorkingPlace": workingplace }),
                                    //    type: 'POST',
                                    //    async: false,
                                    //    contentType: 'application/json; charset=utf-8',
                                    //    success: function (result) {
                                    //        var path = result.Result
                                    //        const linkSource = `data:application/pdf;base64,${path}`;
                                    //        const downloadLink = document.createElement("a");
                                    //        const fileName = "MER_" + apprefno + ".pdf";
                                    //        downloadLink.href = linkSource;
                                    //        downloadLink.download = fileName;
                                    //        downloadLink.click();
                                    //    }
                                    //});

                                    if (medicalRequiredStatus) {
                                        $("#nav-Medical-tab").removeClass("disabled");
                                    }
                                    else {
                                        if (!$("#nav-Medical-tab").hasClass("disabled"))
                                            $("#nav-Medical-tab").addClass("disabled");
                                    }
                                    $('#nav-Medical-tab').click();
                                }
                                else {
                                    setTimeout(function () {
                                        window.location.href = '/kgid-upload-emp-data/';
                                    }, 2000);
                                }
                            }
                            else {
                                //printNBApplication();
                                if (!$("#nav-Medical-tab").hasClass("disabled")) {
                                    $("#nav-Medical-tab").addClass("disabled");
                                }
                                setTimeout(function () {
                                    window.location.href = '/verifydata/uploademployeedata';
                                }, 2000);
                            }
                        }
                        else if ($("#hdnRmrks").val() == 3) {
                            //$("#nav-Application-tab").addClass("disabled");
                            //$("#nav-Medical-tab").removeClass("disabled");
                            //$('#nav-Medical').addClass("show active");
                            $('#divUploadApplication').hide();
                            printMedicalReport();
                        }

                    }
                    else {
                        // 37000 => 
                        $("#txtPDAmount").val($("#hdnNewPolicyAmount").val());
                        SwitchMedicalRequired();
                        if (medicalRequiredStatus) {
                            $("#nav-Medical-tab").removeClass("disabled");
                            $('#nav-Medical-tab').click();
                        }
                        else {
                            //printNBApplication();
                            if (!$("#nav-Medical-tab").hasClass("disabled")) {
                                $("#nav-Medical-tab").addClass("disabled");
                            }
                            setTimeout(function () {
                                window.location.href = '/verifydata/uploademployeedata';
                            }, 2000);
                        }
                    }
                }).setHeader("Submit Application?");
            }
        }
    });
    $("#PaymentTab").steps({
        headerTag: "h2",
        bodyTag: "section",
        transitionEffect: "slideLeft",
        onStepChanging: function (event, currentIndex, newIndex) {
            EnglishToKannada();
            $('#btnAllPrint').remove();
            //$('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "none");
            if (currentIndex > newIndex) {
                return true;
            }
            else {
                if (currentIndex == 0) {
                    return true;
                }
            }
            return true;
        },
        //onStepChanged: function (event, currentIndex, priorIndex) {
        //    EnglishToKannada();
        //},
        onFinished: function (event, currentIndex) {

            GetDeclaration();
            SwitchMedicalRequired();
            GetMBasicDetails();
            GetNomineeDetails();
            alertify.confirm("Are you sure you want to finish the payment?", function () {
                $("#divShowQRCode").show();
                $("#spnBReferanceNo").text($("#spnReferanceNo").text());
                $("#divHeightChart").hide();
                printform();
                //var Applicationcontents = "";
                //var burl = "ViewDataToVerify/PersonalDetailsToView"
                //$.get(burl, function (data) {
                //    $('#divPrintPD').html(data);

                //    $(".action").hide();
                //    $('#divPrintPD').find('.knlan').hide();
                //    $('#divPrintPD').find('.clsdoc').hide();

                //    Applicationcontents = $('#divPrintForm').html();
                //    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>KGID Details</h2></div><div class="form-group col-4"></div></div>' + $("#divKGID").html() + '<div class="html2pdf__page-break"></div>';
                //    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html();
                //    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + '<div class="html2pdf__page-break"></div>';
                //    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintPD").html();
                //    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html() + '<input type="checkbox" value="true" checked /><label>I agree to the terms and condition mentioned above.</label>' + "<hr />";

                //    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();

                //    html2pdf(Applicationcontents, {
                //        margin: 1,
                //        filename: "ApplicationForm" + "_" + date,
                //        image: { type: 'jpeg', quality: 0.98 },
                //        html2canvas: { scale: 1, letterRendering: true },
                //        jsPDF: { unit: 'in', format: 'a4', orientation: 'landscape' },
                //        pagebreak: { mode: 'legacy' }
                //    });
                //});
                $("#divHeightChart").show();
                $("#divShowQRCode").hide();
                $(".action").show();
                //Payment Form
                
                $("#divShowQRCode").show();
                $("#spnBReferanceNo").text($("#spnReferanceNo").text());
    
                var empid = $('#hdnEmployeeID').val();
                var appid = $('#hdnAppID').val();
              //  console.log($('#txtpaymentrefno1').val());
                $.ajax({
                    url: '/Employee/PrintPaymentDetails',
                    data: { EmpId: empid, AppId: appid, challanNo: $('#txtChallanRefNo').val() },
                    type: 'POST',
                    success: function (result) {
                        if (result.Result != '') {
                            var path = result.Result;
                            const linkSource = `data:application/pdf;base64,${path}`;
                            const downloadLink = document.createElement("a");
                            var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();
                            const fileName = "PaymentDetailsForm" + "_" + date + ".pdf";
                            downloadLink.href = linkSource;
                            downloadLink.download = fileName;
                            downloadLink.click();
                        }
                        else {
                            alertify.alert("Employee Detaila are not matching")
                        }
               
                       
                    }
                });
                //var paycontents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Payment Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintPayment").html() + "<hr />";
                //var date = new Date().getDate() + '_' + new Date().getMonth() + '_' + new Date().getFullYear();
                //html2pdf(paycontents, {
                //    margin: 1,
                //    filename: "PaymentDetailsForm" + "_" + date,
                //    image: { type: 'jpeg', quality: 0.98 },
                //    html2canvas: { scale: 2, letterRendering: true },
                //    jsPDF: { unit: 'in', format: 'a3', orientation: 'landscape' },
                //    pagebreak: { mode: 'legacy' }

                //});
                $("#divShowQRCode").hide();
                //Medical Form
                if (medicalRequiredStatus) {
                    PrintMedicalFormPDF();
                    if (medicalRequiredStatus) {
                        $("#nav-Medical-tab").removeClass("disabled");
                    }
                    else {
                        if (!$("#nav-Medical-tab").hasClass("disabled"))
                            $("#nav-Medical-tab").addClass("disabled");
                    }
                    $('#nav-Medical-tab').click();
                }
                else {
                    setTimeout(function () {
                        window.location.href = '/kgid-upload-emp-data/';
                    }, 2000);
                }
            }).setHeader("Confirmation");
        }
    });

    if (medicalRequiredStatus) {
        $("#MedicalTab").steps({
            headerTag: "h2",
            bodyTag: "section",
            transitionEffect: "slideLeft",
            onStepChanging: function (event, currentIndex, newIndex) {
                $('#btnMedicalPrint').remove();
                if (currentIndex > newIndex) {
                    //if (newIndex == 0) {
                    //   $("#MedicalTab ul[aria-label=Pagination] li:nth-child(1)").after('<li><a id="btnMedicalPrint" role="menuitem" onclick="printMedicalReport()" style="cursor: pointer;">Preview</a></li>');
                    //}
                    return true;
                }
                else {
                    if (currentIndex == 0) {
                        $("#txtAddress").val($("#txtEAddress").val());
                        $("#txtPin").val($("#txtEPin").val());
                        return true;
                    }
                    else if (currentIndex == 1) {
                        if (medicalRequiredStatus) {
                            SaveHPhysicalDetails(eid);
                        }
                        return HPserverResponse;//HPserverResponse;
                    }
                    else if (currentIndex == 2) {
                        if (medicalRequiredStatus) {
                            SaveHOtherDetails(eid);
                        }
                        return HOserverResponse;//HOserverResponse;
                    }
                    else if (currentIndex == 3) {
                        if (medicalRequiredStatus) {
                            SaveHHealthDetails();
                        }
                        if (HHserverResponse)
                            GetDoctorDetails();
                        return HHserverResponse;//HHserverResponse;
                    }
                    else if (currentIndex == 4) {
                        if (medicalRequiredStatus) {
                            if ($('.err:visible').length == '0') {
                                SaveHDoctorDetails();
                            }
                            else {
                                return false;
                            }
                            
                        }
                        return HDserverResponse;//HDserverResponse;
                    }
                    else if (currentIndex == 5) {
                        if ($("#chkHDeclaration").prop("checked") == false) {
                            return false;
                        }
                        $('#MedicalTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
                        return true;//HDCserverResponse;
                    }
                }
                return true;
            },
            onStepChanged: function (event, currentIndex, priorIndex) {
                EnglishToKannada();
            },
            onFinished: function (event, currentIndex) {
                if ($("#chkHDeclaration").prop("checked") == false) {
                    alertify.alert("Please agree to the terms and condition.").setHeader("Warning !!!");
                    return false;
                }
                if (currentIndex == 5) {
                    $("#HRefNo").show();
                    alertify.confirm("Are you sure you want to submit the medical report?", function () {
                        $("#HRefNo").show();
                        SaveHDeclarationDetails(eid);
                        if (HPserverResponse != true) {
                            SaveHPhysicalDetails(eid);
                        }
                        else if (HOserverResponse != true) {
                            SaveHOtherDetails();
                        }
                        else if (HHserverResponse != true) {
                            SaveHHealthDetails();
                        }
                        else if (HDserverResponse != true) {
                            if ($('.err:visible').length == '0') {
                                SaveHDoctorDetails();
                            }
                            else {
                                return false;
                            }
                        }
                        $("#HRefNo").hide();
                        setTimeout(function () {
                            window.location.href = '/kgid-upload-emp-data/';
                        }, 2000);
                    }).setHeader("Submit Medical Form?");
                }
            }
        });
    }
    //if ($("#hdnSentBackAppliaction").val() == 1) {
    //    $(".steps > ul > li").removeClass("disabled").addClass("done");
    //}
});
function PrintMedicalFormPDF() {
    var apprefno = $('#spnReferanceNo').text();
    var proposername = $('#txtEmployeeFullName').val();
    var address = $('#txtEAddress').val();
    var pincode = $('#txtEPin').val();
    var phone = $('#txtMobileNumber').val();
    var joiningdate = $('#txtDateOfJoining').val();
    var designation = $('#d_designation_desc').val();
    var workingplace = $('#ewd_place_of_posting').val();

    $.ajax({
        url: '/Employee/PrintMedicalDetails',
        data: JSON.stringify({ "AppRefNo": apprefno, "ProposerName": proposername, "Address": address, "Pincode": pincode, "Phone": phone, "JoiningDate": joiningdate, "Designation": designation, "WorkingPlace": workingplace }),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            var path = result.Result;
            const linkSource = `data:application/pdf;base64,${path}`;
            const downloadLink = document.createElement("a");
            const fileName = "MER_" + apprefno + ".pdf";
            downloadLink.href = linkSource;
            downloadLink.download = fileName;
            downloadLink.click();
        }
    });
}
function rebindValidation() {
    $('form').each(function (i, f) {
        $form = $(f);
        $form.removeData('validator');
        $form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse($form);
    });
}
$(document).ready(function () {

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    if (mm == 12) {
        mm = '01';
        yyyy = yyyy + 1;
    } else {
        mm = parseInt(mm) + 1;
        mm = String(mm).padStart(2, '0');
    }
    var timerdate = dd + '/' + mm + '/' + yyyy;
    // For demo preview end
    $("#ApplicationTab ul[aria-label=Pagination] li:nth-child(1)").after('<li><a id="btnASave" role="menuitem" onclick="ApplicationDetailsSave()">Save</a></li>');
    //$("#PaymentTab ul[aria-label=Pagination] li:nth-child(1)").after('<li><a id="btnPSave" role="menuitem" onclick="PaymentDetailsSave()">Save</a></li>');
    $("#MedicalTab ul[aria-label=Pagination] li:nth-child(1)").after('<li><a id="btnMSave" role="menuitem" onclick="MedicalDetailsSave()">Save</a></li>');

    $("#txtdateofpayment").val(timerdate);
    $("#txtdateofpayment1").val(timerdate);
    if ($("#txtServiceType").val().toLowerCase() === "temporary") {
        $("a[href*='next']").parent("li").addClass("disabled");
        $("a[href*='next']").parent("li").attr("aria-disabled", true);
        $("a[href*='next']").removeAttr("href");
        $("a[href*='previous']").removeAttr("href");
    }

    var eid = $("#divApplication").data("eid");

    $("#spnName").text($("#txtProposerName").val());

    ShowNomineeType();
});

//Personal Tab save functions
function SaveBasicDetails(eid) {
    $("#hdnmarriedstatus").val($("#ddlDRType").val());
    $("#lblEmployeeName").html($("#txtEmployeeFullName").val())

    var formData = new FormData($("#frmAppBasicDetails").get(0));

    $.ajax({
        url: '/Employee/InsertBasicDetails',
        data: formData,
        async: false,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            GetBasicDetails();
            $("#hdnBReferanceNo").val(result);
            BserverResponse = true;
        }, error: function (result) {
            alertify.error("Could not save basic details");
        }
    });

    return BserverResponse;
}
function SaveKGIDDetails(eid) {

    SwitchMedicalRequired();

    var viewModel = {
        'employee_id': eid, 'application_id': $("#hdnApplicationId").val(), 'premium_Amount': $("#txtMPremium").val(),
        'premium_Amount_to_Pay': $("#txtAmount").val(), 'p_kgid_policy_number': parseInt($("#spnReferanceNo").text())
    };

    if ($("#hdnSentBackAppliaction").val() != 0) {

        if ($("#hdnRmrks").val() == 4) {
            $("#hdnMedicalRequired").val(true);
        }
        if ($("#hdnRmrks").val() == 3) {
            $("#hdnMedicalRequired").val(true);
        }
        if ($("#hdnRmrks").val() == 2) {
            $("#hdnMedicalRequired").val(false);
        }
    }
    var isMedicalRequired = $("#hdnMedicalRequired").val();
    $.ajax({
        url: '/Employee/InsertKgidDetails',
        data: JSON.stringify({ "objKGIDDetails": viewModel, "isMedReq": isMedicalRequired }),
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        processData: false,
        cache: false,
        success: function (result) {
            GetKGIDDetails();
            KserverResponse = true;
        }, error: function (result) {
            alertify.error("Could not save KGID details");
        }
    });

    return KserverResponse;
}
function SaveNomineeDetails(eid) {
    var NserverResponse = true;
    var theTotal = 0;
    $("#tblNomineeDetails tr td:nth-child(5)").each(function () {
        var val = this.innerText;
        theTotal += parseInt(val);
    });
    if (theTotal < 100) {
        alertify.error("Percentage share for nominees should be 100 %.").setHeader("warning!!!");
        NserverResponse = false;
    }
    else {
        GetNomineeDetails();
        alertify.success("Nominee details saved successfully");
        NserverResponse = true;
    }
    return NserverResponse;
}
function SaveFamilyDetails(eid) {
    var isMarried = $(".rd-btn-mstatus:checked").val();
    var isOrphan = $(".rd-btn-orphan:checked").val();
    if (isMarried === "True" || ($(".rd-btn-orphan:checked").length > 0 && isOrphan !== "True")) {
        var Familydetails = new Array();
        $("#tblFamilyDetails TBODY TR").each(function () {
            var row = $(this);
            var familyMember = {};
            familyMember.Id = (row.find("TD").eq(10).html() == "true") ? row.attr("data-row-number") : 0;
            familyMember.ApplicationId = $("#hdnAppID").val();
            familyMember.EmpId = $("#hdnAddEmpCode").val(),
                familyMember.NameOfMember = row.find("TD").eq(0).html();
            familyMember.Relation = row.find("TD").eq(1).html();
            familyMember.DateOfBirth = row.find("TD").eq(2).html();
            familyMember.Age = row.find("TD").eq(3).html();
            var aliveDead = true;
            if (row.find("TD").eq(4).html() === "Dead") {
                aliveDead = false;
            }
            familyMember.AliveDead = aliveDead;

            var sibMarried = false;
            if (row.find("TD").eq(5).html() === "Unmarried") {
                sibMarried = false;
            }
            else if (row.find("TD").eq(5).html() === "Married") {
                sibMarried = true;
            }

            familyMember.IsSiblingMarried = sibMarried;
            familyMember.HealthCondition = row.find("TD").eq(6).html();
            familyMember.DateOfDeath = row.find("TD").eq(7).html();
            familyMember.ReasonOfDeath = row.find("TD").eq(8).html();

            Familydetails.push(familyMember);
        });
        var viewModel = {
            'EmployeeId': $("#hdnAddEmpCode").val(),
            'ApplicationId': $("#hdnAppID").val(),
            'FamilyDetails': Familydetails,
            'NoOfBrother': $("#txtNoOfBrother").val(),
            'NoOfSister': $("#txtNoOfSister").val(),
            'NoOfChildren': $("#txtChildren").val()
        };
        $.ajax({
            url: '/Employee/InsertFamilyDetails',
            data: JSON.stringify(viewModel),
            async: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                GetFamilyDetails();
                alertify.success("Family details saved successfully");
                FserverResponse = true;
            }, error: function (result) {
                alertify.error("Could not save family details");
            }
        });
    }
    //}
    FserverResponse = true;
    return FserverResponse;
}
function SavePersonalDetails(eid) {
    var formData = new FormData($("#frmPersonalHealth").get(0));
    var PD = formData.get('ephd_date_of_last_period').split("-");
    var LPD = PD[2] + "/" + PD[1] + "/" + PD[0];
    formData.set('ephd_date_of_last_period', LPD);
    $.ajax({
        url: '/Employee/InsertPersonalDetails',
        data: formData,
        async: false,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result == 1) {
                PserverResponse = true;
                alertify.success("Personal details saved successfully");
            }
            else if (result == 0) {
                PserverResponse = false;
                alertify.error("Could not save personal details");
            }
            //PserverResponse = true;
            //alertify.success("Personal details saved successfully");
        }, error: function (result) {
            PserverResponse = false;
            alertify.error("Could not save personal details");
        }
    });
    return PserverResponse;
}
function SaveMedicalLeaveDetails(eid) {
    if ($("#tblMedicalLeaveDetails").find(".dataTables_empty").length > 0) {
        $.ajax({
            url: '/Employee/DeleteMedicalLeaveDetails',
            data: JSON.stringify({ 'EmpCode': $("#hdnAddEmpCode").val() }),
            async: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            processData: false,
            cache: false,
            success: function (result) {
                GetMedicalLeaveDetails();
                alertify.success("Medical leave details deleted successfully");
                $('#ApplicationTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
                MserverResponse = true;
            }, error: function (result) {
                alertify.error("Could not delete medical leave details");
            }
        });
    }
    else {
        var MedicalLeaveDetails = new Array();
        $("#tblMedicalLeaveDetails TBODY TR").each(function () {
            var row = $(this);
            var MedicalLeave = {};
            MedicalLeave.emld_application_id = $("#hdnAppID").val();
            MedicalLeave.startdate = row.find("TD").eq(0).html();
            MedicalLeave.enddate = row.find("TD").eq(1).html();
            MedicalLeave.emld_no_of_days = row.find("TD").eq(2).html();
            MedicalLeave.emld_leave_reason = row.find("TD").eq(3).html();
            MedicalLeave.emld_upload_document_path = row.find("TD").eq(8).html();
            MedicalLeave.emld_medical_reimbursement = row.find("TD").eq(7).html();
            MedicalLeave.emld_medical_reimbursement_doc = row.find("TD").eq(9).html();
            MedicalLeave.emld_medical_leave_id = row.find("TD").eq(10).html();
            MedicalLeaveDetails.push(MedicalLeave);
        });

        var MedicalLeaveData = {
            'EmpCode': $("#hdnAddEmpCode").val(),
            'MedicalLeaveDetails': MedicalLeaveDetails
        };
        $.ajax({
            url: '/Employee/InsertMedicalLeaveDetails',
            data: JSON.stringify(MedicalLeaveData),//JSON.stringify(),
            async: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                $("#tblMedicalLeaveDetails tbody tr").remove();
                var newClass;
                if ($("tr[data-row-number=" + $("#divMedLeave tr[data-row-number]").length + "]").hasClass("odd")) {
                    newClass = "even";
                } else {
                    newClass = "odd";
                }
                var rowNumber = 0;
                if (result.MedicalLeaveDetails.length == 0) {
                    var AddRow = "<tr class='odd'><td valign='top' colspan='8' class='dataTables_empty'>No data available in table</td></tr>";
                    $("#tblMedicalLeaveDetails tbody").append(AddRow);
                }
                else {
                    for (var l = 0; l < result.MedicalLeaveDetails.length; l++) {
                        rowNumber = rowNumber + 1;
                        if (rowNumber % 2 == 0) {
                            newClass = "even";
                        }
                        else {
                            newClass = "odd";
                        }
                        var fromDate = result.MedicalLeaveDetails[l].startdate;
                        var toDate = result.MedicalLeaveDetails[l].enddate;
                        var leaves = result.MedicalLeaveDetails[l].emld_no_of_days;
                        var remarks = result.MedicalLeaveDetails[l].emld_leave_reason;
                        var supportingDoc = result.MedicalLeaveDetails[l].emld_upload_document_path;
                        var isreimbursed = result.MedicalLeaveDetails[l].emld_medical_reimbursement;
                        var reimburseDoc = result.MedicalLeaveDetails[l].emld_medical_reimbursement_doc;
                        var leaveid = result.MedicalLeaveDetails[l].emld_medical_leave_id;
                        var supportingdocpath = result.MedicalLeaveDetails[l].supportingdocpath;
                        var reimbursedocpath = result.MedicalLeaveDetails[l].reimbursedocpath;
                        var AddRow = "";

                        AddRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + leaves + "</td><td>" + remarks + "</td><td>" + supportingDoc + "</td><td>" + reimburseDoc + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditMedLeaveDetail(" + rowNumber + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteMedLeaveRow(" + rowNumber + ");'>Delete</a></td><td hidden>" + isreimbursed + "</td><td hidden>" + supportingdocpath + "</td><td hidden>" + reimbursedocpath + "</td><td hidden>" + leaveid + "</td></tr>";

                        $("#tblMedicalLeaveDetails tbody").append(AddRow);
                    }
                }
                $('#ApplicationTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
                GetMedicalLeaveDetails();
                alertify.success("Medical Leave details saved successfully");
                MserverResponse = true;
            }, error: function (result) {
                alertify.error("Could not save medical leave details");
                MserverResponse = false;
            }
        });
    }
    return MserverResponse;
}

//DDO status update
function SaveUploadStatus(eid) {
    $.ajax({
        url: '/Employee/UpdateDdoUploadStatus?empid=' + eid + '',
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            var _valid = result;
            if (_valid != null || _valid != 0) {
                DserverResponse = true;
                $("#refNo").show();
            }
        }, error: function (result) {
            alertify.error("Could not save application form");
        }
    });

    return DserverResponse;
}
function ShowNomineeType() {
    if ($("#ddlMStatus").val() == "Unmarried") {
        $("#ddlUMRStatus").attr("style", "display:block;");
        $("#ddlMRStatus").attr("style", "display:none;");
    } else {
        $("#ddlUMRStatus").attr("style", "display:none;");
        $("#ddlMRStatus").attr("style", "display:block;");
    }
}

//Medical Tab save functions
function SaveHPhysicalDetails(eid) {
    if (medicalRequiredStatus) {
        ValidatePhysicalFields();
    }

    if ($('.err:visible').length === 0) {
        var formData = new FormData($("#frmMedHPhysicalDetailsBasicDetails").get(0));
        $.ajax({
            url: '/Employee/InsertHPhysicalDetails',
            data: formData,
            async: false,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                GetPhysicalDetails();
                alertify.success("Physical details saved successfully");
                HPserverResponse = true;
            }, error: function (result) {
                alertify.error("Could not save physical details");
                HPserverResponse = false;
            }
        });
    }
    else {
        HPserverResponse = false;
    }


    return HPserverResponse;
}
function SaveHOtherDetails() {

    var formData = new FormData($("#frmOtherDetails").get(0));

    if (medicalRequiredStatus) {
        ValidateOtherDetails();
    }

    if ($('.err:visible').length === 0) {
        $.ajax({
            url: '/Employee/InsertHOtherDetails',
            async: false,
            data: formData,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.IsSuccess) {
                    GetOtherDetails();
                    alertify.success("Other details saved successfully");
                    HOserverResponse = true;
                }
            }, error: function (response) {
                alertify.error("Could not save other details");
                HOserverResponse = false;
            }
        });
    }
    else {
        HOserverResponse = false;
    }

    return HOserverResponse;
}
function SaveHHealthDetails() {
    var formData = new FormData($("#frmHealthDetails").get(0));

    if (medicalRequiredStatus) {
        ValidateHealthDetails();
    }
    if ($('.err:visible').length === 0) {

        $.ajax({
            url: '/Employee/InsertHHealthDetails',
            async: false,
            data: formData,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.IsSuccess) {
                    GetHealthDetails()
                    alertify.success("Health details saved successfully");
                    HHserverResponse = true;
                }
            }, error: function (response) {
                alertify.error("Could not save health details");
                HHserverResponse = false;
            }
        });
    }
    else {
        HHserverResponse = false;
    }
    return HHserverResponse;
}
function SaveHDoctorDetails() {
    debugger;
    var result = ValidateDoctorDetails();
    if (result == true) {
        HDserverResponse = false;
        if ($('.err:visible').length === 0) {
            var submitUrl = $("#divDoctorDetails").data("submit-url");
            var DoctorDetails = {};
            DoctorDetails.EmployeeId = $("#hdnEmpId").val();
            DoctorDetails.NameOfDoctor = $("#txtDocName").val();
            DoctorDetails.DoctorId = $("#hdnDocId").val();
            DoctorDetails.KGIDOfDoctor = $("#txtKgid").val();
            DoctorDetails.Designation = $("#txtDesignation").val();
            DoctorDetails.NameOfHospital = $("#txtNameOfOffice").val();
            DoctorDetails.KMCCode = $("#txtKMCCode").val();
            DoctorDetails.ApplicationId = $("#spnReferanceNo").text();
            DoctorDetails.emdd_is_kmc_doctor = $(".rd-btn-hodd").prop("checked");
            DoctorDetails.PlaceOfHospital = $("#txtPlaceOfOffice").val();
            if (DoctorDetails.emdd_is_kmc_doctor == false) {
                DoctorDetails.BankAccNumber = $("#txtBankAccNum").val();
                DoctorDetails.IFSCCode = $("#txtIFSCCode").val();
                DoctorDetails.MICRCode = $("#txtMICRCode").val();
            }

            $.ajax({
                url: submitUrl,
                data: JSON.stringify({ "objHDoctorDetails": DoctorDetails }),
                type: 'POST',
                async: false,
                cache: false,
                contentType: 'application/json; charset=utf-8',
                processData: false,
                success: function (response) {
                    if (response.IsSuccess) {
                        GetDoctorDetails();
                        alertify.success("Doctor details saved successful");
                        HDserverResponse = true;
                    }
                    else {
                        alertify.error("Could not save");
                        HDserverResponse = false;
                    }
                }, error: function (response) {
                    alertify.error("Could not save");
                    HDserverResponse = false;
                }
            });
        }
        else {
            HDserverResponse = false;
            return false;
        }
    }
    else {
        HDserverResponse = false;
        return false;
    }
    return HDserverResponse;
}
function SaveHDeclarationDetails(eid) {
    var sts = 0;
    if ($("#chkHDeclaration").prop("checked") == true) {
        sts = 1;
    }
    else {
        return false;
    }
    var viewModel = {
        'md_sys_emp_code': eid, 'md_declaration_status': sts,
        'md_active': 1, 'md_creation_datetime': new Date()
    };
    $.ajax({
        url: '/Employee/InsertHDeclarationDetails',
        data: JSON.stringify(viewModel),
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            var _valid = result;
            if (_valid != null || _valid != 0) {
                alertify.success("Medical Submited Successfully.");
                HDCserverResponse = true;
                $("#HRefNo").show();
            }
        }, error: function (result) {
            alertify.error("Could not submit medical report.");
            HDCserverResponse = false;
        }
    });
    return HDCserverResponse;
}
function SubmitMedicalForm(eid) {
    var url = $("#medical").data("submit-url");
    url = url + "?empId=" + eid;

    $.post(url).done(function (response) {
        alert("Medical details submitted successfully");
    });
}

function printApplicationForm() {
    if ($("#chkDeclaration").prop("checked") == false) {
        alertify.alert("Please agree to the terms and condition.").setHeader("Warning !!!");
        return false;
    }
    $("#spnBReferanceNo").text($("#hdnBReferanceNo").val());
    $("#imgBQRCode").attr("src", $("#hdnQRCode").val());
    $("#divShowQRCode").show();
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Basic Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintBasicDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>KGID Details</h2></div><div class="form-group col-4"></div></div>' + $("#divKGID").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPersonal").html() + "<hr />";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-5"><h2>Medical Leave Details</h2></div><div class="form-group col-4"></div></div>' + $("#divMedLeave").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html();
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>Application Form</title>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.
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
    $('#ApplicationTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
}
function printMedicalReport() {
    if ($("#chkHDeclaration").prop("checked") == false) {
        alertify.alert("Please agree to the terms and condition.").setHeader("Warning !!!");
        return false;
    }
    $("#spnHReferanceNo").text($("#hdnBReferanceNo").val());
    $("#imgHQRCode").attr("src", $("#hdnQRCode").val());
    $("#divHShowQRCode").show();
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Basic Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHBasicDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Physical Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHPhysicalDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Other Details</h3></div><div class="form-group col-4"></div></div>' + $("#divOtherDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Health Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHealthDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Doctor Details</h3></div><div class="form-group col-4"></div></div>' + $("#divDoctorDetails").html(); + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Declaration</h3></div><div class="form-group col-4"></div></div>' + $("#divHDeclaration").html();
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>Medical Report</title>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.
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
    $('#MedicalTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
}
function GenerateChallan() {
    if ($('#txtPayAmount').val()) {
        $.getJSON("../employee/GenerateChallan", { amount: $('#txtPayAmount').val() },
            function (data) {
                $('#txtpaymentrefno').val(data);
            });
    }
    else {
        bootbox.alert("<p class='bootbox-alert'>Please enter amount</p>");
    }
}
function SaveChallanDetails() {
    if (!$('#txtPayAmount').val()) {
        bootbox.alert("<p class='bootbox-alert'>Please enter amount</p>");
    }
    else if (!$('#txtdateofpayment').val()) {
        bootbox.alert("<p class='bootbox-alert'>Please enter date of challan</p>");
    }
    else if (!$('#txtpaymentrefno').val()) {
        bootbox.alert("<p class='bootbox-alert'>Please enter reference no</p>");
    }
    else {
        $.ajax({
            url: '/Employee/AddChallanDetails',
            data: { amount: $('#txtPayAmount').val(), challanNo: $('#txtpaymentrefno').val(), challanDate: $('#txtdateofpayment').val() },
            type: 'POST',
            success: function (result) {
                console.log(result);
                $('#txtpaymentrefno1').val(result.cd_challan_ref_no);
                $('#txtamount1').val(result.cd_amount);
                $('#txttransactionno1').val('113001150541');
                var date = new Date(parseInt(result.cd_datetime_of_challan.substr(6)));
                $('#txtdateofpayment1').val(date);
                bootbox.alert("<p class='bootbox-alert'>Challan Details Successfully Added.</p>");
            }
        });
    }

}

function SaveChallanDetails(eid) {
    var viewModel = {
        'cd_system_emp_code': eid, 'cd_referance_number': $("#spnReferanceNo").text(), 'cd_challan_ref_no': $("#txttransactionno1").val(),//$("#txtpaymentrefno").val(),
        'cd_dept_code': $("#txtDistrictOfffice").val(), 'cd_ddo_code': $("#txtDistrictOfffice").val(), 'cd_purpose_code': $("#txtPurpose").val(), 'cd_subpurpose_code': $("#txtSubPurpose").val(),
        'cd_head_of_account': $("#txtHOA").val(), 'cd_amount': $("#txtPayAmount").val(), 'cd_datetime_of_challan': new Date(), 'cd_status': '1', 'cd_creation_datetime': new Date()
    };
    $.ajax({
        url: '/Employee/InsertChallanDetails',
        data: JSON.stringify(viewModel),
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $("#txtRefNo").val($("#spnReferanceNo").text());
            alertify.success("Payment details saved successfully");
            CserverResponse = true;
        }, error: function (result) {
            alertify.error("Could not save Payment details");
        }
    });

    return CserverResponse;
}

function ApplicationDetailsSave() {
    
    var eid = $("#divApplication").data("eid");
    var StepNo = $("#ApplicationTab ul li.current").find('a').attr('id');
    
    if (StepNo == "ApplicationTab-t-0") {
        var status = ValidateBasicDetails();
        if (status == true) {

        
        if ($('.err:visible').length === 0) {
            var result = SaveBasicDetails(eid);
            if (result === true) {
                alertify.success("Basic details saved successfully");
            }
            return result;
        }
        else {
            return false;
        }
        }
        else {
            return false;
        }
    }
    else if (StepNo == "ApplicationTab-t-1") {
        debugger
        if ($("#hdnSentBackAppliaction").val() == 1) {
            return true;
        } else {
            if (parseInt($("#txtAmount").val()) != 0) {
                if (parseInt($("#txtAmount").val()) < 100) {
                    $("#txtAmount").val($("#txtMPremium").val());
                    $('#errTotalAmount').text('Entered amount should be greater than 100(Rupees).');
                    $('#errTotalAmount').removeAttr("hidden");
                    return false;
                }
            }
            var KGIDPremium = $("#txtMPremium").val();
            var EnteredAmount = $("#txtAmount").val();
            $("#txtPayAmount").val(EnteredAmount);
            $("#hdnKgidPremiumAmount").val(EnteredAmount);
            var grossSalary = $("#hdnGrossPay").val();
            var GrossHalfSal = parseFloat(grossSalary) / 2;//50% of Gross Salary
            var AlreadyPolicy = $(".csTotal").text();
            if (parseFloat(EnteredAmount) < parseFloat(KGIDPremium)) {
                $('#errTotalAmount').text('Premium Amount should be greater than or equal to minimum monthly premium.');
                $("#txtAmount").val($("#txtMPremium").val());
                $('#errTotalAmount').removeAttr("hidden");
                return false;
            }
            var NewPolicyAmount = 0;
            var ExistingPolicyAmount = 0;
            $("#tblKgidPremium tr td:nth-child(2)").each(function () {
                var value = $(this);
                if (value[0].innerHTML !== "") {
                    if (ExistingPolicyAmount == 0) {
                        ExistingPolicyAmount = parseFloat(value[0].nextElementSibling.innerHTML);
                    } else {
                        ExistingPolicyAmount = ExistingPolicyAmount + parseFloat(value[0].nextElementSibling.innerHTML);
                    }
                } else {
                    //if (NewPolicyAmount == 0) {
                    //    NewPolicyAmount = parseFloat(value[0].nextElementSibling.innerHTML);
                    //} else {
                    NewPolicyAmount = NewPolicyAmount + parseFloat(EnteredAmount);
                    //}
                }
            });
            var TotalAmount = 0;
            TotalAmount = parseFloat(ExistingPolicyAmount) + parseFloat(EnteredAmount);
            if (EnteredAmount == 0) {
                TotalAmount = parseFloat(ExistingPolicyAmount) + parseFloat(NewPolicyAmount);
            }

        if (GrossHalfSal > TotalAmount) {
            $("#hdnNewPolicyAmount").val(NewPolicyAmount);
        } else {
            $('#errTotalAmount').text('Total amount should be lesser than 50% of Gross Salary. Minimum amount is ' + KGIDPremium);
            $('#errTotalAmount').removeAttr("hidden");
            $("#txtAmount").val(KGIDPremium);

                return false;
            }
            var SubPolicyCondition = true;
            var UnProgPolicy = false;
            $("#tblKgidPremium tr td:nth-child(2)").each(function () {
                var value = $(this);
                if (value[0].innerHTML == "") {
                    UnProgPolicy = true;
                    $("#hdnNewPolicyAmount").val(NewPolicyAmount);
                    //$("#hdnNewPolicyAmount").val(value[0].nextElementSibling.innerHTML);
                    //$("#txtPayAmount").val(value[0].nextElementSibling.innerHTML);
                    //$("#txtPayAmount").val(0);
                }
                else {
                    if (($("#txtAmount").val() == 0 || $("#txtAmount").val() == "") && UnProgPolicy == false) {
                        $('#errTotalAmount').text('Premium Amount should not be zero for subsequent policy.');
                        $('#errTotalAmount').removeAttr("hidden");
                        SubPolicyCondition = false;
                    }
                }
            });
            if (SubPolicyCondition == true) {
                var result = SaveKGIDDetails(eid);
                if (result === true) {
                    alertify.success("KGID details saved successfully");
                    GetKGIDDetails();
                }
            }
            //if (NewPolicyAmount == 0) {
            //    $("#hdnNewPolicyAmount").val(EnteredAmount);
            //}
            //else {
            //    $("#hdnNewPolicyAmount").val(NewPolicyAmount);
            //}
            return SubPolicyCondition;
        }
    }
    else if (StepNo == "ApplicationTab-t-2") {
        if ($("#tblFamilyDetails TBODY TR").find("td").hasClass("dataTables_empty")) {
            alertify.alert('Add Family Details.').setHeader("Warning !!!");
            return false;
        } else if ($("#tblFamilyDetails TBODY TR").length == 0) {
            alertify.alert('Add Family Details.').setHeader("Warning !!!");
            return false;
        } else {
            var RCount = 0;
            $("#tblFamilyDetails tr td:nth-child(1)").each(function () {
                var RelationData = this.innerText;
                if (RelationData == "Children" || RelationData == "Spouse") {
                    RCount++;
                }
            });
            if (RCount > 0) {
                MrtStatus = "Married";
                $('#ddlNomineeRelation').empty();
                $('#ddlNomineeRelation').append(new Option("-- Select --", ""), new Option("Spouse", "Spouse"), new Option("Children", "Children"));
                RCount = 0;
            } else {
                MrtStatus = "Unmarried";
                $('#ddlNomineeRelation').empty();
                $('#ddlNomineeRelation').append(new Option("-- Select --", ""), new Option("Father", "Father"), new Option("Mother", "Mother"));
            }
            var result = SaveFamilyDetails(eid);
        }
    }
    else if (StepNo == "ApplicationTab-t-3") {
        if ($("#tblNomineeDetails TBODY TR").find("td").hasClass("dataTables_empty")) {
            alertify.alert('Add Nominee Details.').setHeader("Warning!!!");
            return false;
        } else if ($("#tblNomineeDetails TBODY TR").length == 0) {
            alertify.alert('Add Nominee Details.').setHeader("Warning!!!");
            return false;
        } else if (!ValidateSumShare()) {
            alertify.alert('Percentage share for nominees should be 100 %.').setHeader("Warning!!!");
            return false;
        }
        else {
            var result = ValidateSumShare();
            if (result === true) {
                alertify.success("Nominee details saved successfully");
            }
            return result;
        }
    }
    else if (StepNo == "ApplicationTab-t-4") {
        validatePersonalFields();
        if ($('.err:visible').length === 0) {
            var result = SavePersonalDetails(eid);
            if (result == true) {
                GetPersonalDetails();
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
    //else if (StepNo == "ApplicationTab-t-5") {
    //    if (!MserverResponse) {
    //        var result = SaveMedicalLeaveDetails(eid);
    //        return true;
    //    }
    //    else {
    //        alertify.success("Medical leave details saved successfully.");
    //        return true;
    //    }
    //}
    else if (StepNo == "ApplicationTab-t-5") {
        if ($("#chkDeclaration").prop("checked") == false) {
            alertify.alert("Please agree to the terms and condition.").setHeader("Warning !!!");
            return false;
        }
        else {
            alertify.success("Application details saved successfully.");
        }
        SaveUploadStatus(eid);
    }
}
function PaymentDetailsSave() {
    var eid = $("#divApplication").data("eid");

    var StepNo = $("#PaymentTab ul li.current").find('a').attr('id');
    if (StepNo == "PaymentTab-t-0") {
        return SaveChallanDetails(eid);
    }
}
function MedicalDetailsSave() {
    var eid = $("#divApplication").data("eid");

    var StepNo = $("#MedicalTab ul li.current").find('a').attr('id');
    if (StepNo == "MedicalTab-t-1") {
        return SaveHPhysicalDetails(eid);
    } else if (StepNo == "MedicalTab-t-2") {
        return SaveHOtherDetails();
    } else if (StepNo == "MedicalTab-t-3") {
        return SaveHHealthDetails();
    } else if (StepNo == "MedicalTab-t-4") {
        return SaveHDoctorDetails();
    } else if (StepNo == "MedicalTab-t-5") {
        if ($("#chkHDeclaration").prop("checked") == false) {
            alertify.alert("Please agree to the terms and condition.").setHeader("Warning !!!");
            return false;
        }
        alertify.success("Medical Report saved successfully");
        return true;
    }
}

$('#nav-Medical-tab').click(function () {
    if (!$(this).hasClass("disabled")) {
        alertify.confirm("Do you have medical report provided by doctor?", function () {
            $('#MedicalTab ul[aria-label=Pagination] li a[href="#"]').parent().attr("aria-hidden", "false").removeAttr("aria-disabled").removeAttr("class", "disabled");
        }, function () {
            $('#MedicalTab ul[aria-label=Pagination] li a[href="#next"]').parent().removeAttr("aria-hidden").attr("aria-disabled", "true").attr("class", "disabled");
        }).set('labels', { ok: 'Yes', cancel: 'No' }).setHeader("Medical report from doctor");
    }
});

//Personal Tab get functions
function GetBasicDetails() {
    $.ajax({
        url: '/Employee/BasicDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-0").html(data);
            EnglishToKannada();
        }
    });
}
function GetKGIDDetails() {
    $.ajax({
        url: '/Employee/KGIDDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-1").html(data);
            EnglishToKannada();
        }
    });
}
function GetFamilyDetails() {

    $.ajax({
        url: '/Employee/FamilyDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-2").html(data);
            EnglishToKannada();
        }
    });
}
function GetNomineeDetails() {
    $.ajax({
        url: '/Employee/NomineeDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-3").html(data);
            EnglishToKannada();
        }
    });

}
function GetPersonalDetails() {
    $.ajax({
        url: '/Employee/PersonalDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-4").html(data);
            EnglishToKannada();
        }
    });
}
function GetMedicalLeaveDetails() {
    $.ajax({
        url: '/Employee/MedicalLeaveDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-5").html(data);
            EnglishToKannada();
        }
    });
}

function GetDeclaration() {
    $.ajax({
        url: '/Employee/DeclarationDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab-p-5").html(data);
            EnglishToKannada();
            if ($('.knlan').is(":visible")) {
                $("#lblEmployeeNameKN").text($("#txtEmployeeFullNameKN").val());
            }
            else {
                $("#lblEmployeeNameEN").text($("#txtEmployeeFullName").val());
            }
        }
    });
}
//Medical Tab get functions
function GetMBasicDetails() {
    $.ajax({
        url: '/Employee/HBasicDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#MedicalTab-p-0").html(data);
            EnglishToKannada();
        }
    });
}
function GetPhysicalDetails() {
    $.ajax({
        url: '/Employee/HPhysicalDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#MedicalTab-p-1").html(data);
            EnglishToKannada();
        }
    });
}
function GetOtherDetails() {
    $.ajax({
        url: '/Employee/HOtherDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#MedicalTab-p-2").html(data);
            EnglishToKannada();
        }
    });
}
function GetHealthDetails() {
    $.ajax({
        url: '/Employee/HHealthDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#MedicalTab-p-3").html(data);
            EnglishToKannada();
        }
    });
}
function GetDoctorDetails() {
    $.ajax({
        url: '/Employee/HDoctorDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#MedicalTab-p-4").html(data);
            EnglishToKannada();
        }
    });
}

function GetPaymentDetails() {
    $.ajax({
        url: '/Employee/PaymentDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#PaymentTab-p-0").html(data);
            EnglishToKannada();
        }
    });
}
function SwitchMedicalRequired() {
    var dateOfBirthUnformatted = $("#txtBasicDateOfBirth").val().split('-');
    var dateOfBirth = new Date(dateOfBirthUnformatted[2], dateOfBirthUnformatted[1] - 1, dateOfBirthUnformatted[0]);
    var currentDate = new Date();
    var difference = currentDate.getFullYear() - dateOfBirth.getFullYear();
    var initialDeposit = 0;

    var NewPolicyAmount = 0;
    var ExistingPolicyAmount = 0;
    if ($("#hdnSentBackAppliaction").val() != 0) {
        $("#tblKgidPremium tr td:nth-child(2)").each(function () {
            var value = $(this);
            if (value[0].innerHTML !== "") {
                if (ExistingPolicyAmount == 0) {
                    ExistingPolicyAmount = parseFloat(value[0].nextElementSibling.innerHTML);
                } else {
                    ExistingPolicyAmount = ExistingPolicyAmount + parseFloat(value[0].nextElementSibling.innerHTML);
                }
            } else {
                if (NewPolicyAmount == 0) {
                    NewPolicyAmount = parseFloat(value[0].nextElementSibling.innerHTML);
                } else {
                    NewPolicyAmount = NewPolicyAmount + parseFloat(value[0].nextElementSibling.innerHTML);
                }
            }
        });
        initialDeposit = NewPolicyAmount;
    }
    else {
        if ($("#divKGID").find(".dataTables_empty").length === 1) {
            initialDeposit = parseFloat($("#txtAmount").val());
		        $("#hdnNewPolicyAmount").val(initialDeposit);
        }
        else {
            $("#tblKgidPremium tbody tr td:nth-child(2)").each(function () {
                var value = $(this);
                if (value[0].innerHTML == "") {
                    if ($("#txtAmount").val() == "0" && NewPolicyAmount == 0) {
                        NewPolicyAmount = parseFloat(value[0].nextElementSibling.innerHTML);
                    }
                    else if ($("#txtAmount").val() != "0") {
                        NewPolicyAmount = parseFloat($("#txtAmount").val());
                    } else {
                        NewPolicyAmount = NewPolicyAmount + parseFloat(value[0].nextElementSibling.innerHTML);
                    }
                } else {
                    $("#hdnInsuredEmployee").val(1);
                    if (ExistingPolicyAmount == 0) {
                        ExistingPolicyAmount = parseFloat(value[0].nextElementSibling.innerHTML);
                    } else {
                        ExistingPolicyAmount = ExistingPolicyAmount + parseFloat(value[0].nextElementSibling.innerHTML);
                    }
                }
            });
            if (NewPolicyAmount == 0) {
                $("#hdnNewPolicyAmount").val($("#txtAmount").val());
            } else {
                $("#hdnNewPolicyAmount").val(NewPolicyAmount);
            }
            initialDeposit = parseFloat($("#hdnNewPolicyAmount").val());
        }
    }

    if (difference <= 40 && initialDeposit < 1000) {
        medicalRequiredStatus = false;
        $("#hdnMedicalRequired").val(false);
    }
    if (difference > 40) {
        medicalRequiredStatus = true;
        $("#hdnMedicalRequired").val(true);
    }
    if (initialDeposit >= 1000) {
        medicalRequiredStatus = true;
        $("#hdnMedicalRequired").val(true);
    }
}
function printNBApplication() {
    $("#divShowQRCode").show();
    $("#spnBReferanceNo").text($("#spnReferanceNo").text());
    $("#divHeightChart").hide();
    var Applicationcontents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Basic Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintBasicDetails").html();
    Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>KGID Details</h2></div><div class="form-group col-4"></div></div>' + $("#divKGID").html() + '<div class="html2pdf__page-break"></div>';
    Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html() + '<div class="html2pdf__page-break"></div>';
    Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + '<div class="html2pdf__page-break"></div>';
    Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPersonal").html() + '<div class="html2pdf__page-break"></div>';
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-5"><h2>Medical Leave Details</h2></div><div class="form-group col-4"></div></div>' + $("#divMedLeave").html() + '<div class="html2pdf__page-break"></div>';
    Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html() + "<hr />";

    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();

    html2pdf(Applicationcontents, {
        margin: 1,
        filename: "ApplicationForm" + "_" + date,
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 2, letterRendering: true },
        jsPDF: { unit: 'in', format: 'a3', orientation: 'landscape' },
        pagebreak: { mode: 'legacy' }
    });
    $("#divHeightChart").show();
    $("#divShowQRCode").hide();
}
function ValidateNominee() {
    if ($("#tblNomineeDetails TBODY TR").find("td").hasClass("dataTables_empty")) {
        alertify.error("Add Nominee Details.");
        return false;
    } else if ($("#tblNomineeDetails TBODY TR").length == 0) {
        alertify.error("Add Nominee Details.");
        return false;
    } else if (!ValidateSumShare()) {
        alertify.error("Percentage share for nominees should be 100 %.");
        return false;
    }
    else {
        // GetPersonalDetails();
        return true;
    }
}
$(document).ready(function () {
    $("#ApplicationTab-t-3").on("click", function (event) {
        GetNomineeDetails();
    });
    $("#ApplicationTab-t-2").on("click", function (event) {
        GetFamilyDetails();
    });
    $("#ApplicationTab-t-5").on("click", function (event) {
        GetDeclaration();
        GetNomineeDetails();
    });
    $("#nav-Payment-tab").on("click", function (event) {
        GetPaymentDetails();
    });
});

$(document).ready(function () {
    debugger;
    if ($("#hdnSentBackAppliaction").val() != 0) {
       
        if ($("#hdnRmrks").val() == 4) {
          
        }
        if ($("#hdnRmrks").val() == 3) {

            $('#nav-Application').removeClass("show active");
            $('#nav-Medical').addClass("show active");

            $("#nav-Application").removeClass("active");
            $("#nav-Medical").addClass("active");

            sessionStorage.setItem('IsMedicalRequired', medicalRequiredStatus);
            var value = sessionStorage.getItem("IsMedicalRequired");

            $('#hdfIsMedicalRequired').val(value);
           
        }
        if ($("#hdnRmrks").val() == 2) {

            $('#nav-Application').addClass("show active");
            $('#nav-Medical').removeClass("show active");

            $("#nav-Application").addClass("active");
            $("#nav-Medical").removeClass("active");
        }
    }
    else {
        if ($("#hdnPaymentStatus").val() == 1) {
            //$('#nav-Application-tab').addClass("disabled").removeClass("active");
            //$('#nav-Payment-tab').removeClass("disabled").addClass("active");
            //$('#nav-Medical-tab').addClass("disabled").removeClass("active");
            //$('#nav-Application,#nav-Medical').removeClass("show active");
            //$('#nav-Payment').addClass("show active");
            //$("#nav-Payment-tab").click();
            $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "none");
        }
        else if ($("#hdnPaymentStatus").val() == 0 || $("#hdnPaymentStatus").val() == 2 ) {
            debugger;
            $('#nav-Application-tab').addClass("disabled").removeClass("active");
            $('#nav-Payment-tab').removeClass("disabled").addClass("active");
            $('#nav-Medical-tab').addClass("disabled").removeClass("active");
            $('#nav-Application,#nav-Medical').removeClass("show active");
            $('#nav-Payment').addClass("show active");
            $("#nav-Payment-tab").click();
            $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "none");
        }
        else if ($("#hdnPaymentStatus").val() == 3) {
            $('#nav-Application-tab').addClass("disabled").removeClass("active");
            $('#nav-Payment-tab').removeClass("disabled").addClass("active");
            $('#nav-Medical-tab').addClass("disabled").removeClass("active");
            $('#nav-Application,#nav-Medical').removeClass("show active");
            $('#nav-Payment').addClass("show active");
            $("#nav-Payment-tab").click();
            $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
        }
    }
});
function EnglishToKannada() {
    var _knLan = $('.knlan');
    var _EnLan = $('.Enlan');
    if (localStorage.ChangeLang == '0') {
        _EnLan.hide();
        _knLan.show();
        $('#changeLan').val('A');
    }
    else {
        _knLan.hide();
        _EnLan.show();
        $('#changeLan').val('ಕ');
    }
}

function printform() {
    //GetNomineeDetails();
    var Applicationcontents = "";
  
        Applicationcontents = $('#divPrintForm').html();
        //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>KGID Details</h2></div><div class="form-group col-4"></div></div>' + $("#divKGID").html() ;
        //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html();
        //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + '<div class="html2pdf__page-break"></div>';
        //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintPD").html();
        //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html() + '<input type="checkbox" value="true" checked /><label>I agree to the terms and condition mentioned above.</label>' + "<hr />";

    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();

    //html2pdf(Applicationcontents, {
    //    margin: 0.3,
    //    filename: "ApplicationForm" + "_" + date,
    //    image: { type: 'jpeg', quality: 1 },
    //    html2canvas: { scale: 0, letterRendering: true },
    //    jsPDF: { unit: 'in', format: 'letter', orientation: 'Portrait' },
    //    pagebreak: { mode: 'legacy' }
    //});
    let mywindow = window.open('', 'PRINT', 'height=650,width=900,top=100,left=150');

    mywindow.document.write('<html><head><title>ApplicationForm' + date + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(Applicationcontents);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
    $("#divHeightChart").show();
    $("#divShowQRCode").hide();
    $(".action").show();
}
function GetPKGIDDetails() {
    $.ajax({
        url: '/Employee/PKGIDDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#divPKgidDtl").html(data);
            EnglishToKannada();
        }
    });
}