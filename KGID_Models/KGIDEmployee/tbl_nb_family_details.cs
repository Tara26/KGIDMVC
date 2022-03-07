using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_nb_family_details
    {
        [Key]
        public int nfd_id { get; set; }
        public long nfd_sys_emp_code { get; set; }
        public string nfd_relation { get; set; }
        public Nullable<DateTime> nfd_date_of_birth { get; set; }
        public string nfd_living_or_dead { get; set; }
        public string nfd_living_health { get; set; }
        public string nfd_dead_reason { get; set; }
        public Nullable<int> nfd_age_of_dead { get; set; }
        public Nullable<bool> nfd_active { get; set; }
        public Nullable<DateTime> nfd_creation_datetime { get; set; }
        public Nullable<DateTime> nfd_updation_datetime { get; set; }
        public Nullable<int> nfd_created_by { get; set; }
        public Nullable<int> nfd_updated_by { get; set; }
    }
}
