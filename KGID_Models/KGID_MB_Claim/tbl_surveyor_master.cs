using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_MB_Claim
{
    public class tbl_surveyor_master
    {
        [Key]
        public long svm_id { get; set; }
        public string svm_name { get; set; }
        public string svm_mobile_no { get; set; }
        public string svm_email { get; set; }
        public string svm_address { get; set; }
        public string svm_pincode { get; set; }
        public int svm_district_id { get; set; }
        public bool svm_active_status { get; set; }
        public DateTime svm_creation_datetime { get; set; }
        public long svm_created_by { get; set; }
        public DateTime svm_updation_datetime { get; set; }
        public long svm_updated_by { get; set; }
    }
}
