using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace KGID_Models.NBApplication
{
    public class VM_PersonalHealthDetails
    {
        [Key]
        public long ephd_personal_health_details_id { get; set; }
        public Nullable<long> ephd_application_id { get; set; }
        public Nullable<long> ephd_emp_id { get; set; }
        public Nullable<int> ephd_gender_id { get; set; }
        public string ephd_height { get; set; }
        public string ephd_weight { get; set; }
        public Nullable<bool> ephd_is_pregnant { get; set; }
        public Nullable<DateTime> ephd_date_of_last_period { get; set; }
        public Nullable<bool> ephd_health_condition { get; set; }
     //   public Nullable<decimal> ephd_no_of_years_of_marriage { get; set; }
    //    public string ephd_husband_occupation_address { get; set; }
        public bool ephd_active_status { get; set; }
        public DateTime ephd_creation_datetime { get; set; }
        public DateTime ephd_updation_datetime { get; set; }
        public int ephd_created_by { get; set; }
        public int ephd_updated_by { get; set; }
      //  public bool ephd_is_married { get; set; }
        public string PeriodDate { get; set; }
      //  public string ephd_husbandGovEmp { get; set; }
      //  public string ephd_insuredOfficialBrch { get; set; }
      //  public Nullable<bool> IsinsuredOfficialBrch { get; set; }
      //  public string ephd_husbandLIC { get; set; }
      //  public Nullable<bool> IshusbandLIC { get; set; }
      //  public Nullable<bool> ephd_menstruating { get; set; }
      //  public Nullable<DateTime> ephd_lastmensuration { get; set; }
      //  public string lastmensuration { get; set; }
     //   public int ephd_nopregnancies { get; set; }
     //   public string ephd_gonefulltime { get; set; }
      //  public string dateoflastdelivery { get; set; }

       // public Nullable<DateTime> ephd_dateoflastdelivery { get; set; }
       // public string ephd_miscarriages { get; set; }

        public Nullable<bool> IsDrinksDrugs { get; set; }
        public string DrinksDrugsDetails { get; set; }
        public HttpPostedFileBase DrinksDrugsDoc { get; set; }
        public string DrinksDrugsDocFileName { get; set; }

        //public bool IsDiseaseOfLungs { get; set; }
        //public string DiseaseOfLungsDetails { get; set; }
        //public HttpPostedFileBase DiseaseOfLungsDoc { get; set; }
        //public string DiseaseOfLungsDocFileName { get; set; }

        public HttpPostedFileBase InfectiousDiseaseHouseDoc { get; set; }
        public string InfectiousDiseaseHouseDocFileName { get; set; }
        public Nullable<bool> IsInfectiousDiseaseHouse { get; set; }
        public string InfectiousDiseaseHouseDetails { get; set; }

        //public bool IsLiverKidneyDisease { get; set; }
        //public string LiverKidneyDiseaseDetails { get; set; }
        //public HttpPostedFileBase LiverKidneyDiseaseDoc { get; set; }
        //public string LiverKidneyDiseaseDocFileName { get; set; }

        public Nullable<bool> IsStomachDisease { get; set; }
        public string StomachDiseaseDetails { get; set; }
        public HttpPostedFileBase StomachDiseaseDoc { get; set; }
        public string StomachDiseaseDocFileName { get; set; }

        //public HttpPostedFileBase FamilyMemberAffectedByDiseaseDoc { get; set; }
        //public string FamilyMemberAffectedByDiseaseDocFileName { get; set; }
        //public bool IsFamilyMemberAffectedByDisease { get; set; }
        //public string FamilyMemberAffectedByDiseaseDetails { get; set; }

        public Nullable<bool> IsAnyOtherDisease { get; set; }
        public string AnyOtherDiseaseDetails { get; set; }
        public HttpPostedFileBase AnyOtherDiseaseDoc { get; set; }
        public string AnyOtherDiseaseDocFileName { get; set; }

        //public bool IsUrineChecked { get; set; }
        //public string UrineCheckedDetails { get; set; }
        //public HttpPostedFileBase UrineCheckedDoc { get; set; }
        //public string UrineCheckedDocFileName { get; set; }

        //public bool IsRheumaticFever { get; set; }
        //public string RheumaticFeverDetails { get; set; }
        //public HttpPostedFileBase RheumaticFeverDoc { get; set; }
        //public string RheumaticFeverDocFileName { get; set; }

        public Nullable<bool> IsAbsent { get; set; }
        public string AbsentDetails { get; set; }
        public HttpPostedFileBase AbsentDoc { get; set; }
        public string AbsentDocFileName { get; set; }

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

    public class VM_PersonalMedicalHealthDetails
    {
        public int personalhealthid { get; set; }
        public string personalhealthdesc { get; set; }
        public string remarks { get; set; }
        public bool status { get; set; }
        public string documentpath { get; set; }
    }
    public class PersonalData
    {
        public Nullable<long> ephd_application_id { get; set; }
        public Nullable<long> ephd_emp_id { get; set; }
        public string ephd_height { get; set; }
        public string ephd_weight { get; set; }
        public bool ephd_is_pregnant { get; set; }
        public bool ephd_is_married { get; set; }
        public Nullable<DateTime> ephd_date_of_last_period { get; set; }
        public bool ephd_health_condition { get; set; }
        public decimal ephd_no_of_years_of_marriage { get; set; }
        public string ephd_husband_occupation_address { get; set; }
        
        public bool ephd_active_status { get; set; }  
        public Nullable<bool> ephd_husbandGovEmp { get; set; }
        public string ephd_insuredOfficialBrch { get; set; }
        public string ephd_husbandLIC { get; set; }
        public Nullable<bool> ephd_menstruating { get; set; }
        public Nullable<DateTime> ephd_lastmensuration { get; set; }
        
        public int ephd_nopregnancies { get; set; }
        public string ephd_miscarriages { get; set; }
        public Nullable<DateTime> ephd_dateoflastdelivery { get; set; }
        public string ephd_gonefulltime { get; set; }
      
        public Nullable<bool> ephd_IsInsuredOfficialBrch { get; set; }
        public Nullable<bool> ephd_IshusbandLIC { get; set; }

    }
    public class DiseaseData
    {
        public long epdd_application_id { get; set; }
        public long epdd_emp_id { get; set; }
        public int epdd_personal_health_id { get; set; }
	    public bool epdd_status { get; set; }
        public string epdd_remarks { get; set; }
        public string epdd_upload_document_path { get; set; }
    }

    public class tbl_personal_health_disease_master
    {
        [Key]
        public int phdm_personal_health_id { get; set; }
        public string phdm_personal_health_desc { get; set; }
        public int phdm_active_status { get; set; }
        public bool phdm_active { get; set; }
        public DateTime phdm_creation_datetime { get; set; }
        public DateTime phdm_updation_datetime { get; set; }
        public int phdm_created_by { get; set; }
        public int phdm_updated_by { get; set; }
    }
    public class tbl_employee_personal_diesase_details
    {
        [Key]
        public long epdd_personal_disease_id { get; set; }
        public long epdd_application_id { get; set; }
        public long epdd_emp_id { get; set; }
        public long epdd_personal_health_id { get; set; }
        public string epdd_remarks { get; set; }
        public string epdd_upload_document_path { get; set; }
        public bool epdd_status { get; set; }
        public bool epdd_active_status { get; set; }
        public DateTime epdd_creation_datetime { get; set; }
        public DateTime epdd_updation_datetime { get; set; }
        public int epdd_created_by { get; set; }
        public int epdd_updated_by { get; set; }
    }
}
