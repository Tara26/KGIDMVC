using DLL.MBClaimsDLL;
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
    public class MBClaimsBLL : IMBClaimsBLL
    {
        private readonly IMBClaimsDLL _IMBClaimsDLL;

        public MBClaimsBLL()
        {
            this._IMBClaimsDLL = new MBClaimsDLL();
            //claims = new MBClaimsDLL();
        }

        //Dist List
        public List<tbl_district_master> GetDistListBLL()
        {
            return _IMBClaimsDLL.GetDistListDLL();
        }
        //Taluka List
        public List<tbl_taluka_master> GetTalukaListBLL(int DistId)
        {
            return _IMBClaimsDLL.GetTalukaListDLL(DistId);
        }
        //Component List
        public List<tbl_od_cost_component_master> GetComponentListBLL()
        {
            return _IMBClaimsDLL.GetComponentListDLL();
        }
        
        public GetVehicleChassisPolicyDetails GetVehicleAndPolicyDetailsBLL(string textDetails)
        {
            return _IMBClaimsDLL.GetVehicleAndPolicyDetailsDLL(textDetails);

        }
        public SelectList GetDistrictListBLL()
        {
            return _IMBClaimsDLL.GetDistrictListDLL();
        }
        public SelectList GetTalukListBLL(int dm_code)
        {
            return _IMBClaimsDLL.GetTalukListDLL(dm_code);
        }
        
            public SelectList GetRemarksBLL()
        {
            return _IMBClaimsDLL.GetRemarksDLL();
        }
        public long SaveMVCClaimDetailsBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.SaveMVCClaimDetailsDLL(model);
        }
        public int SavePathDetailsBLL(string path, long App_id) {

            return _IMBClaimsDLL.SavePathDetailsDLL(path,App_id);
        }
        public  List<GetVehicleChassisPolicyDetails> GetMVCApplicationFormDataBLL() {

            return _IMBClaimsDLL.GetMVCApplicationFormDataDLL();
        } 
        public  GetVehicleChassisPolicyDetails GetMVCGetDetailsOnChassisBLL(string ChassisNo) {

            return _IMBClaimsDLL.GetMVCGetDetailsOnChassisDLL(ChassisNo);
        }

        public List<GetVehicleChassisPolicyDetails> MvcClaimWorkFlowDetailsBLL(long appid, string chassis)
        {

            return _IMBClaimsDLL.MvcClaimWorkFlowDetailsDLL(appid,chassis);
        }
        public List<GetVehicleChassisPolicyDetails> PetitionerDetailsListBLL(long Appno)
        {

            return _IMBClaimsDLL.GetMVCPetitionerDetailsDLL(Appno);
        } 
        public  List<GetVehicleChassisPolicyDetails> GetMVCRespondantDetailsBLL(long Appno)
        {

            return _IMBClaimsDLL.GetMVCRespondantDetailsDLL(Appno);
        }public  List<GetVehicleChassisPolicyDetails> GetMVCdetailsofCourtBLL(long App_id,int category)
        {

            return _IMBClaimsDLL.GetMVCdetailsofCourtDLL(App_id,category);
        }
        public  List<GetVehicleChassisPolicyDetails> GetMVCDocdetailBLL(long App_id)
        {

            return _IMBClaimsDLL.GetMVCDocdetailDLL(App_id);
        }

        public List<GetVehicleChassisPolicyDetails> GetMVCDocfileForSignBLL(long docID)
        {

            return _IMBClaimsDLL.GetMVCDocfileForSignDLL(docID);
        }
        public  List<GetVehicleChassisPolicyDetails> GetOtherDocdetailBLL(long App_id)
        {

            return _IMBClaimsDLL.GetOtherDocdetailDLL(App_id);
        }
        public SelectList GetInjuryListBLL()
        {
            return _IMBClaimsDLL.GetInjuryListDLL();
        } public SelectList GetstateListBLL()
        {
            return _IMBClaimsDLL.GetstateListDLL();
        }
        public int UpdateWork_flow_DetailsBLL(GetVehicleChassisPolicyDetails model) {

            return _IMBClaimsDLL.UpdateWork_flow_Details(model);
        }
        public long SaveAsDraftMvcDetailsBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.SaveAsDraftMvcDetailsDLL(model);
        } public List<GetVehicleChassisPolicyDetails> GetDraftDetailsBLL()
        {
            return _IMBClaimsDLL.GetDraftDetailsDLL();
        } public int stopMVCFlowOnsLokadhalatSelectBLL(long Appid)
        {
            return _IMBClaimsDLL.stopMVCFlowOnLokadhalatSelectDLL(Appid);
        }public int stopLokadhalatFlowOnSelectBLL(long Appid)
        {
            return _IMBClaimsDLL.stopLokadhalatFlowOnSelectDLL(Appid);
        }public int submitParawiseRemarksBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.submitParawiseRemarksDLL(model);
        }

        public string MVCSignedDocUploadBLL(long docID, long appId, string DocPath)
        {
            var result = _IMBClaimsDLL.MVCSignedDocUploadDLL(docID, appId,DocPath);
            return result;
        }

        public List<GetVehicleChassisPolicyDetails> GetSignedDocBLL(long id)
        {
            return _IMBClaimsDLL.GetSignedDocDLL(id);
        }
		public int UpdateDocumentWork_flow_detailsBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.UpdateDocumentWork_flow_detailsDLL(model);
        }public List<GetVehicleChassisPolicyDetails> GetDocumentDetailsStatusBLL(string GetStatusData, long appId)
        {
            return _IMBClaimsDLL.GetDocumentDetailsStatusDLL( GetStatusData, appId);
        }
        public int saveHearingDatesAndCommentsBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.saveHearingDatesAndCommentsDLL(model);
        }
        public SelectList GetRemarksUpperCourtBLL() {
            return _IMBClaimsDLL.GetRemarksUpperCourtDLL();
        }
        public int saveLowerCourtOpinionDetailsBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.saveLowerCourtOpinionDetailsDLL(model);
        } public int saveClaimApprovalSettleLowerCourtJudgementBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;
            
            result = _IMBClaimsDLL.saveClaimApprovalSettleLowerCourtJudgementDLL(model);
            if (result==1)
            {
                result = _IMBClaimsDLL.UpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;

        } public int SaveDelayNoteToAdvocateHighCourtBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;
            
            result = _IMBClaimsDLL.SaveDelayNoteToAdvocateHighCourtDLL(model);
            if (result==1)
            {
                result = _IMBClaimsDLL.UpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;

        } public int saveAmountToDeposittedToHighCourtBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;
            
            result = _IMBClaimsDLL.saveAmountToDeposittedToHighCourtDLL(model);
            if (result==1)
            {
                result = _IMBClaimsDLL.UpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;

        }
        public int UploadofAmountDepositionLetterLCBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;
            
            result = _IMBClaimsDLL.UploadofAmountDepositionLetterLCDLL(model);
            if (result==1)
            {
                result = _IMBClaimsDLL.UpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;

        }
        public int HighCourtJudgementOpinionDetailsBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.HighCourtJudgementOpinionDetailsDLL(model);
        }
        public int HighCourtClaimSettlementDetailsBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;
            result = _IMBClaimsDLL.HighCourtClaimSettlementDetailsDLL(model);
            if (result == 1)
            {
                result = _IMBClaimsDLL.UpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;
        }

        public int HighCourtJudgementOpinionDetailsKGIDBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.HighCourtJudgementOpinionDetailsKGIDDLL(model);
        }
        public int HighCourtClaimSettlementDetailsKGIDBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;
            result = _IMBClaimsDLL.HighCourtClaimAndSettlementKGIDDLL(model);
            if (result == 1)
            {
                result = _IMBClaimsDLL.UpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;
            
        }
		
		//chethan
		
		public  List<GetVehicleChassisPolicyDetails> GetMVCCourtExecutionBLL(long App_id,int category)
        {

            return _IMBClaimsDLL.GetMVCCourtExecutionDLL(App_id,category);
        }
 public int Update_Court_execution_Work_flow_DetailsBLL(GetVehicleChassisPolicyDetails model) {

            return _IMBClaimsDLL.Update_Court_execution_Work_flow_Details(model);
        }
//public long SaveMVCLokadalathDetailsBLL(GetVehicleChassisPolicyDetails model)
//        {
//            return _IMBClaimsDLL.SaveMVCLokadalathDetailsDLL(model);
//        }

        public int stopMVCandlokadhalathOnCourtExecutionSelectBLL(long Appid)
        {
            return _IMBClaimsDLL.stopMVCandlokadhalathOnCourtExecutionSelectDLL(Appid);
        }

        public int StopCourtExecutionProcessBLL(long Appid)
        {
            return _IMBClaimsDLL.StopCourtExecutionProcessDLL(Appid);
        }

        public long SaveMVCCourtExecutionBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.SaveMVCCourtExecutionDLL(model);
        }

        public int saveCourtDocBLL(string path, long Application_id, string filename)
        {

            return _IMBClaimsDLL.saveCourtDocDLL(path, Application_id, filename);
        }

        public List<GetVehicleChassisPolicyDetails> GetWorkFlowCOurtExecutionBLL(long App_id,string chassis)
        {

            return _IMBClaimsDLL.GetWorkFlowCOurtExecutionDLL(App_id, chassis);
        }

        public List<GetVehicleChassisPolicyDetails> GetCourtExecutionDocDetailsBLL(long App_id)
        {

            return _IMBClaimsDLL.GetCourtExecutionDocDetailsDLL(App_id);
        }
        public int SaveDelayNoteToAdvocateSupremeCourtBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;

            result = _IMBClaimsDLL.SaveDelayNoteToAdvocateSupremeCourtDLL(model);
            if (result == 1)
            {
                result = _IMBClaimsDLL.UpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;

        }  
        public int SupremeCourtOpinionAndJudegementCopyBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;

            result = _IMBClaimsDLL.SupremeCourtOpinionAndJudegementCopyDLL(model);
           
            return result;

        }public int SupremeCourtClaimSettlementDetailsBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;

            
            result = _IMBClaimsDLL.SupremeCourtClaimAndSettlementKGIDDLL(model);
            if (result == 1)
            {
                result = _IMBClaimsDLL.UpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;

        }
        public int SupremeCourtOpinionAndJudegementCopyKGIDBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;

            result = _IMBClaimsDLL.SupremeCourtOpinionAndJudegementCopyKGIDDLL(model);

            return result;

        }
        public int saveSupremeClaimApprovalSettlementBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;


            result = _IMBClaimsDLL.saveSupremeClaimApprovalSettlementDLL(model);
            if (result == 1)
            {
                result = _IMBClaimsDLL.UpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;

        }
        
        public int CEUpdateOpinionLawDeptBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;


            result = _IMBClaimsDLL.CEUpdateOpinionLawDeptDLL(model);
            if (result == 1)
            {
                result = _IMBClaimsDLL.CEUpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;

        } public int CEClaimsettleLawDeptBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;


            result = _IMBClaimsDLL.CEClaimsettleLawDeptDLL(model);
            if (result == 1)
            {
                result = _IMBClaimsDLL.CEUpdateDocumentWork_flow_detailsDLL(model);
            }
            return result;

        }
        public int CEUpdateDocumentWork_flow_detailsBLL(GetVehicleChassisPolicyDetails model)
        {
            var result = _IMBClaimsDLL.CEUpdateDocumentWork_flow_detailsDLL(model);

            return result;
        }  public List<GetVehicleChassisPolicyDetails> CourtExecutionMasterDetailsBLL(long appid)
        {
           

            return _IMBClaimsDLL.CourtExecutionMasterDetailsDLL(appid);
        }
        public List<GetVehicleChassisPolicyDetails> GetCourtExecutiveDocumentDetailsStatusBLL(string GetStatusData, long appId)
        {
            return _IMBClaimsDLL.GetCourtExecutiveDocumentDetailsStatusDLL(GetStatusData, appId);
        }
        public SelectList RemarksJudgementBLL(int Category)
        {
            return _IMBClaimsDLL.RemarksJudgementDLL(Category);
        }
        public int SendBackMvcToCWBLL(GetVehicleChassisPolicyDetails model)
        {

            return _IMBClaimsDLL.SendBackMvcToCWDLL(model);
        }
        public List<GetVehicleChassisPolicyDetails> GetSentBackMVCDetailsBLL( )
        {

            return _IMBClaimsDLL.GetSentBackMVCDetailsDLL();
        }
        public List<GetVehicleChassisPolicyDetails> GetLokadhalathdetailsofCourtBLL(long App_id, int category)
        {
            return _IMBClaimsDLL.GetLokadhalathdetailsofCourtDLL(App_id, category);

        }
        public long SaveMVCLokadalathDetailsBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.SaveMVCLokadalathDetailsDLL(model);
        }

        public int saveLokDocBLL(string path, long Application_id, string filename)
        {

            return _IMBClaimsDLL.saveLokDocDLL(path, Application_id, filename);
        }

        public List<GetVehicleChassisPolicyDetails> GetWorkFlowLokBLL(long App_id, string chassis)
        {

            return _IMBClaimsDLL.GetWorkFlowLokDLL(App_id, chassis);
        }

        public List<GetVehicleChassisPolicyDetails> GetLokDocDetailsBLL(long Appno)
        {
            return _IMBClaimsDLL.GetLokDocDetailsDLL(Appno);

        }
        public List<GetVehicleChassisPolicyDetails> GetLokadalathDetailsBLL(long Appno)
        {
            return _IMBClaimsDLL.GetLokadalathDetailsDLL(Appno);

        }
        public int Update_Lokadalath_Work_flow_DetailsBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.Update_Lokadalath_Work_flow_Details(model);
        }public int saveJudgementCopyDetailsBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.saveJudgementCopyDetailsDLL(model);
        }
        public int saveLokadhalatDocBLL(long appid, string chassis)
        {

            return _IMBClaimsDLL.saveLokadhalatDocDLL(chassis,appid);
        }
        public int UpdateLokadhalatDocumentWork_flow_detailsBLL(GetVehicleChassisPolicyDetails model)
        {
            return _IMBClaimsDLL.UpdateLokadhalatDocumentWork_flow_detailsDLL(model);
        }
        public List<GetVehicleChassisPolicyDetails> GetLokadhalatMasterDetailsBLL(long App_id)
        {
            return _IMBClaimsDLL.GetLokadhalatMasterDetailsDLL(App_id);

        } public List<GetVehicleChassisPolicyDetails> GetLokadhalatDocumentDetailsStatusBLL(string FetchDetails, long appid)
        {
            return _IMBClaimsDLL.GetLokadhalatDocumentDetailsStatusDLL(FetchDetails, appid);

        }
        public int LokClaimsettleLawDeptBLL(GetVehicleChassisPolicyDetails model)
        {
            int result = 0;


            result = _IMBClaimsDLL.LokClaimsettleLawDeptDLL(model);
            if (result == 1)
            {
                result = _IMBClaimsDLL.UpdateLokadhalatDocumentWork_flow_detailsDLL(model);
            }
            return result;

        }
        public List<SelectListItem> GetRemarksLokadhalatCourtBLL()
        {
            return _IMBClaimsDLL.GetRemarksLokadhalatCourtDLL();
        }
        public SelectList RemarksObjectionStatementBLL(int category)
        {
            return _IMBClaimsDLL.RemarksObjectionStatementDLL(category);
        }  public SelectList RemarksPaymentStatementBLL(int category)
        {
            return _IMBClaimsDLL.RemarksPaymentStatementDLL(category);
        } public int saveVehicleNumberBLL(string vehicle_registration_no, string chassisNo)
    
        {
            return _IMBClaimsDLL.saveVehicleNumberDLL(vehicle_registration_no,chassisNo);
        }
        public SelectList RemarksRatificationBLL(int category)
        {
            return _IMBClaimsDLL.RemarksRatificationDLL(category);
        }
        public  SelectList RemarksDelayNoteBLL(int category)
        {
            return _IMBClaimsDLL.RemarksDelayNoteDLL(category);
        }
    }
}
