using KGID_Models.KGID_Bonus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ClaimsBLL
{
    public interface IBonusBLL
    {
        VM_BonusArrears GetBonusArrearList(long loggedInUserId);
        VM_BonusDetails GetBonusDetails(long claimId);
    }
}
