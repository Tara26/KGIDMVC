using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.Admin;
using KGID_Models.KGID_Master;

namespace BLL.AdminBLL
{
    public interface IAdminBLL
    {
        bool SaveDSCDetails(VM_DSCDetails dscDetails);
        VM_EmployeeDscDetails GetEmployeeDSCData(string kgidnum);
    }
}
