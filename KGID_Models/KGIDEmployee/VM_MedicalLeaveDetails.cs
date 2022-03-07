using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGIDEmployee
{
    public class VM_MedicalLeaveDetails
    {
        public VM_MedicalLeaveDetails()
        {
            MedicalLeaveDetails = new List<MedicalLeaveData>();
        }

        public long EmpCode { get; set; }
        public List<MedicalLeaveData> MedicalLeaveDetails { get; set; }
        
    }

    public class MedicalLeaveData
    {
        public long? emld_medical_leave_id { get; set; }
        public long? emld_application_id { get; set; }
        public long emld_emp_id { get; set; }
        public DateTime emld_start_date { get; set; }
        public DateTime emld_end_date { get; set; }
        public decimal emld_no_of_days { get; set; }
        public string emld_leave_reason { get; set; }
        public string emld_upload_document_path { get; set; }
        public string emld_medical_reimbursement { get; set; }
        public bool emld_active_status { get; set; }
        public HttpPostedFileBase doc { get; set; }
        public string docpath { get; set; }
    }
}
