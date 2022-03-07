using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_weight_mapping_master
    {
        [Key]
        public int wmm_id { get; set; }
        public int wmm_height_age_id { get; set; }
        public int wmm_weight { get; set; }
    }
}
