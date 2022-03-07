using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public interface IHrmsPayDetailsMasterBll
    {
        IEnumerable<tbl_hrms_pay_details_master> ListAll();
    }
}
