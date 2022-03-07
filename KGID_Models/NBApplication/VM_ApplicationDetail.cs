using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.NBApplication
{
    public class VM_ApplicationDetail
    {
        public string ApplicationNumber { get; set; }
        public long ApplicationId { get; set; }
        public string QRCode { get; set; }
        public int ApplicationCount { get; set; }
        public int SentBackAppliaction { get; set; }
        public int RestrictApplyingPolicy { get; set; }
        public int Remarks { get; set; }
        public long PaymentStatus { get; set; }
        public string appSubmittedDate { get; set; }
        public int cd_active_status { get; set; }
        public long cd_challan_id { get; set; }
        public string cd_challan_ref_no { get; set; }
       
        public int? amount { get; set; }
        public string Employe_name { get; set; }
        public long challan_status_id { get; set; }
        public long Emp_Id { get; set; }

        public string Challen_TransactionNo { get; set; }
 
        public VM_ApplicationDetail()
        {
            listTrackDetails = new List<VM_trackDetails>();
           
        }
        public List<VM_trackDetails> listTrackDetails { get; set; }
        
    }
    public class VM_trackDetails
    {
        public string application_no { get; set; }
        public string kgid_policy_number { get; set; }
        public string districtNames { get; set; }
        public string assigned_date { get; set; }
        public string application_status { get; set; }
    }
   
}
