using KGID_Models.KGIDLoan;
using System.Collections.Generic;


namespace BLL.KGIDLoanBLL
{
    public interface ILoanApplicationBll
    {
        VM_LoanApplicationModel GetLoanApplicationData(long empId,long loanApplicationId);

        bool LoanApplicationForward(VM_LoanWorkFlow loanApplicaitonForward);

        VM_LoanVerificationDetails GetLoanApplicationListForAll(long empId,int UserRole);

        VM_LoanVerificationDetails GetLoanCancellationListForAll(long empId,string Type);

        int NBLoanCancelRequestAction(long AppId, long EmpId, string Action);

        int GetLoanDisburseMailData(long LoanAppID,string Type);
    }
}
