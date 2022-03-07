using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BLL.GenderMasterBLL;
using BLL.UploadEmployeeBLL;
using Common;
using KGID_Models.KGID_Upload;
using Newtonsoft.Json;
using System.Text;
using ClosedXML.Excel;
using KGID_Models.NBApplication;
using KGID.Models;
using System.Web.Configuration;
using DLL.DBConnection;
using static KGID.FilterConfig;
using BLL.NewEmployeeBLL;

namespace KGID.Controllers
{
    [NoCache]
    public class UploadController : Controller
    {
        private readonly IUploadEmployeeBLL _uploadbll;
        private readonly IGenderMasterBLL _genderbll;
        private CommonMethod objCM = new CommonMethod();
        private KGID_Master objKM = new KGID_Master();
        private readonly INBApplicationBll _INBApplicationbll;
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        public UploadController()
        {
            this._INBApplicationbll = new NBApplicationBll();
            this._uploadbll = new UploadEmployeeBLL();
            this._genderbll = new GenderMasterBLL();
        }
        // GET: Upload
        [Route("kgid-ddo-upload")]
        [Route("Upload/UploadEmployeeDetails")]
        public ActionResult UploadEmployeeDetails()
        {
            if (Session["Categories"] != null)
            {
                int DDORoleID = Convert.ToInt16(UserCategories.DDO);
                string[] categories = Convert.ToString(Session["Categories"]).Split(',');
                VM_BasicDetails _BasicDetails = objKM.GetEmployeeWorkDetailsByEmployeeId(Convert.ToInt64(Session["UID"]));
                if (categories.Contains(DDORoleID.ToString()))
                {
                    ViewBag.DDOCode = _BasicDetails.dm_ddo_code;
                    ViewBag.DEPTCode = _BasicDetails.dm_dept_code;
                    ViewBag.POP = _BasicDetails.ewd_place_of_posting;
                    return View();
                }
            }
            return View("AccessDenied");
        }

        #region Common
        public DataTable ExecuteCmdExcel(string conString)
        {

            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        try
                        {
                            DataTable dt = new DataTable();
                            cmdExcel.Connection = connExcel;
                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();
                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "] where [SLNO] is not null";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                            return dt;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            connExcel.Close();
                        }
                    }
                }
            }
        }

        public Tuple<DataTable, string, UploadModel, string, DataTable> FileUpload(UploadModel model, string path, List<ExcelValidation> validationList, string ddocode)
        {
            DataTable dtError = new DataTable();
            DataTable dtExcelData = new DataTable();
            HttpPostedFileBase postedFile = model.FileUpload;
            string filePath = string.Empty;
            if (postedFile != null)
            {
                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if ((extension != ".xls") && (extension != ".xlsx"))
                {
                    return Tuple.Create(dtError, "Please upload Excel File.", model, filePath, dtExcelData);
                }
                else
                {
                    try
                    {
                        postedFile.SaveAs(filePath);
                        string conString = string.Empty;
                        switch (extension)
                        {
                            case ".xls": //Excel 97-03.
                                conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                                break;
                            case ".xlsx": //Excel 07 and above.
                                conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                                break;
                        }
                        conString = string.Format(conString, filePath);
                        dtExcelData = ExecuteCmdExcel(conString);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        int count = dtExcelData.Rows.Count;
                        int rowNum = 1;
                        if (count == 0)
                        {
                            return Tuple.Create(dtError, "This file does not contain any data", model, filePath, dtExcelData);
                        }
                        dtError = dtExcelData.Clone();
                        if (dtError.Columns.Count > 0)
                        {
                            dtError.Columns.Add("RowNum", typeof(string));
                            dtError.Columns.Add("Error Desc", typeof(string));
                        }
                        string validationColmn = string.Empty;
                        string validation = string.Empty;
                        string validationMsg = string.Empty;
                        bool Valid = true; bool mblnbrvalidation = false; bool pannbrvalidation = false; bool emailvalidation = false;
                        List<string> mblnbrs = new List<string>();
                        List<string> pannbrs = new List<string>();
                        List<string> emails = new List<string>();
                        var uniquemblcount = (from r in dtExcelData.AsEnumerable()
                                              select r["mobile_number"]).Distinct().Count();
                        if (uniquemblcount != dtExcelData.Rows.Count)
                            mblnbrvalidation = true;
                        var uniquepancount = (from r in dtExcelData.AsEnumerable()
                                              select r["pan_number"]).Distinct().Count();
                        if (uniquepancount != dtExcelData.Rows.Count)
                            pannbrvalidation = true;
                        var uniqueemailcount = (from r in dtExcelData.AsEnumerable()
                                                select r["email_id"]).Distinct().Count();
                        if (uniqueemailcount != dtExcelData.Rows.Count)
                            emailvalidation = true;

                        foreach (DataRow dr in dtExcelData.Rows)
                        {
                            rowNum = rowNum + 1;
                            string errorDesc = string.Empty;
                            foreach (ExcelValidation item in validationList)
                            {
                                validationColmn = item.column;
                                validation = item.validation;
                                validationMsg = item.errorMsg;
                                if (!dr.Table.Columns.Contains(validationColmn))
                                {
                                    dtError = new DataTable();
                                    return Tuple.Create(dtError, "Excel file columns do not match with the database columns, Please upload Valid Excel Sheet.", model, filePath, dtExcelData);
                                }
                                if (dr[validationColmn].ToString() == string.Empty || dr[validationColmn] == DBNull.Value)
                                {
                                    errorDesc += string.Join(",", item.column + " column cannot be null ", dtExcelData);
                                    Valid = false;
                                }
                                if (validationColmn != "date_of_birth" && validationColmn != "ewd_date_of_joining" && validationColmn != "date_of_appointment")
                                {
                                    if (!Regex.IsMatch(dr[validationColmn].ToString(), validation))
                                    {
                                        if (dr.IsNull(validationColmn))
                                        {
                                            errorDesc += string.Join(",", item.column + " column cannot be null  ", dtExcelData);
                                        }
                                        Valid = false;
                                    }
                                    if (validationColmn == "ewd_ddo")
                                    {
                                        if (ddocode != dr[validationColmn].ToString())
                                        {
                                            errorDesc += string.Join(",", "DDO code is mismatch. ");
                                        }
                                        Valid = false;
                                    }
                                }
                                else
                                {
                                    if (!Regex.IsMatch(Convert.ToDateTime(dr[validationColmn].ToString()).ToShortDateString(), validation))
                                    {
                                        if (dr.IsNull(validationColmn))
                                        {
                                            errorDesc += string.Join(",", item.column + " column cannot be null  ", dtExcelData);
                                        }
                                        Valid = false;
                                    }
                                    else
                                    {
                                        if (validationColmn == "date_of_birth")
                                        {
                                            int age = DateTime.Today.Year - Convert.ToDateTime(dr[validationColmn].ToString()).Year;
                                            if (age < 18)
                                            {
                                                errorDesc += string.Join(",", "Employee age should not be less than 18 years.");
                                            }
                                            Valid = false;
                                        }
                                        if (validationColmn == "ewd_date_of_joining")
                                        {
                                            int days = (Convert.ToDateTime(dr[validationColmn].ToString()).Date - Convert.ToDateTime(dr["date_of_appointment"].ToString()).Date).Days;
                                            if (days < 0)
                                            {
                                                errorDesc += string.Join(",", "Date of joining should not be prior to appointment date.");
                                            }
                                            Valid = false;
                                        }
                                    }
                                }
                                if (emailvalidation && validationColmn == "email_id")
                                {
                                    if (emails.Contains(dr[validationColmn].ToString()))
                                    {
                                        errorDesc += string.Join(",", "Duplication of email id in uploaded data.");
                                    }
                                    emails.Add(dr[validationColmn].ToString());
                                    Valid = false;
                                }
                                if (mblnbrvalidation && validationColmn == "mobile_number")
                                {
                                    if (mblnbrs.Contains(dr[validationColmn].ToString()))
                                    {
                                        errorDesc += string.Join(",", "Duplication of mobile number in uploaded data.");
                                    }
                                    mblnbrs.Add(dr[validationColmn].ToString());
                                    Valid = false;
                                }
                                if (pannbrvalidation && validationColmn == "pan_number")
                                {
                                    if (pannbrs.Contains(dr[validationColmn].ToString()))
                                    {
                                        errorDesc += string.Join(",", "Duplication of pan number in uploaded data.");
                                    }
                                    pannbrs.Add(dr[validationColmn].ToString());
                                    Valid = false;
                                }
                            }
                            if (!Valid)
                            {
                                errorDesc = errorDesc.TrimEnd(',');
                                if (!string.IsNullOrEmpty(errorDesc))
                                {
                                    dtError.Rows.Add(dr.ItemArray);
                                    dtError.Rows[dtError.Rows.Count - 1]["RowNum"] = rowNum;
                                    dtError.Rows[dtError.Rows.Count - 1]["Error Desc"] = errorDesc.ToString();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Tuple.Create(dtError, ex.Message, model, filePath, dtExcelData);
                    }
                }
            }
            return Tuple.Create(dtError, "Test2", model, filePath, dtExcelData);
        }

        public static void WriteUploadLog(DataTable errorlogtable, string path)
        {

            using (StreamWriter w = new StreamWriter(path))
            {
                w.WriteLine("Log Entry");
                w.WriteLine("--------------------------------------------------");
                if (errorlogtable.Columns.Contains("RowNum"))
                {
                    //headers  
                    w.Write("Row Number,Error Description,");
                    w.Write(w.NewLine);
                    //logs
                    foreach (DataRow dr in errorlogtable.Rows)
                    {
                        if (dr.ItemArray.Any(a => a != DBNull.Value))
                        {
                            w.Write(dr["RowNum"].ToString() + ',');
                            w.Write(dr["Error Desc"].ToString() + ',');
                        }
                        w.Write(w.NewLine);
                    }
                }
                else
                {
                    //headers  
                    w.Write("HRMS Employee Code,Error Description,");
                    w.Write(w.NewLine);
                    //logs
                    foreach (DataRow dr in errorlogtable.Rows)
                    {
                        if (dr.ItemArray.Any(a => a != DBNull.Value))
                        {
                            w.Write(dr["nebd_id"].ToString() + ',');
                            w.Write(dr["Error Desc"].ToString() + ',');
                        }
                        w.Write(w.NewLine);
                    }
                }
            }
        }


        #endregion

        #region UploadExcelData
        [HttpPost]
        public ActionResult UploadExcelData(UploadModel postedFile)
        {
            try
            {
                objCM.ErrorHandler("Enter", 332);
                string employeecode = string.Empty;
                if (Session["UID"] != null)
                {
                    employeecode = Session["UID"].ToString();
                    string ddocode = _uploadbll.GetLoggedDDOCode(Convert.ToInt32(employeecode));
                    string filePath = string.Empty; bool datatosql = false;
                    string logpath = Server.MapPath("~/Uploads/" + DateTime.Now.ToString("MM_dd_yyyy"));
                    string logfilename = "Logs.csv";
                    //return Json(new { excelUploaded = "/uploads/" + DateTime.Now.ToString("MM_dd_yyyy") + "/" + logfilename }, JsonRequestBehavior.AllowGet);
                    if (!Directory.Exists(logpath))
                    {
                        Directory.CreateDirectory(logpath);
                    }
                    if (postedFile != null)
                    {
                        string path = Server.MapPath("~/Uploads/");
                        List<ExcelValidation> validationList = new List<ExcelValidation>();
                        using (StreamReader r = new StreamReader(Server.MapPath("~/Uploads/ExcelUploadValidation.json")))
                        {
                            string json = r.ReadToEnd();
                            //validationList = result.Where(p => p.module == "Flat_Details").ToList();
                            validationList = JsonConvert.DeserializeObject<Validation>(json).ValidationData;
                        }
                        UploadModel cmodel = new UploadModel();
                        cmodel.FileUpload = postedFile.FileUpload;
                        cmodel.fileName = postedFile.FileUpload.FileName;
                        Tuple<DataTable, string, UploadModel, string, DataTable> tupleResult = FileUpload(cmodel, path, validationList, ddocode);
                        if (tupleResult.Item1.Rows.Count == 0 && tupleResult.Item1.Columns.Count > 0)
                        {
                            string conString = string.Empty;
                            DataTable dt = new DataTable();
                            dt = tupleResult.Item5;
                            conString = ConfigurationManager.ConnectionStrings["DbconnectionKGID"].ConnectionString;
                            using (SqlConnection con = new SqlConnection(conString))
                            {
                                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                {
                                    //Set the database table name.
                                    sqlBulkCopy.DestinationTableName = "dbo.tbl_employee_basic_details";

                                    ////[OPTIONAL]: Map the Excel columns with that of the database table

                                    //sqlBulkCopy.ColumnMappings.Add("hrms_employee_code", "hrms_employee_code");
                                    sqlBulkCopy.ColumnMappings.Add("dept_employee_code", "dept_employee_code");
                                    sqlBulkCopy.ColumnMappings.Add("employee_name", "employee_name");
                                    sqlBulkCopy.ColumnMappings.Add("father_name", "father_name");
                                    sqlBulkCopy.ColumnMappings.Add("gender_id", "gender_id");
                                    sqlBulkCopy.ColumnMappings.Add("date_of_birth", "date_of_birth");
                                    sqlBulkCopy.ColumnMappings.Add("place_of_birth", "place_of_birth");
                                    sqlBulkCopy.ColumnMappings.Add("pan_number", "pan_number");
                                    sqlBulkCopy.ColumnMappings.Add("mobile_number", "mobile_number");
                                    sqlBulkCopy.ColumnMappings.Add("email_id", "email_id");
                                    sqlBulkCopy.ColumnMappings.Add("date_of_appointment", "date_of_appointment");
                                    sqlBulkCopy.ColumnMappings.Add("active_status", "active_status");
                                    sqlBulkCopy.ColumnMappings.Add("created_by", "created_by");
                                    sqlBulkCopy.ColumnMappings.Add("updated_by", "updated_by");
                                    sqlBulkCopy.ColumnMappings.Add("creation_datetime", "creation_datetime");
                                    sqlBulkCopy.ColumnMappings.Add("updation_datetime", "updation_datetime");
                                    sqlBulkCopy.ColumnMappings.Add("spouse_name", "spouse_name");
                                    sqlBulkCopy.ColumnMappings.Add("ddo_upload_status", "ddo_upload_status");
                                    sqlBulkCopy.ColumnMappings.Add("user_category_id", "user_category_id");

                                    con.Open();
                                    DataTable finData = _uploadbll.DuplicateRemoveExcelData(dt, employeecode);

                                    if (finData.Columns.Contains("Error Desc"))
                                    {
                                        WriteUploadLog(finData, logpath + "\\" + logfilename);
                                        return Json(new { excelUploaded = "/uploads/" + DateTime.Now.ToString("MM_dd_yyyy") + "/" + logfilename }, JsonRequestBehavior.AllowGet);

                                    }
                                    else if (finData.Rows.Count > 0)
                                    {
                                        datatosql = true;
                                        sqlBulkCopy.WriteToServer(finData);
                                    }

                                    con.Close();
                                }
                            }
                            objCM.ErrorHandler("Mid", 410);
                            if (datatosql)
                            {
                                using (SqlConnection con = new SqlConnection(conString))
                                {
                                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                    {
                                        //Set the database table name.
                                        sqlBulkCopy.DestinationTableName = "tbl_employee_work_details";

                                        ////[OPTIONAL]: Map the Excel columns with that of the database table                          

                                        sqlBulkCopy.ColumnMappings.Add("ewd_emp_id", "ewd_emp_id");//New
                                        sqlBulkCopy.ColumnMappings.Add("ewd_date_of_joining", "ewd_date_of_joining");//New
                                        sqlBulkCopy.ColumnMappings.Add("ewd_payscale_id", "ewd_payscale_id");
                                        sqlBulkCopy.ColumnMappings.Add("ewd_employment_type", "ewd_employment_type");
                                        sqlBulkCopy.ColumnMappings.Add("ewd_designation_id", "ewd_designation_id");
                                        sqlBulkCopy.ColumnMappings.Add("ewd_group_id", "ewd_group_id");
                                        sqlBulkCopy.ColumnMappings.Add("ewd_place_of_posting", "ewd_place_of_posting");
                                        sqlBulkCopy.ColumnMappings.Add("ewd_ddo_id", "ewd_ddo_id");
                                        sqlBulkCopy.ColumnMappings.Add("ewd_created_by", "ewd_created_by");
                                        sqlBulkCopy.ColumnMappings.Add("ewd_updated_by", "ewd_updated_by");
                                        sqlBulkCopy.ColumnMappings.Add("ewd_creation_datetime", "ewd_creation_datetime");
                                        sqlBulkCopy.ColumnMappings.Add("ewd_updation_datetime", "ewd_updation_datetime");
                                        sqlBulkCopy.ColumnMappings.Add("ewd_active_status", "ewd_active_status");

                                        con.Open();
                                        DataTable finDataForWorkDetails = _uploadbll.DuplicateRemoveExcelDataForWorkDetails(dt, employeecode);
                                        if (finDataForWorkDetails.Rows.Count > 0)
                                        {
                                            datatosql = true;
                                            sqlBulkCopy.WriteToServer(finDataForWorkDetails);
                                        }

                                        con.Close();
                                    }
                                }
                            }
                            if (datatosql)
                            {
                                using (SqlConnection con = new SqlConnection(conString))
                                {
                                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                    {
                                        //Set the database table name.
                                        sqlBulkCopy.DestinationTableName = "tbl_hrms_pay_details_master";

                                        ////[OPTIONAL]: Map the Excel columns with that of the database table                          

                                        sqlBulkCopy.ColumnMappings.Add("hrms_emp_id", "hrms_emp_id");
                                        //sqlBulkCopy.ColumnMappings.Add("hrms_emp_code", "hrms_emp_code");
                                        sqlBulkCopy.ColumnMappings.Add("hrms_month_id", "hrms_month_id");
                                        sqlBulkCopy.ColumnMappings.Add("hrms_year_id", "hrms_year_id");
                                        sqlBulkCopy.ColumnMappings.Add("hrms_gross_pay", "hrms_gross_pay");
                                        //sqlBulkCopy.ColumnMappings.Add("hrms_deduction", "hrms_deduction");
                                        //sqlBulkCopy.ColumnMappings.Add("hrms_net_pay", "hrms_net_pay");
                                        sqlBulkCopy.ColumnMappings.Add("hrms_active", "hrms_active");
                                        sqlBulkCopy.ColumnMappings.Add("hrms_creation_datetime", "hrms_creation_datetime");
                                        sqlBulkCopy.ColumnMappings.Add("hrms_created_by", "hrms_created_by");
                                        sqlBulkCopy.ColumnMappings.Add("hrms_updation_datetime", "hrms_updation_datetime");
                                        sqlBulkCopy.ColumnMappings.Add("hrms_updated_by", "hrms_updated_by");

                                        con.Open();
                                        DataTable finDataForPayDetailsMaster = _uploadbll.InsertPayDetailsMasterDetails(dt, employeecode);
                                        if (finDataForPayDetailsMaster.Rows.Count > 0)
                                        {
                                            datatosql = true;
                                            sqlBulkCopy.WriteToServer(finDataForPayDetailsMaster);
                                        }

                                        con.Close();
                                    }
                                }
                            }
                        }
                        else if (tupleResult.Item1.Rows.Count == 0 && tupleResult.Item1.Columns.Count == 0)
                        {
                            //return RedirectToAction("UploadEmployeeDetails", "Upload", new { excelUploaded = tupleResult.Item2 });
                            return Json(new { excelUploaded = tupleResult.Item2 }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            WriteUploadLog(tupleResult.Item1, logpath + "\\" + logfilename);
                            return Json(new { excelUploaded = "/uploads/" + DateTime.Now.ToString("MM_dd_yyyy") + "/" + logfilename }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (datatosql)
                        return Json(new { excelUploaded = "Excel data uploaded successfully" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { excelUploaded = "No data to upload" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { excelUploaded = "Session expired. Please login to upload data." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objCM.ErrorHandler(ex.Message, 503);
                return Json(new { excelUploaded = "Error occured while uploading data." }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Get EmployeeDetails
        [HttpGet]
        public ActionResult GetEmployeeData()
        {
            long EmpID = 0;
            if (Session["UID"] != null)
            {
                EmpID = Convert.ToInt64(Session["UID"]);
            }
            List<VM_BasicDetails> _empData = _uploadbll.GetEmployeeBasicData(EmpID);
            var result = Json(_empData,
                JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;

            //return Json(_empData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete employee
        public int DeleteUploadEmployeeDetails(string empId)
        {
            int result = _uploadbll.DeleteUploadEmployeeDetails(Convert.ToInt64(empId));
            Session["dtDuplicate"] = null;
            Session["dtError"] = null;
            return result;
        }
        #endregion

        #region New Upload Excel Details
        public ActionResult Index()
        {
            ViewBag.message = TempData["Message"];
            //ViewBag.error = TempData["Success"];
            return View();
        }
        [HttpPost]
        public ActionResult UploadExcelDataNew(HttpPostedFileBase postedFile)
        {
            Logger.LogMessage(TracingLevel.INFO, "Coming 1");
            Session["dtDuplicate"] = null;
            Session["dtError"] = null;
            //objError.ErrorHandler("Inside UploadData/UploadExcelData", 387);
            var FinalResult = "";
            try
            {
                var UType = Request.Form["ddlExcelType"];
                //if (model.budgetType.Equals("1") || model.budgetType.Equals("4"))
                //{
                List<ExcelValidation> validationList = new List<ExcelValidation>();
                string path = string.Empty;
                // string uploadType = string.Empty;
                using (StreamReader r = new StreamReader(Server.MapPath("~/Uploads/ExcelValidationNew.json")))
                {
                    string json = r.ReadToEnd();
                    validationList = JsonConvert.DeserializeObject<Validation>(json).ValidationData;
                    path = Server.MapPath("~/Uploads/");
                }
                DataTable dtCoulmn = new DataTable();
                dtCoulmn.Columns.Add("ColumnName", typeof(string));
                dtCoulmn.Columns.Add("Type", typeof(string));
                dtCoulmn.Columns.Add("Value", typeof(string));
                DataRow row = dtCoulmn.NewRow();

                string TableName = string.Empty;

                TableName = WebConfigurationManager.AppSettings["tbl_ddo_upload_staging"];



                DataSet dsStagingColumns = GetStagingColumn(TableName);

                UploadModel cmodel = new UploadModel();
                cmodel.FileUpload = postedFile;
                cmodel.fileName = postedFile.FileName;
                string ddocode = string.Empty;
                if (Session["UID"] != null)
                {
                    string employeecode = Session["UID"].ToString();
                    ddocode = _uploadbll.GetLoggedDDOCode(Convert.ToInt32(employeecode));
                }
                Tuple<DataTable, string, UploadModel, string> tupleResult = FileUpload(cmodel, path, dtCoulmn, TableName, dsStagingColumns, validationList, Session["UID"].ToString(), ddocode);
                if (tupleResult.Item1.Rows.Count > 0)
                {
                    Session["dtError"] = tupleResult.Item1;
                }
                if (!string.IsNullOrEmpty(tupleResult.Item2))
                {
                    if (tupleResult.Item2.Equals("Success"))
                    {

                        ViewBag.message = "Succesfully Uploaded";
                        TempData["Message"] = "Succesfully Uploaded";
                        InsertEmployeeData();
                    }
                    else
                    {
                        TempData["Message"] = tupleResult.Item2;
                    }
                    Response.Write("Data uploaded successfully");
                }
                if (tupleResult.Item3.error.Equals(1))
                {
                    FinalResult = tupleResult.Item3.error.ToString();
                }
                //Response.Redirect("UploadData/Index");
                return RedirectToAction("UploadEmployeeDetails", "Upload");
            }
            catch (Exception ex)
            {
                string ELog = ex.Message;
                Logger.LogMessage(TracingLevel.INFO, ELog);
                //objError.ErrorHandler("Catch - " + ex.Message, 489);
                return RedirectToAction("UploadEmployeeDetails", "Upload");
            }
        }

        public Tuple<DataTable, string, UploadModel, string> FileUpload(UploadModel model, string path, DataTable dtColumn, string tableName, DataSet dsStagingColumns, List<ExcelValidation> validationList, string userId,string ddocode)
        {
            Logger.LogMessage(TracingLevel.INFO, "Coming");
            DataTable dtError = new DataTable();
            HttpPostedFileBase postedFile = model.FileUpload;
            string filePath = string.Empty;
            if (postedFile != null)
            {
                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if ((extension != ".xls") && (extension != ".xlsx"))
                {
                    return Tuple.Create(dtError, "Please Upload Valid Excel Files.", model, filePath);
                }
                else
                {
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                                     // Data Source = CSGSQLSERVER\SQLEXPRESS; Initial Catalog = bpgbs_db; User ID = bpgbs; Password = Royal#123" providerName="System.Data.SqlClient" />
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataTable dtExcelData = new DataTable();
                    conString = string.Format(conString, filePath);
                    dtExcelData = ExecuteCmdExcel(conString);
                    dtError = dtExcelData.Clone();

                    for (int i = dtError.Columns.Count - 1; i >= dsStagingColumns.Tables[0].Rows.Count - 5; i--)
                        dtError.Columns.RemoveAt(i);

                    DataColumnCollection ERRcolumns = dtError.Columns;
                    if (!ERRcolumns.Contains("DDO Code"))
                    {
                        dtError.Columns.Add("DDO Code", typeof(string));
                        dtError.Columns.Add("Department Code", typeof(string));
                        dtError.Columns.Add("Place of Posting", typeof(string));
                    }
                    dtError.Columns.Add("RowNum", typeof(string));
                    dtError.Columns.Add("Error Desc", typeof(string));

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    int count = dtExcelData.Rows.Count;
                    int rowNum = 0;
                    if (count == 0)
                    {
                        return Tuple.Create(dtError, "This file do not contain any data", model, filePath);
                    }
                    else
                    {
                        DataTable dtNew = new DataTable();
                        dtNew = dtExcelData.Copy();
                        for (int i = dtNew.Columns.Count - 1; i >= dsStagingColumns.Tables[0].Rows.Count - 5; i--)
                            dtNew.Columns.RemoveAt(i);

                        VM_BasicDetails _BasicDetails = objKM.GetEmployeeWorkDetailsByEmployeeId(Convert.ToInt64(Session["UID"]));
                        DataColumnCollection columns = dtNew.Columns;
                        if (!columns.Contains("DDO Code"))
                        {
                            dtNew.Columns.Add("DDO Code", typeof(string));
                            dtNew.Columns.Add("Department Code", typeof(string));
                            dtNew.Columns.Add("Place of Posting", typeof(string));
                        }

                        foreach (DataRow row in dtNew.Rows)
                        {
                            row["DDO Code"] = _BasicDetails.dm_ddo_code;
                            row["Department Code"] = _BasicDetails.dm_dept_code;
                            row["Place of Posting"] = _BasicDetails.ewd_place_of_posting;
                        }

                        //Adding new columns to excel data
                        for (int cnt = 0; cnt < dtColumn.Rows.Count; cnt++)
                        {
                            if (dtColumn.Rows[cnt][1].ToString().Equals("bool"))
                            {
                                System.Data.DataColumn boolColumn = new System.Data.DataColumn(dtColumn.Rows[cnt][0].ToString(), typeof(bool));
                                boolColumn.DefaultValue = dtColumn.Rows[cnt][2].ToString();
                                dtNew.Columns.Add(boolColumn);
                            }
                            if (dtColumn.Rows[cnt][1].ToString().Equals("DateTime"))
                            {
                                System.Data.DataColumn datetimeColumn = new System.Data.DataColumn(dtColumn.Rows[cnt][0].ToString(), typeof(DateTime));
                                datetimeColumn.DefaultValue = dtColumn.Rows[cnt][2].ToString();
                                dtNew.Columns.Add(datetimeColumn);
                            }
                            if (dtColumn.Rows[cnt][1].ToString().Equals("int"))
                            {
                                System.Data.DataColumn intColumn = new System.Data.DataColumn(dtColumn.Rows[cnt][0].ToString(), typeof(int));
                                intColumn.DefaultValue = dtColumn.Rows[cnt][2].ToString();
                                dtNew.Columns.Add(intColumn);
                            }
                            if (dtColumn.Rows[cnt][1].ToString().Equals("string"))
                            {
                                System.Data.DataColumn stringColumn = new System.Data.DataColumn(dtColumn.Rows[cnt][0].ToString(), typeof(string));
                                stringColumn.DefaultValue = dtColumn.Rows[cnt][2].ToString();
                                dtNew.Columns.Add(stringColumn);
                            }
                        }
                        string validationColmn = string.Empty;
                        //string validationModule = string.Empty;
                        string validation = string.Empty;
                        string validationMsg = string.Empty;
                        bool Valid = true;bool mblnbrvalidation = false; bool pannbrvalidation = false; bool emailvalidation = false;
                        List<string> mblnbrs = new List<string>();
                        List<string> pannbrs = new List<string>();
                        List<string> emails = new List<string>();
                        var uniquemblcount = (from r in dtExcelData.AsEnumerable()
                                              select r["Mobile Number"]).Distinct().Count();
                        if (uniquemblcount != dtExcelData.Rows.Count)
                            mblnbrvalidation = true;
                        var uniquepancount = (from r in dtExcelData.AsEnumerable()
                                              select r["PAN Number"]).Distinct().Count();
                        if (uniquepancount != dtExcelData.Rows.Count)
                            pannbrvalidation = true;
                        var uniqueemailcount = (from r in dtExcelData.AsEnumerable()
                                                select r["Email Address"]).Distinct().Count();
                        if (uniqueemailcount != dtExcelData.Rows.Count)
                            emailvalidation = true;

                        foreach (DataRow dr in dtNew.Rows)
                        {
                            rowNum = rowNum + 1;
                            string errorDesc = string.Empty;
                            // var alphabetsPattern = @"^[a-zA-Z]+$";
                            foreach (ExcelValidation item in validationList)
                            {
                                validationColmn = item.column;
                                //validationModule = item.module;
                                validation = item.validation;
                                validationMsg = item.errorMsg;
                                if (!dr.Table.Columns.Contains(validationColmn))
                                {
                                    //errorDesc += item.column + " column cannot be null";
                                    //Valid = false;
                                    return Tuple.Create(dtError, "Excel file columns do not match with the database columns, Please upload Valid Excel Sheet.", model, filePath);
                                }
                                //if (dr.IsNull("SITE_NUMBER"))
                                //{
                                //    //dr.Delete();
                                //    // errorDesc += validationMsg + ",";
                                //    errorDesc += item.column + " column cannot be null";
                                //    Valid = false;
                                //}

                                Regex rgx = new Regex(validation);
                                if (!rgx.IsMatch(dr[validationColmn].ToString()))
                                {
                                    if (dr.IsNull(validationColmn))
                                    {
                                        errorDesc += item.column + " column cannot be null,  ";
                                    }
                                    else
                                    {
                                        errorDesc += validationMsg + ",";
                                    }
                                    Valid = false;
                                }
                                else
                                {
                                    if (validationColmn == "DDO Code")
                                    {
                                        if (ddocode != dr[validationColmn].ToString())
                                        {
                                            errorDesc += string.Join(",", "DDO code is mismatch. ");
                                            Valid = false;
                                        }
                                    }
                                    else
                                    {
                                        if (validationColmn == "Pay Scale")
                                        {
                                            string[] amount = dr[validationColmn].ToString().Split('-');
                                            decimal minvalue = Convert.ToDecimal(amount[0]);
                                            decimal maxvalue = Convert.ToDecimal(amount[1]);
                                            var id = _db.tbl_payscales_master.Where(a => a.payscale_maximum == maxvalue && a.payscale_minimum == minvalue).Select(a => a.payscale_id).FirstOrDefault();
                                            if (id == 0)
                                            {
                                                errorDesc += string.Join(",", "Pay Scale is mismatch. ");
                                                Valid = false;
                                            }
                                        }
                                        if (validationColmn == "Employment Type")
                                        {
                                            var Employemnettype = "Permanent";
                                            if (Employemnettype != dr[validationColmn].ToString())
                                            {
                                                errorDesc += string.Join(",", "Employment Type is mismatch. ");
                                                Valid = false;
                                            }
                                        }
                                        if (validationColmn == "Designation")
                                        {
                                            var Design = dr[validationColmn].ToString().ToUpper();
                                            var deptid = _db.tbl_department_master.Where(a => a.dm_deptcode ==  _BasicDetails.dm_dept_code).FirstOrDefault();
                                            var Designation = _db.tbl_designation_master.Where(a => a.d_designation_desc == Design && a.d_dept_id == deptid.dm_dept_id).Select(a => a.d_designation_id).FirstOrDefault();
                                            if (Designation == 0)
                                            {
                                                errorDesc += string.Join(",", "Designation is mismatch. ");
                                                Valid = false;
                                            }
                                        }
                                        if (validationColmn == "Group")
                                        {
                                            var grp = dr[validationColmn].ToString().ToUpper();
                                            var Group = _db.tbl_employee_group_master.Where(a => a.eg_group_desc == grp).Select(a => a.eg_group_id).FirstOrDefault();
                                            if (Group == 0)
                                            {
                                                errorDesc += string.Join(",", "Group is mismatch. ");
                                                Valid = false;
                                            }
                                        }
                                        //if (validationColmn == "Place of Posting")
                                        //{
                                        //    var plce = dr[validationColmn].ToString().ToUpper();
                                        //    var Place = _db.tbl_ddo_master.Where(a => a.dm_ddo_code == ddocode).Select(a => a.dm_ddo_office).FirstOrDefault();
                                        //    if (Place != plce)
                                        //    {
                                        //        errorDesc += string.Join(",", "Place of Posting is mismatch. ");
                                        //        Valid = false;
                                        //    }
                                        //}
                                        //if (validationColmn == "Date of Birth")
                                        //{
                                        //    int age = DateTime.Today.Year - Convert.ToDateTime(dr[validationColmn].ToString()).Year;
                                        //    if(DateTime.Today.Month < Convert.ToDateTime(dr[validationColmn].ToString()).Month)
                                        //    {
                                        //        age = age - 1;
                                        //    }
                                        //    else if((DateTime.Today.Month == Convert.ToDateTime(dr[validationColmn].ToString()).Month) && (DateTime.Today.Day < Convert.ToDateTime(dr[validationColmn].ToString()).Day))
                                        //    {
                                        //        age = age - 1;
                                        //    }
                                        //    if (age < 18)
                                        //    {
                                        //        errorDesc += string.Join(",", "Employee Age cannot be less than 18.");
                                        //    }
                                        //    Valid = false;
                                        //}
                                        //if(validationColmn == "Date of Appointment")
                                        //{
                                        //    int age = Convert.ToDateTime(dr["Date of Appointment"].ToString()).Year - Convert.ToDateTime(dr["Date of Birth"].ToString()).Year;
                                        //    if (Convert.ToDateTime(dr["Date of Appointment"].ToString()).Month < Convert.ToDateTime(dr["Date of Appointment"].ToString()).Month)
                                        //    {
                                        //        age = age - 1;
                                        //    }
                                        //    else if ((Convert.ToDateTime(dr["Date of Appointment"].ToString()).Month == Convert.ToDateTime(dr["Date of Birth"].ToString()).Month) && (Convert.ToDateTime(dr["Date of Appointment"].ToString()).Day < Convert.ToDateTime(dr["Date of Birth"].ToString()).Day))
                                        //    {
                                        //        age = age - 1;
                                        //    }
                                        //    if (age < 18)
                                        //    {
                                        //        errorDesc += string.Join(",", "Appointment date cannot be given if age is less than 18.");
                                        //    }
                                        //    Valid = false;
                                        //}
                                        //if (validationColmn == "Date of Joining")
                                        //{
                                        //    int days = (Convert.ToDateTime(dr[validationColmn].ToString()).Date - Convert.ToDateTime(dr["Date of Appointment"].ToString()).Date).Days;
                                        //    if (days < 0)
                                        //    {
                                        //        errorDesc += string.Join(",", "Date of joining should not be prior to appointment date.");
                                        //    }
                                        //    int age = Convert.ToDateTime(dr["Date of Joining"].ToString()).Year - Convert.ToDateTime(dr["Date of Birth"].ToString()).Year;
                                        //    if (Convert.ToDateTime(dr["Date of Joining"].ToString()).Month < Convert.ToDateTime(dr["Date of Birth"].ToString()).Month)
                                        //    {
                                        //        age = age - 1;
                                        //    }
                                        //    else if ((Convert.ToDateTime(dr["Date of Joining"].ToString()).Month == Convert.ToDateTime(dr["Date of Birth"].ToString()).Month) && (Convert.ToDateTime(dr["Date of Joining"].ToString()).Day < Convert.ToDateTime(dr["Date of Birth"].ToString()).Day))
                                        //    {
                                        //        age = age - 1;
                                        //    }
                                        //    if (age < 18)
                                        //    {
                                        //        errorDesc += string.Join(",", "Joining date cannot be given if age is less than 18.");
                                        //    }
                                        //    Valid = false;
                                        //}
                                        //if (validationColmn == "Department Code")
                                        //{
                                        //    var dcode = dr[validationColmn].ToString().ToUpper();
                                        //    var deptcode = _db.tbl_ddo_master.Where(a => a.dm_ddo_code == ddocode).Select(a => a.dm_dept_code).FirstOrDefault();
                                        //    if (dcode != deptcode)
                                        //    {
                                        //        errorDesc += string.Join(",", "Department Code is mismatch. ");
                                        //        Valid = false;
                                        //    }
                                        //}
                                        if (emailvalidation && validationColmn == "Email Address")
                                        {
                                            if (emails.Contains(dr[validationColmn].ToString()))
                                            {
                                                errorDesc += string.Join(",", "Duplication of email address in uploaded data.");
                                            }
                                            emails.Add(dr[validationColmn].ToString());
                                            Valid = false;
                                        }
                                        if (mblnbrvalidation && validationColmn == "Mobile Number")
                                        {
                                            if (mblnbrs.Contains(dr[validationColmn].ToString()))
                                            {
                                                errorDesc += string.Join(",", "Duplication of mobile number in uploaded data.");
                                            }
                                            mblnbrs.Add(dr[validationColmn].ToString());
                                            Valid = false;
                                        }
                                        if (pannbrvalidation && validationColmn == "PAN Number")
                                        {
                                            if (pannbrs.Contains(dr[validationColmn].ToString()))
                                            {
                                                errorDesc += string.Join(",", "Duplication of pan number in uploaded data.");
                                            }
                                            pannbrs.Add(dr[validationColmn].ToString());
                                            Valid = false;
                                        }
                                    }
                                }
                            }

                            if (!Valid)
                            {
                                errorDesc = errorDesc.TrimEnd(',');
                                if (!string.IsNullOrEmpty(errorDesc))
                                {
                                    dtError.Rows.Add(dr.ItemArray);
                                    dtError.Rows[dtError.Rows.Count - 1]["RowNum"] = (rowNum) + 1;
                                    dtError.Rows[dtError.Rows.Count - 1]["Error Desc"] = errorDesc.ToString();
                                }
                            }
                        }

                        if (dtError.Rows.Count == 0 || (dtError.Rows.Count > 0 && dtError.Rows.Count != dtExcelData.Rows.Count))
                        {
                            conString = ConfigurationManager.ConnectionStrings["DbconnectionKGID"].ConnectionString;
                            using (SqlConnection con = new SqlConnection(conString))
                            {
                                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.FireTriggers, null))
                                {
                                    Dictionary<string, string> vals = new Dictionary<string, string>();
                                    //if (tableName.Equals("tbl_linkhoa_staging"))
                                    //{
                                    //    string deleteTable = "delete from tbl_linkhoa_staging";
                                    //    DBHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["conn"].ConnectionString, deleteTable);
                                    //    // tableName = WebConfigurationManager.AppSettings["LinkHOAStagingTable"];
                                    //}
                                    //for (int i = dtNew.Columns.Count-1; i >= dsStagingColumns.Tables[0].Rows.Count - 1; i--)
                                    //    dtNew.Columns.RemoveAt(i);
                                    if (dtNew.Columns.Count != dsStagingColumns.Tables[0].Rows.Count - 2)
                                    {
                                        return Tuple.Create(dtError, "Excel file columns do not match with the database columns", model, filePath);
                                    }
                                    for (int i = 0; i < dtNew.Columns.Count; i++)
                                    {
                                        if (dtNew.Columns[i].ColumnName == dsStagingColumns.Tables[0].Rows[i]["COLUMN_NAME"].ToString())
                                        {
                                            vals.Add(dtNew.Columns[i].ColumnName, dsStagingColumns.Tables[0].Rows[i]["COLUMN_NAME"].ToString());
                                        }
                                        else
                                        {
                                            return Tuple.Create(dtError, "Mismatch of column - " + dtNew.Columns[i].ColumnName, model, filePath);
                                        }
                                    }
                                    sqlBulkCopy.DestinationTableName = tableName;
                                    for (int i = 0; i < vals.Count; i++)
                                    {
                                        sqlBulkCopy.ColumnMappings.Add(vals.Keys.ElementAt(i), vals.Values.ElementAt(i));
                                    }
                                    if(dtError.Rows.Count != dtNew.Rows.Count)
                                    {
                                        foreach(DataRow dr in dtError.Rows)
                                        {
                                            //var rows = dtNew.Rows.Cast<DataRow>().Where(r => r["SLNO"].ToString() == dr["SLNO"].ToString()).ToArray();
                                            DataRow[] drr = dtNew.Select("SLNO=' " + dr["SLNO"].ToString() + " ' ");
                                            for (int i = 0; i < drr.Length; i++)
                                                drr[i].Delete();
                                            dtNew.AcceptChanges();
                                        }
                                    }
                                    Logger.LogMessage(TracingLevel.INFO, "File upload method Table copied");
                                    con.Open();
                                    sqlBulkCopy.WriteToServer(dtNew);
                                    con.Close();

                                    foreach(DataRow dr in dtNew.Rows)
                                    {
                                        if(dr["Mobile Number"].ToString()!="")
                                        {

                                            var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587991448971);

                                            Logger.LogMessage(TracingLevel.INFO, "AddEmployeeBasicDetails 1107161587991448971" + msg.ToString());
                                            Logger.LogMessage(TracingLevel.INFO, "AddEmployeeBasicDetails mobile no " + dr["Mobile Number"].ToString());
                                           
                                            //string msg = "ನಿಮ್ಮ ಮಾಹಿತಿಯು ಕರ್ನಾಟಕ ಸರ್ಕಾರಿ ವಿಮಾ ಇಲಾಖೆ (KGID) ಯಲ್ಲಿ ನೋಂದಣಿಯಾಗಿದ್ದು, ನಿಮ್ಮ ಮೊಬೈಲ್ ಸಂಖ್ಯೆ/ಇ-ಮೇಲ್ ಉಪಯೋಗಿಸಿ https://kgidonline.karnataka.gov.in ನಲ್ಲಿ ಲಾಗಿನ್‌ ಮಾಡಿ ಹೊಸ ವಿಮಾ ಪ್ರಸ್ತಾವನೆಯನ್ನು ಸಲ್ಲಿಸಲು ಸೂಚಿಸಿದೆ. ಹೆಚ್ಚಿನ ಮಾಹಿತಿಗಾಗಿ ಸಹಾಯವಾಣಿ ಸಂಖ್ಯೆ 080-22536189   ನ್ನು ಸಂಪರ್ಕಿಸಿ " +
                                            //"- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                                            AllCommon.sendUnicodeSMS(dr["Mobile Number"].ToString(), msg, "1107161587991448971");
                                            Logger.LogMessage(TracingLevel.INFO, "SMS Sent " );
                                        }
                                    }
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();

                                    return Tuple.Create(dtError, "Success", model, filePath);
                                }
                            }
                        }
                        else
                        {
                            Session["dtError"] = dtError;
                            DownloadXLS();
                        }
                    }
                }
            }
            else
            {
                return Tuple.Create(dtError, "Please Upload document", model, filePath);
            }
            return Tuple.Create(dtError, string.Empty, model, filePath);
        }
        public FileResult DownloadXLS()
        {
            DataTable dt = (DataTable)Session["dtError"];
            //Session["dtError"] = null;
            if (dt.Rows.Count > 0)
            {
                string FileName = "EmployeeUploadErrorList";
                dt.TableName = "Excel";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + FileName + ".xlsx");
                    }
                }
            }
            else
            {
                return null;
            }
        }
        public FileResult DownloadDuplicateXLS()
        {
            DataTable dt = (DataTable)Session["dtDuplicate"];
            //Session["dtDuplicate"] = null;
            if (dt.Rows.Count > 0)
            {
                string filename = "Duplicate Data " + DateTime.Now.ToString().Replace(":", "");
                dt.TableName = "Excel";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + filename + ".xlsx");
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public void InsertEmployeeData()
        {
            long EmpID = 0;
            Session["dtDuplicate"] = null;
            if (Session["UID"] != null)
            {
                EmpID = Convert.ToInt64(Session["UID"]);
                Logger.LogMessage(TracingLevel.INFO, "InsertEmployeeData");
                DataSet dsDuplicate = new DataSet();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbconnectionKGID"].ConnectionString))
                {
                    using (SqlCommand myCMD = new SqlCommand("sp_kgid_ValidateStagingData", con))
                    {
                        myCMD.CommandType = CommandType.StoredProcedure;
                        SqlParameter sqlparam = new SqlParameter("@Empid", EmpID);
                        myCMD.Parameters.Add(sqlparam);
                        con.Open();
                        myCMD.CommandTimeout = 100;
                        myCMD.ExecuteNonQuery();
                        SqlDataAdapter da = new SqlDataAdapter(myCMD);
                        da.Fill(dsDuplicate);

                    if (dsDuplicate.Tables[0].Rows.Count > 0)
                    {
                        Session["dtDuplicate"] = dsDuplicate.Tables[0];
                    }

                    string DELETE_WRONG_DATA = "DELETE FROM tbl_ddo_upload_staging WHERE ISVALID in (2,3,4,9,7,5,6) or STG_Duplicate in (2,3,4,9,7,5,6)";
                    objCM.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["DbconnectionKGID"].ToString(), DELETE_WRONG_DATA);

                    con.Close();
                }
                //DataTable dt = dsDuplicate.Tables[0];// Session["dtDuplicate"];
                //if (dsDuplicate.Tables[0].Rows.Count > 0)
                //{
                //    DownloadXLS(dsDuplicate.Tables[0]);
                //}
            }
            }
            //int res = DownloadDuplicateXLS(dsDuplicate);
            //if (res == 1)
            //{
            //    string DELETE_WRONG_DATA = "DELETE FROM SITE_STG WHERE VALID_INVALID IN (5,0)";
            //    ExecuteNonQuery(ConfigurationManager.ConnectionStrings["DbconnectionKGID"].ToString(), DELETE_WRONG_DATA);
            //}
        }
        #endregion
        #region Common data
        public DataSet GetStagingColumn(string stagingTableName)
        {
            string strColumns = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + stagingTableName + "' ORDER BY ORDINAL_POSITION";
            DataSet ds = objCM.ExecuteDataset(ConfigurationManager.ConnectionStrings["DbconnectionKGID"].ConnectionString, strColumns);
            return ds;
        }

        #endregion
    }
}