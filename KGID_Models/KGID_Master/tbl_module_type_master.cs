using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_module_type_master
    {
        [Key]
        public int mt_module_type { get; set; }
        public int mt_created_by { get; set; }
        public int mt_updated_by { get; set; }
        public string mt_desc { get; set; }
        public DateTime mt_creation_datetime { get; set; }
        public DateTime mt_updation_datetime { get; set; }
        public bool mt_status { get; set; }
    }
}
