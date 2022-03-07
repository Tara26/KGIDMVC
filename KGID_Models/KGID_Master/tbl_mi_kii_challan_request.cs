using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_mi_kii_challan_request
    {
        [Key]
        public long mi_kii_challan_id { get; set; }
        public string mi_kii_challan_ref_no { get; set; }
        public string mi_kii_challan_amount { get; set; }
        public string mi_kii_challan_app_ref_no { get; set; }
        public long? mi_kii_proposer_id { get; set; }
        public string mi_kii_user_category { get; set; }
        public string mi_kii_response_status_code { get; set; }
        public string mi_kii_response_status { get; set; }
        public string mi_kii_bank_transaction_no { get; set; }
        public string mi_kii_transaction_date { get; set; }
        public string mi_kii_payment_mode { get; set; }
        public string mi_kii_bank_name { get; set; }
        public bool mi_kii_challan_active_status { get; set; }
        public bool mi_kii_payment_status { get; set; }
        public Nullable<System.DateTime> mi_kii_creation_datetime { get; set; }
        public Nullable<System.DateTime> mi_kii_updation_datetime { get; set; }
        public long mi_kii_created_by { get; set; }
        public long mi_kii_updated_by { get; set; }
    }
}
