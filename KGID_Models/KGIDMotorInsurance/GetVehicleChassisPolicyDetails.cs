using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KGID_Models.KGIDMotorInsurance
{
    public class GetVehicleChassisPolicyDetails
    {
        public int policy_motor_insurace_emp_id { get; set; }
        public int mia_application_ref_no { get; set; }
        public bool mia_active { get; set; }
        public int mia_user_category { get; set; }
        public DateTime? OD_from_date { get; set; }
        public string OD_from_date1 { get; set; }
        public DateTime? OD_to_date { get; set; }
        public string OD_to_date1 { get; set; }
        public DateTime? TP_from_date { get; set; }
        public string TP_from_date1 { get; set; }
        public DateTime? TP_to_date { get; set; }
        public string TP_to_date1 { get; set; }
        public string vehicle_model { get; set; }
        public string type_of_Cover { get; set; }
        public string vehicle_registration_no { get; set; }
        public DateTime mivd_Date_Of_Manufacturer { get; set; }
        public int cubic_capacity { get; set; }
        public int seating_capacity_including_driver { get; set; }
        public string vehicle_chasis_no { get; set; }
        public string Vehicle_Type { get; set; }
        public string Vehicle_Category_Type { get; set; }
        public string vehicle_category_desc { get; set; }
        public string Policy_number { get; set; }

        public List<GetVehicleChassisPolicyDetails> GetVehicleChassisPolicyDetailsList { get; set; }
        public GetVehicleChassisPolicyDetails VehicleChassisPolicyDetailsList { get; set; }
        public SelectList TalukaList { get; set; }
        public SelectList DistrictList { get; set; }
        public int District_dm_id { get; set; }
        public string Court_District_Name { get; set; }
        public string Court_Taluk_Name { get; set; }
        public string Court_state_name { get; set; }
        public int District_dm_id1 { get; set; }
        public int Taluk_id { get; set; }
        public int Taluk_id1 { get; set; }
         public SelectList RemarksList { get; set; }
        public int Remarks_id { get; set; }
        public string MVC_number { get; set; }
        public DateTime Court_DateTime { get; set; }
        public string  Name_of_court { get; set; }
        public long pincode_Of_Petitioner { get; set; }
        public string name_Of_Petitioner { get; set; }
        public string petitioner_Address { get; set; }
        public long petitioner_Mobile_no { get; set; }
        public string Respondant_name { get; set; }
        public string Respondant_designation { get; set; }
        public string Respondant_department { get; set; }
        public string Respondant_Agency_name { get; set; }
        public string Respondant_address { get; set; }
        public string Respondant_mobile { get; set; }
        public string Respondant_pincode { get; set; }
        public int Accident_district { get; set; }
        public string Accident_district_name { get; set; }
        public int Accident_taluk { get; set; }
        public string Accident_taluk_name { get; set; }
        public string Accident_hobli { get; set; }
        public string Accident_gramPanchayat { get; set; }
        public string Accident_village { get; set; }
        public string Accident_details { get; set; }
        public decimal Accident_claim_amnt { get; set; }
        public string Accident_rc_details { get; set; }
        public string Accident_fir_details { get; set; }
        public string Accident_dl_details { get; set; }
        public string PreClaimedForm { get; set; }
        public string insurancecopy { get; set; }
        public string Accident_panchnama_details { get; set; }
        public string Accident_object_statement_details { get; set; }
        public string summons_detals { get; set; }
        public string petitioner_details { get; set; }
        public string petitioner_name { get; set; }
        public string CoveringLetter { get; set; }
        public string Prefilled_Claim_Form { get; set; }
        public string Insurance_Copy { get; set; }
        public string DL { get; set; }
        public string DriverstatementandRc { get; set; }
        

        public string CourtTime { get; set; }
        public string claim_Amount { get; set; }
        public long loginId { get; set; }
        public long MVC_claim_app_id { get; set; }

        public long roleID { get; set; }
        public string Vehicle_Registration_Number { get; set; }
        public string Comments_details { get; set; }
        public int Category_id { get; set; }
        public HttpPostedFileBase UploadPdf { get; set; }
        public HttpPostedFileBase UploadDoc { get; set; }

        public List<GetVehicleChassisPolicyDetails> PetitionerAndRspondantDetailsList { get; set; }
        public List<GetVehicleChassisPolicyDetails> PetitionerList { get; set; }
        public List<GetVehicleChassisPolicyDetails> RespondantList { get; set; }
        public List<GetVehicleChassisPolicyDetails> otherDocuments { get; set; }
    
        public List<GetVehicleChassisPolicyDetails> MVCAppDocDetails { get; set; }
        public List<GetVehicleChassisPolicyDetails> CourtDetailsList { get; set; }

        public List<GetVehicleChassisPolicyDetails> SignedDocList { get; set; }
        public List<GetVehicleChassisPolicyDetails> otherDetailsData { get; set; }
        public List<GetVehicleChassisPolicyDetails> MvcClaimWorkFlowDetails { get; set; }
        public List<GetVehicleChassisPolicyDetails> CourtExecutionMasterDetails { get; set; }
        public List<GetVehicleChassisPolicyDetails> Lok_DocDetails { get; set; }
        public List<GetVehicleChassisPolicyDetails> GetWorkFlowLokList { get; set; }
        public List<GetVehicleChassisPolicyDetails> LokadalathDetails { get; set; }
        public List<GetVehicleChassisPolicyDetails> LokadhalatMasterDetails { get; set; }
        public SelectList JudgementRemarksList { get; set; }
        public SelectList RatificationRemarksList { get; set; }
        public SelectList DelayNoteRemarksList { get; set; }
        public List<SelectListItem> OpinionStatusLokadhalat { get; set; }
        public string Court_MVC_number { get; set; }
        public string Lok_doc_Details { get; set; }
        public string OtherDocument { get; set; }
        public DateTime Accident_Time { get; set; }
        public string owner_name_vehicle { get; set; }
        public int cubic_capacity_vehicle { get; set; }
        public string owner_name_vehicle_address { get; set; }
        public int seating_capacity_vehicle { get; set; }

        public string Vehicle_subtype_desc { get; set; }

        public string vehicle_make_desc { get; set; }

        public string Name_of_injured { get; set; }
        public string Father_name { get; set; }
        public string Spouse_name { get; set; }
        public string Address_of_dead_details { get; set; }
        public int Age_of_injured { get; set; }
        public string occupation_of_injured { get; set; }
        public string employer_deceased_details { get; set; }
        public string monthly_income_of_injured { get; set; }
        public string income_tax_of_injured { get; set; }
        public string place_of_accident { get; set; }
        public string accident_DateTime { get; set; }
        public string police_station_of_jurisdiction { get; set; }
       public string police_station_of_compensation { get; set; }
       public int type_injury{ get; set; }
        public string nature_of_injuries_sustained { get; set; }
        public string medical_officer { get; set; }
        public string Period_of_treatment_of_details { get; set; }
        public string Name_of_injury_caused_of_details { get; set; }
        public string Name_and_address_of_applicant_details { get; set; }
        public string relation_with_deceased { get; set; }
        public string title_property_deceased { get; set; }

        public string any_other_information_details { get; set; }

        public string injury_desc { get; set; }
        public int stateID { get; set; }
        public int injury_id { get; set; }
        public SelectList InjuryList { get; set; }
        public SelectList StateList { get; set; }

        public DateTime SubmissionDate { get; set; }
        public string From { get; set; }
        public string Tooo { get; set; }
        public long TO { get; set; }

        public string Remarks { get; set; }

        public string comments { get; set; }
        public  long ByID { get; set; }

        public int application_stat { get; set; }
        public string other_state_court_taluk { get; set; }
        public string other_state_court_dist { get; set; }
        public string cover_letter { get; set; }
        public long scrutinyStatus { get; set; }

        public string authorization_letter { get; set; }
        public int authorization_check { get; set; }
        public string court_parawise { get; set; }

        public long Doc_ref_id { get; set; }
        public string SavePath { get; set; }
        public string filename { get; set; }
        public string RatificationToLawDept { get; set; }
        public string RatificationToKgid { get; set; }
        public List<GetVehicleChassisPolicyDetails> GetDocumentRemarksList { get; set; }
        public string DocFileVariable { get; set; }
        public string dist_id12 { get; set; }
        public string Taluk_id12 { get; set; }
        public string CourtTime2 { get; set; }
        public string CourtTime3 { get; set; }
        public string LowerCourtJudgementCopy { get; set; }
        public string opinionLawfromLawDept { get; set; }
        public SelectList OpinionStatusList { get; set; }
        public SelectList ObjectStatementRemarkList { get; set; }
        public SelectList PaymentRemarkList { get; set; }
        public SelectList OpinionStatusList2 { get; set; }
        public int OpinionId { get; set; }
        public int OpinionId3 { get; set; }
        public int Remarks_id2 { get; set; }
        public string opinionDesc { get; set; }
        public string LowerCourtJudgementDate { get; set; }
        public string awardedAmntLowCourt { get; set; }

        public string claim_settle_awardedAmt { get; set; }
        public string claim_petitionDate { get; set; }
        public string Claim_settle_awardedInterest { get; set; }
        public string Claim_settle_courtcost { get; set; }
        public string Claim_settle_AwardedTotalAmnt { get; set; }
        public string DelayNoteHighCourt { get; set; }
        public string CondonationOfDelay { get; set; }
        public string StayAffidavitHighCourt { get; set; }
        public string GroundsofAppeal { get; set; }
        public string AmtDepositHC { get; set; }
        public string AmtDepositLC { get; set; }
        public string HighCourtJudgementOpinion { get; set; }
        public int? HighCourtOpinionID { get; set; }
        public string HighCourtjudgementDate { get; set; }
        public string HighCourtAwardedAmount { get; set; }
        public string HighCourtOpinionDesc { get; set; }
        public string HighCourstatutoryAmount { get; set; }
        public string HighCourtstatementRemittedDate { get; set; }
        public string HighCourtDepositAmnt { get; set; }
        public string HighCourtDepositAmntRemittedDate { get; set; }
        public string HighCourtClaimAwardedAmnt { get; set; }
        public string HighCourtClaimAwardedInterest { get; set; }
        public string HighCourtClaimSettleCost { get; set; }
        public string HighCourtClaimSettleTotalAmnt { get; set; }
        public int? OpinionId2 { get; set; }
        public string HighCourtOpinionDesc2 { get; set; }
        public string HighCourtJudgementDateKGID { get; set; }
        public string HighCourtAwardedAmountKGID { get; set; }
        public string HighCourtOpinionJudgement2 { get; set; }
		
		//chethan
		 public string Court_ExecutionDetails { get; set; }
         public int Tooooo { get; set; }
         public string Execution_notice { get; set; }
        public string judgement_Copy { get; set; }
        public int doc_len { get; set; }
        public string lok_mvc_ref_no { get; set; }
        public string lok_date { get; set; }
        public long court_ref_no { get; set; }
        public long created_by { get; set; }
    public List<GetVehicleChassisPolicyDetails> GetWorkFlowCOurtExecutionList { get; set; }
          public List<GetVehicleChassisPolicyDetails> CE_DocDetails { get; set; }

        public string awardedAmount_highCourtClaimSttleKGID { get; set; }

        public string awardedInterest_highCourtClaimSttleKGID { get; set; }
        public string courtCost_highCourtClaimSttleKGID { get; set; }
        public string totalAmnt_highCourtClaimSttleKGID { get; set; }
        public string InputDelaySupremeCourt { get; set; }
        public string CondonationOfDelaySupremeC { get; set; }
        public string StayAffidavitSupremeC { get; set; }
        public string GroundsofAppealSupremeC { get; set; }
        public string AmountDepositiontoSupremeCfile { get; set; }
        public string AmountDepositiontoSupremeCToLCfile { get; set; }
        public string IntimationToDistrictCToLCfile { get; set; }
        public string StayOrderToToDistrictSupremeCToLCfile { get; set; }
        public string SupremeJudgementOpiniondate { get; set; }
        public int? SupremeOpinionId { get; set; }
        public string SupremeAwardedAmnt { get; set; }
        public string SupremeOpinionDesc { get; set; }
        public string Supreme_Statuatory_Amount { get; set; }
        public string Statuatory_Amount_Remitted { get; set; }
        public string Supreme_DepositAmount { get; set; }
        public string Supreme_Deposit_Amount_Remitted { get; set; }
        public string Supreme_Awarded_Amount { get; set; }
        public string Supreme_Awarded_Interest { get; set; }
        public string Supreme_Court_Cost { get; set; }
        public string Supreme_Total_Amount { get; set; }
        public bool? statutorypaidStatus { get; set; }
        public string mvc_JudgementSupreme_date2 { get; set; }
        public int? mvc_opinionSupremeStatusID2 { get; set; }
        public string mvc_awardedSupreme_amount2 { get; set; }
        public string mvc_opinionSupremeStatusID2Desc { get; set; }
        public string awardedAmount_supremeCourtKGID { get; set; }
        public string awardedInterest_supremeCourtKGID { get; set; }
        public string courtcost_supremeCourtKGID { get; set; }
        public string TotalAmount_supremeCourtKGID { get; set; }
        public DateTime? Lokadalath_view_date { get; set; }
        public int lokOpinionId { get; set; }
        public int? OpinionIdSupreme { get; set; }
        public string OpinionIdSupremeDesc { get; set; }
        public string Supreme_Awarded_Amount2 { get; set; }
        public string Supreme_judgement_date2 { get; set; }
        public string Supreme_Awarded_Amount_Claims { get; set; }
        public string Supreme_Awarded_intrest_Claims { get; set; }
        public string Supreme_Awarded_court_Claims { get; set; }
        public string Supreme_Awarded_totalAmount_Claims { get; set; }
        public string judgement_Copy_supreme { get; set; }
        public long PreClaimedFormId { get; set; }
        public long Accident_fir_detailsId { get; set; }
        public long cover_letterId { get; set; }
        public long insurancecopyId { get; set; }
        public long Accident_dl_detailsId { get; set; }
        public long court_ParawiseId { get; set; }
        public long object_statementId { get; set; }
        public long ratificationToLawDeptID { get; set; }
        public long HighCourtAuthorizationLetterId { get; set; }
        public long DelayNoteHighCourtID { get; set; }
        public long SupremeDelayNotetoGovtAdvocateSupremeCourtID { get; set; }
        public long SupremeCourtAuthorizationLetterID { get; set; }
        public string Signed_document { get; set; }
        public string CourtSigned_parawise { get; set; }
        public string Signedobject_statement { get; set; }
        public string SignedRatificationToLawDept { get; set; }
        public string SignedDelayNoteHighCourt { get; set; }
        public string SignedHighCourtAuthorizationLetter { get; set; }
        public string SignedSupremeCourtAuthorizationLetter { get; set; }
        public string SignedDelayNotetoGovtAdvocateSupremeCourt { get; set; }
        public string SignedPreClaimedForm { get; set; }
        public string SignedDsRc { get; set; }
        public string SignedCovering_Letter { get; set; }
        public string SignedInsuranceCopy { get; set; }
        public string SignedAccident_dl_details { get; set; }
    }
    public class OtherDocumentS {
        public string OtherDocument { get; set; }

    }
    public class PetitionerData {
        public string name_Of_Petitioner { get; set; }
        public string pincode_Of_Petitioner { get; set; }
        public string petitioner_Address { get; set; }
        public string petitioner_Mobile_no { get; set; }

     
    } public class RespondData {
        public string Respondant_name { get; set; }
        public string Respondant_designation { get; set; }
        public string Respondant_department { get; set; }
        public string Respondant_Agency_name { get; set; }
        public string Respondant_address { get; set; }
        public string Respondant_mobile { get; set; }
        public string Respondant_pincode { get; set; }


    }
}
