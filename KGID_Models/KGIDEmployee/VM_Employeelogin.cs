using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class VM_Employeelogin
    {
        [Key]
        public int EmployeeType { get; set; }
        public string um_mobile_email { get; set; }
        public string um_otp { get; set; }

        public string Message { get; set; }
        //public int txtInputCaptcha { get; set; }
        //public int txtEnteredCaptcha { get; set; }
    }
}
