using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_claim_remarks
    {
        [Key]
        public int remark_id { get; set; }
        public string remark_desc { get; set; }
        public int moduleType { get; set; }

    }
}
