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
        VM_MIOwnDamageClaimDetails GetMIOwnDamageClaimDetailsBLL(long empId, int category);
        VM_ODClaimVerificationDetails GetODClaimApplicationStatusListBLL(long empId, int category);
        List<tbl_district_master> GetDistListBLL();
        List<tbl_taluka_master> GetTalukaListBLL(int DistId);
        List<tbl_od_cost_component_master> GetComponentListBLL();

        long SaveODClaimApplicationDetailsBLL(VM_ODClaimApplicationDetails objCAD);
        VM_ODClaimApplicationDetails GetODClaimApplicationDetailsBLL(long Empid, string PolicyNumber);

        #region OD Claim Workflow
        //OD Claim Workflow
        VM_ODClaimVerificationDetails GetEmployeeDetailsForCWVerificationBLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForSuperintendentVerificationBLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForDDVerificationBLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForDVerificationBLL(long EmpID, string Category);

        VM_MIODClaimDeptVerficationDetails GetWorkFlowDetailsBLL(long applicationId, int category);
        string SaveVerifiedDetailsBLL(VM_MIODClaimDeptVerficationDetails objVerifyDetails);
        #endregion

        // Surveyor Workflow
        VM_ODClaimSurveyorVerificationDetails GetEmployeeDetailsForSurveyorVerificationBLL(long EmpID);
        // View Application
        VM_ODClaimApprovedApplicationDetails GetApprovedApplicationListBLL(long EmpID, string Category);
        VM_ODClaimWorkOrderDetails GetODClaimAprvdAppDetailsBLL(long Empid, string PolicyNumber, string Category);
        List<GetVehicleChassisPolicyDetails> GetVehicleAndPolicyDetailsBLL(string textDetails);
        SelectList GetDistrictListBLL();

        SelectList GetTalukListBLL(int dm_code);
        SelectList GetRemarksBLL();

        long SaveMVCClaimDetailsBLL(GetVehicleChassisPolicyDetails model);
        int SavePathDetailsBLL(string path, long Application_id);

        List<GetVehicleChassisPolicyDetails> GetMVCApplicationFormDataBLL();
        List<GetVehicleChassisPolicyDetails> GetMVCGetDetailsOnChassisBLL(string ChassisNo);
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
         SelectList RemarksJudgementBLL();
        int SendBackMvcToCWBLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> GetSentBackMVCDetailsBLL();

    }
}
