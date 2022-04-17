using KGID_Models.KGID_MB_Claim;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.MBClaimsBLL
{
    public interface IMBClaimsBLL
    {
        List<tbl_district_master> GetDistListBLL();
        List<tbl_taluka_master> GetTalukaListBLL(int DistId);
        List<tbl_od_cost_component_master> GetComponentListBLL();

        
        GetVehicleChassisPolicyDetails GetVehicleAndPolicyDetailsBLL(string textDetails);
        SelectList GetDistrictListBLL();

        SelectList GetTalukListBLL(int dm_code);
        SelectList GetRemarksBLL();

        long SaveMVCClaimDetailsBLL(GetVehicleChassisPolicyDetails model);
        int SavePathDetailsBLL(string path, long Application_id);

        List<GetVehicleChassisPolicyDetails> GetMVCApplicationFormDataBLL();
        GetVehicleChassisPolicyDetails GetMVCGetDetailsOnChassisBLL(string ChassisNo);
        List<GetVehicleChassisPolicyDetails> PetitionerDetailsListBLL(long Appno);
        List<GetVehicleChassisPolicyDetails> GetMVCRespondantDetailsBLL(long Appno);
        List<GetVehicleChassisPolicyDetails> GetMVCdetailsofCourtBLL(long App_id,int Category);
        List<GetVehicleChassisPolicyDetails> GetMVCDocdetailBLL(long App_id);
        List<GetVehicleChassisPolicyDetails> GetOtherDocdetailBLL(long App_id);
        SelectList GetInjuryListBLL();
        SelectList GetstateListBLL();
		int UpdateWork_flow_DetailsBLL(GetVehicleChassisPolicyDetails model);
       List<GetVehicleChassisPolicyDetails> MvcClaimWorkFlowDetailsBLL(long appid, string chassis);
long SaveAsDraftMvcDetailsBLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> GetDraftDetailsBLL();
        int stopMVCFlowOnsLokadhalatSelectBLL(long AppId);
        int stopLokadhalatFlowOnSelectBLL(long AppId);
        int submitParawiseRemarksBLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> GetMVCDocfileForSignBLL(long docID);
        string MVCSignedDocUploadBLL(long docID, long appId,string DocPath);
        List<GetVehicleChassisPolicyDetails> GetSignedDocBLL(long id);
 int UpdateDocumentWork_flow_detailsBLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> GetDocumentDetailsStatusBLL(string fecthDetails, long appId);
        int saveHearingDatesAndCommentsBLL(GetVehicleChassisPolicyDetails model);
        SelectList GetRemarksUpperCourtBLL(); 

       int saveLowerCourtOpinionDetailsBLL(GetVehicleChassisPolicyDetails model);
       int saveClaimApprovalSettleLowerCourtJudgementBLL(GetVehicleChassisPolicyDetails model);
       int SaveDelayNoteToAdvocateHighCourtBLL(GetVehicleChassisPolicyDetails model);
       int saveAmountToDeposittedToHighCourtBLL(GetVehicleChassisPolicyDetails model);
       int UploadofAmountDepositionLetterLCBLL(GetVehicleChassisPolicyDetails model);
        int HighCourtJudgementOpinionDetailsBLL(GetVehicleChassisPolicyDetails model);
        int HighCourtClaimSettlementDetailsBLL(GetVehicleChassisPolicyDetails model);

        int HighCourtJudgementOpinionDetailsKGIDBLL(GetVehicleChassisPolicyDetails model);
        int HighCourtClaimSettlementDetailsKGIDBLL(GetVehicleChassisPolicyDetails model);

        //chethan
        List<GetVehicleChassisPolicyDetails> GetMVCCourtExecutionBLL(long App_id, int Category);
        List<GetVehicleChassisPolicyDetails> GetCourtExecutionDocDetailsBLL(long App_id);

        //long SaveMVCLokadalathDetailsBLL(GetVehicleChassisPolicyDetails model);
        int stopMVCandlokadhalathOnCourtExecutionSelectBLL(long AppId);
        int Update_Court_execution_Work_flow_DetailsBLL(GetVehicleChassisPolicyDetails model);
        int StopCourtExecutionProcessBLL(long AppId);
        long SaveMVCCourtExecutionBLL(GetVehicleChassisPolicyDetails model);
        int saveCourtDocBLL(string path, long Application_id, string filename);
        List<GetVehicleChassisPolicyDetails> GetWorkFlowCOurtExecutionBLL(long App_id, string chassis);

        int SaveDelayNoteToAdvocateSupremeCourtBLL(GetVehicleChassisPolicyDetails model);
        int SupremeCourtOpinionAndJudegementCopyBLL(GetVehicleChassisPolicyDetails model);
        int SupremeCourtClaimSettlementDetailsBLL(GetVehicleChassisPolicyDetails model);
        int SupremeCourtOpinionAndJudegementCopyKGIDBLL(GetVehicleChassisPolicyDetails model);
        int saveSupremeClaimApprovalSettlementBLL(GetVehicleChassisPolicyDetails model);
        int CEUpdateDocumentWork_flow_detailsBLL(GetVehicleChassisPolicyDetails model);
        int CEUpdateOpinionLawDeptBLL(GetVehicleChassisPolicyDetails model);
        int CEClaimsettleLawDeptBLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> CourtExecutionMasterDetailsBLL(long appid);
        List<GetVehicleChassisPolicyDetails> GetCourtExecutiveDocumentDetailsStatusBLL(string GetStatusData, long appId);
         SelectList RemarksJudgementBLL(int category);
        int SendBackMvcToCWBLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> GetSentBackMVCDetailsBLL();
        List<GetVehicleChassisPolicyDetails> GetLokadhalathdetailsofCourtBLL(long App_id, int category);
        long SaveMVCLokadalathDetailsBLL(GetVehicleChassisPolicyDetails model);
        int saveLokDocBLL(string path, long Application_id, string filename);
        List<GetVehicleChassisPolicyDetails> GetWorkFlowLokBLL(long App_id, string chassis);
        List<GetVehicleChassisPolicyDetails> GetLokDocDetailsBLL(long Appno);
        List<GetVehicleChassisPolicyDetails> GetLokadalathDetailsBLL(long Appno);
        int Update_Lokadalath_Work_flow_DetailsBLL(GetVehicleChassisPolicyDetails model);
        int saveJudgementCopyDetailsBLL(GetVehicleChassisPolicyDetails model);
        int saveLokadhalatDocBLL(long appid, string chassis);
        int  UpdateLokadhalatDocumentWork_flow_detailsBLL(GetVehicleChassisPolicyDetails model);
        int LokClaimsettleLawDeptBLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> GetLokadhalatMasterDetailsBLL(long appid);
        List<GetVehicleChassisPolicyDetails> GetLokadhalatDocumentDetailsStatusBLL(string FetchDetails, long appid);
        List<SelectListItem> GetRemarksLokadhalatCourtBLL();
        SelectList RemarksObjectionStatementBLL(int cat);
       SelectList RemarksPaymentStatementBLL(int category);
       int  saveVehicleNumberBLL(string vehicle_registration_no, string chassisNo);
        SelectList RemarksRatificationBLL(int category);
        SelectList RemarksDelayNoteBLL(int category);
    }
}
