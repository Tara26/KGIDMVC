using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_load_deduction_master
    {
        [Key]
        public int ldm_id { get; set; }
        public Nullable<int> ldm_age { get; set; }
        public Nullable<int> ldm_sum_assured { get; set; }
        public string ldm_load_factor { get; set; }
        public Nullable<int> ldm_deduction { get; set; }
    }
}
