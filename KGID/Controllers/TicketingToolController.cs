using BLL.Ticketing_ToolBLL;
using ClosedXML.Excel;
using KGID_Models.Ticketing_Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace KGID.Controllers
{
    public class TicketingToolController : Controller
    {
        private readonly ITicketingToolBLL _TicketingToolbll;
        public TicketingToolController()
        {
            this._TicketingToolbll = new TicketingToolBLL();
        }
        // GET: TicketingTool
        public ActionResult Index()
        {
            return View();
        }
        [Route("kgid-Report-Pro")]
        public ActionResult ReportProblem()
        {
            string Type = Session["Categories"].ToString();
            TTReportProblem _newEmpModel = new TTReportProblem();
            if (Session["UID"] != null)
            {
                _newEmpModel.TT_rp_empid = Convert.ToInt32(Session["UID"]);
                _newEmpModel.TT_rp_emptype = Session["Categories"].ToString();
            }
            _newEmpModel = _TicketingToolbll.GetDetailsByEmpIdBLL(_newEmpModel.TT_rp_empid,_newEmpModel.TT_rp_emptype);
            ////return View("_ViewReportProblemsInGrid", _newEmpModel);
            //return this.PartialView("_ViewReportProblemsInGrid", _newEmpModel);
            return View(_newEmpModel);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadModuleList()
        {
            var _modulelistData = _TicketingToolbll.GetModuleListBLL();

            return Json(_modulelistData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadProblemList()
        {
            var _ProblemlistData = _TicketingToolbll.GetProblemTypeListBLL();

            return Json(_ProblemlistData, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetDetailsById(int id=31)
        public ActionResult GetDetailsById(int? id)
        {
            //if (id <= 0) id = 31;
            tbl_report_problem _newEmpModel = new tbl_report_problem();
            ////int id = 0;
            
            _newEmpModel = _TicketingToolbll.GetDetailsByIdBLL(id);
            if (Session["UID"] != null)
            {
                _newEmpModel.STS = Session["Categories"].ToString();
            }
            _newEmpModel.rp_id = Convert.ToInt64( id);           //retTTReportProblemurn View("_GetDetailsById", _newEmpModel);
            //TTReportProblem _newEmpModel = new TTReportProblem();
            //_newEmpModel = _TicketingToolbll.GetDetailsByIdBLL(id);
            return this.PartialView("_GetDetailsById", _newEmpModel);
        }

        public ActionResult UpdateReportedProblem(TicketingToolReportProblem _newempdetails)
        {
            TicketingToolReportProblem _newEmpModel = new TicketingToolReportProblem();
            if (Session["UID"] != null)
            {
                _newEmpModel.rp_updated_by = Convert.ToInt64(Session["UID"]);
            }
            var _newempDetails = _TicketingToolbll.UpdateReportProblemBll(_newempdetails);

            return View(_newEmpModel);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        [Route("kgid-AllReport-Pro")]
        public ActionResult GetAllReportProblems()
        {
            TTReportProblem _newEmpModel = new TTReportProblem();
            if (Session["UID"] != null)
            {
                _newEmpModel.TT_rp_empid = Convert.ToInt32(Session["UID"]);
            }
            if (ViewBag.newEmpModel == null)
            {
                _newEmpModel = _TicketingToolbll.GetAllReportedProblemsBLL();
            }
            else
            {
                _newEmpModel = (TTReportProblem)ViewBag.newEmpModel;

            }
            return View("GetAllReportProblems", _newEmpModel);

            //return View();
        }
        [HttpPost]
        public ActionResult GetReportProblemsBasedonFilters(int mid,string fdate, string todate,string fstatus)
        {
            TTReportProblem _newEmpModel = new TTReportProblem();
            if (Session["UID"] != null)
            {
                _newEmpModel.TT_rp_empid = Convert.ToInt32(Session["UID"]);
            }
            _newEmpModel = _TicketingToolbll.GetAllReportedProblemsBasedonFiltersBLL(mid,fdate,todate, fstatus);

            //TempData["newEmpModel"] = _newEmpModel;
            ViewBag.newEmpModel = _newEmpModel;
            //TempData.Keep();

            return View("GetAllReportProblems", _newEmpModel);

            //return View();
        }

        public ActionResult SaveReportedProblem(TicketingToolReportProblem model)
        {
            HttpPostedFileBase postedFile = null;
            string filePath = string.Empty;
            string extension = string.Empty;

            if (Request.Files.Count > 0)
            {
                 postedFile = Request.Files[0];
            }

            if (postedFile != null)
            {
                model.UploadedDoc = postedFile;
                model.extensionofDoc = Path.GetExtension(postedFile.FileName);
            }

                TicketingToolReportProblem _newEmpModel = new TicketingToolReportProblem();  
            if (Session["UID"] != null)
            {
                //_newEmpModel.rp_created_by = Convert.ToInt64(Session["UID"]);
                //_newEmpModel.rp_empid = Convert.ToInt64(Session["UID"]);
                model.rp_created_by=Convert.ToInt64(Session["UID"]);
                model.rp_empid = Convert.ToInt64(Session["UID"]);
            }
            var _newempDetails = _TicketingToolbll.SaveReportProblemBll(model);

            //tbl_ddo_master _ddoMasterModel = new tbl_ddo_master();
            //var _ddoMasterDetails = _ddomaster.DDOMasterbll();

            //_newEmpModel.GetNewEmployeeList = _newempDetails;
            //TempData["NewEmpList"] = _newempDetails;

            //ViewBag.Data = _ddoMasterDetails;
            ////TempData["DDOMasterList"] = _ddoMasterDetails;
            return View(_newEmpModel);
        }
        public ActionResult GetReportProblemsByEmpId()
        {
            TTReportProblem _newEmpModel = new TTReportProblem();
            if (Session["UID"] != null)
            {
                _newEmpModel.TT_rp_empid = Convert.ToInt32(Session["UID"]);
                _newEmpModel.TT_rp_emptype = Session["Categories"].ToString();
            }
            _newEmpModel = _TicketingToolbll.GetDetailsByEmpIdBLL(_newEmpModel.TT_rp_empid,_newEmpModel.TT_rp_emptype);
            //return View("_ViewReportProblemsInGrid", _newEmpModel);
            return this.PartialView("_ViewReportProblemsInGrid", _newEmpModel);
        }
        [Route("View-All-Tickets")]
        public ActionResult AdminViewAllReportedProblems() 
        {
            TTReportProblem _newEmpModel = new TTReportProblem();
            if (Session["UID"] != null)
            {
                _newEmpModel.TT_rp_empid = Convert.ToInt32(Session["UID"]);
            }
            if (ViewBag.newEmpModel == null)
            {
                _newEmpModel = _TicketingToolbll.GetAllReportedProblemsBLL();
                Session["dstest"] = _TicketingToolbll.CreateDataTable1();
            }
            else
            {
                _newEmpModel = (TTReportProblem)ViewBag.newEmpModel;

            }
            return View("AdminViewAllReportedProblems", _newEmpModel);
        }
        public void DownloadXLS()
        {
            DataSet ds = (DataSet)Session["dstest"];
            DataTable dt = ds.Tables[0];
            //Session["dtError"] = null;
            if (dt.Rows.Count > 0)
            {
                ///////////////////////////////////
                Response.ContentEncoding = new UTF8Encoding(true);
                Response.Charset = Encoding.UTF8.WebName;
                Response.ContentType = "application/vnd.ms-excel; charset=utf-8";
                Response.AddHeader("content-disposition", string.Format("attachment;filename=ListOfReportedProblems.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));
                StringBuilder fileContent = new StringBuilder();
                foreach (var col in dt.Columns)
                {
                    fileContent.Append(col.ToString() + ",");
                }
                fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (var column in dr.ItemArray)
                    {
                        fileContent.Append("\"" + column.ToString() + "\",");
                    }
                    fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);
                }

                //Write encoding characters first  
                Response.Write('\uFEFF');

                //Write the content  
                Response.Write(fileContent.ToString());

                Response.End();
                ///
                //string FileName = "EmployeeUploadErrorList";
                //dt.TableName = "Excel";
                //using (XLWorkbook wb = new XLWorkbook())
                //{
                //    wb.Worksheets.Add(dt);
                //    using (MemoryStream stream = new MemoryStream())
                //    {
                //        wb.SaveAs(stream);
                //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + FileName + ".xlsx");
                //    }
                //}
         
            }
            //else
            //{
            //return null;
            //}
        }

        //public JsonResult LoadMobileAndEmailByDeptCode(string _ddoCode, long _empCode)
        //{
        //    var _empData = _newemp.GetMobileAndEmailByDeptCode(_ddoCode, _empCode);

        //    return Json(_empData, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult getEmployeeData()
        //{
        //    var _empData = _newemp.GetEmployeeBasicData();

        //    var _empDataList = (from n in _empData
        //                        select new
        //                        {
        //                            n.nebd_dept_emp_code,
        //                            nebd_date_of_birth = n.nebd_date_of_birth.HasValue ? n.nebd_date_of_birth.Value.ToString("dd-MM-yyyy") : string.Empty,
        //                            n.nebd_place_of_birth,
        //                            n.nebd_emp_full_name,
        //                            n.nebd_mobilenumber,
        //                            n.nebd_email,
        //                            n.nebd_father_name,
        //                            n.nebd_spouse_name,
        //                            n.nebd_gender,
        //                            n.nebd_sys_emp_code
        //                        }).ToList();

        //    return Json(_empDataList, JsonRequestBehavior.AllowGet);
        //}
        [Route("sa-assign-tickets")]
        public ActionResult AssignTicketToHelpDesk(int? id, int STS)
        {
            //if (id <= 0) id = 31;
            tbl_report_problem _newEmpModel = new tbl_report_problem();
            ////int id = 0;
            _newEmpModel = _TicketingToolbll.GetDetailsByIdBLL(id);
            //if (_newEmpModel.rp_report_problem_status == "Resolved")
            //{
            //    _newEmpModel.STS = 1;
            //}
            //else
            //{
            //    _newEmpModel.STS = STS;
            //}
            _newEmpModel.rp_id = Convert.ToInt64(id);           //retTTReportProblemurn View("_GetDetailsById", _newEmpModel);
            //TTReportProblem _newEmpModel = new TTReportProblem();
            //_newEmpModel = _TicketingToolbll.GetDetailsByIdBLL(id);
            return this.PartialView("_AssignTicket", _newEmpModel);
        }
        [Route("sa-assign-to-helpdesk")]
        public ActionResult AssignTicket(TicketingToolReportProblem _newempdetails)
        {
            TicketingToolReportProblem _newEmpModel = new TicketingToolReportProblem();
            if (Session["UID"] != null)
            {
                _newEmpModel.rp_updated_by = Convert.ToInt64(Session["UID"]);
            }
            var _newempDetails = _TicketingToolbll.UpdateAssignTicketBll(_newempdetails);

            return View(_newEmpModel);
        }
        //[Route("hd-issue-update")]
        //public ActionResult UpdateIssueDetails(tbl_report_problem _newempdetails)
        //{
        //    TicketingToolReportProblem _newEmpModel = new TicketingToolReportProblem();
        //    if (Session["UID"] != null)
        //    {
        //        _newEmpModel.rp_updated_by = Convert.ToInt64(Session["UID"]);
        //    }
        //    var _newempDetails = _TicketingToolbll.UpdateIssueDetailsBll(_newempdetails);

        //    return View(_newEmpModel);
        //}
        [Route("hd-issue-update")]
        public ActionResult UpdateIssueDetails(TicketingToolReportProblem _newempdetails)
        {
            TicketingToolReportProblem _newEmpModel = new TicketingToolReportProblem();
            if (Session["UID"] != null)
            {
                _newEmpModel.rp_updated_by = Convert.ToInt64(Session["UID"]);
            }
            var _newempDetails = _TicketingToolbll.UpdateIssueDetailsBll(_newempdetails);

            return View(_newEmpModel);
        }
    }
}