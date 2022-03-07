using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_claim_petitioner_details
    {
        [Key]
        public long mvc_claim_petitioner_id { get; set; }
        public long mvc_claim_app_id { get; set; }
        public string mvc_petitioner_name { get; set; }
        public string mvc_petitioner_addres { get; set; }
        public long mvc_petitioner_mobile_no { get; set; }
        public long mvc_petitioner_pincode_no { get; set; }
        public bool mvcp_active_status { get; set; }
      
        public DateTime mvcp_creation_datetime { get; set; }
        public DateTime mvcp_updation_datetime { get; set; }
        public long mvcp_created_by { get; set; }
        public long mvcp_updated_by { get; set; }

    }
}
