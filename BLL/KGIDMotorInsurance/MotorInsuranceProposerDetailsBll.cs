
using DLL;
using DLL.KGIDMotorInsurance;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.KGIDMotorInsurance
{
  public class MotorInsuranceProposerDetailsBll :IMotorInsuranceProposerDetailsBll
    {
        private readonly IMotorInsuranceProposerDetailsDll _IMotorInsuranceProposerDetailsDll;
        public MotorInsuranceProposerDetailsBll()
        {
            this._IMotorInsuranceProposerDetailsDll = new MotorInsuranceProposerDetailsDll();
        }
        public VM_MotorInsuranceProposerDetails MIProposerDetailsBll(long EmployeeCode,string PageType,long RefNo, int Category)
        {
          return _IMotorInsuranceProposerDetailsDll.MIProposerDetailsBll(EmployeeCode, PageType, RefNo,Category);
          
        }
        public long SaveMIProposalAppnRefNo(VM_MotorInsuranceProposerDetails objPD)
        {
            return _IMotorInsuranceProposerDetailsDll.SaveMIProposalAppnRefNo(objPD);
        }

        public long SaveMIRenewalProposalAppnRefNo(VM_MotorInsuranceProposerDetails objPD)
        {
            return _IMotorInsuranceProposerDetailsDll.SaveMIRenewalProposalAppnRefNo(objPD);
        }

        public string SaveMILoanDetails(VM_MIApplicationDetails objMILoanDetails)
        {
            var result = _IMotorInsuranceProposerDetailsDll.SaveMILoanDetails(objMILoanDetails);
            return result;
        }
        //DSC Validation
        public string DSCLoginDetails(long kgidno, string publickey)
        {
            return _IMotorInsuranceProposerDetailsDll.DSCLoginDetails(kgidno, publickey);
        }
    }
}
