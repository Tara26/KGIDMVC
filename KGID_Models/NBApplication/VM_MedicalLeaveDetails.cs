using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.NBApplication
{
    public class VM_MedicalLeaveDetails
    {
        public VM_MedicalLeaveDetails()
        {
            MedicalLeaveDetails = new List<MedicalLeaveData>();
        }

        public long EmpCode { get; set; }
        public List<MedicalLeaveData> MedicalLeaveDetails { get; set; }
        public string JoiningDate { get; set; }

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
        public bool emld_medical_reimbursement { get; set; }
        public string emld_medical_reimbursement_doc { get; set; }
        public bool emld_active_status { get; set; }
        public HttpPostedFileBase doc { get; set; }
        public HttpPostedFileBase reimbursedoc { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string supportingdocpath { get; set; }
        public string reimbursedocpath { get; set; }
        public string Type { get; set; }
    }
}
