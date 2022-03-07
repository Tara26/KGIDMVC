using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.KGID_Policy;

namespace BLL.KGIDPolicyBLL
{
    public interface IPolicyBLL
    {
        VM_FacingSheet GetFacingSheetDetails(long applicationReferenceNumber,long employeeId);
        VM_InitimationLetter GetIntimationLetter(long applicationId);
        string NBBondFacingDocUploadBLL(long AppId,long EmpId,string DocPath,string DocType);
        string GetNBBondDocFileBLL(long AppId, long EmpId);
        string NBSignBondUploadBLL(long AppId, long EmpId, string DocPath);
    }
}
