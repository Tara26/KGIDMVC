using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class VM_MotorInsuranceIDVDetails
    {
        public long miidv_idv_id { get; set; }
        public Nullable<long> miidv_application_id { get; set; }
        public Nullable<long> miidv_emp_id { get; set; }
        public Nullable<decimal> premium_amount { get; set; }//PremiumAmountValue
        //public bool miidv_is_insured_declared_value { get; set; }
        public string miidv_vaahanidvamount { get; set; }
        public string miidv_insured_declared_value_amount { get; set; }
        public string miidv_insured_redeclared_value_amount { get; set; }
        public string miidv_non_electrical_accessories_amount { get; set; }
        public string miidv_electrical_accessories_amount { get; set; }
        public string miidv_side_car_trailer_amount { get; set; }
        public string miidv_value_of_cng_lpg_amount { get; set; }
        public string miidv_total_amount { get; set; }
        //public decimal midv_own_damage_value { get; set; }
        public string miidv_pagetype { get; set; }
        public string miidv_app_ref_id { get; set; }
        public long miidv_idv_premium_amount { get; set; }


    }
}
