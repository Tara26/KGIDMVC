using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGID_Models.NBApplication
{
    public class VM_PaymentDetails
    {
        public int cd_challan_id { get; set; }
        public string cd_challan_ref_no { get; set; }
        public int cd_purpose_id { get; set; }
        public int cd_sub_purpose_id { get; set; }
        public int cd_amount { get; set; }
        public long cd_application_id { get; set; }
        public string cd_date_of_generation { get; set; }
        public string cs_transaction_ref_no { get; set; }
        public int cs_amount { get; set; }
        public long cs_status { get; set; }
        //public bool cs_status { get; set; }
        public int receipttypeid { get; set; }
        public string ddo_code { get; set; }
        public int hoa_id { get; set; }
        public long EmpID { get; set; }
        public int cs_challan_id { get; set; }
        public string cs_transsaction_date { get; set; }
        public string hoa { get; set; }
        public string purpose_desc { get; set; }
        public string sub_purpose_desc { get; set; }
        public string receipt_type_desc { get; set; }
        public string PayStatus { get; set; }
        public string EmpName { get; set; }

        public List<SelectListItem> PurposeTypes { get; set; }
        public List<SelectListItem> SubPurposeTypes { get; set; }
        public List<SelectListItem> ReceiptTypes { get; set; }
        public List<SelectListItem> HOATypes { get; set; }
    }
}
