using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_VerifyData
{
    public class tbl_district_master
    {
        [Key]
        public int dm_id { get; set; }
        public string dm_code { get; set; }
        public string dm_name_english { get; set; }
        public string dm_name_kannada { get; set; }
        public Nullable<bool> dm_status { get; set; }
        public Nullable<System.DateTime> dm_creation_datetime { get; set; }
        public Nullable<System.DateTime> dm_updation_datetime { get; set; }
        public Nullable<int> dm_created_by { get; set; }
        public Nullable<int> dm_updated_by { get; set; }
    }
}
