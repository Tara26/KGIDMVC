using KGID_Models.KGID_VerifyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DistrictMasterBLL
{
   public interface IDistrictMasterBLL
    {
        IEnumerable<tbl_district_master> DistrictMasterbll();
    }
}
