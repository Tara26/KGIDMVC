using DLL.DBConnection;
using KGID_Models.KGID_Loan;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_VerifyData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DLL.KGIDLoanDLL
{
    public class KGIDLoanDetailsDll : IKGIDLoanDetailsDll
    {
        private readonly DbConnection _db = new DbConnection();

        public List<tbl_district_master> GetDistricts()
        {
            return (from district in _db.tbl_district_master
                    select district).ToList();
        }

        public VM_EmployeeDetails GetEmployeeDetails(string policyNumber)
        {
            return (from verDet in _db.tbl_verification_details
                    join kgidMapping in _db.tbl_kgid_mapping_details on verDet.POLICY_NO equals kgidMapping.kmd_subsequent_policy_no into kgidMappingTemp
                    from kgidMapping in kgidMappingTemp.DefaultIfEmpty()
                    where verDet.POLICY_NO == policyNumber
                    select new VM_EmployeeDetails()
                    {
                        EmployeeNumber = kgidMapping.kmd_emp_id,
                        FirstPolicyNumber = Convert.ToInt64(verDet.FIRST_KGIDNO_HRMS),
                        HRMSName = kgidMapping.kmd_emp_name,
                        InsuredName = verDet.EMPLOYEE_NAME_KGID,
                        PolicyNumber = verDet.POLICY_NO
                    }).FirstOrDefault();
        }

        public VM_LoanDetails GetLoanDetailsDll(int loanId)
        {
            var EmpLoanDetails = (from loanTrans in _db.tbl_employee_loan_transaction
                                  where loanTrans.Loan_Id == loanId
                                  select new VM_LoanDetails()
                                  {
                                      Amount = loanTrans.Loan_Amount,
                                      BranchAdjustment = loanTrans.Loan_Deduction,
                                      CreatedDate = loanTrans.Loan_CreatedDate,
                                      GrossLoanAmount = loanTrans.Loan_Gross_Loan_Amount,
                                      Id = loanTrans.Loan_Id,
                                      InstalmentAmount = loanTrans.Loan_Instalment_Amount,
                                      Interest = loanTrans.Loan_Interest,
                                      NetAmount = loanTrans.Loan_Net_Amount,
                                      Others = loanTrans.Loan_Others,
                                      Period = loanTrans.Loan_Period,
                                      PolicyNumber = loanTrans.Loan_Policy_Number,
                                      Premium = loanTrans.Loan_Premium,
                                      PremiumInterest = loanTrans.Loan_Premium_Interest,
                                      SanctionedDate = loanTrans.Loan_Sanctioned_Date,
                                      UpdatedDate = loanTrans.Loan_UpdatedDate,
                                      DistrictId = loanTrans.Loan_dist_id
                                  }).FirstOrDefault();


            //var LoanTrans = (from n in _db.tbl_employee_loan_transaction
            //                    where n.Loan_Policy_Number == _PolicyNumber
            //                    select n).FirstOrDefault();

            //if (LoanTrans == null)
            //{
            //    var kgidPolicyDistrictMapping = (from policyDistrictMapping in _db.tbl_kgid_policy_district_mapping
            //                                     where policyDistrictMapping.district_id == districtId
            //                                     && policyDistrictMapping.policy_no == _PolicyNumber
            //                                     select policyDistrictMapping).FirstOrDefault();

            //    if (kgidPolicyDistrictMapping != null)
            //    {
            //        var EmpVerDetails = (from verDetail in _db.tbl_verification_details
            //                             where verDetail.POLICY_NO == _PolicyNumber
            //                             select verDetail).FirstOrDefault();

            //        if (EmpVerDetails != null)
            //        {
            //            LoanEmpTrans.LoanTrans = new tbl_employee_loan_transaction();
            //        }
            //    }
            //}

            return EmpLoanDetails;
        }

        public VM_SubsequentPolicyDetails GetSubsequentPolicyDetails(string policyNumber)
        {
            VM_SubsequentPolicyDetails subsequentPolicyDetails = new VM_SubsequentPolicyDetails();
            tbl_kgid_mapping_details kgidPolicy = (from subPolicies in _db.tbl_kgid_mapping_details
                                                   where subPolicies.kmd_subsequent_policy_no == policyNumber
                                                   select subPolicies).FirstOrDefault();

            if (kgidPolicy != null)
            {
                List<tbl_kgid_mapping_details> kgidPolicyMappingDetails = (from subPolicies in _db.tbl_kgid_mapping_details
                                                                           where subPolicies.kmd_first_policy_no == kgidPolicy.kmd_first_policy_no
                                                                           select subPolicies).ToList();

                foreach (var kgidPolicyMappingDetail in kgidPolicyMappingDetails)
                {
                    List<VM_LoanDetails> loanDetails = (from loanTDetails in _db.tbl_employee_loan_transaction
                                                                           where loanTDetails.Loan_Policy_Number == kgidPolicyMappingDetail.kmd_subsequent_policy_no
                                                                           select new VM_LoanDetails()
                                                                           {
                                                                               Id = loanTDetails.Loan_Id,
                                                                               PolicyNumber = loanTDetails.Loan_Policy_Number,
                                                                               SanctionedDate = loanTDetails.Loan_Sanctioned_Date,
                                                                           }).ToList();

                    if (loanDetails.Any())
                    {
                        subsequentPolicyDetails.SubsquentPolicies.AddRange(loanDetails);
                    }
                    else
                    {
                        loanDetails = new List<VM_LoanDetails>();
                        VM_LoanDetails loanDetail = new VM_LoanDetails();
                        loanDetail.PolicyNumber = kgidPolicyMappingDetail.kmd_subsequent_policy_no;
                        loanDetails.Add(loanDetail);
                        subsequentPolicyDetails.SubsquentPolicies.AddRange(loanDetails);
                    }
                }
            }

            return subsequentPolicyDetails;
        }

        public int SaveLoanDetailsDll(VM_LoanDetails loanTDetails)
        {
            int result = 0;
            var loanDetails = (from n in _db.tbl_employee_loan_transaction
                               where n.Loan_Id == loanTDetails.Id
                               select n).FirstOrDefault();

            if (loanDetails != null)
            {
                loanDetails.Loan_Policy_Number = loanTDetails.PolicyNumber;
                loanDetails.Loan_Gross_Loan_Amount = loanTDetails.GrossLoanAmount;
                loanDetails.Loan_Premium = loanTDetails.Premium;
                loanDetails.Loan_Sanctioned_Date = loanTDetails.SanctionedDate;
                loanDetails.Loan_Premium_Interest = loanTDetails.PremiumInterest;
                loanDetails.Loan_Deduction = loanTDetails.BranchAdjustment;
                loanDetails.Loan_Amount = loanTDetails.Amount;
                loanDetails.Loan_Net_Amount = loanTDetails.NetAmount;
                loanDetails.Loan_Interest = loanTDetails.Interest;
                loanDetails.Loan_Period = loanTDetails.Period;
                loanDetails.Loan_Others = loanTDetails.Others;
                loanDetails.Loan_Instalment_Amount = loanTDetails.InstalmentAmount;
                loanDetails.Loan_UpdatedDate = DateTime.Now;
                _db.SaveChanges();
                result = 2;
            }
            else
            {
                if (!string.IsNullOrEmpty(loanTDetails.PolicyNumber))
                {
                    loanDetails = new tbl_employee_loan_transaction();
                    loanDetails.Loan_Policy_Number = loanTDetails.PolicyNumber;
                    loanDetails.Loan_Gross_Loan_Amount = loanTDetails.GrossLoanAmount;
                    loanDetails.Loan_Premium = loanTDetails.Premium;
                    loanDetails.Loan_Sanctioned_Date = loanTDetails.SanctionedDate;
                    loanDetails.Loan_Premium_Interest = loanTDetails.PremiumInterest;
                    loanDetails.Loan_Deduction = loanTDetails.BranchAdjustment;
                    loanDetails.Loan_Amount = loanTDetails.Amount;
                    loanDetails.Loan_Net_Amount = loanTDetails.NetAmount;
                    loanDetails.Loan_Interest = loanTDetails.Interest;
                    loanDetails.Loan_Period = loanTDetails.Period;
                    loanDetails.Loan_Others = loanTDetails.Others;
                    loanDetails.Loan_Instalment_Amount = loanTDetails.InstalmentAmount;
                    loanDetails.Loan_dist_id = loanTDetails.DistrictId;
                    loanDetails.Loan_CreatedDate = DateTime.Now;
                    loanDetails.Loan_UpdatedDate = DateTime.Now;

                    _db.tbl_employee_loan_transaction.Add(loanDetails);
                    _db.SaveChanges();
                    result = 1;
                }
            }
            return result;
        }
    }
}
