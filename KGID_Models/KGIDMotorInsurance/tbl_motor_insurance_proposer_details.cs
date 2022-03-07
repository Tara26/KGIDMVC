using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_motor_insurance_proposer_details
    {
        [Key]
        public int mipd_proposer_id { get; set; }
        public string mipd_proposer_fullname { get; set; }
        public string mipd_address { get; set; }
        public Nullable<int> mipd_pincode { get; set; }
        public Nullable<long> mipd_telephone_no { get; set; }
        public Nullable<long> mipd_fax_no { get; set; }
        public string mipd_email { get; set; }
        public string mipd_occupation { get; set; }
        public int mipd_type_of_cover { get; set; }

       
    }
}
