using KGID_Models.KGID_Loan;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DLL.NBLoanDLL
{
    public interface INBLoanDLL
    {
        IEnumerable<SelectListItem> GetLoanFamilyRelationList();
        IEnumerable<SelectListItem> GetLoanPurposeList();
        VM_LoanApplicationForm GetLoanApplicationForm(long EmpID);
        List<VM_LoanBranchAdjustments> GetLoanBADetails(long EmpID);
        int SaveLoanApplication(VM_LoanApplicationForm loanApp);
        string GetSpouseKgidNumber(long empId);
        int UploadPayslip(VM_LoanApplicationForm model);
        VM_LoanApplicationStatus GetLoanApplicationStaus(long EmpID);
        int CancelLoanApplication(VM_ApplicationStatus loanApplicationCancel);
    }
}
