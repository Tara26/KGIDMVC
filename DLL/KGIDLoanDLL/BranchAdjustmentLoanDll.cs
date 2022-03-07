using DLL.DBConnection;
using KGID_Models.KGIDLoan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDLoanDLL
{
    public class BranchAdjustmentLoanDll:IBranchAdjustmentLoanDll
    {
        private readonly DbConnection _db = new DbConnection();
        private readonly Common_Connection _Conn = new Common_Connection();

        public IEnumerable<tbl_branch_adjustment_loan> ListAll()
        {
            return null;
        }
        public int GetDueAmount(long emp_id)
        {
            string result;
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@employee_id",emp_id)
            };
            result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_getBranchAdjustmentDueAmount");
            return Convert.ToInt32(result);
        }


        public IEnumerable<VM_LoanDueList> GetDueList(long empId)
        {
            List<VM_LoanDueList> paymentDueList = new List<VM_LoanDueList>();

            DataSet dsBD = new DataSet();

            SqlParameter[] sqlparam =
            {
                new SqlParameter("@employee_id",empId)
            };

            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_GetPaymentDueList");

            if (dsBD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsBD.Tables[0].Rows)
                {
                    VM_LoanDueList paymentDue = new VM_LoanDueList();

                    paymentDue.PolicyNumber = dr.Field<long?>("bap_policy_id").Value;
                    paymentDue.Month = dr.Field<string>("mm_month_desc").ToString();
                    paymentDue.YearId = dr.Field<int?>("bap_year_id").Value;
                    paymentDue.Amount = dr.Field<int?>("bap_premium").Value;
                    paymentDue.InterestAmount = dr.Field<int?>("bap_penal_interest").Value;
                    paymentDueList.Add(paymentDue);
                }
            }
            return paymentDueList;
        }
    }
}
