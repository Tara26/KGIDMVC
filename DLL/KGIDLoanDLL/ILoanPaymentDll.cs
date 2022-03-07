using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace DLL.KGIDLoanDLL
{
   public interface ILoanPaymentDll
    {
        IEnumerable<tbl_loan_payment> ListAll();
    }
}
