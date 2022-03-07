using KGID_Models.KGID_Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDLoanDLL
{
    public interface ILoanDetailsDll
    {
        VM_LoanDDO GetLoanDetailsByLoanId(long loanReferenceId);
        
        List<VM_LoanVerificationDetails> GetDetailsForLoanVerification(long EmpCode, string LoanVerifier);
    }
}
