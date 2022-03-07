using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class NomineeList
    {
        public long nd_sys_emp_code { get; set; }
        public List<tbl_nominee_details> NomineeDetailsList { get; set; }
    }
}
