using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace DLL.KGIDLoanDLL
{
    public interface ILoanApplicationDll
    {
     

        VM_LoanApplicationModel GetLoanApplicationData(long empId, long loanApplicationId);

        bool LoanApplicationForward(VM_LoanWorkFlow loanApplicationForward);
        #region 
        VM_LoanVerificationDetails GetLoanApplicationListForAll(long empId, int UserRole);
        #endregion
        VM_LoanVerificationDetails GetLoanCancellationListForAll(long empId, string Type);

        int NBLoanCancelRequestAction(long AppId, long EmpId, string Action);

        int GetLoanDisburseMailData(long AppId,string Type);
    }
}
