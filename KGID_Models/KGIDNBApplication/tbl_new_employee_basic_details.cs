using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_new_employee_basic_details
    {
        [Key]
        public int nebd_id { get; set; }
        public Nullable<long> nebd_sys_emp_code { get; set; }
        public string nebd_dept_emp_code { get; set; }
        public string nebd_ddo_code { get; set; }
        public string nebd_emp_full_name { get; set; }
        public string nebd_father_name { get; set; }
        public string nebd_spouse_name { get; set; }
        public string nebd_gender { get; set; }
        public Nullable<DateTime> nebd_date_of_birth { get; set; }
        public string nebd_place_of_birth { get; set; }
        public string nebd_pan { get; set; }
        public string nebd_mobilenumber { get; set; }
        public string nebd_email { get; set; }
        public Nullable<DateTime> nebd_date_of_appointment { get; set; }
        public Nullable<bool> nebd_active { get; set; }
        public Nullable<int> nebd_created_by { get; set; }
        public Nullable<int> nebd_updated_by { get; set; }
        public string nebd_kgid_number { get; set; }
        public IEnumerable<tbl_new_employee_basic_details> GetNewEmployeeList { get; set; }
        public Nullable<System.DateTime> nebd_creation_datetime { get; set; }
        public Nullable<System.DateTime> nebd_updation_datetime { get; set; }
        public string nebd_policy_number { get; set; }
        public Nullable<decimal> nebd_premium { get; set; }
        public string nebd_kgid_roles { get; set; }
        public Nullable<bool> nebd_is_married { get; set; }
        public Nullable<bool> nebd_is_spouse_govt_emp { get; set; }
        public string nebd_spouse_kgid_no { get; set; }
        public Nullable<bool> is_orphan { get; set; }
    }
}
