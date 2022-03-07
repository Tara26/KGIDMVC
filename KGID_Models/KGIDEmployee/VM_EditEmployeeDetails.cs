using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using KGID_Models.KGIDNBApplication;

namespace KGID_Models.KGIDEmployee
{
    public class VM_EditEmployeeDetails
    {
        public VM_EditEmployeeDetails()
        {
            Genders = new List<SelectListItem>();
        }

        public long EmpId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        public List<SelectListItem> Genders { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string PANNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public DateTime? DateOfAppointment { get; set; }
        public bool IsActive { get; set; }
        public string DDOCode { get; set; }
        public string DepartmentCode { get; set; }
        public tbl_employee_work_details tbl_Employee_Work_Details { get; set; }
    }
}
