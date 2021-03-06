using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_challan_details
    {
        [Key]        
        public long cd_id { get; set; }
        public long cd_challan_id { get; set; }
        public long cd_application_id { get; set; }
        public string cd_challan_ref_no { get; set; }
        public string cd_dept_code { get; set; }
        public string cd_referance_number { get; set; }
        public string cd_ddo_code { get; set; }      
        public string cd_purpose_code { get; set; }
        public string cd_subpurpose_code { get; set; }
        public long? cd_system_emp_code { get; set; }
        public string cd_head_of_account { get; set; }        
        public Nullable<int> cd_amount { get; set; }
        public int cd_active_status { get; set; }
        
        [ConcurrencyCheck]
        public Nullable<System.DateTime> cd_datetime_of_challan { get; set; }
        public Nullable<byte> cd_status { get; set; }
        public Nullable<System.DateTime> cd_creation_datetime { get; set; }
        public Nullable<System.DateTime> cd_updation_datetime { get; set; }
        public Nullable<int> cd_created_by { get; set; }
        public Nullable<int> cd_updated_by { get; set; }

    }
}
