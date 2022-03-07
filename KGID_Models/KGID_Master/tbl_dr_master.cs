using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_dr_master
    {
        [Key]
        public int dr_id { get; set; }
        public string dr_desc { get; set; }
        public bool? dr_status { get; set; }
        public DateTime? dr_creation_datetime { get; set; }
        public DateTime? dr_updation_datetime { get; set; }
        public long? dr_created_by { get; set; }
        public long? dr_updated_by { get; set; }
    }
}
