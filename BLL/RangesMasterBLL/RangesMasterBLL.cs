using DLL.DBConnection;
using KGID_Models.KGID_VerifyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.RangesMasterDLL;

namespace BLL.RangesMasterBLL
{
   public class RangesMasterBLL:IRangesMasterBLL
    {
        private readonly IRangesMasterDLL _IRangesMasterdll;
        public RangesMasterBLL()
        {
            this._IRangesMasterdll = new RangesMasterDLL();
        }
        public IEnumerable<tbl_ranges_master> RangesMasterdll()
        {
            var result = _IRangesMasterdll.RangesMasterdll();
            return result;
        }
        public IEnumerable<tbl_ranges_master> GetStartRangesByDistId(int _distId)
        {
            var result = _IRangesMasterdll.GetStartRangesByDistId(_distId);
            return result;
        }
        public tbl_ranges_master GetEndRangesByDistIdandStartRangeId(int _distId, int _rmId)
        {
            var result = _IRangesMasterdll.GetEndRangesByDistIdandStartRangeId(_distId, _rmId);
            return result;
        }
        public tbl_ranges_master GetRangesById(int _rmId)
        {
            var result = _IRangesMasterdll.GetRangesById(_rmId);
            return result;
        }
    }
}
