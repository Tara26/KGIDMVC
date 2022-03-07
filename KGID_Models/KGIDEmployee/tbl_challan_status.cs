using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
   public class tbl_challan_status
    {
        [Key]
        public long cs_challan_status_id { get; set; }
        public long cs_challan_id { get; set; }
        public string cs_transaction_ref_no { get; set; }
        public int cs_amount { get; set; }
        public DateTime cs_date_of_transaction { get; set; }
        public long cs_status { get; set; }
        public int cs_active_status { get; set; }
        public DateTime cs_creation_datetime { get; set; }
        public DateTime cs_updation_datetime { get; set; }
        public long cs_created_by { get; set; }
        public long cs_updated_by { get; set; }
    }
}
