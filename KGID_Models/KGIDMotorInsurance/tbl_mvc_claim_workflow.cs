using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
   
    public class tbl_mvc_claim_workflow
    {
        [Key]
        public long mvc_claim_id { get; set; }
       public long mvc_claim_app_id { get; set; }
        public string micw_vehicle_number { get; set; }
        public string micw_policy_number { get; set; }
        public long micw_remarks { get; set; }
        public string micw_comments { get; set; }
        public long micw_verified_by { get; set; }
        public bool micw_checklist_status { get; set; }
        public int micw_application_status { get; set; }
        public bool micw_surveyor_registered { get; set; }
        public decimal micw_approved_damage_cost { get; set; }
        public bool micw_active_status { get; set; }
        public DateTime micw_creation_datetime { get; set; }
        public DateTime micw_updation_datetime { get; set; }
        public long micw_created_by { get; set; }
        public long micw_updated_by { get; set; }
        public long micw_assigned_to { get; set; }
        public int? mvc_parawiseRemarkLawyer { get; set; }
        public int mvc_main_flow { get; set; }
        public bool? mvc_parawiseRemarkLawyerStatus { get; set; }
        public int? mvc_objecttionStatement { get; set; }
        public bool? mvc_objecttionStatementStatus { get; set; } 
        public int? mvc_ratificationLawDept { get; set; }
        public bool? mvc_ratificationLawDeptStatus { get; set; }

        public int? mvc_lower_Court_judgementCopy { get; set; }
        public bool? mvc_lower_Court_judgementCopyStatus { get; set; }
        public int? mvc_OpinionFromLawDepartment { get; set; }
        public bool? mvc_OpinionFromLawDepartmentStatus { get; set; }
        public int? mvc_ClaimApprovalSettleLowerCourt { get; set; }
        public bool? mvc_ClaimApprovalSettleLowerCourtStatus { get; set; }
        public int? mvc_DraftForDelayNoteHighCourt { get; set; }
        public bool? mvc_DraftForDelayNoteHighCourtStatus { get; set; }
        public int? mvc_amntDepositToHighCourt { get; set; }
        public bool? mvc_amntDepositToHighCourtStatus { get; set; }
        public int? mvc_amntDepositToLowCourt { get; set; }
        public bool? mvc_amntDepositToLowCourtStatus { get; set; }
        public int? mvc_HighCourtJudgementOpinion { get; set; }
        public bool? mvc_HighCourtJudgementOpinionStatus { get; set; } 
        public int? mvc_claimsettleHighCourtJudgement { get; set; }
        public bool? mvc_claimsettleHighCourtJudgementStatus { get; set; }
        public int? mvc_HighCourtNoticePetition { get; set; }
        public bool? mvc_HighCourtNoticePetitionStatus { get; set; } 
        public int? mvc_HighCourtCoveringLetter { get; set; }
        public bool? mvc_HighCourtCoveringLetterStatus { get; set; }
        public int? mvc_opinionStatusHighCourtKGID { get; set; }
        public bool? mvc_opinionStatusHighCourtKGIDStatus { get; set; }
        public int? mvc_ClaimSettleHighCourt { get; set; }
        public bool? mvc_ClaimSettleLowerHighStatus { get; set; }
         public int? mvc_inputDelaysupremeCourtDraft { get; set; }
        public bool? mvc_inputDelaysupremeCourtDraftStatus { get; set; } 
        public int? mvc_amntDepositToSupremeC { get; set; }
        public bool? mvc_amntDepositToSupremeCStatus { get; set; }
         public int? mvc_amntDepositSupremeCToLC { get; set; }
        public bool? mvc_amntDepositSupremeCToLCStatus { get; set; }
        public int? mvc_supremeOpinionJudgement { get; set; }
        public bool? mvc_supremeOpinionJudgementStatus { get; set; }
        public int? mvc_supremeClaimSettle { get; set; }
        public bool? mvc_supremeClaimSettleStatus { get; set; }
        public int? mvc_SupremeCourtNoticePetition { get; set; }
        public bool? mvc_SupremeCourtNoticePetitionStatus { get; set; } 
        public int? mvc_SupremeCourtCoveringLetter { get; set; }
        public bool? mvc_SupremeCourtCoveringLetterStatus { get; set; }
         public int? mvc_SupremeOpinionclaimJudgmnt { get; set; }
        public bool? mvc_SupremeOpinionclaimJudgmntstatus { get; set; }
        public int? mvc_claimSettlesupremeCourtKGID { get; set; }
        public bool? mvc_claimSettleSupremeCourtKGIDStatus { get; set; }


    }
}
