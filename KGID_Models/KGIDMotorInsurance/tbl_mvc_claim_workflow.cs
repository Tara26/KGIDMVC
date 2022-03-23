using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
   
    public class tbl_mvc_claim_workflow
    {
        [Key]
        public long mvc_claim_id { get; set; }
       public long mvc_claim_app_id { get; set; }
        public string micw_vehicle_number { get; set; }
        public string micw_policy_number { get; set; }
        public long micw_remarks { get; set; }
        public string micw_comments { get; set; }
        public long micw_verified_by { get; set; }
        public bool micw_checklist_status { get; set; }
        public int micw_application_status { get; set; }
        public bool micw_surveyor_registered { get; set; }
        public decimal micw_approved_damage_cost { get; set; }
        public bool micw_active_status { get; set; }
        public DateTime micw_creation_datetime { get; set; }
        public DateTime micw_updation_datetime { get; set; }
        public long micw_created_by { get; set; }
        public long micw_updated_by { get; set; }
        public long micw_assigned_to { get; set; }
        public int? mvc_parawiseRemarkLawyer { get; set; }
        public int mvc_main_flow { get; set; }
        public bool? mvc_parawiseRemarkLawyerStatus { get; set; }
        public int? mvc_objecttionStatement { get; set; }
        public bool? mvc_objecttionStatementStatus { get; set; } 
        public int? mvc_ratificationLawDept { get; set; }
        public bool? mvc_ratificationLawDeptStatus { get; set; }

    }
}
