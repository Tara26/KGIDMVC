using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_MB_Claim
{
    public class VM_ODClaimApprovedApplicationDetails
    {
        public VM_ODClaimApprovedApplicationDetails()
        {
            ApprovedAppDetails = new List<ApprovedApplicationDetails>();
        }
        public IList<ApprovedApplicationDetails> ApprovedAppDetails { get; set; }
        public class ApprovedApplicationDetails
        {
            public long EmpolyeeId { get; set; }
            public long CategoryId { get; set; }
            public decimal DamageCost { get; set; }
            public decimal ApprovedDamageCost { get; set; }
            public string VehicleNo { get; set; }
            public long ApplicationRefNo { get; set; }
            public long ApplicationId { get; set; }
            public string MIPolicyNo { get; set; }
            public string ApplicationStatus { get; set; }
            public List<ApprovedApplicationDetails> VehicleAndPolicyDetails { get; set; }

        }
    }
}
