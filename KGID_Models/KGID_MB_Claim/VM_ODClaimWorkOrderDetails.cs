using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_MB_Claim
{
    public class VM_ODClaimWorkOrderDetails
    {
        public IList<ApprovedClaimComponentList> ClaimComponentListDetails { get; set; }
        //public List<ClaimComponentList1> ClaimComponentsList { get; set; }
        public long Odca_id { get; set; }
        public string Odca_claim_app_no { get; set; }
        public long Odca_claim_id { get; set; }
        public long Odca_proposer_id { get; set; }
        public string Odca_category_id { get; set; }
        public string Odca_vehicle_number { get; set; }
        //public long Odca_policy_id { get; set; }
        public string Odca_policy_number { get; set; }
        public DateTime? Odca_date_time_of_accident { get; set; }
        public long Odca_accident_cause_id { get; set; }
        public string Odca_place_of_accident { get; set; }
        public string Odca_district_name { get; set; }
        public string Odca_taluka_name { get; set; }
        public decimal Odca_damage_cost { get; set; }
        public decimal micw_approved_damage_cost { get; set; }
        public bool Odca_active_status { get; set; }
        public DateTime WorkOrderDate { get; set; }
        public string vy_vehicle_year { get; set; }
        public string ProposerName { get; set; }
        public string ProposerAddress { get; set; }
        public string DdoOffice { get; set; }
    }
    public class ApprovedClaimComponentList
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
