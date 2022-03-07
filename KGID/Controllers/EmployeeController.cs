using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.NewEmployeeBLL;
using BLL.DDOMasterBLL;
using BLL.DeptMasterBLL;
using KGID_Models.KGID_User;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using BLL.InsuredEmployeeBll;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using HtmlAgilityPack;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using KGID_Models.NBApplication;
using BLL.UploadEmployeeBLL;
using KGID_Models.KGID_Policy;
using System.Net;
using System.Web.Routing;
using static KGID.FilterConfig;
using KGID_Models.KGID_VerifyData;
//PDF DSC SignIn
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using iTextSharp.text.pdf.security;
using System.Security.Cryptography.X509Certificates;
using DLL.DBConnection;
using KGID_Models.KGID_Master;
using Common;
using System.Web.Configuration;

using System.Threading;

namespace KGID.Controllers
{
   
    public class EmployeeController : Controller
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly INewEmployeeDetailsBLL _newemp;
        private readonly IDDOMasterBLL _ddomaster;
        private readonly IDeptMasterBLL _deptmaster;

        private readonly IInsuredEmployeeBll _InsuredEmployeebll;
        private readonly INBApplicationBll _INBApplicationbll;
        private readonly IUploadEmployeeBLL _uploadbll;

        private readonly AllCommon _commnobj = new AllCommon();
        public EmployeeController()
        {
            this._newemp = new NewEmployeeDetailsBLL();
            this._ddomaster = new DDOMasterBLL();
            this._deptmaster = new DeptMasterBLL();

            this._InsuredEmployeebll = new InsuredEmployeeBll();
            this._INBApplicationbll = new NBApplicationBll();
            this._uploadbll = new UploadEmployeeBLL();
        }
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewEmployeeSearch(tbl_new_employee_basic_details _newempdetails)
        {
            tbl_new_employee_basic_details _newEmpModel = new tbl_new_employee_basic_details();
            var _newempDetails = _newemp.EmpDetailsbll(_newempdetails);

            tbl_ddo_master _ddoMasterModel = new tbl_ddo_master();
            var _ddoMasterDetails = _ddomaster.DDOMasterbll();

            _newEmpModel.GetNewEmployeeList = _newempDetails;
            //TempData["NewEmpList"] = _newempDetails;
            ViewBag.NewEmp = _newempDetails;
            ViewBag.Data = _ddoMasterDetails;
            //TempData["DDOMasterList"] = _ddoMasterDetails;
            return View(_newEmpModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadEmployeeNamesByDDO(string _ddoCode, long _empCode)
        {
            var _depData = _newemp.GetEmpNameByDeptCode(_ddoCode, _empCode);

            return Json(_depData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadEmployeeNamesByDDOCode(string _ddoCode)
        {
            var _depData = _newemp.LoadEmployeeNamesByDDOCode(_ddoCode);

            return Json(_depData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadMobileAndEmailByDeptCode(string _ddoCode, long _empCode)
        {
            var _empData = _newemp.GetMobileAndEmailByDeptCode(_ddoCode, _empCode);

            return Json(_empData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getEmployeeData()
        {
            var _empData = _newemp.GetEmployeeBasicData();

            var _empDataList = (from n in _empData
                                select new
                                {
                                    n.nebd_dept_emp_code,
                                    nebd_date_of_birth = n.nebd_date_of_birth.HasValue ? n.nebd_date_of_birth.Value.ToString("dd-MM-yyyy") : string.Empty,
                                    n.nebd_place_of_birth,
                                    n.nebd_emp_full_name,
                                    n.nebd_mobilenumber,
                                    n.nebd_email,
                                    n.nebd_father_name,
                                    n.nebd_spouse_name,
                                    n.nebd_gender,
                                    n.nebd_sys_emp_code
                                }).ToList();

            return Json(_empDataList, JsonRequestBehavior.AllowGet);
        }
        #region InsuredEmployee
        public ActionResult InsuredEmployee(VM_Employee objEmployee)
        {
            if (objEmployee.KGIDNO != 0)
            {
                VM_Employee objGetEmpDetails = new VM_Employee();
                objGetEmpDetails.GetDetails = _InsuredEmployeebll.ViewInsuredEmployeebll(objEmployee.KGIDNO);
                return View(objGetEmpDetails);
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult GetMobileNumberOfInsured(long kgId)
        {
            long mobileNumber = 0;
            try
            {
                Logger.LogMessage(TracingLevel.INFO, "GetMobileNumberOfInsured()");
                mobileNumber = _newemp.GetMobileNumberByKGID(kgId);

                if (mobileNumber > 1)
                {
                    return Json(new { IsSuccess = true, MobileNumber = mobileNumber, Message = string.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "Error=" + ex.Message);
            }
            return Json(new { IsSuccess = false, MobileNumber = mobileNumber, Message = "Please enter valid credentials" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult CaseWorker()
        {
            return View();
        }

        public ActionResult Superintendent()
        {
            return View();
        }

        public ActionResult DIO()
        {
            return View();
        }
        [Route("kgid-ins")]
        public ActionResult GetInstructions()
        {
            long EmployeeID = 0;
            if (Session["UID"] != null)
            {
                EmployeeID = Convert.ToInt64(Session["UID"]);
            }
            string CheckEmpAgeStatus = _INBApplicationbll.CheckEmployeeAge(EmployeeID);
            if (CheckEmpAgeStatus == "1")
            {
                ViewBag.Instructions = "1";
                return View("Instruction");
            }
            else if (CheckEmpAgeStatus == "-1")
            {
                ViewBag.Instructions = "-1";
                return View("Instruction");
            }
            else
            {
                if (CheckEmpAgeStatus == "0")
                {
                    ViewBag.Instructions = "0";
                    return View("Instruction");
                }
                else
                {
                    return RedirectToAction("ViewApplicationDetails", "VerifyDetails");
                }
            }
        }

        public ActionResult UploadEmployeeDetails()
        {
            return View();
        }

        public ActionResult EditEmployeeDetails(string empId)
        {
            VM_EditEmployeeDetails employeeDetails = _INBApplicationbll.GetEmployeeDetailsById(Convert.ToInt64(empId));
            return PartialView("_EmployeeDetails", employeeDetails);
        }

        #region ApplicationForm
        //[HttpPost]
        [Route("kgid-app")]
        [CustomAuthorize("Employee")]
        public ActionResult ApplicationForm()/*long empId*/
        {
            long empId = Convert.ToInt64(Session["UID"]);
            string EmployeeId;
            EmployeeId = Request.Params["hdnUID"];
            VM_ApplicationDetail applicationDetails = new VM_ApplicationDetail();
            if (empId != 0)
            {
                applicationDetails = _INBApplicationbll.GenerateApplicationNumber(empId);
                if (applicationDetails.ApplicationCount == 0)
                {
                    if (applicationDetails.RestrictApplyingPolicy > 50)
                    {
                        ViewBag.RestrictApplyingPolicy = true;
                    }
                    else
                    {
                        ViewBag.RestrictApplyingPolicy = false;
                        ViewBag.ApplicationProcess = false;
                        if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationNumber))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(applicationDetails.ApplicationNumber, QRCodeGenerator.ECCLevel.Q);
                                using (Bitmap bitMap = qrCode.GetGraphic(20))
                                {
                                    bitMap.Save(ms, ImageFormat.Png);
                                    applicationDetails.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                                }
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.ApplicationProcess = true;
                }
                Session["UID"] = empId;
            }

            return View(applicationDetails);
        }

        public ActionResult BasicDetailsToView()
        {
            VM_BasicDetails _BasicData = _INBApplicationbll.BasicDetailsBll(Convert.ToInt64(Session["UID"]));
            if (_BasicData == null)
            {
                _BasicData = new VM_BasicDetails();
            }
            else
            {
                if (_BasicData.kad_kgid_application_number != null && _BasicData.kad_kgid_application_number != "")
                {
                    Session["AppRefNo"] = _BasicData.kad_kgid_application_number;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(_BasicData.kad_kgid_application_number.ToString(), QRCodeGenerator.ECCLevel.Q);
                        using (Bitmap bitMap = qrCode.GetGraphic(20))
                        {
                            bitMap.Save(ms, ImageFormat.Png);
                            _BasicData.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            return this.PartialView("_BasicDetails", _BasicData);
        }

        public string GetNomineeList(long AppID, string IsMarried)
        {
            string result = string.Empty;
            result = _INBApplicationbll.GetNomineeList(AppID, IsMarried);
            return result;
        }

        //public ActionResult NEBasicDetailsToView()
        //{
        //    var _BasicData = _INBApplicationbll.NEBasicDetailsBll(Convert.ToInt32(Session["UID"]));
        //    return this.PartialView("_BasicDetails", _BasicData);
        //}
        public ActionResult KGIDDetailsToView()
        {
            VM_PolicyDetails _KGIDData = _INBApplicationbll.KGIDDetailsBll(Convert.ToInt64(Session["UID"]));
            if (_KGIDData == null)
            {
                _KGIDData = new VM_PolicyDetails();
            }

            return this.PartialView("_KGIDDetails", _KGIDData);
        }
        public ActionResult NomineeDetailsToView()
        {
            VM_NomineeDetails _NomineeData = _INBApplicationbll.NomineeDetailsBll(Convert.ToInt64(Session["UID"]));
            if (_NomineeData == null)
            {
                _NomineeData = new VM_NomineeDetails();
            }

            return this.PartialView("_NomineeDetails", _NomineeData);
        }

        public ActionResult DeclarationDetailsToView()
        {
            return this.PartialView("_DeclarationDetails");
        }
        public int InsertKgidDetails(VM_PolicyDetails objKGIDDetails, bool isMedReq)
        {
            Session["IsMedicalRequired"] = isMedReq;
            Session["IsApplicationReqd"] = true;
            int result = _INBApplicationbll.SaveNBPolicyBll(objKGIDDetails);
            if (result == 1)
            {
                // TempData["Payscale"] = objKGIDDetails;
                ViewBag.Payscal = objKGIDDetails;
            }
            return result;
        }
        // Restrict the nominee details to maximum 4
        public long MaxNomineeDetails(long EmpId)
        {
            var result = _INBApplicationbll.RestrictNomineeDetails(EmpId);
            return result;
        }
        public long InsertNomineeDetails(VM_NomineeDetail objNomineeDetails)
        {
            var result = _INBApplicationbll.SaveNBNomineeBll(objNomineeDetails);
            return result;
        }

        public long DeleteNomineeDetails(tbl_nominee_details objnomineeDetails)
        {
            var result = _INBApplicationbll.DeleteNBNomineeBll(objnomineeDetails.nd_id);
            return result;
        }

        public long InsertDeclarationDetails(tbl_nb_declaration_master objDeclarationDetails)
        {
            try
            {
                var result = _INBApplicationbll.SaveNBDeclarationBll(objDeclarationDetails);
                if (result != 0)
                {
                    //TempData["RefNo"] = result;
                    ViewBag.refno = result;
                }
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public ActionResult HBasicDetailsToView()
        {
            VM_BasicDetails _BasicData = _INBApplicationbll.BasicDetailsBll(Convert.ToInt64(Session["UID"]));
            if (_BasicData == null)
            {
                _BasicData = new VM_BasicDetails();
            }

            return this.PartialView("_HBasicDetails", _BasicData);
        }
        #endregion ApplicationForm end

        #region Payment Tab
        //Payment
        #region Payment

        public ActionResult Payment()
        {
            return View();
        }
        #endregion Payment


        public long GenerateChallan(int amount)
        {
            //int challanNo;
            //var challanId = 0;

            var challanDetails = _newemp.AddChallanDetails(amount);
            try
            {


                //string msg = "ನಿಮ್ಮ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ "+ ApprefNo + " ರ ಪ್ರಾರಂಭಿಕ ಠೇವಣಿ ರೂ"+ amount+"ಗಳಿಗೆ ಖಜಾನೆ -II ನಲ್ಲಿ ಚಲನ್‌ ಸೃಜನೆಯಾಗಿದ್ದು, ಚಲನ್ ಸಂಖ್ಯೆ"+ challanDetails.cd_referance_number + " ಆಗಿರುತ್ತದೆ."
                //            +"- ವಿಮಾ ಇಲಾಖೆ(KGID).";


                var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587385732081);
                Logger.LogMessage(TracingLevel.INFO, "GenerateChallan  1107161587385732081 " + msg);


                var ApprefNo = (from ad in _db.tbl_kgid_application_details where ad.kad_application_id == challanDetails.cd_application_id select ad.kad_kgid_application_number).FirstOrDefault();
                var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == challanDetails.cd_system_emp_code select eb.mobile_number).FirstOrDefault();

                msg = msg.Replace("#var1#", ApprefNo);
                msg = msg.Replace("#var2#", amount.ToString());
                msg = msg.Replace("#var3#", challanDetails.cd_referance_number.ToString());

                Logger.LogMessage(TracingLevel.INFO, "GenerateChallan SMS msg" + msg);
                Logger.LogMessage(TracingLevel.INFO, "GenerateChallan mobile number" + mobile.ToString());

                //AllCommon.sendUnicodeSMS(Session["MobileNo"].ToString(), msg, "1107161587385732081");
                Logger.LogMessage(TracingLevel.INFO, "GenerateChallan SMS sent" + mobile.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "GenerateChallan" + ex.Message.ToString());
            }

            return challanDetails.cd_id;
        }

        [HttpPost]
        public ActionResult AddChallanDetails(DateTime challanDate, int amount = 0, long challanNo = 0)
        {
            tbl_challan_details chaDetails = new tbl_challan_details();
            tbl_payment_status_details paymentStatusDetails = new tbl_payment_status_details();
            if (challanNo > 0)
            {

                chaDetails.cd_amount = amount;
                chaDetails.cd_challan_ref_no = challanNo.ToString();
                chaDetails.cd_datetime_of_challan = challanDate;
                chaDetails.cd_id = challanNo;

                _newemp.UpdateChallanDetails(chaDetails);

                paymentStatusDetails.psd_challan_ref_no = challanNo.ToString();
                paymentStatusDetails.psd_datetime = challanDate;
                paymentStatusDetails.psd_transaction_number = "113001150541";

                _newemp.AddPaymentStatus(paymentStatusDetails);
            }

            return Json(chaDetails, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult PrintChallanDetails(long EmpId, long AppId, long challanNo = 0)
        //{
        //    long EID = Convert.ToInt64(Session["UID"]);
        //    if (EID == EmpId)
        //    {
        //        //VM_ChallanPrintDetails NBChallanDetails1 = new VM_ChallanPrintDetails();
        //        //NBChallanDetails = _newemp.ChallanprintDetailsBLL(EmpId, AppId);
        //        Task<VM_ChallanPrintDetails> NBChallanDetails = Task<VM_ChallanPrintDetails>.Factory.StartNew(() => _newemp.ChallanprintDetailsBLL(EmpId, AppId));
        //        VM_ChallanPrintDetails NBChallanDetails1 = NBChallanDetails.Result;
        //        Task<string> filepath1 = Task<string>.Factory.StartNew(() => FillNBChallan(NBChallanDetails1, AppId));
        //        string filepath = filepath1.Result;
        //        return File(filepath, "application/pdf");
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //    //return NBChallanDetails;
        //}

        public ActionResult PrintChallanDetails(long EmpId, long AppId, long challanNo = 0)
        {
            long EID = Convert.ToInt64(Session["UID"]);
            if (EID == EmpId)
            {
                VM_ChallanPrintDetails NBChallanDetails = new VM_ChallanPrintDetails();
                NBChallanDetails = _newemp.ChallanprintDetailsBLL(EmpId, AppId);
                string filepath = FillNBChallan(NBChallanDetails, AppId);
                //return View("FacingSheet", facingSheet1);
                return File(filepath, "application/pdf");
            }
            else
            {
                return View();
            }
            //return NBChallanDetails;
        }

        #region NB Challan Print
        private string FillNBChallan(VM_ChallanPrintDetails NBChallanDetails, long result)
        {
            //DateTime dt = DateTime.Now;
            string amtwords = ConvertToWords(Convert.ToString(NBChallanDetails.p_premium));
            NBChallanDetails.Category = "Government";
            NBChallanDetails.GrandTotal = Convert.ToString(NBChallanDetails.p_premium);
            //KG MONTH YEAR 8011 01-8digits
            //string my = dt.ToString("MMyy");
            //string timestamp = dt.ToString("hhmmssff");
            string challanrefno = NBChallanDetails.challan_ref_no;

            //NBChallanDetails.challan_ref_no = challanrefno;
            ////////////////////////////////////////////////////////////////
            string pdfTemplate = Server.MapPath("~/Challans/NB/Challan_NB_Test.pdf");
            //string newFile = Server.MapPath("~/Challans/NB/" + challanrefno + ".pdf");
            //string newFile = @"C:/Documents/Challans/NB/" + challanrefno + ".pdf";
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"Challans\NB\" + challanrefno + ".pdf";
            }
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            {//Facing Sheet Details
             //var date1 = facingSheet.DateOfIssue?.ToString("dd-MM-yyyy");

                fields.SetField("ChallanValidity", NBChallanDetails.Challan_Validity);
                fields.SetField("District", NBChallanDetails.dm_name_english.ToString().Trim().ToUpper());
                fields.SetField("Department", NBChallanDetails.dm_deptname_english);
                fields.SetField("DDOOffice", NBChallanDetails.dm_ddo_office);
                fields.SetField("Category", NBChallanDetails.Category);
                //fields.SetField("Date", NBChallanDetails.challan_date.ToString().Trim().ToUpper());
                fields.SetField("Date", NBChallanDetails.challan_date);
                fields.SetField("ChallanReferenceNo", NBChallanDetails.challan_ref_no);
                fields.SetField("ChallanStatus", NBChallanDetails.challan_status);
                fields.SetField("DDOCode", NBChallanDetails.dm_ddo_code);
                fields.SetField("RemitterName", NBChallanDetails.employee_name.ToString().Trim().ToUpper());
                fields.SetField("MobileNo", NBChallanDetails.mobile_number.ToString().Trim().ToUpper());
                fields.SetField("Address", NBChallanDetails.ead_address);

                fields.SetField("Purpose", NBChallanDetails.purpose_desc);
                fields.SetField("HOA", NBChallanDetails.hoa_desc);
                fields.SetField("SubPurposeName", NBChallanDetails.sub_purpose_desc);
                fields.SetField("PurposeSpecificID", NBChallanDetails.purpose_id);
                fields.SetField("Amount", NBChallanDetails.p_premium.ToString().Trim().ToUpper());
                fields.SetField("RemittanceBank", NBChallanDetails.RemittanceBank);
                fields.SetField("GrandTotal", NBChallanDetails.GrandTotal);
                fields.SetField("TotalAmountinWords", amtwords);
                //fields.SetField("ChequeDDNo", NBChallanDetails.Cheque_DD_No);
                //fields.SetField("ChequeDDBank", NBChallanDetails.Cheque_DD_Bank);
                //fields.SetField("IFSCode", NBChallanDetails.IFSC_Code);
                //fields.SetField("MICRCode", NBChallanDetails.MICR_Code);
                //fields.SetField("ChequeDDDate", NBChallanDetails.Cheque_DD_Date.ToString().Trim().ToUpper());
                //fields.SetField("ChequeDDDate", NBChallanDetails.Cheque_DD_Date?.ToString("dd-MM-yyyy"));
            }
            pdfStamper.Close();
            return newFile;
        }
        #endregion
        [ValidateInput(false)]
        public ActionResult PrintChallanDetails_old(long challanNo = 0)
        {
            var challanDetails = new tbl_challan_details();
            if (challanNo > 0)
            {
                challanDetails = _newemp.FindChallanDetailsById(challanNo);

            }


            string data = @"<div id=""Grid"">
        <div style="" text-align:center;display:inline-block;padding:15px;""> 

            <table cellpadding="" 5"" cellspacing="" 0"" style="" border: none;font-size: 9pt;text-align:center;width:100%;"">
            <tr><td></tdstyle="" width:120px;height:100px""></td>  <td style="" width:120px;height:100px"">[imagelogo]</td><tdstyle="" width:120px;height:100px""></td></tr>
</table>

            <table cellpadding="" 5"" cellspacing="" 0"" style="" border: 1px solid #ccc;font-size: 9pt;text-align:center;width:100%;"">
                <tr>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">District</th>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">Department Name</th>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">Treasury Name</th>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">DDO Office</th>

                </tr>

                <tr>
                    <td style="" width:120px;border: 1px solid #ccc"">[District]</td>
                    <td style="" width:120px;border: 1px solid #ccc"">[DepartmentName]</td>
                    <td style="" width:120px;border: 1px solid #ccc"">[TreasuryName]</td>

                    <td style="" width:120px;border: 1px solid #ccc"">[DDOOffice]</td>
                </tr>
                <tr>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">Category</th>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">Date</th>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">Reference Number</th>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">DDO Code</th>

                </tr>
                <tr>
                    <td style="" width:120px;border: 1px solid #ccc"">[Category]</td>
                    <td style="" width:120px;border: 1px solid #ccc"">[Date]</td>
                    <td style="" width:120px;border: 1px solid #ccc"">[ReferenceNumber]</td>

                    <td style="" width:120px;border: 1px solid #ccc"">[DDOCode]</td>
                </tr>
                <tr>
                    <th colspan=""1"" style="" background-color: #B8DBFD;border: 1px solid #ccc"">Remitter Name</th>
                    <td colspan=""3"" style="" width:120px;border: 1px solid #ccc"">[RemitterName]</td>

                </tr>
                <tr>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">Purpose</th>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">HOA</th>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">Purpose Specific Id</th>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">Amount</th>

                </tr>
                <tr>
                    <td style="" width:120px;border: 1px solid #ccc"">[Purpose]</td>
                    <td style="" width:120px;border: 1px solid #ccc"">[HOA]</td>
                    <td style="" width:120px;border: 1px solid #ccc"">[PurposeSpecificId]</td>

                    <td style="" width:120px;border: 1px solid #ccc"">[Amount]</td>
                </tr>
                <tr>
                    <td style="" width:120px;border: 1px solid #ccc""></td>
                    <td style="" width:120px;border: 1px solid #ccc""></td>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">Total Amount</th>
                    <td style="" width:120px;border: 1px solid #ccc"">[TotalAmount]</td>
                </tr>
                <tr>
                    <td style="" width:120px;border: 1px solid #ccc""></td>
                    <td style="" width:120px;border: 1px solid #ccc""></td>
                    <th style="" background-color: #B8DBFD;border: 1px solid #ccc"">Total Amount in Words</th>
                    <td style="" width:120px;border: 1px solid #ccc"">[TotalAmountInWords]</td>
                </tr>
                <tr>

                    <th colspan=""4"" style="" background-color: #B8DBFD;border: 1px solid #ccc"">Payment Details</th>
                </tr>
                <tr>
                   
                    <th colspan=""1"">Payment Mode</th>
                    <td colspan=""3"" style="" width:120px;border: 1px solid #ccc"">[PaymentMode]</td>
                </tr>
            </table>
        </div>
    </div>";
            int num = challanDetails.cd_amount ?? 0;
            string result;
            ConvertNumberToText(num, out result);

            data = data.Replace("[District]", "Bangaluru Urban");
            data = data.Replace("[DepartmentName]", "Karnataka Government Insurance Department");
            data = data.Replace("[TreasuryName]", "Bangalore Urban");
            data = data.Replace("[DDOOffice]", "Karnataka Government Insurance Department, Bangalore Urban");
            data = data.Replace("[Category]", "Government");
            data = data.Replace("[Date]", challanDetails.cd_datetime_of_challan.ToString());
            data = data.Replace("[ReferenceNumber]", "113001150541");
            data = data.Replace("[DDOCode]", "12027O");
            data = data.Replace("[RemitterName]", challanDetails.cd_system_emp_code.ToString());
            data = data.Replace("[Purpose]", "KGID Premium");
            data = data.Replace("[HOA]", "Revenue Head of Account");
            data = data.Replace("[PurposeSpecificId]", "00545");
            data = data.Replace("[Amount]", challanDetails.cd_amount.ToString());
            data = data.Replace("[TotalAmount]", challanDetails.cd_amount.ToString());
            data = data.Replace("[TotalAmountInWords]", result);
            data = data.Replace("[PaymentMode]", "Cash");

            data = data.Replace("[imagelogo]", "<img style='width:100px;height:100px;' src='http://localhost:52373/Content/Images/KGID-KA.png' />");

            HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
            HtmlDocument doc = new HtmlDocument();
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(data);
            data = doc.DocumentNode.OuterHtml;
            try
            {
                if (data != "")
                {
                    using (MemoryStream stream = new System.IO.MemoryStream())
                    {
                        StringReader sr = new StringReader(data);
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 100f, 0f);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        //string imageURL = Server.MapPath(".") + "/KGID-KA.png";
                        //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        pdfDoc.Close();
                        var fileName = "Applicant" + challanDetails.cd_challan_ref_no + ".pdf";
                        return File(stream.ToArray(), "application/pdf", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;

        }

        static bool HelperConvertNumberToText(int num, out string buf)

        {

            string[] strones = {

            "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight",

            "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen",

            "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen",

          };



            string[] strtens = {

              "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty",

              "Seventy", "Eighty", "Ninety", "Hundred"

          };



            string result = "";

            buf = "";

            int single, tens, hundreds;



            if (num > 1000)

                return false;



            hundreds = num / 100;

            num = num - hundreds * 100;

            if (num < 20)

            {

                tens = 0; // special case

                single = num;

            }

            else

            {

                tens = num / 10;

                num = num - tens * 10;

                single = num;

            }



            result = "";



            if (hundreds > 0)

            {

                result += strones[hundreds - 1];

                result += " Hundred ";

            }

            if (tens > 0)

            {

                result += strtens[tens - 1];

                result += " ";

            }

            if (single > 0)

            {

                result += strones[single - 1];

                result += " ";

            }



            buf = result;

            return true;

        }

        static bool ConvertNumberToText(int num, out string result)

        {

            string tempString = "";

            int thousands;

            int temp;

            result = "";

            if (num < 0 || num > 100000)

            {

                System.Console.WriteLine(num + " \tNot Supported");

                return false;

            }



            if (num == 0)

            {

                System.Console.WriteLine(num + " \tZero");

                return false;

            }



            if (num < 1000)

            {

                HelperConvertNumberToText(num, out tempString);

                result += tempString;

            }

            else

            {

                thousands = num / 1000;

                temp = num - thousands * 1000;

                HelperConvertNumberToText(thousands, out tempString);

                result += tempString;

                result += "Thousand ";

                HelperConvertNumberToText(temp, out tempString);

                result += tempString;

            }

            return true;

        }

        public int InsertChallanDetails(tbl_challan_details objChalnDetails)
        {
            //int result = _INBApplicationbll.SaveNBPayscaleBll(objKGIDDetails);

            //if(objChalnDetails.cd_amount == null)
            //{
            //    objChalnDetails.cd_amount = 0;
            //}

            int result = _INBApplicationbll.SaveChallanBll(objChalnDetails);
            if (result == 1)
            {
                //TempData["ChallanDetails"] = objChalnDetails;
                ViewBag.chaldetails = objChalnDetails;
            }
            return result;
        }
        #endregion Payment Tab

        #region Medical Report
        public ActionResult HPhysicalDetailsToView()
        {
            VM_MPhysicalDetails _HPhysicalDetailsData = _INBApplicationbll.HPhysicalDetailsBll(Convert.ToInt64(Session["UID"]));
            if (_HPhysicalDetailsData == null)
            {
                _HPhysicalDetailsData = new VM_MPhysicalDetails();
            }

            return this.PartialView("_HPhysicalDetails", _HPhysicalDetailsData);
        }
        public ActionResult HOtherDetailsToView()
        {
            VM_MOtherDetails _HOtherDetailsData = _INBApplicationbll.HOtherDetailsBll(Convert.ToInt64(Session["UID"]));
            _HOtherDetailsData.employee_id = Convert.ToInt64(Session["UID"]);
            if (_HOtherDetailsData == null)
            {
                _HOtherDetailsData = new VM_MOtherDetails();
            }

            return this.PartialView("_HOtherDetails", _HOtherDetailsData);
        }
        public ActionResult HHealthDetailsToView()
        {
            VM_MOtherDetails _HHealthDetailsData = _INBApplicationbll.HHealthDetailsBll(Convert.ToInt64(Session["UID"]));
            _HHealthDetailsData.employee_id = Convert.ToInt64(Session["UID"]);
            if (string.IsNullOrEmpty(_HHealthDetailsData.GoodLifeCycleDesc) && _HHealthDetailsData.GoodLifeCycle == false)
            {
                _HHealthDetailsData.GoodLifeCycle = true;
            }
            if (_HHealthDetailsData == null)
            {
                _HHealthDetailsData = new VM_MOtherDetails();
            }

            return this.PartialView("_HHealthDetails", _HHealthDetailsData);
        }

        public ActionResult HDoctorDetailsToView()
        {
            VM_DoctorDetails _HDoctorDetailsData = new VM_DoctorDetails();
            _HDoctorDetailsData = _INBApplicationbll.GetDoctorDetails(Convert.ToInt64(Session["UID"]));

            return this.PartialView("_HDoctorDetails", _HDoctorDetailsData);
        }

        public ActionResult HDeclarationDetailsToView()
        {
            //tbl_medical_declaration _HDeclarationData = _INBApplicationbll.HDeclarationDetailsBll(Convert.ToInt64(Session["UID"]));
            //if (_HDeclarationData == null)
            //{
            //    _HDeclarationData = new tbl_medical_declaration();
            //}

            //return this.PartialView("_HDeclarationDetails", _HDeclarationData);
            return this.PartialView("_HDeclarationDetails");
        }
        [HttpGet]
        public JsonResult GetDoctorsByKMCCode(string kmcCode)
        {
            IEnumerable<SelectListItem> doctorsList = new List<SelectListItem>();

            doctorsList = _INBApplicationbll.GetDoctorsByKMCCode(kmcCode);

            return Json(doctorsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDoctorDetailByKMCCode(string docKMCCode)
        {
            VM_DoctorDetail doctorDetail = new VM_DoctorDetail();

            doctorDetail = _INBApplicationbll.GetDoctorDetailByKMCCode(docKMCCode);

            return Json(doctorDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public int InsertHPhysicalDetails(VM_MPhysicalDetails objHPhysicalDetails)
        {
            //if (Session["IsMedicalRequired"] != null)
            //{
            //    ViewBag.IsMedicalRequired = Session["IsMedicalRequired"];
            //}
            //else
            //{
            //    Session["IsMedicalRequired"] = objHPhysicalDetails.empd_ismedicalreqd;
            //    ViewBag.IsMedicalRequired = Session["IsMedicalRequired"];
            //}
            Session["IsMedicalRequired"] = true;
            Session["IsApplicationReqd"] = true;


            var result = _INBApplicationbll.SaveHPhysicalDetailsBll(objHPhysicalDetails);
            return result;
        }
        public JsonResult InsertHOtherDetails(VM_MOtherDetails objHOtherDetails)
        {
            bool isSuccess = false;
            string message = "Error saving other details";
            int response = _INBApplicationbll.SaveHOtherDetailsBll(objHOtherDetails);
            if (response > 0)
            {
                isSuccess = true;
                message = "Other details saved successfully";
            }

            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertHHealthDetails(VM_MOtherDetails objHHealthDetails)
        {
            bool isSuccess = false;
            string message = "Error saving health details";
            int response = _INBApplicationbll.SaveHHealthDetailsBll(objHHealthDetails);

            if (response > 0)
            {
                isSuccess = true;
                message = "Other details saved successfully";
            }

            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertDoctorDetails(VM_DoctorDetails objHDoctorDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            int result = _INBApplicationbll.SaveHDoctorDetailsBll(objHDoctorDetails);

            if (result != 0)
            {
                isSuccess = true;
                message = "Doctor details saved successfully";
            }

            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }
        public long InsertHDeclarationDetails(tbl_medical_declaration objDeclarationDetails)
        {
            try
            {
                var result = _INBApplicationbll.SaveHNBDeclarationBll(objDeclarationDetails);
                if (result != 0)
                {
                    //TempData["RefNo"] = result;
                    ViewBag.refno = result;
                }
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public JsonResult SubmitMedicalForm(string empId)
        {
            var result = _INBApplicationbll.SubmitMedicalForm(Convert.ToInt64(Session["UID"]));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion Medical Report End

        [Route("kgid-ddo")]
        [CustomAuthorize("DDO")]
        public ActionResult DetailsForDDOVerification()
        {
            VM_DDOVerificationDetails verificationDetails = _INBApplicationbll.GetEmployeeDetailsForDDOVerification(Convert.ToInt64(Session["UID"]));
            Thread.Sleep(2000);
            Logger.LogMessage(TracingLevel.INFO, "Pending Issues DDO " + verificationDetails.PendingApplications.ToString());
            Logger.LogMessage(TracingLevel.INFO, "Total Issues DDO " + verificationDetails.TotalReceived.ToString());
            return View("DDO", verificationDetails);
        }
        //New

        public long InsertBasicDetails(VM_BasicDetails objBasicDetails)
        {
            long result = _INBApplicationbll.SaveNBBasicBll(objBasicDetails);
            BasicDetailsToView();
            Session["AppRefNumber"] = result;
            return result;
        }
        #region UploadEdit
        #region ADD DETAILS ENTRY BY DDO
        public ActionResult AddEmployeeDetails()
        {
            //VM_BasicDetails employeeDetails = _uploadbll.GetEmployeeDetailsById(Convert.ToInt64(Session["UID"]));

            VM_BasicDetails employeeDetails = _uploadbll.GetDDODetailsById(Convert.ToInt64(Session["UID"]));
            return View(employeeDetails);
        }
        public JsonResult AddEmployeeBasicDetails(VM_BasicDetails employeeDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            employeeDetails.created_by = Convert.ToInt32(Session["UID"]);
            //string result = _INBApplicationbll.SaveEmployeeBasicDetails(employeeDetails);
            //employeeDetails.date_of_appointment = _commnobj.DateConversion(employeeDetails.date_of_appointment);
            //employeeDetails.date_of_birth = _commnobj.DateConversion(employeeDetails.date_of_birth);
            //employeeDetails.ewd_date_of_joining = _commnobj.DateConversion(employeeDetails.ewd_date_of_joining);
            string result = _INBApplicationbll.AddEmployeeBasicDetails(employeeDetails);

            if (result != "2" && result != "1")
            {
                string[] duplicate = result.Split(',');

                if (duplicate.Length > 0)
                {
                    if (duplicate.Contains("Mobile number") || duplicate.Contains("Pan number") || duplicate.Contains("Email id"))
                    {
                        result = result.TrimEnd(',');
                        message = "Provided " + result + " for the employee code is already registerd.";
                    }

                    isSuccess = false;
                }
            }

            else
            {

                try
                {
                    var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587991448971);

                    Logger.LogMessage(TracingLevel.INFO, "AddEmployeeBasicDetails 1107161587991448971" + msg.ToString());
                    Logger.LogMessage(TracingLevel.INFO, "AddEmployeeBasicDetails mobile no " + employeeDetails.mobile_number.ToString());

                    //AllCommon.sendUnicodeSMS(employeeDetails.mobile_number.ToString(), msg, "1107161587991448971");
                    Logger.LogMessage(TracingLevel.INFO, "AddEmployeeBasicDetails SMS sent ");
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "AddEmployeeBasicDetails" + ex.Message.ToString());
                }
                isSuccess = true;
                message = "Employee details saved successfully";
            }
            //else
            //{
            //    isSuccess = false;
            //    message = "Provided Mobile number/Pan number/Email id for the employee code is already registerd.";
            //}

            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult EditUploadEmployeeDetails(string empId)
        {
            VM_BasicDetails employeeDetails = _uploadbll.GetEmployeeDetailsById(Convert.ToInt64(empId));
            return PartialView("_EmployeeDetails", employeeDetails);
        }
        public JsonResult SaveEmployeeBasicDetails(VM_BasicDetails employeeDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            //employeeDetails.date_of_appointment = _commnobj.DateConversion(employeeDetails.date_of_appointment);
            //employeeDetails.date_of_birth = _commnobj.DateConversion(employeeDetails.date_of_birth);
            //employeeDetails.ewd_date_of_joining = _commnobj.DateConversion(employeeDetails.ewd_date_of_joining);
            string result = _INBApplicationbll.SaveEmployeeBasicDetails(employeeDetails);

            if (result != "2" && result != "1")
            {
                string[] duplicate = result.Split(',');

                if (duplicate.Length > 0)
                {
                    if (duplicate.Contains("Mobile number") || duplicate.Contains("Pan number") || duplicate.Contains("Email id"))
                    {
                        result = result.TrimEnd(',');
                        message = "Provided " + result + " for the employee code is already registerd.";
                    }

                    isSuccess = false;
                }
            }

            else
            {
                isSuccess = true;
                message = "Fields updated successfully";
            }
            //else
            //{
            //    isSuccess = false;
            //    message = "Provided Mobile number/Pan number/Email id for the employee code is already registerd.";
            //}

            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PersonalHealth
        public ActionResult PersonalDetailsToView()
        {
            VM_PersonalHealthDetails _PersonalData = _INBApplicationbll.PersonalDetailsBll(Convert.ToInt64(Session["UID"]));
            if (_PersonalData == null)
            {
                _PersonalData = new VM_PersonalHealthDetails();
            }

            return this.PartialView("_PersonalDetails", _PersonalData);
        }
        public int InsertPersonalDetails(VM_PersonalHealthDetails objPersonalDetails)
        {
            if (Session["UID"] != null)
            {
                objPersonalDetails.ephd_emp_id = Convert.ToInt64(Session["UID"]);
            }
            if (objPersonalDetails.ephd_emp_id != null && objPersonalDetails.ephd_application_id != null)
            {
                try
                {
                    Logger.LogMessage(TracingLevel.INFO, "InsertPersonalDetails Enter");
                    var result = _INBApplicationbll.SaveNBPersonalBll(objPersonalDetails);
                    //TempData["Personal"] = objPersonalDetails;
                    ViewBag.Personal = objPersonalDetails;
                    return result;
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "InsertPersonalDetails >" + ex.Message + "--Inner Exception--" + ex.InnerException + "--StackTrace--" + ex.StackTrace);
                    return 0;
                }
            }
            else
            {
                return 0;
            }

            //var result = _INBApplicationbll.SaveNBPersonalBll(objPersonalDetails);
            //TempData["Personal"] = objPersonalDetails;
            //return result;
        }
        #endregion

        #region MedicalLeave Details
        public ActionResult MedicalLeaveDetailsToView()
        {
            VM_MedicalLeaveDetails _MedicalLeaveData = _INBApplicationbll.MedicalLeaveDetailsBll(Convert.ToInt64(Session["UID"]), "");
            if (_MedicalLeaveData == null)
            {
                _MedicalLeaveData = new VM_MedicalLeaveDetails();
            }

            return PartialView("_MedicalLeaveDetails", _MedicalLeaveData);
        }

        public ActionResult DDOMedicalLeaveDetails()
        {
            long empid = 0;//Convert.ToInt64(RouteData.Values["empId"]);
            if (Session["RUID"] != null)
            {
                empid = Convert.ToInt64(Session["RUID"]);
                Session["DDOVerifiedEmp"] = empid;
            }
            if (empid != 0)
            {
                VM_MedicalLeaveDetails _MedicalLeaveData = _INBApplicationbll.MedicalLeaveDetailsBll(empid, "DDO");
                if (_MedicalLeaveData == null)
                {
                    _MedicalLeaveData = new VM_MedicalLeaveDetails();
                }
                return PartialView("_MedicalLeaveDetails", _MedicalLeaveData);
            }
            else
            {
                return RedirectToAction("DetailsForDDOVerification", "Employee");
            }
        }

        public ActionResult OtherLoginMedicalLeaveDetails()
        {
            long empid = 0;//Convert.ToInt64(RouteData.Values["empId"]);
            if (Session["RUID"] != null)
            {
                empid = Convert.ToInt64(Session["RUID"]);
                Session["DDOVerifiedEmp"] = empid;
            }
            VM_MedicalLeaveDetails _MedicalLeaveData = _INBApplicationbll.MedicalLeaveDetailsBll(empid, "Other");
            if (_MedicalLeaveData == null)
            {
                _MedicalLeaveData = new VM_MedicalLeaveDetails();
            }
            return PartialView("_MedicalLeaveDetails", _MedicalLeaveData);
        }

        public JsonResult InsertMedicalLeaveDetails(VM_MedicalLeaveDetails MedicalLeaveData)
        {
            VM_MedicalLeaveDetails result = _INBApplicationbll.SaveNBMedicalLeaveBll(MedicalLeaveData);
            return Json(result, JsonRequestBehavior.AllowGet);
            //return result;
        }
        public string UploadMedicalLeaveDocument(MedicalLeaveData MedicalLeaveDetails)
        {
            if (Session["UID"] != null)
            {
                if (Convert.ToInt64(Session["UID"]) == MedicalLeaveDetails.emld_emp_id)
                    MedicalLeaveDetails.emld_emp_id = Convert.ToInt64(Session["UID"]);
                else
                    if (Session["DDOVerifiedEmp"] != null)
                    MedicalLeaveDetails.emld_emp_id = Convert.ToInt64(Session["DDOVerifiedEmp"]);
            }
            string result = _INBApplicationbll.UploadMedicalLeaveDoc(MedicalLeaveDetails);
            return result;
        }
        public int DeleteMedicalLeaveDetails(long EmpCode)
        {
            int result = _INBApplicationbll.DeleteMedicalLeaveBll(EmpCode);
            return result;
        }
        public int SaveDDOMedicalLeave(MedicalLeaveData MedicalLeaveData)
        {
            int result = _INBApplicationbll.SaveDDOMedicalLeaveBll(MedicalLeaveData);
            return result;
        }
        #endregion
        #region Print MedicalLeave Details
        //Print Medical Details
        public ActionResult PrintMedicalDetails(string AppRefNo, string ProposerName, string Address, string Pincode, string Phone, string JoiningDate, string Designation, string WorkingPlace)
        {
            bool isSuccess = false;
            string message = string.Empty;
            string result = string.Empty;
            VM_BasicDetails MedicalDetails = new VM_BasicDetails();
            MedicalDetails.kad_kgid_application_number = AppRefNo.ToString().Trim().ToUpper();
            MedicalDetails.employee_name = ProposerName.ToString().Trim().ToUpper();
            MedicalDetails.ead_address = Address.ToString().Trim().ToUpper();
            MedicalDetails.ead_pincode = Convert.ToInt32(Pincode);
            MedicalDetails.mobile_number = Convert.ToInt64(Phone);
            MedicalDetails.ewd_date_of_joining = JoiningDate.ToString().Trim().ToUpper();
            MedicalDetails.d_designation_desc = Designation.ToString().Trim().ToUpper();
            MedicalDetails.ewd_place_of_posting = WorkingPlace.ToString().Trim().ToUpper();
            if (MedicalDetails.employee_name != "")
            {
                isSuccess = true;
                message = "Medical Details Successfully Generated";
            }
            string filepath = FillMED(MedicalDetails, AppRefNo);
            byte[] pdfByteArray = System.IO.File.ReadAllBytes(filepath);
            string base64EncodedPDF = System.Convert.ToBase64String(pdfByteArray);
            return Json(new { IsSuccess = isSuccess, Message = message, Result = base64EncodedPDF }, JsonRequestBehavior.AllowGet);
        }
        //Fill Medical Print Details
        private string FillMED(VM_BasicDetails MedicalDetails, string AppRefNo)
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToShortDateString();
            string pdfTemplate = Server.MapPath("~/PdfTemplate/MedicalExaminationReport/MER_Template_Kannada.pdf");
            string newFile = Server.MapPath("~/PdfTemplate/MedicalExaminationReport/" + "MER_" + AppRefNo + ".pdf");
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            string path = Server.MapPath("~/PdfTemplate/MedicalExaminationReport/" + MedicalDetails.kad_kgid_application_number + ".png");
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(MedicalDetails.kad_kgid_application_number, QRCodeGenerator.ECCLevel.Q);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        byte[] bytes = ms.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    //applicationDetails.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
            PushbuttonField ad = fields.GetNewPushbuttonFromField("MER_QR");
            if (ad != null)
            {
                ad.Layout = PushbuttonField.LAYOUT_ICON_ONLY;
                ad.ProportionalIcon = true;
                ad.Image = iTextSharp.text.Image.GetInstance(path);
                fields.ReplacePushbuttonField("MER_QR", ad.Field);
            }

            {//Medical Examination Report Details 
                fields.SetField("ProposerName", MedicalDetails.employee_name);
                fields.SetField("Address", MedicalDetails.ead_address);
                fields.SetField("Pincode", MedicalDetails.ead_pincode.ToString());
                fields.SetField("JoiningDate", MedicalDetails.ewd_date_of_joining);
                fields.SetField("ApplicationRefNo", MedicalDetails.kad_kgid_application_number.ToString());
                fields.SetField("PhoneNo", MedicalDetails.mobile_number.ToString());
                fields.SetFieldProperty("PresentWorkingOffice", "textsize", 10f, null);
                fields.SetField("PresentWorkingOffice", MedicalDetails.ewd_place_of_posting);
                fields.SetField("PresentDesignation", MedicalDetails.d_designation_desc);
                //fields.SetField("JoiningDate", MedicalDetails.ewd_date_of_joining?.ToString("dd-MM-yyyy"));
            }

            pdfStamper.Close();
            return newFile;
        }
        #endregion
        #region FamilyDetails

        public ActionResult FamilyDetailsToView()
        {
            VM_FamilyDetails _FamilyData = _INBApplicationbll.FamilyDetailsBll(Convert.ToInt64(Session["UID"]));
            if (_FamilyData == null)
            {
                _FamilyData = new VM_FamilyDetails();
            }

            return this.PartialView("_FamilyDetails", _FamilyData);
        }

        public int InsertFamilyDetails(VM_FamilyDetails objFamilyDetails)
        {
            var result = _INBApplicationbll.SaveNBFamilyBll(objFamilyDetails);
            // TempData["Family"] = objFamilyDetails;
            ViewBag.Emp = objFamilyDetails;

            return result;
        }
        public int CheckFamilyMemberDetails(long rowNum)
        {
            var result = _INBApplicationbll.CheckFamilyMemberDetailsBll(rowNum);
            return result;
        }

        public int DeleteFamilyDetails(long LeaveID)
        {
            var result = _INBApplicationbll.DeleteNBFamilyBll(LeaveID);
            return result;
        }
        #endregion
        #region DDO Status Update
        public long UpdateDdoUploadStatus(long empid)
        {
            try
            {
                var result = _INBApplicationbll.SaveNBDdoUploadStatusBll(empid);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public JsonResult GetNomineeNameList(long empId, string type)
        {
            BindDropDownModel result = new BindDropDownModel();
            result = _INBApplicationbll.GetNomineeNameListBll(empId, type);
            return Json(result, JsonRequestBehavior.AllowGet); ;
        }
        #endregion
        #region Payment
        public ActionResult PaymentDetailsToView()
        {
            VM_PaymentDetails obj = _INBApplicationbll.NBPaymentDll(Convert.ToInt64(Session["UID"]));
            return this.PartialView("_PaymentDetails", obj);
        }
        [HttpPost]
        public ActionResult InsertPaymentDetails(VM_PaymentDetails objPaymentDetails)
        {
            long result = _INBApplicationbll.SaveNBPaymentBll(objPaymentDetails);
            Session["Amount"] = objPaymentDetails.cd_amount;
            return RedirectToAction("KhajaneIIGateway", "Policy");
        }
        [HttpPost]
        public long InsertChalanStatusDetails(VM_PaymentDetails objPaymentDetails)
        {
            long result = _INBApplicationbll.SaveNBChallanStatusDll(objPaymentDetails);
            return result;
        }
        public JsonResult PaymentDetailsToDownload()
        {
            VM_PaymentDetails obj = _INBApplicationbll.NBPaymentDownloadDll(Convert.ToInt64(Session["UID"]));
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintPaymentDetails(long EmpId, long AppId, long challanNo = 0)
        {
            Session["IsApplicationReqd"] = true;
            long EID = Convert.ToInt64(Session["UID"]);
            if (EID == EmpId)
            {

                VM_ChallanPrintDetails NBChallanDetails = new VM_ChallanPrintDetails();
                NBChallanDetails = _newemp.ChallanprintDetailsBLL(EmpId, AppId);
                string filepath = FillNBChallan(NBChallanDetails, AppId);

                byte[] pdfByteArray = System.IO.File.ReadAllBytes(filepath);
                string base64EncodedPDF = System.Convert.ToBase64String(pdfByteArray);
                return Json(new { Result = base64EncodedPDF }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Change role
        public ActionResult AddUserRole()
        {
            return View();
        }
        public JsonResult GetEmpDetails(string kgidnum)
        {
            VM_BasicDetails empDetail = _newemp.GetEmployeeData(kgidnum);
            return Json(new { EmpDetail = empDetail }, JsonRequestBehavior.AllowGet);
        }
        public int SaveUserRole(long kgidnum, int ddo, int cw, int avgcw, int sup, int dio, int dd, int d,int AD, int nb, int loan, int claims, int motor, int odclaims, int mvcclaims)
        {
            string empId = Convert.ToString(Session["UID"]);
            if (empId != null && empId != "")
            {
                int result = _newemp.SaveCategoryDetailsBll(kgidnum, ddo, cw, avgcw, sup, dio, dd, d, AD, nb, loan, claims, motor, odclaims, mvcclaims, Convert.ToInt64(empId));
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion
        public int NBApplicationCancel(long AppId, long EmpId, string Comments)
        {
            int result = _INBApplicationbll.NBApplicationCancel(AppId, EmpId, Comments);
            return result;
        }
        #region Convert Number into Words
        string[] words0 = { "Zero ", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine ", "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen ", "Twenty " };
        string[] words2 = { "Zero ", "Ten ", "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety ", "Hundred " };
        string[] words3 = { "Hundred ", "Thousand ", "Lakh ", "Crore " };
        int[] numbers = new int[] { 0, 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };
        string numstr;
        string words = "";
        int tempNum;
        int temp = 0;
        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric    
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping    
                    String place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits)
                    {
                        case 1://ones' range    

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range    
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range    
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range    
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        private String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents    
                        endStr = "Paisa " + endStr;//Cents    
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertToRupee(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }
        private string ConvertToRupee(string number)
        {
            numstr = number.ToString();
            words = "";
            tempNum = Convert.ToInt32(number);
            temp = 0;
            while (numstr != "0" && numstr.Length != 0)
            {
                switch (numstr.Length)
                {
                    case 1:
                        words += words0[tempNum];
                        numstr = "";
                        break;
                    case 2:
                        if (tempNum <= 20)
                        {
                            words += words0[tempNum];
                            numstr = "";
                        }
                        else
                        {
                            temp = tempNum / numbers[2];
                            words += words2[temp];
                            tempNum = tempNum % numbers[2];
                            numstr = tempNum.ToString();
                        }
                        break;
                    case 3:
                        Method1(3, "Hundred ");
                        break;
                    case 4:
                        Method1(4, "Thousand ");
                        break;
                    case 5:
                        Method2(4, "Thousand ");
                        break;
                    case 6:
                        Method1(6, "Lakh ");
                        break;
                    case 7:
                        Method2(6, "Lakh ");
                        break;
                    case 8:
                        Method1(8, "Crore ");
                        break;
                    case 9:
                        Method2(8, "Crore ");
                        break;
                    default:
                        break;
                }
            }
            words += "Rupees";
            return words;
        }
        private void Method1(int n, string wo)
        {
            temp = tempNum / numbers[n];
            words += words0[temp] + wo;
            tempNum = tempNum % numbers[n];
            numstr = tempNum.ToString();
        }
        private void Method2(int n, string wo)
        {
            temp = tempNum / numbers[n];
            if (temp == 10)
                words += words0[temp] + wo;
            else if (temp <= 20)
                words += words0[temp] + wo;
            else
            {
                int twoDig = temp / numbers[2];
                int digit = temp % numbers[2];
                words += words2[twoDig] + words0[digit] + wo;
            }
            tempNum = tempNum % numbers[n];
            numstr = tempNum.ToString();
        }
        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }

        public string DSCSignIn(string Kgidno, string PublicKey, VM_DeptVerificationDetails VerifiedDetails)
        {
            long empid = Convert.ToInt64(Session["UID"]);
            string PdfContentFile = string.Empty;
            int LoadFactorID = 0;
            int DeductionLoadFactorID = 0;
            try
            {
                var res = (from n in _db.tbl_policy_details where n.p_application_id == VerifiedDetails.ApplicationId && n.p_emp_id == VerifiedDetails.EmpCode select n.p_premium).FirstOrDefault();
                VerifiedDetails.PremiumAmount = Convert.ToDecimal(res.Value);
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
            try
            {
                var res = (from n in _db.tbl_load_factor_master where n.lfm_load_factor_desc == VerifiedDetails.LoadFactor select n.lfm_load_factor_id).FirstOrDefault();
                LoadFactorID = Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
            try
            {
                var res = (from n in _db.tbl_dl_factor_master where n.dlfm_dl_factor_desc == VerifiedDetails.DeductionLoadFactor select n.dlfm_dl_factor_id).FirstOrDefault();
                DeductionLoadFactorID = Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
            //,(SELECT lfm_load_factor_id FROM tbl_load_factor_master WHERE lfm_load_factor_desc = @load_factor)            
            //,(SELECT dlfm_dl_factor_id FROM tbl_dl_factor_master WHERE dlfm_dl_factor_desc = @dl_factor)     
            //SELECT p_premium FROM tbl_policy_details WHERE p_application_id = @kawt_application_id AND p_emp_id = @employee_id AND p_active_status = 1
            string result = _INBApplicationbll.DSCLogin(empid, PublicKey);

            if (result == "SUCCESS")
            {
                PdfContentFile = CreatePDF(VerifiedDetails.ApplicationId, VerifiedDetails.PremiumAmount, VerifiedDetails.SumAssured, LoadFactorID, DeductionLoadFactorID, VerifiedDetails.EmpCode);
            }

            if (result == "SUCCESS")
            {
                string SignedPdf = string.Empty;
                if (PdfContentFile != null && PdfContentFile != "")
                {
                    SignedPdf = SignPdf(PdfContentFile, VerifiedDetails.ApplicationId, PublicKey);
                }
                if (SignedPdf != null)
                {
                    FileInfo file = new FileInfo(PdfContentFile);
                    file.Delete();
                    tbl_dsc_details objPD = new tbl_dsc_details();
                    // fields to be insert
                    objPD.dd_emp_id = VerifiedDetails.EmpCode;
                    objPD.dd_kgid_workflow_id = VerifiedDetails.ApplicationId;
                    objPD.dd_premium_amount = VerifiedDetails.PremiumAmount;
                    objPD.dd_sum_assured = VerifiedDetails.SumAssured;
                    objPD.dd_dl_factor_id = LoadFactorID;
                    objPD.dd_load_factor_id = DeductionLoadFactorID;
                    objPD.dd_dsc_path = SignedPdf;
                    objPD.dd_active_status = true;
                    objPD.dd_created_by = empid;
                    objPD.dd_creation_datetime = DateTime.Now;
                    objPD.dd_updated_by = empid;
                    objPD.dd_updation_datetime = DateTime.Now;
                    _db.tbl_dsc_details.Add(objPD);
                    _db.SaveChanges();
                    return result;
                }
                else
                {
                    //FileInfo file = new FileInfo(PdfContentFile);
                    //file.Delete();
                    return "FAILURE";
                }
            }
            return result;
        }
        public ActionResult DSCSignInKGID(string Kgidno, string PublicKey, VM_DeptVerificationDetails VerifiedDetails)
        {
            long empid = Convert.ToInt64(Session["UID"]);
            string PdfContentFile = string.Empty;
            int LoadFactorID = 0;
            int DeductionLoadFactorID = 0;
            try
            {
                var res = (from n in _db.tbl_policy_details where n.p_application_id == VerifiedDetails.ApplicationId && n.p_emp_id == VerifiedDetails.EmpCode select n.p_premium).FirstOrDefault();
                VerifiedDetails.PremiumAmount = Convert.ToDecimal(res.Value);
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
            try
            {
                var res = (from n in _db.tbl_load_factor_master where n.lfm_load_factor_desc == VerifiedDetails.LoadFactor select n.lfm_load_factor_id).FirstOrDefault();
                LoadFactorID = Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
            try
            {
                var res = (from n in _db.tbl_dl_factor_master where n.dlfm_dl_factor_desc == VerifiedDetails.DeductionLoadFactor select n.dlfm_dl_factor_id).FirstOrDefault();
                DeductionLoadFactorID = Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
            //,(SELECT lfm_load_factor_id FROM tbl_load_factor_master WHERE lfm_load_factor_desc = @load_factor)            
            //,(SELECT dlfm_dl_factor_id FROM tbl_dl_factor_master WHERE dlfm_dl_factor_desc = @dl_factor)     
            //SELECT p_premium FROM tbl_policy_details WHERE p_application_id = @kawt_application_id AND p_emp_id = @employee_id AND p_active_status = 1
            string result = _INBApplicationbll.DSCLogin(empid, PublicKey);
            bool isSuccess = false;
            string message = string.Empty;
            string filename = PdfContentFile;
            if (result == "SUCCESS")
            {
                PdfContentFile = CreatePDF(VerifiedDetails.ApplicationId, VerifiedDetails.PremiumAmount, VerifiedDetails.SumAssured, LoadFactorID, DeductionLoadFactorID, VerifiedDetails.EmpCode);
                ////
                if (PdfContentFile != null)
                {
                    //FileInfo file = new FileInfo(PdfContentFile);
                    //file.Delete();
                    tbl_dsc_details objPD = new tbl_dsc_details();
                    // fields to be insert
                    objPD.dd_emp_id = VerifiedDetails.EmpCode;
                    objPD.dd_kgid_workflow_id = VerifiedDetails.ApplicationId;
                    objPD.dd_premium_amount = VerifiedDetails.PremiumAmount;
                    objPD.dd_sum_assured = VerifiedDetails.SumAssured;
                    objPD.dd_dl_factor_id = LoadFactorID;
                    objPD.dd_load_factor_id = DeductionLoadFactorID;
                    objPD.dd_dsc_path = PdfContentFile;
                    objPD.dd_active_status = true;
                    objPD.dd_created_by = empid;
                    objPD.dd_creation_datetime = DateTime.Now;
                    objPD.dd_updated_by = empid;
                    objPD.dd_updation_datetime = DateTime.Now;
                    _db.tbl_dsc_details.Add(objPD);
                    _db.SaveChanges();

                    //PdfReader inputPdf = new PdfReader(filename);
                    ////string newFile = Server.MapPath(unsignedpdf);
                    //string newFile = Server.MapPath("~/Documents/VerifyDataPdfDocs/" + "Signed_PDF" + DateTime.Now.Ticks + ".pdf");
                    //FileStream signedPdf = new FileStream(newFile, FileMode.Create);

                    //byte[] bytes = System.IO.File.ReadAllBytes(PdfContentFile);

                    //string strBytes = Convert.ToBase64String(bytes);
                    isSuccess = true;
                    message = "Success";
                    //return Json(new { IsSuccess = isSuccess, Message = message, Data = strBytes }, JsonRequestBehavior.AllowGet);
                    return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
                }
                /////

            }

            else
            {
                result = "FAILURE";
                isSuccess = false;
                message = "Failure";
                return Json(new { IsSuccess = isSuccess, Message = message, Data = result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsSuccess = isSuccess, Message = message, Data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DSCSignInDIO(string PublicKey)
        {
            long empid = Convert.ToInt64(Session["UID"]);
            bool isSuccess = false;
            string message = string.Empty;
            string result = _INBApplicationbll.DSCLogin(empid, PublicKey);
            if (result == "SUCCESS")
            {
                isSuccess = true;
                message = "Success";
                //return Json(new { IsSuccess = isSuccess, Message = message, Data = strBytes }, JsonRequestBehavior.AllowGet);
                return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                isSuccess = false;
                message = "Failure";
                //return Json(new { IsSuccess = isSuccess, Message = message, Data = strBytes }, JsonRequestBehavior.AllowGet);
                return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        //Added by Venkatesh
        #region CreatePDF
        public string CreatePDF(long app_id, decimal premium_amount, decimal? sum_assured, int load_factor_id, int dl_factor_id, long emp_id)
        {
            PdfPTable fieldServiceReportDesign = MainPage(app_id, premium_amount, sum_assured, load_factor_id, dl_factor_id, emp_id);
            Document document = new Document(PageSize.A4, 0f, 0f, 0f, 17f);
            MemoryStream stream = new MemoryStream();
            using (PdfWriter pdfWriter = PdfWriter.GetInstance(document, stream))
            {
                pdfWriter.CloseStream = false;
                document.Open();
                document.Add(fieldServiceReportDesign);
                document.Close();
                stream.Flush(); //Always catches me out  
                stream.Position = 0;
            }
            string subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"VerifyDataPdfDocs\";
            string PdfFile = subPath + "PDF_" + DateTime.Now.Ticks + ".pdf";
            using (FileStream file = new FileStream(PdfFile, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(file);
            }
            return PdfFile;
            //return File(stream, "application/vnd.pdf", "PDF_" + DateTime.Now.Ticks + ".pdf");
        }
        private PdfPTable MainPage(long app_id, decimal premium_amount, decimal? sum_assured, int load_factor_id, int dl_factor_id, long emp_id)
        {
            //Main table  
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;
            table.DefaultCell.BackgroundColor = new BaseColor(255, 255, 255);
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.HeaderRows = 2;
            #region Header  
            //header details  
            PdfPHeaderCell hcell = new PdfPHeaderCell();
            hcell.Border = 0;
            hcell.Padding = 7f;
            hcell.BackgroundColor = new BaseColor(5, 150, 215);
            hcell.HorizontalAlignment = Element.ALIGN_CENTER;
            hcell.AddElement(GetHeader("KGID"));
            table.AddCell(hcell);
            //space  
            hcell = new PdfPHeaderCell();
            hcell.Border = 0;
            hcell.Padding = 7f;
            hcell.MinimumHeight = 10F;
            hcell.BackgroundColor = new BaseColor(255, 255, 255);
            hcell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(hcell);
            #endregion
            PdfPCell cell = new PdfPCell();
            hcell.Border = 0;
            hcell.Padding = 7f;
            hcell.MinimumHeight = 10F;
            hcell.BackgroundColor = new BaseColor(255, 255, 255);
            hcell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            hcell.AddElement(GetData(app_id, premium_amount, sum_assured, load_factor_id, dl_factor_id, emp_id));
            table.AddCell(hcell);
            //leave blank  
            cell = new PdfPCell();
            cell.Border = 0;
            cell.Padding = 0;
            cell.BackgroundColor = new BaseColor(255, 255, 255);
            cell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
            table.SplitLate = false;
            return table;
        }
        private PdfPTable GetHeader(string headerText)
        {
            PdfPCell cell = null;
            Phrase phrs = null;
            //Header design table  
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;
            table.DefaultCell.Padding = 0;
            table.DefaultCell.Border = 0;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            phrs = new Phrase();
            phrs.Add(new Chunk(headerText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11f, iTextSharp.text.Font.BOLD, new BaseColor(255, 255, 255))));
            cell = new PdfPCell(phrs);
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
            return table;
        }
        private PdfPTable GetData(long app_id, decimal premium_amount, decimal? sum_assured, int load_factor_id, int dl_factor_id, long emp_id)
        {
            PdfPCell cell = null;
            Phrase phrs = null;
            //body design table  
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;
            table.DefaultCell.Padding = 0;
            table.DefaultCell.Border = 0;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            phrs = new Paragraph();
            //phrs.Add(new Chunk("Application ID:  "+ app_id + "", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0))));
            //phrs.Add(new Chunk("\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0))));
            phrs.Add(new Paragraph("Employee ID:  " + emp_id + ""));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("Application ID:  " + app_id + ""));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("Premium Amount:  " + premium_amount + ""));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("Sum Assured:  " + sum_assured + ""));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("Load Factor ID:  " + load_factor_id + ""));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("DL Factor ID:  " + dl_factor_id + ""));
            phrs.Add(new Paragraph("\n"));
            phrs.Add(new Paragraph("\n"));
            //phrs.Add(new Chunk("\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0))));
            //phrs.Add(new Phrase("Hello !", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0))));

            cell = new PdfPCell(phrs);
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
            return table;
        }
        #endregion
        //Added by Venkatesh
        #region SignPdf
        private string SignPdf(string unsignedpdf, long AppID, string PublicKey)
        {
            try
            {
                X509CertificateParser cp = new X509CertificateParser();
                X509Certificate2 certClient = null;

                X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                st.Open(OpenFlags.MaxAllowed);
                X509Certificate2Collection collection = st.Certificates;
                for (int i = 0; i < collection.Count; i++)
                {
                    if (Convert.ToBase64String(collection[i].PublicKey.EncodedKeyValue.RawData) == PublicKey)
                    {
                        certClient = collection[i];
                    }
                    //if (collection[i].Subject.Contains("AJAY ADALA"))
                    //{
                    //    certClient = collection[i];
                    //}
                }

                st.Close();

                //Get Cert Chain
                IList<X509Certificate> chain = new List<X509Certificate>();
                X509Chain x509Chain = new X509Chain();

                x509Chain.Build(certClient);

                foreach (X509ChainElement x509ChainElement in x509Chain.ChainElements)
                {
                    chain.Add(DotNetUtilities.FromX509Certificate(x509ChainElement.Certificate));
                }

                string filename = unsignedpdf;

                PdfReader inputPdf = new PdfReader(filename);
                //string newFile = Server.MapPath(unsignedpdf);
                string newFile = Server.MapPath("~/Documents/VerifyDataPdfDocs/" + "Signed_PDF" + "" + AppID + "" + DateTime.Now.Ticks + ".pdf");
                FileStream signedPdf = new FileStream(newFile, FileMode.Create);
                PdfStamper pdfStamper = PdfStamper.CreateSignature(inputPdf, signedPdf, '\0');


                IExternalSignature externalSignature = new X509Certificate2Signature(certClient, "SHA-256");

                PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;

                //signatureAppearance.Reason = "My Signature";

                signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(300, 100, 80, 10), inputPdf.NumberOfPages, "Signature");
                signatureAppearance.Acro6Layers = false;
                signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
                MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0,
                    CryptoStandard.CMS);

                inputPdf.Close();
                pdfStamper.Close();
                return newFile;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("The operation was canceled by the user or DSC token is not found! Please insert DSC token", "Centre for Smart Governance, Karnataka!!!");
                return null;
            }
        }
        #endregion
        #region RegisterDscSign //Added by Venkatesh
        public ActionResult DscRegistration()
        {
            return View();
        }
        public string UserDscRegistration(string PublicKey, DateTime DateOfExpiring)
        {
            long empid = Convert.ToInt64(Session["UID"]);
            int employeeid = Convert.ToInt32(Session["UID"]);
            try
            {
                tbl_dsc_master objDscMaster = new tbl_dsc_master();
                // fields to be insert
                objDscMaster.dsc_emp_id = empid;
                objDscMaster.dsc_kgid_number = "";
                objDscMaster.dsc_public_key = PublicKey;
                objDscMaster.dsc_date_of_expiring = DateOfExpiring;
                objDscMaster.dsc_dsc_serial_no = 0;
                objDscMaster.dsc_active = true;
                objDscMaster.dsc_created_by = employeeid;
                objDscMaster.dsc_creation_datetime = DateTime.Now;
                objDscMaster.dsc_updated_by = employeeid;
                objDscMaster.dsc_updation_datetime = DateTime.Now;
                _db.tbl_dsc_master.Add(objDscMaster);
                _db.SaveChanges();
                //return result;
                return "SuccessfullyAdded";
            }
            catch (Exception ex)
            {
                return "RegistrationFailed";
            }
        }
        public ActionResult GetDSCDetails(string PublicKey)
        {
            bool isSuccess = false;
            string message = string.Empty;
            try
            {
                X509CertificateParser cp = new X509CertificateParser();
                X509Certificate2 certClient = null;

                X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                st.Open(OpenFlags.MaxAllowed);
                X509Certificate2Collection collection = st.Certificates;
                for (int i = 0; i < collection.Count; i++)
                {
                    if (Convert.ToBase64String(collection[i].PublicKey.EncodedKeyValue.RawData) == PublicKey)
                    {
                        certClient = collection[i];
                    }
                }
                string Subject_Name = certClient.SubjectName.Name.ToString();
                string[] Subject_NameList = Subject_Name.Split(new char[] { ',', '=' });
                string Signatory_Name = Subject_NameList[1];
                string Start_Date = certClient.GetEffectiveDateString();
                string Expiry_Date = certClient.GetExpirationDateString();
                string Serial_Number = certClient.SerialNumber;
                string IssuerName = certClient.IssuerName.Name.ToString();
                string[] IssueAuthorityList = IssuerName.Split(new char[] { ',', '=' });
                string Issue_Authority = IssueAuthorityList[1];
                st.Close();
                isSuccess = true;
                return Json(new { IsSuccess = isSuccess, Message = PublicKey, SignatoryName = Signatory_Name, StartDate = Start_Date, ExpiryDate = Expiry_Date, SerialNumber = Serial_Number, IssueAuthority = Issue_Authority }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                isSuccess = false;
                message = "Cannot proceed..! Please try again..!";

                return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Policy Generation
        [Route("kgid-pg")]
        public ActionResult PolicyGeneration()
        {
            long EmployeeID = 0;
            string EmpDOB = string.Empty;
            if (Session["UID"] != null)
            {
                EmployeeID = Convert.ToInt64(Session["UID"]);
            }

            VM_PolicyGeneration obj = new VM_PolicyGeneration();
            obj.anp_emp_id = EmployeeID;
            obj.Status = string.Empty;
            return View("NewPolicyGeneration", obj);
        }
        [Route("SavePG")]
        public ActionResult SavePolicyGenerationData(VM_PolicyGeneration obj)
        {
            string result = _INBApplicationbll.SavePolicyGeneration(obj);
            //return result;
            return Json(new { PolicyNumber = result, RedirectUrl = "/kgid-view-app/" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update user details
        [Route("kgid-update-em")]
        public ActionResult UserDetails()
        {
            return View();
        }
        [Route("get-user-details")]
        public JsonResult GetUserDetails(long KGIDNumber)
        {
            string result = _INBApplicationbll.GetUserDetails(KGIDNumber);
            if (result == "0~0")
            {
                return Json(new { EmployeeId = "0", EmilID = "0", MobileNumber = "0" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { EmployeeId = result.Split('~')[0].ToString(), EmilID = result.Split('~')[1].ToString(), MobileNumber = result.Split('~')[2].ToString() }, JsonRequestBehavior.AllowGet);
            //  return Json(new { MobileNumber = result.Split('~')[1].ToString(), EmilID = result.Split('~')[0].ToString() }, JsonRequestBehavior.AllowGet);
        }
        [Route("get-user-details-basedOnMobNum")]
        public JsonResult GetUserDetailsbasedOnMobNum(long MobileNumber)
        {
            string result = _INBApplicationbll.GetUserDetailsbasedOnMobNum(MobileNumber);
            if (result == "0~0")
            {
                return Json(new { EmployeeId = "0", EmilID = "0", MobileNumber = "0" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { KgidNum = result.Split('~')[0].ToString(), EmployeeId = result.Split('~')[1].ToString(), EmilID = result.Split('~')[2].ToString(), MobileNumber = result.Split('~')[3].ToString() }, JsonRequestBehavior.AllowGet);
        }
        [Route("get-paymentapplication-details")]
        public ActionResult GetpaymentapplicationDetails(string applicationRefNum)
        {
            List<VM_ApplicationDetail> result = _INBApplicationbll.GetpaymentapplicationDetails(applicationRefNum);
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item.PaymentStatus == 2)
                    { ViewBag.paymentstatus = "true"; }
                    else
                    { ViewBag.paymentstatus = "false"; }
                }
                ViewBag.count = result.Count();
                Session["count"] = result.Count();
                ViewBag.payment = result;
            }
            if (result == null)
            {
                result = new List<VM_ApplicationDetail>();
                ViewBag.payment = result;
            }
            return PartialView("DisplayPaymentDetails", result);
            //return Json(new { MobileNumber = result.Split('~')[1].ToString(), EmilID = result.Split('~')[0].ToString() }, JsonRequestBehavior.AllowGet);
        }

        // Delete and Update challan details for payment Verification
        [Route("delete-challanDetails")]
        public string DeletenUpdateChallanDetails(long cd_challan_id, string cd_challan_ref_no, long applicationId)
        {
            var tempCount = Session["count"];
            string ChallanStatusRowCount = tempCount.ToString();
            string result = _INBApplicationbll.DeleteForPaymentVerification(cd_challan_id, cd_challan_ref_no, applicationId, ChallanStatusRowCount);
            return result;
        }

        [Route("update-paymentdetail-details-ChallanStatus")]
        public string UpdatePaymentVerification(long paymentstatus, long challanId)
        {
            string result = _INBApplicationbll.UpdatePaymentVerification(paymentstatus, challanId);
            return result;
        }


        [Route("kgid-update-paymentverification")]
        public ActionResult PaymentVerification()
        {
            List<VM_ApplicationDetail> result = null;
            return View(result);
        }
       /* [Route("update-user-details")]
        public string UpdateUserDetails(long KGIDNumber, long MobileNumber, string EMailID, long EmployeeId)
        {
            string result = _INBApplicationbll.UpdateUserDetails(KGIDNumber, MobileNumber, EMailID, EmployeeId);
            return result;
        }*/
        #endregion

        #region Update ddo details
        [Route("kgid-update-ddo")]
        public ActionResult DDODetails()
        {
            return View();
        }
        [Route("kgid-get-dio-ofc-details")]
        public JsonResult GetDIOOfficeDetails(string DDOCode, string KGIDNo)
        {
            string result = _INBApplicationbll.GetDIOOfficeDetails(DDOCode, KGIDNo);
            string empdet = result.Split('~')[1].ToString();
            string ddo = result.Split('~')[0].ToString();
            return Json(new { DIOOffice = result.Split('~')[0].ToString(), EmpKGIDNo = result.Split('~')[1].ToString() }, JsonRequestBehavior.AllowGet);

        }
        [Route("kgid-get-ddo-details")]
        public JsonResult GetDDODetails(long KGIDNumber)
        {
            string result = _INBApplicationbll.GetDDODetails(KGIDNumber);
            //return result;
            return Json(new { DDOCode = result.Split('~')[0].ToString(), DIOOffice = result.Split('~')[1].ToString(), Employeename = result.Split('~')[2].ToString(), EmpMobileNo = result.Split('~')[3].ToString() }, JsonRequestBehavior.AllowGet);


            //return Json(new { DDOCode = result.Split('~')[0].ToString(), DIOOffice = result.Split('~')[1].ToString() }, JsonRequestBehavior.AllowGet);
        }
        [Route("kgid-check-ddo-details")]
        public string CheckDDODetails(long KGIDNumber, string DDOCode, string DIOOffice)
        {
            string result = _INBApplicationbll.CheckDDODetails(KGIDNumber, DDOCode, DIOOffice);
            return result;
        }
        [Route("kgid-update-ddo-details")]
        public string UpdateDDODetails(long KGIDNumber, string DDOCode, string DIOOffice)
        {
            string result = _INBApplicationbll.UpdateDDODetails(KGIDNumber, DDOCode, DIOOffice);
            return result;
        }
        #endregion

        public ActionResult PrintChallanDuplicate(long EmpId, long AppId, long challanNo = 0)
        {
            long EID = EmpId;
            if (EID == EmpId)
            {
                VM_ChallanPrintDetails NBChallanDetails = new VM_ChallanPrintDetails();
                NBChallanDetails = _newemp.ChallanprintDetailsBLL(EmpId, AppId);
                string filepath = FillNBChallanDuplicate(NBChallanDetails, AppId);
                //return View("FacingSheet", facingSheet1);
                return File(filepath, "application/pdf");
            }
            else
            {
                return View();
            }
            //return NBChallanDetails;
        }
        private string FillNBChallanDuplicate(VM_ChallanPrintDetails NBChallanDetails, long result)
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToShortDateString();
            string amtwords = ConvertToWords(Convert.ToString(NBChallanDetails.p_premium));
            NBChallanDetails.Category = "Government";
            NBChallanDetails.GrandTotal = Convert.ToString(NBChallanDetails.p_premium);
            //KG MONTH YEAR 8011 01-8digits
            //string my = dt.ToString("MMyy");
            //string timestamp = dt.ToString("hhmmssff");
            //string challanrefno = "KG" + my + "801101" + timestamp;
            NBChallanDetails.challan_ref_no = NBChallanDetails.challan_ref_no;
            ////////////////////////////////////////////////////////////////
            string pdfTemplate = Server.MapPath("~/Challans/NB/Challan_NB_Test.pdf");
            //string newFile = Server.MapPath("~/Challans/NB/" + challanrefno + ".pdf");
            //string newFile = @"C:/Documents/Challans/NB/" + challanrefno + ".pdf";
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"Challans\NB\" + NBChallanDetails.challan_ref_no + "_Duplicate" + ".pdf";
            }
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            {//Facing Sheet Details
             //var date1 = facingSheet.DateOfIssue?.ToString("dd-MM-yyyy");

                fields.SetField("ChallanValidity", NBChallanDetails.Challan_Validity);
                fields.SetField("District", NBChallanDetails.dm_name_english.ToString().Trim().ToUpper());
                fields.SetField("Department", NBChallanDetails.dm_deptname_english);
                fields.SetField("DDOOffice", NBChallanDetails.dm_ddo_office);
                fields.SetField("Category", NBChallanDetails.Category);
                //fields.SetField("Date", NBChallanDetails.challan_date.ToString().Trim().ToUpper());
                fields.SetField("Date", NBChallanDetails.challan_date);
                fields.SetField("ChallanReferenceNo", NBChallanDetails.challan_ref_no);
                fields.SetField("DDOCode", NBChallanDetails.dm_ddo_code);
                fields.SetField("RemitterName", NBChallanDetails.employee_name.ToString().Trim().ToUpper());
                fields.SetField("MobileNo", NBChallanDetails.mobile_number.ToString().Trim().ToUpper());
                fields.SetField("Address", NBChallanDetails.ead_address);

                fields.SetField("Purpose", NBChallanDetails.purpose_desc);
                fields.SetField("HOA", NBChallanDetails.hoa_desc);
                fields.SetField("SubPurposeName", NBChallanDetails.sub_purpose_desc);
                fields.SetField("PurposeSpecificID", NBChallanDetails.purpose_id);
                fields.SetField("Amount", NBChallanDetails.p_premium.ToString().Trim().ToUpper());
                fields.SetField("RemittanceBank", NBChallanDetails.RemittanceBank);
                fields.SetField("GrandTotal", NBChallanDetails.GrandTotal);
                fields.SetField("TotalAmountinWords", amtwords);
                fields.SetField("ChallanStatus", NBChallanDetails.challan_status);
                fields.SetField("ChequeDDNo", NBChallanDetails.Cheque_DD_No);
                fields.SetField("ChequeDDBank", NBChallanDetails.Cheque_DD_Bank);
                fields.SetField("IFSCode", NBChallanDetails.IFSC_Code);
                fields.SetField("MICRCode", NBChallanDetails.MICR_Code);
                fields.SetField("ChequeDDDate", NBChallanDetails.Cheque_DD_Date.ToString().Trim().ToUpper());
                //fields.SetField("ChequeDDDate", NBChallanDetails.Cheque_DD_Date?.ToString("dd-MM-yyyy"));
            }
            pdfStamper.Close();
            return newFile;
        }

        [Route("ProposerTrackDetails")]
        public ActionResult Track_proposal_number()
        {
            return View();
        }
        public JsonResult getProposerTrackDetails(VM_ApplicationDetail employeeDetails)
        {
            string applicationNo = employeeDetails.ApplicationNumber;
            VM_ApplicationDetail obj = new VM_ApplicationDetail();
            obj.listTrackDetails = _INBApplicationbll.getProposerTrackDetailsBll(applicationNo);
            //if (obj.listTrackDetails.Count == 0)
            //{
            //}               
            // return View("Track_proposal_number",applicationTrackDetails);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        //
        //[HttpPost]
        //[Route("update-user-details")]

        //public async Task<ActionResult> UpdateUserDetails(long KGIDNumber, long MobileNumber, string EMailID, long EmployeeId)
        //{
        //    bool IsAreaAlreadyExists = await _INBApplicationbll.IsMobNoAlreadyExistsAsync(KGIDNumber, MobileNumber, EmployeeId, EMailID);
        //    if (!IsAreaAlreadyExists)
        //    {
        //        return Json(await _INBApplicationbll.UpdateUserDetails1(KGIDNumber, MobileNumber, EMailID, EmployeeId) ? 1 : 0);
        //    }
        //    else
        //        return Json(2);
        //    //return Json(IsAreaAlreadyExists ? 2 : 3);

        //    ////string result = _INBApplicationbll.UpdateUserDetails(KGIDNumber, MobileNumber, EMailID, EmployeeId);
        //    //return result;
        //}
    }
}