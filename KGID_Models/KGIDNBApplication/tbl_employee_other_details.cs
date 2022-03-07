using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_employee_other_details
    {
       [Key]
       public long eod_detail_id { get; set; }
       public long eod_emp_id { get; set; }
       public bool eod_emp_married { get; set; }
       public bool eod_spouse_govt_emp { get; set; }
       public bool eod_emp_orphan { get; set; }
       public string eod_spouse_kgid_number { get; set; }
       public string eod_spouse_pan_number { get; set; }
       public long eod_application_id { get; set; }
       public bool eod_active_status { get; set; }
       public long eod_created_by { get; set; }
       public DateTime eod_creation_datetime { get; set; }
       public long eod_updated_by { get; set; }
       public DateTime eod_updation_datetime { get; set; }
    }
}
