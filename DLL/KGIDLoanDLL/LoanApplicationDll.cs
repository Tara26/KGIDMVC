using DLL.DBConnection;
using KGID_Models.KGIDLoan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Configuration;

namespace DLL.KGIDLoanDLL
{
    public class LoanApplicationDll : ILoanApplicationDll
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();

        #region GetLoanApplicationListForAll
        public VM_LoanVerificationDetails GetLoanApplicationListForAll(long empId, int UserRole)
        {
            //List<VM_LoanApplicationListForAll> loanApplicationList = new List<VM_LoanApplicationListForAll>();
            VM_LoanVerificationDetails loanApplication = new VM_LoanVerificationDetails();
            try
            {
                DataSet dsLDDO = new DataSet();

                SqlParameter[] sqlparam =
                {
                new SqlParameter("@EType",UserRole),
                new SqlParameter("@employee_id",empId)
            };

                dsLDDO = _Conn.ExeccuteDataset(sqlparam, "sp_loan_selectLoanDepartmentWorkflowDetails");

                var EmployeeVerification = dsLDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeloanVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    EmployeeDesignation = dataRow.Field<string>("EmployeeDesignation"),
                    ApplicationNumber = dataRow.Field<string>("la_loan_ref_no"),
                    ApplicationId = dataRow.Field<long>("la_loan_application_id"),
                    Remarks = dataRow.Field<string>("Remarks"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();

                var LastUpdatedStatus = dsLDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeloanVerificationDetail
                { 
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("la_loan_ref_no"),
                    Status = dataRow.Field<string>("asm_status_desc")
                }).ToList();

                if (UserRole == 5)
                {
                    var ApprovedStatus = dsLDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeloanVerificationDetail
                    {
                        // EmployeeCode = dataRow.Field<long>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = dataRow.Field<string>("p_kgid_policy_number"),
                        Status = dataRow.Field<string>("asm_status_desc"),
                        // LastUpdatedDate = dataRow.Field<string>("law_creation_datetime"),
                        //ApplicationId = dataRow.Field<long>("la_loan_ref_no"),
                        // Premium = dataRow.Field<string>("p_premium")
                    }).ToList();
                    loanApplication.ApprovedEmployeeStatus = ApprovedStatus;
                }


                loanApplication.EmployeeLoanVerificationDetails = EmployeeVerification;
                loanApplication.LastUpdatedStatusForLoan = LastUpdatedStatus;
               
                if (dsLDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsLDDO.Tables[2].Rows.Count == 1)
                    {
                        loanApplication.TotalReceived = Convert.ToInt64(dsLDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        loanApplication.SentBackApplication = 0;
                    }
                    else
                    {
                        loanApplication.TotalReceived = Convert.ToInt64(dsLDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsLDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        loanApplication.SentBackApplication = Convert.ToInt64(dsLDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsLDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsLDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    loanApplication.ForwardedApplications = Convert.ToInt64(dsLDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsLDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    loanApplication.PendingApplications = Convert.ToInt64(dsLDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch(Exception ex)
            {

            }
            return loanApplication;
        }

        public VM_LoanApplicationModel GetLoanApplicationData(long empId, long loanApplicationId)
        {
            Int32? CustomParseInt(object value)
            {
                if (value == null || value is DBNull) return 0;
                return Convert.ToInt32(value);
            }
            VM_LoanApplicationModel loanApplicationData = new VM_LoanApplicationModel();
            loanApplicationData.PolicyData = new List<VM_LoanDetailsModel>();
            loanApplicationData.WorkFlowData = new List<VM_LoanWorkFlow>();
            loanApplicationData.HRMSPayDetails = new List<VM_HRMSPayDetails>();
            DataSet dsBD = new DataSet();

            SqlParameter[] sqlparam =
            {
                new SqlParameter("@employee_id",empId),
                new SqlParameter("@loan_application_id",loanApplicationId)
            };

            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_loan_getLoanApplicationWorkflowData");

            if (dsBD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsBD.Tables[0].Rows)
                {
                    VM_LoanDetailsModel loanDetail = new VM_LoanDetailsModel();
                    loanDetail.PolicyID = dr.Field<long?>("lpm_policy_id").Value;
                    loanDetail.PolicyNumber = dr.Field<string>("p_kgid_policy_number");
                    loanDetail.DateOfSanction = dr.Field<DateTime?>("p_sanction_date").Value;
                    loanDetail.SumAssured = float.Parse(dr["p_sum_assured"].ToString());
                    loanDetail.PremiumAmount = float.Parse(dr["p_premium"].ToString());
                    loanDetail.EnterAmount = dr.Field<int?>("lpm_applied_amount").Value;
                    loanApplicationData.PolicyData.Add(loanDetail);
                }
            }

            if (dsBD.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in dsBD.Tables[1].Rows)
                {
                    loanApplicationData.LoanReferenceNo = dr.Field<string>("la_loan_ref_no");
                    loanApplicationData.LoanApplicationId = dr.Field<long>("ld_application_id");
                    loanApplicationData.EmployeeName = dr.Field<string>("employee_name");
                    //loanApplicationData.PurposeId = dr.Field<int?>("la_loan_purpose_id").Value;
                    loanApplicationData.PurposeName = dr.Field<string>("lp_purpose_desc");
                    loanApplicationData.FamilyRelationId = dr.Field<int?>("la_loan_relation_id").Value;
                    loanApplicationData.FamilyRelationDesc = dr.Field<string>("lfr_loan_relation_desc");
                    loanApplicationData.FamilyRelationName = dr.Field<string>("lfr_family_name");
                    loanApplicationData.FamilyRelationAge = CustomParseInt(dr["lfr_family_age"]).Value;

                    //loanApplicationData.DateOfApplication = dr.Field<DateTime?>("la_date_of_application").Value;
                    loanApplicationData.EnteredAmount = dr.Field<decimal>("ld_applied_amount");
                    loanApplicationData.Deductions = dr.Field<decimal>("ld_loan_deduction");
                    loanApplicationData.NetAmount = dr.Field<decimal>("ld_net_amount");
                    loanApplicationData.NoOfPrincipleInstallments = dr.Field<int?>("ld_principle_months").Value;
                    loanApplicationData.NoOfIntrestInstallments = dr.Field<int?>("ld_interest_months").Value;
                    loanApplicationData.PrincipleInstallmentAmt = dr.Field<decimal?>("ld_month_wise_principal").Value;
                    loanApplicationData.InterestInstallmentAmt = dr.Field<decimal?>("ld_month_wise_intrest").Value;
                }
            }
            if (dsBD.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in dsBD.Tables[2].Rows)
                {
                    VM_LoanWorkFlow workFlow = new VM_LoanWorkFlow();
                    workFlow.law_applicant_name = dr.Field<string>("employee_name");
                    workFlow.law_application_status = dr.Field<string>("asm_status_desc");
                    workFlow.law_comments = dr.Field<string>("law_comments");
                    workFlow.law_remarks = dr.Field<string>("law_remarks");
                    workFlow.law_reference_no = dr.Field<string>("la_loan_ref_no");
                    workFlow.law_verified_by = dr.Field<long>("law_verified_by");
                    workFlow.From = dr.Field<string>("From");
                    workFlow.To = dr.Field<string>("To");
                    workFlow.CreationDateTime = dr.Field<string>("CreationDateTime");
                    loanApplicationData.WorkFlowData.Add(workFlow);
                }
            }
            if (dsBD.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow dr in dsBD.Tables[3].Rows)
                {
                    VM_HRMSPayDetails hrmsData = new VM_HRMSPayDetails();

                    if (dr["payscaletype"].ToString() == "paydocument")
                    {
                        hrmsData.payscaledocument = dr["ld_upload_document_path"].ToString();
                        hrmsData.payscaletype = dr["payscaletype"].ToString();
                        loanApplicationData.HRMSPayDetails.Add(hrmsData);
                    }
                    else
                    {
                        hrmsData.hrms_hrms_pay_id = dr.Field<long?>("hrms_hrms_pay_id").Value;
                        hrmsData.hrms_emp_id = dr.Field<long?>("hrms_emp_id").Value;
                        hrmsData.hrms_gross_pay = CustomParseInt(dr["hrms_gross_pay"]).Value;
                        hrmsData.hrms_net_pay = CustomParseInt(dr["hrms_net_pay"]).Value;
                        hrmsData.hrms_deduction = CustomParseInt(dr["hrms_deduction"]).Value;
                        hrmsData.hrms_month = dr["mm_month_desc"].ToString();
                        hrmsData.hrms_year = dr["hrms_year_id"].ToString();
                        hrmsData.payscaletype = dr["payscaletype"].ToString();
                        loanApplicationData.HRMSPayDetails.Add(hrmsData);
                    }
                }
            }
            return loanApplicationData;
        }

        public bool LoanApplicationForward(VM_LoanWorkFlow loanApplicationForward)
        {
            try
            {
                string res;
                int status_id = Convert.ToInt32(loanApplicationForward.law_current_application_status);
                //long appliation_id = law_loan_application_id;
                DataSet dsBD = new DataSet();

                string result;

                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@law_loan_application_id",loanApplicationForward.law_loan_application_id),
                    new SqlParameter("@law_verified_by", loanApplicationForward.law_verified_by),
                    new SqlParameter("@law_remarks", loanApplicationForward.law_remarks),
                    new SqlParameter("@law_comments", loanApplicationForward.law_comments),
                    new SqlParameter("@law_checklist_verification_status", loanApplicationForward.law_checklist_verification_status),
                    new SqlParameter("@law_application_status", loanApplicationForward.law_application_status),
                    new SqlParameter("@law_created_by", loanApplicationForward.law_verified_by),
                    new SqlParameter("@law_creation_datetime", DateTime.Now),
                    new SqlParameter("@selectedcategory", loanApplicationForward.selectedcategory)
                };

                result = _Conn.ExecuteCmd(sqlparam, "sp_loan_submitLoanApplicationWorkflow");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public VM_LoanVerificationDetails GetLoanCancellationListForAll(long empId, string Type)
        {
            VM_LoanVerificationDetails loanApplication = new VM_LoanVerificationDetails();

            try
            {
                DataSet dsLDDO = new DataSet();

                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",empId),
                    new SqlParameter("@type",Type)
                };

                dsLDDO = _Conn.ExeccuteDataset(sqlparam, "sp_loan_selectLoanCancelReportDetails");

                var CancelLoanReport = dsLDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeloanVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("la_loan_ref_no"),
                    Status = dataRow.Field<string>("AppStatus"),
                    LastUpdatedDate = dataRow.Field<string>("CreationDateTime"),
                    ApplicationId = dataRow.Field<long>("la_loan_application_id"),
                    Type = Type
                }).ToList();

                loanApplication.CancelLoanReport = CancelLoanReport;
            }
            catch (Exception ex)
            {

            }
            return loanApplication;
        }

        public int NBLoanCancelRequestAction(long AppId, long EmpId, string Action)
        {
            int response = 0;
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@AppId",AppId),
                    new SqlParameter("@EmpId",EmpId),
                    new SqlParameter("@Action",Action)
                };
                response = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_loan_cancelrequestaction"));
            }
            catch (Exception ex)
            {
                response = 0;
            }
            return response;
        }

        public int GetLoanDisburseMailData(long AppId,string Type)
        {
            int result = 0;
            try
            {
                DataSet dsLDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@LoanAppID",AppId),
                    new SqlParameter("@Type",Type)
                };
                dsLDDO = _Conn.ExeccuteDataset(sqlparam, "sp_loan_GetLoanDisburseMailData");

                string ToAddr = string.Empty;
                string DisburseAmt = string.Empty;
                if (dsLDDO != null)
                {
                    foreach (DataRow dataRow in dsLDDO.Tables[0].Rows)
                    {
                        ToAddr = dataRow[0].ToString() + "," + dataRow[1].ToString();
                        DisburseAmt = dataRow[2].ToString();
                    }
                }
                string Subject = "Loan Disburse Amount";
                string Body = "Loan Application ID - " + AppId + " <br/> Disburse Amount - " + DisburseAmt;
                string SendMailID = SendEmail(ToAddr, Body, Subject);
                result = 1;
            }
            catch(Exception ex)
            {
                result = 0;
            }
            return result;
        }
        #endregion
        public string SendEmail(string to_Address, string Body, string Subject)
        {
            try
            {
                string emailID = WebConfigurationManager.AppSettings["emailID"];
                string Password = WebConfigurationManager.AppSettings["Password"];
                string AppHost = WebConfigurationManager.AppSettings["Host"];
                var AppPort = (WebConfigurationManager.AppSettings["Port"]);

                string subject = Subject;
                string body = Body;

                var smtp = new SmtpClient
                {

                    Host = AppHost,
                    Port = int.Parse(AppPort),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                    Credentials = new NetworkCredential(emailID, Password)
                };

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                using (var mess = new MailMessage(emailID, to_Address)
                {
                    Subject = subject,
                    Body = body

                })
                {
                    smtp.Send(mess);
                }
                return body;
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                return message;

            }
        }
    }
}
