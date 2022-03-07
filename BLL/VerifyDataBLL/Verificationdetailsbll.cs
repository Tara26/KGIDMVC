using System.Collections.Generic;
using DLL.VerifyDataDLL;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;

namespace BLL.VerifyDataBLL
{
    public class Verificationdetailsbll : IVerificationdetailsbll
    {
        private readonly IVerificationdetailsdll _IVerificationdetailsdll;
        public Verificationdetailsbll()
        {
            this._IVerificationdetailsdll = new Verificationdetailsdll();
        }
        public IEnumerable<tbl_verification_details> UserDetailsbll(tbl_verification_details _Userdetails)
        {
            var result = _IVerificationdetailsdll.UserDetailsdll(_Userdetails);
            return result;
        }
        public tbl_verification_details EmployeeDetailsbll(int KGIDNO)
        {
            var result = _IVerificationdetailsdll.EmployeeDetailsdll(KGIDNO);
            return result;
        }
        public tbl_verification_details UpdateStatusbll(string Status, VM_VerifiedBy _VBY, int UID)
        {
            var result = _IVerificationdetailsdll.UpdateStatusdll(Status, _VBY, UID);
            return result;
        }
        public tbl_verified_details AddVerifiedDetailsbll(tbl_verified_details _Verified_Details)
        {
            var result = _IVerificationdetailsdll.AddVerifiedDetailsbll(_Verified_Details);
            return result;
        }

        public IEnumerable<tbl_verification_details> getVerificationData(int _VerificationStatus, string policyNo, int districtId)
        {
            var result = _IVerificationdetailsdll.getVerificationDatadll(_VerificationStatus, policyNo, districtId);
            return result;
        }
        //public IEnumerable<tbl_verification_details> getVerificationData(int _VerificationStatus,int _startNo,int _endNo)
        //{
        //    var result = _IVerificationdetailsdll.getVerificationDatadll(_VerificationStatus, _startNo, _endNo);
        //    return result;
        //}
        public VM_VerifiedBy GetVerificationDetailById(int _vdId)
        {
            var result = _IVerificationdetailsdll.GetVerificationDetailById(_vdId);
            return result;
        }


        public int SaveEmployeeFormBll(tbl_upload_employeeform objEmpForm)
        {
            var result = _IVerificationdetailsdll.SaveEmployeeFormDll(objEmpForm);
            return result;
        }
        public VM_Upload_EmployeeForm GetUploadDocBll(long _EmpId)
        {
            var result = _IVerificationdetailsdll.GetUploadDocDll(_EmpId);
            return result;
        }

        public VM_DDOVerificationDetails GetEmployeeApplicationStatusBll(long empId)
        {
            return _IVerificationdetailsdll.GetEmployeeApplicationStatusDll(empId);
        }
        public VM_NBBond getNBBondDetails(long policyNo)
        {
            var result = _IVerificationdetailsdll.getNBBondDetailsdll(policyNo);
            return result;
        }
        
        public VM_DDOVerificationDetails GetEmployeeNBBondFacingSheetBll(long empId)
        {
            return _IVerificationdetailsdll.GetEmployeeNBBondFacingSheetDll(empId);
        }
        public VM_DDOVerificationDetails GetEmployeeIntimationLetterBll(long empId)
        {
            return _IVerificationdetailsdll.GetEmployeeIntimationLetterDll(empId);
        }
        public VM_PolicyCancellationDetails GetPolicyCancellationDetails(long EmpId, string Type)
        {
            return _IVerificationdetailsdll.GetPolicyCancellationDetails(EmpId,Type);
        }
        public int NBAppCancelRequestAction(long AppId, long EmpId, string action)
        {
            return _IVerificationdetailsdll.NBAppCancelRequestAction(AppId, EmpId, action);
        }
    }
}
