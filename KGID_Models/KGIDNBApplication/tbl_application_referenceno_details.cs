using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_application_referenceno_details
    {
        [Key]
        public int ard_id { get; set; }
        public long ard_application_reference_no { get; set; }
        public Nullable<long> ard_system_emp_code { get; set; }
        public Nullable<DateTime> ard_datetime { get; set; }
        public int ard_submission_status { get; set; }
        public Nullable<bool> ard_active_status { get; set; }
        public Nullable<DateTime> ard_creation_datetime { get; set; }
        public Nullable<DateTime> ard_updation_datetime { get; set; }
        public Nullable<int> ard_created_by { get; set; }
        public Nullable<int> ard_updated_by { get; set; }
    }
}
