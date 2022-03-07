using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public interface ILoanExtractionDetailsBll
    {
        IEnumerable<tbl_loan_extraction_details> ListAll();
    }
}
