using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_lokadalath_document
    {
        [Key]
        public int mvc_lok_doc_id { get; set; }
        public long mvc_ref_no { get; set; }
        public string lok_doc_path { get; set; }
        public bool lok_active_status { get; set; }
        public DateTime lok_doc_creation_datetime { get; set; }
        public DateTime lok_doc_updation_datetime { get; set; }
        public long created_by { get; set; }
        public long updated_by { get; set; }
    }
}
