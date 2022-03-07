using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_nb_nominee_details
    {
        [Key]
        public int nnd_id { get; set; }
        public long nnd_sys_emp_code { get; set; }
        public string nnd_nominee_name { get; set; }
        public bool nnd_IsMinor { get; set; }
        public string nnd_current_address1 { get; set; }
        public string nnd_current_address2 { get; set; }
        public string nnd_current_address3 { get; set; }
        public string nnd_taluka { get; set; }
        public string nnd_district { get; set; }
        public Nullable<int> nnd_pincode { get; set; }
        public Nullable<System.DateTime> nnd_date_of_birth { get; set; }
        public string nnd_relation { get; set; }
        public Nullable<int> nnd_percentage_share { get; set; }
        public Nullable<bool> nnd_active { get; set; }
        public Nullable<System.DateTime> nnd_creation_datetime { get; set; }
        public Nullable<System.DateTime> nnd_updation_datetime { get; set; }
        public Nullable<int> nnd_created_by { get; set; }
        public Nullable<int> nnd_updated_by { get; set; }
        public Nullable<int> nnd_age { get; set; }
        public Nullable<int> nnd_guardian_age { get; set; }
        public string nnd_guardian_name { get; set; }

        //public tbl_nb_nominee_details NomineeDetails { get; set; }
    }
}
