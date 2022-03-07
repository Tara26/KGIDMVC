using KGID_Models.KGID_User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DeptMasterDLL
{
    public interface IDeptMasterDLL
    {
        IEnumerable<tbl_dept_master> DeptMasterdll();
        IEnumerable<tbl_dept_master> DeptNameByDeptCode(string deptCode);
    }
}
