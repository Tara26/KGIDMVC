using DLL.DBConnection;
using KGID_Models.KGIDLoan;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DLL.KGIDLoanDLL
{
    public class LoanFamilyRelationMasterDll:ILoanFamilyRelationMasterDll
    {
        private readonly DbConnection _db = new DbConnection();
        private readonly Common_Connection _Conn = new Common_Connection();

        public IEnumerable<tbl_loan_family_relation_master> ListAll()
        {
            List<tbl_loan_family_relation_master> loanFamilyDataList = new List<tbl_loan_family_relation_master>();
            DataSet dsBD = new DataSet();

            SqlParameter[] sqlparam =
            {                
            };

            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getLoanFamilyRelationDetails");

            if (dsBD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsBD.Tables[0].Rows)
                {
                    tbl_loan_family_relation_master loanFamilyData = new tbl_loan_family_relation_master();

                    loanFamilyData.lfr_loan_relation_id = dr.Field<int>("lfr_loan_relation_id");
                    loanFamilyData.lfr_loan_relation_desc = dr.Field<string>("lfr_loan_relation_desc");                   
                    loanFamilyDataList.Add(loanFamilyData);
                }
            }
            return loanFamilyDataList;
        }
    }
}
