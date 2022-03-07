using Common;
using DLL.DBConnection;
using KGID_Models.Ticketing_Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DLL
{
   public class TicketingToolDLL:ITicketingToolDLL
    {
        private readonly Common_Connection _Conn = new Common_Connection();
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly AllCommon _commnobj = new AllCommon();

        public IEnumerable<tbl_module_type_master> GetModuleListDLL()
        {
            List<tbl_module_type_master> types = new List<tbl_module_type_master>();

            types = (from t in _db.tbl_module_type_master
                     where t.mt_status == true
                     select (new tbl_module_type_master { mt_desc = t.mt_desc, mt_module_type = t.mt_module_type })).ToList<tbl_module_type_master>();

            return types;
        }
        public IEnumerable<tbl_problem_type_master> GetProblemTypeListDLL()
        {
            List<tbl_problem_type_master> types = new List<tbl_problem_type_master>();

            types = (from t in _db.tbl_problem_type_master

                     select (new tbl_problem_type_master { pr_description = t.pr_description, pt_id = t.pt_id })).ToList<tbl_problem_type_master>();

            return types;
        }
        public class DetailView
        {
            public string SiteName { get; set; }
            public string ItemType { get; set; }
            public string AssetStorage { get; set; }


        }
        public DataSet CreateDataTable1()
        {
            long EmpID = 0;
            TTReportProblem ApplicationDetails = new TTReportProblem();
                DataSet dsMBList = new DataSet();
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@rp_empid",EmpID),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetExcelDownloadData");
            }
            catch (Exception ex)
            {

            }
            return dsMBList;
        }
            public DataTable CreateDataTable(string className)
        {
            DataTable dt = new System.Data.DataTable();
            Type classtype = GetType();
            if (className == "TicketingToolReportProblem")
            {
                classtype = typeof(TicketingToolReportProblem);
            }
            
            PropertyInfo[] properties = classtype.GetProperties();
           // TTReportProblem tTReport1 = new TTReportProblem();

           // var properties1 = tTReport1.TicketingToolReportProblemlist.GetType();


            foreach (System.Reflection.PropertyInfo pi in properties)
            {
                //if (className == "VM_FamilyDetail" && pi.Name != "EditDeleteStatus" && pi.Name != "AppliactionSentBack" && pi.Name != "ApplicationInsured")
                //    dt.Columns.Add(pi.Name);
                //else if (className != "VM_FamilyDetail")
                    dt.Columns.Add(pi.Name);
            }
            //DataRow dr=null ;
            //dt.Rows.Add(dr);
            //DetailView fieldsInst = new DetailView();
            //// Get the type of DetailView.
            
            //Type fieldsType = typeof(tTReport1.TicketingToolReportProblemlist);

            //PropertyInfo[] props = fieldsType.GetProperties(BindingFlags.Public
            //    | BindingFlags.Instance);


            //for (int i = 0; i < props.Length; i++)
            //{
            //    Console.WriteLine("   {0}",
            //        props[i].Name);
            //}
            
            return dt;
        }
        public TTReportProblem GetAllReportedProblemsDLL()
        {
            long EmpID = 0;
            TTReportProblem ApplicationDetails = new TTReportProblem();
            try
            {
                DataSet dsMBList = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@rp_empid",EmpID),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetAllReportedProblems");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new TicketingToolReportProblem
                    {
                        // RowNumber = dataRow.Field<long>("RowNumber"),
                        rp_id = dataRow.Field<long>("rp_id"),
                        rp_ticket_no = dataRow.Field<string>("rp_ticket_no"),
                        //rp_empid = dataRow.Field<long>("rp_empid"),
                        mt_desc = dataRow.Field<string>("mt_desc"),
                        pr_description = dataRow.Field<string>("pr_description"),
                        //rp_complaint_description = dataRow.Field<string>("rp_complaint_description"),
                        //rp_upload_document = dataRow.Field<string>("rp_upload_document"),
                        rp_report_problem_status = dataRow.Field<string>("rp_report_problem_status"),
                        //rp_date_of_submission = dataRow.Field<DateTime>("rp_date_of_submission"),
                        //rp_date_of_resolve = dataRow["rp_date_of_resolve"].ToString()!=""? DateTime.Parse(dataRow["rp_date_of_resolve"].ToString()).ToString("dd/MM/yyyy"):"",
                        rp_remarks = dataRow.Field<string>("rp_remarks"),
                        rp_status = dataRow.Field<bool>("rp_status"),
                        SubmissionDate = dataRow.Field<string>("SubmissionDate"),
                        ResolveDate = dataRow.Field<string>("ResolveDate"),
                        //rp_creation_datetime = dataRow.Field<DateTime>("rp_creation_datetime"),
                        //rp_updation_datetime = dataRow.Field<DateTime>("rp_updation_datetime"),
                        //rp_created_by = dataRow.Field<long>("rp_created_by"),
                        //rp_updated_by = dataRow.Field<long>("rp_updated_by")
                        AssignedTo =dataRow.Field<string>("al_agency_user_id")
                    }).ToList();
                    ApplicationDetails.TicketingToolReportProblemlist = MBList;
                    DataTable st = new DataTable();
                    CreateDataTable("TicketingToolReportProblem");
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }

        public TTReportProblem GetDetailsByEmpIdDll(long EmpID,string emptype)
        {
            TTReportProblem ApplicationDetails = new TTReportProblem();
            try
            {
                DataSet dsMBList = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",EmpID),
                    new SqlParameter("@type",emptype),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetDetailsByEmpId");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new TicketingToolReportProblem
                    {
                        //RowNumber = dataRow.Field<long>("RowNumber"),
                        rp_id = dataRow.Field<long>("rp_id"),
                        rp_ticket_no = dataRow.Field<string>("rp_ticket_no"),
                        //rp_empid = dataRow.Field<long>("rp_empid"),
                        mt_desc = dataRow.Field<string>("mt_desc"),
                        pr_description = dataRow.Field<string>("pr_description"),
                        //rp_complaint_description = dataRow.Field<string>("rp_complaint_description"),
                        //rp_upload_document = dataRow.Field<string>("rp_upload_document"),
                        rp_report_problem_status = dataRow.Field<string>("rp_report_problem_status"),
                        //rp_date_of_submission = dataRow.Field<DateTime>("rp_date_of_submission"),
                        //rp_date_of_resolve = dataRow["rp_date_of_resolve"].ToString() != "" ? DateTime.Parse(dataRow["rp_date_of_resolve"].ToString()).ToString("dd/MM/yyyy") : "",
                        rp_remarks = dataRow.Field<string>("rp_remarks"),
                        rp_status = dataRow.Field<bool>("rp_status"),
                        UTYPE= dataRow.Field<int>("UTYPE"),
                        SubmissionDate = dataRow.Field<string>("SubmissionDate"),
                        ResolveDate = dataRow.Field<string>("ResolveDate"),
                        //rp_creation_datetime = dataRow.Field<DateTime>("rp_creation_datetime"),
                        //rp_updation_datetime = dataRow.Field<DateTime>("rp_updation_datetime"),
                        //rp_created_by = dataRow.Field<long>("rp_created_by"),
                        //rp_updated_by = dataRow.Field<long>("rp_updated_by")
                    }).ToList();
                    ApplicationDetails.TicketingToolReportProblemlist = MBList;
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }
        public TTReportProblem GetDetailsByIdDll1(int ID)
        {
            TTReportProblem ApplicationDetails = new TTReportProblem();
            try
            {
                DataSet dsMBList = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@Id",ID),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetDetailsById");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new TicketingToolReportProblem
                    {
                        //RowNumber = dataRow.Field<long>("RowNumber"),
                        
                        rp_id = dataRow.Field<long>("rp_id"),
                        rp_ticket_no = dataRow.Field<string>("rp_ticket_no"),
                        //rp_empid = dataRow.Field<long>("rp_empid"),
                        rp_module_id = dataRow.Field<int>("rp_module_id"),
                        rp_problem_type_id = dataRow.Field<int>("rp_problem_type_id"),
                        rp_complaint_description = dataRow.Field<string>("rp_complaint_description"),
                        rp_upload_document = dataRow.Field<string>("rp_upload_document"),
                        rp_report_problem_status = dataRow.Field<string>("rp_report_problem_status"),
                        rp_date_of_submission = dataRow.Field<DateTime>("rp_date_of_submission"),
                        //rp_date_of_resolve = dataRow.Field<DateTime>("rp_date_of_resolve"),
                        rp_remarks = dataRow.Field<string>("rp_remarks"),
                        //rp_status = dataRow.Field<bool>("rp_status"),
                        //rp_creation_datetime = dataRow.Field<DateTime>("rp_creation_datetime"),
                        //rp_updation_datetime = dataRow.Field<DateTime>("rp_updation_datetime"),
                        //rp_created_by = dataRow.Field<long>("rp_created_by"),
                        //rp_updated_by = dataRow.Field<long>("rp_updated_by")
                    }).ToList();
                    ApplicationDetails.TicketingToolReportProblemlist = MBList;
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }
        public tbl_report_problem GetDetailsByIdDll(int? ID)
        {
            tbl_report_problem ApplicationDetails = new tbl_report_problem();
            try
            {
                DataSet dsMBList = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@Id",ID),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetDetailsById");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new tbl_report_problem
                    {
                        //RowNumber = dataRow.Field<long>("RowNumber"),
                        rp_id = dataRow.Field<long>("rp_id"),
                        rp_ticket_no = dataRow.Field<string>("rp_ticket_no"),
                        //rp_empid = dataRow.Field<long>("rp_empid"),
                        rp_module_id = dataRow.Field<int>("rp_module_id"),
                        rp_problem_type_id = dataRow.Field<int>("rp_problem_type_id"),
                        rp_complaint_description = dataRow.Field<string>("rp_complaint_description"),
                        rp_upload_document = dataRow.Field<string>("rp_upload_document"),
                        rp_report_problem_status = dataRow.Field<string>("rp_report_problem_status"),
                        rp_date_of_submission = dataRow.Field<DateTime>("rp_date_of_submission"),
                        //rp_date_of_resolve = dataRow.Field<DateTime>("rp_date_of_resolve"),
                        rp_remarks = dataRow.Field<string>("rp_remarks"),
                        //rp_status = dataRow.Field<bool>("rp_status"),
                        //rp_creation_datetime = dataRow.Field<DateTime>("rp_creation_datetime"),
                        //rp_updation_datetime = dataRow.Field<DateTime>("rp_updation_datetime"),
                        //rp_created_by = dataRow.Field<long>("rp_created_by"),
                        //rp_updated_by = dataRow.Field<long>("rp_updated_by")
                    }).ToList<tbl_report_problem>();
                    ApplicationDetails = MBList.First<tbl_report_problem>();
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }

        public bool SaveReportProblemDll(TicketingToolReportProblem rp)
        {
            int res = 0;
            bool result = false;
            try
            {

               string s= TTUploadDocument(rp.UploadedDoc, rp.rp_empid, rp.extensionofDoc);
                rp.rp_upload_document = s;
                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@rp_ticket_no",rp.rp_ticket_no),
                    new SqlParameter("@rp_empid",rp.rp_empid),
                    new SqlParameter("@rp_module_id",rp.rp_module_id),
                    new SqlParameter("@rp_problem_type_id",rp.rp_problem_type_id),
                    new SqlParameter("@rp_complaint_description",rp.rp_complaint_description),
                    new SqlParameter("@rp_upload_document",rp.rp_upload_document),
                    //new SqlParameter("@rp_report_problem_status","Pending"),// rp.rp_report_problem_status),
                    //new SqlParameter("@rp_date_of_resolve",rp.rp_date_of_resolve),
                    //new SqlParameter("@rp_remarks",rp.rp_remarks),
                    //new SqlParameter("@rp_status",rp.rp_status),
                    new SqlParameter("@rp_created_by",rp.rp_created_by)
                    
                };
                //result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_InsertReportProblem"));
                res = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_InsertReportProblem"));
                if (res > 0) result = true; else result = false;
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        //Print MI Challan Print Details 
        //public VM_ChallanPrintDetails PrintMIChallanDetailsDll(long EmpID, int Category, string RefNos, string Type)
        //{
        //    VM_ChallanPrintDetails NBChallanDetails = new VM_ChallanPrintDetails();
        //    DataSet details = new DataSet();
        //    SqlParameter[] parms = {
        //      new SqlParameter("@empId",EmpID),
        //      new SqlParameter("@applicationId",RefNos),
        //      new SqlParameter("@category",Convert.ToString(Category)),
        //      new SqlParameter("@type",Type)
        //    };
        //    details = _Conn.ExeccuteDataset(parms, "sp_kgid_MB_Print_PaymentDetails");
        //    if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
        //    {
        //        NBChallanDetails.dm_ddo_code = details.Tables[0].Rows[0]["dm_ddo_code"].ToString();
        //        NBChallanDetails.dm_ddo_office = details.Tables[0].Rows[0]["dm_ddo_office"].ToString();
        //        NBChallanDetails.dm_deptname_english = details.Tables[0].Rows[0]["dm_deptname_english"].ToString();
        //        NBChallanDetails.dm_name_english = details.Tables[0].Rows[0]["dm_name_english"].ToString();
        //        NBChallanDetails.employee_name = details.Tables[0].Rows[0]["employee_name"].ToString();
        //        NBChallanDetails.ead_address = details.Tables[0].Rows[0]["ead_address"].ToString();
        //        NBChallanDetails.mobile_number = details.Tables[0].Rows[0]["mobile_number"].ToString();
        //        NBChallanDetails.hoa_desc = details.Tables[0].Rows[0]["hoa_desc"].ToString();
        //        NBChallanDetails.purpose_id = details.Tables[0].Rows[0]["purpose_id"].ToString();
        //        NBChallanDetails.purpose_desc = details.Tables[0].Rows[0]["purpose_desc"].ToString();
        //        NBChallanDetails.sub_purpose_desc = details.Tables[0].Rows[0]["sub_purpose_desc"].ToString();
        //        NBChallanDetails.p_premium = (details.Tables[0].Rows[0]["p_premium"] == DBNull.Value) ? (double?)0 : Convert.ToDouble((details.Tables[0].Rows[0]["p_premium"]));
        //        //NBChallanDetails.LastUpdatedDateTime = Convert.ToDateTime(details.Tables[0].Rows[0]["p_updation_datetime"].ToString());
        //    }
        //    return NBChallanDetails;
        //}
        //
        private string TTUploadDocument(HttpPostedFileBase document, long? ApplicationID, string docType)
        {
            string subPath = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
                subPath = "/TTDocuments/" + ApplicationID.ToString() + "/" + docType;
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(subPath));
                if (!exists)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                }

                string path = Path.Combine(HttpContext.Current.Server.MapPath(subPath), fileName);
                document.SaveAs(path);
                subPath = subPath + "/" + fileName;
            }

            return subPath;
        }

        public TTReportProblem GetAllReportedProblemsBasedonFiltersDLL(int moduleid, string fdate, string tdate, string fstatus)
        {
            TTReportProblem ApplicationDetails = new TTReportProblem();
            try
            {
                string FROMDATE = string.Empty;
                string TODATE = string.Empty;
                if (fdate != null && fdate!=string.Empty)
                {
                    FROMDATE = _commnobj.DateConversion(Convert.ToDateTime(fdate).ToShortDateString());
                }
                if (tdate != null && tdate != string.Empty)
                {
                    TODATE = _commnobj.DateConversion(Convert.ToDateTime(tdate).ToShortDateString());
                }
                if(fstatus== "--Select--")
                { fstatus = string.Empty; }
                //DateTime Fdat = Convert.ToDateTime(fdate);
                //var FROMDATE = Fdat.ToString("yyyy-MM-dd");
                //DateTime Tdat = Convert.ToDateTime(tdate);
                //var TODATE = Tdat.ToString("yyyy-MM-dd");
                DataSet dsMBList = new DataSet();
               // if(fstatus=="")
               // { fstatus = ""; }
               // else { fstatus = ""; }
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@module",moduleid),
                    new SqlParameter("@fdate", FROMDATE),
                    new SqlParameter("@tdate",TODATE),
                    new SqlParameter("@fstatus",fstatus),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetAllReportedProblemsBasedonFilters");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new TicketingToolReportProblem
                    {
                        // RowNumber = dataRow.Field<long>("RowNumber"),
                        rp_id = dataRow.Field<long>("rp_id"),
                        rp_ticket_no = dataRow.Field<string>("rp_ticket_no"),
                        //rp_empid = dataRow.Field<long>("rp_empid"),
                        mt_desc = dataRow.Field<string>("mt_desc"),
                        pr_description = dataRow.Field<string>("pr_description"),
                        //rp_complaint_description = dataRow.Field<string>("rp_complaint_description"),
                        //rp_upload_document = dataRow.Field<string>("rp_upload_document"),
                        rp_report_problem_status = dataRow.Field<string>("rp_report_problem_status"),
                        //rp_date_of_submission = dataRow.Field<DateTime>("rp_date_of_submission"),
                        SubmissionDate = dataRow.Field<string>("SubmissionDate"),
                        //rp_date_of_resolve = dataRow.Field<DateTime>("rp_date_of_resolve"),
                        rp_remarks = dataRow.Field<string>("rp_remarks"),
                        rp_status = dataRow.Field<bool>("rp_status"),
                        //rp_creation_datetime = dataRow.Field<DateTime>("rp_creation_datetime"),
                        //rp_updation_datetime = dataRow.Field<DateTime>("rp_updation_datetime"),
                        //rp_created_by = dataRow.Field<long>("rp_created_by"),
                        //rp_updated_by = dataRow.Field<long>("rp_updated_by")
                    }).ToList();
                    ApplicationDetails.TicketingToolReportProblemlist = MBList;
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }

        public bool UpdateReportProblemDll(TicketingToolReportProblem rp)
        {
            int res = 0;
            bool result = false;
            try
            {
                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@id",rp.rp_id),
                    //new SqlParameter("@rp_empid",rp.rp_empid),
                    //new SqlParameter("@rp_module_id",rp.rp_module_id),
                    //new SqlParameter("@rp_problem_type_id",rp.rp_problem_type_id),
                    //new SqlParameter("@rp_complaint_description",rp.rp_complaint_description),
                    //new SqlParameter("@rp_upload_document",rp.rp_upload_document),
                    new SqlParameter("@status",rp.rp_report_problem_status),// rp.rp_report_problem_status),
                    //new SqlParameter("@rp_date_of_resolve",rp.rp_date_of_resolve),
                    new SqlParameter("@comment",rp.rp_remarks),
                    new SqlParameter("@assignto",rp.rp_assignedto),
                    new SqlParameter("@updatedBy",rp.rp_created_by)
                };
                //result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_InsertReportProblem"));
                res = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_UpdateReportedProblem"));
                if (res > 0) result = true; else result = false;
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public bool UpdateAssignTicketDll(TicketingToolReportProblem rp)
        {
            int res = 0;
            bool result = false;
            try
            {
                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@id",rp.rp_id),
                    new SqlParameter("@assignto",rp.rp_assignedto),
                    new SqlParameter("@updatedBy",rp.rp_updated_by)
                };
                res = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_assignissue"));
                if (res > 0) result = true; else result = false;
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public bool UpdateIssueDetailsDll(TicketingToolReportProblem rp)
        {
            int res = 0;
            bool result = false;
            try
            {
                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@id",rp.rp_id),
                    new SqlParameter("@status",rp.rp_report_problem_status),
                    new SqlParameter("@comment",rp.rp_remarks),
                    new SqlParameter("@updatedBy",rp.rp_updated_by)
                };
                //result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_InsertReportProblem"));
                res = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_UpdateIssueDetails"));
                if (res > 0) result = true; else result = false;
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
