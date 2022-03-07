using KGID_Models.KGID_User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DeptMasterBLL
{
    public interface IDeptMasterBLL
    {
        IEnumerable<tbl_dept_master> DeptMasterbll();
        IEnumerable<tbl_dept_master> DeptNameByDeptCode(string deptCode);
    }
}
