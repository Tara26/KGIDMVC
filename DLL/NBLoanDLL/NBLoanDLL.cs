using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using DLL.DBConnection;
using KGID_Models.KGID_Loan;

namespace DLL.NBLoanDLL
{
    public class NBLoanDLL : INBLoanDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();
        public IEnumerable<SelectListItem> GetLoanFamilyRelationList()
        {
            var types = new List<SelectListItem>();
            SelectListItem list = new SelectListItem();
            list.Text = "Select Family Member";
            list.Value = "0";
            types.Add(list);
            var dblist = (from t in _db.tbl_loan_family_relation_master
                        select (new SelectListItem { Text = t.lfr_loan_relation_desc, Value = t.lfr_loan_relation_id.ToString() })).ToList();
            types.AddRange(dblist);
            return types;
        }
        public IEnumerable<SelectListItem> GetLoanPurposeList()
        {
            var types = new List<SelectListItem>();
            SelectListItem list = new SelectListItem();
            list.Text = "Select Loan Purpose";
            list.Value = "0";
            types.Add(list);
            var dblist = (from t in _db.tbl_loan_purpose_master
                        select (new SelectListItem { Text = t.lp_purpose_desc, Value = t.lp_loan_purpose_id.ToString() })).ToList();
            types.AddRange(dblist);
            return types;
        }
        public VM_LoanApplicationForm GetLoanApplicationForm(long EmpID)
        {
            VM_LoanApplicationForm objLoanApp = new VM_LoanApplicationForm();
            try
            {
                DataSet dsDD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpID",EmpID)
                };
                dsDD = _Conn.ExeccuteDataset(sqlparam, "sp_loan_selectExistingLoanDetails");
                if (dsDD.Tables.Count > 0)
                {
                    if (dsDD.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsDD.Tables[0].Rows)
                        {
                            VM_PolicyDetails objPolicyDetails = new VM_PolicyDetails();
                            objPolicyDetails.EmpID = EmpID;
                            //objPolicyDetails.LoanAppID = Convert.ToInt16(dr["LoanAppID"]);
                            objPolicyDetails.PolicyNo = Convert.ToInt64(dr["PolicyNo"]);
                            objPolicyDetails.SumAssured = Convert.ToDecimal(dr["SumAssured"]);
                            objPolicyDetails.PremiumAmount = Convert.ToInt16(dr["PremiumAmount"]);
                            objPolicyDetails.EligibleLoanAmount = Convert.ToDecimal(dr["EligibleLoanAmount"]);
                            objPolicyDetails.LoanAmount = dr["LoanAmount"].ToString();
                            objPolicyDetails.SanctionDate = dr["SanctionDate"].ToString();
                            //objPolicyDetails.LoanBalanceAmt = dr["BalanceAmt"].ToString();
                            objLoanApp.listPolicyDetails.Add(objPolicyDetails);
                        }
                    }
                    if (dsDD.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsDD.Tables[1].Rows)
                        {
                            VM_HRMSPayDetails objPayDetails = new VM_HRMSPayDetails();
                            objPayDetails.CurrYear = Convert.ToInt16(dr["CurrYear"]);
                            objPayDetails.Deductions = Convert.ToDecimal(dr["Deductions"]);
                            objPayDetails.EmpID = Convert.ToInt32(dr["EmpID"]);
                            objPayDetails.GrossPay = Convert.ToDecimal(dr["GrossPay"]);
                            objPayDetails.NetPay = Convert.ToDecimal(dr["NetPay"]);
                            objPayDetails.PrevMonth = dr["PrevMonth"].ToString();
                            objLoanApp.listHRMSPayDetails.Add(objPayDetails);
                        }
                    }
                    if (dsDD.Tables[2].Rows.Count > 0)
                    {
                        objLoanApp.Deductions = Convert.ToDecimal(dsDD.Tables[2].Rows[0]["BalanceAmt"]);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objLoanApp;
        }
        public string GetSpouseKgidNumber(long empId)
        {
            string result = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@emp_id",empId)
                };
                result = _Conn.ExecuteCmd(sqlparam, "sp_loan_GetSpouseKgid");
            }
            catch(Exception ex)
            {

            }
            return result;
        }
        public List<VM_LoanBranchAdjustments> GetLoanBADetails(long EmpID)
        {
            List<VM_LoanBranchAdjustments> lstBADetails = new List<VM_LoanBranchAdjustments>();
            try
            {
                DataSet dsDD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpID",EmpID)
                };
                dsDD = _Conn.ExeccuteDataset(sqlparam, "sp_loan_selectBALoanDetails");
                if (dsDD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsDD.Tables[0].Rows)
                    {
                        VM_LoanBranchAdjustments obj = new VM_LoanBranchAdjustments();
                        obj.LoanDue = Convert.ToDecimal(dr["LoanDue"]);
                        obj.LoanInterestDue = Convert.ToDecimal(dr["LoanInterestDue"]);
                        obj.PremiumDue = Convert.ToDecimal(dr["PremiumDue"]);
                        obj.PremiumInterestDue = Convert.ToDecimal(dr["PremiumInterestDue"]);
                        obj.Month = dr["DueMonth"].ToString();
                        obj.Year = Convert.ToInt32(dr["DueYear"]);
                        obj.Policy_No = Convert.ToInt32(dr["Policy_No"]);
                        lstBADetails.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstBADetails;
        }
        public int SaveLoanApplication(VM_LoanApplicationForm loanApp)
        {
            int result = 0;
            try
            {
                DataTable dt_policydetails = new DataTable();
                dt_policydetails = CreateDataTable("VM_PolicyDetails");
                dt_policydetails.Columns.Remove("LoanBalanceAmt");
                dt_policydetails.Columns.Remove("LoanAmount");
                foreach (var l in loanApp.listPolicyDetails)
                {
                    DataRow dr = dt_policydetails.NewRow();
                    dr["EmpID"] = loanApp.EmpID;
                    dr["PolicyNo"] = l.PolicyNo;
                    dr["LoanNo"] = l.LoanNo;
                    dr["EligibleLoanAmount"] = l.EligibleLoanAmount;
                    dr["AppliedAmount"] = l.LoanAmount;
                    dr["LoanAppID"] = l.LoanAppID;
                    dr["SumAssured"] = l.SumAssured;
                    dr["SanctionDate"] = (l.SanctionDate == null) ? "" : l.SanctionDate;
                    dr["PremiumAmount"] = l.PremiumAmount;
                    dt_policydetails.Rows.Add(dr);
                }
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@PolicyDetails",dt_policydetails),
                    new SqlParameter("@PrincipalInstallments",loanApp.PrincipalInstallments),
                    new SqlParameter("@InterestInstallments",loanApp.InterestInstallments),
                    new SqlParameter("@PurposeID",loanApp.PurposeID),
                    new SqlParameter("@RelationAge",loanApp.RelationAge),
                    new SqlParameter("@FamilyRelationID",loanApp.FamilyRelationID),
                    new SqlParameter("@RelationName",(loanApp.RelationName == null) ? "" : loanApp.RelationName),
                    new SqlParameter("@Deductions",loanApp.Deductions),
                    new SqlParameter("@EmpID",loanApp.EmpID),
                    new SqlParameter("@NetAmount",loanApp.NetAmount),
                    new SqlParameter("@AppliedAmount",loanApp.AppliedAmount),
                    new SqlParameter("@MonthWisePrincipal",loanApp.MonthWisePInstallments),
                    new SqlParameter("@MonthWiseIntrest",loanApp.MonthWiseIInstallments),
                    new SqlParameter("@DocumentFilePath",""),
                    new SqlParameter("@SpouseKGID",loanApp.SpouseKGIDNumber),
                    //new SqlParameter("@LoanAppID",loanApp.LoanAppID)
                };
                   result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_loan_saveloanapplication"));
            }
            catch(Exception ex)
            {
                
            }
            return result;
        }

        public int UploadPayslip(VM_LoanApplicationForm model)
        {
            int result = 0;
            try
            {
                string UploadPath = UploadDocument(model.Document, model.EmpID, "Payslip");
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@UploadPath",UploadPath),
                    new SqlParameter("@EmpID",model.EmpID)
                };
                result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_loan_saveUploadedPayslip"));
            }
            catch(Exception ex)
            {

            }
            return result;
        }
        public VM_LoanApplicationStatus GetLoanApplicationStaus(long EmpID)
        {
            VM_LoanApplicationStatus objLoanAppStatus = new VM_LoanApplicationStatus();
            try
            {
                DataSet dsDD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpID",EmpID)
                };
                dsDD = _Conn.ExeccuteDataset(sqlparam, "sp_loan_selectloanStatus");
                if (dsDD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsDD.Tables[0].Rows)
                    {
                        VM_ApplicationStatus obj = new VM_ApplicationStatus();
                        obj.LoanID = Convert.ToInt32(dr["LoanID"]);
                        obj.EmpID = Convert.ToInt32(dr["EmpID"]);
                        obj.LoanRefNo = dr["LoanRefNo"].ToString();
                        obj.EmpName = dr["EmpName"].ToString();
                        obj.EmpDepartment = dr["EmpDepartment"].ToString();
                        obj.EmpDesignation = dr["EmpDesignation"].ToString();
                        obj.LoanStatus = dr["LoanStatus"].ToString();
                        objLoanAppStatus.listApplicationStatus.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objLoanAppStatus;
        }
        public int CancelLoanApplication(VM_ApplicationStatus loanApplicationCancel)
        {
            int result = 0;
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@LoanID",loanApplicationCancel.LoanID),
                    new SqlParameter("@EmpID",loanApplicationCancel.EmpID)
                };
                result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_loan_cancelLoanApplication"));
            }
            catch(Exception ex)
            {

            }
            return result;
        }
        #region Common Method
        private string UploadDocument(HttpPostedFileBase document, long empId, string docType)
        {
            string subPath = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
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

        public DataTable CreateDataTable(string className)
        {
            DataTable dt = new System.Data.DataTable();
            Type classtype = GetType();
            if (className == "VM_PolicyDetails")
            {
                classtype = typeof(VM_PolicyDetails);
            }

            PropertyInfo[] properties = classtype.GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                dt.Columns.Add(pi.Name);
            }
            return dt;
        }
        #endregion
    }
}
