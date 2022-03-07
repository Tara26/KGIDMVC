using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_medical_declaration
    {
        [Key]
        public int md_id { get; set; }
        public long md_sys_emp_code { get; set; }
        public bool md_declaration_status { get; set; }
        public Nullable<long> md_referance_no { get; set; }
        public Nullable<bool> md_active { get; set; }
        public Nullable<System.DateTime> md_creation_datetime { get; set; }
        public Nullable<System.DateTime> md_updation_datetime { get; set; }
        public Nullable<int> md_created_by { get; set; }
        public Nullable<int> md_updated_by { get; set; }
    }
}
