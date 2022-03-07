using KGID_Models.KGIDEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class VM_BasicDetails
    {
        public tbl_new_employee_basic_details EmployeeBasicDetails { get; set; }
        public tbl_employee_address_details EmployeeAddressDetails { get; set; }
        public tbl_employee_work_details EmployeeWorkDetails { get; set; }
        public string EmployeeDesignation { get; set; }
        public string PresentWorkingoffice { get; set; }
        public string QRCode { get; set; }
        public tbl_application_referenceno_details EmployeeReferanceDetails { get; set; }
    }
}
