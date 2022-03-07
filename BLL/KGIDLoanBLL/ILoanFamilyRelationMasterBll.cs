using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public interface ILoanFamilyRelationMasterBll
    {
        IEnumerable<tbl_loan_family_relation_master> ListAll();
    }
}
