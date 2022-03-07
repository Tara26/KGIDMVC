using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_nominee_number_details
    {
        [Key]
        public int nnd_id { get; set; }
        public Nullable<long> nnd_sys_emp_code { get; set; }
        public Nullable<int> nnd_no_of_brothers { get; set; }
        public Nullable<int> nnd_no_of_sisters { get; set; }
        public Nullable<int> nnd_no_of_children { get; set; }
        public Nullable<bool> nnd_active { get; set; }
        public Nullable<DateTime> nnd_creation_datetime { get; set; }
        public Nullable<DateTime> nnd_updation_datetime { get; set; }
        public Nullable<int> nnd_created_by { get; set; }
        public Nullable<int> nnd_updated_by { get; set; }
    }
}
