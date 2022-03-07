using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_family_relation_master
    {
        [Key]
        public int frm_relation_id { get; set; }
        public string frm_relation_desc { get; set; }
        public DateTime? frm_creation_datetime { get; set; }
        public DateTime? frm_updation_datetime { get; set; }
        public int? frm_created_by { get; set; }
        public int? frm_updated_by { get; set; }
        public bool? frm_active_status { get; set; }
        public long frm_family_type_id { get; set; }
    }
}
