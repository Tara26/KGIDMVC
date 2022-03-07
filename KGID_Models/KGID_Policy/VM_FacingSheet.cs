using KGID_Models.KGID_Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Policy
{
    public class VM_FacingSheet
    {
        public VM_FacingSheet()
        {
            Policies = new List<string>();
        }

        public string InsurerName { get; set; }

        public List<string> Policies { get; set; }

        public string ApplicationNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string FirstKGIDNumber { get; set; }
        public string DistrictInsuranceOfficeAddress { get; set; }
        public string ChallanRefNo { get; set; }
        public string ChallanDate { get; set; }

        public decimal InitialDeposit { get; set; }
        public decimal EstimatedPremium { get; set; }
        public string DateOfLiability { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Age { get; set; }
        public decimal MonthlyPremium { get; set; }
        public decimal? InsuranceAmount { get; set; }
        public string EffectiveMonthYear { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public string LoadFactor { get; set; }

        public string DLFactor { get; set; }

        public decimal SumAssured { get; set; }

        /////Verification Details--added by venkatesh
        public string DDOName { get; set; }
        public string DDOVDate { get; set; }

        public string CWName { get; set; }
        public string CWVDate { get; set; }

        public string SIName { get; set; }
        public string SIVDate { get; set; }

        public string DIOName { get; set; }
        public string DIOVDate { get; set; }

        public string DDName { get; set; }
        public string DDVDate { get; set; }

        public string DName { get; set; }
        public string DVDate { get; set; }
    }
}
