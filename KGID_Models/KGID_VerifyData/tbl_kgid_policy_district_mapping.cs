using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGID_VerifyData
{
    
    public class tbl_kgid_policy_district_mapping
    {
        [Key]
        public string policy_no { get; set; }
        public string district_name { get; set; }
        public Nullable<bool> status { get; set; }
        public Nullable<System.DateTime> creation_datetime { get; set; }
        public Nullable<System.DateTime> updation_datetime { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<int> district_id { get; set; }
    }
}
