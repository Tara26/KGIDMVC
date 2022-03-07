using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.Claim;

namespace DLL.ClaimsDLL
{
    public interface IClaimsDLL
    {
        VM_ClaimEmployeeDetail GetMaturityClaimEmployeeDetails(long empId);
        VM_ClaimApplications GetMaturityClaimApplications(int empType);
        VM_ClaimEmployeeDetail GetPreMaturityClaimEmployeeDetails(long empId);
        VM_ClaimApplications GetPrematurityClaimApplications(int claimTypeId);
        string SaveVerifiedDetailsDll(VM_ClaimEmployeeDetail objVerification);
        IList<VM_ClaimWorkflowDetail> GetClaimWorkFlowDetails(long applicationId);
        VM_EmpDetailForPrematureClaimApplication GetEmployeeDetailByKGIDNumber(string kgidNumber);
        string ForwardApplicationToCaseworker(VM_EmpDetailForPrematureClaimApplication empDetail);
        VM_ClaimSubTypes GetClaimSubTypes(int claimType);
        bool InitiateDeathClaimApplication(VM_ClaimDocumentsPathNames docNames);

        VM_ClaimEmployeeDetail GetDeathClaimEmployeeDetails(long empId);
        VM_ClaimApplications GetClaimApplications(int empType, int claimType, long loggedInUserId);
        string SaveApplicationDetails(VM_EmpDetailForDeathClaimApplication empDetail);
        VM_ClaimRequiredDocuments GetAdditionalDocumentsList(long empId);
    }
}
