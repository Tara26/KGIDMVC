using DLL.UploadEmployeeDLL;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using System.Collections.Generic;
using System.Data;

namespace BLL.UploadEmployeeBLL
{
    public class UploadEmployeeBLL : IUploadEmployeeBLL
    {
        private readonly IUploadEmployeeDLL _IUploadEmployeeDLL;
        public UploadEmployeeBLL()
        {
            this._IUploadEmployeeDLL = new UploadEmployeeDLL();
        }
        public DataTable DuplicateRemoveExcelData(DataTable dt, string empcode)
        {
            var result = _IUploadEmployeeDLL.DuplicateRemoveExcelData(dt, empcode);
            return result;
        }

        public DataTable DuplicateRemoveExcelDataForWorkDetails(DataTable dt, string empcode)
        {
            var result = _IUploadEmployeeDLL.DuplicateRemoveExcelDataForWorkDetails(dt, empcode);
            return result;
        }

        public DataTable InsertPayDetailsMasterDetails(DataTable dt, string employeecode)
        {
            var result = _IUploadEmployeeDLL.InsertPayDetailsMasterDetails(dt, employeecode);
            return result;
        }

        public List<VM_BasicDetails> GetEmployeeBasicData(long EmpID)
        {
            var result = _IUploadEmployeeDLL.GetEmployeeBasicData(EmpID);
            return result;
        }
        public VM_BasicDetails GetEmployeeDetailsById(long empId)
        {
            var result = _IUploadEmployeeDLL.GetEmployeeDetailsById(empId);
            return result;
        }
        public VM_BasicDetails GetDDODetailsById(long empId)
        {
            var result = _IUploadEmployeeDLL.GetDDODetailsById(empId);
            return result;
        }
        public string GetLoggedDDOCode(long EmpID)
        {
            var result = _IUploadEmployeeDLL.GetLoggedDDOCode(EmpID);
            return result;
        }
        public int DeleteUploadEmployeeDetails(long empId)
        {
            var result = _IUploadEmployeeDLL.DeleteUploadEmployeeDetails(empId);
            return result;
        }
    }
}
