using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public interface ILoanDelegationMasterBll
    {
        IEnumerable<tbl_loan_delegation_master> ListAll();
    }
}
