using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.KGIDPolicyDLL;
using KGID_Models.KGID_Policy;

namespace BLL.KGIDPolicyBLL
{
    public class PolicyBLL : IPolicyBLL
    {
        private readonly IPolicyDLL policy;

        public PolicyBLL()
        {
            policy = new PolicyDLL();
        }

        public VM_FacingSheet GetFacingSheetDetails(long applicationReferenceNumber,long employeeId)
        {
            return policy.GetFacingSheetDetails(applicationReferenceNumber, employeeId);
        }
        public VM_InitimationLetter GetIntimationLetter(long applicationId)
        {
            return policy.GetIntimationLetter(applicationId);
        }
        //NB BOND and Facing Sheet Doc Upload
        public string NBBondFacingDocUploadBLL(long AppId, long EmpId, string DocPath, string DocType)
        {
            var result = policy.NBBondFacingDocUploadDLL(AppId,EmpId,DocPath,DocType);
            return result;
        }
        public string GetNBBondDocFileBLL(long AppId, long EmpId)
        {
            var result = policy.GetNBBondDocFileDLL(AppId, EmpId);
            return result;
        }
        public string NBSignBondUploadBLL(long AppId, long EmpId, string DocPath)
        {
            var result = policy.NBSignBondUploadDLL(AppId, EmpId, DocPath);
            return result;
        }
    }
}
