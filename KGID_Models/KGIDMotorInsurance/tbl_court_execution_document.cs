using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_court_execution_document
    {
        [Key]
        public int mvc_court_doc_id { get; set; }
        public long mvc_ref_no { get; set; }
        public string doc_path { get; set; }
        public bool active_status { get; set; }
        public DateTime court_doc_creation_datetime { get; set; }
        public DateTime court_doc_updation_datetime { get; set; }
        public long created_by { get; set; }
        public long updated_by { get; set; }
    }
}
