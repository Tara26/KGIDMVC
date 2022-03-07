using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDMotorInsurance
{
    public interface IMotorInsuranceRenewalDetailsDll
    {
        VM_MotorInsuranceRenewalDetails MIRenwalDetailsDll(long EmployeeCode, int Category);
    }
}
