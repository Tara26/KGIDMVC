using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.ClaimsDLL;
using KGID_Models.KGID_Bonus;

namespace BLL.ClaimsBLL
{
    public class BonusBLL : IBonusBLL
    {
        private readonly IBonusDLL bonusDLL;

        public BonusBLL()
        {
            bonusDLL = new BonusDLL();
        }

        public VM_BonusArrears GetBonusArrearList(long loggedInUserId)
        {
            try
            {
                return bonusDLL.GetBonusArrearList(loggedInUserId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public VM_BonusDetails GetBonusDetails(long claimId)
        {
            try
            {
                return bonusDLL.GetBonusDetails(claimId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
