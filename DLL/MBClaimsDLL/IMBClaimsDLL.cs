using KGID_Models.KGID_MB_Claim;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL.MBClaimsDLL
{
    public interface IMBClaimsDLL
    {
        VM_MIOwnDamageClaimDetails GetMIOwnDamageClaimDetailsDLL(long empId, int category);
        VM_ODClaimVerificationDetails GetODClaimApplicationStatusListDLL(long empId, int category);
        List<tbl_district_master> GetDistListDLL();
        List<tbl_taluka_master> GetTalukaListDLL(int DistId);
        List<tbl_od_cost_component_master> GetComponentListDLL();

        long SaveODClaimApplicationDetailsDLL(VM_ODClaimApplicationDetails objCAD);
        VM_ODClaimApplicationDetails GetODClaimApplicationDetailsDLL(long Empid, string PolicyNumber);

        //OD Claim Workflow
        VM_ODClaimVerificationDetails GetEmployeeDetailsForCWVerificationDLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForSuperintendentVerificationDLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForDDVerificationDLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForDVerificationDLL(long EmpID, string Category);

        VM_MIODClaimDeptVerficationDetails GetWorkFlowDetailsDLL(long applicationId, int category);
        string SaveVerifiedDetailsDLL(VM_MIODClaimDeptVerficationDetails objVerifyDetails);

        // Surveyor Workflow
        VM_ODClaimSurveyorVerificationDetails GetEmployeeDetailsForSurveyorVerificationDLL(long EmpID);

        // View Application
        VM_ODClaimApprovedApplicationDetails GetApprovedApplicationListDLL(long EmpID, string Category);
        VM_ODClaimWorkOrderDetails GetODClaimAprvdAppDetailsDLL(long Empid, string PolicyNumber, string Category);
        List<GetVehicleChassisPolicyDetails> GetVehicleAndPolicyDetailsDLL(string txtDetails);
         SelectList GetDistrictListDLL();
        SelectList GetTalukListDLL(int dm_code);
        SelectList GetRemarksDLL();
        long SaveMVCClaimDetailsDLL(GetVehicleChassisPolicyDetails model);
        int SavePathDetailsDLL(string path,long Application_id);

        int PetitionerRespondantDetailsDLL(long Application_id, GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> GetMVCApplicationFormDataDLL();
        List<GetVehicleChassisPolicyDetails> GetMVCGetDetailsOnChassisDLL(string ChassisNo);
        List<GetVehicleChassisPolicyDetails> GetMVCPetitionerDetailsDLL(long Appno);
        List<GetVehicleChassisPolicyDetails> GetMVCRespondantDetailsDLL(long Appno);
        List<GetVehicleChassisPolicyDetails> GetMVCdetailsofCourtDLL(long App_id,int category);
        List<GetVehicleChassisPolicyDetails> GetMVCDocdetailDLL(long App_id);
        List<GetVehicleChassisPolicyDetails> GetOtherDocdetailDLL(long Appno);
        SelectList GetInjuryListDLL();
        SelectList GetstateListDLL();

        int UpdateWork_flow_Details(GetVehicleChassisPolicyDetails model);
		List<GetVehicleChassisPolicyDetails> MvcClaimWorkFlowDetailsDLL(long appid, string chassis);
  long SaveAsDraftMvcDetailsDLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> GetDraftDetailsDLL();
       int stopMVCFlowOnLokadhalatSelectDLL(long Appid);
       int stopLokadhalatFlowOnSelectDLL(long Appid);
       int submitParawiseRemarksDLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> GetMVCDocfileForSignDLL(long docID);
        string MVCSignedDocUploadDLL(long docID, long appId,string DocPath);
        List<GetVehicleChassisPolicyDetails> GetSignedDocDLL(long id);
int UpdateDocumentWork_flow_detailsDLL(GetVehicleChassisPolicyDetails model);
       List<GetVehicleChassisPolicyDetails> GetDocumentDetailsStatusDLL(string GetStatusData, long appId);
       int saveHearingDatesAndCommentsDLL(GetVehicleChassisPolicyDetails model);
        SelectList GetRemarksUpperCourtDLL(); 
        int saveLowerCourtOpinionDetailsDLL(GetVehicleChassisPolicyDetails model);
        int saveClaimApprovalSettleLowerCourtJudgementDLL(GetVehicleChassisPolicyDetails model);
        int SaveDelayNoteToAdvocateHighCourtDLL(GetVehicleChassisPolicyDetails model);
        int saveAmountToDeposittedToHighCourtDLL(GetVehicleChassisPolicyDetails model);
        int UploadofAmountDepositionLetterLCDLL(GetVehicleChassisPolicyDetails model);
        int HighCourtJudgementOpinionDetailsDLL(GetVehicleChassisPolicyDetails model);
        int HighCourtClaimSettlementDetailsDLL(GetVehicleChassisPolicyDetails model);
        int HighCourtJudgementOpinionDetailsKGIDDLL(GetVehicleChassisPolicyDetails model);
        int HighCourtClaimAndSettlementKGIDDLL(GetVehicleChassisPolicyDetails model);
   
   //CHethan
   
     List<GetVehicleChassisPolicyDetails> GetMVCCourtExecutionDLL(long App_id, int category);
   List<GetVehicleChassisPolicyDetails> GetCourtExecutionDocDetailsDLL(long App_id);
  int Update_Court_execution_Work_flow_Details(GetVehicleChassisPolicyDetails model);
  int stopMVCandlokadhalathOnCourtExecutionSelectDLL(long Appid);
        int StopCourtExecutionProcessDLL(long Appid);

        long SaveMVCCourtExecutionDLL(GetVehicleChassisPolicyDetails model);
        int saveCourtDocDLL(string path, long Application_id, string filename);

        List<GetVehicleChassisPolicyDetails> GetWorkFlowCOurtExecutionDLL(long App_id, string chassis);
        int SaveDelayNoteToAdvocateSupremeCourtDLL(GetVehicleChassisPolicyDetails model);
        int SupremeCourtOpinionAndJudegementCopyDLL(GetVehicleChassisPolicyDetails model);
        int SupremeCourtClaimAndSettlementKGIDDLL(GetVehicleChassisPolicyDetails model);
        int SupremeCourtOpinionAndJudegementCopyKGIDDLL(GetVehicleChassisPolicyDetails model);
        int saveSupremeClaimApprovalSettlementDLL(GetVehicleChassisPolicyDetails model);
        int CEUpdateOpinionLawDeptDLL(GetVehicleChassisPolicyDetails model);
        int CEUpdateDocumentWork_flow_detailsDLL(GetVehicleChassisPolicyDetails model);
         int CEClaimsettleLawDeptDLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> CourtExecutionMasterDetailsDLL(long appid);
        List<GetVehicleChassisPolicyDetails> GetCourtExecutiveDocumentDetailsStatusDLL(string FetchDetails, long appid);
        SelectList RemarksJudgementDLL();
        int SendBackMvcToCWDLL(GetVehicleChassisPolicyDetails model);
        List<GetVehicleChassisPolicyDetails> GetSentBackMVCDetailsDLL();
    }
}
