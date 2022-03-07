using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDLoan
{
    public class tbl_loan_application
    {
        [Key]
        public long la_loan_application_id { get; set; }
        public string la_loan_ref_no { get; set; }
        public long? la_emp_id { get; set; }
        public int? la_loan_purpose_id { get; set; }
        public DateTime? la_date_of_application { get; set; }
        public int? la_loan_relation_id { get; set; }
        public bool la_active { get; set; }
        public DateTime la_creation_datetime { get; set; }
        public int la_created_by { get; set; }
        public DateTime la_updation_datetime { get; set; }
        public int la_updated_by { get; set; }
    }
}
