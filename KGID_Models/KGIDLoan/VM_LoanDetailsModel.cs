using System;
using System.Collections.Generic;
using System.Web;

namespace KGID_Models.KGIDLoan
{
    public class VM_LoanDetailsModel
    {

        public long PolicyID { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime DateOfSanction { get; set; }
        public float SumAssured { get; set; }
        public float PremiumAmount { get; set; }
        public int EligibleLoanAmount { get; set; }
        public string PolicyStatus { get; set; }
        public int NoOfPremiums { get; set; }
        public int EnterAmount { get; set; }      

        public bool IsPriorActive { get; set; }
        public bool IsPreviousLoanTakenPeriodCompleted { get; set; }

        public bool IsLoanApplied { get; set; }

        public bool IsSalaryDeduction { get; set; }
        public bool IsAgeLessThanRequiredAge { get; set; }
        public bool IsGovEmployee { get; set; }

    }
    public class VM_SposeData
    {
        public string KgidNumber { get; set; }
        public string PanNumber { get; set; }
    }
    public class VM_LoanApplicationModel
    {

        public List<VM_LoanDetailsModel> PolicyData { get; set; }
        public string PolicyData1 { get; set; }
        public List<VM_LoanWorkFlow> WorkFlowData { get; set; }
        public List<VM_HRMSPayDetails> HRMSPayDetails { get; set; }

        public decimal EnteredAmount { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetAmount { get; set; }
        public int PurposeId { get; set; }
        public string EmployeeName { get; set; }
        public string PurposeName { get; set; }
        public int FamilyRelationId { get; set; }   
        public string FamilyRelationDesc { get; set; }
        public string FamilyRelationName { get; set; }
        public int FamilyRelationAge { get; set; }
        public int NoOfPrincipleInstallments { get; set; }
        public int NoOfIntrestInstallments { get; set; }
        public int PrincipleInstallmentAmount { get; set; }
        public int InterestInstallmentAmount { get; set; }
        public bool LoanFamilyPurpose { get; set; }
        public string LoanReferenceNo { get; set; }
        public long LoanApplicationId { get; set; }
        public DateTime DateOfApplication { get; set; }
        public decimal PrincipleInstallmentAmt { get; set; }
        public decimal InterestInstallmentAmt { get; set; }

        public HttpPostedFileBase HrmsPaySlip { get; set; }
        public string SpouseKgid { get; set; }
        public string SpousePanNumber { get; set; }

    }

    public class VM_LoanDueList
    {

        public long PolicyNumber { get; set; }
        public long LoanId { get; set; }
        public int Amount { get; set; }
        public int InterestAmount { get; set; }
        public int MonthId { get; set; }
        public string Month { get; set; }
        public int YearId { get; set; }
        public string Purpose { get; set; }
        public string FamilyRelation { get; set; }
    }

    public class VM_PolicyDetails
    {

        public long lpm_loan_extraction_id { get; set; }
        public long lpm_policy_id { get; set; }
        public int lpm_eligible_amount { get; set; }
        public int lpm_applied_amount { get; set; }
        public bool lpm_active { get; set; }
        public DateTime lpm_creation_datetime { get; set; }
        public int lpm_created_by { get; set; }
        public DateTime lpm_updation_datetime { get; set; }
        public int lpm_updated_by { get; set; }  
        
    }

    public class VM_LoanApplicationStat
    {

        public int Received { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Pending { get; set; }

    }

    public class VM_LoanApplicationList
    {
        public long PolocyID { get; set; }

        public int SNo { get; set; }
        public long LoanApplicationId { get; set; }
        public long EmpId { get; set; }
        public string LoanReferenceNo { get; set; }
        public string EmpName { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpDepartment { get; set; }
        public int LoanApplicationStatus { get; set; }
        public string Status { get; set; }

    }
    #region LoanApplicationListForAll
    public class VM_LoanApplicationListForAll
    {
       // public long PolocyID { get; set; }

       // public int SNo { get; set; }
        public long LoanApplicationId { get; set; }
        public long EmpId { get; set; }
        public string LoanReferenceNo { get; set; }
        public string EmpName { get; set; }
        public string EmpDesignation { get; set; }
        public int LoanApplicationStatus { get; set; }
        public bool IsActiveStatus { get; set; }
        public bool ApplicationStatus { get; set; }

    }
    #endregion

    public class VM_LoanWorkFlow
    {
        public long EmployeeID { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string CreationDateTime { get; set; }

        public long law_loan_application_id { get; set; }
        public long law_verified_by { get; set; }
        public string law_remarks { get; set; }
        public string law_comments { get; set; }
        public string law_application_status { get; set; }
        public string law_applicant_name { get; set; }
        public string law_reference_no { get; set; }
        public bool law_checklist_verification_status { get; set; }
        public string law_current_application_status { get; set; }
        public int selectedcategory { get; set; }
    }


    public class VM_LoanVerificationDetails
    {
        public VM_LoanVerificationDetails()
        {
            EmployeeLoanVerificationDetails = new List<EmployeeloanVerificationDetail>();
            LastUpdatedStatusForLoan = new List<EmployeeloanVerificationDetail>();
            ApprovedEmployeeStatus = new List<EmployeeloanVerificationDetail>();
            CancelLoanReport = new List<EmployeeloanVerificationDetail>();
        }
        public IList<EmployeeloanVerificationDetail> EmployeeLoanVerificationDetails { get; set; }

        public IList<EmployeeloanVerificationDetail> LastUpdatedStatusForLoan { get; set; }

        public IList<EmployeeloanVerificationDetail> ApprovedEmployeeStatus { get; set; }

        public IList<EmployeeloanVerificationDetail> CancelRquestApplicationDetials { get; set; }

        public IList<EmployeeloanVerificationDetail> CancelLoanReport { get; set; }

        public long TotalReceived { get; set; }

        public long ForwardedApplications { get; set; }

        public long SentBackApplication { get; set; }

        public long PendingApplications { get; set; }
    }

    public class EmployeeloanVerificationDetail
    {
        public string Name { get; set; }

        public long? EmployeeCode { get; set; }

        public string EmployeeDesignation { get; set; }

        public string ApplicationNumber { get; set; }

        public long ApplicationId { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }

        public bool IsActive { get; set; }
        
        public string LastUpdatedDate { get; set; }
        public string Type { get; set; }

    }
	public class VM_HRMSPayDetails
    {
        public long hrms_hrms_pay_id { get; set; }
        public long hrms_emp_id { get; set; }
        public long hrms_emp_code { get; set; }
        public string hrms_month { get; set; }
        public string hrms_year { get; set; }
        public int hrms_gross_pay { get; set; }
        public int hrms_deduction { get; set; }
        public int hrms_net_pay { get; set; }     
        public string payscaletype { get; set; }
        public string payscaledocument { get; set; }
    }
}
