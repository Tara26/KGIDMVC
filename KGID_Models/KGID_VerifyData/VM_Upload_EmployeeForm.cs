using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGID_VerifyData
{
    public class VM_Upload_EmployeeForm
    {
        public long App_Employee_Code  { get; set; }
        public HttpPostedFileBase ApplicationFormDoc { get; set; }
        public HttpPostedFileBase MedicalFormDoc { get; set; }
        public string ApplicationFormDocName { get; set; }
        public string MedicalFormDocName { get; set; }
    }
}
