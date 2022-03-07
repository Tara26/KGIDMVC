using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_vehicle_type_master
    {
        [Key]
        public long vht_vehicle_type_id { get; set; }
        public string  vht_vehicle_type_desc { get; set; }
        public bool vht_status { get; set; }
      
        public DateTime vht_creation_datetime { get; set; }
        public int vht_created_by { get; set; }
        public DateTime vht_updation_datetime { get; set; }
        public int vht_updated_by { get; set; }
    }
}
