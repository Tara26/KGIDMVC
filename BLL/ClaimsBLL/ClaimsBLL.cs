using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Common;
using DLL.ClaimsDLL;
using KGID_Models.Claim;

namespace BLL.ClaimsBLL
{
    public class ClaimsBLL : IClaimsBLL
    {
        private readonly IClaimsDLL claims;

        public ClaimsBLL()
        {
            claims = new ClaimsDLL();
        }

        public VM_ClaimEmployeeDetail GetMaturityClaimEmployeeDetails(long empId)
        {
            return claims.GetMaturityClaimEmployeeDetails(empId);
        }

        public VM_ClaimEmployeeDetail GetPreMaturityClaimEmployeeDetails(long empId)
        {
            return claims.GetPreMaturityClaimEmployeeDetails(empId);
        }

        public VM_ClaimEmployeeDetail GetDeathClaimEmployeeDetails(long empId)
        {
            return claims.GetDeathClaimEmployeeDetails(empId);
        }

        public VM_ClaimApplications GetClaimApplications(int empType, int claimType, long loggedInUserId)
        {
            return claims.GetClaimApplications(empType, claimType, loggedInUserId);
        }

        public string SaveVerifiedDetailsBll(VM_ClaimEmployeeDetail objVerification)
        {
            return claims.SaveVerifiedDetailsDll(objVerification);
        }

        public IList<VM_ClaimWorkflowDetail> GetClaimWorkFlowDetails(long applicationId)
        {
            IList<VM_ClaimWorkflowDetail> workflowDetails = new List<VM_ClaimWorkflowDetail>();

            return claims.GetClaimWorkFlowDetails(applicationId);
        }

        public VM_EmpDetailForPrematureClaimApplication GetEmployeeDetailByKGIDNumber(string kgidNumber)
        {
            return claims.GetEmployeeDetailByKGIDNumber(kgidNumber);
        }

        public string ForwardApplicationToCaseworker(VM_EmpDetailForPrematureClaimApplication empDetail)
        {
            if (empDetail.RelievingLetterDoc != null)
            {
                empDetail.RelievingLetterDocTypeId = (int)ClaimDocuments.RELIEVINGLETTER;
                empDetail.RelievingLetterDocPath = UploadDocument(empDetail.RelievingLetterDoc, empDetail.Id, (int)ClaimDocuments.RELIEVINGLETTER, "Claim Documents");
            }

            if (empDetail.ResignationAcceptanceLetterDoc != null)
            {
                empDetail.ResignationAcceptanceLetterDocTypeId = (int)ClaimDocuments.RESIGNATIONACCEPTANCELETTER;
                empDetail.ResignationAcceptanceLetterDocPath = UploadDocument(empDetail.ResignationAcceptanceLetterDoc, empDetail.Id, (int)ClaimDocuments.RESIGNATIONACCEPTANCELETTER, "Claim Documents");
            }

            if (empDetail.ResignationLetterDoc != null)
            {
                empDetail.ResignationLetterDocTypeId = (int)ClaimDocuments.RESIGNATIONLETTER;
                empDetail.ResignationLetterDocPath = UploadDocument(empDetail.ResignationLetterDoc, empDetail.Id, (int)ClaimDocuments.RESIGNATIONLETTER, "Claim Documents");
            }

            if (empDetail.TerminationLetterDoc != null)
            {
                empDetail.TerminationLetterDocTypeId = (int)ClaimDocuments.TERMINATIONLETTER;
                empDetail.TerminationLetterDocPath = UploadDocument(empDetail.TerminationLetterDoc, empDetail.Id, (int)ClaimDocuments.TERMINATIONLETTER, "Claim Documents");
            }

            return claims.ForwardApplicationToCaseworker(empDetail);
        }

        public IList<SelectListItem> GetClaimSubTypes(int claimType)
        {
            List<SelectListItem> claimSubTypes = null;

            VM_ClaimSubTypes claimSubTypesDB = claims.GetClaimSubTypes(claimType);
            if (claimSubTypesDB != null)
            {
                claimSubTypes = new List<SelectListItem>();
                foreach (var claimSubType in claimSubTypesDB.ClaimSubTypes)
                {
                    claimSubTypes.Add(new SelectListItem()
                    {
                        Text = claimSubType.ClaimSubType,
                        Value = claimSubType.Id.ToString()
                    });
                }
            }

            return claimSubTypes;
        }

        public bool InitiateDeathClaimApplication(VM_ClaimRequiredDocuments claimDocuments)
        {
            VM_ClaimDocumentsPathNames documentsPathNames = new VM_ClaimDocumentsPathNames();
            documentsPathNames.ClaimSubTypeId = claimDocuments.ClaimSubTypeId;
            documentsPathNames.EmpId = claimDocuments.EmpId;
            documentsPathNames.VerifiedByEmpId = claimDocuments.VerifiedByEmpId;

            if (claimDocuments.ClaimApplication != null)
            {
                documentsPathNames.ClaimApplicationDocTypeId = (int)ClaimDocuments.CLAIMAPPLICATION;
                documentsPathNames.ClaimApplication = UploadDocument(claimDocuments.ApplicationNominee, claimDocuments.EmpId, (int)ClaimDocuments.CLAIMAPPLICATION, "Claim Documents");
            }
            else
                documentsPathNames.ClaimApplication = "";

            if (claimDocuments.DeathCertificate != null)
            {
                documentsPathNames.DeathCertificateDocTypeId = (int)ClaimDocuments.DEATHCERTIFICATE;
                documentsPathNames.DeathCertificate = UploadDocument(claimDocuments.DeathCertificate, claimDocuments.EmpId, (int)ClaimDocuments.DEATHCERTIFICATE, "Claim Documents");
            }
            else
                documentsPathNames.DeathCertificate = "";

            if (claimDocuments.PolicyBonds != null)
            {
                documentsPathNames.PolicyBondsDocTypeId = (int)ClaimDocuments.POLICYBONDS;
                documentsPathNames.PolicyBonds = UploadDocument(claimDocuments.PolicyBonds, claimDocuments.EmpId, (int)ClaimDocuments.POLICYBONDS, "Claim Documents");
            }
            else
                documentsPathNames.PolicyBonds = "";

            if (claimDocuments.MedicalAttendanceReport != null)
            {
                documentsPathNames.MedicalAttendanceReportDocTypeId = (int)ClaimDocuments.MEDICALATTENDANCEREPORT;
                documentsPathNames.MedicalAttendanceReport = UploadDocument(claimDocuments.MedicalAttendanceReport, claimDocuments.EmpId, (int)ClaimDocuments.MEDICALATTENDANCEREPORT, "Claim Documents");
            }
            else
                documentsPathNames.MedicalAttendanceReport = "";

            if (claimDocuments.SurvivalCertificate != null)
            {
                documentsPathNames.SurvivalCertificateDocTypeId = (int)ClaimDocuments.SURVIVALCERTIFICATE;
                documentsPathNames.SurvivalCertificate = UploadDocument(claimDocuments.SurvivalCertificate, claimDocuments.EmpId, (int)ClaimDocuments.SURVIVALCERTIFICATE, "Claim Documents");
            }
            else
                documentsPathNames.SurvivalCertificate = "";

            if (claimDocuments.LeavesAvailed != null)
            {
                documentsPathNames.LeavesAvailedDocTypeId = (int)ClaimDocuments.LEAVESAVAILEDONMEDICALGROUNDS;
                documentsPathNames.LeavesAvailed = UploadDocument(claimDocuments.LeavesAvailed, claimDocuments.EmpId, (int)ClaimDocuments.LEAVESAVAILEDONMEDICALGROUNDS, "Claim Documents");
            }
            else
                documentsPathNames.LeavesAvailed = "";

            if (claimDocuments.MedicalReimbursement != null)
            {
                documentsPathNames.MedicalReimbursementDocTypeId = (int)ClaimDocuments.MEDICALATTENDANCEREPORT;
                documentsPathNames.MedicalReimbursement = UploadDocument(claimDocuments.MedicalReimbursement, claimDocuments.EmpId, (int)ClaimDocuments.MEDICALATTENDANCEREPORT, "Claim Documents");
            }
            else
                documentsPathNames.MedicalReimbursement = "";

            if (claimDocuments.DeathSummary != null)
            {
                documentsPathNames.DeathSummaryDocTypeId = (int)ClaimDocuments.DEATHSUMMARY;
                documentsPathNames.DeathSummary = UploadDocument(claimDocuments.DeathSummary, claimDocuments.EmpId, (int)ClaimDocuments.DEATHSUMMARY, "Claim Documents");
            }
            else
                documentsPathNames.DeathSummary = "";


            if (claimDocuments.DischargeSummary != null)
            {
                documentsPathNames.DischargeSummaryDocTypeId = (int)ClaimDocuments.DISCHARGESUMMARY;
                documentsPathNames.DischargeSummary = UploadDocument(claimDocuments.DischargeSummary, claimDocuments.EmpId, (int)ClaimDocuments.DISCHARGESUMMARY, "Claim Documents");
            }
            else
                documentsPathNames.DischargeSummary = "";

            if (claimDocuments.PoliceReport != null)
            {
                documentsPathNames.PoliceReportDocTypeId = (int)ClaimDocuments.POLICEREPORT;
                documentsPathNames.PoliceReport = UploadDocument(claimDocuments.PoliceReport, claimDocuments.EmpId, (int)ClaimDocuments.POLICEREPORT, "Claim Documents");
            }
            else
                documentsPathNames.PoliceReport = "";

            if (claimDocuments.FIR != null)
            {
                documentsPathNames.FIRDocTypeId = (int)ClaimDocuments.FIR;
                documentsPathNames.FIR = UploadDocument(claimDocuments.FIR, claimDocuments.EmpId, (int)ClaimDocuments.FIR, "Claim Documents");
            }
            else
                documentsPathNames.FIR = "";


            if (claimDocuments.PostMortemReport != null)
            {
                documentsPathNames.PostMortemReportDocTypeId = (int)ClaimDocuments.POSTMORTEMREPORT;
                documentsPathNames.PostMortemReport = UploadDocument(claimDocuments.PostMortemReport, claimDocuments.EmpId, (int)ClaimDocuments.POSTMORTEMREPORT, "Claim Documents");
            }
            else
                documentsPathNames.PostMortemReport = "";

            if (claimDocuments.CourtOrderDeclaringDead != null)
            {
                documentsPathNames.CourtOrderDeclaringDeadDocTypeId = (int)ClaimDocuments.COURTORDERDECLARINGDEAD;
                documentsPathNames.CourtOrderDeclaringDead = UploadDocument(claimDocuments.CourtOrderDeclaringDead, claimDocuments.EmpId, (int)ClaimDocuments.COURTORDERDECLARINGDEAD, "Claim Documents");
            }
            else
                documentsPathNames.CourtOrderDeclaringDead = "";

            if (claimDocuments.SuccessionCertificate != null)
            {
                documentsPathNames.SuccessionCertificateDocTypeId = (int)ClaimDocuments.SUCCESSIONCERTIFICATE;
                documentsPathNames.SuccessionCertificate = UploadDocument(claimDocuments.SuccessionCertificate, claimDocuments.EmpId, (int)ClaimDocuments.SUCCESSIONCERTIFICATE, "Claim Documents");
            }
            else
                documentsPathNames.SuccessionCertificate = "";

            if (claimDocuments.ConsentLetter != null)
            {
                documentsPathNames.ConsentLetterDocTypeId = (int)ClaimDocuments.CONSENTLETTER;
                documentsPathNames.ConsentLetter = UploadDocument(claimDocuments.ConsentLetter, claimDocuments.EmpId, (int)ClaimDocuments.CONSENTLETTER, "Claim Documents");
            }
            else
                documentsPathNames.ConsentLetter = "";

            if (claimDocuments.CourtJudgment != null)
            {
                documentsPathNames.CourtJudgmentDocTypeId = (int)ClaimDocuments.COURTJUDGMENT;
                documentsPathNames.CourtJudgment = UploadDocument(claimDocuments.CourtJudgment, claimDocuments.EmpId, (int)ClaimDocuments.COURTJUDGMENT, "Claim Documents");
            }
            else
                documentsPathNames.CourtJudgment = "";

            if (claimDocuments.GuardianshipCertificate != null)
            {
                documentsPathNames.GuardianshipCertificateDocTypeId = (int)ClaimDocuments.GUARDIANSHIPCERTIFICATE;
                documentsPathNames.GuardianshipCertificate = UploadDocument(claimDocuments.GuardianshipCertificate, claimDocuments.EmpId, (int)ClaimDocuments.GUARDIANSHIPCERTIFICATE, "Claim Documents");
            }
            else
                documentsPathNames.GuardianshipCertificate = "";

            if (claimDocuments.MarriageCertificate != null)
            {
                documentsPathNames.MarriageCertificateDocTypeId = (int)ClaimDocuments.MARRIAGECERTIFICATE;
                documentsPathNames.MarriageCertificate = UploadDocument(claimDocuments.MarriageCertificate, claimDocuments.EmpId, (int)ClaimDocuments.MARRIAGECERTIFICATE, "Claim Documents");
            }
            else
                documentsPathNames.MarriageCertificate = "";

            int counter = 1;
            foreach (var item in claimDocuments.OtherDocuments)
            {
                documentsPathNames.OtherDocumentPaths.Add(UploadDocument(item, claimDocuments.EmpId, (int)ClaimDocuments.OTHERDOCUMENT, "Claim Documents", counter));
                counter++;
            }

            var result = claims.InitiateDeathClaimApplication(documentsPathNames);
            return result;
        }

        public string SaveApplicationDetails(VM_EmpDetailForDeathClaimApplication empDetail)
        {
            return claims.SaveApplicationDetails(empDetail);
        }

        public VM_ClaimRequiredDocuments GetAdditionalDocumentsList(long empId)
        {
            return claims.GetAdditionalDocumentsList(empId);
        }

        private string UploadDocument(HttpPostedFileBase document, long empId, int docId, string docType, int? otherDocumentId = null)
        {
            string subPath = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileNameWithExtention = Path.GetFileName(document.FileName);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithExtention);
                string fileName = string.Empty;
                if (otherDocumentId.HasValue)
                {
                    fileName = fileNameWithoutExtension + docId.ToString() + otherDocumentId.ToString() + DateTime.Now.Ticks + Path.GetExtension(fileNameWithExtention);
                }
                else
                {
                    fileName = fileNameWithoutExtension + docId.ToString() + DateTime.Now.Ticks + Path.GetExtension(fileNameWithExtention);
                }                
                subPath = "/Documents/" + empId.ToString() + "/" + docType;
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(subPath));
                if (!exists)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                }

                string path = Path.Combine(HttpContext.Current.Server.MapPath(subPath), fileName);
                document.SaveAs(path);
                subPath = subPath + "/" + fileName;
            }

            return subPath;
        }
    }
}
