using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class VM_MB_Bond_Details
    {
        //Policy details
        public string SectionCode { get; set; }
        public string CertificateNo { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyType { get; set; }
        public string ChallanNo { get; set; }
       
        public string Sanction_Date { get; set; }
        public string ProposerNameAddress { get; set; }
        public Nullable<double> PolicyPremiumAmount { get; set; }
     
        public string ODFrom_Date { get; set; }
       
        public string ODTo_Date { get; set; }
       
        public string TPFrom_Date { get; set; }
        
        public string TPTo_Date { get; set; }

        //
        //Vehicle Details
        public string Regisration_no { get; set; }
        public string Make_of_vehicle { get; set; }

        public string vehicle_class{ get; set; }
        public int Year_of_manufacturer { get; set; }
        public Nullable<int> Cubic_capacity { get; set; }
        public string GVW { get; set; }
        public Nullable<int> SeatingCapacity { get; set; }
        public string Engine_no { get; set; }
        public string Chasis_no { get; set; }
        public string Zone { get; set; }
        public int VehicleCategory { get; set; }
        /// UnUsed<summary>
        public long Motor_insurance_app_id { get; set; }
        public long Application_ref_no { get; set; }
        public long Proposer_id { get; set; }
        public decimal Total_idv_amount { get; set; }
        public string Registration_authority_and_location { get; set; }
        /// </summary>

        //IDV Data
        public string Insured_declared_value_amount { get; set; }
        public string Non_electrical_accessories_amount { get; set; }
        public string Electrical_accessories_amount { get; set; }
        public string Side_car_trailer_amount { get; set; }
        public string Value_of_cng_lpg_amount { get; set; }
        public string Depreciation { get; set; }
        public string Idv_total_amount { get; set; }

        //A. Own Damage
        //Policy Calculation Details
        public string BP { get; set; }
        public string ODPercentage { get; set; }
        public string ODPercentageValue { get; set; }
        public string ODSubTotal1 { get; set; }
        public string ODPremium { get; set; }
        public string GVWExtraAmount { get; set; }
        public string ODGovtRebate { get; set; }
        public string ODGovtRebateValue { get; set; }
        public string ODSubTotal2 { get; set; }
        public string EAPercentage { get; set; }
        public string EAPercentageValue { get; set; }
        public string ODLPGKitPercentage { get; set; }
        public string ODLPGKitValue { get; set; }
        public string ImportedPercentage { get; set; }
        public string ImportedVehiclesValue { get; set; }
        public string FibreGlassFuelTank { get; set; }
        public string ODDrivingInstitutionPercentage { get; set; }
        public string ODDrivingInstitutionPercentageValue { get; set; }
        public string ODAddOtherPercentage { get; set; }
        public string ODAddOtherPercentageValue { get; set; }
        public string ODSubTotal3 { get; set; }
        public string AutomobilePercentage { get; set; }
        public string AutomobileValue { get; set; }
        public string HandicappedPercentage { get; set; }
        public string HandicappedValue { get; set; }
        public string AntiTheftDevicePercentage { get; set; }
        public string AntiTheftDeviceValue { get; set; }
        public string ODSubTotal4 { get; set; }
        public string ODMalus { get; set; }
        public string ODMalusValue { get; set; }
        public string ODNCB { get; set; }
        public string ODNCBValue { get; set; }
        public string ODOthersPercentage { get; set; }
        public string ODOthersValue { get; set; }
        public string ODTotal { get; set; }




        public string txtbpidvValue { get; set; }
        //public string res { get; set; }
        public string txtlgrodValue { get; set; }
        //public string res1 { get; set; }
        public string txtrebatetotodvalue { get; set; }
        public string txtsubtotlpgodValue { get; set; }
        // public string txtsubtotextraVlaue { get; set; }
        public string txtlessncbValue { get; set; }
        //public string res3 { get; set; }
        public string txtodtotValue { get; set; }
        //public string res4 { get; set; }
        // public string txtlprValue { get; set; }
        // public string res5 { get; set; }
        //B. LIABILITY TO PUBLIC RISK
        public string LPRValue { get; set; }
        public string LPRGovtRebate { get; set; }
        public string LPRGovtRebateValue { get; set; }
        public string LPRSubTotal1 { get; set; }
        public string LPRDrivingInstitutionPercentage { get; set; }
        public string LPRDrivingInstitutionPercentageValue { get; set; }
        public string LPRLPGKitPercentage { get; set; }
        public string LPRLPGKitValue { get; set; }
        public string LPRSubTotal2 { get; set; }
        public string DriverRisk { get; set; }
        public string PillionRisk { get; set; }
        public string PassengersRisk { get; set; }
        public string CleanersRisk { get; set; }
        public string CooliesRisk { get; set; }
        public string LPRMalus { get; set; }
        public string LPRMalusValue { get; set; }
        public string LPRTotal { get; set; }

        public string txtlgrlprValue { get; set; }
        //public string res6 { get; set; }
        public string txtsubtotlprValue { get; set; }
        //public string res7 { get; set; }
        //public string txtcngamntrValue { get; set; }

        //public string txtlpgkitlprValue { get; set; }
        //public string res8 { get; set; }
        //public string txtsubtotlpglprValue { get; set; }
        //public string res9 { get; set; }
        public string res10 { get; set; }
        public string res11 { get; set; }
        public string txtlprtotValue { get; set; }
        //public string res12 { get; set; }
        public string txttotABValue { get; set; }
        //public string res13 { get; set; }

        public string txtgstamtValue { get; set; }
        //public string res14 { get; set; }
        public string txttotalcrpremiumValue { get; set; }
        //public string res15 { get; set; }
        /////////C.TAX Final Premium Calculation
        public string TotalA { get; set; }
        public string TotalB { get; set; }
        public string TotalAB { get; set; }
        public string PreviousYearDifference { get; set; }
        public string CurrentYearDifference { get; set; }
        public string Premium { get; set; }
        public string SGSTofPremium { get; set; }
        public string CGSTofPremium { get; set; }
        public string GSTofPremium { get; set; }
        public string FinalAmount { get; set; }
        public string PayablePremium { get; set; }
        public string Endorsements { get; set; }
        //End
    }
}
