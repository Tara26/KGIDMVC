using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDLoanBLL
{
    public class BranchAdjustmentLoanBll:IBranchAdjustmentLoanBll
    {
        private readonly IBranchAdjustmentLoanDll _IBranchAdjustmentLoanDll;
        public BranchAdjustmentLoanBll()
        {
            this._IBranchAdjustmentLoanDll = new BranchAdjustmentLoanDll();
        }
        public IEnumerable<tbl_branch_adjustment_loan> ListAll()
        {
            var result = _IBranchAdjustmentLoanDll.ListAll();
            return result;
        }
       public int GetDueAmount(long empId)
        {
            var result = _IBranchAdjustmentLoanDll.GetDueAmount(empId);
            return result;
        }
       public IEnumerable<VM_LoanDueList> GetDueList(long empId)
        {
            var result = _IBranchAdjustmentLoanDll.GetDueList(empId);
            return result;
        }
    }
}
