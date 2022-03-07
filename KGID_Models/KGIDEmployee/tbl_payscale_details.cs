using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_payscale_details
    {
        [Key]
        public int pd_id { get; set; }
        public long pd_payscale_code { get; set; }
        public string pd_kgid_number { get; set; }
        public long? pd_sys_emp_code { get; set; }
        public long? pd_app_ref_no { get; set; }
        public int pd_payscale { get; set; }
        public Nullable<decimal> pd_average_pay { get; set; }
        public Nullable<decimal> pd_kgid_premium { get; set; }
        public Nullable<System.DateTime> pd_creation_datetime { get; set; }
        public Nullable<System.DateTime> pd_updation_datetime { get; set; }
        public Nullable<int> pd_created_by { get; set; }
        public Nullable<int> pd_updated_by { get; set; }
    }

    public class VM_KGIDDetails
    {
        public int Id { get; set; }
        public string KGIDNumber { get; set; }
        public long? AppRefNo { get; set; }
        public long? EmpCode { get; set; }
        public string SactionDate { get; set; }
        public long PayScaleCode { get; set; }
        public Nullable<decimal> AveragePay { get; set; }
        public Nullable<decimal> KGIDPremium { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }

    public class PremiumDetails
    {
        public PremiumDetails()
        {
            KGIDPremiumDetails = new List<VM_KGIDDetails>();
        }

        public int KgidPolicy { get; set; }
        public long? EmpCode { get; set; }
        public long? AppRefNo { get; set; }
        public long PayScaleCode { get; set; }
        public Nullable<decimal> KgidPremium { get; set; }
        public Nullable<decimal> KgidPremiumAmount { get; set; }

        //public string nebd_policy_number { get; set; }
        //public Nullable<decimal> nebd_premium { get; set; }
        public IList<VM_KGIDDetails> KGIDPremiumDetails { get; set; }
    }
}
