using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_claim_doc_details
    {
        [Key]
        public long mvc_claim_dd_id { get; set; }
        public long mvcdd_claim_app_id { get; set; }
        public long mvcdd_claim_due_id  { get; set; }
        public string mvcdd_doc_upload_path  { get; set; }
        public bool mvcdd_active_status  { get; set; }
        public DateTime mvcdd_creation_datetime  { get; set; }
        public DateTime mvcdd_updation_datetime  { get; set; }
        public long mvcdd_created_by  { get; set; }
        public long mvcdd_updated_by  { get; set; }

        public int mvcdd_signed_status { get; set; }
        public string mvcdd_doc_signed_upload_path { get; set; }

    }
}
