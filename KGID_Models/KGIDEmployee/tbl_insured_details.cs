using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_insured_details
    {
        [Key]
        public int id_emp_code { get; set; }
        public long id_kgid_policy_no { get; set; }
        public string id_emp_fullname { get; set; }
        public string id_father_name { get; set; }
        public string id_gender { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> id_dob { get; set; }
        public int id_pay_scale { get; set; }
        public Nullable<int> id_designation { get; set; }
        public string id_permanent_temporary { get; set; }
        public string id_group { get; set; }
        public string id_mobile { get; set; }
        public string id_email { get; set; }
        public string id_place_of_posting { get; set; }
        public string id_dept_code { get; set; }
        public Nullable<bool> id_status { get; set; }
        public Nullable<System.DateTime> id_creation_datetime { get; set; }
        public Nullable<System.DateTime> id_updation_datetime { get; set; }
        public Nullable<int> id_created_by { get; set; }
        public Nullable<int> id_updated_by { get; set; }
    }
}
