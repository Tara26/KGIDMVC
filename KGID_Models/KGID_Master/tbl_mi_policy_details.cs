using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
   public  class tbl_mi_policy_details
    {
        [Key]
        public long p_mi_policy_id { get; set; }
        public string p_mi_policy_number { get; set; }
        public long p_mi_emp_id { get; set; }
        public double? p_mi_premium { get; set; }
        //public float MIPremium { get; set; }
        public long p_mi_application_id { get; set; }
        public string p_mi_category_id { get; set; }
        public int? p_mi_liability { get; set; }

        public DateTime p_mi_tpto_date { get; set; }
        public DateTime p_mi_tpfrom_date { get; set; }

        public DateTime p_mi_from_date { get; set; }

        public DateTime? p_mi_to_date { get; set; }
    }
}
