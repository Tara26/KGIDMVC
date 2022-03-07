using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class VM_DDOVerificationDetails
    {
        public VM_DDOVerificationDetails()
        {
            EmployeeVerificationDetails = new List<EmployeeDDOVerificationDetail>();
            IEmployeeVerificationDetails = new List<EmployeeDDOVerificationDetail>();
            LastUpdatedStatusForEmployees = new List<EmployeeDDOVerificationDetail>();
            ApprovedEmployeeStatus = new List<EmployeeDDOVerificationDetail>();
        }
        public IList<EmployeeDDOVerificationDetail> EmployeeVerificationDetails { get; set; }

        public IList<EmployeeDDOVerificationDetail> IEmployeeVerificationDetails { get; set; }

        public IList<EmployeeDDOVerificationDetail> LastUpdatedStatusForEmployees { get; set; }

        public IList<EmployeeDDOVerificationDetail> ApprovedEmployeeStatus { get; set; }

        public long TotalReceived { get; set; }

        public long ForwardedApplications { get; set; }

        public long SentBackApplication { get; set; }

        public long PendingApplications { get; set; }

        public string Department { get; set; }

        public string Designation { get; set; }
    }

    public class EmployeeDDOVerificationDetail
    {
        public string Name { get; set; }

        public long? EmployeeCode { get; set; }

        public string ApplicationNumber { get; set; }

        public long ApplicationId { get; set; }

        public string Remarks { get; set; }

        public string Comments { get; set; }

        public string Status { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }//Need to remove

        public string LastUpdatedDate { get; set; }

        public string Premium { get; set; }

        public string PolicyNumber { get; set; }

        public string SanctionedDate { get; set; }

        public int Priority { get; set; }

        public int RowNum { get; set; }

        public string District { get; set; }
        public string Department { get; set; }

        public string NBBondDocPath { get; set; }
        public string NBFSDocPath { get; set; }
        public string NBSignBondDocPath { get; set; }
    }
}
