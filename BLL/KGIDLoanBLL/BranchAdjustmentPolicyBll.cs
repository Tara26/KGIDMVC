using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDLoanBLL
{
    public class BranchAdjustmentPolicyBll:IBranchAdjustmentPolicyBll
    {
        private readonly IBranchAdjustmentPolicyDll _IIBranchAdjustmentPolicyDll;
        public BranchAdjustmentPolicyBll()
        {
            this._IIBranchAdjustmentPolicyDll = new BranchAdjustmentPolicyDll();
        }
        public IEnumerable<tbl_branch_adjustment_policy> ListAll()
        {
            var result = _IIBranchAdjustmentPolicyDll.ListAll();
            return result;
        }
    }
}
