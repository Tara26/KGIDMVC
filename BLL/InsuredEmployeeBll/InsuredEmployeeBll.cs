using DLL.InsuredEmployeeDll;
using KGID_Models.KGIDEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InsuredEmployeeBll
{
    public class InsuredEmployeeBll : IInsuredEmployeeBll
    {
        private readonly IInsuredEmployeeDll _IInsuredEmployeedll;
        public InsuredEmployeeBll()
        {
            this._IInsuredEmployeedll = new InsuredEmployeeDll();
        }

        public tbl_insured_details_new ViewInsuredEmployeebll(long kgidNo)
        {
            return _IInsuredEmployeedll.ViewInsuredEmployee(kgidNo);
        }
    }
}
