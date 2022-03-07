using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_dept_verification_details
    {
        [Key]
        public int dvd_verification_id { get; set; }
        public long dvd_sys_emp_code { get; set; }
        public long dvd_application_ref_no { get; set; }
        public Nullable<int> dvd_medical_leave { get; set; }
        public bool dvd_verify_proposer_details { get; set; }
        public bool dvd_verify_payment_details { get; set; }
        public bool dvd_verify_medical_report_details { get; set; }
        public bool dvd_verify_medical_condition { get; set; }
        public string dvd_remarks { get; set; }
        public int dvd_application_status { get; set; }
        public bool dvd_status { get; set; }
        public string dvd_health_report_upload_path { get; set; }
        public Nullable<System.DateTime> dvd_creation_datetime { get; set; }
        public Nullable<System.DateTime> dvd_updation_datetime { get; set; }
        public Nullable<long> dvd_created_by { get; set; }
        public Nullable<long> dvd_updated_by { get; set; }
    }
}
