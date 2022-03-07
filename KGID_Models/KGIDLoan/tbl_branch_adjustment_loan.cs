using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDLoan
{
    public class tbl_branch_adjustment_loan
    {
        public long bal_branch_loan_id { get; set; }
        public long bal_emp_id { get; set; }
        public long bal_loan_application_id { get; set; }
        public int bal_month_id { get; set; }
        public int bal_year_id { get; set; }
        public int bal_amount { get; set; }
        public int bal_penal_interest { get; set; }
        public bool bal_active { get; set; }
        public DateTime bal_creation_datetime { get; set; }
        public int bal_created_by { get; set; }
        public DateTime bal_updation_datetime { get; set; }
        public int bal_updated_by { get; set; }

    }
}
