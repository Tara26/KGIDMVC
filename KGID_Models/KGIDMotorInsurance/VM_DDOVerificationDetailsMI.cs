using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGID_Models.KGIDMotorInsurance
{

    public class VM_DDOVerificationDetailsMI
    {
        public VM_DDOVerificationDetailsMI()
        {
            EmployeeVerificationDetails = new List<EmployeeDDOVerificationDetailMI>();
            LastUpdatedStatusForEmployees = new List<EmployeeDDOVerificationDetailMI>();
            ApprovedEmployeeStatus = new List<EmployeeDDOVerificationDetailMI>();
            PolicyPremiumDetailMI = new List<PolicyPremiumDetailMI>();
            RenewalPolicyPremiumDetailMI = new List<RenewalPolicyPremiumDetailMI>();
        }
        public IList<EmployeeDDOVerificationDetailMI> EmployeeVerificationDetails { get; set; }

        public IList<EmployeeDDOVerificationDetailMI> LastUpdatedStatusForEmployees { get; set; }

        public IList<EmployeeDDOVerificationDetailMI> ViewStatusForEmployees { get; set; }

        public IList<EmployeeDDOVerificationDetailMI> ApprovedEmployeeStatus { get; set; }

        public IList<PolicyPremiumDetailMI> PolicyPremiumDetailMI { get; set; }
        public IList<RenewalPolicyPremiumDetailMI> RenewalPolicyPremiumDetailMI { get; set; }
        public IList<EmployeeDDOVerificationDetailMI> CancelEmployeeStatus { get; set; }

        public int Category { get; set; }
        public long TotalReceived { get; set; }

        public long ForwardedApplications { get; set; }

        public long SentBackApplication { get; set; }

        public long PendingApplications { get; set; }

        public long ApplicationRefNumber { get; set; }

        public long CancelApplication { get; set; }

          //ICT 18-09-2021 ....
        public string RefNos { get; set; }
        public string ChallanAmount { get; set; }
        public string PayAmount { get; set; }
        public string DDOCODE { get; set; }
        public string XML_FILE_NAME { get; set; }
        public string SAN_ORDER_NO { get; set; }
        public string SAN_DATE { get; set; }
        public string VOUCHERNO { get; set; }
        public string HOACODE { get; set; }

        public string SAN_DATE_DISP { get; set; }

        public string CHALLAN_DATE { get; set; }

        public string AMT_IN_WORD { get; set; }

        public string RO_ADDRES { get; set; }
        //Drop Down List  17-SEP-2021
        public List<SelectListItem> HOAList { get; set; }

        //ICT 18-09-2021 ....
    }

    public class EmployeeDDOVerificationDetailMI
    {
        public string Name { get; set; }
        public string District { get; set; }

        public long? EmployeeCode { get; set; }

        public string ApplicationNumber { get; set; }
        public string registrationNo { get; set; }
        public long ApplicationId { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }

        public string LastUpdatedDate { get; set; }
        public string Premium { get; set; }
        public Nullable<double> PolicyPremium { get; set; }

        public int AppStatusID { get; set; }
        public string CategoryId { get; set; }
        public string PolicyNumber { get; set; }
        public string UnsignBondDocPath { get; set; }
        public string SignedBondDocPath { get; set; }
        public string PrevApplicationNumber { get; set; }

        public string VehicleModelName { get; set; }

        public string VehicleManufactureName { get; set; }
        public string ChasisNo { get; set; }
        public string EngineNo { get; set; }
        public string VehicleYear { get; set; }
        public string TypeofCover { get; set; }

        public long Workflowid { get; set; }
    }
    public class PolicyPremiumDetailMI
    {

        public long EmployeeCode { get; set; }

        public long ApplicationNumber { get; set; }
        public string registrationNo { get; set; }
        public long? ApplicationId { get; set; }

        public string VehicleMakeName { get; set; }

        public string VehicleManufactureName { get; set; }

        public string VehicleModelName { get; set; }

        public string ChasisNo { get; set; }

        public string EngineNo { get; set; }

        public string TypeofCover { get; set; }

        public long? PolicyId { get; set; }

        public Nullable<double> PolicyPremiumAmount { get; set; }

        public string DDOCode { get; set; }

        //public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }

        public string VehicleYear { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public string Name { get; set; }
        public string PolicyNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string MBBondDocPath { get; set; }
        public string MBSignedBondDocPath { get; set; }
        //public string LastUpdatedDate { get; set; }
        //public string Premium { get; set; }
        //public int AppStatusID { get; set; }
    }
    public class RenewalPolicyPremiumDetailMI
    {

        public long EmployeeCode { get; set; }

        public long ApplicationNumber { get; set; }
        public string registrationNo { get; set; }
        public long? ApplicationId { get; set; }

        public string VehicleMakeName { get; set; }

        public string VehicleManufactureName { get; set; }

        public string VehicleModelName { get; set; }

        public string ChasisNo { get; set; }

        public string EngineNo { get; set; }

        public string TypeofCover { get; set; }

        public long? PolicyId { get; set; }

        public Nullable<double> PolicyPremiumAmount { get; set; }

        public string DDOCode { get; set; }

        //public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }

        public string VehicleYear { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public string Name { get; set; }
        public string PolicyNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string MBBondDocPath { get; set; }
        public string MBSignedBondDocPath { get; set; }
        //public string LastUpdatedDate { get; set; }
        //public string Premium { get; set; }
        //public int AppStatusID { get; set; }
    }
}

