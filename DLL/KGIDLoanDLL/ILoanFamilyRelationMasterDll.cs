using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace DLL.KGIDLoanDLL
{
    public interface ILoanFamilyRelationMasterDll
    {
        IEnumerable<tbl_loan_family_relation_master> ListAll();
    }
}
