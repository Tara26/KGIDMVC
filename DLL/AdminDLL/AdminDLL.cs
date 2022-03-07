using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DBConnection;
using KGID_Models.Admin;
using KGID_Models.KGID_Master;

namespace DLL.AdminDLL
{
    public class AdminDLL : IAdminDLL
    {
        private readonly Common_Connection _Conn = new Common_Connection();

        public bool SaveDSCDetails(VM_DSCDetails dscDetails)
        {
            string response = string.Empty;

            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@KGIDNumber", dscDetails.KGIDNumber),
                    new SqlParameter("@EmpId", dscDetails.EmpId),
                    new SqlParameter("@DSCPublicKey", dscDetails.DSCPublicKey),
                    new SqlParameter("@DateOfIssue", dscDetails.DSCIssueDate),
                    new SqlParameter("@DateOfExpiry", dscDetails.DateOfExpiry),
                    new SqlParameter("@DSCSerialNumber", dscDetails.DSCSerialNumber),
                    new SqlParameter("@DSCNameOfAuthority", dscDetails.SignatoryName),
                    new SqlParameter("@DSCAuthorityOfIssuer", dscDetails.IssueAuthority),
                    new SqlParameter("@LoggedInUserId", dscDetails.LoggedInUser)
                };

                response = _Conn.ExecuteCmd(sqlparam, "sp_admin_saveDSCDetails");

                if (!string.IsNullOrEmpty(response))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }            

            return false;
        }
        public VM_EmployeeDscDetails GetEmployeeDSCData(string kgidnum)
        
{
            VM_EmployeeDscDetails objDetl = new VM_EmployeeDscDetails();
            DataSet dsBD = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@kgidnumber",kgidnum)
            };
            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_GetEmployeeDSCDetails");
            if (dsBD.Tables[0].Rows.Count > 0)
            {
                if(Convert.ToInt64(dsBD.Tables[0].Rows[0]["dsc_emp_id"])!=0)
                {
                    objDetl.dsc_emp_id = Convert.ToInt64(dsBD.Tables[0].Rows[0]["dsc_emp_id"]);
                    objDetl.dsc_kgid_number = dsBD.Tables[0].Rows[0]["dsc_kgid_number"].ToString();
                    objDetl.dsc_dsc_serial_no = dsBD.Tables[0].Rows[0]["dsc_dsc_serial_no"].ToString();
                    objDetl.dsc_public_key = dsBD.Tables[0].Rows[0]["dsc_public_key"].ToString();
                    objDetl.dsc_date_of_issue = Convert.ToDateTime(dsBD.Tables[0].Rows[0]["dsc_date_of_issue"]);
                    objDetl.dsc_date_of_expiring = Convert.ToDateTime(dsBD.Tables[0].Rows[0]["dsc_date_of_expiring"]);
                    objDetl.dsc_name_of_authority = dsBD.Tables[0].Rows[0]["dsc_name_of_authority"].ToString();
                    objDetl.dsc_authority_of_issuer = dsBD.Tables[0].Rows[0]["dsc_authority_of_issuer"].ToString();
                    objDetl.dsc_active =Convert.ToBoolean(dsBD.Tables[0].Rows[0]["dsc_active"]);
                    ////\
                    objDetl.employee_name = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                    objDetl.designation = dsBD.Tables[0].Rows[0]["d_designation_desc"].ToString();
                    objDetl.department = dsBD.Tables[0].Rows[0]["dm_deptname_english"].ToString();
                    objDetl.user_category_id = dsBD.Tables[0].Rows[0]["user_category_id"].ToString();
                }
                else
                {
                    objDetl.first_kgid_policy_no = dsBD.Tables[0].Rows[0]["first_kgid_policy_no"].ToString();
                    objDetl.employee_name = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                    objDetl.designation = dsBD.Tables[0].Rows[0]["d_designation_desc"].ToString();
                    objDetl.department = dsBD.Tables[0].Rows[0]["dm_deptname_english"].ToString();
                    objDetl.user_category_id = dsBD.Tables[0].Rows[0]["user_category_id"].ToString();
                    objDetl.employee_id = Convert.ToInt64(dsBD.Tables[0].Rows[0]["employee_id"]);
                }
            }
            return objDetl;
        }
    }
}
