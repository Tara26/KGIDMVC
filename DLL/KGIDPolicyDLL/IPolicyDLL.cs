using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.KGID_Policy;

namespace DLL.KGIDPolicyDLL
{
    public interface IPolicyDLL
    {
        VM_FacingSheet GetFacingSheetDetails(long applicationReferenceNumber,long employeeId);
        VM_InitimationLetter GetIntimationLetter(long applicationId);
        string NBBondFacingDocUploadDLL(long AppId, long EmpId, string DocPath, string DocType);
        string GetNBBondDocFileDLL(long AppId, long EmpId);
        string NBSignBondUploadDLL(long AppId, long EmpId, string DocPath);
    }
}
