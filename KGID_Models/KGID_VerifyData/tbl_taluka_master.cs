using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_VerifyData
{
    public class tbl_taluka_master
    {
        [Key]
        public int tm_id { get; set; }
        public string tm_code { get; set; }
        public int tm_distid { get; set; }
        public string tm_englishname { get; set; }
        public string tm_kannadaname { get; set; }
        public Nullable<bool> tm_active { get; set; }
        public Nullable<System.DateTime> tm_creation_datetime { get; set; }
        public Nullable<System.DateTime> tm_updation_datetime { get; set; }
        public Nullable<int> tm_created_by { get; set; }
        public Nullable<int> tm_updated_by { get; set; }
    }
}
