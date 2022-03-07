using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using System.Collections.Generic;
using System.Data;

namespace BLL.UploadEmployeeBLL
{
    public interface IUploadEmployeeBLL
    {
        DataTable DuplicateRemoveExcelData(DataTable dt, string empcode);
        DataTable DuplicateRemoveExcelDataForWorkDetails(DataTable dt,string empcode);
        DataTable InsertPayDetailsMasterDetails(DataTable dt, string employeecode);
        List<VM_BasicDetails> GetEmployeeBasicData(long EmpID);
        VM_BasicDetails GetEmployeeDetailsById(long empId);
        VM_BasicDetails GetDDODetailsById(long empId);
        string GetLoggedDDOCode(long EmpID);
        int DeleteUploadEmployeeDetails(long empId);
    }
}
