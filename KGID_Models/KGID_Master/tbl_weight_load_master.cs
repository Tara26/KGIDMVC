using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_weight_load_master
    {
        [Key]
        public int wlm_id { get; set; }
        public int wlm_min_weight { get; set; }
        public int wlm_max_weight { get; set; }
        public string wlm_load_factor { get; set; }
        public string wlm_deduction_load_factor { get; set; }
        public string wlm_category { get; set; }
        public int wlm_dl_factor_id { get; set; }
    }
}
