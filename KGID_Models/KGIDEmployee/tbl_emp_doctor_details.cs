using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_emp_doctor_details
    {
        [Key]
        public int edd_id { get; set; }
        public Nullable<long> edd_sys_emp_code { get; set; }
        public string edd_kmc_code { get; set; }
        public string edd_name_of_doctor { get; set; }
        public string edd_designation { get; set; }
        public string edd_name_of_officer { get; set; }
        public string edd_kgid { get; set; }
        public Nullable<bool> edd_status { get; set; }
        public Nullable<DateTime> edd_creation_datetime { get; set; }
        public Nullable<DateTime> edd_updation_datetime { get; set; }
        public Nullable<int> edd_created_by { get; set; }
        public Nullable<int> edd_updated_by { get; set; }
    }
}
