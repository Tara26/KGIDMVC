using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public class LoanApplicationBll : ILoanApplicationBll
    {
        private readonly ILoanApplicationDll _ILoanApplicationDll;

        public LoanApplicationBll()
        {
            this._ILoanApplicationDll = new LoanApplicationDll();
        }

        public VM_LoanApplicationModel GetLoanApplicationData(long empId, long loanApplicationId)
        {
            var result = _ILoanApplicationDll.GetLoanApplicationData(empId, loanApplicationId);
            return result;
        }

        public bool LoanApplicationForward(VM_LoanWorkFlow loanApplicaitonForward)
        {
            var result = _ILoanApplicationDll.LoanApplicationForward(loanApplicaitonForward);
            return result;
        }
        public VM_LoanVerificationDetails GetLoanApplicationListForAll(long empId, int UserRole)
        {
            return _ILoanApplicationDll.GetLoanApplicationListForAll(empId,UserRole);
            //return result;
        }
        public VM_LoanVerificationDetails GetLoanCancellationListForAll(long empId,string Type)
        {
            return _ILoanApplicationDll.GetLoanCancellationListForAll(empId, Type);
        }
        public int NBLoanCancelRequestAction(long AppId, long EmpId, string Action)
        {
            return _ILoanApplicationDll.NBLoanCancelRequestAction(AppId, EmpId, Action);
        }
        public int GetLoanDisburseMailData(long AppId,string Type)
        {
            return _ILoanApplicationDll.GetLoanDisburseMailData(AppId,Type);
        }
    }
}
