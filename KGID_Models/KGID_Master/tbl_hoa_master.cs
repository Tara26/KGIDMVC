using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_hoa_master
    {
        [Key]
        public int hoa_id { get; set; }
        public int hoa { get; set; }
        public string hoa_desc { get; set; }
    }
}
