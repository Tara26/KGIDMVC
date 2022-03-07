using KGID_Models.KGID_User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DDOMasterDLL
{
    public interface IDDOMasterDLL
    {
        IEnumerable<tbl_ddo_master> DDOMasterdll();
    }
}
