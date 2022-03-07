using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGID_Loan
{
    public class tbl_employee_loan_transaction
    {
        [Key]
        public int Loan_Id { get; set; }
        public string Loan_Policy_Number { get; set; }
        [Required(ErrorMessage = "Please Enter Gross Loan Amount")]
        [DisplayName("Gross Loan Amount")]
        public Nullable<decimal> Loan_Gross_Loan_Amount { get; set; }
        [Required(ErrorMessage = "Please Enter Premium")]
        [DisplayName("Premium")]
        public Nullable<decimal> Loan_Premium { get; set; }
        //[Required(ErrorMessage = "Please Enter Sanctioned Date")]
        //[DisplayName("Sanctioned Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public string Loan_Sanctioned_Date { get; set; }
        [Required(ErrorMessage = "Please Enter Premium Interest")]
        [DisplayName("Premium Interest")]
        public Nullable<decimal> Loan_Premium_Interest { get; set; }
        public Nullable<decimal> Loan_Deduction { get; set; }
        [Required(ErrorMessage = "Please Enter Loan Amount")]
        [DisplayName("Loan Amount")]
        public Nullable<decimal> Loan_Amount { get; set; }
        public Nullable<decimal> Loan_Net_Amount { get; set; }
        public Nullable<decimal> Loan_Instalment_Amount { get; set; }
        [Required(ErrorMessage = "Please Enter Interest")]
        [DisplayName("Interest")]
        public Nullable<decimal> Loan_Interest { get; set; }
        [Required(ErrorMessage = "Please Enter Period")]
        [DisplayName("Loan Period")]
        [Range(10, 40)]
        public Nullable<int> Loan_Period { get; set; }
        [Required(ErrorMessage = "Please Enter Others")]
        [DisplayName("Others")]
        public Nullable<decimal> Loan_Others { get; set; }
        public Nullable<DateTime> Loan_CreatedDate { get; set; }
        public Nullable<DateTime> Loan_UpdatedDate { get; set; }
        public Nullable<int> Loan_dist_id { get; set; }
    }
}
