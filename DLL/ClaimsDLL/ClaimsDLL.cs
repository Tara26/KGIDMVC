using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DLL.DBConnection;
using KGID_Models.Claim;

namespace DLL.ClaimsDLL
{
    public class ClaimsDLL : IClaimsDLL
    {
        private readonly Common_Connection _Conn = new Common_Connection();

        public VM_ClaimEmployeeDetail GetMaturityClaimEmployeeDetails(long empId)
        {
            VM_ClaimEmployeeDetail claimEmployeeDetail = null;

            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employeeId", empId)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_claims_getMaturityClaimDetails");
                if (dsClaims.Tables[0].Rows.Count > 0)
                {
                    claimEmployeeDetail = new VM_ClaimEmployeeDetail();
                    foreach (DataRow row in dsClaims.Tables[0].Rows)
                    {
                        VM_ClaimDetail claimDetail = new VM_ClaimDetail();
                        claimDetail.PolicyNumber = row["PolicyNumber"].ToString();
                        claimDetail.SumAssured = Convert.ToDecimal(row["SumAssured"].ToString());
                        claimDetail.IsBondReceived = Convert.ToBoolean(row["IsBondReceived"].ToString());
                        claimDetail.NetAmount = Convert.ToDecimal(row["NetAmount"].ToString());
                        claimDetail.PayableAmount = Convert.ToDecimal(row["PayableAmount"].ToString());
                        claimDetail.UnpaidLoanPremium = Convert.ToDecimal(row["UnpaidLoanPremium"].ToString());
                        claimDetail.UnpaidPolicyPremium = Convert.ToDecimal(row["UnpaidPolicyPremium"].ToString());
                        claimDetail.BonusAmount = Convert.ToDecimal(row["BonusAmount"].ToString());
                        //claimDetail.IsBondReceived = Convert.ToBoolean(row["IsBondReceived"].ToString());

                        ///TODO: Add additional fields for maturity claims

                        claimEmployeeDetail.ClaimDetails.Add(claimDetail);
                    }
                }

                if (dsClaims.Tables[1].Rows.Count > 0 && claimEmployeeDetail.ClaimDetails.Count > 0)
                {
                    claimEmployeeDetail.EmpName = dsClaims.Tables[1].Rows[0]["EmpName"].ToString();
                    claimEmployeeDetail.EmpDesignation = dsClaims.Tables[1].Rows[0]["EmpDesignation"].ToString();
                    claimEmployeeDetail.EmpDepartment = dsClaims.Tables[1].Rows[0]["EmpDepartment"].ToString();
                }

            }
            catch (Exception ex)
            {
            }

            return claimEmployeeDetail;
        }

        public VM_ClaimApplications GetMaturityClaimApplications(int empType)
        {
            VM_ClaimApplications maturityClaimApplications = null;

            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                     new SqlParameter("@emptype", empType)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_claims_getMaturityClaimApplications");
                if (dsClaims.Tables[0].Rows.Count > 0)
                {
                    maturityClaimApplications = new VM_ClaimApplications();
                    foreach (DataRow row in dsClaims.Tables[0].Rows)
                    {
                        VM_ClaimApplication claimApplication = new VM_ClaimApplication();
                        claimApplication.ClaimRefNumber = row["ClaimRefNumber"].ToString();
                        claimApplication.ClaimApplicationId = Convert.ToInt64(row["ClaimApplicationId"].ToString());
                        claimApplication.EmpId = Convert.ToInt64(row["EmpId"].ToString());
                        claimApplication.EmpName = row["EmpName"].ToString();
                        claimApplication.MaturityDate = Convert.ToDateTime(row["MaturityDate"].ToString());
                        maturityClaimApplications.Claims.Add(claimApplication);
                    }
                }
            }
            catch (Exception)
            {
            }

            return maturityClaimApplications;
        }

        public VM_ClaimEmployeeDetail GetPreMaturityClaimEmployeeDetails(long empId)
        {
            VM_ClaimEmployeeDetail claimEmployeeDetail = null;

            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employeeId", empId)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_claims_getPrematurityClaimDetails");
                if (dsClaims.Tables[0].Rows.Count > 0)
                {
                    claimEmployeeDetail = new VM_ClaimEmployeeDetail();
                    foreach (DataRow row in dsClaims.Tables[0].Rows)
                    {
                        VM_ClaimDetail claimDetail = new VM_ClaimDetail();
                        claimDetail.PolicyNumber = row["PolicyNumber"].ToString();
                        claimDetail.SumAssured = Convert.ToDecimal(row["SumAssured"].ToString());
                        claimDetail.IsBondReceived = Convert.ToBoolean(row["IsBondReceived"].ToString());
                        claimDetail.NetAmount = Convert.ToDecimal(row["NetAmount"].ToString());
                        claimDetail.PayableAmount = Convert.ToDecimal(row["PayableAmount"].ToString());
                        claimDetail.UnpaidLoanPremium = Convert.ToDecimal(row["UnpaidLoanPremium"].ToString());
                        claimDetail.UnpaidPolicyPremium = Convert.ToDecimal(row["UnpaidPolicyPremium"].ToString());
                        claimDetail.BonusAmount = Convert.ToDecimal(row["BonusAmount"].ToString());

                        ///TODO: Add additional fields for maturity claims

                        claimEmployeeDetail.ClaimDetails.Add(claimDetail);
                    }
                }

                if (dsClaims.Tables[1].Rows.Count > 0 && claimEmployeeDetail.ClaimDetails.Count > 0)
                {
                    claimEmployeeDetail.EmpName = dsClaims.Tables[1].Rows[0]["EmpName"].ToString();
                    claimEmployeeDetail.EmpDesignation = dsClaims.Tables[1].Rows[0]["EmpDesignation"].ToString();
                    claimEmployeeDetail.EmpDepartment = dsClaims.Tables[1].Rows[0]["EmpDepartment"].ToString();
                }

                if (dsClaims.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in dsClaims.Tables[2].Rows)
                    {
                        VM_ClaimDocument claimDocument = new VM_ClaimDocument();
                        var filePath = row["DocumentPath"].ToString();
                        claimDocument.DocumentFileName = Path.GetFileNameWithoutExtension(filePath);
                        claimDocument.DocumentPath = filePath;
                        claimDocument.DocumentType = row["DocumentType"].ToString();
                        claimEmployeeDetail.ClaimDocuments.Add(claimDocument);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return claimEmployeeDetail;
        }

        public VM_ClaimApplications GetPrematurityClaimApplications(int claimTypeId)
        {
            VM_ClaimApplications maturityClaimApplications = null;

            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@claimType", claimTypeId)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_claims_getPrematurityClaimApplications");
                if (dsClaims.Tables[0].Rows.Count > 0)
                {
                    maturityClaimApplications = new VM_ClaimApplications();
                    foreach (DataRow row in dsClaims.Tables[0].Rows)
                    {
                        VM_ClaimApplication claimApplication = new VM_ClaimApplication();
                        claimApplication.ClaimRefNumber = row["ClaimRefNumber"].ToString();
                        claimApplication.EmpId = Convert.ToInt64(row["EmpId"].ToString());
                        claimApplication.EmpName = row["EmpName"].ToString();
                        claimApplication.MaturityDate = Convert.ToDateTime(row["MaturityDate"].ToString());

                        maturityClaimApplications.Claims.Add(claimApplication);
                    }
                }
            }
            catch (Exception)
            {
            }

            return maturityClaimApplications;
        }

        public string SaveVerifiedDetailsDll(VM_ClaimEmployeeDetail objVerification)
        {
            string returnString = string.Empty;

            try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@employee_id",objVerification.EmpCode),
                    new SqlParameter("@cw_claim_application_id",objVerification.ApplicationRefNo),
                    new SqlParameter("@cw_verified_by",objVerification.CreatedBy),
                    new SqlParameter("@cw_checklist_verification_status",objVerification.VerifyProposerDetails),
                    new SqlParameter("@cw_remarks",objVerification.Remarks),
                    new SqlParameter("@cw_comments",objVerification.Comments),
                    new SqlParameter("@cw_application_status",objVerification.ApplicationStatus),
                };

                returnString = _Conn.ExecuteCmd(sqlparam, "sp_claims_InsertClaimWorkflowDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnString;
        }

        public IList<VM_ClaimWorkflowDetail> GetClaimWorkFlowDetails(long applicationId)
        {
            IList<VM_ClaimWorkflowDetail> workflowDetails = null;

            try
            {
                DataSet dsCWD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@applicationId",applicationId)
                };

                dsCWD = _Conn.ExeccuteDataset(sqlparam, "sp_claims_getClaimWorkflowDetails");

                if (dsCWD.Tables.Count > 0 && dsCWD.Tables[0].Rows.Count > 0)
                {
                    workflowDetails = new List<VM_ClaimWorkflowDetail>();
                    foreach (DataRow dr in dsCWD.Tables[0].Rows)
                    {
                        VM_ClaimWorkflowDetail workflowDetail = new VM_ClaimWorkflowDetail();
                        workflowDetail.ApplicationRefNo = dr["ApplicationRefNo"].ToString();
                        workflowDetail.From = dr["From"].ToString();
                        workflowDetail.To = dr["To"].ToString();
                        workflowDetail.Remarks = dr["Remarks"].ToString();
                        workflowDetail.Comments = dr["Comments"].ToString();
                        workflowDetail.CreationDateTime = dr["CreationDateTime"].ToString();
                        workflowDetail.ApplicationStatus = dr["ApplicationStatus"].ToString();
                        workflowDetails.Add(workflowDetail);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            //if (workflowDetails.Count > 0)
            //{

            //    return workflowDetails.OrderByDescending(t => t.CreationDateTime).ToList();

            //}
            //else
            //{
            return workflowDetails;
            //}

        }

        public VM_EmpDetailForPrematureClaimApplication GetEmployeeDetailByKGIDNumber(string kgidNumber)
        {
            VM_EmpDetailForPrematureClaimApplication empDetailForPreMatureClaimApplication = null;

            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@kgidNumber", kgidNumber)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_claims_getEmpDetailForPrematureClaimApplication");

                if (dsClaims.Tables[0].Rows.Count > 0)
                {
                    empDetailForPreMatureClaimApplication = new VM_EmpDetailForPrematureClaimApplication();
                    empDetailForPreMatureClaimApplication.Name = dsClaims.Tables[0].Rows[0]["EmpName"].ToString();
                    empDetailForPreMatureClaimApplication.Designation = dsClaims.Tables[0].Rows[0]["EmpDesignation"].ToString();
                    empDetailForPreMatureClaimApplication.Department = dsClaims.Tables[0].Rows[0]["EmpDepartment"].ToString();
                    empDetailForPreMatureClaimApplication.Id = Convert.ToInt64(dsClaims.Tables[0].Rows[0]["EmpId"].ToString());
                    empDetailForPreMatureClaimApplication.Age = Convert.ToInt32(dsClaims.Tables[0].Rows[0]["Age"].ToString());
                }
            }
            catch (Exception ex)
            {
            }

            return empDetailForPreMatureClaimApplication;
        }

        public string ForwardApplicationToCaseworker(VM_EmpDetailForPrematureClaimApplication empDetail)
        {
            string response = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                        {
                    new SqlParameter("@employeeId", empDetail.Id),
                    new SqlParameter("@claimTypeId", empDetail.ClaimTypeId),
                    new SqlParameter("@claimSubTypeId", empDetail.ClaimSubTypeId),
                    new SqlParameter("@createdBy", empDetail.DDOUserId),
                    new SqlParameter("@createdDateTime", DateTime.Now),
                    new SqlParameter("@updatedBy", empDetail.DDOUserId),
                    new SqlParameter("@updatedDateTime",  DateTime.Now),

                    new SqlParameter("@relievingLetterPath", empDetail.RelievingLetterDocPath),
                    new SqlParameter("@resignationAcceptanceLetterPath", empDetail.ResignationAcceptanceLetterDocPath),
                    new SqlParameter("@resignationLetterPath", empDetail.ResignationLetterDocPath),
                    new SqlParameter("@terminationLetterPath", empDetail.TerminationLetterDocPath),

                    new SqlParameter("@relievingLetterDocTypeId", empDetail.RelievingLetterDocTypeId),
                    new SqlParameter("@resignationAcceptanceLetterDocTypeId", empDetail.ResignationAcceptanceLetterDocTypeId),
                    new SqlParameter("@resignationLetterDocTypeId", empDetail.ResignationLetterDocTypeId),
                    new SqlParameter("@terminationLetterDocTypeId", empDetail.TerminationLetterDocTypeId)
                };

                response = _Conn.ExecuteCmd(sqlparam, "sp_claims_initiatePrematureClaimApplication");
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public VM_ClaimSubTypes GetClaimSubTypes(int claimType)
        {
            VM_ClaimSubTypes claimSubTypes = null;
            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@claimTypeId", claimType)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_claims_getClaimSubTypes");
                if (dsClaims.Tables.Count > 0 && dsClaims.Tables[0].Rows.Count > 0)
                {
                    claimSubTypes = new VM_ClaimSubTypes();
                    foreach (DataRow rowItem in dsClaims.Tables[0].Rows)
                    {
                        claimSubTypes.ClaimSubTypes.Add(new VM_ClaimSubType()
                        {
                            Id = Convert.ToInt32(rowItem["Id"].ToString()),
                            ClaimSubType = rowItem["ClaimSubType"].ToString()
                        });
                    }
                }
            }
            catch (Exception)
            {
            }

            return claimSubTypes;
        }

        public bool InitiateDeathClaimApplication(VM_ClaimDocumentsPathNames docNames)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt_OtherClaimDocuments = new DataTable();
                dt_OtherClaimDocuments.Columns.Add("FilePath");
                foreach (var objDocFilePath in docNames.OtherDocumentPaths)
                {
                    DataRow dr = dt_OtherClaimDocuments.NewRow();
                    dr["FilePath"] = objDocFilePath;
                    dt_OtherClaimDocuments.Rows.Add(dr);
                }

                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@VerifiedByEmpId",docNames.VerifiedByEmpId),
                    new SqlParameter("@EmpId",docNames.EmpId),

                    new SqlParameter("@DeathCertificate",docNames.DeathCertificate),
                    new SqlParameter("@ClaimApplication",docNames.ClaimApplication),
                    new SqlParameter("@PolicyBonds",docNames.PolicyBonds),
                    new SqlParameter("@MedicalAttendanceReport",docNames.MedicalAttendanceReport),
                    new SqlParameter("@SurvivalCertificate",docNames.SurvivalCertificate),
                    new SqlParameter("@LeavesAvailed",docNames.LeavesAvailed),
                    new SqlParameter("@MedicalReimbursement",docNames.MedicalReimbursement),
                    new SqlParameter("@DeathSummary",docNames.DeathSummary),
                    new SqlParameter("@DischargeSummary",docNames.DischargeSummary),
                    new SqlParameter("@PoliceReport",docNames.PoliceReport),
                    new SqlParameter("@FIR",docNames.FIR),
                    new SqlParameter("@PostMortemReport",docNames.PostMortemReport),
                    new SqlParameter("@CourtOrderDeclaringDead",docNames.CourtOrderDeclaringDead),
                    new SqlParameter("@SuccessionCertificate",docNames.SuccessionCertificate),
                    new SqlParameter("@ConsentLetter",docNames.ConsentLetter),
                    new SqlParameter("@CourtJudgment",docNames.CourtJudgment),
                    new SqlParameter("@GuardianshipCertificate",docNames.GuardianshipCertificate),
                    new SqlParameter("@MarriageCertificate", docNames.MarriageCertificate),

                    new SqlParameter("@DeathCertificateDocTypeId", docNames.DeathCertificateDocTypeId),
                    new SqlParameter("@ClaimApplicationDocTypeId", docNames.ClaimApplicationDocTypeId),
                    new SqlParameter("@PolicyBondsDocTypeId", docNames.PolicyBondsDocTypeId),
                    new SqlParameter("@MedicalAttendanceReportDocTypeId", docNames.MedicalAttendanceReportDocTypeId),
                    new SqlParameter("@SurvivalCertificateDocTypeId", docNames.SurvivalCertificateDocTypeId),
                    new SqlParameter("@LeavesAvailedDocTypeId", docNames.LeavesAvailedDocTypeId),
                    new SqlParameter("@MedicalReimbursementDocTypeId", docNames.MedicalReimbursementDocTypeId),
                    new SqlParameter("@DeathSummaryDocTypeId", docNames.DeathSummaryDocTypeId),
                    new SqlParameter("@DischargeSummaryDocTypeId", docNames.DischargeSummaryDocTypeId),
                    new SqlParameter("@PoliceReportDocTypeId", docNames.PoliceReportDocTypeId),
                    new SqlParameter("@FIRDocTypeId", docNames.FIRDocTypeId),
                    new SqlParameter("@PostMortemReportDocTypeId", docNames.PostMortemReportDocTypeId),
                    new SqlParameter("@CourtOrderDeclaringDeadDocTypeId", docNames.CourtOrderDeclaringDeadDocTypeId),
                    new SqlParameter("@SuccessionCertificateDocTypeId", docNames.SuccessionCertificateDocTypeId),
                    new SqlParameter("@ConsentLetterDocTypeId", docNames.ConsentLetterDocTypeId),
                    new SqlParameter("@CourtJudgmentDocTypeId", docNames.CourtJudgmentDocTypeId),
                    new SqlParameter("@GuardianshipCertificateDocTypeId", docNames.GuardianshipCertificateDocTypeId),
                    new SqlParameter("@MarriageCertificateDocTypeId", docNames.MarriageCertificateDocTypeId),

                    new SqlParameter("@OtherDocuments", dt_OtherClaimDocuments)
                };

                returnString = _Conn.ExecuteCmd(sqlparam, "sp_claims_saveDocumentsForDeathClaimApplication");
                if (returnString == "1") return true;
                else return false;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public VM_ClaimEmployeeDetail GetDeathClaimEmployeeDetails(long empId)
        {
            VM_ClaimEmployeeDetail claimEmployeeDetail = null;

            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employeeId", empId)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_claims_getDeathClaimDetails");
                if (dsClaims.Tables[0].Rows.Count > 0)
                {
                    claimEmployeeDetail = new VM_ClaimEmployeeDetail();
                    foreach (DataRow row in dsClaims.Tables[0].Rows)
                    {
                        VM_ClaimDetail claimDetail = new VM_ClaimDetail();
                        claimDetail.PolicyNumber = row["PolicyNumber"].ToString();
                        claimDetail.SumAssured = Convert.ToDecimal(row["SumAssured"].ToString());
                        claimDetail.IsBondReceived = Convert.ToBoolean(row["IsBondReceived"].ToString());
                        claimDetail.NetAmount = Convert.ToDecimal(row["NetAmount"].ToString());
                        claimDetail.PayableAmount = Convert.ToDecimal(row["PayableAmount"].ToString());
                        claimDetail.UnpaidLoanPremium = Convert.ToDecimal(row["UnpaidLoanPremium"].ToString());
                        claimDetail.UnpaidPolicyPremium = Convert.ToDecimal(row["UnpaidPolicyPremium"].ToString());
                        claimDetail.BonusAmount = Convert.ToDecimal(row["BonusAmount"].ToString());

                        ///TODO: Add additional fields for maturity claims

                        claimEmployeeDetail.ClaimDetails.Add(claimDetail);
                    }
                }

                if (dsClaims.Tables[1].Rows.Count > 0 && claimEmployeeDetail.ClaimDetails.Count > 0)
                {
                    claimEmployeeDetail.EmpName = dsClaims.Tables[1].Rows[0]["EmpName"].ToString();
                    claimEmployeeDetail.EmpDesignation = dsClaims.Tables[1].Rows[0]["EmpDesignation"].ToString();
                    claimEmployeeDetail.EmpDepartment = dsClaims.Tables[1].Rows[0]["EmpDepartment"].ToString();
                }

                if (dsClaims.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in dsClaims.Tables[2].Rows)
                    {
                        VM_ClaimDocument claimDocument = new VM_ClaimDocument();
                        var filePath = row["DocumentPath"].ToString();
                        claimDocument.DocumentFileName = Path.GetFileNameWithoutExtension(filePath);
                        claimDocument.DocumentPath = filePath;
                        claimDocument.DocumentType = row["DocumentType"].ToString();
                        claimEmployeeDetail.ClaimDocuments.Add(claimDocument);
                    }                    
                }

            }
            catch (Exception ex)
            {
            }

            return claimEmployeeDetail;
        }

        public VM_ClaimApplications GetClaimApplications(int empType, int claimType, long loggedInUserId)
        {
            VM_ClaimApplications maturityClaimApplications = null;

            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                     new SqlParameter("@emptype", empType),
                     new SqlParameter("@claimtype", claimType),
                     new SqlParameter("@loggedInUserId", loggedInUserId)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_claims_getClaimApplications");
                if (dsClaims.Tables[0].Rows.Count > 0)
                {
                    maturityClaimApplications = new VM_ClaimApplications();
                    foreach (DataRow row in dsClaims.Tables[0].Rows)
                    {
                        VM_ClaimApplication claimApplication = new VM_ClaimApplication();
                        claimApplication.ClaimRefNumber = row["ClaimRefNumber"].ToString();
                        claimApplication.ClaimApplicationId = Convert.ToInt64(row["ClaimApplicationId"].ToString());
                        claimApplication.EmpId = Convert.ToInt64(row["EmpId"].ToString());
                        claimApplication.EmpName = row["EmpName"].ToString();
                        claimApplication.MaturityDate = Convert.ToDateTime(row["MaturityDate"].ToString());
                        maturityClaimApplications.Claims.Add(claimApplication);
                    }
                }

                if (dsClaims.Tables[1].Rows.Count > 0)
                {
                    if (dsClaims.Tables[1].Rows.Count == 1)
                    {
                        maturityClaimApplications.TotalReceived = Convert.ToInt64(dsClaims.Tables[1].Rows[0]["ApplicationCount"]);
                        maturityClaimApplications.SentBackApplication = 0;
                    }
                    else
                    {
                        maturityClaimApplications.TotalReceived = Convert.ToInt64(dsClaims.Tables[1].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsClaims.Tables[1].Rows[1]["ApplicationCount"]);
                        maturityClaimApplications.SentBackApplication = Convert.ToInt64(dsClaims.Tables[1].Rows[1]["ApplicationCount"]);
                    }

                    maturityClaimApplications.ForwardedApplications = Convert.ToInt64(dsClaims.Tables[1].Rows[0]["ApplicationCount"]);
                    maturityClaimApplications.PendingApplications = Convert.ToInt64(dsClaims.Tables[1].Rows[0]["Pending"]);
                }
            }
            catch (Exception)
            {
            }

            return maturityClaimApplications;
        }

        public string SaveApplicationDetails(VM_EmpDetailForDeathClaimApplication empDetail)
        {
            string returnString = string.Empty;

            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@DateOfDeath",empDetail.DateOfDeath),
                    new SqlParameter("@CauseOfDeath",empDetail.CauseOfDeath),
                    new SqlParameter("@IsMarried",empDetail.IsMarried),
                    new SqlParameter("@IsNomineeAlive",empDetail.IsNomineeAlive),
                    new SqlParameter("@NameOfNominee",empDetail.NameOfNominee),
                    new SqlParameter("@DoctorName",empDetail.DoctorName),
                    new SqlParameter("@DoctorDetail",empDetail.DoctorDetail),
                    new SqlParameter("@AgeOfNominee",empDetail.AgeOfNominee),
                    new SqlParameter("@NomineeRelation",empDetail.NomineeRelation),
                    new SqlParameter("@NameOfGaurrdian",empDetail.NameOfGaurrdian),
                    new SqlParameter("@DetailsOfGaurdian",empDetail.DetailsOfGaurdian),
                    new SqlParameter("@ClaimSubTypeId",empDetail.ClaimSubTypeId),
                    new SqlParameter("@VerifiedByEmpId",empDetail.VerifiedByEmpId),
                    new SqlParameter("@EmpId",empDetail.Id),
                    new SqlParameter("@Today", DateTime.Now)
                };

                returnString = _Conn.ExecuteCmd(sqlparam, "sp_claims_InsertDeathClaimApplicationDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnString;
        }

        public VM_ClaimRequiredDocuments GetAdditionalDocumentsList(long empId)
        {
            VM_ClaimRequiredDocuments claimRequiredDocuments = null;

            try
            {
                DataSet dsClaims = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId", empId)
                };

                dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_claims_getAdditionalDocumentList");
                if (dsClaims.Tables[0].Rows.Count > 0)
                {
                    claimRequiredDocuments = new VM_ClaimRequiredDocuments();

                    bool beforeMarriedStatus = Convert.ToBoolean(dsClaims.Tables[0].Rows[0]["MarriedStatusFromEmpDetail"].ToString());
                    bool marriedStatusAtClaim = Convert.ToBoolean(dsClaims.Tables[0].Rows[0]["IsMarried"].ToString());

                    if (!beforeMarriedStatus && marriedStatusAtClaim)
                    {
                        claimRequiredDocuments.IsMarriedAfterPolicyApplication = true;
                    }

                    claimRequiredDocuments.IsNomineeAlive = Convert.ToBoolean(dsClaims.Tables[0].Rows[0]["IsNomineeAlive"].ToString());
                    claimRequiredDocuments.IsNomineeMinor = Convert.ToInt32(dsClaims.Tables[0].Rows[0]["NomineeAge"].ToString()) < 18 ? true : false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return claimRequiredDocuments;
        }
    }
}
