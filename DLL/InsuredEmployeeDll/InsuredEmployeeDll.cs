using DLL.DBConnection;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DLL.InsuredEmployeeDll
{
    public class InsuredEmployeeDll : IInsuredEmployeeDll
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        
        public tbl_insured_details_new ViewInsuredEmployee(long kgIdNo)
        {
            return (from emp in _db.tbl_insured_details_new
                    where emp.id_sys_emp_code == kgIdNo
                    select emp).FirstOrDefault();
        }

    }
}
