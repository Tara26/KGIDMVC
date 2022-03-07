using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.KGID_Bonus;

namespace DLL.ClaimsDLL
{
    public interface IBonusDLL
    {
        VM_BonusArrears GetBonusArrearList(long loggedInUserId);
        VM_BonusDetails GetBonusDetails(long empId);
    }
}
