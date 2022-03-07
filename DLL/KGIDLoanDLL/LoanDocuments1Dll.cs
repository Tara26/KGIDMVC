using DLL.DBConnection;
using KGID_Models.KGID_Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDLoanDLL
{
    public class LoanDocumentsDll:ILoanDocumentsDll
    {
        private readonly DbConnection _db = new DbConnection();

        public IEnumerable<tbl_loan_documents> ListAll()
        {
            return (from loanDocuments in _db.tbl_loan_documents
                    select loanDocuments).ToList();
        }

        public tbl_loan_documents FindById(long loanReferenceId)
        {
            return (from loanMaster in _db.tbl_loan_documents
                    where loanMaster.ld_loan_id == loanReferenceId
                    select loanMaster).FirstOrDefault();
        }
    }
}
