using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace DLL.KGIDLoanDLL
{
    public interface ILoanDelegationMasterDll
    {
        IEnumerable<tbl_loan_delegation_master> ListAll();
    }
}
