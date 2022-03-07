using KGID_Models.Attrebute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KGID_Models.KGIDMotorInsurance
{
    public class VM_MotorInsuranceOtherDetails
    {
        [Key]
        public long miod_other_details_id { get; set; }
        public Nullable<long> miod_application_id { get; set; }
        public Nullable<long> miod_emp_id { get; set; }
        public Nullable<int> miod_vehicle_type_id { get; set; }
        //public string ephd_height { get; set; }
        //public string ephd_weight { get; set; }
        public string miod_app_ref_id { get; set; }
        //public Nullable<bool> ephd_is_pregnant { get; set; }
        public bool miod_is_driving_tuitions { get; set; }
        public string miod_PageType { get; set; }
        //public Nullable<DateTime> ephd_date_of_last_period { get; set; }

        //public Nullable<bool> ephd_health_condition { get; set; }
        public bool miod_is_non_conventioanal_source { get; set; }
        public string miod_is_non_conventioanal_source_details { get; set; }


        public bool miod_is_own_premises { get; set; }
        public bool miod_is_commercial_purpose { get; set; }
        public bool miod_is_foreign_embasy { get; set; }
        public bool miod_is_vintage_car { get; set; }
        public bool miod_is_for_blind_or_ph { get; set; }
        public bool miod_is_fibre_glass_tank { get; set; }

        public bool miod_is_bi_fuel_system { get; set; }
        public string miod_bi_fuel_amount { get; set; }

        public bool miod_is_higher_deductible { get; set; }
        public string miod_higher_deductible_amount { get; set; }

        public bool miod_is_automobile_association_of_india { get; set; }
        public string miod_name_of_association { get; set; }
        public string miod_membership_no { get; set; }
        public string miod_date_of_expiry { get; set; }

        public bool miod_is_cover_legal_liability { get; set; }
        public string miod_cll_driver_conductor_count { get; set; }
        public string miod_cll_other_emp_count { get; set; }
        public string miod_cll_non_fare_passengers_count { get; set; }

        public bool miod_is_no_claim_bonus { get; set; }
        [ValidateFile]
        public HttpPostedFileBase miod_is_no_claim_bonus_doc { get; set; }
        public string Miod_is_no_claim_bonus_doc_filename { get; set; }
        public string miod_is_no_claim_bonus_details { get; set; }

        public bool miod_is_anti_theft { get; set; }
        [ValidateFile]
        public HttpPostedFileBase miod_is_anti_theft_doc { get; set; }
        public string Miod_is_anti_theft_doc_filename { get; set; }
        public string miod_is_anti_theft_details { get; set; }

        public bool miod_is_liability_third_parties { get; set; }
        public bool miod_is_higher_towing_charges { get; set; }
        public string miod_is_higher_towing_charges_amount { get; set; }


        public bool miod_is_include_personal_accident { get; set; }
        public string miod_pa_driver_conductor_count { get; set; }
        public string miod_pa_other_emp_count { get; set; }
        public string miod_pa_unnamed_passengers_count { get; set; }


        public bool miod_is_include_personal_accident_for_persons { get; set; }
        public string miod_ipap_name1 { get; set; }
        public string miod_ipap_name1_amount { get; set; }
        public string miod_ipap_name2 { get; set; }
        public string miod_ipap_name2_amount { get; set; }
        public string miod_ipap_name3 { get; set; }
        public string miod_ipap_name3_amount { get; set; }


        public bool miod_is_include_pa_cover_for_unnamed_persons { get; set; }
        public string miod_ipaun_name1 { get; set; }
        public string miod_ipaun_name1_amount { get; set; }
        public string miod_ipaun_name2 { get; set; }
        public string miod_ipaun_name2_amount { get; set; }
        public string miod_ipaun_name3 { get; set; }
        public string miod_ipaun_name3_amount { get; set; }

        

        //public bool ephd_is_married { get; set; }
        public bool miod_is_geographical { get; set; }
        public string miod_geographical_ext1 { get; set; }
        public string miod_geographical_ext2 { get; set; }
        public string miod_geographical_ext3 { get; set; }
        //Drop Down List
        public List<SelectListItem> GeographicalExtensionList { get; set; }

        //public Nullable<decimal> ephd_no_of_years_of_marriage { get; set; }
        //public string ephd_husband_occupation_address { get; set; }
        //public bool ephd_active_status { get; set; }
        //public DateTime ephd_creation_datetime { get; set; }
        //public DateTime ephd_updation_datetime { get; set; }
        //public int ephd_created_by { get; set; }
        //public int ephd_updated_by { get; set; }



        //public string PeriodDate { get; set; }

        //public bool IsDrinksDrugs { get; set; }
        //public string DrinksDrugsDetails { get; set; }
        //public HttpPostedFileBase DrinksDrugsDoc { get; set; }
        //public string DrinksDrugsDocFileName { get; set; }

        //public bool IsDiseaseOfLungs { get; set; }
        //public string DiseaseOfLungsDetails { get; set; }
        //public HttpPostedFileBase DiseaseOfLungsDoc { get; set; }
        //public string DiseaseOfLungsDocFileName { get; set; }

        //public HttpPostedFileBase InfectiousDiseaseHouseDoc { get; set; }
        //public string InfectiousDiseaseHouseDocFileName { get; set; }
        //public bool IsInfectiousDiseaseHouse { get; set; }
        //public string InfectiousDiseaseHouseDetails { get; set; }

        //public bool IsLiverKidneyDisease { get; set; }
        //public string LiverKidneyDiseaseDetails { get; set; }
        //public HttpPostedFileBase LiverKidneyDiseaseDoc { get; set; }
        //public string LiverKidneyDiseaseDocFileName { get; set; }

        //public bool IsStomachDisease { get; set; }
        //public string StomachDiseaseDetails { get; set; }
        //public HttpPostedFileBase StomachDiseaseDoc { get; set; }
        //public string StomachDiseaseDocFileName { get; set; }

        //public HttpPostedFileBase FamilyMemberAffectedByDiseaseDoc { get; set; }
        //public string FamilyMemberAffectedByDiseaseDocFileName { get; set; }
        //public bool IsFamilyMemberAffectedByDisease { get; set; }
        //public string FamilyMemberAffectedByDiseaseDetails { get; set; }

        //public bool IsAnyOtherDisease { get; set; }
        //public string AnyOtherDiseaseDetails { get; set; }
        //public HttpPostedFileBase AnyOtherDiseaseDoc { get; set; }
        //public string AnyOtherDiseaseDocFileName { get; set; }

        //public bool IsUrineChecked { get; set; }
        //public string UrineCheckedDetails { get; set; }
        //public HttpPostedFileBase UrineCheckedDoc { get; set; }
        //public string UrineCheckedDocFileName { get; set; }

        //public bool IsRheumaticFever { get; set; }
        //public string RheumaticFeverDetails { get; set; }
        //public HttpPostedFileBase RheumaticFeverDoc { get; set; }
        //public string RheumaticFeverDocFileName { get; set; }

        //public bool IsAbsent { get; set; }
        //public string AbsentDetails { get; set; }
        //public HttpPostedFileBase AbsentDoc { get; set; }
        //public string AbsentDocFileName { get; set; }

        //public bool IsPlaceChange { get; set; }
        //public string PlaceChangeDetails { get; set; }
        //public HttpPostedFileBase PlaceChangeDoc { get; set; }
        //public string PlaceChangeDocFileName { get; set; }

        //public bool IsProposalMade { get; set; }
        //public bool ProposalAccepted { get; set; }
        //public string ProposalAcceptedDetails { get; set; }
        //public bool ProposalPostponed { get; set; }
        //public string ProposalPostponedDetails { get; set; }
        //public bool ProposalDeclined { get; set; }
        //public string ProposalDeclinedDetails { get; set; }
        //public string OrganizationOrPolicyNumber { get; set; }
        //public string PolicyOrProposalNumber { get; set; }
        //public HttpPostedFileBase ProposalDoc { get; set; }
        //public string ProposalDocFileName { get; set; }

        //public bool IsExaminedBefore { get; set; }
        //public string ExaminedPremiumAmountDetails { get; set; }
        //public string ExaminedPolicyNumberDetails { get; set; }
        //public HttpPostedFileBase ExaminedDoc { get; set; }
        //public string ExaminedDocFileName { get; set; }

        //public string InsuranceProposalDetails { get; set; }
        //public HttpPostedFileBase InsuranceProposalDoc { get; set; }
        //public string InsuranceProposalDocFileName { get; set; }

        //public HttpPostedFileBase MedicalAdviceDoc { get; set; }
        //public string MedicalAdviceDocFileName { get; set; }
        //public bool IsMedicalAdvice { get; set; }
        //public string MedicalAdviceDetails { get; set; }

        //public bool IsBrainDisease { get; set; }
        //public string BrainDiseaseDetails { get; set; }
        //public HttpPostedFileBase BrainDiseaseDoc { get; set; }
        //public string BrainDiseaseDocFileName { get; set; }

    }
    public class MIOtherDetailsData
    {
        public long vod_application_id { get; set; }
        public long vod_emp_id { get; set; }
        public bool vod_status { get; set; }
        public bool vod_active { get; set; }
        //public DateTime vod_creation_datetime { get; set; }
        //public DateTime vod_updation_datetime { get; set; }
        //public int vod_created_by { get; set; }
        //public int vod_updated_by { get; set; }
    }
    public class MIOtherDetailsResponseData
    {
        public long vdr_application_id { get; set; } 
        public long vdr_emp_id { get; set; }
        public long vdr_other_details_id { get; set; }
        public bool vdr_response { get; set; }
        public string vdr_response_details1 { get; set; }
        public string vdr_response_details2 { get; set; }
        public string vdr_response_details3 { get; set; }
        public string vdr_response_details4 { get; set; }
        public string vdr_response_details5 { get; set; }
        public string vdr_response_details6 { get; set; }
        public bool vdr_status { get; set; }
        public bool vdr_active { get; set; }
        //public DateTime vdr_creation_datetime { get; set; }
        //public DateTime vdr_updation_datetime { get; set; }
        //public int vdr_created_by { get; set; }
        //public int vdr_updated_by { get; set; }
    }
}
