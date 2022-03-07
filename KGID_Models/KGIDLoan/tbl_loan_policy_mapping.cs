using System;

namespace KGID_Models.KGIDLoan
{
   public class tbl_loan_policy_mapping
    {
        public long lpm_loan_policy_id { get; set; }
        public long? lpm_loan_extraction_id { get; set; }
        public long? lpm_policy_id { get; set; }
        public int? lpm_applied_amount { get; set; }
        public bool lpm_active { get; set; }
        public DateTime lpm_creation_datetime { get; set; }
        public int lpm_lpm_created_by { get; set; }
        public DateTime lpm_updation_datetime { get; set; }
        public int lpm_updated_by { get; set; }

    }
}
