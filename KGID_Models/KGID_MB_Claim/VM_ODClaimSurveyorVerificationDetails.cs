using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_MB_Claim
{
    public class VM_ODClaimSurveyorVerificationDetails
    {
        public VM_ODClaimSurveyorVerificationDetails()
        {
            ApplicantVerificationDetails = new List<ApplicantVerificationDetailMIODClaim>();
            LastUpdatedStatusForEmployees = new List<ApplicantVerificationDetailMIODClaim>();
            ApprovedEmployeeStatus = new List<ApplicantVerificationDetailMIODClaim>();
        }
        public IList<ApplicantVerificationDetailMIODClaim> ApplicantVerificationDetails { get; set; }

        public IList<ApplicantVerificationDetailMIODClaim> LastUpdatedStatusForEmployees { get; set; }

        public IList<ApplicantVerificationDetailMIODClaim> ApprovedEmployeeStatus { get; set; }

        public long TotalReceived { get; set; }

        public long ForwardedApplications { get; set; }

        public long SentBackApplication { get; set; }

        public long AssignedApplications { get; set; }

        public long ApplicationRefNumber { get; set; }

        //challan print
        public string RefNos { get; set; }
        public string ChallanAmount { get; set; }
    }
    public class ApplicantVerificationDetailMIODClaim
    {
        public string Name { get; set; }

        public long? EmployeeCode { get; set; }

        public string ApplicationNumber { get; set; }

        public string VehicleNumber { get; set; }

        public long ApplicationId { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }

        public string LastUpdatedDate { get; set; }
        public string Premium { get; set; }
        public int AppStatusID { get; set; }
        public string CategoryId { get; set; }
        public string PrevApplicationNumber { get; set; }
    }
    
}
