using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace DLL.KGIDLoanDLL
{
   public interface IHrmsPayDetailsMasterDll
    {
        IEnumerable<tbl_hrms_pay_details_master> ListAll();
    }
}
