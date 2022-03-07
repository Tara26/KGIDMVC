using DLL.NBLoanDLL;
using KGID_Models.KGID_Loan;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BLL.NBLoanBLL
{
    public class NBLoanBLL : INBLoanBLL
    {
        private readonly INBLoanDLL _INBLoanDLL;
        public NBLoanBLL()
        {
            this._INBLoanDLL = new NBLoanDLL();
        }
        public IEnumerable<SelectListItem> GetLoanFamilyRelationList()
        {
            return _INBLoanDLL.GetLoanFamilyRelationList();
        }
        public IEnumerable<SelectListItem> GetLoanPurposeList()
        {
            return _INBLoanDLL.GetLoanPurposeList();
        }
        public VM_LoanApplicationForm GetLoanApplicationForm(long EmpID)
        {
            return _INBLoanDLL.GetLoanApplicationForm(EmpID);
        }
        public List<VM_LoanBranchAdjustments> GetLoanBADetails(long EmpID)
        {
            return _INBLoanDLL.GetLoanBADetails(EmpID);
        }
        public int SaveLoanApplication(VM_LoanApplicationForm loanApp)
        {
            return _INBLoanDLL.SaveLoanApplication(loanApp);
        }
        public string GetSpouseKgidNumber(long empId)
        {
            return _INBLoanDLL.GetSpouseKgidNumber(empId);
        }
        public int UploadPayslip(VM_LoanApplicationForm model)
        {
            return _INBLoanDLL.UploadPayslip(model);
        }
        public VM_LoanApplicationStatus GetLoanApplicationStaus(long EmpID)
        {
            return _INBLoanDLL.GetLoanApplicationStaus(EmpID);
        }
        public int CancelLoanApplication(VM_ApplicationStatus loanApplicationCancel)
        {
            return _INBLoanDLL.CancelLoanApplication(loanApplicationCancel);
        }
    }
}
