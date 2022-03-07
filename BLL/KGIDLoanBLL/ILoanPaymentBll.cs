using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public interface ILoanPaymentBll
    {
        IEnumerable<tbl_loan_payment> ListAll();
    }
}
