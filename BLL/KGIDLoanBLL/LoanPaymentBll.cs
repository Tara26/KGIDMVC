using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public class LoanPaymentBll:ILoanPaymentBll
    {
        private readonly ILoanPaymentDll _ILoanPaymentDll;
        public LoanPaymentBll()
        {
            this._ILoanPaymentDll = new LoanPaymentDll();
        }
        public IEnumerable<tbl_loan_payment> ListAll()
        {
            var result = _ILoanPaymentDll.ListAll();
            return result;
        }
    }
}
