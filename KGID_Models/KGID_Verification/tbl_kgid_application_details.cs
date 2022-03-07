using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGID_Verification
{
    public class tbl_kgid_application_details
    {
        [Key]
        public long kad_application_id { get; set; }
        public string kad_kgid_application_number { get; set; }
        public string kad_date_of_submission { get; set; }
        public long kad_emp_id { get; set; }
        public bool kad_active_status { get; set; }
        public long kad_created_by { get; set; }
        public long kad_updated_by { get; set; }
        public DateTime kad_creation_datetime { get; set; }
        public DateTime kad_updation_datetime { get; set; }
    }
}
