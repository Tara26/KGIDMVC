using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_receipt_purpose_master
    {
        [Key]
        public int purpose_id { get; set; }
        public int purpose_code { get; set; }
        public string purpose_desc { get; set; }
        public int hoa_id { get; set; }
        public int receipt_type_id { get; set; }
    }
    
}
