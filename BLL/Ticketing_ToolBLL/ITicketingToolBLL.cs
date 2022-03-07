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
   public interface ITicketingToolBLL
    {
        IEnumerable<tbl_module_type_master> GetModuleListBLL();
        IEnumerable<tbl_problem_type_master> GetProblemTypeListBLL();
        bool SaveReportProblemBll(TicketingToolReportProblem _newempdetails);
        TTReportProblem GetDetailsByEmpIdBLL(int empid,string emptype);
        tbl_report_problem GetDetailsByIdBLL(int? id);
        TTReportProblem GetAllReportedProblemsBLL();
        TTReportProblem GetAllReportedProblemsBasedonFiltersBLL(int moduleid, string fdate, string tdate, string fstatus);
        bool UpdateReportProblemBll(TicketingToolReportProblem rp);
        DataTable CreateDataTable(string className);
        DataSet CreateDataTable1();
        bool UpdateAssignTicketBll(TicketingToolReportProblem _newempdetails);
        bool UpdateIssueDetailsBll(TicketingToolReportProblem rp);
    }
}
