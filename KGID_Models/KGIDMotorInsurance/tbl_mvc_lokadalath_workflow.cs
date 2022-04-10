using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_lokadalath_workflow
    {
        [Key]
        public int mvc_lok_execution_id { get; set; }
        public long mvc_lok_exe_app_id { get; set; }
        public string lok_veh_no { get; set; }
        public string lok_veh_policy_no { get; set; }
        public long lok_remarks { get; set; }
        public string lok_comment { get; set; }
        public long lok_verified_by { get; set; }
        public bool lok_checklist_status { get; set; }
        public int lok_application_status { get; set; }
        public bool lok_active_status { get; set; }
        public DateTime lok_creation_datetime { get; set; }
        public DateTime lok_updation_datetime { get; set; }
        public long lok_created_by { get; set; }
        public long lok_updated_by { get; set; }
        public long lok_assigned_to { get; set; }
        public long verified_by_category { get; set; }
        public long assigned_To_category { get; set; } 
        public int? loc_opinionJudgementCopy { get; set; }
        public bool? loc_opinionJudgementCopyStatus { get; set; }
        public int Lok_Mainflow { get; set; }
        public int? lok_supreme_opinionJudgementCopy { get; set; }
        public bool? lok_supreme_opinionJudgementCopyStatus { get; set; } 
        public int? Lok_ClaimSettleOnLowerCourt2 { get; set; }
        public bool? Lok_ClaimSettleOnLowerCourt2Status { get; set; }
        public int? lok_claimSettle_Highcourt { get; set; }
        public bool? lok_claimSettle_HighStatus { get; set; } 
        public int? lok_supreme_opinionJudgementCopy2 { get; set; }
        public bool? lok_supreme_opinionJudgementCopyStatus2 { get; set; }
        public int? Lok_supreme_ClaimSettleOnLowerCourt { get; set; }
        public bool? Lok_supreme_ClaimSettleOnLowerCourtStatus { get; set; }

    }
}
