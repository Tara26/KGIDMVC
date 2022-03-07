using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class DoubleVerificationDataResponseKII
    {
        public string deptRefNum { get; set; }
        public string pymntstatus { get; set; }
        public string pymntMode { get; set; }
        public string bankName { get; set; }
        public string paidAmount { get; set; }
        public string currentTimeStamp { get; set; }
        public string statusCode { get; set; }
        public string statusDesc { get; set; }
        public string status { get; set; }
    }
}
