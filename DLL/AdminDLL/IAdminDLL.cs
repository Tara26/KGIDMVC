using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.Admin;
using KGID_Models.KGID_Master;

namespace DLL.AdminDLL
{
    public interface IAdminDLL
    {
        bool SaveDSCDetails(VM_DSCDetails dscDetails);
        VM_EmployeeDscDetails GetEmployeeDSCData(string kgidnum);
    }
}
