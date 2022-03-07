using DLL.KGIDMotorInsurance;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDMotorInsurance
{
   public class MotorInsuranceRenewalDetailsBll:IMotorInsuranceRenewalDetailsBll
    {
        private readonly IMotorInsuranceRenewalDetailsDll _IMotorInsuranceRenewalDetailsDll;
        public MotorInsuranceRenewalDetailsBll()
        {
            this._IMotorInsuranceRenewalDetailsDll = new MotorInsuranceRenewalDetailsDll();
        }
        public VM_MotorInsuranceRenewalDetails MIRenwalDetailsBll(long EmployeeCode, int Category)
        {
            return _IMotorInsuranceRenewalDetailsDll.MIRenwalDetailsDll(EmployeeCode, Category);

        }
       
    }
}
