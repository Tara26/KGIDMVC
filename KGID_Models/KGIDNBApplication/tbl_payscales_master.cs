using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_payscales_master
    {
        [Key]
        public int payscale_id { get; set; }
        public Nullable<decimal> payscale_minimum { get; set; }
        public Nullable<decimal> payscale_maximum { get; set; }
        public Nullable<decimal> payscale_average { get; set; }
        public Nullable<int> payscale_status { get; set; }
        public Nullable<bool> pm_active { get; set; }
        public Nullable<System.DateTime> pm_creation_datetime { get; set; }
        public Nullable<System.DateTime> pm_updation_datetime { get; set; }
        public Nullable<int> pm_created_by { get; set; }
        public Nullable<int> pm_updated_by { get; set; }
    }
}
