using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.Claim
{
    public class VM_ClaimRequiredDocuments
    {
        public VM_ClaimRequiredDocuments()
        {
            OtherDocuments = new List<HttpPostedFileBase>();
        }

        public int DocumentTypeId { get; set; }
        public int ClaimSubTypeId { get; set; }
        public long VerifiedByEmpId { get; set; }
        public long EmpId { get; set; }

        public HttpPostedFileBase ApplicationNominee { get; set; }
        public HttpPostedFileBase DeathCertificate { get; set; }
        public HttpPostedFileBase ClaimApplication { get; set; }
        public HttpPostedFileBase PolicyBonds { get; set; }
        public HttpPostedFileBase MedicalAttendanceReport { get; set; }
        public HttpPostedFileBase SurvivalCertificate { get; set; }
        public HttpPostedFileBase LeavesAvailed { get; set; }
        public HttpPostedFileBase MedicalReimbursement { get; set; }
        public HttpPostedFileBase DeathSummary { get; set; }
        public HttpPostedFileBase DischargeSummary { get; set; }
        public HttpPostedFileBase PoliceReport { get; set; }
        public HttpPostedFileBase FIR { get; set; }
        public HttpPostedFileBase PostMortemReport { get; set; }
        public HttpPostedFileBase CourtOrderDeclaringDead { get; set; }
        public HttpPostedFileBase SuccessionCertificate { get; set; }
        public HttpPostedFileBase ConsentLetter { get; set; }
        public HttpPostedFileBase CourtJudgment { get; set; }
        public HttpPostedFileBase GuardianshipCertificate { get; set; }
        public HttpPostedFileBase MarriageCertificate { get; set; }

        public bool IsNomineeAlive { get; set; }
        public bool IsNomineeMinor { get; set; }
        public bool IsMarriedAfterPolicyApplication { get; set; }

        public IList<HttpPostedFileBase> OtherDocuments { get; set; }
    }

    public class VM_ClaimDocumentsPathNames
    {
        public VM_ClaimDocumentsPathNames()
        {
            OtherDocumentPaths = new List<string>();
        }

        public long WorkFlowId { get; set; }
        public int DocumentTypeId { get; set; }
        public long VerifiedByEmpId { get; set; }
        public long EmpId { get; set; }
        public int ClaimSubTypeId { get; set; }

        public string DeathCertificate { get; set; }
        public int DeathCertificateDocTypeId { get; set; }
        public string ClaimApplication { get; set; }
        public int ClaimApplicationDocTypeId { get; set; }
        public string PolicyBonds { get; set; }
        public int PolicyBondsDocTypeId { get; set; }
        public string MedicalAttendanceReport { get; set; }
        public int MedicalAttendanceReportDocTypeId { get; set; }
        public string SurvivalCertificate { get; set; }
        public int SurvivalCertificateDocTypeId { get; set; }
        public string LeavesAvailed { get; set; }
        public int LeavesAvailedDocTypeId { get; set; }
        public string MedicalReimbursement { get; set; }
        public int MedicalReimbursementDocTypeId { get; set; }
        public string DeathSummary { get; set; }
        public int DeathSummaryDocTypeId { get; set; }
        public string DischargeSummary { get; set; }
        public int DischargeSummaryDocTypeId { get; set; }
        public string PoliceReport { get; set; }
        public int PoliceReportDocTypeId { get; set; }
        public string FIR { get; set; }
        public int FIRDocTypeId { get; set; }
        public string PostMortemReport { get; set; }
        public int PostMortemReportDocTypeId { get; set; }
        public string CourtOrderDeclaringDead { get; set; }
        public int CourtOrderDeclaringDeadDocTypeId { get; set; }
        public string SuccessionCertificate { get; set; }
        public int SuccessionCertificateDocTypeId { get; set; }
        public string ConsentLetter { get; set; }
        public int ConsentLetterDocTypeId { get; set; }
        public string CourtJudgment { get; set; }
        public int CourtJudgmentDocTypeId { get; set; }
        public string GuardianshipCertificate { get; set; }
        public int GuardianshipCertificateDocTypeId { get; set; }
        public string MarriageCertificate { get; set; }
        public int MarriageCertificateDocTypeId { get; set; }

        public IList<string> OtherDocumentPaths { get; set; }
    }
}
