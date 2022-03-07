using KGID_Models.KGID_User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class VM_NBApplication
    {
        public tbl_nb_family_details GetFamilyDetails { get; set; }
        public tbl_nb_nominee_details GetNomineeDetails { get; set; }
        public tbl_user_master GetUserDetails { get; set; }
        public tbl_payscale_details GetPayscaleDetails { get; set; }
    }
}
