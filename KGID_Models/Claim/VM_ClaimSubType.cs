using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.Claim
{
    public class VM_ClaimSubType
    {
        public int Id { get; set; }
        public string ClaimSubType { get; set; }
    }

    public class VM_ClaimSubTypes
    {
        public VM_ClaimSubTypes()
        {
            ClaimSubTypes = new List<VM_ClaimSubType>();
        }

        public IList<VM_ClaimSubType> ClaimSubTypes { get; set; }
    }
}
