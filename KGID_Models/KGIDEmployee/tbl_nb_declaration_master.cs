using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_nb_declaration_master
    {
        [Key]
        public int ndm_id { get; set; }
        public long ndm_declaration_code { get; set; }
        public bool ndm_declaration_status { get; set; }
        public Nullable<long> ndm_referance_no { get; set; }
        public Nullable<bool> ndm_active { get; set; }
        public Nullable<System.DateTime> ndm_creation_datetime { get; set; }
        public Nullable<System.DateTime> ndm_updation_datetime { get; set; }
        public Nullable<int> ndm_created_by { get; set; }
        public Nullable<int> ndm_updated_by { get; set; }
    }
}
