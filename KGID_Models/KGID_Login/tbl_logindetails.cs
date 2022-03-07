using System;
using System.Collections.Generic;
using KGID_Models.KGIDNBApplication;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Login
{
    public class tbl_logindetails
    {
        public tbl_logindetails(string val)
        {
            dynamc_code = val;
        }
        public tbl_logindetails()
        {


        }
        public int um_user_id { get; set; }
        [Required(ErrorMessage = "Please Enter Username")]
        public string um_user_name { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string um_user_password { get; set; }
        public Nullable<int> um_designation_id { get; set; }
        public Nullable<int> um_office_id { get; set; }
        public Nullable<int> um_post_id { get; set; }
        public string um_mobile_number { get; set; }
        public string um_email { get; set; }
        public Nullable<int> um_status { get; set; }
        public Nullable<System.DateTime> um_creation_datetime { get; set; }
        public Nullable<System.DateTime> um_updation_datetime { get; set; }
        public string um_user_New_password { get; set; }
        public string um_user_Confirm_password { get; set; }
        public string um_user_category_id { get; set; }

        public string txtInputCaptcha { get; set; }
        [Required(ErrorMessage = "Please Enter Captcha")]
        public string txtEnteredCaptcha { get; set; }
        public int nebd_id { get; set; }
        public Nullable<long> nebd_sys_emp_code { get; set; }
        public string nebd_dept_emp_code { get; set; }
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
        public Nullable<System.DateTime> nebd_creation_datetime { get; set; }
        public Nullable<System.DateTime> nebd_updation_datetime { get; set; }
        public IEnumerable<tbl_new_employee_basic_details> GetNewEmployeeList { get; set; }

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
        private string dynamc_code;
        public string Dynamc_Code
        {
            get { return dynamc_code; }
            set { dynamc_code = value; }

        }
        public string image_captcha { get; set; }
    }
}
