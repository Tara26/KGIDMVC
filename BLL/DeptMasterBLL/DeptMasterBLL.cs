using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.KGID_User;
using DLL.DeptMasterDLL;

namespace BLL.DeptMasterBLL
{
   public class DeptMasterBLL: IDeptMasterBLL
    {
        private readonly IDeptMasterDLL _IDeptMasterdll;
        public DeptMasterBLL()
        {
            this._IDeptMasterdll = new DeptMasterDLL();
        }
        public IEnumerable<tbl_dept_master> DeptMasterbll()
        {
            var result = _IDeptMasterdll.DeptMasterdll();
            return result;
        }
        public IEnumerable<tbl_dept_master> DeptNameByDeptCode(string _deptCode)
        {
            var result = _IDeptMasterdll.DeptNameByDeptCode(_deptCode);
            return result;
        }
    }
}
