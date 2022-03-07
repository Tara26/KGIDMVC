using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public class LoanFamilyRelationMasterBll:ILoanFamilyRelationMasterBll
    {
        private readonly ILoanFamilyRelationMasterDll _ILoanFamilyRelationMasterDll;
        public LoanFamilyRelationMasterBll()
        {
            this._ILoanFamilyRelationMasterDll = new LoanFamilyRelationMasterDll();
        }
        public IEnumerable<tbl_loan_family_relation_master> ListAll()
        {
            var result = _ILoanFamilyRelationMasterDll.ListAll();
            return result;
        }
    }
}
