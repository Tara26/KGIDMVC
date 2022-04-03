using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_application_details
    {
        [Key]
        public int mvc_claim_id { get; set; }
        public long mvc_claim_app_id { get; set; }
        public string chassis_no { get; set; }
        public string policy_no  { get; set; }
        public string mvc_no { get; set; }
        public DateTime date_of_petition { get; set; }
        public string name_of_court { get; set; }
        public int court_district { get; set; }

        public int court_taluk { get; set; }
        public string name_of_petitioner { get; set; }
        public long petitioner_pincode   { get; set; }
        public long petitioner_mobile_no { get; set; }
        public string respondant_name { get; set; }
        public string respondant_designation  { get; set; }
        public string respondant_department  { get; set; }
        public string respondant_agencyName    { get; set; }
        public string respondant_address   { get; set; }
        public long respondant_mobile_no   { get; set; }
        public long respondant_pincode   { get; set; }
        public int accident_district  { get; set; }
        public int accident_taluk { get; set; }
        public string accident_hobli    { get; set; }
        public string accident_grampanchayat { get; set; }
        public string accident_village  { get; set; }
        public string accident_details   { get; set; }
        public Decimal claim_amount { get; set; }
        public string rc_details  { get; set; }
        public string dl_details  { get; set; }
        public string fir_details  { get; set; }
        public string panchanama_details  { get; set; }
        public int remarks  { get; set; }
        public bool status_id { get; set; }
        public bool verified_document { get; set; }
        public long created_by  { get; set; }
        public long updated_by { get; set; }
        public DateTime mvc_claim_creation_datetime  { get; set; }
        public DateTime mvc_claim_updation_datetime  { get; set; }
        public string acdnt_name_of_injured_person { get; set; }
        public string acdnt_person_father_name { get; set; }
        public string acdnt_person_spouse_name { get; set; }
        public string acdnt_person_full_address { get; set; }
        public int acdnt_person_age { get; set; }
        public string acdnt_person_occupation { get; set; }
        public string acdnt_emp_nameadress_deceased { get; set; }
        public decimal acdnt_person_monthly_income { get; set; }
        public decimal acdnt_comp_claimed_tax { get; set; }
        public string acdnt_place_accident { get; set; }
        public DateTime? acdnt_date_time_of_accident { get; set; }
        public string acdnt_police_station_details { get; set; }
        public string acdnt_compens_claimed_travelling { get; set; }
        public int acdnt_type_of_injury { get; set; }
        public string acdnt_nature_of_injury { get; set; }
        public string acdnt_medical_officer_detail { get; set; }
        public string acdnt_period_treatment_expend { get; set; }
        public string acdnt_name_of_injury_caused { get; set; }
        public string acdnt_applicant_details { get; set; }
        public string acdnt_relation_details { get; set; }
        public string acdnt_title_to_property { get; set; }

        public string acdt_any_other_info { get; set; }
        public int state_id { get; set; }
        public int app_saved_status { get; set; }

        public string other_state_court_dist { get; set; }
        public string other_state_court_taluk { get; set; }
        public string court_parawise_remarks { get; set; }
        public DateTime? Ratification_hearingDate { get; set; }
        public DateTime? Ratification_hearingNextHearingDate { get; set; }
        public string Ratification_hearingComments { get; set; }
        
        public int? opinionStatusLowerCourt { get; set; }
        
        public DateTime? lowerCourtJudgementDate { get; set; }
        
        public decimal? awardedAmount_lowerCourt { get; set; }
        public decimal? mvc_claimSettleAwardedAmount { get; set; }
        public DateTime? mvc_claimSettlePetitionDate { get; set; }
        public decimal? mvc_claimSettleAwardedInterest { get; set; }
        public decimal? mvc_claimSettlecourtCost { get; set; }
        public decimal? mvc_claimSettleAwardedTotalAmount { get; set; }
        public string mvc_InputForDelayNoteHighCourt { get; set; }
        public decimal?  mvc_amntDepoHighCourt { get; set; }
        public bool? mvc_amntDepoHighCourtStatus { get; set; }
        public decimal? mvc_amntDepoLowCourt { get; set; }
        public bool? mvc_amntDepoLowCourtStatus { get; set; }
        public int? opinionStatusHighCourt { get; set; }

        public DateTime? HighCourtJudgementDate { get; set; }

        public decimal? awardedAmount_highCourt { get; set; }
        public decimal? HighCourt_Statuatory_Amount { get; set; }
        public DateTime? HighCourt_statAmnt_RemittedDate { get; set; }
        public decimal? HighCourtDeposit_Amount { get; set; }
        public DateTime? HighCourt_depositAmnt_RemittedDate { get; set; }
        public decimal? HighCourtClaimAwarded_Amount { get; set; }
        public decimal? HighCourtClaimAwarded_Interest { get; set; }
        public decimal? HighCourtClaimSettle_cost { get; set; }
        public decimal? HighCourtClaimSettle_TotalAmnt { get; set; }
        public int? opinionStatusHighCourtKGID { get; set; }
        public DateTime? HighCourtJudgementDateKGID { get; set; }
        public decimal? awardedAmount_highCourtKGID { get; set; }
        public decimal? awardedAmount_highCourtClaimSttleKGID { get; set; }
        public decimal? awardedInterest_highCourtClaimSttleKGID { get; set; }
        public decimal? courtCost_highCourtClaimSttleKGID { get; set; }
        public decimal? totalAmnt_highCourtClaimSttleKGID { get; set; }
        public string inputDelaysupremeCourt { get; set; }
        public int? OpinionSupremeCourt { get; set; }
        public DateTime? Supreme_judgement_Date { get; set; }
        public decimal? SupremeopinionAwardAmnt { get; set; }
        public decimal? Supreme_Statuatory_Amount { get; set; }
        public DateTime? Statuatory_Amount_Remitted { get; set; }
        public decimal? Supreme_DepositAmount { get; set; }
        public DateTime? Supreme_Deposit_Amount_Remitted { get; set; }
        public decimal? Supreme_Awarded_Amount { get; set; }
        public decimal? Supreme_Awarded_Interest { get; set; }
        public decimal? Supreme_Court_Cost { get; set; }
        public decimal? Supreme_Total_Amount { get; set; }
        public bool? statutorypaidStatus { get; set; }
        public DateTime? mvc_JudgementSupreme_date2 { get; set; }
        public int? mvc_opinionSupremeStatusID2 { get; set; }
        public decimal? mvc_awardedSupreme_amount2 { get; set; } 
        public decimal? awardedAmount_supremeCourtKGID { get; set; }
        public decimal? awardedInterest_supremeCourtKGID { get; set; } 
        public decimal? courtcost_supremeCourtKGID { get; set; }
        public decimal? TotalAmount_supremeCourtKGID { get; set; }



    }
}





















