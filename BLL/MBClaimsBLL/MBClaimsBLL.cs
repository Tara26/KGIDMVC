using DLL.MBClaimsDLL;
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
    public class MBClaimsBLL : IMBClaimsBLL
    {
        private readonly IMBClaimsDLL _IMBClaimsDLL;

        public MBClaimsBLL()
        {
            this._IMBClaimsDLL = new MBClaimsDLL();
            //claims = new MBClaimsDLL();
        }
        public VM_MIOwnDamageClaimDetails GetMIOwnDamageClaimDetailsBLL(long empId, int category)
        {
            return _IMBClaimsDLL.GetMIOwnDamageClaimDetailsDLL(empId, category);
        }
        public VM_ODClaimVerificationDetails GetODClaimApplicationStatusListBLL(long empId, int category)
        {
            return _IMBClaimsDLL.GetODClaimApplicationStatusListDLL(empId, category);
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
        //Save OD Claim Application Details
        public long SaveODClaimApplicationDetailsBLL(VM_ODClaimApplicationDetails objCAD)
        {
            return _IMBClaimsDLL.SaveODClaimApplicationDetailsDLL(objCAD);
        }
        //Get OD Claim Application Details
        public VM_ODClaimApplicationDetails GetODClaimApplicationDetailsBLL(long Empid, string PolicyNumber)
        {
            return _IMBClaimsDLL.GetODClaimApplicationDetailsDLL(Empid, PolicyNumber);
        }


        #region OD Claim Workfow
        // OD Claim Workfow
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForCWVerificationBLL(long EmpId, string Category)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForCWVerificationDLL(EmpId, Category);
        }
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForSuperintendentVerificationBLL(long EmpId, string Category)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForSuperintendentVerificationDLL(EmpId, Category);
        }
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForDDVerificationBLL(long EmpId, string Category)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForDDVerificationDLL(EmpId, Category);
        }
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForDVerificationBLL(long EmpId, string Category)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForDVerificationDLL(EmpId, Category);
        }

        public VM_MIODClaimDeptVerficationDetails GetWorkFlowDetailsBLL(long applicationId, int category)
        {
            return _IMBClaimsDLL.GetWorkFlowDetailsDLL(applicationId, category);
        }
        public string SaveVerifiedDetailsBLL(VM_MIODClaimDeptVerficationDetails objVerifyDetails)
        {
            return _IMBClaimsDLL.SaveVerifiedDetailsDLL(objVerifyDetails);
        }
        #endregion

        // Surveyor Workflow
        public VM_ODClaimSurveyorVerificationDetails GetEmployeeDetailsForSurveyorVerificationBLL(long EmpId)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForSurveyorVerificationDLL(EmpId);
        }

        //Aproved Application Work Order View
        public VM_ODClaimApprovedApplicationDetails GetApprovedApplicationListBLL(long EmpID, string Category)
        {
            return _IMBClaimsDLL.GetApprovedApplicationListDLL(EmpID,Category);
        }
        public VM_ODClaimWorkOrderDetails GetODClaimAprvdAppDetailsBLL(long Empid, string PolicyNumber, string Category)
        {
            return _IMBClaimsDLL.GetODClaimAprvdAppDetailsDLL(Empid, PolicyNumber,Category);
        }
        public List<GetVehicleChassisPolicyDetails> GetVehicleAndPolicyDetailsBLL(string textDetails)
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
        public  List<GetVehicleChassisPolicyDetails> GetMVCGetDetailsOnChassisBLL(string ChassisNo) {

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
    }
}
