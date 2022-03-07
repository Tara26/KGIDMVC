using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_category_master
    {
        [Key]
        public int cm_category_id { get; set; }
        public string cm_category_desc { get; set; }
        public bool? cm_status { get; set; }
        public DateTime? cm_creation_datetime { get; set; }
        public DateTime? cm_updation_datetime { get; set; }
        public long? cm_created_by { get; set; }
        public long? cm_updated_by { get; set; }
    }
}
