using KGID_Models.KGID_VerifyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.RangesMasterBLL
{
    public interface IRangesMasterBLL
    {
        IEnumerable<tbl_ranges_master> RangesMasterdll();
        IEnumerable<tbl_ranges_master> GetStartRangesByDistId(int _distId);
       tbl_ranges_master GetEndRangesByDistIdandStartRangeId(int _distId, int _startRangeId);
        tbl_ranges_master GetRangesById(int _rmId);
    }
}
