using DLL.KGIDLoanDLL;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;

namespace BLL.KGIDLoanBLL
{
    public class HrmsPayDetailsMasterBll:IHrmsPayDetailsMasterBll
    {
        private readonly IHrmsPayDetailsMasterDll _IHrmsPayDetailsMasterDll;
        public HrmsPayDetailsMasterBll()
        {
            this._IHrmsPayDetailsMasterDll = new HrmsPayDetailsMasterDll();
        }
        public IEnumerable<tbl_hrms_pay_details_master> ListAll()
        {
            var result = _IHrmsPayDetailsMasterDll.ListAll();
            return result;
        }
    }
}
