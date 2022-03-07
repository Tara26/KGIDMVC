using KGID_Models.KGID_Loan;
using KGID_Models.KGID_VerifyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDLoanDLL
{
    public interface IKGIDLoanDetailsDll
    {
        VM_LoanDetails GetLoanDetailsDll(int loanId);
        int SaveLoanDetailsDll(VM_LoanDetails _LoanTDetails);
        List<tbl_district_master> GetDistricts();
        VM_SubsequentPolicyDetails GetSubsequentPolicyDetails(string policyNumber);
        VM_EmployeeDetails GetEmployeeDetails(string policyNumber);
    }
}
