using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_motor_insurance_application
    {
        [Key]
        public long mia_motor_insurance_app_id { get; set; }
        public long mia_application_ref_no { get; set; }
        public long mia_proposer_id { get; set; }
        public int mia_vehicle_model_id { get; set; }
        public string mia_engine_no { get; set; }
        public string mia_chasis_no { get; set; }
        public int mia_application_status { get; set; }
        public decimal mia_total_idv_amount { get; set; }
        public bool mia_declaration_status { get; set; }
        public DateTime mia_date_of_submission { get; set; }
        public string mia_vehicle_reg_no { get; set; }
        public string mia_regisration_no { get; set; }
        public DateTime mia_date_of_reg { get; set; }
        public bool mia_active { get; set; }
        public int mia_make_of_vehicle { get; set; }
        public int mia_cubic_capacity { get; set; }
        public int mia_seating_capacity_including_driver { get; set; }
        public int mia_vehicle_fuel_type { get; set; }
        public int mia_year_of_manufacturer { get; set; }
        public string mia_registration_authority_and_location { get; set; }

        public DateTime mia_creation_datetime { get; set; }
        public int mia_created_by { get; set; }
        public DateTime mia_updation_datetime { get; set; }
        public int mia_updated_by { get; set; }
        public int mia_type_of_cover { get; set; }
        public string mia_address { get; set; }
        public string mia_owner_of_the_vehicle { get; set; }

    }
}
