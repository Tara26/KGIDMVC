using KGID_Models.KGID_User;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using KGID_Models.NBApplication;
using KGID_Models.Dashboard;

namespace BLL.NewEmployeeBLL
{
    public interface INBApplicationBll
    {
        //int SaveNBNomineeBll(tbl_nb_nominee_details objNominee);
        //long SaveNBAddressBll(tbl_employee_address_details objBasicDetails);
        //int SaveNBNomineeBll(NomineeList objNominee);
        //int SaveNBPayscaleBll(VM_KGIDDetails objPayscale);
        //int SaveNBFamilyBll(VM_FamilyDetails objFamilyDetails);
        //int SaveNBPersonalBll(tbl_personal_health_details objPersonalDetails);
        //int SaveNBPersonalBll(VM_PersonalDetails objPersonalDetails);
        long SaveNBDeclarationBll(tbl_nb_declaration_master objDeclartaionDetails);
        int SaveChallanBll(tbl_challan_details objChallan);
        tbl_new_employee NEBasicDetailsBll(long EmployeeCode);
        //tbl_insured_details BasicDetailsBll(long EmployeeCode);
        //PremiumDetails KGIDDetailsBll(long EmployeeCode);
        //tbl_nb_nominee_details NomineeDetailsBll(long EmployeeCode);
        //VM_FamilyDetails FamilyDetailsBll(long EmployeeCode);
        //VM_PersonalDetails PersonalDetailsBll(long EmployeeCode);
        tbl_nb_declaration_master DeclarationDetailsBll(long EmployeeCode);

        //VM_BasicDetails BasicDetailsBll(long EmployeeCode);
        //NomineeList NomineeDetailsBll(long EmployeeCode);


        //tbl_medical_report_tran HPhysicalDetailsBll(long EmployeeCode);
        //int SaveHPhysicalDetailsBll(tbl_medical_report_tran objHPhysicalDetails);
        VM_DeptVerificationDetails GetMedicalLeaveDetails(long empId,long applicationId);
        //int SaveHOtherDetailsBll(VM_OtherDetails objHOtherDetails);
        //VM_OtherDetails HOtherDetailsBll(long EmployeeCode);

        //VM_HealthDetails HHealthDetailsBll(long EmployeeCode);
        //int SaveHHealthDetailsBll(VM_HealthDetails objHHealthDetails);

        //List<string> GetKMCCodesByEmployeeId(long empId);
        IEnumerable<SelectListItem> GetDoctorsByKMCCode(string kmcCode);
        VM_DoctorDetail GetDoctorDetailByKMCCode(string docId);
        int SaveHDoctorDetailsBll(VM_DoctorDetails objHDoctorDetails);
        VM_DoctorDetails GetDoctorDetails(long empId);
        int SubmitMedicalForm(long empId);
        VM_DDOVerificationDetails GetEmployeeDetailsForDDOVerification(long empId);
        tbl_medical_declaration HDeclarationDetailsBll(long EmployeeCode);
        long SaveHNBDeclarationBll(tbl_medical_declaration objDeclartaionDetails);


        string SaveVerifiedDetailsBll(VM_DeptVerificationDetails objVerification);
        //VM_VerificationDetails GetEmployeeDetailsForCWVerification(long empId);
        VM_EditEmployeeDetails GetEmployeeDetailsById(long empId);
        VM_VerificationDetails GetEmployeeDetailsForAVGCWVerification(long empId);

        ////int SaveEmployeeBasicDetails(VM_BasicDetails employeeDetails);
        //VM_DeptVerificationDetails GetPreviousVerificationDetails(long empId);
        //VM_VerificationDetails GetEmployeeDetailsForSIVerification(long empId);
        //VM_VerificationDetails GetEmployeeDetailsForDIOVerification(long empId);
        //VM_VerificationDetails GetEmployeeDetailsForDDVerification(long empId);
        //VM_VerificationDetails GetEmployeeDetailsForDVerification(long empId);
        //VM_MedicalLeaveDetails MedicalLeaveDetailsBll(long empId);
        //int SaveNBMedicalLeaveBll(VM_MedicalLeaveDetails objMedicalLeaveDetails);
        //int DeleteNBFamilyBll(long employeeCode);
        long DeleteNBNomineeBll(long nd_sys_emp_code);

        //New

        long SaveNBBasicBll(VM_BasicDetails basicDetails);

        VM_BasicDetails BasicDetailsBll(long EmployeeCode);
        VM_PolicyDetails KGIDDetailsBll(long EmployeeCode);
        int SaveNBPolicyBll(VM_PolicyDetails policyDetails);


        VM_MPhysicalDetails HPhysicalDetailsBll(long EmployeeCode);
        int SaveHPhysicalDetailsBll(VM_MPhysicalDetails objHPhysicalDetails);


        int SaveHOtherDetailsBll(VM_MOtherDetails objHOtherDetails);
        VM_MOtherDetails HOtherDetailsBll(long EmployeeCode);

        VM_MOtherDetails HHealthDetailsBll(long EmployeeCode);
        int SaveHHealthDetailsBll(VM_MOtherDetails objHHealthDetails);
		
        long RestrictNomineeDetails(long EmpId);
		long SaveNBNomineeBll(VM_NomineeDetail objNominee);
        VM_NomineeDetails NomineeDetailsBll(long EmployeeCode);
		
		int SaveNBPersonalBll(VM_PersonalHealthDetails objPersonalDetails);
        string SaveEmployeeBasicDetails(VM_BasicDetails employeeDetails);
        string AddEmployeeBasicDetails(VM_BasicDetails employeeDetails);
        VM_PersonalHealthDetails PersonalDetailsBll(long EmployeeCode);
        VM_MedicalLeaveDetails MedicalLeaveDetailsBll(long empId,string loginType);
        VM_MedicalLeaveDetails SaveNBMedicalLeaveBll(VM_MedicalLeaveDetails objMedicalLeaveDetails);
        string UploadMedicalLeaveDoc(MedicalLeaveData objMedicalLeaveDetails);
        int DeleteMedicalLeaveBll(long EmpCode);

        VM_VerificationDetails GetEmployeeDetailsForCWVerification(long empId);
        VM_DeptVerificationDetails GetPreviousVerificationDetails(long empId);
        VM_VerificationDetails GetEmployeeDetailsForSIVerification(long empId);
        VM_VerificationDetails GetEmployeeDetailsForDIOVerification(long empId);
        VM_VerificationDetails GetEmployeeDetailsForDDVerification(long empId);
        VM_VerificationDetails GetEmployeeDetailsForDVerification(long empId);

        IList<VM_WorkflowDetail> GetWorkFlowDetails(long applicationId);
        VM_DeptVerificationDetails GetPolicyCalculations(long empId, long applicationId);
		int SaveNBFamilyBll(VM_FamilyDetails objFamilyDetails);
        int CheckFamilyMemberDetailsBll(long FMID);
        VM_FamilyDetails FamilyDetailsBll(long EmployeeCode);
        int DeleteNBFamilyBll(long leaveid);
        int SaveNBDdoUploadStatusBll(long empid);
        VM_ApplicationDetail GenerateApplicationNumber(long empId);
        BindDropDownModel GetNomineeNameListBll(long empId,string type);

        long SaveNBPaymentBll(VM_PaymentDetails objPaymentDetails);
        long SaveNBChallanStatusDll(VM_PaymentDetails objPaymentDetails);
        VM_PaymentDetails NBPaymentDll(long EmpID);
        VM_PaymentDetails NBChallanDetailsDll(long EmpID);
        VM_PaymentDetails NBPaymentDownloadDll(long EmpID);
        string DSCLogin(long empid, string publickey);
        IEnumerable<SelectListItem> GetFamilyRelationList(long EmpId);

        int NBApplicationCancel(long AppId, long EmpId, string Comments);
        int SaveDDOMedicalLeaveBll(MedicalLeaveData objMedicalLeaveDetails);
        string GetNomineeList(long AppID,string IsMarried);
        string CheckEmployeeAge(long EmployeeID);
        string SavePolicyGeneration(VM_PolicyGeneration objPG);
        string UpdateNBChallanStatusDll(string ReqChallanRefNo,long StatusCode, long EmpID);

        List<VM_EmpDashboardData> GetDetailsBasedOnKGIDNo(long KgidNo);
        string GetEmailSMSTemplate(long templateid);

        string GetUserDetails(long KGIDNumber);
        string GetUserDetailsbasedOnMobNum(long MobileNumber);
        List<VM_ApplicationDetail> GetpaymentapplicationDetails(string KGIDNumber);
        string UpdatePaymentVerification(long paymentstatus, long challanId);
        string DeleteForPaymentVerification(long cd_challan_id, string cd_challan_ref_no, long applicationId,string ChallanStatusRowCount);
        string UpdateUserDetails(long KGIDNumber, long MobileNumber, string EMailID, long EmployeeId);

        List<VM_PolicyGeneration> GetPolicyList(long EmpID);
        int SavePolicyStatus(long ID);
		
		string GetDIOOfficeDetails(string DDOCode,string KGIDNo);
		string GetDDODetails(long KGIDNumber);
        string CheckDDODetails(long KGIDNumber, string DDOCode, string DIOOffice);
        string UpdateDDODetails(long KGIDNumber, string DDOCode, string DIOOffice);
        List<VM_trackDetails> getProposerTrackDetailsBll(string applicationNo);
        VM_DeptVerificationDetails GetUploadedDocumentBll(long empId, long applicationId);
        //
        Task<bool> UpdateUserDetails1(long KGIDNumber, long MobileNumber, string EMailID, long EmployeeId);
        Task<bool> IsMobNoAlreadyExistsAsync(long KGIDNumber, long MobileNumber, long EmployeeId, string EMailID);
    }
}
