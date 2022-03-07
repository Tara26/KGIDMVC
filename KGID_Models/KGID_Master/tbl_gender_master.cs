using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_User
{
    public class tbl_gender_master
    {
        [Key]
        public int gender_id { get; set; }
        public string gender_desc { get; set; }
        public Nullable<bool> gender_status { get; set; }
        public Nullable<bool> active_status { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<int> updated_by { get; set; }
    }
}
