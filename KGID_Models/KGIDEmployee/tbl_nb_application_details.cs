using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_nb_application_details
    {
        [Key]
        public int nad_id { get; set; }
        public long nad_sys_emp_code { get; set; }
        public string nad_current_address1 { get; set; }
        public string nad_current_address2 { get; set; }
        public string nad_taluka { get; set; }
        public string nad_district { get; set; }
        public Nullable<int> nad_pincode { get; set; }
        public string nad_marital_status { get; set; }
        public string nad_spouse_name { get; set; }
        public Nullable<int> nad_no_of_nominees { get; set; }
        public string nad_guardian_name { get; set; }
        public string nad_guardian_address1 { get; set; }
        public string nad_guardian_address2 { get; set; }
        public string nad_guardian_address3 { get; set; }
        public string nad_guardian_taluka { get; set; }
        public string nad_guardian_district { get; set; }
        public Nullable<int> nad_guardian_pincode { get; set; }
        public Nullable<int> nad_no_of_borthers { get; set; }
        public Nullable<int> nad_no_of_sisters { get; set; }
        public Nullable<int> nad_no_of_children { get; set; }
        public Nullable<bool> nad_active { get; set; }
        public Nullable<System.DateTime> nad_creation_datetime { get; set; }
        public Nullable<System.DateTime> nad_updation_datetime { get; set; }
        public Nullable<int> nad_created_by { get; set; }
        public Nullable<int> nad_updated_by { get; set; }
    }
}
