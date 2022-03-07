using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_User
{
    public class tbl_login_master
    {
        [Key]
        public int um_user_id { get; set; }
        public string um_user_name { get; set; }
        public string um_user_password { get; set; }
        public string um_kgid_number { get; set; }
        public Nullable<int> um_designation_id { get; set; }
        public Nullable<int> um_office_id { get; set; }
        public Nullable<int> um_post_id { get; set; }
        public string um_mobile_number { get; set; }
        public string um_email { get; set; }
        public Nullable<int> um_status { get; set; }
        public Nullable<System.DateTime> um_creation_datetime { get; set; }
        public Nullable<System.DateTime> um_updation_datetime { get; set; }
        public string um_role { get; set; }

        public string txtInputCaptcha { get; set; }
        //[Required(ErrorMessage = "Please Enter Captcha")]
        public string txtEnteredCaptcha { get; set; }
    }
}
