using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
    public class tbl_kgid_roles
    {
        [Key]
        public int KGID_RoleId { get; set; }
        public string KGID_RoleName { get; set; }
    }
}
