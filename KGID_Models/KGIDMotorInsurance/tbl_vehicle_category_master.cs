using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_vehicle_category_master
    {
        [Key]
        public int vc_vehicle_category_id { get; set; }
        public string vc_vehicle_category_desc { get; set; }
        public bool vc_status { get; set; }

        public DateTime vc_creation_datetime { get; set; }
        public int vc_created_by { get; set; }
        public DateTime vc_updation_datetime { get; set; }
        public int vc_updated_by { get; set; }
        public string vc_vehicle_type_id { get; set; }
        public long vc_vehicle_subtype_id { get; set; }



    }
}
