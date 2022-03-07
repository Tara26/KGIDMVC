using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_payment_purpose_master
    {
        public long ppm_purpose_id { get; set; }
        public string ppm_purpose_desc { get; set; }
        public string ppm_head_of_account { get; set; }
        public byte ppm_status { get; set; }
        public Nullable<System.DateTime> cd_creation_datetime { get; set; }
        public Nullable<System.DateTime> cd_updation_datetime { get; set; }
        public Nullable<int> cd_created_by { get; set; }
        public Nullable<int> cd_updated_by { get; set; }


    }
}
