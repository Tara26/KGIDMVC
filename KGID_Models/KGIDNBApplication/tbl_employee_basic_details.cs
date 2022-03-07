using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_employee_basic_details
    {
        [Key]
        public long employee_id { get; set; }
        public Nullable<long> hrms_employee_code { get; set; }
        public Nullable<int> dept_employee_code { get; set; }
        public string employee_name { get; set; }
        public string father_name { get; set; }
        public string spouse_name { get; set; }

        public string employee_name_kannada { get; set; }
        public string father_name_kannada { get; set; }
        public string spouse_name_kannada { get; set; }

        public Nullable<int> gender_id { get; set; }
        public Nullable<DateTime> date_of_birth { get; set; }
        public string place_of_birth { get; set; }
        public string pan_number { get; set; }
        public Nullable<DateTime> date_of_appointment { get; set; }
        public Nullable<long> mobile_number { get; set; }
        public string email_id { get; set; }
        public Nullable<bool> active_status { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public Nullable<long> created_by { get; set; }
        public Nullable<long> updated_by { get; set; }
        public long? first_kgid_policy_no { get; set; }
        public string user_category_id { get; set; }
        public Nullable<bool> ddo_upload_status { get; set; }
        public Nullable<int> dr_status { get; set; }
        public string Current_spouse_name { get; set; }
        public string recipient_id { get; set; }
    }
}
