using System.Collections.Generic;
using System.Web;

namespace KGID_Models.KGID_Loan
{
    public class VM_LoanApplicationForm
    {
        public VM_LoanApplicationForm()
        {
            listHRMSPayDetails = new List<VM_HRMSPayDetails>();
            listPolicyDetails = new List<VM_PolicyDetails>();
        }
        public long LoanAppID { get; set; }
        public long EmpID { get; set; }
        public bool IsSpouseGovtEmp { get; set; }
        public long SpouseKGIDNumber { get; set; }
        public int PrincipalInstallments { get; set; }
        public int InterestInstallments { get; set; }
        public int PurposeID { get; set; }
        public int FamilyRelationID { get; set; }
        public string RelationName { get; set; }
        public int RelationAge { get; set; }
        public decimal NetAmount { get; set; }
        public decimal AppliedAmount { get; set; }
        public decimal Deductions { get; set; }
        public decimal MonthWisePInstallments { get; set; }
        public decimal MonthWiseIInstallments { get; set; }
        public HttpPostedFileBase Document { get; set; }
        public string DocumentFileName { get; set; }
        public string DocumentFilePath { get; set; }
        public List<VM_HRMSPayDetails> listHRMSPayDetails { get; set; }
        public List<VM_PolicyDetails> listPolicyDetails { get; set; }
    }
    public class VM_LoanBranchAdjustments
    {
        public long Policy_No { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public decimal LoanDue { get; set; }
        public decimal LoanInterestDue { get; set; }
        public decimal PremiumDue { get; set; }
        public decimal PremiumInterestDue { get; set; }
    }
    public class VM_HRMSPayDetails
    {
        public long EmpID { get; set; }
        public string PrevMonth { get; set; }
        public int CurrYear { get; set; }
        public decimal GrossPay { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
    }
    public class VM_PolicyDetails
    {
        public long EmpID { get; set; }
        public long PolicyNo { get; set; }
        public long LoanNo { get; set; }
        public decimal EligibleLoanAmount { get; set; }
        public string LoanAmount { get; set; }
        public long LoanAppID { get; set; }
        public string SanctionDate { get; set; }
        public decimal SumAssured { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal AppliedAmount { get; set; }
        public string LoanBalanceAmt { get; set; }
    }
    public class VM_LoanApplicationStatus
    {
        public VM_LoanApplicationStatus()
        {
            listApplicationStatus = new List<VM_ApplicationStatus>();
        }
        public List<VM_ApplicationStatus> listApplicationStatus { get; set; }
    }
    public class VM_ApplicationStatus
    {
        public long LoanID { get; set; }
        public long EmpID { get; set; }
        public string LoanRefNo { get; set; }
        public string EmpName { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpDepartment { get; set; }
        public string LoanStatus { get; set; }
    }
}
