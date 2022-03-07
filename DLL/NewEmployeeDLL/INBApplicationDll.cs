using KGID_Models.Dashboard;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_User;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL.NewEmployeeDLL
{
    public interface INBApplicationDll
    {
        //int SaveNBNominee(tbl_nb_nominee_details objNominee);
        //long SaveNBAddress(tbl_employee_address_details objBasic);
        //int SaveNBNominee(NomineeList objNominee);
        //int SaveNBPayscale(VM_KGIDDetails objPayscale);
        //int SaveNBFamily(VM_FamilyDetails objFamily);
        //int SaveNBPersonal(tbl_personal_health_details objPersonal);
        //int SaveNBPersonal(VM_PersonalDetails objPersonal);
        long SaveNBDeclaration(tbl_nb_declaration_master objDecl);
        int ChallanDetailsDll(tbl_challan_details objChallan);
        tbl_new_employee NEBasicDetailsDll(long EmployeeCode);
        //tbl_insured_details BasicDetailsDll(long EmployeeCode);
        //PremiumDetails KGIDDetailsDll(long EmployeeCode);
        //tbl_nb_nominee_details NomineeDetailsDll(long EmployeeCode);
        //VM_FamilyDetails FamilyDetailsDll(long EmployeeCode);
        //VM_PersonalDetails PersonalDetailsDll(long EmployeeCode);
        tbl_nb_declaration_master DeclarationDetailsDll(long EmployeeCode);
        tbl_medical_declaration HDeclarationDetailsDll(long EmployeeCode);
        long SaveHNBDeclaration(tbl_medical_declaration objDecl);

        //NomineeList NomineeDetailsDll(long EmployeeCode);

        //tbl_medical_report_tran HPhysicalDetailsDll(long EmployeeCode);
        //int SaveHPhysicalDetails(tbl_medical_report_tran objHMedical);

        //int SaveHOtherDetails(VM_OtherDetails objHOtherDtl);
        //VM_OtherDetails HOtherDetailsDll(long EmployeeCode);

        //int SaveHHealthDetails(VM_HealthDetails objHHealthDtl);
        //VM_HealthDetails HHealthDetailsDll(long EmployeeCode);
        //List<string> GetKMCCodesByEmployeeId(long empId);
        //Dictionary<int, string> GetDoctorsByKMCCode(string kmcCode);
        //VM_DoctorDetail GetDoctorDetailByKMCCode(string docKMCCode);
        //int SaveHDoctorDetailsBll(VM_DoctorDetails objHDoctorDetails);
        //tbl_emp_doctor_details GetPreviouslySavedDoctorDetails(long empId);
        //int SubmitMedicalForm(long empId);

        string SaveVerifiedDetails(VM_DeptVerificationDetails objVerification);
        //VM_VerificationDetails GetEmployeeDetailsForCWVerification(long empId);
        //VM_VerificationDetails GetEmployeeDetailsForSIVerification(long empId);
        tbl_new_employee_basic_details GetEmployeeDetailsByEmployeeId(long empId);
        //int SaveEmployeeBasicDetails(VM_EditEmployeeDetails employeeDetails);
        tbl_employee_work_details GetEmployeeWorkDetailsByEmployeeId(long empId);
        VM_DeptVerificationDetails GetPreviousVerificationDetails(long empId);
        //VM_VerificationDetails GetEmployeeDetailsForDIOVerification(long empId);
        //int SaveNBBasic(tbl_new_employee_basic_details employeeBasicDetails);
        //VM_VerificationDetails GetEmployeeDetailsForDDVerification(long empId);
        //VM_VerificationDetails GetEmployeeDetailsForDVerification(long empId);
        int ApprovePolicy(long applicationRefNumber);
        //VM_MedicalLeaveDetails MedicalLeaveDetailsDll(long empId);
        //int SaveMedicalLeaveDll(VM_MedicalLeaveDetails objMedicalLeaveDetails);
        //VM_DeptVerificationDetails GetMedicalLeaveDetails(long empId);
        //int DeleteNBFamilyDll(long employeeCode);
        long DeleteNBNomineeDll(long employeeCode);


        //New

        long SaveNBBasic(VM_BasicDetails employeeBasicDetails);
        VM_BasicDetails NewEmployeeBasicDetailsDll(long EmployeeCode);
        VM_PolicyDetails KGIDDetailsDll(long EmployeeCode);

        int SaveNBPolicy(VM_PolicyDetails objPOLICY);

        VM_MPhysicalDetails HPhysicalDetailsDll(long EmployeeCode);
        int SaveHPhysicalDetails(VM_MPhysicalDetails objHMedical);

        int SaveHOtherDetails(VM_MOtherDetails objHOtherDtl);
        VM_MOtherDetails HOtherDetailsDll(long EmployeeCode);

        int SaveHHealthDetails(VM_MOtherDetails objHHealthDtl);
        VM_MOtherDetails HHealthDetailsDll(long EmployeeCode);
		
		long RestrictNomineeDetails(long EmpId);
        long SaveNBNominee(VM_NomineeDetail objNominee);
		VM_NomineeDetails NomineeDetailsDll(long EmployeeCode);
		
		Dictionary<long, string> GetDoctorsByKMCCode(string kmcCode);
		VM_DoctorDetail GetDoctorDetailByKMCCode(string docKMCCode);
		
		int SaveHDoctorDetailsBll(VM_DoctorDetails objHDoctorDetails);
        VM_DoctorDetail GetPreviouslySavedDoctorDetails(long empId);
		
		int SubmitMedicalForm(long empId);
        IList<tbl_family_relation_master> GetRelations();
		
		string SaveEmployeeBasicDetails(VM_BasicDetails employeeDetails);
		string AddEmployeeBasicDetails(VM_BasicDetails employeeDetails);
        VM_PersonalHealthDetails PersonalDetailsDll(long EmployeeCode);
        int SaveNBPersonal(VM_PersonalHealthDetails objPersonal);
        VM_MedicalLeaveDetails MedicalLeaveDetailsDll(long empId, string loginType);
        VM_MedicalLeaveDetails SaveMedicalLeaveDll(VM_MedicalLeaveDetails objMedicalLeaveDetails);
        string UploadMedicalLeaveDoc(MedicalLeaveData objMedicalLeaveDetails);
        int DeleteMedicalLeaveDll(long EmpCode);

        VM_DDOVerificationDetails GetEmployeeDetailsForDDOVerification(long empId);
        VM_VerificationDetails GetEmployeeDetailsForCWVerification(long empId);
        VM_VerificationDetails GetEmployeeDetailsForSIVerification(long empId);
        VM_VerificationDetails GetEmployeeDetailsForDIOVerification(long empId);
        VM_VerificationDetails GetEmployeeDetailsForDDVerification(long empId);
        VM_VerificationDetails GetEmployeeDetailsForDVerification(long empId);

        VM_DeptVerificationDetails GetMedicalLeaveDetails(long empId, long applicationId);

        IList<VM_WorkflowDetail> GetWorkFlowDetails(long applicationId);
        VM_DeptVerificationDetails GetPolicyCalculations(long empId, long applicationId);
        VM_ApplicationDetail GenerateApplicationNumber(long empId);
        VM_VerificationDetails GetEmployeeDetailsForAVGCWVerification(long empId);
		int SaveNBFamily(VM_FamilyDetails objFamily);
        VM_FamilyDetails FamilyDetailsDll(long EmployeeCode);
        int DeleteNBFamilyDll(long leaveid);
        int CheckFamilyMemberDetailsDll(long FMID);
        int SaveNBDdoUploadStatusBll(long empid);
        BindDropDownModel GetNomineeNameListBll(long empId,string type);

        long SaveNBPaymentDll(VM_PaymentDetails objPaymentDetails);
        long SaveNBChallanStatusDll(VM_PaymentDetails objPaymentDetails);
        VM_PaymentDetails NBPaymentDll(long EmpID);
        VM_PaymentDetails NBChallanDetailsDll(long EmpID);
        VM_PaymentDetails NBPaymentDownloadDll(long EmpID);
        string DSCLogin(long kgidno, string publickey);
        IEnumerable<SelectListItem> GetFamilyRelationList(long EmpId);

        int NBApplicationCancel(long AppId, long EmpId, string Comments);
        int SaveDDOMedicalLeaveDll(MedicalLeaveData objMedicalLeaveDetails);
        string GetNomineeList(long AppID,string IsMarried);
        string CheckEmployeeAge(long EmployeeID);
        string SavePolicyGeneration(VM_PolicyGeneration objPG);
        string UpdateNBChallanStatusDll(string ChallanRefNo,long StatusCode, long EmpID);
        List<VM_EmpDashboardData> GetDetailsBasedOnKGIDNo(long KgidNo);
        string GetEmailSMSTemplate(long templateid);
        string GetUserDetails(long KGIDNumber);
        string GetUserDetailsbasedOnMobNum(long MobileNumber);
        List<VM_ApplicationDetail> GetpaymentapplicationDetails(string KGIDNumber);
        string UpdatePaymentVerification(long paymentstatus, long challanId);
        string DeleteForPaymentVerification(long cd_challan_id, string cd_challan_ref_no, long applicationId,string ChallanStatusRowCount);
        string UpdateUserDetails(long KGIDNumber, long MobileNumber,string EMailID, long EmployeeId);
        List<VM_PolicyGeneration> GetPolicyList(long EmpID);
        int SavePolicyStatus(long ID);
        string GetDIOOfficeDetails(string DDOCode, string KGIDNo);
        string GetDDODetails(long KGIDNumber);
        string CheckDDODetails(long KGIDNumber, string DDOCode, string DIOOffice);
        string UpdateDDODetails(long KGIDNumber, string DDOCode, string DIOOffice);
        List<VM_trackDetails> getProposerTrackDetailsDll(string applicationNo);

        //
        Task<bool> UpdateUserDetails1(long KGIDNumber, long MobileNumber, string EMailID, long EmployeeId);
        VM_DeptVerificationDetails GetUploadedDocuments(long empId, long applicationId);
        Task<bool> IsMobNoAlreadyExistsAsync(long KGIDNumber, long MobileNumber, long EmployeeId, string EMailID);
    }
}
