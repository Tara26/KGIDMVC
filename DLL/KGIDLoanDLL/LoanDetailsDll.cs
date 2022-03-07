using Common;
using DLL.DBConnection;
using KGID_Models.KGID_Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDLoanDLL
{
    public class LoanDetailsDll:ILoanDetailsDll
    {
        private readonly DbConnection _db = new DbConnection();

        
        public VM_LoanDDO GetLoanDetailsByLoanId(long loanReferenceId)
        {
            VM_LoanDDO loanApplication = new VM_LoanDDO();
            var loanDetail = (from loanData in _db.tbl_loan_details
                              where loanData.loan_id == loanReferenceId
                              select loanData).FirstOrDefault();

            if (loanDetail != null)
            {
                var loanPurposeData = (from loanPurposeMaster in _db.tbl_loan_purpose_master
                                       where loanPurposeMaster.loan_purpose_id == loanDetail.loan_purpose_id
                                       select loanPurposeMaster).FirstOrDefault();
                if (loanPurposeData != null)
                {
                    loanApplication.LoanPurposeName = loanPurposeData.loan_purpose_desc;
                    loanApplication.LoanPurposeId = loanPurposeData.loan_purpose_id;
                }

                var loanFamilyData = (from loanFamilyMaster in _db.tbl_loan_family_member
                                      where loanFamilyMaster.lfm_id == loanDetail.loan_family_member_id
                                      select loanFamilyMaster).FirstOrDefault();
                if (loanFamilyData != null)
                {
                    loanApplication.LoanFamilyName = loanFamilyData.lfm_loan_member_desc;
                    loanApplication.LoanFamilyId = loanFamilyData.lfm_id;
                }

                loanApplication.loan_instalment_months_of_principle_amount = loanDetail.loan_instalment_months_of_principle_amount??0;
                loanApplication.loan_instalment_months_of_interest_amount = loanDetail.loan_instalment_months_of_interest_amount?? 0;
                if (loanDetail.spouse_insurance_id != null)
                {
                    loanApplication.SpouseKgidNumber = loanDetail.spouse_insurance_id.ToString();
                }

                loanApplication.LoanReferenceNo = loanDetail.loan_id;
                loanApplication.PolicyNumber = loanDetail.loan_application_ref_no;
                var loanStatusData = (from loanStatusMaster in _db.tbl_status_master
                                     // where loanStatusMaster.sm_id == loanDetail.loan_status
                                      select loanStatusMaster).FirstOrDefault();
                if (loanStatusData != null)
                {
                    loanApplication.LoanStatus = loanStatusData.sm_description;
                }               
            }


            return loanApplication;
        }

       
        public List<VM_LoanVerificationDetails> GetDetailsForLoanVerification(long EmpCode, string LoanVerifier)
        {
            List<VM_LoanVerificationDetails> objlistVerificationDetails = new List<VM_LoanVerificationDetails>();
            if (LoanVerifier == Convert.ToString(UserCategories.DDO))
            {
                var ddocode = (from d in _db.tbl_new_employee_basic_details
                               where d.nebd_sys_emp_code == EmpCode
                               select d.nebd_ddo_code).FirstOrDefault();

                objlistVerificationDetails = (from loan in _db.tbl_loan_details
                                              join map in _db.tbl_kgid_mapping_details on loan.insurance_id equals map.kmd_subsequent_policy_no
                                              where loan.ddo_code == ddocode && loan.active_status == true
                                              select new VM_LoanVerificationDetails
                                              {
                                                  loan_id = loan.loan_id,
                                                  insurance_id = loan.insurance_id,
                                                  loan_application_ref_no = loan.loan_application_ref_no,
                                                  employeename = map.kmd_emp_name,
                                                  submission_date = loan.submission_date,
                                                  loanstatus = loan.loan_status
                                              }).ToList();
            }
            if (LoanVerifier == Convert.ToString(UserCategories.CASEWORKER))
            {
                objlistVerificationDetails = (from loan in _db.tbl_loan_details
                                              join map in _db.tbl_kgid_mapping_details on loan.insurance_id equals map.kmd_subsequent_policy_no
                                              where loan.active_status == true && (loan.loan_status == "R1" || loan.loan_status == "B1")
                                              select new VM_LoanVerificationDetails
                                              {
                                                  loan_id = loan.loan_id,
                                                  insurance_id = loan.insurance_id,
                                                  loan_application_ref_no = loan.loan_application_ref_no,
                                                  employeename = map.kmd_emp_name,
                                                  submission_date = loan.submission_date,
                                                  loanstatus = loan.loan_status
                                              }).ToList();
            }
            return objlistVerificationDetails;
        }
    }
}
