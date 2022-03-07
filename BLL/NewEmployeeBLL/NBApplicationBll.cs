using DLL.NewEmployeeDLL;
using KGID_Models.KGID_User;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using KGID_Models.NBApplication;
using KGID_Models.Dashboard;
using System.Threading.Tasks;

namespace BLL.NewEmployeeBLL
{
    public class NBApplicationBll : INBApplicationBll
    {
        private readonly INBApplicationDll _INBApplicationdll;
        public NBApplicationBll()
        {
            this._INBApplicationdll = new NBApplicationDll();
        }
        //public int SaveNBNomineeBll(NomineeList objNominee)
        //{
        //   var result = _INBApplicationdll.SaveNBNominee(objNominee);
        //    return result;
        //}

        //public long SaveNBAddressBll(tbl_employee_address_details objBasicDetails)
        //{
        //    var result = _INBApplicationdll.SaveNBAddress(objBasicDetails);
        //    return result;
        //}
        //public int SaveNBPayscaleBll(VM_KGIDDetails objPayscale)
        //{
        //    var result = _INBApplicationdll.SaveNBPayscale(objPayscale);
        //    return result;
        //}
        //public int SaveNBFamilyBll(VM_FamilyDetails objFamilyDetails)
        //{
        //    var result = _INBApplicationdll.SaveNBFamily(objFamilyDetails);
        //    return result;
        //}
        //public int SaveNBPersonalBll(VM_PersonalDetails objPersonalDetails)
        //{
         //   var result = _INBApplicationdll.SaveNBPersonal(objPersonalDetails);
        //    return result;
        //}
        public long SaveNBDeclarationBll(tbl_nb_declaration_master objDeclartaionDetails)
        {
            var result = _INBApplicationdll.SaveNBDeclaration(objDeclartaionDetails);
            return result;
        }

        public tbl_new_employee NEBasicDetailsBll(long EmployeeCode)
        {
            
            var x = _INBApplicationdll.NEBasicDetailsDll(EmployeeCode);
            return x;

        }
        //public NomineeList NomineeDetailsBll(long EmployeeCode)
        //{

         //   var x = _INBApplicationdll.NomineeDetailsDll(EmployeeCode);
         //   return x;

        //}
        //public VM_FamilyDetails FamilyDetailsBll(long EmployeeCode)
        //{

        //    var x = _INBApplicationdll.FamilyDetailsDll(EmployeeCode);
        //    return x;

        //}
        //public VM_PersonalDetails PersonalDetailsBll(long EmployeeCode)
        //{
        //    var x = _INBApplicationdll.PersonalDetailsDll(EmployeeCode);
        //    return x;

        //}
        public tbl_nb_declaration_master DeclarationDetailsBll(long EmployeeCode)
        {

            var x = _INBApplicationdll.DeclarationDetailsDll(EmployeeCode);
            return x;

        }

        public tbl_medical_declaration HDeclarationDetailsBll(long EmployeeCode)
        {

            var x = _INBApplicationdll.HDeclarationDetailsDll(EmployeeCode);
            return x;

        }

        //public int SaveHPhysicalDetailsBll(tbl_medical_report_tran objHPhysicalDetails)
        //{
        //    var result = _INBApplicationdll.SaveHPhysicalDetails(objHPhysicalDetails);
        //    return result;
        //}

        //public tbl_medical_report_tran HPhysicalDetailsBll(long EmployeeCode)
        //{
        //    var x = _INBApplicationdll.HPhysicalDetailsDll(EmployeeCode);
        //    return x;
        //}
        
        //public int SaveHOtherDetailsBll(VM_OtherDetails objHOtherDetails)
        //{
        //    return _INBApplicationdll.SaveHOtherDetails(objHOtherDetails);
        //}

        //public VM_OtherDetails HOtherDetailsBll(long EmployeeCode)
        //{
        //    var x = _INBApplicationdll.HOtherDetailsDll(EmployeeCode);
        //    return x;
        //}
        
        //public int SaveHHealthDetailsBll(VM_HealthDetails objHHealthDetails)
        //{
        //    return _INBApplicationdll.SaveHHealthDetails(objHHealthDetails);
        //}
        //public VM_HealthDetails HHealthDetailsBll(long EmployeeCode)
        //{
        //    var x = _INBApplicationdll.HHealthDetailsDll(EmployeeCode);
        //    return x;
        //}

        //public List<string> GetKMCCodesByEmployeeId(long empId)
        //{
        //    return _INBApplicationdll.GetKMCCodesByEmployeeId(empId);
        //}

        public IEnumerable<SelectListItem> GetDoctorsByKMCCode(string kmcCode)
        {
            var doctorsDetails = _INBApplicationdll.GetDoctorsByKMCCode(kmcCode);

            if (doctorsDetails != null && doctorsDetails.Count > 0)
            {
                return doctorsDetails.Select(t => new SelectListItem()
                {
                    Value = t.Key.ToString(),
                    Text = t.Value
                });
            }

            return null;
        }

        public VM_DoctorDetail GetDoctorDetailByKMCCode(string docKMCCode)
        {
            return _INBApplicationdll.GetDoctorDetailByKMCCode(docKMCCode);
        }

        public int SaveHDoctorDetailsBll(VM_DoctorDetails objHDoctorDetails)
        {
            return _INBApplicationdll.SaveHDoctorDetailsBll(objHDoctorDetails);
        }

        //private tbl_emp_doctor_details GetPreviouslySavedDoctorDetails(long empId)
        //{
         //   return _INBApplicationdll.GetPreviouslySavedDoctorDetails(empId);
        //}

        //public VM_DoctorDetails GetDoctorDetails(long empId)
        //{
         //   VM_DoctorDetails docDetails = new VM_DoctorDetails();

         //   tbl_emp_doctor_details doctorDetails = GetPreviouslySavedDoctorDetails(empId);

            //var listOfKMCCodes = GetKMCCodesByEmployeeId(empId);
          //  docDetails.EmpId = empId;
            //docDetails.KMCCodes = listOfKMCCodes.Select(kmcCode => new SelectListItem()
            //{
            //    Text = kmcCode,
            //    Value = kmcCode
            //}).ToList();

           // if (doctorDetails != null)
           // {
            //    docDetails.KMCCode = doctorDetails.edd_kmc_code;
            //    docDetails.DoctorDetail.Designation = doctorDetails.edd_designation;
            //    docDetails.DoctorDetail.KGID = doctorDetails.edd_kgid;
            //    docDetails.DoctorDetail.NameOfOffice = doctorDetails.edd_name_of_officer;
             //   docDetails.DoctorDetail.DoctorName = doctorDetails.edd_name_of_doctor;

                //docDetails.KMCCodeId = docDetails.KMCCodes.FirstOrDefault(t => t.Text.Equals(doctorDetails.edd_kmc_code, StringComparison.OrdinalIgnoreCase)).Value;
                //docDetails.Doctors = GetDoctorsByKMCCode(docDetails.KMCCodeId);
                //docDetails.DoctorId = Convert.ToInt32(docDetails.Doctors.FirstOrDefault(t => t.Text.Equals(doctorDetails.edd_name_of_doctor, StringComparison.OrdinalIgnoreCase)).Value); 
            //}

            //return docDetails;
        //}

        public int SubmitMedicalForm(long empId)
        {
            return _INBApplicationdll.SubmitMedicalForm(empId);
        }

        //public VM_DDOVerificationDetails GetEmployeeDetailsForDDOVerification(long empId)
        //{
        //    return _INBApplicationdll.GetEmployeeDetailsForDDOVerification(empId);
        //}

        //public VM_VerificationDetails GetEmployeeDetailsForCWVerification(long empId)
        //{
        //    return _INBApplicationdll.GetEmployeeDetailsForCWVerification(empId);
        //}

        //public VM_VerificationDetails GetEmployeeDetailsForSIVerification(long empId)
        //{
        //    return _INBApplicationdll.GetEmployeeDetailsForSIVerification(empId);
        //}

        //public VM_VerificationDetails GetEmployeeDetailsForDIOVerification(long empId)
        //{
        //    return _INBApplicationdll.GetEmployeeDetailsForDIOVerification(empId);
        //}

        //public VM_VerificationDetails GetEmployeeDetailsForDDVerification(long empId)
        //{
        //    return _INBApplicationdll.GetEmployeeDetailsForDDVerification(empId);
        //}

        //public VM_VerificationDetails GetEmployeeDetailsForDVerification(long empId)
        //{
        //    return _INBApplicationdll.GetEmployeeDetailsForDVerification(empId);
        //}

        public string SaveVerifiedDetailsBll(VM_DeptVerificationDetails objVerification)
        {
            var result = _INBApplicationdll.SaveVerifiedDetails(objVerification);
            return result;
        }

        public VM_EditEmployeeDetails GetEmployeeDetailsById(long empId)
        {
            VM_EditEmployeeDetails employeeDetails = new VM_EditEmployeeDetails();
            employeeDetails.tbl_Employee_Work_Details = new tbl_employee_work_details();
            tbl_new_employee_basic_details newEmployeeDetails = _INBApplicationdll.GetEmployeeDetailsByEmployeeId(empId);

            tbl_employee_work_details employeeWorkDetails = _INBApplicationdll.GetEmployeeWorkDetailsByEmployeeId(empId);
            if (newEmployeeDetails != null)
            {
                employeeDetails.EmpId = newEmployeeDetails.nebd_sys_emp_code.HasValue ? newEmployeeDetails.nebd_sys_emp_code.Value : 0;
                employeeDetails.DateOfAppointment = newEmployeeDetails.nebd_date_of_appointment;
                employeeDetails.DateOfBirth = newEmployeeDetails.nebd_date_of_birth;
                employeeDetails.DDOCode = newEmployeeDetails.nebd_ddo_code;
                employeeDetails.DepartmentCode = newEmployeeDetails.nebd_dept_emp_code;
                employeeDetails.EmailId = newEmployeeDetails.nebd_email;
                employeeDetails.FatherName = newEmployeeDetails.nebd_father_name;
                employeeDetails.Gender = newEmployeeDetails.nebd_gender;
                employeeDetails.IsActive = newEmployeeDetails.nebd_active.HasValue ? newEmployeeDetails.nebd_active.Value : false;
                employeeDetails.MobileNumber = newEmployeeDetails.nebd_mobilenumber;
                employeeDetails.Name = newEmployeeDetails.nebd_emp_full_name;
                employeeDetails.PANNumber = newEmployeeDetails.nebd_pan;
                employeeDetails.PlaceOfBirth = newEmployeeDetails.nebd_place_of_birth;
                employeeDetails.SpouseName = newEmployeeDetails.nebd_spouse_name;

                employeeDetails.Genders = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = "MALE",
                        Value = "MALE"
                    },
                    new SelectListItem()
                    {
                        Text = "FEMALE",
                        Value = "FEMALE"
                    },
                };
            }

            //if (employeeWorkDetails != null)
            //{
            //    employeeDetails.tbl_Employee_Work_Details.ewd_date_of_joining_post = employeeWorkDetails.ewd_date_of_joining_post;
            //    employeeDetails.tbl_Employee_Work_Details.ewd_payscle_code = employeeWorkDetails.ewd_payscle_code ?? "";
            //    employeeDetails.tbl_Employee_Work_Details.ewd_permanent_temporary = employeeWorkDetails.ewd_permanent_temporary ?? "";
            //    employeeDetails.tbl_Employee_Work_Details.ewd_designation = employeeWorkDetails.ewd_designation ?? 0;
            //    employeeDetails.tbl_Employee_Work_Details.ewd_group = employeeWorkDetails.ewd_group ?? "";
            //    employeeDetails.tbl_Employee_Work_Details.ewd_place_of_posting = employeeWorkDetails.ewd_place_of_posting ?? "";
            //}           
           
            return employeeDetails;
        }

        public long SaveHNBDeclarationBll(tbl_medical_declaration objDeclartaionDetails)
        {
            var result = _INBApplicationdll.SaveHNBDeclaration(objDeclartaionDetails);
            return result;
        }
        //public int SaveEmployeeBasicDetails(VM_EditEmployeeDetails employeeDetails)
        //{
        //    return _INBApplicationdll.SaveEmployeeBasicDetails(employeeDetails);
        //}

        public VM_DeptVerificationDetails GetPreviousVerificationDetails(long empId)
        {
            return _INBApplicationdll.GetPreviousVerificationDetails(empId);
        }

        
		public int SaveChallanBll(tbl_challan_details objChallan)
        {

            var x = _INBApplicationdll.ChallanDetailsDll(objChallan);
            return x;

        }

        //public VM_MedicalLeaveDetails MedicalLeaveDetailsBll(long empId)
        //{
        //    return _INBApplicationdll.MedicalLeaveDetailsDll(empId);
        //}

        //public int SaveNBMedicalLeaveBll(VM_MedicalLeaveDetails objMedicalLeaveDetails)
        //{
        //    return _INBApplicationdll.SaveMedicalLeaveDll(objMedicalLeaveDetails);
        //}

        public VM_DeptVerificationDetails GetMedicalLeaveDetails(long empId,long applicationId)
        {
            return _INBApplicationdll.GetMedicalLeaveDetails(empId, applicationId);
        }

        //public int DeleteNBFamilyBll(long employeeCode)
        //{
        //   return _INBApplicationdll.DeleteNBFamilyDll(employeeCode);
        //}
        public long DeleteNBNomineeBll(long employeeCode)
        {
            return _INBApplicationdll.DeleteNBNomineeDll(employeeCode);
        }
        //==============================================================================================================
        //New
        public long SaveNBBasicBll(VM_BasicDetails basicDetails)
        {
            long referanceNo = _INBApplicationdll.SaveNBBasic(basicDetails);
            return referanceNo;
        }

        public VM_BasicDetails BasicDetailsBll(long EmployeeCode)
        {
            var x = _INBApplicationdll.NewEmployeeBasicDetailsDll(EmployeeCode);
            return x;
        }
        public VM_PolicyDetails KGIDDetailsBll(long EmployeeCode)
        {
            var x = _INBApplicationdll.KGIDDetailsDll(EmployeeCode);
            return x;
        }
        public int SaveNBPolicyBll(VM_PolicyDetails objPolicy)
        {
            var result = _INBApplicationdll.SaveNBPolicy(objPolicy);
            return result;
        }

        public int SaveHPhysicalDetailsBll(VM_MPhysicalDetails objHPhysicalDetails)
        {
            var result = _INBApplicationdll.SaveHPhysicalDetails(objHPhysicalDetails);
            return result;
        }

        public VM_MPhysicalDetails HPhysicalDetailsBll(long EmployeeCode)
        {
            var x = _INBApplicationdll.HPhysicalDetailsDll(EmployeeCode);
            return x;
        }
        public int SaveHOtherDetailsBll(VM_MOtherDetails objHOtherDetails)
        {
            return _INBApplicationdll.SaveHOtherDetails(objHOtherDetails);
        }
        public VM_MOtherDetails HOtherDetailsBll(long EmployeeCode)
        {
            var x = _INBApplicationdll.HOtherDetailsDll(EmployeeCode);
            return x;
        }
        public int SaveHHealthDetailsBll(VM_MOtherDetails objHHealthDetails)
        {
            return _INBApplicationdll.SaveHHealthDetails(objHHealthDetails);
        }
        public VM_MOtherDetails HHealthDetailsBll(long EmployeeCode)
        {
            var x = _INBApplicationdll.HHealthDetailsDll(EmployeeCode);
            return x;
        }
        
   public long RestrictNomineeDetails(long EmpId)
        {
            var result = _INBApplicationdll.RestrictNomineeDetails(EmpId);
            return result;
        }
        public long SaveNBNomineeBll(VM_NomineeDetail objNominee)
        {
            var result = _INBApplicationdll.SaveNBNominee(objNominee);
            return result;
        }
		public VM_NomineeDetails NomineeDetailsBll(long EmployeeCode)
        {
            var nomineeDetails = _INBApplicationdll.NomineeDetailsDll(EmployeeCode);
            return nomineeDetails;
        }
        public int SaveNBFamilyBll(VM_FamilyDetails objFamilyDetails)
        {
            var result = _INBApplicationdll.SaveNBFamily(objFamilyDetails);
            return result;
        }
        public int CheckFamilyMemberDetailsBll(long FMID)
        {
            var result = _INBApplicationdll.CheckFamilyMemberDetailsDll(FMID);
            return result;
        }
        public VM_FamilyDetails FamilyDetailsBll(long EmployeeCode)
        {
            var familyDetails = _INBApplicationdll.FamilyDetailsDll(EmployeeCode);
            return familyDetails;
        }
        public int DeleteNBFamilyBll(long leavied)
        {
            return _INBApplicationdll.DeleteNBFamilyDll(leavied);
        }
        private VM_DoctorDetail GetPreviouslySavedDoctorDetails(long empId)
        {
            return _INBApplicationdll.GetPreviouslySavedDoctorDetails(empId);
        }

        public VM_DoctorDetails GetDoctorDetails(long empId)
        {
            VM_DoctorDetails docDetails = new VM_DoctorDetails();

            VM_DoctorDetail doctorDetail = GetPreviouslySavedDoctorDetails(empId);

            //var listOfKMCCodes = GetKMCCodesByEmployeeId(empId);
            docDetails.EmployeeId = empId;
            //docDetails.KMCCodes = listOfKMCCodes.Select(kmcCode => new SelectListItem()
            //{
            //    Text = kmcCode,
            //    Value = kmcCode
            //}).ToList();

            if (doctorDetail != null)
            {
                docDetails.Designation = doctorDetail.Designation;
                docDetails.KGIDOfDoctor = doctorDetail.KGIDOfDoctor;
                docDetails.NameOfHospital = doctorDetail.NameOfHospital;
                docDetails.NameOfDoctor = doctorDetail.DoctorName;
                docDetails.DoctorId = doctorDetail.DoctorId;
                docDetails.KMCCode = doctorDetail.KMCCode;
                docDetails.IMCCode = doctorDetail.IMCCode;
                docDetails.PlaceOfHospital = doctorDetail.PlaceOfHospital;
                docDetails.emdd_is_kmc_doctor = doctorDetail.emdd_is_kmc_doctor;
                if (!docDetails.emdd_is_kmc_doctor)
                {
                    docDetails.BankAccNumber = doctorDetail.BankAccNumber;
                    docDetails.IFSCCode = doctorDetail.IFSCCode;
                    docDetails.MICRCode = doctorDetail.MICRCode;
                }
            }

            return docDetails;
        }
		public string SaveEmployeeBasicDetails(VM_BasicDetails employeeDetails)
        {
            return _INBApplicationdll.SaveEmployeeBasicDetails(employeeDetails);
        }
        public string AddEmployeeBasicDetails(VM_BasicDetails employeeDetails)
        {
            return _INBApplicationdll.AddEmployeeBasicDetails(employeeDetails);
        }
        public VM_PersonalHealthDetails PersonalDetailsBll(long EmployeeCode)
        {
            var x = _INBApplicationdll.PersonalDetailsDll(EmployeeCode);
            return x;

        }
        public int SaveNBPersonalBll(VM_PersonalHealthDetails objPersonalDetails)
        {
            var result = _INBApplicationdll.SaveNBPersonal(objPersonalDetails);
            return result;
        }
        public VM_MedicalLeaveDetails MedicalLeaveDetailsBll(long empId, string loginType)
        {
            return _INBApplicationdll.MedicalLeaveDetailsDll(empId, loginType);
        }
        public VM_MedicalLeaveDetails SaveNBMedicalLeaveBll(VM_MedicalLeaveDetails objMedicalLeaveDetails)
        {
            return _INBApplicationdll.SaveMedicalLeaveDll(objMedicalLeaveDetails);
        }
        public string UploadMedicalLeaveDoc(MedicalLeaveData objMedicalLeaveDetails)
        {
            return _INBApplicationdll.UploadMedicalLeaveDoc(objMedicalLeaveDetails);
        }
        public int DeleteMedicalLeaveBll(long EmpCode)
        {
            return _INBApplicationdll.DeleteMedicalLeaveDll(EmpCode);
        }
        public VM_DDOVerificationDetails GetEmployeeDetailsForDDOVerification(long empId)
        {
            return _INBApplicationdll.GetEmployeeDetailsForDDOVerification(empId);
        }

        public VM_VerificationDetails GetEmployeeDetailsForCWVerification(long empId)
        {
            return _INBApplicationdll.GetEmployeeDetailsForCWVerification(empId);
        }

        public VM_VerificationDetails GetEmployeeDetailsForSIVerification(long empId)
        {
            return _INBApplicationdll.GetEmployeeDetailsForSIVerification(empId);
        }

        public VM_VerificationDetails GetEmployeeDetailsForDIOVerification(long empId)
        {
            return _INBApplicationdll.GetEmployeeDetailsForDIOVerification(empId);
        }

        public VM_VerificationDetails GetEmployeeDetailsForDDVerification(long empId)
        {
            return _INBApplicationdll.GetEmployeeDetailsForDDVerification(empId);
        }

        public VM_VerificationDetails GetEmployeeDetailsForDVerification(long empId)
        {
            return _INBApplicationdll.GetEmployeeDetailsForDVerification(empId);
        }
        public IList<VM_WorkflowDetail> GetWorkFlowDetails(long applicationId)
        {
            //IList<VM_WorkflowDetail> workflowDetails = new List<VM_WorkflowDetail>();

            return _INBApplicationdll.GetWorkFlowDetails(applicationId);
        }

        public VM_DeptVerificationDetails GetPolicyCalculations(long empId, long applicationId)
        {
            return _INBApplicationdll.GetPolicyCalculations(empId, applicationId);
        }

        public VM_ApplicationDetail GenerateApplicationNumber(long empId)
        {
            return _INBApplicationdll.GenerateApplicationNumber(empId);
        }

        public VM_VerificationDetails GetEmployeeDetailsForAVGCWVerification(long empId)
        {
            return _INBApplicationdll.GetEmployeeDetailsForAVGCWVerification(empId);
        }
        public int SaveNBDdoUploadStatusBll(long empid)
        {
            var result = _INBApplicationdll.SaveNBDdoUploadStatusBll(empid);
            return result;
        }
        public BindDropDownModel GetNomineeNameListBll(long empId ,string type)
        {
            var result = _INBApplicationdll.GetNomineeNameListBll(empId ,type);
            return result;
        }
        public long SaveNBPaymentBll(VM_PaymentDetails objPaymentDetails) {
            var result = _INBApplicationdll.SaveNBPaymentDll(objPaymentDetails);
            return result;
        }
        public long SaveNBChallanStatusDll(VM_PaymentDetails objPaymentDetails)
        {
            var result = _INBApplicationdll.SaveNBChallanStatusDll(objPaymentDetails);
            return result;
        }
        public VM_PaymentDetails NBPaymentDll(long EmpID)
        {
            var result = _INBApplicationdll.NBPaymentDll(EmpID);
            return result;
        }
        public VM_PaymentDetails NBChallanDetailsDll(long EmpID)
        {
            var result = _INBApplicationdll.NBChallanDetailsDll(EmpID);
            return result;
        }
        public VM_PaymentDetails NBPaymentDownloadDll(long EmpID)
        {
            var result = _INBApplicationdll.NBPaymentDownloadDll(EmpID);
            return result;
        }

        public IEnumerable<SelectListItem> GetFamilyRelationList(long EmpId)
        {
            var result = _INBApplicationdll.GetFamilyRelationList(EmpId);
            return result;
        }
		public string DSCLogin(long kgidno, string publickey)
        {
            return _INBApplicationdll.DSCLogin(kgidno, publickey);
        }
        public int NBApplicationCancel(long AppId, long EmpId,string Comments)
        {
            return _INBApplicationdll.NBApplicationCancel(AppId, EmpId, Comments);
        }
        public int SaveDDOMedicalLeaveBll(MedicalLeaveData objMedicalLeaveDetails)
        {
            return _INBApplicationdll.SaveDDOMedicalLeaveDll(objMedicalLeaveDetails);
        }
        public string GetNomineeList(long AppID,string IsMarried)
        {
            return _INBApplicationdll.GetNomineeList(AppID,IsMarried);
        }
        public string CheckEmployeeAge(long EmployeeID)
        {
            return _INBApplicationdll.CheckEmployeeAge(EmployeeID);
        }
        public string SavePolicyGeneration(VM_PolicyGeneration objPG)
        {
            return _INBApplicationdll.SavePolicyGeneration(objPG);
        }
        public string UpdateNBChallanStatusDll(string ReqChallanRefNo,long StatusCode, long EmpID)
        {
            return _INBApplicationdll.UpdateNBChallanStatusDll(ReqChallanRefNo, StatusCode,EmpID);
        }
        public List<VM_EmpDashboardData> GetDetailsBasedOnKGIDNo(long KgidNo)
        {
            return _INBApplicationdll.GetDetailsBasedOnKGIDNo(KgidNo);
        }
        public string GetEmailSMSTemplate(long templateid)
        {
            return _INBApplicationdll.GetEmailSMSTemplate(templateid);
        }
        public string GetUserDetails(long KGIDNumber)
        {
            return _INBApplicationdll.GetUserDetails(KGIDNumber);
        }
        public string GetUserDetailsbasedOnMobNum(long MobileNumber)
        {
            return _INBApplicationdll.GetUserDetailsbasedOnMobNum(MobileNumber);
        }
        public List<VM_ApplicationDetail> GetpaymentapplicationDetails(string KGIDNumber)
        {
            return _INBApplicationdll.GetpaymentapplicationDetails(KGIDNumber);
        }    

        public  string UpdatePaymentVerification(long paymentstatus, long challanId)
        {
            return _INBApplicationdll.UpdatePaymentVerification(paymentstatus, challanId);
        }
        public string DeleteForPaymentVerification(long cd_challan_id, string cd_challan_ref_no, long applicationId, string ChallanStatusRowCount)
        {
            return _INBApplicationdll.DeleteForPaymentVerification(cd_challan_id, cd_challan_ref_no, applicationId, ChallanStatusRowCount);
        }
        public string UpdateUserDetails(long KGIDNumber, long MobileNumber, string EMailID,long EmployeeId)
        {
            return _INBApplicationdll.UpdateUserDetails(KGIDNumber, MobileNumber, EMailID, EmployeeId);
        }       
        public List<VM_PolicyGeneration> GetPolicyList(long EmpID)
        {
            return _INBApplicationdll.GetPolicyList(EmpID);
        }
        public int SavePolicyStatus(long ID)
        {
            return _INBApplicationdll.SavePolicyStatus(ID);
        }
        public string GetDIOOfficeDetails(string DDOCode, string KGIDNo)
        {
            return _INBApplicationdll.GetDIOOfficeDetails(DDOCode, KGIDNo);
        }
        public string GetDDODetails(long KGIDNumber)
        {
            return _INBApplicationdll.GetDDODetails(KGIDNumber);
        }
        public string CheckDDODetails(long KGIDNumber, string DDOCode, string DIOOffice)
        {
            return _INBApplicationdll.CheckDDODetails(KGIDNumber, DDOCode, DIOOffice);
        }
        public string UpdateDDODetails(long KGIDNumber, string DDOCode, string DIOOffice)
        {
            return _INBApplicationdll.UpdateDDODetails(KGIDNumber, DDOCode, DIOOffice);
        }
 		public List<VM_trackDetails> getProposerTrackDetailsBll(string applicationNo)
        {
            return _INBApplicationdll.getProposerTrackDetailsDll(applicationNo);
        }
        //
        public async Task<bool> UpdateUserDetails1(long KGIDNumber, long MobileNumber, string EMailID, long EmployeeId)
        {
            return await _INBApplicationdll.UpdateUserDetails1(KGIDNumber, MobileNumber, EMailID, EmployeeId);
        }


        public async Task<bool> IsMobNoAlreadyExistsAsync(long KGIDNumber, long MobileNumber, long EmployeeId, string EMailID)
        {
            return await _INBApplicationdll.IsMobNoAlreadyExistsAsync(KGIDNumber, MobileNumber, EmployeeId, EMailID);
        }

        public VM_DeptVerificationDetails GetUploadedDocumentBll(long empId, long applicationId)
        {
            return _INBApplicationdll.GetUploadedDocuments(empId, applicationId);
        }
    }
}
