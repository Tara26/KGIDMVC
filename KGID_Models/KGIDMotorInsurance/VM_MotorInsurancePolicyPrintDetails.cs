using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class VM_MotorInsurancePolicyPrintDetails
    {
        public long motor_insurance_app_id { get; set; }
        public long application_ref_no { get; set; }
        public long proposer_id { get; set; }

        public string vehicle_make { get; set; }

        public string vehicleClass { get; set; }
        public string engine_no { get; set; }
        public string chasis_no { get; set; }
        public int application_status { get; set; }
        public decimal total_idv_amount { get; set; }
        public string vehicle_reg_no { get; set; }
        public string regisration_no { get; set; }
        public DateTime? date_of_reg { get; set; }
        public bool active { get; set; }
        public decimal vehicle_weight { get; set; }
        public Nullable<int> cubic_capacity { get; set; }
        public Nullable<int> seating_capacity_including_driver { get; set; }
        public int year_of_manufacturer { get; set; }
        public string registration_authority_and_location { get; set; }
        public int type_of_cover { get; set; }
        public string zone { get; set; }
        public int vehiclecategory { get; set; }
        public string nameandaddress { get; set; }
        public int vehicletype { get; set; }
        //
        public string Endorsements { get; set; }
        public bool isfiberglassfitted { get; set; }
        public bool isautomobileassociation { get; set; }
        public bool isdrivinginstitution { get; set; }
        //
        public decimal? own_damage_value { get; set; }
        public decimal premium_liability_value { get; set; }
        public int vehicle_min_value { get; set; }
        public int depreciation_value { get; set; }
        //
        public decimal? od_gov_discount { get; set; }
        public decimal? mia_own_damage_additional_value { get; set; }
        public decimal? mia_premium_liability_additional_value { get; set; }
        public decimal? liability_gov_discount { get; set; }
        public decimal? driver_amount { get; set; }
        public decimal? passenger_amount { get; set; }
        public int? ph_malus_value { get; set; }
        public int? ph_ncb_value{ get; set; }
        public string policy_number { get; set; }
        public Nullable<double> premium { get; set; }
        public string sanction_date { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string tp_from_date { get; set; }
        public string tp_to_date { get; set; }

        public string insured_declared_value_amount { get; set; }
        public string non_electrical_accessories_amount { get; set; }
        public string electrical_accessories_amount { get; set; }
        public string side_car_trailer_amount { get; set; }
        public string value_of_cng_lpg_amount { get; set; }
        public string total_amount { get; set; }
        public Nullable<int> PolicyMonths { get; set; }
    }
}
