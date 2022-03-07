using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.NBApplication;

namespace KGID_Models.KGID_VerifyData
{
    public class VM_NBBond
    {

        //public tbl_new_employee_basic_details EmployeeBasicDetails { get; set; }
        //public tbl_employee_address_details EmployeeAddressDetails { get; set; }
        //public tbl_employee_work_details EmployeeWorkDetails { get; set; }
        //public string EmployeeDesignation { get; set; }
        //public string PresentWorkingoffice { get; set; }



        //public int KgidPolicy { get; set; }
        //public Nullable<decimal> KgidPremium { get; set; }
        //public Nullable<decimal> KgidPremiumAmount { get; set; }
        //public IEnumerable<tbl_payscale_details> PayscaleDetails { get; set; }



        //public List<tbl_nominee_details> NomineeDetailsList { get; set; }


        //public tbl_verification_details VerificationDetails { get; set; }


        public VM_BasicDetails EmployeeBasicDetails { get; set; }
        
        public List<VM_NomineeDetail> NomineeDetailsList { get; set; }

        
    }
}
