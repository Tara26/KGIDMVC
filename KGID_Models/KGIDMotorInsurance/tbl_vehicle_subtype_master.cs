using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_vehicle_subtype_master
    {
        [Key]
        public long vst_vehicle_subtype_id { get; set; }
        public string vst_vehicle_subtype_desc { get; set; }
        public bool vst_status { get; set; }

        public DateTime vst_creation_datetime { get; set; }
        public int vst_created_by { get; set; }
        public DateTime vst_updation_datetime { get; set; }
        public int vst_updated_by { get; set; }
    }
}
