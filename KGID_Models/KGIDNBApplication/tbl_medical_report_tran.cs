using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_medical_report_tran
    {
        [Key]
        public int mrt_tran_id { get; set; }
        public Nullable<long> mrt_sys_emp_code { get; set; }
        public string mrt_height { get; set; }
        public string mrt_weight { get; set; }
        public string mrt_pulserate_number { get; set; }
        public string mrt_breathing_rate { get; set; }
        public string mrt_blood_pressure { get; set; }
        public string mrt_low_dystolic { get; set; }
        public string mrt_high_systolic { get; set; }
        public string mrt_pulse_rate { get; set; }
        public string mrt_remarks { get; set; }
        public Nullable<bool> mrt_status { get; set; }
        public Nullable<System.DateTime> mrt_creation_datetime { get; set; }
        public Nullable<System.DateTime> mrt_updation_datetime { get; set; }
        public Nullable<int> mrt_created_by { get; set; }
        public Nullable<int> mrt_updated_by { get; set; }
        public string mrt_load_factor { get; set; }
    }
}
