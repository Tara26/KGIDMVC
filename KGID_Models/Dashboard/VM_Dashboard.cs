using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.Dashboard
{
    public class VM_Dashboard
    {
        public VM_Dashboard()
        {
            listDashboardData = new List<VM_EmpDashboardData>();
        }
        public int Initial { get; set; }
        public int SentBackToEmployee { get; set; }
        public int ForwardToDDO { get; set; }
        public int SentBackToDDO { get; set; }
        public int ForwardToCaseworker { get; set; }
        public int SentBackToCaseworker { get; set; }
        public int ForwardToSuperintendent { get; set; }
        public int SentBackToSuperintendent { get; set; }
        public int ForwardToDIO { get; set; }
        public int SentBackToDIO { get; set; }
        public int ForwardToDD { get; set; }
        public int SentBackToDD { get; set; }
        public int ForwardToD { get; set; }
        public int NeedHealthOpinion { get; set; }
        public int Approved { get; set; }
        public List<VM_EmpDashboardData> listDashboardData { get; set; }
    }

    public class VM_ListOfPolicyDetails
    {
        public VM_ListOfPolicyDetails()
        {
            listDashboardData = new List<VM_EmpDashboardData>();
        }

        public long? FirstKGIDRefNo { get; set; }
        public List<VM_EmpDashboardData> listDashboardData { get; set; }
    }

    public class VM_Applications
    {
        public int ApplicationStatus { get; set; }
        public long ApplicationReferenceNumber { get; set; }
        public long EmpCode { get; set; }
    }

    public class VM_EmpDashboardData
    {
        public long? PolicyID { get; set; }
        public long EmployeeID { get; set; }
        public string KGIDPolicyNumber { get; set; }
        public long KGIDfirstHRMS { get; set; }
        public string SanctionDate { get; set; }
        public decimal SumAssured { get; set; }
        public decimal PremiumAmt { get; set; }
        public DateTime RiskDate { get; set; }
        public string status { get; set; }
    }
}
