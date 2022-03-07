using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DBConnection;
using KGID_Models.Dashboard;

namespace DLL.DashboardDLL
{
    public class DashboardDLL : IDashboardDLL
    {
        private DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();

        public VM_Dashboard GetDashboardData(long userId)
        {
            var loggedInEmpDetail = (from ddoEmp in _db.tbl_new_employee_basic_details
                                     where ddoEmp.nebd_sys_emp_code == userId
                                     select ddoEmp).FirstOrDefault();

            var appDetails = (from appDet in _db.tbl_dept_verification_details
                              join empDet in _db.tbl_new_employee_basic_details on appDet.dvd_sys_emp_code equals empDet.nebd_sys_emp_code
                              where empDet.nebd_ddo_code == loggedInEmpDetail.nebd_ddo_code
                              select appDet).ToList();

            VM_Dashboard dashboard = new VM_Dashboard();

            var applicationsByRef = appDetails.GroupBy(t => t.dvd_application_ref_no);

            foreach (var applications in applicationsByRef)
            {
                if (applications.Any(t => t.dvd_status && t.dvd_application_status == 15))
                {
                    dashboard.Approved++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 14))
                {
                    dashboard.NeedHealthOpinion++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 13))
                {
                    dashboard.ForwardToD++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 12))
                {
                    dashboard.SentBackToDD++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 11))
                {
                    dashboard.ForwardToDD++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 10))
                {
                    dashboard.SentBackToDIO++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 9))
                {
                    dashboard.ForwardToDIO++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 8))
                {
                    dashboard.SentBackToSuperintendent++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 7))
                {
                    dashboard.ForwardToSuperintendent++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 6))
                {
                    dashboard.SentBackToCaseworker++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 5))
                {
                    dashboard.ForwardToCaseworker++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 4))
                {
                    dashboard.SentBackToDDO++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 3))
                {
                    dashboard.ForwardToDDO++;
                }
                else if (applications.Any(t => t.dvd_status && t.dvd_application_status == 2))
                {
                    dashboard.SentBackToEmployee++;
                }
                else
                {
                    dashboard.Initial++;
                }
            }

            return dashboard;
        }

        public List<VM_EmpDashboardData> GetDashboardInsuredEmpData(long userId)
        {
            List<VM_EmpDashboardData> obj = new List<VM_EmpDashboardData>();
            try
            {
                DataSet dsBD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpID",userId)
                };
                dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getDashboardInsuredEmpData");
                
                if (dsBD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsBD.Tables[0].Rows)
                    {
                        VM_EmpDashboardData objEmp = new VM_EmpDashboardData();
                        objEmp.PolicyID = Convert.ToInt32(dr["p_policy_id"]);
                        objEmp.EmployeeID = Convert.ToInt32(dr["p_emp_id"]);
                        objEmp.KGIDPolicyNumber = dr["KGIDPolicyNumber"].ToString();
                        objEmp.SanctionDate = dr["SanctionDate"].ToString();
                        objEmp.SumAssured = Convert.ToDecimal(dr["SumAssured"]);
                        objEmp.PremiumAmt = Convert.ToDecimal(dr["p_premium"]);
                        obj.Add(objEmp);
                    }
                    
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
    }
}
