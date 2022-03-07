using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_MB_Claim
{
    public class tbl_repairer_master
    {
        [Key]
        public long rep_id { get; set; }
        public string rep_name { get; set; }
        public string rep_contact_person { get; set; }
        public string rep_address { get; set; }
        public string rep_email { get; set; }
        public string rep_mobile { get; set; }
        public string rep_gstin { get; set; }
        public string rep_pan { get; set; }
        public bool rep_status { get; set; }
        public DateTime rep_creation_datetime { get; set; }
        public int rep_created_by { get; set; }
        public DateTime rep_updation_datetime { get; set; }
        public int rep_updated_by { get; set; }
    }
}
