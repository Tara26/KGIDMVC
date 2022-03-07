using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.KGIDMotorInsurance
{
    public interface IMotorInsuranceProposerDetailsBll
    {
        VM_MotorInsuranceProposerDetails MIProposerDetailsBll(long EmployeeCode,string PageType,long RefNo,int Category);

        long SaveMIProposalAppnRefNo(VM_MotorInsuranceProposerDetails objPD);

        long SaveMIRenewalProposalAppnRefNo(VM_MotorInsuranceProposerDetails objPD);

        string SaveMILoanDetails(VM_MIApplicationDetails objMILoanDetails);
        
        //DSC Validation
        string DSCLoginDetails(long empid, string publickey);
    }
}
