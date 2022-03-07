using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public interface ILoanPolicyMappingBll
    {
        IEnumerable<tbl_loan_policy_mapping> ListAll();
    }
}
