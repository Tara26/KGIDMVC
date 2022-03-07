using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.Ticketing_Tool
{
    public class tbl_problem_type_master
    {
        [Key]
        public int pt_id { get; set; }
        public int pr_created_by { get; set; }
        public int pr_updated_by { get; set; }
        public string pr_description { get; set; }
        public DateTime pr_creation_datetime { get; set; }
        public DateTime pr_updation_datetime { get; set; }
        public bool pr_status { get; set; }
    }
}
