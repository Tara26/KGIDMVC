using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    
    public class tbl_employee_work_details
    {
        [Key]
        public int ewd_work_id { get; set; }
        public Nullable<long> ewd_emp_id { get; set; }
        public Nullable<DateTime> ewd_date_of_joining { get; set; }
        public int ewd_payscale_id { get; set; }
        public Nullable<int> ewd_employment_type { get; set; }
        public int? ewd_designation_id { get; set; }
        public int ewd_group_id { get; set; }
        public string ewd_place_of_posting { get; set; }
        public int ewd_ddo_id { get; set; }
        public Nullable<bool> ewd_active_status { get; set; }
        public Nullable<long> ewd_created_by { get; set; }
        public Nullable<long> ewd_updated_by { get; set; }
        public Nullable<System.DateTime> ewd_creation_datetime { get; set; }
        public Nullable<System.DateTime> ewd_updation_datetime { get; set; }
    }
}
