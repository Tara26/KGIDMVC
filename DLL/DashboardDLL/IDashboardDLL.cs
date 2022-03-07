using KGID_Models.Dashboard;
using System.Collections.Generic;

namespace DLL.DashboardDLL
{
    public interface IDashboardDLL
    {
        VM_Dashboard GetDashboardData(long userId);
        List<VM_EmpDashboardData> GetDashboardInsuredEmpData(long userId);
    }
}