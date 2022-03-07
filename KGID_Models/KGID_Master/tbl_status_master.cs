using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_status_master
    {
        [Key]
        public int sm_id { get; set; }
        public string sm_description { get; set; }
    }
}
