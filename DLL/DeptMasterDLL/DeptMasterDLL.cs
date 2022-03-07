using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DBConnection;
using KGID_Models.KGID_User;

namespace DLL.DeptMasterDLL
{
    public class DeptMasterDLL : IDeptMasterDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        tbl_dept_master deptmaster;
        public IEnumerable<tbl_dept_master> DeptMasterdll()
        {
            var deptMaster = (from n in _db.tbl_department_master
                             select n).ToList();
            return deptMaster;
        }
        public IEnumerable<tbl_dept_master> DeptNameByDeptCode(string _deptCode)
        {     
             var deptList = (from n in _db.tbl_department_master
                             where n.dm_deptcode == _deptCode
                             select n).ToList();                       
              return deptList;
         }   
   }
}
