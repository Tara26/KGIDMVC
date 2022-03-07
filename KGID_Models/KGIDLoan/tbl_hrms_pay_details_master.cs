using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDLoan
{
    public class tbl_hrms_pay_details_master
    {
        [Key]
        public long hrms_hrms_pay_id { get; set; }
        public long? hrms_emp_id { get; set; }
        public long? hrms_emp_code { get; set; }
        public int? hrms_month_id { get; set; }
        public int? hrms_year_id { get; set; }
        public int? hrms_gross_pay { get; set; }
        public int? hrms_deduction { get; set; }
        public int? hrms_net_pay { get; set; }
        public bool? hrms_active { get; set; }
        public DateTime? hrms_creation_datetime { get; set; }
        public int? hrms_created_by { get; set; }
        public DateTime? hrms_updation_datetime { get; set; }
        public int? hrms_updated_by { get; set; }

    }
}
