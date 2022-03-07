using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_medical_unit_code_master
    {
        [Key]
        public int mcm_unit_code { get; set; }
        public string mcm_unit_description { get; set; }
        public Nullable<bool> mcm_active { get; set; }
        public Nullable<System.DateTime> mcm_creation_datetime { get; set; }
        public Nullable<System.DateTime> mcm_updation_datetime { get; set; }
        public Nullable<int> mcm_created_by { get; set; }
        public Nullable<int> mcm_updated_by { get; set; }
    }
}
