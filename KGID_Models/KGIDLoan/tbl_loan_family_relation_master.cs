using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDLoan
{
    public class tbl_loan_family_relation_master
    {
        [Key]
        public int lfr_loan_relation_id { get; set; }
        public string lfr_loan_relation_desc { get; set; }
        public bool? lfr_active { get; set; }
        public DateTime? lfr_creation_datetime { get; set; }
        public int? lfr_created_by { get; set; }
        public DateTime? lfr_updation_datetime { get; set; }
        public int? lfr_updated_by { get; set; }
    }
}
