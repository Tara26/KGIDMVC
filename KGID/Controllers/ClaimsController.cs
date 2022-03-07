using BLL.ClaimsBLL;
using Common;
using KGID_Models.Claim;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGID.Controllers
{
    [RoutePrefix("Claims")]
    public class ClaimsController : Controller
    {
        private readonly IClaimsBLL claims;

        public ClaimsController()
        {
            claims = new ClaimsBLL();
        }

        #region DDO

        public ActionResult DDOPreMatureClaimApplication()
        {
            VM_EmpDetailForPrematureClaimApplication empDetail = new VM_EmpDetailForPrematureClaimApplication();
            empDetail.ClaimSubTypes = claims.GetClaimSubTypes((int)ClaimTypes.PREMATURITY);
            return View("DDOInitiatePrematureClaimApplication", empDetail);
        }

        public ActionResult DDODeathClaimApplication()
        {
            VM_EmpDetailForDeathClaimApplication empDetail = new VM_EmpDetailForDeathClaimApplication();
            empDetail.ClaimSubTypes = claims.GetClaimSubTypes((int)ClaimTypes.DEATH);
            return View("DDOInitiateDeathClaimApplication", empDetail);
        }

        public JsonResult GetEmployeeDetailsForDDOPrematureClaims(string kgidNumber)
        {
            VM_EmpDetailForPrematureClaimApplication empDetail = claims.GetEmployeeDetailByKGIDNumber(kgidNumber);
            return Json(new { EmpDetail = empDetail }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ForwardApplicationToCaseworker(VM_EmpDetailForPrematureClaimApplication empDetail)
        {
            string message = string.Empty;
            bool isCreated = false;
            bool isAlreadyInitiated = false;
            empDetail.DDOUserId = Convert.ToInt64(Session["UID"]);
            string response = claims.ForwardApplicationToCaseworker(empDetail);
            if (!string.IsNullOrEmpty(response))
            {
                if (response != "0")
                {
                    isCreated = true;
                    message = "Claim Application initiated successfully";
                }
                else
                {
                    isAlreadyInitiated = true;
                    message = "Claim Application has been already initiated";
                }
            }
            else
            {
                message = "Please check KGID number and retry";
            }

            return Json(new { IsCreated = isCreated, Message = message, IsAlreadyInitiated = isAlreadyInitiated }, JsonRequestBehavior.AllowGet);
        }

        #endregion DDO

        public ActionResult PreMaturityClaimEmployeeDetails(long empId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetPreMaturityClaimEmployeeDetails(empId);
            return View(employeeDetail);
        }

        public JsonResult SaveApplicationDetails(VM_EmpDetailForDeathClaimApplication empDetail)
        {
            bool isDetailsSaved = false;
            bool isAlreadyInitiated = false;
            string message = string.Empty;
            empDetail.VerifiedByEmpId = Convert.ToInt64(Session["UID"]);
            string response = claims.SaveApplicationDetails(empDetail);

            if (response.Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                message = "Claim initiated successfully";
                isDetailsSaved = true;
            }
            else if (response.Equals("2", StringComparison.OrdinalIgnoreCase))
            {
                message = "Claim has been already initiated";
                isAlreadyInitiated = true;
            }

            return Json(new { IsSuccess = isDetailsSaved, IsAlreadyInitiated = isAlreadyInitiated, Message = message, ClaimSubTypeId = empDetail.ClaimSubTypeId, EmpId = empDetail.Id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClaimRequiredDocuments(int claimSubTypeId, long empId)
        {
            VM_ClaimRequiredDocuments claimRequiredDocuments = new VM_ClaimRequiredDocuments();
            claimRequiredDocuments = claims.GetAdditionalDocumentsList(empId);
            claimRequiredDocuments.ClaimSubTypeId = claimSubTypeId;
            claimRequiredDocuments.EmpId = empId;
            claimRequiredDocuments.OtherDocuments.Add(claimRequiredDocuments.ApplicationNominee);
            return View(claimRequiredDocuments);
        }

        public PartialViewResult GetDeathClaimApplicationForm(int claimSubTypeId)
        {
            if (claimSubTypeId != 0)
            {
                VM_EmpDetailForDeathClaimApplication empDetailForDeathClaimApplication = new VM_EmpDetailForDeathClaimApplication();
                if (claimSubTypeId == (int)ClaimSubTypes.ILLNESS)
                {
                    return PartialView("_IlnessDeathClaim", empDetailForDeathClaimApplication);
                }
                else if (claimSubTypeId == (int)ClaimSubTypes.UNNATURALDEATH)
                {
                    return PartialView("_UnnaturalDeathClaim", empDetailForDeathClaimApplication);
                }
                else if (claimSubTypeId == (int)ClaimSubTypes.MISSINGABSCONDING)
                {
                    return PartialView("_MissingDeathClaim", empDetailForDeathClaimApplication);
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SumbitClaimDocuments(VM_ClaimRequiredDocuments claimDocuments)
        {
            claimDocuments.VerifiedByEmpId = Convert.ToInt64(Session["UID"]);
            var data = claims.InitiateDeathClaimApplication(claimDocuments);
            return RedirectToAction("dashboard", "home");
        }


        #region MaturityClaim
        #region Case Worker
        public ActionResult CWMaturityClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetMaturityClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);
            return View("CWMaturityClaimVerification", employeeDetail);
        }
        public ActionResult CWMaturityClaimApplications()
        {
            int empType = (int)Verifiers.CW;
            int claimType = (int)ClaimTypes.MATURITY;
            ViewBag.ClaimType = ClaimTypes.MATURITY;
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            VM_ClaimApplications maturityClaims = new VM_ClaimApplications();
            maturityClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", maturityClaims);
        }
        [Route("CWSaveMClaimVData")]
        public ActionResult CWInsertMClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("CWMaturityClaimApplications");
            }

            return null;
        }
        #endregion

        #region Superintendent
        public ActionResult SIMaturityClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetMaturityClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);
            return View("SIMaturityClaimVerification", employeeDetail);
        }
        public ActionResult SIMaturityClaimApplications()
        {
            int empType = (int)Verifiers.SUPERINTENDENT;
            int claimType = (int)ClaimTypes.MATURITY;
            ViewBag.ClaimType = ClaimTypes.MATURITY;
            VM_ClaimApplications maturityClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            maturityClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", maturityClaims);
        }
        [Route("SISaveMClaimVData")]
        public ActionResult SIInsertMClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("SIMaturityClaimApplications");
            }

            return null;
        }

        #endregion

        #region DIO
        public ActionResult DIOMaturityClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetMaturityClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);
            return View("DIOMaturityClaimVerification", employeeDetail);
        }
        public ActionResult DIOMaturityClaimApplications()
        {
            int empType = (int)Verifiers.DIO;
            int claimType = (int)ClaimTypes.MATURITY;
            ViewBag.ClaimType = ClaimTypes.MATURITY;
            VM_ClaimApplications maturityClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            maturityClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", maturityClaims);
        }
        [Route("DIOSaveMClaimVData")]
        public ActionResult DIOSaveMClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("DIOMaturityClaimApplications");
            }

            return null;
        }

        #endregion

        #region DD
        public ActionResult DDMaturityClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetMaturityClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);
            return View("DDMaturityClaimVerification", employeeDetail);
        }
        public ActionResult DDMaturityClaimApplications()
        {
            int empType = (int)Verifiers.DEPUTYDIRECTOR;
            int claimType = (int)ClaimTypes.MATURITY;
            ViewBag.ClaimType = ClaimTypes.MATURITY;
            VM_ClaimApplications maturityClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            maturityClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", maturityClaims);
        }
        [Route("DDSaveMClaimVData")]
        public ActionResult DDSaveMClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("DDMaturityClaimApplications");
            }

            return null;
        }

        #endregion

        #region D
        public ActionResult DMaturityClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetMaturityClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);

            return View("DMaturityClaimVerification", employeeDetail);
        }
        public ActionResult DMaturityClaimApplications()
        {
            int empType = (int)Verifiers.DIRECTOR;
            int claimType = (int)ClaimTypes.MATURITY;
            ViewBag.ClaimType = ClaimTypes.MATURITY;
            VM_ClaimApplications maturityClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            maturityClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", maturityClaims);
        }
        [Route("DSaveMClaimVData")]
        public ActionResult DSaveMClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("DMaturityClaimApplications");
            }

            return null;
        }

        #endregion

        #endregion

        #region PreMaturityClaim



        #region Case Worker
        public ActionResult CWPreMaturityClaimApplications()
        {
            int empType = (int)Verifiers.CW;
            int claimType = (int)ClaimTypes.PREMATURITY;
            ViewBag.ClaimType = ClaimTypes.PREMATURITY;
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            VM_ClaimApplications prematurityClaims = new VM_ClaimApplications();
            prematurityClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", prematurityClaims);
        }
        public ActionResult CWPreMaturityClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetPreMaturityClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);

            return View("CWPre-MaturityClaimVerification", employeeDetail);
        }
        [Route("CWSavePreMClaimVData")]
        public ActionResult CWSavePreMClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("CWPreMaturityClaimApplications");
            }

            return null;
        }
        #endregion

        #region Superintendent
        public ActionResult SIPreMaturityClaimApplications()
        {
            int empType = (int)Verifiers.SUPERINTENDENT;
            int claimType = (int)ClaimTypes.PREMATURITY;
            ViewBag.ClaimType = ClaimTypes.PREMATURITY;
            VM_ClaimApplications prematurityClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            prematurityClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", prematurityClaims);
        }
        public ActionResult SIPreMaturityClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetPreMaturityClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);

            return View("SIPre-MaturityClaimVerification", employeeDetail);
        }
        [Route("SISavePreMClaimVData")]
        public ActionResult SISavePreMClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("SIPreMaturityClaimApplications");
            }

            return null;
        }

        #endregion

        #region DIO
        public ActionResult DIOPreMaturityClaimApplications()
        {
            int empType = (int)Verifiers.DIO;
            int claimType = (int)ClaimTypes.PREMATURITY;
            ViewBag.ClaimType = ClaimTypes.PREMATURITY;
            VM_ClaimApplications prematurityClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            prematurityClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", prematurityClaims);
        }
        public ActionResult DIOPreMaturityClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetPreMaturityClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);

            return View("DIOPre-MaturityClaimVerification", employeeDetail);
        }
        [Route("DIOSavePreMClaimVData")]
        public ActionResult DIOSavePreMClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("DIOPreMaturityClaimApplications");
            }

            return null;
        }

        #endregion

        #region DD
        public ActionResult DDPreMaturityClaimApplications()
        {
            int empType = (int)Verifiers.DEPUTYDIRECTOR;
            int claimType = (int)ClaimTypes.PREMATURITY;
            ViewBag.ClaimType = ClaimTypes.PREMATURITY;
            VM_ClaimApplications prematurityClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            prematurityClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", prematurityClaims);
        }
        public ActionResult DDPreMaturityClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetPreMaturityClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);

            return View("DDPre-MaturityClaimVerification", employeeDetail);
        }
        [Route("DDSavePreMClaimVData")]
        public ActionResult DDSavePreMClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("DDPreMaturityClaimApplications");
            }

            return null;
        }

        #endregion

        #region D
        public ActionResult DPreMaturityClaimApplications()
        {
            int empType = (int)Verifiers.DIRECTOR;
            int claimType = (int)ClaimTypes.PREMATURITY;
            ViewBag.ClaimType = ClaimTypes.PREMATURITY;
            VM_ClaimApplications prematurityClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            prematurityClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", prematurityClaims);
        }
        public ActionResult DPreMaturityClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetPreMaturityClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);

            return View("DPre-MaturityClaimVerification", employeeDetail);
        }
        [Route("DSavePreMClaimVData")]
        public ActionResult DSavePreMClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("DPreMaturityClaimApplications");
            }

            return null;
        }

        #endregion


        #endregion

        #region DeathClaim
        #region Case Worker
        public ActionResult CWDeathClaimApplications()
        {
            int empType = (int)Verifiers.CW;
            int claimType = (int)ClaimTypes.DEATH;
            ViewBag.ClaimType = ClaimTypes.DEATH;
            VM_ClaimApplications deathClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            deathClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", deathClaims);
        }
        public ActionResult CWDeathClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetDeathClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);
            return View("CWDeathClaimVerification", employeeDetail);
        }
        [Route("CWSaveDeathClaimVData")]
        public ActionResult CWSaveDeathClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("CWDeathClaimApplications");
            }

            return null;
        }
        #endregion

        #region Superintendent
        public ActionResult SIDeathClaimApplications()
        {
            int empType = (int)Verifiers.SUPERINTENDENT;
            int claimType = (int)ClaimTypes.DEATH;
            ViewBag.ClaimType = ClaimTypes.DEATH;
            VM_ClaimApplications deathClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            deathClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", deathClaims);
        }
        public ActionResult SIDeathClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetDeathClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);
            return View("SIDeathClaimVerification", employeeDetail);
        }
        [Route("SISaveDeathClaimVData")]
        public ActionResult SISaveDeathClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("SIDeathClaimApplications");
            }

            return null;
        }

        #endregion

        #region DIO
        public ActionResult DIODeathClaimApplications()
        {
            int claimType = (int)ClaimTypes.DEATH;
            int empType = (int)Verifiers.DIO;
            ViewBag.ClaimType = ClaimTypes.DEATH;
            VM_ClaimApplications deathClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            deathClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", deathClaims);
        }
        public ActionResult DIODeathClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetDeathClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);
            return View("DIODeathClaimVerification", employeeDetail);
        }
        [Route("DIOSaveDeathClaimVData")]
        public ActionResult DIOSaveDeathClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("DIODeathClaimApplications");
            }

            return null;
        }

        #endregion

        #region DD
        public ActionResult DDDeathClaimApplications()
        {
            int empType = (int)Verifiers.DEPUTYDIRECTOR;
            int claimType = (int)ClaimTypes.DEATH;
            ViewBag.ClaimType = ClaimTypes.DEATH;
            VM_ClaimApplications deathClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            deathClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", deathClaims);
        }
        public ActionResult DDDeathClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetDeathClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);
            return View("DDDeathClaimVerification", employeeDetail);
        }
        [Route("DDSaveDeathClaimVData")]
        public ActionResult DDSaveDeathClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("DDDeathClaimApplications");
            }

            return null;
        }

        #endregion

        #region D
        public ActionResult DDeathClaimApplications()
        {
            int empType = (int)Verifiers.DIRECTOR;
            int claimType = (int)ClaimTypes.DEATH;
            ViewBag.ClaimType = ClaimTypes.DEATH;
            VM_ClaimApplications deathClaims = new VM_ClaimApplications();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            deathClaims = claims.GetClaimApplications(empType, claimType, loggedInUserId);
            return View("ClaimApplications", deathClaims);
        }
        public ActionResult DDeathClaimDetails(long empId, long applicationId)
        {
            VM_ClaimEmployeeDetail employeeDetail = new VM_ClaimEmployeeDetail();
            employeeDetail = claims.GetDeathClaimEmployeeDetails(empId);
            employeeDetail.WorkFlowDetails = claims.GetClaimWorkFlowDetails(applicationId);

            return View("DDeathClaimVerification", employeeDetail);
        }
        [Route("DSaveDeathClaimVData")]
        public ActionResult DSaveDeathClaimVerifiedData(VM_ClaimEmployeeDetail caseWorkerVerifiedDetails)
        {
            caseWorkerVerifiedDetails.CreatedBy = Convert.ToInt64(Session["UID"]);
            caseWorkerVerifiedDetails.ApplicationRefNo = caseWorkerVerifiedDetails.ApplicationId;
            string result = claims.SaveVerifiedDetailsBll(caseWorkerVerifiedDetails);
            if (Convert.ToInt32(result) > 0)
            {
                return RedirectToAction("DDeathClaimApplications");
            }

            return null;
        }

        #endregion


        #endregion

    }
}