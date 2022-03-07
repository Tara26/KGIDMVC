using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
   public class tbl_motor_insurance_vehicle_details
    {
        [Key]
        public int mivd_registration_id { get; set; }
        public string mivd_registration_no { get; set; }
        public DateTime  mivd_date_of_registration { get; set; }
        public string mivd_registration_authority_and_location { get; set; }
        public DateTime mivd_year_of_manufacturer { get; set; }
        public int mivd_engine_no { get; set; }
        public string mivd_chasis_no { get; set; }
        public int mivd_make_of_vehicle { get; set; }
        public string mivd_type_of_model { get; set; }
        public int mivd_cubic_capacity { get; set; }
        public int mivd_seating_capacity_including_driver { get; set; }
        public int mivd_vehicle_fuel_type { get; set; }
        public int mivd_vehicle_type { get; set; }
        public int mivd_application_id { get; set; }
        public int   mivd_cat_type_id { get; set; }
        public int mivd_vehicle_category { get; set; }
        public int mivd_vehicle_subtype { get; set; }
    }
}
