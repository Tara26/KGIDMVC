using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class VM_EmployeeDscDetails
    {
        public long employee_id { get; set; }
        public string first_kgid_policy_no { get; set; }
        public string employee_name { get; set; }
        public string designation { get; set; }
        public string department { get; set; }
        public string user_category_id { get; set; }

        //Get Dsc Existing Employee Details
        public long dsc_emp_id { get; set; }
        public string dsc_public_key { get; set; }
        public DateTime dsc_date_of_issue { get; set; }
        public DateTime dsc_date_of_expiring { get; set; }
        public string dsc_dsc_serial_no { get; set; }
        public string dsc_kgid_number { get; set; }
        public string dsc_name_of_authority { get; set; }
        public string dsc_authority_of_issuer { get; set; }
        public bool dsc_active { get; set; }
    }
}
