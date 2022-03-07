using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_medical_leave
    {
        [Key]
        public int med_leave_id { get; set; }
        public long? sys_emp_code { get; set; }
        public string leave_start_date { get; set; }
        public string leave_end_date { get; set; }
        public decimal? leave_totl_duration { get; set; }
        public string leave_reason { get; set; }
        public string leave_supporting_doc_path { get; set; }
        public bool? leave_is_active { get; set; }
        public int? leave_created_by { get; set; }
        public DateTime? leave_created_on { get; set; }
        public int? leave_updated_by { get; set; }
        public DateTime? leave_updated_on { get; set; }
    }
}
