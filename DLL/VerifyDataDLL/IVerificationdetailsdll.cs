using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using System.Collections.Generic;

namespace DLL.VerifyDataDLL
{
    public interface IVerificationdetailsdll
    {
        IEnumerable<tbl_verification_details> UserDetailsdll(tbl_verification_details _users);
        tbl_verification_details EmployeeDetailsdll(int KGIDNO);
        tbl_verification_details UpdateStatusdll(string Status, VM_VerifiedBy _VBY, int UID);

        tbl_verified_details AddVerifiedDetailsbll(tbl_verified_details _Verified_Details);
        IEnumerable<tbl_verification_details> getVerificationDatadll(int _VerificationStatus,string policyNo, int districtId);

        //IEnumerable<tbl_verification_details> getVerificationDatadll(int _VerificationStatus,int _startNo,int _endNo);

        //tbl_verification_details GetVerificationDetailById(int _vdId);
        VM_VerifiedBy GetVerificationDetailById(int _vdId);

        int SaveEmployeeFormDll(tbl_upload_employeeform objEmpForm);
        VM_Upload_EmployeeForm GetUploadDocDll(long _EmpId);


        VM_NBBond getNBBondDetailsdll(long policyNo);


        VM_DDOVerificationDetails GetEmployeeApplicationStatusDll(long empId);
        //For DIO
        VM_DDOVerificationDetails GetEmployeeNBBondFacingSheetDll(long empId);
        //For DDO
        VM_DDOVerificationDetails GetEmployeeIntimationLetterDll(long empId);
        VM_PolicyCancellationDetails GetPolicyCancellationDetails(long EmpId, string Type);
        int NBAppCancelRequestAction(long AppId, long EmpId, string action);
    }
}
