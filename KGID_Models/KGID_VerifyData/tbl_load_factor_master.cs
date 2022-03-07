using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_VerifyData
{
    public class tbl_load_factor_master
    {
        [Key]
        public int lfm_load_factor_id { get; set; }
        public string lfm_load_factor_desc { get; set; }
        public int lfm_load_value { get; set; }
        public bool lfm_active { get; set; }
        public int lfm_created_by { get; set; }
        public DateTime lfm_creation_datetime { get; set; }
        public int lfm_updated_by { get; set; }
        public DateTime lfm_updation_datetime { get; set; }
    }
}
