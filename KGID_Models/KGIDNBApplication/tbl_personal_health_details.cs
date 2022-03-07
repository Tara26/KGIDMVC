using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_personal_health_details
    {
        [Key]
        public int phd_id { get; set; }
        public long phd_sys_emp_code { get; set; }
        public string phd_health_code { get; set; }
        public Nullable<bool> phd_health_condition { get; set; }
        public Nullable<bool> phd_treatment_details { get; set; }
        public string phd_Height { get; set; }
        public string phd_Weight { get; set; }
        public Nullable<DateTime> phd_period_date { get; set; }
        public Nullable<bool> phd_Pregnant { get; set; }

        public string phd_Health_Remarks { get; set; }
        public Nullable<bool> phd_active { get; set; }
        public Nullable<DateTime> phd_creation_datetime { get; set; }
        public Nullable<DateTime> phd_updation_datetime { get; set; }
        public Nullable<int> phd_created_by { get; set; }
        public Nullable<int> phd_updated_by { get; set; }
    }
}
