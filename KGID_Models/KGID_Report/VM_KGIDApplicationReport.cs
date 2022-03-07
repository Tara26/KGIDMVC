using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Report
{
    public class VM_KGIDApplicationReport
    {
        public string Name { get; set; }
        public string ApplicationRefNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string District { get; set; }
        public string Department { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }

    public class VM_KGIDApplicationReports
    {
        public VM_KGIDApplicationReports()
        {
            ApplicationReports = new List<VM_KGIDApplicationReport>();
        }

        public List<VM_KGIDApplicationReport> ApplicationReports { get; set; }
    }

    public class VM_KGIDApplicationReportDetails
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
