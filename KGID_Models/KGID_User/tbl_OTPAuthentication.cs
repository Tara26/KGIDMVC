using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGID_User
{
    public class tbl_OTPAuthentication
    {
        [Key]
        public int oat_id { get; set; }

        public string oat_sys_emp_code { get; set; }
        
        public int oat_OTP { get; set; }

        public Nullable<System.DateTime> oat_date { get; set; }

        public string oat_activity { get; set; }

        public string oat_authn_status { get; set; }

    }
}
