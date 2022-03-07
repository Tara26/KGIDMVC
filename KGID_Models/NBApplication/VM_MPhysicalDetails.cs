using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.NBApplication
{
    public class VM_MPhysicalDetails
    {
        //Medical physical Details
        public long employee_id { get; set; }
        public Nullable<long> application_id { get; set; }
        public string empd_height { get; set; }
        public string empd_weight { get; set; }
        public string empd_pulse_rate { get; set; }
        public string empd_breathing_rate { get; set; }
        public string empd_blood_pressure { get; set; }
        public string empd_low_dystolic { get; set; }
        public string empd_high_systolic { get; set; }
        public string empd_remarks { get; set; }
        public bool empd_ismedicalreqd { get; set; }
        public string emp_gender { get; set; }
    }
}
