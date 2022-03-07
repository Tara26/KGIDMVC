using KGID_Models.KGIDEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InsuredEmployeeBll
{
    public interface IInsuredEmployeeBll
    {
        tbl_insured_details_new ViewInsuredEmployeebll(long kgidNo);
    }
}
