using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_medical_report_master
    {
        [Key]
        public int mrm_id { get; set; }
        public long mrm_report_code { get; set; }
        public int mrm_med_unit_code { get; set; }
        public string mrm_report_description { get; set; }
        public Nullable<bool> mrm_active { get; set; }
        public Nullable<System.DateTime> mrm_creation_datetime { get; set; }
        public Nullable<System.DateTime> mrm_updation_datetime { get; set; }
        public Nullable<int> mrm_created_by { get; set; }
        public Nullable<int> mrm_updated_by { get; set; }
        public string mrm_med_doc { get; set; }
    }
}
