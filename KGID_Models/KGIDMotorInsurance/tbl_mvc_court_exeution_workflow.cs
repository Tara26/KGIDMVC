using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_court_exeution_workflow
    {

        [Key]
        public int mvc_court_execution_id { get; set; }
        public long mvc_court_exe_app_id { get; set; }
        public string court_veh_chassis_no { get; set; }
        public string court_veh_policy_no { get; set; }
        public long court_remarks { get; set; }
        public string court_comment { get; set; }
        public long court_verified_by { get; set; }
        public bool court_checklist_status { get; set; }
        public int court_application_status { get; set; }
        public bool court_active_status { get; set; }
        public DateTime court_creation_datetime { get; set; }
        public DateTime court_updation_datetime { get; set; }
        public long court_created_by { get; set; }
        public long court_updated_by { get; set; }
        public long court_assigned_to { get; set; }
        public long verified_by_category { get; set; }
        public long assigned_To_category { get; set; }
        public int? CE_OpinionLawDept { get; set; }
        public bool? CE_OpinionLawDeptStatus { get; set; }
        public int? CE_ClaimSettleDept { get; set; }
        public bool? CE_ClaimSettleDeptStatus { get; set; }
        public int? CE_Mainflow { get; set; }

    }
}
