using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDLoan
{
    public class tbl_loan_extraction_details
    {
        [Key]
        public long let_loan_extraction_id { get; set; }
        public long? let_loan_ref_no { get; set; }
        public int? let_applied_amount { get; set; }
        public int? let_loan_deduction { get; set; }
        public int? let_principle_months { get; set; }
        public int? let_interest_months { get; set; }
        public bool let_active { get; set; }
        public DateTime let_creation_datetime { get; set; }
        public int let_created_by { get; set; }
        public DateTime let_updation_datetime { get; set; }
        public int let_updated_by { get; set; }

    }
}
