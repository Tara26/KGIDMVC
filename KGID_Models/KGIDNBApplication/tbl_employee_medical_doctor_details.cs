using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_employee_medical_doctor_details
    {
        [Key]
        public long emdd_id { get; set; }
        public long? emdd_employee_id { get; set; }
        public long? emdd_application_id { get; set; }
        public long? emdd_doctor_id { get; set; }
        public string emdd_name_of_doctor { get; set; }
        public string emdd_designation { get; set; }
        public string emdd_name_of_hospital { get; set; }
        public bool? emdd_active_status { get; set; }
        public long? emdd_created_by { get; set; }
        public DateTime? emdd_creation_datetime { get; set; }
        public long? emdd_updated_by { get; set; }
        public DateTime? emdd_updation_datetime { get; set; }
    }
}
