using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_employee_family_details
    {
        [Key]
        public long efd_family_id { get; set; }
        public long? efd_emp_id { get; set; }
        public int? efd_relation_id { get; set; }
        public string efd_name_of_member { get; set; }
        public DateTime efd_date_of_birth { get; set; }
        public bool? efd_alive_or_dead { get; set; }
        public DateTime? efd_date_of_death { get; set; }
        public int? efd_age { get; set; }
        public string efd_reason_of_death { get; set; }
        public string efd_health_condition { get; set; }
        public bool? efd_married { get; set; }
        public long? efd_application_id { get; set; }
        public bool efd_active { get; set; }
        public DateTime efd_creation_datetime { get; set; }
        public long efd_created_by { get; set; }
        public DateTime efd_updation_datetime { get; set; }
        public long efd_updated_by { get; set; }
    }
}
