using BLL.KGIDMotorInsurance;
using BLL.KGIDMIMasterBLL;
using Common;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using KGID_Models.KGID_Policy;
using System.Net;
/// <KII_Integration_References>
using com.sun.org.apache.xml.@internal.security.c14n;
using com.sun.org.apache.xml.@internal.security.utils;
using java.io;
using java.security;
using javax.crypto;
using javax.crypto.spec;
using javax.xml.stream;
using javax.xml.stream.events;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Net.Cache;
using log4net;
using DLL.DBConnection;
using KGID_Models.KGID_Master;

using Renci.SshNet;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Web.Configuration;

namespace KGID.Controllers
{

    public class MotorInsuranceController : Controller
    {

        private readonly IMotorInsuranceProposerDetailsBll _IMotorInsuranceProposerDetailsBll;
        private readonly IMotorInsuranceVehicleDetailsBll _IMotorInsuranceVehicleDetailsBll;
        private readonly IMotorInsuranceRenewalDetailsBll _IMotorInsuranceRenewalDetailsBll;

        public MotorInsuranceController()
        {
            this._IMotorInsuranceProposerDetailsBll = new MotorInsuranceProposerDetailsBll();
            this._IMotorInsuranceVehicleDetailsBll = new MotorInsuranceVehicleDetailsBll();
            this._IMotorInsuranceRenewalDetailsBll = new MotorInsuranceRenewalDetailsBll();
        }

        // GET: MotorInsurance
        public ActionResult Index()
        {
            return View();
        }
        //[HttpGet]
        [Route("mi-e-fa")]
        public ActionResult MotorInsuranceEmployeeApplication()
        {
            Session.Remove("RID");
            string result = _IMotorInsuranceVehicleDetailsBll.GetEmployeeLoanDetails(Convert.ToInt64(Session["UID"]));
            if(result=="")
            {
                result = "1";
            }
            

            if (result == "1" || result == "2")
            {
                return View();
            }
            else if (result == "3" || result == "5"|| result == "7" || result == "11" ||result=="15")
            {
                return RedirectToAction("MotorInsuranceStatusOfApplication");
            }
            else
            {
                VM_MIApplicationDetails obj = new VM_MIApplicationDetails();
                ViewBag.Message = String.Format("Not eligible,will be applicable only for loan taken employees");
                return View(obj);
            }

        }

        //[HttpGet]
        [Route("mi-dpt-faf/{PageType?}/{refNo?}/{category?}")]
        [Route("mi-agy-fa/{PageType?}/{refNo?}/{category?}")]
        [Route("mi-loa-edt-apln/{PageType?}/{refNo?}/{category?}/{status?}")]
        [Route("mi-soa-edt-apln/{PageType?}/{refNo?}/{category?}")]
        public ActionResult MotorInsuranceApplication(string PageType = null, long refNo = 0, string category = "",int status=0)
        {
            Session["RID"] = (refNo != 0) ? refNo : 0;

            VM_MIApplicationDetails obj = new VM_MIApplicationDetails();

            obj.Type = Convert.ToString((PageType== "empty")?"": PageType);
            if(status==2)
            {
                obj.SentBackAppliaction = 1;
            }
            else if (status == 94)
            {
                obj.SentBackAppliaction = 94;
            }
            else
            {
                obj.SentBackAppliaction = 0;
            }
            
            return View(obj);
        }

        //=========================View Data related functions ================================
        #region View Data related functions
        //[HttpGet]
       // [Route("ProposerDetailsToView")]
        public ActionResult ProposerDetailsToView(string PageType, string refNo = "")
        {
            VM_MotorInsuranceProposerDetails _ProposerDetail = new VM_MotorInsuranceProposerDetails();
            try
            {
                if (Convert.ToInt32(Session["SelectedCategory"]) == 2)
                {
                    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), (PageType == null) ? "" : PageType, (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["SelectedCategory"]));
                }
                else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
                {
                    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), (PageType == null) ? "" : PageType, (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["Categories"]));
                }
                else if (Convert.ToInt32(Session["SelectedCategory"]) == 1)
                {
                    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), "Emp", (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["SelectedCategory"]));
                }
               
            }
            catch(Exception ex)
            {
               
            }
          
            if (!String.IsNullOrEmpty(_ProposerDetail.mipd_kgid_application_number))
            {
                Session["RID"] = _ProposerDetail.mipd_kgid_application_number;
                TempData["Deptmartment"] = _ProposerDetail.mipd_Department;
                using (MemoryStream ms = new MemoryStream())
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(_ProposerDetail.mipd_kgid_application_number.ToString(), QRCodeGenerator.ECCLevel.Q);
                    using (Bitmap bitMap = qrCode.GetGraphic(20))
                    {
                        bitMap.Save(ms, ImageFormat.Png);
                        _ProposerDetail.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return this.PartialView("_ProposerDetails", _ProposerDetail);
        }
        //[HttpGet]
        //[Route("VehicleDetailsToView")]
        public ActionResult VehicleDetailsToView()
        {
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetMIVehicleDetails(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));

            if(_VehicleDetails.mipd_type_of_cover_id!=0)
            {
                if (_VehicleDetails.mipd_type_of_cover_id == 3)
                {
                    _VehicleDetails.mipd_type_of_cover_list = _VehicleDetails.mipd_type_of_cover_list.Where(a => a.Value == "3").Select(a => a).ToList();
                }
                else if (_VehicleDetails.mipd_type_of_cover_id != 2 || _VehicleDetails.mipd_type_of_cover_id != 1)
                {
                    _VehicleDetails.mipd_type_of_cover_list = _VehicleDetails.mipd_type_of_cover_list.Where(a => a.Value != "3").Select(a => a).ToList();
                }
                else
                {
                    _VehicleDetails.mipd_type_of_cover_list = _VehicleDetails.mipd_type_of_cover_list;
                }
            }
            
            return this.PartialView("_VehicleDetails", _VehicleDetails);
        }

        public string CheckVehicleExists(VM_MotorInsuranceVehicleDetails details)
        {
            string isMatching = "";
            string chassisNo = details.mivd_chasis_no;
            string EngineNo = details.mivd_engine_no;
            string result = _IMotorInsuranceVehicleDetailsBll.CheckVehicleExists(chassisNo, EngineNo);
            if(result!="")
            {
                string[] resultsplit = result.Split('$');
                if (resultsplit.Length > 0)
                {
                    foreach (var item in resultsplit)
                    {
                        if (item != "0" )
                        {
                            string[] ref_off = item.Split('~');
                            if (ref_off[1] == Convert.ToString(Session["UID"]))
                            {
                                if (ref_off[0] == Convert.ToString(Session["RID"]))
                                {

                                    isMatching = "yes" + "@";
                                }
                                else
                                {
                                    isMatching = "no" + "@ref";
                                }

                            }
                            else
                            {
                                isMatching = "no" + "@usid@" + ref_off[2];
                            }
                        }
                        else
                        {

                            if (item == Convert.ToString(Session["RID"]) || item == "0")
                            {
                                isMatching = "yes";
                            }
                            else
                            {
                                isMatching = "no";
                            }
                        }

                    }
                }

            }
            else
            {
                isMatching = "yes";
            }


            return isMatching;
        }

        //[HttpGet]
        //[Route("OtherDetailsToView")]
        public ActionResult OtherDetailsToView()
        {
            VM_MotorInsuranceOtherDetails _OtherData = _IMotorInsuranceVehicleDetailsBll.OtherDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));
            if (_OtherData == null)
            {
                _OtherData = new VM_MotorInsuranceOtherDetails();
            }
            return this.PartialView("_OtherDetails", _OtherData);
        }
        //[HttpGet]
        //[Route("IDVDetailsToView")]
        public ActionResult IDVDetailsToView()
        {
            VM_MotorInsuranceIDVDetails _IDVData = _IMotorInsuranceVehicleDetailsBll.IDVDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));
            if (_IDVData == null)
            {
                _IDVData = new VM_MotorInsuranceIDVDetails();
            }
            return this.PartialView("_IDVDetails", _IDVData);
        }
        //[HttpGet]
        //[Route("PreviousHistoryToView")]
        public ActionResult PreviousHistoryToView()
        {
            //ViewBag.TypeofCover = "Package Policy";
            VM_MotorInsurancePreviousHistoryDetails onjPreviousHistory = _IMotorInsuranceVehicleDetailsBll.PreviousHistoryDetails(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));

            return this.PartialView("_PreviousHistoryMI", onjPreviousHistory);
        }
        //[HttpGet]
        //[Route("DeclarationToView")]
        public ActionResult DeclarationToView()
        {
            //VM_MotorInsuranceProposerDetails obj = new VM_MotorInsuranceProposerDetails();
            //obj.mipd_Department = TempData["Deptmartment"] as string;
            //if(Session["RID"]!=null)
            //{
            //    obj.mipd_kgid_application_number = Session["RID"].ToString();
            //}
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetMIVehicleDetails(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));
            
            return this.PartialView("_DeclarationFormMI", _VehicleDetails);
        }
        //[HttpGet]
        //[Route("MIDocumentDetailsToView")]
        public ActionResult MIDocumentDetailsToView()
        {
            VM_MI_Upload_Documents _MIDocumentData = _IMotorInsuranceVehicleDetailsBll.MIDocumentDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));
            if (_MIDocumentData == null)
            {
                _MIDocumentData = new VM_MI_Upload_Documents();
            }
            return this.PartialView("_MIDocumentDetails", _MIDocumentData);
        }
        #endregion
        //=========================Insert Data related functions ================================
        #region Insert Data related functions
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [Route("InsertMIVehicleDetails")]
        public JsonResult InsertMIVehicleDetails(VM_MotorInsuranceVehicleDetails objMVDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            decimal OwnDamageV = 0;
            decimal PremiumLiabilityV = 0;
            int MinValue = 0;
            int Depreciation = 0;
            string Zone = "";
            string Pagetype = "";
            decimal AdditionalAmt = 0;
            decimal govDiscount = 0;
            decimal PLgovDiscount = 0;
            decimal PLDriverAmt = 0;
            decimal PLPassengerAmt = 0;
            string result = _IMotorInsuranceVehicleDetailsBll.SaveMIVehicleDetailsData(objMVDetails);
            result = result.Trim().Replace("\r\n", string.Empty);

            string[] res = result.Split(',');


            if (objMVDetails.mivd_pagetype == "Renewal" || objMVDetails.mivd_pagetype == "EditRenewal")
            {
                if (!string.IsNullOrEmpty(result))
                {
                    isSuccess = true;
                    message = "Vehicle details saved successfully";
                    
                    OwnDamageV = Convert.ToDecimal(objMVDetails.mivd_own_damage_value);
                    PremiumLiabilityV = Convert.ToDecimal(objMVDetails.mivd_premium_liability_value);
                    MinValue = Convert.ToInt32(objMVDetails.mivd_vehicle_min_value);
                    Depreciation = Convert.ToInt32(objMVDetails.mivd_Depreciation_value);
                    Zone = objMVDetails.mivd_Zone;
                    AdditionalAmt = objMVDetails.mivd_Additionalamt;
                    govDiscount = objMVDetails.mivd_govDiscount;
                    PLgovDiscount = objMVDetails.mivd_PLgovDiscount;
                    PLDriverAmt = objMVDetails.mivd_PLDriverAmt;
                    PLPassengerAmt = objMVDetails.mivd_PLPassengerAmt;
                    Pagetype = objMVDetails.mivd_pagetype;


                }
                return Json(new { IsSuccess = isSuccess, Message = message, OwnDamageValue = OwnDamageV,
                    PremiumLiabilityValue = PremiumLiabilityV, VehMinValue = MinValue, DepreciationValue = Depreciation,
                    Zonetype = res.Length,
                    AdditionAmt= AdditionalAmt,
                    govDiscnt= govDiscount,
                    PLgovDiscountValue= PLgovDiscount,
                    PLDriverAmtValue= PLDriverAmt,
                    PLPassengerAmtValue= PLPassengerAmt,
                    Pagetype = objMVDetails.mivd_pagetype,

            }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                if (res.Length > 9)
                {
                    isSuccess = true;
                    message = "Vehicle details saved successfully";

                    OwnDamageV = Convert.ToDecimal(res[0]);
                    PremiumLiabilityV = Convert.ToDecimal(res[1]);
                    MinValue = Convert.ToInt32(res[2]);
                    Depreciation = Convert.ToInt32(res[3]);
                    Zone = res[4];
                    AdditionalAmt = Convert.ToDecimal(res[5]);
                    govDiscount = Convert.ToDecimal(res[6]);
                    PLgovDiscount = Convert.ToDecimal(res[7]);
                    PLDriverAmt = Convert.ToDecimal(res[8]);
                    PLPassengerAmt = Convert.ToDecimal(res[9]);
                    return Json(new { IsSuccess = isSuccess,
                        Message = message,
                        OwnDamageValue = OwnDamageV,
                        PremiumLiabilityValue = PremiumLiabilityV,
                        VehMinValue = MinValue,
                        DepreciationValue = Depreciation, Zonetype = Zone,
                        AdditionAmt = AdditionalAmt,
                        govDiscnt = govDiscount,
                        PLgovDiscountValue = PLgovDiscount,
                        PLDriverAmtValue = PLDriverAmt,
                        PLPassengerAmtValue = PLPassengerAmt,
                        Pagetype = objMVDetails.mivd_pagetype
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        Zone = result;
                        isSuccess = false;
                        message = "Cannot proceed,User belong to " + result + " .Please select other category";
                    }
                    if (string.IsNullOrEmpty(result))
                    {
                        Zone = result;
                        isSuccess = false;
                        message = "Cannot proceed further,please try again later.";
                    }
                    return Json(new { IsSuccess = isSuccess, Message = message, Zonetype = res.Length }, JsonRequestBehavior.AllowGet);

                }
            }
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [Route("InsertMIAppnReferenceno")]
        public JsonResult InsertMIAppnReferenceno(VM_MotorInsuranceProposerDetails objMIPDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            long RefNo = 0;
            string QRCode = string.Empty;
            long result = 0;
            objMIPDetails.mipd_employee_id = Convert.ToInt64(Session["UID"]);
            objMIPDetails.mipd_kgid_application_number = (Convert.ToString(Session["RID"]) == "0" ? "" : Convert.ToString(Session["RID"]));
            objMIPDetails.mipd_category = Convert.ToString(Session["Categories"]);

            if (objMIPDetails.mipd_pagetype == "Renewal")
            {

                if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
                {
                    objMIPDetails.mipd_category = Convert.ToString(Session["SelectedCategory"]);
                    result = _IMotorInsuranceProposerDetailsBll.SaveMIRenewalProposalAppnRefNo(objMIPDetails);
                }
                else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
                {
                    result = _IMotorInsuranceProposerDetailsBll.SaveMIRenewalProposalAppnRefNo(objMIPDetails);
                }
                else
                {
                    result = _IMotorInsuranceProposerDetailsBll.SaveMIRenewalProposalAppnRefNo(objMIPDetails);
                }
            }
            else
            {
                if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
                {
                    objMIPDetails.mipd_category = Convert.ToString(Session["SelectedCategory"]);
                    result = _IMotorInsuranceProposerDetailsBll.SaveMIProposalAppnRefNo(objMIPDetails);
                }
                else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
                {
                    result = _IMotorInsuranceProposerDetailsBll.SaveMIProposalAppnRefNo(objMIPDetails);
                }

                else
                {
                    objMIPDetails.mipd_category = Convert.ToString(Session["SelectedCategory"]);
                    result = _IMotorInsuranceProposerDetailsBll.SaveMIProposalAppnRefNo(objMIPDetails);
                }


            }

            if (result > 2)
            {
                RefNo = result;
                Session["RID"] = result;
                isSuccess = true;
                message = "Proposer details saved successfully";
                //QR Code 
                using (MemoryStream ms = new MemoryStream())
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(result.ToString(), QRCodeGenerator.ECCLevel.Q);
                    using (Bitmap bitMap = qrCode.GetGraphic(20))
                    {
                        bitMap.Save(ms, ImageFormat.Png);
                        QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            else if (result != 0)
            {
                RefNo = Convert.ToInt64(Session["RID"]);
                isSuccess = true;
                message = "Proposer details saved successfully";
            }

            return Json(new { IsSuccess = isSuccess, Message = message, ReferenceNo = RefNo, QRCodeImage = QRCode }, JsonRequestBehavior.AllowGet);
        }
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[Route("InsertOtherDetails")]
        public JsonResult InsertOtherDetails(VM_MotorInsuranceOtherDetails objPersonalDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            objPersonalDetails.miod_application_id = Convert.ToInt64(Session["RID"]);
            if (objPersonalDetails.miod_application_id != 0)
            {
                int result = _IMotorInsuranceVehicleDetailsBll.SaveOtherDetailsBll(objPersonalDetails);

                if (result != 0)
                {
                    isSuccess = true;
                    message = "Other details saved successfully";
                }
            }
            else
            {
                isSuccess = false;
                message = "Other details could not saved";
            }
            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[Route("InsertIDVDetails")]
        public JsonResult InsertIDVDetails(VM_MotorInsuranceIDVDetails objPersonalDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            objPersonalDetails.miidv_application_id = Convert.ToInt64(Session["RID"]);
            if (objPersonalDetails.miidv_application_id != 0)
            {
                if(Convert.ToInt32(objPersonalDetails.premium_amount)!=0)
                {
                    int result = _IMotorInsuranceVehicleDetailsBll.SaveIDVDetailsBll(objPersonalDetails);
                    if (result != 0)
                    {
                        isSuccess = true;
                        message = "IDV details saved successfully";
                    }
                }
                else
                {
                    isSuccess = false;
                    message = "IDV details could not saved";
                }

            }
            else
            {
                isSuccess = false;
                message = "IDV details could not saved";
            }
            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[Route("InsertMIDocumentDetails")]
        public JsonResult InsertMIDocumentDetails(VM_MI_Upload_Documents objMIDocDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            objMIDocDetails.App_Proposer_ID = Convert.ToInt64(Session["UID"]);
            objMIDocDetails.MI_App_Reference_ID = Convert.ToInt64(Session["RID"]);
            if (objMIDocDetails.MI_App_Reference_ID != 0)
            {
                int result = _IMotorInsuranceVehicleDetailsBll.SaveMIDocumentDetailsBll(objMIDocDetails);
                if (result != 0)
                {
                    isSuccess = true;
                    message = "Document details saved successfully";
                }
            }
            else
            {
                isSuccess = false;
                message = "Document details could not saved";
            }
            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[Route("InsertMIPreviousHistoryDetails")]
        public JsonResult InsertMIPreviousHistoryDetails(VM_MotorInsurancePreviousHistoryDetails objMIPreviousDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            string PageType = string.Empty;
            int mcb =0;
            int ncb = 0;
            if (objMIPreviousDetails.ph_reference==0)
            objMIPreviousDetails.ph_reference = Convert.ToInt64(Session["RID"]);
            if (objMIPreviousDetails.ph_reference != 0)
            {
                
                Session["ORID"] = objMIPreviousDetails.ph_reference;
                long  rr= Convert.ToInt64(Session["RID"]);
                string result = _IMotorInsuranceVehicleDetailsBll.SaveMIPreviousHistoryDetails(objMIPreviousDetails);
                
                string[] values = result.Split(',');
                if(values.Length>1)
                {
                    
                    isSuccess = true;
                    mcb = Convert.ToInt32(values[0]);
                    ncb = Convert.ToInt32(values[1]);
                    PageType = objMIPreviousDetails.mivd_pagetype;
                    message = "Previous history details saved successfully";
                    return Json(new { IsSuccess = isSuccess, Message = message, PageType = objMIPreviousDetails.mivd_pagetype, mcb = Convert.ToInt32(values[0]), ncb = Convert.ToInt32(values[1]) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    isSuccess = true; 
                    PageType = objMIPreviousDetails.mivd_pagetype;
                    message = "Previous history details saved successfully";
                    return Json(new { IsSuccess = isSuccess, Message = message, PageType = objMIPreviousDetails.mivd_pagetype }, JsonRequestBehavior.AllowGet);
                }
               
            }
            else
            {
                isSuccess = false;
                PageType = objMIPreviousDetails.mivd_pagetype;
                message = "Previous history details could not saved";
                return Json(new { IsSuccess = isSuccess, Message = message, PageType = objMIPreviousDetails.mivd_pagetype }, JsonRequestBehavior.AllowGet);
            }
        //return Json(new { IsSuccess = isSuccess, Message = message, PageType = objMIPreviousDetails.mivd_pagetype}, JsonRequestBehavior.AllowGet);
        }
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[Route("InsertMIChallanDetails")]
        string deptRefNum = string.Empty;
        long amount = 0;
        string transactiondate = string.Empty;
        string chlntransactiondate = string.Empty;
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        public JsonResult PayMIChallanDetailsKII(string RefNos, string ChallanAmount, string Type)
        {
            bool isSuccess = false;
            string message = string.Empty;
            string result = string.Empty;
            HttpCookie cookieAppRefNo = HttpContext.Request.Cookies.Get("amrn");
            HttpCookie cookieChalnAmt = HttpContext.Request.Cookies.Get("amch");
            string AppRefNo = cookieAppRefNo.Value;
            AppRefNo = System.Web.HttpUtility.UrlDecode(AppRefNo);
            string ChalnAmt = cookieChalnAmt.Value;
            //if (Request.Cookies["amrn"] != null && Request.Cookies["amch"] != null)
            //{
            //    var ar = new HttpCookie("amrn");
            //    var ca = new HttpCookie("amch");
            //    ar.Expires = DateTime.Now.AddDays(-1);
            //    ca.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(ar);
            //    Response.Cookies.Add(ca);
            //}
            if (ChallanAmount == ChalnAmt && AppRefNo == RefNos)
            {
                Session["KIIReturn"] = "MBPay";
                string dd = DateTime.Now.ToString("dd");
                string MM = DateTime.Now.ToString("MM");
                string yy = DateTime.Now.ToString("yy");
                string ddHHmmss = DateTime.Now.ToString("ddHHmmss");
                deptRefNum = "KD" + MM + yy + "8011" + ddHHmmss;
                transactiondate = DateTime.Now.ToString("ddMMyyyy");
                chlntransactiondate = DateTime.Now.ToString("dd/MM/yyyy").Replace('-', '/');
                //string testdataenc = "VhrkkSQ5YXM%2BZJ49L439AKCEJZzsN5PNVD1WMGXbOu0XTy60BJSDp8LJDWviKwGTPX0we7NfLgxa%0D%0AN7iabx7f837vvVNeNjp4dfUJTBLRN5wAxuqTLDbXRylnuM%2F2e8hZiGjY4LDeafJ53cab7dai6XIf%0D%0Axcp5gxMg1TmYN4DmpadwzNCOsKF8W8g8A7FUeW05%2F3w35rFXH1XmmWW45AVevd8Y3dDikSZlX1%2BG%0D%0AQpm9ZE%2BGf4gbQgmxP4CKObQ7W6epxpazPTxnTD30FKeMOiRKfAY9ByYLgG48QKRIrBVHZcGvZq58%0D%0A0y7MM5ZEW3rB5EMg";
                //var dec = HttpUtility.UrlDecode(testdataenc);
                //dec.Replace(" ", "");
                //var resdecData = SymmetricDecrypt(dec, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                //string SymmetricDecryptData11 = SymmetricDecrypt(testdataenc, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                //objPaymentDetails.cd_amount = 1;
                //objPaymentDetails.cd_challan_ref_no = deptRefNum;
                amount = Convert.ToInt64(1);
                //deptRefNum = objPaymentDetails.cd_challan_ref_no;
                //Log.Debug(deptRefNum);
                //Log.Debug(amount);
                ///////KII Integration Start//////
                ByteArrayOutputStream fileWriter = null;
                StringBuilder content = null;
                string currPath = string.Empty;
                string SignedresultContent = string.Empty;
                string KIIsignresponse = string.Empty;
                string resFile = TextFileCreate(Convert.ToInt64(amount), deptRefNum);
                XMLInputFactory factory = XMLInputFactory.newInstance();
                //File fileLoc = new File(filePath);
                FileReader fileReader = new FileReader(resFile);
                //XMLStreamReader reader = factory.createXMLStreamReader(fileReader);
                //content = new StringBuilder();
                // Parsing XML using stream reader and writing to a ByteArrayOutputStream
                string AsBase64String = string.Empty;
                byte[] AsBytes = System.IO.File.ReadAllBytes(resFile);
                fileReader = new FileReader(resFile);
                XMLStreamReader reader = factory.createXMLStreamReader(fileReader);
                content = new StringBuilder();
                // Parsing XML using stream reader and writing to a ByteArrayOutputStream
                while (reader.hasNext())
                {
                    int eventType = reader.next();
                    switch (eventType)
                    {

                        case XMLEvent.START_ELEMENT:

                            currPath = currPath + "/" + reader.getLocalName();
                            //Instead
                            if (currPath.Contains("data"))
                            {
                                String startTag = "";
                                //Instead
                                if (reader.getLocalName().Equals("data"))
                                {
                                    fileWriter = new ByteArrayOutputStream();
                                    startTag = "<" + reader.getLocalName();
                                    for (int k = 0; k < reader.getNamespaceCount(); k++)
                                    {
                                        if (reader.getNamespacePrefix(k) != null)
                                            startTag = startTag + " xmlns:" + reader.getNamespacePrefix(k) + "=\"" + reader.getNamespaceURI(k) + "\"";
                                        else
                                            startTag = startTag + " xmlns=\"" + reader.getNamespaceURI(k) + "\"";
                                    }
                                    startTag = startTag + ">";
                                }
                                else
                                {
                                    startTag = "<" + reader.getLocalName() + ">";
                                }

                                if (fileWriter != null)
                                    fileWriter.write(Encoding.ASCII.GetBytes(startTag));
                            }
                            break;

                        case XMLStreamConstants.CHARACTERS:
                            if (fileWriter != null)
                            {
                                fileWriter.write(Encoding.ASCII.GetBytes(reader.getText()));
                            }
                            break;

                        case XMLStreamConstants.END_ELEMENT:
                            //Instead
                            if (currPath.Contains("data"))
                            {
                                string endTag = "</" + reader.getLocalName() + ">";

                                if (fileWriter != null)
                                {
                                    fileWriter.write(Encoding.ASCII.GetBytes(endTag));
                                }
                            }
                            content = new StringBuilder();
                            //RemoveLasttag(currPath);
                            currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                            break;

                        case XMLEvent.CDATA:
                            break;
                        case XMLEvent.SPACE:
                            break;

                    }
                }
                com.sun.org.apache.xml.@internal.security.Init.init();
                Canonicalizer canon = Canonicalizer.getInstance(Canonicalizer.ALGO_ID_C14N_OMIT_COMMENTS);
                byte[] canonXmlBytes = canon.canonicalize(fileWriter.toByteArray());
                string beforesignedData = Convert.ToBase64String(canonXmlBytes);
                string beforecanonXmlData = Encoding.UTF8.GetString(AsBytes);
                string aftercanonXmlData = Encoding.UTF8.GetString(canonXmlBytes);

                //SIGN DATA WITH PFX FILE
                string xml_inBase64 = Convert.ToBase64String(AsBytes);
                string em = Encoding.UTF8.GetString(canonXmlBytes);

                //WebAPI Service Call
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri("http://49.206.243.83:9090/SignXmlData/");
                    client.BaseAddress = new Uri("http://10.96.158.108:5050/SignBinaryData/");
                    //client.BaseAddress = new Uri("http://10.96.158.49:9090/SignBinaryData/");
                    object reqdata = new
                    {
                        data = beforesignedData
                    };
                    var myContent = JsonConvert.SerializeObject(reqdata);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    /////////
                    var resultsac = client.PostAsync("getSignforDataByte", byteContent).Result;
                    SignedresultContent = resultsac.Content.ReadAsStringAsync().Result;
                    client.CancelPendingRequests();
                    //Console.WriteLine("about to dispose the client");
                    client.Dispose();
                    //return resultContent;
                    //Log.Debug(SignedresultContent);
                }
                ////////
                try
                {
                    if (SignedresultContent != null)
                    {
                        string data = GetKIISignDetails(SignedresultContent, Encoding.UTF8.GetString(AsBytes));
                        SignedDataResponseKII signedDataResponseK2 = new SignedDataResponseKII();
                        signedDataResponseK2 = JsonConvert.DeserializeObject<SignedDataResponseKII>(data);
                        //Log.Debug(data);
                        if (signedDataResponseK2.statusCode == "KII-RCTER-00" && signedDataResponseK2.statusDescription == "Success")
                        {
                            string transactiondate = DateTime.Now.ToString("ddMMyyyy");
                            //EmpId and Category
                            long EID = 0;
                            string UCat = string.Empty;
                            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
                            {
                                EID = Convert.ToInt64(Session["UID"]);
                                UCat = Convert.ToString(Session["SelectedCategory"]);
                            }
                            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
                            {
                                EID = Convert.ToInt64(Session["UID"]);
                                UCat = Convert.ToString(Session["Categories"]);
                            }
                            else
                            {
                                EID = Convert.ToInt64(Session["UID"]);
                                UCat = Convert.ToString(Session["Categories"]);
                            }

                            //tbl_mi_kii_challan_request  
                            tbl_mi_kii_challan_request objEmp = new tbl_mi_kii_challan_request();
                            // fields to be insert
                            objEmp.mi_kii_challan_ref_no = deptRefNum;
                            objEmp.mi_kii_challan_app_ref_no = RefNos;
                            objEmp.mi_kii_proposer_id = EID;
                            objEmp.mi_kii_user_category = UCat;
                            objEmp.mi_kii_challan_amount = Convert.ToString(amount);
                            objEmp.mi_kii_transaction_date = transactiondate;
                            objEmp.mi_kii_challan_active_status = false;
                            objEmp.mi_kii_created_by = EID;
                            objEmp.mi_kii_updated_by = EID;
                            objEmp.mi_kii_creation_datetime = DateTime.Now;
                            objEmp.mi_kii_updation_datetime = DateTime.Now;
                            _db.tbl_mi_kii_challan_request.Add(objEmp);
                            _db.SaveChanges();
                            // executes the commands to implement the changes to the database
                            //_db.SubmitChanges();

                            string HashChechsumMD5 = "dept_ref_no=" + deptRefNum + "|txn_date=" + transactiondate + "|amount=" + amount + "|dept_pwd=1234";
                            string AfterHashChechsumMD5 = GetMD5Checksum(HashChechsumMD5);
                            string BeforeEncryptedStringData = "dept_ref_no=" + deptRefNum + "|txn_date=" + transactiondate + "|amount=" + amount + "|dept_pwd=1234|checkSum=" + AfterHashChechsumMD5 + "";
                            string EncryptedStringData = SymetricEncrypt(BeforeEncryptedStringData, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                            //Log.Debug("Encrypt Data After Signed Data Success");
                            //Log.Debug(EncryptedStringData);
                            string SymmetricDecryptData = SymmetricDecrypt(EncryptedStringData, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                            //Log.Debug("Test Decrypt Data");
                            //Log.Debug(SymmetricDecryptData);
                            string KIIurl = "https://preprodk2.karnataka.gov.in/wps/portal/Home/DepartmentPayment/?uri=receiptsample:com.tcs.departmentpage:departmentportlet";
                            string redirect_url = "" + KIIurl + "" + "&encdata=" + EncryptedStringData + "&dept_code=12C";
                            //Log.Debug("Redirect url here");
                            //Log.Debug(redirect_url);
                            //RemotePost myremotepost = new RemotePost();
                            //myremotepost.Url = redirect_url;
                            //myremotepost.Add("surl", "https://49.206.243.82/stgkgid/Home/Return");//Change the success url here depending upon the port number of your local system.  
                            //myremotepost.Add("furl", "https://49.206.243.82/stgkgid/Home/Return");//Change the failure url here depending upon the port number of your local system.  
                            //myremotepost.Post();
                            //Log.Debug("Redirect Done");
                            //string finalresponse = GrtUrl(KIIurl, EncryptedStringData, "12C");
                            //return redirect_url;
                            result = redirect_url;
                            isSuccess = true;
                            message = redirect_url;
                        }
                        else
                        {
                            //Log.Error("redirect error");
                            isSuccess = false;
                            message = "KII Redirect error";
                            return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
                            //return null;
                            //Unable to signed the data
                        }
                    }
                    else
                    {
                        //Log.Error("signed content error");
                        //Signed Data Not Captured
                        isSuccess = false;
                        message = "Signed Data Not Captured";
                        return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    message = "Payment Unsuccessful";
                    return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
                    //Log.Error("Error Level", ex);
                }
                //return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                isSuccess = false;
                message = "Payment Unsuccessful";
            }
            return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
        }
        public string TextFileCreate(long ChallanAmount, string Refno)
        {
            // KD0221801112345678
            //string dd = DateTime.Now.ToString("dd");
            //string MM = DateTime.Now.ToString("MM");
            //string yy = DateTime.Now.ToString("yy");
            //string ddHHmmss = DateTime.Now.ToString("ddHHmmss");
            //deptRefNum = "KD" + MM + yy + "8011" + ddHHmmss;
            //var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            string newFile = Server.MapPath("~/MIDocuments/0/" + deptRefNum + ".txt");
            //string fileName = @"D:\VenkatTXFITx.txt";
            System.IO.FileInfo fi = new System.IO.FileInfo(newFile);
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (fi.Exists)
                {
                    fi.Delete();
                }

                // Create a new file     
                using (System.IO.StreamWriter strmwrtr = fi.CreateText())
                {
                    //strmwrtr.WriteLine("New file created: {0}", DateTime.Now.ToString());
                    //strmwrtr.WriteLine("Author: Venkatesh);
                    strmwrtr.WriteLine("<data>");
                    strmwrtr.WriteLine("<RctReceiveValidateChlnRq>");
                    strmwrtr.WriteLine("      <chlnDate>"+chlntransactiondate+"</chlnDate>");
                    strmwrtr.WriteLine("      <deptCode>12C</deptCode>");
                    strmwrtr.WriteLine("      <ddoCode>12026D</ddoCode>");//20139O//12028O
                    strmwrtr.WriteLine("      <deptRefNum>" + deptRefNum + "</deptRefNum>");
                    strmwrtr.WriteLine("      <rctReceiveValidateChlnDtls>");
                    strmwrtr.WriteLine("        <amount>" + ChallanAmount + "</amount>");
                    strmwrtr.WriteLine("        <deptPrpsId>2</deptPrpsId>");
                    strmwrtr.WriteLine("        <prpsName>8011~00~105~2~00~000</prpsName>");//8011~00~105~2~00~000
                    strmwrtr.WriteLine("        <subPrpsName>013</subPrpsName>");
                    strmwrtr.WriteLine("        <subDeptRefNum>"+deptRefNum+"</subDeptRefNum>");
                    strmwrtr.WriteLine("      </rctReceiveValidateChlnDtls>");
                    strmwrtr.WriteLine("      <rmtrName>prathik</rmtrName>");
                    strmwrtr.WriteLine("      <totalAmount>" + ChallanAmount + "</totalAmount>");
                    strmwrtr.WriteLine("      <trsryCode>572H</trsryCode>");
                    strmwrtr.WriteLine("</RctReceiveValidateChlnRq>");
                    strmwrtr.WriteLine("</data>");
                }

                //Write file contents on console.
                //using (StreamReader sr = File.OpenText(fileName))
                //{
                //    string s = "";
                //    while ((s = sr.ReadLine()) != null)
                //    {
                //        //Console.WriteLine(s);
                //    }
                //}
                return newFile;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        public string GetKIISignDetails(string signeddata, string xmldata)
        {
            try
            {
                string URL = "https://preprodkhajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/RctReceiveValidateChlnService?wsdl";

                string xml2 = @"<?xml version='1.0' encoding='utf-8'?>"
                    +
                    "<soapenv:Envelope xmlns:soapenv=" + "'" + "http://schemas.xmlsoap.org/soap/envelope/" + "'" + " xmlns:ser=" + "'" + "http://service.receivevalidatechallan.dept.rct.integration.ifms.gov.in/" + "'" + " xmlns:head=" + "'" + "http://header.ei.integration.ifms.gov.in/" + "'" + ">"
                    + "\n"
                    + "   <soapenv:Header>"
                    + "\n"
                    + "      <ser:Header>"
                    + "\n"
                    + "         <head:agencyCode>EA_KID</head:agencyCode>"
                     + "\n"
                    + "         <head:integrationCode>RCT033</head:integrationCode>"
                     + "\n"
                    + "         <head:uirNo>EA_KID-RCT033-"+transactiondate+"-" + deptRefNum + "</head:uirNo>"
                     + "\n"
                    + "      </ser:Header>"
                     + "\n"
                    + "   </soapenv:Header>"
                     + "\n"
                    + "   <soapenv:Body>"
                     + "\n"
                    + "      <ser:envelopedDataReq>"
                     + "\n"
                    + "         <Signature>" + signeddata + "</Signature>"
                    + "\n"
                    + xmldata
                    + "      </ser:envelopedDataReq>"
                     + "\n"
                    + "   </soapenv:Body>"
                     + "\n"
                    + "</soapenv:Envelope>";

                /////////////

                //////////////////////



                string responseStr = string.Empty;
                string jsonText = string.Empty;
                //var _url = "https://preprodkhajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/";
                //var _action = "RctReceiveValidateChlnService?wsdl";

                //XmlDocument soapEnvelopeXml = CreateSoapEnvelope(signeddata, xml2);
                // WebRequest.DefaultCachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                //Log.Debug("Request for Signature Validation");
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(URL);
                byte[] bytes;
                bytes = Encoding.UTF8.GetBytes(xml2);
                request.ContentType = "text/xml";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                //request.SendChunked = false;
                // request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0, no-cache, no-store");
                //request.KeepAlive = true;
                //request.AllowWriteStreamBuffering = false;
                //request.ServicePoint.ConnectionLimit = 10;    // The default value of 2 within a ConnectionGroup caused me always a "Timeout exception" because a user's 1-3 concurrent WebRequests within a second.
                //request.ServicePoint.MaxIdleTime = 5 * 1000;  // (5 sec) default was 100000 (100 sec).  Max idle time for a connection within a ConnectionGroup for reuse before closing

                //Log.Debug("FileStream Rquesting");
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                //Log.Debug("FileStream Rquest Done");
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                //Log.Debug("Response from KII Signing:  " + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    responseStr = new StreamReader(responseStream).ReadToEnd();
                    XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(responseStr));
                    ////
                    XDocument xDoc = XDocument.Load(new System.IO.StringReader(responseStr));

                    var unwrappedResponse = xDoc.Descendants((XNamespace)"http://schemas.xmlsoap.org/soap/envelope/" + "Body").First().FirstNode;
                    //var xmlwithoutheaders = unwrappedResponse.ToString();
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(unwrappedResponse.ToString());
                    jsonText = JsonConvert.SerializeXmlNode(doc1.ChildNodes[0].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                    ////
                    //var xmlWithoutNs = xmlDocumentWithoutNs.ToString();
                    //Log.Debug("Response Data:  " + xmlWithoutNs);
                    //return Content(responseStr);
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(xmlWithoutNs);
                    //XmlNode node = doc.SelectSingleNode("Basic_vehicle_detailsResult");
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);

                    // To convert JSON text contained in string json into an XML node
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
                    ViewBag.Response = responseStr;
                   // ViewBag.Response1 = doc.InnerText;
                    //jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                    //jsonText = JsonConvert.SerializeXmlNode(doc);
                    //Log.Debug("After Json Convervation Data: " + jsonText);
                }

                //VM_MotorInsuranceVehicleDetails obj = new VM_MotorInsuranceVehicleDetails();
                //dynamic result = JsonConvert.DeserializeObject(ViewBag.Response);

                //obj = _IMotorInsuranceVehicleDetailsBll.BindVahanResponseDetailstoModel(result);

                return jsonText;
            }
            catch (Exception ex)
            {
                //Log.Error("Signing Data Error Level", ex);
                return null;
            }
        }

        public class SignedDataResponseKII
        {
            public string deptRefNum { get; set; }
            public string totalAmount { get; set; }
            public string statusCode { get; set; }
            public string statusDescription { get; set; }
        }
        //Implemented based on requirement--Added by Venkatesh--
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
        //Remote post--Added by Venkatesh
        public class RemotePost
        {
            //Remote post added by--Venkatesh
            private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();


            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            public void Post()
            {
                System.Web.HttpContext.Current.Response.Clear();

                System.Web.HttpContext.Current.Response.Write("<html><head>");

                System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
                System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");

                System.Web.HttpContext.Current.Response.End();
            }
        }

        public static string GetMD5Checksum(string filename)
        {
            byte[] b = CreateChecksum(filename);
            string result = "";
            for (int i = 0; i < b.Length; i++)
            {
                result += java.lang.Integer.toString((b[i] & 0xff) + 0x100, 16).Substring(1);
            }
            return result;
        }
        public static byte[] CreateChecksum(string filename)
        {
            InputStream fis = new ByteArrayInputStream(System.Text.Encoding.UTF8.GetBytes(filename));
            byte[] buffer = new byte[1024];
            MessageDigest complete = MessageDigest.getInstance("MD5");
            int numRead;
            do
            {
                numRead = fis.read(buffer);
                if (numRead > 0)
                {
                    complete.update(buffer, 0, numRead);
                }
            }
            while (numRead != -1);
            fis.close();
            return complete.digest();
        }

        public static string SymetricEncrypt(string text, string secretkey)
        {
            //Log.Debug("Request SymetricEncryptData: " + text);
            byte[] raw;
            string encryptedString;
            SecretKeySpec skeyspec;

            Cipher cipher;
            try
            {
                byte[] encryptText = Encoding.UTF8.GetBytes(text);
                //byte[] encryptText = text.getBytes("UTF-8");
                //FileInputStream fileinputstream = new FileInputStream("D://KII//KGID_KHAJANE.key");
                //byte[] abyte = new byte[fileinputstream.available()];
                byte[] abyte = Encoding.UTF8.GetBytes(secretkey);
                //fileinputstream.read(abyte);
                //fileinputstream.close();

                byte[] keyBytes = new byte[16];
                int len = abyte.Length;
                if (len > keyBytes.Length) len = keyBytes.Length;
                //System.arraycopy(abyte, 0, keyBytes, 0, len);
                Array.Copy(abyte, 0, keyBytes, 0, len);
                raw = Base64.decode(secretkey);
                skeyspec = new SecretKeySpec(keyBytes, "AES");
                IvParameterSpec ivSpec = new IvParameterSpec(keyBytes);
                cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
                cipher.init(Cipher.ENCRYPT_MODE, skeyspec, ivSpec);
                //byte[] results = cipher.doFinal(encryptText);
                //string beforesignedData = Convert.ToBase64String(results);
                encryptedString = Base64.encode(cipher.doFinal(encryptText));
                //encryptedString = Convert.ToBase64String(cipher.doFinal(encryptText));
                return encryptedString;
            }
            catch (Exception ex)
            {
                //Log.Error("SymetricEncrypt Error Level", ex);
                return null;
            }
        }
        public static string SymmetricDecrypt(string text, string secretkey)
        {
            Cipher cipher;
            string encryptedString;
            byte[] encryptText = null;
            byte[] raw;
            SecretKeySpec skeySpec;
            try
            {
                //FileInputStream fileinputstream = new FileInputStream("D://KII//KGID_KHAJANE.key");
                //byte[] abyte = new byte[fileinputstream.available()];
                byte[] abyte = Encoding.UTF8.GetBytes(secretkey);
                //fileinputstream.read(abyte);
                //fileinputstream.close();
                byte[] keyBytes = new byte[16];
                int len = abyte.Length;
                if (len > keyBytes.Length) len = keyBytes.Length;
                //System.arraycopy(abyte, 0, keyBytes, 0, len);
                Array.Copy(abyte, 0, keyBytes, 0, len);
                //raw = Base64.decode(secretkey);
                skeySpec = new SecretKeySpec(keyBytes, "AES");
                //encryptText = System.Text.Encoding.UTF8.GetBytes(text);
                //encryptText = Base64.decode(text);
                encryptText = Convert.FromBase64String(text);

                IvParameterSpec ivSpec = new IvParameterSpec(keyBytes);
                cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
                cipher.init(Cipher.DECRYPT_MODE, skeySpec, ivSpec);
                //encryptedString = new String(cipher.doFinal(encryptText));
                //encryptedString = Convert.ToBase64String(cipher.doFinal(encryptText));
                encryptedString = Encoding.UTF8.GetString(cipher.doFinal(encryptText));
                return encryptedString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult MIDoubleVerificationDetails()
        {
            HttpCookie StudentCookies = new HttpCookie("StudentCookies");
            StudentCookies.Value = "hallo";
            StudentCookies.Expires = DateTime.Now.AddHours(1);
            Response.SetCookie(StudentCookies);
            Response.Flush();   
            bool isSuccess = false;
            string message = string.Empty;
            string result = string.Empty;
            try {
                if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
                {
                    long EID = Convert.ToInt64(Session["UID"]);
                    string UCat = Convert.ToString(Session["SelectedCategory"]);
                    tbl_mi_kii_challan_request contact = _db.tbl_mi_kii_challan_request.FirstOrDefault(m => m.mi_kii_challan_ref_no == "KD0221801119182527" && m.mi_kii_proposer_id == EID && m.mi_kii_user_category == UCat&&m.mi_kii_challan_active_status==false);

                    isSuccess = true;
                    message = "OK";
                    return Json(new { IsSuccess = isSuccess, Message = message, Result = contact }, JsonRequestBehavior.AllowGet);
                }
                else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
                {
                    long EID = Convert.ToInt64(Session["UID"]);
                    string UCat = Convert.ToString(Session["Categories"]);
                    tbl_mi_kii_challan_request contact = _db.tbl_mi_kii_challan_request.FirstOrDefault(m => m.mi_kii_challan_ref_no == "KD0221801119182527" && m.mi_kii_proposer_id == EID && m.mi_kii_user_category == UCat && m.mi_kii_challan_active_status == false);

                    isSuccess = true;
                    message = "OK";
                    return Json(new { IsSuccess = isSuccess, Message = message, Result = contact }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    long EID = Convert.ToInt64(Session["UID"]);
                    string UCat = Convert.ToString(Session["Categories"]);
                    tbl_mi_kii_challan_request contact = _db.tbl_mi_kii_challan_request.FirstOrDefault(m => m.mi_kii_challan_ref_no == "KD0221801119182527" && m.mi_kii_proposer_id == EID && m.mi_kii_user_category == UCat && m.mi_kii_challan_active_status == false);

                    isSuccess = true;
                    message = "OK";
                    return Json(new { IsSuccess = isSuccess, Message = message, Result = contact }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch(Exception ex)
            {
                isSuccess = false;
                message = "Error";
                return Json(new { IsSuccess = isSuccess, Message = message, Result = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult InsertMIChallanDetailsKII(string RefNos, string ChallanAmount, string Type)
        {
            bool isSuccess = false;
            string message = string.Empty;
            string result = string.Empty;
            HttpCookie cookieAppRefNo = HttpContext.Request.Cookies.Get("amrn");
            HttpCookie cookieChalnAmt = HttpContext.Request.Cookies.Get("amch");
            string AppRefNo = cookieAppRefNo.Value;
            AppRefNo = System.Web.HttpUtility.UrlDecode(AppRefNo);
            string ChalnAmt = cookieChalnAmt.Value;
            if (Request.Cookies["amrn"] != null && Request.Cookies["amch"] != null)
            {
                var ar = new HttpCookie("amrn");
                var ca = new HttpCookie("amch");
                ar.Expires = DateTime.Now.AddDays(-1);
                ca.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(ar);
                Response.Cookies.Add(ca);
            }
            if (ChallanAmount == ChalnAmt&& AppRefNo == RefNos)
            {
                //string abc = Session["chlnamt"].ToString();
                if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
                {
                    result = _IMotorInsuranceVehicleDetailsBll.SaveMIChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]), RefNos, Convert.ToInt32(ChallanAmount), Type);
                }
                else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
                {
                    result = _IMotorInsuranceVehicleDetailsBll.SaveMIChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]), RefNos, Convert.ToInt32(ChallanAmount), Type);
                }
                else
                {
                    result = _IMotorInsuranceVehicleDetailsBll.SaveMIChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]), RefNos, Convert.ToInt32(ChallanAmount), Type);
                }
                if (result != "")
                {
                    isSuccess = true;
                    message = "Payment Successfully Completed";
                }

                return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                isSuccess = false;
                message = "Payment Unsuccessful";
                return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
            }
        }
        //Print MI Challan Details
        public ActionResult PrintMIChallanDetails(string RefNos, string ChallanAmount, string Type)
        {
            string t = Convert.ToString(ChallanAmount);
            bool isSuccess = false;
            string message = string.Empty;
            string result = string.Empty;
            //long challanamt1 = Convert.ToInt64(t);
            //int challanamt = Convert.ToInt32(ChallanAmount);
            HttpCookie cookieAppRefNo = HttpContext.Request.Cookies.Get("amrnp");
            HttpCookie cookieChalnAmt = HttpContext.Request.Cookies.Get("amchp");
            string AppRefNo = cookieAppRefNo.Value;
            AppRefNo = System.Web.HttpUtility.UrlDecode(AppRefNo);
            string ChalnAmt = cookieChalnAmt.Value;
            if (Request.Cookies["amrnp"] != null && Request.Cookies["amchp"] != null)
            {
                var ar = new HttpCookie("amrnp");
                var ca = new HttpCookie("amchp");
                ar.Expires = DateTime.Now.AddDays(-1);
                ca.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(ar);
                Response.Cookies.Add(ca);
            }
            if (ChallanAmount == ChalnAmt && AppRefNo == RefNos)
            {
                VM_ChallanPrintDetails MBChallanDetails = new VM_ChallanPrintDetails();
                if (Convert.ToInt32(Session["SelectedCategory"]) == 2)
                {
                    MBChallanDetails = _IMotorInsuranceVehicleDetailsBll.PrintMIChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]), RefNos, Type);
                }
                else if (Session["Categories"]!= null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
                {
                    MBChallanDetails = _IMotorInsuranceVehicleDetailsBll.PrintMIChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]), RefNos, Type);
                }
                else
                {
                    MBChallanDetails = _IMotorInsuranceVehicleDetailsBll.PrintMIChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]), RefNos, Type);
                }
                if (MBChallanDetails.employee_name != "")
                {
                    isSuccess = true;
                    message = "Challan Successfully Generated";
                }
                string filepath = FillMBChallan(MBChallanDetails, RefNos, ChallanAmount);
                //byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                //string contentType = MimeMapping.GetMimeMapping(filepath);
                //string fileName = @"C:\TEMP\TEST.pdf";
                byte[] pdfByteArray = System.IO.File.ReadAllBytes(filepath);
                string base64EncodedPDF = System.Convert.ToBase64String(pdfByteArray);
                //return File(filedata, contentType);

                //return View(filepath);

                return Json(new { IsSuccess = isSuccess, Message = message, Result = base64EncodedPDF }, JsonRequestBehavior.AllowGet);
                //return File(filepath, "application/pdf");
            }
            else
            {
                isSuccess = false;
                message = "Payment Unsuccessful";
                return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region MB Challan Print
        private string FillMBChallan(VM_ChallanPrintDetails MBChallanDetails, string RefNos, string ChallanAmount)
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToShortDateString();
            string amtwords = ConvertToWords(ChallanAmount);
            MBChallanDetails.Category = "Government";
            MBChallanDetails.GrandTotal = Convert.ToString(MBChallanDetails.p_premium);
            //KG MONTH YEAR 8011 01-8digits
            string my = dt.ToString("MMyy");
            string timestamp = dt.ToString("hhmmssff");
            string challanrefno = "KD" + my + "801100" + timestamp;
            MBChallanDetails.challan_ref_no = challanrefno;
            ////////////////////////////////////////////////////////////////
            string pdfTemplate = Server.MapPath("~/Challans/MB/Challan_MB_Test.pdf");
            string newFile = Server.MapPath("~/Challans/MB/" + challanrefno + ".pdf");
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            {//Facing Sheet Details
             //var date1 = facingSheet.DateOfIssue?.ToString("dd-MM-yyyy");

                fields.SetField("ChallanValidity", MBChallanDetails.Challan_Validity);
                //fields.SetField("District", MBChallanDetails.dm_name_english.ToString().Trim().ToUpper());
                fields.SetField("Department", MBChallanDetails.dm_deptname_english);
                fields.SetField("DDOOffice", MBChallanDetails.dm_ddo_office);
                fields.SetField("Category", MBChallanDetails.Category);
                //fields.SetField("Date", NBChallanDetails.challan_date.ToString().Trim().ToUpper());
                fields.SetField("Date", date);
                fields.SetField("ChallanReferenceNo", MBChallanDetails.challan_ref_no);
                fields.SetField("DDOCode", MBChallanDetails.dm_ddo_code);
                fields.SetField("RemitterName", MBChallanDetails.employee_name.ToString().Trim().ToUpper());
                fields.SetField("MobileNo", MBChallanDetails.mobile_number.ToString().Trim().ToUpper());
                fields.SetField("Address", MBChallanDetails.ead_address);

                fields.SetField("Purpose", MBChallanDetails.purpose_desc);
                fields.SetField("HOA", MBChallanDetails.hoa_desc);
                fields.SetField("SubPurposeName", MBChallanDetails.sub_purpose_desc);
                fields.SetField("PurposeSpecificID", MBChallanDetails.purpose_id);
                fields.SetField("Amount", ChallanAmount.ToString().Trim().ToUpper());
                fields.SetField("RemittanceBank", MBChallanDetails.RemittanceBank);
                fields.SetField("GrandTotal", ChallanAmount);
                fields.SetField("TotalAmountinWords", amtwords);
                fields.SetField("ChequeDDNo", MBChallanDetails.Cheque_DD_No);
                fields.SetField("ChequeDDBank", MBChallanDetails.Cheque_DD_Bank);
                fields.SetField("IFSCode", MBChallanDetails.IFSC_Code);
                fields.SetField("MICRCode", MBChallanDetails.MICR_Code);
                fields.SetField("ChequeDDDate", MBChallanDetails.Cheque_DD_Date.ToString().Trim().ToUpper());
                //fields.SetField("ChequeDDDate", NBChallanDetails.Cheque_DD_Date?.ToString("dd-MM-yyyy"));
            }
            pdfStamper.Close();
            return newFile;
        }
        #endregion
        //=========================View Application Data related functions ================================
        #region View Application Data related functions
        [Route("mi-agy-soa")]
        [Route("mi-dpt-soa")]
        [Route("mi-e-soa")]
        public ActionResult MotorInsuranceStatusOfApplication()
        {
            VM_DDOVerificationDetailsMI verificationDetails = null ;
            if (Convert.ToInt32(Session["SelectedCategory"]) == 2)
            {
                verificationDetails = _IMotorInsuranceVehicleDetailsBll.getMIApplicationEmployeeStatusList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                verificationDetails = _IMotorInsuranceVehicleDetailsBll.getMIApplicationEmployeeList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            else if(Convert.ToInt32(Session["SelectedCategory"]) == 1)
            {
                verificationDetails = _IMotorInsuranceVehicleDetailsBll.getMIApplicationEmployeeList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            return View(verificationDetails);
        }
        [Route("MIApplications")]
        public ActionResult MotorInsuranceApplications()
        {
            VM_DDOVerificationDetailsMI verificationDetails;
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
            {
                verificationDetails = _IMotorInsuranceVehicleDetailsBll.getMIApplicationEmployeeList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                verificationDetails = _IMotorInsuranceVehicleDetailsBll.getMIApplicationEmployeeList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            else
            {
                verificationDetails = _IMotorInsuranceVehicleDetailsBll.getMIApplicationEmployeeList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            return View(verificationDetails);
        }
        [Route("mi-agy-mp")]
        [Route("mi-dpt-mp")]
        [Route("mi-e-mp")]
        public ActionResult MotorInsuranceListOfApplications()
        {
            VM_DDOVerificationDetailsMI ApplicationDetails = null ;
            try
            {
                if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
                {
                    ApplicationDetails = _IMotorInsuranceVehicleDetailsBll.getMIApplicationEmployeeList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
                }
                else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
                {
                    ApplicationDetails = _IMotorInsuranceVehicleDetailsBll.getMIApplicationEmployeeList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
                }
                else
                {
                    ApplicationDetails = _IMotorInsuranceVehicleDetailsBll.getMIApplicationEmployeeList(Convert.ToInt64(Session["UID"]), 1);
                }
            }
            catch(Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "MotorInsuranceListOfApplications");
            }
            

            return View(ApplicationDetails);
        }
        [Route("ScheduleOfPremium")]
        public ActionResult ScheduleOfPremium()
        {
            VM_MotorInsuranceIDVDetails _IDVData = _IMotorInsuranceVehicleDetailsBll.IDVDetailsBll(17, 15102020114932);
            if (_IDVData == null)
            {
                _IDVData = new VM_MotorInsuranceIDVDetails();
            }
            //return this.PartialView("_IDVDetails", _IDVData);
            return View(_IDVData);
        }
        [Route("mi-soa-vmib-apln/{Type?}/{AppRefNo?}/{AppId?}/{EmpID?}")]
        public ActionResult PrintMIBond(string Type = null, long AppRefNo = 0, long AppId = 0, long EmpID = 0)
        {
            long EID = Convert.ToInt64(Session["UID"]);
            if (EID == EmpID)
            {
                VM_MotorInsurancePolicyPrintDetails MIPD = _IMotorInsuranceVehicleDetailsBll.MIPolicyPrintDetailsBll(Type, EmpID, AppRefNo);
                if (MIPD == null)
                {
                    MIPD = new VM_MotorInsurancePolicyPrintDetails();
                }
                //FillForm();
                return View(MIPD);
            }
            else
            {
                VM_MotorInsurancePolicyPrintDetails MIPD = _IMotorInsuranceVehicleDetailsBll.MIPolicyPrintDetailsBll(Type, EmpID, AppRefNo);
                if (MIPD == null)
                {
                    MIPD = new VM_MotorInsurancePolicyPrintDetails();
                }
                //FillForm();
                return View(MIPD);
                //return View();
            }
        }
        private void FillForm()
        {
            String pdfTemplate = @"D:\Venkat\Working\KGID 30\KGID\KGID\MIDocuments\sample.pdf";
            String newFile = @"D:\Venkat\Working\KGID 30\KGID\KGID\MIDocuments\sample_output2.pdf";
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            DateTime dt = new DateTime();
            string date = Convert.ToString(dt);
            fields.SetField("Date", date);
            fields.SetField("Text1", "Peter");
            fields.SetField("Text2", "12345");
            fields.SetField("Text1_2", "98764");
            pdfStamper.Close();

            //string pdfTemplate = @"D:\Venkat\Working\KGID 30\KGID\KGID\MIDocuments\sample.pdf";

            //string newFile = @"D:\Venkat\Working\KGID 30\KGID\KGID\MIDocuments\sample_output1.pdf";
            //PdfReader pdfReader = new PdfReader(pdfTemplate);
            //PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            //AcroFields pdfFormFields = pdfStamper.AcroFields;
            //// set form pdfFormFields  
            //// The first worksheet and W-4 form  
            //pdfFormFields.SetField("Textbox1", "1");
            //pdfFormFields.SetField("Textbox2", "2");
            //// report by reading values from completed PDF  
            //string sTmp = "W-4 Completed for " + pdfFormFields.GetField("Textbox1") + " " + pdfFormFields.GetField("Textbox2");
            ////MessageBox.Show(sTmp, "Finished");
            //// flatten the form to remove editting options, set it to false  
            //// to leave the form open to subsequent manual edits  
            //pdfStamper.FormFlattening = false;
            //// close the pdf  
            //pdfStamper.Close();
        }
        [HttpGet]
        [Route("MotorBranchApplications")]
        public ActionResult MotorBranchApplications()
        {
            VM_MBApplicationDetails ApplicationDetails;
            ApplicationDetails = _IMotorInsuranceVehicleDetailsBll.GetMBApplicationListBll(1, 1);
            return View(ApplicationDetails);
        }
        #endregion

        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[Route("GetModelListBasedonMake")]
        public JsonResult GetModelListBasedonMake(int makeid)
        {
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetModelListBasedonMake(makeid);
            return Json(_VehicleDetails, JsonRequestBehavior.AllowGet);
        }
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[Route("GetVehCatergoryList")]
        public JsonResult GetVehCatergoryList(string VehTypeid, long VehSubTypeid)
        {
            IEnumerable<tbl_vehicle_category_master> obj = _IMotorInsuranceVehicleDetailsBll.GetVehCatergoryList(VehTypeid, VehSubTypeid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[Route("GetManufactureListBasedonMake")]
        public JsonResult GetManufactureListBasedonMake(int makeid)
        {
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetManufactureListBasedonMake(makeid);
            return Json(_VehicleDetails, JsonRequestBehavior.AllowGet);
        }
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[Route("GetRTODetails")]
        public JsonResult GetRTODetails(string ChasisNo, string EngineNo)
        {
            long empId = Convert.ToInt64(Session["UID"]);
            //bool isSuccess = false;
            string message = string.Empty;
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetRTODetailsBll(ChasisNo, EngineNo);
            return Json(_VehicleDetails, JsonRequestBehavior.AllowGet);
        }

        #region Fresh Workflow
        [Route("DDOPropDetails")]
        public ActionResult DetailsForDDOVerification()
        {
            ViewBag.Verifier = Verifiers.DDO;
            ViewBag.Verify = 2;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeDetailsForDDOVerification(Convert.ToInt64(Session["UID"]));
            return View("VerificationDetails", verificationDetails);
        }

        [CustomAuthorize("Caseworker")]
        [Route("mi-cw-adt")]
        public ActionResult DetailsForCWVerification()
        {
            ViewBag.Verifier = Verifiers.CW;
            ViewBag.Verify = 3;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeDetailsForCWVerification(Convert.ToInt64(Session["UID"]));
            return View("VerificationDetails", verificationDetails);
        }
        [Route("mi-si-adt")]
        [CustomAuthorize("Superintendent")]
        public ActionResult DetailsForsuperintendentVerification()
        {
            ViewBag.Verifier = Verifiers.SUPERINTENDENT;
            ViewBag.Verify = 4;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeDetailsForSuperintendentVerification(Convert.ToInt64(Session["UID"]));
            return View("VerificationDetails", verificationDetails);
        }
        [Route("DIOPropDetails")]
        public ActionResult DetailsForDIOVerification()
        {
            ViewBag.Verifier = Verifiers.DIO;
            ViewBag.Verify = 5;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeDetailsForDIOVerification(Convert.ToInt64(Session["UID"]));
            return View("VerificationDetails", verificationDetails);
        }

        [Route("mi-ad-adt")]
        [CustomAuthorize("Assitant Director")]
        public ActionResult DetailsForADVerification()
        {
            ViewBag.Verifier = Verifiers.ASSITANTDIRECTOR;
            ViewBag.Verify = 15;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeDetailsForADVerification(Convert.ToInt64(Session["UID"]));
            return View("VerificationDetails", verificationDetails);
        }

        [Route("mi-dd-adt")]
        [CustomAuthorize("Deputy Director")]
        public ActionResult DetailsForDDVerification()
        {
            ViewBag.Verifier = Verifiers.DEPUTYDIRECTOR;
            ViewBag.Verify = 6;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeDetailsForDDVerification(Convert.ToInt64(Session["UID"]));
            return View("VerificationDetails", verificationDetails);
        }
        [Route("DPropDetails")]
        public ActionResult DetailsForDVerification()
        {
            ViewBag.Verifier = Verifiers.DIRECTOR;
            ViewBag.Verify = 7;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeDetailsForDVerification(Convert.ToInt64(Session["UID"]));
            return View("VerificationDetails", verificationDetails);
        }
        #endregion

        #region MI Renewal Process
        [Route("mi-agy-r-af")]
        [Route("mi-agy-r-ra")]
        [Route("mi-e-re")]
        [Route("mi-dpt-r-ra")]
        public ActionResult MIRenewalApplication()
        {
            VM_MotorInsuranceRenewalDetails _RenewalDetail = new VM_MotorInsuranceRenewalDetails();
            if (Convert.ToInt32(Session["SelectedCategory"])==2)
            {
                _RenewalDetail = _IMotorInsuranceRenewalDetailsBll.MIRenwalDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                _RenewalDetail = _IMotorInsuranceRenewalDetailsBll.MIRenwalDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            else if(Convert.ToInt32(Session["SelectedCategory"])==1)
            {
                _RenewalDetail = _IMotorInsuranceRenewalDetailsBll.MIRenwalDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            //[sp_kgid_mi_renewalApplication]
            return View(_RenewalDetail);
        }
        [Route("mi-r-rn-apln/{PageType?}/{refNo?}/{category?}/{RenewalRefNo?}")]
        [Route("mi-r-edt-apln/{PageType?}/{refNo?}/{category?}/{RenewalRefNo?}")]
        public ActionResult MotorInsuranceRenewalApplication(string PageType = null, long refNo = 0, string category = "", long RenewalRefNo = 0)
        {
            VM_MIApplicationDetails obj = new VM_MIApplicationDetails();
            obj.Type = Convert.ToString(PageType);
            Session.Remove("RID");
            obj.RenewalRefNo = RenewalRefNo;
            obj.previousRefNo = refNo;

            return View(obj);
        }
        //[Route("RenewalProposerDetailsToView")]
        public ActionResult RenewalProposerDetailsToView(string PageType, string refNo)
        {
            VM_MotorInsuranceProposerDetails _ProposerDetail = new VM_MotorInsuranceProposerDetails();
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
            {
                _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), (PageType == null) ? "" : PageType, Convert.ToInt64(refNo), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), (PageType == null) ? "" : PageType, Convert.ToInt64(refNo), Convert.ToInt32(Session["Categories"]));
            }
            else
            {
                if(PageType== "Renewal" ||PageType== "EditRenewal")
                {

                    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), (PageType == null) ? "" : (PageType=="Renewal"? "EmpRenewal": PageType), Convert.ToInt64(refNo), Convert.ToInt32(Session["Categories"]));
                }
                else
                {
                    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), "Emp", 0, Convert.ToInt32(Session["Categories"]));

                }
            }

            if (!String.IsNullOrEmpty(_ProposerDetail.mipd_kgid_application_number))
            {
                Session["RID"] = _ProposerDetail.mipd_kgid_application_number;
                TempData["Deptmartment"] = _ProposerDetail.mipd_Department;
                using (MemoryStream ms = new MemoryStream())
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(_ProposerDetail.mipd_kgid_application_number.ToString(), QRCodeGenerator.ECCLevel.Q);
                    using (Bitmap bitMap = qrCode.GetGraphic(20))
                    {
                        bitMap.Save(ms, ImageFormat.Png);
                        _ProposerDetail.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return this.PartialView("_ProposerDetails", _ProposerDetail);
        }
        //[Route("RenewalVehicleDetailsToView")]
        public ActionResult RenewalVehicleDetailsToView(string PageType, long refNo,long RenewalRefNo)
        {
            VM_MotorInsuranceVehicleDetails _VehicleDetails=new VM_MotorInsuranceVehicleDetails();
           
            _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetMIRenewalVehicleDetails(Convert.ToInt64(Session["UID"]), refNo, RenewalRefNo);
            _VehicleDetails.mivd_pagetype = PageType;
           
            return this.PartialView("_VehicleDetails", _VehicleDetails);
        }
        //[Route("RenewalIDVDetailsToView")]
        public ActionResult RenewalIDVDetailsToView(string PageType, string refNo = "")
        {
            VM_MotorInsuranceIDVDetails _IDVData = _IMotorInsuranceVehicleDetailsBll.IDVDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt64(refNo));
            if (_IDVData == null)
            {
                _IDVData = new VM_MotorInsuranceIDVDetails();
            }
            _IDVData.miidv_pagetype = PageType;
            return this.PartialView("_IDVDetails", _IDVData);
        }
        //[Route("RenewalPreviousHistoryToView")]
        public ActionResult RenewalPreviousHistoryToView(string PageType, string refNo = "")
        {
            if(Session["ORID"]!=null)
            {
                if (Convert.ToString(Session["ORID"]) != refNo)
                {
                    refNo = Session["ORID"].ToString();
                }
            }
            
            VM_MotorInsurancePreviousHistoryDetails onjPreviousHistory = _IMotorInsuranceVehicleDetailsBll.PreviousHistoryDetails(Convert.ToInt64(Session["UID"]), Convert.ToInt64(refNo));
            onjPreviousHistory.mivd_pagetype = PageType;
            return this.PartialView("_PreviousHistoryMI", onjPreviousHistory);
        }
        //[Route("RenewalOtherDetailsToView")]
        public ActionResult RenewalOtherDetailsToView(string PageType, string refNo = "")
        {
            VM_MotorInsuranceOtherDetails _OtherData = _IMotorInsuranceVehicleDetailsBll.OtherDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt64(refNo));
            if (_OtherData == null)
            {
                _OtherData = new VM_MotorInsuranceOtherDetails();
            }
            _OtherData.miod_PageType = PageType;
            return this.PartialView("_OtherDetails", _OtherData);
        }

        public ActionResult RenewalDeclarationToView(string PageType, string refNo = "")
        {
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetMIVehicleDetails(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));
            _VehicleDetails.mivd_pagetype = PageType;
            _VehicleDetails.mi_referenceno = Convert.ToInt64(refNo);

            return this.PartialView("_RenewalDeclarationFormMI", _VehicleDetails);
        }

        public ActionResult RenewalMIDocumentDetailsToView(string PageType)
        {
            VM_MI_Upload_Documents _MIDocumentData = _IMotorInsuranceVehicleDetailsBll.MIDocumentDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));
            if (_MIDocumentData == null)
            {
                _MIDocumentData = new VM_MI_Upload_Documents();
            }
            _MIDocumentData.MIPAgetype = PageType;
            return this.PartialView("_MIDocumentDetails", _MIDocumentData);

           
        }

        [Route("mi-dpt-r-rmp")]
        [Route("mi-agy-r-mp")]
        public ActionResult MotorInsuranceListOfRenewalApplications()
        {
            VM_DDOVerificationDetailsMI ApplicationDetails;
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
            {
                ApplicationDetails = _IMotorInsuranceVehicleDetailsBll.getMIRenewalApplicationList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                ApplicationDetails = _IMotorInsuranceVehicleDetailsBll.getMIRenewalApplicationList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            else
            {
                ApplicationDetails = _IMotorInsuranceVehicleDetailsBll.getMIRenewalApplicationList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }

            return View(ApplicationDetails);
        }
        [Route("mi-agy-r-soa")]
        [Route("mi-dpt-r-rsoa")]

        public ActionResult MotorInsuranceStatusOfRenewalApplication()
        {
            VM_DDOVerificationDetailsMI verificationDetails;
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
            {
                verificationDetails = _IMotorInsuranceVehicleDetailsBll.getMIRenewalApplicationList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                verificationDetails = _IMotorInsuranceVehicleDetailsBll.getMIRenewalApplicationList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            else
            {
                verificationDetails = _IMotorInsuranceVehicleDetailsBll.getMIRenewalApplicationList(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            return View(verificationDetails);
        }
  
        #endregion

        #region Renewal Workflow
        [Route("mi-cw-radt")]
        public ActionResult RenewalDetailsForCWVerification()
        {
            ViewBag.Verifier = Verifiers.CW;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeRenewalDetailsForCWVerification(Convert.ToInt64(Session["UID"]));
            return View("RenewalVerificationDetails", verificationDetails);
        }
        [Route("mi-si-radt")]
        public ActionResult RenewalDetailsForsuperintendentVerification()
        {
            ViewBag.Verifier = Verifiers.SUPERINTENDENT;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeRenewalDetailsForSuperintendentVerification(Convert.ToInt64(Session["UID"]));
            return View("RenewalVerificationDetails", verificationDetails);
        }

        [Route("mi-ad-radt")]
        [CustomAuthorize("Assitant Director")]
        public ActionResult RenewalDetailsForADVerification()
        {
            ViewBag.Verifier = Verifiers.ASSITANTDIRECTOR;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeRenewalDetailsForADVerification(Convert.ToInt64(Session["UID"]));
            return View("RenewalVerificationDetails", verificationDetails);
        }


        [Route("mi-dd-radt")]
        public ActionResult RenewalDetailsForDDVerification()
        {
            ViewBag.Verifier = Verifiers.DEPUTYDIRECTOR;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeRenewalDetailsForDDVerification(Convert.ToInt64(Session["UID"]));
            return View("RenewalVerificationDetails", verificationDetails);
        }
        [Route("RenewalDPropDetails")]
        public ActionResult RenewalDetailsForDVerification()
        {
            ViewBag.Verifier = Verifiers.DIRECTOR;
            VM_DDOVerificationDetailsMI verificationDetails = _IMotorInsuranceVehicleDetailsBll.GetEmployeeRenewalDetailsForDVerification(Convert.ToInt64(Session["UID"]));
            return View("RenewalVerificationDetails", verificationDetails);
        }
        #endregion
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[Route("InsELD")]
        public ActionResult InsertEmployeeLoanDetails(VM_MIApplicationDetails objMILoanDetails)
        {

            bool isSuccess = false;
            string message = string.Empty;
            if (objMILoanDetails.EmployeeId != 0)
            {
                string result = _IMotorInsuranceProposerDetailsBll.SaveMILoanDetails(objMILoanDetails);
                if (result != "")
                {
                    isSuccess = true;
                    message = "Loan details saved successfully";
                }
            }
            else
            {
                isSuccess = false;
                message = "Loan details could not saved";
            }
            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DSCSignInKGIDMB(string Kgidno, string PublicKey)
        {
            long empid = Convert.ToInt64(Session["UID"]);
            Logger.LogMessage(TracingLevel.INFO, "DSC SIGN IN KGID MB -EmpId&PublicKey" + empid + "& " + PublicKey);
            string result = _IMotorInsuranceProposerDetailsBll.DSCLoginDetails(empid, PublicKey);
            Logger.LogMessage(TracingLevel.INFO, "DSC SIGN IN KGID MB - Result: " + result);
            bool isSuccess = false;
            string message = string.Empty;
            if (result == "SUCCESS")
            {
                isSuccess = true;
                message = "Success";
                return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                result = "FAILURE";
                isSuccess = false;
                message = "Failure";
                return Json(new { IsSuccess = isSuccess, Message = message, Data = result }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new { IsSuccess = isSuccess, Message = message, Data = result }, JsonRequestBehavior.AllowGet);
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

        #endregion

        [Route("kgid-mi-c-req/{category?}")]
        public ActionResult MotorInsuranceCancellation(int? category)
        {
            VM_MotorInsuranceCancellation obj = new VM_MotorInsuranceCancellation();
            obj = _IMotorInsuranceVehicleDetailsBll.GetCancelReasonList();
            return View(obj);
        }
        public JsonResult MICheckValidVehicleNo(string VehicleNo, int Category)
        {
            string empid = "";
            string appid = "";
            string isSuccess = "";
            string result = _IMotorInsuranceVehicleDetailsBll.CheckVehicleNo(VehicleNo, Category);

            if (result.Length > 1)
            {
                string[] value = result.Split(',');
                empid = value[1];
                appid = value[0];
                isSuccess = "True";
                return Json(new { Sucess = isSuccess, Empid = empid, Appid = appid }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                isSuccess = result.ToString();
                return Json(new { Sucess = isSuccess, Empid = empid, Appid = appid }, JsonRequestBehavior.AllowGet);
            }


        }

        public int MIAppCancelRequestAction(VM_MotorInsuranceCancellation objMIcancellation)
        {
            string message = string.Empty;
            //objMIcancellation.App_Proposer_ID = objMIcancellation.App_Proposer_ID
            //objMIcancellation.MI_App_Reference_ID = Convert.ToInt64(Session["RID"]);

            int result = _IMotorInsuranceVehicleDetailsBll.MIAppCancelRequestAction(objMIcancellation);
            return result;
        }

        public JsonResult VehicleDetailsForCancellation(string VehicleNo)
        {
            VM_MotorInsuranceCancellation obj = new VM_MotorInsuranceCancellation();
            obj = _IMotorInsuranceVehicleDetailsBll.VehicleDetailsForCancellation(VehicleNo);
            return Json(obj, JsonRequestBehavior.AllowGet);
            //return this.PartialView("_MIVehicleDetailsForCancellation", obj);
        }

        public JsonResult GetTypeOfVehiclebasedonCategory(int categoryID)
        {
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetTypeOfVehiclebasedonCategory(categoryID);
            return Json(_VehicleDetails, JsonRequestBehavior.AllowGet);

        }

        // ICT Source 18-09-2021
        public JsonResult PayMIChallanDetails(string RefNos, string ChallanAmount, string DDOCODE, string Type)
        {
            Logger.LogMessage(TracingLevel.INFO, "PayMIChallanDetails: Ref:" + RefNos + "ChallanAmount:" + ChallanAmount + "DDOCODE:" + DDOCODE);

            bool isSuccess = false;
            string message = string.Empty;
            string result = string.Empty;
            string XMLFileName = string.Empty;
            string SANORDERNO = string.Empty;
            string SANDATE = string.Empty;
            string hoacode = string.Empty;
            string voucherno = string.Empty;
            string SignedresultContent = string.Empty;
            String vDataContent = string.Empty;

            if (1 == 1)
            {
                try
                {
                    Session["KIIReturn"] = "MBPay";
                    string[] subs = RefNos.Split('$');
                    deptRefNum = subs[0];
                    XMLFileName = subs[1];
                    SANORDERNO = subs[2];
                    SANDATE = subs[3];
                    voucherno = subs[4];
                    hoacode = subs[5];

                    transactiondate = DateTime.Now.ToString("ddMMyyyy");
                    chlntransactiondate = DateTime.Now.ToString("dd/MM/yyyy").Replace('-', '/');

                    amount = Convert.ToInt64(ChallanAmount);

                    ///////KII Integration Start//////
                    string currPath = string.Empty;

                    string KIIsignresponse = string.Empty;

                    Logger.LogMessage(TracingLevel.INFO, "File Generation start");
                    string resFile = TFCNewPolicyPremiumDATA(Convert.ToInt64(amount), deptRefNum, DDOCODE, SANORDERNO, SANDATE, voucherno, hoacode);
                    Logger.LogMessage(TracingLevel.INFO, "File Generation End");

                    vDataContent = System.IO.File.ReadAllText(resFile);
                    string vApiUri = @"http://49.206.243.84:6060/rest-service/kgid/SignXml";
                    StringContent data = new StringContent(vDataContent, Encoding.UTF8, "text/xml");

                    using (var client = new HttpClient())
                    {
                        var response = client.PostAsync(vApiUri, data).Result;

                        if (response.IsSuccessStatusCode)
                        {

                            SignedresultContent = response.Content.ReadAsStringAsync().Result;

                        }
                        else
                        {
                            Logger.LogMessage(TracingLevel.INFO, response.IsSuccessStatusCode.ToString());
                            isSuccess = false;
                            message = "Error in Signature Generation api :" + response.IsSuccessStatusCode;
                            return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, ex.ToString());
                    isSuccess = false;
                    message = "Error Gen file:" + ex.Message.ToString();
                    return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
                }

                try
                {

                    if (SignedresultContent != null)
                    {
                        var KIISFTPHost = System.Configuration.ConfigurationManager.AppSettings["KIISFTPHost"];
                        Int32 KIISFTPPORT = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KIISFTPPORT"]);
                        var KIISFTUSERID = System.Configuration.ConfigurationManager.AppSettings["KIISFTUSERID"];
                        var KIISFTPASSWORD = System.Configuration.ConfigurationManager.AppSettings["KIISFTPASSWORD"];

                        String XMLFilePath = TFCNewPolicyPremiumXML(SignedresultContent, vDataContent, deptRefNum, XMLFileName);
                        var connectionInfo = new PasswordConnectionInfo(KIISFTPHost, KIISFTPPORT, KIISFTUSERID, KIISFTPASSWORD);

                        // FOR UPLOAD XML
                        try
                        {
                            using (var sftp = new SftpClient(connectionInfo))
                            {
                                using (var ssh = new SshClient(connectionInfo))
                                {
                                    ssh.Connect();
                                    sftp.Connect();
                                    using (Stream stream = System.IO.File.OpenRead(XMLFilePath))
                                    {
                                        sftp.UploadFile(stream, @"/EA_KID/BPS025/request/" + XMLFileName + ".xml");
                                    }
                                    message = "File Uploaded Successfully";
                                    sftp.Disconnect();
                                    ssh.Disconnect();

                                    string Requestresult = string.Empty;
                                    Requestresult = _IMotorInsuranceVehicleDetailsBll.UpdateBPS025RequestBll("Yes", deptRefNum);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogMessage(TracingLevel.INFO, ex.ToString());
                            isSuccess = false;
                            message = "Upload File Error:" + ex.Message.ToString();
                            return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
                        }

                        // Wait
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();
                        Thread.Sleep(30000);
                        stopWatch.Stop();

                        // FOR DOWNLOAD XML
                        try
                        {
                            using (var sftp = new SftpClient(connectionInfo))
                            {
                                using (var ssh = new SshClient(connectionInfo))
                                {
                                    ssh.Connect();
                                    sftp.Connect();

                                    // download file
                                    string localFilePath = Server.MapPath("~/MIDocuments/BPS025/acknowledgement/" + XMLFileName + "-ack" + ".xml");

                                    using (FileStream file = System.IO.File.OpenWrite(localFilePath))
                                    {
                                        sftp.DownloadFile(@"/OutGoing/EA_KID/BPS025/acknowledgement/" + XMLFileName + "-ack" + ".xml", file);
                                    }
                                    sftp.Disconnect();
                                    ssh.Disconnect();
                                    message += "</br> File Downloaded SuccessFully";
                                    string ackresult = string.Empty;
                                    ackresult = _IMotorInsuranceVehicleDetailsBll.UpdateBPS025AckBll(localFilePath, deptRefNum);

                                    isSuccess = true;

                                    return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogMessage(TracingLevel.INFO, ex.ToString());
                            isSuccess = false;
                            message = "Download File Error:" + ex.Message.ToString();
                            return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    // IF SignedresultContent IS NULL
                    else
                    {
                        isSuccess = false;
                        message = "Signed Data Not Captured";
                        return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, ex.ToString());
                    isSuccess = false;
                    message = ex.Message.ToString();
                    return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);

                }

            }
            else
            {
                isSuccess = false;
                message = "Payment Unsuccessful";
            }
            return Json(new
            {
                IsSuccess = isSuccess,
                Message = message,
                Result = result
            }, JsonRequestBehavior.AllowGet);
        }
        // ICT CHANGE
        public JsonResult InsertMIChallanDetails(string RefNos, string ChallanAmount, string sanno, DateTime sandate, string voucherno, string hoa, string ddocode, string Type)
        {
            bool isSuccess = false;
            string message = string.Empty;
            string result = string.Empty;
            HttpCookie cookieAppRefNo = HttpContext.Request.Cookies.Get("amrn");
            HttpCookie cookieChalnAmt = HttpContext.Request.Cookies.Get("amch");
            string AppRefNo = cookieAppRefNo.Value;
            AppRefNo = System.Web.HttpUtility.UrlDecode(AppRefNo);
            string ChalnAmt = cookieChalnAmt.Value;
            if (Request.Cookies["amrn"] != null && Request.Cookies["amch"] != null)
            {
                var ar = new HttpCookie("amrn");
                var ca = new HttpCookie("amch");
                ar.Expires = DateTime.Now.AddDays(-1);
                ca.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(ar);
                Response.Cookies.Add(ca);
            }
            if (ChallanAmount == ChalnAmt && AppRefNo == RefNos)
            {
                //string abc = Session["chlnamt"].ToString();
                if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
                {
                    //##ICT##
                    //result = _IMotorInsuranceVehicleDetailsBll.SaveMIChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]), RefNos, Convert.ToInt32(ChallanAmount), Type);
                    result = _IMotorInsuranceVehicleDetailsBll.InsertChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]), RefNos, Convert.ToInt32(ChallanAmount), sanno, sandate, voucherno, hoa, ddocode, Type);
                }
                else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
                {
                    //##ICT##
                    // result = _IMotorInsuranceVehicleDetailsBll.SaveMIChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]), RefNos, Convert.ToInt32(ChallanAmount), Type);
                    result = _IMotorInsuranceVehicleDetailsBll.InsertChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]), RefNos, Convert.ToInt32(ChallanAmount), sanno, sandate, voucherno, hoa, ddocode, Type);
                }
                else
                {
                    //##ICT##
                    //result = _IMotorInsuranceVehicleDetailsBll.SaveMIChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]), RefNos, Convert.ToInt32(ChallanAmount), Type);
                    result = _IMotorInsuranceVehicleDetailsBll.InsertChallanDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]), RefNos, Convert.ToInt32(ChallanAmount), sanno, sandate, voucherno, hoa, ddocode, Type);
                }
                if (result != "")
                {
                    isSuccess = true;
                    message = "Challan Generated " + result;
                }

                return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                isSuccess = false;
                message = "Payment Unsuccessful";
                return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
            }
        }
        // ICT CHANGE
        public string TFCNewPolicyPremiumDATA(long ChallanAmount, string Refno, string DDOCODE, string SANORDERNO, string SANDATE, string voucherno, string hoacode)
        {
            Logger.LogMessage(TracingLevel.INFO, "SignData File Creating in MIDocuments/0");
            string newFile = Server.MapPath("~/MIDocuments/0/" + Refno + ".txt");
            Logger.LogMessage(TracingLevel.INFO, "MI BPSO25 SignData File Path: " + newFile);
            
            System.IO.FileInfo fi = new System.IO.FileInfo(newFile);
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (fi.Exists)
                {
                    fi.Delete();
                }

                // Create a new file     
                using (System.IO.StreamWriter strmwrtr = fi.CreateText())
                {
                    strmwrtr.WriteLine(@"<?xml version='1.0' encoding='UTF-8' standalone='no'?>");
                    strmwrtr.WriteLine("<generalBillBSRq:envelopedData xmlns:generalBillBSRq='http://request.generalbillbsurl.postal.bl.integration.ifms.gov.in/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:schemaLocation='http://request.generalbillbsurl.postal.bl.integration.ifms.gov.in/ DCVehicleInsurance.xsd '>");
                    strmwrtr.WriteLine("<data>");
                    strmwrtr.WriteLine("<generalBillDtlsReq>");
                    strmwrtr.WriteLine("<integrationCode>BPS025</integrationCode>");
                    strmwrtr.WriteLine("<blReqId>" + Refno + "</blReqId>");
                    strmwrtr.WriteLine("<sanctionOrderNo>" + SANORDERNO + "</sanctionOrderNo>");
                    strmwrtr.WriteLine("<sanctionOrderDate>" + SANDATE + "</sanctionOrderDate>");
                    strmwrtr.WriteLine("<ddoCode>" + DDOCODE + "</ddoCode>");   // ?First KGID
                    strmwrtr.WriteLine("<billType>DC Bill</billType>");
                    strmwrtr.WriteLine("<claimType>Vehicle Insurance Bill</claimType>");
                    strmwrtr.WriteLine("<blSector>1</blSector>");
                    strmwrtr.WriteLine("<pmntModeCode>2</pmntModeCode>");
                    strmwrtr.WriteLine("<expenditureHoa>" + hoacode + "</expenditureHoa>");
                    strmwrtr.WriteLine("<hoaChargedVoted>2</hoaChargedVoted>");
                    strmwrtr.WriteLine("<nillBill>Y</nillBill>");
                    strmwrtr.WriteLine("<grossAmt>" + ChallanAmount + "</grossAmt>");
                    strmwrtr.WriteLine("<deductionAmt>" + ChallanAmount + "</deductionAmt>");
                    strmwrtr.WriteLine("<netAmt>0</netAmt>");

                    strmwrtr.WriteLine("<subVoucherList>");
                    strmwrtr.WriteLine("<subVoucherDtls>");
                    strmwrtr.WriteLine("<subVoucherNo>" + voucherno + "</subVoucherNo>");
                    strmwrtr.WriteLine("<descriptionOfExpenditure>Vehicle Insurance Bill</descriptionOfExpenditure>");
                    strmwrtr.WriteLine("<billDate>" + SANDATE + "</billDate>");
                    strmwrtr.WriteLine("<grossAmt>" + ChallanAmount + "</grossAmt>");
                    strmwrtr.WriteLine("</subVoucherDtls>");
                    strmwrtr.WriteLine("</subVoucherList>");

                    strmwrtr.WriteLine("<deductionCodeList>");
                    strmwrtr.WriteLine("<deductionDtls>");
                    //strmwrtr.WriteLine("<deductionCode>12C</deductionCode>");
                    strmwrtr.WriteLine("<deductionCode>D90</deductionCode>");
                    strmwrtr.WriteLine("<deductionHoa>801100105200000</deductionHoa>");
                    strmwrtr.WriteLine("<deductionAmt>" + ChallanAmount + "</deductionAmt>");
                    strmwrtr.WriteLine("</deductionDtls>");
                    strmwrtr.WriteLine("</deductionCodeList>");
                    strmwrtr.WriteLine("</generalBillDtlsReq>");
                    strmwrtr.WriteLine("</data>");
                    strmwrtr.WriteLine("</generalBillBSRq:envelopedData>");

                }

                return newFile;
            }
            catch (Exception Ex)
            {
                Logger.LogMessage(TracingLevel.ERROR, Ex.StackTrace);
                return null;
            }
        }
        
        // ICT CHANGE
        public string TFCNewPolicyPremiumXML(string signeddata, string xmldata, String REFNO, String XMLFileName)
        {

            string newFile = Server.MapPath("~/MIDocuments/BPS025/request/" + XMLFileName + ".xml");

            System.IO.FileInfo fi = new System.IO.FileInfo(newFile);
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (fi.Exists)
                {
                    fi.Delete();
                }

                // Create a new file     
                using (System.IO.StreamWriter strmwrtr = fi.CreateText())
                {
                    string xml2 = signeddata;
                    strmwrtr.WriteLine(xml2);
                }
                return newFile;
            }
            catch (Exception Ex)
            {
                Logger.LogMessage(TracingLevel.ERROR, Ex.StackTrace);
                return null;
            }
        }

        #region View Payment Status
        [Route("mi-dpt-psoa")]
        public ActionResult MotorInsurancePaymentStatusOfApplication()
        {
            VM_MotorInsurancePaymentStatus PaymentStatusDetails = null;
            if (Convert.ToInt32(Session["SelectedCategory"]) == 2)
            {
                PaymentStatusDetails = _IMotorInsuranceVehicleDetailsBll.MotorInsurancePaymentStatusBll(Session["UID"].ToString());
            }

            return View(PaymentStatusDetails);

        }
        #endregion

        public JsonResult DownloadBPS025Ack(string XMLFileName, string deptRefNum)
        {
            bool isSuccess = false;
            string message = string.Empty;
            string result = string.Empty;

            var KIISFTPHost = System.Configuration.ConfigurationManager.AppSettings["KIISFTPHost"];
            Int32 KIISFTPPORT = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KIISFTPPORT"]);
            var KIISFTUSERID = System.Configuration.ConfigurationManager.AppSettings["KIISFTUSERID"];
            var KIISFTPASSWORD = System.Configuration.ConfigurationManager.AppSettings["KIISFTPASSWORD"];

            var connectionInfo = new PasswordConnectionInfo(KIISFTPHost, KIISFTPPORT, KIISFTUSERID, KIISFTPASSWORD);

            // FOR DOWNLOAD XML
            try
            {
                using (var sftp = new SftpClient(connectionInfo))
                {
                    using (var ssh = new SshClient(connectionInfo))
                    {
                        ssh.Connect();
                        sftp.Connect();

                        // download file
                        string localFilePath = Server.MapPath("~/MIDocuments/BPS025/acknowledgement/" + XMLFileName + "-ack" + ".xml");

                        using (FileStream file = System.IO.File.OpenWrite(localFilePath))
                        {
                            sftp.DownloadFile(@"/OutGoing/EA_KID/BPS025/acknowledgement/" + XMLFileName + "-ack" + ".xml", file);
                        }
                        sftp.Disconnect();
                        ssh.Disconnect();
                        message += "</br> File Downloaded SuccessFully";
                        string ackresult = string.Empty;
                        ackresult = _IMotorInsuranceVehicleDetailsBll.UpdateBPS025AckBll(localFilePath, deptRefNum);

                        isSuccess = true;

                        return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, ex.ToString());
                isSuccess = false;
                message = "Download File Error:" + ex.Message.ToString();
                return Json(new { IsSuccess = isSuccess, Message = message, Result = result }, JsonRequestBehavior.AllowGet);
            }

        }

        [Route("mi-viewchl-det/{ID}")]
        public ActionResult VerifyMIChallanDetails(string ID)
        {
            VM_DDOVerificationDetailsMI ApplicationDetails = null;

            ApplicationDetails = _IMotorInsuranceVehicleDetailsBll.GetChallanDetailsBll(ID, Convert.ToInt64(Session["UID"]));

            return View(ApplicationDetails);
        }

        [Route("mi-printchl-det/{ID}")]
        public ActionResult PrintMIChallan(string ID)
        {
            VM_DDOVerificationDetailsMI ApplicationDetails = null;

            ApplicationDetails = _IMotorInsuranceVehicleDetailsBll.GetChallanDetailsBll(ID, Convert.ToInt64(Session["UID"]));

            return View(ApplicationDetails);
        }

        //TODO's
    }
}