using KGID_Models.KGID_Policy;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDMotorInsurance
{
    public interface IMotorInsuranceVehicleDetailsDll
    {
        
        string SaveMIVehicleDetailsData(VM_MotorInsuranceVehicleDetails vmVehicleDetails);
        VM_MotorInsuranceVehicleDetails GetRTODetailsBll(string chasisNo, string EngineNo);

        VM_MotorInsuranceOtherDetails OtherDetailsDll(long EmployeeCode, long ReferenceId);
        int SaveMIOtherDetailsDll(VM_MotorInsuranceOtherDetails objOD);

        VM_MotorInsuranceIDVDetails IDVDetailsDll(long EmployeeCode, long ReferenceId);
        int SaveMIIDVDetailsDll(VM_MotorInsuranceIDVDetails objIDV);
        
		VM_MotorInsurancePreviousHistoryDetails PreviousHistoryDetails(long EmployeeCode, long ReferenceId);
        VM_MotorInsuranceVehicleDetails GetMIVehicleDetails(long Empid, long RefID);
        string CheckVehicleExists(string chassisNo, string engineNo);

        VM_MI_Upload_Documents MIDocumentDetailsDll(long EmployeeCode, long ReferenceId);
        int SaveMIDocumentDetailsDll(VM_MI_Upload_Documents objMIDocDetails);
        string SaveMIChallanDetailsDll(long EmpID, int Category, string RefNos, int ChallanAmount, string Type);
        string InsertChallanDetailsDll(long EmpID, int Category, string Applications, int ChallanAmount, string sanno, DateTime sandate, string voucherno, string hoa, string ddocode, string Type);
        VM_ChallanPrintDetails PrintMIChallanDetailsDll(long EmpID, int Category, string RefNos, string Type);
        string SaveMIPreviousHistoryDetails(VM_MotorInsurancePreviousHistoryDetails vmPreviousHistoryDetails);

        VM_DDOVerificationDetailsMI GetEmployeeDetailsForDDOVerification(long empId);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForCWVerification(long empId);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForSuperintendentVerification(long empId);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForDIOVerification(long empId);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForADVerification(long empId);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForDDVerification(long empId);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForDVerification(long empId);

        
        IList<VM_MIWorkFlowDetails> GetWorkFlowDetails(long applicationId,int category);

        string SaveVerifiedDetailsBll(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails);
        VM_MotorInsuranceVehicleDetails GetModelListBasedonMake(int makeid);

        VM_DDOVerificationDetailsMI getMIApplicationEmployeeList(long empId, int Category);
        VM_DDOVerificationDetailsMI getMIApplicationEmployeeStatusList(long empId, int Category);

        PolicyPremiumDetailMI selectPaymentDetailsMI(string pagetype,long empId, int applicationId);
         IEnumerable<tbl_vehicle_category_master> GetVehCatergoryList(string TypeId, long SubTypeID);
        VM_MotorInsuranceVehicleDetails GetManufactureListBasedonMake(int makeid);
        VM_MotorInsuranceVehicleDetails GetTypeOfVehiclebasedonCategory(int categoryID);
        VM_MotorInsuranceVehicleDetails GetMIRenewalVehicleDetails(long Empid, long RefID,long RenewalRefNo);
		VM_MotorInsurancePolicyPrintDetails MIPolicyPrintDetailsDll(string Type,long EmployeeCode, long ReferenceId);
        //Save MB Bond
        string MBBondDocUploadDll(long AppId, long EmpId, string DocPath, string DocType);
        //Sign MB Bond
        string GetMBBondDocFileDLL(long AppId, long EmpId);
        string MBSignBondUploadDLL(long AppId, long EmpId, string DocPath);

        VM_MBApplicationDetails GetMBApplicationListDll(long EmpID, int Category);

        VM_DDOVerificationDetailsMI getMIRenewalApplicationList(long empId, int Category);
        string GetEmployeeLoanDetails(long UserID);
        //GetMBBondDetails
        VM_DDOVerificationDetailsMI GetMBBondDetailsDll(long empId);
        #region Renewal Workflow
        VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForCWVerification(long empId);
        VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForSuperintendentVerification(long empId);
        VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForADVerification(long empId);
        VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForDDVerification(long empId);
        VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForDVerification(long empId);


        IList<VM_MIWorkFlowDetails> GetRenewalWorkFlowDetails(long applicationId, int category);
        string SaveRenewalVerifiedDetailsDll(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails);
        VM_MotorInsuranceVehicleDetails BindVahanResponseDetailstoModel(dynamic responseStr);
        #endregion

        #region Motor Insurance Policy Cancellation
        VM_MotorInsuranceCancellation GetCancelReasonList();
        string CheckVehicleNo(string vehicleNo, int Category);
        int MIAppCancelRequestAction(VM_MotorInsuranceCancellation objMIcancellation);
        VM_MotorInsuranceCancellation VehicleDetailsForCancellationDll(string vehicleNo);
        #endregion


        //ICT  18-09-2021
        string UpdateBPS025Ack(string AckFilePath, string ChallanRefno);

        string UpdateBPS025Request(string Upload, string ChallanRefno);

        VM_MotorInsurancePaymentStatus MotorInsurancePaymentStatusDll(string EmpId);
        //ICT CHANGE 14-SEP-2021
        VM_DDOVerificationDetailsMI GetChallanDetailsDll(string ChallanNo, long EmpID);
    }
}
