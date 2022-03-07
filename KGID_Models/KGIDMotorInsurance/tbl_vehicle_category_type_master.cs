using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
   
    public class tbl_vehicle_category_type_master
    {
        [Key]
        public int vct_vehicle_category_type_id { get; set; }
       public string vct_vehicle_category_type_code { get; set; }
        public string  vct_vehicle_category_type_desc { get; set; }
        public bool vct_status { get; set; }
        public DateTime? vct_creation_datetime{ get; set; }
        public DateTime? vct_updation_datetime { get; set; }
        public int vct_created_by{ get; set; }
    }
}
