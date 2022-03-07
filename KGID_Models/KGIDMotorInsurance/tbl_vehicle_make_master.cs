using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
   public class tbl_vehicle_make_master
    {
        [Key]
        public int vm_vehicle_make_id { get; set; }
        public string vm_vehicle_make_desc { get; set; }
        public string vm_vehicle_make_code { get; set; }
        public bool vm_status { get; set; }
        public DateTime vm_creation_datetime { get; set; }
        public DateTime vm_updation_datetime { get; set; }
        public int vm_created_by { get; set; }
        public int vm_updated_by { get; set; }
    }
}
