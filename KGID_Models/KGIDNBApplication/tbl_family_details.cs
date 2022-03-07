using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_family_details
    {
        [Key]
        public int fd_id { get; set; }
        public Nullable<long> fd_sys_emp_code { get; set; }
        public string fd_name_of_family_member { get; set; }
        public string fd_relation { get; set; }
        public string fd_date_of_birth { get; set; }
        public string fd_living { get; set; }
        public string fd_health_details_living { get; set; }
        public string fd_reason_for_death { get; set; }
        public Nullable<int> fd_age { get; set; }
        public Nullable<bool> fd_active { get; set; }
        public Nullable<DateTime> fd_creation_datetime { get; set; }
        public Nullable<DateTime> fd_updation_datetime { get; set; }
        public Nullable<int> fd_created_by { get; set; }
        public Nullable<int> fd_updated_by { get; set; }
        public Nullable<bool> fd_is_sibling_married { get; set; }
        public Nullable<DateTime> fd_date_of_death { get; set; }
    }
}
