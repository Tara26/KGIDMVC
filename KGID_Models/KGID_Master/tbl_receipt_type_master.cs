using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_receipt_type_master
    {
        [Key]
        public int receipt_type_id { get; set; }
        public string receipt_type_desc { get; set; }
    }
}
