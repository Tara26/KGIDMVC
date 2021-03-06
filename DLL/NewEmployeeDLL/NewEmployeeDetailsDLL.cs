using KGID_Models.KGID_User;
using System.Linq;
using DLL.DBConnection;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Data;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using KGID_Models.KGID_VerifyData;
using System.Data.SqlClient;
using KGID_Models.NBApplication;
using KGID_Models.KGID_Policy;

namespace DLL.NewEmployeeDLL
{
    public class NewEmployeeDetailsDLL : INewEmployeeDetailsDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();
        tbl_new_employee_basic_details newempdetails;
        public IEnumerable<tbl_new_employee_basic_details> NewEmpDetailsdll(tbl_new_employee_basic_details _newemp)
        {
            var empdetails = (from n in _db.tbl_new_employee_basic_details
                              select n).ToList();
            return empdetails;
        }


        public tbl_dept_master DeptName(string _deptname)
        {
            var empdetails = (from n in _db.tbl_department_master
                              where n.dm_deptcode == newempdetails.nebd_dept_emp_code && n.dm_deptname_english == _deptname
                              select n).FirstOrDefault();
            return empdetails;
        }

        public tbl_new_employee_basic_details GetMobileAndEmailByDeptCode(string _ddoCode, long _empCode)
        {
            var empdetails = (from n in _db.tbl_new_employee_basic_details
                              where n.nebd_ddo_code == _ddoCode && n.nebd_sys_emp_code == _empCode
                              select n).FirstOrDefault();
            return empdetails;
        }

        public IEnumerable<tbl_new_employee_basic_details> GetEmpNameByDeptCode(string _ddoCode, long _empCode)
        {
            var empdetails = (from n in _db.tbl_new_employee_basic_details
                              where n.nebd_ddo_code == _ddoCode && n.nebd_sys_emp_code == _empCode
                              select n).ToList();
            return empdetails;
        }

        public IEnumerable<tbl_new_employee_basic_details> LoadEmployeeNamesByDDOCode(string _ddoCode)
        {
            var empdetails = (from n in _db.tbl_new_employee_basic_details
                              where n.nebd_ddo_code == _ddoCode
                              select n).ToList();
            return empdetails;
        }

        public IEnumerable<tbl_new_employee_basic_details> GetEmployeeBasicData()
        {
            var empdetails = (from n in _db.tbl_new_employee_basic_details
                              select n).ToList();
            return empdetails;
        }

        //public tbl_new_employee_basic_details VerifyKGIDAndMobileNo(long kgId, string mobileNumber)
        //{
        //    var IEmployeeDtls = (from n in _db.tbl_new_employee_basic_details
        //                         where n.nebd_sys_emp_code == kgId && n.nebd_mobilenumber.Equals(mobileNumber)
        //                         select n).FirstOrDefault();
        //    return IEmployeeDtls;
        //}

        public tbl_challan_details AddChallanDetails(int amount)
        {
            tbl_challan_details challanDetails = new tbl_challan_details();
            challanDetails.cd_amount = amount;
            challanDetails.cd_challan_ref_no = "0";

            var refNo = (from n in _db.tbl_challan_details
                         orderby 1 descending
                         select n.cd_id).FirstOrDefault();

            if (refNo > 0)
            {
                challanDetails.cd_challan_ref_no = (refNo + 1).ToString();
            }
            else
            {
                challanDetails.cd_challan_ref_no = "1";
            }
            var challanDetails1 = _db.tbl_challan_details.Add(challanDetails);
            _db.SaveChanges();
            return challanDetails1;
        }
        public tbl_challan_details UpdateChallanDetails(tbl_challan_details chaDetails)
        {
            if (chaDetails != null)
            {
                var challanaDetails = (from n in _db.tbl_challan_details
                                       where n.cd_id == chaDetails.cd_id
                                       select n).FirstOrDefault();
                if (challanaDetails != null)
                {
                    challanaDetails.cd_amount = chaDetails.cd_amount;
                    challanaDetails.cd_challan_ref_no = chaDetails.cd_challan_ref_no;
                    challanaDetails.cd_datetime_of_challan = chaDetails.cd_datetime_of_challan;
                    _db.SaveChanges();
                }
            }
            return chaDetails;
        }

        public tbl_payment_status_details AddPaymentStatus(tbl_payment_status_details payDetails)
        {
            if (payDetails != null)
            {
                _db.tbl_payment_status_details.Add(payDetails);
                _db.SaveChanges();
            }
            return payDetails;
        }
        public tbl_challan_details FindChallanDetailsById(long Id)
        {
            if (Id > 0)
            {
                var challanDeatails = (from n in _db.tbl_challan_details
                                       where n.cd_id == Id
                                       select n).FirstOrDefault();
                return challanDeatails;
            }
            return null;

        }

        public tbl_new_employee_basic_details GetEmployeeBasicData(long empId)
        {
            return (from emp in _db.tbl_new_employee_basic_details
                    where emp.nebd_sys_emp_code == empId
                    select emp).FirstOrDefault();
        }

        //public string GetMobileNumberByKGID(long kgId)
        //{
        //    var IEmployeeDtls = (from n in _db.tbl_new_employee_basic_details
        //                         where n.nebd_sys_emp_code == kgId
        //                         select n.nebd_mobilenumber).FirstOrDefault();
        //    return IEmployeeDtls;
        //}

        //public tbl_new_employee_basic_details GetEmployeeByMobileNumber(string mobileNumber)
        //{
        //    var empdetails = (from n in _db.tbl_new_employee_basic_details
        //                      where n.nebd_mobilenumber == mobileNumber
        //                      select n).FirstOrDefault();
        //    return empdetails;
        //}

        //public tbl_new_employee_basic_details GetEmployeeByEmail(string email)
        //{
        //    var empdetails = (from n in _db.tbl_new_employee_basic_details
        //                      where n.nebd_email == email
        //                      select n).FirstOrDefault();
        //    return empdetails;
        //}

        public bool GetEmployeeStatusDll(long EmpId)
        {
            DataSet dsES = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@employee_id",EmpId)
            };

            dsES = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getEmployeeStatus");
            if (dsES.Tables[0].Rows.Count > 0)
            {
                //    employeeLoginDetail.EmployeeId = Convert.ToInt64(dsBD.Tables[0].Rows[0]["employee_id"].ToString());
                //    employeeLoginDetail.EmployeeName = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                //    employeeLoginDetail.UserCategory = dsBD.Tables[0].Rows[0]["user_category_id"].ToString();
                return true;
            }
            return false;
        }

        public VM_InsuredEmployeeLoginDetail VerifyKGIDAndMobileNo(string kgId, long mobileNumber)
        {
            VM_InsuredEmployeeLoginDetail employeeLoginDetail = new VM_InsuredEmployeeLoginDetail();
            DataSet dsBD = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@kgId",kgId),
                new SqlParameter("@mobileNumber", mobileNumber)
            };

            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectInsuredEmployeeLoginDetail");
            if (dsBD.Tables[0].Rows.Count > 0)
            {
                employeeLoginDetail.EmployeeId = Convert.ToInt64(dsBD.Tables[0].Rows[0]["employee_id"].ToString());
                employeeLoginDetail.EmployeeName = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                employeeLoginDetail.UserCategory = dsBD.Tables[0].Rows[0]["user_category_id"].ToString();
                employeeLoginDetail.Department = dsBD.Tables[0].Rows[0]["dm_deptname_english"].ToString();
                employeeLoginDetail.Designation = dsBD.Tables[0].Rows[0]["d_designation_desc"].ToString();
                employeeLoginDetail.FirstKGIDNo = dsBD.Tables[0].Rows[0]["first_kgid_policy_no"].ToString();
                employeeLoginDetail.Email = dsBD.Tables[0].Rows[0]["email_id"].ToString();
            }

            return employeeLoginDetail;
        }

        public VM_NewEmployeeLoginDetail GetEmployeeByEmail(string email)
        {
            VM_NewEmployeeLoginDetail employeeLoginDetail = new VM_NewEmployeeLoginDetail();
            DataSet dsBD = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@email",email)
            };

            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_verifyByEmailNewEmployeeLoginDetail");
            if (dsBD.Tables[0].Rows.Count > 0)
            {
                employeeLoginDetail.EmployeeId = Convert.ToInt64(dsBD.Tables[0].Rows[0]["employee_id"].ToString());
                employeeLoginDetail.EmployeeName = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                employeeLoginDetail.UserCategory = dsBD.Tables[0].Rows[0]["user_category_id"].ToString();
                employeeLoginDetail.Department = dsBD.Tables[0].Rows[0]["dm_deptname_english"].ToString();
                employeeLoginDetail.Designation = dsBD.Tables[0].Rows[0]["d_designation_desc"].ToString();
                employeeLoginDetail.FirstKGIDNumber = dsBD.Tables[0].Rows[0]["first_kgid_policy_no"].ToString();
                employeeLoginDetail.Email= dsBD.Tables[0].Rows[0]["email_id"].ToString();
            }

            return employeeLoginDetail;
        }

        public long GetMobileNumberByKGID(long kgId)
        {
            VM_InsuredEmployeeLoginDetail employeeLoginDetail = new VM_InsuredEmployeeLoginDetail();
            DataSet dsBD = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@kgId",kgId)
            };

            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_verifyKGIDEmployeeLoginDetail");
            if (dsBD.Tables[0].Rows.Count > 0)
            {
                var mobile = dsBD.Tables[0].Rows[0]["mobile_number"].ToString();
                employeeLoginDetail.MobileNumber = (mobile=="")?0:Convert.ToInt64(dsBD.Tables[0].Rows[0]["mobile_number"].ToString());
            }

            return employeeLoginDetail.MobileNumber;
        }

        public VM_NewEmployeeLoginDetail GetEmployeeByMobileNumber(long mobileNumber)
        {
            VM_NewEmployeeLoginDetail employeeLoginDetail = new VM_NewEmployeeLoginDetail();
            DataSet dsBD = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@mobileNumber",mobileNumber)
            };

            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_verifyByMobileNumberNewEmployeeLoginDetail");
            if (dsBD.Tables[0].Rows.Count > 0)
            {
                employeeLoginDetail.EmployeeId = Convert.ToInt64(dsBD.Tables[0].Rows[0]["employee_id"].ToString());
                employeeLoginDetail.EmployeeName = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                employeeLoginDetail.UserCategory = dsBD.Tables[0].Rows[0]["user_category_id"].ToString();
                employeeLoginDetail.Department = dsBD.Tables[0].Rows[0]["dm_deptname_english"].ToString();
                employeeLoginDetail.Designation = dsBD.Tables[0].Rows[0]["d_designation_desc"].ToString();
                employeeLoginDetail.FirstKGIDNumber = dsBD.Tables[0].Rows[0]["first_kgid_policy_no"].ToString();

            }

            return employeeLoginDetail;
        }

        public VM_BasicDetails GetEmployeeData(string kgidnum)
        {
            
            VM_BasicDetails objDetl = new VM_BasicDetails();
            DataSet dsBD = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@kgidnumber",kgidnum)
            };
            dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_getEmployeeDetails");
            if (dsBD.Tables[0].Rows.Count > 0)
            {
                objDetl.first_kgid_policy_no = dsBD.Tables[0].Rows[0]["first_kgid_policy_no"].ToString();
                objDetl.employee_name = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                objDetl.designation= dsBD.Tables[0].Rows[0]["d_designation_desc"].ToString();
                objDetl.department= dsBD.Tables[0].Rows[0]["dm_deptname_english"].ToString();
                objDetl.user_category_id= dsBD.Tables[0].Rows[0]["user_category_id"].ToString();
                
            }
            return objDetl;
        }
         DataTable dtSaveEmpCategoryData = new DataTable();
        public void SaveCategoryDetailsData(int? DistID, int CategoryID, int DDOCodeID, long EmpId, bool status, int ModuleType)
        {
            try
            {
                DataRow Ddr = dtSaveEmpCategoryData.NewRow();
                Ddr["ap_dist_id"] = DistID;
                Ddr["ap_category_id"] = CategoryID;
                Ddr["ap_ddocode_id"] = DDOCodeID;
                Ddr["ap_emp_id"] = EmpId;
                Ddr["ap_active_status"] = status;
                Ddr["ModuleType"] = ModuleType;
                dtSaveEmpCategoryData.Rows.Add(Ddr);
            }
            catch (Exception ex)
            {

            }
        }
        public int SaveCategoryDetailsDll (long kgidmun, int ddo,int cw, int avgcw, int sup,int dio, int dd, int d, int AD, int nb, int loan, int claims, int motor, int odclaims,int mvcclaims, long empId)
        {
            bool temp = true;
            int result = 0;
            if (ddo==1)
            {
                var UserDtls = (from ebd in _db.tbl_employee_basic_details.Where(n => n.first_kgid_policy_no == kgidmun && n.active_status == true)
                                join ewd in _db.tbl_employee_work_details on ebd.employee_id equals ewd.ewd_emp_id
                                select ewd).FirstOrDefault();
                var DDOlist = (from ebd in _db.tbl_employee_basic_details.Where(n => n.user_category_id.Contains(",2") && n.active_status == true && n.first_kgid_policy_no != kgidmun)
                               join ewd in _db.tbl_employee_work_details.Where(m => m.ewd_ddo_id == UserDtls.ewd_ddo_id) on ebd.employee_id equals ewd.ewd_emp_id
                               select ebd).FirstOrDefault();
                if(DDOlist!=null)
                {
                    temp = false;
                }
                
            }
            if (temp)
            {


                var Res = _db.tbl_employee_basic_details.
                    Join(_db.tbl_employee_work_details, u => u.employee_id, uir => uir.ewd_emp_id,
                        (u, uir) => new { u, uir }).
                    Join(_db.tbl_ddo_master, r => r.uir.ewd_ddo_id, ro => ro.dm_ddo_id, (r, ro) => new { r, ro }).Where(m => m.r.u.first_kgid_policy_no == kgidmun).Select(m => new RoleAssignCategoryModuleType
                    {
                        DDOID = m.r.uir.ewd_ddo_id,
                        DistictID = m.ro.dm_district_id,
                        EmpID = m.r.u.employee_id
                    }).FirstOrDefault();

                dtSaveEmpCategoryData.Columns.Add("ap_dist_id");
                dtSaveEmpCategoryData.Columns.Add("ap_category_id");
                dtSaveEmpCategoryData.Columns.Add("ap_ddocode_id");
                dtSaveEmpCategoryData.Columns.Add("ap_emp_id");
                dtSaveEmpCategoryData.Columns.Add("ap_active_status");
                dtSaveEmpCategoryData.Columns.Add("ModuleType");

                DataRow Ddr = dtSaveEmpCategoryData.NewRow();
                Ddr["ap_dist_id"] = Res.DistictID;
                Ddr["ap_category_id"] =4;
                Ddr["ap_ddocode_id"] = Res.DDOID;
                Ddr["ap_emp_id"] = Res.EmpID;
                Ddr["ap_active_status"] = true;
                Ddr["ModuleType"] = 4;
                dtSaveEmpCategoryData.Rows.Add(Ddr);

                if (cw == 1)
                {
                    if (nb == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 3, Res.DDOID, Res.EmpID, true, 1);
                    }
                    if (loan == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 3, Res.DDOID, Res.EmpID, true, 2);
                    }
                    if (claims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 3, Res.DDOID, Res.EmpID, true, 3);
                    }
                    if (motor == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 3, Res.DDOID, Res.EmpID, true, 4);
                       
                    }
                    if (odclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 3, Res.DDOID, Res.EmpID, true, 5);
                    }
                    if (mvcclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 3, Res.DDOID, Res.EmpID, true, 6);
                    }
                    else if (nb == 0 && loan == 0 && claims == 0 && motor == 0 && odclaims == 0 && mvcclaims == 0)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 3, Res.DDOID, Res.EmpID, true, 0);
                    }
                }
                if (avgcw == 1)
                {
                    if (nb == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 10, Res.DDOID, Res.EmpID, true, 1);
                    }
                    if (loan == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 10, Res.DDOID, Res.EmpID, true, 2);
                    }
                    if (claims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 10, Res.DDOID, Res.EmpID, true, 3);
                    }
                    if (motor == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 10, Res.DDOID, Res.EmpID, true, 4);
                    }
                    if (odclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 10, Res.DDOID, Res.EmpID, true, 5);
                    }
                    if (mvcclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 10, Res.DDOID, Res.EmpID, true, 6);
                    }
                    else if (nb == 0 && loan == 0 && claims == 0 && motor == 0 && odclaims == 0 && mvcclaims == 0)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 10, Res.DDOID, Res.EmpID, true, 0);
                    }
                }
                if (sup == 1)
                {
                    if (nb == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 4, Res.DDOID, Res.EmpID, true, 1);
                    }
                    if (loan == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 4, Res.DDOID, Res.EmpID, true, 2);
                    }
                    if (claims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 4, Res.DDOID, Res.EmpID, true, 3);
                    }
                    if (motor == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 4, Res.DDOID, Res.EmpID, true, 4);
                       
                    }
                    if (odclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 4, Res.DDOID, Res.EmpID, true, 5);
                    }
                    if (mvcclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 4, Res.DDOID, Res.EmpID, true, 6);
                    }
                    else if (nb == 0 && loan == 0 && claims == 0 && motor == 0 && odclaims == 0 && mvcclaims == 0)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 4, Res.DDOID, Res.EmpID, true, 0);
                    }
                    //else
                    //{
                    //    SaveCategoryDetailsData(Res.DistictID, cw, Res.DDOID, Res.EmpID, true, null);
                    //}
                }
                if (dio == 1)
                {
                    if (nb == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 5, Res.DDOID, Res.EmpID, true, 1);
                    }
                    if (loan == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 5, Res.DDOID, Res.EmpID, true, 2);
                    }
                    if (claims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 5, Res.DDOID, Res.EmpID, true, 3);
                    }
                    if (motor == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 5, Res.DDOID, Res.EmpID, true, 4);
                      
                    }
                    if (odclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 5, Res.DDOID, Res.EmpID, true, 5);
                    }
                    if (mvcclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 5, Res.DDOID, Res.EmpID, true, 6);
                    }
                    else if (nb == 0 && loan == 0 && claims == 0 && motor == 0 && odclaims == 0 && mvcclaims == 0)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 5, Res.DDOID, Res.EmpID, true, 0);
                    }
                }
                if (AD == 1)
                {
                    if (nb == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 15, Res.DDOID, Res.EmpID, true, 1);
                    }
                    if (loan == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 15, Res.DDOID, Res.EmpID, true, 2);
                    }
                    if (claims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 15, Res.DDOID, Res.EmpID, true, 3);
                    }
                    if (motor == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 15, Res.DDOID, Res.EmpID, true, 4);

                    }
                    if (odclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 15, Res.DDOID, Res.EmpID, true, 5);
                    }
                    if (mvcclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 15, Res.DDOID, Res.EmpID, true, 6);
                    }
                    else if (nb == 0 && loan == 0 && claims == 0 && motor == 0 && odclaims == 0 && mvcclaims == 0)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 15, Res.DDOID, Res.EmpID, true, 0);
                    }
                }
                if (dd == 1)
                {
                    if (nb == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 6, Res.DDOID, Res.EmpID, true, 1);
                    }
                    if (loan == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 6, Res.DDOID, Res.EmpID, true, 2);
                    }
                    if (claims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 6, Res.DDOID, Res.EmpID, true, 3);
                    }
                    if (motor == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 6, Res.DDOID, Res.EmpID, true, 4);
                    }
                    if (odclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 6, Res.DDOID, Res.EmpID, true, 5);
                    }
                    if (mvcclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 6, Res.DDOID, Res.EmpID, true, 6);
                    }
                    else if (nb == 0 && loan == 0 && claims == 0 && motor == 0 && odclaims == 0 && mvcclaims == 0)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 6, Res.DDOID, Res.EmpID, true, 0);
                    }
                }
                if (d == 1)
                {
                    if (nb == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 7, Res.DDOID, Res.EmpID, true, 1);
                    }
                    if (loan == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 7, Res.DDOID, Res.EmpID, true, 2);
                    }
                    if (claims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 7, Res.DDOID, Res.EmpID, true, 3);
                    }
                    if (motor == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 7, Res.DDOID, Res.EmpID, true, 4);
                    }
                    if (odclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 7, Res.DDOID, Res.EmpID, true, 5);
                    }
                    if (mvcclaims == 1)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 7, Res.DDOID, Res.EmpID, true, 6);
                    }
                    else if (nb == 0 && loan == 0 && claims == 0 && motor == 0 && odclaims == 0 && mvcclaims == 0)
                    {
                        SaveCategoryDetailsData(Res.DistictID, 7, Res.DDOID, Res.EmpID, true, 0);
                    }
                }
               
                int result1 = 0;
                try
                {
                    SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@kgidmun",kgidmun),
                    new SqlParameter("@ddo",ddo),
                    new SqlParameter("@cw",cw),
                    new SqlParameter("@avgcw",avgcw),
                    new SqlParameter("@sup",sup),
                    new SqlParameter("@dio",dio),
                    new SqlParameter("@dd",dd),
                    new SqlParameter("@d",d),
                    new SqlParameter("@Ad",AD)
                };
                    result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_ADDRole"));
                    SqlParameter[] sqlparam1 =
                    {
                    new SqlParameter("@eid",empId),
                    //moduletype
                    new SqlParameter("@EmpID",Res.EmpID),
                    new SqlParameter("@SaveEmpRoleCategoryData",dtSaveEmpCategoryData)
                };
                    result1 = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam1, "sp_kgid_AssignApplicationRole"));

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }
        //NB Challan Print Details
        public VM_ChallanPrintDetails ChallanprintDetailsDLL(long EmpId, long AppId)
        {
            VM_ChallanPrintDetails NBChallanDetails = new VM_ChallanPrintDetails();
            DataSet details = new DataSet();

            SqlParameter[] parms = {
              new SqlParameter("@empId",EmpId),
              new SqlParameter("@applicationId",AppId),
            };
            details = _Conn.ExeccuteDataset(parms, "sp_kgid_NB_Print_PaymentDetails");
            if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
            {
                NBChallanDetails.dm_ddo_code = details.Tables[0].Rows[0]["dm_ddo_code"].ToString();
                NBChallanDetails.dm_ddo_office = details.Tables[0].Rows[0]["dm_ddo_office"].ToString();
                NBChallanDetails.dm_deptname_english = details.Tables[0].Rows[0]["dm_deptname_english"].ToString();
                NBChallanDetails.dm_name_english = details.Tables[0].Rows[0]["dm_name_english"].ToString();
                NBChallanDetails.employee_name = details.Tables[0].Rows[0]["employee_name"].ToString();
                NBChallanDetails.ead_address = details.Tables[0].Rows[0]["ead_address"].ToString();
                NBChallanDetails.mobile_number = details.Tables[0].Rows[0]["mobile_number"].ToString();
                NBChallanDetails.hoa_desc = details.Tables[0].Rows[0]["hoa_desc"].ToString();
                NBChallanDetails.purpose_id = details.Tables[0].Rows[0]["purpose_id"].ToString();
                NBChallanDetails.purpose_desc = details.Tables[0].Rows[0]["purpose_desc"].ToString();
                NBChallanDetails.sub_purpose_desc = details.Tables[0].Rows[0]["sub_purpose_desc"].ToString();
                NBChallanDetails.p_premium = (details.Tables[0].Rows[0]["p_premium"] == DBNull.Value) ? (double?)0 : Convert.ToDouble((details.Tables[0].Rows[0]["p_premium"]));
                NBChallanDetails.LastUpdatedDateTime = Convert.ToDateTime(details.Tables[0].Rows[0]["p_updation_datetime"].ToString());
                NBChallanDetails.challan_ref_no = details.Tables[0].Rows[0]["cd_challan_ref_no"].ToString();
                NBChallanDetails.challan_date = details.Tables[0].Rows[0]["cd_date"].ToString();
                NBChallanDetails.challan_status = details.Tables[0].Rows[0]["csm_status"].ToString();
                NBChallanDetails.ApplicationReferenceNo= details.Tables[0].Rows[0]["kad_kgid_application_number"].ToString();
            }
            return NBChallanDetails;
        }
    }
}
