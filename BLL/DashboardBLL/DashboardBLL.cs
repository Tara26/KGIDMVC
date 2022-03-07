using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DashboardDLL;
using DLL.DBConnection;
using KGID_Models.Dashboard;

namespace BLL.DashboardBLL
{
    public class DashboardBLL : IDashboardBLL
    {
        private IDashboardDLL dashboardDLL;

        public DashboardBLL()
        {
            dashboardDLL = new DashboardDLL();
        }

        public VM_Dashboard GetDashboardData(long userId)
        {
            return dashboardDLL.GetDashboardData(userId);
        }
        public List<VM_EmpDashboardData> GetDashboardInsuredEmpData(long userId)
        {
            return dashboardDLL.GetDashboardInsuredEmpData(userId);
        }
    }
}
