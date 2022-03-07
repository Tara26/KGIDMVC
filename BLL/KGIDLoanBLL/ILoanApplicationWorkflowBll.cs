using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public interface ILoanApplicationWorkflowBll
    {
        IEnumerable<tbl_loan_application_workflow> ListAll();
    }
}
