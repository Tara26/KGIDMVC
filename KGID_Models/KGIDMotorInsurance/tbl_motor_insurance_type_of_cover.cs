using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_motor_insurance_type_of_cover
    {
        [Key]
        public int mitoc_type_cover_id { get; set; }
        public string mitoc_type_cover_name { get; set; }
        public Nullable<bool> mitoc_type_cover_status { get; set; }
        public Nullable<bool> mitoc_active_status { get; set; }
        public Nullable<DateTime> mitoc_creation_datetime { get; set; }
        public Nullable<DateTime> mitoc_updation_datetime { get; set; }
        public Nullable<int> mitoc_created_by { get; set; }
        public Nullable<int> mitoc_updated_by { get; set; }

    }
}
