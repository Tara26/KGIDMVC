using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_kgid_mapping_details
    {
        [Key]
        public int kmd_id { get; set; }
        public string kmd_first_policy_no { get; set; }
        public string kmd_subsequent_policy_no { get; set; }
        public Nullable<long> kmd_emp_id { get; set; }
        public string kmd_emp_name { get; set; }
        public Nullable<bool> kmd_status { get; set; }
        public Nullable<DateTime> kmd_creation_datetime { get; set; }
        public Nullable<DateTime> kmd_updation_datetime { get; set; }
        public Nullable<int> kmd_created_by { get; set; }
        public Nullable<int> kmd_updated_by { get; set; }
    }
}
