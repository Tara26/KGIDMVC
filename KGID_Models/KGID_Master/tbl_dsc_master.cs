using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_dsc_master
    {
        [Key]
        public int dsc_dsc_id { get; set; }
        public string dsc_kgid_number { get; set; }
        public string dsc_public_key { get; set; }
        public DateTime dsc_date_of_expiring { get; set; }
        public int dsc_dsc_serial_no { get; set; }
        public bool dsc_active { get; set; }
        public int dsc_created_by { get; set; }
        public DateTime dsc_creation_datetime { get; set; }
        public int dsc_updated_by { get; set; }
        public DateTime dsc_updation_datetime { get; set; }
        public long dsc_emp_id { get; set; }
    }
}
