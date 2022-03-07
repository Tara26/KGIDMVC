using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.NBApplication
{
    public class VM_PolicyCancellationDetails
    {
        public VM_PolicyCancellationDetails()
        {
            listPolicyCancelDetails = new List<VM_PolicyCancellationData    >();
        }
        public List<VM_PolicyCancellationData> listPolicyCancelDetails { get; set; }
    }
    public class VM_PolicyCancellationData
    {
        public string EmployeeName { get; set; }
        public long EmployeeID { get; set; }
        public long ApplicationID { get; set; }
        public string ApplicationReferenceNo { get; set; }
        public string EmployeeType { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string Type { get; set; }
    }
}
