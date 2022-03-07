using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDLoan
{
    public class tbl_loan_application_workflow
    {
        [Key]
        public long law_loan_workflow_id { get; set; }
        public long law_loan_application_id { get; set; }
        public long law_verified_by { get; set; }
        public string law_remarks { get; set; }
        public string law_comments { get; set; }
        public bool? law_checklist_verification_status { get; set; }
        public int law_application_status { get; set; }
        public bool law_active { get; set; }
        public DateTime law_creation_datetime { get; set; }
        public int law_created_by { get; set; }
        public DateTime law_updation_datetime { get; set; }
        public int law_updated_by { get; set; }
    }
}
