using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDLoan
{
    public class tbl_loan_delegation_master
    {
        [Key]
        public int ld_loan_delegation_id { get; set; }
        public int? ld_category_id { get; set; }
        public int? ld_min_amount { get; set; }
        public int? ld_max_amount { get; set; }
        public bool? ld_active { get; set; }
        public DateTime? ld_creation_datetime { get; set; }
        public int? ld_created_by { get; set; }
        public DateTime? ld_updation_datetime { get; set; }
        public int? ld_updated_by { get; set; }
    }
}
