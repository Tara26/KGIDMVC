using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_policy_details
    {
        [Key]
        public long p_policy_id { get; set; }
        public string p_kgid_policy_number { get; set; }
        public long p_emp_id { get; set; }
        public DateTime? p_sanction_date { get; set; }
        public DateTime? p_maturity_date { get; set; }
        public double? p_premium { get; set; }
        public int? p_load_factor_id { get; set; }
        public int? p_dl_factor_id { get; set; }
        public long p_application_id { get; set; }
        public double? p_sum_assured { get; set; }
        public int? p_no_of_premium { get; set; }
        public string p_bond_upload_path { get; set; }
        public string p_fs_upload_path { get; set; }
        public bool p_active_status { get; set; }
        public long p_created_by { get; set; }
        public DateTime p_creation_datetime { get; set; }
        public long p_updated_by { get; set; }
        public DateTime p_updation_datetime { get; set; }
        public int? p_age { get; set; }
    }
}
