using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_MB_Claim
{
    public class VM_ODClaimVerificationDetails
    {
        public VM_ODClaimVerificationDetails()
        {
            EmployeeVerificationDetails = new List<EmployeeVerificationDetailMIODClaim>();
            LastUpdatedStatusForEmployees = new List<EmployeeVerificationDetailMIODClaim>();
            ApprovedEmployeeStatus = new List<EmployeeVerificationDetailMIODClaim>();
            PolicyPremiumDetailMI = new List<PolicyPremiumDetailMI>();
        }
        public IList<EmployeeVerificationDetailMIODClaim> EmployeeVerificationDetails { get; set; }

        public IList<EmployeeVerificationDetailMIODClaim> LastUpdatedStatusForEmployees { get; set; }
        public IList<EmployeeVerificationDetailMIODClaim> ViewStatusForEmployees { get; set; }

        public IList<EmployeeVerificationDetailMIODClaim> ApprovedEmployeeStatus { get; set; }

        public IList<PolicyPremiumDetailMI> PolicyPremiumDetailMI { get; set; }

        public long TotalReceived { get; set; }

        public long ForwardedApplications { get; set; }

        public long SentBackApplication { get; set; }

        public long PendingApplications { get; set; }

        public long ApplicationRefNumber { get; set; }

        //challan print
        public string RefNos { get; set; }
        public string ChallanAmount { get; set; }
    }
    public class EmployeeVerificationDetailMIODClaim
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
        public string PolicyNumber { get; set; }
        public string Premium { get; set; }
        public int AppStatusID { get; set; }
        public string CategoryId { get; set; }
        public string PrevApplicationNumber { get; set; }
    }
    public class PolicyPremiumDetailMI
    {

        public long EmployeeCode { get; set; }

        public long ApplicationNumber { get; set; }

        public long? ApplicationId { get; set; }

        public string VehicleMakeName { get; set; }

        public string VehicleManufactureName { get; set; }

        public string VehicleModelName { get; set; }

        public long? PolicyId { get; set; }

        public Nullable<double> PolicyPremiumAmount { get; set; }

        public string DDOCode { get; set; }

        //public bool IsActive { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }

        //public string LastUpdatedDate { get; set; }
        //public string Premium { get; set; }
        //public int AppStatusID { get; set; }
    }
}
