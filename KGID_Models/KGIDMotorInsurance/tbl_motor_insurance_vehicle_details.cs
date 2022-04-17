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
       /// public int mivd_registration_id { get; set; }
       public int mivd_vehicle_details_id { get; set; }
        public string mivd_registration_no { get; set; }
        public DateTime  mivd_date_of_registration { get; set; }
        public string mivd_registration_authority_and_location { get; set; }
        public int mivd_year_of_manufacturer { get; set; }
        public string mivd_engine_no { get; set; }
        public string mivd_chasis_no { get; set; }
        public int mivd_make_of_vehicle { get; set; }
        public string mivd_type_of_model { get; set; }
        public int mivd_cubic_capacity { get; set; }
        public int mivd_seating_capacity_including_driver { get; set; }
        public int mivd_vehicle_fuel_type { get; set; }

        public int mivd_vehicle_type { get; set; }
        public decimal mivd_vehicle_weight { get; set; }

        public int mivd_cat_type_id { get; set; }

        public int mivd_vehicle_category { get; set; }
        public int mivd_vehicle_subtype { get; set; }

        public  long mivd_application_id { get; set; }
        public  bool mivd_status { get; set; }
        public  DateTime mivd_updation_date { get; set; }
        public  DateTime mivd_creation_date { get; set; }
        public  DateTime mivd_date_of_manufacturer { get; set; }
        public  string mivd_created_by { get; set; }
        public  string mivd_updated_by { get; set; }
        public  int mivd_rto_id { get; set; }
        public  int mivd_class_id { get; set; }
        public  string mivd_vahanidvamount { get; set; }
        public Nullable<System.DateTime> mivd_vehicle_fit_upto { get; set; }














    }
}
