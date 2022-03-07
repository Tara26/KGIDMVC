using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public class LoanPolicyMappingBll:ILoanPolicyMappingBll
    {
        private readonly ILoanPolicyMappingDll _ILoanPolicyMappingDll;
        public LoanPolicyMappingBll()
        {
            this._ILoanPolicyMappingDll = new LoanPolicyMappingDll();
        }
        public IEnumerable<tbl_loan_policy_mapping> ListAll()
        {
            var result = _ILoanPolicyMappingDll.ListAll();
            return result;
        }
    }
}
