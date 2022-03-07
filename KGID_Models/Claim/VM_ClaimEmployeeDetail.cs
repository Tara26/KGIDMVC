using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.Claim;

namespace KGID_Models.Claim
{
    public class VM_ClaimEmployeeDetail
    {
        public VM_ClaimEmployeeDetail()
        {
            ClaimDetails = new List<VM_ClaimDetail>();
            WorkFlowDetails = new List<VM_ClaimWorkflowDetail>();
            ClaimDocuments = new List<VM_ClaimDocument>();
        }

      
        public long EmpCode { get; set; }
        public long ApplicationRefNo { get; set; }
        public long ApplicationId { get; set; }
        public string EmpName { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpDepartment { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public int ApplicationStatus { get; set; }
        public bool VerifyProposerDetails { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public IList<VM_ClaimDetail> ClaimDetails { get; set; }
        public IList<VM_ClaimWorkflowDetail> WorkFlowDetails { get; set; }
        public IList<VM_ClaimDocument> ClaimDocuments { get; set; }
    }

    public class VM_ClaimDetail
    {
        public string PolicyNumber { get; set; }
        public decimal SumAssured { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal UnpaidPolicyPremium { get; set; }
        public decimal UnpaidLoanPremium { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public bool IsBondReceived { get; set; }
        public bool IsPolicyActive { get; set; }
    }

    public class VM_ClaimDocument
    {
        public string DocumentType { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentFileName { get; set; }
    }
}
