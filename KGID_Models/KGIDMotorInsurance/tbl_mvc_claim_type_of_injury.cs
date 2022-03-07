using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_claim_type_of_injury
    {
        [Key]
        public int injury_type_id { get; set; }
        public string injury_type_desc { get; set; }
    }
}
