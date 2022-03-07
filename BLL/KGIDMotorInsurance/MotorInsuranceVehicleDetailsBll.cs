using DLL.KGIDMotorInsurance;
using KGID_Models.KGID_Policy;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDMotorInsurance
{
  public  class MotorInsuranceVehicleDetailsBll:IMotorInsuranceVehicleDetailsBll
    {
        private readonly IMotorInsuranceVehicleDetailsDll _IMotorInsuranceVehicleDetailsDll;
        public MotorInsuranceVehicleDetailsBll()
        {
            this._IMotorInsuranceVehicleDetailsDll = new MotorInsuranceVehicleDetailsDll();
        }
       public  VM_MotorInsuranceVehicleDetails GetMIVehicleDetails(long Empid, long RefID)
        {
           return _IMotorInsuranceVehicleDetailsDll.GetMIVehicleDetails(Empid, RefID);
        }
        public string CheckVehicleExists(string chassisNo, string engineNo)
        {
            return _IMotorInsuranceVehicleDetailsDll.CheckVehicleExists(chassisNo, engineNo);
        }
        public VM_MotorInsuranceVehicleDetails GetMIRenewalVehicleDetails(long Empid, long RefID,long RenewalRefNo)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetMIRenewalVehicleDetails(Empid, RefID, RenewalRefNo);
        }

        public string SaveMIVehicleDetailsData(VM_MotorInsuranceVehicleDetails vmVehicleDetails)
        {
            return _IMotorInsuranceVehicleDetailsDll.SaveMIVehicleDetailsData(vmVehicleDetails);
        }
        public VM_MotorInsuranceVehicleDetails GetRTODetailsBll(string chasisNo, string EngineNo)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetRTODetailsBll(chasisNo, EngineNo);
        }
        public VM_MotorInsuranceOtherDetails OtherDetailsBll(long EmployeeCode, long ReferenceId)
        {
            var x = _IMotorInsuranceVehicleDetailsDll.OtherDetailsDll(EmployeeCode,ReferenceId);
            return x;

        }
        public int SaveOtherDetailsBll(VM_MotorInsuranceOtherDetails objPersonalDetails)
        {
            var result = _IMotorInsuranceVehicleDetailsDll.SaveMIOtherDetailsDll(objPersonalDetails);
            return result;
        }
        public VM_MotorInsuranceIDVDetails IDVDetailsBll(long EmployeeCode, long ReferenceId)
        {
            var x = _IMotorInsuranceVehicleDetailsDll.IDVDetailsDll(EmployeeCode,ReferenceId);
            return x;

        }
        public int SaveIDVDetailsBll(VM_MotorInsuranceIDVDetails objIDVDetails)
        {
            var result = _IMotorInsuranceVehicleDetailsDll.SaveMIIDVDetailsDll(objIDVDetails);
            return result;
        }
		public VM_MotorInsurancePreviousHistoryDetails PreviousHistoryDetails(long EmployeeCode, long ReferenceId)
        {
            return _IMotorInsuranceVehicleDetailsDll.PreviousHistoryDetails(EmployeeCode, ReferenceId);
        }
      public  string  SaveMIPreviousHistoryDetails(VM_MotorInsurancePreviousHistoryDetails vmPreviousHistoryDetails)
        {
            return _IMotorInsuranceVehicleDetailsDll.SaveMIPreviousHistoryDetails(vmPreviousHistoryDetails);
        }
        public VM_MI_Upload_Documents MIDocumentDetailsBll(long EmployeeCode, long ReferenceId)
        {
            return _IMotorInsuranceVehicleDetailsDll.MIDocumentDetailsDll(EmployeeCode, ReferenceId);
            //return x;
        }
        public int SaveMIDocumentDetailsBll(VM_MI_Upload_Documents objMIDocDetails)
        {
            return _IMotorInsuranceVehicleDetailsDll.SaveMIDocumentDetailsDll(objMIDocDetails);
            //return result;
        }
        public string SaveMIChallanDetailsBll(long EmpID, int Category, string RefNos, int ChallanAmount, string Type)
        {
            return _IMotorInsuranceVehicleDetailsDll.SaveMIChallanDetailsDll(EmpID,Category,RefNos,ChallanAmount,Type);
        }
        // ICT
        public string InsertChallanDetailsBll(long EmpID, int Category, string RefNos, int ChallanAmount, string sanno, DateTime sandate, string voucherno, string hoa, string ddocode, string Type)
        {
            return _IMotorInsuranceVehicleDetailsDll.InsertChallanDetailsDll(EmpID, Category, RefNos, ChallanAmount, sanno, sandate, voucherno, hoa, ddocode, Type);
        }

        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForDDOVerification(long EmpId)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeDetailsForDDOVerification(EmpId);
        }

        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForCWVerification(long EmpId)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeDetailsForCWVerification(EmpId);
        }
        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForSuperintendentVerification(long EmpId)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeDetailsForSuperintendentVerification(EmpId);
        }
        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForDIOVerification(long EmpId)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeDetailsForDIOVerification(EmpId);
        }
        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForADVerification(long EmpId)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeDetailsForADVerification(EmpId);
        }
        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForDDVerification(long EmpId)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeDetailsForDDVerification(EmpId);
        }
       
        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForDVerification(long EmpId)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeDetailsForDVerification(EmpId);
        }
        public  IList<VM_MIWorkFlowDetails> GetWorkFlowDetails(long applicationId,int category)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetWorkFlowDetails(applicationId,category);
        }
        public string SaveVerifiedDetailsBll(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            return _IMotorInsuranceVehicleDetailsDll.SaveVerifiedDetailsBll(objVerifyDetails);
        }
        public VM_MotorInsuranceVehicleDetails GetModelListBasedonMake(int makeid)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetModelListBasedonMake(makeid);
        }
        public VM_MotorInsuranceVehicleDetails GetManufactureListBasedonMake(int makeid)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetManufactureListBasedonMake(makeid);
        }
        public VM_MotorInsuranceVehicleDetails GetTypeOfVehiclebasedonCategory(int categoryID)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetTypeOfVehiclebasedonCategory(categoryID);
        }
        public VM_DDOVerificationDetailsMI getMIApplicationEmployeeList(long empId,int Category)
        {
            return _IMotorInsuranceVehicleDetailsDll.getMIApplicationEmployeeList(empId, Category);
        }
        public VM_DDOVerificationDetailsMI getMIApplicationEmployeeStatusList(long empId, int Category)
        {
            return _IMotorInsuranceVehicleDetailsDll.getMIApplicationEmployeeStatusList(empId, Category);
        }
        public PolicyPremiumDetailMI selectPaymentDetailsMI(string PageType,long empId, int applicationId)
        {
            return _IMotorInsuranceVehicleDetailsDll.selectPaymentDetailsMI(PageType,empId, applicationId);
        }

        public IEnumerable<tbl_vehicle_category_master> GetVehCatergoryList(string TypeId, long SubTypeID)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetVehCatergoryList(TypeId, SubTypeID);
        }

        public VM_MotorInsurancePolicyPrintDetails MIPolicyPrintDetailsBll(string Type,long EmployeeCode, long ReferenceId)
        {
            return _IMotorInsuranceVehicleDetailsDll.MIPolicyPrintDetailsDll(Type,EmployeeCode, ReferenceId);

        }
        //Save MB Bond
        public string MBBondDocUploadBLL(long AppId, long EmpId, string DocPath, string DocType)
        {
            return _IMotorInsuranceVehicleDetailsDll.MBBondDocUploadDll(AppId, EmpId, DocPath, DocType);
        }
        //Sign MB Bond
        public string GetMBBondDocFileBLL(long AppId, long EmpId)
        {
            var result = _IMotorInsuranceVehicleDetailsDll.GetMBBondDocFileDLL(AppId, EmpId);
            return result;
        }
        public string MBSignBondUploadBLL(long AppId, long EmpId, string DocPath)
        {
            var result = _IMotorInsuranceVehicleDetailsDll.MBSignBondUploadDLL(AppId, EmpId, DocPath);
            return result;
        }

        //GET MB Bond Details
        public VM_DDOVerificationDetailsMI GetMBBondDetailsBll(long empId)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetMBBondDetailsDll(empId);
        }
        public VM_DDOVerificationDetailsMI getMIRenewalApplicationList(long empId, int Category)
        {
            return _IMotorInsuranceVehicleDetailsDll.getMIRenewalApplicationList(empId, Category);
        }
        public VM_MBApplicationDetails GetMBApplicationListBll(long EmpID, int Category)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetMBApplicationListDll(EmpID,Category);
        }


        #region Renewal Workflow
        public VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForCWVerification(long EmpID)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeRenewalDetailsForCWVerification(EmpID);
        }
        public VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForSuperintendentVerification(long EmpID)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeRenewalDetailsForSuperintendentVerification(EmpID);
        }
        public VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForADVerification(long EmpID)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeRenewalDetailsForADVerification(EmpID);
        }
        public VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForDDVerification(long EmpID)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeRenewalDetailsForDDVerification(EmpID);
        }
        public VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForDVerification(long EmpID)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeRenewalDetailsForDVerification(EmpID);
        }


        public IList<VM_MIWorkFlowDetails> GetRenewalWorkFlowDetails(long applicationId, int category)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetRenewalWorkFlowDetails(applicationId, category);
        }
        public string SaveRenewalVerifiedDetailsBll(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails)
        {
            return _IMotorInsuranceVehicleDetailsDll.SaveRenewalVerifiedDetailsDll(objVerifyDetails);
        }

        public string GetEmployeeLoanDetails(long UserId)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetEmployeeLoanDetails(UserId);
        }
        #endregion

        public VM_MotorInsuranceVehicleDetails BindVahanResponseDetailstoModel(dynamic responseStr)
        {
            return _IMotorInsuranceVehicleDetailsDll.BindVahanResponseDetailstoModel(responseStr);
        }

        public VM_MotorInsuranceCancellation GetCancelReasonList()
        {
            return _IMotorInsuranceVehicleDetailsDll.GetCancelReasonList();
        }

        public string CheckVehicleNo(string vehicleNo, int Category)
        {
            return _IMotorInsuranceVehicleDetailsDll.CheckVehicleNo(vehicleNo, Category);
        }

        public int MIAppCancelRequestAction(VM_MotorInsuranceCancellation objMIcancellation)
        {
            return _IMotorInsuranceVehicleDetailsDll.MIAppCancelRequestAction(objMIcancellation);
        }

        public VM_MotorInsuranceCancellation VehicleDetailsForCancellation(string vehicleNo)
        {
            return _IMotorInsuranceVehicleDetailsDll.VehicleDetailsForCancellationDll(vehicleNo);
        }
        

        // ICT  18-09-2021
        public string UpdateBPS025AckBll(string AckFilePath, string ChallanRefno)
        {
            return _IMotorInsuranceVehicleDetailsDll.UpdateBPS025Ack(AckFilePath,ChallanRefno);
        }
        public VM_MotorInsurancePaymentStatus MotorInsurancePaymentStatusBll(string EmpId)
        {
            return _IMotorInsuranceVehicleDetailsDll.MotorInsurancePaymentStatusDll(EmpId);
        }

        public string UpdateBPS025RequestBll(string Upload, string ChallanRefno)
        {
            return _IMotorInsuranceVehicleDetailsDll.UpdateBPS025Request(Upload, ChallanRefno);
        }

        public VM_ChallanPrintDetails PrintMIChallanDetailsBll(long EmpID, int Category, string RefNos, string Type)
        {
            return _IMotorInsuranceVehicleDetailsDll.PrintMIChallanDetailsDll(EmpID, Category, RefNos, Type);
        }

        public VM_DDOVerificationDetailsMI GetChallanDetailsBll(string ChallanNo, long EmpID)
        {
            return _IMotorInsuranceVehicleDetailsDll.GetChallanDetailsDll(ChallanNo, EmpID);
        }

    }
}
