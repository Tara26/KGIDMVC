using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KGID_Models.KGIDEmployee
{
    public class tbl_payment_status_details
    {
        [Key]
        [ConcurrencyCheck]
        public int psd_status_id { get; set; }
        [ConcurrencyCheck]
        public long? psd_system_emp_code { get; set; }
        [ConcurrencyCheck]
        public string psd_challan_ref_no { get; set; }
        [ConcurrencyCheck]
        public string psd_status_of_payment { get; set; }
        [ConcurrencyCheck]
        public Nullable<System.DateTime> psd_datetime { get; set; }
        [ConcurrencyCheck]
        public string  psd_transaction_number { get; set; }
        [ConcurrencyCheck]
        public byte? psd_status { get; set; }
        [ConcurrencyCheck]
        public Nullable<System.DateTime> psd_creation_datetime { get; set; }
        [ConcurrencyCheck]
        public Nullable<System.DateTime> psd_updation_datetime { get; set; }
        [ConcurrencyCheck]
        public int? psd_created_by { get; set; }
        [ConcurrencyCheck]
        public int? psd_updated_by { get; set; }
    }
}
