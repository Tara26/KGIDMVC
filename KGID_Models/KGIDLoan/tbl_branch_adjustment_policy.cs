using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDLoan
{
   public class tbl_branch_adjustment_policy
    {
        public long bap_branch_policy_id { get; set; }
        public long bap_emp_id { get; set; }
        public long bap_policy_id { get; set; }
        public int bap_month_id { get; set; }
        public int bap_year_id { get; set; }
        public int bap_premium { get; set; }
        public int bap_penal_interest { get; set; }
        public bool bap_active { get; set; }
        public DateTime bap_creation_datetime { get; set; }
        public int bap_created_by { get; set; }
        public DateTime bap_updation_datetime { get; set; }
        public int bap_updated_by { get; set; }
    }
}
