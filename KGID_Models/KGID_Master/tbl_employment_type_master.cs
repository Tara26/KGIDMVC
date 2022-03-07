using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_User
{
    public class tbl_employment_type_master
    {
        [Key]
        public int et_employee_type_id { get; set; }
        public string et_employee_type_desc { get; set; }
        public Nullable<bool> et_active { get; set; }
        public Nullable<DateTime> et_creation_datetime { get; set; }
        public Nullable<DateTime> et_updation_datetime { get; set; }
        public Nullable<int> et_created_by { get; set; }
        public Nullable<int> et_updated_by { get; set; }
    }
}
