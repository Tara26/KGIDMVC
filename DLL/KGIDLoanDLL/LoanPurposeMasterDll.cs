using DLL.DBConnection;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DLL.KGIDLoanDLL
{
    public class LoanPurposeMasterDll:ILoanPurposeMasterDll
    {
        private readonly DbConnection _db = new DbConnection();
        private readonly Common_Connection _Conn = new Common_Connection();
        public IEnumerable<tbl_loan_purpose_master> ListAll()
        {
            List<tbl_loan_purpose_master> loanPurposeDataList = new List<tbl_loan_purpose_master>();
            DataSet dsBD = new DataSet();

            SqlParameter[] sqlparam =
            {
            };

            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getLoanPurposeDetails");

            if (dsBD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsBD.Tables[0].Rows)
                {
                    tbl_loan_purpose_master loanPurposeData = new tbl_loan_purpose_master();

                    loanPurposeData.lp_loan_purpose_id = dr.Field<int>("lp_loan_purpose_id");
                    loanPurposeData.lp_purpose_desc = dr.Field<string>("lp_purpose_desc");
                    loanPurposeDataList.Add(loanPurposeData);
                }
            }
            return loanPurposeDataList;
        }
    }
}
