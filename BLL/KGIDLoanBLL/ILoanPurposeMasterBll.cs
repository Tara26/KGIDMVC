using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public interface ILoanPurposeMasterBll
    {
        IEnumerable<tbl_loan_purpose_master> ListAll();
    }
}
