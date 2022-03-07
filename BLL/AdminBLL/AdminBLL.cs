using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.AdminDLL;
using KGID_Models.Admin;
using KGID_Models.KGID_Master;

namespace BLL.AdminBLL
{
    public class AdminBLL : IAdminBLL
    {
        private readonly IAdminDLL admin;

        public AdminBLL()
        {
            admin = new AdminDLL();
        }

        public bool SaveDSCDetails(VM_DSCDetails dscDetails)
        {
            return admin.SaveDSCDetails(dscDetails);
        }
        public VM_EmployeeDscDetails GetEmployeeDSCData(string kgidnum)
        {
            return admin.GetEmployeeDSCData(kgidnum);
        }
    }
}
