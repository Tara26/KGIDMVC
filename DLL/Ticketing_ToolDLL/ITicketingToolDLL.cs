
using KGID_Models.Ticketing_Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL
{
    public interface ITicketingToolDLL
    {
        IEnumerable<tbl_module_type_master> GetModuleListDLL();
        IEnumerable<tbl_problem_type_master> GetProblemTypeListDLL();
        TTReportProblem GetAllReportedProblemsDLL();
        TTReportProblem GetAllReportedProblemsBasedonFiltersDLL(int moduleid, string fdate, string tdate, string fstatus);
        bool SaveReportProblemDll(TicketingToolReportProblem rp);
        bool UpdateReportProblemDll(TicketingToolReportProblem rp);
        TTReportProblem GetDetailsByEmpIdDll(long EmpID,string emptype);
        tbl_report_problem GetDetailsByIdDll(int? ID);
        DataTable CreateDataTable(string className);
        DataSet CreateDataTable1();
        bool UpdateAssignTicketDll(TicketingToolReportProblem rp);
        bool UpdateIssueDetailsDll(TicketingToolReportProblem rp);
    }
}
