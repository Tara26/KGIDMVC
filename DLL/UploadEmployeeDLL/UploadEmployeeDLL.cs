using DLL.DBConnection;
using KGID_Models.KGID_Upload;
using KGID_Models.KGIDLoan;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace DLL.UploadEmployeeDLL
{
    public class UploadEmployeeDLL : IUploadEmployeeDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        
        public string GetLoggedDDOCode(long EmpID)
        {
            string DDOCode = string.Empty;
            DDOCode = (from w in _db.tbl_employee_work_details 
                       join d in _db.tbl_ddo_master on w.ewd_ddo_id equals d.dm_ddo_id
                       where w.ewd_emp_id == EmpID
                       select d.dm_ddo_code
                      ).FirstOrDefault();
            return DDOCode;
        }

        public DataTable DuplicateRemoveExcelData(DataTable dt, string empcode)
        {
            DataTable excelDatatable = new DataTable();
            DataTable dtError = new DataTable();
            try
            {
                var empdetails = (from n in _db.tbl_employee_basic_details
                                  select n).ToList();

                DateTime? CustomParseDateTime(object value)
                {
                    if (value == null || value is DBNull) return null;
                    return Convert.ToDateTime(value);
                }

                Int32? CustomParseInt(object value)
                {
                    if (value == null || value is DBNull) return null;
                    return Convert.ToInt32(value);
                }
                Boolean? CustomParseBoolean(object value)
                {
                    if (value == null || value is DBNull) return null;
                    else if (value.ToString() == "1") return true;
                    else if (value.ToString() == "0") return false;
                    return Convert.ToBoolean(value);
                }
                var employeeBasicDetails = (from DataRow dr in dt.Rows
                                            join gender in _db.tbl_gender_master 
                                            on Convert.ToString(dr["gender"]).Trim() equals gender.gender_desc
                                            join dept in _db.tbl_department_master
                                            on Convert.ToString(dr["dept_employee_code"]).Trim() equals dept.dm_deptcode
                                            select new VM_BasicDetails()
                                            {
                                                //employee_id = Convert.ToInt64(dr["employee_id"]),
                                                //hrms_employee_code = Convert.ToInt64(dr["hrms_employee_code"]),
                                                dept_employee_code = Convert.ToInt64(dept.dm_dept_id),
                                                employee_name = Convert.ToString(dr["employee_name"] ?? null),
                                                father_name = Convert.ToString(dr["father_name"] ?? null),
                                                gender_id =  Convert.ToInt16(gender.gender_id),
                                                date_of_birth = dr["date_of_birth"].ToString(),
                                                place_of_birth = Convert.ToString(dr["place_of_birth"]),
                                                pan_number = Convert.ToString(dr["pan_number"]),
                                                mobile_number = Convert.ToInt64(dr["mobile_number"]),
                                                email_id = Convert.ToString(dr["email_id"] ?? null),
                                                date_of_appointment = dr["date_of_appointment"].ToString(),
                                                spouse_name = Convert.ToString(dr["spouse_name"] ?? null)
                                            }).ToList();

                //var finList = (from excelEmpList in employeeBasicDetails
                //               join stuempList in empdetails
                //               on excelEmpList.hrms_employee_code equals stuempList.hrms_employee_code
                //               select excelEmpList).ToList();

                var ExcelData = (from excelEmpList in employeeBasicDetails
                                select excelEmpList).ToList();
                if (ExcelData.Count() > 0)
                {

                    var checkmbldup = (from DataRow dr in dt.Rows
                                       from emp in empdetails
                                       where emp.pan_number == Convert.ToString(dr["pan_number"]) || emp.mobile_number == Convert.ToInt64(dr["mobile_number"]) || emp.email_id == Convert.ToString(dr["email_id"])
                                       select dr);

                    if (checkmbldup.Count() > 0)
                    {
                        dtError = dt.Clone();
                        dtError.Columns.Add("Row Number");
                        dtError.Columns.Add("Error Desc").DefaultValue = "Provided Mobile number/Pan number/Email id for the employee code is already registerd.";
                        foreach (var dup in checkmbldup)
                        {
                            dtError.ImportRow(dup);
                        }
                        return dtError;
                    }

                    //excelDatatable.Columns.Add("hrms_employee_code", typeof(long));
                    excelDatatable.Columns.Add("dept_employee_code", typeof(string));
                    excelDatatable.Columns.Add("employee_name", typeof(string));
                    excelDatatable.Columns.Add("father_name", typeof(string));
                    excelDatatable.Columns.Add("gender_id", typeof(string));
                    excelDatatable.Columns.Add("date_of_birth", typeof(DateTime));
                    excelDatatable.Columns.Add("place_of_birth", typeof(string));
                    excelDatatable.Columns.Add("pan_number", typeof(string));
                    excelDatatable.Columns.Add("mobile_number", typeof(string));
                    excelDatatable.Columns.Add("email_id", typeof(string));
                    excelDatatable.Columns.Add("date_of_appointment", typeof(DateTime));
                    excelDatatable.Columns.Add("spouse_name", typeof(string));
                    excelDatatable.Columns.Add("ddo_upload_status", typeof(bool));
                    excelDatatable.Columns.Add("active_status", typeof(bool));
                    excelDatatable.Columns.Add("created_by", typeof(int));
                    excelDatatable.Columns.Add("updated_by", typeof(int));
                    excelDatatable.Columns.Add("creation_datetime", typeof(DateTime));
                    excelDatatable.Columns.Add("updation_datetime", typeof(DateTime));
                    excelDatatable.Columns.Add("user_category_id", typeof(int));

                    //For each item in your list, add those items
                    //in your data table
                    foreach (var item in ExcelData)
                    {
                        excelDatatable.Rows.Add(item.dept_employee_code, item.employee_name,
                            item.father_name, item.gender_id, item.date_of_birth, item.place_of_birth,
                            item.pan_number, item.mobile_number, item.email_id, item.date_of_appointment, item.spouse_name, 
                            0, 1,empcode, empcode, DateTime.Now.ToString(), DateTime.Now.ToString(),1);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return excelDatatable;
        }
        
        public DataTable DuplicateRemoveExcelDataForWorkDetails(DataTable dt, string empcode)
        {

            DataTable excelDatatable = new DataTable();
            try
            {
                var empdetails = (from n in _db.tbl_employee_work_details
                                  select n).ToList();

                DateTime? CustomParseDateTime(object value)
                {
                    if (value == null || value is DBNull) return null;
                    return Convert.ToDateTime(value);
                }

                Int32? CustomParseInt(object value)
                {
                    if (value == null || value is DBNull) return null;
                    return Convert.ToInt32(value);
                }
                Boolean? CustomParseBoolean(object value)
                {
                    if (value == null || value is DBNull) return null;
                    else if (value.ToString() == "1") return true;
                    else if (value.ToString() == "0") return false;
                    return Convert.ToBoolean(value);
                }

                long? CustomParseLong(object value)
                {
                    if (value == null || value is DBNull) return null;
                    return Convert.ToInt64(value);
                }

                var employeeWorkDetails = (from DataRow dr in dt.Rows
                                           join emp in _db.tbl_employee_basic_details on Convert.ToInt64(dr["mobile_number"]) equals emp.mobile_number //&& dr["pan_number"] equals emp.pan_number
                                           join eg in _db.tbl_employee_group_master on  Convert.ToString(dr["ewd_group"]).Trim() equals eg.eg_group_desc.Trim()
                                           join desg in _db.tbl_designation_master on Convert.ToString(dr["ewd_designation"]).Trim() equals desg.d_designation_desc.Trim()
                                           join ddo in _db.tbl_ddo_master on Convert.ToString(dr["ewd_ddo"]).Trim() equals ddo.dm_ddo_code.Trim()
                                           join ps in _db.tbl_payscales_master on Convert.ToString(dr["ewd_payscale"]).Trim() equals Convert.ToString(Math.Round(Convert.ToDouble(ps.payscale_minimum)))+'-'+ Convert.ToString(Math.Round(Convert.ToDouble(ps.payscale_maximum)))
                                           join et in _db.tbl_employment_type_master on Convert.ToString(dr["ewd_employment_type"]).Trim() equals et.et_employee_type_desc
                                           select new VM_BasicDetails()
                                            {
                                                employee_id = emp.employee_id,
                                                ewd_date_of_joining = dr["ewd_date_of_joining"].ToString(),
                                                ewd_place_of_posting = Convert.ToString(dr["ewd_place_of_posting"]),
                                                ewd_employment_type = et.et_employee_type_id,
                                                ewd_payscale_id = ps.payscale_id,
                                                ewd_designation_id = desg.d_designation_id,
                                                ewd_group_id = eg.eg_group_id,
                                                ewd_ddo_id = ddo.dm_ddo_id,
                                           }).ToList();

                var finList = (from excelEmpList in employeeWorkDetails
                               join stuempList in empdetails
                               on excelEmpList.employee_id equals stuempList.ewd_emp_id
                               select excelEmpList).ToList();

                var fin1List = (from excelEmpList in employeeWorkDetails
                                select excelEmpList).Except(finList).ToList();
                if (fin1List.Count() > 0)
                {
                    excelDatatable.Columns.Add("ewd_emp_id", typeof(long));
                    excelDatatable.Columns.Add("ewd_date_of_joining", typeof(DateTime));
                    excelDatatable.Columns.Add("ewd_payscale_id", typeof(int));
                    excelDatatable.Columns.Add("ewd_employment_type", typeof(string));
                    excelDatatable.Columns.Add("ewd_designation_id", typeof(int));
                    excelDatatable.Columns.Add("ewd_group_id", typeof(int));
                    excelDatatable.Columns.Add("ewd_place_of_posting", typeof(string));
                    excelDatatable.Columns.Add("ewd_ddo_id", typeof(int));
                    excelDatatable.Columns.Add("ewd_created_by", typeof(long));
                    excelDatatable.Columns.Add("ewd_updated_by", typeof(long));
                    excelDatatable.Columns.Add("ewd_creation_datetime", typeof(DateTime));
                    excelDatatable.Columns.Add("ewd_updation_datetime", typeof(DateTime));
                    excelDatatable.Columns.Add("ewd_active_status", typeof(bool));

                    //For each item in your list, add those items
                    //in your data table
                    foreach (var item in fin1List)
                    {
                        excelDatatable.Rows.Add(item.employee_id, item.ewd_date_of_joining, item.ewd_payscale_id, item.ewd_employment_type, item.ewd_designation_id,
                            item.ewd_group_id, item.ewd_place_of_posting, item.ewd_ddo_id,empcode,empcode,DateTime.Now.ToString(),DateTime.Now.ToString(),1);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return excelDatatable;
        }

        public DataTable InsertPayDetailsMasterDetails(DataTable dt, string empcode)
        {
            DataTable excelDatatable = new DataTable();
            try
            {
                var employeeWorkDetails = (from DataRow dr in dt.Rows
                                           join emp in _db.tbl_employee_basic_details on Convert.ToInt64(dr["mobile_number"]) equals emp.mobile_number
                                           select new VM_BasicDetails()
                                           {
                                               employee_id = emp.employee_id,
                                               //hrms_employee_code = Convert.ToInt64(dr["hrms_employee_code"]),
                                               monthid = DateTime.Now.Month,
                                               yearid = DateTime.Now.Year,
                                               grosspay = dr["ewd_payscale"].ToString().Split('-')[0]
                                           }).ToList();
                if(employeeWorkDetails.Count > 0)
                {
                    excelDatatable.Columns.Add("hrms_emp_id", typeof(long));
                    //excelDatatable.Columns.Add("hrms_emp_code", typeof(long));
                    excelDatatable.Columns.Add("hrms_month_id", typeof(int));
                    excelDatatable.Columns.Add("hrms_year_id", typeof(int));
                    excelDatatable.Columns.Add("hrms_gross_pay", typeof(int));
                    excelDatatable.Columns.Add("hrms_active", typeof(bool));
                    excelDatatable.Columns.Add("hrms_creation_datetime", typeof(DateTime));
                    excelDatatable.Columns.Add("hrms_created_by", typeof(int));
                    excelDatatable.Columns.Add("hrms_updation_datetime", typeof(DateTime));
                    excelDatatable.Columns.Add("hrms_updated_by", typeof(int));

                    //For each item in your list, add those items
                    //in your data table
                    foreach (var item in employeeWorkDetails)
                    {
                        excelDatatable.Rows.Add(item.employee_id, item.monthid, item.yearid,
                            item.grosspay, 1, DateTime.Now.ToString(), item.employee_id, DateTime.Now.ToString(),item.employee_id);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return excelDatatable;
        }


        public List<VM_BasicDetails> GetEmployeeBasicData(long EmpID)
        {
            List<VM_BasicDetails> listBasicDetails = new List<VM_BasicDetails>();

            int ddoid = 0;
            ddoid = (from w in _db.tbl_employee_work_details
                     where w.ewd_emp_id == EmpID select w.ewd_ddo_id).FirstOrDefault();

            listBasicDetails = (from n in _db.tbl_employee_basic_details
                                join work in _db.tbl_employee_work_details on n.employee_id equals work.ewd_emp_id
                              join dept in _db.tbl_department_master on n.dept_employee_code equals dept.dm_dept_id
                              join gender in _db.tbl_gender_master on n.gender_id equals gender.gender_id
                              where (n.ddo_upload_status == false || n.ddo_upload_status == null) && n.active_status == true
                              && work.ewd_ddo_id == ddoid && n.employee_id != EmpID

                                orderby n.employee_id descending
                              select new VM_BasicDetails
                              {
                                  employee_id = n.employee_id,
                                  hrms_employee_code = n.hrms_employee_code,
                                  employee_name = n.employee_name,
                                  date_of_birth = n.date_of_birth.ToString(),
                                  gender_desc = gender.gender_desc,
                                  mobile_number = n.mobile_number,
                                  email_id = n.email_id,
                                  father_name = n.father_name,
                                  department = dept.dm_deptname_english
                              }).ToList();
            return listBasicDetails;
        }

        public VM_BasicDetails GetDDODetailsById(long empId)//NEW EMPLOYEE DETAILS AS PER DDO DETAILS
        {
            VM_BasicDetails employeeDetails = new VM_BasicDetails();
            tbl_employee_basic_details newEmployeeDetails = (from emp in _db.tbl_employee_basic_details where emp.employee_id == empId
                                                                 select emp).FirstOrDefault();

            tbl_employee_work_details employeeWorkDetails = (from emp in _db.tbl_employee_work_details
                                                             join basic in _db.tbl_employee_basic_details on emp.ewd_emp_id equals basic.employee_id
                                                             where basic.employee_id == empId
                                                             select emp).FirstOrDefault();
            
            if (newEmployeeDetails != null)
            {
                //employeeDetails.employee_id = newEmployeeDetails.employee_id;
                //employeeDetails.date_of_birth = string.Format("{0:dd-MM-yyyy}", newEmployeeDetails.date_of_birth);
                employeeDetails.dept_employee_code = newEmployeeDetails.dept_employee_code;
                //employeeDetails.email_id = newEmployeeDetails.email_id;
                //employeeDetails.father_name = newEmployeeDetails.father_name;
                //employeeDetails.gender =newEmployeeDetails.gender_id.ToString();
                //employeeDetails.activestatus = newEmployeeDetails.active_status.HasValue ? newEmployeeDetails.active_status.Value : false;
                //employeeDetails.mobile_number = newEmployeeDetails.mobile_number;
                //employeeDetails.employee_name = newEmployeeDetails.employee_name;
                //employeeDetails.pan_number = newEmployeeDetails.pan_number;
                //employeeDetails.place_of_birth = newEmployeeDetails.place_of_birth;
                //employeeDetails.date_of_appointment = string.Format("{0:dd-MM-yyyy}", newEmployeeDetails.date_of_appointment);
                //employeeDetails.spouse_name = newEmployeeDetails.spouse_name;
            }

            if (employeeWorkDetails != null)
            {
                //employeeDetails.ewd_date_of_joining = string.Format("{0:dd-MM-yyyy}", employeeWorkDetails.ewd_date_of_joining);
                employeeDetails.ewd_place_of_posting = employeeWorkDetails.ewd_place_of_posting ?? "";
                //employeeDetails.payscalecode = employeeWorkDetails.ewd_payscale_id.ToString();
                //employeeDetails.emptype = employeeWorkDetails.ewd_employment_type.ToString();
                //employeeDetails.designation = employeeWorkDetails.ewd_designation_id.ToString();
                //employeeDetails.group = employeeWorkDetails.ewd_group_id.ToString();
                employeeDetails.ewd_ddo_id = employeeWorkDetails.ewd_ddo_id;
            }
            employeeDetails.Genders = (from g in _db.tbl_gender_master
                                       where g.active_status == true
                                       select (new SelectListItem { Text = g.gender_desc, Value = g.gender_id.ToString() })).ToList();
            employeeDetails.Departments = (from d in _db.tbl_department_master
                                           where d.dm_active == true
                                           select (new SelectListItem { Text = d.dm_deptcode, Value = d.dm_dept_id.ToString() })).ToList();
            employeeDetails.PayscaleCodes = (from ps in _db.tbl_payscales_master
                                             where ps.payscale_status == 1
                                             select (new SelectListItem { Text = (ps.payscale_minimum).ToString() + "-" + (ps.payscale_maximum).ToString(), Value = ps.payscale_id.ToString() })).ToList();
            employeeDetails.EmploymentTypes = (from d in _db.tbl_employment_type_master
                                               where d.et_active == true
                                               select (new SelectListItem { Text = d.et_employee_type_desc, Value = d.et_employee_type_id.ToString() })).ToList();
            employeeDetails.Designations = (from d in _db.tbl_designation_master
                                            where  d.d_dept_id==newEmployeeDetails.dept_employee_code && d.d_status == 1 
                                            orderby d.d_designation_desc
                                            select (new SelectListItem { Text = d.d_designation_desc, Value = d.d_designation_id.ToString() })).ToList();
            employeeDetails.Groups = (from g in _db.tbl_employee_group_master
                                      where g.eg_active == true
                                      select (new SelectListItem { Text = g.eg_group_desc, Value = g.eg_group_id.ToString() })).ToList();
            employeeDetails.DDOCodes = (from g in _db.tbl_ddo_master
                                        where g.dm_active == true
                                        select (new SelectListItem { Text = g.dm_ddo_code, Value = g.dm_ddo_id.ToString() })).ToList();
            return employeeDetails;
        }
        public VM_BasicDetails GetEmployeeDetailsById(long empId)
        {
            VM_BasicDetails employeeDetails = new VM_BasicDetails();
            tbl_employee_basic_details newEmployeeDetails = (from emp in _db.tbl_employee_basic_details where emp.employee_id == empId
                                                                 select emp).FirstOrDefault();

            tbl_employee_work_details employeeWorkDetails = (from emp in _db.tbl_employee_work_details
                                                             join basic in _db.tbl_employee_basic_details on emp.ewd_emp_id equals basic.employee_id
                                                             where basic.employee_id == empId
                                                             select emp).FirstOrDefault();
            
            if (newEmployeeDetails != null)
            {
                employeeDetails.employee_id = newEmployeeDetails.employee_id;
                //employeeDetails.date_of_birth = string.Format("{0:dd-MM-yyyy}", newEmployeeDetails.date_of_birth);
                employeeDetails.dateofbirth =  newEmployeeDetails.date_of_birth;

                employeeDetails.department = newEmployeeDetails.dept_employee_code.ToString();
                employeeDetails.email_id = newEmployeeDetails.email_id;
                employeeDetails.father_name = newEmployeeDetails.father_name;
                employeeDetails.gender =newEmployeeDetails.gender_id.ToString();
                employeeDetails.activestatus = newEmployeeDetails.active_status.HasValue ? newEmployeeDetails.active_status.Value : false;
                employeeDetails.mobile_number = newEmployeeDetails.mobile_number;
                employeeDetails.employee_name = newEmployeeDetails.employee_name;
                employeeDetails.pan_number = newEmployeeDetails.pan_number;
                employeeDetails.place_of_birth = newEmployeeDetails.place_of_birth;
                //employeeDetails.date_of_appointment = string.Format("{0:dd-MM-yyyy}", newEmployeeDetails.date_of_appointment);
                employeeDetails.dateofappointment = newEmployeeDetails.date_of_appointment;


                employeeDetails.spouse_name = newEmployeeDetails.spouse_name;
                employeeDetails.father_name_kannada = newEmployeeDetails.father_name_kannada;
                employeeDetails.employee_name_kannada = newEmployeeDetails.employee_name_kannada;
                employeeDetails.spouse_name_kannada = newEmployeeDetails.spouse_name_kannada;
            }

            if (employeeWorkDetails != null)
            {
                //employeeDetails.ewd_date_of_joining = string.Format("{0:dd-MM-yyyy}", employeeWorkDetails.ewd_date_of_joining);
                employeeDetails.ewddateofjoining = employeeWorkDetails.ewd_date_of_joining;

                employeeDetails.ewd_place_of_posting = employeeWorkDetails.ewd_place_of_posting ?? "";
                employeeDetails.payscalecode = employeeWorkDetails.ewd_payscale_id.ToString();
                employeeDetails.emptype = employeeWorkDetails.ewd_employment_type.ToString();
                employeeDetails.designation = employeeWorkDetails.ewd_designation_id.ToString();
                employeeDetails.group = employeeWorkDetails.ewd_group_id.ToString();
                employeeDetails.ddocode = employeeWorkDetails.ewd_ddo_id.ToString();
            }
            employeeDetails.Genders = (from g in _db.tbl_gender_master
                                       where g.active_status == true
                                       select (new SelectListItem { Text = g.gender_desc, Value = g.gender_id.ToString() })).ToList();
            employeeDetails.Departments = (from d in _db.tbl_department_master
                                           where d.dm_active == true
                                           select (new SelectListItem { Text = d.dm_deptcode, Value = d.dm_dept_id.ToString() })).ToList();
            employeeDetails.PayscaleCodes = (from ps in _db.tbl_payscales_master
                                             where ps.payscale_status == 1
                                             select (new SelectListItem { Text = (ps.payscale_minimum).ToString() + "-" + (ps.payscale_maximum).ToString(), Value = ps.payscale_id.ToString() })).ToList();
            employeeDetails.EmploymentTypes = (from d in _db.tbl_employment_type_master
                                               where d.et_active == true
                                               select (new SelectListItem { Text = d.et_employee_type_desc, Value = d.et_employee_type_id.ToString() })).ToList();
            employeeDetails.Designations = (from d in _db.tbl_designation_master
                                            where d.d_dept_id == newEmployeeDetails.dept_employee_code && d.d_status == 1
                                            orderby d.d_designation_desc
                                            select (new SelectListItem { Text = d.d_designation_desc, Value = d.d_designation_id.ToString() })).ToList();
            employeeDetails.Groups = (from g in _db.tbl_employee_group_master
                                      where g.eg_active == true
                                      select (new SelectListItem { Text = g.eg_group_desc, Value = g.eg_group_id.ToString() })).ToList();
            employeeDetails.DDOCodes = (from g in _db.tbl_ddo_master
                                        where g.dm_active == true
                                        select (new SelectListItem { Text = g.dm_ddo_code, Value = g.dm_ddo_id.ToString() })).ToList();
            return employeeDetails;
        }
        public int DeleteUploadEmployeeDetails(long empId)
        {
            int result = 0;
            try
            {
                tbl_employee_basic_details objemp = (from e in _db.tbl_employee_basic_details
                                                     where e.employee_id == empId
                                                     select e).FirstOrDefault();
                _db.tbl_employee_basic_details.Remove(objemp);
                _db.SaveChanges();
                tbl_employee_work_details objwork = (from w in _db.tbl_employee_work_details
                                                     where w.ewd_emp_id == empId
                                                     select w).FirstOrDefault();
                _db.tbl_employee_work_details.Remove(objwork);
                _db.SaveChanges();
                tbl_hrms_pay_details_master objpay = (from h in _db.tbl_hrms_pay_details_master
                                                      where h.hrms_emp_id == empId
                                                      select h).FirstOrDefault();
                _db.tbl_hrms_pay_details_master.Remove(objpay);
                _db.SaveChanges();
                result = 1;
            }
            catch(Exception ex)
            {
                result = 0;
            }
            return result;
        }
    }
}
