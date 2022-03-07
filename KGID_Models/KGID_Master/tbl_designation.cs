using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_designation
    {
        [Key]
        public int d_id { get; set; }
        public string d_designation_name { get; set; }
        public Nullable<int> d_status { get; set; }
        public Nullable<System.DateTime> d_creation_datetime { get; set; }
        public Nullable<System.DateTime> d_updation_datetime { get; set; }
        public Nullable<int> d_created_by { get; set; }
        public Nullable<int> d_updated_by { get; set; }
    }
    public class tbl_designation_master
    {
        [Key]
        public int d_designation_id { get; set; }
        public int d_dept_id { get; set; }

        public string d_designation_desc { get; set; }
        public Nullable<int> d_status { get; set; }
        public Nullable<System.DateTime> d_creation_datetime { get; set; }
        public Nullable<System.DateTime> d_updation_datetime { get; set; }
        public Nullable<int> d_created_by { get; set; }
        public Nullable<int> d_updated_by { get; set; }
    }
}
