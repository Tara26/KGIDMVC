using DLL;
using KGID_Models.Ticketing_Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BLL.Ticketing_ToolBLL
{
   public class TicketingToolBLL:ITicketingToolBLL
    {
        private readonly ITicketingToolDLL _ITicketingTooldll;
        public TicketingToolBLL()
        {
            this._ITicketingTooldll = new TicketingToolDLL();
        }

        public IEnumerable<tbl_module_type_master> GetModuleListBLL()
        {
            return _ITicketingTooldll.GetModuleListDLL();
        }
        public IEnumerable<tbl_problem_type_master> GetProblemTypeListBLL()
        {
            return _ITicketingTooldll.GetProblemTypeListDLL();
        }
        public TTReportProblem GetDetailsByEmpIdBLL(int empid,string emptype)
        {
            return _ITicketingTooldll.GetDetailsByEmpIdDll(empid,emptype);
        }
        public tbl_report_problem GetDetailsByIdBLL(int? id)
        {
            return _ITicketingTooldll.GetDetailsByIdDll(id);
        }
        //public TTReportProblem GetDetailsByIdBLL(int id)
        //{
        //    return _ITicketingTooldll.GetDetailsByIdDll(id);
        //}
        public bool SaveReportProblemBll(TicketingToolReportProblem _newempdetails)
        {
            var result = _ITicketingTooldll.SaveReportProblemDll(_newempdetails);
            return result;
        }
        public TTReportProblem GetAllReportedProblemsBLL()
        {
            return _ITicketingTooldll.GetAllReportedProblemsDLL();

        }
        

        public TTReportProblem GetAllReportedProblemsBasedonFiltersBLL(int moduleid, string fdate, string tdate, string fstatus)
        {
            return _ITicketingTooldll.GetAllReportedProblemsBasedonFiltersDLL(moduleid, fdate,  tdate, fstatus);
        }
        public bool UpdateReportProblemBll(TicketingToolReportProblem rp)
        {
             var result = _ITicketingTooldll.UpdateReportProblemDll(rp);
            return result;
        }

        public DataTable CreateDataTable(string className)
        {
            return _ITicketingTooldll.CreateDataTable(className);
        }

        public DataSet CreateDataTable1()
        {
            return _ITicketingTooldll.CreateDataTable1();
        }

        public bool UpdateAssignTicketBll(TicketingToolReportProblem _newempdetails)
        {
            var result = _ITicketingTooldll.UpdateAssignTicketDll(_newempdetails);
            return result;
        }

        public bool UpdateIssueDetailsBll(TicketingToolReportProblem rp)
        {
            var result = _ITicketingTooldll.UpdateIssueDetailsDll(rp);
            return result;
        }
    }
}
