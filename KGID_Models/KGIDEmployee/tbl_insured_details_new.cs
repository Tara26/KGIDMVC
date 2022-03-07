using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_insured_details_new
    {
        [Key]
        public long id_id { get; set; }
        public Nullable<long> id_sys_emp_code { get; set; }
        public string id_dept_emp_code { get; set; }
        public string id_emp_full_name { get; set; }
        public string id_father_name { get; set; }
        public string id_spouse_name { get; set; }
        public string id_gender { get; set; }
        public Nullable<DateTime> id_date_of_birth { get; set; }
        public string id_place_of_birth { get; set; }
        public string id_pan { get; set; }
        public string id_mobilenumber { get; set; }
        public string id_email { get; set; }
        public Nullable<DateTime> id_date_of_appointment { get; set; }
        public Nullable<DateTime> id_date_of_joining_post { get; set; }
        public string id_payscle_code { get; set; }
        public string id_permanent_temporary { get; set; }
        public string id_designation { get; set; }
        public string id_group { get; set; }
        public string id_place_of_posting { get; set; }
        public string id_ddo_code { get; set; }
        public Nullable<bool> id_active { get; set; }
        public Nullable<int> id_created_by { get; set; }
        public Nullable<int> id_updated_by { get; set; }
        public Nullable<DateTime> id_creation_datetime { get; set; }
        public Nullable<DateTime> id_updation_datetime { get; set; }
    }
}
