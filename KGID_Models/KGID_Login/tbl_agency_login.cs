using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Login
{
   public class tbl_agency_login
    {
        [Key]
        public long al_agency_login_id { get; set; }
        public string al_agency_user_id { get; set; }
        public string al_password { get; set; }
        public long al_agency_ddo_id { get; set; }
        public bool al_status { get; set; }
        public string al_user_category_id { get; set; }
        public DateTime al_creation_datetime { get; set; }
        public int al_created_by { get; set; }


    }
}
