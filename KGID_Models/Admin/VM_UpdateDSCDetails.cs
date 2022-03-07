using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.Admin
{
    public class VM_DSCDetails
    {
        public string KGIDNumber { get; set; }
        public long EmpId { get; set; }
        public string DSCPublicKey { get; set; }
        public DateTime DSCIssueDate { get; set; }
        public DateTime DateOfExpiry { get; set; }
        public string DSCSerialNumber { get; set; }
        public string IssueAuthority { get; set; }
        public string SignatoryName { get; set; }
        public long LoggedInUser { get; set; }
    }
}
