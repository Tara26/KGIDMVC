using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.NBApplication
{
    public class VM_InsuredEmployeeLoginDetail
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string UserCategory { get; set; }
        public long MobileNumber { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string FirstKGIDNo { get; set; }
        public string Email { get; set; }
    }

    public class VM_NewEmployeeLoginDetail
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string UserCategory { get; set; }
        public long MobileNumber { get; set; }
        public string FirstKGIDNumber { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
    }
}
