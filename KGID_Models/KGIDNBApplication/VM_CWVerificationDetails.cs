using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class VM_VerificationDetails
    {
        public VM_VerificationDetails()
        {
            EmployeeVerificationDetails = new List<EmployeeVerificationDetail>();
            IEmployeeVerificationDetails = new List<EmployeeVerificationDetail>();
            LastUpdatedStatusForEmployees = new List<EmployeeVerificationDetail>();
            NeedsHealthOpinionForEmployees = new List<EmployeeVerificationDetail>();
        }

        public IList<EmployeeVerificationDetail> EmployeeVerificationDetails { get; set; }

        public IList<EmployeeVerificationDetail> IEmployeeVerificationDetails { get; set; }

        public IList<EmployeeVerificationDetail> LastUpdatedStatusForEmployees { get; set; }

        public IList<EmployeeVerificationDetail> NeedsHealthOpinionForEmployees { get; set; }

        public long TotalReceived { get; set; }

        public long ForwardedApplications { get; set; }

        public long SentBackApplication { get; set; }

        public long PendingApplications { get; set; }
    }

    public class EmployeeVerificationDetail
    {
        public string Name { get; set; }

        public long? EmployeeCode { get; set; }

        public long ApplicationId { get; set; }

        public string ApplicationNumber { get; set; }

        public string Status { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }

        public string LastUpdatedDate { get; set; }

        public int Priority { get; set; }
        public string District { get; set; }
        public string Department { get; set; }
    }
}
