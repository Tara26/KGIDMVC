using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_respondent_details
    {
        [Key]
        public long mvcrd_respondent_id { get; set; }
         public long mvc_claim_app_id { get; set; }
        public string mvcrd_respondent_name { get; set; }
        public string mvcrd_designation_name { get; set; }
        public string mvcrd_department_name{ get; set; }
    public string mvcrd_agency_name { get; set; }
    public string mvcrd_respondent_addres { get; set; }
    public long mvcrd_pincode_no { get; set; }
    public long mvcrd_respondent_mobile_no { get; set; }
    public bool mvcrd_active_status { get; set; }
    public DateTime mvcrd_creation_datetime { get; set; }
    public DateTime mvcrd_updation_datetime { get; set; }
    public long mvcrd_created_by { get; set; }
    public long mvcrd_updated_by { get; set; }

}
}
