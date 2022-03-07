using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_post
    {
        [Key]
        public int p_post_id { get; set; }
        public string p_post_name { get; set; }
        public Nullable<int> p_status { get; set; }
        public Nullable<System.DateTime> p_creation_datetime { get; set; }
        public Nullable<System.DateTime> p_updation_datetime { get; set; }
        public Nullable<int> p_created_by { get; set; }
        public Nullable<int> p_updated_by { get; set; }
    }
}
