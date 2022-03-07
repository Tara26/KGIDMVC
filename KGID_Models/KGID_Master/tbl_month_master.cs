using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
   public class tbl_month_master
    {
        [Key]
        public int mm_month_id { get; set; }
        public string mm_month_desc { get; set; }
        //public bool mm_active_status { get; set; }

    }
}
