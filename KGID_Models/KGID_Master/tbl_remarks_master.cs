using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_remarks_master
    {
        [Key]
        public int RM_Remarks_id { get; set; }
        public string RM_Remarks_Desc { get; set; }
        public string RM_Comments { get; set; }
        public int RM_Module_Type { get; set; }
        public bool RM_Active_Status { get; set; }
    }
}
