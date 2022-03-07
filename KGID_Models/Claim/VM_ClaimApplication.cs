using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.Claim
{
    public class VM_ClaimApplication
    {
        public long EmpId { get; set; }
        public string EmpName { get; set; }
        public string ClaimRefNumber { get; set; }
        public long ClaimApplicationId { get; set; }
        public DateTime MaturityDate { get; set; }
    }

    public class VM_ClaimApplications
    {
        public VM_ClaimApplications()
        {
            Claims = new List<VM_ClaimApplication>();
        }     
        public IList<VM_ClaimApplication> Claims { get; set; }

        public long TotalReceived { get; set; }

        public long ForwardedApplications { get; set; }

        public long SentBackApplication { get; set; }

        public long PendingApplications { get; set; }
    }
}
