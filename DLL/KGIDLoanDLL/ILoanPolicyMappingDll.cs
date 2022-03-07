using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace DLL.KGIDLoanDLL
{
    public interface ILoanPolicyMappingDll
    {
        IEnumerable<tbl_loan_policy_mapping> ListAll();
    }
}
