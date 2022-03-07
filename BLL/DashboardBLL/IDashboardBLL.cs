using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.Dashboard;

namespace BLL.DashboardBLL
{
    public interface IDashboardBLL
    {
        VM_Dashboard GetDashboardData(long userId);
        List<VM_EmpDashboardData> GetDashboardInsuredEmpData(long userId);
    }
}
