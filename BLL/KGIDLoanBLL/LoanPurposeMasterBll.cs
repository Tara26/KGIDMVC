using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public class LoanPurposeMasterBll : ILoanPurposeMasterBll
    {
        private readonly ILoanPurposeMasterDll _ILoanPurposeMasterDll;
        public LoanPurposeMasterBll()
        {
            this._ILoanPurposeMasterDll = new LoanPurposeMasterDll();
        }
        public IEnumerable<tbl_loan_purpose_master> ListAll()
        {
            var result = _ILoanPurposeMasterDll.ListAll();
            return result;
        }
    }
}
