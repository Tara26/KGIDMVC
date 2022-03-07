using KGID_Models.KGIDNBApplication;
using System.Web;

namespace KGID_Models.KGIDEmployee
{
    public class VM_PersonalDetails
    {
        public bool IsDrinksDrugs { get; set; }
        public string DrinksDrugsDetails { get; set; }
        public HttpPostedFileBase DrinksDrugsDoc { get; set; }
        public string DrinksDrugsDocFileName { get; set; }

        public bool IsDiseaseOfLungs { get; set; }
        public string DiseaseOfLungsDetails { get; set; }
        public HttpPostedFileBase DiseaseOfLungsDoc { get; set; }
        public string DiseaseOfLungsDocFileName { get; set; }

        public HttpPostedFileBase InfectiousDiseaseHouseDoc { get; set; }
        public string InfectiousDiseaseHouseDocFileName { get; set; }
        public bool IsInfectiousDiseaseHouse { get; set; }
        public string InfectiousDiseaseHouseDetails { get; set; }

        public bool IsLiverKidneyDisease { get; set; }
        public string LiverKidneyDiseaseDetails { get; set; }
        public HttpPostedFileBase LiverKidneyDiseaseDoc { get; set; }
        public string LiverKidneyDiseaseDocFileName { get; set; }

        public bool IsStomachDisease { get; set; }
        public string StomachDiseaseDetails { get; set; }
        public HttpPostedFileBase StomachDiseaseDoc { get; set; }
        public string StomachDiseaseDocFileName { get; set; }

        public HttpPostedFileBase FamilyMemberAffectedByDiseaseDoc { get; set; }
        public string FamilyMemberAffectedByDiseaseDocFileName { get; set; }
        public bool IsFamilyMemberAffectedByDisease { get; set; }
        public string FamilyMemberAffectedByDiseaseDetails { get; set; }

        public bool IsAnyOtherDisease { get; set; }
        public string AnyOtherDiseaseDetails { get; set; }
        public HttpPostedFileBase AnyOtherDiseaseDoc { get; set; }
        public string AnyOtherDiseaseDocFileName { get; set; }

        public bool IsUrineChecked { get; set; }
        public string UrineCheckedDetails { get; set; }
        public HttpPostedFileBase UrineCheckedDoc { get; set; }
        public string UrineCheckedDocFileName { get; set; }

        public bool IsRheumaticFever { get; set; }
        public string RheumaticFeverDetails { get; set; }
        public HttpPostedFileBase RheumaticFeverDoc { get; set; }
        public string RheumaticFeverDocFileName { get; set; }

        public bool IsAbsent { get; set; }
        public string AbsentDetails { get; set; }
        public HttpPostedFileBase AbsentDoc { get; set; }
        public string AbsentDocFileName { get; set; }

        public bool IsPlaceChange { get; set; }
        public string PlaceChangeDetails { get; set; }
        public HttpPostedFileBase PlaceChangeDoc { get; set; }
        public string PlaceChangeDocFileName { get; set; }

        public bool IsProposalMade { get; set; }
        public bool ProposalAccepted { get; set; }
        public string ProposalAcceptedDetails { get; set; }
        public bool ProposalPostponed { get; set; }
        public string ProposalPostponedDetails { get; set; }
        public bool ProposalDeclined { get; set; }
        public string ProposalDeclinedDetails { get; set; }
        public string OrganizationOrPolicyNumber { get; set; }
        public string PolicyOrProposalNumber { get; set; }
        public HttpPostedFileBase ProposalDoc { get; set; }
        public string ProposalDocFileName { get; set; }

        public bool IsExaminedBefore { get; set; }
        public string ExaminedPremiumAmountDetails { get; set; }
        public string ExaminedPolicyNumberDetails { get; set; }
        public HttpPostedFileBase ExaminedDoc { get; set; }
        public string ExaminedDocFileName { get; set; }

        public bool IsPurdha { get; set; }
        public string PurdhaDetails { get; set; }
        public bool IsMarried { get; set; }
        public string MarriedTenureDetails { get; set; }
        public bool IsHusbandKGWorker { get; set; }
        public string OccupationAndAddress { get; set; }

        public string InsuranceProposalDetails { get; set; }
        public HttpPostedFileBase InsuranceProposalDoc { get; set; }
        public string InsuranceProposalDocFileName { get; set; }

        public tbl_personal_disease_details PersonalDisease { get; set; }
        //public string phd_Health_Remarks { get; set; }
        //public Nullable<bool> phd_active { get; set; }
        //public Nullable<DateTime> phd_creation_datetime { get; set; }
        //public tbl_nb_personal_health PersonalHealth { get; set; }
        public tbl_personal_health_details PersonalHealth { get; set; }

        public long EmpCode { get; set; }

        public HttpPostedFileBase MedicalAdviceDoc { get; set; }
        public string MedicalAdviceDocFileName { get; set; }
        public bool IsMedicalAdvice { get; set; }
        public string MedicalAdviceDetails { get; set; }

        public bool IsBrainDisease { get; set; }
        public string BrainDiseaseDetails { get; set; }
        public HttpPostedFileBase BrainDiseaseDoc { get; set; }
        public string BrainDiseaseDocFileName { get; set; }

        //public string phd_health_code { get; set; }
        //public Nullable<bool> phd_health_condition { get; set; }
        //public string phd_treatment_details { get; set; }
        //public string phd_Height { get; set; }
        //public string phd_Weight { get; set; }
        //public Nullable<DateTime> phd_period_date { get; set; }
        //public Nullable<bool> phd_Pregnant { get; set; }
    }
}