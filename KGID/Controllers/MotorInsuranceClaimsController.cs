﻿using BLL.MBClaimsBLL;
using Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KGID_Models.KGID_MB_Claim;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGID.Controllers
{
    public class MotorInsuranceClaimsController : Controller
    {
        private readonly IMBClaimsBLL _IMBClaimsBLL;

        public MotorInsuranceClaimsController()
        {
            this._IMBClaimsBLL = new MBClaimsBLL();
        }
        // GET: MotorInsuranceClaims
        [Route("mi-agy-odca")]
        [Route("mi-e-odc")]
        [Route("mi-dpt-odc")]
        public ActionResult OwnDamageClaimApplication()
        {
            VM_MIOwnDamageClaimDetails MIODclaimDetails;
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            else
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            //ViewBag.Departments = new SelectList(MIODclaimDetails.MIOwnDamageClaimDetails(), "id", "name");
            return View(MIODclaimDetails);
        }
        public JsonResult OwnDamageClaimApplicationList()
        {
            VM_MIOwnDamageClaimDetails MIODclaimDetails;
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            else
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            //ViewBag.Departments = new SelectList(MIODclaimDetails.MIOwnDamageClaimDetails(), "id", "name");
            return Json(MIODclaimDetails, JsonRequestBehavior.AllowGet);
        }
        public class ClaimComponentListArray
        {
            public string ID { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
        }
        public ActionResult SaveODClaimDetails(VM_ODClaimApplicationDetails ODCAD)
        //public JsonResult SaveODClaimDetails(List<ClaimComponentListArray> ClaimComponentListArrays1)
        {
            bool isSuccess = false;
            string message = string.Empty;
            long RefNo = 0;
            long result = 0;

            //ODCAD.Odca_proposer_id = Convert.ToInt64(Session["UID"]);
            //ODCAD.Odca_claim_app_no = (Convert.ToString(Session["RID"]) == "0" ? "" : Convert.ToString(Session["RID"]));
            //ODCAD.Odca_category_id = Convert.ToString(Session["Categories"]);
            result = _IMBClaimsBLL.SaveODClaimApplicationDetailsBLL(ODCAD);
            if (result != 0)
            {
                if (result == 2)
                {
                    RefNo = Convert.ToInt64(Session["RID"]);
                    isSuccess = true;
                    message = "Proposer details saved successfully";
                }
                else if (result > 3)
                {
                    //RefNo = Convert.ToInt64(Session["RID"]);
                    RefNo = result;
                    isSuccess = true;
                    message = "Proposer details saved successfully";
                }
                else
                {
                    RefNo = result;
                    isSuccess = false;
                    message = "Proposer details saved unsuccessfull";
                }

            }
            else
            {
                RefNo = result;
                isSuccess = false;
                message = "Proposer details saved unsuccessfull";
            }

            return Json(new { IsSuccess = isSuccess, Message = message, ReferenceNo = RefNo }, JsonRequestBehavior.AllowGet);

            //string data = "";
            //foreach (ClaimComponentListArray ClaimComponentaistarray in ClaimComponentListArrays)
            //{
            //    data += ClaimComponentaistarray.ID + "" + ClaimComponentaistarray.Type;
            //}
            //return Json(data);
        }
        //public ActionResult InsertComponentDetails(ClaimComponentList objClaimComponentList)
        //{
        //    return View();
        //}
        public ActionResult GetODClaimDetails(string PolicyNumber)
        {
            bool isSuccess = false;
            string message = string.Empty;
            //long RefNo = 0;
            //long result = 0;
            long ProposerId = Convert.ToInt64(Session["UID"]);
            //ODCAD.Odca_claim_app_no = (Convert.ToString(Session["RID"]) == "0" ? "" : Convert.ToString(Session["RID"]));
            //ODCAD.Odca_category_id = Convert.ToString(Session["Categories"]);
            VM_ODClaimApplicationDetails ODClaimDetails = _IMBClaimsBLL.GetODClaimApplicationDetailsBLL(ProposerId, PolicyNumber);
            //result = _IMBClaimsBLL.SaveODClaimApplicationDetailsBLL(ODCAD);

            if (ODClaimDetails.Odca_claim_app_no != "")
            {
                //RefNo = Convert.ToInt64(Session["RID"]);
                Session["RID"] = ODClaimDetails.Odca_claim_app_no;
                isSuccess = true;
                message = "Proposer details saved successfully";
            }

            return Json(new { IsSuccess = isSuccess, Message = message, Data = ODClaimDetails }, JsonRequestBehavior.AllowGet);

        }
        [Route("mi-odc-soa")]
        public ActionResult OwnDamageClaimStatusOfApplication()
        {
            VM_ODClaimVerificationDetails verificationDetails = null;
            if (Convert.ToInt32(Session["SelectedCategory"]) == 2)
            {
                verificationDetails = _IMBClaimsBLL.GetODClaimApplicationStatusListBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                verificationDetails = _IMBClaimsBLL.GetODClaimApplicationStatusListBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            else if (Convert.ToInt32(Session["SelectedCategory"]) == 1)
            {
                verificationDetails = _IMBClaimsBLL.GetODClaimApplicationStatusListBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            return View(verificationDetails);
        }
        public ActionResult ThirdPartyDamageClaimApplication()
        {
            VM_MIOwnDamageClaimDetails MIODclaimDetails;
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            else
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            return View(MIODclaimDetails);
        }
        #region TestPrint
        public class ComponentReport
        {
            #region Declaration
            int _totalColumn = 3;
            Document _document;
            Font _fontStyle;
            PdfPTable _pdfTable = new PdfPTable(3);
            PdfPCell _pdfCell;
            MemoryStream _memoryStream = new MemoryStream();
            IList<ClaimComponentList1> _Lists = new List<ClaimComponentList1>();
            VM_ODClaimApplicationDetails Data = new VM_ODClaimApplicationDetails();
            #endregion
            public byte[] PrepareReport(VM_ODClaimApplicationDetails list)
            {
                _Lists = list.ClaimComponentListDetails;
                Data = list;
                #region
                _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
                _document.SetPageSize(PageSize.A4);
                _document.SetMargins(20f, 20f, 20f, 20f);
                _pdfTable.WidthPercentage = 100;
                _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                PdfWriter.GetInstance(_document, _memoryStream);
                _document.Open();
                _pdfTable.SetWidths(new float[] { 20f, 150f, 100f });
                #endregion

                this.ReportHeader();
                this.ReportBody();
                _pdfTable.HeaderRows = 2;
                _document.Add(_pdfTable);
                _document.Close();
                return _memoryStream.ToArray();
            }
            private void ReportHeader()
            {
                _fontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
                _pdfCell = new PdfPCell(new Phrase("Karnataka Government Insurance Department", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                //////////////////////////////
                _fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _pdfCell = new PdfPCell(new Phrase("Bangalore - 1" + "\n" + "__________________________________________________________________________________________" + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
            }
            private void ReportBody()
            {
                #region From Address data
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _pdfCell = new PdfPCell(new Phrase("From" + "\n" + "The Director" + "\n" + "(Motor Branch)" + "\n" + "Karnataka Government Insurance Department" + "\n" + "P.B. No.325. Bangalore - 1" + "\n" + "\n" + "To" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region To Address data
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _pdfCell = new PdfPCell(new Phrase("M/s " + Convert.ToString(Data.Odca_policy_number) + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Heading
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _pdfCell = new PdfPCell(new Phrase("No MTB. 1. " + "\n" + "Claim Discharge Form" + "\n" + "Encls:-" + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Heading Date
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _pdfCell = new PdfPCell(new Phrase("Dated: " + DateTime.Now + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Data
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _pdfCell = new PdfPCell(new Phrase("Dear Sir, " + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Data1
                Phrase phrase = new Phrase();
                phrase.Add(new Chunk("     SUBJECT:-", FontFactory.GetFont("Tahoma", 10f, 1)));
                phrase.Add(new Chunk(" Accident to Vehicle No. ", FontFactory.GetFont("Tahoma", 10f, 0)));
                phrase.Add(new Chunk(Convert.ToString(Data.Odca_vehicle_number), FontFactory.GetFont("Tahoma", 10f, 1)));
                phrase.Add(new Chunk(" Owned by Sri ", FontFactory.GetFont("Tahoma", 10f, 0)));
                phrase.Add(new Chunk(Convert.ToString(Data.Odca_policy_number), FontFactory.GetFont("Tahoma", 10f, 1)));
                phrase.Add(new Chunk(" on ", FontFactory.GetFont("Tahoma", 10f, 0)));
                phrase.Add(new Chunk(Convert.ToString(Data.Odca_date_time_of_accident), FontFactory.GetFont("Tahoma", 10f, 1)));
                phrase.Add(new Chunk(" Policy No. ", FontFactory.GetFont("Tahoma", 10f, 0)));
                phrase.Add(new Chunk(Convert.ToString(Data.Odca_policy_number), FontFactory.GetFont("Tahoma", 10f, 1)));
                phrase.Add(new Chunk("\n" + "\n", FontFactory.GetFont("Tahoma", 10f, 0)));
                //_fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                //_pdfCell = new PdfPCell(new Phrase("Accident to Vehicle No. " + Convert.ToString(Data.Odca_vehicle_number) + " Owned by Sri " +Convert.ToString(Data.Odca_policy_number)+" on "+ Convert.ToString(Data.Odca_date_time_of_accident)+" Policy No. " +Convert.ToString(Data.Odca_policy_number)+"\n"+"\n", _fontStyle));
                _pdfCell = new PdfPCell(new Phrase(phrase));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Extra Break Line
                _fontStyle = FontFactory.GetFont("Tahoma", 11f, 0);
                _pdfCell = new PdfPCell(new Phrase("-o-" + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Data2
                Phrase phrase1 = new Phrase();
                phrase1.Add(new Chunk("     With reference to your Estimate No. ", FontFactory.GetFont("Tahoma", 10f, 0)));
                phrase1.Add(new Chunk(Convert.ToString(Data.Odca_claim_app_no), FontFactory.GetFont("Tahoma", 10f, 1)));
                phrase1.Add(new Chunk(" dated ", FontFactory.GetFont("Tahoma", 10f, 0)));
                phrase1.Add(new Chunk(Convert.ToString(Data.Odca_date_time_of_accident), FontFactory.GetFont("Tahoma", 10f, 1)));
                phrase1.Add(new Chunk(" for Rs. ", FontFactory.GetFont("Tahoma", 10f, 0)));
                phrase1.Add(new Chunk(Convert.ToString(Data.Odca_damage_cost), FontFactory.GetFont("Tahoma", 10f, 1)));
                phrase1.Add(new Chunk(" . I am to request you kindly to carry out the repairs to the vehicle subject to the following terms and conditions :- ", FontFactory.GetFont("Tahoma", 10f, 0)));
                phrase1.Add(new Chunk("\n" + "\n", FontFactory.GetFont("Tahoma", 10f, 0)));
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                //_pdfCell = new PdfPCell(new Phrase("With reference to your Estimate No. " + Convert.ToString(Data.Odca_claim_app_no) + " dated " + Convert.ToString(Data.Odca_date_time_of_accident) + " for Rs. " + Convert.ToString(Data.Odca_damage_cost) + " . I am to request you kindly to carry out the repairs to the vehicle subject to the following terms and conditions :- " + "\n" + "\n", _fontStyle));
                _pdfCell = new PdfPCell(new Phrase(phrase1));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Table Components Heading 
                _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _pdfCell = new PdfPCell(new Phrase("S.No.", _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                ///////////////////////////////
                _pdfCell = new PdfPCell(new Phrase("Component", _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _pdfTable.AddCell(_pdfCell);
                //////////////////////////////
                _pdfCell = new PdfPCell(new Phrase("Value", _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Table Components Body 
                _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                int serialNumber = 1;
                foreach (ClaimComponentList1 list in _Lists)
                {
                    _pdfCell = new PdfPCell(new Phrase(serialNumber++.ToString(), _fontStyle));
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfCell);
                    /////////////////////////////////
                    _pdfCell = new PdfPCell(new Phrase(list.Type, _fontStyle));
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfCell);
                    ////////////////////////////////
                    _pdfCell = new PdfPCell(new Phrase(list.Value, _fontStyle));
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfCell);
                    _pdfTable.CompleteRow();
                }
                #endregion
                #region Break Line
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _pdfCell = new PdfPCell(new Phrase("\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Data End1
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _pdfCell = new PdfPCell(new Phrase("After completion of repairs of the vehicle as per conditions stipulated above,Bill of repairs and damaged parts may kindly be arranged to be sent along with the enclosed Claim Discharge Form duly signed by the Insured for taking further action." + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Data End2
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _pdfCell = new PdfPCell(new Phrase("Thanking you." + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Data End3
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _pdfCell = new PdfPCell(new Phrase("Yours faithfully." + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Data End4
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _pdfCell = new PdfPCell(new Phrase("\n" + "\n" + "\n" + "\n" + "For Director," + "\n" + "Insurance Department." + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Data End5
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _pdfCell = new PdfPCell(new Phrase("Copy to:-" + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Data End6
                Phrase phrase2 = new Phrase();
                phrase2.Add(new Chunk("     Sri ", FontFactory.GetFont("Tahoma", 10f, 0)));
                phrase2.Add(new Chunk(Convert.ToString(Data.Odca_policy_number), FontFactory.GetFont("Tahoma", 10f, 1)));
                phrase2.Add(new Chunk(" for favour of information. He is requested to sent the final result of the Police investigation report of accident for takes further action. And also send .. along with .. & Signature after repair photos of vehicle & also send account details of Garage.", FontFactory.GetFont("Tahoma", 10f, 0)));
                phrase2.Add(new Chunk("\n" + "\n", FontFactory.GetFont("Tahoma", 10f, 0)));
                //_fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                //_pdfCell = new PdfPCell(new Phrase("Accident to Vehicle No. " + Convert.ToString(Data.Odca_vehicle_number) + " Owned by Sri " +Convert.ToString(Data.Odca_policy_number)+" on "+ Convert.ToString(Data.Odca_date_time_of_accident)+" Policy No. " +Convert.ToString(Data.Odca_policy_number)+"\n"+"\n", _fontStyle));
                _pdfCell = new PdfPCell(new Phrase(phrase2));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Subject Data End7
                _fontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _pdfCell = new PdfPCell(new Phrase("For Director," + "\n" + "Insurance Department" + "\n" + "___________________________________________________________________________________________________" + "\n" + "\n", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion

                #region extra data
                _fontStyle = FontFactory.GetFont("Tahoma", 11f, 0);
                _pdfCell = new PdfPCell(new Phrase("-o-", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                /////////////////////////////////


            }
        }
        public ActionResult Report(ClaimComponentListArray student)
        {
            ComponentReport componentReport = new ComponentReport();
            // byte[] abytes = componentReport.PrepareReport(GetList());
            return File("", "application/pdf");
        }
        public List<ClaimComponentListArray> GetList()
        {
            List<ClaimComponentListArray> Lists = new List<ClaimComponentListArray>();
            ClaimComponentListArray list = new ClaimComponentListArray();
            for (int i = 1; i <= 6; i++)
            {
                list = new ClaimComponentListArray();
                list.ID = Convert.ToString(i);
                list.Type = "Name" + i;
                list.Value = "Amount" + i;
                Lists.Add(list);
            }
            return Lists;
        }
        public ActionResult Print(string PolicyNumber)
        {
            bool isSuccess = false;
            string message = string.Empty;
            PolicyNumber = "123456F";
            long ProposerId = Convert.ToInt64(Session["UID"]);
            VM_ODClaimApplicationDetails ODClaimDetails = _IMBClaimsBLL.GetODClaimApplicationDetailsBLL(ProposerId, PolicyNumber);
            ComponentReport componentReport = new ComponentReport();
            byte[] abytes = componentReport.PrepareReport(ODClaimDetails);
            return File(abytes, "application/pdf");


            //return Json(new { IsSuccess = isSuccess, Message = message, Data = ODClaimDetails }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region test
        public class StudentReport
        {
            #region Declaration
            int _totalColumn = 3;
            Document _document;
            Font _fontStyle;
            PdfPTable _pdfTable = new PdfPTable(3);
            PdfPCell _pdfCell;
            MemoryStream _memoryStream = new MemoryStream();
            List<Student> _students = new List<Student>();
            #endregion
            public byte[] PrepareReport(List<Student> students)
            {
                _students = students;
                #region
                _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
                _document.SetPageSize(PageSize.A4);
                _document.SetMargins(20f, 20f, 20f, 20f);
                _pdfTable.WidthPercentage = 100;
                _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                PdfWriter.GetInstance(_document, _memoryStream);
                _document.Open();
                _pdfTable.SetWidths(new float[] { 20f, 150f, 100f });
                #endregion

                this.ReportHeader();
                this.ReportBody();
                _pdfTable.HeaderRows = 2;
                _document.Add(_pdfTable);
                _document.Close();
                return _memoryStream.ToArray();
            }
            private void ReportHeader()
            {
                _fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _pdfCell = new PdfPCell(new Phrase("Heading", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                //////////////////////////////
                _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _pdfCell = new PdfPCell(new Phrase("Table List", _fontStyle));
                _pdfCell.Colspan = _totalColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.Border = 0;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
            }
            private void ReportBody()
            {
                #region
                _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _pdfCell = new PdfPCell(new Phrase("Table Content1", _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _pdfCell.ExtraParagraphSpace = 0;
                _pdfTable.AddCell(_pdfCell);
                ///////////////////////////////
                _pdfCell = new PdfPCell(new Phrase("Table Content2", _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _pdfTable.AddCell(_pdfCell);
                //////////////////////////////
                _pdfCell = new PdfPCell(new Phrase("Table Content3", _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                #endregion
                #region Table Body
                _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                int serialNumber = 1;
                foreach (Student student in _students)
                {
                    _pdfCell = new PdfPCell(new Phrase(serialNumber++.ToString(), _fontStyle));
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfCell);
                    /////////////////////////////////
                    _pdfCell = new PdfPCell(new Phrase(student.Name, _fontStyle));
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfCell);
                    ////////////////////////////////
                    _pdfCell = new PdfPCell(new Phrase(student.Roll, _fontStyle));
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfCell);
                    _pdfTable.CompleteRow();
                }
                #endregion
            }
        }

        public ActionResult Report1(Student student)
        {
            StudentReport studentReport = new StudentReport();
            byte[] abytes = studentReport.PrepareReport(GetStudents());
            return File(abytes, "application/pdf");
        }

        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            Student student = new Student();
            for (int i = 1; i <= 250; i++)
            {
                student = new Student();
                student.Id = i;
                student.Name = "Student" + i;
                student.Roll = "Roll" + i;
                students.Add(student);
            }
            return students;
        }

        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Roll { get; set; }
        }
        #endregion
        //MI Claim WorkFlow
        #region Motor Insurance CW WorkFlow
        [Route("mi-odcdcw-dpt/{Category}")]
        [Route("mi-odcdcw-agy/{Category}")]
        [Route("mi-odcdcw-emp/{Category}")]
        [CustomAuthorize("Caseworker")]
        public ActionResult ODClaimDetailsForCWVerification(string Category)
        {
            ViewBag.Verifier = Verifiers.CW;
            VM_ODClaimVerificationDetails odcverificationDetails = _IMBClaimsBLL.GetEmployeeDetailsForCWVerificationBLL(Convert.ToInt64(Session["UID"]), Category);
            return View("ClaimVerificationDetails", odcverificationDetails);
        }
        [Route("mi_odc_cw_va/{empId}/{applicationId}/{refNo}/{category}")]
        [CustomAuthorize("Caseworker")]
        public ActionResult MIODClaimCWVerification(string empId, string applicationId, string refNo, string category)
        {
            VM_MIODClaimDeptVerficationDetails verificationDetails = new VM_MIODClaimDeptVerficationDetails();
            if (Convert.ToInt64(refNo) != 0)
            {
                Session["RID"] = refNo;
            }
            if (Convert.ToInt64(empId) != 0)
            {
                verificationDetails = _IMBClaimsBLL.GetWorkFlowDetailsBLL(Convert.ToInt64(refNo), Convert.ToInt32(category));
                Session["RUID"] = empId;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("SaveCWMIODCVData")]
        [CustomAuthorize("Caseworker")]
        public ActionResult InsertODClaimVerifyDetailsCW(VM_MIODClaimDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMBClaimsBLL.SaveVerifiedDetailsBLL(objVerifyDetails);
            return RedirectToAction("ODClaimDetailsForCWVerification", "MotorInsuranceClaims", new { @Category = objVerifyDetails.Category });
        }
        #endregion

        #region Motor Insurance Superintendent WorkFlow
        [Route("mi-odcdsi-dpt/{Category}")]
        [Route("mi-odcdsi-agy/{Category}")]
        [Route("mi-odcdsi-emp/{Category}")]
        [CustomAuthorize("Superintendent")]
        public ActionResult ODClaimDetailsForsuperintendentVerification(string Category)
        {
            ViewBag.Verifier = Verifiers.SUPERINTENDENT;
            VM_ODClaimVerificationDetails odcverificationDetails = _IMBClaimsBLL.GetEmployeeDetailsForSuperintendentVerificationBLL(Convert.ToInt64(Session["UID"]), Category);
            return View("ClaimVerificationDetails", odcverificationDetails);
        }
        [Route("mi_odc_si_va/{empId}/{applicationId}/{refNo}/{category}")]
        [CustomAuthorize("Superintendent")]
        public ActionResult MIODClaimSuperintendentVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MIODClaimDeptVerficationDetails verificationDetails = new VM_MIODClaimDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {
                verificationDetails = _IMBClaimsBLL.GetWorkFlowDetailsBLL(refNo, category);
                Session["RUID"] = empId;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("SaveSIMIODCVData")]
        [CustomAuthorize("Superintendent")]
        public ActionResult InsertODClaimVerifyDetailsSuperintendent(VM_MIODClaimDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMBClaimsBLL.SaveVerifiedDetailsBLL(objVerifyDetails);
            return RedirectToAction("ODClaimDetailsForsuperintendentVerification", "MotorInsuranceClaims", new { @Category = objVerifyDetails.Category });
        }
        #endregion

        #region Motor Insurance DD WorkFlow
        [Route("mi-odcddd-dpt/{Category}")]
        [Route("mi-odcddd-agy/{Category}")]
        [Route("mi-odcddd-eply/{Category}")]
        [CustomAuthorize("Deputy Director")]
        public ActionResult ODClaimDetailsForDDVerification(string Category)
        {
            ViewBag.Verifier = Verifiers.DEPUTYDIRECTOR;
            VM_ODClaimVerificationDetails verificationDetails = _IMBClaimsBLL.GetEmployeeDetailsForDDVerificationBLL(Convert.ToInt64(Session["UID"]), Category);
            return View("ClaimVerificationDetails", verificationDetails);
        }
        [Route("mi_odc_dd_va/{empId}/{applicationId}/{refNo}/{category}")]
        [CustomAuthorize("Deputy Director")]
        public ActionResult MIODClaimDDVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MIODClaimDeptVerficationDetails verificationDetails = new VM_MIODClaimDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails = _IMBClaimsBLL.GetWorkFlowDetailsBLL(refNo, category);
                Session["RUID"] = empId;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("SaveDDMIODCVData")]
        [CustomAuthorize("Deputy Director")]
        public ActionResult InsertODClaimVerifyDetailsDD(VM_MIODClaimDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMBClaimsBLL.SaveVerifiedDetailsBLL(objVerifyDetails);
            return RedirectToAction("ODClaimDetailsForDDVerification", "MotorInsuranceClaims", new { @Category = objVerifyDetails.Category });
        }
        #endregion

        #region Motor Insurance D WorkFlow
        [Route("mi-odcdd-dpt/{Category}")]
        [Route("mi-odcdd-agy/{Category}")]
        [Route("mi-odcdd-eply/{Category}")]
        [CustomAuthorize("Director")]
        public ActionResult ODClaimDetailsForDVerification(string Category)
        {
            ViewBag.Verifier = Verifiers.DIRECTOR;
            VM_ODClaimVerificationDetails verificationDetails = _IMBClaimsBLL.GetEmployeeDetailsForDVerificationBLL(Convert.ToInt64(Session["UID"]), Category);
            return View("VerificationDetails", verificationDetails);
        }
        [Route("mi_odc_d_va/{empId}/{applicationId}/{refNo}/{category}")]
        [CustomAuthorize("Director")]
        public ActionResult MIODClaimDVerification(long empId, long applicationId, long refNo, int category)
        {
            VM_MIODClaimDeptVerficationDetails verificationDetails = new VM_MIODClaimDeptVerficationDetails();
            if (refNo != 0)
            {
                Session["RID"] = refNo;
            }
            if (empId != 0)
            {

                verificationDetails = _IMBClaimsBLL.GetWorkFlowDetailsBLL(applicationId, category);
                Session["RUID"] = empId;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("SaveDMIVData")]
        [CustomAuthorize("Director")]
        public ActionResult InsertODClaimVerifyDetailsD(VM_MIODClaimDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMBClaimsBLL.SaveVerifiedDetailsBLL(objVerifyDetails);
            return RedirectToAction("ODClaimDetailsForDVerification", "MotorInsuranceClaims", new { @Category = objVerifyDetails.Category });
        }
        #endregion


        [Route("mi-odcapd")]
        public ActionResult ODClaimApprovedAppDetails(string Category)
        {
            //ViewBag.Verifier = Verifiers.DEPUTYDIRECTOR;
            VM_ODClaimApprovedApplicationDetails ApprovedAppDetails = _IMBClaimsBLL.GetApprovedApplicationListBLL(Convert.ToInt64(Session["UID"]), Category);
            return View(ApprovedAppDetails);
        }
        [Route("mi_odc_aprvd_app/{empId}/{applicationId}/{refNo}/{PolicyNo}/{category}")]
        public ActionResult ODClaimWorkOrderView(long empId, long applicationId, long refNo, string PolicyNo, string Category)
        {
            VM_ODClaimWorkOrderDetails ODClaimDetails = _IMBClaimsBLL.GetODClaimAprvdAppDetailsBLL(empId, PolicyNo, Category);
            return View(ODClaimDetails);
        }
        #region Motor Insurance Surveyor WorkFlow
        [Route("mi-odcdcw-surveyor")]
        public ActionResult ODClaimDetailsForSurveyorVerification()
        {
            //ViewBag.Verifier = Verifiers.CW;
            VM_ODClaimSurveyorVerificationDetails odcvDetails = _IMBClaimsBLL.GetEmployeeDetailsForSurveyorVerificationBLL(Convert.ToInt64(Session["UID"]));
            return View(odcvDetails);
        }
        [Route("mi_odc_surveyor_va/{empId}/{applicationId}/{refNo}/{category}")]
        public ActionResult MIODClaimSurveyorVerification(string empId, string applicationId, string refNo, string category)
        {
            VM_MIODClaimDeptVerficationDetails verificationDetails = new VM_MIODClaimDeptVerficationDetails();
            if (Convert.ToInt64(refNo) != 0)
            {
                Session["RID"] = refNo;
            }
            if (Convert.ToInt64(empId) != 0)
            {
                verificationDetails = _IMBClaimsBLL.GetWorkFlowDetailsBLL(Convert.ToInt64(refNo), Convert.ToInt32(category));
                Session["RUID"] = empId;
            }
            return View(verificationDetails);
        }
        [HttpPost]
        [Route("SaveSurveyorMIODCVData")]
        //[CustomAuthorize("Surveyor")]
        public ActionResult InsertODClaimVerifyDetailsSurveyor(VM_MIODClaimDeptVerficationDetails objVerifyDetails)
        {
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            string result = _IMBClaimsBLL.SaveVerifiedDetailsBLL(objVerifyDetails);
            return RedirectToAction("ODClaimDetailsForSurveyorVerification", "MotorInsuranceClaims", new { @Category = objVerifyDetails.Category });
        }
        #endregion
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
        public ActionResult GetComponentList()
        {
            List<KeyValuePair<long, string>> distlist = new List<KeyValuePair<long, string>>();

            List<tbl_od_cost_component_master> getdistlist = _IMBClaimsBLL.GetComponentListBLL();

            foreach (var item in getdistlist)
            {
                distlist.Add(new KeyValuePair<long, string>(item.odcc_id, item.odcc_description));
            }

            return Json(distlist, JsonRequestBehavior.AllowGet);
        }

        [Route("mi-dpt-odc-mvcappform")]
        public ActionResult MvcClaimApplicationForm()
        {


            VM_MIOwnDamageClaimDetails MIODclaimDetails;
            GetVehicleChassisPolicyDetails MVCClaimDetails = new GetVehicleChassisPolicyDetails();

            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                MIODclaimDetails = _IMBClaimsBLL.GetMIOwnDamageClaimDetailsBLL(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["Categories"]));
            }
            else
            {
                MVCClaimDetails.DistrictList = _IMBClaimsBLL.GetDistrictListBLL();
                MVCClaimDetails.TalukaList = _IMBClaimsBLL.GetTalukListBLL(0);
                MVCClaimDetails.RemarksList = _IMBClaimsBLL.GetRemarksBLL();
                MVCClaimDetails.InjuryList = _IMBClaimsBLL.GetInjuryListBLL();
                MVCClaimDetails.StateList = _IMBClaimsBLL.GetstateListBLL();
                MVCClaimDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCApplicationFormDataBLL();
                MVCClaimDetails.otherDetailsData = _IMBClaimsBLL.GetDraftDetailsBLL();


            }
            //ViewBag.Departments = new SelectList(MIODclaimDetails.MIOwnDamageClaimDetails(), "id", "name");
            return View(MVCClaimDetails);
        }
        public JsonResult GetVehiclePolicyAndChassisDetails(string textDetails)
        {
            GetVehicleChassisPolicyDetails VehicleDetailsModel = new GetVehicleChassisPolicyDetails();
            VehicleDetailsModel.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetVehicleAndPolicyDetailsBLL(textDetails);
            VehicleDetailsModel.GetVehicleChassisPolicyDetailsList[0].OD_to_date1 = (VehicleDetailsModel.GetVehicleChassisPolicyDetailsList[0].OD_to_date).Value.ToString("dd/MM/yyyy");
            VehicleDetailsModel.GetVehicleChassisPolicyDetailsList[0].OD_from_date1 = (VehicleDetailsModel.GetVehicleChassisPolicyDetailsList[0].OD_from_date).Value.ToString("dd/MM/yyyy");
            VehicleDetailsModel.GetVehicleChassisPolicyDetailsList[0].TP_from_date1 = (VehicleDetailsModel.GetVehicleChassisPolicyDetailsList[0].TP_from_date).Value.ToString("dd/MM/yyyy");
            VehicleDetailsModel.GetVehicleChassisPolicyDetailsList[0].TP_to_date1 = (VehicleDetailsModel.GetVehicleChassisPolicyDetailsList[0].TP_to_date).Value.ToString("dd/MM/yyyy");

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
            model.loginId = Convert.ToInt64(Session["UID"]);
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
            var j = 0;
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();

            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);

            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(Appno);
            GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_to_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_from_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_from_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_to_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(Appno);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(Appno);
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

            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(ApplicationNo);

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

            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            
        //GetDetails.MvcClaimWorkFlowDetails = _IMBClaimsBLL.MvcClaimWorkFlowDetailsBLL(appid, chassis);
            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_to_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_from_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_from_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_to_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(appid);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
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
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                
            }
            return View(GetDetails);
        }
        [Route("mvc_sup_va/{chassis}/{appid}")]
        public ActionResult MVCClaimsuperintendedVerification(long appid, string chassis)
        {
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();

            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);
            

            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_to_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_from_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_from_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_to_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(appid);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
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
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                
            }
            return View(GetDetails);
        }
        public JsonResult MvcSendToAd(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            model.claim_Amount = "0";
              var result = _IMBClaimsBLL.UpdateWork_flow_DetailsBLL(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [Route("mvc_AD_va/{chassis}/{appid}")]
        public ActionResult MVCClaimADVerification(long appid, string chassis)
        {
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();

            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);


            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_to_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_from_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_from_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_to_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(appid);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
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
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

            }
            return View(GetDetails);
        }
        [Route("mvc_ad_ver")]
        public ActionResult MVCClaimADApprovalProcess()
        {
            ViewBag.Verifier = Verifiers.ASSITANTDIRECTOR;
            ViewBag.Verify = 15;
            
            GetVehicleChassisPolicyDetails GetDetails = new GetVehicleChassisPolicyDetails();
            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCApplicationFormDataBLL();
            return View(GetDetails);
        }
        public JsonResult MvcSendToDir(GetVehicleChassisPolicyDetails model)
        {
            model.loginId = Convert.ToInt64(Session["UID"]);
            model.Category_id = Convert.ToInt32(Session["SelectedCategory"]);
            model.claim_Amount = "0";
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

            GetDetails.GetVehicleChassisPolicyDetailsList = _IMBClaimsBLL.GetMVCGetDetailsOnChassisBLL(chassis);


            GetDetails.PetitionerList = _IMBClaimsBLL.PetitionerDetailsListBLL(appid);
            GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_to_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_from_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].OD_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_from_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_from_date).Value.ToString("dd/MM/yyyy");
            GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_to_date1 = (GetDetails.GetVehicleChassisPolicyDetailsList[0].TP_to_date).Value.ToString("dd/MM/yyyy");
            GetDetails.RespondantList = _IMBClaimsBLL.GetMVCRespondantDetailsBLL(appid);
            GetDetails.CourtDetailsList = _IMBClaimsBLL.GetMVCdetailsofCourtBLL(appid);
            GetDetails.CourtDetailsList[0].CourtTime = (GetDetails.CourtDetailsList[0].Court_DateTime).ToString("dd/MM/yyyy");
            GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
            GetDetails.otherDetailsData = _IMBClaimsBLL.GetOtherDocdetailBLL(appid);
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
                if (path.Contains("/Court_Notice_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].summons_detals = GetDetails.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    GetDetails.MVCAppDocDetails[0].petitioner_details = GetDetails.MVCAppDocDetails[i].Accident_details;
                }

            }
            return View(GetDetails);
        }


    }

}