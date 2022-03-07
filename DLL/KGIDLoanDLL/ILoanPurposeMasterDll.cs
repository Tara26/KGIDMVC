using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace DLL.KGIDLoanDLL
{
    public interface ILoanPurposeMasterDll
    {
        IEnumerable<tbl_loan_purpose_master> ListAll();
    }
}
