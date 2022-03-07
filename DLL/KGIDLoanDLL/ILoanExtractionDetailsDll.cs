using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace DLL.KGIDLoanDLL
{
    public interface ILoanExtractionDetailsDll
    {
        IEnumerable<tbl_loan_extraction_details> ListAll();
    }
}
