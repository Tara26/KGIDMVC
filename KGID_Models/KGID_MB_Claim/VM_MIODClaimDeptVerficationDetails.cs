using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_MB_Claim
{
    public class VM_MIODClaimDeptVerficationDetails
    {
        public VM_MIODClaimDeptVerficationDetails()
        {
            WorkFlowDetails = new List<VM_MIODClaimWorkFlowDetails>();
            ODClaimApplicationDetails = new List<ODClaimApplicationDetails>();
            ClaimUploadDocumentDetails = new List<ODClaimsDocumetsDetails>();
            ClaimUploadImageDetails = new List<ODClaimsImageDetails>();
            ClaimsComponentDetailsApplicant = new List<ODClaimsComponentDetailsApplicant>();
            ClaimsComponentDetailsSurveyor = new List<ODClaimsComponentDetailsSurveyor>();
            ClaimsComponentDetailsDepartment = new List<ODClaimsComponentDetailsDepartment>();
        }
        public Nullable<DateTime> DateOfInspection { get; set; }
        public long SurveyorId { get; set; }
        public long RepairerId { get; set; }
        public decimal DamageCost { get; set; }
        public decimal ApprovedDamageCost { get; set; }
        public int VerificationId { get; set; }
        public long EmpCode { get; set; }
        public long ApplicationRefNo { get; set; }
        public long ApplicationId { get; set; }
        public string MIPolicyNo { get; set; }
        public bool VerifyApplicationDetails { get; set; }
        public bool VerifyDocumenttDetails { get; set; }
        public bool VerifyImageDetails { get; set; }
        //public bool VerifyOtherCondition { get; set; }
        public string PreviousRemarks { get; set; }
        public int Remarks { get; set; }
        public string Comments { get; set; }
        public int ApplicationStatus { get; set; }
        //public bool VerifyPaymentDetails { get; set; }
        //  public string HealthReportUploadPath { get; set; }
        //  public HttpPostedFileBase HealthUploadDoc { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public int Category { get; set; }
        //  public string LoadFactor { get; set; }
        //  public string DeductionLoadFactor { get; set; }
        //  public IList<SelectListItem> DeductionLoadFactors { get; set; }
        //  public bool IsHealthOpinion { get; set; }
        //   public decimal? SumAssured { get; set; }

        public IList<VM_MIODClaimWorkFlowDetails> WorkFlowDetails { get; set; }
        public IList<ODClaimApplicationDetails> ODClaimApplicationDetails { get; set; }
        public IList<ODClaimsDocumetsDetails> ClaimUploadDocumentDetails { get; set; }
        public IList<ODClaimsImageDetails> ClaimUploadImageDetails { get; set; }
        public IList<ODClaimsComponentDetailsApplicant> ClaimsComponentDetailsApplicant { get; set; }
        public IList<ODClaimsComponentDetailsSurveyor> ClaimsComponentDetailsSurveyor { get; set; }
        public IList<ODClaimsComponentDetailsDepartment> ClaimsComponentDetailsDepartment { get; set; }
    }
}
    public class ODClaimApplicationDetails
    {
        public long OD_Claim_Application_No { get; set; }
        public long OD_Claim_Proposer_ID { get; set; }
        public string OD_Claim_Vehicle_Number { get; set; }
        public string OD_Claim_Policy_Number { get; set; }
        public long OD_Claim_ID { get; set; }
        public string OD_Claim_Damage_Cost { get; set; }
        public string OD_Claim_Datetime_of_Accident { get; set; }
        public long OD_Claim_Accident_Cause_ID { get; set; }
        public string OD_Claim_Place_of_Accident { get; set; }
        public long OD_Claim_District_ID { get; set; }
        public long OD_Claim_Taluka_ID { get; set; }
        public string OD_Claim_District_Name { get; set; }
        public string OD_Claim_Taluka_Name { get; set; }
    }
    public class ODClaimsDocumetsDetails
    {
        public long OD_Claim_ID { get; set; }
        public string OD_Claim_Description { get; set; }
        public string OD_Claim_Document_Description { get; set; }
        public long OD_Claim_Doc_id { get; set; }
        public long OD_Claim_Application_id { get; set; }
        public long OD_Claim_Due_id { get; set; }
        public string OD_Claim_Doc_Upload_Path { get; set; }
    }
    public class ODClaimsImageDetails
    {
        public long OD_Claim_App_id { get; set; }
        public string OD_Claim_Image_Description { get; set; }
        public string OD_Claim_Doc_Upload_Path { get; set; }
    }
    public class ODClaimsComponentDetailsApplicant
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
    public class ODClaimsComponentDetailsSurveyor
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
    public class ODClaimsComponentDetailsDepartment
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
