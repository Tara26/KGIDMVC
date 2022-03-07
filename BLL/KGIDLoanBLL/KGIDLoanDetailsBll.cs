using DLL.KGIDLoanDLL;
using KGID_Models.KGID_Loan;
using KGID_Models.KGID_VerifyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.KGIDLoanBLL
{
    public class KGIDLoanDetailsBll : IKGIDLoanDetailsBll
    {
        private readonly IKGIDLoanDetailsDll _ILoan;

        public KGIDLoanDetailsBll()
        {
            this._ILoan = new KGIDLoanDetailsDll();
        }

        public List<SelectListItem> GetDestricts()
        {
            List<SelectListItem> districts = new List<SelectListItem>();
            List<tbl_district_master> districtList = _ILoan.GetDistricts();
            districts = districtList.Select(x => new SelectListItem()
            {
                Text = x.dm_name_english,
                Value = x.dm_id.ToString()
            }).OrderBy(t => t.Text).ToList();

            return districts;
        }

        public VM_EmployeeDetails GetEmployeeDetails(string policyNumber)
        {
            return _ILoan.GetEmployeeDetails(policyNumber);
        }

        public VM_LoanDetails GetEmployeeLoanDetails(int loanId)
        {
            var _LoanData = _ILoan.GetLoanDetailsDll(loanId);
            _LoanData.Districts = GetDestricts();
            return _LoanData;
        }

        public VM_SubsequentPolicyDetails GetSubsequentPolicyDetails(string policyNumber)
        {
            return _ILoan.GetSubsequentPolicyDetails(policyNumber);
        }

        public int SaveLoanDetailsBll(VM_LoanDetails _LoanDetails)
        {
            var _RData = _ILoan.SaveLoanDetailsDll(_LoanDetails);
            return _RData;
        }
    }
}
