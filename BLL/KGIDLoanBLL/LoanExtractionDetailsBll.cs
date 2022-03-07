using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public class LoanExtractionDetailsBll:ILoanExtractionDetailsBll
    {
        private readonly ILoanExtractionDetailsDll _ILoanExtractionDetailsDll;
        public LoanExtractionDetailsBll()
        {
            this._ILoanExtractionDetailsDll = new LoanExtractionDetailsDll();
        }
        public IEnumerable<tbl_loan_extraction_details> ListAll()
        {
            var result = _ILoanExtractionDetailsDll.ListAll();
            return result;
        }
    }
}
