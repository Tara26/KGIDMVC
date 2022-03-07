using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Policy
{
    public class VM_InitimationLetter
    {
        public string DDOAddress { get; set; }
        public string DIOAddress { get; set; }
        public string KGIDNumber { get; set; }
        public string NameOfPolicyHolder { get; set; }
        public string NameOfPolicyHolderKan { get; set; }
        public string NameOfFatherKan { get; set; }
        public string Designation { get; set; }
        public DateTime RiskDate { get; set; }
        public decimal SumAssured { get; set; }
        public decimal MonthlyPremium { get; set; }
        public decimal TwoMonthPremium { get; set; }
        public DateTime DateOfMaturity { get; set; }
        public string CommencementMonth { get; set; }
        public string FourthMonth { get; set; }
        public string ParentName { get; set; }
        public long VerifiedByEmpId { get; set; }
        public DateTime VerifiedDate { get; set; }
        public string VerifiedName { get; set; }

        public string ApprovedMonth { get; set; }
        public string FutureMonth { get; set; }

        public string SVerifiedDate { get; set; }
        public string SRiskDate { get; set; }
        public string SDateOfMaturity { get; set; }
        public string ProposalSubmissionDate { get; set; }
    }
}
