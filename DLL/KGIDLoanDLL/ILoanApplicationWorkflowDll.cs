using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace DLL.KGIDLoanDLL
{
    public interface ILoanApplicationWorkflowDll
    {
        IEnumerable<tbl_loan_application_workflow> ListAll();
    }
}
