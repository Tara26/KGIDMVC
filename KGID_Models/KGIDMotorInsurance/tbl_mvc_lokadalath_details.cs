using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_lokadalath_details
    {
        [Key]
        public int lok_execution_id { get; set; }
        public long lok_mvc_ref_no { get; set; }
        public string lok_chassis_no { get; set; }
        public string lok_policy_no { get; set; }
        public long lok_created_by { get; set; }
        public long lok_updated_by { get; set; }
        public DateTime lok_creation_datetime { get; set; }
        public DateTime lok_updation_datetime { get; set; }
        public int lok_verified_by { get; set; }
        public int lok_assigned_to { get; set; }
        public bool lok_status_id { get; set; }
        public DateTime lokadalath_date { get; set; }
        public int? mvc_lok_OpinionId { get; set; }
        public DateTime? mvc_lok_judgementDate { get; set; }
        public decimal? mvc_lok_awarded_amount { get; set; }
        public int? lok_supreme_opinionId { get; set; }
        public DateTime? lok_supreme_judgementDate { get; set; }
        public decimal? lok_supreme_awarded_amount { get; set; }
        public decimal? lok_claimSettle_awarded_amount { get; set; }
        public decimal? lok_claimSettle_awarded_Intrest_amount { get; set; }
        public decimal? lok_claimSettle_courtcost { get; set; }
        public decimal? lok_claimSettle_totalAmount { get; set; }
        public decimal? lok_claimSettle_Highawarded_amount { get; set; }
        public decimal? lok_claimSettle_Highawarded_Intrest_amount { get; set; }
        public decimal? lok_claimSettle_Highcourtcost { get; set; }
        public decimal? lok_claimSettle_HightotalAmount { get; set; }

        public int? lok_supreme_opinionId2 { get; set; }
        public DateTime? lok_supreme_judgementDate2 { get; set; }
        public decimal? lok_supreme_awarded_amount2 { get; set; }
        public decimal? lok_Suprm_claimSettle_awarded_amount { get; set; }
        public decimal? lok__suprm_claimSettle_awarded_Intrest_amount { get; set; }
        public decimal? lok_suprm_claimSettle_courtcost { get; set; }
        public decimal? lok_sprm_claimSettle_totalAmount { get; set; }
    }
}
