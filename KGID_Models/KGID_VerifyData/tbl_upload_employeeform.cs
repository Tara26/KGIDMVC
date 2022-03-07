using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_VerifyData
{
    public class tbl_upload_employeeform
    {
        [Key]
        public int App_Id { get; set; }
        public long App_Employee_Code { get; set; }
        public long App_ApplicationID { get; set; }
        public string App_Application_Form { get; set; }    
        public string App_Medical_Form { get; set; }
        public Nullable<bool> App_Active { get; set; }
        public Nullable<System.DateTime> App_Creation_Date { get; set; }
        public Nullable<long> App_Created_By { get; set; }
    }
}
