using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using System.Collections.Generic;

namespace BLL.VerifyDataBLL
{
    public interface IVerificationdetailsbll
    {
        IEnumerable<tbl_verification_details> UserDetailsbll(tbl_verification_details _Userdetails);

        tbl_verification_details EmployeeDetailsbll(int KGIDNO);

        tbl_verification_details UpdateStatusbll(string Status, VM_VerifiedBy _VBY, int UID);

        tbl_verified_details AddVerifiedDetailsbll(tbl_verified_details _Verified_Details);

        IEnumerable<tbl_verification_details> getVerificationData(int _VerificationStatus,string PolicyNo, int statename);
        //IEnumerable<tbl_verification_details> getVerificationData(int _VerificationStatus,int _startNo,int _endNo);

        //tbl_verification_details GetVerificationDetailById(int Vd_Id);
        VM_VerifiedBy GetVerificationDetailById(int Vd_Id);

        int SaveEmployeeFormBll(tbl_upload_employeeform objEmpForm);
        VM_Upload_EmployeeForm GetUploadDocBll(long _EmpId);

        VM_DDOVerificationDetails GetEmployeeApplicationStatusBll(long empId);
        VM_NBBond getNBBondDetails(long PolicyNo);

        VM_DDOVerificationDetails GetEmployeeNBBondFacingSheetBll(long empId);
        VM_DDOVerificationDetails GetEmployeeIntimationLetterBll(long empId);
        VM_PolicyCancellationDetails GetPolicyCancellationDetails(long EmpId,string Type);
        int NBAppCancelRequestAction(long AppId, long EmpId, string action);
    }
}
