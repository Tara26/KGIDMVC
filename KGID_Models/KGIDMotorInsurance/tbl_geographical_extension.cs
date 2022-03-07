using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_geographical_extension
    {
        [Key]
        public int ge_geographical_extension_id { get; set; }
        public string ge_name_of_country { get; set; }
        public bool ge_active { get; set; }
        //public Nullable<int> mipd_pincode { get; set; }
        public DateTime ge_creation_datetime { get; set; }
        public int ge_created_by { get; set; }
        public DateTime ge_updation_datetime { get; set; }
        public int ge_updated_by { get; set; }
    }
}
