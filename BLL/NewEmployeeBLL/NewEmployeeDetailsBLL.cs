using System;
using System.Collections.Generic;
using KGID_Models.KGID_User;
using DLL.NewEmployeeDLL;
using System.Data;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using KGID_Models.KGID_VerifyData;
using KGID_Models.NBApplication;
using KGID_Models.KGID_Policy;

namespace BLL.NewEmployeeBLL
{
    public class NewEmployeeDetailsBLL : INewEmployeeDetailsBLL
    {
        private readonly INewEmployeeDetailsDLL _INewEmpdetailsdll;
        public NewEmployeeDetailsBLL()
        {
            this._INewEmpdetailsdll = new NewEmployeeDetailsDLL();
        }
        public IEnumerable<tbl_new_employee_basic_details> EmpDetailsbll(tbl_new_employee_basic_details _newempdetails)
        {
            var result = _INewEmpdetailsdll.NewEmpDetailsdll(_newempdetails);
            return result;
        }


        public tbl_dept_master DeptName(string _deptname)
        {
            var result = _INewEmpdetailsdll.DeptName(_deptname);
            return result;
        }
        public tbl_new_employee_basic_details GetMobileAndEmailByDeptCode(string _ddoCode, long _empCode)
        {
            var result = _INewEmpdetailsdll.GetMobileAndEmailByDeptCode(_ddoCode, _empCode);
            return result;
        }
        public IEnumerable<tbl_new_employee_basic_details> GetEmpNameByDeptCode(string _ddoCode, long _empCode)
        {
            var result = _INewEmpdetailsdll.GetEmpNameByDeptCode(_ddoCode, _empCode);
            return result;
        }

        public IEnumerable<tbl_new_employee_basic_details> LoadEmployeeNamesByDDOCode(string _ddoCode)
        {
            var result = _INewEmpdetailsdll.LoadEmployeeNamesByDDOCode(_ddoCode);
            return result;
        }

        public IEnumerable<tbl_new_employee_basic_details> GetEmployeeBasicData()
        {
            var result = _INewEmpdetailsdll.GetEmployeeBasicData();
            return result;
        }

        public tbl_challan_details AddChallanDetails(int amount)
        {
            var result = _INewEmpdetailsdll.AddChallanDetails(amount);
            return result;
        }
        public tbl_challan_details UpdateChallanDetails(tbl_challan_details chaDetails)
        {
            var result = _INewEmpdetailsdll.UpdateChallanDetails(chaDetails);
            return result;
        }
        public tbl_payment_status_details AddPaymentStatus(tbl_payment_status_details payDetails)
        {
            var result = _INewEmpdetailsdll.AddPaymentStatus(payDetails);
            return result;
        }
        public tbl_challan_details FindChallanDetailsById(long Id)
        {
            var result = _INewEmpdetailsdll.FindChallanDetailsById(Id);
            return result;
        }

        public tbl_new_employee_basic_details GetNewEmployeeDetails(long empId)
        {
            return _INewEmpdetailsdll.GetEmployeeBasicData(empId);
        }

        //public tbl_new_employee_basic_details VerifyKGIDAndMobileNo(long kgId, string mobileNumber)
        //{
        //    var result = _INewEmpdetailsdll.VerifyKGIDAndMobileNo(kgId, mobileNumber);
        //    return result;
        //}

        //public string GetMobileNumberByKGID(long kgId)
        //{
        //    string mobileNumber = _INewEmpdetailsdll.GetMobileNumberByKGID(kgId);
        //    return mobileNumber;
        //}

        //public tbl_new_employee_basic_details GetEmployeeByMobileNumber(string mobileNumber)
        //{
        //    return _INewEmpdetailsdll.GetEmployeeByMobileNumber(mobileNumber);
        //}

        //public tbl_new_employee_basic_details GetEmployeeByEmail(string email)
        //{
        //    return _INewEmpdetailsdll.GetEmployeeByEmail(email);
        //}

        public bool GetEmployeeStatusBll(long EmpId)
        {
            return _INewEmpdetailsdll.GetEmployeeStatusDll(EmpId);
        }

        //NEW 
        public VM_InsuredEmployeeLoginDetail VerifyKGIDAndMobileNo(string kgId, long mobileNumber)
        {
            var result = _INewEmpdetailsdll.VerifyKGIDAndMobileNo(kgId, mobileNumber);
            return result;
        }

        public long GetMobileNumberByKGID(long kgId)
        {
            long mobileNumber = _INewEmpdetailsdll.GetMobileNumberByKGID(kgId);
            return mobileNumber;
        }

        public VM_NewEmployeeLoginDetail GetEmployeeByMobileNumber(long mobileNumber)
        {
            return _INewEmpdetailsdll.GetEmployeeByMobileNumber(mobileNumber);
        }

        public VM_NewEmployeeLoginDetail GetEmployeeByEmail(string email)
        {
            return _INewEmpdetailsdll.GetEmployeeByEmail(email);
        }
        public VM_BasicDetails GetEmployeeData(string kgidnum)
        {
            return _INewEmpdetailsdll.GetEmployeeData(kgidnum);
        }
        public int SaveCategoryDetailsBll(long kgidmun, int ddo, int cw, int avgcw, int sup, int dio, int dd, int d, int AD, int nb, int loan, int claims, int motor, int odclaims, int mvcclaims, long empId)
        {
            return _INewEmpdetailsdll.SaveCategoryDetailsDll(kgidmun,ddo,cw,avgcw,sup,dio,dd,d, AD,nb, loan,claims,motor,odclaims,mvcclaims,empId);
        }
        //NB Challan Print Details
        public VM_ChallanPrintDetails ChallanprintDetailsBLL(long EmpId, long AppId)
        {
            return _INewEmpdetailsdll.ChallanprintDetailsDLL(EmpId,AppId);
        }
    }
}
