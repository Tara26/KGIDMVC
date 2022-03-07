using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGID_Loan
{
    public class tbl_employee_loan_details
    {
        [Key]
        public long eld_id { get; set; }
        public long eld_policy_number { get; set; }
        public long? eld_first_policy_number { get; set; }
        public long? eld_sys_emp_code { get; set; }
        public string eld_hrms_name { get; set; }
        public string eld_insured_name { get; set; }
    }
}
