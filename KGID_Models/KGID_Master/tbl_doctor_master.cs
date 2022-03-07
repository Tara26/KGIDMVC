using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_doctor_master
    {
        [Key]
        public long dm_doctor_master_id { get; set; }
        public string dm_kmc_code { get; set; }
        public string dm_imc_code { get; set; }
        public string dm_name_of_doctor { get; set; }
        public string dm_designation { get; set; }
        public string dm_name_of_hospital { get; set; }
        public long dm_policy_id { get; set; }
        public int dm_number_of_years { get; set; }
        public Nullable<bool> dm_active_status { get; set; }
        public Nullable<DateTime> dm_creation_datetime { get; set; }
        public Nullable<DateTime> dm_updation_datetime  { get; set; }
        public Nullable<long> dm_created_by { get; set; }
        public Nullable<long> dm_updated_by { get; set; }
    }
}
