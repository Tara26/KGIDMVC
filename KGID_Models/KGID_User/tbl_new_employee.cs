using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace KGID_Models.KGID_User
{
    public class tbl_new_employee
    {
        [Key]
        public string em_sys_emp_code { get; set; }
        public string em_dept_emp_code { get; set; }
        public string em_empl_full_name { get; set; }
        public string em_father_name { get; set; }
        public string em_gender { get; set; }
        public Nullable<System.DateTime> em_date_of_birth { get; set; }
        public Nullable<System.DateTime> em_date_of_appointment { get; set; }
        public Nullable<System.DateTime> em_dateof_joining { get; set; }
        public string em_payscale_code { get; set; }
        public string em_permanent_temporary { get; set; }
        public int em_designation { get; set; }
        public string em_group { get; set; }
        public string em_mobilenumber { get; set; }
        public string em_email { get; set; }
        public string em_place_of_posting { get; set; }
        public string em_dept_code { get; set; }
        public Nullable<bool> em_active { get; set; }
        public Nullable<int> em_created_by { get; set; }
        public Nullable<int> em_updated_by { get; set; }
        public Nullable<System.DateTime> em_creation_datetime { get; set; }
        public Nullable<System.DateTime> em_updation_datetime { get; set; }

        public IEnumerable<tbl_new_employee> GetNewEmployeeList { get; set; }      

    }

    public class ExcelValidation
    {
        public string module { get; set; }
        public string column { get; set; }
        public string validation { get; set; }
        public string errorMsg { get; set; }
    }

    public class Validation
    {
        public List<ExcelValidation> ValidationData { get; set; }
    }
    public class ExcelModel
    {
        public HttpPostedFileBase FileUpload { get; set; }
        public int error { get; set; }
        public string rowNum { get; set; }
        public string errorDesc { get; set; }
        public List<ExcelModel> gdErrorList { get; set; }
        public string fileName { get; set; }
        public string year { get; set; }
        public string budgetDesc { get; set; }
    }
    public class UploadModel
    {
    public HttpPostedFileBase FileUpload { get; set; }
    public int error { get; set; }
    public string rowNum { get; set; }
    public string errorDesc { get; set; }
    public List<UploadModel> gdErrorList { get; set; }
    public string fileName { get; set; }
    public string year { get; set; }
    public string propertyDesc { get; set; }
    public string propertyType { get; set; }
    }
}
