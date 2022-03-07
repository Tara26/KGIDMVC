using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_payment_subpurpose_master
    {
        public long psm_subpurpose_id { get; set; }
        public string psm_subpurpose_desc { get; set; }
        public long psm_purpose_id { get; set; }
        public Nullable<byte> psd_status { get; set; }
        public Nullable<System.DateTime> cd_creation_datetime { get; set; }
        public Nullable<System.DateTime> cd_updation_datetime { get; set; }
        public Nullable<int> cd_created_by { get; set; }
        public Nullable<int> cd_updated_by { get; set; }
    }
}
