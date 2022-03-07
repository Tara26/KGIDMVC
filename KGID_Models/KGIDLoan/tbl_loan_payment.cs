using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDLoan
{
    public class tbl_loan_payment
    {
        [Key]
        public long lp_loan_payment_id { get; set; }
        public long? lp_loan_extraction_id { get; set; }
        public int? lp_year_id { get; set; }
        public int? lp_month_id { get; set; }
        public int? lp_payment_due{ get; set; }
        public int? lp_amount_paid { get; set; }
        public DateTime? lp_date_of_payment { get; set; }
        public string lp_receipt_reference_no { get; set; }
        public bool? lp_payment_status { get; set; }
        public bool lp_active { get; set; }
        public DateTime lp_creation_datetime { get; set; }
        public int lp_created_by { get; set; }
        public DateTime lp_updation_datetime { get; set; }
        public int lp_updated_by { get; set; }

    }
}
