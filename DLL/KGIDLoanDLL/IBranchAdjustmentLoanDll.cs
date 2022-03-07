using KGID_Models.KGIDLoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDLoanDLL
{
    public interface IBranchAdjustmentLoanDll
    {
        IEnumerable<tbl_branch_adjustment_loan> ListAll();

        int GetDueAmount(long emp_id);

        IEnumerable<VM_LoanDueList> GetDueList(long empId);
    }
}
