using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGID_User
{
    public class tbl_employee_group_master
    {
        [Key]
        public int eg_group_id { get; set; }
        public string eg_group_desc { get; set; }
        public Nullable<bool> eg_active { get; set; }
        public Nullable<DateTime> eg_creation_datetime { get; set; }
        public Nullable<DateTime> eg_updation_datetime { get; set; }
        public Nullable<int> eg_created_by { get; set; }
        public Nullable<int> eg_updated_by { get; set; }
    }
}
