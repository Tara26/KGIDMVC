using DLL.DistrictMasterDLL;
using KGID_Models.KGID_VerifyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DistrictMasterBLL
{
    public class DistrictMasterBLL:IDistrictMasterBLL
    {
        private readonly IDistrictMasterDLL _IDistrictMasterdll;
        public DistrictMasterBLL()
        {
            this._IDistrictMasterdll = new DistrictMasterDLL();
        }
        public IEnumerable<tbl_district_master> DistrictMasterbll()
        {
            var result = _IDistrictMasterdll.DistrictMasterdll();
            return result.OrderBy(t => t.dm_name_english);
        }
    }
}
