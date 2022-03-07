using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using DLL.DBConnection;
using KGID_Models.KGID_VerifyData;

namespace DLL.DistrictMasterDLL
{
    public class DistrictMasterDLL:IDistrictMasterDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        tbl_district_master distmaster;
        public IEnumerable<tbl_district_master> DistrictMasterdll()
        {
            var distMaster = (from n in _db.tbl_district_master
                              select n).ToList();
            return distMaster;
        }
    }
}
