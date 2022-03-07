using KGID_Models.KGIDEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.InsuredEmployeeDll
{
    public interface IInsuredEmployeeDll
    {
        tbl_insured_details_new ViewInsuredEmployee(long KGIDNO);
    }
}
