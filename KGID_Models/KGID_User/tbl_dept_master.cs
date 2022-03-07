using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGID_User
{
    public class tbl_dept_master
    {
        [Key]
        public int dm_deptid { get; set; }
        public string dm_deptcode { get; set; }
        public string dm_deptname_english { get; set; }
        public string dm_deptname_kannada { get; set; }
        public bool dm_active { get; set; }
        public Nullable<System.DateTime> dm_creation_datetime { get; set; }
        public Nullable<System.DateTime> dm_updation_datetime { get; set; }

    }
}
