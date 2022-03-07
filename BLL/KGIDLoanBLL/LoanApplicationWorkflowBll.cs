using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public class LoanApplicationWorkflowBll:ILoanApplicationWorkflowBll
    {
        private readonly ILoanApplicationWorkflowDll _ILoanApplicationWorkflowDll;
        public LoanApplicationWorkflowBll()
        {
            this._ILoanApplicationWorkflowDll = new LoanApplicationWorkflowDll();
        }
        public IEnumerable<tbl_loan_application_workflow> ListAll()
        {
            var result = _ILoanApplicationWorkflowDll.ListAll();
            return result;
        }
    }
}
