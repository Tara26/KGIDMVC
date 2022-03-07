using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGID_Verification
{
    public class tbl_kgid_application_workflow_details
    {
        [Key]
        public long kawt_workflow_id { get; set; }
        public long kawt_application_id { get; set; }
        public long kawt_verified_by { get; set; }
        public long? kawt_assigned_to { get; set; }
        public string kawt_remarks { get; set; }
        public string kawt_comments { get; set; }
        public int? kawt_number_of_medical_leaves { get; set; }
        public bool kawt_checklist_verification_status { get; set; }
        public int kawt_application_status { get; set; }
        public bool kawt_active_status { get; set; }
        public long kawt_created_by { get; set; }
        public long kawt_updated_by { get; set; }
        public DateTime kawt_creation_datetime { get; set; }
        public DateTime kawt_updation_datetime { get; set; }
    }
}
