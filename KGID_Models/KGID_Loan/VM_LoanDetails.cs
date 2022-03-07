using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGID_Models.KGID_Loan
{
    public class VM_LoanDetails
    {
        public VM_LoanDetails()
        {
            NetAmount = Decimal.Parse("0.00");
            BranchAdjustment = Decimal.Parse("0.00");
            InstalmentAmount = Decimal.Parse("0.00");
            Period = 40;
            Districts = new List<SelectListItem>();
            GrossLoanAmount = Decimal.Parse("0.00");
            Premium = Decimal.Parse("0.00");
            PremiumInterest = Decimal.Parse("0.00");
            Amount = Decimal.Parse("0.00");
            Interest = Decimal.Parse("0.00");
            Others = Decimal.Parse("0.00");
        }

        public int Id { get; set; }

        public string PolicyNumber { get; set; }
        public List<SelectListItem> Districts { get; set; }
        public string DistrictName { get; set; }

        [Required(ErrorMessage = "Please select district")]
        public int? DistrictId { get; set; }

        [Required(ErrorMessage = "Please Enter Gross Loan Amount")]
        [DisplayName("Gross Loan Amount")]
        public decimal? GrossLoanAmount { get; set; }

        [Required(ErrorMessage = "Please Enter Premium")]
        [DisplayName("Premium")]
        public decimal? Premium { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public string SanctionedDate { get; set; }

        [Required(ErrorMessage = "Please Enter Premium Interest")]
        [DisplayName("Premium Interest")]
        public decimal? PremiumInterest { get; set; }

        public decimal? BranchAdjustment { get; set; }

        [Required(ErrorMessage = "Please Enter Loan Amount")]
        [DisplayName("Loan Amount")]
        public decimal? Amount { get; set; }

        public decimal? NetAmount { get; set; }

        public decimal? InstalmentAmount { get; set; }

        [Required(ErrorMessage = "Please Enter Interest")]
        [DisplayName("Interest")]
        public decimal? Interest { get; set; }

        [Required(ErrorMessage = "Please Enter Period")]
        [DisplayName("Loan Period")]
        [Range(10, 40)]
        public int? Period { get; set; }

        [Required(ErrorMessage = "Please Enter Others")]
        [DisplayName("Others")]
        public decimal? Others { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int LoanId { get; set; }
    }

    public class VM_EmployeeDetails
    {
        public VM_EmployeeDetails()
        {
        }

        [Required(ErrorMessage = "Please Enter Policy Number")]
        [DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }
        public string InsuredName { get; set; }
        public long? EmployeeNumber { get; set; }
        public string HRMSName { get; set; }
        public long? FirstPolicyNumber { get; set; }
    }

    public class VM_SubsequentPolicyDetails
    {
        public VM_SubsequentPolicyDetails()
        {
            SubsquentPolicies = new List<VM_LoanDetails>();
            EmployeeDetails = new VM_EmployeeDetails();
        }

        public List<VM_LoanDetails> SubsquentPolicies { get; set; }
        public VM_EmployeeDetails EmployeeDetails { get; set; }
    }
}
