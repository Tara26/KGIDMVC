using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.Ticketing_Tool
{
    public class tbl_report_problem
    {
        [Key]
        public long rp_id { get; set; }
        public string rp_ticket_no  { get; set; }
        public int rp_empid { get; set; }
        public int rp_module_id { get; set; }
        public int rp_problem_type_id { get; set; }
        public int rp_assignedto { get; set; }
        public string rp_complaint_description { get; set; }
        public string rp_upload_document { get; set; }
        public string rp_report_problem_status { get; set; }
        public string rp_remarks { get; set; }
        public DateTime rp_date_of_submission { get; set; }
        public DateTime rp_date_of_resolve { get; set; }
        public DateTime rp_creation_datetime { get; set; }
        public DateTime rp_updation_datetime { get; set; }
        public bool rp_status { get; set; }
        public int rp_created_by { get; set; }
        public int rp_updated_by { get; set; }
        public string STS { get; set; }
    }
}
