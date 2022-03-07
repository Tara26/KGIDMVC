using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_od_cost_component_master
    {
        [Key]
        public long odcc_id { get; set; }
        public string odcc_description { get; set; }
        public long? odcc_vehicle_type_id { get; set; }
        public Nullable<bool> odcc_status { get; set; }
        public Nullable<System.DateTime> odcc_creation_datetime { get; set; }
        public Nullable<System.DateTime> odcc_updation_datetime { get; set; }
        public Nullable<int> odcc_created_by { get; set; }
        public Nullable<int> odcc_updated_by { get; set; }
    }
}
