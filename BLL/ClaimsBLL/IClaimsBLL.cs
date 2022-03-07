using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using KGID_Models.Claim;

namespace BLL.ClaimsBLL
{
    public interface IClaimsBLL
    {
        VM_ClaimEmployeeDetail GetMaturityClaimEmployeeDetails(long empId);
        VM_ClaimEmployeeDetail GetPreMaturityClaimEmployeeDetails(long empId);
        string SaveVerifiedDetailsBll(VM_ClaimEmployeeDetail objVerification);
        IList<VM_ClaimWorkflowDetail> GetClaimWorkFlowDetails(long applicationId);
        VM_EmpDetailForPrematureClaimApplication GetEmployeeDetailByKGIDNumber(string kgidNumber);
        string ForwardApplicationToCaseworker(VM_EmpDetailForPrematureClaimApplication empDetail);
        IList<SelectListItem> GetClaimSubTypes(int claimType);
        bool InitiateDeathClaimApplication(VM_ClaimRequiredDocuments docNames);
        VM_ClaimEmployeeDetail GetDeathClaimEmployeeDetails(long empId);
        VM_ClaimApplications GetClaimApplications(int empType, int claimType, long loggedInUserId);
        string SaveApplicationDetails(VM_EmpDetailForDeathClaimApplication empDetail);
        VM_ClaimRequiredDocuments GetAdditionalDocumentsList(long empId);
    }
}
