using KGID_Models.KGID_Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.KGIDLoanBLL
{
    public interface IKGIDLoanDetailsBll
    {
        VM_EmployeeDetails GetEmployeeDetails(string policyNumber);
        VM_LoanDetails GetEmployeeLoanDetails(int loanId);
        int SaveLoanDetailsBll(VM_LoanDetails _LoanDetails);
        List<SelectListItem> GetDestricts();
        VM_SubsequentPolicyDetails GetSubsequentPolicyDetails(string policyNumber);
    }
}
