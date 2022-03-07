using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_VerifyData
{
    public class tbl_dl_factor_master
    {
        [Key]
        public int dlfm_dl_factor_id { get; set; }
        public string dlfm_dl_factor_desc { get; set; }
        public int dlfm_no_of_years { get; set; }
        public decimal dlfm_amount { get; set; }
        public int dlfm_load_factor_id { get; set; }
        public int dlfm_active_status { get; set; }
        public bool dlfm_status { get; set; }
        public int lfm_created_by { get; set; }
        public DateTime lfm_creation_datetime { get; set; }
        public int lfm_updated_by { get; set; }
        public DateTime lfm_updation_datetime { get; set; }
    }
}
