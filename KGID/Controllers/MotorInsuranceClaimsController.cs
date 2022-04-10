using BLL.MBClaimsBLL;
using Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _ExcelSheet = Microsoft.Office.Interop.Excel;

namespace KGID.Controllers
{
    public class MotorInsuranceClaimsController : Controller
    {
        private readonly IMBClaimsBLL _IMBClaimsBLL;

        public MotorInsuranceClaimsController()
        {
            this._IMBClaimsBLL = new MBClaimsBLL();
        }

        //Master Data Dist&Taluka
        public ActionResult GetDistList()
        {
            List<KeyValuePair<int, string>> distlist = new List<KeyValuePair<int, string>>();

            List<tbl_district_master> getdistlist = _IMBClaimsBLL.GetDistListBLL();

            foreach (var item in getdistlist)
            {
                distlist.Add(new KeyValuePair<int, string>(item.dm_id, item.dm_name_english));
            }

            return Json(distlist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTalukaList(int DistId)
        {
            List<KeyValuePair<int, string>> talukalist = new List<KeyValuePair<int, string>>();

            List<tbl_taluka_master> gettalukalist = _IMBClaimsBLL.GetTalukaListBLL(DistId);

            foreach (var item in gettalukalist)
            {
                talukalist.Add(new KeyValuePair<int, string>(item.tm_id, item.tm_englishname));
            }

            return Json(talukalist, JsonRequestBehavior.AllowGet);
        }
        [Route("mi-dpt-odc-mvcappform")]
        public ActionResult MvcClaimApplicationForm()
        {
            GetVehicleChassisPolicyDetails MVCClaimDetails = new GetVehicleChassisPolicyDetails();

            MVCClaimDetails.DistrictList = _IMBClaimsBLL.GetDistrictListBLL();
            MVCClaimDetails.TalukaList = _IMBClaimsBLL.GetTalukListBLL(0);
            MVCClaimDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();
                MVCClaimDetails.InjuryList = _IMBClaimsBLL.GetInjuryListBLL();
                MVCClaimDetails.StateList = _IMBClaimsBLL.GetstateListBLL();
                MVCClaimDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCApplicationFormDataBLL();
                MVCClaimDetails.otherDetailsData = _IMBClaimsBLL.GetDraftDetailsBLL();
                MVCClaimDetails.CourtExecutionMasterDetails = _IMBClaimsBLL.GetSentBackMVCDetailsBLL();
            return View(MVCClaimDetails);
        }
        public JsonResult GetVehiclePolicyAndChassisDetails(string textDetails)
        {
            GetVehicleChassisPolicyDetails VehicleDetailsModel = new GetVehicleChassisPolicyDetails();
            VehicleDetailsModel.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetVehicleAndPolicyDetailsBLL(textDetails);
            VehicleDetailsModel.VehicleChassisPolicyDetailsList.OD_to_date1 = (VehicleDetailsModel.VehicleChassisPolicyDetailsList.OD_to_date).Value.ToString("dd/MM/yyyy");
            VehicleDetailsModel.VehicleChassisPolicyDetailsList.OD_from_date1 = (VehicleDetailsModel.VehicleChassisPolicyDetailsList.OD_from_date).Value.ToString("dd/MM/yyyy");
            VehicleDetailsModel.VehicleChassisPolicyDetailsList.TP_from_date1 = (VehicleDetailsModel.VehicleChassisPolicyDetailsList.TP_from_date).Value.ToString("dd/MM/yyyy");
            VehicleDetailsModel.VehicleChassisPolicyDetailsList.TP_to_date1 = (VehicleDetailsModel.VehicleChassisPolicyDetailsList.TP_to_date).Value.ToString("dd/MM/yyyy");

            // var result = 1;
            return Json(VehicleDetailsModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaluksOnDistrict(int District_dm_id)
        {

            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            var result = _IMBClaimsBLL.GetTalukListBLL(District_dm_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveMVCClaimDetails(GetVehicleChassisPolicyDetails model)
        {
            long result = 0;
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]); 
            model.roleID = 4; 
            if (model.application_stat == 2) { 
                 result = _IMBClaimsBLL.SaveMVCClaimDetailsBLL(model);
               }
            else
            {
                 result = _IMBClaimsBLL.SaveAsDraftMvcDetailsBLL(model);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadFiles(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/Rc_details/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully");
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }

            return Json("no files were selected !");
        }
        public JsonResult uploadDLDetails(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/Dl_details/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully");
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }
        public JsonResult uploadPanchanamaLDetails(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/Panchanama_details/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }
        public JsonResult uploadFirDetails(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/Fir_details/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }
        public JsonResult uploadObjectStatementDetails(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/ObjectStatement_details/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }
        public JsonResult uploadCourtNoticeDetails(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/Court_Notice_details/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }
        public JsonResult uploadpetitionerDetails(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/Petitioner_details/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }

        //upload CoveringLetter

        public JsonResult uploadCoveringLetter(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/Covering_Letter/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }

        //upload Prefilled Claim Form 
        public JsonResult uploadPrefilledClaimForm(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/PrefilledClaimForm/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }

        

        //Driver statement and Request to Submit RC * :
        public JsonResult uploadDsRc(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/DsRc/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }

        //DL
        public JsonResult uploadDL(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/DL/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }

        //Insurance Copy To Owner of the Vehicle 
        public JsonResult uploadInsuranceCopy(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/InsuranceCopy/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }
        [HttpPost]
        //public JsonResult SavePetitionerRespondantDetails(long Application_id, List<PetitionerData> PetitionerList, List<RespondData> RespondantList) {

        //    var result = _IMBClaimsBLL.PetitionerRespondantDetailsBLL(Application_id, PetitionerList, RespondantList);

        //    return Json("File uploaded successfully");
        //}
        public JsonResult SavePetitionerRespondantDetails(GetVehicleChassisPolicyDetails model) {

           // var result = _IMBClaimsBLL.PetitionerRespondantDetailsBLL(Application_id, PetitionerList, RespondantList);

            return Json("File uploaded successfully");
        }
        public JsonResult uploadOtherDocuments(long App_id,int fileId)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;
                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/OtherDocuments/" + fileId + "/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);

                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }


        [Route("mvc-si-adt")]
        [CustomAuthorize("Superintendent")]
        public ActionResult MvcClaimApprovalProcess()
        {
            ViewBag.Verifier = Verifiers.SUPERINTENDENT;
            ViewBag.Verify = 4;
            //VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeDetailsForSuperintendentVerification(Convert.ToInt64(Session["UID"]));
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCApplicationFormDataBLL();
            return View(GetDetails);
        }
        public JsonResult GetMVCdetailsforSuperindenant(string chassis, long Appno)
        {
            var loginId = Convert.ToInt64(Session["UID"]);
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();

            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);

            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(Appno);
            GetDetails.VehicleChassisPolicyDetailsList.OD_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.OD_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(Appno);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(Appno,category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(Appno);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(Appno);
            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/Dl_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_dl_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Fir_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_fir_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/ObjectStatement_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_object_statement_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Panchanama_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_panchnama_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Rc_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_dl_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Summons_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

            }

            return Json(GetDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMVCdetailsofCourt(long ApplicationNo)
        {
            var loginId = Convert.ToInt64(Session["UID"]);
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(ApplicationNo, category);

            return Json(GetDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendMVCTohigherHierarchy(string vehicleNo, long Appno)
        {
            var loginId = Convert.ToInt64(Session["UID"]);
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();

            //GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(ApplicationNo);

            return Json(GetDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GenaratePrefilledDocs()
        {


            var res = 1;

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [Route("mvc_cw_va/{chassis}/{appid}")]

        [CustomAuthorize("Caseworker")]
        public ActionResult MVCClaimCaseWorkerVerification(long appid, string chassis)
        {
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();

            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();

            GetDetails.MvcClaimWorkFlowDetails = _IMBClaimsBLL.MvcClaimWorkFlowDetailsBLL(appid, chassis);
            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.VehicleChassisPolicyDetailsList.OD_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.OD_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(appid,category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
            GetDetails.OpinionStatusList = _IMBClaimsBLL.GetRemarksUpperCourtBLL();
            GetDetails.JudgementRemarksList = _IMBClaimsBLL.RemarksJudgementBLL();
            GetDetails.ObjectStatementRemarkList = _IMBClaimsBLL.RemarksObjectionStatementBLL(category);
            GetDetails.PaymentRemarkList = _IMBClaimsBLL.RemarksPaymentStatementBLL(category);
            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/Covering_Letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].CoveringLetter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DL/"))
                {
                    GetDetails.MVCAppDocDetails[0].DL = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Insurance_Copy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].DriverstatementandRc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].Prefilled_Claim_Form = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Authorization_letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].authorization_letter = GetDetails.MVCAppDocDetails[i].Accident_details;
                    GetDetails.authorization_check = 1;
                }if (path.Contains("/ParawiseRemarksSubmissionToLawyer/"))
                {
                    GetDetails.MVCAppDocDetails[0].court_parawise = GetDetails.MVCAppDocDetails[i].Accident_details;
                   // GetDetails.authorization_check = 1;
                }if (path.Contains("/ObjectStatement_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_object_statement_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                   // GetDetails.authorization_check = 1;
                }
                if (path.Contains("/RatificationLetterLawDept/"))
                {
                    GetDetails.MVCAppDocDetails[0].RatificationToLawDept = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/RatificationLetterToKGID/"))
                {
                    GetDetails.MVCAppDocDetails[0].RatificationToKgid = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/LowerCourtJudgementCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].LowerCourtJudgementCopy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/opinionFromLawDept/"))
                {
                    GetDetails.MVCAppDocDetails[0].opinionLawfromLawDept = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/DelayNotetoGovtAdvocateHighCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].DelayNoteHighCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/CondonationOfDelay/"))
                {
                    GetDetails.MVCAppDocDetails[0].CondonationOfDelay = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/StayAffidavitHighCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayAffidavitHighCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/GroundsofAppeal/"))
                {
                    GetDetails.MVCAppDocDetails[0].GroundsofAppeal = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/AmountDepostionLetterToLowerCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmtDepositLC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/AmountDepostionLetterToLowerCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtJudgementOpinion = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/HighCourtJudgementOpinionFormLawDeptDetails/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtOpinionDesc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/High_Court_Notice/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtDepositAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/High_Court_PetitionCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimAwardedAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/HighCourtCoveringLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimSettleCost = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/HighCourtAuthorizationLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimSettleTotalAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/HighCourtJudgementOpinionFormLawDeptDetails2/")) 
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtOpinionJudgement2 = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/DelayNotetoGovtAdvocateSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].InputDelaySupremeCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/CondonationOfDelaySupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].CondonationOfDelaySupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/StayAffidavitSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayAffidavitSupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/GroundsofAppealSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].GroundsofAppealSupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/AmountDepositiontoSupreme_Court/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmountDepositiontoSupremeCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/Amount_Deposition_supremeCourtTOLowercourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmountDepositiontoSupremeCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/Intimation_DistrictKGID_from_supremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].IntimationToDistrictCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/StayOrderfromSupremeCourtToDistrictKGIDOffice/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayOrderToToDistrictSupremeCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/SupremeCourtJudgementCopyandopinionfromLawDeptReceivedatKGID/"))
                {
                    GetDetails.MVCAppDocDetails[0].SupremeOpinionDesc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/Supreme_Court_PetitionCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Statuatory_Amount_Remitted = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/Supreme_Court_Notice/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Deposit_Amount_Remitted = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/SupremeCourtCoveringLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Total_Amount = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/SupremeCourtAuthorizationLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Court_Cost = GetDetails.MVCAppDocDetails[i].Accident_details;
                }if (path.Contains("/SupremeOpinionNoticeFromLawDept2/"))
                {
                    GetDetails.MVCAppDocDetails[0].awardedAmount_highCourtClaimSttleKGID = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
            }
            return View(GetDetails);
        }
        [Route("mvc_sup_va/{chassis}/{appid}")]
        public ActionResult MVCClaimsuperintendedVerification(long appid, string chassis)
        {
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();

            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
           GetDetails.MvcClaimWorkFlowDetails = _IMBClaimsBLL.MvcClaimWorkFlowDetailsBLL(appid, chassis);
            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.VehicleChassisPolicyDetailsList.OD_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.OD_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(appid,category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
            GetDetails.MvcClaimWorkFlowDetails = _IMBClaimsBLL.MvcClaimWorkFlowDetailsBLL(appid, chassis);
            GetDetails.JudgementRemarksList = _IMBClaimsBLL.RemarksJudgementBLL();
            GetDetails.ObjectStatementRemarkList = _IMBClaimsBLL.RemarksObjectionStatementBLL(category);
            GetDetails.PaymentRemarkList = _IMBClaimsBLL.RemarksPaymentStatementBLL(category);


            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/Covering_Letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].CoveringLetter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DL/"))
                {
                    GetDetails.MVCAppDocDetails[0].DL = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Insurance_Copy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].DriverstatementandRc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].Prefilled_Claim_Form = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Authorization_letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].authorization_letter = GetDetails.MVCAppDocDetails[i].Accident_details;
                    GetDetails.authorization_check = 1;
                }
                if (path.Contains("/ParawiseRemarksSubmissionToLawyer/"))
                {
                    GetDetails.MVCAppDocDetails[0].court_parawise = GetDetails.MVCAppDocDetails[i].Accident_details;
                    // GetDetails.authorization_check = 1;
                }
                if (path.Contains("/ObjectStatement_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_object_statement_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                    // GetDetails.authorization_check = 1;
                }
                if (path.Contains("/RatificationLetterLawDept/"))
                {
                    GetDetails.MVCAppDocDetails[0].RatificationToLawDept = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/RatificationLetterToKGID/"))
                {
                    GetDetails.MVCAppDocDetails[0].RatificationToKgid = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/LowerCourtJudgementCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].LowerCourtJudgementCopy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/opinionFromLawDept/"))
                {
                    GetDetails.MVCAppDocDetails[0].opinionLawfromLawDept = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DelayNotetoGovtAdvocateHighCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].DelayNoteHighCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/CondonationOfDelay/"))
                {
                    GetDetails.MVCAppDocDetails[0].CondonationOfDelay = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/StayAffidavitHighCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayAffidavitHighCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/GroundsofAppeal/"))
                {
                    GetDetails.MVCAppDocDetails[0].GroundsofAppeal = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/AmountDepostionLetterToLowerCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmtDepositLC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/AmountDepostionLetterToLowerCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtJudgementOpinion = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtJudgementOpinionFormLawDeptDetails/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtOpinionDesc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/High_Court_Notice/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtDepositAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/High_Court_PetitionCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimAwardedAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtCoveringLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimSettleCost = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtAuthorizationLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimSettleTotalAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtJudgementOpinionFormLawDeptDetails2/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtOpinionJudgement2 = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DelayNotetoGovtAdvocateSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].InputDelaySupremeCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/CondonationOfDelaySupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].CondonationOfDelaySupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/StayAffidavitSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayAffidavitSupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/GroundsofAppealSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].GroundsofAppealSupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/AmountDepositiontoSupreme_Court/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmountDepositiontoSupremeCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Amount_Deposition_supremeCourtTOLowercourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmountDepositiontoSupremeCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Intimation_DistrictKGID_from_supremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].IntimationToDistrictCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/StayOrderfromSupremeCourtToDistrictKGIDOffice/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayOrderToToDistrictSupremeCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeCourtJudgementCopyandopinionfromLawDeptReceivedatKGID/"))
                {
                    GetDetails.MVCAppDocDetails[0].SupremeOpinionDesc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Supreme_Court_PetitionCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Statuatory_Amount_Remitted = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Supreme_Court_Notice/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Deposit_Amount_Remitted = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeCourtCoveringLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Total_Amount = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeCourtAuthorizationLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Court_Cost = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeOpinionNoticeFromLawDept2/"))
                {
                    GetDetails.MVCAppDocDetails[0].awardedAmount_highCourtClaimSttleKGID = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
            }
            return View(GetDetails);
        }
        public JsonResult MvcSendToAd(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            model.roleID =15;
            var sendBack = 0;
            if(model.Remarks_id == 20)
            {
                model.roleID = 3;
                sendBack = _IMBClaimsBLL.SendBackMvcToCWBLL(model);
            }
              var result = _IMBClaimsBLL.UpdateWork_flow_DetailsBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [Route("mvc_AD_va/{chassis}/{appid}")]
        public ActionResult MVCClaimADVerification(long appid, string chassis)
        {
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();

            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            GetDetails.MvcClaimWorkFlowDetails = _IMBClaimsBLL.MvcClaimWorkFlowDetailsBLL(appid, chassis);
            GetDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();

            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.VehicleChassisPolicyDetailsList.OD_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.OD_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(appid, category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
            GetDetails.JudgementRemarksList = _IMBClaimsBLL.RemarksJudgementBLL();
            GetDetails.ObjectStatementRemarkList = _IMBClaimsBLL.RemarksObjectionStatementBLL(category);
            GetDetails.PaymentRemarkList = _IMBClaimsBLL.RemarksPaymentStatementBLL(category);

            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].PreClaimedForm = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_fir_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Covering_Letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].cover_letter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].insurancecopy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

                //added new
                if (path.Contains("/DL/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_dl_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Authorization_letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].authorization_letter = GetDetails.MVCAppDocDetails[i].Accident_details;
                    GetDetails.authorization_check = 1;
                }
                if (path.Contains("/ParawiseRemarksSubmissionToLawyer/"))
                {
                    GetDetails.MVCAppDocDetails[0].court_parawise = GetDetails.MVCAppDocDetails[i].Accident_details;
                    // GetDetails.authorization_check = 1;
                }
                if (path.Contains("/ObjectStatement_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_object_statement_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                    // GetDetails.authorization_check = 1;
                }
                if (path.Contains("/RatificationLetterLawDept/"))
                {
                    GetDetails.MVCAppDocDetails[0].RatificationToLawDept = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/RatificationLetterToKGID/"))
                {
                    GetDetails.MVCAppDocDetails[0].RatificationToKgid = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/LowerCourtJudgementCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].LowerCourtJudgementCopy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/opinionFromLawDept/"))
                {
                    GetDetails.MVCAppDocDetails[0].opinionLawfromLawDept = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DelayNotetoGovtAdvocateHighCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].DelayNoteHighCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/CondonationOfDelay/"))
                {
                    GetDetails.MVCAppDocDetails[0].CondonationOfDelay = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/StayAffidavitHighCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayAffidavitHighCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/GroundsofAppeal/"))
                {
                    GetDetails.MVCAppDocDetails[0].GroundsofAppeal = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/High_Court_Notice/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtDepositAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/High_Court_PetitionCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimAwardedAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtCoveringLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimSettleCost = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtAuthorizationLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimSettleTotalAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtJudgementOpinionFormLawDeptDetails2/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtOpinionJudgement2 = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DelayNotetoGovtAdvocateSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].InputDelaySupremeCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/CondonationOfDelaySupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].CondonationOfDelaySupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/StayAffidavitSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayAffidavitSupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/GroundsofAppealSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].GroundsofAppealSupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/AmountDepositiontoSupreme_Court/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmountDepositiontoSupremeCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Amount_Deposition_supremeCourtTOLowercourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmountDepositiontoSupremeCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Intimation_DistrictKGID_from_supremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].IntimationToDistrictCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/StayOrderfromSupremeCourtToDistrictKGIDOffice/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayOrderToToDistrictSupremeCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeCourtJudgementCopyandopinionfromLawDeptReceivedatKGID/"))
                {
                    GetDetails.MVCAppDocDetails[0].SupremeOpinionDesc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Supreme_Court_PetitionCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Statuatory_Amount_Remitted = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Supreme_Court_Notice/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Deposit_Amount_Remitted = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeCourtCoveringLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Total_Amount = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeCourtAuthorizationLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Court_Cost = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeOpinionNoticeFromLawDept2/"))
                {
                    GetDetails.MVCAppDocDetails[0].awardedAmount_highCourtClaimSttleKGID = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtJudgementOpinionFormLawDeptDetails/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtOpinionDesc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
            }
            return View(GetDetails);
        }
        [Route("mvc_ad_ver")]
        [Route("mvc_dd_ver")]

        public ActionResult MVCClaimADApprovalProcess()
        {
            ViewBag.Verifier = Verifiers.ASSITANTDIRECTOR;
            ViewBag.Verify = 15;
            
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCApplicationFormDataBLL();
            return View(GetDetails);
        }
        public JsonResult MvcSendToCWFromDir(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            model.claim_Amount = "0";
            model.roleID = 3;
            var sendBack = 0;
            if (model.Remarks_id == 20)
            {
                model.roleID = 3;
                sendBack = _IMBClaimsBLL.SendBackMvcToCWBLL(model);
            }
            var result = _IMBClaimsBLL.UpdateWork_flow_DetailsBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("mvc_dir_ver")]
        public ActionResult MVCClaimDirectorApprovalProcess()
        {
            ViewBag.Verifier = Verifiers.DIRECTOR;
            ViewBag.Verify = 7;

            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCApplicationFormDataBLL();
            return View(GetDetails);
        }


        [Route("mvc_DIR_va/{chassis}/{appid}")]
        public ActionResult MVCClaimDirectorVerification(long appid, string chassis)
        {
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            GetDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();
            GetDetails.MvcClaimWorkFlowDetails = _IMBClaimsBLL.MvcClaimWorkFlowDetailsBLL(appid, chassis);
            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.VehicleChassisPolicyDetailsList.OD_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.OD_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(appid, category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
            GetDetails.JudgementRemarksList = _IMBClaimsBLL.RemarksJudgementBLL();
            GetDetails.ObjectStatementRemarkList = _IMBClaimsBLL.RemarksObjectionStatementBLL(category);
            GetDetails.OpinionStatusList = _IMBClaimsBLL.GetRemarksUpperCourtBLL();
            GetDetails.PaymentRemarkList = _IMBClaimsBLL.RemarksPaymentStatementBLL(category);

            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/Covering_Letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].CoveringLetter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DL/"))
                {
                    GetDetails.MVCAppDocDetails[0].DL = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Insurance_Copy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].DriverstatementandRc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].Prefilled_Claim_Form = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Authorization_letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].authorization_letter = GetDetails.MVCAppDocDetails[i].Accident_details;
                    GetDetails.authorization_check = 1;
                }
                if (path.Contains("/ParawiseRemarksSubmissionToLawyer/"))
                {
                    GetDetails.MVCAppDocDetails[0].court_parawise = GetDetails.MVCAppDocDetails[i].Accident_details;
                    // GetDetails.authorization_check = 1;
                }
                if (path.Contains("/ObjectStatement_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_object_statement_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                    // GetDetails.authorization_check = 1;
                }
                if (path.Contains("/RatificationLetterLawDept/"))
                {
                    GetDetails.MVCAppDocDetails[0].RatificationToLawDept = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/RatificationLetterToKGID/"))
                {
                    GetDetails.MVCAppDocDetails[0].RatificationToKgid = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/LowerCourtJudgementCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].LowerCourtJudgementCopy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/opinionFromLawDept/"))
                {
                    GetDetails.MVCAppDocDetails[0].opinionLawfromLawDept = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DelayNotetoGovtAdvocateHighCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].DelayNoteHighCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/CondonationOfDelay/"))
                {
                    GetDetails.MVCAppDocDetails[0].CondonationOfDelay = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/StayAffidavitHighCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayAffidavitHighCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/GroundsofAppeal/"))
                {
                    GetDetails.MVCAppDocDetails[0].GroundsofAppeal = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/AmountDepostionLetterToLowerCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmtDepositLC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/AmountDepostionLetterToLowerCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtJudgementOpinion = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtJudgementOpinionFormLawDeptDetails/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtOpinionDesc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/High_Court_Notice/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtDepositAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/High_Court_PetitionCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimAwardedAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtCoveringLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimSettleCost = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtAuthorizationLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtClaimSettleTotalAmnt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/HighCourtJudgementOpinionFormLawDeptDetails2/"))
                {
                    GetDetails.MVCAppDocDetails[0].HighCourtOpinionJudgement2 = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DelayNotetoGovtAdvocateSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].InputDelaySupremeCourt = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/CondonationOfDelaySupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].CondonationOfDelaySupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/StayAffidavitSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayAffidavitSupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/GroundsofAppealSupremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].GroundsofAppealSupremeC = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/AmountDepositiontoSupreme_Court/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmountDepositiontoSupremeCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Amount_Deposition_supremeCourtTOLowercourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].AmountDepositiontoSupremeCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Intimation_DistrictKGID_from_supremeCourt/"))
                {
                    GetDetails.MVCAppDocDetails[0].IntimationToDistrictCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/StayOrderfromSupremeCourtToDistrictKGIDOffice/"))
                {
                    GetDetails.MVCAppDocDetails[0].StayOrderToToDistrictSupremeCToLCfile = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeCourtJudgementCopyandopinionfromLawDeptReceivedatKGID/"))
                {
                    GetDetails.MVCAppDocDetails[0].SupremeOpinionDesc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Supreme_Court_PetitionCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Statuatory_Amount_Remitted = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Supreme_Court_Notice/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Deposit_Amount_Remitted = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeCourtCoveringLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Total_Amount = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeCourtAuthorizationLetter/"))
                {
                    GetDetails.MVCAppDocDetails[0].Supreme_Court_Cost = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/SupremeOpinionNoticeFromLawDept2/"))
                {
                    GetDetails.MVCAppDocDetails[0].awardedAmount_highCourtClaimSttleKGID = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
            }
            return View(GetDetails);
        }

        public JsonResult MvcClaimChangesEdit(string chassis, long Appno)
        {
                  var loginId = Convert.ToInt64(Session["UID"]);
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();

            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);

            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(Appno);
            GetDetails.VehicleChassisPolicyDetailsList.OD_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.OD_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_from_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.VehicleChassisPolicyDetailsList.TP_to_date1 = (GetDetails.VehicleChassisPolicyDetailsList.TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(Appno);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(Appno,category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.CourtDetailsList[0].Court_DateTime = (GetDetails.CourtDetailsList[0].Court_DateTime);
            GetDetails.CourtDetailsList[0].CourtTime2 = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("yyyy-MM-dd");
            GetDetails.CourtDetailsList[0].CourtTime3 = Convert.ToDateTime(GetDetails.CourtDetailsList[0].accident_DateTime).ToString("yyyy-MM-ddThh:mm");
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(Appno);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(Appno);
            for (int i = 0; i<GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].PreClaimedForm = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].Accident_fir_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Covering_Letter/"))
                {
                  GetDetails.MVCAppDocDetails[0].cover_letter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                      GetDetails.MVCAppDocDetails[0].insurancecopy = GetDetails.MVCAppDocDetails[i].Accident_details;
                  }
           
                  if (path.Contains("/Court_Notice_details/"))
                  {
                       GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                  }
                  if (path.Contains("/Petitioner_details/"))
                   {
                          GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                     }
            
             //added new
                   if (path.Contains("/DL/"))
                    {
                             GetDetails.MVCAppDocDetails[0].Accident_dl_details = GetDetails.MVCAppDocDetails[i].Accident_details;
             }

            }

            return Json(GetDetails, JsonRequestBehavior.AllowGet);
        }
      
        public JsonResult StopMvcProcess(string chassis, long Appno)
        {
          
        var result = _IMBClaimsBLL.stopMVCFlowOnsLokadhalatSelectBLL(Appno);


            return Json(result, JsonRequestBehavior.AllowGet);
        }
    
        public JsonResult uploadAuthorizationLetter(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/Authorization_letter/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }
        public JsonResult sendDetailsToHirarchy(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else {
                model.roleID = 4;
            }
           
            var result = _IMBClaimsBLL.UpdateWork_flow_DetailsBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult sendParawiseRemarks(GetVehicleChassisPolicyDetails model)
        {

       
            var result = _IMBClaimsBLL.submitParawiseRemarksBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    public JsonResult uploadparawiseRemarksSubmit(long App_id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/ParawiseRemarksSubmissionToLawyer/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");

            
        } public JsonResult uploadFileSubmit(long App_id,string pathName)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/"+ pathName + "/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.SavePathDetailsBLL(pathServer, App_id);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");

            
        }
        public JsonResult SendDocWorkFlow(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
              if (model.Category_id == 4 && model.Remarks_id == 20)
            {
                model.roleID = 3;
            }

            var result = _IMBClaimsBLL.UpdateDocumentWork_flow_detailsBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDocumentStatus(string GetStatusData,long appId) {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();

            GetDocumentDetails.GetDocumentRemarksList = _IMBClaimsBLL.GetDocumentDetailsStatusBLL(GetStatusData, appId);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);

        }
        public JsonResult saveHearingDatesAndComments(GetVehicleChassisPolicyDetails model) {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();

           int result = _IMBClaimsBLL.saveHearingDatesAndCommentsBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);

        }
        public JsonResult saveLowerCourtOpinionDetails(GetVehicleChassisPolicyDetails model) {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();

           int result = _IMBClaimsBLL.saveLowerCourtOpinionDetailsBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);

        }
        public JsonResult saveClaimApprovalSettleLowerCourtJudgement(GetVehicleChassisPolicyDetails model) {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.saveClaimApprovalSettleLowerCourtJudgementBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        } public JsonResult saveDelayNoteToGovtHighCourt(GetVehicleChassisPolicyDetails model) {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.SaveDelayNoteToAdvocateHighCourtBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult saveAmountToDeposittedToHighCourt(GetVehicleChassisPolicyDetails model) {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.saveAmountToDeposittedToHighCourtBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UploadofAmountDepositionLetterLC(GetVehicleChassisPolicyDetails model) {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.UploadofAmountDepositionLetterLCBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult HighCourtJudgementOpinionDetails(GetVehicleChassisPolicyDetails model)
        {

            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();

            int result = _IMBClaimsBLL.HighCourtJudgementOpinionDetailsBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        } 
        public JsonResult HighCourtClaimSettlementDetails(GetVehicleChassisPolicyDetails model)
        {

            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.HighCourtClaimSettlementDetailsBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult HighCourtJudgementOpinionDetails2(GetVehicleChassisPolicyDetails model)
        {

            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();

            int result = _IMBClaimsBLL.HighCourtJudgementOpinionDetailsKGIDBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult HighCourtClaimSettlementDetails2(GetVehicleChassisPolicyDetails model)
        {

            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.HighCourtClaimSettlementDetailsKGIDBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        }
		//Chethan
		  public JsonResult MvcCourtExecution(string chassis, long Appno)
        {

            var result = _IMBClaimsBLL.stopMVCandlokadhalathOnCourtExecutionSelectBLL(Appno);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("mvc_cou_exe_cw/{chassis}/{appid}")]
        public ActionResult CourtExecutionProcessView(string chassis, long appid)
        {
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            GetDetails.DistrictList = _IMBClaimsBLL.GetDistrictListBLL();
            GetDetails.TalukaList = _IMBClaimsBLL.GetTalukListBLL(0);
            GetDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();
            GetDetails.InjuryList = _IMBClaimsBLL.GetInjuryListBLL();
            GetDetails.StateList = _IMBClaimsBLL.GetstateListBLL();
            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCCourtExecutionBLL(appid, category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.CourtDetailsList[0].Court_DateTime = (GetDetails.CourtDetailsList[0].Court_DateTime);
            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
            GetDetails.CE_DocDetails = _IMBClaimsBLL.GetCourtExecutionDocDetailsBLL(appid);
            GetDetails.OpinionStatusList = _IMBClaimsBLL.GetRemarksUpperCourtBLL();
            GetDetails.CourtExecutionMasterDetails = _IMBClaimsBLL.CourtExecutionMasterDetailsBLL(appid);

            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/Covering_Letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].CoveringLetter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DL/"))
                {
                    GetDetails.MVCAppDocDetails[0].DL = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Insurance_Copy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].DriverstatementandRc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].Prefilled_Claim_Form = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

            }
            if (GetDetails.CE_DocDetails.Count > 0)
            {
                GetDetails.doc_len = GetDetails.CE_DocDetails.Count;
                for (int i = 0; i < GetDetails.CE_DocDetails.Count; i++)
                {
                    string path = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                    if (path.Contains("/Execution_notice/"))
                    {
                        GetDetails.CE_DocDetails[0].Execution_notice = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                    }
                    if (path.Contains("/judgement_copy/"))
                    {
                        GetDetails.CE_DocDetails[0].judgement_Copy = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                    } if (path.Contains("/CEOpinionFromLawDepartment/"))
                    {
                        GetDetails.CE_DocDetails[0].SupremeOpinionDesc = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                    }
                }
                
            }
            else
            {
                GetDetails.doc_len = 0;
            }

            int dd = GetDetails.CE_DocDetails.Count;
            
            string lok_policy = GetDetails.VehicleChassisPolicyDetailsList.Policy_number;
            string lok_court_date = GetDetails.CourtDetailsList[0].CourtTime;
            string lok_court_date2 = lok_court_date.Replace("/", "");
            string lok_mvc_no = GetDetails.CourtDetailsList[0].MVC_number;
            string lok_ref_no = lok_policy + "-" + lok_court_date2 + "-" + lok_mvc_no;
            /*GetDetails.lok_ref_no = lok_ref_no;*/
            GetDetails.GetWorkFlowCOurtExecutionList = _IMBClaimsBLL.GetWorkFlowCOurtExecutionBLL(appid, chassis);

            return View(GetDetails);
        }

        public JsonResult StopCourtExecutionProcess(string chassis, long Appno)
        {

            var result = _IMBClaimsBLL.StopCourtExecutionProcessBLL(Appno);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveMVCCourtExecution(GetVehicleChassisPolicyDetails model)
        {
            long result = 0;
            model.created_by = Convert.ToInt64(Session["UID"]);
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);

            result = _IMBClaimsBLL.SaveMVCCourtExecutionBLL(model);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult uploadCourtExecutionDoc(long App_id, string filename)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/courtExecution/" + filename +"/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName1 = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName1);
                    string pathServer = fileDirectory + fileName1;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.saveCourtDocBLL(pathServer, App_id, filename);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }

        [Route("mvc_cou_exe_sup/{chassis}/{appid}")]
        public ActionResult CourtExecutionProcessViewSuperident(string chassis, long appid)
        {
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            GetDetails.DistrictList = _IMBClaimsBLL.GetDistrictListBLL();
            GetDetails.TalukaList = _IMBClaimsBLL.GetTalukListBLL(0);
            GetDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();
            GetDetails.InjuryList = _IMBClaimsBLL.GetInjuryListBLL();
            GetDetails.StateList = _IMBClaimsBLL.GetstateListBLL();
            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCCourtExecutionBLL(appid, category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.CourtDetailsList[0].Court_DateTime = (GetDetails.CourtDetailsList[0].Court_DateTime);
            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
            GetDetails.CE_DocDetails = _IMBClaimsBLL.GetCourtExecutionDocDetailsBLL(appid);
            GetDetails.OpinionStatusList = _IMBClaimsBLL.GetRemarksUpperCourtBLL();
            GetDetails.CourtExecutionMasterDetails = _IMBClaimsBLL.CourtExecutionMasterDetailsBLL(appid);
            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/Covering_Letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].CoveringLetter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DL/"))
                {
                    GetDetails.MVCAppDocDetails[0].DL = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Insurance_Copy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].DriverstatementandRc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].Prefilled_Claim_Form = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

            }

            for (int i = 0; i < GetDetails.CE_DocDetails.Count; i++)
            {
                string path = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                if (path.Contains("/Execution_notice/"))
                {
                    GetDetails.CE_DocDetails[0].Execution_notice = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                }
                if (path.Contains("/judgement_copy/"))
                {
                    GetDetails.CE_DocDetails[0].judgement_Copy = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                }
                if (path.Contains("/CEOpinionFromLawDepartment/"))
                {
                    GetDetails.CE_DocDetails[0].SupremeOpinionDesc = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                }
            }

            string lok_policy = GetDetails.VehicleChassisPolicyDetailsList.Policy_number;
            string lok_court_date = GetDetails.CourtDetailsList[0].CourtTime;
            string lok_court_date2 = lok_court_date.Replace("/", "");
            string lok_mvc_no = GetDetails.CourtDetailsList[0].MVC_number;
            string lok_ref_no = lok_policy + "-" + lok_court_date2 + "-" + lok_mvc_no;
            /*GetDetails.lok_ref_no = lok_ref_no;*/
            GetDetails.GetWorkFlowCOurtExecutionList = _IMBClaimsBLL.GetWorkFlowCOurtExecutionBLL(appid, chassis);

            return View(GetDetails);
        }

        public JsonResult CESendToAd(GetVehicleChassisPolicyDetails model)
        {

            model.created_by = Convert.ToInt64(Session["UID"]);
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);
            var result = _IMBClaimsBLL.Update_Court_execution_Work_flow_DetailsBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
         public JsonResult CESendToDir(GetVehicleChassisPolicyDetails model)
        {

            model.created_by = Convert.ToInt64(Session["UID"]);
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);
            //
            var result = _IMBClaimsBLL.Update_Court_execution_Work_flow_DetailsBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("mvc_cou_exe_AD/{chassis}/{appid}")]
        public ActionResult CourtExecutionProcessViewAD(string chassis, long appid)
        {
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            GetDetails.DistrictList = _IMBClaimsBLL.GetDistrictListBLL();
            GetDetails.TalukaList = _IMBClaimsBLL.GetTalukListBLL(0);
            GetDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();
            GetDetails.InjuryList = _IMBClaimsBLL.GetInjuryListBLL();
            GetDetails.StateList = _IMBClaimsBLL.GetstateListBLL();
            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCCourtExecutionBLL(appid, category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.CourtDetailsList[0].Court_DateTime = (GetDetails.CourtDetailsList[0].Court_DateTime);
            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
            GetDetails.CE_DocDetails = _IMBClaimsBLL.GetCourtExecutionDocDetailsBLL(appid);
            GetDetails.OpinionStatusList = _IMBClaimsBLL.GetRemarksUpperCourtBLL();
            GetDetails.CourtExecutionMasterDetails = _IMBClaimsBLL.CourtExecutionMasterDetailsBLL(appid);
            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/Covering_Letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].CoveringLetter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DL/"))
                {
                    GetDetails.MVCAppDocDetails[0].DL = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Insurance_Copy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].DriverstatementandRc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].Prefilled_Claim_Form = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

            }

            for (int i = 0; i < GetDetails.CE_DocDetails.Count; i++)
            {
                string path = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                if (path.Contains("/Execution_notice/"))
                {
                    GetDetails.CE_DocDetails[0].Execution_notice = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                }
                if (path.Contains("/judgement_copy/"))
                {
                    GetDetails.CE_DocDetails[0].judgement_Copy = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                }
                if (path.Contains("/CEOpinionFromLawDepartment/"))
                {
                    GetDetails.CE_DocDetails[0].SupremeOpinionDesc = GetDetails.CE_DocDetails[i].Court_ExecutionDetails;
                }
            }

            string lok_policy = GetDetails.VehicleChassisPolicyDetailsList.Policy_number;
            string lok_court_date = GetDetails.CourtDetailsList[0].CourtTime;
            string lok_court_date2 = lok_court_date.Replace("/", "");
            string lok_mvc_no = GetDetails.CourtDetailsList[0].MVC_number;
            string lok_ref_no = lok_policy + "-" + lok_court_date2 + "-" + lok_mvc_no;
            /*GetDetails.lok_ref_no = lok_ref_no;*/
            GetDetails.GetWorkFlowCOurtExecutionList = _IMBClaimsBLL.GetWorkFlowCOurtExecutionBLL(appid, chassis);

            return View(GetDetails);
        }
        public JsonResult saveDelayNoteToGovtSupremeCourt(GetVehicleChassisPolicyDetails model)
        {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.SaveDelayNoteToAdvocateSupremeCourtBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SupremeCourtOpinionAndJudegementCopy(GetVehicleChassisPolicyDetails model) {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.SupremeCourtOpinionAndJudegementCopyBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SupremeCourtClaimSettlementDetails(GetVehicleChassisPolicyDetails model)
        {

            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.SupremeCourtClaimSettlementDetailsBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SupremeCourtOpinionAndJudegementCopy2(GetVehicleChassisPolicyDetails model)
        {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.SupremeCourtOpinionAndJudegementCopyKGIDBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        } public JsonResult saveSupremeClaimApprovalSettlement(GetVehicleChassisPolicyDetails model)
        {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }
            int result = _IMBClaimsBLL.saveSupremeClaimApprovalSettlementBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CEClaimApprovalMethod(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }

            var result = _IMBClaimsBLL.CEClaimsettleLawDeptBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CEUpdateOpinionLawDeptMethod(GetVehicleChassisPolicyDetails model) {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }

            var result = _IMBClaimsBLL.CEUpdateOpinionLawDeptBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCEDocumentStatus(string GetStatusData, long appId)
        {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();

            GetDocumentDetails.GetDocumentRemarksList = _IMBClaimsBLL.GetCourtExecutiveDocumentDetailsStatusBLL(GetStatusData, appId);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SendCEWorkflow(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }

            var result = _IMBClaimsBLL.CEUpdateDocumentWork_flow_detailsBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveMVCLokadalathDetails(GetVehicleChassisPolicyDetails model)
        {
            long result = 0;
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);
            model.created_by = Convert.ToInt64(Session["UID"]);
            result = _IMBClaimsBLL.SaveMVCLokadalathDetailsBLL(model);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult uploadLokadalathDoc(long App_id, string filename)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/Lokadalath/" + filename + "/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName1 = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName1);
                    string pathServer = fileDirectory + fileName1;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.saveLokDocBLL(pathServer, App_id, filename);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }
        [Route("mvc_lok_Sup/{chassis}/{appid}")]
        public ActionResult LokadhalatProcessViewSuperident(string chassis, long appid)
        {
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            GetDetails.DistrictList = _IMBClaimsBLL.GetDistrictListBLL();
            GetDetails.TalukaList = _IMBClaimsBLL.GetTalukListBLL(0);
            GetDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();
            GetDetails.InjuryList = _IMBClaimsBLL.GetInjuryListBLL();
            GetDetails.StateList = _IMBClaimsBLL.GetstateListBLL();
            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetLokadhalathdetailsofCourtBLL(appid, category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.CourtDetailsList[0].Court_DateTime = (GetDetails.CourtDetailsList[0].Court_DateTime);
            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
            GetDetails.Lok_DocDetails = _IMBClaimsBLL.GetLokDocDetailsBLL(appid);
            GetDetails.GetWorkFlowLokList = _IMBClaimsBLL.GetWorkFlowLokBLL(appid, chassis);
            GetDetails.LokadalathDetails = _IMBClaimsBLL.GetLokadalathDetailsBLL(appid);
            GetDetails.LokadhalatMasterDetails = _IMBClaimsBLL.GetLokadhalatMasterDetailsBLL(appid);
            GetDetails.OpinionStatusList = _IMBClaimsBLL.GetRemarksUpperCourtBLL();
            GetDetails.JudgementRemarksList = _IMBClaimsBLL.RemarksJudgementBLL();
            // GetDetails.LokadalathDetails[0].Lokadalath_view_date;
            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/Covering_Letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].CoveringLetter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DL/"))
                {
                    GetDetails.MVCAppDocDetails[0].DL = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Insurance_Copy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].DriverstatementandRc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].Prefilled_Claim_Form = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

            }
            if (GetDetails.Lok_DocDetails.Count > 0)
            {
                for (int i = 0; i < GetDetails.Lok_DocDetails.Count; i++)
                {
                    string path = GetDetails.Lok_DocDetails[i].Lok_doc_Details;
                    if (path.Contains("/Lokadalath_notice/"))
                    {
                        GetDetails.Lok_DocDetails[0].Lok_doc_Details = GetDetails.Lok_DocDetails[i].Lok_doc_Details;
                    }
                    if (path.Contains("/opinionJudgementCopyFromLawDept/"))
                    {
                        GetDetails.Lok_DocDetails[0].RatificationToLawDept = GetDetails.Lok_DocDetails[i].Lok_doc_Details;

                    }
                    if (path.Contains("/JudgementOpininonSupremeCopyFromLawDept/"))
                    {
                        GetDetails.Lok_DocDetails[0].judgement_Copy = GetDetails.Lok_DocDetails[i].Lok_doc_Details;

                    }
                    if (path.Contains("/JudgementCopySupremeCourt2/"))
                    {
                        GetDetails.Lok_DocDetails[0].judgement_Copy_supreme = GetDetails.Lok_DocDetails[i].Lok_doc_Details;

                    }
                }

            }



            return View(GetDetails);
        }

        public JsonResult LokSendToAd(GetVehicleChassisPolicyDetails model)
        {

            model.created_by = Convert.ToInt64(Session["UID"]);
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);
            model.roleID = 15;
            if (model.Remarks_id == 20) {
                model.roleID = 3;
            }
            var result = _IMBClaimsBLL.Update_Lokadalath_Work_flow_DetailsBLL(model);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("mvc_lok_AD/{chassis}/{appid}")]
        public ActionResult LokadhalatProcessViewAD(string chassis, long appid)
        {
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            GetDetails.DistrictList = _IMBClaimsBLL.GetDistrictListBLL();
            GetDetails.TalukaList = _IMBClaimsBLL.GetTalukListBLL(0);
            GetDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();
            GetDetails.InjuryList = _IMBClaimsBLL.GetInjuryListBLL();
            GetDetails.StateList = _IMBClaimsBLL.GetstateListBLL();
            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetLokadhalathdetailsofCourtBLL(appid, category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.CourtDetailsList[0].Court_DateTime = (GetDetails.CourtDetailsList[0].Court_DateTime);
            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
            GetDetails.Lok_DocDetails = _IMBClaimsBLL.GetLokDocDetailsBLL(appid);
            GetDetails.GetWorkFlowLokList = _IMBClaimsBLL.GetWorkFlowLokBLL(appid, chassis);
            GetDetails.LokadalathDetails = _IMBClaimsBLL.GetLokadalathDetailsBLL(appid);
            GetDetails.OpinionStatusList = _IMBClaimsBLL.GetRemarksUpperCourtBLL();
            GetDetails.JudgementRemarksList = _IMBClaimsBLL.RemarksJudgementBLL();
            GetDetails.LokadhalatMasterDetails = _IMBClaimsBLL.GetLokadhalatMasterDetailsBLL(appid);
    
            // GetDetails.LokadalathDetails[0].Lokadalath_view_date;
            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/Covering_Letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].CoveringLetter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DL/"))
                {
                    GetDetails.MVCAppDocDetails[0].DL = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Insurance_Copy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].DriverstatementandRc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].Prefilled_Claim_Form = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

            }
            if (GetDetails.Lok_DocDetails.Count > 0)
            {
                for (int i = 0; i < GetDetails.Lok_DocDetails.Count; i++)
                {
                    string path = GetDetails.Lok_DocDetails[i].Lok_doc_Details;
                    if (path.Contains("/Lokadalath_notice/"))
                    {
                        GetDetails.Lok_DocDetails[0].Lok_doc_Details = GetDetails.Lok_DocDetails[i].Lok_doc_Details;
                    }
                    if (path.Contains("/opinionJudgementCopyFromLawDept/"))
                    {
                        GetDetails.Lok_DocDetails[0].RatificationToLawDept = GetDetails.Lok_DocDetails[i].Lok_doc_Details;

                    }
                    if (path.Contains("/JudgementOpininonSupremeCopyFromLawDept/"))
                    {
                        GetDetails.Lok_DocDetails[0].judgement_Copy = GetDetails.Lok_DocDetails[i].Lok_doc_Details;

                    }
                    if (path.Contains("/JudgementCopySupremeCourt2/"))
                    {
                        GetDetails.Lok_DocDetails[0].judgement_Copy_supreme = GetDetails.Lok_DocDetails[i].Lok_doc_Details;

                    }
                }

            }



            return View(GetDetails);
        }
        [Route("mvc_lok_cw/{chassis}/{appid}")]
        public ActionResult LokadhalatProcessView(string chassis, long appid)
        {
            var category = Convert.ToInt32(Session["SelectedCategory"]);
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            GetDetails.OpinionStatusLokadhalat = _IMBClaimsBLL.GetRemarksLokadhalatCourtBLL();
            GetDetails.OpinionStatusList = _IMBClaimsBLL.GetRemarksUpperCourtBLL();
            
            GetDetails.DistrictList = _IMBClaimsBLL.GetDistrictListBLL();
            GetDetails.TalukaList = _IMBClaimsBLL.GetTalukListBLL(0);
            GetDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();
            GetDetails.InjuryList = _IMBClaimsBLL.GetInjuryListBLL();
            GetDetails.StateList = _IMBClaimsBLL.GetstateListBLL();
            GetDetails.VehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetLokadhalathdetailsofCourtBLL(appid, category);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.CourtDetailsList[0].Court_DateTime = (GetDetails.CourtDetailsList[0].Court_DateTime);
            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
            GetDetails.Lok_DocDetails = _IMBClaimsBLL.GetLokDocDetailsBLL(appid);
            GetDetails.GetWorkFlowLokList = _IMBClaimsBLL.GetWorkFlowLokBLL(appid, chassis);
            GetDetails.LokadalathDetails = _IMBClaimsBLL.GetLokadalathDetailsBLL(appid);

            GetDetails.JudgementRemarksList = _IMBClaimsBLL.RemarksJudgementBLL();
            GetDetails.LokadhalatMasterDetails = _IMBClaimsBLL.GetLokadhalatMasterDetailsBLL(appid);
            // GetDetails.LokadalathDetails[0].Lokadalath_view_date;
            for (int i = 0; i < GetDetails.MVCAppDocDetails.Count; i++)
            {
                string path = GetDetails.MVCAppDocDetails[i].Accident_details;

                if (path.Contains("/Covering_Letter/"))
                {
                    GetDetails.MVCAppDocDetails[0].CoveringLetter = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DL/"))
                {
                    GetDetails.MVCAppDocDetails[0].DL = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/InsuranceCopy/"))
                {
                    GetDetails.MVCAppDocDetails[0].Insurance_Copy = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/DsRc/"))
                {
                    GetDetails.MVCAppDocDetails[0].DriverstatementandRc = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/PrefilledClaimForm/"))
                {
                    GetDetails.MVCAppDocDetails[0].Prefilled_Claim_Form = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

            }
            if (GetDetails.Lok_DocDetails.Count > 0)
            {
                for (int i = 0; i < GetDetails.Lok_DocDetails.Count; i++)
                {
                    string path = GetDetails.Lok_DocDetails[i].Lok_doc_Details;
                    if (path.Contains("/Lokadalath_notice/"))
                    {
                        GetDetails.Lok_DocDetails[0].Lok_doc_Details = GetDetails.Lok_DocDetails[i].Lok_doc_Details;
                    }
                    if (path.Contains("/opinionJudgementCopyFromLawDept/")) {
                        GetDetails.Lok_DocDetails[0].RatificationToLawDept = GetDetails.Lok_DocDetails[i].Lok_doc_Details;

                    } if (path.Contains("/JudgementOpininonSupremeCopyFromLawDept/")) {
                        GetDetails.Lok_DocDetails[0].judgement_Copy = GetDetails.Lok_DocDetails[i].Lok_doc_Details;

                    }
                    if (path.Contains("/JudgementCopySupremeCourt2/"))
                    {
                        GetDetails.Lok_DocDetails[0].judgement_Copy_supreme = GetDetails.Lok_DocDetails[i].Lok_doc_Details;

                    }
                }

            }



            return View(GetDetails);
        }

        public JsonResult StopLokadhalatProcess(string chassis, long Appno)
        {

            var result = _IMBClaimsBLL.stopLokadhalatFlowOnSelectBLL(Appno);


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult saveJudgementCopyDetails(GetVehicleChassisPolicyDetails model)
        {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();

            int result = _IMBClaimsBLL.saveJudgementCopyDetailsBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);

        }
        public JsonResult uploadLOkFileSubmit(long App_id, string pathName)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string moblie_number = string.Empty;
                    string fileDirectory = string.Empty;

                    fileDirectory = "/Content//MVC_Claim_files/" + App_id + "/Lokadhalat/" + pathName + "/";
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath(fileDirectory));
                    string path = Path.Combine(Server.MapPath(fileDirectory), fileName);
                    string pathServer = fileDirectory + fileName;

                    // save the file
                    file.SaveAs(path);
                    var res = _IMBClaimsBLL.saveLokadhalatDocBLL( App_id, pathServer);
                    return Json("File uploaded successfully", JsonRequestBehavior.AllowGet);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");


        }
        public JsonResult SendLokadhalatDocWorkFlow(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }

            var result = _IMBClaimsBLL.UpdateLokadhalatDocumentWork_flow_detailsBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLokadhalatDocumentStatus(string GetStatusData, long appId)
        {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();

            GetDocumentDetails.GetDocumentRemarksList = _IMBClaimsBLL.GetLokadhalatDocumentDetailsStatusBLL(GetStatusData, appId);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);

        }
        public JsonResult saveSupremeJudgementCopyDetails(GetVehicleChassisPolicyDetails model)
        {
            GetVehicleChassisPolicyDetails GetDocumentDetails = new GetVehicleChassisPolicyDetails();

            int result = _IMBClaimsBLL.saveJudgementCopyDetailsBLL(model);
            return Json(GetDocumentDetails, JsonRequestBehavior.AllowGet);

        }
        public JsonResult LokClaimApprovalMethod(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }

            var result = _IMBClaimsBLL.LokClaimsettleLawDeptBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LokJudgementSupremeMethod(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 4)
            {
                model.roleID = 15;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }

            var result = _IMBClaimsBLL.LokClaimsettleLawDeptBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendDirectorApprovalDocWorkFlow(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt32(Session["SelectedCategory"]);//Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            if (model.Category_id == 6 || model.Category_id==15)
            {
                model.roleID = 7;

            }
            else if (model.Category_id == 3)
            {
                model.roleID = 4;
            }
            else
            {
                model.roleID = 3;
            }

            var result = _IMBClaimsBLL.UpdateDocumentWork_flow_detailsBLL(model); 
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult saveVehicleNumber(string vehicle_registration_no ,string chassisNo)
        {
           
            var result = _IMBClaimsBLL.saveVehicleNumberBLL(vehicle_registration_no,chassisNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}