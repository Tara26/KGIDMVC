using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace DLL.KGIDLoanDLL
{
    public interface ILoanDocumentsDll
    {
        IEnumerable<tbl_loan_documents> ListAll();
    }
}
