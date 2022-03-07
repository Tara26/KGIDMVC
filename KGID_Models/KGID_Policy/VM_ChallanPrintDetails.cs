using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Policy
{
   public class VM_ChallanPrintDetails
    {
        public string dm_ddo_code { get; set; }
        public string dm_ddo_office { get; set; }
        public string dm_deptname_english { get; set; }
        public string dm_name_english { get; set; }
        public string employee_name { get; set; }
        public string ead_address { get; set; }
        public string mobile_number { get; set; }
        public string hoa_desc { get; set; }
        public string purpose_id { get; set; }
        public string purpose_desc { get; set; }
        public string sub_purpose_desc { get; set; }
        public Nullable<double> p_premium { get; set; }
        public string challan_ref_no { get; set; }
        // public DateTime? challan_date { get; set; }
        public string challan_date { get; set; }
        public string challan_status { get; set; }
        public string RemittanceBank { get; set; }
        public string GrandTotal { get; set; }
        public string AmountInWords { get; set; }
        public string Cheque_DD_No { get; set; }
        public string Cheque_DD_Bank { get; set; }
        public string IFSC_Code { get; set; }
        public string MICR_Code { get; set; }
        public DateTime? Cheque_DD_Date { get; set; }
        public string Challan_Validity { get; set; }
        public string Category { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public string ApplicationReferenceNo { get; set; }
    }
}
