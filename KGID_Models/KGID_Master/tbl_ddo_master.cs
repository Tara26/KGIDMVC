using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_User
{
    public class tbl_ddo_master
    {
        [Key]
        public int dm_ddo_id { get; set; }
        public string dm_ddo_code { get; set; }
        public string dm_ddo_office { get; set; }
        public string dm_dept_code { get; set; }
        public Nullable<int> dm_district_id { get; set; }
        public Nullable<int> dm_taluka_id { get; set; }
        public Nullable<bool> dm_active { get; set; }
        public Nullable<System.DateTime> dm_creation_datetime { get; set; }
        public Nullable<System.DateTime> dm_updation_datetime { get; set; }
        public Nullable<int> dm_created_by { get; set; }
        public Nullable<int> dm_updated_by { get; set; }
        public Nullable<int> dm_dept_id { get; set; }


        public string dm_hod_desc { get; set; }
        public string dm_designation { get; set; }
        public string dm_address { get; set; }
        public Nullable<int> dm_pincode { get; set; }
        public string dm_telephone_no { get; set; }
        public string dm_fax_no { get; set; }
        public string dm_email { get; set; }

        public string dm_occupation { get; set; }
       

    }
}
