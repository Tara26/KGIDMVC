using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_VerifyData
{
    public class tbl_ranges_master
    {
        [Key]
        public int rm_id { get; set; }
        public Nullable<int> rm_dist_id { get; set; }
        public int rm_start_no { get; set; }
        public string rm_end_no { get; set; }
        public Nullable<bool> rm_status { get; set; }
        public Nullable<System.DateTime> rm_creation_datetime { get; set; }
        public Nullable<System.DateTime> rm_updation_datetime { get; set; }
        public Nullable<int> rm_created_by { get; set; }
        public Nullable<int> rm_updated_by { get; set; }
        
    }
}
