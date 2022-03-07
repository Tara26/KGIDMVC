using System;
using KGID_Models.KGID_User;
using System.Collections.Generic;
using System.Data;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using KGID_Models.KGID_VerifyData;
using KGID_Models.NBApplication;
using KGID_Models.KGID_Policy;

namespace DLL.NewEmployeeDLL
{
    public interface INewEmployeeDetailsDLL
    {
        IEnumerable<tbl_new_employee_basic_details> NewEmpDetailsdll(tbl_new_employee_basic_details _newemp);

        tbl_dept_master DeptName(string _deptname);
        tbl_new_employee_basic_details GetMobileAndEmailByDeptCode(string _ddoCode, long _empCode);
        IEnumerable<tbl_new_employee_basic_details> GetEmpNameByDeptCode(string _ddoCode, long _empCode);
        IEnumerable<tbl_new_employee_basic_details> LoadEmployeeNamesByDDOCode(string _deptCode);
        IEnumerable<tbl_new_employee_basic_details> GetEmployeeBasicData();


        tbl_challan_details AddChallanDetails(int amount);
        tbl_challan_details UpdateChallanDetails(tbl_challan_details chaDetails);
        tbl_payment_status_details AddPaymentStatus(tbl_payment_status_details payDetails);
        tbl_challan_details FindChallanDetailsById(long Id);
        tbl_new_employee_basic_details GetEmployeeBasicData(long empId);
        


        bool GetEmployeeStatusDll(long EmpId);
		//NEW
		VM_InsuredEmployeeLoginDetail VerifyKGIDAndMobileNo(string kgId, long mobileNumber);
        long GetMobileNumberByKGID(long kgId);
        VM_NewEmployeeLoginDetail GetEmployeeByMobileNumber(long mobileNumber);
        VM_NewEmployeeLoginDetail GetEmployeeByEmail(string email);

        VM_BasicDetails GetEmployeeData(string kgidnum);
        int SaveCategoryDetailsDll(long kgidmun, int ddo, int cw, int avgcw, int sup, int dio, int dd, int d, int AD, int nb, int loan, int claims, int motor, int odclaims, int mvcclaims,long empId);
        //NB Print Challan
        VM_ChallanPrintDetails ChallanprintDetailsDLL(long EmpId, long AppId);
    }
}
