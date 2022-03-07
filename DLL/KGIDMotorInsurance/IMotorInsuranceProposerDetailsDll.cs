using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDMotorInsurance
{
    public interface IMotorInsuranceProposerDetailsDll
    {
        VM_MotorInsuranceProposerDetails MIProposerDetailsBll(long employeeCode,string PageType, long RefNo, int Category);

        long SaveMIProposalAppnRefNo(VM_MotorInsuranceProposerDetails objPD);

        long SaveMIRenewalProposalAppnRefNo(VM_MotorInsuranceProposerDetails objPD);

        string SaveMILoanDetails(VM_MIApplicationDetails objMILoanDetails);
        
        //DSC Validation
        string DSCLoginDetails(long empid, string publickey);
    }
}
