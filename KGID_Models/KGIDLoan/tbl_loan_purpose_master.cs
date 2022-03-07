using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDLoan
{
    public class tbl_loan_purpose_master
    {
        [Key]
        public int lp_loan_purpose_id { get; set; }
        public string lp_purpose_desc { get; set; }
        public bool? lp_active { get; set; }
        public DateTime? lp_creation_datetime { get; set; }
        public int? lp_created_by { get; set; }
        public DateTime? lp_updation_datetime { get; set; }
        public int? lp_updated_by { get; set; }
    }
}
