using KGID_Models.KGIDLoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDLoanBLL
{
    public interface IBranchAdjustmentLoanBll
    {
        IEnumerable<tbl_branch_adjustment_loan> ListAll();

        int GetDueAmount(long empId);

        IEnumerable<VM_LoanDueList> GetDueList(long empId);
    }
}
