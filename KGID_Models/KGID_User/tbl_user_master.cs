using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_User
{
    public class tbl_user_master
    {
        [Key]
        public int um_user_id { get; set; }
        public string um_emp_name { get; set; }
        public long um_emp_code { get; set; }
        public Nullable<System.DateTime> um_dob { get; set; }
        public string um_email { get; set; }
        public string um_father_name { get; set; }
        public string um_mother_name { get; set; }
        public string um_designation { get; set; }
        public string um_working_office { get; set; }
        public Nullable<System.DateTime> um_joining_date { get; set; }
        public string um_phone { get; set; }
        public string um_birth_place { get; set; }
        public string um_job_type { get; set; }
        public string um_pay_scale { get; set; }
        public Nullable<int> um_status { get; set; }
        public Nullable<System.DateTime> um_creation_datetime { get; set; }
        public Nullable<System.DateTime> um_updation_datetime { get; set; }
    }
}
