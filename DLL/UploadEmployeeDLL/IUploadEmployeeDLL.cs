using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using System.Collections.Generic;
using System.Data;

namespace DLL.UploadEmployeeDLL
{
    public interface IUploadEmployeeDLL
    {
        //Method to remove duplicate rows from excel datatable to sql upload --Employee basic details
        DataTable DuplicateRemoveExcelData(DataTable dt, string empcode);
        //Method to remove duplicate rows from excel datatable to sql upload --Employee work details
        DataTable DuplicateRemoveExcelDataForWorkDetails(DataTable dt, string empcode);
        //Method to insert in pay details master table
        DataTable InsertPayDetailsMasterDetails(DataTable dt, string empcode);
        //Method to load employee basic details based on ddo code
        List<VM_BasicDetails> GetEmployeeBasicData(long EmpID);
        //Method to get employee basic and work details based on employee code
        VM_BasicDetails GetEmployeeDetailsById(long empId);
        VM_BasicDetails GetDDODetailsById(long empId);
        //Get Logged in ddo code
        string GetLoggedDDOCode(long EmpID);
        //Delete employee
        int DeleteUploadEmployeeDetails(long empId);
    }
}
