using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DBConnection;
using KGID_Models.KGID_Loan;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_Policy;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDEmployee;

namespace DLL.KGIDPolicyDLL
{
    public class PolicyDLL : IPolicyDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();

        public VM_FacingSheet GetFacingSheetDetails(long applicationId,long employeeId)
        {
            VM_FacingSheet facingSheet = new VM_FacingSheet();
            
            DataSet dsBD = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@employeeId", employeeId),
                new SqlParameter("@applicationId", applicationId)
            };

            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_policy_getFacingSheetDetails");
            if (dsBD.Tables[0].Rows.Count > 0)
            {
                facingSheet.Age = Convert.ToString(dsBD.Tables[0].Rows[0]["Age"].ToString());
                facingSheet.DateOfBirth = Convert.ToDateTime(dsBD.Tables[0].Rows[0]["DateOfBirth"].ToString());
                
                if (dsBD.Tables[0].Rows[0]["DateOfIssue"].ToString() != null && dsBD.Tables[0].Rows[0]["DateOfIssue"].ToString() != "")
                {
                    facingSheet.DateOfIssue = Convert.ToDateTime(dsBD.Tables[0].Rows[0]["DateOfIssue"].ToString());
                }
                facingSheet.EstimatedPremium = Convert.ToDecimal(dsBD.Tables[0].Rows[0]["Premium"].ToString());
                facingSheet.InitialDeposit = Convert.ToDecimal(dsBD.Tables[0].Rows[0]["Premium"].ToString());
                if (dsBD.Tables[0].Rows[0]["SumAssured"].ToString() != null && dsBD.Tables[0].Rows[0]["SumAssured"].ToString() != "")
                {
                    facingSheet.InsuranceAmount = Convert.ToDecimal(dsBD.Tables[0].Rows[0]["SumAssured"].ToString());
                    facingSheet.SumAssured = Convert.ToDecimal(dsBD.Tables[0].Rows[0]["SumAssured"].ToString());
                }
                //facingSheet.InsuranceAmount = Convert.ToDecimal(dsBD.Tables[0].Rows[0]["SumAssured"].ToString());
                facingSheet.InsurerName = dsBD.Tables[0].Rows[0]["EmpName"].ToString();
                facingSheet.LoadFactor = dsBD.Tables[0].Rows[0]["LoadFactor"].ToString();
                facingSheet.MonthlyPremium = Convert.ToDecimal(dsBD.Tables[0].Rows[0]["Premium"].ToString());
                //facingSheet.SumAssured = Convert.ToDecimal(dsBD.Tables[0].Rows[0]["SumAssured"].ToString());
                facingSheet.DLFactor = dsBD.Tables[0].Rows[0]["DLFactor"].ToString();
                facingSheet.ApplicationNumber = dsBD.Tables[0].Rows[0]["ApplicationNumber"].ToString();
                facingSheet.FirstKGIDNumber = dsBD.Tables[0].Rows[0]["FirstKGIDNumber"].ToString();
                facingSheet.DistrictInsuranceOfficeAddress = dsBD.Tables[0].Rows[0]["DistrictInsuranceOfficeAddress"].ToString();
                if (dsBD.Tables[0].Rows[0]["PolicyNumber"].ToString() != null && dsBD.Tables[0].Rows[0]["PolicyNumber"].ToString() != "")
                {
                    facingSheet.PolicyNumber = dsBD.Tables[0].Rows[0]["PolicyNumber"].ToString();
                    facingSheet.EffectiveMonthYear = dsBD.Tables[0].Rows[0]["MaturityDate"].ToString();
                    //facingSheet.SumAssured = dsBD.Tables[0].Rows[0]["SumAssured"].ToString() != "" ? Convert.ToDecimal(dsBD.Tables[0].Rows[0]["SumAssured"].ToString()) : 0;
                    //facingSheet.InsuranceAmount = dsBD.Tables[0].Rows[0]["SumAssured"].ToString() != "" ? Convert.ToDecimal(dsBD.Tables[0].Rows[0]["SumAssured"].ToString()) : 0;
                }
                facingSheet.ChallanRefNo = dsBD.Tables[0].Rows[0]["ChallanRefNo"].ToString();
                facingSheet.ChallanDate = dsBD.Tables[0].Rows[0]["ChallanDate"].ToString();
                //Verification Details
                facingSheet.DDOName= dsBD.Tables[0].Rows[0]["DDOName"].ToString();
                facingSheet.DDOVDate = dsBD.Tables[0].Rows[0]["DDOVDate"].ToString();

                facingSheet.CWName = dsBD.Tables[0].Rows[0]["CWName"].ToString();
                facingSheet.CWVDate = dsBD.Tables[0].Rows[0]["CWVDate"].ToString();

                facingSheet.SIName = dsBD.Tables[0].Rows[0]["SIName"].ToString();
                facingSheet.SIVDate = dsBD.Tables[0].Rows[0]["SIVDate"].ToString();

                facingSheet.DIOName = dsBD.Tables[0].Rows[0]["DIOName"].ToString();
                facingSheet.DIOVDate = dsBD.Tables[0].Rows[0]["DIOVDate"].ToString();

                facingSheet.DDName = dsBD.Tables[0].Rows[0]["DDName"].ToString();
                facingSheet.DDVDate = dsBD.Tables[0].Rows[0]["DDVDate"].ToString();

                facingSheet.DName = dsBD.Tables[0].Rows[0]["DName"].ToString();
                facingSheet.DVDate = dsBD.Tables[0].Rows[0]["DVDate"].ToString();
            }


            if (dsBD.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow rowItem in dsBD.Tables[1].Rows)
                {
                    facingSheet.Policies.Add(rowItem["PolicyNumber"].ToString());
                }
            }

            return facingSheet;
        }
        public VM_InitimationLetter GetIntimationLetter(long applicationId)
        {
            VM_InitimationLetter initimationLetter = new VM_InitimationLetter();

            try
            {
                DataSet dsBD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@applicationId", applicationId)
                };

                dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_policy_getIntimationLetterDetails");
                if (dsBD.Tables[0].Rows.Count > 0)
                {
                   // initimationLetter.DateOfMaturity = Convert.ToDateTime(dsBD.Tables[0].Rows[0]["DateOfMaturity"].ToString());
                    initimationLetter.SDateOfMaturity = dsBD.Tables[0].Rows[0]["SDateOfMaturity"].ToString();

                    initimationLetter.DDOAddress = dsBD.Tables[0].Rows[0]["DDOAddress"].ToString();
                    initimationLetter.DIOAddress = dsBD.Tables[0].Rows[0]["DIOAddress"].ToString();
                    initimationLetter.Designation = dsBD.Tables[0].Rows[0]["Designation"].ToString();
                    initimationLetter.KGIDNumber = dsBD.Tables[0].Rows[0]["KGIDNumber"].ToString();
                    initimationLetter.MonthlyPremium = Convert.ToDecimal(dsBD.Tables[0].Rows[0]["MonthlyPremium"].ToString());
                    initimationLetter.TwoMonthPremium = Convert.ToDecimal(dsBD.Tables[0].Rows[0]["TwoMonthPremium"].ToString());
                    initimationLetter.NameOfPolicyHolder = dsBD.Tables[0].Rows[0]["NameOfPolicyHolder"].ToString();
                    initimationLetter.NameOfPolicyHolderKan = dsBD.Tables[0].Rows[0]["NameOfPolicyHolderKan"].ToString();
                    initimationLetter.NameOfFatherKan = dsBD.Tables[0].Rows[0]["NameOfFatherKan"].ToString();

                    //initimationLetter.RiskDate = dsBD.Tables[0].Rows[0]["RiskDate"].ToString();
                    initimationLetter.SRiskDate = dsBD.Tables[0].Rows[0]["SRiskDate"].ToString();

                    initimationLetter.CommencementMonth = dsBD.Tables[0].Rows[0]["FourthMonth"].ToString();
                    initimationLetter.CommencementMonth = dsBD.Tables[0].Rows[0]["CommencementMonth"].ToString(); //initimationLetter.RiskDate.AddMonths(1).ToString("MMM-yyyy");
                    initimationLetter.SumAssured = Convert.ToDecimal(dsBD.Tables[0].Rows[0]["SumAssured"].ToString());
                    initimationLetter.ParentName = dsBD.Tables[0].Rows[0]["ParentName"].ToString();
                    initimationLetter.VerifiedByEmpId = Convert.ToInt64(dsBD.Tables[0].Rows[0]["VerifiedByEmpId"]);

                    //initimationLetter.VerifiedDate = Convert.ToDateTime(dsBD.Tables[0].Rows[0]["VerifiedDate"].ToString());
                    initimationLetter.SVerifiedDate = dsBD.Tables[0].Rows[0]["SVerifiedDate"].ToString();

                    initimationLetter.VerifiedName = dsBD.Tables[0].Rows[0]["VerifiedName"].ToString();
                    initimationLetter.FutureMonth = dsBD.Tables[0].Rows[0]["FutureMonth"].ToString();
                    initimationLetter.ApprovedMonth = dsBD.Tables[0].Rows[0]["ApprovedMonth"].ToString();
                    initimationLetter.ProposalSubmissionDate = dsBD.Tables[0].Rows[0]["ProposalSubmissionDate"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return initimationLetter;
        }

        public string NBBondFacingDocUploadDLL(long AppId, long EmpId, string DocPath, string DocType)
        {
            string returnString = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@AppId",AppId),
                    new SqlParameter("@EmpId",EmpId),
                    new SqlParameter("@DocPath",DocPath),
                    new SqlParameter("@DocType",DocType)
                };
                returnString = _Conn.ExecuteCmd(sqlparam, "sp_kgid_NB_BOND_and_FS_Doc_Upload");
            }
            catch (Exception ex)
            {

            }
            return returnString;
        }
        public string GetNBBondDocFileDLL(long AppId, long EmpId)
        {
            string returnString = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@AppId",AppId),
                    new SqlParameter("@EmpId",EmpId)
                };
                returnString = _Conn.ExecuteCmd(sqlparam, "sp_kgid_Get_NB_BOND_Doc");
            }
            catch (Exception ex)
            {

            }
            return returnString;
        }

        public string NBSignBondUploadDLL(long AppId, long EmpId, string DocPath)
        {
            string returnString = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@AppId",AppId),
                    new SqlParameter("@EmpId",EmpId),
                    new SqlParameter("@DocPath",DocPath)
                };
                returnString = _Conn.ExecuteCmd(sqlparam, "sp_kgid_NB_Sign_BOND_Doc_Upload");
            }
            catch (Exception ex)
            {

            }
            return returnString;
        }
    }
}
