using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
   public class LoanDelegationMasterBll:ILoanDelegationMasterBll
    {
        private readonly ILoanDelegationMasterDll _ILoanDelegationMasterDll;
        public LoanDelegationMasterBll()
        {
            this._ILoanDelegationMasterDll = new LoanDelegationMasterDll();
        }
        public IEnumerable<tbl_loan_delegation_master> ListAll()
        {
            var result = _ILoanDelegationMasterDll.ListAll();
            return result;
        }
    }
}
