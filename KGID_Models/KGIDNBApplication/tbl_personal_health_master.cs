using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_personal_health_master
    {
        [Key]
        public int phm_health_code { get; set; }
        public string phm_health_desc { get; set; }
        public Nullable<int> phm_status_type { get; set; }
        public Nullable<bool> phm_active { get; set; }
        public Nullable<DateTime> phm_creation_datetime { get; set; }
        public Nullable<DateTime> phm_updation_datetime { get; set; }
        public Nullable<int> phm_created_by { get; set; }
        public Nullable<int> phm_updated_by { get; set; }
    }
}
