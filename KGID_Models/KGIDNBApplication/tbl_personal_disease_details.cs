using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_personal_disease_details
    {
        [Key]
        public int pdd_id { get; set; }
        public Nullable<long> pdd_sys_emp_code { get; set; }
        public int pdd_personal_health_code { get; set; }
        public string pdd_details { get; set; }
        public Nullable<bool> pdd_active { get; set; }
        public Nullable<System.DateTime> pdd_creation_datetime { get; set; }
        public Nullable<System.DateTime> pdd_updation_datetime { get; set; }
        public Nullable<int> pdd_created_by { get; set; }
        public Nullable<int> pdd_updated_by { get; set; }
        public string pdd_disease_doc { get; set; }
    }
}
