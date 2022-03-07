using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public interface ILoanDocumentsBll
    {
        IEnumerable<tbl_loan_documents> ListAll();
    }
}
