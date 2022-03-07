using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DBConnection;
using KGID_Models.KGID_User;

namespace DLL.DDOMasterDLL
{
    public class DDOMasterDLL: IDDOMasterDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        tbl_ddo_master ddomaster;
        public IEnumerable<tbl_ddo_master> DDOMasterdll()
        {
            var ddomaster = (from n in _db.tbl_ddo_master
                              select n).ToList();
            return ddomaster;
        }      
    }
}
