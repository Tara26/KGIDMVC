using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Bonus
{
    public class VM_BonusArrear
    {
        public string ClaimRefNumber { get; set; }
        public string NameOfEmployee { get; set; }
        public long ClaimId { get; set; }
    }

    public class VM_BonusArrears
    {
        public VM_BonusArrears()
        {
            BonusArrears = new List<VM_BonusArrear>();
        }

        public List<VM_BonusArrear> BonusArrears { get; set; }
    }

    public class VM_BonusDetails
    {
        public VM_BonusDetails()
        {
            BonusDetails = new List<VM_BonusDetail>();
        }

        public string NameOfEmployee { get; set; }
        public string Designation { get; set; }
        public string ClaimRefNumber { get; set; }
        public string DateOfMaturity { get; set; }
        public decimal TotalBonusAmount { get; set; }

        public List<VM_BonusDetail> BonusDetails { get; set; }
    }

    public class VM_BonusDetail
    {
        public string PolicyNumber { get; set; }
        public decimal BonusAmount { get; set; }
    }
}
