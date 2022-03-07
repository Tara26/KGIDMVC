using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDMotorInsurance
{
    public interface IMotorInsuranceRenewalDetailsBll
    {
        VM_MotorInsuranceRenewalDetails MIRenwalDetailsBll(long EmployeeCode, int Category);
       
    }
}
