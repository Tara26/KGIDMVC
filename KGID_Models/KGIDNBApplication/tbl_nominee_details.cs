using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_nominee_details
    {
        [Key]
        public int nd_id { get; set; }
        public Nullable<long> nd_sys_emp_code { get; set; }
        public string nd_marital_status { get; set; }
        public string nd_nominee_name { get; set; }
        public string nd_relation { get; set; }
        public Nullable<int> nd_age { get; set; }
        public Nullable<int> nd_nominee_share { get; set; }
        public string nd_minor { get; set; }
        public string nd_name_of_guardian { get; set; }
        public string nd_relation_with_guardian { get; set; }
        public string nd_guardian_address { get; set; }
        public Nullable<int> nd_guardian_pincode { get; set; }
        public Nullable<bool> nd_active { get; set; }
        public Nullable<System.DateTime> nd_creation_datetime { get; set; }
        public Nullable<System.DateTime> nd_updation_datetime { get; set; }
        public Nullable<int> nd_created_by { get; set; }
        public Nullable<int> nd_updated_by { get; set; }
        public Nullable<bool> nd_is_sibling_married { get; set; }
    }
    
}
