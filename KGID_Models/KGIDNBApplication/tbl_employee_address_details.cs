using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_employee_address_details
    {
        [Key]
        public int ead_id { get; set; }
        public Nullable<long> ead_sys_emp_code { get; set; }
        [Required(ErrorMessage = "Please Enter Address")]
        public string ead_address { get; set; }
        [Required(ErrorMessage = "Please Enter Pincode")]
        public string ead_pincode { get; set; }
        public Nullable<System.DateTime> ead_date { get; set; }
        public Nullable<bool> ead_active { get; set; }
        public Nullable<int> ead_created_by { get; set; }
        public Nullable<int> ead_updated_by { get; set; }
        public Nullable<System.DateTime> ead_creation_datetime { get; set; }
        public Nullable<System.DateTime> ead_updation_datetime { get; set; }
    }
}
