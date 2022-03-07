using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_height_age_master
    {
        [Key]
        public int ha_id { get; set; }
        public int ha_min_height_cms { get; set; }
        public int ha_max_height_cms { get; set; }
        public decimal ha_height_feets { get; set; }
        public int ha_min_age { get; set; }
        public int ha_max_age { get; set; }
    }
}
