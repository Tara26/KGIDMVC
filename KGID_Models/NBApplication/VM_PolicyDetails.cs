using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.NBApplication
{
    public class VM_PolicyDetails
    {
        public long employee_id { get; set; }
        public long application_id { get; set; }

        public long p_policy_id { get; set; }
        public long p_kgid_policy_number { get; set; }
        public long p_emp_id { get; set; }
        public string p_sanction_date { get; set; }
        public Nullable<double> p_premium { get; set; }
        public int p_load_factor_id { get; set; }
        public int p_dl_factor_id { get; set; }
        public long p_application_id { get; set; }

        public int payscale_id { get; set; }
        public Nullable<decimal> payscale_minimum { get; set; }
        public Nullable<decimal> payscale_maximum { get; set; }
        public Nullable<decimal> payscale_average { get; set; }
        public Nullable<int> payscale_status { get; set; }

        public decimal premium_Amount { get; set; }
        public decimal premium_Amount_to_Pay { get; set; }

        public decimal gross_pay { get; set; }
        public decimal emp_gross_pay { get; set; }
        public IList<VM_PolicyDetails> KGIDPolicyList { get; set; }
    }
}
