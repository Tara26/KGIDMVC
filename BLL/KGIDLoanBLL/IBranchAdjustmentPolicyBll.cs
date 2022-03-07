using KGID_Models.KGIDLoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDLoanBLL
{
    public interface IBranchAdjustmentPolicyBll
    {
        IEnumerable<tbl_branch_adjustment_policy> ListAll();
    }
}
