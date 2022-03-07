using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_employee_nominee_details
    {
        [Key]
        public long end_nominee_id { get; set; }
        public long? end_emp_id { get; set; }
        public long? end_application_id { get; set; }
        public int? end_relation_id { get; set; }
        public string end_name_of_nominee { get; set; }
        public string end_name_of_guardian { get; set; }
        public int? end_guardian_relation_id { get; set; }
        public int? end_percentage_of_share { get; set; }
        public long? end_family_id { get; set; }
        public bool end_active { get; set; }
        public DateTime end_creation_datetime { get; set; }
        public long end_created_by { get; set; }
        public DateTime end_updation_datetime { get; set; }
        public long end_updated_by { get; set; }
    }
}
