using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using KGID_Models.KGIDMotorInsurance;

namespace KGID_Models.KGID_VerifyData
{
   public class VM_MotorInsuranceDeptVerficationDetails
    {
        public VM_MotorInsuranceDeptVerficationDetails()
        {
            WorkFlowDetails = new List<VM_MIWorkFlowDetails>();           
        }
        public int VerificationId { get; set; }
        public long EmpCode { get; set; }
        public long ApplicationRefNo { get; set; }

        public bool VerifyProposerDetails { get; set; }
        public bool VerifyVehicletDetails { get; set; }
        public bool VerifyPreviousHistoryDetails { get; set; }
        //public bool VerifyIDVDetails { get; set; }
        public bool VerifyOtherCondition { get; set; }
        public string PreviousRemarks { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public int ApplicationStatus { get; set; }
        public bool VerifyPaymentDetails { get; set; }
        //  public string HealthReportUploadPath { get; set; }
        //  public HttpPostedFileBase HealthUploadDoc { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public int Category { get; set; }
        //  public string LoadFactor { get; set; }
        //  public string DeductionLoadFactor { get; set; }
        //  public IList<SelectListItem> DeductionLoadFactors { get; set; }
        //  public bool IsHealthOpinion { get; set; }
        //   public decimal? SumAssured { get; set; }

        public IList<VM_MIWorkFlowDetails> WorkFlowDetails { get; set; }



    }
}
