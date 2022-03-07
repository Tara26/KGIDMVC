using KGID_Models.KGID_VerifyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DBConnection;

namespace DLL.DistrictMasterDLL
{
    public interface IDistrictMasterDLL
    {
        IEnumerable<tbl_district_master> DistrictMasterdll();
    }
}
