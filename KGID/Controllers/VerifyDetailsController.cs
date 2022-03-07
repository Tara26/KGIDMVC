using BLL.DDOMasterBLL;
using BLL.DeptMasterBLL;
using BLL.InsuredEmployeeBll;
using BLL.KGIDMotorInsurance;
using BLL.NewEmployeeBLL;
using BLL.RemarkMasterBLL;
using BLL.VerifyDataBLL;
using Common;
using iTextSharp.text.pdf;
using iTextSharp.text;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using KGID_Models.KGIDMotorInsurance;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using iTextSharp.text.pdf.security;
using System.Security.Cryptography.X509Certificates;
using BLL.KGIDPolicyBLL;
using KGID_Models.KGID_Policy;
using static KGID.FilterConfig;
using DLL.DBConnection;
using System.Web.Configuration;
using System.Globalization;
using KGID.Models;
using System.Threading;

namespace KGID.Controllers
{
    [NoCache]
    public class VerifyDetailsController : Controller
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly IVerificationdetailsbll _Objemployee;
        private readonly INBApplicationBll _INBApplicationbll;
        private readonly IRemarkMasterBLL _Iremarklistbll;
        private readonly IMotorInsuranceVehicleDetailsBll _IMIApplicationbll;
        private readonly IPolicyBLL policyBLL;
        public VerifyDetailsController()
        {
            this._Objemployee = new Verificationdetailsbll();
            this._INBApplicationbll = new NBApplicationBll();
            this._Iremarklistbll = new RemarkMasterBLL();
            this._IMIApplicationbll = new MotorInsuranceVehicleDetailsBll();
            policyBLL = new PolicyBLL();
        }

        public ActionResult GetEmployeeApplicationForm(string empId)
        {
            Session["VerEmpId"] = empId;
            return View("Index");
        }
        [Route("SaveDDOVData")]
        public string InsertVerifyDetails(VM_DeptVerificationDetails objVerifyDetails)
        {
            string result = "";
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            
            result = _INBApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            //string result = _INBApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            //string result = "3";
            //VM_FacingSheet facingSheet = new VM_FacingSheet();
            //facingSheet = policyBLL.GetFacingSheetDetails(189, 134);
            //string filepath = FillFacingSheet(facingSheet, result);
            if (Convert.ToInt32(result) == 1)
            {
                if (objVerifyDetails.ApplicationStatus == 2)
                {

                }
                if (objVerifyDetails.ApplicationStatus == 5)
                {
                    var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == objVerifyDetails.EmpCode select eb.mobile_number).FirstOrDefault();
                    var appRefNo = (from ka in _db.tbl_kgid_application_details where ka.kad_application_id == objVerifyDetails.ApplicationId select ka.kad_kgid_application_number).FirstOrDefault();
                    var assignedto = (from kw in _db.tbl_kgid_application_workflow_details where kw.kawt_application_id == objVerifyDetails.ApplicationId && kw.kawt_active_status==true select kw.kawt_assigned_to).FirstOrDefault();
                    var DistrictOffice = (from ew in _db.tbl_employee_work_details
                                          join ddo in _db.tbl_ddo_master on ew.ewd_ddo_id equals ddo.dm_ddo_id
                                          where ew.ewd_emp_id == assignedto
                                          select ddo.dm_ddo_office).FirstOrDefault();


                        string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + appRefNo + " ಯು ದಿನಾಂಕ " + DateTime.Now + " ದಂದು ಜಿಲ್ಲಾ ವಿಮಾ ಕಛೇರಿ, " + DistrictOffice + " ಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ."
                         + "– ವಿಮಾ ಇಲಾಖೆ(KGID).";
                        //AllCommon.sendOTPMSG(mobile.ToString(), msg);                   
                       // TempData["VerifyDetails"] = objVerifyDetails;
                       ViewBag.VerifyDetails = objVerifyDetails;


                }
            }
            //return RedirectToRoute("/kgid-ddo/", new { area = "" });
            return result;
        }


        [Route("kgid-ddo-verification")]
        [CustomAuthorize("DDO")]
        public ActionResult DDOVerification(long empId = 0, long applicationId = 0)
        {
            if (Session["UID"] != null)
            {

                VM_DeptVerificationDetails verificationDetails = new VM_DeptVerificationDetails();
                if (empId != 0 && applicationId != 0)
                {
                    TempData["empId"] = empId;
                    TempData["applicationId"] = applicationId;
                }
                if (TempData["empId"] != null && TempData["applicationId"] != null)
                {
                    empId = Convert.ToInt32(TempData.Peek("empId"));
                    applicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                    TempData.Keep("empId");
                    TempData.Keep("applicationId");
                    if (empId != 0)
                    {
                        Session["RUID"] = empId;
                        verificationDetails = _INBApplicationbll.GetMedicalLeaveDetails(empId, applicationId);
                        VM_DeptVerificationDetails policyCalculationDetails = _INBApplicationbll.GetPolicyCalculations(empId, applicationId);
                        if (policyCalculationDetails != null)
                        {
                            verificationDetails.LoadFactor = policyCalculationDetails.LoadFactor;
                            verificationDetails.SumAssured = policyCalculationDetails.SumAssured;
                            verificationDetails.DeductionLoadFactors = policyCalculationDetails.DeductionLoadFactors;
                            verificationDetails.DeductionLoadFactor = policyCalculationDetails.DeductionLoadFactor;
                        }
                        verificationDetails.WorkFlowDetails = _INBApplicationbll.GetWorkFlowDetails(applicationId);
                        //Session["RUID"] = empId;
                    }
                }
                else
                {
                    return RedirectToAction("DetailsForDDOVerification", "Employee");
                }
                return View(verificationDetails);
            }
            else
            {
                return RedirectToAction("DetailsForDDOVerification", "Employee");
            }
        }
        public ActionResult EmployeeDocumentDownload(string id, string name)
        {
            try
            {
                VM_Upload_EmployeeForm objUEData = new VM_Upload_EmployeeForm();
                objUEData.App_Employee_Code = Convert.ToInt64(id);
                if (objUEData.App_Employee_Code != 0)
                {
                    objUEData = _Objemployee.GetUploadDocBll(objUEData.App_Employee_Code);
                }
                string fullPath = "";
                if (name == "Application Form")
                {
                    fullPath = objUEData.ApplicationFormDocName;
                }
                else if (name == "Medical Report")
                {
                    fullPath = objUEData.MedicalFormDocName;
                }

                return File(fullPath, "application/pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Case Worker

        [Route("kgid-cw")]
        [CustomAuthorize("Caseworker")]
        public ActionResult DetailsForCaseWorkerVerification()
        {
            ViewBag.Verifier = Verifiers.CW;
            VM_VerificationDetails verificationDetails = _INBApplicationbll.GetEmployeeDetailsForCWVerification(Convert.ToInt64(Session["UID"]));
            Thread.Sleep(2000);
            Logger.LogMessage(TracingLevel.INFO, "Pending Issues CWD " + verificationDetails.PendingApplications.ToString());
            Logger.LogMessage(TracingLevel.INFO, "Total Issues CWD " + verificationDetails.TotalReceived.ToString());
            return View("VerificationDetails", verificationDetails);
        }

        [Route("kgid-cw-verification")]
        [CustomAuthorize("Caseworker")]
        public ActionResult GetApplicationFormForCW(long empId = 0, long applicationId = 0, string verificationType = "")
        {
            if (empId != 0 && applicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = applicationId;
                TempData["verificationType"] = verificationType;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null && TempData["verificationType"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                applicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                verificationType = TempData.Peek("verificationType").ToString();
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                TempData.Keep("verificationType");

                if (empId != 0)
                {
                    Session["RUID"] = empId;
                }

                if (!string.IsNullOrEmpty(verificationType) && verificationType.Equals("NHO"))
                {
                    ViewBag.VerificationType = "NHO";
                }
                VM_DeptVerificationDetails deptVerificationDetails = _INBApplicationbll.GetMedicalLeaveDetails(empId, applicationId);
                VM_DeptVerificationDetails policyCalculationDetails = _INBApplicationbll.GetPolicyCalculations(empId, applicationId);
                if (policyCalculationDetails != null)
                {
                    deptVerificationDetails.LoadFactor = policyCalculationDetails.LoadFactor;
                    deptVerificationDetails.SumAssured = policyCalculationDetails.SumAssured;
                    deptVerificationDetails.DeductionLoadFactors = policyCalculationDetails.DeductionLoadFactors;
                    deptVerificationDetails.DeductionLoadFactor = policyCalculationDetails.DeductionLoadFactor;
                }
                deptVerificationDetails.WorkFlowDetails = _INBApplicationbll.GetWorkFlowDetails(applicationId);
                return View("CWVerification", deptVerificationDetails);
            }
            else
            {
                return RedirectToAction("DetailsForCaseWorkerVerification", "VerifyDetails");
            }
        }

        [Route("SaveCWVData")]
        public string InsertCWVerifiedDetails(VM_DeptVerificationDetails caseWorkerVerifiedDetails)
        {
            if (Session["UID"] != null)
            {
                string result = "";
                caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
                caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;

                if (caseWorkerVerifiedDetails.ApplicationStatus == 7)
                {
                    // var assignedto = (from kw in _db.tbl_kgid_application_workflow_details where kw.kawt_application_id == caseWorkerVerifiedDetails.ApplicationId && kw.kawt_active_status == true select kw.kawt_assigned_to).FirstOrDefault();
                    //var assignedto = (from kw in _db.tbl_assign_application where kw.kawt_application_id == caseWorkerVerifiedDetails.ApplicationId && kw.kawt_active_status == true select kw.kawt_assigned_to).FirstOrDefault();


                        result = _INBApplicationbll.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
                        //Save FacingSheet
                        VM_FacingSheet facingSheet1 = new VM_FacingSheet();
                        facingSheet1 = policyBLL.GetFacingSheetDetails(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode);
                        string FSfilepath = FillFacingSheet(facingSheet1, caseWorkerVerifiedDetails.ApplicationRefNo);
                        string FSresult = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode, FSfilepath, "FacingSheet");
                    // Save FacingSheet END                   
                   
                }
                if (Convert.ToInt32(result) > 0)
                {
                    return result;
                    //  return RedirectToRoute("/kgid-cw/");
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        #endregion Case Worker

        #region AVG Case Worker

        [Route("kgid-avg-cw")]
        [CustomAuthorize("AVG Caseworker")]
        public ActionResult DetailsForAVGCaseWorkerVerification()
        {
            ViewBag.Verifier = Verifiers.AVGCW;
            VM_VerificationDetails verificationDetails = _INBApplicationbll.GetEmployeeDetailsForAVGCWVerification(Convert.ToInt64(Session["UID"]));
            Thread.Sleep(2000);
            Logger.LogMessage(TracingLevel.INFO, "Pending Issues AVGC " + verificationDetails.PendingApplications.ToString());
            Logger.LogMessage(TracingLevel.INFO, "Total Issues AVGC " + verificationDetails.TotalReceived.ToString());
            return View("VerificationDetails", verificationDetails);
        }

        [Route("kgid-avg-cw-verification")]
        [CustomAuthorize("AVG Caseworker")]
        public ActionResult GetApplicationFormForAVGCW(long empId = 0, long applicationId = 0)
        {
            if (empId != 0 && applicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = applicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                applicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                if (empId != 0)
                {
                    Session["RUID"] = empId;
                }
                VM_DeptVerificationDetails deptVerificationDetails = _INBApplicationbll.GetMedicalLeaveDetails(empId, applicationId);
                deptVerificationDetails.WorkFlowDetails = _INBApplicationbll.GetWorkFlowDetails(applicationId);
                return View("AVGCWVerification", deptVerificationDetails);
            }
            else
            {
                return RedirectToAction("DetailsForAVGCaseWorkerVerification", "VerifyDetails");
            }
        }

        [Route("SaveAVGCWVData")]
        public string InsertAVGCWVerifiedDetails(VM_DeptVerificationDetails caseWorkerVerifiedDetails)
        {
            if (Session["UID"] != null)
            {
                caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
                caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
                string result = _INBApplicationbll.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
                if (Convert.ToInt32(result) > 0)
                {
                    return result;
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        #endregion AVG Case Worker

        #region Superintendent

        [Route("kgid-si")]
        [CustomAuthorize("Superintendent")]
        public ActionResult DetailsForSuperintendentVerification()
        {
            ViewBag.Verifier = Verifiers.SUPERINTENDENT;
            VM_VerificationDetails verificationDetails = _INBApplicationbll.GetEmployeeDetailsForSIVerification(Convert.ToInt64(Session["UID"]));
            Thread.Sleep(2000);
            Logger.LogMessage(TracingLevel.INFO, "Pending Issues SI " + verificationDetails.PendingApplications.ToString());
            Logger.LogMessage(TracingLevel.INFO, "Total Issues SI " + verificationDetails.TotalReceived.ToString());
            return View("VerificationDetails", verificationDetails);
        }

        [Route("kgid-si-verification")]
        [CustomAuthorize("Superintendent")]
        public ActionResult GetApplicationFormForSuperintendent(long empId = 0, long applicationId = 0)
        {
            if (empId != 0 && applicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = applicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                applicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                if (empId != 0)
                {
                    Session["RUID"] = empId;
                }
                //VM_DeptVerificationDetails deptVerificationDetails = _INBApplicationbll.GetPreviousVerificationDetails(Convert.ToInt64(empId));
                VM_DeptVerificationDetails deptVerificationDetails = _INBApplicationbll.GetMedicalLeaveDetails(empId, applicationId);
                VM_DeptVerificationDetails policyCalculationDetails = _INBApplicationbll.GetPolicyCalculations(empId, applicationId);
                if (policyCalculationDetails != null)
                {
                    deptVerificationDetails.LoadFactor = policyCalculationDetails.LoadFactor;
                    deptVerificationDetails.SumAssured = policyCalculationDetails.SumAssured;
                    deptVerificationDetails.DeductionLoadFactors = policyCalculationDetails.DeductionLoadFactors;
                    deptVerificationDetails.DeductionLoadFactor = policyCalculationDetails.DeductionLoadFactor;
                }
                deptVerificationDetails.WorkFlowDetails = _INBApplicationbll.GetWorkFlowDetails(applicationId);
                return View("SIVerification", deptVerificationDetails);
            }
            else
            {
                return RedirectToAction("DetailsForSuperintendentVerification", "VerifyDetails");
            }
        }

        [Route("SaveSIVData")]
        public string InsertSuperintendentVerifiedDetails(VM_DeptVerificationDetails caseWorkerVerifiedDetails)
        {
            if (Session["UID"] != null)
            {
                string result = "";
                caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
                caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
                if (caseWorkerVerifiedDetails.ApplicationStatus == 9)
                {
                    
                    result = _INBApplicationbll.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
                    //Save FacingSheet
                    VM_FacingSheet facingSheet1 = new VM_FacingSheet();
                    facingSheet1 = policyBLL.GetFacingSheetDetails(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode);
                    string FSfilepath = FillFacingSheet(facingSheet1, caseWorkerVerifiedDetails.ApplicationRefNo);
                    string FSresult = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode, FSfilepath, "FacingSheet");
                    // Save FacingSheet END                    
                }
                if (Convert.ToInt32(result) > 0)
                {
                    return result;
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        #endregion Superintendent

        #region DIO

        [Route("kgid-dio")]
        [CustomAuthorize("DIO")]
        public ActionResult DetailsForDIOVerification()
        {
            ViewBag.Verifier = Verifiers.DIO;
            VM_VerificationDetails verificationDetails = _INBApplicationbll.GetEmployeeDetailsForDIOVerification(Convert.ToInt64(Session["UID"]));
            Thread.Sleep(2000);
            Logger.LogMessage(TracingLevel.INFO, "Pending Issues DIO " + verificationDetails.PendingApplications.ToString());
            Logger.LogMessage(TracingLevel.INFO, "Total Issues DIO " + verificationDetails.TotalReceived.ToString());
            return View("VerificationDetails", verificationDetails);
        }

        [Route("kgid-dio-verification")]
        [CustomAuthorize("DIO")]
        public ActionResult GetApplicationFormForDIO(long empId = 0, long applicationId = 0)
        {
            if (empId != 0 && applicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = applicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                applicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                if (empId != 0)
                {
                    Session["RUID"] = empId;
                }
                //VM_DeptVerificationDetails deptVerificationDetails = _INBApplicationbll.GetPreviousVerificationDetails(Convert.ToInt64(empId));
                VM_DeptVerificationDetails deptVerificationDetails = _INBApplicationbll.GetMedicalLeaveDetails(empId, applicationId);
                VM_DeptVerificationDetails policyCalculationDetails = _INBApplicationbll.GetPolicyCalculations(empId, applicationId);
                if (policyCalculationDetails != null)
                {
                    deptVerificationDetails.LoadFactor = policyCalculationDetails.LoadFactor;
                    deptVerificationDetails.SumAssured = policyCalculationDetails.SumAssured;
                    deptVerificationDetails.DeductionLoadFactors = policyCalculationDetails.DeductionLoadFactors;
                    deptVerificationDetails.DeductionLoadFactor = policyCalculationDetails.DeductionLoadFactor;
                }
                deptVerificationDetails.WorkFlowDetails = _INBApplicationbll.GetWorkFlowDetails(applicationId);
                return View("DIOVerification", deptVerificationDetails);
            }
            else
            {
                return RedirectToAction("DetailsForDIOVerification", "VerifyDetails");
            }
        }

        [Route("SaveDIOVData")]
        public ActionResult InsertDIOVerifiedDetails(VM_DeptVerificationDetails caseWorkerVerifiedDetails)
        {
            if (Session["UID"] != null)
            {
                try
                {
                    caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
                    caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
                    string returnString = _INBApplicationbll.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
                    Logger.LogMessage(TracingLevel.INFO, "//NB Bond Policy Number After Approve:- " + returnString);
                    //string returnString = "BNG12136536215633";
                    //Save FacingSheet
                    VM_FacingSheet facingSheet1 = new VM_FacingSheet();
                    facingSheet1 = policyBLL.GetFacingSheetDetails(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode);
                    string FSfilepath = FillFacingSheet(facingSheet1, caseWorkerVerifiedDetails.ApplicationRefNo);
                    string FSresult = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode, FSfilepath, "FacingSheet");
                    // Save FacingSheet END

                    string empname = (from bd in _db.tbl_employee_basic_details where bd.employee_id == caseWorkerVerifiedDetails.EmpCode select bd.employee_name).FirstOrDefault();
                    if (caseWorkerVerifiedDetails.ApplicationStatus != 15)//&& Convert.ToInt32(returnString) > 0
                    {

                        if (caseWorkerVerifiedDetails.ApplicationStatus == 4)
                        {
                            var ddoempid = (from eb in _db.tbl_employee_work_details where eb.ewd_emp_id == caseWorkerVerifiedDetails.EmpCode select eb.ewd_ddo_id).FirstOrDefault();

                            var ddomobileno = (from ebd in _db.tbl_employee_basic_details
                                               join wd in _db.tbl_employee_work_details on ebd.employee_id equals wd.ewd_emp_id
                                               where ebd.user_category_id.Contains("1,2") && wd.ewd_ddo_id == ddoempid
                                               select ebd.mobile_number).FirstOrDefault();
                            // var ddoempid = (from wf in _db.tbl_kgid_application_workflow_details where wf.kawt_application_id == caseWorkerVerifiedDetails.ApplicationId && wf.kawt_active_status == true select wf.kawt_assigned_to).FirstOrDefault();
                            //var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == ddoempid select eb.mobile_number).FirstOrDefault();

                            string msg = "ವೇತನ ಬಟವಾಡೆ ಅಧಿಕಾರಿಗಳೇ, ಶ್ರೀ/ಶ್ರೀಮತಿ " + empname + " ರವರ" + facingSheet1.ApplicationNumber + " ಸಂಖ್ಯೆಯ ವಿಮಾ ಪ್ರಸ್ತಾವನೆಯು ಅಗತ್ಯ ಮಾಹಿತಿ ಕೋರಿ ಆಕ್ಷೇಪಗೊಂಡಿದೆ.  ಆಕ್ಷೇಪಿಸಿದ ವಿವರಕ್ಕಾಗಿ https://kgidonline.karnataka.gov.in ಗೆ ಲಾಗಿನ್ ಆಗಿ ತಿಳಿದು ಮುಂದಿನ ಕ್ರಮ ಕೈಗೊಳ್ಳಲು ಕೋರಲಾಗಿದೆ."
                        + "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                            //AllCommon.sendOTPMSG(ddomobileno.ToString(), msg);
                        }
                        return Json(new { PolicyNumber = string.Empty,Result= returnString, RedirectUrl = "/kgid-dio/" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (caseWorkerVerifiedDetails.ApplicationStatus == 15 && !string.IsNullOrEmpty(returnString))
                    {
                        //CREATE AND SAVE NB BOND
                        string result = string.Empty;
                        long PolicyNo = Convert.ToInt64(caseWorkerVerifiedDetails.ApplicationRefNo);//Application ID
                        Logger.LogMessage(TracingLevel.INFO, "//NB Bond Details Application ID:- " + PolicyNo);
                        long EmployeeId = caseWorkerVerifiedDetails.EmpCode;
                        //int PolicyNo = 40335;
                        VM_NBBond _empNBBond = new VM_NBBond();
                        _empNBBond = _Objemployee.getNBBondDetails(PolicyNo);
                        Logger.LogMessage(TracingLevel.INFO, "//NB Bond Details test:- " + _empNBBond.EmployeeBasicDetails.employee_name);
                        string filepath = FillFormNB(_empNBBond, returnString);
                        //string filepath = FillFormNBDocPdf(_empNBBond, returnString);

                        Logger.LogMessage(TracingLevel.INFO, "//NB Bond Return Path:- " + filepath);
                        result = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, _empNBBond.EmployeeBasicDetails.employee_id, filepath, "NBBOND");

                        //CREATE AND SAVE NB BOND END

                        //Save Approved FacingSheet
                        VM_FacingSheet facingSheetaprvd = new VM_FacingSheet();
                        facingSheetaprvd = policyBLL.GetFacingSheetDetails(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode);
                        string FSfilepathaprvd = FillApprovedFacingSheet(facingSheetaprvd, caseWorkerVerifiedDetails.ApplicationRefNo);
                        string FSresultaprvd = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode, FSfilepathaprvd, "FacingSheet");
                        Logger.LogMessage(TracingLevel.INFO, "//Facing Sheet After Approve Path:- " + FSfilepathaprvd);
                        // Save FacingSheet END
                        //string signedfilepath = NBSignPdf(filepath, returnString,caseWorkerVerifiedDetails.PublicKey);
                        //if (signedfilepath != null)
                        //{15
                        //    FileInfo file = new FileInfo(filepath);
                        //   file.Delete();
                        //   result = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, _empNBBond.EmployeeBasicDetails.employee_id, signedfilepath, "NBBOND");
                        //}
                        try
                        {
                            //string Month = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).AddMonths(1).ToString("MMMM");
                            //string Year = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).ToString("yy");
                            string[] Temp = Convert.ToString(_empNBBond.EmployeeBasicDetails.p_sanction_date).Split('-');
                            var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == caseWorkerVerifiedDetails.EmpCode select eb).FirstOrDefault();
                            string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + facingSheetaprvd.ApplicationNumber + " ಯು ಅಂಗೀಕಾರವಾಗಿದ್ದು, ನಿಮ್ಮ ಪಾಲಿಸಿ ಸಂಖ್ಯೆ " + returnString + " ಆಗಿರುತ್ತದೆ.ಪಾಲಿಸಿ ಬಾಂಡ್‌ ಮತ್ತು ಅಂಗೀಕಾರದ ಸೂಚನೆಗಳನ್ನು ನಿಮ್ಮ ರಿಜಿಸ್ಟರ್ಡ್‌ ಇ-ಮೇಲ್‌ / KGID ಲಾಗಿನ್ ನಲ್ಲಿ ಡೌನ್‌ಲೋಡ್‌ ಮಾಡಿಕೊಳ್ಳಬಹುದಾಗಿದೆ. ಮುಂದುವರೆದು, ರೂ " + _empNBBond.EmployeeBasicDetails.p_premium + " ಗಳನ್ನು " + Convert.ToInt32(Temp[1]) + 1 + "/" + Temp[2] + " ತಿಂಗಳು / ವರ್ಷ ದಿಂದ ಕ್ರಮವಾಗಿ ತಮ್ಮ ವೇತನದಿಂದ ಕಟಾವಣೆ ಮಾಡಿಸತಕ್ಕದ್ದು."
                                     + " - ವಿಮಾ ಇಲಾಖೆ(KGID).";

                            //AllCommon.sendOTPMSG(mobile.ToString(), msg);
                            //AllCommon.SendEmailWithAttachment
                            //AllCommon objemail = new AllCommon();
                            //objemail.SendEmailWithAttachment(mobile.email_id, msg, "KGID Policy Bond.", filepath); 
                        }
                        catch (Exception ex)
                        {
                            Logger.LogMessage(TracingLevel.INFO, "DIO Approve Policy Message catch" + ex.Message);
                        }

                        return Json(new { PolicyNumber = returnString, RedirectUrl = "/kgid-dio/" }, JsonRequestBehavior.AllowGet);
                        //return Json(new { PolicyNumber = returnString, UnSignedBond = filepath, AppId = PolicyNo, EmpId = EmployeeId, RedirectUrl = "/kgid-dio/" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        #endregion DIO

        [Route("kgid-view-app")]
        [CustomAuthorize("Employee")]
        public ActionResult ViewApplicationDetails()
        {
            VM_DDOVerificationDetails verificationDetails = _Objemployee.GetEmployeeApplicationStatusBll(Convert.ToInt64(Session["UId"]));

            return View(verificationDetails);
        }

        #region Deputy Director

        [Route("kgid-dd")]
        [CustomAuthorize("Deputy Director")]
        public ActionResult DetailsForDDVerification()
        {
            ViewBag.Verifier = Verifiers.DEPUTYDIRECTOR;
            VM_VerificationDetails verificationDetails = _INBApplicationbll.GetEmployeeDetailsForDDVerification(Convert.ToInt64(Session["UID"]));
            Thread.Sleep(2000);
            Logger.LogMessage(TracingLevel.INFO, "Pending Issues DD " + verificationDetails.PendingApplications.ToString());
            Logger.LogMessage(TracingLevel.INFO, "Total Issues DD " + verificationDetails.TotalReceived.ToString());
            return View("VerificationDetails", verificationDetails);
        }

        [Route("kgid-dd-verification")]
        [CustomAuthorize("Deputy Director")]
        public ActionResult GetApplicationFormForDD(long empId = 0, long applicationId = 0)
        {
            if (empId != 0 && applicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = applicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                applicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                if (empId != 0)
                {
                    Session["RUID"] = empId;
                }

                VM_DeptVerificationDetails deptVerificationDetails = _INBApplicationbll.GetMedicalLeaveDetails(empId, applicationId);
                VM_DeptVerificationDetails policyCalculationDetails = _INBApplicationbll.GetPolicyCalculations(empId, applicationId);
                if (policyCalculationDetails != null)
                {
                    deptVerificationDetails.LoadFactor = policyCalculationDetails.LoadFactor;
                    deptVerificationDetails.SumAssured = policyCalculationDetails.SumAssured;
                    deptVerificationDetails.DeductionLoadFactors = policyCalculationDetails.DeductionLoadFactors;
                    deptVerificationDetails.DeductionLoadFactor = policyCalculationDetails.DeductionLoadFactor;
                }
                deptVerificationDetails.WorkFlowDetails = _INBApplicationbll.GetWorkFlowDetails(applicationId);
                return View("DDVerification", deptVerificationDetails);
            }
            else
            {
                return RedirectToAction("DetailsForDDVerification", "VerifyDetails");
            }
        }

        [Route("SaveDDVData")]
        public ActionResult InsertDDVerifiedDetails(VM_DeptVerificationDetails caseWorkerVerifiedDetails)
        {
            if (Session["UID"] != null)
            {
                caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
                caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
                string returnString = _INBApplicationbll.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
                //Save FacingSheet
                VM_FacingSheet facingSheet1 = new VM_FacingSheet();
                facingSheet1 = policyBLL.GetFacingSheetDetails(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode);
                string FSfilepath = FillFacingSheet(facingSheet1, caseWorkerVerifiedDetails.ApplicationRefNo);
                string FSresult = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode, FSfilepath, "FacingSheet");
                // Save FacingSheet END
                if (caseWorkerVerifiedDetails.ApplicationStatus != 15 && Convert.ToInt32(returnString) > 0)
                {
                    return Json(new { PolicyNumber = string.Empty,Result=returnString, RedirectUrl = "/kgid-dd/" }, JsonRequestBehavior.AllowGet);
                }
                else if (caseWorkerVerifiedDetails.ApplicationStatus == 15 && !string.IsNullOrEmpty(returnString))
                {
                    //Save Approved FacingSheet
                    VM_FacingSheet facingSheetaprvd = new VM_FacingSheet();
                    facingSheetaprvd = policyBLL.GetFacingSheetDetails(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode);
                    string FSfilepathaprvd = FillApprovedFacingSheet(facingSheetaprvd, caseWorkerVerifiedDetails.ApplicationRefNo);
                    string FSresultaprvd = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode, FSfilepathaprvd, "FacingSheet");
                    // Save FacingSheet END
                    string result = string.Empty;
                    int PolicyNo = Convert.ToInt32(caseWorkerVerifiedDetails.ApplicationRefNo);//Application ID
                    long EmployeeId = caseWorkerVerifiedDetails.EmpCode;
                    VM_NBBond _empNBBond = new VM_NBBond();
                    _empNBBond = _Objemployee.getNBBondDetails(PolicyNo);
                    Logger.LogMessage(TracingLevel.INFO, "//NB Bond Details test:- " + _empNBBond.EmployeeBasicDetails.employee_name);
                    string filepath = FillFormNB(_empNBBond, returnString);
                    //string filepath = FillFormNBDocPdf(_empNBBond, returnString);
                    result = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, _empNBBond.EmployeeBasicDetails.employee_id, filepath, "NBBOND");
                    //string signedfilepath = NBSignPdf(filepath, returnString, caseWorkerVerifiedDetails.PublicKey);
                    //if (signedfilepath != null)
                    //{
                    //    FileInfo file = new FileInfo(filepath);
                    //    file.Delete();
                    //    result = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, _empNBBond.EmployeeBasicDetails.employee_id, signedfilepath, "NBBOND");
                    //}
                    var ddoempid = (from eb in _db.tbl_employee_work_details where eb.ewd_emp_id == caseWorkerVerifiedDetails.EmpCode select eb.ewd_ddo_id).FirstOrDefault();

                    var ddomobileno = (from ebd in _db.tbl_employee_basic_details
                                       join wd in _db.tbl_employee_work_details on ebd.employee_id equals wd.ewd_emp_id
                                       where ebd.user_category_id.Contains("1,2") && wd.ewd_ddo_id == ddoempid
                                       select ebd.mobile_number).FirstOrDefault();
                    //string Month = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).AddMonths(1).ToString("MMMM");
                    //string Year = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).ToString("yy");
                    string[] Temp = Convert.ToString(_empNBBond.EmployeeBasicDetails.p_sanction_date).Split('-');
                    var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == caseWorkerVerifiedDetails.EmpCode select eb).FirstOrDefault();
                    string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + facingSheetaprvd.ApplicationNumber + " ಯು ಅಂಗೀಕಾರವಾಗಿದ್ದು, ನಿಮ್ಮ ಪಾಲಿಸಿ ಸಂಖ್ಯೆ " + returnString + " ಆಗಿರುತ್ತದೆ.ಪಾಲಿಸಿ ಬಾಂಡ್‌ ಮತ್ತು ಅಂಗೀಕಾರದ ಸೂಚನೆಗಳನ್ನು ನಿಮ್ಮ ರಿಜಿಸ್ಟರ್ಡ್‌ ಇ-ಮೇಲ್‌ / KGID ಲಾಗಿನ್ ನಲ್ಲಿ ಡೌನ್‌ಲೋಡ್‌ ಮಾಡಿಕೊಳ್ಳಬಹುದಾಗಿದೆ. ಮುಂದುವರೆದು, ರೂ " + _empNBBond.EmployeeBasicDetails.p_premium + " ಗಳನ್ನು " + Convert.ToInt32(Temp[1]) + 1  + "/" + Temp[2] + " ತಿಂಗಳು / ವರ್ಷ ದಿಂದ ಕ್ರಮವಾಗಿ ತಮ್ಮ ವೇತನದಿಂದ ಕಟಾವಣೆ ಮಾಡಿಸತಕ್ಕದ್ದು."
                             + " - ವಿಮಾ ಇಲಾಖೆ(KGID).";
                    //AllCommon.sendOTPMSG(mobile.ToString(), msg);
                    string ddomsg = "ವೇತನ ಬಟವಾಡೆ ಅಧಿಕಾರಿಗಳೇ, ಶ್ರೀ/ಶ್ರೀಮತಿ" + mobile.employee_name + " ರವರ" + facingSheetaprvd.ApplicationNumber + " ಸಂಖ್ಯೆಯ ವಿಮಾ ಪ್ರಸ್ತಾವನೆಯು ಅಂಗೀಕಾರಗೊಂಡಿದ್ದು, ಪಾಲಿಸಿ ಸಂಖ್ಯೆ" + returnString + "ರ ವಿಮಾಕಂತು ರೂ" + _empNBBond.EmployeeBasicDetails.p_premium + "ಗಳನ್ನು" + Convert.ToInt32(Temp[1]) + 1 + "/" + Temp[2] + " ತಿಂಗಳು/ ವರ್ಷ ದಿಂದ ಕ್ರಮವಾಗಿ ಇವರ ವೇತನದಿಂದ ಕಟಾಯಿಸುವುದು."
    + "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                    //AllCommon.sendOTPMSG(ddomobileno.ToString(), ddomsg);
                    //AllCommon objemail = new AllCommon();
                    // objemail.SendEmailWithAttachment(mobile.email_id, msg, "KGID Policy Bond.", filepath);
                    return Json(new { PolicyNumber = returnString, RedirectUrl = "/kgid-dd/" }, JsonRequestBehavior.AllowGet);
                    //return Json(new { PolicyNumber = returnString, UnSignedBond = filepath, AppId = PolicyNo, EmpId = EmployeeId, RedirectUrl = "/kgid-dd/" }, JsonRequestBehavior.AllowGet);            }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        #endregion Deputy Director

        #region Director

        [Route("kgid-d")]
        [CustomAuthorize("Director")]
        public ActionResult DetailsForDVerification()
        {
            ViewBag.Verifier = Verifiers.DIRECTOR;
            VM_VerificationDetails verificationDetails = _INBApplicationbll.GetEmployeeDetailsForDVerification(Convert.ToInt64(Session["UID"]));
            Thread.Sleep(2000);
            Logger.LogMessage(TracingLevel.INFO, "Pending Issues D " + verificationDetails.PendingApplications.ToString());
            Logger.LogMessage(TracingLevel.INFO, "Total Issues D " + verificationDetails.TotalReceived.ToString());
            return View("VerificationDetails", verificationDetails);
        }

        [Route("kgid-d-verification")]
        [CustomAuthorize("Director")]
        public ActionResult GetApplicationFormForD(long empId = 0, long applicationId = 0)
        {
            if (empId != 0 && applicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = applicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                applicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                if (empId != 0)
                {
                    Session["RUID"] = empId;
                }

                VM_DeptVerificationDetails deptVerificationDetails = _INBApplicationbll.GetMedicalLeaveDetails(empId, applicationId);
                VM_DeptVerificationDetails policyCalculationDetails = _INBApplicationbll.GetPolicyCalculations(empId, applicationId);
                if (policyCalculationDetails != null)
                {
                    deptVerificationDetails.LoadFactor = policyCalculationDetails.LoadFactor;
                    deptVerificationDetails.SumAssured = policyCalculationDetails.SumAssured;
                    deptVerificationDetails.DeductionLoadFactors = policyCalculationDetails.DeductionLoadFactors;
                    deptVerificationDetails.DeductionLoadFactor = policyCalculationDetails.DeductionLoadFactor;
                }
                deptVerificationDetails.WorkFlowDetails = _INBApplicationbll.GetWorkFlowDetails(applicationId);
                return View("DVerification", deptVerificationDetails);
            }
            else
            {
                return RedirectToAction("DetailsForDVerification", "VerifyDetails");
            }
        }

        [Route("SaveDVData")]
        public ActionResult InsertDVerifiedDetails(VM_DeptVerificationDetails caseWorkerVerifiedDetails)
        {
            if (Session["UID"] != null)
            {
                caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
                caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
                string returnString = _INBApplicationbll.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
                //Save FacingSheet
                VM_FacingSheet facingSheet1 = new VM_FacingSheet();
                facingSheet1 = policyBLL.GetFacingSheetDetails(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode);
                string FSfilepath = FillFacingSheet(facingSheet1, caseWorkerVerifiedDetails.ApplicationRefNo);
                string FSresult = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode, FSfilepath, "FacingSheet");
                // Save FacingSheet END
                if (caseWorkerVerifiedDetails.ApplicationStatus != 15 && Convert.ToInt32(returnString) > 0)
                {
                    return Json(new { PolicyNumber = string.Empty, RedirectUrl = "/kgid-d/" }, JsonRequestBehavior.AllowGet);
                }
                else if (caseWorkerVerifiedDetails.ApplicationStatus == 15 && !string.IsNullOrEmpty(returnString))
                {
                    //Save Approved FacingSheet
                    VM_FacingSheet facingSheetaprvd = new VM_FacingSheet();
                    facingSheetaprvd = policyBLL.GetFacingSheetDetails(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode);
                    string FSfilepathaprvd = FillApprovedFacingSheet(facingSheetaprvd, caseWorkerVerifiedDetails.ApplicationRefNo);
                    string FSresultaprvd = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, caseWorkerVerifiedDetails.EmpCode, FSfilepathaprvd, "FacingSheet");
                    // Save FacingSheet END
                    string result = string.Empty;
                    int PolicyNo = Convert.ToInt32(caseWorkerVerifiedDetails.ApplicationRefNo);//Application ID
                    long EmployeeId = caseWorkerVerifiedDetails.EmpCode;
                    VM_NBBond _empNBBond = new VM_NBBond();
                    _empNBBond = _Objemployee.getNBBondDetails(PolicyNo);
                    Logger.LogMessage(TracingLevel.INFO, "//NB Bond Details test:- " + _empNBBond.EmployeeBasicDetails.employee_name);
                    string filepath = FillFormNB(_empNBBond, returnString);
                    //string filepath = FillFormNBDocPdf(_empNBBond, returnString);
                    result = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, _empNBBond.EmployeeBasicDetails.employee_id, filepath, "NBBOND");
                    //string signedfilepath = NBSignPdf(filepath, returnString, caseWorkerVerifiedDetails.PublicKey);
                    //if (signedfilepath != null)
                    //{
                    //   FileInfo file = new FileInfo(filepath);
                    //    file.Delete();
                    //   result = policyBLL.NBBondFacingDocUploadBLL(caseWorkerVerifiedDetails.ApplicationRefNo, _empNBBond.EmployeeBasicDetails.employee_id, signedfilepath, "NBBOND");
                    //}
                    //string Month = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).AddMonths(1).ToString("MMMM");
                    //string Year = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).ToString("yy");
                    string[] Temp = Convert.ToString(_empNBBond.EmployeeBasicDetails.p_sanction_date).Split('-');
                    var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == caseWorkerVerifiedDetails.EmpCode select eb).FirstOrDefault();
                    string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + facingSheetaprvd.ApplicationNumber + " ಯು ಅಂಗೀಕಾರವಾಗಿದ್ದು, ನಿಮ್ಮ ಪಾಲಿಸಿ ಸಂಖ್ಯೆ " + returnString + " ಆಗಿರುತ್ತದೆ.ಪಾಲಿಸಿ ಬಾಂಡ್‌ ಮತ್ತು ಅಂಗೀಕಾರದ ಸೂಚನೆಗಳನ್ನು ನಿಮ್ಮ ರಿಜಿಸ್ಟರ್ಡ್‌ ಇ-ಮೇಲ್‌ / KGID ಲಾಗಿನ್ ನಲ್ಲಿ ಡೌನ್‌ಲೋಡ್‌ ಮಾಡಿಕೊಳ್ಳಬಹುದಾಗಿದೆ. ಮುಂದುವರೆದು, ರೂ " + _empNBBond.EmployeeBasicDetails.p_premium + " ಗಳನ್ನು " + Convert.ToInt32(Temp[1]) + 1 + "/" + Temp[2] + " ತಿಂಗಳು / ವರ್ಷ ದಿಂದ ಕ್ರಮವಾಗಿ ತಮ್ಮ ವೇತನದಿಂದ ಕಟಾವಣೆ ಮಾಡಿಸತಕ್ಕದ್ದು."
                             + " - ವಿಮಾ ಇಲಾಖೆ(KGID).";
                    // AllCommon.sendOTPMSG(mobile.mobile_number.ToString(), msg);
                    string emailmsg = "Dear Insured," + "\r\n"
      + "We are glad to Inform you that, your proposal with reference no " + facingSheetaprvd.ApplicationNumber + " has been accepted and KGID Policy bond has been generated on " + _empNBBond.EmployeeBasicDetails.p_sanction_date + "\r\n"
      + " The Policy details are as follows: KGID Policy No " + returnString
      + " Please find the KGID Policy Bond and the acceptance notice attached."
      + "This document is digitally signed ,hence does not require physical signature."
      + " Warm Regards,"
      + "KGID, Official Branch";
                    AllCommon objemail = new AllCommon();
                    objemail.SendEmail(mobile.email_id, emailmsg, "KGID Policy Bond.");
                    // objemail.SendEmailWithAttachment(mobile.email_id, msg, "KGID Policy Bond.", filepath);
                    return Json(new { PolicyNumber = returnString, RedirectUrl = "/kgid-d/" }, JsonRequestBehavior.AllowGet);
                    //return Json(new { PolicyNumber = returnString, UnSignedBond = filepath, AppId = PolicyNo, EmpId = EmployeeId, RedirectUrl = "/kgid-d/" }, JsonRequestBehavior.AllowGet);            }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        #endregion Director

        public string GetRemarkComments(int RemarkID)
        {
            string comment = _Iremarklistbll.GetRemarkComments(RemarkID);
            return comment;
        }
        [Route("VIEW-NBB-FS")]
        public ActionResult ViewNBBondFacingsheet()
        {
            VM_DDOVerificationDetails verificationDetails = _Objemployee.GetEmployeeNBBondFacingSheetBll(Convert.ToInt64(Session["UId"]));

            return View(verificationDetails);
        }
        #region Facing Sheet Generation
        private string FillFacingSheet(VM_FacingSheet facingSheet, long result)
        {
            /////Verification Details
            if (facingSheet.CWName == "" && facingSheet.SIName == "" && facingSheet.DIOName == "" && facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.CWName = "";
                facingSheet.CWVDate = "";
                facingSheet.SIName = "";
                facingSheet.SIVDate = "";
                facingSheet.DIOName = "";
                facingSheet.DIOVDate = "";
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.SIName == "" && facingSheet.DIOName == "" && facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.SIName = "";
                facingSheet.SIVDate = "";
                facingSheet.DIOName = "";
                facingSheet.DIOVDate = "";
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DIOName == "" && facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.DIOName = "";
                facingSheet.DIOVDate = "";
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DName == "")
            {
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            //////////
            if (facingSheet.DIOName == facingSheet.DDName && facingSheet.DDName == facingSheet.DName)
            {
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DDName == facingSheet.DName)
            {
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            ////////////////////////////////////////////
            string pdfTemplate = Server.MapPath("~/PdfTemplate/FacingSheet/Facingsheet_Template_Kannada.pdf");
            //string newFile = Server.MapPath("~/PdfTemplate/FacingSheet/" + result + "UnSigned" + ".pdf");
            //string newFile = @"C:/Documents/PdfTemplate/FacingSheet/" + result + "UnSigned" + ".pdf";
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\FacingSheet\" + result + "UnSigned" + ".pdf";
            }
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            {//Facing Sheet Details
                var date1 = facingSheet.DateOfIssue?.ToString("dd-MM-yyyy");
                string date = DateTime.Now.ToShortDateString();
                fields.SetField("ApplicationRefNo", facingSheet.ApplicationNumber.ToString().Trim().ToUpper());
                fields.SetField("PolicyNumber", facingSheet.PolicyNumber);
                fields.SetFieldProperty("DIO", "textsize", 10.5f, null);
                fields.SetField("DIO", facingSheet.DistrictInsuranceOfficeAddress);
                fields.SetField("EmployeeName", facingSheet.InsurerName.ToString().Trim().ToUpper());
                //fields.SetField("DepositDate", facingSheet.InitialDeposit.ToString().Trim().ToUpper());
                fields.SetField("SubmittedBy", facingSheet.InsurerName.ToString().Trim().ToUpper());
                fields.SetField("DepositAmount", facingSheet.MonthlyPremium.ToString().Trim().ToUpper());
                //fields.SetField("CWDate", facingSheet.DateOfLiability);
                //fields.SetField("SIDDDate", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy"));
                //fields.SetField("KeyPoints", KeyPoints);
                //fields.SetField("OrderofPassage", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy").Trim().ToUpper());
                fields.SetField("ChallanRefNumber", facingSheet.ChallanRefNo);
                fields.SetField("ChallanDate", facingSheet.ChallanDate);
                fields.SetField("DateofRisk", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy"));
                fields.SetField("DOB", facingSheet.DateOfBirth.ToString("dd-MM-yyyy"));
                fields.SetFieldProperty("InsurerAge", "textsize", 10.5f, null);
                fields.SetField("InsurerAge", facingSheet.Age.ToString().Trim().ToUpper());
                fields.SetField("MonthlyPremium", facingSheet.MonthlyPremium.ToString().Trim().ToUpper());
                fields.SetField("SumAssuredAmount", facingSheet.InsuranceAmount.ToString().Trim().ToUpper());
                fields.SetField("MaturityMonthYear", facingSheet.EffectiveMonthYear);
                fields.SetField("DateofIssuePB", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy"));
                //fields.SetField("DIODate", date);
                /////Verification Details--added by venkatesh
                fields.SetField("DDOName", facingSheet.DDOName);
                fields.SetField("DDOVDate", facingSheet.DDOVDate);
                fields.SetField("CWName", facingSheet.CWName);
                fields.SetField("CWVDate", facingSheet.CWVDate);
                fields.SetField("SIName", facingSheet.SIName);
                fields.SetField("SIVDate", facingSheet.SIVDate);
                fields.SetField("DIOName", facingSheet.DIOName);
                fields.SetField("DIOVDate", facingSheet.DIOVDate);
                fields.SetField("DDName", facingSheet.DDName);
                fields.SetField("DDVDate", facingSheet.DDVDate);
                fields.SetField("DName", facingSheet.DName);
                fields.SetField("DVDate", facingSheet.DVDate);
            }
            if (facingSheet.Policies.Count == 1)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
            }
            if (facingSheet.Policies.Count == 2)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
            }
            if (facingSheet.Policies.Count == 3)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
            }
            if (facingSheet.Policies.Count == 4)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
            }
            if (facingSheet.Policies.Count == 5)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
            }
            if (facingSheet.Policies.Count == 6)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
                fields.SetField("Policy6", facingSheet.Policies[5]);
            }
            if (facingSheet.Policies.Count == 7)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
                fields.SetField("Policy6", facingSheet.Policies[5]);
                fields.SetField("Policy7", facingSheet.Policies[6]);
            }
            if (facingSheet.Policies.Count == 8)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
                fields.SetField("Policy6", facingSheet.Policies[5]);
                fields.SetField("Policy7", facingSheet.Policies[6]);
                fields.SetField("Policy8", facingSheet.Policies[7]);
            }
            pdfStamper.Close();
            return newFile;
        }
        private string FillApprovedFacingSheet(VM_FacingSheet facingSheet, long result)
        {
            string KeyPoints = string.Empty;
            if (facingSheet.LoadFactor == null || facingSheet.LoadFactor == "")
            {
                facingSheet.LoadFactor = "none";
            }
            if (facingSheet.DLFactor == null || facingSheet.DLFactor == "")
            {
                facingSheet.DLFactor = "none";
            }
            if(facingSheet.DLFactor == "none")
            {
                KeyPoints = "Load Factor: " + facingSheet.LoadFactor;
            }
            else
            {
                KeyPoints = "Load Factor: " + facingSheet.LoadFactor + " & " + "DL Factor: " + facingSheet.DLFactor;
            }
/////Verification Details
            if (facingSheet.CWName == "" && facingSheet.SIName == "" && facingSheet.DIOName == "" && facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.CWName = "";
                facingSheet.CWVDate = "";
                facingSheet.SIName = "";
                facingSheet.SIVDate = "";
                facingSheet.DIOName = "";
                facingSheet.DIOVDate = "";
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.SIName == "" && facingSheet.DIOName == "" && facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.SIName = "";
                facingSheet.SIVDate = "";
                facingSheet.DIOName = "";
                facingSheet.DIOVDate = "";
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DIOName == "" && facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.DIOName = "";
                facingSheet.DIOVDate = "";
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DName == "")
            {
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            //////////
            if (facingSheet.DIOName == facingSheet.DDName && facingSheet.DDName == facingSheet.DName)
            {
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DDName == facingSheet.DName)
            {
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            ////////////////////////////////////////////
            string pdfTemplate = Server.MapPath("~/PdfTemplate/FacingSheet/Facingsheet_Template_Kannada.pdf");
            //string newFile = Server.MapPath("~/PdfTemplate/FacingSheet/" + result + "UnSigned" + ".pdf");
            //string newFile = @"C:/Documents/PdfTemplate/FacingSheet/" + result + "UnSigned" + ".pdf";
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\FacingSheet\" + result + "UnSigned" + ".pdf";
            }

            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            {//Facing Sheet Details
                var date1 = facingSheet.DateOfIssue?.ToString("dd-MM-yyyy");
                string date = DateTime.Now.ToShortDateString();
                fields.SetField("ApplicationRefNo", facingSheet.ApplicationNumber.ToString().Trim().ToUpper());
                fields.SetField("PolicyNumber", facingSheet.PolicyNumber);
                fields.SetFieldProperty("DIO", "textsize", 10.5f, null);
                fields.SetField("DIO", facingSheet.DistrictInsuranceOfficeAddress);
                fields.SetField("EmployeeName", facingSheet.InsurerName.ToString().Trim().ToUpper());
                //fields.SetField("DepositDate", facingSheet.InitialDeposit.ToString().Trim().ToUpper());
                fields.SetField("SubmittedBy", facingSheet.InsurerName.ToString().Trim().ToUpper());
                fields.SetField("DepositAmount", facingSheet.MonthlyPremium.ToString().Trim().ToUpper());
                //fields.SetField("CWDate", facingSheet.DateOfLiability);
                //fields.SetField("SIDDDate", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy"));
                fields.SetField("KeyPoints", KeyPoints);
                //fields.SetField("OrderofPassage", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy").Trim().ToUpper());
                fields.SetField("ChallanRefNumber", facingSheet.ChallanRefNo);
                fields.SetField("ChallanDate", facingSheet.ChallanDate);
                fields.SetField("DateofRisk", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy"));
                fields.SetField("DOB", facingSheet.DateOfBirth.ToString("dd-MM-yyyy"));
                fields.SetFieldProperty("InsurerAge", "textsize", 10.5f, null);
                fields.SetField("InsurerAge", facingSheet.Age.ToString().Trim().ToUpper());
                fields.SetField("MonthlyPremium", facingSheet.MonthlyPremium.ToString().Trim().ToUpper());
                fields.SetField("SumAssuredAmount", facingSheet.InsuranceAmount.ToString().Trim().ToUpper());
                fields.SetField("MaturityMonthYear", facingSheet.EffectiveMonthYear);
                fields.SetField("DateofIssuePB", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy"));
                //fields.SetField("DIODate", date);
                //Verification Details
                fields.SetField("CWName", facingSheet.CWName.ToString());
                fields.SetField("CWVDate", facingSheet.CWVDate.ToString());

                fields.SetField("SIName", facingSheet.SIName.ToString());
                fields.SetField("SIVDate", facingSheet.SIVDate.ToString());

                fields.SetField("DIOName", facingSheet.DIOName.ToString());
                fields.SetField("DIOVDate", facingSheet.DIOVDate.ToString());

                fields.SetField("DDName", facingSheet.DDName.ToString());
                fields.SetField("DDVDate", facingSheet.DDVDate.ToString());

                fields.SetField("DName", facingSheet.DName.ToString());
                fields.SetField("DVDate", facingSheet.DVDate.ToString());
            }
            if (facingSheet.Policies.Count == 1)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
            }
            if (facingSheet.Policies.Count == 2)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
            }
            if (facingSheet.Policies.Count == 3)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
            }
            if (facingSheet.Policies.Count == 4)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
            }
            if (facingSheet.Policies.Count == 5)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
            }
            if (facingSheet.Policies.Count == 6)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
                fields.SetField("Policy6", facingSheet.Policies[5]);
            }
            if (facingSheet.Policies.Count == 7)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
                fields.SetField("Policy6", facingSheet.Policies[5]);
                fields.SetField("Policy7", facingSheet.Policies[6]);
            }
            if (facingSheet.Policies.Count == 8)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
                fields.SetField("Policy6", facingSheet.Policies[5]);
                fields.SetField("Policy7", facingSheet.Policies[6]);
                fields.SetField("Policy8", facingSheet.Policies[7]);
            }
            pdfStamper.Close();
            return newFile;
        }
        #endregion
        #region NB Bond Generation
        private string FillFormNB(VM_NBBond _empNBBond, string PolicyNumber)
        {
            try
            {
                Logger.LogMessage(TracingLevel.INFO, "//NB Bond Data:- " + _empNBBond);
                //Logger.LogMessage(TracingLevel.INFO, "NB Basic Details Filled-Test Dates-DOB&SD:-" + _empNBBond.EmployeeBasicDetails.date_of_birth+ _empNBBond.EmployeeBasicDetails.p_sanction_date);
                
                string KeyPoints = string.Empty;
                if (_empNBBond.EmployeeBasicDetails.LoadFactor == "" && _empNBBond.EmployeeBasicDetails.DLFactor == "")
                {
                    _empNBBond.EmployeeBasicDetails.LoadFactor = "OR";
                    KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor;
                }
                if (_empNBBond.EmployeeBasicDetails.LoadFactor != "" && _empNBBond.EmployeeBasicDetails.DLFactor == "")
                {
                    KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor;
                    //_empNBBond.EmployeeBasicDetails.DLFactor = "none";
                }
                if (_empNBBond.EmployeeBasicDetails.LoadFactor == "" && _empNBBond.EmployeeBasicDetails.DLFactor != "")
                {
                    _empNBBond.EmployeeBasicDetails.LoadFactor = "OR";
                    KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor + " & " + "DL Factor: " + _empNBBond.EmployeeBasicDetails.DLFactor;
                }
                if (_empNBBond.EmployeeBasicDetails.LoadFactor != "" && _empNBBond.EmployeeBasicDetails.DLFactor != "")
                {
                    KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor + " & " + "DL Factor: " + _empNBBond.EmployeeBasicDetails.DLFactor;
                }
                //KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor + " & " + "DL Factor: " + _empNBBond.EmployeeBasicDetails.DLFactor;
                Logger.LogMessage(TracingLevel.INFO, "//NB Bond KeyPoints:- " + KeyPoints);
                string pdfTemplate = Server.MapPath("~/PdfTemplate/NBBond/NB_BOND_Template_Kannada.pdf");
                //string newFile = Server.MapPath("~/PdfTemplate/NBBond/" + PolicyNumber + "UnSigned" + DateTime.Now.Ticks + ".pdf");
                //string newFile = @"C:/Documents/PdfTemplate/NBBond/" + PolicyNumber + "UnSigned" + DateTime.Now.Ticks + ".pdf";
                string newFile = string.Empty;
                if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                {
                    newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\NBBond\" + PolicyNumber + "UnSigned" + DateTime.Now.Ticks + ".pdf";
                }
                PdfReader pdfReader = new PdfReader(pdfTemplate);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
                Logger.LogMessage(TracingLevel.INFO, "NB Bond Created Path Details:-" + newFile);
                AcroFields fields = pdfStamper.AcroFields;
                //It's important to set this value before setting the fields or the appearances won't be created.
                fields.GenerateAppearances = true;
                //GenerateAppearances End
                //Set KA Font
                //string KAfont = Server.MapPath("~/Content/Nirmala.TTF");
                //var NirmalaFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), KAfont);
                //var NirmalaBaseFont = BaseFont.CreateFont(NirmalaFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //End
                string NameDesignation = _empNBBond.EmployeeBasicDetails.employee_name.ToString() + ", " + _empNBBond.EmployeeBasicDetails.designation.ToString().Trim().ToUpper() + ", " + _empNBBond.EmployeeBasicDetails.dm_ddo_office.ToString().Trim().ToUpper();
                //string SumAssuredWords = ConvertToWords(_empNBBond.EmployeeBasicDetails.p_total_sum_assured);
                string SumAssuredWords = NumberToWords(Convert.ToInt32(_empNBBond.EmployeeBasicDetails.p_total_sum_assured));
                SumAssuredWords = SumAssuredWords + " Rupees Only";
                //string[] DOBSplit = _empNBBond.EmployeeBasicDetails.date_of_birth.Split(new Char[] {'-'});
                //var totalYears =(DateTime.Today - new DateTime(Convert.ToInt32(DOBSplit[2]), Convert.ToInt32(DOBSplit[1]), Convert.ToInt32(DOBSplit[0]))).TotalDays/ 365.2425;
                //Logger.LogMessage(TracingLevel.INFO, "NB Basic Details Filled-NameDesignation&SumAssuredWords:-" + NameDesignation + SumAssuredWords);
                int Age = new DateTime((DateTime.Now - _empNBBond.EmployeeBasicDetails.emp_date_of_birth).Ticks).Year;
                Logger.LogMessage(TracingLevel.INFO, "NB Basic Details Filled-Age:-" + Age);
                //int year = _empNBBond.EmployeeBasicDetails.emp_date_of_birth.Year;
                //Logger.LogMessage(TracingLevel.INFO, "NB Basic Details Filled-year:-" + year);
                //DateTime dob =Convert.ToDateTime(_empNBBond.EmployeeBasicDetails.date_of_birth);
                //int finalyear = year + 55;
                //string testdate=String.Format("{0:MMMM}", DateTime.Parse(_empNBBond.EmployeeBasicDetails.date_of_birth));
                //Logger.LogMessage(TracingLevel.INFO, "NB Basic Details Filled-testdate-month:-" + testdate);
                //string month = _empNBBond.EmployeeBasicDetails.emp_date_of_birth.ToString("MMMM");
                //Logger.LogMessage(TracingLevel.INFO, "NB Basic Details Filled-month:-" + month);
                string DueDate = _empNBBond.EmployeeBasicDetails.DueDate;
                Logger.LogMessage(TracingLevel.INFO, "NB Basic Details Filled-DueDate:-" + DueDate);
                string finalpayment = _empNBBond.EmployeeBasicDetails.FinalPayment;
                //string EndMonth = _empNBBond.EmployeeBasicDetails.emp_date_of_birth.AddMonths(-1).ToString("MMMM");
                string EndMonthYear = _empNBBond.EmployeeBasicDetails.EndMonthYear;
                string ApprovedMonth = _empNBBond.EmployeeBasicDetails.ApprovedMonth;
                string ApprovedYear = _empNBBond.EmployeeBasicDetails.ApprovedYear;
                {//Basic Details
                    //fields.SetField("MedicalExaminationDate", _empNBBond.EmployeeBasicDetails.App_Creation_Date.ToString().Trim().ToUpper());
                    fields.SetField("MedicalExaminationDate","");
                    fields.SetFieldProperty("NameDesignation", "textsize", 10.5f, null);
                    fields.SetField("NameDesignation", NameDesignation);
                    fields.SetField("FatherName", _empNBBond.EmployeeBasicDetails.father_name.ToString().Trim().ToUpper());
                    fields.SetField("PolicyNumber", _empNBBond.EmployeeBasicDetails.p_kgid_policy_number.ToString().Trim().ToUpper());
                    fields.SetField("DateofRisk", _empNBBond.EmployeeBasicDetails.p_sanction_date.ToString().Trim().ToUpper());
                    fields.SetField("AdmittedDOB", _empNBBond.EmployeeBasicDetails.date_of_birth.ToString().Trim().ToUpper());
                    fields.SetField("MonthlyPremium", _empNBBond.EmployeeBasicDetails.p_premium.ToString().Trim().ToUpper());
                    fields.SetField("SumAssured", _empNBBond.EmployeeBasicDetails.p_total_sum_assured.ToString().Trim().ToUpper());
                    fields.SetField("MonthofFinalPayment", finalpayment.ToString());
                    fields.SetField("SumAssuredWords", SumAssuredWords);
                    fields.SetField("EndMonthYear", EndMonthYear);
                    fields.SetField("PRiskMonth", ApprovedMonth);
                    fields.SetField("PRiskYear", ApprovedYear);
                    fields.SetField("RiskDateYear", _empNBBond.EmployeeBasicDetails.p_sanction_date.ToString());
                    //fields.SetField("DueDate", DueDate);
                    fields.SetFieldProperty("CaseWorkerName", "textsize", 10.5f, null);
                    fields.SetField("CaseWorkerName", _empNBBond.EmployeeBasicDetails.CaseWorkerName.ToString().Trim().ToUpper());
                    fields.SetField("CaseWorkerVerifiedDate", _empNBBond.EmployeeBasicDetails.CaseWorkerVerifiedDate.ToString());
                    fields.SetFieldProperty("SuperintendentName", "textsize", 10.5f, null);
                    fields.SetField("SuperintendentName", _empNBBond.EmployeeBasicDetails.SuperintendentName.ToString().Trim().ToUpper());
                    fields.SetField("SuperintendentVerifiedDate", _empNBBond.EmployeeBasicDetails.SuperintendentVerifiedDate.ToString());
                    fields.SetField("ApprovedDate", _empNBBond.EmployeeBasicDetails.p_sanction_date.ToString().Trim().ToUpper());
                    fields.SetField("ApprovedYear", ApprovedYear);
                    fields.SetField("DateofProposal", _empNBBond.EmployeeBasicDetails.ApplicationSubmitedDate.ToString());
                    fields.SetFieldProperty("DIOAddress", "textsize", 10.5f, null);
                    fields.SetField("DIOAddress", _empNBBond.EmployeeBasicDetails.DIO_Office_Address.ToString());
                    fields.SetField("SpecialRemarks", KeyPoints);
                }
                Logger.LogMessage(TracingLevel.INFO, "NB Basic Details Filled:-" + _empNBBond.EmployeeBasicDetails.ApplicationSubmitedDate.ToString());
                {//ASSIGNMENT FORM Details
                    fields.SetField("PName", _empNBBond.EmployeeBasicDetails.employee_name.ToString().Trim().ToUpper());
                    fields.SetField("PFatherName", _empNBBond.EmployeeBasicDetails.father_name.ToString().Trim().ToUpper());
                    fields.SetField("PAge", Age.ToString().Trim().ToUpper());
                    fields.SetFieldProperty("APolicyNumber", "textsize", 10.5f, null);
                    fields.SetField("APolicyNumber", _empNBBond.EmployeeBasicDetails.p_kgid_policy_number.ToString().Trim().ToUpper());
                    fields.SetFieldProperty("APolicyDate", "textsize", 10.5f, null);
                    fields.SetField("APolicyDate", _empNBBond.EmployeeBasicDetails.p_sanction_date.ToString().Trim().ToUpper());
                    fields.SetField("ASum", _empNBBond.EmployeeBasicDetails.p_total_sum_assured.ToString().Trim().ToUpper());
                    fields.SetField("APName", _empNBBond.EmployeeBasicDetails.employee_name.ToString().Trim().ToUpper());
                }
                Logger.LogMessage(TracingLevel.INFO, "ASSIGNMENT FORM Details Test:-" + _empNBBond.EmployeeBasicDetails.p_total_sum_assured.ToString());
                if (_empNBBond.NomineeDetailsList != null)
                {

                    if (_empNBBond.NomineeDetailsList.Count() == 1)
                    {//Nominee Details 1
                        fields.SetFieldProperty("NomineeSNo1", "textsize", 10.5f, null);
                        fields.SetField("NomineeSNo1", "1");
                        fields.SetFieldProperty("NomineeName1", "textsize", 10.5f, null);
                        fields.SetField("NomineeName1", _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString());
                        string RelationChars = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        long RelationCharLength = RelationChars.LongCount();
                        Logger.LogMessage(TracingLevel.INFO, "RelationCharLength:-" + RelationCharLength);
                        if (RelationCharLength > 10)
                        {
                            fields.SetFieldProperty("NomineeRelationship1", "textsize", 8.5f, null);
                            fields.SetField("NomineeRelationship1", _empNBBond.NomineeDetailsList[0].Relation.ToString());
                        }
                        else
                        {
                            fields.SetFieldProperty("NomineeRelationship1", "textsize", 10.5f, null);
                            fields.SetField("NomineeRelationship1", _empNBBond.NomineeDetailsList[0].Relation.ToString());
                        }
                        fields.SetFieldProperty("NomineeAge1", "textsize", 10.5f, null);
                        fields.SetField("NomineeAge1", _empNBBond.NomineeDetailsList[0].NomineeAge.ToString());
                        fields.SetFieldProperty("NomineeShare1", "textsize", 10.5f, null);
                        fields.SetField("NomineeShare1", _empNBBond.NomineeDetailsList[0].PercentageShare.ToString());
                        fields.SetFieldProperty("NomineeGuardian1", "textsize", 10.5f, null);
                        fields.SetField("NomineeGuardian1", _empNBBond.NomineeDetailsList[0].NameOfGaurdian);
                        fields.SetFieldProperty("NomineeGRelation1", "textsize", 10.5f, null);
                        fields.SetField("NomineeGRelation1", _empNBBond.NomineeDetailsList[0].GaurdianRelation);
                        Logger.LogMessage(TracingLevel.INFO, "//Nominee Details 1 Filled" + _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString());
                    }

                    if (_empNBBond.NomineeDetailsList.Count() == 2)
                    {//Nominee Details 2
                        fields.SetFieldProperty("NomineeSNo1", "textsize", 10.5f, null);
                        fields.SetField("NomineeSNo1", "1");
                        fields.SetFieldProperty("NomineeName1", "textsize", 10.5f, null);
                        fields.SetField("NomineeName1", _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString().Trim().ToUpper());
                        string RelationChars = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        long RelationCharLength = RelationChars.LongCount();
                        if (RelationCharLength > 10)
                        {
                            fields.SetFieldProperty("NomineeRelationship1", "textsize", 8.5f, null);
                            fields.SetField("NomineeRelationship1", _empNBBond.NomineeDetailsList[0].Relation.ToString());
                        }
                        else
                        {
                            fields.SetFieldProperty("NomineeRelationship1", "textsize", 10.5f, null);
                            fields.SetField("NomineeRelationship1", _empNBBond.NomineeDetailsList[0].Relation.ToString());
                        }
                        fields.SetFieldProperty("NomineeAge1", "textsize", 10.5f, null);
                        fields.SetField("NomineeAge1", _empNBBond.NomineeDetailsList[0].NomineeAge.ToString());
                        fields.SetFieldProperty("NomineeShare1", "textsize", 10.5f, null);
                        fields.SetField("NomineeShare1", _empNBBond.NomineeDetailsList[0].PercentageShare.ToString());
                        fields.SetFieldProperty("NomineeGuardian1", "textsize", 10.5f, null);
                        fields.SetField("NomineeGuardian1", _empNBBond.NomineeDetailsList[0].NameOfGaurdian);
                        fields.SetFieldProperty("NomineeGRelation1", "textsize", 10.5f, null);
                        fields.SetField("NomineeGRelation1", _empNBBond.NomineeDetailsList[0].GaurdianRelation);
                        //2
                        fields.SetFieldProperty("NomineeSNo2", "textsize", 10.5f, null);
                        fields.SetField("NomineeSNo2", "2");
                        fields.SetFieldProperty("NomineeName1", "textsize", 10.5f, null);
                        fields.SetField("NomineeName2", _empNBBond.NomineeDetailsList[1].NameOfNominee.ToString().Trim().ToUpper());
                        string RelationChars22 = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        long RelationCharLength22 = RelationChars22.LongCount();
                        if (RelationCharLength22 > 10)
                        {
                            fields.SetFieldProperty("NomineeRelationship2", "textsize", 8.5f, null);
                            fields.SetField("NomineeRelationship2", _empNBBond.NomineeDetailsList[1].Relation.ToString());
                        }
                        else
                        {
                            fields.SetFieldProperty("NomineeRelationship2", "textsize", 10.5f, null);
                            fields.SetField("NomineeRelationship2", _empNBBond.NomineeDetailsList[1].Relation.ToString());
                        }
                        fields.SetFieldProperty("NomineeAge2", "textsize", 10.5f, null);
                        fields.SetField("NomineeAge2", _empNBBond.NomineeDetailsList[1].NomineeAge.ToString());
                        fields.SetFieldProperty("NomineeShare2", "textsize", 10.5f, null);
                        fields.SetField("NomineeShare2", _empNBBond.NomineeDetailsList[1].PercentageShare.ToString());
                        fields.SetFieldProperty("NomineeGuardian2", "textsize", 10.5f, null);
                        fields.SetField("NomineeGuardian2", _empNBBond.NomineeDetailsList[1].NameOfGaurdian);
                        fields.SetFieldProperty("NomineeGRelation2", "textsize", 10.5f, null);
                        fields.SetField("NomineeGRelation2", _empNBBond.NomineeDetailsList[1].GaurdianRelation);
                    }
                    if (_empNBBond.NomineeDetailsList.Count() == 3)
                    {//Nominee Details 3
                        fields.SetFieldProperty("NomineeSNo1", "textsize", 10.5f, null);
                        fields.SetField("NomineeSNo1", "1");
                        fields.SetFieldProperty("NomineeName1", "textsize", 10.5f, null);
                        fields.SetField("NomineeName1", _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString().Trim().ToUpper());
                        string RelationChars = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        long RelationCharLength = RelationChars.LongCount();
                        if (RelationCharLength > 10)
                        {
                            fields.SetFieldProperty("NomineeRelationship1", "textsize", 8.5f, null);
                            fields.SetField("NomineeRelationship1", _empNBBond.NomineeDetailsList[0].Relation.ToString());
                        }
                        else
                        {
                            fields.SetFieldProperty("NomineeRelationship1", "textsize", 10.5f, null);
                            fields.SetField("NomineeRelationship1", _empNBBond.NomineeDetailsList[0].Relation.ToString());
                        }
                        fields.SetFieldProperty("NomineeAge1", "textsize", 10.5f, null);
                        fields.SetField("NomineeAge1", _empNBBond.NomineeDetailsList[0].NomineeAge.ToString().Trim().ToUpper());
                        fields.SetFieldProperty("NomineeShare1", "textsize", 10.5f, null);
                        fields.SetField("NomineeShare1", _empNBBond.NomineeDetailsList[0].PercentageShare.ToString().Trim().ToUpper());
                        fields.SetFieldProperty("NomineeGuardian1", "textsize", 10.5f, null);
                        fields.SetField("NomineeGuardian1", _empNBBond.NomineeDetailsList[0].NameOfGaurdian);
                        fields.SetFieldProperty("NomineeGRelation1", "textsize", 10.5f, null);
                        fields.SetField("NomineeGRelation1", _empNBBond.NomineeDetailsList[0].GaurdianRelation);
                        //2
                        fields.SetFieldProperty("NomineeSNo2", "textsize", 10.5f, null);
                        fields.SetField("NomineeSNo2", "2");
                        fields.SetFieldProperty("NomineeName1", "textsize", 10.5f, null);
                        fields.SetField("NomineeName2", _empNBBond.NomineeDetailsList[1].NameOfNominee.ToString().Trim().ToUpper());
                        string RelationChars2 = _empNBBond.NomineeDetailsList[1].Relation.ToString();
                        long RelationCharLength2 = RelationChars2.LongCount();
                        if (RelationCharLength2 > 10)
                        {
                            fields.SetFieldProperty("NomineeRelationship2", "textsize", 8.5f, null);
                            fields.SetField("NomineeRelationship2", _empNBBond.NomineeDetailsList[1].Relation.ToString());
                        }
                        else
                        {
                            fields.SetFieldProperty("NomineeRelationship2", "textsize", 10.5f, null);
                            fields.SetField("NomineeRelationship2", _empNBBond.NomineeDetailsList[1].Relation.ToString());
                        }
                        fields.SetFieldProperty("NomineeAge2", "textsize", 10.5f, null);
                        fields.SetField("NomineeAge2", _empNBBond.NomineeDetailsList[1].NomineeAge.ToString().Trim().ToUpper());
                        fields.SetFieldProperty("NomineeShare2", "textsize", 10.5f, null);
                        fields.SetField("NomineeShare2", _empNBBond.NomineeDetailsList[1].PercentageShare.ToString().Trim().ToUpper());
                        fields.SetFieldProperty("NomineeGuardian2", "textsize", 10.5f, null);
                        fields.SetField("NomineeGuardian2", _empNBBond.NomineeDetailsList[1].NameOfGaurdian);
                        fields.SetFieldProperty("NomineeGRelation2", "textsize", 10.5f, null);
                        fields.SetField("NomineeGRelation2", _empNBBond.NomineeDetailsList[1].GaurdianRelation);
                        //3
                        fields.SetFieldProperty("NomineeSNo3", "textsize", 10.5f, null);
                        fields.SetField("NomineeSNo3", "3");
                        fields.SetFieldProperty("NomineeName3", "textsize", 10.5f, null);
                        fields.SetField("NomineeName3", _empNBBond.NomineeDetailsList[2].NameOfNominee.ToString());
                        string RelationChars3 = _empNBBond.NomineeDetailsList[2].Relation.ToString();
                        long RelationCharLength3 = RelationChars3.LongCount();
                        if (RelationCharLength3 > 10)
                        {
                            fields.SetFieldProperty("NomineeRelationship3", "textsize", 8.5f, null);
                            fields.SetField("NomineeRelationship3", _empNBBond.NomineeDetailsList[2].Relation.ToString());
                        }
                        else
                        {
                            fields.SetFieldProperty("NomineeRelationship3", "textsize", 10.5f, null);
                            fields.SetField("NomineeRelationship3", _empNBBond.NomineeDetailsList[2].Relation.ToString());
                        }
                        fields.SetFieldProperty("NomineeAge3", "textsize", 10.5f, null);
                        fields.SetField("NomineeAge3", _empNBBond.NomineeDetailsList[2].NomineeAge.ToString());
                        fields.SetFieldProperty("NomineeShare3", "textsize", 10.5f, null);
                        fields.SetField("NomineeShare3", _empNBBond.NomineeDetailsList[2].PercentageShare.ToString());
                        fields.SetFieldProperty("NomineeGuardian3", "textsize", 10.5f, null);
                        fields.SetField("NomineeGuardian3", _empNBBond.NomineeDetailsList[2].NameOfGaurdian);
                        fields.SetFieldProperty("NomineeGRelation3", "textsize", 10.5f, null);
                        fields.SetField("NomineeGRelation3", _empNBBond.NomineeDetailsList[2].GaurdianRelation);
                    }
                    if (_empNBBond.NomineeDetailsList.Count() == 4)
                    {//Nominee Details 4
                        fields.SetFieldProperty("NomineeSNo1", "textsize", 10.5f, null);
                        fields.SetField("NomineeSNo1", "1");
                        fields.SetFieldProperty("NomineeName1", "textsize", 10.5f, null);
                        fields.SetField("NomineeName1", _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString());
                        string RelationChars = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        long RelationCharLength = RelationChars.LongCount();
                        if (RelationCharLength > 10)
                        {
                            fields.SetFieldProperty("NomineeRelationship1", "textsize", 8.5f, null);
                            fields.SetField("NomineeRelationship1", _empNBBond.NomineeDetailsList[0].Relation.ToString());
                        }
                        else
                        {
                            fields.SetFieldProperty("NomineeRelationship1", "textsize", 10.5f, null);
                            fields.SetField("NomineeRelationship1", _empNBBond.NomineeDetailsList[0].Relation.ToString());
                        }
                        fields.SetFieldProperty("NomineeAge1", "textsize", 10.5f, null);
                        fields.SetField("NomineeAge1", _empNBBond.NomineeDetailsList[0].NomineeAge.ToString());
                        fields.SetFieldProperty("NomineeShare1", "textsize", 10.5f, null);
                        fields.SetField("NomineeShare1", _empNBBond.NomineeDetailsList[0].PercentageShare.ToString());
                        fields.SetFieldProperty("NomineeGuardian1", "textsize", 10.5f, null);
                        fields.SetField("NomineeGuardian1", _empNBBond.NomineeDetailsList[0].NameOfGaurdian);
                        fields.SetFieldProperty("NomineeGRelation1", "textsize", 10.5f, null);
                        fields.SetField("NomineeGRelation1", _empNBBond.NomineeDetailsList[0].GaurdianRelation);
                        //2
                        fields.SetFieldProperty("NomineeSNo2", "textsize", 10.5f, null);
                        fields.SetField("NomineeSNo2", "2");
                        fields.SetFieldProperty("NomineeName1", "textsize", 10.5f, null);
                        fields.SetField("NomineeName2", _empNBBond.NomineeDetailsList[1].NameOfNominee.ToString());
                        string RelationChars2 = _empNBBond.NomineeDetailsList[1].Relation.ToString();
                        long RelationCharLength2 = RelationChars2.LongCount();
                        if (RelationCharLength2 > 10)
                        {
                            fields.SetFieldProperty("NomineeRelationship2", "textsize", 8.5f, null);
                            fields.SetField("NomineeRelationship2", _empNBBond.NomineeDetailsList[1].Relation.ToString());
                        }
                        else
                        {
                            fields.SetFieldProperty("NomineeRelationship2", "textsize", 10.5f, null);
                            fields.SetField("NomineeRelationship2", _empNBBond.NomineeDetailsList[1].Relation.ToString());
                        }
                        fields.SetFieldProperty("NomineeAge2", "textsize", 10.5f, null);
                        fields.SetField("NomineeAge2", _empNBBond.NomineeDetailsList[1].NomineeAge.ToString());
                        fields.SetFieldProperty("NomineeShare2", "textsize", 10.5f, null);
                        fields.SetField("NomineeShare2", _empNBBond.NomineeDetailsList[1].PercentageShare.ToString());
                        fields.SetFieldProperty("NomineeGuardian2", "textsize", 10.5f, null);
                        fields.SetField("NomineeGuardian2", _empNBBond.NomineeDetailsList[1].NameOfGaurdian);
                        fields.SetFieldProperty("NomineeGRelation2", "textsize", 10.5f, null);
                        fields.SetField("NomineeGRelation2", _empNBBond.NomineeDetailsList[1].GaurdianRelation);
                        //3
                        fields.SetFieldProperty("NomineeSNo3", "textsize", 10.5f, null);
                        fields.SetField("NomineeSNo3", "3");
                        fields.SetFieldProperty("NomineeName3", "textsize", 10.5f, null);
                        fields.SetField("NomineeName3", _empNBBond.NomineeDetailsList[2].NameOfNominee.ToString());
                        string RelationChars3 = _empNBBond.NomineeDetailsList[2].Relation.ToString();
                        long RelationCharLength3 = RelationChars3.LongCount();
                        if (RelationCharLength3 > 10)
                        {
                            fields.SetFieldProperty("NomineeRelationship3", "textsize", 8.5f, null);
                            fields.SetField("NomineeRelationship3", _empNBBond.NomineeDetailsList[2].Relation.ToString());
                        }
                        else
                        {
                            fields.SetFieldProperty("NomineeRelationship3", "textsize", 10.5f, null);
                            fields.SetField("NomineeRelationship3", _empNBBond.NomineeDetailsList[2].Relation.ToString());
                        }
                        fields.SetFieldProperty("NomineeAge3", "textsize", 10.5f, null);
                        fields.SetField("NomineeAge3", _empNBBond.NomineeDetailsList[2].NomineeAge.ToString());
                        fields.SetFieldProperty("NomineeShare3", "textsize", 10.5f, null);
                        fields.SetField("NomineeShare3", _empNBBond.NomineeDetailsList[2].PercentageShare.ToString());
                        fields.SetFieldProperty("NomineeGuardian3", "textsize", 10.5f, null);
                        fields.SetField("NomineeGuardian3", _empNBBond.NomineeDetailsList[2].NameOfGaurdian);
                        fields.SetFieldProperty("NomineeGRelation3", "textsize", 10.5f, null);
                        fields.SetField("NomineeGRelation3", _empNBBond.NomineeDetailsList[2].GaurdianRelation);
                        //4
                        fields.SetFieldProperty("NomineeSNo4", "textsize", 10.5f, null);
                        fields.SetField("NomineeSNo4", "4");
                        fields.SetFieldProperty("NomineeName4", "textsize", 10.5f, null);
                        fields.SetField("NomineeName4", _empNBBond.NomineeDetailsList[3].NameOfNominee.ToString());
                        string RelationChars4 = _empNBBond.NomineeDetailsList[3].Relation.ToString();
                        long RelationCharLength4 = RelationChars4.LongCount();
                        if (RelationCharLength4 > 10)
                        {
                            fields.SetFieldProperty("NomineeRelationship4", "textsize", 8.5f, null);
                            fields.SetField("NomineeRelationship4", _empNBBond.NomineeDetailsList[3].Relation.ToString());
                        }
                        else
                        {
                            fields.SetFieldProperty("NomineeRelationship4", "textsize", 10.5f, null);
                            fields.SetField("NomineeRelationship4", _empNBBond.NomineeDetailsList[3].Relation.ToString());
                        }
                        fields.SetFieldProperty("NomineeAge4", "textsize", 10.5f, null);
                        fields.SetField("NomineeAge4", _empNBBond.NomineeDetailsList[3].NomineeAge.ToString());
                        fields.SetFieldProperty("NomineeShare4", "textsize", 10.5f, null);
                        fields.SetField("NomineeShare4", _empNBBond.NomineeDetailsList[3].PercentageShare.ToString());
                        fields.SetFieldProperty("NomineeGuardian4", "textsize", 10.5f, null);
                        fields.SetField("NomineeGuardian4", _empNBBond.NomineeDetailsList[3].NameOfGaurdian);
                        fields.SetFieldProperty("NomineeGRelation4", "textsize", 10.5f, null);
                        fields.SetField("NomineeGRelation4", _empNBBond.NomineeDetailsList[3].GaurdianRelation);
                    }
                }
                pdfStamper.Close();
                Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//NB Bond Path:- " + newFile);
                return newFile;
            }
            catch(Exception e)
            {
                Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//NB Bond Error message:- " + e.Message);
                return null;
            }
        }
        private string NBSignPdf(string unsignedpdf, string PolicyNumber, string PublicKey)
        {
            try
            {
                //ErrorLogs("logFile_DSC", "Program Begins");

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
                //string newFile = Server.MapPath("~/PdfTemplate/NBBond/" + PolicyNumber + DateTime.Now.Ticks + ".pdf");
                //string newFile = @"C:/Documents/PdfTemplate/NBBond/" + PolicyNumber + DateTime.Now.Ticks + ".pdf";
                string newFile = string.Empty;
                if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                {
                    newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\NBBond\" + PolicyNumber + DateTime.Now.Ticks + ".pdf";
                }

                FileStream signedPdf = new FileStream(newFile, FileMode.Create);
                PdfStamper pdfStamper = PdfStamper.CreateSignature(inputPdf, signedPdf, '\0');


                IExternalSignature externalSignature = new X509Certificate2Signature(certClient, "SHA-256");

                PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;

                //signatureAppearance.Reason = "My Signature";
                signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(590, 80, 400, 10), 1, "Signature");
                signatureAppearance.Acro6Layers = true;
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
        #region NB Bond Generation Word //Venkatesh
        public string FillFormNBDocPdf(VM_NBBond _empNBBond, string PolicyNumber)
        {
            string KeyPoints = string.Empty;
            if (_empNBBond.EmployeeBasicDetails.LoadFactor == "" && _empNBBond.EmployeeBasicDetails.DLFactor == "")
            {
                _empNBBond.EmployeeBasicDetails.LoadFactor = "OR";
                KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor;
            }
            if (_empNBBond.EmployeeBasicDetails.LoadFactor != "" && _empNBBond.EmployeeBasicDetails.DLFactor == "")
            {
                KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor;
                //_empNBBond.EmployeeBasicDetails.DLFactor = "none";
            }
            if (_empNBBond.EmployeeBasicDetails.LoadFactor == "" && _empNBBond.EmployeeBasicDetails.DLFactor != "")
            {
                _empNBBond.EmployeeBasicDetails.LoadFactor = "OR";
                KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor + " & " + "DL Factor: " + _empNBBond.EmployeeBasicDetails.DLFactor;
            }
            if (_empNBBond.EmployeeBasicDetails.LoadFactor != "" && _empNBBond.EmployeeBasicDetails.DLFactor != "")
            {
                KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor + " & " + "DL Factor: " + _empNBBond.EmployeeBasicDetails.DLFactor;
            }
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//KeyPoints:- " + KeyPoints);
            string NameDesignation = string.Empty;
            string SumAssuredWords = string.Empty;
            int Age;int year;int finalyear;
            string month = string.Empty;
            string DueDate = string.Empty;
            string finalpayment = string.Empty;
            string EndMonthYear = string.Empty;
            string ApprovedMonth = string.Empty;
            string ApprovedYear = string.Empty;
            try
            {
                NameDesignation = _empNBBond.EmployeeBasicDetails.employee_name_kannada.ToString() + ", " + _empNBBond.EmployeeBasicDetails.designation.ToString().Trim().ToUpper() + ", " + _empNBBond.EmployeeBasicDetails.dm_ddo_office.ToString().Trim().ToUpper();
                SumAssuredWords = ConvertToWords(_empNBBond.EmployeeBasicDetails.p_total_sum_assured);
                Age = new DateTime((DateTime.Now - _empNBBond.EmployeeBasicDetails.emp_date_of_birth).Ticks).Year;
                year = _empNBBond.EmployeeBasicDetails.emp_date_of_birth.Year;
                finalyear = year + 55;
                month = _empNBBond.EmployeeBasicDetails.emp_date_of_birth.ToString("MMMM");
                DueDate = _empNBBond.EmployeeBasicDetails.DueDate;
                finalpayment = _empNBBond.EmployeeBasicDetails.FinalPayment;
                //string EndMonth = _empNBBond.EmployeeBasicDetails.emp_date_of_birth.AddMonths(-1).ToString("MMMM");
                EndMonthYear = _empNBBond.EmployeeBasicDetails.EndMonthYear;
                ApprovedMonth = _empNBBond.EmployeeBasicDetails.ApprovedMonth;
                ApprovedYear = _empNBBond.EmployeeBasicDetails.ApprovedYear;
            }
            catch(Exception e)
            {
                Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//Basic Details Error:- " + e.Message);
            }
            /////------------
            string DocTemplate = Server.MapPath("~/PdfTemplate/NBBond/NBBond_Kannada_Doc_Template.docx");
            //string pdfTemplate = Server.MapPath("~/PdfTemplate/NBBond/NB_BOND_Template_Kannada.pdf");
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//Word Doc Template Path:- "+ DocTemplate);
            try
            {
                Microsoft.Office.Interop.Word.Application word1 = new Microsoft.Office.Interop.Word.Application();
                Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//Word Doc Template Opening:- ");
                Microsoft.Office.Interop.Word.Document doc1 = word1.Documents.Open(DocTemplate);
                //doc.Activate();
                Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//Word Doc Template Opened:- ");
            }
            catch(Exception e)
            {
                Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//Word Doc Template Opened Error:- "+e.Message);
            }
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = word.Documents.Open(DocTemplate);
            //doc.Activate();
            ///--------------
            try
            {
                doc.FormFields["DIOAddress"].Result = _empNBBond.EmployeeBasicDetails.DIO_Office_Address.ToString();
                doc.FormFields["MEDate"].Result = _empNBBond.EmployeeBasicDetails.App_Creation_Date.ToString().Trim().ToUpper();
                doc.FormFields["NameDesignation"].Result = NameDesignation;
                doc.FormFields["FatherName"].Result = _empNBBond.EmployeeBasicDetails.father_name_kannada;
                doc.FormFields["DateofProposal"].Result = _empNBBond.EmployeeBasicDetails.ApplicationSubmitedDate.ToString();
                doc.FormFields["PolicyNumber"].Result = _empNBBond.EmployeeBasicDetails.p_kgid_policy_number.ToString();
                doc.FormFields["DateofRisk"].Result = _empNBBond.EmployeeBasicDetails.p_sanction_date.ToString();
                doc.FormFields["AdmittedDOB"].Result = _empNBBond.EmployeeBasicDetails.date_of_birth.ToString();
                doc.FormFields["MonthlyPremium"].Result = _empNBBond.EmployeeBasicDetails.p_premium.ToString();
                doc.FormFields["SumAssured"].Result = _empNBBond.EmployeeBasicDetails.p_total_sum_assured.ToString();
                doc.FormFields["MonthofFinalPayment"].Result = finalpayment.ToString();
                doc.FormFields["SumAssuredWords"].Result = SumAssuredWords;
                doc.FormFields["EndMonthYear"].Result = EndMonthYear;
                doc.FormFields["PRiskMonth"].Result = ApprovedMonth;
                doc.FormFields["PRiskYear"].Result = ApprovedYear;
                doc.FormFields["RiskDateYear"].Result = _empNBBond.EmployeeBasicDetails.p_sanction_date.ToString();
                doc.FormFields["CaseWorkerName"].Result = _empNBBond.EmployeeBasicDetails.CaseWorkerName.ToString().Trim().ToUpper();
                doc.FormFields["CWVerifiedDate"].Result = _empNBBond.EmployeeBasicDetails.CaseWorkerVerifiedDate.ToString();
                doc.FormFields["SuperintendentName"].Result = _empNBBond.EmployeeBasicDetails.SuperintendentName.ToString().Trim().ToUpper();
                doc.FormFields["SIVerifiedDate"].Result = _empNBBond.EmployeeBasicDetails.SuperintendentVerifiedDate;
                //doc.FormFields["ApprovedDate"].Result = _empNBBond.EmployeeBasicDetails.p_sanction_date.ToString();
                //doc.FormFields["ApprovedYear"].Result = ApprovedYear;
                doc.FormFields["SpecialRemarks"].Result = KeyPoints;
            }
            catch (Exception e)
            {
                Logger.LogMessage(TracingLevel.INFO, "//NB Bond Details Filling:- " + e.Message);
            }
            try
            {
                if (_empNBBond.NomineeDetailsList != null)
                {

                    if (_empNBBond.NomineeDetailsList.Count() == 1)
                    {//Nominee Details 1
                        doc.FormFields["NomineeName1"].Result = _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship1"].Result = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        doc.FormFields["NomineeAge1"].Result = _empNBBond.NomineeDetailsList[0].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian1"].Result = _empNBBond.NomineeDetailsList[0].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation1"].Result = _empNBBond.NomineeDetailsList[0].GaurdianRelation;


                        Logger.LogMessage(TracingLevel.INFO, "//Nominee Details 1 Filled" + _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString());
                    }

                    if (_empNBBond.NomineeDetailsList.Count() == 2)
                    {//Nominee Details 2
                        doc.FormFields["NomineeName1"].Result = _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship1"].Result = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        doc.FormFields["NomineeAge1"].Result = _empNBBond.NomineeDetailsList[0].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian1"].Result = _empNBBond.NomineeDetailsList[0].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation1"].Result = _empNBBond.NomineeDetailsList[0].GaurdianRelation;
                        //2
                        doc.FormFields["NomineeName2"].Result = _empNBBond.NomineeDetailsList[1].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship2"].Result = _empNBBond.NomineeDetailsList[1].Relation.ToString();
                        doc.FormFields["NomineeAge2"].Result = _empNBBond.NomineeDetailsList[1].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian2"].Result = _empNBBond.NomineeDetailsList[1].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation2"].Result = _empNBBond.NomineeDetailsList[1].GaurdianRelation;
                    }
                    if (_empNBBond.NomineeDetailsList.Count() == 3)
                    {//Nominee Details 3
                        doc.FormFields["NomineeName1"].Result = _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship1"].Result = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        doc.FormFields["NomineeAge1"].Result = _empNBBond.NomineeDetailsList[0].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian1"].Result = _empNBBond.NomineeDetailsList[0].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation1"].Result = _empNBBond.NomineeDetailsList[0].GaurdianRelation;
                        //2
                        doc.FormFields["NomineeName2"].Result = _empNBBond.NomineeDetailsList[1].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship2"].Result = _empNBBond.NomineeDetailsList[1].Relation.ToString();
                        doc.FormFields["NomineeAge2"].Result = _empNBBond.NomineeDetailsList[1].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian2"].Result = _empNBBond.NomineeDetailsList[1].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation2"].Result = _empNBBond.NomineeDetailsList[1].GaurdianRelation;
                        //3
                        doc.FormFields["NomineeName3"].Result = _empNBBond.NomineeDetailsList[2].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship3"].Result = _empNBBond.NomineeDetailsList[2].Relation.ToString();
                        doc.FormFields["NomineeAge3"].Result = _empNBBond.NomineeDetailsList[2].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian3"].Result = _empNBBond.NomineeDetailsList[2].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation3"].Result = _empNBBond.NomineeDetailsList[2].GaurdianRelation;
                    }
                    if (_empNBBond.NomineeDetailsList.Count() == 4)
                    {//Nominee Details 4
                        doc.FormFields["NomineeName1"].Result = _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship1"].Result = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        doc.FormFields["NomineeAge1"].Result = _empNBBond.NomineeDetailsList[0].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian1"].Result = _empNBBond.NomineeDetailsList[0].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation1"].Result = _empNBBond.NomineeDetailsList[0].GaurdianRelation;
                        //2
                        doc.FormFields["NomineeName2"].Result = _empNBBond.NomineeDetailsList[1].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship2"].Result = _empNBBond.NomineeDetailsList[1].Relation.ToString();
                        doc.FormFields["NomineeAge2"].Result = _empNBBond.NomineeDetailsList[1].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian2"].Result = _empNBBond.NomineeDetailsList[1].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation2"].Result = _empNBBond.NomineeDetailsList[1].GaurdianRelation;
                        //3
                        doc.FormFields["NomineeName3"].Result = _empNBBond.NomineeDetailsList[2].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship3"].Result = _empNBBond.NomineeDetailsList[2].Relation.ToString();
                        doc.FormFields["NomineeAge3"].Result = _empNBBond.NomineeDetailsList[2].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian3"].Result = _empNBBond.NomineeDetailsList[2].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation3"].Result = _empNBBond.NomineeDetailsList[2].GaurdianRelation;
                        //4
                        doc.FormFields["NomineeName4"].Result = _empNBBond.NomineeDetailsList[3].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship4"].Result = _empNBBond.NomineeDetailsList[3].Relation.ToString();
                        doc.FormFields["NomineeAge4"].Result = _empNBBond.NomineeDetailsList[3].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian4"].Result = _empNBBond.NomineeDetailsList[3].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation4"].Result = _empNBBond.NomineeDetailsList[3].GaurdianRelation;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogMessage(TracingLevel.INFO, "//NB Bond Details Nominee Filling:- " + e.Message);
            }
            string newDocFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newDocFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\NBBond\" + PolicyNumber + "UnSigned" + DateTime.Now.Ticks + ".docx";
            }
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//NB Word Doc Bond Path:- " + newDocFile);
            doc.SaveAs(newDocFile);
            doc.Close();
            word.Quit();
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//NB Word Doc Bond Created:- "+ newDocFile);
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\NBBond\" + PolicyNumber + "UnSigned" + DateTime.Now.Ticks + ".pdf";
            }
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//NB Word PDF Bond Path:- " + newFile);
            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document wordDocument = appWord.Documents.Open(newDocFile);
            wordDocument.ExportAsFixedFormat(newFile, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            wordDocument.Close();
            appWord.Quit();
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//NB Word PDF Bond Created:- " + newFile);
            //System.Diagnostics.Process.Start(@"C:\Documents\CDoc2.docx");
            return newFile;
        }
        #endregion
        #region NB BOND Sign Generatation From UI
        public string GetFileForSigning(RequestFile requestFile)
        {
            Image_convert_model _file_obj = new Image_convert_model();
            byte[] binFile = null;
            try
            {
                string AppID = requestFile.RefID;
                string EmpID = requestFile.RefType;
                string UnSignedBond = policyBLL.GetNBBondDocFileBLL(Convert.ToInt64(AppID), Convert.ToInt64(EmpID));
                //string filename = System.Web.Hosting.HostingEnvironment.MapPath("~/PdfTemplate/NBBond/NB_BOND_Template.pdf");
                string pdfFilePath = UnSignedBond;
                byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);

                string strBytes = Convert.ToBase64String(bytes);

                _file_obj = new Image_convert_model
                {
                    File_Name = "Sample.pdf",
                    File_bytes = strBytes,
                    File_token = "",
                    RefID = AppID,
                    RefType = "1",
                    DSC_user_name = ""
                };

                return _file_obj.File_bytes;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                //return Request.CreateResponse(HttpStatusCode.OK, _responce_model, Configuration.Formatters.JsonFormatter);
            }
            return "";
        }
        //public string UploadSignedFile(Image_convert_model _Model)
        //{
        //    File_Responce_model _responce_model = new File_Responce_model();
        //    try
        //    {
        //        //_Model.File_Path = Server.MapPath("~/PdfTemplate/NBBond/");
        //        //_Model.File_Path = @"C:/Documents/PdfTemplate/NBBond/";
        //        if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
        //        {
        //            _Model.File_Path = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\NBBond\";
        //        }
        //        if (_Model.File_Name != "" && _Model.File_bytes != "")
        //        {
        //            //string serverFileName = GenerateUniqueCode(5);
        //            string serverFileName = _Model.RefID;
        //            //string filePathSigned = @"C:/Documents/PdfTemplate/NBBond/" + serverFileName  + "_Signed.pdf";
        //            string filePathSigned = string.Empty;
        //            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
        //            {
        //                filePathSigned = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\NBBond\";
        //            }
        //            byte[] imageBytes = Convert.FromBase64String(_Model.File_bytes);
        //            string FileName = serverFileName + "_Signed.pdf";
        //            string path = _Model.File_Path;
        //            string imgPath = Path.Combine(path, FileName);
        //            //Check if directory exist
        //            if (!System.IO.Directory.Exists(path))
        //            {
        //                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
        //            }
        //            System.IO.File.WriteAllBytes(imgPath, imageBytes);


        //            _responce_model.Status = true;
        //            _responce_model.Message = "success";
        //            _responce_model.return_reponce = "File Upload successfully.";
        //            string result = string.Empty;
        //            //string signedfilepath = Server.MapPath(filePathSigned);
        //            string signedfilepath = imgPath;
        //            result = policyBLL.NBSignBondUploadBLL(Convert.ToInt64(_Model.RefID), Convert.ToInt64(_Model.RefType), signedfilepath);

        //            var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == Convert.ToInt64(_Model.RefType) select eb).FirstOrDefault();
        //            string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ ";

        //            //AllCommon objemail = new AllCommon();
        //            //objemail.SendEmailWithAttachment(mobile.email_id, msg, "KGID Policy Bond.", signedfilepath);
        //        }
        //        else
        //        {
        //            _responce_model.Status = false;
        //            _responce_model.Message = "failed";
        //            _responce_model.return_reponce = "File unble to upload.";
        //        }

        //        return "true";
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //        int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
        //        return "false";
        //    }
        //}
        public string UploadSignedFile(Image_convert_model _Model)
        {
            File_Responce_model _responce_model = new File_Responce_model();
            try
            {
                //_Model.File_Path = Server.MapPath("~/PdfTemplate/NBBond/");
                //_Model.File_Path = @"C:/Documents/PdfTemplate/NBBond/";
                if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                {
                    _Model.File_Path = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\NBBond\";
                }
                if (_Model.File_Name != "" && _Model.File_bytes != "")
                {
                    //string serverFileName = GenerateUniqueCode(5);
                    string serverFileName = _Model.RefID;
                    //string filePathSigned = @"C:/Documents/PdfTemplate/NBBond/" + serverFileName  + "_Signed.pdf";
                    string filePathSigned = string.Empty;
                    if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    {
                        filePathSigned = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\NBBond\";
                    }
                    byte[] imageBytes = Convert.FromBase64String(_Model.File_bytes);
                    string FileName = serverFileName + "_Signed.pdf";
                    string path = _Model.File_Path;
                    string imgPath = Path.Combine(path, FileName);
                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }
                    System.IO.File.WriteAllBytes(imgPath, imageBytes);


                    _responce_model.Status = true;
                    _responce_model.Message = "success";
                    _responce_model.return_reponce = "File Upload successfully.";
                    string result = string.Empty;
                    //string signedfilepath = Server.MapPath(filePathSigned);
                    string signedfilepath = imgPath;
                    result = policyBLL.NBSignBondUploadBLL(Convert.ToInt64(_Model.RefID), Convert.ToInt64(_Model.RefType), signedfilepath);
                    try
                    {
                        MemoryStream stream = new MemoryStream(imageBytes);
                        var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == Convert.ToInt64(_Model.RefType) select eb).FirstOrDefault();
                        string msg = "ನಿಮ್ಮ ಹೊಸ ಎನ್ಬಿ ಬಾಂಡ್ ನೀತಿಯನ್ನು ರಚಿಸಲಾಗಿದೆ";
                        AllCommon objCommon = new AllCommon();
                        objCommon.SendEmailWithAttachment(mobile.email_id, msg, "KGID Policy Bond", stream);

                    }
                    catch
                    {

                    }
                }
                else
                {
                    _responce_model.Status = false;
                    _responce_model.Message = "failed";
                    _responce_model.return_reponce = "File unble to upload.";
                }

                return "true";
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                return "false";
            }
        }
        public string GenerateUniqueCode(int num)
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            characters += alphabets + small_alphabets + numbers;
            string otp = string.Empty;
            for (int i = 0; i < num; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
        public class SelCertAttribs
        {
            public string CertThumbPrint
            {
                get;
                set;
            }

            public string eMail
            {
                get;
                set;
            }

            public DateTime ExpDate
            {
                get;
                set;
            }

            public string PublicKey
            {
                get;
                set;
            }
            public string PrivateKey { get; set; }

            public string SelCertSubject
            {
                get;
                set;
            }

            public DateTime ValidFrom
            {
                get;
                set;
            }
            public object issuerName { get; internal set; }

            public SelCertAttribs()
            {
            }
        }
        public class Image_convert_model
        {
            public string File_Name { get; set; }
            public string File_bytes { get; set; }
            public string File_Path { get; set; }
            public string File_token { get; set; }
            public string RefID { get; set; }
            public string RefType { get; set; }
            public string DSC_user_name { get; set; }
        }
        public class RequestFile
        {
            public string RefID { get; set; }
            public string RefType { get; set; }
        }
        public class File_Responce_model
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public string return_reponce { get; set; }
        }
        #endregion
        #region NB DDO Intimation Letter
        [Route("VIEW-NB-DDO-IL")]
        public ActionResult ViewDDOIntimationLetter()
        {
            VM_DDOVerificationDetails verificationDetails = _Objemployee.GetEmployeeIntimationLetterBll(Convert.ToInt64(Session["UId"]));

            return View(verificationDetails);
        }
        #endregion

        [Route("mi-loa-vw-apln/{empId}/{applicationId}/{refNo}/{category}")]
        [Route("mi-vw-apln/{empId}/{applicationId}/{refNo}/{category}")]
        [Route("mi-soa-vw-apln/{empId}/{applicationId}/{refNo}/{category}")]
        public ActionResult MIViewApplication(long empId, long applicationId, long refNo, int category)
        {
            long EID = Convert.ToInt64(Session["UID"]);
            if (EID == empId)
            {
                VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
                if (refNo != 0)
                {
                    Session["RID"] = refNo;
                }
                if (empId != 0)
                {
                    verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetWorkFlowDetails(Convert.ToInt64(applicationId), Convert.ToInt32(category));
                    Session["RUID"] = empId;

                }
                return View(verificationDetails);
            }
            else
            {
                return View();
            }
        }

        #region Motor Insurance DDO
        [Route("DDOProDetails")]
        public ActionResult MIDDOVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;


            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("SaveDDOMIVData")]
        public ActionResult InsertVerifyDetailsMI(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            //if (Convert.ToInt32(result) == 1)
            //{
            //    TempData["VerifyDetails"] = objVerifyDetails;
            //}
            return RedirectToAction("DetailsForDDOVerification", "MotorInsurance", new { area = "" });
        }
        #endregion

        #region Motor Insurance CW
        [Route("mi_cw_va/{empId}/{applicationId}/{refNo}/{category}/{status?}")]
        public ActionResult MICWVerification(string empId, string applicationId, string refNo, string category, string status = "")
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (Convert.ToInt64(refNo) != 0)
            {
                Session["RID"] = refNo;
            }
            if (Convert.ToInt64(empId) != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetWorkFlowDetails(Convert.ToInt64(applicationId), Convert.ToInt32(category));
                Session["RUID"] = empId;
                //Session["SelectedCategory"] = category;
                //Session["Categories"] = category;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("SaveCWMIVData")]
        public string InsertVerifyDetailsCW(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            if (Convert.ToInt32(result) == 1)
            {
                if (objVerifyDetails.ApplicationStatus == 2)
                {

                }
                if (objVerifyDetails.ApplicationStatus == 7)
                {
                    //var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == objVerifyDetails.EmpCode select eb.mobile_number).FirstOrDefault();
                    //var appRefNo = (from ka in _db.tbl_kgid_application_details where ka.kad_application_id == objVerifyDetails.ApplicationId select ka.kad_kgid_application_number).FirstOrDefault();
                    //var assignedto = (from kw in _db.tbl_kgid_application_workflow_details where kw.kawt_application_id == objVerifyDetails.ApplicationId && kw.kawt_active_status == true select kw.kawt_assigned_to).FirstOrDefault();
                    //var DistrictOffice = (from ew in _db.tbl_employee_work_details
                    //                      join ddo in _db.tbl_ddo_master on ew.ewd_ddo_id equals ddo.dm_ddo_id
                    //                      where ew.ewd_emp_id == assignedto
                    //                      select ddo.dm_ddo_office).FirstOrDefault();


                    //string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + appRefNo + " ಯು ದಿನಾಂಕ " + DateTime.Now + " ದಂದು ಜಿಲ್ಲಾ ವಿಮಾ ಕಛೇರಿ, " + DistrictOffice + " ಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ."
                    // + "– ವಿಮಾ ಇಲಾಖೆ(KGID).";
                    ////AllCommon.sendOTPMSG(mobile.ToString(), msg);

                }
                //TempData["VerifyDetails"] = objVerifyDetails;
                ViewBag.VerifyDetails = objVerifyDetails;
            }
            return result;
            //return RedirectToAction("DetailsForCWVerification", "MotorInsurance", new { @Category = objVerifyDetails.Category });
        }
        #endregion

        #region Motor Insurance Superintendent
        [Route("mi_si_va/{empId}/{applicationId}/{refNo}/{category}/{status?}")]
        public ActionResult MISuperintendentVerification(long empId, long applicationId, long refNo, int category, string status = "")
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;
                //Session["SelectedCategory"] = category;
                //Session["Categories"] = category;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("SaveSIMIVData")]
        public string InsertVerifyDetailsSuperintendent(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            if (Convert.ToInt32(result) == 1)
            {

            }

           // TempData["VerifyDetails"] = objVerifyDetails;
            ViewBag.VerifyDetails = objVerifyDetails;
            //return RedirectToAction("DetailsForSuperintendentVerification", "MotorInsurance", new { area = "" });
            //RedirectToAction("DetailsForSuperintendentVerification", "MotorInsurance", new { @Category = objVerifyDetails.Category });

            return result;

        }
        #endregion

        #region Motor Insurance DIO
        [Route("DIOPropDetails")]
        public ActionResult MIDIOVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;
                Session["SelectedCategory"] = category;
                Session["Categories"] = category;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("SaveDIOMIVData")]
        public ActionResult InsertVerifyDetailsDIO(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            //if (Convert.ToInt32(result) == 1)
            //{
            //    TempData["VerifyDetails"] = objVerifyDetails;
            //}
            //return RedirectToAction("DetailsForDIOVerification", "MotorInsurance", new { area = "" });
            return RedirectToAction("DetailsForDIOVerification", "MotorInsurance", new { @Category = objVerifyDetails.Category });
        }
        #endregion

        #region Motor insurance AD
        [Route("mi_ad_va/{empId}/{applicationId}/{refNo}/{category}/{status?}")]
        public ActionResult MIADVerification(long empId, long applicationId, long refNo, int category, string status = "")
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;
                //Session["SelectedCategory"] = category;
                //Session["Categories"] = category;
            }
            return View(verificationDetails);
        }

        [HttpPost]
        [Route("SaveADMIVData")]
        public ActionResult InsertVerifyDetailsAD(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            // Save FacingSheet END
            if (objVerifyDetails.ApplicationStatus != 15 && Convert.ToInt32(result) > 0)
            {
                return Json(new { PolicyNumber = string.Empty, RedirectUrl = "/mi-ad-adt" }, JsonRequestBehavior.AllowGet);
            }
            else if (objVerifyDetails.ApplicationStatus == 15 && !string.IsNullOrEmpty(result))
            {
                //CREATE AND SAVE MB BOND
                string MB_Result = string.Empty;
                VM_MotorInsurancePolicyPrintDetails MIPD = _IMIApplicationbll.MIPolicyPrintDetailsBll("NEW", objVerifyDetails.EmpCode, objVerifyDetails.ApplicationRefNo);//Type,EmpID,AppRefNo
                VM_MB_Bond_Details _MB_Bond = new VM_MB_Bond_Details();
                _MB_Bond = GetMBBondCalculationDetails(MIPD);
                string MIBondPath = FillFormMB(_MB_Bond, result);
                //Save Bond
                MB_Result = _IMIApplicationbll.MBBondDocUploadBLL(objVerifyDetails.ApplicationRefNo, objVerifyDetails.EmpCode, MIBondPath, "NEW");//AppRefNo,EmpID,DocPath,Type
                return Json(new { PolicyNumber = result, RedirectUrl = "/mi-ad-adt" }, JsonRequestBehavior.AllowGet);

                //              string Month = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).AddMonths(1).ToString("MMMM");
                //              string Year = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).ToString("yy");
                //              var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == caseWorkerVerifiedDetails.EmpCode select eb).FirstOrDefault();
                //              string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + facingSheetaprvd.ApplicationNumber + " ಯು ಅಂಗೀಕಾರವಾಗಿದ್ದು, ನಿಮ್ಮ ಪಾಲಿಸಿ ಸಂಖ್ಯೆ " + returnString + " ಆಗಿರುತ್ತದೆ.ಪಾಲಿಸಿ ಬಾಂಡ್‌ ಮತ್ತು ಅಂಗೀಕಾರದ ಸೂಚನೆಗಳನ್ನು ನಿಮ್ಮ ರಿಜಿಸ್ಟರ್ಡ್‌ ಇ-ಮೇಲ್‌ / KGID ಲಾಗಿನ್ ನಲ್ಲಿ ಡೌನ್‌ಲೋಡ್‌ ಮಾಡಿಕೊಳ್ಳಬಹುದಾಗಿದೆ. ಮುಂದುವರೆದು, ರೂ " + _empNBBond.EmployeeBasicDetails.p_premium + " ಗಳನ್ನು " + Month + "/" + Year + " ತಿಂಗಳು / ವರ್ಷ ದಿಂದ ಕ್ರಮವಾಗಿ ತಮ್ಮ ವೇತನದಿಂದ ಕಟಾವಣೆ ಮಾಡಿಸತಕ್ಕದ್ದು."
                //                       + " - ವಿಮಾ ಇಲಾಖೆ(KGID).";
                //              //AllCommon.sendOTPMSG(mobile.mobile_number.ToString(), msg);
                //              string emailmsg = "Dear Insured," + "\r\n"
                //+ "We are glad to Inform you that, your proposal with reference no " + facingSheetaprvd.ApplicationNumber + " has been accepted and KGID Policy bond has been generated on " + _empNBBond.EmployeeBasicDetails.p_sanction_date + "\r\n"
                //+ " The Policy details are as follows: KGID Policy No " + returnString
                //+ " Please find the KGID Policy Bond and the acceptance notice attached."
                //+ "This document is digitally signed ,hence does not require physical signature."
                //+ " Warm Regards,"
                //+ "KGID, Official Branch";
                //              AllCommon objemail = new AllCommon();
                //              // objemail.SendEmail(mobile.email_id, emailmsg, "KGID Policy Bond.");

                //return Json(new { PolicyNumber = result, RedirectUrl = "/mi-dd-dpt/" + objVerifyDetails.Category }, JsonRequestBehavior.AllowGet);
                ////return Json(new { PolicyNumber = returnString, UnSignedBond = filepath, AppId = PolicyNo, EmpId = EmployeeId, RedirectUrl = "/kgid-d/" }, JsonRequestBehavior.AllowGet);            }
            }

            return null;
        }
        #endregion


        #region Motor Insurance DD
        [Route("mi_dd_va/{empId}/{applicationId}/{refNo}/{category}/{status?}")]
        public ActionResult MIDDVerification(long empId, long applicationId, long refNo, int category, string status = "")
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;
                //Session["SelectedCategory"] = category;
                //Session["Categories"] = category;
            }
            return View(verificationDetails);
        }
        //[HttpPost]
        //[Route("SaveDDMIVData")]
        //public ActionResult InsertVerifyDetailsDD(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        //{
        //    objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
        //    //string result = _IMIApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
        //    string result = "BNG112233445566123456";
        //    if (objVerifyDetails.ApplicationStatus == 15 && !string.IsNullOrEmpty(result))
        //    {
        //        int PolicyNo = 200;//Application ID
        //        VM_NBBond _empNBBond = new VM_NBBond();
        //        _empNBBond = _Objemployee.getNBBondDetails(PolicyNo);
        //        string filepath = FillFormMB(_empNBBond, result);
        //        string signedfilepath= MBSignPdf(filepath,result);
        //        FileInfo file = new FileInfo(filepath);
        //        file.Delete();
        //        //MB Bond
        //        //VM_MotorInsurancePolicyPrintDetails MIPD = _IMIApplicationbll.MIPolicyPrintDetailsBll("New", 1, 1122020102555);//Type,EmpID,AppRefNo
        //        //VM_MB_Bond_Details _MB_Bond = new VM_MB_Bond_Details();
        //        //_MB_Bond = GetMBBondCalculationDetails(MIPD);
        //    }
        //    //return RedirectToAction("DetailsForDDVerification", "MotorInsurance", new { area = "" });
        //    return RedirectToAction("DetailsForDDVerification", "MotorInsurance", new { @Category = objVerifyDetails.Category });
        //}
        [HttpPost]
        [Route("SaveDDMIVData")]
        public ActionResult InsertVerifyDetailsDD(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            // Save FacingSheet END
            if (objVerifyDetails.ApplicationStatus != 15 && Convert.ToInt32(result) > 0)
            {
                return Json(new { PolicyNumber = string.Empty, RedirectUrl = "/mi-dd-adt" }, JsonRequestBehavior.AllowGet);
            }
            else if (objVerifyDetails.ApplicationStatus == 15 && !string.IsNullOrEmpty(result))
            {
                //CREATE AND SAVE MB BOND
                string MB_Result = string.Empty;
                VM_MotorInsurancePolicyPrintDetails MIPD = _IMIApplicationbll.MIPolicyPrintDetailsBll("NEW", objVerifyDetails.EmpCode, objVerifyDetails.ApplicationRefNo);//Type,EmpID,AppRefNo
                VM_MB_Bond_Details _MB_Bond = new VM_MB_Bond_Details();
                _MB_Bond = GetMBBondCalculationDetails(MIPD);
                string MIBondPath = FillFormMB(_MB_Bond, result);
                //Save Bond
                MB_Result = _IMIApplicationbll.MBBondDocUploadBLL(objVerifyDetails.ApplicationRefNo, objVerifyDetails.EmpCode, MIBondPath, "NEW");//AppRefNo,EmpID,DocPath,Type
                return Json(new { PolicyNumber = result, RedirectUrl = "/mi-dd-adt" }, JsonRequestBehavior.AllowGet);

                //              string Month = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).AddMonths(1).ToString("MMMM");
                //              string Year = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).ToString("yy");
                //              var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == caseWorkerVerifiedDetails.EmpCode select eb).FirstOrDefault();
                //              string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + facingSheetaprvd.ApplicationNumber + " ಯು ಅಂಗೀಕಾರವಾಗಿದ್ದು, ನಿಮ್ಮ ಪಾಲಿಸಿ ಸಂಖ್ಯೆ " + returnString + " ಆಗಿರುತ್ತದೆ.ಪಾಲಿಸಿ ಬಾಂಡ್‌ ಮತ್ತು ಅಂಗೀಕಾರದ ಸೂಚನೆಗಳನ್ನು ನಿಮ್ಮ ರಿಜಿಸ್ಟರ್ಡ್‌ ಇ-ಮೇಲ್‌ / KGID ಲಾಗಿನ್ ನಲ್ಲಿ ಡೌನ್‌ಲೋಡ್‌ ಮಾಡಿಕೊಳ್ಳಬಹುದಾಗಿದೆ. ಮುಂದುವರೆದು, ರೂ " + _empNBBond.EmployeeBasicDetails.p_premium + " ಗಳನ್ನು " + Month + "/" + Year + " ತಿಂಗಳು / ವರ್ಷ ದಿಂದ ಕ್ರಮವಾಗಿ ತಮ್ಮ ವೇತನದಿಂದ ಕಟಾವಣೆ ಮಾಡಿಸತಕ್ಕದ್ದು."
                //                       + " - ವಿಮಾ ಇಲಾಖೆ(KGID).";
                //              //AllCommon.sendOTPMSG(mobile.mobile_number.ToString(), msg);
                //              string emailmsg = "Dear Insured," + "\r\n"
                //+ "We are glad to Inform you that, your proposal with reference no " + facingSheetaprvd.ApplicationNumber + " has been accepted and KGID Policy bond has been generated on " + _empNBBond.EmployeeBasicDetails.p_sanction_date + "\r\n"
                //+ " The Policy details are as follows: KGID Policy No " + returnString
                //+ " Please find the KGID Policy Bond and the acceptance notice attached."
                //+ "This document is digitally signed ,hence does not require physical signature."
                //+ " Warm Regards,"
                //+ "KGID, Official Branch";
                //              AllCommon objemail = new AllCommon();
                //              // objemail.SendEmail(mobile.email_id, emailmsg, "KGID Policy Bond.");

                //return Json(new { PolicyNumber = result, RedirectUrl = "/mi-dd-dpt/" + objVerifyDetails.Category }, JsonRequestBehavior.AllowGet);
                ////return Json(new { PolicyNumber = returnString, UnSignedBond = filepath, AppId = PolicyNo, EmpId = EmployeeId, RedirectUrl = "/kgid-d/" }, JsonRequestBehavior.AllowGet);            }
            }
            return null;
        }
        #endregion
        #region Print Premium Calculation
        public ActionResult GetPremiumCalcSheet(long applicationId, long empid, string Type)
        {
            VM_MotorInsurancePolicyPrintDetails MIPD = _IMIApplicationbll.MIPolicyPrintDetailsBll(Type, empid, applicationId);//Type,EmpID,AppRefNo
            VM_MB_Bond_Details _MB_Bond = new VM_MB_Bond_Details();
            if (Type == "New")
            {
                _MB_Bond = GetInsuranceCalculationDetails(MIPD);
            }
            else if (Type == "Renewal")
            {
                _MB_Bond = GetInsuranceCalculationDetailsRenewal(MIPD);
            }
            string MIBondPath = FillFormMB(_MB_Bond, Convert.ToString(MIPD.application_ref_no));
            return File(MIBondPath, "application/pdf");
        }
        public VM_MB_Bond_Details GetInsuranceCalculationDetails(VM_MotorInsurancePolicyPrintDetails MIPD)
        {
            VM_MB_Bond_Details MB_Bond = new VM_MB_Bond_Details();
            //Policy Details
            MB_Bond.PolicyPremiumAmount = MIPD.premium;
            //MB_Bond.PolicyNumber = MIPD.policy_number;
            //MB_Bond.From_DateTime = MIPD.from_date;
            // DateTime FD = MIPD.from_date.Value;
            //MB_Bond.From_Date = FD.ToString("dd-MM-yyyy");
            //MB_Bond.To_DateTime = MIPD.to_date;
            //DateTime TD = MIPD.to_date.Value;
            //MB_Bond.To_Date = TD.ToString("dd-MM-yyyy");
            //MB_Bond.Sanction_DateTime = MIPD.sanction_date;
            //DateTime SD = MIPD.sanction_date.Value;
            //MB_Bond.Sanction_Date = SD.ToString("dd-MM-yyyy");
            //
            //Proposer Details
            MB_Bond.ProposerNameAddress = MIPD.nameandaddress;
            MB_Bond.PolicyType = MIPD.type_of_cover == 1 ? "Liability Only Policy"
         : MIPD.type_of_cover == 2 ? "Package Policy"
         : MIPD.type_of_cover == 3 ? "Bundle Policy"
         : MIPD.type_of_cover == 4 ? "O"
         : "";


            //Vehicle Details
            MB_Bond.Application_ref_no = MIPD.application_ref_no;
            MB_Bond.Proposer_id = MIPD.proposer_id;
            MB_Bond.Regisration_no = MIPD.regisration_no;
            MB_Bond.Registration_authority_and_location = MIPD.registration_authority_and_location;
            MB_Bond.Year_of_manufacturer = MIPD.year_of_manufacturer;
            MB_Bond.Chasis_no = MIPD.chasis_no;
            MB_Bond.Engine_no = MIPD.engine_no;
            MB_Bond.SeatingCapacity = MIPD.seating_capacity_including_driver;
            MB_Bond.Cubic_capacity = MIPD.cubic_capacity;
            MB_Bond.GVW = Convert.ToString(MIPD.vehicle_weight);
            MB_Bond.Zone = MIPD.zone;
            MB_Bond.VehicleCategory = MIPD.vehiclecategory;
            MB_Bond.Make_of_vehicle = MIPD.vehicleClass;
            MB_Bond.Endorsements = MIPD.Endorsements;
            //MB_Bond.vehicle_class = MIPD.vehicleClass;
            //IDV Details
            MB_Bond.Insured_declared_value_amount = MIPD.insured_declared_value_amount;
            MB_Bond.Side_car_trailer_amount = MIPD.side_car_trailer_amount;
            MB_Bond.Non_electrical_accessories_amount = MIPD.non_electrical_accessories_amount;
            MB_Bond.Electrical_accessories_amount = MIPD.electrical_accessories_amount;
            MB_Bond.Value_of_cng_lpg_amount = MIPD.value_of_cng_lpg_amount;
            MB_Bond.Depreciation = Convert.ToString(MIPD.depreciation_value);
            decimal total_amount = Convert.ToDecimal(MIPD.total_amount);
            string ctotal_amount = String.Format("{0:0,0.00}", total_amount);
            MB_Bond.Idv_total_amount = ctotal_amount;
            decimal GrossVehicleWeight = 0;
            if (MIPD.vehiclecategory == 5 || MIPD.vehiclecategory == 6)
            {
                decimal weight = MIPD.vehicle_weight;
                decimal actualweight = 12000;
                if (weight > actualweight)
                {
                    decimal weightdiff = (weight - actualweight) / 100;
                    GrossVehicleWeight = weightdiff * 27;
                }
            }
            int rGrossVehicleWeight = Convert.ToInt32(GrossVehicleWeight);
            string cGrossVehicleWeight = String.Format("{0:0,0.00}", rGrossVehicleWeight);
            if (MIPD.type_of_cover == 2)
            {
                //A. Own Damage
                MB_Bond.BP = Convert.ToString(MIPD.mia_own_damage_additional_value);
                if (MB_Bond.BP == "0")
                {
                    MB_Bond.BP = "";
                }
                MB_Bond.ODPercentage = Convert.ToString(MIPD.own_damage_value);
                decimal odvalue = Convert.ToDecimal(MIPD.own_damage_value);
                decimal res = ((total_amount / 100) * (odvalue));
                int rres = Convert.ToInt32(res);
                decimal odsubtot = Convert.ToDecimal(MIPD.mia_own_damage_additional_value) + rres;
                int rodsubtot = Convert.ToInt32(odsubtot);
                string cres = String.Format("{0:0,0.00}", rodsubtot);
                MB_Bond.ODPercentageValue = String.Format("{0:0,0.00}", rres);
                MB_Bond.ODSubTotal1 = cres;
                MB_Bond.ODPremium = cres;
                MB_Bond.GVWExtraAmount = cGrossVehicleWeight;
                //MB_Bond.txtbpidvValue = cres;
                decimal res1 = ((odsubtot / 100) * Convert.ToDecimal(MIPD.od_gov_discount));
                int rres1 = Convert.ToInt32(res1);
                string cres1 = String.Format("{0:0,0.00}", rres1);
                MB_Bond.ODGovtRebate = Convert.ToString(MIPD.od_gov_discount);
                MB_Bond.ODGovtRebateValue = cres1;
                //MB_Bond.txtlgrodValue = cres1;
                decimal txtrebatetotodvalue = rodsubtot - rres1 + rGrossVehicleWeight;
                int rtxtrebatetotodvalue = Convert.ToInt32(txtrebatetotodvalue);
                string ctxtrebatetotodvalue = String.Format("{0:0,0.00}", rtxtrebatetotodvalue);
                MB_Bond.ODSubTotal2 = ctxtrebatetotodvalue;
                decimal Addele = ((Convert.ToDecimal(MIPD.electrical_accessories_amount) / 100) * 4);
                decimal Addlpg = ((Convert.ToDecimal(MIPD.value_of_cng_lpg_amount) / 100) * 4);
                decimal FiberGlassValue = 0;
                if (MIPD.isfiberglassfitted == true)
                {
                    if (MIPD.vehiclecategory == 16)
                    {
                        FiberGlassValue = 100;
                    }
                    else
                    {
                        FiberGlassValue = 50;
                    }
                    MB_Bond.FibreGlassFuelTank = String.Format("{0:0,0.00}", FiberGlassValue); ;
                }
                decimal ODDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    ODDrvingInstitutionValue = ((Convert.ToDecimal(txtrebatetotodvalue) / 100) * 60);
                    MB_Bond.ODDrivingInstitutionPercentage = "60";
                    MB_Bond.ODDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", ODDrvingInstitutionValue); ;
                }
                decimal Addnonele = ((Convert.ToDecimal(MIPD.non_electrical_accessories_amount) / 100) * 4);
                decimal ODAddSubtot = txtrebatetotodvalue + Addele + Addlpg + FiberGlassValue + ODDrvingInstitutionValue + Addnonele;
                int rODAddSubtot = Convert.ToInt32(ODAddSubtot);
                MB_Bond.EAPercentage = "4";
                MB_Bond.EAPercentageValue = String.Format("{0:0,0.00}", Addele);
                MB_Bond.ODLPGKitPercentage = "4";
                MB_Bond.ODLPGKitValue = String.Format("{0:0,0.00}", Addlpg);
                MB_Bond.ODAddOtherPercentage = "4";
                MB_Bond.ODAddOtherPercentageValue = String.Format("{0:0,0.00}", Addnonele);
                MB_Bond.ODSubTotal3 = String.Format("{0:0,0.00}", rODAddSubtot);
                decimal AutoMobileValue = 0;
                if (MIPD.vehicletype == 1)
                {
                    if (MIPD.isautomobileassociation == true)
                    {
                        AutoMobileValue = ((rODAddSubtot / 100) * 5);
                        MB_Bond.AutomobilePercentage = "5";
                        MB_Bond.AutomobileValue = String.Format("{0:0,0.00}", AutoMobileValue); ;
                    }
                }
                decimal ODLessSubtot = ODAddSubtot - AutoMobileValue;
                int rODLessSubtot = Convert.ToInt32(ODLessSubtot);
                MB_Bond.ODSubTotal4 = String.Format("{0:0,0.00}", rODLessSubtot);

                //MB_Bond.ODMalusValue=
                MB_Bond.ODMalus = MIPD.ph_malus_value.ToString();

                decimal txtlessmalusValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtlessmalusValue = Convert.ToInt32(txtlessmalusValue);
                string ctxtlessmalusValue = String.Format("{0:0,0.00}", rtxtlessmalusValue);
                MB_Bond.ODMalusValue = ctxtlessmalusValue;

                MB_Bond.ODNCB = MIPD.ph_ncb_value.ToString();

                //MB_Bond.txtrebatetotodvalue = ctxtrebatetotodvalue;
                decimal txtlessncbValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_ncb_value));
                int rtxtlessncbValue = Convert.ToInt32(txtlessncbValue);
                string ctxtlessncbValue = String.Format("{0:0,0.00}", rtxtlessncbValue);
                MB_Bond.ODNCBValue = ctxtlessncbValue;

                //MB_Bond.txtrebatetotodvalue = ctxtlessncbValue;
                decimal txtodtotValue = (rODLessSubtot + rtxtlessmalusValue) - rtxtlessncbValue;
                int rtxtodtotValue = Convert.ToInt32(txtodtotValue);
                decimal sidecardiscount = 0;
                if (Convert.ToDecimal(MIPD.side_car_trailer_amount) > 1)
                {
                    sidecardiscount = ((rtxtodtotValue / 100) * 25);
                }
                decimal odafterSDdiscount = rtxtodtotValue - sidecardiscount;
                int rodafterSDdiscount = Convert.ToInt32(odafterSDdiscount);
                string ctxtodtotValue = String.Format("{0:0,0.00}", rodafterSDdiscount);
                MB_Bond.ODOthersPercentage = "25";
                MB_Bond.ODOthersValue = String.Format("{0:0,0.00}", sidecardiscount);
                MB_Bond.ODTotal = ctxtodtotValue;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                decimal LPRvalue = Convert.ToDecimal(MIPD.premium_liability_value);
                int rLPRvalue = Convert.ToInt32(LPRvalue);
                decimal txtlgrlprValue = ((LPRvalue / 100) * Convert.ToDecimal(MIPD.liability_gov_discount));
                int rtxtlgrlprValue = Convert.ToInt32(txtlgrlprValue);
                string ctxtlgrlprValue = String.Format("{0:0,0.00}", rtxtlgrlprValue);
                MB_Bond.LPRValue = Convert.ToString(MIPD.premium_liability_value);
                MB_Bond.LPRGovtRebate = Convert.ToString(MIPD.liability_gov_discount);
                MB_Bond.LPRGovtRebateValue = ctxtlgrlprValue;
                //MB_Bond.txtlgrlprValue = ctxtlgrlprValue;
                decimal txtsubtotlprValue = rLPRvalue - rtxtlgrlprValue;
                int rtxtsubtotlprValue = Convert.ToInt32(txtsubtotlprValue);
                string ctxtsubtotlprValue = String.Format("{0:0,0.00}", rtxtsubtotlprValue);
                MB_Bond.LPRSubTotal1 = ctxtsubtotlprValue;
                //MB_Bond.txtsubtotlprValue = ctxtsubtotlprValue;
                decimal txtcngamntrValue = Convert.ToDecimal(MIPD.value_of_cng_lpg_amount);
                int rtxtcngamntrValue = Convert.ToInt32(txtcngamntrValue);
                int txtlpgkitlprValue = 0;
                if (rtxtcngamntrValue != 0)
                {
                    txtlpgkitlprValue = 60;
                    MB_Bond.LPRLPGKitValue = String.Format("{0:0,0.00}", txtlpgkitlprValue);
                }
                int rtxtsubtotlpglprValue = rtxtsubtotlprValue + txtlpgkitlprValue;
                string crtxtsubtotlpglprValue = String.Format("{0:0,0.00}", rtxtsubtotlpglprValue);
                MB_Bond.LPRSubTotal2 = crtxtsubtotlpglprValue;
                decimal DriverAmount = Convert.ToDecimal(MIPD.driver_amount);
                string cDriverAmount = String.Format("{0:0,0.00}", DriverAmount);
                MB_Bond.DriverRisk = cDriverAmount;
                //MB_Bond.res10 = cDriverAmount;
                decimal PassengerAmount = 0;
                string cPassengerAmount = string.Empty;
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    PassengerAmount = Convert.ToDecimal(MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else if (MIPD.vehiclecategory == 17)
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver - 1) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    MB_Bond.PillionRisk = cPassengerAmount;
                }
                else
                {
                    MB_Bond.PassengersRisk = cPassengerAmount;
                }
                //MB_Bond.PassengersRisk = cPassengerAmount;
                //MB_Bond.res11 = cPassengerAmount;
                decimal txtlprtotValue = (rtxtsubtotlpglprValue + DriverAmount + PassengerAmount);
                int rtxtlprtotValue = Convert.ToInt32(txtlprtotValue);
                string ctxtlprtotValue = String.Format("{0:0,0.00}", rtxtlprtotValue);
                MB_Bond.LPRTotal = ctxtlprtotValue;
                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = rodafterSDdiscount + rtxtlprtotValue;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = ctxtodtotValue;
                MB_Bond.TotalB = ctxtlprtotValue;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            else if (MIPD.type_of_cover == 1)
            {
                //A. Own Damage
                MB_Bond.ODTotal = string.Empty;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                decimal LPRvalue = Convert.ToDecimal(MIPD.premium_liability_value);
                int rLPRvalue = Convert.ToInt32(LPRvalue);
                decimal txtlgrlprValue = Math.Round(((LPRvalue / 100) * Convert.ToDecimal(MIPD.liability_gov_discount)));
                int rtxtlgrlprValue = Convert.ToInt32(txtlgrlprValue);
                string ctxtlgrlprValue = String.Format("{0:0,0.00}", rtxtlgrlprValue);
                MB_Bond.LPRValue = Convert.ToString(MIPD.premium_liability_value);
                MB_Bond.LPRGovtRebate = Convert.ToString(MIPD.liability_gov_discount);
                MB_Bond.LPRGovtRebateValue = ctxtlgrlprValue;
                //MB_Bond.txtlgrlprValue = ctxtlgrlprValue;
                decimal txtsubtotlprValue = Math.Round(LPRvalue - txtlgrlprValue);
                int rtxtsubtotlprValue = Convert.ToInt32(txtsubtotlprValue);
                string ctxtsubtotlprValue = String.Format("{0:0,0.00}", rtxtsubtotlprValue);
                MB_Bond.LPRSubTotal1 = ctxtsubtotlprValue;
                //MB_Bond.txtsubtotlprValue = ctxtsubtotlprValue;
                decimal LPRDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    LPRDrvingInstitutionValue = ((Convert.ToDecimal(rtxtsubtotlprValue) / 100) * 60);
                    MB_Bond.LPRDrivingInstitutionPercentage = "60";
                    MB_Bond.LPRDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", LPRDrvingInstitutionValue); ;
                }
                decimal txtcngamntrValue = Convert.ToDecimal(MIPD.value_of_cng_lpg_amount);
                int rtxtcngamntrValue = Convert.ToInt32(txtcngamntrValue);
                int txtlpgkitlprValue = 0;
                if (rtxtcngamntrValue != 0)
                {
                    txtlpgkitlprValue = 60;
                    MB_Bond.LPRLPGKitValue = String.Format("{0:0,0.00}", txtlpgkitlprValue);
                }
                int rtxtsubtotlpglprValue = rtxtsubtotlprValue + txtlpgkitlprValue+ Convert.ToInt32(LPRDrvingInstitutionValue);
                string crtxtsubtotlpglprValue = String.Format("{0:0,0.00}", rtxtsubtotlpglprValue);
                MB_Bond.LPRSubTotal2 = crtxtsubtotlpglprValue;
                decimal DriverAmount = Convert.ToDecimal(MIPD.driver_amount);
                string cDriverAmount = String.Format("{0:0,0.00}", DriverAmount);
                MB_Bond.DriverRisk = cDriverAmount;
                //MB_Bond.res10 = cDriverAmount;
                decimal PassengerAmount = 0;
                string cPassengerAmount = string.Empty;
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    PassengerAmount = Convert.ToDecimal(MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else if (MIPD.vehiclecategory == 17)
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver - 1) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    MB_Bond.PillionRisk = cPassengerAmount;
                }
                else
                {
                    MB_Bond.PassengersRisk = cPassengerAmount;
                }
                //MB_Bond.PassengersRisk = cPassengerAmount;
                //MB_Bond.res11 = cPassengerAmount;

                MB_Bond.LPRMalus = MIPD.ph_malus_value.ToString();

                decimal txtaddmalusValue = ((rtxtsubtotlpglprValue / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtaddmalusValue = Convert.ToInt32(txtaddmalusValue);
                string ctxtaddmalusValue = String.Format("{0:0,0.00}", rtxtaddmalusValue);
                MB_Bond.LPRMalusValue = ctxtaddmalusValue;

                decimal txtlprtotValue = (rtxtsubtotlpglprValue + DriverAmount + PassengerAmount + rtxtaddmalusValue);
                int rtxtlprtotValue = Convert.ToInt32(txtlprtotValue);
                string ctxtlprtotValue = String.Format("{0:0,0.00}", rtxtlprtotValue);
                MB_Bond.LPRTotal = ctxtlprtotValue;
                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = 0 + rtxtlprtotValue;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = string.Empty;
                MB_Bond.TotalB = ctxtlprtotValue;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            else if (MIPD.type_of_cover == 3)
            {
                //A. Own Damage
                MB_Bond.BP = Convert.ToString(MIPD.mia_own_damage_additional_value);
                if (MB_Bond.BP == "0")
                {
                    MB_Bond.BP = "";
                }
                MB_Bond.ODPercentage = Convert.ToString(MIPD.own_damage_value);
                decimal odvalue = Convert.ToDecimal(MIPD.own_damage_value);
                decimal res = ((total_amount / 100) * (odvalue));
                int rres = Convert.ToInt32(res);
                decimal odsubtot = Convert.ToDecimal(MIPD.mia_own_damage_additional_value) + rres;
                int rodsubtot = Convert.ToInt32(odsubtot);
                string cres = String.Format("{0:0,0.00}", rodsubtot);
                MB_Bond.ODPercentageValue = String.Format("{0:0,0.00}", rres);
                MB_Bond.ODSubTotal1 = cres;
                MB_Bond.ODPremium = cres;
                MB_Bond.GVWExtraAmount = cGrossVehicleWeight;
                //MB_Bond.txtbpidvValue = cres;
                decimal res1 = ((odsubtot / 100) * Convert.ToDecimal(MIPD.od_gov_discount));
                int rres1 = Convert.ToInt32(res1);
                string cres1 = String.Format("{0:0,0.00}", rres1);
                MB_Bond.ODGovtRebate = Convert.ToString(MIPD.od_gov_discount);
                MB_Bond.ODGovtRebateValue = cres1;
                //MB_Bond.txtlgrodValue = cres1;
                decimal txtrebatetotodvalue = rodsubtot - rres1 + rGrossVehicleWeight;
                int rtxtrebatetotodvalue = Convert.ToInt32(txtrebatetotodvalue);
                string ctxtrebatetotodvalue = String.Format("{0:0,0.00}", rtxtrebatetotodvalue);
                MB_Bond.ODSubTotal2 = ctxtrebatetotodvalue;
                decimal Addele = ((Convert.ToDecimal(MIPD.electrical_accessories_amount) / 100) * 4);
                decimal Addlpg = ((Convert.ToDecimal(MIPD.value_of_cng_lpg_amount) / 100) * 4);
                decimal FiberGlassValue = 0;
                if (MIPD.isfiberglassfitted == true)
                {
                    if (MIPD.vehiclecategory == 16)
                    {
                        FiberGlassValue = 100;
                    }
                    else
                    {
                        FiberGlassValue = 50;
                    }
                    MB_Bond.FibreGlassFuelTank = String.Format("{0:0,0.00}", FiberGlassValue); ;
                }
                decimal ODDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    ODDrvingInstitutionValue = ((Convert.ToDecimal(txtrebatetotodvalue) / 100) * 60);
                    MB_Bond.ODDrivingInstitutionPercentage = "60";
                    MB_Bond.ODDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", ODDrvingInstitutionValue); ;
                }
                decimal Addnonele = ((Convert.ToDecimal(MIPD.non_electrical_accessories_amount) / 100) * 4);
                decimal ODAddSubtot = txtrebatetotodvalue + Addele + Addlpg + FiberGlassValue + ODDrvingInstitutionValue + Addnonele;
                int rODAddSubtot = Convert.ToInt32(ODAddSubtot);
                MB_Bond.EAPercentage = "4";
                MB_Bond.EAPercentageValue = String.Format("{0:0,0.00}", Addele);
                MB_Bond.ODLPGKitPercentage = "4";
                MB_Bond.ODLPGKitValue = String.Format("{0:0,0.00}", Addlpg);
                MB_Bond.ODAddOtherPercentage = "4";
                MB_Bond.ODAddOtherPercentageValue = String.Format("{0:0,0.00}", Addnonele);
                MB_Bond.ODSubTotal3 = String.Format("{0:0,0.00}", rODAddSubtot);
                decimal AutoMobileValue = 0;
                if (MIPD.vehicletype == 1)
                {
                    if (MIPD.isautomobileassociation == true)
                    {
                        AutoMobileValue = ((rODAddSubtot / 100) * 5);
                        MB_Bond.AutomobilePercentage = "5";
                        MB_Bond.AutomobileValue = String.Format("{0:0,0.00}", AutoMobileValue); ;
                    }
                }
                decimal ODLessSubtot = ODAddSubtot - AutoMobileValue;
                int rODLessSubtot = Convert.ToInt32(ODLessSubtot);
                MB_Bond.ODSubTotal4 = String.Format("{0:0,0.00}", rODLessSubtot);

                //MB_Bond.ODMalusValue=
                MB_Bond.ODMalus = MIPD.ph_malus_value.ToString();

                decimal txtlessmalusValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtlessmalusValue = Convert.ToInt32(txtlessmalusValue);
                string ctxtlessmalusValue = String.Format("{0:0,0.00}", rtxtlessmalusValue);
                MB_Bond.ODMalusValue = ctxtlessmalusValue;

                MB_Bond.ODNCB = MIPD.ph_ncb_value.ToString();

                //MB_Bond.txtrebatetotodvalue = ctxtrebatetotodvalue;
                decimal txtlessncbValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_ncb_value));
                int rtxtlessncbValue = Convert.ToInt32(txtlessncbValue);
                string ctxtlessncbValue = String.Format("{0:0,0.00}", rtxtlessncbValue);
                MB_Bond.ODNCBValue = ctxtlessncbValue;

                //MB_Bond.txtrebatetotodvalue = ctxtlessncbValue;
                decimal txtodtotValue = (rODLessSubtot + rtxtlessmalusValue) - rtxtlessncbValue;
                int rtxtodtotValue = Convert.ToInt32(txtodtotValue);
                decimal sidecardiscount = 0;
                if (Convert.ToDecimal(MIPD.side_car_trailer_amount) > 1)
                {
                    sidecardiscount = ((rtxtodtotValue / 100) * 25);
                }
                decimal odafterSDdiscount = rtxtodtotValue - sidecardiscount;
                int rodafterSDdiscount = Convert.ToInt32(odafterSDdiscount);
                string ctxtodtotValue = String.Format("{0:0,0.00}", rodafterSDdiscount);
                MB_Bond.ODOthersPercentage = "25";
                MB_Bond.ODOthersValue = String.Format("{0:0,0.00}", sidecardiscount);
                MB_Bond.ODTotal = ctxtodtotValue;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                decimal LPRvalue = Convert.ToDecimal(MIPD.premium_liability_value);
                int rLPRvalue = Convert.ToInt32(LPRvalue);
                decimal txtlgrlprValue = ((LPRvalue / 100) * Convert.ToDecimal(MIPD.liability_gov_discount));
                int rtxtlgrlprValue = Convert.ToInt32(txtlgrlprValue);
                string ctxtlgrlprValue = String.Format("{0:0,0.00}", rtxtlgrlprValue);
                MB_Bond.LPRValue = Convert.ToString(MIPD.premium_liability_value);
                MB_Bond.LPRGovtRebate = Convert.ToString(MIPD.liability_gov_discount);
                MB_Bond.LPRGovtRebateValue = ctxtlgrlprValue;
                //MB_Bond.txtlgrlprValue = ctxtlgrlprValue;
                decimal txtsubtotlprValue = rLPRvalue - rtxtlgrlprValue;
                int rtxtsubtotlprValue = Convert.ToInt32(txtsubtotlprValue);
                string ctxtsubtotlprValue = String.Format("{0:0,0.00}", rtxtsubtotlprValue);
                MB_Bond.LPRSubTotal1 = ctxtsubtotlprValue;
                //MB_Bond.txtsubtotlprValue = ctxtsubtotlprValue;
                decimal txtcngamntrValue = Convert.ToDecimal(MIPD.value_of_cng_lpg_amount);
                int rtxtcngamntrValue = Convert.ToInt32(txtcngamntrValue);
                int txtlpgkitlprValue = 0;
                if (rtxtcngamntrValue != 0)
                {
                    txtlpgkitlprValue = 60;
                    MB_Bond.LPRLPGKitValue = String.Format("{0:0,0.00}", txtlpgkitlprValue);
                }
                int rtxtsubtotlpglprValue = rtxtsubtotlprValue + txtlpgkitlprValue;
                string crtxtsubtotlpglprValue = String.Format("{0:0,0.00}", rtxtsubtotlpglprValue);
                MB_Bond.LPRSubTotal2 = crtxtsubtotlpglprValue;
                decimal DriverAmount = Convert.ToDecimal(MIPD.driver_amount);
                string cDriverAmount = String.Format("{0:0,0.00}", DriverAmount);
                MB_Bond.DriverRisk = cDriverAmount;
                //MB_Bond.res10 = cDriverAmount;
                decimal PassengerAmount = 0;
                string cPassengerAmount = string.Empty;
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    PassengerAmount = Convert.ToDecimal(MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else if (MIPD.vehiclecategory == 17)
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver - 1) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    MB_Bond.PillionRisk = cPassengerAmount;
                }
                else
                {
                    MB_Bond.PassengersRisk = cPassengerAmount;
                }
                //MB_Bond.PassengersRisk = cPassengerAmount;
                //MB_Bond.res11 = cPassengerAmount;
                decimal txtlprtotValue = (rtxtsubtotlpglprValue + DriverAmount + PassengerAmount);
                int rtxtlprtotValue = Convert.ToInt32(txtlprtotValue);
                string ctxtlprtotValue = String.Format("{0:0,0.00}", rtxtlprtotValue);
                MB_Bond.LPRTotal = ctxtlprtotValue;
                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = rodafterSDdiscount + rtxtlprtotValue;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = ctxtodtotValue;
                MB_Bond.TotalB = ctxtlprtotValue;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            else if (MIPD.type_of_cover == 4)
            {
                //A. Own Damage
                MB_Bond.BP = Convert.ToString(MIPD.mia_own_damage_additional_value);
                if (MB_Bond.BP == "0")
                {
                    MB_Bond.BP = "";
                }
                MB_Bond.ODPercentage = Convert.ToString(MIPD.own_damage_value);
                decimal odvalue = Convert.ToDecimal(MIPD.own_damage_value);
                decimal res = ((total_amount / 100) * (odvalue));
                int rres = Convert.ToInt32(res);
                decimal odsubtot = Convert.ToDecimal(MIPD.mia_own_damage_additional_value) + rres;
                int rodsubtot = Convert.ToInt32(odsubtot);
                string cres = String.Format("{0:0,0.00}", rodsubtot);
                MB_Bond.ODPercentageValue = String.Format("{0:0,0.00}", rres);
                MB_Bond.ODSubTotal1 = cres;
                MB_Bond.ODPremium = cres;
                MB_Bond.GVWExtraAmount = cGrossVehicleWeight;
                //MB_Bond.txtbpidvValue = cres;
                decimal res1 = ((odsubtot / 100) * Convert.ToDecimal(MIPD.od_gov_discount));
                int rres1 = Convert.ToInt32(res1);
                string cres1 = String.Format("{0:0,0.00}", rres1);
                MB_Bond.ODGovtRebate = Convert.ToString(MIPD.od_gov_discount);
                MB_Bond.ODGovtRebateValue = cres1;
                //MB_Bond.txtlgrodValue = cres1;
                decimal txtrebatetotodvalue = rodsubtot - rres1 + rGrossVehicleWeight;
                int rtxtrebatetotodvalue = Convert.ToInt32(txtrebatetotodvalue);
                string ctxtrebatetotodvalue = String.Format("{0:0,0.00}", rtxtrebatetotodvalue);
                MB_Bond.ODSubTotal2 = ctxtrebatetotodvalue;
                decimal Addele = ((Convert.ToDecimal(MIPD.electrical_accessories_amount) / 100) * 4);
                decimal Addlpg = ((Convert.ToDecimal(MIPD.value_of_cng_lpg_amount) / 100) * 4);
                decimal FiberGlassValue = 0;
                if (MIPD.isfiberglassfitted == true)
                {
                    if (MIPD.vehiclecategory == 16)
                    {
                        FiberGlassValue = 100;
                    }
                    else
                    {
                        FiberGlassValue = 50;
                    }
                    MB_Bond.FibreGlassFuelTank = String.Format("{0:0,0.00}", FiberGlassValue); ;
                }
                decimal ODDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    ODDrvingInstitutionValue = ((Convert.ToDecimal(txtrebatetotodvalue) / 100) * 60);
                    MB_Bond.ODDrivingInstitutionPercentage = "60";
                    MB_Bond.ODDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", ODDrvingInstitutionValue); ;
                }
                decimal Addnonele = ((Convert.ToDecimal(MIPD.non_electrical_accessories_amount) / 100) * 4);
                decimal ODAddSubtot = txtrebatetotodvalue + Addele + Addlpg + FiberGlassValue + ODDrvingInstitutionValue + Addnonele;
                int rODAddSubtot = Convert.ToInt32(ODAddSubtot);
                MB_Bond.EAPercentage = "4";
                MB_Bond.EAPercentageValue = String.Format("{0:0,0.00}", Addele);
                MB_Bond.ODLPGKitPercentage = "4";
                MB_Bond.ODLPGKitValue = String.Format("{0:0,0.00}", Addlpg);
                MB_Bond.ODAddOtherPercentage = "4";
                MB_Bond.ODAddOtherPercentageValue = String.Format("{0:0,0.00}", Addnonele);
                MB_Bond.ODSubTotal3 = String.Format("{0:0,0.00}", rODAddSubtot);
                decimal AutoMobileValue = 0;
                if (MIPD.vehicletype == 1)
                {
                    if (MIPD.isautomobileassociation == true)
                    {
                        AutoMobileValue = ((rODAddSubtot / 100) * 5);
                        MB_Bond.AutomobilePercentage = "5";
                        MB_Bond.AutomobileValue = String.Format("{0:0,0.00}", AutoMobileValue); ;
                    }
                }
                decimal ODLessSubtot = ODAddSubtot - AutoMobileValue;
                int rODLessSubtot = Convert.ToInt32(ODLessSubtot);
                MB_Bond.ODSubTotal4 = String.Format("{0:0,0.00}", rODLessSubtot);

                //MB_Bond.ODMalusValue=
                MB_Bond.ODMalus = MIPD.ph_malus_value.ToString();

                decimal txtlessmalusValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtlessmalusValue = Convert.ToInt32(txtlessmalusValue);
                string ctxtlessmalusValue = String.Format("{0:0,0.00}", rtxtlessmalusValue);
                MB_Bond.ODMalusValue = ctxtlessmalusValue;

                MB_Bond.ODNCB = MIPD.ph_ncb_value.ToString();

                //MB_Bond.txtrebatetotodvalue = ctxtrebatetotodvalue;
                decimal txtlessncbValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_ncb_value));
                int rtxtlessncbValue = Convert.ToInt32(txtlessncbValue);
                string ctxtlessncbValue = String.Format("{0:0,0.00}", rtxtlessncbValue);
                MB_Bond.ODNCBValue = ctxtlessncbValue;

                //MB_Bond.txtrebatetotodvalue = ctxtlessncbValue;
                decimal txtodtotValue = (rODLessSubtot + rtxtlessmalusValue) - rtxtlessncbValue;
                int rtxtodtotValue = Convert.ToInt32(txtodtotValue);
                decimal sidecardiscount = 0;
                if (Convert.ToDecimal(MIPD.side_car_trailer_amount) > 1)
                {
                    sidecardiscount = ((rtxtodtotValue / 100) * 25);
                }
                decimal odafterSDdiscount = rtxtodtotValue - sidecardiscount;
                int rodafterSDdiscount = Convert.ToInt32(odafterSDdiscount);
                string ctxtodtotValue = String.Format("{0:0,0.00}", rodafterSDdiscount);
                MB_Bond.ODOthersPercentage = "25";
                MB_Bond.ODOthersValue = String.Format("{0:0,0.00}", sidecardiscount);
                MB_Bond.ODTotal = ctxtodtotValue;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                MB_Bond.LPRTotal = string.Empty;

                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = odafterSDdiscount + 0;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = ctxtodtotValue;
                MB_Bond.TotalB = string.Empty;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            return MB_Bond;
        }
        public VM_MB_Bond_Details GetInsuranceCalculationDetailsRenewal(VM_MotorInsurancePolicyPrintDetails MIPD)
        {
            VM_MB_Bond_Details MB_Bond = new VM_MB_Bond_Details();
            //Policy Details
            MB_Bond.PolicyPremiumAmount = MIPD.premium;
            //MB_Bond.PolicyNumber = MIPD.policy_number;
            //MB_Bond.From_DateTime = MIPD.from_date;
            // DateTime FD = MIPD.from_date.Value;
            //MB_Bond.From_Date = FD.ToString("dd-MM-yyyy");
            //MB_Bond.To_DateTime = MIPD.to_date;
            //DateTime TD = MIPD.to_date.Value;
            //MB_Bond.To_Date = TD.ToString("dd-MM-yyyy");
            //MB_Bond.Sanction_DateTime = MIPD.sanction_date;
            //DateTime SD = MIPD.sanction_date.Value;
            //MB_Bond.Sanction_Date = SD.ToString("dd-MM-yyyy");
            //
            //Proposer Details
            MB_Bond.ProposerNameAddress = MIPD.nameandaddress;
            MB_Bond.PolicyType = MIPD.type_of_cover == 1 ? "Liability Only Policy"
         : MIPD.type_of_cover == 2 ? "Package Policy"
         : MIPD.type_of_cover == 3 ? "Bundle Policy"
         : MIPD.type_of_cover == 4 ? "O"
         : "";


            //Vehicle Details
            MB_Bond.Application_ref_no = MIPD.application_ref_no;
            MB_Bond.Proposer_id = MIPD.proposer_id;
            MB_Bond.Regisration_no = MIPD.regisration_no;
            MB_Bond.Registration_authority_and_location = MIPD.registration_authority_and_location;
            MB_Bond.Year_of_manufacturer = MIPD.year_of_manufacturer;
            MB_Bond.Chasis_no = MIPD.chasis_no;
            MB_Bond.Engine_no = MIPD.engine_no;
            MB_Bond.SeatingCapacity = MIPD.seating_capacity_including_driver;
            MB_Bond.Cubic_capacity = MIPD.cubic_capacity;
            MB_Bond.GVW = Convert.ToString(MIPD.vehicle_weight);
            MB_Bond.Zone = MIPD.zone;
            MB_Bond.VehicleCategory = MIPD.vehiclecategory;
            MB_Bond.Make_of_vehicle = MIPD.vehicleClass;
            MB_Bond.Endorsements = MIPD.Endorsements;
            //MB_Bond.vehicle_class = MIPD.vehicleClass;
            //IDV Details
            MB_Bond.Insured_declared_value_amount = MIPD.insured_declared_value_amount;
            MB_Bond.Side_car_trailer_amount = MIPD.side_car_trailer_amount;
            MB_Bond.Non_electrical_accessories_amount = MIPD.non_electrical_accessories_amount;
            MB_Bond.Electrical_accessories_amount = MIPD.electrical_accessories_amount;
            MB_Bond.Value_of_cng_lpg_amount = MIPD.value_of_cng_lpg_amount;
            MB_Bond.Depreciation = Convert.ToString(MIPD.depreciation_value);
            decimal total_amount = Convert.ToDecimal(MIPD.total_amount);
            string ctotal_amount = String.Format("{0:0,0.00}", total_amount);
            MB_Bond.Idv_total_amount = ctotal_amount;
            decimal GrossVehicleWeight = 0;
            if (MIPD.vehiclecategory == 5 || MIPD.vehiclecategory == 6)
            {
                decimal weight = MIPD.vehicle_weight;
                decimal actualweight = 12000;
                if (weight > actualweight)
                {
                    decimal weightdiff = (weight - actualweight) / 100;
                    GrossVehicleWeight = weightdiff * 27;
                }
            }
            int rGrossVehicleWeight = Convert.ToInt32(GrossVehicleWeight);
            string cGrossVehicleWeight = String.Format("{0:0,0.00}", rGrossVehicleWeight);
            if (MIPD.type_of_cover == 2)
            {
                //A. Own Damage
                MB_Bond.BP = Convert.ToString(MIPD.mia_own_damage_additional_value);
                if (MB_Bond.BP == "0")
                {
                    MB_Bond.BP = "";
                }
                MB_Bond.ODPercentage = Convert.ToString(MIPD.own_damage_value);
                decimal odvalue = Convert.ToDecimal(MIPD.own_damage_value);
                decimal res = ((total_amount / 100) * (odvalue));
                int rres = Convert.ToInt32(res);
                decimal odsubtot = Convert.ToDecimal(MIPD.mia_own_damage_additional_value) + rres;
                int rodsubtot = Convert.ToInt32(odsubtot);
                string cres = String.Format("{0:0,0.00}", rodsubtot);
                MB_Bond.ODPercentageValue = String.Format("{0:0,0.00}", rres);
                MB_Bond.ODSubTotal1 = cres;
                MB_Bond.ODPremium = cGrossVehicleWeight;
                //MB_Bond.txtbpidvValue = cres;
                decimal res1 = ((odsubtot / 100) * Convert.ToDecimal(MIPD.od_gov_discount));
                int rres1 = Convert.ToInt32(res1);
                string cres1 = String.Format("{0:0,0.00}", rres1);
                MB_Bond.ODGovtRebate = Convert.ToString(MIPD.od_gov_discount);
                MB_Bond.ODGovtRebateValue = cres1;
                //MB_Bond.txtlgrodValue = cres1;
                decimal txtrebatetotodvalue = rodsubtot - rres1 + rGrossVehicleWeight;
                int rtxtrebatetotodvalue = Convert.ToInt32(txtrebatetotodvalue);
                string ctxtrebatetotodvalue = String.Format("{0:0,0.00}", rtxtrebatetotodvalue);
                MB_Bond.ODSubTotal2 = ctxtrebatetotodvalue;
                decimal Addele = ((Convert.ToDecimal(MIPD.electrical_accessories_amount) / 100) * 4);
                decimal Addlpg = ((Convert.ToDecimal(MIPD.value_of_cng_lpg_amount) / 100) * 4);
                decimal FiberGlassValue = 0;
                if (MIPD.isfiberglassfitted == true)
                {
                    if (MIPD.vehiclecategory == 16)
                    {
                        FiberGlassValue = 100;
                    }
                    else
                    {
                        FiberGlassValue = 50;
                    }
                    MB_Bond.FibreGlassFuelTank = String.Format("{0:0,0.00}", FiberGlassValue); ;
                }
                decimal ODDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    ODDrvingInstitutionValue = ((Convert.ToDecimal(txtrebatetotodvalue) / 100) * 60);
                    MB_Bond.ODDrivingInstitutionPercentage = "60";
                    MB_Bond.ODDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", ODDrvingInstitutionValue); ;
                }
                decimal Addnonele = ((Convert.ToDecimal(MIPD.non_electrical_accessories_amount) / 100) * 4);
                decimal ODAddSubtot = txtrebatetotodvalue + Addele + Addlpg + FiberGlassValue + ODDrvingInstitutionValue + Addnonele;
                int rODAddSubtot = Convert.ToInt32(ODAddSubtot);
                MB_Bond.EAPercentage = "4";
                MB_Bond.EAPercentageValue = String.Format("{0:0,0.00}", Addele);
                MB_Bond.ODLPGKitPercentage = "4";
                MB_Bond.ODLPGKitValue = String.Format("{0:0,0.00}", Addlpg);
                MB_Bond.ODAddOtherPercentage = "4";
                MB_Bond.ODAddOtherPercentageValue = String.Format("{0:0,0.00}", Addnonele);
                MB_Bond.ODSubTotal3 = String.Format("{0:0,0.00}", rODAddSubtot);
                decimal AutoMobileValue = 0;
                if (MIPD.vehicletype == 1)
                {
                    if (MIPD.isautomobileassociation == true)
                    {
                        AutoMobileValue = ((rODAddSubtot / 100) * 5);
                        MB_Bond.AutomobilePercentage = "5";
                        MB_Bond.AutomobileValue = String.Format("{0:0,0.00}", AutoMobileValue); ;
                    }
                }
                decimal ODLessSubtot = ODAddSubtot - AutoMobileValue;
                int rODLessSubtot = Convert.ToInt32(ODLessSubtot);
                MB_Bond.ODSubTotal4 = String.Format("{0:0,0.00}", rODLessSubtot);

                //MB_Bond.ODMalusValue=
                MB_Bond.ODMalus = MIPD.ph_malus_value.ToString();

                decimal txtlessmalusValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtlessmalusValue = Convert.ToInt32(txtlessmalusValue);
                string ctxtlessmalusValue = String.Format("{0:0,0.00}", rtxtlessmalusValue);
                MB_Bond.ODMalusValue = ctxtlessmalusValue;

                MB_Bond.ODNCB = MIPD.ph_ncb_value.ToString();

                //MB_Bond.txtrebatetotodvalue = ctxtrebatetotodvalue;
                decimal txtlessncbValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_ncb_value));
                int rtxtlessncbValue = Convert.ToInt32(txtlessncbValue);
                string ctxtlessncbValue = String.Format("{0:0,0.00}", rtxtlessncbValue);
                MB_Bond.ODNCBValue = ctxtlessncbValue;

                //MB_Bond.txtrebatetotodvalue = ctxtlessncbValue;
                decimal txtodtotValue = (rODLessSubtot + rtxtlessmalusValue) - rtxtlessncbValue;
                int rtxtodtotValue = Convert.ToInt32(txtodtotValue);
                decimal sidecardiscount = 0;
                if (Convert.ToDecimal(MIPD.side_car_trailer_amount) > 1)
                {
                    sidecardiscount = ((rtxtodtotValue / 100) * 25);
                }
                decimal odafterSDdiscount = rtxtodtotValue - sidecardiscount;
                int rodafterSDdiscount = Convert.ToInt32(odafterSDdiscount);
                string ctxtodtotValue = String.Format("{0:0,0.00}", rodafterSDdiscount);
                MB_Bond.ODOthersPercentage = "25";
                MB_Bond.ODOthersValue = String.Format("{0:0,0.00}", sidecardiscount);
                MB_Bond.ODTotal = ctxtodtotValue;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                decimal LPRvalue = Convert.ToDecimal(MIPD.premium_liability_value);
                int rLPRvalue = Convert.ToInt32(LPRvalue);
                decimal txtlgrlprValue = ((LPRvalue / 100) * Convert.ToDecimal(MIPD.liability_gov_discount));
                int rtxtlgrlprValue = Convert.ToInt32(txtlgrlprValue);
                string ctxtlgrlprValue = String.Format("{0:0,0.00}", rtxtlgrlprValue);
                MB_Bond.LPRValue = Convert.ToString(MIPD.premium_liability_value);
                MB_Bond.LPRGovtRebate = Convert.ToString(MIPD.liability_gov_discount);
                MB_Bond.LPRGovtRebateValue = ctxtlgrlprValue;
                //MB_Bond.txtlgrlprValue = ctxtlgrlprValue;
                decimal txtsubtotlprValue = rLPRvalue - rtxtlgrlprValue;
                int rtxtsubtotlprValue = Convert.ToInt32(txtsubtotlprValue);
                string ctxtsubtotlprValue = String.Format("{0:0,0.00}", rtxtsubtotlprValue);
                MB_Bond.LPRSubTotal1 = ctxtsubtotlprValue;
                //MB_Bond.txtsubtotlprValue = ctxtsubtotlprValue;
                decimal txtcngamntrValue = Convert.ToDecimal(MIPD.value_of_cng_lpg_amount);
                int rtxtcngamntrValue = Convert.ToInt32(txtcngamntrValue);
                int txtlpgkitlprValue = 0;
                if (rtxtcngamntrValue != 0)
                {
                    txtlpgkitlprValue = 60;
                    MB_Bond.LPRLPGKitValue = String.Format("{0:0,0.00}", txtlpgkitlprValue);
                }
                int rtxtsubtotlpglprValue = rtxtsubtotlprValue + txtlpgkitlprValue;
                string crtxtsubtotlpglprValue = String.Format("{0:0,0.00}", rtxtsubtotlpglprValue);
                MB_Bond.LPRSubTotal2 = crtxtsubtotlpglprValue;
                decimal DriverAmount = Convert.ToDecimal(MIPD.driver_amount);
                string cDriverAmount = String.Format("{0:0,0.00}", DriverAmount);
                MB_Bond.DriverRisk = cDriverAmount;
                //MB_Bond.res10 = cDriverAmount;
                decimal PassengerAmount = 0;
                string cPassengerAmount = string.Empty;
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    PassengerAmount = Convert.ToDecimal(MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else if (MIPD.vehiclecategory == 17)
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver - 1) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    MB_Bond.PillionRisk = cPassengerAmount;
                }
                else
                {
                    MB_Bond.PassengersRisk = cPassengerAmount;
                }
                //MB_Bond.PassengersRisk = cPassengerAmount;
                //MB_Bond.res11 = cPassengerAmount;
                decimal txtlprtotValue = (rtxtsubtotlpglprValue + DriverAmount + PassengerAmount);
                int rtxtlprtotValue = Convert.ToInt32(txtlprtotValue);
                string ctxtlprtotValue = String.Format("{0:0,0.00}", rtxtlprtotValue);
                MB_Bond.LPRTotal = ctxtlprtotValue;
                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = rodafterSDdiscount + rtxtlprtotValue;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = ctxtodtotValue;
                MB_Bond.TotalB = ctxtlprtotValue;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            else if (MIPD.type_of_cover == 1)
            {
                //A. Own Damage
                MB_Bond.ODTotal = string.Empty;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                decimal LPRvalue = Convert.ToDecimal(MIPD.premium_liability_value);
                int rLPRvalue = Convert.ToInt32(LPRvalue);
                decimal txtlgrlprValue = Math.Round(((LPRvalue / 100) * Convert.ToDecimal(MIPD.liability_gov_discount)));
                int rtxtlgrlprValue = Convert.ToInt32(txtlgrlprValue);
                string ctxtlgrlprValue = String.Format("{0:0,0.00}", rtxtlgrlprValue);
                MB_Bond.LPRValue = Convert.ToString(MIPD.premium_liability_value);
                MB_Bond.LPRGovtRebate = Convert.ToString(MIPD.liability_gov_discount);
                MB_Bond.LPRGovtRebateValue = ctxtlgrlprValue;
                //MB_Bond.txtlgrlprValue = ctxtlgrlprValue;
                decimal txtsubtotlprValue = Math.Round(LPRvalue - txtlgrlprValue);
                int rtxtsubtotlprValue = Convert.ToInt32(txtsubtotlprValue);
                string ctxtsubtotlprValue = String.Format("{0:0,0.00}", rtxtsubtotlprValue);
                MB_Bond.LPRSubTotal1 = ctxtsubtotlprValue;
                //MB_Bond.txtsubtotlprValue = ctxtsubtotlprValue;
                decimal LPRDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    LPRDrvingInstitutionValue = ((Convert.ToDecimal(rtxtsubtotlprValue) / 100) * 60);
                    MB_Bond.LPRDrivingInstitutionPercentage = "60";
                    MB_Bond.LPRDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", LPRDrvingInstitutionValue); ;
                }
                decimal txtcngamntrValue = Convert.ToDecimal(MIPD.value_of_cng_lpg_amount);
                int rtxtcngamntrValue = Convert.ToInt32(txtcngamntrValue);
                int txtlpgkitlprValue = 0;
                if (rtxtcngamntrValue != 0)
                {
                    txtlpgkitlprValue = 60;
                    MB_Bond.LPRLPGKitValue = String.Format("{0:0,0.00}", txtlpgkitlprValue);
                }
                int rtxtsubtotlpglprValue = rtxtsubtotlprValue + txtlpgkitlprValue + Convert.ToInt32(LPRDrvingInstitutionValue);
                string crtxtsubtotlpglprValue = String.Format("{0:0,0.00}", rtxtsubtotlpglprValue);
                MB_Bond.LPRSubTotal2 = crtxtsubtotlpglprValue;
                decimal DriverAmount = Convert.ToDecimal(MIPD.driver_amount);
                string cDriverAmount = String.Format("{0:0,0.00}", DriverAmount);
                MB_Bond.DriverRisk = cDriverAmount;
                //MB_Bond.res10 = cDriverAmount;
                decimal PassengerAmount = 0;
                string cPassengerAmount = string.Empty;
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    PassengerAmount = Convert.ToDecimal(MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else if (MIPD.vehiclecategory == 17)
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver - 1) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    MB_Bond.PillionRisk = cPassengerAmount;
                }
                else
                {
                    MB_Bond.PassengersRisk = cPassengerAmount;
                }
                //MB_Bond.PassengersRisk = cPassengerAmount;
                //MB_Bond.res11 = cPassengerAmount;

                MB_Bond.LPRMalus = MIPD.ph_malus_value.ToString();

                decimal txtaddmalusValue = ((rtxtsubtotlpglprValue / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtaddmalusValue = Convert.ToInt32(txtaddmalusValue);
                string ctxtaddmalusValue = String.Format("{0:0,0.00}", rtxtaddmalusValue);
                MB_Bond.LPRMalusValue = ctxtaddmalusValue;

                decimal txtlprtotValue = (rtxtsubtotlpglprValue + DriverAmount + PassengerAmount + rtxtaddmalusValue);
                int rtxtlprtotValue = Convert.ToInt32(txtlprtotValue);
                string ctxtlprtotValue = String.Format("{0:0,0.00}", rtxtlprtotValue);
                MB_Bond.LPRTotal = ctxtlprtotValue;
                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = 0 + rtxtlprtotValue;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = string.Empty;
                MB_Bond.TotalB = ctxtlprtotValue;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            else if (MIPD.type_of_cover == 3)
            {
                //A. Own Damage
                MB_Bond.BP = Convert.ToString(MIPD.mia_own_damage_additional_value);
                if (MB_Bond.BP == "0")
                {
                    MB_Bond.BP = "";
                }
                MB_Bond.ODPercentage = Convert.ToString(MIPD.own_damage_value);
                decimal odvalue = Convert.ToDecimal(MIPD.own_damage_value);
                decimal res = ((total_amount / 100) * (odvalue));
                int rres = Convert.ToInt32(res);
                decimal odsubtot = Convert.ToDecimal(MIPD.mia_own_damage_additional_value) + rres;
                int rodsubtot = Convert.ToInt32(odsubtot);
                string cres = String.Format("{0:0,0.00}", rodsubtot);
                MB_Bond.ODPercentageValue = String.Format("{0:0,0.00}", rres);
                MB_Bond.ODSubTotal1 = cres;
                MB_Bond.ODPremium = cGrossVehicleWeight;
                //MB_Bond.txtbpidvValue = cres;
                decimal res1 = ((odsubtot / 100) * Convert.ToDecimal(MIPD.od_gov_discount));
                int rres1 = Convert.ToInt32(res1);
                string cres1 = String.Format("{0:0,0.00}", rres1);
                MB_Bond.ODGovtRebate = Convert.ToString(MIPD.od_gov_discount);
                MB_Bond.ODGovtRebateValue = cres1;
                //MB_Bond.txtlgrodValue = cres1;
                decimal txtrebatetotodvalue = rodsubtot - rres1 + rGrossVehicleWeight;
                int rtxtrebatetotodvalue = Convert.ToInt32(txtrebatetotodvalue);
                string ctxtrebatetotodvalue = String.Format("{0:0,0.00}", rtxtrebatetotodvalue);
                MB_Bond.ODSubTotal2 = ctxtrebatetotodvalue;
                decimal Addele = ((Convert.ToDecimal(MIPD.electrical_accessories_amount) / 100) * 4);
                decimal Addlpg = ((Convert.ToDecimal(MIPD.value_of_cng_lpg_amount) / 100) * 4);
                decimal FiberGlassValue = 0;
                if (MIPD.isfiberglassfitted == true)
                {
                    if (MIPD.vehiclecategory == 16)
                    {
                        FiberGlassValue = 100;
                    }
                    else
                    {
                        FiberGlassValue = 50;
                    }
                    MB_Bond.FibreGlassFuelTank = String.Format("{0:0,0.00}", FiberGlassValue); ;
                }
                decimal ODDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    ODDrvingInstitutionValue = ((Convert.ToDecimal(txtrebatetotodvalue) / 100) * 60);
                    MB_Bond.ODDrivingInstitutionPercentage = "60";
                    MB_Bond.ODDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", ODDrvingInstitutionValue); ;
                }
                decimal Addnonele = ((Convert.ToDecimal(MIPD.non_electrical_accessories_amount) / 100) * 4);
                decimal ODAddSubtot = txtrebatetotodvalue + Addele + Addlpg + FiberGlassValue + ODDrvingInstitutionValue + Addnonele;
                int rODAddSubtot = Convert.ToInt32(ODAddSubtot);
                MB_Bond.EAPercentage = "4";
                MB_Bond.EAPercentageValue = String.Format("{0:0,0.00}", Addele);
                MB_Bond.ODLPGKitPercentage = "4";
                MB_Bond.ODLPGKitValue = String.Format("{0:0,0.00}", Addlpg);
                MB_Bond.ODAddOtherPercentage = "4";
                MB_Bond.ODAddOtherPercentageValue = String.Format("{0:0,0.00}", Addnonele);
                MB_Bond.ODSubTotal3 = String.Format("{0:0,0.00}", rODAddSubtot);
                decimal AutoMobileValue = 0;
                if (MIPD.vehicletype == 1)
                {
                    if (MIPD.isautomobileassociation == true)
                    {
                        AutoMobileValue = ((rODAddSubtot / 100) * 5);
                        MB_Bond.AutomobilePercentage = "5";
                        MB_Bond.AutomobileValue = String.Format("{0:0,0.00}", AutoMobileValue); ;
                    }
                }
                decimal ODLessSubtot = ODAddSubtot - AutoMobileValue;
                int rODLessSubtot = Convert.ToInt32(ODLessSubtot);
                MB_Bond.ODSubTotal4 = String.Format("{0:0,0.00}", rODLessSubtot);

                //MB_Bond.ODMalusValue=
                MB_Bond.ODMalus = MIPD.ph_malus_value.ToString();

                decimal txtlessmalusValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtlessmalusValue = Convert.ToInt32(txtlessmalusValue);
                string ctxtlessmalusValue = String.Format("{0:0,0.00}", rtxtlessmalusValue);
                MB_Bond.ODMalusValue = ctxtlessmalusValue;

                MB_Bond.ODNCB = MIPD.ph_ncb_value.ToString();

                //MB_Bond.txtrebatetotodvalue = ctxtrebatetotodvalue;
                decimal txtlessncbValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_ncb_value));
                int rtxtlessncbValue = Convert.ToInt32(txtlessncbValue);
                string ctxtlessncbValue = String.Format("{0:0,0.00}", rtxtlessncbValue);
                MB_Bond.ODNCBValue = ctxtlessncbValue;

                //MB_Bond.txtrebatetotodvalue = ctxtlessncbValue;
                decimal txtodtotValue = (rODLessSubtot + rtxtlessmalusValue) - rtxtlessncbValue;
                int rtxtodtotValue = Convert.ToInt32(txtodtotValue);
                decimal sidecardiscount = 0;
                if (Convert.ToDecimal(MIPD.side_car_trailer_amount) > 1)
                {
                    sidecardiscount = ((rtxtodtotValue / 100) * 25);
                }
                decimal odafterSDdiscount = rtxtodtotValue - sidecardiscount;
                int rodafterSDdiscount = Convert.ToInt32(odafterSDdiscount);
                string ctxtodtotValue = String.Format("{0:0,0.00}", rodafterSDdiscount);
                MB_Bond.ODOthersPercentage = "25";
                MB_Bond.ODOthersValue = String.Format("{0:0,0.00}", sidecardiscount);
                MB_Bond.ODTotal = ctxtodtotValue;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                decimal LPRvalue = Convert.ToDecimal(MIPD.premium_liability_value);
                int rLPRvalue = Convert.ToInt32(LPRvalue);
                decimal txtlgrlprValue = ((LPRvalue / 100) * Convert.ToDecimal(MIPD.liability_gov_discount));
                int rtxtlgrlprValue = Convert.ToInt32(txtlgrlprValue);
                string ctxtlgrlprValue = String.Format("{0:0,0.00}", rtxtlgrlprValue);
                MB_Bond.LPRValue = Convert.ToString(MIPD.premium_liability_value);
                MB_Bond.LPRGovtRebate = Convert.ToString(MIPD.liability_gov_discount);
                MB_Bond.LPRGovtRebateValue = ctxtlgrlprValue;
                //MB_Bond.txtlgrlprValue = ctxtlgrlprValue;
                decimal txtsubtotlprValue = rLPRvalue - rtxtlgrlprValue;
                int rtxtsubtotlprValue = Convert.ToInt32(txtsubtotlprValue);
                string ctxtsubtotlprValue = String.Format("{0:0,0.00}", rtxtsubtotlprValue);
                MB_Bond.LPRSubTotal1 = ctxtsubtotlprValue;
                //MB_Bond.txtsubtotlprValue = ctxtsubtotlprValue;
                decimal txtcngamntrValue = Convert.ToDecimal(MIPD.value_of_cng_lpg_amount);
                int rtxtcngamntrValue = Convert.ToInt32(txtcngamntrValue);
                int txtlpgkitlprValue = 0;
                if (rtxtcngamntrValue != 0)
                {
                    txtlpgkitlprValue = 60;
                    MB_Bond.LPRLPGKitValue = String.Format("{0:0,0.00}", txtlpgkitlprValue);
                }
                int rtxtsubtotlpglprValue = rtxtsubtotlprValue + txtlpgkitlprValue;
                string crtxtsubtotlpglprValue = String.Format("{0:0,0.00}", rtxtsubtotlpglprValue);
                MB_Bond.LPRSubTotal2 = crtxtsubtotlpglprValue;
                decimal DriverAmount = Convert.ToDecimal(MIPD.driver_amount);
                string cDriverAmount = String.Format("{0:0,0.00}", DriverAmount);
                MB_Bond.DriverRisk = cDriverAmount;
                //MB_Bond.res10 = cDriverAmount;
                decimal PassengerAmount = 0;
                string cPassengerAmount = string.Empty;
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    PassengerAmount = Convert.ToDecimal(MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else if (MIPD.vehiclecategory == 17)
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver - 1) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    MB_Bond.PillionRisk = cPassengerAmount;
                }
                else
                {
                    MB_Bond.PassengersRisk = cPassengerAmount;
                }
                //MB_Bond.PassengersRisk = cPassengerAmount;
                //MB_Bond.res11 = cPassengerAmount;
                decimal txtlprtotValue = (rtxtsubtotlpglprValue + DriverAmount + PassengerAmount);
                int rtxtlprtotValue = Convert.ToInt32(txtlprtotValue);
                string ctxtlprtotValue = String.Format("{0:0,0.00}", rtxtlprtotValue);
                MB_Bond.LPRTotal = ctxtlprtotValue;
                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = rodafterSDdiscount + rtxtlprtotValue;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = ctxtodtotValue;
                MB_Bond.TotalB = ctxtlprtotValue;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            else if (MIPD.type_of_cover == 4)
            {
                //A. Own Damage
                MB_Bond.BP = Convert.ToString(MIPD.mia_own_damage_additional_value);
                if (MB_Bond.BP == "0")
                {
                    MB_Bond.BP = "";
                }
                MB_Bond.ODPercentage = Convert.ToString(MIPD.own_damage_value);
                decimal odvalue = Convert.ToDecimal(MIPD.own_damage_value);
                decimal res = ((total_amount / 100) * (odvalue));
                int rres = Convert.ToInt32(res);
                decimal odsubtot = Convert.ToDecimal(MIPD.mia_own_damage_additional_value) + rres;
                int rodsubtot = Convert.ToInt32(odsubtot);
                string cres = String.Format("{0:0,0.00}", rodsubtot);
                MB_Bond.ODPercentageValue = String.Format("{0:0,0.00}", rres);
                MB_Bond.ODSubTotal1 = cres;
                MB_Bond.ODPremium = cGrossVehicleWeight;
                //MB_Bond.txtbpidvValue = cres;
                decimal res1 = ((odsubtot / 100) * Convert.ToDecimal(MIPD.od_gov_discount));
                int rres1 = Convert.ToInt32(res1);
                string cres1 = String.Format("{0:0,0.00}", rres1);
                MB_Bond.ODGovtRebate = Convert.ToString(MIPD.od_gov_discount);
                MB_Bond.ODGovtRebateValue = cres1;
                //MB_Bond.txtlgrodValue = cres1;
                decimal txtrebatetotodvalue = rodsubtot - rres1 + rGrossVehicleWeight;
                int rtxtrebatetotodvalue = Convert.ToInt32(txtrebatetotodvalue);
                string ctxtrebatetotodvalue = String.Format("{0:0,0.00}", rtxtrebatetotodvalue);
                MB_Bond.ODSubTotal2 = ctxtrebatetotodvalue;
                decimal Addele = ((Convert.ToDecimal(MIPD.electrical_accessories_amount) / 100) * 4);
                decimal Addlpg = ((Convert.ToDecimal(MIPD.value_of_cng_lpg_amount) / 100) * 4);
                decimal FiberGlassValue = 0;
                if (MIPD.isfiberglassfitted == true)
                {
                    if (MIPD.vehiclecategory == 16)
                    {
                        FiberGlassValue = 100;
                    }
                    else
                    {
                        FiberGlassValue = 50;
                    }
                    MB_Bond.FibreGlassFuelTank = String.Format("{0:0,0.00}", FiberGlassValue); ;
                }
                decimal ODDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    ODDrvingInstitutionValue = ((Convert.ToDecimal(txtrebatetotodvalue) / 100) * 60);
                    MB_Bond.ODDrivingInstitutionPercentage = "60";
                    MB_Bond.ODDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", ODDrvingInstitutionValue); ;
                }
                decimal Addnonele = ((Convert.ToDecimal(MIPD.non_electrical_accessories_amount) / 100) * 4);
                decimal ODAddSubtot = txtrebatetotodvalue + Addele + Addlpg + FiberGlassValue + ODDrvingInstitutionValue + Addnonele;
                int rODAddSubtot = Convert.ToInt32(ODAddSubtot);
                MB_Bond.EAPercentage = "4";
                MB_Bond.EAPercentageValue = String.Format("{0:0,0.00}", Addele);
                MB_Bond.ODLPGKitPercentage = "4";
                MB_Bond.ODLPGKitValue = String.Format("{0:0,0.00}", Addlpg);
                MB_Bond.ODAddOtherPercentage = "4";
                MB_Bond.ODAddOtherPercentageValue = String.Format("{0:0,0.00}", Addnonele);
                MB_Bond.ODSubTotal3 = String.Format("{0:0,0.00}", rODAddSubtot);
                decimal AutoMobileValue = 0;
                if (MIPD.vehicletype == 1)
                {
                    if (MIPD.isautomobileassociation == true)
                    {
                        AutoMobileValue = ((rODAddSubtot / 100) * 5);
                        MB_Bond.AutomobilePercentage = "5";
                        MB_Bond.AutomobileValue = String.Format("{0:0,0.00}", AutoMobileValue); ;
                    }
                }
                decimal ODLessSubtot = ODAddSubtot - AutoMobileValue;
                int rODLessSubtot = Convert.ToInt32(ODLessSubtot);
                MB_Bond.ODSubTotal4 = String.Format("{0:0,0.00}", rODLessSubtot);

                //MB_Bond.ODMalusValue=
                MB_Bond.ODMalus = MIPD.ph_malus_value.ToString();

                decimal txtlessmalusValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtlessmalusValue = Convert.ToInt32(txtlessmalusValue);
                string ctxtlessmalusValue = String.Format("{0:0,0.00}", rtxtlessmalusValue);
                MB_Bond.ODMalusValue = ctxtlessmalusValue;

                MB_Bond.ODNCB = MIPD.ph_ncb_value.ToString();

                //MB_Bond.txtrebatetotodvalue = ctxtrebatetotodvalue;
                decimal txtlessncbValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_ncb_value));
                int rtxtlessncbValue = Convert.ToInt32(txtlessncbValue);
                string ctxtlessncbValue = String.Format("{0:0,0.00}", rtxtlessncbValue);
                MB_Bond.ODNCBValue = ctxtlessncbValue;

                //MB_Bond.txtrebatetotodvalue = ctxtlessncbValue;
                decimal txtodtotValue = (rODLessSubtot + rtxtlessmalusValue) - rtxtlessncbValue;
                int rtxtodtotValue = Convert.ToInt32(txtodtotValue);
                decimal sidecardiscount = 0;
                if (Convert.ToDecimal(MIPD.side_car_trailer_amount) > 1)
                {
                    sidecardiscount = ((rtxtodtotValue / 100) * 25);
                }
                decimal odafterSDdiscount = rtxtodtotValue - sidecardiscount;
                int rodafterSDdiscount = Convert.ToInt32(odafterSDdiscount);
                string ctxtodtotValue = String.Format("{0:0,0.00}", rodafterSDdiscount);
                MB_Bond.ODOthersPercentage = "25";
                MB_Bond.ODOthersValue = String.Format("{0:0,0.00}", sidecardiscount);
                MB_Bond.ODTotal = ctxtodtotValue;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                MB_Bond.LPRTotal = string.Empty;

                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = odafterSDdiscount + 0;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = ctxtodtotValue;
                MB_Bond.TotalB = string.Empty;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            return MB_Bond;
        }


        #endregion
        #region Print MI Bond
        private string FillFormMB(VM_MB_Bond_Details _MBBond, string PolicyNumber)
        {
            string pdfTemplate = Server.MapPath("~/MIBond/Motor_Branch_Bond_Template.pdf");
            //string newFile = Server.MapPath("~/MIBond/" + PolicyNumber + "UnSigned" + ".pdf");
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"MIBond\" + PolicyNumber + "UnSigned" + ".pdf";
            }
            //PdfWriter.GetInstance(doc1, new FileStream(newFile, FileMode.Create));
            //doc1.Open();
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            {//Basic Details
                //fields.SetField("SectionCode", _MBBond.SectionCode == null ? "" : _MBBond.SectionCode.ToString());
                //fields.SetField("SectionCode", _MBBond.SectionCode.ToString());
                fields.SetField("CertificateNo", _MBBond.Application_ref_no == 0 ? "" : _MBBond.Application_ref_no.ToString());
                //fields.SetField("CertificateNo", _MBBond.CertificateNo.ToString());
                fields.SetField("PolicyNo", _MBBond.PolicyNumber == null ? "" : _MBBond.PolicyNumber.ToString());
                //fields.SetField("PolicyNo", _MBBond.PolicyNumber.ToString());
                fields.SetField("PolicyType", _MBBond.PolicyType == null ? "" : _MBBond.PolicyType.ToString());
                //fields.SetField("PolicyType", _MBBond.PolicyType.ToString());
                fields.SetField("ChallanNo", _MBBond.ChallanNo == null ? "" : _MBBond.ChallanNo.ToString());
                //fields.SetField("ChallanNo", _MBBond.ChallanNo.ToString());
                fields.SetField("Date", _MBBond.Sanction_Date == null ? "" : _MBBond.Sanction_Date.ToString());
                //fields.SetField("Date", _MBBond.Sanction_Date.ToString());
                fields.SetField("ProposerNameAddress", _MBBond.ProposerNameAddress == null ? "" : _MBBond.ProposerNameAddress.ToString());
                //fields.SetField("ProposerNameAddress", _MBBond.ProposerNameAddress.ToString());
                fields.SetField("PremiumPaid", _MBBond.PolicyPremiumAmount == null ? "" : _MBBond.PolicyPremiumAmount.ToString());
                //fields.SetField("PremiumPaid", _MBBond.PolicyPremiumAmount.ToString());
                fields.SetField("ODRiskFromDate", _MBBond.ODFrom_Date == null ? "" : _MBBond.ODFrom_Date.ToString());
                //fields.SetField("RiskFromDate", _MBBond.From_Date.ToString());
                fields.SetField("ODRiskToDate", _MBBond.ODTo_Date == null ? "" : _MBBond.ODTo_Date.ToString());
                //fields.SetField("RiskToDate", _MBBond.To_Date.ToString());
                fields.SetField("TPRiskFromDate", _MBBond.TPFrom_Date == null ? "" : _MBBond.TPFrom_Date.ToString());
                //fields.SetField("RiskFromDate", _MBBond.From_Date.ToString());
                fields.SetField("TPRiskToDate", _MBBond.TPTo_Date == null ? "" : _MBBond.TPTo_Date.ToString());
                //fields.SetField("RiskToDate", _MBBond.To_Date.ToString());
                fields.SetField("RegistrationNo", _MBBond.Regisration_no == null ? "" : _MBBond.Regisration_no.ToString());
                //fields.SetField("RegistrationNo", _MBBond.Regisration_no.ToString());
                fields.SetField("Make", _MBBond.Make_of_vehicle == null ? "" : _MBBond.Make_of_vehicle.ToString());
                fields.SetField("YearofManufacture", _MBBond.Year_of_manufacturer.ToString());
                fields.SetField("CC", _MBBond.Cubic_capacity.ToString());
                fields.SetField("GVW", _MBBond.GVW == null ? "" : _MBBond.GVW.ToString());
                //fields.SetField("GVW", _MBBond.GVW.ToString());
                fields.SetField("SeatingCapacity", _MBBond.SeatingCapacity == null ? "" : _MBBond.SeatingCapacity.ToString());
                //fields.SetField("SeatingCapacity", _MBBond.SeatingCapacity.ToString());
                fields.SetField("EngineNo", _MBBond.Engine_no.ToString());
                fields.SetField("ChassisNo", _MBBond.Chasis_no.ToString());
                fields.SetField("Zone", _MBBond.Zone == null ? "" : _MBBond.Zone.ToString());
                //fields.SetField("Zone", _MBBond.Zone.ToString());
                fields.SetField("InsuredDecalreValue", _MBBond.Insured_declared_value_amount == null ? "" : _MBBond.Insured_declared_value_amount.ToString());
                //fields.SetField("InsuredDecalreValue", _MBBond.Insured_declared_value_amount.ToString());
                fields.SetField("SideCarTrailer", _MBBond.Side_car_trailer_amount == null ? "" : _MBBond.Side_car_trailer_amount.ToString());
                //fields.SetField("SideCar", _MBBond.Side_car_trailer_amount.ToString());
                //fields.SetField("Trailer", _MBBond.Side_car_trailer_amount == null ? "" : _MBBond.Side_car_trailer_amount.ToString());
                //fields.SetField("Trailer", _MBBond.Side_car_trailer_amount.ToString());
                fields.SetField("NonElectrical", _MBBond.Non_electrical_accessories_amount == null ? "" : _MBBond.Non_electrical_accessories_amount.ToString());
                //fields.SetField("NonElectrical", _MBBond.Non_electrical_accessories_amount.ToString());
                fields.SetField("Electrical", _MBBond.Electrical_accessories_amount == null ? "" : _MBBond.Electrical_accessories_amount.ToString());
                //fields.SetField("Electrical", _MBBond.Electrical_accessories_amount.ToString());
                fields.SetField("CngLpgUnit", _MBBond.Value_of_cng_lpg_amount == null ? "" : _MBBond.Value_of_cng_lpg_amount.ToString());
                //fields.SetField("CNGUnit", _MBBond.Value_of_cng_lpg_amount.ToString());
                //fields.SetField("LPGUnit", _MBBond.Value_of_cng_lpg_amount == null ? "" : _MBBond.Value_of_cng_lpg_amount.ToString());
                //fields.SetField("LPGUnit", _MBBond.Value_of_cng_lpg_amount.ToString());
                fields.SetField("Depreciation", _MBBond.Depreciation == null ? "" : _MBBond.Depreciation.ToString());
                //fields.SetField("Depreciation", _MBBond.Depreciation.ToString());
                fields.SetField("TotalPVV", _MBBond.Idv_total_amount.ToString());
            }
            {//OD Details
                fields.SetField("BP", _MBBond.BP == null ? "" : _MBBond.BP.ToString());
                //fields.SetField("BP", _MBBond.BP.ToString());
                fields.SetField("ODPercentage", _MBBond.ODPercentage == null ? "" : _MBBond.ODPercentage.ToString());
                //fields.SetField("ODPercentage", _MBBond.ODPercentage.ToString());
                fields.SetField("ODPercentageValue", _MBBond.ODPercentageValue == null ? "" : _MBBond.ODPercentageValue.ToString());
                //fields.SetField("ODPercentageValue", _MBBond.ODPercentageValue.ToString());
                fields.SetField("ODSubTotal1", _MBBond.ODSubTotal1 == null ? "" : _MBBond.ODSubTotal1.ToString());
                //fields.SetField("ODSubTotal1", _MBBond.ODSubTotal1.ToString());
                fields.SetField("ODPremium", _MBBond.ODPremium == null ? "" : _MBBond.ODPremium.ToString());
                //fields.SetField("ODPremium", _MBBond.ODPremium.ToString());
                fields.SetField("GVWExtraAmount", _MBBond.GVWExtraAmount == null ? "" : _MBBond.GVWExtraAmount.ToString());
                //fields.SetField("GVWExtraAmount", _MBBond.GVWExtraAmount.ToString());
                fields.SetField("ODGovtRebate", _MBBond.ODGovtRebate == null ? "" : _MBBond.ODGovtRebate.ToString());
                //fields.SetField("ODGovtRebate", _MBBond.ODGovtRebate.ToString());
                fields.SetField("ODGovtRebateValue", _MBBond.ODGovtRebateValue == null ? "" : _MBBond.ODGovtRebateValue.ToString());
                //fields.SetField("ODGovtRebateValue", _MBBond.ODGovtRebateValue.ToString());
                fields.SetField("ODSubTotal2", _MBBond.ODSubTotal2 == null ? "" : _MBBond.ODSubTotal2.ToString());
                //fields.SetField("ODSubTotal2", _MBBond.ODSubTotal2.ToString());
                fields.SetField("EAPercentage", _MBBond.EAPercentage == null ? "" : _MBBond.EAPercentage.ToString());
                //fields.SetField("ElectricalAccessories", _MBBond.ElectricalAccessories.ToString());
                fields.SetField("EAPercentageValue", _MBBond.EAPercentageValue == null ? "" : _MBBond.EAPercentageValue.ToString());
                //fields.SetField("ElectricalAccessories", _MBBond.ElectricalAccessories.ToString());
                fields.SetField("ODLPGKitPercentage", _MBBond.ODLPGKitPercentage == null ? "" : _MBBond.ODLPGKitPercentage.ToString());
                //fields.SetField("LPGKitPercentage", _MBBond.LPGKitPercentage.ToString());
                fields.SetField("ODLPGKitValue", _MBBond.ODLPGKitValue == null ? "" : _MBBond.ODLPGKitValue.ToString());
                //fields.SetField("LPGKitValue", _MBBond.LPGKitValue.ToString());
                fields.SetField("ImportedPercentage", _MBBond.ImportedPercentage == null ? "" : _MBBond.ImportedPercentage.ToString());
                //fields.SetField("ImportedPercentage", _MBBond.ImportedPercentage.ToString());
                fields.SetField("ImportedVehiclesValue", _MBBond.ImportedVehiclesValue == null ? "" : _MBBond.ImportedVehiclesValue.ToString());
                //fields.SetField("ImportedVehiclesValue", _MBBond.ImportedVehiclesValue.ToString());
                fields.SetField("FibreGlassFuelTank", _MBBond.FibreGlassFuelTank == null ? "" : _MBBond.FibreGlassFuelTank.ToString());
                //fields.SetField("FibreGlassFuelTank", _MBBond.FibreGlassFuelTank.ToString());
                fields.SetField("ODDIPercentage", _MBBond.ODDrivingInstitutionPercentage == null ? "" : _MBBond.ODDrivingInstitutionPercentage.ToString());
                //fields.SetField("DrivingInstitutionPercentage", _MBBond.DrivingInstitutionPercentage.ToString());
                fields.SetField("ODDIPercentageValue", _MBBond.ODDrivingInstitutionPercentage == null ? "" : _MBBond.ODDrivingInstitutionPercentageValue.ToString());
                //fields.SetField("DrivingInstitutionPercentage", _MBBond.DrivingInstitutionPercentage.ToString());
                fields.SetField("AddODOtherPercentage", _MBBond.ODAddOtherPercentage == null ? "" : _MBBond.ODAddOtherPercentage.ToString());
                //fields.SetField("AddODOtherPercentage", _MBBond.ODAddOtherPercentage.ToString());
                fields.SetField("AddODOtherValue", _MBBond.ODAddOtherPercentageValue == null ? "" : _MBBond.ODAddOtherPercentageValue.ToString());
                //fields.SetField("AddODOtherValue", _MBBond.ElectricalAccessories.ToString());
                fields.SetField("ODSubTotal3", _MBBond.ODSubTotal3 == null ? "" : _MBBond.ODSubTotal3.ToString());
                //fields.SetField("ODSubTotal3", _MBBond.ODSubTotal3.ToString());
                fields.SetField("AutomobilePercentage", _MBBond.AutomobilePercentage == null ? "" : _MBBond.AutomobilePercentage.ToString());
                //fields.SetField("AutomobilePercentage", _MBBond.AutomobilePercentage.ToString());
                fields.SetField("AutomobileValue", _MBBond.AutomobileValue == null ? "" : _MBBond.AutomobileValue.ToString());
                //fields.SetField("AutomobileValue", _MBBond.AutomobileValue.ToString());
                fields.SetField("HandicappedPercentage", _MBBond.HandicappedPercentage == null ? "" : _MBBond.HandicappedPercentage.ToString());
                //fields.SetField("HandicappedPercentage", _MBBond.HandicappedPercentage.ToString());
                fields.SetField("HandicappedValue", _MBBond.HandicappedValue == null ? "" : _MBBond.HandicappedValue.ToString());
                //fields.SetField("HandicappedValue", _MBBond.HandicappedValue.ToString());
                fields.SetField("AntiTheftDevicePercentage", _MBBond.AntiTheftDevicePercentage == null ? "" : _MBBond.AntiTheftDevicePercentage.ToString());
                //fields.SetField("AntiTheftDevicePercentage", _MBBond.AntiTheftDevicePercentage.ToString());
                fields.SetField("AntiTheftDeviceValue", _MBBond.AntiTheftDeviceValue == null ? "" : _MBBond.AntiTheftDeviceValue.ToString());
                //fields.SetField("AntiTheftDeviceValue", _MBBond.AntiTheftDeviceValue.ToString());
                fields.SetField("ODSubTotal4", _MBBond.ODSubTotal4 == null ? "" : _MBBond.ODSubTotal4.ToString());
                //fields.SetField("ODSubTotal4", _MBBond.ODSubTotal4.ToString());
                fields.SetField("ODMalus", _MBBond.ODMalus == null ? "" : _MBBond.ODMalus.ToString());
                //fields.SetField("ODMalus", _MBBond.ODMalus.ToString());
                fields.SetField("ODMalusValue", _MBBond.ODMalusValue == null ? "" : _MBBond.ODMalusValue.ToString());
                //fields.SetField("ODMalusValue", _MBBond.ODMalusValue.ToString());
                fields.SetField("ODNCB", _MBBond.ODNCB == null ? "" : _MBBond.ODNCB.ToString());
                //fields.SetField("ODNCB", _MBBond.ODNCB.ToString());
                fields.SetField("ODNCBValue", _MBBond.ODNCBValue == null ? "" : _MBBond.ODNCBValue.ToString());
                //fields.SetField("ODNCBValue", _MBBond.ODNCBValue.ToString());
                fields.SetField("LessODOtherPercentage", _MBBond.ODOthersPercentage == null ? "" : _MBBond.ODOthersPercentage.ToString());
                //fields.SetField("LessODOtherPercentage", _MBBond.ODOthersPercentage.ToString());
                fields.SetField("ODOthersValue", _MBBond.ODOthersValue == null ? "" : _MBBond.ODOthersValue.ToString());
                //fields.SetField("ODOthersValue", _MBBond.ODOthersValue.ToString());
                fields.SetField("ODTotal", _MBBond.ODTotal == null ? "" : _MBBond.ODTotal.ToString());
                //fields.SetField("ODTotal", _MBBond.ODTotal.ToString());
            }
            {//LPR Details
                fields.SetField("LPRValue", _MBBond.LPRValue == null ? "" : _MBBond.LPRValue.ToString());
                //fields.SetField("LPRValue", _MBBond.LPRValue.ToString());
                fields.SetField("LPRGovtRebate", _MBBond.LPRGovtRebate == null ? "" : _MBBond.LPRGovtRebate.ToString());
                //fields.SetField("LPRGovtRebate", _MBBond.LPRGovtRebate.ToString());
                fields.SetField("LPRGovtRebateValue", _MBBond.LPRGovtRebateValue == null ? "" : _MBBond.LPRGovtRebateValue.ToString());
                //fields.SetField("LPRGovtRebateValue", _MBBond.LPRGovtRebateValue.ToString());
                fields.SetField("LPRSubTotal1", _MBBond.LPRSubTotal1 == null ? "" : _MBBond.LPRSubTotal1.ToString());
                //fields.SetField("LPRSubTotal1", _MBBond.LPRSubTotal1.ToString());
                fields.SetField("LPRDIPercentage", _MBBond.LPRDrivingInstitutionPercentage == null ? "" : _MBBond.LPRDrivingInstitutionPercentage.ToString());
                //fields.SetField("DriverRisk", _MBBond.DriverRisk.ToString());
                fields.SetField("LPRDIPercentageValue", _MBBond.LPRDrivingInstitutionPercentageValue == null ? "" : _MBBond.LPRDrivingInstitutionPercentageValue.ToString());
                //fields.SetField("DriverRisk", _MBBond.DriverRisk.ToString());
                fields.SetField("LPRLPGKitPercentage", _MBBond.LPRLPGKitPercentage == null ? "" : _MBBond.LPRLPGKitPercentage.ToString());
                //fields.SetField("LPGKitPercentage", _MBBond.LPGKitPercentage.ToString());
                fields.SetField("LPRLPGKitValue", _MBBond.LPRLPGKitValue == null ? "" : _MBBond.LPRLPGKitValue.ToString());
                //fields.SetField("LPGKitValue", _MBBond.LPGKitValue.ToString());
                fields.SetField("LPRSubTotal2", _MBBond.LPRSubTotal2 == null ? "" : _MBBond.LPRSubTotal2.ToString());
                //fields.SetField("LPRSubTotal2", _MBBond.LPRSubTotal2.ToString());
                fields.SetField("DriverRisk", _MBBond.DriverRisk == null ? "" : _MBBond.DriverRisk.ToString());
                //fields.SetField("DriverRisk", _MBBond.DriverRisk.ToString());
                fields.SetField("PillionRisk", _MBBond.PillionRisk == null ? "" : _MBBond.PillionRisk.ToString());
                //fields.SetField("PillionRisk", _MBBond.PillionRisk.ToString());
                fields.SetField("PassengersRisk", _MBBond.PassengersRisk == null ? "" : _MBBond.PassengersRisk.ToString());
                //fields.SetField("PassengersRisk", _MBBond.PassengersRisk.ToString());
                fields.SetField("CleanersRisk", _MBBond.CleanersRisk == null ? "" : _MBBond.CleanersRisk.ToString());
                //fields.SetField("CleanersRisk", _MBBond.CleanersRisk.ToString());
                fields.SetField("CooliesRisk", _MBBond.CooliesRisk == null ? "" : _MBBond.CooliesRisk.ToString());
                //fields.SetField("CooliesRisk", _MBBond.CooliesRisk.ToString());
                fields.SetField("LPRMalus", _MBBond.LPRMalus == null ? "" : _MBBond.LPRMalus.ToString());
                //fields.SetField("LPRMalus", _MBBond.LPRMalus.ToString());
                fields.SetField("LPRMalusValue", _MBBond.LPRMalusValue == null ? "" : _MBBond.LPRMalusValue.ToString());
                //fields.SetField("LPRMalusValue", _MBBond.LPRMalusValue.ToString());
                fields.SetField("LPRTotal", _MBBond.LPRTotal == null ? "" : _MBBond.LPRTotal.ToString());
                //fields.SetField("LPRTotal", _MBBond.LPRTotal.ToString());
            }
            {//TAX Final Premium Details
                fields.SetField("TotalA", _MBBond.TotalA == null ? "" : _MBBond.TotalA.ToString());
                //fields.SetField("TotalA", _MBBond.TotalA.ToString());
                fields.SetField("TotalB", _MBBond.TotalB == null ? "" : _MBBond.TotalB.ToString());
                //fields.SetField("TotalB", _MBBond.TotalB.ToString());
                fields.SetField("TotalAB", _MBBond.TotalAB == null ? "" : _MBBond.TotalAB.ToString());
                //fields.SetField("TotalAB", _MBBond.TotalAB.ToString());
                fields.SetField("PreviousYearDifference", _MBBond.PreviousYearDifference == null ? "" : _MBBond.PreviousYearDifference.ToString());
                //fields.SetField("PreviousYearDifference", _MBBond.PreviousYearDifference.ToString());
                fields.SetField("CurrentYearDifference", _MBBond.CurrentYearDifference == null ? "" : _MBBond.CurrentYearDifference.ToString());
                //fields.SetField("CurrentYearDifference", _MBBond.CurrentYearDifference.ToString());
                fields.SetField("Premium", (_MBBond.Premium==null)?"": _MBBond.Premium.ToString());
                fields.SetField("CGSTofPremium", _MBBond.CGSTofPremium.ToString());
                fields.SetField("SGSTofPremium", _MBBond.SGSTofPremium.ToString());
                //fields.SetField("CGSTofPremium", _MBBond.GSTofPremium.ToString());
                fields.SetField("FinalAmount", _MBBond.FinalAmount.ToString());
                fields.SetField("PayablePremium", _MBBond.PayablePremium.ToString());
                fields.SetField("Endorsements", _MBBond.Endorsements.ToString());
            }
            pdfStamper.Close();
            return newFile;
        }
        private string FillFormMBCalculationsheet(VM_MB_Bond_Details _MBBond, string PolicyNumber)
        {
            string pdfTemplate = Server.MapPath("~/MIBond/Motor_Branch_Bond_Template.pdf");
            //string newFile = Server.MapPath("~/MIBond/" + PolicyNumber + "UnSigned" + ".pdf");
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"MIBond\Calculationsheet\" + PolicyNumber + ".pdf";
            }
            //PdfWriter.GetInstance(doc1, new FileStream(newFile, FileMode.Create));
            //doc1.Open();
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            {//Basic Details
                //fields.SetField("SectionCode", _MBBond.SectionCode == null ? "" : _MBBond.SectionCode.ToString());
                //fields.SetField("SectionCode", _MBBond.SectionCode.ToString());
                fields.SetField("CertificateNo", _MBBond.Application_ref_no == 0 ? "" : _MBBond.Application_ref_no.ToString());
                //fields.SetField("CertificateNo", _MBBond.CertificateNo.ToString());
                fields.SetField("PolicyNo", _MBBond.PolicyNumber == null ? "" : _MBBond.PolicyNumber.ToString());
                //fields.SetField("PolicyNo", _MBBond.PolicyNumber.ToString());
                fields.SetField("PolicyType", _MBBond.PolicyType == null ? "" : _MBBond.PolicyType.ToString());
                //fields.SetField("PolicyType", _MBBond.PolicyType.ToString());
                fields.SetField("ChallanNo", _MBBond.ChallanNo == null ? "" : _MBBond.ChallanNo.ToString());
                //fields.SetField("ChallanNo", _MBBond.ChallanNo.ToString());
                fields.SetField("Date", _MBBond.Sanction_Date == null ? "" : _MBBond.Sanction_Date.ToString());
                //fields.SetField("Date", _MBBond.Sanction_Date.ToString());
                fields.SetField("ProposerNameAddress", _MBBond.ProposerNameAddress == null ? "" : _MBBond.ProposerNameAddress.ToString());
                //fields.SetField("ProposerNameAddress", _MBBond.ProposerNameAddress.ToString());
                fields.SetField("PremiumPaid", _MBBond.PolicyPremiumAmount == null ? "" : _MBBond.PolicyPremiumAmount.ToString());
                //fields.SetField("PremiumPaid", _MBBond.PolicyPremiumAmount.ToString());
                fields.SetField("ODRiskFromDate", _MBBond.ODFrom_Date == null ? "" : _MBBond.ODFrom_Date.ToString());
                //fields.SetField("RiskFromDate", _MBBond.From_Date.ToString());
                fields.SetField("ODRiskToDate", _MBBond.ODTo_Date == null ? "" : _MBBond.ODTo_Date.ToString());
                //fields.SetField("RiskToDate", _MBBond.To_Date.ToString());
                fields.SetField("TPRiskFromDate", _MBBond.TPFrom_Date == null ? "" : _MBBond.TPFrom_Date.ToString());
                //fields.SetField("RiskFromDate", _MBBond.From_Date.ToString());
                fields.SetField("TPRiskToDate", _MBBond.TPTo_Date == null ? "" : _MBBond.TPTo_Date.ToString());
                //fields.SetField("RiskToDate", _MBBond.To_Date.ToString());
                fields.SetField("RegistrationNo", _MBBond.Regisration_no == null ? "" : _MBBond.Regisration_no.ToString());
                //fields.SetField("RegistrationNo", _MBBond.Regisration_no.ToString());
                fields.SetField("Make", _MBBond.Make_of_vehicle == null ? "" : _MBBond.Make_of_vehicle.ToString());
                fields.SetField("YearofManufacture", _MBBond.Year_of_manufacturer.ToString());
                fields.SetField("CC", _MBBond.Cubic_capacity.ToString());
                fields.SetField("GVW", _MBBond.GVW == null ? "" : _MBBond.GVW.ToString());
                //fields.SetField("GVW", _MBBond.GVW.ToString());
                fields.SetField("SeatingCapacity", _MBBond.SeatingCapacity == null ? "" : _MBBond.SeatingCapacity.ToString());
                //fields.SetField("SeatingCapacity", _MBBond.SeatingCapacity.ToString());
                fields.SetField("EngineNo", _MBBond.Engine_no.ToString());
                fields.SetField("ChassisNo", _MBBond.Chasis_no.ToString());
                fields.SetField("Zone", _MBBond.Zone == null ? "" : _MBBond.Zone.ToString());
                //fields.SetField("Zone", _MBBond.Zone.ToString());
                fields.SetField("InsuredDecalreValue", _MBBond.Insured_declared_value_amount == null ? "" : _MBBond.Insured_declared_value_amount.ToString());
                //fields.SetField("InsuredDecalreValue", _MBBond.Insured_declared_value_amount.ToString());
                fields.SetField("SideCarTrailer", _MBBond.Side_car_trailer_amount == null ? "" : _MBBond.Side_car_trailer_amount.ToString());
                //fields.SetField("SideCar", _MBBond.Side_car_trailer_amount.ToString());
                //fields.SetField("Trailer", _MBBond.Side_car_trailer_amount == null ? "" : _MBBond.Side_car_trailer_amount.ToString());
                //fields.SetField("Trailer", _MBBond.Side_car_trailer_amount.ToString());
                fields.SetField("NonElectrical", _MBBond.Non_electrical_accessories_amount == null ? "" : _MBBond.Non_electrical_accessories_amount.ToString());
                //fields.SetField("NonElectrical", _MBBond.Non_electrical_accessories_amount.ToString());
                fields.SetField("Electrical", _MBBond.Electrical_accessories_amount == null ? "" : _MBBond.Electrical_accessories_amount.ToString());
                //fields.SetField("Electrical", _MBBond.Electrical_accessories_amount.ToString());
                fields.SetField("CngLpgUnit", _MBBond.Value_of_cng_lpg_amount == null ? "" : _MBBond.Value_of_cng_lpg_amount.ToString());
                //fields.SetField("CNGUnit", _MBBond.Value_of_cng_lpg_amount.ToString());
                //fields.SetField("LPGUnit", _MBBond.Value_of_cng_lpg_amount == null ? "" : _MBBond.Value_of_cng_lpg_amount.ToString());
                //fields.SetField("LPGUnit", _MBBond.Value_of_cng_lpg_amount.ToString());
                fields.SetField("Depreciation", _MBBond.Depreciation == null ? "" : _MBBond.Depreciation.ToString());
                //fields.SetField("Depreciation", _MBBond.Depreciation.ToString());
                fields.SetField("TotalPVV", _MBBond.Idv_total_amount.ToString());
            }
            {//OD Details
                fields.SetField("BP", _MBBond.BP == null ? "" : _MBBond.BP.ToString());
                //fields.SetField("BP", _MBBond.BP.ToString());
                fields.SetField("ODPercentage", _MBBond.ODPercentage == null ? "" : _MBBond.ODPercentage.ToString());
                //fields.SetField("ODPercentage", _MBBond.ODPercentage.ToString());
                fields.SetField("ODPercentageValue", _MBBond.ODPercentageValue == null ? "" : _MBBond.ODPercentageValue.ToString());
                //fields.SetField("ODPercentageValue", _MBBond.ODPercentageValue.ToString());
                fields.SetField("ODSubTotal1", _MBBond.ODSubTotal1 == null ? "" : _MBBond.ODSubTotal1.ToString());
                //fields.SetField("ODSubTotal1", _MBBond.ODSubTotal1.ToString());
                fields.SetField("ODPremium", _MBBond.ODPremium == null ? "" : _MBBond.ODPremium.ToString());
                //fields.SetField("ODPremium", _MBBond.ODPremium.ToString());
                fields.SetField("GVWExtraAmount", _MBBond.GVWExtraAmount == null ? "" : _MBBond.GVWExtraAmount.ToString());
                //fields.SetField("GVWExtraAmount", _MBBond.GVWExtraAmount.ToString());
                fields.SetField("ODGovtRebate", _MBBond.ODGovtRebate == null ? "" : _MBBond.ODGovtRebate.ToString());
                //fields.SetField("ODGovtRebate", _MBBond.ODGovtRebate.ToString());
                fields.SetField("ODGovtRebateValue", _MBBond.ODGovtRebateValue == null ? "" : _MBBond.ODGovtRebateValue.ToString());
                //fields.SetField("ODGovtRebateValue", _MBBond.ODGovtRebateValue.ToString());
                fields.SetField("ODSubTotal2", _MBBond.ODSubTotal2 == null ? "" : _MBBond.ODSubTotal2.ToString());
                //fields.SetField("ODSubTotal2", _MBBond.ODSubTotal2.ToString());
                fields.SetField("EAPercentage", _MBBond.EAPercentage == null ? "" : _MBBond.EAPercentage.ToString());
                //fields.SetField("ElectricalAccessories", _MBBond.ElectricalAccessories.ToString());
                fields.SetField("EAPercentageValue", _MBBond.EAPercentageValue == null ? "" : _MBBond.EAPercentageValue.ToString());
                //fields.SetField("ElectricalAccessories", _MBBond.ElectricalAccessories.ToString());
                fields.SetField("ODLPGKitPercentage", _MBBond.ODLPGKitPercentage == null ? "" : _MBBond.ODLPGKitPercentage.ToString());
                //fields.SetField("LPGKitPercentage", _MBBond.LPGKitPercentage.ToString());
                fields.SetField("ODLPGKitValue", _MBBond.ODLPGKitValue == null ? "" : _MBBond.ODLPGKitValue.ToString());
                //fields.SetField("LPGKitValue", _MBBond.LPGKitValue.ToString());
                fields.SetField("ImportedPercentage", _MBBond.ImportedPercentage == null ? "" : _MBBond.ImportedPercentage.ToString());
                //fields.SetField("ImportedPercentage", _MBBond.ImportedPercentage.ToString());
                fields.SetField("ImportedVehiclesValue", _MBBond.ImportedVehiclesValue == null ? "" : _MBBond.ImportedVehiclesValue.ToString());
                //fields.SetField("ImportedVehiclesValue", _MBBond.ImportedVehiclesValue.ToString());
                fields.SetField("FibreGlassFuelTank", _MBBond.FibreGlassFuelTank == null ? "" : _MBBond.FibreGlassFuelTank.ToString());
                //fields.SetField("FibreGlassFuelTank", _MBBond.FibreGlassFuelTank.ToString());
                fields.SetField("ODDIPercentage", _MBBond.ODDrivingInstitutionPercentage == null ? "" : _MBBond.ODDrivingInstitutionPercentage.ToString());
                //fields.SetField("DrivingInstitutionPercentage", _MBBond.DrivingInstitutionPercentage.ToString());
                fields.SetField("ODDIPercentageValue", _MBBond.ODDrivingInstitutionPercentage == null ? "" : _MBBond.ODDrivingInstitutionPercentageValue.ToString());
                //fields.SetField("DrivingInstitutionPercentage", _MBBond.DrivingInstitutionPercentage.ToString());
                fields.SetField("AddODOtherPercentage", _MBBond.ODAddOtherPercentage == null ? "" : _MBBond.ODAddOtherPercentage.ToString());
                //fields.SetField("AddODOtherPercentage", _MBBond.ODAddOtherPercentage.ToString());
                fields.SetField("AddODOtherValue", _MBBond.ODAddOtherPercentageValue == null ? "" : _MBBond.ODAddOtherPercentageValue.ToString());
                //fields.SetField("AddODOtherValue", _MBBond.ElectricalAccessories.ToString());
                fields.SetField("ODSubTotal3", _MBBond.ODSubTotal3 == null ? "" : _MBBond.ODSubTotal3.ToString());
                //fields.SetField("ODSubTotal3", _MBBond.ODSubTotal3.ToString());
                fields.SetField("AutomobilePercentage", _MBBond.AutomobilePercentage == null ? "" : _MBBond.AutomobilePercentage.ToString());
                //fields.SetField("AutomobilePercentage", _MBBond.AutomobilePercentage.ToString());
                fields.SetField("AutomobileValue", _MBBond.AutomobileValue == null ? "" : _MBBond.AutomobileValue.ToString());
                //fields.SetField("AutomobileValue", _MBBond.AutomobileValue.ToString());
                fields.SetField("HandicappedPercentage", _MBBond.HandicappedPercentage == null ? "" : _MBBond.HandicappedPercentage.ToString());
                //fields.SetField("HandicappedPercentage", _MBBond.HandicappedPercentage.ToString());
                fields.SetField("HandicappedValue", _MBBond.HandicappedValue == null ? "" : _MBBond.HandicappedValue.ToString());
                //fields.SetField("HandicappedValue", _MBBond.HandicappedValue.ToString());
                fields.SetField("AntiTheftDevicePercentage", _MBBond.AntiTheftDevicePercentage == null ? "" : _MBBond.AntiTheftDevicePercentage.ToString());
                //fields.SetField("AntiTheftDevicePercentage", _MBBond.AntiTheftDevicePercentage.ToString());
                fields.SetField("AntiTheftDeviceValue", _MBBond.AntiTheftDeviceValue == null ? "" : _MBBond.AntiTheftDeviceValue.ToString());
                //fields.SetField("AntiTheftDeviceValue", _MBBond.AntiTheftDeviceValue.ToString());
                fields.SetField("ODSubTotal4", _MBBond.ODSubTotal4 == null ? "" : _MBBond.ODSubTotal4.ToString());
                //fields.SetField("ODSubTotal4", _MBBond.ODSubTotal4.ToString());
                fields.SetField("ODMalus", _MBBond.ODMalus == null ? "" : _MBBond.ODMalus.ToString());
                //fields.SetField("ODMalus", _MBBond.ODMalus.ToString());
                fields.SetField("ODMalusValue", _MBBond.ODMalusValue == null ? "" : _MBBond.ODMalusValue.ToString());
                //fields.SetField("ODMalusValue", _MBBond.ODMalusValue.ToString());
                fields.SetField("ODNCB", _MBBond.ODNCB == null ? "" : _MBBond.ODNCB.ToString());
                //fields.SetField("ODNCB", _MBBond.ODNCB.ToString());
                fields.SetField("ODNCBValue", _MBBond.ODNCBValue == null ? "" : _MBBond.ODNCBValue.ToString());
                //fields.SetField("ODNCBValue", _MBBond.ODNCBValue.ToString());
                fields.SetField("LessODOtherPercentage", _MBBond.ODOthersPercentage == null ? "" : _MBBond.ODOthersPercentage.ToString());
                //fields.SetField("LessODOtherPercentage", _MBBond.ODOthersPercentage.ToString());
                fields.SetField("ODOthersValue", _MBBond.ODOthersValue == null ? "" : _MBBond.ODOthersValue.ToString());
                //fields.SetField("ODOthersValue", _MBBond.ODOthersValue.ToString());
                fields.SetField("ODTotal", _MBBond.ODTotal == null ? "" : _MBBond.ODTotal.ToString());
                //fields.SetField("ODTotal", _MBBond.ODTotal.ToString());
            }
            {//LPR Details
                fields.SetField("LPRValue", _MBBond.LPRValue == null ? "" : _MBBond.LPRValue.ToString());
                //fields.SetField("LPRValue", _MBBond.LPRValue.ToString());
                fields.SetField("LPRGovtRebate", _MBBond.LPRGovtRebate == null ? "" : _MBBond.LPRGovtRebate.ToString());
                //fields.SetField("LPRGovtRebate", _MBBond.LPRGovtRebate.ToString());
                fields.SetField("LPRGovtRebateValue", _MBBond.LPRGovtRebateValue == null ? "" : _MBBond.LPRGovtRebateValue.ToString());
                //fields.SetField("LPRGovtRebateValue", _MBBond.LPRGovtRebateValue.ToString());
                fields.SetField("LPRSubTotal1", _MBBond.LPRSubTotal1 == null ? "" : _MBBond.LPRSubTotal1.ToString());
                //fields.SetField("LPRSubTotal1", _MBBond.LPRSubTotal1.ToString());
                fields.SetField("LPRDIPercentage", _MBBond.LPRDrivingInstitutionPercentage == null ? "" : _MBBond.LPRDrivingInstitutionPercentage.ToString());
                //fields.SetField("DriverRisk", _MBBond.DriverRisk.ToString());
                fields.SetField("LPRDIPercentageValue", _MBBond.LPRDrivingInstitutionPercentageValue == null ? "" : _MBBond.LPRDrivingInstitutionPercentageValue.ToString());
                //fields.SetField("DriverRisk", _MBBond.DriverRisk.ToString());
                fields.SetField("LPRLPGKitPercentage", _MBBond.LPRLPGKitPercentage == null ? "" : _MBBond.LPRLPGKitPercentage.ToString());
                //fields.SetField("LPGKitPercentage", _MBBond.LPGKitPercentage.ToString());
                fields.SetField("LPRLPGKitValue", _MBBond.LPRLPGKitValue == null ? "" : _MBBond.LPRLPGKitValue.ToString());
                //fields.SetField("LPGKitValue", _MBBond.LPGKitValue.ToString());
                fields.SetField("LPRSubTotal2", _MBBond.LPRSubTotal2 == null ? "" : _MBBond.LPRSubTotal2.ToString());
                //fields.SetField("LPRSubTotal2", _MBBond.LPRSubTotal2.ToString());
                fields.SetField("DriverRisk", _MBBond.DriverRisk == null ? "" : _MBBond.DriverRisk.ToString());
                //fields.SetField("DriverRisk", _MBBond.DriverRisk.ToString());
                fields.SetField("PillionRisk", _MBBond.PillionRisk == null ? "" : _MBBond.PillionRisk.ToString());
                //fields.SetField("PillionRisk", _MBBond.PillionRisk.ToString());
                fields.SetField("PassengersRisk", _MBBond.PassengersRisk == null ? "" : _MBBond.PassengersRisk.ToString());
                //fields.SetField("PassengersRisk", _MBBond.PassengersRisk.ToString());
                fields.SetField("CleanersRisk", _MBBond.CleanersRisk == null ? "" : _MBBond.CleanersRisk.ToString());
                //fields.SetField("CleanersRisk", _MBBond.CleanersRisk.ToString());
                fields.SetField("CooliesRisk", _MBBond.CooliesRisk == null ? "" : _MBBond.CooliesRisk.ToString());
                //fields.SetField("CooliesRisk", _MBBond.CooliesRisk.ToString());
                fields.SetField("LPRMalus", _MBBond.LPRMalus == null ? "" : _MBBond.LPRMalus.ToString());
                //fields.SetField("LPRMalus", _MBBond.LPRMalus.ToString());
                fields.SetField("LPRMalusValue", _MBBond.LPRMalusValue == null ? "" : _MBBond.LPRMalusValue.ToString());
                //fields.SetField("LPRMalusValue", _MBBond.LPRMalusValue.ToString());
                fields.SetField("LPRTotal", _MBBond.LPRTotal == null ? "" : _MBBond.LPRTotal.ToString());
                //fields.SetField("LPRTotal", _MBBond.LPRTotal.ToString());
            }
            {//TAX Final Premium Details
                fields.SetField("TotalA", _MBBond.TotalA == null ? "" : _MBBond.TotalA.ToString());
                //fields.SetField("TotalA", _MBBond.TotalA.ToString());
                fields.SetField("TotalB", _MBBond.TotalB == null ? "" : _MBBond.TotalB.ToString());
                //fields.SetField("TotalB", _MBBond.TotalB.ToString());
                fields.SetField("TotalAB", _MBBond.TotalAB == null ? "" : _MBBond.TotalAB.ToString());
                //fields.SetField("TotalAB", _MBBond.TotalAB.ToString());
                fields.SetField("PreviousYearDifference", _MBBond.PreviousYearDifference == null ? "" : _MBBond.PreviousYearDifference.ToString());
                //fields.SetField("PreviousYearDifference", _MBBond.PreviousYearDifference.ToString());
                fields.SetField("CurrentYearDifference", _MBBond.CurrentYearDifference == null ? "" : _MBBond.CurrentYearDifference.ToString());
                //fields.SetField("CurrentYearDifference", _MBBond.CurrentYearDifference.ToString());
                fields.SetField("Premium", (_MBBond.Premium==null)?"": _MBBond.Premium.ToString());
                fields.SetField("CGSTofPremium", _MBBond.CGSTofPremium.ToString());
                fields.SetField("SGSTofPremium", _MBBond.SGSTofPremium.ToString());
                //fields.SetField("CGSTofPremium", _MBBond.GSTofPremium.ToString());
                fields.SetField("FinalAmount", _MBBond.FinalAmount.ToString());
                fields.SetField("PayablePremium", _MBBond.PayablePremium.ToString());
                fields.SetField("Endorsements", _MBBond.Endorsements.ToString());
            }
            pdfStamper.Close();
            return newFile;
        }

        private string MBSignPdf(string unsignedpdf, string PolicyNumber)
        {
            try
            {
                //ErrorLogs("logFile_DSC", "Program Begins");

                X509CertificateParser cp = new X509CertificateParser();
                X509Certificate2 certClient = null;

                X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                st.Open(OpenFlags.MaxAllowed);
                X509Certificate2Collection collection = st.Certificates;
                for (int i = 0; i < collection.Count; i++)
                {
                    if (collection[i].Subject.Contains("AJAY ADALA"))
                    {
                        certClient = collection[i];
                    }
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
                string newFile = Server.MapPath("~/MIBond/" + PolicyNumber + ".pdf");
                FileStream signedPdf = new FileStream(newFile, FileMode.Create);
                PdfStamper pdfStamper = PdfStamper.CreateSignature(inputPdf, signedPdf, '\0');


                IExternalSignature externalSignature = new X509Certificate2Signature(certClient, "SHA-256");

                PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;

                //signatureAppearance.Reason = "My Signature";
                signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(10, 00, 200, 100), inputPdf.NumberOfPages, "Signature");
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
        public VM_MB_Bond_Details GetMBBondCalculationDetails(VM_MotorInsurancePolicyPrintDetails MIPD)
        {
            VM_MB_Bond_Details MB_Bond = new VM_MB_Bond_Details();
            //Policy Details
            MB_Bond.PolicyPremiumAmount = MIPD.premium;
            MB_Bond.PolicyNumber = MIPD.policy_number;
            //MB_Bond.ODFrom_Date = MIPD.from_date;
            //MB_Bond.ODTo_Date = MIPD.to_date;
            //MB_Bond.TPFrom_Date = MIPD.tp_from_date;
            //MB_Bond.TPTo_Date = MIPD.tp_to_date;
            if (MIPD.type_of_cover == 1)
            {
                MB_Bond.TPFrom_Date = MIPD.from_date;
                MB_Bond.TPTo_Date = MIPD.to_date;
            }
            else if (MIPD.type_of_cover == 2)
            {
                MB_Bond.ODFrom_Date = MIPD.from_date;
                MB_Bond.ODTo_Date = MIPD.to_date;
                MB_Bond.TPFrom_Date = MIPD.tp_from_date;
                MB_Bond.TPTo_Date = MIPD.tp_to_date;
            }
            else if (MIPD.type_of_cover == 3)
            {
                MB_Bond.ODFrom_Date = MIPD.from_date;
                MB_Bond.ODTo_Date = MIPD.to_date;
                MB_Bond.TPFrom_Date = MIPD.tp_from_date;
                MB_Bond.TPTo_Date = MIPD.tp_to_date;
            }
            MB_Bond.Sanction_Date = MIPD.sanction_date;
            //Proposer Details
            MB_Bond.ProposerNameAddress = MIPD.nameandaddress;
            MB_Bond.PolicyType = MIPD.type_of_cover == 1 ? "Liability Only Policy"
         : MIPD.type_of_cover == 2 ? "Package Policy"
         : MIPD.type_of_cover == 3 ? "Bundle Policy"
         : MIPD.type_of_cover == 4 ? "O"
         : "";
            //Vehicle Details
            MB_Bond.Application_ref_no = MIPD.application_ref_no;
            MB_Bond.Proposer_id = MIPD.proposer_id;
            MB_Bond.Regisration_no = MIPD.regisration_no;
            MB_Bond.Registration_authority_and_location = MIPD.registration_authority_and_location;
            MB_Bond.Year_of_manufacturer = MIPD.year_of_manufacturer;
            MB_Bond.Chasis_no = MIPD.chasis_no;
            MB_Bond.Engine_no = MIPD.engine_no;
            MB_Bond.SeatingCapacity = MIPD.seating_capacity_including_driver;
            MB_Bond.Cubic_capacity = MIPD.cubic_capacity;
            MB_Bond.GVW = Convert.ToString(MIPD.vehicle_weight);
            MB_Bond.Zone = MIPD.zone;
            MB_Bond.VehicleCategory = MIPD.vehiclecategory;
            MB_Bond.Make_of_vehicle = MIPD.vehicle_make;
            MB_Bond.Endorsements = MIPD.Endorsements;
            ///IDV Details
            MB_Bond.Insured_declared_value_amount = MIPD.insured_declared_value_amount;
            MB_Bond.Side_car_trailer_amount = MIPD.side_car_trailer_amount;
            MB_Bond.Non_electrical_accessories_amount = MIPD.non_electrical_accessories_amount;
            MB_Bond.Electrical_accessories_amount = MIPD.electrical_accessories_amount;
            MB_Bond.Value_of_cng_lpg_amount = MIPD.value_of_cng_lpg_amount;
            MB_Bond.Depreciation = Convert.ToString(MIPD.depreciation_value);
            decimal total_amount = Convert.ToDecimal(MIPD.total_amount);
            string ctotal_amount = String.Format("{0:0,0.00}", total_amount);
            MB_Bond.Idv_total_amount = ctotal_amount;

            decimal GrossVehicleWeight = 0;
            if (MIPD.vehiclecategory == 5 || MIPD.vehiclecategory == 6)
            {
                decimal weight = MIPD.vehicle_weight;
                decimal actualweight = 12000;
                if (weight > actualweight)
                {
                    decimal weightdiff = (weight - actualweight) / 100;
                    GrossVehicleWeight = weightdiff * 27;
                }
            }
            int rGrossVehicleWeight = Convert.ToInt32(GrossVehicleWeight);
            string cGrossVehicleWeight = String.Format("{0:0,0.00}", rGrossVehicleWeight);

            if (MIPD.type_of_cover == 2)
            {
                //A. Own Damage
                MB_Bond.BP = Convert.ToString(MIPD.mia_own_damage_additional_value);
                if (MB_Bond.BP == "0")
                {
                    MB_Bond.BP = "";
                }
                MB_Bond.ODPercentage = Convert.ToString(MIPD.own_damage_value);
                decimal odvalue = Convert.ToDecimal(MIPD.own_damage_value);
                decimal res = ((total_amount / 100) * (odvalue));
                int rres = Convert.ToInt32(res);
                decimal odsubtot = Convert.ToDecimal(MIPD.mia_own_damage_additional_value) + rres;
                int rodsubtot = Convert.ToInt32(odsubtot);
                string cres = String.Format("{0:0,0.00}", rodsubtot);
                MB_Bond.ODPercentageValue = String.Format("{0:0,0.00}", rres);
                MB_Bond.ODSubTotal1 = cres;
                MB_Bond.ODPremium = cres;
                MB_Bond.GVWExtraAmount = cGrossVehicleWeight;
                //MB_Bond.txtbpidvValue = cres;
                decimal res1 = ((odsubtot / 100) * Convert.ToDecimal(MIPD.od_gov_discount));
                int rres1 = Convert.ToInt32(res1);
                string cres1 = String.Format("{0:0,0.00}", rres1);
                MB_Bond.ODGovtRebate = Convert.ToString(MIPD.od_gov_discount);
                MB_Bond.ODGovtRebateValue = cres1;
                //MB_Bond.txtlgrodValue = cres1;
                decimal txtrebatetotodvalue = rodsubtot - rres1 + rGrossVehicleWeight;
                int rtxtrebatetotodvalue = Convert.ToInt32(txtrebatetotodvalue);
                string ctxtrebatetotodvalue = String.Format("{0:0,0.00}", rtxtrebatetotodvalue);
                MB_Bond.ODSubTotal2 = ctxtrebatetotodvalue;
                decimal Addele = ((Convert.ToDecimal(MIPD.electrical_accessories_amount) / 100) * 4);
                decimal Addlpg = ((Convert.ToDecimal(MIPD.value_of_cng_lpg_amount) / 100) * 4);
                decimal FiberGlassValue = 0;
                if (MIPD.isfiberglassfitted == true)
                {
                    if (MIPD.vehiclecategory == 16)
                    {
                        FiberGlassValue = 100;
                    }
                    else
                    {
                        FiberGlassValue = 50;
                    }
                    MB_Bond.FibreGlassFuelTank = String.Format("{0:0,0.00}", FiberGlassValue); ;
                }
                decimal ODDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    ODDrvingInstitutionValue = ((Convert.ToDecimal(txtrebatetotodvalue) / 100) * 60);
                    MB_Bond.ODDrivingInstitutionPercentage = "60";
                    MB_Bond.ODDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", ODDrvingInstitutionValue); ;
                }
                decimal Addnonele = ((Convert.ToDecimal(MIPD.non_electrical_accessories_amount) / 100) * 4);
                decimal ODAddSubtot = txtrebatetotodvalue + Addele + Addlpg + FiberGlassValue + ODDrvingInstitutionValue + Addnonele;
                int rODAddSubtot = Convert.ToInt32(ODAddSubtot);
                MB_Bond.EAPercentage = "4";
                MB_Bond.EAPercentageValue = String.Format("{0:0,0.00}", Addele);
                MB_Bond.ODLPGKitPercentage = "4";
                MB_Bond.ODLPGKitValue = String.Format("{0:0,0.00}", Addlpg);
                MB_Bond.ODAddOtherPercentage = "4";
                MB_Bond.ODAddOtherPercentageValue = String.Format("{0:0,0.00}", Addnonele);
                MB_Bond.ODSubTotal3 = String.Format("{0:0,0.00}", rODAddSubtot);
                decimal AutoMobileValue = 0;
                if (MIPD.vehicletype == 1)
                {
                    if (MIPD.isautomobileassociation == true)
                    {
                        AutoMobileValue = ((rODAddSubtot / 100) * 5);
                        MB_Bond.AutomobilePercentage = "5";
                        MB_Bond.AutomobileValue = String.Format("{0:0,0.00}", AutoMobileValue); ;
                    }
                }
                decimal ODLessSubtot = ODAddSubtot - AutoMobileValue;
                int rODLessSubtot = Convert.ToInt32(ODLessSubtot);
                MB_Bond.ODSubTotal4 = String.Format("{0:0,0.00}", rODLessSubtot);

                //MB_Bond.ODMalusValue=
                MB_Bond.ODMalus = MIPD.ph_malus_value.ToString();

                decimal txtlessmalusValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtlessmalusValue = Convert.ToInt32(txtlessmalusValue);
                string ctxtlessmalusValue = String.Format("{0:0,0.00}", rtxtlessmalusValue);
                MB_Bond.ODMalusValue = ctxtlessmalusValue;

                MB_Bond.ODNCB = MIPD.ph_ncb_value.ToString();

                //MB_Bond.txtrebatetotodvalue = ctxtrebatetotodvalue;
                decimal txtlessncbValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_ncb_value));
                int rtxtlessncbValue = Convert.ToInt32(txtlessncbValue);
                string ctxtlessncbValue = String.Format("{0:0,0.00}", rtxtlessncbValue);
                MB_Bond.ODNCBValue = ctxtlessncbValue;

                //MB_Bond.txtrebatetotodvalue = ctxtlessncbValue;
                decimal txtodtotValue = (rODLessSubtot + rtxtlessmalusValue) - rtxtlessncbValue;
                int rtxtodtotValue = Convert.ToInt32(txtodtotValue);
                decimal sidecardiscount = 0;
                if (Convert.ToDecimal(MIPD.side_car_trailer_amount) > 1)
                {
                    sidecardiscount = ((rtxtodtotValue / 100) * 25);
                }
                decimal odafterSDdiscount = rtxtodtotValue - sidecardiscount;
                int rodafterSDdiscount = Convert.ToInt32(odafterSDdiscount);
                string ctxtodtotValue = String.Format("{0:0,0.00}", rodafterSDdiscount);
                MB_Bond.ODOthersPercentage = "25";
                MB_Bond.ODOthersValue = String.Format("{0:0,0.00}", sidecardiscount);
                MB_Bond.ODTotal = ctxtodtotValue;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                decimal LPRvalue = Convert.ToDecimal(MIPD.premium_liability_value);
                int rLPRvalue = Convert.ToInt32(LPRvalue);
                decimal txtlgrlprValue = ((LPRvalue / 100) * Convert.ToDecimal(MIPD.liability_gov_discount));
                int rtxtlgrlprValue = Convert.ToInt32(txtlgrlprValue);
                string ctxtlgrlprValue = String.Format("{0:0,0.00}", rtxtlgrlprValue);
                MB_Bond.LPRValue = Convert.ToString(MIPD.premium_liability_value);
                MB_Bond.LPRGovtRebate = Convert.ToString(MIPD.liability_gov_discount);
                MB_Bond.LPRGovtRebateValue = ctxtlgrlprValue;
                //MB_Bond.txtlgrlprValue = ctxtlgrlprValue;
                decimal txtsubtotlprValue = rLPRvalue - rtxtlgrlprValue;
                int rtxtsubtotlprValue = Convert.ToInt32(txtsubtotlprValue);
                string ctxtsubtotlprValue = String.Format("{0:0,0.00}", rtxtsubtotlprValue);
                MB_Bond.LPRSubTotal1 = ctxtsubtotlprValue;
                //MB_Bond.txtsubtotlprValue = ctxtsubtotlprValue;
                decimal txtcngamntrValue = Convert.ToDecimal(MIPD.value_of_cng_lpg_amount);
                int rtxtcngamntrValue = Convert.ToInt32(txtcngamntrValue);
                int txtlpgkitlprValue = 0;
                if (rtxtcngamntrValue != 0)
                {
                    txtlpgkitlprValue = 60;
                    MB_Bond.LPRLPGKitValue = String.Format("{0:0,0.00}", txtlpgkitlprValue);
                }
                int rtxtsubtotlpglprValue = rtxtsubtotlprValue + txtlpgkitlprValue;
                string crtxtsubtotlpglprValue = String.Format("{0:0,0.00}", rtxtsubtotlpglprValue);
                MB_Bond.LPRSubTotal2 = crtxtsubtotlpglprValue;
                decimal DriverAmount = Convert.ToDecimal(MIPD.driver_amount);
                string cDriverAmount = String.Format("{0:0,0.00}", DriverAmount);
                MB_Bond.DriverRisk = cDriverAmount;
                //MB_Bond.res10 = cDriverAmount;
                decimal PassengerAmount = 0;
                string cPassengerAmount = string.Empty;
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    PassengerAmount = Convert.ToDecimal(MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else if (MIPD.vehiclecategory == 17)
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver - 1) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    MB_Bond.PillionRisk = cPassengerAmount;
                }
                else
                {
                    MB_Bond.PassengersRisk = cPassengerAmount;
                }
                //MB_Bond.PassengersRisk = cPassengerAmount;
                //MB_Bond.res11 = cPassengerAmount;
                decimal txtlprtotValue = (rtxtsubtotlpglprValue + DriverAmount + PassengerAmount);
                int rtxtlprtotValue = Convert.ToInt32(txtlprtotValue);
                string ctxtlprtotValue = String.Format("{0:0,0.00}", rtxtlprtotValue);
                MB_Bond.LPRTotal = ctxtlprtotValue;
                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = rodafterSDdiscount + rtxtlprtotValue;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = ctxtodtotValue;
                MB_Bond.TotalB = ctxtlprtotValue;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            else if (MIPD.type_of_cover == 1)
            {
                //A. Own Damage
                MB_Bond.ODTotal = string.Empty;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                decimal LPRvalue = Convert.ToDecimal(MIPD.premium_liability_value);
                int rLPRvalue = Convert.ToInt32(LPRvalue);
                decimal txtlgrlprValue = ((LPRvalue / 100) * Convert.ToDecimal(MIPD.liability_gov_discount));
                int rtxtlgrlprValue = Convert.ToInt32(txtlgrlprValue);
                string ctxtlgrlprValue = String.Format("{0:0,0.00}", rtxtlgrlprValue);
                MB_Bond.LPRValue = Convert.ToString(MIPD.premium_liability_value);
                MB_Bond.LPRGovtRebate = Convert.ToString(MIPD.liability_gov_discount);
                MB_Bond.LPRGovtRebateValue = ctxtlgrlprValue;
                //MB_Bond.txtlgrlprValue = ctxtlgrlprValue;
                decimal txtsubtotlprValue = rLPRvalue - rtxtlgrlprValue;
                int rtxtsubtotlprValue = Convert.ToInt32(txtsubtotlprValue);
                string ctxtsubtotlprValue = String.Format("{0:0,0.00}", rtxtsubtotlprValue);
                MB_Bond.LPRSubTotal1 = ctxtsubtotlprValue;
                //MB_Bond.txtsubtotlprValue = ctxtsubtotlprValue;
                decimal LPRDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    LPRDrvingInstitutionValue = ((Convert.ToDecimal(rtxtsubtotlprValue) / 100) * 60);
                    MB_Bond.LPRDrivingInstitutionPercentage = "60";
                    MB_Bond.LPRDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", LPRDrvingInstitutionValue); ;
                }
                decimal txtcngamntrValue = Convert.ToDecimal(MIPD.value_of_cng_lpg_amount);
                int rtxtcngamntrValue = Convert.ToInt32(txtcngamntrValue);
                int txtlpgkitlprValue = 0;
                if (rtxtcngamntrValue != 0)
                {
                    txtlpgkitlprValue = 60;
                    MB_Bond.LPRLPGKitValue = String.Format("{0:0,0.00}", txtlpgkitlprValue);
                }
                int rtxtsubtotlpglprValue = rtxtsubtotlprValue + txtlpgkitlprValue+Convert.ToInt32(LPRDrvingInstitutionValue);
                string crtxtsubtotlpglprValue = String.Format("{0:0,0.00}", rtxtsubtotlpglprValue);
                MB_Bond.LPRSubTotal2 = crtxtsubtotlpglprValue;
                decimal DriverAmount = Convert.ToDecimal(MIPD.driver_amount);
                string cDriverAmount = String.Format("{0:0,0.00}", DriverAmount);
                MB_Bond.DriverRisk = cDriverAmount;
                //MB_Bond.res10 = cDriverAmount;
                decimal PassengerAmount = 0;
                string cPassengerAmount = string.Empty;
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    PassengerAmount = Convert.ToDecimal(MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else if (MIPD.vehiclecategory == 17)
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver - 1) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    MB_Bond.PillionRisk = cPassengerAmount;
                }
                else
                {
                    MB_Bond.PassengersRisk = cPassengerAmount;
                }
                //MB_Bond.PassengersRisk = cPassengerAmount;
                //MB_Bond.res11 = cPassengerAmount;
                decimal txtlprtotValue = (rtxtsubtotlpglprValue + DriverAmount + PassengerAmount);
                int rtxtlprtotValue = Convert.ToInt32(txtlprtotValue);
                string ctxtlprtotValue = String.Format("{0:0,0.00}", rtxtlprtotValue);
                MB_Bond.LPRTotal = ctxtlprtotValue;
                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = 0 + rtxtlprtotValue;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = string.Empty;
                MB_Bond.TotalB = ctxtlprtotValue;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            else if (MIPD.type_of_cover == 3)
            {
                //A. Own Damage
                MB_Bond.BP = Convert.ToString(MIPD.mia_own_damage_additional_value);
                if (MB_Bond.BP == "0")
                {
                    MB_Bond.BP = "";
                }
                MB_Bond.ODPercentage = Convert.ToString(MIPD.own_damage_value);
                decimal odvalue = Convert.ToDecimal(MIPD.own_damage_value);
                decimal res = ((total_amount / 100) * (odvalue));
                int rres = Convert.ToInt32(res);
                decimal odsubtot = Convert.ToDecimal(MIPD.mia_own_damage_additional_value) + rres;
                int rodsubtot = Convert.ToInt32(odsubtot);
                string cres = String.Format("{0:0,0.00}", rodsubtot);
                MB_Bond.ODPercentageValue = String.Format("{0:0,0.00}", rres);
                MB_Bond.ODSubTotal1 = cres;
                MB_Bond.ODPremium = cres;
                MB_Bond.GVWExtraAmount = cGrossVehicleWeight;
                //MB_Bond.txtbpidvValue = cres;
                decimal res1 = ((odsubtot / 100) * Convert.ToDecimal(MIPD.od_gov_discount));
                int rres1 = Convert.ToInt32(res1);
                string cres1 = String.Format("{0:0,0.00}", rres1);
                MB_Bond.ODGovtRebate = Convert.ToString(MIPD.od_gov_discount);
                MB_Bond.ODGovtRebateValue = cres1;
                //MB_Bond.txtlgrodValue = cres1;
                decimal txtrebatetotodvalue = rodsubtot - rres1 + rGrossVehicleWeight;
                int rtxtrebatetotodvalue = Convert.ToInt32(txtrebatetotodvalue);
                string ctxtrebatetotodvalue = String.Format("{0:0,0.00}", rtxtrebatetotodvalue);
                MB_Bond.ODSubTotal2 = ctxtrebatetotodvalue;
                decimal Addele = ((Convert.ToDecimal(MIPD.electrical_accessories_amount) / 100) * 4);
                decimal Addlpg = ((Convert.ToDecimal(MIPD.value_of_cng_lpg_amount) / 100) * 4);
                decimal FiberGlassValue = 0;
                if (MIPD.isfiberglassfitted == true)
                {
                    if (MIPD.vehiclecategory == 16)
                    {
                        FiberGlassValue = 100;
                    }
                    else
                    {
                        FiberGlassValue = 50;
                    }
                    MB_Bond.FibreGlassFuelTank = String.Format("{0:0,0.00}", FiberGlassValue); ;
                }
                decimal ODDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    ODDrvingInstitutionValue = ((Convert.ToDecimal(txtrebatetotodvalue) / 100) * 60);
                    MB_Bond.ODDrivingInstitutionPercentage = "60";
                    MB_Bond.ODDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", ODDrvingInstitutionValue); ;
                }
                decimal Addnonele = ((Convert.ToDecimal(MIPD.non_electrical_accessories_amount) / 100) * 4);
                decimal ODAddSubtot = txtrebatetotodvalue + Addele + Addlpg + FiberGlassValue + ODDrvingInstitutionValue + Addnonele;
                int rODAddSubtot = Convert.ToInt32(ODAddSubtot);
                MB_Bond.EAPercentage = "4";
                MB_Bond.EAPercentageValue = String.Format("{0:0,0.00}", Addele);
                MB_Bond.ODLPGKitPercentage = "4";
                MB_Bond.ODLPGKitValue = String.Format("{0:0,0.00}", Addlpg);
                MB_Bond.ODAddOtherPercentage = "4";
                MB_Bond.ODAddOtherPercentageValue = String.Format("{0:0,0.00}", Addnonele);
                MB_Bond.ODSubTotal3 = String.Format("{0:0,0.00}", rODAddSubtot);
                decimal AutoMobileValue = 0;
                if (MIPD.vehicletype == 1)
                {
                    if (MIPD.isautomobileassociation == true)
                    {
                        AutoMobileValue = ((rODAddSubtot / 100) * 5);
                        MB_Bond.AutomobilePercentage = "5";
                        MB_Bond.AutomobileValue = String.Format("{0:0,0.00}", AutoMobileValue); ;
                    }
                }
                decimal ODLessSubtot = ODAddSubtot - AutoMobileValue;
                int rODLessSubtot = Convert.ToInt32(ODLessSubtot);
                MB_Bond.ODSubTotal4 = String.Format("{0:0,0.00}", rODLessSubtot);

                //MB_Bond.ODMalusValue=
                MB_Bond.ODMalus = MIPD.ph_malus_value.ToString();

                decimal txtlessmalusValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtlessmalusValue = Convert.ToInt32(txtlessmalusValue);
                string ctxtlessmalusValue = String.Format("{0:0,0.00}", rtxtlessmalusValue);
                MB_Bond.ODMalusValue = ctxtlessmalusValue;

                MB_Bond.ODNCB = MIPD.ph_ncb_value.ToString();

                //MB_Bond.txtrebatetotodvalue = ctxtrebatetotodvalue;
                decimal txtlessncbValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_ncb_value));
                int rtxtlessncbValue = Convert.ToInt32(txtlessncbValue);
                string ctxtlessncbValue = String.Format("{0:0,0.00}", rtxtlessncbValue);
                MB_Bond.ODNCBValue = ctxtlessncbValue;

                //MB_Bond.txtrebatetotodvalue = ctxtlessncbValue;
                decimal txtodtotValue = (rODLessSubtot + rtxtlessmalusValue) - rtxtlessncbValue;
                int rtxtodtotValue = Convert.ToInt32(txtodtotValue);
                decimal sidecardiscount = 0;
                if (Convert.ToDecimal(MIPD.side_car_trailer_amount) > 1)
                {
                    sidecardiscount = ((rtxtodtotValue / 100) * 25);
                }
                decimal odafterSDdiscount = rtxtodtotValue - sidecardiscount;
                int rodafterSDdiscount = Convert.ToInt32(odafterSDdiscount);
                string ctxtodtotValue = String.Format("{0:0,0.00}", rodafterSDdiscount);
                MB_Bond.ODOthersPercentage = "25";
                MB_Bond.ODOthersValue = String.Format("{0:0,0.00}", sidecardiscount);
                MB_Bond.ODTotal = ctxtodtotValue;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                decimal LPRvalue = Convert.ToDecimal(MIPD.premium_liability_value);
                int rLPRvalue = Convert.ToInt32(LPRvalue);
                decimal txtlgrlprValue = ((LPRvalue / 100) * Convert.ToDecimal(MIPD.liability_gov_discount));
                int rtxtlgrlprValue = Convert.ToInt32(txtlgrlprValue);
                string ctxtlgrlprValue = String.Format("{0:0,0.00}", rtxtlgrlprValue);
                MB_Bond.LPRValue = Convert.ToString(MIPD.premium_liability_value);
                MB_Bond.LPRGovtRebate = Convert.ToString(MIPD.liability_gov_discount);
                MB_Bond.LPRGovtRebateValue = ctxtlgrlprValue;
                //MB_Bond.txtlgrlprValue = ctxtlgrlprValue;
                decimal txtsubtotlprValue = rLPRvalue - rtxtlgrlprValue;
                int rtxtsubtotlprValue = Convert.ToInt32(txtsubtotlprValue);
                string ctxtsubtotlprValue = String.Format("{0:0,0.00}", rtxtsubtotlprValue);
                MB_Bond.LPRSubTotal1 = ctxtsubtotlprValue;
                //MB_Bond.txtsubtotlprValue = ctxtsubtotlprValue;
                decimal txtcngamntrValue = Convert.ToDecimal(MIPD.value_of_cng_lpg_amount);
                int rtxtcngamntrValue = Convert.ToInt32(txtcngamntrValue);
                int txtlpgkitlprValue = 0;
                if (rtxtcngamntrValue != 0)
                {
                    txtlpgkitlprValue = 60;
                    MB_Bond.LPRLPGKitValue = String.Format("{0:0,0.00}", txtlpgkitlprValue);
                }
                int rtxtsubtotlpglprValue = rtxtsubtotlprValue + txtlpgkitlprValue;
                string crtxtsubtotlpglprValue = String.Format("{0:0,0.00}", rtxtsubtotlpglprValue);
                MB_Bond.LPRSubTotal2 = crtxtsubtotlpglprValue;
                decimal DriverAmount = Convert.ToDecimal(MIPD.driver_amount);
                string cDriverAmount = String.Format("{0:0,0.00}", DriverAmount);
                MB_Bond.DriverRisk = cDriverAmount;
                //MB_Bond.res10 = cDriverAmount;
                decimal PassengerAmount = 0;
                string cPassengerAmount = string.Empty;
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    PassengerAmount = Convert.ToDecimal(MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else if (MIPD.vehiclecategory == 17)
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                else
                {
                    PassengerAmount = Convert.ToDecimal((MIPD.seating_capacity_including_driver - 1) * MIPD.passenger_amount);
                    cPassengerAmount = String.Format("{0:0,0.00}", PassengerAmount);
                }
                if (MIPD.vehiclecategory == 1 || MIPD.vehiclecategory == 2 || MIPD.vehiclecategory == 3 || MIPD.vehiclecategory == 4)
                {
                    MB_Bond.PillionRisk = cPassengerAmount;
                }
                else
                {
                    MB_Bond.PassengersRisk = cPassengerAmount;
                }
                //MB_Bond.PassengersRisk = cPassengerAmount;
                //MB_Bond.res11 = cPassengerAmount;
                decimal txtlprtotValue = (rtxtsubtotlpglprValue + DriverAmount + PassengerAmount);
                int rtxtlprtotValue = Convert.ToInt32(txtlprtotValue);
                string ctxtlprtotValue = String.Format("{0:0,0.00}", rtxtlprtotValue);
                MB_Bond.LPRTotal = ctxtlprtotValue;
                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = rodafterSDdiscount + rtxtlprtotValue;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = ctxtodtotValue;
                MB_Bond.TotalB = ctxtlprtotValue;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            else if (MIPD.type_of_cover == 4)
            {
                //A. Own Damage
                MB_Bond.BP = Convert.ToString(MIPD.mia_own_damage_additional_value);
                if (MB_Bond.BP == "0")
                {
                    MB_Bond.BP = "";
                }
                MB_Bond.ODPercentage = Convert.ToString(MIPD.own_damage_value);
                decimal odvalue = Convert.ToDecimal(MIPD.own_damage_value);
                decimal res = ((total_amount / 100) * (odvalue));
                int rres = Convert.ToInt32(res);
                decimal odsubtot = Convert.ToDecimal(MIPD.mia_own_damage_additional_value) + rres;
                int rodsubtot = Convert.ToInt32(odsubtot);
                string cres = String.Format("{0:0,0.00}", rodsubtot);
                MB_Bond.ODPercentageValue = String.Format("{0:0,0.00}", rres);
                MB_Bond.ODSubTotal1 = cres;
                MB_Bond.ODPremium = cres;
                MB_Bond.GVWExtraAmount = cGrossVehicleWeight;
                //MB_Bond.txtbpidvValue = cres;
                decimal res1 = ((odsubtot / 100) * Convert.ToDecimal(MIPD.od_gov_discount));
                int rres1 = Convert.ToInt32(res1);
                string cres1 = String.Format("{0:0,0.00}", rres1);
                MB_Bond.ODGovtRebate = Convert.ToString(MIPD.od_gov_discount);
                MB_Bond.ODGovtRebateValue = cres1;
                //MB_Bond.txtlgrodValue = cres1;
                decimal txtrebatetotodvalue = rodsubtot - rres1 + rGrossVehicleWeight;
                int rtxtrebatetotodvalue = Convert.ToInt32(txtrebatetotodvalue);
                string ctxtrebatetotodvalue = String.Format("{0:0,0.00}", rtxtrebatetotodvalue);
                MB_Bond.ODSubTotal2 = ctxtrebatetotodvalue;
                decimal Addele = ((Convert.ToDecimal(MIPD.electrical_accessories_amount) / 100) * 4);
                decimal Addlpg = ((Convert.ToDecimal(MIPD.value_of_cng_lpg_amount) / 100) * 4);
                decimal FiberGlassValue = 0;
                if (MIPD.isfiberglassfitted == true)
                {
                    if (MIPD.vehiclecategory == 16)
                    {
                        FiberGlassValue = 100;
                    }
                    else
                    {
                        FiberGlassValue = 50;
                    }
                    MB_Bond.FibreGlassFuelTank = String.Format("{0:0,0.00}", FiberGlassValue); ;
                }
                decimal ODDrvingInstitutionValue = 0;
                if (MIPD.isdrivinginstitution == true)
                {
                    ODDrvingInstitutionValue = ((Convert.ToDecimal(txtrebatetotodvalue) / 100) * 60);
                    MB_Bond.ODDrivingInstitutionPercentage = "60";
                    MB_Bond.ODDrivingInstitutionPercentageValue = String.Format("{0:0,0.00}", ODDrvingInstitutionValue); ;
                }
                decimal Addnonele = ((Convert.ToDecimal(MIPD.non_electrical_accessories_amount) / 100) * 4);
                decimal ODAddSubtot = txtrebatetotodvalue + Addele + Addlpg + FiberGlassValue + ODDrvingInstitutionValue + Addnonele;
                int rODAddSubtot = Convert.ToInt32(ODAddSubtot);
                MB_Bond.EAPercentage = "4";
                MB_Bond.EAPercentageValue = String.Format("{0:0,0.00}", Addele);
                MB_Bond.ODLPGKitPercentage = "4";
                MB_Bond.ODLPGKitValue = String.Format("{0:0,0.00}", Addlpg);
                MB_Bond.ODAddOtherPercentage = "4";
                MB_Bond.ODAddOtherPercentageValue = String.Format("{0:0,0.00}", Addnonele);
                MB_Bond.ODSubTotal3 = String.Format("{0:0,0.00}", rODAddSubtot);
                decimal AutoMobileValue = 0;
                if (MIPD.vehicletype == 1)
                {
                    if (MIPD.isautomobileassociation == true)
                    {
                        AutoMobileValue = ((rODAddSubtot / 100) * 5);
                        MB_Bond.AutomobilePercentage = "5";
                        MB_Bond.AutomobileValue = String.Format("{0:0,0.00}", AutoMobileValue); ;
                    }
                }
                decimal ODLessSubtot = ODAddSubtot - AutoMobileValue;
                int rODLessSubtot = Convert.ToInt32(ODLessSubtot);
                MB_Bond.ODSubTotal4 = String.Format("{0:0,0.00}", rODLessSubtot);

                //MB_Bond.ODMalusValue=
                MB_Bond.ODMalus = MIPD.ph_malus_value.ToString();

                decimal txtlessmalusValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_malus_value));
                int rtxtlessmalusValue = Convert.ToInt32(txtlessmalusValue);
                string ctxtlessmalusValue = String.Format("{0:0,0.00}", rtxtlessmalusValue);
                MB_Bond.ODMalusValue = ctxtlessmalusValue;

                MB_Bond.ODNCB = MIPD.ph_ncb_value.ToString();

                //MB_Bond.txtrebatetotodvalue = ctxtrebatetotodvalue;
                decimal txtlessncbValue = ((rODLessSubtot / 100) * Convert.ToInt32(MIPD.ph_ncb_value));
                int rtxtlessncbValue = Convert.ToInt32(txtlessncbValue);
                string ctxtlessncbValue = String.Format("{0:0,0.00}", rtxtlessncbValue);
                MB_Bond.ODNCBValue = ctxtlessncbValue;

                //MB_Bond.txtrebatetotodvalue = ctxtlessncbValue;
                decimal txtodtotValue = (rODLessSubtot + rtxtlessmalusValue) - rtxtlessncbValue;
                int rtxtodtotValue = Convert.ToInt32(txtodtotValue);
                decimal sidecardiscount = 0;
                if (Convert.ToDecimal(MIPD.side_car_trailer_amount) > 1)
                {
                    sidecardiscount = ((rtxtodtotValue / 100) * 25);
                }
                decimal odafterSDdiscount = rtxtodtotValue - sidecardiscount;
                int rodafterSDdiscount = Convert.ToInt32(odafterSDdiscount);
                string ctxtodtotValue = String.Format("{0:0,0.00}", rodafterSDdiscount);
                MB_Bond.ODOthersPercentage = "25";
                MB_Bond.ODOthersValue = String.Format("{0:0,0.00}", sidecardiscount);
                MB_Bond.ODTotal = ctxtodtotValue;
                //MB_Bond.txtodtotValue = ctxtodtotValue;

                //B. LIABILITY TO PUBLIC RISK
                MB_Bond.LPRTotal = string.Empty;

                //MB_Bond.txtlprtotValue = ctxtlprtotValue;
                decimal txttotABValue = odafterSDdiscount + 0;
                int rtxttotABValue = Convert.ToInt32(txttotABValue);
                string ctxttotABValue = String.Format("{0:0,0.00}", rtxttotABValue);
                MB_Bond.TotalA = ctxtodtotValue;
                MB_Bond.TotalB = string.Empty;
                MB_Bond.TotalAB = ctxttotABValue;
                MB_Bond.Premium = ctxttotABValue;
                //MB_Bond.txttotABValue = ctxttotABValue;
                decimal txtsgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtsgstamtValue = Convert.ToInt32(txtsgstamtValue);
                string ctxtsgstamtValue = String.Format("{0:0,0.00}", rtxtsgstamtValue);
                MB_Bond.CGSTofPremium = ctxtsgstamtValue;
                decimal txtcgstamtValue = (Convert.ToDecimal(rtxttotABValue) / 100) * 9;
                int rtxtcgstamtValue = Convert.ToInt32(txtcgstamtValue);
                string ctxtcgstamtValue = String.Format("{0:0,0.00}", rtxtcgstamtValue);
                MB_Bond.SGSTofPremium = ctxtcgstamtValue;
                //MB_Bond.txtgstamtValue = ctxtgstamtValue;
                decimal txttotalcrpremiumValue = rtxttotABValue + rtxtsgstamtValue + rtxtcgstamtValue;
                int rtxttotalcrpremiumValue = Convert.ToInt32(txttotalcrpremiumValue);
                string ctxttotalcrpremiumValue = String.Format("{0:0,0.00}", rtxttotalcrpremiumValue);
                MB_Bond.FinalAmount = ctxttotalcrpremiumValue;
                MB_Bond.PayablePremium = ctxttotalcrpremiumValue;
                //MB_Bond.txttotalcrpremiumValue = ctxttotalcrpremiumValue;
            }
            return MB_Bond;
        }
        #endregion
        #region MI BOND Generatation From UI
        public string MIGetFileForSigning(RequestFile requestFile)
        {
            Image_convert_model _file_obj = new Image_convert_model();
            byte[] binFile = null;
            try
            {
                string AppID = requestFile.RefID;
                string EmpID = requestFile.RefType;
                string UnSignedBond = _IMIApplicationbll.GetMBBondDocFileBLL(Convert.ToInt64(AppID), Convert.ToInt64(EmpID));
                //string filename = System.Web.Hosting.HostingEnvironment.MapPath("~/PdfTemplate/NBBond/NB_BOND_Template.pdf");
                string pdfFilePath = UnSignedBond;
                byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);

                string strBytes = Convert.ToBase64String(bytes);

                _file_obj = new Image_convert_model
                {
                    File_Name = "Sample.pdf",
                    File_bytes = strBytes,
                    File_token = "",
                    RefID = AppID,
                    RefType = "1",
                    DSC_user_name = ""
                };

                return _file_obj.File_bytes;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                //return Request.CreateResponse(HttpStatusCode.OK, _responce_model, Configuration.Formatters.JsonFormatter);
            }
            return "";
        }
        public string MIUploadSignedFile(Image_convert_model _Model)
        {
            File_Responce_model _responce_model = new File_Responce_model();
            try
            {
                //_Model.File_Path = Server.MapPath("~/PdfTemplate/MIBond/");
                //_Model.File_Path = @"C:/Documents/PdfTemplate/NBBond/";
                if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                {
                    _Model.File_Path = WebConfigurationManager.AppSettings["RootDirectory"] + @"MIBOND\";
                }
                if (_Model.File_Name != "" && _Model.File_bytes != "")
                {
                    //string serverFileName = GenerateUniqueCode(5);
                    string serverFileName = _Model.RefID;
                    //string filePathSigned = @"C:/Documents/PdfTemplate/NBBond/" + serverFileName  + "_Signed.pdf";
                    //string filePathSigned = string.Empty;
                    //if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    //{
                    //    filePathSigned = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\MIBond\";
                    //}
                    byte[] imageBytes = Convert.FromBase64String(_Model.File_bytes);
                    string FileName = serverFileName + "_Signed.pdf";
                    string path = _Model.File_Path;
                    string imgPath = Path.Combine(path, FileName);
                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }
                    System.IO.File.WriteAllBytes(imgPath, imageBytes);


                    _responce_model.Status = true;
                    _responce_model.Message = "success";
                    _responce_model.return_reponce = "File Upload successfully.";
                    string result = string.Empty;
                    //string signedfilepath = Server.MapPath(filePathSigned);
                    string signedfilepath = imgPath;
                    result = _IMIApplicationbll.MBSignBondUploadBLL(Convert.ToInt64(_Model.RefID), Convert.ToInt64(_Model.RefType), signedfilepath);
                }
                else
                {
                    _responce_model.Status = false;
                    _responce_model.Message = "failed";
                    _responce_model.return_reponce = "File unble to upload.";
                }

                return "true";
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                return "false";
            }
        }
        #endregion

        #region Motor Insurance D
        [Route("DPropDetails")]
        public ActionResult MIDVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;
                //Session["SelectedCategory"] = category;
                //Session["Categories"] = category;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("SaveDMIVData")]
        public ActionResult InsertVerifyDetailsD(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            //bool isSuccess = false;
            //string message = string.Empty;
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            //if (Convert.ToInt32(result) == 1)
            //{
            //    TempData["VerifyDetails"] = objVerifyDetails;
            //}
            //return RedirectToAction("DetailsForDVerification", "MotorInsurance", new { area = "" });
            return RedirectToAction("DetailsForDVerification", "MotorInsurance", new { @Category = objVerifyDetails.Category });
            //return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [Route("VIEW-MB-BD")]
        public ActionResult ViewMBBondDetails()
        {
            VM_DDOVerificationDetailsMI verifydetails = _IMIApplicationbll.GetMBBondDetailsBll(Convert.ToInt64(Session["UId"]));

            return View(verifydetails);
        }
        [Route("mi-r-vw-apln/{empId}/{applicationId}/{refNo}/{category}/{PrevRefNo?}")]
        public ActionResult MIViewRenewalApplication(long empId, long applicationId, long refNo, int category)
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {
                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetRenewalWorkFlowDetails(Convert.ToInt64(applicationId), Convert.ToInt32(category));
                Session["RUID"] = empId;

            }
            return View(verificationDetails);
        }
        #region Motor Insurance Renewal Workflow
        [Route("mi_rcw_va/{empId}/{applicationId}/{refNo}/{category}/{PrevRefNo?}")]
        public ActionResult MICWRenewalVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetRenewalWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;
                //Session["SelectedCategory"] = category;
                //Session["Categories"] = category;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("RSaveCWMIVData")]
        public string InsertVerifyRenewalDetailsCW(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveRenewalVerifiedDetailsBll(objVerifyDetails);
            //if (Convert.ToInt32(result) == 1)
            //{
            //    TempData["VerifyDetails"] = objVerifyDetails;
            //}
            //return RedirectToAction("RenewalDetailsForCWVerification", "MotorInsurance", new { area = "" });
            //return RedirectToAction("RenewalDetailsForCWVerification", "MotorInsurance", new { @Category = objVerifyDetails.Category });
            return result;
        }
        [Route("mi_rsi_va/{empId}/{applicationId}/{refNo}/{category}/{PrevRefNo?}")]
        public ActionResult MISuperintendentRenewalVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetRenewalWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;
                //Session["SelectedCategory"] = category;
                //Session["Categories"] = category;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("RSaveSIMIVData")]
        public string InsertVerifyRenewalDetailsSuperintendent(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveRenewalVerifiedDetailsBll(objVerifyDetails);
            if (Convert.ToInt32(result) == 1)
            {

            }

           // TempData["VerifyDetails"] = objVerifyDetails;
            ViewBag.VerifyDetails = objVerifyDetails;
            //return RedirectToAction("DetailsForSuperintendentVerification", "MotorInsurance", new { area = "" });
            //RedirectToAction("DetailsForSuperintendentVerification", "MotorInsurance", new { @Category = objVerifyDetails.Category });

            return result;
        }
        [Route("mi_rad_va/{empId}/{applicationId}/{refNo}/{category}/{PrevRefNo?}")]
        public ActionResult MIADRenewalVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetRenewalWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;
                //Session["SelectedCategory"] = category;
                //Session["Categories"] = category;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("RSaveADMIVData")]
        public ActionResult InsertVerifyRenewalDetailsAD(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveRenewalVerifiedDetailsBll(objVerifyDetails);
            if (objVerifyDetails.ApplicationStatus != 15 && Convert.ToInt32(result) > 0)
            {
                return Json(new { PolicyNumber = string.Empty, RedirectUrl = "/mi-ad-radt" }, JsonRequestBehavior.AllowGet);
            }
            else if (objVerifyDetails.ApplicationStatus == 15 && !string.IsNullOrEmpty(result))
            {
                //CREATE AND SAVE MB BOND
                string MB_Result = string.Empty;
                VM_MotorInsurancePolicyPrintDetails MIPD = _IMIApplicationbll.MIPolicyPrintDetailsBll("Renewal", objVerifyDetails.EmpCode, objVerifyDetails.ApplicationRefNo);//Type,EmpID,AppRefNo
                VM_MB_Bond_Details _MB_Bond = new VM_MB_Bond_Details();
                _MB_Bond = GetMBBondCalculationDetails(MIPD);
                string MIBondPath = FillFormMB(_MB_Bond, result);
                //Save Bond
                MB_Result = _IMIApplicationbll.MBBondDocUploadBLL(objVerifyDetails.ApplicationRefNo, objVerifyDetails.EmpCode, MIBondPath, "Renewal");//AppRefNo,EmpID,DocPath,Type
                return Json(new { PolicyNumber = result, RedirectUrl = "/mi-ad-radt" }, JsonRequestBehavior.AllowGet);

                //              string Month = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).AddMonths(1).ToString("MMMM");
                //              string Year = DateTime.Parse(_empNBBond.EmployeeBasicDetails.p_sanction_date).ToString("yy");
                //              var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == caseWorkerVerifiedDetails.EmpCode select eb).FirstOrDefault();
                //              string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + facingSheetaprvd.ApplicationNumber + " ಯು ಅಂಗೀಕಾರವಾಗಿದ್ದು, ನಿಮ್ಮ ಪಾಲಿಸಿ ಸಂಖ್ಯೆ " + returnString + " ಆಗಿರುತ್ತದೆ.ಪಾಲಿಸಿ ಬಾಂಡ್‌ ಮತ್ತು ಅಂಗೀಕಾರದ ಸೂಚನೆಗಳನ್ನು ನಿಮ್ಮ ರಿಜಿಸ್ಟರ್ಡ್‌ ಇ-ಮೇಲ್‌ / KGID ಲಾಗಿನ್ ನಲ್ಲಿ ಡೌನ್‌ಲೋಡ್‌ ಮಾಡಿಕೊಳ್ಳಬಹುದಾಗಿದೆ. ಮುಂದುವರೆದು, ರೂ " + _empNBBond.EmployeeBasicDetails.p_premium + " ಗಳನ್ನು " + Month + "/" + Year + " ತಿಂಗಳು / ವರ್ಷ ದಿಂದ ಕ್ರಮವಾಗಿ ತಮ್ಮ ವೇತನದಿಂದ ಕಟಾವಣೆ ಮಾಡಿಸತಕ್ಕದ್ದು."
                //                       + " - ವಿಮಾ ಇಲಾಖೆ(KGID).";
                //              //AllCommon.sendOTPMSG(mobile.mobile_number.ToString(), msg);
                //              string emailmsg = "Dear Insured," + "\r\n"
                //+ "We are glad to Inform you that, your proposal with reference no " + facingSheetaprvd.ApplicationNumber + " has been accepted and KGID Policy bond has been generated on " + _empNBBond.EmployeeBasicDetails.p_sanction_date + "\r\n"
                //+ " The Policy details are as follows: KGID Policy No " + returnString
                //+ " Please find the KGID Policy Bond and the acceptance notice attached."
                //+ "This document is digitally signed ,hence does not require physical signature."
                //+ " Warm Regards,"
                //+ "KGID, Official Branch";
                //              AllCommon objemail = new AllCommon();
                //              // objemail.SendEmail(mobile.email_id, emailmsg, "KGID Policy Bond.");

                //return Json(new { PolicyNumber = result, RedirectUrl = "/mi-dd-dpt/" + objVerifyDetails.Category }, JsonRequestBehavior.AllowGet);
                ////return Json(new { PolicyNumber = returnString, UnSignedBond = filepath, AppId = PolicyNo, EmpId = EmployeeId, RedirectUrl = "/kgid-d/" }, JsonRequestBehavior.AllowGet);            }
            }
            return null;
            //return RedirectToAction("RenewalDetailsForADVerification", "MotorInsurance", new { @Category = objVerifyDetails.Category });
        }
        [Route("mi_rdd_va/{empId}/{applicationId}/{refNo}/{category}/{PrevRefNo?}")]
        public ActionResult MIDDRenewalVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetRenewalWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;
                //Session["SelectedCategory"] = category;
                //Session["Categories"] = category;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("RSaveDDMIVData")]
        public ActionResult InsertVerifyRenewalDetailsDD(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveRenewalVerifiedDetailsBll(objVerifyDetails);
            //if (Convert.ToInt32(result) == 1)
            //{
            //    TempData["VerifyDetails"] = objVerifyDetails;
            //}
            //return RedirectToAction("RenewalDetailsForDDVerification", "MotorInsurance", new { area = "" });
            return RedirectToAction("RenewalDetailsForDDVerification", "MotorInsurance", new { @Category = objVerifyDetails.Category });
        }
        [Route("RDPropDetails")]
        public ActionResult MIDRenewalVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MotorInsuranceDeptVerficationDetails verificationDetails = new VM_MotorInsuranceDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails.WorkFlowDetails = _IMIApplicationbll.GetRenewalWorkFlowDetails(applicationId, category);
                Session["RUID"] = empId;
                //Session["SelectedCategory"] = category;
                //Session["Categories"] = category;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("RSaveDMIVData")]
        public ActionResult InsertVerifyRenewalDetailsD(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMIApplicationbll.SaveRenewalVerifiedDetailsBll(objVerifyDetails);
            //if (Convert.ToInt32(result) == 1)
            //{
            //    TempData["VerifyDetails"] = objVerifyDetails;
            //}
            //return RedirectToAction("RenewalDetailsForDVerification", "MotorInsurance", new { area = "" });
            return RedirectToAction("RenewalDetailsForDVerification", "MotorInsurance", new { @Category = objVerifyDetails.Category });
        }
        #endregion
        #region Convert Number into Words
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += NumberToWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }
            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " Lakh ";
                number %= 100000;
            }
            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }
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
        #endregion

        #region PolicyCancellationDetails
        [Route("kgid-c-req")]
        public ActionResult PolicyCancellationDetails(string Type = "")
        {
            VM_PolicyCancellationDetails obj = new VM_PolicyCancellationDetails();
            if (Session["UID"] != null)
            {
                obj = _Objemployee.GetPolicyCancellationDetails(Convert.ToInt64(Session["UID"]), Type);
            }
            return View(obj);
        }

        public int NBAppCancelRequestAction(long AppId, long EmpId, string Action)
        {
            int result = _Objemployee.NBAppCancelRequestAction(AppId, EmpId, Action);
            return result;
        }

        #endregion
        public JsonResult WordDocPdf(VM_NBBond _empNBBond, string PolicyNumber)
        {
            PolicyNumber = "BNG17042021"; 
            int PolicyNo = 40496;
            _empNBBond = _Objemployee.getNBBondDetails(PolicyNo);
            string KeyPoints = string.Empty;
            if (_empNBBond.EmployeeBasicDetails.LoadFactor == "" && _empNBBond.EmployeeBasicDetails.DLFactor == "")
            {
                _empNBBond.EmployeeBasicDetails.LoadFactor = "OR";
                KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor;
            }
            if (_empNBBond.EmployeeBasicDetails.LoadFactor != "" && _empNBBond.EmployeeBasicDetails.DLFactor == "")
            {
                KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor;
                //_empNBBond.EmployeeBasicDetails.DLFactor = "none";
            }
            if (_empNBBond.EmployeeBasicDetails.LoadFactor == "" && _empNBBond.EmployeeBasicDetails.DLFactor != "")
            {
                _empNBBond.EmployeeBasicDetails.LoadFactor = "OR";
                KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor + " & " + "DL Factor: " + _empNBBond.EmployeeBasicDetails.DLFactor;
            }
            if (_empNBBond.EmployeeBasicDetails.LoadFactor != "" && _empNBBond.EmployeeBasicDetails.DLFactor != "")
            {
                KeyPoints = "Load Factor: " + _empNBBond.EmployeeBasicDetails.LoadFactor + " & " + "DL Factor: " + _empNBBond.EmployeeBasicDetails.DLFactor;
            }
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//KeyPoints:- " + KeyPoints);
            string NameDesignation = string.Empty;
            string SumAssuredWords = string.Empty;
            int Age; int year; int finalyear;
            string month = string.Empty;
            string DueDate = string.Empty;
            string finalpayment = string.Empty;
            string EndMonthYear = string.Empty;
            string ApprovedMonth = string.Empty;
            string ApprovedYear = string.Empty;
            try
            {
                NameDesignation = _empNBBond.EmployeeBasicDetails.employee_name_kannada.ToString() + ", " + _empNBBond.EmployeeBasicDetails.designation.ToString().Trim().ToUpper() + ", " + _empNBBond.EmployeeBasicDetails.dm_ddo_office.ToString().Trim().ToUpper();
                SumAssuredWords = ConvertToWords(_empNBBond.EmployeeBasicDetails.p_total_sum_assured);
                Age = new DateTime((DateTime.Now - _empNBBond.EmployeeBasicDetails.emp_date_of_birth).Ticks).Year;
                year = _empNBBond.EmployeeBasicDetails.emp_date_of_birth.Year;
                finalyear = year + 55;
                month = _empNBBond.EmployeeBasicDetails.emp_date_of_birth.ToString("MMMM");
                DueDate = _empNBBond.EmployeeBasicDetails.DueDate;
                finalpayment = _empNBBond.EmployeeBasicDetails.FinalPayment;
                //string EndMonth = _empNBBond.EmployeeBasicDetails.emp_date_of_birth.AddMonths(-1).ToString("MMMM");
                EndMonthYear = _empNBBond.EmployeeBasicDetails.EndMonthYear;
                ApprovedMonth = _empNBBond.EmployeeBasicDetails.ApprovedMonth;
                ApprovedYear = _empNBBond.EmployeeBasicDetails.ApprovedYear;
            }
            catch (Exception e)
            {
                Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//Basic Details Error:- " + e.Message);
            }
            /////------------
            string DocTemplate = Server.MapPath("~/PdfTemplate/NBBond/NBBond_Kannada_Doc_Template.docx");
            //string pdfTemplate = Server.MapPath("~/PdfTemplate/NBBond/NB_BOND_Template_Kannada.pdf");
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//Word Doc Template Path:- " + DocTemplate);
            try
            {
                Microsoft.Office.Interop.Word.Application word1 = new Microsoft.Office.Interop.Word.Application();
                Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//Word Doc Template Opening:- ");
                Microsoft.Office.Interop.Word.Document doc1 = word1.Documents.Open(DocTemplate);
                //doc.Activate();
                Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//Word Doc Template Opened:- ");
            }
            catch (Exception e)
            {
                Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//Word Doc Template Opened Error:- " + e.Message);
            }
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = word.Documents.Open(DocTemplate);
            ///--------------
            try
            {
                doc.FormFields["DIOAddress"].Result = _empNBBond.EmployeeBasicDetails.DIO_Office_Address.ToString();
                doc.FormFields["MEDate"].Result = _empNBBond.EmployeeBasicDetails.App_Creation_Date.ToString().Trim().ToUpper();
                doc.FormFields["NameDesignation"].Result = NameDesignation;
                doc.FormFields["FatherName"].Result = _empNBBond.EmployeeBasicDetails.father_name_kannada;
                doc.FormFields["DateofProposal"].Result = _empNBBond.EmployeeBasicDetails.ApplicationSubmitedDate.ToString();
                doc.FormFields["PolicyNumber"].Result = _empNBBond.EmployeeBasicDetails.p_kgid_policy_number.ToString();
                doc.FormFields["DateofRisk"].Result = _empNBBond.EmployeeBasicDetails.p_sanction_date.ToString();
                doc.FormFields["AdmittedDOB"].Result = _empNBBond.EmployeeBasicDetails.date_of_birth.ToString();
                doc.FormFields["MonthlyPremium"].Result = _empNBBond.EmployeeBasicDetails.p_premium.ToString();
                doc.FormFields["SumAssured"].Result = _empNBBond.EmployeeBasicDetails.p_total_sum_assured.ToString();
                doc.FormFields["MonthofFinalPayment"].Result = finalpayment.ToString();
                doc.FormFields["SumAssuredWords"].Result = SumAssuredWords;
                doc.FormFields["EndMonthYear"].Result = EndMonthYear;
                doc.FormFields["PRiskMonth"].Result = ApprovedMonth;
                doc.FormFields["PRiskYear"].Result = ApprovedYear;
                doc.FormFields["RiskDateYear"].Result = _empNBBond.EmployeeBasicDetails.p_sanction_date.ToString();
                doc.FormFields["CaseWorkerName"].Result = _empNBBond.EmployeeBasicDetails.CaseWorkerName.ToString().Trim().ToUpper();
                doc.FormFields["CWVerifiedDate"].Result = _empNBBond.EmployeeBasicDetails.CaseWorkerVerifiedDate.ToString();
                doc.FormFields["SuperintendentName"].Result = _empNBBond.EmployeeBasicDetails.SuperintendentName.ToString().Trim().ToUpper();
                doc.FormFields["SIVerifiedDate"].Result = _empNBBond.EmployeeBasicDetails.SuperintendentVerifiedDate;
                //doc.FormFields["ApprovedDate"].Result = _empNBBond.EmployeeBasicDetails.p_sanction_date.ToString();
                //doc.FormFields["ApprovedYear"].Result = ApprovedYear;
                doc.FormFields["SpecialRemarks"].Result = KeyPoints;
            }
            catch (Exception e)
            {
                Logger.LogMessage(TracingLevel.INFO, "//NB Bond Details Filling:- " + e.Message);
            }
            try
            {
                if (_empNBBond.NomineeDetailsList != null)
                {

                    if (_empNBBond.NomineeDetailsList.Count() == 1)
                    {//Nominee Details 1
                        doc.FormFields["NomineeName1"].Result = _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship1"].Result = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        doc.FormFields["NomineeAge1"].Result = _empNBBond.NomineeDetailsList[0].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian1"].Result = _empNBBond.NomineeDetailsList[0].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation1"].Result = _empNBBond.NomineeDetailsList[0].GaurdianRelation;


                        Logger.LogMessage(TracingLevel.INFO, "//Nominee Details 1 Filled" + _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString());
                    }

                    if (_empNBBond.NomineeDetailsList.Count() == 2)
                    {//Nominee Details 2
                        doc.FormFields["NomineeName1"].Result = _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship1"].Result = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        doc.FormFields["NomineeAge1"].Result = _empNBBond.NomineeDetailsList[0].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian1"].Result = _empNBBond.NomineeDetailsList[0].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation1"].Result = _empNBBond.NomineeDetailsList[0].GaurdianRelation;
                        //2
                        doc.FormFields["NomineeName2"].Result = _empNBBond.NomineeDetailsList[1].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship2"].Result = _empNBBond.NomineeDetailsList[1].Relation.ToString();
                        doc.FormFields["NomineeAge2"].Result = _empNBBond.NomineeDetailsList[1].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian2"].Result = _empNBBond.NomineeDetailsList[1].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation2"].Result = _empNBBond.NomineeDetailsList[1].GaurdianRelation;
                    }
                    if (_empNBBond.NomineeDetailsList.Count() == 3)
                    {//Nominee Details 3
                        doc.FormFields["NomineeName1"].Result = _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship1"].Result = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        doc.FormFields["NomineeAge1"].Result = _empNBBond.NomineeDetailsList[0].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian1"].Result = _empNBBond.NomineeDetailsList[0].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation1"].Result = _empNBBond.NomineeDetailsList[0].GaurdianRelation;
                        //2
                        doc.FormFields["NomineeName2"].Result = _empNBBond.NomineeDetailsList[1].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship2"].Result = _empNBBond.NomineeDetailsList[1].Relation.ToString();
                        doc.FormFields["NomineeAge2"].Result = _empNBBond.NomineeDetailsList[1].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian2"].Result = _empNBBond.NomineeDetailsList[1].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation2"].Result = _empNBBond.NomineeDetailsList[1].GaurdianRelation;
                        //3
                        doc.FormFields["NomineeName3"].Result = _empNBBond.NomineeDetailsList[2].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship3"].Result = _empNBBond.NomineeDetailsList[2].Relation.ToString();
                        doc.FormFields["NomineeAge3"].Result = _empNBBond.NomineeDetailsList[2].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian3"].Result = _empNBBond.NomineeDetailsList[2].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation3"].Result = _empNBBond.NomineeDetailsList[2].GaurdianRelation;
                    }
                    if (_empNBBond.NomineeDetailsList.Count() == 4)
                    {//Nominee Details 4
                        doc.FormFields["NomineeName1"].Result = _empNBBond.NomineeDetailsList[0].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship1"].Result = _empNBBond.NomineeDetailsList[0].Relation.ToString();
                        doc.FormFields["NomineeAge1"].Result = _empNBBond.NomineeDetailsList[0].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian1"].Result = _empNBBond.NomineeDetailsList[0].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation1"].Result = _empNBBond.NomineeDetailsList[0].GaurdianRelation;
                        //2
                        doc.FormFields["NomineeName2"].Result = _empNBBond.NomineeDetailsList[1].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship2"].Result = _empNBBond.NomineeDetailsList[1].Relation.ToString();
                        doc.FormFields["NomineeAge2"].Result = _empNBBond.NomineeDetailsList[1].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian2"].Result = _empNBBond.NomineeDetailsList[1].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation2"].Result = _empNBBond.NomineeDetailsList[1].GaurdianRelation;
                        //3
                        doc.FormFields["NomineeName3"].Result = _empNBBond.NomineeDetailsList[2].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship3"].Result = _empNBBond.NomineeDetailsList[2].Relation.ToString();
                        doc.FormFields["NomineeAge3"].Result = _empNBBond.NomineeDetailsList[2].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian3"].Result = _empNBBond.NomineeDetailsList[2].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation3"].Result = _empNBBond.NomineeDetailsList[2].GaurdianRelation;
                        //4
                        doc.FormFields["NomineeName4"].Result = _empNBBond.NomineeDetailsList[3].NameOfNominee.ToString();
                        doc.FormFields["NomineeRelationship4"].Result = _empNBBond.NomineeDetailsList[3].Relation.ToString();
                        doc.FormFields["NomineeAge4"].Result = _empNBBond.NomineeDetailsList[3].NomineeAge.ToString();
                        doc.FormFields["NomineeGuardian4"].Result = _empNBBond.NomineeDetailsList[3].NameOfGaurdian;
                        doc.FormFields["NomineeGRelation4"].Result = _empNBBond.NomineeDetailsList[3].GaurdianRelation;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogMessage(TracingLevel.INFO, "//NB Bond Details Nominee Filling:- " + e.Message);
            }
            string newDocFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newDocFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\NBBond\" + PolicyNumber + "UnSigned" + DateTime.Now.Ticks + ".docx";
            }
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//NB Word Doc Bond Path:- " + newDocFile);
            doc.SaveAs(newDocFile);
            doc.Close();
            word.Quit();
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//NB Word Doc Bond Created:- " + newDocFile);
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\NBBond\" + PolicyNumber + "UnSigned" + DateTime.Now.Ticks + ".pdf";
            }
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//NB Word PDF Bond Path:- " + newFile);
            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document wordDocument = appWord.Documents.Open(newDocFile);
            wordDocument.ExportAsFixedFormat(newFile, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            wordDocument.Close();
            appWord.Quit();
            Logger.LogMessage(TracingLevel.INFO, "FillFormNB()//NB Word PDF Bond Created:- " + newFile);
            //System.Diagnostics.Process.Start(@"C:\Documents\CDoc2.docx");
            return null;
        }
        [Route("kgid-dio-prop-form")]
        public ActionResult GetProposalFormDetails()
        {

            List<VM_PolicyGeneration> listPolicy = new List<VM_PolicyGeneration>();
            long UserID = 0;
            if (Session["UID"] != null)
            {
                UserID = Convert.ToInt32(Session["UID"]);
            }
            listPolicy = _INBApplicationbll.GetPolicyList(UserID);
            VM_proposalList obj = new VM_proposalList();
            obj.listDetails = listPolicy;
            return View(obj);
        }

        public JsonResult UpdatePolicyStatus(long applyid)
        {
            int result = _INBApplicationbll.SavePolicyStatus(applyid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
    }
}