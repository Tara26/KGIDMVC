using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DBConnection;
using KGID_Models.KGID_VerifyData;

namespace DLL.RangesMasterDLL
{
    public class RangesMasterDLL:IRangesMasterDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        tbl_ranges_master rangmaster;
        public IEnumerable<tbl_ranges_master> RangesMasterdll()
        {
            var rangMaster = (from n in _db.tbl_ranges_master
                              select n).ToList();
            return rangMaster;
        }
        public IEnumerable<tbl_ranges_master> GetStartRangesByDistId(int _distId)
        {
            var rangMaster = (from n in _db.tbl_ranges_master where n.rm_dist_id == _distId
                              select n).ToList();
            return rangMaster;
        }
        public tbl_ranges_master GetEndRangesByDistIdandStartRangeId(int _distId, int _rmId)
        {
            var rangMaster = (from n in _db.tbl_ranges_master
                              where n.rm_dist_id == _distId && n.rm_id == _rmId
                              select n).FirstOrDefault();
            return rangMaster;
        }

        public tbl_ranges_master GetRangesById(int _rmId)
        {
            var rangMaster = (from n in _db.tbl_ranges_master
                              where n.rm_id == _rmId
                              select n).FirstOrDefault();
            return rangMaster;
        }
       
    }
}
