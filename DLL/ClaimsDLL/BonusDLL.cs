using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DLL.DBConnection;
using KGID_Models.KGID_Bonus;

namespace DLL.ClaimsDLL
{
    public class BonusDLL : IBonusDLL
    {
        private readonly Common_Connection _Conn = new Common_Connection();

        public VM_BonusArrears GetBonusArrearList(long loggedInUserId)
        {
            VM_BonusArrears bonusArrears = null;

            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@loggedInUserId", loggedInUserId)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_bonus_getBonusArrearList");
                if (dsClaims.Tables[0].Rows.Count > 0)
                {
                    bonusArrears = new VM_BonusArrears();
                    foreach (DataRow row in dsClaims.Tables[0].Rows)
                    {
                        VM_BonusArrear bonusArrear = new VM_BonusArrear();
                        bonusArrear.ClaimRefNumber = row["ClaimRefNumber"].ToString();
                        bonusArrear.NameOfEmployee = row["NameOfEmployee"].ToString();

                        bonusArrears.BonusArrears.Add(bonusArrear);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.ERROR, ex.Message);
            }

            return bonusArrears;
        }

        public VM_BonusDetails GetBonusDetails(long claimId)
        {
            VM_BonusDetails bonusDetails = null;

            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@claimId", claimId)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_bonus_getBonusArrearDetails");
                if (dsClaims.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsClaims.Tables[0].Rows)
                    {
                        VM_BonusDetail bonusDetail = new VM_BonusDetail();
                        bonusDetail.PolicyNumber = row["PolicyNumber"].ToString();
                        bonusDetail.BonusAmount = Convert.ToDecimal(row["BonusAmount"].ToString());

                        bonusDetails.BonusDetails.Add(bonusDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.ERROR, ex.Message);
            }

            return bonusDetails;
        }
    }
}
