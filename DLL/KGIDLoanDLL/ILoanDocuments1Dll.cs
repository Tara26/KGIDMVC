using KGID_Models.KGID_Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDLoanDLL
{
    public interface ILoanDocumentsDll
    {
        IEnumerable<tbl_loan_documents> ListAll();

        tbl_loan_documents FindById(long loanReferenceId);
    }
}
