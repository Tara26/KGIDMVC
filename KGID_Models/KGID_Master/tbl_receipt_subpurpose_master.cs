using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_receipt_subpurpose_master
    {
        [Key]
        public int sub_purpose_id { get; set; }
        public int sub_purpose_code { get; set; }
        public string sub_purpose_desc { get; set; }
        public int purpose_id { get; set; }
    }
}
