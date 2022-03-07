using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.Ticketing_Tool
{
    public class TTReportProblem
    {
        public IList<TicketingToolReportProblem> TicketingToolReportProblemlist { get; set; }
        public int TT_rp_empid { get; set; }
        public string TT_rp_emptype { get; set; }
        public TTReportProblem()
        {
            TicketingToolReportProblemlist = new List<TicketingToolReportProblem>();
        }
    }
    public class TicketingToolReportProblem
    {
        public long RowNumber { get; set; }
        public string extensionofDoc { get; set; }
        public HttpPostedFileBase UploadedDoc { get; set; }
        [Key]
        public long rp_id { get; set; }
        public string rp_ticket_no { get; set; }
        public long rp_empid { get; set; }
        public int rp_module_id { get; set; }
        public int rp_problem_type_id { get; set; }
        public string rp_complaint_description { get; set; }
        public string mt_desc { get; set; }
        public string pr_description { get; set; }
        public string rp_upload_document { get; set; }
        public string rp_report_problem_status { get; set; }
        public string rp_remarks { get; set; }
        public DateTime rp_date_of_submission { get; set; }
        public string rp_date_of_resolve { get; set; }
        public DateTime rp_creation_datetime { get; set; }
        public DateTime rp_updation_datetime { get; set; }
        public bool rp_status { get; set; }
        public long rp_created_by { get; set; }
        public long rp_updated_by { get; set; }
        public int rp_assignedto { get; set; }
        public string AssignedTo { get; set; }
        public int UTYPE { get; set; }
        public string SubmissionDate { get; set; }
        public string ResolveDate { get; set; }
    }
}
