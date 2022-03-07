using BLL.KGIDLoanBLL;
using BLL.NBLoanBLL;
using Common;
using KGID.Models;
using KGID_Models.KGID_Loan;
using KGID_Models.KGIDLoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static KGID.FilterConfig;

namespace KGID.Controllers
{
    public class LoanController : Controller
    {
        private readonly INBLoanBLL loanBll;
        private readonly ILoanApplicationBll loanApplicationBll;
        public LoanController()
        {
            loanApplicationBll = new LoanApplicationBll();
            loanBll = new NBLoanBLL();
        }
            // GET: Loan
        public ActionResult Index()
        {
            return View();
        }

        #region Loan Application Form
        [Route("kgid-loan-app")]
        [CustomAuthorize("Employee")]
        public ActionResult LoanApplicationForm()
        {
            VM_LoanApplicationForm loanApp = new VM_LoanApplicationForm();
            if (Session["UID"] != null)
            {
                long EmpID = (long)Session["UID"];
                loanApp = loanBll.GetLoanApplicationForm(EmpID);
                loanApp.IsSpouseGovtEmp = false;
                return View("LoanApplicationForm", loanApp);
            }
            return View("LoanApplicationForm", loanApp);
        }
        public ActionResult GetSpouseKgidNumber()
        {
            if (Session["UID"] != null)
            {
                long empId = (long)Session["UID"];
                string IsSpouseKgidAllow = loanBll.GetSpouseKgidNumber(empId);
                return Json(IsSpouseKgidAllow, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public int SaveLoanApplication(VM_LoanApplicationForm model)
        {
            int result = 0;
            if (Session["UID"] != null)
            {
                model.EmpID = (long)Session["UID"];
                result = loanBll.SaveLoanApplication(model);
            }
            return result;
        }
        public int UploadPayslip(VM_LoanApplicationForm model)
        {
            int result = 0;
            if (Session["UID"] != null)
            {
                model.EmpID = (long)Session["UID"];
                result = loanBll.UploadPayslip(model);
            }
            return result;
        }

        public JsonResult GetLoanBADetails()
        {
            long EmpID = (long)Session["UID"];
            List<VM_LoanBranchAdjustments> lstBADetails = loanBll.GetLoanBADetails(EmpID);
            return Json(lstBADetails,JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Loan Application Status
        [Route("kgid-loan-app-status")]
        [CustomAuthorize("Employee")]
        public ActionResult LoanApplicationStatus()
        {
            long empId = (long)Session["UID"];
            VM_LoanApplicationStatus loanApplicationList = loanBll.GetLoanApplicationStaus(empId);
            return View(loanApplicationList);
        }
        public int loanApplicationCancel(VM_ApplicationStatus loanApplicationCancel)
        {
            int result = loanBll.CancelLoanApplication(loanApplicationCancel);
            return result;
        }
        #endregion

        #region DDO LoanVerification
        [Route("kgid-ddo-loan")]
        [CustomAuthorize("DDO")]
        public ActionResult LoanDetailsForDDOVerification()
        {
            ViewBag.Verifier = Verifiers.DDO;
            VM_LoanVerificationDetails loanAppDetails = new VM_LoanVerificationDetails();
            long empId = (long)Session["UID"];
            loanAppDetails = loanApplicationBll.GetLoanApplicationListForAll(empId, Convert.ToInt16(UserCategories.DDO));
            return View("LoanVerificationDetails", loanAppDetails);
        }
        [Route("kgid-ddo-loan-verification")]
        [CustomAuthorize("DDO")]
        public ActionResult GetLoanApplicationFormForDDO(long empId = 0, long loanApplicationId = 0)
        {
            if (empId != 0 && loanApplicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = loanApplicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                loanApplicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                var loanApplicationData = loanApplicationBll.GetLoanApplicationData(empId, loanApplicationId);
                return View("DDOLoanVerification", loanApplicationData);
            }
            else
            {
                return RedirectToAction("LoanDetailsForDDOVerification", "Loan");
            }
        }
        #endregion

        #region CW LoanVerification
        [Route("kgid-cw-loan")]
        [CustomAuthorize("Caseworker")]
        public ActionResult LoanDetailsForCWVerification()
        {
            ViewBag.Verifier = Verifiers.CW;
            VM_LoanVerificationDetails loanAppDetails = new VM_LoanVerificationDetails();
            long empId = (long)Session["UID"];
            loanAppDetails = loanApplicationBll.GetLoanApplicationListForAll(empId, Convert.ToInt16(UserCategories.CASEWORKER));
            return View("LoanVerificationDetails", loanAppDetails);
        }
        [Route("kgid-cw-loan-verification")]
        [CustomAuthorize("Caseworker")]
        public ActionResult GetLoanApplicationFormForCW(long empId = 0, long loanApplicationId = 0)
        {
            if (empId != 0 && loanApplicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = loanApplicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                loanApplicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                var loanApplicationData = loanApplicationBll.GetLoanApplicationData(empId, loanApplicationId);
                return View("CWLoanVerification", loanApplicationData);
            }
            else
            {
                return RedirectToAction("LoanDetailsForCWVerification", "Loan");
            }
        }
        #endregion

        #region SI LoanVerification
        [Route("kgid-si-loan")]
        [CustomAuthorize("Superintendent")]
        public ActionResult LoanDetailsForSIVerification()
        {
            ViewBag.Verifier = Verifiers.SUPERINTENDENT;
            VM_LoanVerificationDetails loanAppDetails = new VM_LoanVerificationDetails();
            long empId = (long)Session["UID"];
            loanAppDetails = loanApplicationBll.GetLoanApplicationListForAll(empId, Convert.ToInt16(UserCategories.SUPERINTENDENT));
            return View("LoanVerificationDetails", loanAppDetails);
        }
        [Route("kgid-si-loan-verification")]
        [CustomAuthorize("Superintendent")]
        public ActionResult GetLoanApplicationFormForSI(long empId = 0, long loanApplicationId = 0)
        {
            if (empId != 0 && loanApplicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = loanApplicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                loanApplicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                var loanApplicationData = loanApplicationBll.GetLoanApplicationData(empId, loanApplicationId);
                return View("SILoanVerification", loanApplicationData);
            }
            else
            {
                return RedirectToAction("LoanDetailsForSIVerification", "Loan");
            }
        }
        #endregion

        #region DIO LoanVerification
        [Route("kgid-dio-loan")]
        [CustomAuthorize("DIO")]
        public ActionResult LoanDetailsForDIOVerification()
        {
            ViewBag.Verifier = Verifiers.DIO;
            VM_LoanVerificationDetails loanAppDetails = new VM_LoanVerificationDetails();
            long empId = (long)Session["UID"];
            loanAppDetails = loanApplicationBll.GetLoanApplicationListForAll(empId, Convert.ToInt16(UserCategories.DIO));
            return View("LoanVerificationDetails", loanAppDetails);
        }
        [Route("kgid-dio-loan-verification")]
        [CustomAuthorize("DIO")]
        public ActionResult GetLoanApplicationFormForDIO(long empId = 0, long loanApplicationId = 0)
        {
            if (empId != 0 && loanApplicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = loanApplicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                loanApplicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                var loanApplicationData = loanApplicationBll.GetLoanApplicationData(empId, loanApplicationId);
                return View("DIOLoanVerification", loanApplicationData);
            }
            else
            {
                return RedirectToAction("LoanDetailsForDIOVerification", "Loan");
            }
        }
        #endregion

        #region DD LoanVerification
        [Route("kgid-dd-loan")]
        [CustomAuthorize("Deputy Director")]
        public ActionResult LoanDetailsForDDVerification()
        {
            ViewBag.Verifier = Verifiers.DEPUTYDIRECTOR;
            VM_LoanVerificationDetails loanAppDetails = new VM_LoanVerificationDetails();
            long empId = (long)Session["UID"];
            loanAppDetails = loanApplicationBll.GetLoanApplicationListForAll(empId, Convert.ToInt16(UserCategories.DEPUTYDIRECTOR));
            return View("LoanVerificationDetails", loanAppDetails);
        }
        [Route("kgid-dd-loan-verification")]
        [CustomAuthorize("Deputy Director")]
        public ActionResult GetLoanApplicationFormForDD(long empId = 0, long loanApplicationId = 0)
        {
            if (empId != 0 && loanApplicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = loanApplicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                loanApplicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                var loanApplicationData = loanApplicationBll.GetLoanApplicationData(empId, loanApplicationId);
                return View("DDLoanVerification", loanApplicationData);
            }
            else
            {
                return RedirectToAction("LoanDetailsForDDVerification", "Loan");
            }
        }
        #endregion

        #region D LoanVerification
        [Route("kgid-d-loan")]
        [CustomAuthorize("Director")]
        public ActionResult LoanDetailsForDVerification()
        {
            ViewBag.Verifier = Verifiers.DIRECTOR;
            VM_LoanVerificationDetails loanAppDetails = new VM_LoanVerificationDetails();
            long empId = (long)Session["UID"];
            loanAppDetails = loanApplicationBll.GetLoanApplicationListForAll(empId, Convert.ToInt16(UserCategories.DIRECTOR));
            return View("LoanVerificationDetails", loanAppDetails);
        }
        [Route("kgid-d-loan-verification")]
        [CustomAuthorize("Director")]
        public ActionResult GetLoanApplicationFormForD(long empId = 0, long loanApplicationId = 0)
        {
            if (empId != 0 && loanApplicationId != 0)
            {
                TempData["empId"] = empId;
                TempData["applicationId"] = loanApplicationId;
            }
            if (TempData["empId"] != null && TempData["applicationId"] != null)
            {
                empId = Convert.ToInt32(TempData.Peek("empId"));
                loanApplicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                TempData.Keep("empId");
                TempData.Keep("applicationId");
                var loanApplicationData = loanApplicationBll.GetLoanApplicationData(empId, loanApplicationId);
                return View("DLoanVerification", loanApplicationData);
            }
            else
            {
                return RedirectToAction("LoanDetailsForDVerification", "Loan");
            }
        }
        #endregion

        #region Forward Reject Loan
        [HttpPost]
        public ActionResult LoanApplicationForward(VM_LoanWorkFlow loanApplicationWorkflowModel)
        {
            if (Session["UID"] != null)
            {
                loanApplicationWorkflowModel.law_verified_by = (long)Session["UID"];
            }
            if (Session["SelectedCategory"] != null)
            {
                loanApplicationWorkflowModel.selectedcategory = Convert.ToInt16(Session["SelectedCategory"]);
            }
            var loanApplicationForward = loanApplicationBll.LoanApplicationForward(loanApplicationWorkflowModel);
            return Json(loanApplicationForward, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoanApplicationReject(VM_LoanWorkFlow loanApplicationWorkflowModel)
        {
            if (Session["UID"] != null)
            {
                loanApplicationWorkflowModel.law_verified_by = (long)Session["UID"];
            }
            var loanApplicationForward = loanApplicationBll.LoanApplicationForward(loanApplicationWorkflowModel);
            return Json(loanApplicationForward, JsonRequestBehavior.AllowGet);
        }
        #endregion
       
        #region Loan Cancellation Department User
        [Route("kgid-loan-c-req")]
        [CustomAuthorize("DDO","DIO")]
        public ActionResult LoanCancellationDetails(string Type = "")
        {
            VM_LoanVerificationDetails loanAppDetails = new VM_LoanVerificationDetails();
            //long empId = (long)Session["UID"];
            if (Session["UID"] != null)
            {
                loanAppDetails = loanApplicationBll.GetLoanCancellationListForAll((long)Session["UID"],Type);
            }
            
            return View("LoanCancellationDetails", loanAppDetails);
        }

        public int NBLoanCancelRequestAction(long AppId, long EmpId, string Action)
        {
            int result = loanApplicationBll.NBLoanCancelRequestAction(AppId, EmpId, Action);
            return result;
        }
        #endregion

        #region Loan Application Disburse
        public int LoanApplicationDisburse(long LoanAppID = 0,string Type = "")
        {
            int result = loanApplicationBll.GetLoanDisburseMailData(LoanAppID, Type);
            return result;
        }
        #endregion
    }
}