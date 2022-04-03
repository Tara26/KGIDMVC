using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mvc_court_execution
    {
        [Key]
        public int court_execution_id { get; set; }
        public long court_mvc_ref_no { get; set; }
        public string court_chassis_no { get; set; }
        public string court_policy_no { get; set; }
        public long created_by { get; set; }
        public long updated_by { get; set; }
        public DateTime mvc_court_creation_datetime { get; set; }
        public DateTime mvc_court_updation_datetime { get; set; }
        public int verified_by { get; set; }
        public int assigned_to { get; set; }
        public bool status_id { get; set; }
        public decimal? CE_AwardedAmount { get; set; }
        public decimal? CE_AwardedInterest { get; set; }
        public decimal? CE_courtcost { get; set; }
        public decimal? CE_totalAmount { get; set; }
        public DateTime? CE_JudgementDate { get; set; }
        public decimal? CE_ClaimAwarded_Amount { get; set; }
        public int? CE_Opinion_Status { get; set; }

    }
}
