using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public class LoanDocumentsBll:ILoanDocumentsBll
    {
        private readonly ILoanDocumentsDll _ILoanDocumentsDll;
        public LoanDocumentsBll()
        {
            this._ILoanDocumentsDll = new LoanDocumentsDll();
        }
        public IEnumerable<tbl_loan_documents> ListAll()
        {
            var result = _ILoanDocumentsDll.ListAll();
            return result;
        }
    }
}
