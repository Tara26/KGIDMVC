using BLL.DistrictMasterBLL;
using BLL.VerifyDataBLL;
using KGID_Models.KGID_VerifyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.DistrictMasterBLL;
using BLL.RangesMasterBLL;
using System.IO;
using System.Net;
using DLL.DBConnection;
using System.Text;
//using DocumentFormat.OpenXml.Wordprocessing;
//using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using static KGID.FilterConfig;
using Common;
using BLL.NewEmployeeBLL;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using KGID_Models.NBApplication;
using KGID.Models;

namespace KGID.Controllers
{
    [NoCache]
    public class VerifyDataController : Controller
    {
        private readonly DbConnectionKGID _dbVerify = new DbConnectionKGID();
        private readonly IVerificationdetailsbll _Objemployee;
        private readonly IDistrictMasterBLL _districtMasterbll;
        private readonly IRangesMasterBLL _rangesMasterbll;
        private readonly INBApplicationBll _INBApplicationbll;
        public VerifyDataController()
        {
            this._Objemployee = new Verificationdetailsbll();
            this._districtMasterbll = new DistrictMasterBLL();
            this._rangesMasterbll = new RangesMasterBLL();
            this._INBApplicationbll = new NBApplicationBll();
        }
        // GET: VerifyData
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VerifyData(tbl_verification_details _Viewemp)
        {
            if (ViewBag.EmpDetails != null)
            {
                tbl_verification_details _empModel = new tbl_verification_details();
                _empModel = ViewBag.EmpDetails as tbl_verification_details;
                return View(_empModel);
            }
            return View();
        }

        public ActionResult SearchEmployee(tbl_verification_details _Viewemp)
        {
            tbl_verification_details _empModel = new tbl_verification_details();
            var _empData = _Objemployee.UserDetailsbll(_Viewemp);
            _empModel.GetEmployeeDataList = _empData;
            //TempData["EmpList"] = _empData;

            ViewBag.EmpList = _empData;
            tbl_district_master _distMasterModel = new tbl_district_master();
            var _ddoMasterDetails = _districtMasterbll.DistrictMasterbll();

            ViewBag.DistrictNames = _ddoMasterDetails;
            var aaa = TempData["NODATA"];
            ViewBag.NODATA = TempData["NODATA"];

            return View(_empModel);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadStartRangesByDistId(int _distId)
        {
            var _startRange = _rangesMasterbll.GetStartRangesByDistId(_distId);

            return Json(_startRange, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadEndRangesByDistIdandStartRangeId(int _distId, int _rmId)
        {
            var _endRange = _rangesMasterbll.GetEndRangesByDistIdandStartRangeId(_distId, _rmId);
            ViewBag.endRange = _endRange;
            return Json(_endRange, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetEmployeeData(int KGIDNO)
        {
            if (KGIDNO != 0)
            {
                tbl_verification_details _empModel = new tbl_verification_details();
                var _empDetails = _Objemployee.EmployeeDetailsbll(KGIDNO);
                _empModel = _empDetails;
               // TempData["EmpDetails"] = _empDetails;
                ViewBag.EmpDetails = _empDetails;
                return RedirectToAction("VerifyData", "VerifyData");
            }
            else
            {
                return RedirectToAction("SearchEmployee", "VerifyData");
            }
        }
        //public ActionResult Submit(string submit, tbl_verification_details _KGIDNO)
        //{
        //    string Value = string.Empty;
        //    if (submit == "Verify")
        //    {
        //        Value = "V";
        //    }
        //    else
        //    {
        //        Value = "R";
        //    }
        //    tbl_verification_details _empModel = new tbl_verification_details();
        //    var _empDetails = _Objemployee.UpdateStatusbll(Value, _KGIDNO.VERIFIER_REMARKS, _KGIDNO.FIRST_KGIDNO_HRMS, Convert.ToInt32(Session["UID"]));
        //    _empModel = _empDetails;
        //    TempData["EmpDetails"] = _empDetails;
        //    //return RedirectToAction("VerifyData", "VerifyData");
        //    return RedirectToAction("SearchEmployee", "VerifyData");
        //}

        [HttpGet]
        public ActionResult getVerificationData(int status, string _policyNo, string districtId)
        {
            var _empData = new List<tbl_verification_details>();
            var _empDataList = new object();
            _empDataList = _Objemployee.getVerificationData(status, _policyNo, Convert.ToInt32(districtId));
            Session["DistrictId"] = districtId;
            //if (rmId > 0)
            //{
            //    tbl_ranges_master rangeMaster = _rangesMasterbll.GetRangesById(rmId);
            //    _empData = _Objemployee.getVerificationData(status, rangeMaster.rm_start_no, int.Parse(rangeMaster.rm_end_no)).ToList();
            //    _empDataList = (from n in _empData
            //                    select new
            //                    {
            //                        n.VD_ID,
            //                        n.FIRST_KGIDNO_HRMS,
            //                        n.EMPLOYEE_NAME_KGID,
            //                        DATE_OF_BIRTH_KGID = n.DATE_OF_BIRTH_KGID.ToString(),
            //                        n.FATHER_NAME_KGID,
            //                        n.EMPLOYEE_AGE,
            //                        DATE_OF_RISK = n.DATE_OF_RISK.ToString()
            //                    }).ToList();
            //}
            //else
            //{
            //    _empData = _Objemployee.getVerificationData(status).ToList();

            //    _empDataList = (from n in _empData
            //                    select new
            //                    {
            //                        n.VD_ID,
            //                        n.FIRST_KGIDNO_HRMS,
            //                        n.EMPLOYEE_NAME_KGID,
            //                        DATE_OF_BIRTH_KGID = n.DATE_OF_BIRTH_KGID.ToString(),
            //                        n.FATHER_NAME_KGID,
            //                        n.EMPLOYEE_AGE,
            //                        DATE_OF_RISK = n.DATE_OF_RISK.ToString()
            //                    }).ToList();
            //}



            return Json(_empDataList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditVerifyData(int vd_id = 0)//For Showing Details Record
        {
            if (vd_id > 0)
            {
                //tbl_verification_details _empModel = _Objemployee.GetVerificationDetailById(vd_id);
                VM_VerifiedBy _empModel = _Objemployee.GetVerificationDetailById(vd_id);

                return View("VerifyData", _empModel);
            }

            return RedirectToAction("SearchEmployee", "VerifyData");

        }

        [HttpPost]
        public ActionResult EditVerifyData(string submit, VM_VerifiedBy _VBY)
        {
            if (_VBY != null)
            {
                string Value = string.Empty;
                if (submit == "Verify" || submit == "Save & Verify")
                {
                    Value = "V";
                }
                else
                {
                    Value = "F";
                }
                tbl_verification_details _empModel = new tbl_verification_details();
                var _empDetails = _Objemployee.UpdateStatusbll(Value, _VBY, Convert.ToInt32(Session["UID"]));
                //if (!Value.Equals("R"))
                //{
                //    tbl_verified_details _verifiedDetailModel = new tbl_verified_details();
                //    _verifiedDetailModel.VD_ID = _Verification_Details.VD_ID;
                //    _verifiedDetailModel.EMPID_HRMS = _Verification_Details.EMPID_HRMS;
                //    _verifiedDetailModel.POLICY_NO = _Verification_Details.POLICY_NO;
                //    _verifiedDetailModel.FIRST_KGIDNO_HRMS = _Verification_Details.FIRST_KGIDNO_HRMS;
                //    _verifiedDetailModel.EMPLOYEE_NAME_HRMS = _Verification_Details.EMPLOYEE_NAME_HRMS;
                //    _verifiedDetailModel.EMPLOYEE_NAME_KGID = _Verification_Details.EMPLOYEE_NAME_KGID;
                //    _verifiedDetailModel.GENDER_HRMS = _Verification_Details.GENDER_HRMS;
                //    _verifiedDetailModel.DATE_OF_BIRTH_HRMS = _Verification_Details.DATE_OF_BIRTH_HRMS;
                //    _verifiedDetailModel.DATE_OF_BIRTH_KGID = _Verification_Details.DATE_OF_BIRTH_KGID;
                //    _verifiedDetailModel.EMPLOYEE_AGE = _Verification_Details.EMPLOYEE_AGE;
                //    _verifiedDetailModel.FATHER_NAME_HRMS = _Verification_Details.FATHER_NAME_HRMS;
                //    _verifiedDetailModel.FATHER_NAME_KGID = _Verification_Details.FATHER_NAME_KGID;
                //    _verifiedDetailModel.DATE_OF_RISK = _Verification_Details.DATE_OF_RISK;
                //    _verifiedDetailModel.PREMIUM = _Verification_Details.PREMIUM;
                //    _verifiedDetailModel.SUM_ASSURED = _Verification_Details.SUM_ASSURED;
                //    _verifiedDetailModel.NOMINEE_NAME = _Verification_Details.NOMINEE_NAME;
                //    _verifiedDetailModel.NOMINEE_RELATION = _Verification_Details.NOMINEE_RELATION;
                //    _verifiedDetailModel.NOMINEE_AGE = _Verification_Details.NOMINEE_AGE;
                //    _verifiedDetailModel.ACCEPTANCE_TYPE = _Verification_Details.ACCEPTANCE_TYPE;
                //    _verifiedDetailModel.REMARKS = _Verification_Details.REMARKS;
                //    _verifiedDetailModel.STATUS = Value;
                //    _verifiedDetailModel.VERIFIED_BY = Convert.ToInt32(Session["UID"]);

                //    _Objemployee.AddVerifiedDetailsbll(_verifiedDetailModel);

                //    return RedirectToAction("SearchEmployee", "VerifyData");
                //}
            }
            return RedirectToAction("SearchEmployee", "VerifyData");
        }
        [Route("kgid-upload-emp-data")]
        public ActionResult UploadEmployeeData()
        {

            DbConnectionKGID _db = new DbConnectionKGID();
            VM_Upload_EmployeeForm objUEData = new VM_Upload_EmployeeForm();
            ViewBag.IsMedicalRequired = Session["IsMedicalRequired"];
            //hemanth 
            //if(Session["IsApplicationReqd"] ==true)
            ViewBag.IsApplicationRequired = Session["IsApplicationReqd"];
            objUEData.App_Employee_Code = Convert.ToInt64(Session["UID"]);
            if (objUEData.App_Employee_Code != 0)
            {
                objUEData = _Objemployee.GetUploadDocBll(objUEData.App_Employee_Code);
            }
            objUEData.App_Employee_Code = Convert.ToInt64(Session["UID"]);
            return View(objUEData);
        }
        public ActionResult UploadEmployeeDownload(int id)
        {
            VM_Upload_EmployeeForm objUEData = new VM_Upload_EmployeeForm();
            objUEData.App_Employee_Code = Convert.ToInt64(Session["UID"]);
            if (objUEData.App_Employee_Code != 0)
            {
                objUEData = _Objemployee.GetUploadDocBll(objUEData.App_Employee_Code);
            }
            string fullPath = "";
            if (id == 1)
            {
                fullPath = objUEData.ApplicationFormDocName;
            }
            else if (id == 2)
            {
                fullPath = objUEData.MedicalFormDocName;
            }

            //string fullPath = Path.Combine(Server.MapPath("~/Images/ArticleOfAssociations"), _filepathAppDoc);
            //string fullPath = Server.MapPath("~/EmployeeDocuments/900811781/ApplicationForm/UntitledDocument.pdf");

            return File(fullPath, "application/pdf");

            //return View(objUEData);
        }

        [Route("kgid-upload-form-data")]
        public ActionResult UploadEmployeeData(VM_Upload_EmployeeForm objUF)
        {
            int result=0;
            tbl_upload_employeeform objEmpForm = new tbl_upload_employeeform();
            if (ModelState.IsValid)
            {
                //string path = Server.MapPath("~/Uploads/");
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                string AppPath = UploadDocument(objUF.ApplicationFormDoc, objUF.App_Employee_Code, "ApplicationForm");
                string MedPath = UploadDocument(objUF.MedicalFormDoc, objUF.App_Employee_Code, "MedicalForm");
                objEmpForm.App_Employee_Code = Convert.ToInt64(Session["UID"]);
                objEmpForm.App_Application_Form = AppPath;
                objEmpForm.App_Medical_Form = MedPath;
                //string SubPathForApp = "/EmployeeDocuments/" + objEmpForm.App_Employee_Code.ToString() + "/ApplicationForm/";
                //string SubPathForMed = "/EmployeeDocuments/" + objEmpForm.App_Employee_Code.ToString() + "/MedicalForm/";
                if (Session["IsApplicationReqd"]!=null)
                {
                    //objEmpForm.App_Application_Form = SubPathForApp + objUF.ApplicationFormDoc.FileName;
                    objEmpForm.App_Application_Form = AppPath;
                }
                if ((bool)Session["IsMedicalRequired"])
                {
                    //objEmpForm.App_Medical_Form = SubPathForMed + objUF.MedicalFormDoc.FileName;
                    objEmpForm.App_Medical_Form = MedPath;
                }

                 result = _Objemployee.SaveEmployeeFormBll(objEmpForm);
                if (result == 1)
                {
                    //TempData["EmpForm"] = objEmpForm;
                    ViewBag.EmpForm = objEmpForm;

                    try
                    {
                        var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587481508452);

                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587481508452" + msg);
                        var ApprefNo = (from ad in _dbVerify.tbl_kgid_application_details where ad.kad_emp_id == objEmpForm.App_Employee_Code select ad.kad_kgid_application_number).ToList().LastOrDefault();
                        var mobile = (from eb in _dbVerify.tbl_employee_basic_details where eb.employee_id == objEmpForm.App_Employee_Code select eb).FirstOrDefault();
                        var ddoempid = (from eb in _dbVerify.tbl_employee_work_details where eb.ewd_emp_id == objEmpForm.App_Employee_Code select eb.ewd_ddo_id).FirstOrDefault();

                        var ddomobileno = (from ebd in _dbVerify.tbl_employee_basic_details
                                           join wd in _dbVerify.tbl_employee_work_details on ebd.employee_id equals wd.ewd_emp_id
                                           where ebd.user_category_id.Contains("1,2") && wd.ewd_ddo_id == ddoempid
                                           select ebd.mobile_number).FirstOrDefault();
                        // var challandate = (from ch in _dbVerify.tbl_challan_details where ch.cd_system_emp_code == objEmpForm.App_Employee_Code select ch.cd_updation_datetime).FirstOrDefault();
                        var challandate = _dbVerify.tbl_challan_details.Where(a => a.cd_application_id == objEmpForm.App_ApplicationID).Select(a => a.cd_updation_datetime).FirstOrDefault();

                        msg = msg.Replace("#var1#", ApprefNo);
                        msg = msg.Replace("#var2#", challandate.ToString());
                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587481508452 replace of values" + msg);
                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData obile.mobile_number.ToString()  " + mobile.mobile_number.ToString());
                        //string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + ApprefNo + " ಯು ದಿನಾಂಕ " + challandate + " ದಂದು ಯಶಸ್ವಿಯಾಗಿ ನಿಮ್ಮ ವೇತನ ಬಟವಾಡೆ ಮಾಡುವ ಅಧಿಕಾರಿಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ,"
                        //    + "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                        AllCommon.sendUnicodeSMS(mobile.mobile_number.ToString(), msg, "1107161587481508452");
                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData SMS Sent ");
                        var ddomsg = _INBApplicationbll.GetEmailSMSTemplate(1107161587541292075);

                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587541292075 " + msg);
                        
                        ddomsg =ddomsg.Replace("#var1#", mobile.employee_name);
                        ddomsg = ddomsg.Replace("#var2#", ApprefNo);

                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData  ddo replace of values " + msg);
                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData ddomobileno.ToString()  " + ddomobileno.ToString());


                        //                string ddomsg = "ವೇತನ ಬಟವಾಡೆ ಅಧಿಕಾರಿಗಳೇ, ಶ್ರೀ/ಶ್ರೀಮತಿ " + mobile.employee_name + " ರವರ " + ApprefNo + "  ಸಂಖ್ಯೆಯ ವಿಮಾ ಪ್ರಸ್ತಾವನೆಯು ಪರಿಶೀಲನೆಗಾಗಿ ನಿಮ್ಮ ಲಾಗಿನ್‌ಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ. https://kgidonline.karnataka.gov.in ಗೆ ಲಾಗಿನ್ ಆಗಿ ಮುಂದಿನ ಕ್ರಮ ಕೈಗೊಳ್ಳಲು ಕೋರಿದೆ."
                        //+ "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                        AllCommon.sendUnicodeSMS(ddomobileno.ToString(), ddomsg, "1107161587541292075");
                        Logger.LogMessage(TracingLevel.INFO, "SMS Sent ");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData" + ex.Message.ToString());
                    }

                    // return RedirectToAction("ViewApplicationDetails", "VerifyDetails");
                    return Json(new { RedirectUrl = "/kgid-view-app/" }, JsonRequestBehavior.AllowGet);
                }
                else if (result == 2)
                {
                   // TempData["EmpForm"] = objEmpForm;
                   ViewBag.EmpForm = objEmpForm;
                    try
                    {
                        var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587481508452);

                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587481508452" + msg);
                        var ApprefNo = (from ad in _dbVerify.tbl_kgid_application_details where ad.kad_emp_id == objEmpForm.App_Employee_Code select ad.kad_kgid_application_number).ToList().LastOrDefault();
                        var mobile = (from eb in _dbVerify.tbl_employee_basic_details where eb.employee_id == objEmpForm.App_Employee_Code select eb).FirstOrDefault();
                        var ddoempid = (from eb in _dbVerify.tbl_employee_work_details where eb.ewd_emp_id == objEmpForm.App_Employee_Code select eb.ewd_ddo_id).FirstOrDefault();

                    var ddomobileno = (from ebd in _dbVerify.tbl_employee_basic_details
                                       join wd in _dbVerify.tbl_employee_work_details on ebd.employee_id equals wd.ewd_emp_id
                                       where ebd.user_category_id.Contains("1,2") && wd.ewd_ddo_id == ddoempid
                                       select ebd.mobile_number).FirstOrDefault();
                    var challandate = (from ch in _dbVerify.tbl_challan_details where ch.cd_application_id == objEmpForm.App_ApplicationID select ch.cd_updation_datetime).FirstOrDefault();
                    

                        msg = msg.Replace("#var1#", ApprefNo);
                        msg = msg.Replace("#var2#", challandate.ToString());
                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587481508452 replace of values" + msg);
                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData obile.mobile_number.ToString()  " + mobile.mobile_number.ToString());
                        //string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + ApprefNo + " ಯು ದಿನಾಂಕ " + challandate + " ದಂದು ಯಶಸ್ವಿಯಾಗಿ ನಿಮ್ಮ ವೇತನ ಬಟವಾಡೆ ಮಾಡುವ ಅಧಿಕಾರಿಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ,"
                        //    + "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                        AllCommon.sendUnicodeSMS(mobile.mobile_number.ToString(), msg, "1107161587481508452");
                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData SMS Sent ");
                        var ddomsg = _INBApplicationbll.GetEmailSMSTemplate(1107161587541292075);

                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587541292075 " + msg);

                        ddomsg = ddomsg.Replace("#var1#", mobile.employee_name);
                        ddomsg = ddomsg.Replace("#var2#", ApprefNo);

                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData  ddo replace of values " + msg);
                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData ddomobileno.ToString()  " + ddomobileno.ToString());


                        //                string ddomsg = "ವೇತನ ಬಟವಾಡೆ ಅಧಿಕಾರಿಗಳೇ, ಶ್ರೀ/ಶ್ರೀಮತಿ " + mobile.employee_name + " ರವರ " + ApprefNo + "  ಸಂಖ್ಯೆಯ ವಿಮಾ ಪ್ರಸ್ತಾವನೆಯು ಪರಿಶೀಲನೆಗಾಗಿ ನಿಮ್ಮ ಲಾಗಿನ್‌ಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ. https://kgidonline.karnataka.gov.in ಗೆ ಲಾಗಿನ್ ಆಗಿ ಮುಂದಿನ ಕ್ರಮ ಕೈಗೊಳ್ಳಲು ಕೋರಿದೆ."
                        //+ "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                        AllCommon.sendUnicodeSMS(ddomobileno.ToString(), ddomsg, "1107161587541292075");
                        Logger.LogMessage(TracingLevel.INFO, "SMS Sent ");
                    }
                    catch(Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData" + ex.Message.ToString());
                    }
                    
                    //return RedirectToAction("ViewApplicationDetails", "VerifyDetails");
                    return Json(new { RedirectUrl = "/kgid-view-app/" }, JsonRequestBehavior.AllowGet);
                }
            }
            //return View(objEmpForm);
            //return RedirectToAction("UploadEmployeeData", "VerifyData");
            return Json(new { RedirectUrl = "/kgid-upload-emp-data/", Result = result }, JsonRequestBehavior.AllowGet);
            //return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }
        private string UploadDocument(HttpPostedFileBase document, long empId, string docType)
        {
            string path = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);




                string subPath = string.Empty;// = "~/EmployeeDocuments/" + empId.ToString() + "/" + docType;
                
                if (docType == "ApplicationForm")
                {
                    //subPath = @"F:/Documents/EmployeeDocuments/ApplicationForm/";// + empId.ToString() + "/" + docType;
                    if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    {
                        subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"EmployeeDocuments\ApplicationForm\";
                    }
                }
                else if (docType == "MedicalForm")
                {
                    //subPath = @"F:/Documents/EmployeeDocuments/MedicalForm/";
                    if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    {
                        subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"EmployeeDocuments\MedicalForm\";
                    }
                }
                //bool exists = Directory.Exists(Server.MapPath(subPath));
                //if (!exists)
                //{
                //    Directory.CreateDirectory(Server.MapPath(subPath));
                //}

                //path = Path.Combine(Server.MapPath(subPath), fileName);
                string FileNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                path = subPath + empId.ToString() + FileNo + fileName;

                document.SaveAs(path);
            }

            return path;
        }

        #region NB Bond
        public ActionResult PrintNBBond(int PolicyNo = 0)
        {
            //int PolicyNo = 0;
            VM_NBBond _empNBBond = new VM_NBBond();
            //_empData.Verification_details = _Objemployee.getVerificationDataByST(Convert.ToInt32(objVerification.status), objVerification.PolicyNo, objVerification.distid);

            _empNBBond = _Objemployee.getNBBondDetails(PolicyNo);
            //CreatePdf1();
            return View(_empNBBond);
        }
        //public FileResult CreatePdf()
        //{
        //    MemoryStream workStream = new MemoryStream();
        //    StringBuilder status = new StringBuilder("");
        //    DateTime dTime = DateTime.Now;
        //    //file name to be created   
        //    string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
        //    Document doc = new Document();
        //    doc.SetMargins(0f, 0f, 0f, 0f);
        //    //Create PDF Table with 5 columns  
        //    PdfPTable tableLayout = new PdfPTable(5);
        //    doc.SetMargins(0f, 0f, 0f, 0f);
        //    //Create PDF Table  

        //    //file will created in this path  
        //    string strAttachment = Server.MapPath("~/Documents/NBBond/" + strPDFFileName);

        //    PdfWriter.GetInstance(doc, workStream).CloseStream = false;
        //    //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
        //    doc.Open();

        //    //Add Content to PDF   
        //    //doc.Add("Abcd");
        //    //doc.NewPage();
        //    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, new StringReader("ABCD"));
        //    doc.Add(Add_Content_To_PDF(tableLayout));
        //    // Closing the document  
        //    doc.Close();

        //    byte[] byteInfo = workStream.ToArray();
        //    workStream.Write(byteInfo, 0, byteInfo.Length);
        //    workStream.Position = 0;


        //    return File(workStream, "application/pdf", strPDFFileName);

        //}
        //protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        //{

        //    float[] headers = { 50, 24, 45, 35, 50 };  //Header Widths
        //    tableLayout.SetWidths(headers);        //Set the pdf headers
        //    tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
        //    tableLayout.HeaderRows = 1;

        //    tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


        //    ////Add header
        //    AddCellToHeader(tableLayout, "EmployeeId");
        //    AddCellToHeader(tableLayout, "Name");
        //    AddCellToHeader(tableLayout, "Gender");
        //    AddCellToHeader(tableLayout, "City");
        //    AddCellToHeader(tableLayout, "Hire Date");

        //    return tableLayout;
        //}
        //private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        //{

        //    tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0) });
        //}
        #endregion

        //public FileResult CreatePdf1()
        //{
        //    //MemoryStream workStream = new MemoryStream();
        //    //StringBuilder status = new StringBuilder("");
        //    DateTime dTime = DateTime.Now; //file name to be created 
        //    string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + ".pdf");
        //    Document doc = new Document(); doc.SetMargins(0f, 0f, 0f, 0f);
        //    ////Create PDF Table with 5 columns 
        //    //PdfPTable tableLayout = new PdfPTable(5);
        //    //doc.SetMargins(0f, 0f, 0f, 0f);
        //    //Create PDF Table 
        //    //file will created in this path 

        //   PdfDocument pdf = new PdfDocument();
        //    pdf.Save(Server.MapPath("~/Documents/" + strPDFFileName));
        //    string strAttachment = Server.MapPath("~/Documents/" + strPDFFileName);
        //    //iTextSharp.text.pdf.PdfWriter.GetInstance(doc, workStream).CloseStream = false;
        //    //doc.Open();
        //    ////Add Content to PDF 
        //    //doc.Add(Add_Content_To_PDF1(tableLayout));
        //    //// Closing the document 
        //    //doc.Close();
        //    //byte[] byteInfo = workStream.ToArray();
        //    //workStream.Write(byteInfo, 0, byteInfo.Length);
        //    //workStream.Position = 0;

        //    string line = null;
        //    System.IO.TextReader readFile = new StreamReader("Text.txt");
        //    int yPoint = 0;

        //    PdfDocument pdf = new PdfDocument();
        //    pdf.Info.Title = "TXT to PDF";
        //    PdfPage pdfPage = pdf.AddPage();
        //    XGraphics graph = XGraphics.FromPdfPage(pdfPage);
        //    XFont font = new XFont("Verdana", 20, XFontStyle.Regular);

        //    while (true)
        //    {
        //        line = readFile.ReadLine();
        //        if (line == null)
        //        {
        //            break; // TODO: might not be correct. Was : Exit While
        //        }
        //        else
        //        {
        //            graph.DrawString(line, font, XBrushes.Black, new XRect(40, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
        //            yPoint = yPoint + 40;
        //        }
        //    }

        //    string pdfFilename = "txttopdf.pdf";
        //    pdf.Save(pdfFilename);
        //    readFile.Close();
        //    readFile = null;
        //    Process.Start(pdfFilename);



        //    return File( "application/pdf", strPDFFileName);
        //}
        //protected PdfPTable Add_Content_To_PDF1(PdfPTable tableLayout)
        //{
        //    float[] headers = { 50, 24, 45, 35, 50 };
        //    //Header Widths 
        //    tableLayout.SetWidths(headers);
        //    //Set the pdf headers 
        //    tableLayout.WidthPercentage = 100;
        //    //Set the PDF File witdh percentage 
        //    tableLayout.HeaderRows = 1;
        //    //Add Title to the PDF file at the top 
        //    //List<Employee> employees = _context.employees.ToList<Employee>();
        //    tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });
        //    ////Add header 
        //    AddCellToHeader1(tableLayout, "EmployeeId");
        //    AddCellToHeader1(tableLayout, "Name");
        //    AddCellToHeader1(tableLayout, "Gender");
        //    AddCellToHeader1(tableLayout, "City");
        //    AddCellToHeader1(tableLayout, "Hire Date");
        //    ////Add body 
        //    //foreach (var emp in employees)
        //    //{
        //        AddCellToBody(tableLayout, "A");
        //        AddCellToBody(tableLayout, "B");
        //        AddCellToBody(tableLayout, "Male");
        //        AddCellToBody(tableLayout, "Bng");
        //        AddCellToBody(tableLayout, "01-10-2020");
        //    //}
        //    return tableLayout;
        //}
        //// Method to add single cell to the Header 
        //private static void AddCellToHeader1(PdfPTable tableLayout, string cellText)
        //{
        //    tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0) });
        //}
        //// Method to add single cell to the body 
        //private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        //{
        //    tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        //}
        public ActionResult PrintFilePath(string FilePath)
        {
            bool isSuccess = false;
            string message = string.Empty;
            string result = string.Empty;
            try
            {
                byte[] pdfByteArray = System.IO.File.ReadAllBytes(FilePath);
                string base64EncodedPDF = System.Convert.ToBase64String(pdfByteArray);
                isSuccess = true;
                message = "Download File Successfully Generated";
                //return File(base64EncodedPDF, "application/pdf");
                return Json(new { IsSuccess = isSuccess, Message = message, Result = base64EncodedPDF }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                isSuccess = false;
                message = "Download File Generated Fail..!";
                return Json(new { IsSuccess = isSuccess, Message = message, Result = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        #region UPLOADSTATUS
        public VM_BasicDetails GetEmployeeApplicationStatusDll(long empId)
        {
            Common_Connection _Conn = new Common_Connection();
            VM_BasicDetails objBD = new VM_BasicDetails();
            try
            {
                DataSet dsEFD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmployeeID",empId)
                };

                dsEFD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_uploadmerststus");
                if (dsEFD.Tables[0].Rows.Count > 0)
                {
                    objBD.monthid = Convert.ToInt32(dsEFD.Tables[0].Rows[0]["EMPLOYEEAGE"].ToString());//monthid USED FOR AGE OF EMPLOYEE
                    objBD.p_premium = Convert.ToDouble(dsEFD.Tables[0].Rows[0]["PREMIUM"].ToString());
                    objBD.ead_application_id= Convert.ToInt64(dsEFD.Tables[0].Rows[0]["SentBackApplication"].ToString());//ead_application_id USED FOR SentBackApplication
                    objBD.eod_application_id= Convert.ToInt64(dsEFD.Tables[0].Rows[0]["SentBackRemark"].ToString());//eod_application_id USED FOR SentBackRemark
                }
            }
            catch (Exception ex)
            {

            }
            return objBD;
        }
        #endregion
    }
}