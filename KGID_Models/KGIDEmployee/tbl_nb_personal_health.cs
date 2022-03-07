using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_nb_personal_health
    {
        [Key]
        public int nph_id { get; set; }
        public long nph_sys_emp_code { get; set; }
        public string nph_health_code { get; set; }
        public Nullable<int> nph_health_condition { get; set; }
        public string nph_under_goes_surgery { get; set; }
        public string nph_height { get; set; }
        public string nph_weight { get; set; }
        public Nullable<DateTime> nph_recent_period { get; set; }
        public string nph_pregnant { get; set; }
        public string nph_other_diseases { get; set; }

        public Nullable<int> nph_status { get; set; }
        public Nullable<bool> nph_active { get; set; }
        public Nullable<DateTime> nph_creation_datetime { get; set; }
        public Nullable<DateTime> nph_updation_datetime { get; set; }
        public Nullable<int> nph_created_by { get; set; }
        public Nullable<int> nph_updated_by { get; set; }
    }
}
