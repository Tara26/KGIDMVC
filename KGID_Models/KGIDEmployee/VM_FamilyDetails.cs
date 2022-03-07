using KGID_Models.KGIDNBApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class VM_FamilyDetails
    {
        public long EmployeeCode { get; set; }
        public IEnumerable<tbl_family_details> GetFamilyDetails { get; set; }
        public int NoOfBrother { get; set; }
        public int NoOfSister { get; set; }
        public int NoOfChildren { get; set; }
    }
}
