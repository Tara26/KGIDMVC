using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_VerifyData
{
    public class tbl_dsc_details
    {
        [Key]
        public long dd_detail_id { get; set; }
        public long dd_kgid_workflow_id { get; set; }
        public string dd_hash_value { get; set; }
        public string dd_dsc_path { get; set; }
        public decimal? dd_premium_amount { get; set; }
        public decimal? dd_sum_assured { get; set; }
        public int? dd_load_factor_id { get; set; }
        public int? dd_dl_factor_id { get; set; }
        public bool dd_active_status { get; set; }
        public long dd_created_by { get; set; }
        public DateTime dd_creation_datetime { get; set; }
        public long dd_updated_by { get; set; }
        public DateTime dd_updation_datetime { get; set; }
        public long dd_emp_id { get; set; }
    }
}
