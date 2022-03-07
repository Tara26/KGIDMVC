using Common;
using DLL.DBConnection;
using KGID_Models.Dashboard;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_User;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDEmployee;
//using KGID_Models.KGIDLoan;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace DLL.NewEmployeeDLL
{
    public class NBApplicationDll : INBApplicationDll
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();
        private readonly AllCommon _commnobj = new AllCommon();
        private string UploadDocument(HttpPostedFileBase document, long empId, string docType)
        {
            string subPath = string.Empty;
            try
            {
                if (document != null && document.ContentLength > 0)
                {
                    var supportedTypes = new[] { "pdf" };
                    var fileExt = System.IO.Path.GetExtension(document.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        return "File Extension Is InValid - Only Upload PDF File";

                    }
                    string fileName = Path.GetFileName(document.FileName);
                    if (docType == "Medical Others")
                    {
                        //subPath = @"\\10.10.132.33\DOCUMENTS\Medical Others\";// + empId.ToString() + "/" + docType;
                        if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                        {
                            subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"MedicalOthers\";
                        }
                    }
                    else if (docType == "MedicalLeave")
                    {
                        //subPath = @"\\10.10.132.33\DOCUMENTS\MedicalLeave\";
                        if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                        {
                            subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"MedicalLeave\";
                        }
                    }
                    else if (docType == "Health Opinion")
                    {
                        //subPath = @"\\10.10.132.33\DOCUMENTS\Health Opinion\";
                        if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                        {
                            subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"HealthOpinion\";
                        }
                    }
                    else if (docType == "PolicyGeneration")
                    {
                        //subPath = @"\\10.10.132.33\DOCUMENTS\Health Opinion\";
                        if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                        {
                            subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"PolicyGeneration\";
                        }
                    }
                    else
                    {
                        //subPath = @"\\10.10.132.33\DOCUMENTS\Medical Health\";
                        if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                        {
                            subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"MedicalHealth\";
                        }
                    }

                    //string path = Path.Combine(HttpContext.Current.Server.MapPath(subPath), fileName);
                    string FileNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                    //string path = subPath + empId.ToString() + FileNo + fileName;
                    string path = Path.Combine(subPath, empId.ToString() + FileNo + fileName);

                    document.SaveAs(path);
                    subPath = path;// subPath + "/" + fileName;
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "UploadDocument1" + ex.Message);
            }
            return subPath;
        }

        public void SaveDeatils(VM_PersonalDetails objPersonal)
        {

            tbl_personal_disease_details objPDDetails = new tbl_personal_disease_details();
            if (objPersonal.IsMedicalAdvice == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 1;
                objPDDetails.pdd_details = objPersonal.MedicalAdviceDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsFamilyMemberAffectedByDisease == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 2;
                objPDDetails.pdd_details = objPersonal.FamilyMemberAffectedByDiseaseDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsInfectiousDiseaseHouse == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 3;
                objPDDetails.pdd_details = objPersonal.InfectiousDiseaseHouseDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsBrainDisease == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 4;
                objPDDetails.pdd_details = objPersonal.BrainDiseaseDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsDiseaseOfLungs == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 5;
                objPDDetails.pdd_details = objPersonal.DiseaseOfLungsDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsLiverKidneyDisease == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 6;
                objPDDetails.pdd_details = objPersonal.LiverKidneyDiseaseDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsStomachDisease == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 7;
                objPDDetails.pdd_details = objPersonal.StomachDiseaseDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsRheumaticFever == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 8;
                objPDDetails.pdd_details = objPersonal.RheumaticFeverDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsUrineChecked == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 9;
                objPDDetails.pdd_details = objPersonal.UrineCheckedDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsAnyOtherDisease == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 10;
                objPDDetails.pdd_details = objPersonal.AnyOtherDiseaseDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsDrinksDrugs == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 11;
                objPDDetails.pdd_details = objPersonal.DrinksDrugsDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsAbsent == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 12;
                objPDDetails.pdd_details = objPersonal.AbsentDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsPlaceChange == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 13;
                objPDDetails.pdd_details = objPersonal.PlaceChangeDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsProposalMade == true)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 14;
                objPDDetails.pdd_details = string.Empty;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);

                if (objPersonal.ProposalAccepted)
                {
                    objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                    objPDDetails.pdd_personal_health_code = 15;
                    objPDDetails.pdd_details = objPersonal.ProposalAcceptedDetails;
                    objPDDetails.pdd_creation_datetime = DateTime.Now;
                    objPDDetails.pdd_active = true;
                    _db.tbl_personal_disease_details.Add(objPDDetails);
                }
                if (objPersonal.ProposalPostponed)
                {
                    objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                    objPDDetails.pdd_personal_health_code = 16;
                    objPDDetails.pdd_details = objPersonal.ProposalPostponedDetails;
                    objPDDetails.pdd_creation_datetime = DateTime.Now;
                    objPDDetails.pdd_active = true;
                    _db.tbl_personal_disease_details.Add(objPDDetails);
                }
                if (objPersonal.ProposalDeclined)
                {
                    objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                    objPDDetails.pdd_personal_health_code = 17;
                    objPDDetails.pdd_details = objPersonal.ProposalDeclinedDetails;
                    objPDDetails.pdd_creation_datetime = DateTime.Now;
                    objPDDetails.pdd_active = true;
                    _db.tbl_personal_disease_details.Add(objPDDetails);
                }
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 18;
                objPDDetails.pdd_details = objPersonal.OrganizationOrPolicyNumber;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);

                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 19;
                objPDDetails.pdd_details = objPersonal.PolicyOrProposalNumber;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsPurdha)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 20;
                objPDDetails.pdd_details = string.Empty;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
            if (objPersonal.IsMarried)
            {
                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 21;
                objPDDetails.pdd_details = string.Empty;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);

                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 22;
                objPDDetails.pdd_details = objPersonal.MarriedTenureDetails;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);

                if (objPersonal.IsHusbandKGWorker)
                {
                    objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                    objPDDetails.pdd_personal_health_code = 23;
                    objPDDetails.pdd_details = string.Empty;
                    objPDDetails.pdd_creation_datetime = DateTime.Now;
                    objPDDetails.pdd_active = true;
                    _db.tbl_personal_disease_details.Add(objPDDetails);
                }

                objPDDetails.pdd_sys_emp_code = objPersonal.EmpCode;
                objPDDetails.pdd_personal_health_code = 24;
                objPDDetails.pdd_details = objPersonal.OccupationAndAddress;
                objPDDetails.pdd_creation_datetime = DateTime.Now;
                objPDDetails.pdd_active = true;
                _db.tbl_personal_disease_details.Add(objPDDetails);
                _db.SaveChanges();
            }
        }
        public long SaveNBDeclaration(tbl_nb_declaration_master objDecl)
        {
            long _identity = 0;

            var _DeclarationDetails = (from n in _db.tbl_nb_declaration_master
                                       where n.ndm_declaration_code == objDecl.ndm_declaration_code
                                       select n).FirstOrDefault();
            //var _RefDetails = (from n in _db.tbl_application_referenceno_details
            //                   where n.ard_system_emp_code == objDecl.ndm_declaration_code
            //                   select n).FirstOrDefault();
            if (_DeclarationDetails != null)
            {
                _DeclarationDetails.ndm_declaration_status = objDecl.ndm_declaration_status;
                _DeclarationDetails.ndm_updation_datetime = DateTime.Now;
                _db.SaveChanges();
                _identity = 2;// Convert.ToInt64(_RefDetails.ard_application_reference_no);
            }
            else
            {
                //string RefNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                //tbl_application_referenceno_details objRefno = new tbl_application_referenceno_details();
                //objRefno.ard_system_emp_code = objDecl.ndm_declaration_code;
                //objRefno.ard_application_reference_no = Convert.ToInt64(RefNo);
                //objRefno.ard_creation_datetime = DateTime.Now;
                ////objRefno.ard_created_by= objEmpForm.App_Employee_Code;
                //objRefno.ard_datetime = DateTime.Now;
                //objRefno.ard_active_status = true;
                //objRefno.ard_submission_status = 1;
                //_db.tbl_application_referenceno_details.Add(objRefno);

                _db.tbl_nb_declaration_master.Add(objDecl);
                _db.SaveChanges();
                _identity = 1;// Convert.ToInt64(RefNo);
            }
            // objPayscale.fm_registration_id;
            return _identity;
        }
        public tbl_new_employee NEBasicDetailsDll(long EmployeeCode)
        {
            var BasicDtls = (from n in _db.tbl_new_employee
                             join _Deg in _db.tbl_designation on n.em_designation equals _Deg.d_id

                             //where n.em_employeecode == EmployeeCode
                             select n
                             //select new tbl_user_master
                             //{
                             //    um_emp_code = n.um_emp_code,
                             //    um_emp_name = n.um_emp_name,
                             //    um_dob = n.um_dob,
                             //    um_email = n.um_email,
                             //    um_father_name = n.um_father_name,
                             //    um_mother_name = n.um_mother_name,
                             //    um_designation = n.um_designation,
                             //    um_working_office = n.um_working_office,
                             //    um_joining_date = n.um_joining_date,
                             //    um_phone = n.um_phone,
                             //    um_birth_place = n.um_birth_place,
                             //    um_job_type = n.um_job_type,
                             //    um_pay_scale = n.um_pay_scale
                             //}
                             ).FirstOrDefault();
            return BasicDtls;
        }

        public tbl_employee_work_details GetEmployeeWorkDetailsByEmployeeId(long empId)
        {
            return (from emp in _db.tbl_employee_work_details
                    where emp.ewd_emp_id == empId
                    select emp).FirstOrDefault();
        }
        public tbl_new_employee_basic_details GetEmployeeDetailsByEmployeeId(long empId)
        {
            return (from emp in _db.tbl_new_employee_basic_details
                    where emp.nebd_sys_emp_code == empId
                    select emp).FirstOrDefault();
        }

        public decimal CalculateKGIDPremium(decimal AVGPAY, decimal PAYSCL)
        {
            return ((AVGPAY * PAYSCL) / 100);
        }

        public tbl_nb_declaration_master DeclarationDetailsDll(long EmployeeCode)
        {
            var DeclartaionDtls = (from n in _db.tbl_nb_declaration_master

                                   where n.ndm_declaration_code == EmployeeCode
                                   select n).FirstOrDefault();
            var RefDtls = (from RNo in _db.tbl_application_referenceno_details

                           where RNo.ard_system_emp_code == EmployeeCode
                           select RNo).FirstOrDefault();
            if (DeclartaionDtls != null && RefDtls != null)
            {
                DeclartaionDtls.ndm_referance_no = RefDtls.ard_application_reference_no;
            }

            return DeclartaionDtls;
        }


        private string GetLoadFactor(long? empCode, string heightInString, string weightInString)
        {
            int age = 0;
            int height = Convert.ToInt32(heightInString);
            int weight = Convert.ToInt32(weightInString);
            tbl_weight_load_master weightLoadMaster = null;

            if (empCode.HasValue)
            {
                tbl_personal_health_details personalHealthDetails = (from personalHealth in _db.tbl_personal_health_details
                                                                     where personalHealth.phd_sys_emp_code == empCode
                                                                     && personalHealth.phd_active.Value
                                                                     select personalHealth).FirstOrDefault();

                int diffWeight = 0;
                string weightCategory = string.Empty;

                if (personalHealthDetails.phd_Pregnant.Value)
                {
                    weightCategory = "PREGNANT";
                    weightLoadMaster = (from weightLoad in _db.tbl_weight_load_master
                                        where weightLoad.wlm_category.Equals(weightCategory, StringComparison.OrdinalIgnoreCase)
                                        select weightLoad).FirstOrDefault();
                }
                else
                {
                    age = GetAge(empCode);

                    tbl_height_age_master ageHeightDetails = (from ageHeight in _db.tbl_height_age_master
                                                              where ageHeight.ha_min_height_cms <= height
                                                              && ageHeight.ha_max_height_cms >= height
                                                              && ageHeight.ha_min_age <= age
                                                              && ageHeight.ha_max_age >= age
                                                              select ageHeight).FirstOrDefault();

                    tbl_weight_mapping_master weightMapping = (from weightMaster in _db.tbl_weight_mapping_master
                                                               where weightMaster.wmm_height_age_id == ageHeightDetails.ha_id
                                                               select weightMaster).FirstOrDefault();

                    if (weight > weightMapping.wmm_weight)
                    {
                        diffWeight = weight - weightMapping.wmm_weight;
                        weightCategory = "HIGHER";
                    }
                    else
                    {
                        diffWeight = weightMapping.wmm_weight - weight;
                        weightCategory = "LOWER";
                    }

                    weightLoadMaster = (from weightLoad in _db.tbl_weight_load_master
                                        where weightLoad.wlm_min_weight <= diffWeight
                                        && weightLoad.wlm_max_weight >= diffWeight
                                        && weightLoad.wlm_category.Equals(weightCategory, StringComparison.OrdinalIgnoreCase)
                                        select weightLoad).FirstOrDefault();
                }
            }

            string loadFactor = string.Empty;
            if (!string.IsNullOrEmpty(weightLoadMaster.wlm_load_factor))
            {
                loadFactor = weightLoadMaster.wlm_load_factor;
            }

            if (!string.IsNullOrEmpty(weightLoadMaster.wlm_deduction_load_factor))
            {
                loadFactor = string.Format("{0}/{1}", loadFactor, weightLoadMaster.wlm_deduction_load_factor);
            }

            return loadFactor;
        }

        private int GetAge(long? empCode)
        {
            int age = 0;
            DateTime? dateOfBirth = (from emp in _db.tbl_new_employee_basic_details
                                     where emp.nebd_sys_emp_code == empCode
                                     select emp.nebd_date_of_birth).FirstOrDefault();

            if (dateOfBirth.HasValue)
            {
                age = DateTime.Now.AddYears(-dateOfBirth.Value.Year).Year;
            }

            return age;
        }


        public string GetEmployeeDeptCodeByEmployeeId(long empId)
        {
            var employee = (from emp in _db.tbl_new_employee_basic_details
                            where emp.nebd_sys_emp_code == empId
                            select emp).FirstOrDefault();
            string deptCode = string.Empty;
            if (employee != null)
            {
                deptCode = employee.nebd_dept_emp_code;
            }

            return deptCode;
        }

        public Dictionary<long, string> GetDoctorsByKMCCode(string kmcCode)
        {
            return (from doc in _db.tbl_doctor_master
                    where doc.dm_kmc_code == kmcCode
                    select doc).ToDictionary(k => k.dm_doctor_master_id, v => v.dm_name_of_doctor);
        }

        public VM_DoctorDetail GetDoctorDetailByKMCCode(string docKMCCode)
        {
            VM_DoctorDetail objDD = new VM_DoctorDetail();
            try
            {
                DataSet dsDD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@docKMCCode",docKMCCode)
                };
                dsDD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getDoctorDetailsByKMCCode");
                if (dsDD.Tables[0].Rows.Count > 0)
                {
                    objDD.Designation = dsDD.Tables[0].Rows[0]["Designation"].ToString();
                    objDD.NameOfHospital = dsDD.Tables[0].Rows[0]["NameOfHospital"].ToString();
                    objDD.DoctorName = dsDD.Tables[0].Rows[0]["DoctorName"].ToString();
                    objDD.KGIDOfDoctor = dsDD.Tables[0].Rows[0]["KGIDOfDoctor"].ToString();
                    objDD.DoctorId = Convert.ToInt32(dsDD.Tables[0].Rows[0]["DoctorId"]);
                    objDD.PlaceOfHospital = dsDD.Tables[0].Rows[0]["PlaceOfHospital"].ToString();
                }

            }
            catch (Exception ex)
            {

            }
            return objDD;
        }
        public int SaveHDoctorDetailsBll(VM_DoctorDetails objHDoctorDetails)
        {

            int result = 0;
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",objHDoctorDetails.EmployeeId),
                    new SqlParameter("@designation",objHDoctorDetails.Designation),
                    new SqlParameter("@name_of_doctor",objHDoctorDetails.NameOfDoctor),
                    new SqlParameter("@name_of_hospital",objHDoctorDetails.NameOfHospital),
                    new SqlParameter("@doctor_kgid",objHDoctorDetails.KGIDOfDoctor),
                    new SqlParameter("@doctor_id",objHDoctorDetails.DoctorId),
                    new SqlParameter("@emdd_is_kmc_dctr",objHDoctorDetails.emdd_is_kmc_doctor),
                    new SqlParameter("@bankaccnbr",objHDoctorDetails.BankAccNumber),
                    new SqlParameter("@ifsccode",objHDoctorDetails.IFSCCode),
                    new SqlParameter("@micrcode",objHDoctorDetails.MICRCode),
                    new SqlParameter("@imccode",objHDoctorDetails.KMCCode),
                    new SqlParameter("@placeofhospital",objHDoctorDetails.PlaceOfHospital)
                };
                result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertDoctorDetails"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public VM_DoctorDetail GetPreviouslySavedDoctorDetails(long empId)
        {
            VM_DoctorDetail objDD = new VM_DoctorDetail();
            try
            {
                DataSet dsDD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",empId)
                };
                dsDD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectDoctorDetails");
                if (dsDD.Tables[0].Rows.Count > 0)
                {
                    objDD.Designation = dsDD.Tables[0].Rows[0]["Designation"].ToString();
                    objDD.DoctorName = dsDD.Tables[0].Rows[0]["DctrName"].ToString();
                    objDD.KGIDOfDoctor = dsDD.Tables[0].Rows[0]["PolicyNumber"].ToString();
                    objDD.NameOfHospital = dsDD.Tables[0].Rows[0]["HsptlName"].ToString();
                    objDD.DoctorId = Convert.ToInt32(dsDD.Tables[0].Rows[0]["DoctorId"]);
                    objDD.KMCCode = dsDD.Tables[0].Rows[0]["KMCcode"].ToString();
                    objDD.IMCCode = dsDD.Tables[0].Rows[0]["KMCcode"].ToString();
                    objDD.PlaceOfHospital = dsDD.Tables[0].Rows[0]["emdd_place_of_hospital"].ToString();
                    objDD.emdd_is_kmc_doctor = Convert.ToBoolean(dsDD.Tables[0].Rows[0]["iskmcdctr"]);
                    if (!objDD.emdd_is_kmc_doctor)
                    {
                        objDD.BankAccNumber = dsDD.Tables[0].Rows[0]["BankAccNumber"].ToString();
                        objDD.IFSCCode = dsDD.Tables[0].Rows[0]["IFSCCode"].ToString();
                        objDD.MICRCode = dsDD.Tables[0].Rows[0]["MICRCode"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objDD;
        }
        public int SubmitMedicalForm(long empId)
        {
            int _identity = 0;
            tbl_application_referenceno_details appRefNoDetails = (from appDetail in _db.tbl_application_referenceno_details
                                                                   where appDetail.ard_system_emp_code == empId
                                                                   select appDetail).FirstOrDefault();

            if (appRefNoDetails != null)
            {
                appRefNoDetails.ard_active_status = true;
                appRefNoDetails.ard_submission_status = 1;
                appRefNoDetails.ard_updation_datetime = DateTime.Now;
                _identity = 2;
            }
            else
            {
                appRefNoDetails = new tbl_application_referenceno_details();
                appRefNoDetails.ard_active_status = true;
                appRefNoDetails.ard_application_reference_no = Convert.ToInt64(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", ""));
                appRefNoDetails.ard_creation_datetime = DateTime.Now;
                appRefNoDetails.ard_datetime = DateTime.Now;
                appRefNoDetails.ard_submission_status = 1;
                appRefNoDetails.ard_system_emp_code = empId;
                appRefNoDetails.ard_updation_datetime = DateTime.Now;
                _db.tbl_application_referenceno_details.Add(appRefNoDetails);
                _identity = 1;
            }

            _db.SaveChanges();
            return _identity;
        }


        public void SendInsurancePolicyNotification(VM_NotificationDetails notificationDetails)
        {
            try
            {
                VM_Email emailContent = new VM_Email();
                emailContent.ToEmail = notificationDetails.EmpEmailId;
                emailContent.Body = "New policy is generated. The policy number is " + notificationDetails.PolicyNumber;
                emailContent.Subject = "New policy number generated - " + notificationDetails.PolicyNumber;
                _commnobj.SendEmail(emailContent.ToEmail, emailContent.Body, emailContent.Subject); //(emailContent);

                emailContent = new VM_Email();
                emailContent.ToEmail = notificationDetails.DDOEmailId;
                emailContent.Body = "For " + notificationDetails.EmpName + " policy has been generated with ID " + notificationDetails.PolicyNumber;
                emailContent.Subject = "Policy generated for " + notificationDetails.EmpName;
                _commnobj.SendEmail(emailContent.ToEmail, emailContent.Body, emailContent.Subject);

                if (notificationDetails.DIOEmailId != string.Empty)
                {
                    emailContent = new VM_Email();
                    emailContent.ToEmail = notificationDetails.DIOEmailId;
                    emailContent.Body = "For " + notificationDetails.EmpName + " policy has been generated with ID " + notificationDetails.PolicyNumber;
                    emailContent.Subject = "Policy generated for " + notificationDetails.EmpName;
                    _commnobj.SendEmail(emailContent.ToEmail, emailContent.Body, emailContent.Subject);
                }

                string message = "New policy is generated. The policy number is " + notificationDetails.PolicyNumber;
                // AllCommon.sendOTPMSG(notificationDetails.EmpMobileNumber.ToString(), message);
            }
            catch (Exception ex)
            {
            }
        }

        public int SaveEmployeeBasicDetails(VM_EditEmployeeDetails employeeDetails)
        {
            var newEmpDetails = (from emp in _db.tbl_new_employee_basic_details
                                 where emp.nebd_sys_emp_code == employeeDetails.EmpId
                                 select emp).FirstOrDefault();

            if (newEmpDetails != null)
            {
                newEmpDetails.nebd_active = employeeDetails.IsActive;
                newEmpDetails.nebd_date_of_appointment = employeeDetails.DateOfAppointment;
                newEmpDetails.nebd_date_of_birth = employeeDetails.DateOfBirth;
                newEmpDetails.nebd_ddo_code = employeeDetails.DDOCode;
                newEmpDetails.nebd_dept_emp_code = employeeDetails.DepartmentCode;
                newEmpDetails.nebd_email = employeeDetails.EmailId;
                newEmpDetails.nebd_emp_full_name = employeeDetails.Name;
                newEmpDetails.nebd_father_name = employeeDetails.FatherName;
                newEmpDetails.nebd_gender = employeeDetails.Gender;
                newEmpDetails.nebd_mobilenumber = employeeDetails.MobileNumber;
                newEmpDetails.nebd_pan = employeeDetails.PANNumber;
                newEmpDetails.nebd_place_of_birth = employeeDetails.PlaceOfBirth;
                newEmpDetails.nebd_spouse_name = employeeDetails.SpouseName;
                newEmpDetails.nebd_updation_datetime = DateTime.Now;
            }
            var newEmpWorkDetails = (from empl in _db.tbl_employee_work_details
                                     where empl.ewd_emp_id == employeeDetails.EmpId
                                     select empl).FirstOrDefault();

            if (newEmpWorkDetails != null)
            {
                newEmpWorkDetails.ewd_date_of_joining = employeeDetails.tbl_Employee_Work_Details.ewd_date_of_joining;
                newEmpWorkDetails.ewd_payscale_id = employeeDetails.tbl_Employee_Work_Details.ewd_payscale_id;
                newEmpWorkDetails.ewd_employment_type = employeeDetails.tbl_Employee_Work_Details.ewd_employment_type ?? 0;
                newEmpWorkDetails.ewd_designation_id = employeeDetails.tbl_Employee_Work_Details.ewd_designation_id ?? 0;
                newEmpWorkDetails.ewd_group_id = employeeDetails.tbl_Employee_Work_Details.ewd_group_id;
                newEmpWorkDetails.ewd_place_of_posting = employeeDetails.tbl_Employee_Work_Details.ewd_place_of_posting ?? "";
            }

            return _db.SaveChanges();
        }

        public VM_DeptVerificationDetails GetPreviousVerificationDetails(long empId)
        {
            VM_DeptVerificationDetails deptVerificationDetails = (from verDetail in _db.tbl_dept_verification_details
                                                                  join physical in _db.tbl_medical_report_tran on verDetail.dvd_sys_emp_code equals physical.mrt_sys_emp_code
                                                                  where verDetail.dvd_sys_emp_code == empId
                                                                  && verDetail.dvd_status
                                                                  select new VM_DeptVerificationDetails()
                                                                  {
                                                                      HealthReportUploadPath = verDetail.dvd_health_report_upload_path,
                                                                      EmpCode = verDetail.dvd_sys_emp_code,
                                                                      MedicalLeave = verDetail.dvd_medical_leave,
                                                                      LoadFactor = physical.mrt_load_factor,
                                                                      ApplicationRefNo = verDetail.dvd_application_ref_no,
                                                                      ApplicationStatus = verDetail.dvd_application_status
                                                                  }).FirstOrDefault();

            if (deptVerificationDetails.LoadFactor.LastIndexOf('/') > -1)
            {
                string[] loadFactors = deptVerificationDetails.LoadFactor.Split('/');
                deptVerificationDetails.LoadFactor = loadFactors[0];
                deptVerificationDetails.DeductionLoadFactor = loadFactors[1];
            }

            if (!string.IsNullOrEmpty(deptVerificationDetails.HealthReportUploadPath))
            {
                deptVerificationDetails.IsHealthOpinion = true;
                deptVerificationDetails.HealthReportUploadPath = GetFileName(deptVerificationDetails.HealthReportUploadPath);
            }

            int age = GetAge(empId);

            tbl_load_deduction_master loadDeduction = (from loadDed in _db.tbl_load_deduction_master
                                                       where loadDed.ldm_age == age
                                                       && loadDed.ldm_load_factor == deptVerificationDetails.LoadFactor
                                                       select loadDed).FirstOrDefault();

            List<tbl_payscale_details> payscaleDetails = (from payScale in _db.tbl_payscale_details
                                                          where payScale.pd_payscale_code == empId
                                                          select payScale).ToList();

            decimal totalPremium = payscaleDetails.Sum(t => t.pd_kgid_premium.Value);
            deptVerificationDetails.SumAssured = (loadDeduction.ldm_sum_assured - loadDeduction.ldm_deduction) * totalPremium;

            return deptVerificationDetails;
        }
        public tbl_medical_declaration HDeclarationDetailsDll(long EmployeeCode)
        {
            var DeclartaionDtls = (from n in _db.tbl_medical_declaration

                                   where n.md_sys_emp_code == EmployeeCode
                                   select n).FirstOrDefault();
            var RefDtls = (from RNo in _db.tbl_application_referenceno_details

                           where RNo.ard_system_emp_code == EmployeeCode
                           select RNo).FirstOrDefault();
            if (DeclartaionDtls != null)
            {
                DeclartaionDtls.md_referance_no = RefDtls.ard_application_reference_no;
            }

            return DeclartaionDtls;
        }
        public long SaveHNBDeclaration(tbl_medical_declaration objDecl)
        {
            long _identity = 0;

            var _DeclarationDetails = (from n in _db.tbl_medical_declaration
                                       where n.md_sys_emp_code == objDecl.md_sys_emp_code
                                       select n).FirstOrDefault();
            if (_DeclarationDetails != null)
            {
                _DeclarationDetails.md_declaration_status = objDecl.md_declaration_status;
                _DeclarationDetails.md_updation_datetime = DateTime.Now;
                _db.SaveChanges();
                _identity = 2;
            }
            else
            {
                string RefNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                objDecl.md_referance_no = Convert.ToInt64(RefNo);
                objDecl.md_creation_datetime = DateTime.Now;
                _db.tbl_medical_declaration.Add(objDecl);
                _db.SaveChanges();
                _identity = 1;

            }
            return _identity;
        }


        public int ChallanDetailsDll(tbl_challan_details objChallan)
        {
            int _identity = 0;

            var challanDtl = (from n in _db.tbl_challan_details

                              where n.cd_system_emp_code == objChallan.cd_system_emp_code
                              select n).FirstOrDefault();
            if (challanDtl == null)
            {
                _db.tbl_challan_details.Add(objChallan);
                _db.SaveChanges();
                _identity = 1;
            }

            return _identity;
        }

        public int ApprovePolicy(long applicationRefNumber)
        {
            return 1;
        }

        //New
        public long DeleteNBNomineeDll(long nomineeId)
        {
            long result = 0;
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@end_id",nomineeId),

                };
                result = Convert.ToInt64(_Conn.ExecuteCmd(sqlparam, "sp_kgid_deleteNomineedetails"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #region start new implimentation 
        #region NB Application
        #region Basic details
        public long SaveNBBasic(VM_BasicDetails employeeBasicDetails)
        {
            long result = 0;
            try
            {
                if (employeeBasicDetails.isKGIDPAN == true)
                {
                    employeeBasicDetails.eod_spouse_pan_number = employeeBasicDetails.eod_spouse_pan_number;
                    employeeBasicDetails.eod_spouse_kgid_number = null;
                }
                else
                {
                    employeeBasicDetails.eod_spouse_kgid_number = employeeBasicDetails.eod_spouse_kgid_number;
                    employeeBasicDetails.eod_spouse_pan_number = null;
                }

                if (employeeBasicDetails.dr_id == 1)
                    employeeBasicDetails.divorced_upload_doc_path = UploadDocument(employeeBasicDetails.divorced_upload_doc, employeeBasicDetails.employee_id, "DivorceDoc");
                else
                    employeeBasicDetails.divorced_upload_doc_path = string.Empty;

                //string RefNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                SqlParameter[] sqlparam =
                {
                    //new SqlParameter("@Referance_number",RefNo),
                    
                    new SqlParameter("@spouse_name",employeeBasicDetails.spouse_name),
                    new SqlParameter("@spouse_name_kannada",employeeBasicDetails.spouse_name_kannada),
                    new SqlParameter("@ead_address",employeeBasicDetails.ead_address),
                    new SqlParameter("@ead_pincode",employeeBasicDetails.ead_pincode),
                    new SqlParameter("@eod_emp_married",employeeBasicDetails.eod_emp_married),
                    new SqlParameter("@eod_spouse_govt_emp",employeeBasicDetails.eod_spouse_govt_emp),
                    new SqlParameter("@eod_spouse_kgid_number",employeeBasicDetails.eod_spouse_kgid_number),
                    new SqlParameter("@eod_spouse_pan_number",employeeBasicDetails.eod_spouse_pan_number),
                    new SqlParameter("@eod_emp_orphan",employeeBasicDetails.eod_emp_orphan),
                    new SqlParameter("@employee_id",employeeBasicDetails.employee_id),
                    new SqlParameter("@dr_id",employeeBasicDetails.dr_id),
                    new SqlParameter("@Current_spouse_name",employeeBasicDetails.Current_spouse_name),
                    new SqlParameter("@divorce_upload_path",employeeBasicDetails.divorced_upload_doc_path)
                };
                result = Convert.ToInt64(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertBasicdetails"));

            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public VM_BasicDetails NewEmployeeBasicDetailsDll(long EmployeeCode)
        {
            VM_BasicDetails objBD = new VM_BasicDetails();
            try
            {
                DataSet dsBD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",EmployeeCode)
                };
                dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectBasicDetails");
                if (dsBD.Tables[0].Rows.Count > 0)
                {
                    objBD.employee_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["employee_id"]);
                    objBD.employee_name = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                    objBD.spouse_name = dsBD.Tables[0].Rows[0]["spouse_name"].ToString();
                    //
                    objBD.email_id = dsBD.Tables[0].Rows[0]["email_id"].ToString();
                    objBD.pan_number = dsBD.Tables[0].Rows[0]["pan_number"].ToString();

                    objBD.DIO_Office_Address = dsBD.Tables[0].Rows[0]["DIOOffice"].ToString();
                    //objBD.ewd_place_of_posting = dsBD.Tables[0].Rows[0]["ewd_place_of_posting"].ToString();
                    objBD.ewd_place_of_posting = dsBD.Tables[0].Rows[0]["PresentWorkingOffice"].ToString();
                    objBD.father_name = dsBD.Tables[0].Rows[0]["father_name"].ToString();

                    objBD.date_of_birth = dsBD.Tables[0].Rows[0]["date_of_birth"].ToString();
                    objBD.place_of_birth = dsBD.Tables[0].Rows[0]["place_of_birth"].ToString();

                    objBD.gender_desc = dsBD.Tables[0].Rows[0]["gender_desc"].ToString();
                    objBD.ead_address = dsBD.Tables[0].Rows[0]["ead_address"].ToString();

                    objBD.ead_pincode = Convert.ToInt32(dsBD.Tables[0].Rows[0]["ead_pincode"]);
                    objBD.mobile_number = Convert.ToInt64(dsBD.Tables[0].Rows[0]["mobile_number"]);

                    objBD.ewd_date_of_joining = dsBD.Tables[0].Rows[0]["ewd_date_of_joining"].ToString();
                    objBD.et_employee_type_desc = dsBD.Tables[0].Rows[0]["et_employee_type_desc"].ToString();

                    objBD.d_designation_desc = dsBD.Tables[0].Rows[0]["d_designation_desc"].ToString();
                    objBD.payscle_code = dsBD.Tables[0].Rows[0]["payscale_code"].ToString();

                    objBD.eg_group_desc = dsBD.Tables[0].Rows[0]["eg_group_desc"].ToString();
                    //objBD.eod_emp_married = Convert.ToBoolean(dsBD.Tables[0].Rows[0]["eod_emp_married"]);

                    objBD.eod_spouse_govt_emp = Convert.ToBoolean(dsBD.Tables[0].Rows[0]["eod_spouse_govt_emp"]);
                    objBD.eod_emp_orphan = Convert.ToBoolean(dsBD.Tables[0].Rows[0]["eod_emp_orphan"]);

                    objBD.eod_spouse_kgid_number = dsBD.Tables[0].Rows[0]["eod_spouse_kgid_number"].ToString();
                    objBD.eod_spouse_pan_number = dsBD.Tables[0].Rows[0]["eod_spouse_pan_number"].ToString();
                    if (dsBD.Tables[0].Rows[0]["dr_status"].ToString() == null || dsBD.Tables[0].Rows[0]["dr_status"].ToString() == "")
                    {
                        objBD.dr_id = 0;
                    }
                    else
                    {
                        objBD.dr_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["dr_status"]);
                    }
                    objBD.Current_spouse_name = dsBD.Tables[0].Rows[0]["Current_spouse_name"].ToString();
                    if (objBD.eod_spouse_pan_number == "")
                    {
                        objBD.isKGIDPAN = false;
                        objBD.eod_spouse_pan_number = "";
                    }
                    else
                    {
                        objBD.isKGIDPAN = true;
                        objBD.eod_spouse_kgid_number = "";
                    }
                    objBD.kad_kgid_application_number = dsBD.Tables[0].Rows[0]["kad_kgid_application_number"].ToString();
                    objBD.employee_name_kannada = dsBD.Tables[0].Rows[0]["employee_name_kannada"].ToString();
                    objBD.father_name_kannada = dsBD.Tables[0].Rows[0]["father_name_kannada"].ToString();
                    objBD.spouse_name_kannada = dsBD.Tables[0].Rows[0]["spouse_name_kannada"].ToString();
                    objBD.kad_application_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["kad_application_id"]);
                    if (!string.IsNullOrEmpty(objBD.spouse_name) || !string.IsNullOrEmpty(objBD.spouse_name_kannada))
                        objBD.eod_emp_married = true;
                    else
                        objBD.eod_emp_married = false;
                    if (objBD.dr_id == 1)
                    {
                        objBD.divorced_upload_doc_path = dsBD.Tables[0].Rows[0]["divorced_upload_doc_path"].ToString();
                    }
                    //Payment Details
                    objBD.ProposalSubmissionDate = dsBD.Tables[0].Rows[0]["ProposalSubmissionDate"].ToString();
                    objBD.ChallanReferenceNumber = dsBD.Tables[0].Rows[0]["cd_challan_ref_no"].ToString();
                    objBD.ChallanAmount = dsBD.Tables[0].Rows[0]["cd_amount"].ToString();
                    objBD.ChallanPaymentDate = dsBD.Tables[0].Rows[0]["cd_date_of_generation"].ToString();
                }

            }
            catch (Exception ex)
            {

            }
            return objBD;
        }
        #endregion
        #region Kgid details
        public VM_PolicyDetails KGIDDetailsDll(long EmployeeCode)
        {
            VM_PolicyDetails objPD = new VM_PolicyDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",EmployeeCode)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectKGIDDetails");
                //if (dsPD.Tables.Count == 1)
                //{
                if (dsPD.Tables[0].Rows.Count > 0)
                {
                    objPD.employee_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["employee_id"].ToString());
                    objPD.premium_Amount = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["PremiumAmount"].ToString());
                    objPD.premium_Amount_to_Pay = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["PremiumAmount"].ToString());
                    objPD.application_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["kad_application_id"].ToString());
                    objPD.gross_pay = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["gross_pay"]);
                }
                //}
                //else
                //{
                //if (dsPD.Tables[0].Rows.Count > 0)
                //{
                //    objPD.employee_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["employee_id"].ToString());
                //    objPD.premium_Amount = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["PremiumAmount"].ToString());
                //    objPD.premium_Amount_to_Pay = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["PremiumAmount"].ToString());
                //    objPD.application_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["ead_application_id"].ToString());
                //    objPD.gross_pay = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["gross_pay"].ToString());

                //}
                //else
                //{
                //    objPD.employee_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["employee_id"].ToString());
                //    objPD.premium_Amount = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["PremiumAmount"].ToString());
                //    objPD.premium_Amount_to_Pay = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["PremiumAmount"].ToString());
                //    objPD.application_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["ead_application_id"].ToString());
                //    objPD.gross_pay = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["gross_pay"]);

                //}
                if (dsPD.Tables.Count > 1)
                {
                    if (dsPD.Tables[1].Rows.Count > 0)
                    {
                        var list = dsPD.Tables[1].AsEnumerable().Select(dataRow => new VM_PolicyDetails
                        {
                            employee_id = dataRow.Field<long>("employee_id"),
                            p_kgid_policy_number = dataRow.Field<long>("p_kgid_policy_number"),
                            p_sanction_date = dataRow.Field<string>("p_sanction_date"),
                            p_premium = dataRow.Field<double?>("p_premium"),
                            application_id = dataRow.Field<long>("p_application_id")
                        }).ToList();
                        objPD.KGIDPolicyList = list;
                    }
                }
                //}
                //if (dsPD.Tables[1].Rows.Count > 0)
                //{
                //    objPD.emp_gross_pay = Convert.ToDecimal(dsPD.Tables[1].Rows[0]["hrms_gross_pay"]);
                //}
                //else
                //{
                //    objPD.emp_gross_pay = 0;
                //}

            }
            catch (Exception ex)
            {

            }
            return objPD;
        }
        public int SaveNBPolicy(VM_PolicyDetails objPolicy)
        {
            string result = "0";
            try
            {
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",objPolicy.employee_id),
                new SqlParameter("@p_premium",objPolicy.premium_Amount_to_Pay),
                new SqlParameter("@p_application_id",objPolicy.application_id)
            };
                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_insertPolicydetails");

            }
            catch (Exception ex)
            {

            }
            return Convert.ToInt32(result);
        }
        public int SaveNBDdoUploadStatusBll(long empid)
        {
            string result = "0";
            try
            {
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",empid),

            };
                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_UpdateDdoUploadStatus");

            }
            catch (Exception ex)
            {

            }
            return Convert.ToInt32(result);
        }
        #endregion
        #region Medical Physical Details
        public VM_MPhysicalDetails HPhysicalDetailsDll(long EmployeeCode)
        {
            VM_MPhysicalDetails objMPD = new VM_MPhysicalDetails();
            try
            {
                DataSet dsMPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",EmployeeCode)
                };
                dsMPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectMedicalPhysicalDetails");
                if (dsMPD.Tables[0].Rows.Count > 0)
                {
                    objMPD.employee_id = Convert.ToInt64(dsMPD.Tables[0].Rows[0]["employee_id"].ToString());
                    objMPD.application_id = Convert.ToInt64(dsMPD.Tables[0].Rows[0]["kad_application_id"].ToString());
                    objMPD.empd_height = dsMPD.Tables[0].Rows[0]["empd_height"].ToString();
                    objMPD.empd_weight = dsMPD.Tables[0].Rows[0]["empd_weight"].ToString();
                    objMPD.empd_pulse_rate = dsMPD.Tables[0].Rows[0]["empd_pulse_rate"].ToString();
                    objMPD.empd_breathing_rate = dsMPD.Tables[0].Rows[0]["empd_breathing_rate"].ToString();
                    objMPD.empd_high_systolic = dsMPD.Tables[0].Rows[0]["empd_high_systolic"].ToString();
                    objMPD.empd_low_dystolic = dsMPD.Tables[0].Rows[0]["empd_low_dystolic"].ToString();
                    objMPD.empd_blood_pressure = dsMPD.Tables[0].Rows[0]["empd_blood_pressure"].ToString();
                    objMPD.empd_remarks = dsMPD.Tables[0].Rows[0]["empd_remarks"].ToString();
                    objMPD.emp_gender = dsMPD.Tables[0].Rows[0]["gender_desc"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return objMPD;
        }
        public int SaveHPhysicalDetails(VM_MPhysicalDetails objHMedical)
        {
            string result = "0";
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",objHMedical.employee_id),
                    new SqlParameter("@application_id",objHMedical.application_id),
                    new SqlParameter("@empd_height",objHMedical.empd_height),
                    new SqlParameter("@empd_weight",objHMedical.empd_weight),

                    new SqlParameter("@empd_pulse_rate",objHMedical.empd_pulse_rate),
                    new SqlParameter("@empd_breathing_rate",objHMedical.empd_breathing_rate),
                    new SqlParameter("@empd_blood_pressure",objHMedical.empd_blood_pressure),
                    new SqlParameter("@empd_low_dystolic",objHMedical.empd_low_dystolic),

                    new SqlParameter("@empd_high_systolic",objHMedical.empd_high_systolic),
                    new SqlParameter("@empd_remarks",objHMedical.empd_remarks),
                };
                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_insertMedicalPhysicalDetails");

            }
            catch (Exception ex)
            {

            }
            return Convert.ToInt32(result);

        }
        #endregion
        #region Medical Other & Health Details
        public VM_MOtherDetails HOtherDetailsDll(long EmployeeCode)
        {

            VM_MOtherDetails objOtherDataList = new VM_MOtherDetails();
            try
            {
                DataSet dsOD = new DataSet();
                SqlParameter[] sqlparam =
                    {
                        new SqlParameter("@type",1),
                        new SqlParameter("@employee_id",EmployeeCode)
                    };
                dsOD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectOtherHealthDetails");
                if (dsOD.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
                    {
                        objOtherDataList.employee_id = Convert.ToInt64(dsOD.Tables[0].Rows[0]["employee_id"].ToString());
                        objOtherDataList.application_id = Convert.ToInt64(dsOD.Tables[0].Rows[0]["kad_application_id"].ToString());


                        if (Convert.ToInt32(dsOD.Tables[0].Rows[i]["emd_medical_health_id"]) == 1)
                        {
                            objOtherDataList.AdmittedToHospital = Convert.ToBoolean(dsOD.Tables[0].Rows[i]["emd_status"]);
                            objOtherDataList.AdmittedToHospitalDesc = dsOD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objOtherDataList.AdmittedToHospitalDocFileName = dsOD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsOD.Tables[0].Rows[i]["emd_medical_health_id"]) == 2)
                        {
                            objOtherDataList.Accident = Convert.ToBoolean(dsOD.Tables[0].Rows[i]["emd_status"]);
                            objOtherDataList.AccidentDesc = dsOD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objOtherDataList.AccidentDocFileName = dsOD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsOD.Tables[0].Rows[i]["emd_medical_health_id"]) == 3)
                        {
                            objOtherDataList.UndergoneTest = Convert.ToBoolean(dsOD.Tables[0].Rows[i]["emd_status"]);
                            objOtherDataList.UndergoneTestDesc = dsOD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objOtherDataList.UndergoneTestDocFileName = dsOD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsOD.Tables[0].Rows[i]["emd_medical_health_id"]) == 4)
                        {
                            objOtherDataList.UndergoneAnyTreatment = Convert.ToBoolean(dsOD.Tables[0].Rows[i]["emd_status"]);
                            objOtherDataList.UndergoneAnyTreatmentDesc = dsOD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objOtherDataList.UndergoneAnyTreatmentDocFileName = dsOD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objOtherDataList;
        }

        private string GetFileName(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                return path.Substring(path.LastIndexOf('/') + 1).Substring(path.LastIndexOf('\\') + 1);
            }

            return string.Empty;
        }
        DataTable dtMedicalData = new DataTable();
        public int SaveHOtherDetails(VM_MOtherDetails objHOtherDtl)
        {
            string result = "0";
            try
            {
                dtMedicalData.Columns.Add("emd_emp_id");
                dtMedicalData.Columns.Add("emd_application_id");
                dtMedicalData.Columns.Add("emd_medical_health_id");
                dtMedicalData.Columns.Add("emd_status");
                dtMedicalData.Columns.Add("emd_remarks");
                dtMedicalData.Columns.Add("emd_upload_document_path");

                SaveOtherData(objHOtherDtl.employee_id, objHOtherDtl.application_id, 1, objHOtherDtl.AdmittedToHospital, objHOtherDtl.AdmittedToHospitalDesc, objHOtherDtl.AdmittedToHospitalDoc, objHOtherDtl.AdmittedToHospitalDocFileName, "Medical Others");

                SaveOtherData(objHOtherDtl.employee_id, objHOtherDtl.application_id, 2, objHOtherDtl.Accident, objHOtherDtl.AccidentDesc, objHOtherDtl.AccidentDoc, objHOtherDtl.AccidentDocFileName, "Medical Others");

                SaveOtherData(objHOtherDtl.employee_id, objHOtherDtl.application_id, 3, objHOtherDtl.UndergoneTest, objHOtherDtl.UndergoneTestDesc, objHOtherDtl.UndergoneTestDoc, objHOtherDtl.UndergoneTestDocFileName, "Medical Others");

                SaveOtherData(objHOtherDtl.employee_id, objHOtherDtl.application_id, 4, objHOtherDtl.UndergoneAnyTreatment, objHOtherDtl.UndergoneAnyTreatmentDesc, objHOtherDtl.UndergoneAnyTreatmentDoc, objHOtherDtl.UndergoneAnyTreatmentDocFileName, "Medical Others");
                if (dtMedicalData.Select().Any(e => e.ItemArray[5].ToString() == "File Extension Is InValid - Only Upload PDF File"))
                {
                    result = "-1";
                    return Convert.ToInt32(result);
                }
                else
                {
                    SqlParameter[] sqlparam =
                        {
                        new SqlParameter("@employee_id",objHOtherDtl.employee_id),
                        new SqlParameter("@Details",dtMedicalData),
                    };
                    result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_insertMedicalOtherDetails");
                }
            }
            catch (Exception ex)
            {

            }
            return Convert.ToInt32(result);
        }


        public void SaveOtherData(long emd_emp_id, long emd_application_id, int emd_medical_health_id, bool? emd_status, string emd_remarks, HttpPostedFileBase medicalDoc, string emd_upload_document_path, string medDocType)
        {
            try
            {
                //if (string.IsNullOrEmpty(emd_upload_document_path))
                //{
                //    emd_upload_document_path = UploadDocument(medicalDoc, emd_emp_id, medDocType);
                //}
                DataRow drMD = dtMedicalData.NewRow();
                drMD["emd_emp_id"] = emd_emp_id;
                drMD["emd_application_id"] = emd_application_id;
                drMD["emd_medical_health_id"] = emd_medical_health_id;
                drMD["emd_status"] = emd_status;
                drMD["emd_remarks"] = emd_remarks;
                string path = UploadDocument(medicalDoc, emd_emp_id, medDocType);
                if (path != "")
                    drMD["emd_upload_document_path"] = UploadDocument(medicalDoc, emd_emp_id, medDocType); //emd_upload_document_path;
                else
                {
                    drMD["emd_upload_document_path"] = emd_upload_document_path;
                }
                dtMedicalData.Rows.Add(drMD);
            }
            catch (Exception ex)
            {

            }
        }
        public int SaveHHealthDetails(VM_MOtherDetails objHHealthDtl)
        {
            string result = "0";
            try
            {
                dtMedicalData.Columns.Add("emd_emp_id");
                dtMedicalData.Columns.Add("emd_application_id");
                dtMedicalData.Columns.Add("emd_medical_health_id");
                dtMedicalData.Columns.Add("emd_status");
                dtMedicalData.Columns.Add("emd_remarks");
                dtMedicalData.Columns.Add("emd_upload_document_path");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 5, objHHealthDtl.HavingIllnessinChest, objHHealthDtl.HavingIllnessinChestDesc, objHHealthDtl.HavingIllnessinChestDoc, objHHealthDtl.HavingIllnessinChestDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 6, objHHealthDtl.HavingIllnessinTeeth, objHHealthDtl.HavingIllnessinTeethDesc, objHHealthDtl.HavingIllnessinTeethDoc, objHHealthDtl.HavingIllnessinTeethDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 7, objHHealthDtl.Disability, objHHealthDtl.DisabilityDesc, objHHealthDtl.DisabilityDoc, objHHealthDtl.DisabilityDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 8, objHHealthDtl.HaveThyroid, objHHealthDtl.HaveThyroidDesc, objHHealthDtl.HaveThyroidDoc, objHHealthDtl.HaveThyroidDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 9, objHHealthDtl.EnlargementSpleenLiver, objHHealthDtl.EnlargementSpleenLiverDesc, objHHealthDtl.EnlargementSpleenLiverDoc, objHHealthDtl.EnlargementSpleenLiverDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 10, objHHealthDtl.GastroIntestinateTrack, objHHealthDtl.GastroIntestinateTrackDesc, objHHealthDtl.GastroIntestinateTrackDoc, objHHealthDtl.GastroIntestinateTrackDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 11, objHHealthDtl.SufferFromHernia, objHHealthDtl.SufferFromHerniaDesc, objHHealthDtl.SufferFromHerniaDoc, objHHealthDtl.SufferFromHerniaDocFileName, "Medical Health");
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 12, objHHealthDtl.SufferFromUrinaryTract, objHHealthDtl.SufferFromUrinaryTractDesc, objHHealthDtl.SufferFromUrinaryTractDoc, objHHealthDtl.SufferFromUrinaryTractDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 13, objHHealthDtl.NervousSystemDisease, objHHealthDtl.NervousSystemDiseaseDesc, objHHealthDtl.NervousSystemDiseaseDoc, objHHealthDtl.NervousSystemDiseaseDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 14, objHHealthDtl.UndergoneSurgery, objHHealthDtl.UndergoneSurgeryDesc, objHHealthDtl.UndergoneSurgeryDoc, objHHealthDtl.UndergoneSurgeryDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 15, objHHealthDtl.AccidentWoundMarks, objHHealthDtl.AccidentWoundMarksDesc, objHHealthDtl.AccidentWoundMarksDoc, objHHealthDtl.AccidentWoundMarksDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 16, objHHealthDtl.AdverseSymptomInHealth, objHHealthDtl.AdverseSymptomInHealthDesc, objHHealthDtl.AdverseSymptomInHealthDoc, objHHealthDtl.AdverseSymptomInHealthDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 17, objHHealthDtl.BreastIllness, objHHealthDtl.BreastIllnessDesc, objHHealthDtl.BreastIllnessDoc, objHHealthDtl.BreastIllnessDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 18, objHHealthDtl.BreastCancer, objHHealthDtl.BreastCancerDesc, objHHealthDtl.BreastCancerDoc, objHHealthDtl.BreastCancerDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 19, objHHealthDtl.ClueInPregancy, objHHealthDtl.ClueInPregancyDesc, objHHealthDtl.ClueInPregancyDoc, objHHealthDtl.ClueInPregancyDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 20, objHHealthDtl.BiologicalIllness, objHHealthDtl.BiologicalIllnessDesc, objHHealthDtl.BiologicalIllnessDoc, objHHealthDtl.BiologicalIllnessDocFileName, "Medical Health");

                SaveOtherData(objHHealthDtl.employee_id, objHHealthDtl.application_id, 21, objHHealthDtl.GoodLifeCycle, objHHealthDtl.GoodLifeCycleDesc, objHHealthDtl.GoodLifeCycleDoc, objHHealthDtl.GoodLifeCycleDocFileName, "Medical Health");
                if (dtMedicalData.Select().Any(e => e.ItemArray[5].ToString() == "File Extension Is InValid - Only Upload PDF File"))
                {
                    result = "-1";
                    return Convert.ToInt32(result);
                }
                else
                {
                    SqlParameter[] sqlparam =
                        {
                        new SqlParameter("@employee_id",objHHealthDtl.employee_id),
                        new SqlParameter("@Details",dtMedicalData),
                    };
                    result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_insertMedicalHealthDetails");
                }
            }
            catch (Exception ex)
            {

            }
            return Convert.ToInt32(result);
        }
        public VM_MOtherDetails HHealthDetailsDll(long EmployeeCode)
        {
            VM_MOtherDetails objHealthDataList = new VM_MOtherDetails();
            try
            {
                DataSet dsHD = new DataSet();
                SqlParameter[] sqlparam =
                    {
                        new SqlParameter("@type",2),
                        new SqlParameter("@employee_id",EmployeeCode)
                    };
                dsHD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectOtherHealthDetails");
                if (dsHD.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsHD.Tables[0].Rows.Count; i++)
                    {
                        objHealthDataList.employee_id = Convert.ToInt64(dsHD.Tables[0].Rows[0]["employee_id"].ToString());
                        objHealthDataList.application_id = Convert.ToInt64(dsHD.Tables[0].Rows[0]["kad_application_id"].ToString());
                        objHealthDataList.gender_id = Convert.ToInt32(dsHD.Tables[0].Rows[0]["gender_id"].ToString());
                        if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 5)
                        {
                            objHealthDataList.HavingIllnessinChest = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.HavingIllnessinChestDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.HavingIllnessinChestDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 6)
                        {
                            objHealthDataList.HavingIllnessinTeeth = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.HavingIllnessinTeethDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.HavingIllnessinTeethDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 7)
                        {
                            objHealthDataList.Disability = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.DisabilityDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.DisabilityDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 8)
                        {
                            objHealthDataList.HaveThyroid = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.HaveThyroidDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.HaveThyroidDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 9)
                        {
                            objHealthDataList.EnlargementSpleenLiver = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.EnlargementSpleenLiverDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.EnlargementSpleenLiverDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 10)
                        {
                            objHealthDataList.GastroIntestinateTrack = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.GastroIntestinateTrackDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.GastroIntestinateTrackDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 11)
                        {
                            objHealthDataList.SufferFromHernia = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.SufferFromHerniaDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.SufferFromHerniaDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 12)
                        {
                            objHealthDataList.SufferFromUrinaryTract = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.SufferFromUrinaryTractDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.SufferFromUrinaryTractDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 13)
                        {
                            objHealthDataList.NervousSystemDisease = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.NervousSystemDiseaseDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.NervousSystemDiseaseDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 14)
                        {
                            objHealthDataList.UndergoneSurgery = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.UndergoneSurgeryDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.UndergoneSurgeryDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 15)
                        {
                            objHealthDataList.AccidentWoundMarks = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.AccidentWoundMarksDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.AccidentWoundMarksDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 16)
                        {
                            objHealthDataList.AdverseSymptomInHealth = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.AdverseSymptomInHealthDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.AdverseSymptomInHealthDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 17)
                        {
                            objHealthDataList.BreastIllness = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.BreastIllnessDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.BreastIllnessDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 18)
                        {
                            objHealthDataList.BreastCancer = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.BreastCancerDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.BreastCancerDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");

                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 19)
                        {
                            objHealthDataList.ClueInPregancy = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.ClueInPregancyDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.ClueInPregancyDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 20)
                        {
                            objHealthDataList.BiologicalIllness = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            objHealthDataList.BiologicalIllnessDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.BiologicalIllnessDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }
                        else if (Convert.ToInt32(dsHD.Tables[0].Rows[i]["emd_medical_health_id"]) == 21)
                        {
                            //objHealthDataList.GoodLifeCycle = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            if (dsHD.Tables[0].Rows[i]["emd_remarks"].ToString() == "")
                            {
                                objHealthDataList.GoodLifeCycle = true;
                            }
                            else
                            {
                                objHealthDataList.GoodLifeCycle = Convert.ToBoolean(dsHD.Tables[0].Rows[i]["emd_status"]);
                            }
                            objHealthDataList.GoodLifeCycleDesc = dsHD.Tables[0].Rows[i]["emd_remarks"].ToString();
                            objHealthDataList.GoodLifeCycleDocFileName = dsHD.Tables[0].Rows[i]["emd_upload_document_path"].ToString().Replace(@"\", @"/");
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objHealthDataList;
        }
        #endregion
        #region Family Details
        public int SaveNBFamily(VM_FamilyDetails objFamilyDetails)
        {
            try
            {
                if (objFamilyDetails.FamilyDetails != null)
                {
                    DataTable dt_FamilyData = new DataTable();
                    dt_FamilyData = CreateDataTable("VM_FamilyDetail");
                    foreach (VM_FamilyDetail objFamily in objFamilyDetails.FamilyDetails)
                    {
                        objFamily.DateOfBirth = _commnobj.DateConversion(objFamily.DateOfBirth);
                        if (objFamily.DateOfDeath != null)
                        {
                            objFamily.DateOfDeath = _commnobj.DateConversion(objFamily.DateOfDeath);
                        }

                        DataRow dr = dt_FamilyData.NewRow();
                        dr["Id"] = objFamily.Id;
                        dr["EmpId"] = objFamilyDetails.EmployeeId;
                        dr["Relation"] = objFamily.Relation;
                        dr["NameOfMember"] = objFamily.NameOfMember;
                        dr["DateOfBirth"] = objFamily.DateOfBirth; //(objFamily.DateOfBirth != null) ? Convert.ToDateTime(objFamily.DateOfBirth).ToString("dd/MM/yyyy") : null;
                        dr["AliveDead"] = objFamily.AliveDead;
                        dr["DateOfDeath"] = objFamily.DateOfDeath; //(objFamily.DateOfDeath != null) ? Convert.ToDateTime(objFamily.DateOfDeath).ToString("dd/MM/yyyy") : null;
                        dr["Age"] = objFamily.Age;
                        dr["ReasonOfDeath"] = objFamily.ReasonOfDeath;
                        dr["HealthCondition"] = objFamily.HealthCondition;
                        dr["IsSiblingMarried"] = objFamily.IsSiblingMarried;
                        dr["ApplicationId"] = objFamily.ApplicationId;
                        dt_FamilyData.Rows.Add(dr);

                    }
                    SqlParameter[] sqlparam =
                    {
                        new SqlParameter("@EmpCode",objFamilyDetails.EmployeeId),
                        new SqlParameter("@ApplicationID",objFamilyDetails.ApplicationId),
                        new SqlParameter("@FamilyMemberData",dt_FamilyData)
                    };
                    int result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertFamilyMemberdetails"));
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "SaveNBFamily(dll) :- " + ex.Message);
            }
            return _db.SaveChanges();
        }
        public int CheckFamilyMemberDetailsDll(long FMID)
        {
            int result = 1;
            try
            {
                var familydetail = (from fm in _db.tbl_employee_family_details
                                    where fm.efd_family_id == FMID
                                    select fm).FirstOrDefault();
                var loggedInUserDDOCode = (from nd in _db.tbl_employee_nominee_details
                                           where nd.end_family_id == FMID && nd.end_active == true
                                           && nd.end_application_id == familydetail.efd_application_id
                                           select nd).FirstOrDefault();
                if (loggedInUserDDOCode != null)
                    result = 0;
                else
                    result = 1;
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public IList<tbl_family_relation_master> GetRelations()
        {
            return (from relations in _db.tbl_family_relation_master
                    select relations).ToList();
        }

        public VM_FamilyDetails FamilyDetailsDll(long EmployeeCode)
        {
            VM_FamilyDetails familyDetails = new VM_FamilyDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",EmployeeCode)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectFamilyDetails");
                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        familyDetails.EmployeeId = EmployeeCode;
                        foreach (DataRow dr in dsPD.Tables[0].Rows)
                        {
                            VM_FamilyDetail objfamData = new VM_FamilyDetail();
                            objfamData.Id = Convert.ToInt64(dr["Id"]);
                            objfamData.NameOfMember = dr["NameOfMember"].ToString();
                            objfamData.Relation = dr["Relation"].ToString();
                            objfamData.DateOfBirth = dr["DateOfBirth"].ToString();
                            objfamData.Age = (dr["Age"] != DBNull.Value) ? Convert.ToInt16(dr["Age"]) : 0;
                            objfamData.AliveDead = Convert.ToBoolean(dr["AliveDead"]);
                            objfamData.IsSiblingMarried = Convert.ToBoolean(dr["IsSiblingMarried"]);
                            objfamData.HealthCondition = dr["HealthCondition"].ToString();
                            objfamData.DateOfDeath = (dr["DateOfDeath"] != null) ? dr["DateOfDeath"].ToString() : "";
                            objfamData.ReasonOfDeath = dr["ReasonOfDeath"].ToString();
                            objfamData.EditDeleteStatus = dr["EditDeleteStatus"] != DBNull.Value ? dr["EditDeleteStatus"].ToString() : "New";
                            objfamData.AppliactionSentBack = Convert.ToBoolean(dr["AppliactionSentBack"]);
                            objfamData.ApplicationInsured = Convert.ToBoolean(dr["ApplicationInsured"]);
                            familyDetails.FamilyDetails.Add(objfamData);
                        }
                    }
                    familyDetails.NoOfBrother = dsPD.Tables[0].AsEnumerable().Where(a => a.Field<string>("Relation") == "Brother").Count();
                    familyDetails.NoOfSister = dsPD.Tables[0].AsEnumerable().Where(a => a.Field<string>("Relation") == "Sister").Count();
                    familyDetails.NoOfChildren = dsPD.Tables[0].AsEnumerable().Where(a => a.Field<string>("Relation") == "Daughter" || a.Field<string>("Relation") == "Son").Count();

                    var types = new List<SelectListItem>();
                    //var dbtypes = new List<SelectListItem>();
                    bool isMarried = _db.tbl_employee_other_details.Where(a => a.eod_emp_id == EmployeeCode).Select(a => a.eod_emp_married).FirstOrDefault();
                    bool isOrphan = _db.tbl_employee_other_details.Where(a => a.eod_emp_id == EmployeeCode).Select(a => a.eod_emp_orphan).FirstOrDefault();
                    int? isDivorced = _db.tbl_employee_basic_details.Where(a => a.employee_id == EmployeeCode).Select(a => a.dr_status).FirstOrDefault();
                    string FatherName = _db.tbl_employee_basic_details.Where(a => a.employee_id == EmployeeCode).Select(a => a.father_name).FirstOrDefault();

                    if (isMarried && (isDivorced == 0 || isDivorced == null || isDivorced == 2) && !isOrphan && FatherName != null && FatherName != string.Empty)
                    {
                        types = (from t in _db.tbl_family_relation_master
                                 where t.frm_family_type_id == 1 && t.frm_relation_desc != "None"
                                 select (new SelectListItem { Text = t.frm_relation_desc, Value = t.frm_relation_id.ToString() })).ToList();
                    }
                    else if (isDivorced != null && isDivorced == 1 && !isOrphan)
                    {
                        types = (from t in _db.tbl_family_relation_master
                                 where t.frm_family_type_id == 1 && t.frm_relation_desc != "Spouse" && t.frm_relation_desc != "None"
                                 select (new SelectListItem { Text = t.frm_relation_desc, Value = t.frm_relation_id.ToString() })).ToList();
                    }

                    else if (isOrphan && isDivorced == 1)
                    {
                        types = (from t in _db.tbl_family_relation_master
                                 where t.frm_family_type_id == 1 && (t.frm_relation_desc != "Father" && t.frm_relation_desc != "Mother" && t.frm_relation_desc != "Brother" && t.frm_relation_desc != "Sister" && t.frm_relation_desc != "Spouse")
                                 select (new SelectListItem { Text = t.frm_relation_desc, Value = t.frm_relation_id.ToString() })).ToList();
                    }
                    else if ((isOrphan && isMarried) || (isMarried && (isDivorced == 0 || isDivorced == null || isDivorced == 2) && !isOrphan && (FatherName == string.Empty || FatherName == null)))
                    {
                        types = (from t in _db.tbl_family_relation_master
                                 where t.frm_family_type_id == 1 && (t.frm_relation_desc != "Father" && t.frm_relation_desc != "Mother" && t.frm_relation_desc != "Brother" && t.frm_relation_desc != "Sister" && t.frm_relation_desc != "None")
                                 select (new SelectListItem { Text = t.frm_relation_desc, Value = t.frm_relation_id.ToString() })).ToList();
                    }

                    else
                    {
                        types = (from t in _db.tbl_family_relation_master
                                 where t.frm_family_type_id == 1 && (t.frm_relation_desc != "Spouse" && t.frm_relation_desc != "Daughter" && t.frm_relation_desc != "Son" && t.frm_relation_desc != "None")
                                 select (new SelectListItem { Text = t.frm_relation_desc, Value = t.frm_relation_id.ToString() })).ToList();
                    }
                    //types.Add(new SelectListItem { Text = "Select Relation Name", Value = "0" });
                    familyDetails.FamilyRelationList = types;
                    familyDetails.IsMarried = isMarried;
                    familyDetails.IsOrphan = isOrphan;
                }
            }
            catch (Exception ex)
            {

            }
            return familyDetails;
        }

        public int DeleteNBFamilyDll(long leaveid)
        {
            SqlParameter[] sqlparam =
                    {
                        new SqlParameter("@FamilyMemberID",leaveid)
                    };
            int result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_deleteFamilyMemberdetails"));
            return result;
        }
        #endregion
        #region Nominee Details

        //public VM_NomineeDetails NomineeDetailsDll(long EmployeeCode)
        //{
        //    var nomineeDetailsFromDB = (from nominee in _db.tbl_employee_nominee_details
        //                                where nominee.end_emp_id == EmployeeCode && nominee.end_active == true
        //                                select nominee).ToList();

        //    //var familyDetailsFromDB = (from family in _db.tbl_employee_family_details
        //    //                           where family.efd_emp_id == EmployeeCode && family.efd_active == true
        //    //                           select family).ToList();

        //    var relations = GetRelations();

        //    VM_NomineeDetails nomineeDetails = new VM_NomineeDetails();

        //    if (nomineeDetailsFromDB.Count != 0)
        //    {
        //        nomineeDetails.EmployeeId = EmployeeCode;

        //        foreach (var nomineeDetailFromDB in nomineeDetailsFromDB)
        //        {
        //            VM_NomineeDetail nomineeDetail = new VM_NomineeDetail();
        //            nomineeDetail.FamilyMemberId = nomineeDetailFromDB.end_family_id;
        //            nomineeDetail.Id = nomineeDetailFromDB.end_nominee_id;
        //            nomineeDetail.NameOfGaurdian = nomineeDetailFromDB.end_name_of_guardian;
        //            nomineeDetail.NameOfNominee = nomineeDetailFromDB.end_name_of_nominee;
        //            nomineeDetail.PercentageShare = nomineeDetailFromDB.end_percentage_of_share;
        //            nomineeDetail.RelationId = nomineeDetailFromDB.end_relation_id;
        //            nomineeDetail.Relation = relations.FirstOrDefault(t => t.frm_relation_id == nomineeDetailFromDB.end_relation_id).frm_relation_desc;

        //            //if (nomineeDetailFromDB.end_family_id.HasValue)
        //            //{
        //            //    var familyMember = familyDetailsFromDB.FirstOrDefault(t => t.efd_relation_id == nomineeDetailFromDB.end_relation_id);

        //            //    string dateOfBirthString = familyMember.efd_date_of_birth.ToShortDateString();
        //            //    var dateOfBirthArray = dateOfBirthString.Split('-');
        //            //    DateTime dateOfBirth = new DateTime(Convert.ToInt32(dateOfBirthArray[2]), Convert.ToInt32(dateOfBirthArray[1]), Convert.ToInt32(dateOfBirthArray[0]));
        //            //    nomineeDetail.Age = DateTime.Now.Year - dateOfBirth.Year;
        //            //}

        //            nomineeDetails.NomineeDetails.Add(nomineeDetail);
        //        }
        //    }
        //    return nomineeDetails;
        //}

        //public int SaveNBNominee(VM_NomineeDetail objNominee)
        //{
        //if (objNominee != null && objNominee.NomineeDetails.All(t => !string.IsNullOrEmpty(t.Relation)))
        //{
        //    var nomineeDetailsFromDB = (from nominee in _db.tbl_employee_nominee_details
        //                                where nominee.end_emp_id == objNominee.EmployeeId
        //                                select nominee).ToList();

        //    var familyDetailsFromDB = (from family in _db.tbl_employee_family_details
        //                               where family.efd_emp_id == objNominee.EmployeeId && family.efd_active == true
        //                               select family).ToList();

        //    var applicationDetails = (from appDet in _db.tbl_kgid_application_details
        //                              where appDet.kad_emp_id == objNominee.EmployeeId
        //                              && appDet.kad_active_status
        //                              select appDet).FirstOrDefault();

        //    var relations = GetRelations();

        //    if (nomineeDetailsFromDB.Any())
        //    {
        //        _db.tbl_employee_nominee_details.RemoveRange(nomineeDetailsFromDB);
        //        _db.SaveChanges();
        //    }

        //    foreach (var nomineeDetail in objNominee.NomineeDetails)
        //    {
        //        DateTime currentDateTime = DateTime.Now;
        //        tbl_employee_nominee_details nomineeMember = new tbl_employee_nominee_details
        //        {
        //            end_active = true,
        //            end_application_id = applicationDetails.kad_application_id,
        //            end_created_by = objNominee.EmployeeId,
        //            end_creation_datetime = currentDateTime,
        //            end_emp_id = objNominee.EmployeeId,
        //            end_relation_id = relations.FirstOrDefault(t => t.frm_relation_desc.Equals(nomineeDetail.Relation)).frm_relation_id,
        //            end_name_of_guardian = nomineeDetail.NameOfGaurdian,
        //            end_name_of_nominee = nomineeDetail.NameOfNominee,
        //            end_percentage_of_share = nomineeDetail.PercentageShare,
        //            end_updated_by = objNominee.EmployeeId,
        //            end_updation_datetime = currentDateTime

        //        };

        //        var relationId = relations.FirstOrDefault(t => t.frm_relation_desc.Equals(nomineeDetail.Relation)).frm_relation_id;
        //        var familyMember = familyDetailsFromDB.FirstOrDefault(t => t.efd_relation_id == relationId);
        //        if (familyMember != null)
        //        {
        //            nomineeMember.end_family_id = familyMember.efd_family_id;
        //        }

        //        if (!string.IsNullOrEmpty(nomineeDetail.GaurdianRelation))
        //        {
        //            nomineeMember.end_guardian_relation_id = relations.FirstOrDefault(t => t.frm_relation_desc.Equals(nomineeDetail.GaurdianRelation)).frm_relation_id;
        //        }

        //        _db.tbl_employee_nominee_details.Add(nomineeMember);
        //    }
        //}

        //    return _db.SaveChanges();
        //}

        public VM_NomineeDetails NomineeDetailsDll(long EmployeeCode)
        {
            VM_NomineeDetails nomineeDetails = new VM_NomineeDetails();
            DataSet dsND = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@employee_id",EmployeeCode)
                };
            dsND = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectNomineeDetails");
            try
            {
                if (dsND.Tables.Count > 0)
                {
                    if (dsND.Tables[0].Rows.Count > 0)
                    {
                        nomineeDetails.EmployeeId = EmployeeCode;

                        foreach (DataRow dr in dsND.Tables[0].Rows)
                        {
                            VM_NomineeDetail nomineeDetail = new VM_NomineeDetail();
                            nomineeDetail.FamilyMemberId = Convert.ToInt64(dr["end_family_id"]);
                            nomineeDetail.Id = Convert.ToInt64(dr["end_nominee_id"]);
                            nomineeDetail.NameOfGaurdian = dr["end_name_of_guardian"].ToString();
                            nomineeDetail.GaurdianRelation = dr["end_relation_of_guardian"].ToString();
                            nomineeDetail.NameOfNominee = dr["end_name_of_nominee"].ToString();
                            nomineeDetail.PercentageShare = Convert.ToInt32(dr["end_percentage_of_share"]);
                            nomineeDetail.RelationId = (dr["end_relation_id"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["end_relation_id"]);
                            nomineeDetail.Relation = dr["end_relation"].ToString();
                            if (nomineeDetail.Relation == "Guardian")
                                nomineeDetail.Age = (dr["end_guardian_age"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["end_guardian_age"]);
                            else
                                nomineeDetail.Age = (dr["end_age"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["end_age"]);
                            nomineeDetails.NomineeDetails.Add(nomineeDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return nomineeDetails;
        }
        public long SaveNBNominee(VM_NomineeDetail objNominee)
        {
            long result = 0;
            try
            {
                SqlParameter[] sqlparam =
                {

                    new SqlParameter("@end_id",objNominee.Id ),
                    new SqlParameter("@end_emp_id",objNominee.EmpId),
                    new SqlParameter("@end_created_by",objNominee.EmpId),
                    new SqlParameter("@end_name_of_nominee",objNominee.NameOfNominee),
                    new SqlParameter("@end_relation",objNominee.Relation),
                    new SqlParameter("@end_percentage_of_share",objNominee.PercentageShare),
                    new SqlParameter("@end_name_of_guardian",objNominee.NameOfGaurdian),
                    new SqlParameter("@end_guardian_relation",objNominee.GaurdianRelation),
                    new SqlParameter("@end_guardian_age",objNominee.Age)
                };
                result = Convert.ToInt64(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertNomineedetails"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public BindDropDownModel GetNomineeNameListBll(long EmployeeCode, string TypeOfDDL)
        {
            BindDropDownModel nomineenameList = new BindDropDownModel();
            DataSet dsND = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@employee_id",EmployeeCode)
            };
            dsND = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getNomineeList");
            try
            {
                if (dsND.Tables.Count > 0)
                {
                    if (dsND.Tables[0].Rows.Count > 0)
                    {
                        List<VM_DropDownList> lstNominee = new List<VM_DropDownList>();
                        foreach (DataRow dr in dsND.Tables[0].Rows)
                        {
                            VM_DropDownList nomineelist = new VM_DropDownList();
                            nomineelist.Id = Convert.ToInt32(dr["Id"]);
                            nomineelist.Value = dr["Description"].ToString();
                            lstNominee.Add(nomineelist);
                        }
                        nomineenameList.NomineeList = lstNominee;
                    }
                    if (dsND.Tables[1].Rows.Count > 0)
                    {
                        List<VM_DropDownList> lstGuardian = new List<VM_DropDownList>();
                        foreach (DataRow dr in dsND.Tables[1].Rows)
                        {
                            VM_DropDownList nomineelist = new VM_DropDownList();
                            nomineelist.Id = Convert.ToInt32(dr["Id"]);
                            nomineelist.Value = dr["Description"].ToString();
                            lstGuardian.Add(nomineelist);
                        }
                        nomineenameList.GuardianList = lstGuardian;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return nomineenameList;
        }

        #endregion
        #region Personal Details
        public VM_PersonalHealthDetails PersonalDetailsDll(long EmployeeCode)
        {
            VM_PersonalHealthDetails objPD = new VM_PersonalHealthDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",EmployeeCode)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectPersonalHealthDetails");
                bool PersonalData = false;
                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        objPD.ephd_application_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["kad_application_id"]);
                    }
                    if (dsPD.Tables.Count == 2)
                    {
                        if (dsPD.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsPD.Tables[1].Rows)
                            {
                                if (!PersonalData)
                                {
                                    PersonalData = true;
                                    objPD.ephd_emp_id = Convert.ToInt64(dsPD.Tables[1].Rows[0]["ephd_emp_id"]);
                                    objPD.ephd_gender_id = Convert.ToInt32(dsPD.Tables[1].Rows[0]["gender_id"]);
                                    objPD.ephd_health_condition = Convert.ToBoolean(dsPD.Tables[1].Rows[0]["ephd_health_condition"]);
                                    objPD.ephd_is_pregnant = (dsPD.Tables[1].Rows[0]["ephd_is_pregnant"] == DBNull.Value) ? false : Convert.ToBoolean(dsPD.Tables[1].Rows[0]["ephd_is_pregnant"]);
                                    objPD.ephd_height = dsPD.Tables[1].Rows[0]["ephd_height"].ToString();
                                    objPD.ephd_weight = dsPD.Tables[1].Rows[0]["ephd_weight"].ToString();
                                    // objPD.ephd_husband_occupation_address = dsPD.Tables[1].Rows[0]["ephd_husband_occupation_address"].ToString();
                                    // objPD.ephd_no_of_years_of_marriage = Convert.ToDecimal(dsPD.Tables[1].Rows[0]["ephd_no_of_years_of_marriage"]);
                                    //  objPD.ephd_is_married = (dsPD.Tables[1].Rows[0]["ephd_is_married"] == DBNull.Value) ? false : Convert.ToBoolean(dsPD.Tables[1].Rows[0]["ephd_is_married"]);
                                    objPD.PeriodDate = (dsPD.Tables[1].Rows[0]["ephd_date_of_last_period"] != null) ? dsPD.Tables[1].Rows[0]["ephd_date_of_last_period"].ToString() : "";
                                    //   objPD.ephd_husbandGovEmp = dsPD.Tables[1].Rows[0]["ephd_husbandGovEmp"].ToString();
                                    //  objPD.ephd_husbandLIC = dsPD.Tables[1].Rows[0]["ephd_husbandLIC"].ToString();
                                    //  objPD.lastmensuration = (dsPD.Tables[1].Rows[0]["ephd_lastmensuration"] != null) ? dsPD.Tables[1].Rows[0]["ephd_lastmensuration"].ToString() : "";
                                    //  objPD.ephd_menstruating = Convert.ToBoolean(dsPD.Tables[1].Rows[0]["ephd_menstruating"]);
                                    //  objPD.ephd_nopregnancies = Convert.ToInt32(dsPD.Tables[1].Rows[0]["ephd_nopregnancies"].ToString());
                                    //  objPD.dateoflastdelivery = (dsPD.Tables[1].Rows[0]["ephd_dateoflastdelivery"] != null) ? dsPD.Tables[1].Rows[0]["ephd_dateoflastdelivery"].ToString() : "";
                                    //   objPD.ephd_gonefulltime = dsPD.Tables[1].Rows[0]["ephd_gonefulltime"].ToString();
                                    //   objPD.ephd_insuredOfficialBrch = dsPD.Tables[1].Rows[0]["ephd_insuredOfficialBrch"].ToString();
                                    //   objPD.ephd_miscarriages = dsPD.Tables[1].Rows[0]["ephd_miscarriages"].ToString();
                                    //   objPD.IsinsuredOfficialBrch = Convert.ToBoolean(dsPD.Tables[1].Rows[0]["ephd_IsInsuredOfficialBrch"]);
                                    //   objPD.IshusbandLIC = Convert.ToBoolean(dsPD.Tables[1].Rows[0]["ephd_IshusbandLIC"]);

                                    if (objPD.PeriodDate == "01-01-1900")
                                        objPD.PeriodDate = string.Empty;
                                    //if (objPD.lastmensuration == "01-01-1900")
                                    //    objPD.lastmensuration = string.Empty;
                                    //if (objPD.dateoflastdelivery == "01-01-1900")
                                    //    objPD.dateoflastdelivery = string.Empty;
                                }
                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 1)
                                //{
                                //    objPD.IsMedicalAdvice = true;
                                //    objPD.MedicalAdviceDetails = dr["epdd_remarks"].ToString();
                                //    objPD.MedicalAdviceDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}
                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 2)
                                //{
                                //    objPD.IsFamilyMemberAffectedByDisease = true;
                                //    objPD.FamilyMemberAffectedByDiseaseDetails = dr["epdd_remarks"].ToString();
                                //    //objPersonal.FamilyMemberAffectedByDiseaseDocFileName = GetFileName(PersonalDisDetails[i].pdd_disease_doc);
                                //    objPD.FamilyMemberAffectedByDiseaseDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}
                                if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 3)
                                {
                                    objPD.IsInfectiousDiseaseHouse = true;
                                    objPD.InfectiousDiseaseHouseDetails = dr["epdd_remarks"].ToString();
                                    //objPersonal.InfectiousDiseaseHouseDocFileName = GetFileName(PersonalDisDetails[i].pdd_disease_doc);
                                    objPD.InfectiousDiseaseHouseDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                }
                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 4)
                                //{
                                //    objPD.IsBrainDisease = true;
                                //    objPD.BrainDiseaseDetails = dr["epdd_remarks"].ToString();
                                //    objPD.BrainDiseaseDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}
                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 5)
                                //{
                                //    objPD.IsDiseaseOfLungs = true;
                                //    objPD.DiseaseOfLungsDetails = dr["epdd_remarks"].ToString();
                                //    objPD.DiseaseOfLungsDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}
                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 6)
                                //{
                                //    objPD.IsLiverKidneyDisease = true;
                                //    objPD.LiverKidneyDiseaseDetails = dr["epdd_remarks"].ToString();
                                //    objPD.LiverKidneyDiseaseDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}
                                if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 7)
                                {
                                    objPD.IsStomachDisease = true;
                                    objPD.StomachDiseaseDetails = dr["epdd_remarks"].ToString();
                                    objPD.StomachDiseaseDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                }
                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 8)
                                //{
                                //    objPD.IsRheumaticFever = true;
                                //    objPD.RheumaticFeverDetails = dr["epdd_remarks"].ToString();
                                //    objPD.RheumaticFeverDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}
                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 9)
                                //{
                                //    objPD.IsUrineChecked = true;
                                //    objPD.UrineCheckedDetails = dr["epdd_remarks"].ToString();
                                //    objPD.UrineCheckedDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}
                                if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 10)
                                {
                                    objPD.IsAnyOtherDisease = true;
                                    objPD.AnyOtherDiseaseDetails = dr["epdd_remarks"].ToString();
                                    objPD.AnyOtherDiseaseDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                }
                                if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 11)
                                {
                                    objPD.IsDrinksDrugs = true;
                                    objPD.DrinksDrugsDetails = dr["epdd_remarks"].ToString();
                                    objPD.DrinksDrugsDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                }
                                if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 12)
                                {
                                    objPD.IsAbsent = true;
                                    objPD.AbsentDetails = dr["epdd_remarks"].ToString();
                                    objPD.AbsentDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                }
                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 13)
                                //{
                                //    objPD.IsPlaceChange = true;
                                //    objPD.PlaceChangeDetails = dr["epdd_remarks"].ToString();
                                //    objPD.PlaceChangeDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}
                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 14)
                                //{
                                //    objPD.IsProposalMade = true;
                                //}

                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 15)
                                //{
                                //    objPD.ProposalAccepted = true;
                                //    objPD.ProposalAcceptedDetails = dr["epdd_remarks"].ToString();
                                //    objPD.ProposalDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}

                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 16)
                                //{
                                //    objPD.ProposalPostponed = true;
                                //    objPD.ProposalPostponedDetails = dr["epdd_remarks"].ToString();
                                //    objPD.ProposalDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}

                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 17)
                                //{
                                //    objPD.ProposalDeclined = true;
                                //    objPD.ProposalDeclinedDetails = dr["epdd_remarks"].ToString();
                                //    objPD.ProposalDocFileName = dr["epdd_upload_document_path"].ToString().Replace(@"\", @"/");
                                //}

                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 18)
                                //{
                                //    objPD.OrganizationOrPolicyNumber = dr["epdd_remarks"].ToString();
                                //}
                                //if (Convert.ToInt16(dr["epdd_personal_health_id"]) == 19)
                                //{
                                //    objPD.PolicyOrProposalNumber = dr["epdd_remarks"].ToString();
                                //}
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }

        public DataTable CreateDataTable(string className)
        {
            DataTable dt = new System.Data.DataTable();
            Type classtype = GetType();
            if (className == "PersonalData")
            {
                classtype = typeof(PersonalData);
            }
            else if (className == "DiseaseData")
            {
                classtype = typeof(DiseaseData);
            }
            else if (className == "MedicalLeaveData")
            {
                classtype = typeof(MedicalLeaveData);
            }
            else if (className == "VM_FamilyDetail")
            {
                classtype = typeof(VM_FamilyDetail);
            }
            PropertyInfo[] properties = classtype.GetProperties();

            foreach (System.Reflection.PropertyInfo pi in properties)
            {
                if (className == "VM_FamilyDetail" && pi.Name != "EditDeleteStatus" && pi.Name != "AppliactionSentBack" && pi.Name != "ApplicationInsured")
                    dt.Columns.Add(pi.Name);
                else if (className != "VM_FamilyDetail")
                    dt.Columns.Add(pi.Name);
            }
            return dt;
        }
        public int SaveNBPersonal(VM_PersonalHealthDetails objPersonal)
        {
            int result = 0;
            string PDate = string.Empty;
            try
            {
                // string MDate = string.Empty;
                // string DDate = string.Empty;
                //if (!string.IsNullOrEmpty(objPersonal.PeriodDate))
                //{
                //    PDate = _commnobj.DateConversion2(objPersonal.PeriodDate);
                //}
                //if (!string.IsNullOrEmpty(objPersonal.lastmensuration))
                //{
                //    MDate = _commnobj.DateConversion(objPersonal.lastmensuration);
                //}
                //if (!string.IsNullOrEmpty(objPersonal.dateoflastdelivery))
                //{
                //    DDate = _commnobj.DateConversion(objPersonal.dateoflastdelivery);
                //}
                DataTable dt_PersonalData = new DataTable();
                PersonalData objPersonalData = new PersonalData();
                dt_PersonalData = CreateDataTable("PersonalData");
                DataRow dr = dt_PersonalData.NewRow();
                dr["ephd_emp_id"] = objPersonal.ephd_emp_id;
                dr["ephd_health_condition"] = objPersonal.ephd_health_condition ?? false;
                dr["ephd_active_status"] = true;
                dr["ephd_application_id"] = objPersonal.ephd_application_id ?? 0;
                dr["ephd_date_of_last_period"] = (objPersonal.ephd_date_of_last_period) != null ? Convert.ToDateTime(objPersonal.ephd_date_of_last_period).ToString("MM/dd/yyyy") : null; //objPersonal.ephd_date_of_last_period;
                dr["ephd_height"] = objPersonal.ephd_height;
                //  dr["ephd_husband_occupation_address"] = objPersonal.ephd_husband_occupation_address ?? "";
                dr["ephd_is_pregnant"] = objPersonal.ephd_is_pregnant ?? false;
                // dr["ephd_no_of_years_of_marriage"] = objPersonal.ephd_no_of_years_of_marriage ?? 0;
                dr["ephd_weight"] = objPersonal.ephd_weight;
                //  dr["ephd_is_married"] = objPersonal.ephd_is_married;

                //  dr["ephd_husbandGovEmp"] = objPersonal.ephd_husbandGovEmp;
                //  dr["ephd_IsInsuredOfficialBrch"] = objPersonal.IsinsuredOfficialBrch ?? false;
                //  dr["ephd_IshusbandLIC"] = objPersonal.IshusbandLIC ?? false;
                //   dr["ephd_insuredOfficialBrch"] = objPersonal.ephd_insuredOfficialBrch;
                //   dr["ephd_husbandLIC"] = objPersonal.ephd_husbandLIC; ;
                //   dr["ephd_menstruating"] = objPersonal.ephd_menstruating ?? false;
                //if (objPersonal.ephd_lastmensuration != null)
                //dr["ephd_lastmensuration"] = (objPersonal.PeriodDate) != null ? Convert.ToDateTime(objPersonal.PeriodDate).ToString("MM/dd/yyyy") : null; //PDate;

                //dr["ephd_nopregnancies"] = objPersonal.ephd_nopregnancies;
                //dr["ephd_gonefulltime"] = objPersonal.ephd_gonefulltime;
                dr["ephd_is_pregnant"] = objPersonal.ephd_is_pregnant;
                // if (objPersonal.ephd_dateoflastdelivery != null)
                //dr["ephd_dateoflastdelivery"] = DDate;
                //Convert.ToDateTime(objPersonal.dateoflastdelivery).ToString("MM/dd/yyyy");

                // dr["ephd_miscarriages"] = objPersonal.ephd_miscarriages;

                dt_PersonalData.Rows.Add(dr);
                DataTable dt_DiseaseData = CreateDataTable("DiseaseData");
                //if (objPersonal.IsMedicalAdvice == true)
                //{
                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 1, objPersonal.MedicalAdviceDetails, objPersonal.MedicalAdviceDoc, dt_DiseaseData);
                //}
                //if (objPersonal.IsFamilyMemberAffectedByDisease == true)
                //{
                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 2, objPersonal.FamilyMemberAffectedByDiseaseDetails, objPersonal.FamilyMemberAffectedByDiseaseDoc, dt_DiseaseData);
                //}
                if (objPersonal.IsInfectiousDiseaseHouse == true)
                {
                    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 3, objPersonal.InfectiousDiseaseHouseDetails, objPersonal.InfectiousDiseaseHouseDoc, dt_DiseaseData);
                }
                //if (objPersonal.IsBrainDisease == true)
                //{
                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 4, objPersonal.BrainDiseaseDetails, objPersonal.BrainDiseaseDoc, dt_DiseaseData);
                //}
                //if (objPersonal.IsDiseaseOfLungs == true)
                //{
                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 5, objPersonal.DiseaseOfLungsDetails, objPersonal.DiseaseOfLungsDoc, dt_DiseaseData);
                //}
                //if (objPersonal.IsLiverKidneyDisease == true)
                //{
                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 6, objPersonal.LiverKidneyDiseaseDetails, objPersonal.LiverKidneyDiseaseDoc, dt_DiseaseData);
                //}
                if (objPersonal.IsStomachDisease == true)
                {
                    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 7, objPersonal.StomachDiseaseDetails, objPersonal.StomachDiseaseDoc, dt_DiseaseData);
                }
                //if (objPersonal.IsRheumaticFever == true)
                //{
                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 8, objPersonal.RheumaticFeverDetails, objPersonal.RheumaticFeverDoc, dt_DiseaseData);
                //}
                //if (objPersonal.IsUrineChecked == true)
                //{
                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 9, objPersonal.UrineCheckedDetails, objPersonal.UrineCheckedDoc, dt_DiseaseData);
                //}
                if (objPersonal.IsAnyOtherDisease == true)
                {
                    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 10, objPersonal.AnyOtherDiseaseDetails, objPersonal.AnyOtherDiseaseDoc, dt_DiseaseData);
                }
                if (objPersonal.IsDrinksDrugs == true)
                {
                    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 11, objPersonal.DrinksDrugsDetails, objPersonal.DrinksDrugsDoc, dt_DiseaseData);
                }
                if (objPersonal.IsAbsent == true)
                {
                    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 12, objPersonal.AbsentDetails, objPersonal.AbsentDoc, dt_DiseaseData);
                }
                //if (objPersonal.IsPlaceChange == true)
                //{
                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 13, objPersonal.PlaceChangeDetails, objPersonal.PlaceChangeDoc, dt_DiseaseData);
                //}
                //if (objPersonal.IsProposalMade == true)
                //{
                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 14, string.Empty, null, dt_DiseaseData);

                //    if (objPersonal.ProposalAccepted)
                //    {
                //        dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 15, objPersonal.ProposalAcceptedDetails, objPersonal.ProposalDoc, dt_DiseaseData);
                //    }
                //    else if (objPersonal.ProposalPostponed)
                //    {
                //        dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 16, objPersonal.ProposalPostponedDetails, objPersonal.ProposalDoc, dt_DiseaseData);
                //    }
                //    else if (objPersonal.ProposalDeclined)
                //    {
                //        dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 17, objPersonal.ProposalDeclinedDetails, objPersonal.ProposalDoc, dt_DiseaseData);
                //    }
                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 18, objPersonal.OrganizationOrPolicyNumber, null, dt_DiseaseData);

                //    dt_DiseaseData = UpdateDeatils(objPersonal.ephd_emp_id, objPersonal.ephd_application_id, 19, objPersonal.PolicyOrProposalNumber, null, dt_DiseaseData);
                //}

                if (dt_DiseaseData.Select().Any(e => e.ItemArray[5].ToString() == "File Extension Is InValid - Only Upload PDF File"))
                {
                    result = -1;
                    return result;
                }
                else
                {
                    SqlParameter[] sqlparam =
                        {
                         new SqlParameter("@EmpID",objPersonal.ephd_emp_id),
                         new SqlParameter("@PersonalData",dt_PersonalData),
                         new SqlParameter("@DiseaseData",dt_DiseaseData)
                        };
                    result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertPersonaldetails"));
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "insertPersonaldetails:-" + ex.Message);
            }
            return result;
        }

        public DataTable UpdateDeatils(long? EmpCode, long? ApplicationID, int HealthCode, string DiseasesDetails, HttpPostedFileBase diseaseDoc, DataTable dt_diseasedata)
        {
            //if (string.IsNullOrEmpty(epdd_upload_document_path))
            //{
            //    epdd_upload_document_path = UploadDocument(medicalDoc, emd_emp_id, medDocType);
            //}
            DataRow Ddr = dt_diseasedata.NewRow();
            Ddr["epdd_emp_id"] = EmpCode ?? 0;
            Ddr["epdd_personal_health_id"] = HealthCode;
            Ddr["epdd_remarks"] = DiseasesDetails;
            Ddr["epdd_status"] = true;
            if (diseaseDoc != null)
                Ddr["epdd_upload_document_path"] = UploadDocument(diseaseDoc, EmpCode, "Personal");
            Ddr["epdd_application_id"] = ApplicationID ?? 0;
            dt_diseasedata.Rows.Add(Ddr);
            return dt_diseasedata;
        }

        private string UploadDocument(HttpPostedFileBase document, long? empId, string docType)
        {
            string subPath = string.Empty;
            try
            {
                string fileExt = string.Empty;
                var supportedTypes = new[] { "pdf" }; if (document != null)
                {
                    fileExt = System.IO.Path.GetExtension(document.FileName).Substring(1);
                }
                if (!supportedTypes.Contains(fileExt))
                {
                    return "File Extension Is InValid - Only Upload PDF File";
                }
                if (document != null && document.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(document.FileName);
                    //subPath = @"F:/DOCUMENTS/";
                    subPath = WebConfigurationManager.AppSettings["RootDirectory"];
                    if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    {
                        subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"Personal\";
                    }
                    //bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(subPath));
                    //if (!exists)
                    //{
                    //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                    //}
                    string FileNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                    //string FPath = subPath + empId.ToString() + FileNo +  fileName;
                    //string P = HttpContext.Current.Server.MapPath(subPath) + fileName;
                    string path = Path.Combine(subPath, empId.ToString() + FileNo + fileName);

                    Logger.LogMessage(TracingLevel.INFO, "UploadDocument2 Path:-" + path);
                    document.SaveAs(path);
                    //subPath = subPath + "/" + fileName;
                    subPath = path;
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "UploadDocument2:-" + ex.Message);
            }
            return subPath;
        }
        #endregion
        #region MedicalLeave
        public VM_MedicalLeaveDetails MedicalLeaveDetailsDll(long empId, string loginType)
        {
            VM_MedicalLeaveDetails objPD = new VM_MedicalLeaveDetails();
            List<MedicalLeaveData> lstMedicalLeaveDetails = new List<MedicalLeaveData>();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",empId)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectMedicalLeaveDetails");

                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPD.Tables[0].Rows)
                        {

                            MedicalLeaveData objLeaveData = new MedicalLeaveData();
                            objLeaveData.emld_application_id = Convert.ToInt64(dr["emld_application_id"]);
                            objLeaveData.emld_medical_leave_id = Convert.ToInt64(dr["emld_medical_leave_id"]);
                            objLeaveData.emld_emp_id = Convert.ToInt64(dr["emld_emp_id"]);
                            objLeaveData.startdate = dr["emld_start_date"].ToString(); //.ToString("MM/dd/yyyy");
                            objLeaveData.enddate = dr["emld_end_date"].ToString();
                            objLeaveData.emld_leave_reason = dr["emld_leave_reason"].ToString();
                            objLeaveData.emld_no_of_days = Convert.ToDecimal(dr["emld_no_of_days"]);
                            objLeaveData.emld_medical_reimbursement = Convert.ToBoolean(dr["emld_medical_reimbursement"]);
                            objLeaveData.emld_upload_document_path = dr["emld_upload_document_path"].ToString();//GetFileName(dr["emld_upload_document_path"].ToString());
                            objLeaveData.supportingdocpath = dr["emld_upload_document_path"].ToString();
                            objLeaveData.emld_medical_reimbursement_doc = dr["emld_medical_reimbursement_doc"].ToString();
                            objLeaveData.reimbursedocpath = dr["emld_medical_reimbursement_doc"].ToString();
                            objLeaveData.Type = loginType;
                            lstMedicalLeaveDetails.Add(objLeaveData);
                        }
                        objPD.MedicalLeaveDetails = lstMedicalLeaveDetails;
                    }
                    if (dsPD.Tables[1].Rows.Count > 0)
                    {
                        objPD.JoiningDate = dsPD.Tables[1].Rows[0]["DateOJoining"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objPD;
        }

        public VM_MedicalLeaveDetails SaveMedicalLeaveDll(VM_MedicalLeaveDetails objMedicalLeaveDetails)
        {
            VM_MedicalLeaveDetails list = new VM_MedicalLeaveDetails();
            try
            {
                DataTable dt_MedicalLeaveData = new DataTable();
                MedicalLeaveData objMedicalLeaveData = new MedicalLeaveData();
                dt_MedicalLeaveData = CreateDataTable("MedicalLeaveData");
                dt_MedicalLeaveData.Columns.Remove("startdate");
                dt_MedicalLeaveData.Columns.Remove("enddate");
                dt_MedicalLeaveData.Columns.Remove("supportingdocpath");
                dt_MedicalLeaveData.Columns.Remove("reimbursedocpath");
                dt_MedicalLeaveData.Columns.Remove("doc");
                dt_MedicalLeaveData.Columns.Remove("reimbursedoc");
                foreach (MedicalLeaveData obj in objMedicalLeaveDetails.MedicalLeaveDetails)
                {
                    if (!string.IsNullOrEmpty(obj.startdate))
                    {
                        obj.startdate = _commnobj.DateConversion(obj.startdate);
                    }
                    if (!string.IsNullOrEmpty(obj.enddate))
                    {
                        obj.enddate = _commnobj.DateConversion(obj.enddate);
                    }
                    DataRow dr = dt_MedicalLeaveData.NewRow();
                    dr["emld_application_id"] = obj.emld_application_id ?? 0;
                    dr["emld_emp_id"] = objMedicalLeaveDetails.EmpCode;
                    dr["emld_active_status"] = true;
                    dr["emld_end_date"] = _commnobj.DateConversion(obj.enddate);
                    dr["emld_start_date"] = _commnobj.DateConversion(obj.startdate);
                    dr["emld_leave_reason"] = obj.emld_leave_reason;
                    dr["emld_medical_reimbursement"] = Convert.ToBoolean(obj.emld_medical_reimbursement);
                    dr["emld_medical_leave_id"] = obj.emld_medical_leave_id ?? 0;
                    dr["emld_upload_document_path"] = obj.emld_upload_document_path;
                    dr["emld_no_of_days"] = obj.emld_no_of_days;
                    dr["emld_medical_reimbursement_doc"] = obj.emld_medical_reimbursement_doc;
                    dt_MedicalLeaveData.Rows.Add(dr);
                }
                SqlParameter[] sqlparam =
                {
                     new SqlParameter("@MedicalLeaveData",dt_MedicalLeaveData),
                     new SqlParameter("@EmpCode",objMedicalLeaveDetails.EmpCode)
                };
                int result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertMedicalleavedetails"));

                //list = MedicalLeaveDetailsDll(objMedicalLeaveDetails.EmpCode);
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public string UploadMedicalLeaveDoc(MedicalLeaveData objMedicalLeaveDetails)
        {
            string docpath = string.Empty, reimbursedoc = string.Empty;
            if (objMedicalLeaveDetails.doc != null)
            {
                if (objMedicalLeaveDetails.doc.ContentType == "application/pdf")
                {
                    docpath = UploadDocument(objMedicalLeaveDetails.doc, objMedicalLeaveDetails.emld_emp_id, "MedicalLeave");
                }
                else
                {
                    docpath = objMedicalLeaveDetails.doc.FileName.Replace("'", "").Replace("'", "");
                }
            }
            if (objMedicalLeaveDetails.reimbursedoc != null)
            {
                if (objMedicalLeaveDetails.reimbursedoc.ContentType == "application/pdf")
                {
                    reimbursedoc = UploadDocument(objMedicalLeaveDetails.reimbursedoc, objMedicalLeaveDetails.emld_emp_id, "MedicalLeave");
                }
                else
                {
                    reimbursedoc = objMedicalLeaveDetails.reimbursedoc.FileName.Replace("'", "").Replace("'", "");
                }

            }
            return docpath + "~" + reimbursedoc;
        }

        public int DeleteMedicalLeaveDll(long EmpCode)
        {
            SqlParameter[] param =
            {
                 new SqlParameter("@EmpCode",EmpCode)
            };
            int output = Convert.ToInt32(_Conn.ExecuteCmd(param, "sp_kgid_deleteMedicalLeaveDetails"));
            return output;
        }
        #endregion
        #region upload
        public string AddEmployeeBasicDetails(VM_BasicDetails employeeDetails)
        {
            string DuplicateDetails = "";
            try
            {
                //var newEmpDetails = (from emp in _db.tbl_employee_basic_details
                //                     where emp.employee_id == employeeDetails.employee_id
                //                     select emp).FirstOrDefault();
                //var CheckDupEmpDetails = (from emp in _db.tbl_employee_basic_details
                //                          where ((emp.pan_number == employeeDetails.pan_number || emp.mobile_number == employeeDetails.mobile_number
                //                          || emp.email_id == employeeDetails.email_id) && emp.employee_id != employeeDetails.employee_id)
                //                          select emp).Count();

                var CheckDupMobile = (from emp in _db.tbl_employee_basic_details
                                      where ((emp.mobile_number == employeeDetails.mobile_number) && emp.employee_id != employeeDetails.employee_id)
                                      select emp).Count();
                var CheckDupEmail = (from emp in _db.tbl_employee_basic_details
                                     where ((emp.email_id == employeeDetails.email_id) && emp.employee_id != employeeDetails.employee_id)
                                     select emp).Count();
                var CheckDupPanNo = (from emp in _db.tbl_employee_basic_details
                                     where ((emp.pan_number == employeeDetails.pan_number) && emp.employee_id != employeeDetails.employee_id)
                                     select emp).Count();

                if (CheckDupMobile > 0)
                {
                    DuplicateDetails = "Mobile number,";
                }
                if (CheckDupEmail > 0)
                {
                    DuplicateDetails = "Email id" + "," + DuplicateDetails;
                }
                if (CheckDupPanNo > 0)
                {
                    DuplicateDetails = "Pan number" + "," + DuplicateDetails;
                }

                //employeeDetails.dm_dept_code = (from d in _db.tbl_department_master
                //                                where d.dm_active == true && d.dm_deptcode == employeeDetails.department
                //                                select d.dm_dept_id.ToString()).FirstOrDefault();
                int CODE = Convert.ToInt32(employeeDetails.payscalecode);
                employeeDetails.payscale_minimum = (from ps in _db.tbl_payscales_master
                                                    where ps.payscale_status == 1 && ps.payscale_id == employeeDetails.ewd_payscale_id
                                                    select ps.payscale_minimum).FirstOrDefault();

                if (DuplicateDetails == "")
                {
                    using (var dbContextTransaction = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            tbl_employee_basic_details objBD = new tbl_employee_basic_details();
                            objBD.dept_employee_code = Convert.ToInt32(employeeDetails.dept_employee_code);
                            objBD.employee_name = employeeDetails.employee_name;
                            objBD.father_name = employeeDetails.father_name;
                            objBD.spouse_name = employeeDetails.spouse_name;

                            objBD.employee_name_kannada = employeeDetails.employee_name_kannada;
                            objBD.father_name_kannada = employeeDetails.father_name_kannada;
                            objBD.spouse_name_kannada = employeeDetails.spouse_name_kannada;

                            objBD.gender_id = employeeDetails.gender_id;
                            objBD.date_of_birth = employeeDetails.dateofbirth;
                            objBD.place_of_birth = employeeDetails.place_of_birth;
                            objBD.pan_number = employeeDetails.pan_number;
                            objBD.date_of_appointment = employeeDetails.dateofappointment;
                            objBD.mobile_number = employeeDetails.mobile_number;
                            objBD.email_id = employeeDetails.email_id;
                            objBD.user_category_id = "1";
                            objBD.active_status = true;
                            objBD.creation_datetime = DateTime.Now;
                            objBD.updation_datetime = DateTime.Now;
                            objBD.created_by = employeeDetails.created_by;
                            objBD.updated_by = employeeDetails.created_by;
                            objBD.ddo_upload_status = false;
                            _db.tbl_employee_basic_details.Add(objBD);
                            _db.SaveChanges();

                            long EMPID = (from emp in _db.tbl_employee_basic_details orderby emp.employee_id descending select emp.employee_id).First();

                            //long ID = objBD.employee_id;

                            tbl_employee_work_details objEWDetails = new tbl_employee_work_details();
                            objEWDetails.ewd_emp_id = EMPID;
                            objEWDetails.ewd_place_of_posting = employeeDetails.ewd_place_of_posting;
                            objEWDetails.ewd_date_of_joining = employeeDetails.ewddateofjoining;
                            objEWDetails.ewd_payscale_id = employeeDetails.ewd_payscale_id;
                            objEWDetails.ewd_employment_type = employeeDetails.et_employee_type_id;
                            objEWDetails.ewd_group_id = employeeDetails.ewd_group_id;
                            objEWDetails.ewd_designation_id = employeeDetails.d_designation_id;
                            objEWDetails.ewd_ddo_id = employeeDetails.ewd_ddo_id;
                            objEWDetails.ewd_active_status = true;
                            objEWDetails.ewd_created_by = employeeDetails.created_by;
                            objEWDetails.ewd_creation_datetime = DateTime.Now;
                            objEWDetails.ewd_updated_by = employeeDetails.created_by;
                            objEWDetails.ewd_updation_datetime = DateTime.Now;
                            _db.tbl_employee_work_details.Add(objEWDetails);
                            _db.SaveChanges();

                            KGID_Models.KGIDLoan.tbl_hrms_pay_details_master objHRMSPay = new KGID_Models.KGIDLoan.tbl_hrms_pay_details_master();
                            objHRMSPay.hrms_emp_id = EMPID;
                            objHRMSPay.hrms_month_id = Convert.ToInt32(DateTime.Now.Month - 1);
                            objHRMSPay.hrms_year_id = Convert.ToInt32(DateTime.Now.Year - 1);
                            objHRMSPay.hrms_gross_pay = Convert.ToInt32(employeeDetails.payscale_minimum);
                            objHRMSPay.hrms_active = true;
                            objHRMSPay.hrms_creation_datetime = DateTime.Now;
                            objHRMSPay.hrms_created_by = employeeDetails.created_by;
                            objHRMSPay.hrms_updation_datetime = DateTime.Now;
                            objHRMSPay.hrms_updated_by = employeeDetails.created_by;
                            _db.tbl_hrms_pay_details_master.Add(objHRMSPay);
                            _db.SaveChanges();
                            dbContextTransaction.Commit();
                            DuplicateDetails = "1";
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return DuplicateDetails;
        }
        public string SaveEmployeeBasicDetails(VM_BasicDetails employeeDetails)
        {
            Logger.LogMessage(TracingLevel.INFO, "SaveEmployeeBasicDetails Enter");
            string DuplicateDetails = "";
            try
            {
                var newEmpDetails = (from emp in _db.tbl_employee_basic_details
                                     where emp.employee_id == employeeDetails.employee_id
                                     select emp).FirstOrDefault();
                var CheckDupEmpDetails = (from emp in _db.tbl_employee_basic_details
                                          where ((emp.pan_number == employeeDetails.pan_number || emp.mobile_number == employeeDetails.mobile_number
                                          || emp.email_id == employeeDetails.email_id) && emp.employee_id != employeeDetails.employee_id)
                                          select emp).Count();

                var CheckDupMobile = (from emp in _db.tbl_employee_basic_details
                                      where ((emp.mobile_number == employeeDetails.mobile_number) && emp.employee_id != employeeDetails.employee_id)
                                      select emp).Count();
                var CheckDupEmail = (from emp in _db.tbl_employee_basic_details
                                     where ((emp.email_id == employeeDetails.email_id) && emp.employee_id != employeeDetails.employee_id)
                                     select emp).Count();
                var CheckDupPanNo = (from emp in _db.tbl_employee_basic_details
                                     where ((emp.pan_number == employeeDetails.pan_number) && emp.employee_id != employeeDetails.employee_id)
                                     select emp).Count();

                if (CheckDupMobile > 0)
                {
                    DuplicateDetails = "Mobile number,";
                }
                if (CheckDupEmail > 0)
                {
                    DuplicateDetails = "Email id" + "," + DuplicateDetails;
                }
                if (CheckDupPanNo > 0)
                {
                    DuplicateDetails = "Pan number" + "," + DuplicateDetails;
                }


                if (CheckDupEmpDetails < 1)
                {
                    if (newEmpDetails != null)
                    {


                        newEmpDetails.active_status = employeeDetails.active_status;
                        newEmpDetails.date_of_appointment = employeeDetails.dateofappointment;
                        Logger.LogMessage(TracingLevel.INFO, "date_of_appointment :-" + newEmpDetails.date_of_appointment);
                        newEmpDetails.date_of_birth = employeeDetails.dateofbirth;
                        Logger.LogMessage(TracingLevel.INFO, "date_of_birth :-" + newEmpDetails.date_of_birth);
                        newEmpDetails.dept_employee_code = Convert.ToInt32(employeeDetails.department);
                        newEmpDetails.email_id = employeeDetails.email_id;
                        newEmpDetails.employee_name = employeeDetails.employee_name;
                        newEmpDetails.father_name = employeeDetails.father_name;
                        newEmpDetails.gender_id = Convert.ToInt16(employeeDetails.gender);
                        newEmpDetails.mobile_number = employeeDetails.mobile_number;
                        newEmpDetails.pan_number = employeeDetails.pan_number;
                        newEmpDetails.place_of_birth = employeeDetails.place_of_birth;
                        newEmpDetails.spouse_name = employeeDetails.spouse_name;
                        newEmpDetails.updation_datetime = DateTime.Now;
                        newEmpDetails.employee_name_kannada = employeeDetails.employee_name_kannada;
                        newEmpDetails.father_name_kannada = employeeDetails.father_name_kannada;
                        newEmpDetails.spouse_name_kannada = employeeDetails.spouse_name_kannada;
                    }
                    var newEmpWorkDetails = (from empl in _db.tbl_employee_work_details
                                             join emp in _db.tbl_employee_basic_details on empl.ewd_emp_id equals emp.employee_id
                                             where emp.employee_id == employeeDetails.employee_id
                                             select empl).FirstOrDefault();

                    if (newEmpWorkDetails != null)
                    {
                        newEmpWorkDetails.ewd_date_of_joining = employeeDetails.ewddateofjoining;
                        Logger.LogMessage(TracingLevel.INFO, "ewd_date_of_joining :-" + newEmpWorkDetails.ewd_date_of_joining);
                        newEmpWorkDetails.ewd_place_of_posting = employeeDetails.ewd_place_of_posting;
                        newEmpWorkDetails.ewd_payscale_id = Convert.ToInt16(employeeDetails.payscalecode);
                        newEmpWorkDetails.ewd_employment_type = Convert.ToInt16(employeeDetails.emptype);
                        newEmpWorkDetails.ewd_designation_id = Convert.ToInt16(employeeDetails.designation);
                        newEmpWorkDetails.ewd_group_id = Convert.ToInt16(employeeDetails.group);
                        newEmpWorkDetails.ewd_ddo_id = Convert.ToInt16(employeeDetails.ddocode);
                    }

                    return Convert.ToString(_db.SaveChanges());
                }

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "GetEmailSMSTemplate" + ex.Message);
            }
            return DuplicateDetails;
        }
        #endregion
        #region Get Medical Leave
        public VM_DeptVerificationDetails GetMedicalLeaveDetails(long empId, long applicationId)
        {
            VM_DeptVerificationDetails verificationDetails = new VM_DeptVerificationDetails();
            //verificationDetails.MedicalLeave = Convert.ToInt32((from medLeave in _db.tbl_medical_leave
            //                                                    where medLeave.sys_emp_code == empId
            //                                                    select medLeave).ToList().Sum(t => t.leave_totl_duration));
            try
            {
                DataSet dsMLC = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",empId),
                    new SqlParameter("@applicationId",applicationId)
                };
                dsMLC = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectMedicalLeaveCount");
                if (dsMLC.Tables[0].Rows.Count > 0)
                {
                    verificationDetails.MedicalLeave = Convert.ToInt32(dsMLC.Tables[0].Rows[0]["emld_no_of_days"]);
                }

                if (dsMLC.Tables[1].Rows.Count > 0)
                {
                    //verificationDetails.HealthReportUploadPath = dsMLC.Tables[1].Rows[0]["HealthReportUploadPath"].ToString();
                    verificationDetails.ApplicationStatus = Convert.ToInt32(dsMLC.Tables[1].Rows[0]["ApplicationStatusId"].ToString());

                    verificationDetails.ApplicationId = Convert.ToInt64(dsMLC.Tables[1].Rows[0]["ApplicationId"].ToString());
                    verificationDetails.EmpCode = Convert.ToInt64(dsMLC.Tables[1].Rows[0]["EmployeeId"].ToString());
                    verificationDetails.Remarks = dsMLC.Tables[1].Rows[0]["Remarks"].ToString();
                    verificationDetails.Comments = dsMLC.Tables[1].Rows[0]["Comments"].ToString();
                    verificationDetails.DeductionLoadFactor = dsMLC.Tables[1].Rows[0]["DLFactor"].ToString();
                    verificationDetails.LoadFactor = dsMLC.Tables[1].Rows[0]["LoadFactor"].ToString();
                    verificationDetails.SumAssured = Convert.ToDecimal(dsMLC.Tables[1].Rows[0]["SumAssured"].ToString());

                    if (!string.IsNullOrEmpty(verificationDetails.HealthReportUploadPath))
                    {
                        verificationDetails.IsHealthOpinion = true;
                    }
                }
                if (dsMLC.Tables[2].Rows.Count > 0)
                {
                    verificationDetails.IsMedicalRequired = Convert.ToBoolean(dsMLC.Tables[2].Rows[0]["IsMedicalRequired"]);
                }
                if (dsMLC.Tables[3].Rows.Count > 0)
                {
                    verificationDetails.ApplicationFormLink = dsMLC.Tables[3].Rows[0]["App_Application_Form"].ToString();
                    verificationDetails.MedicalFormLink = dsMLC.Tables[3].Rows[0]["App_Medical_Form"].ToString();
                }

                if (dsMLC.Tables[4].Rows.Count > 0)
                {

                    foreach (DataRow row in dsMLC.Tables[4].Rows)
                    {
                        HealthOpinion objHO = new HealthOpinion();
                        //verificationDetails.HealthOpinionDocuments.Add(row["HealthOpinionDocument"].ToString());
                        objHO.HealthOpinionDocuments = row["HealthOpinionDocument"].ToString();
                        objHO.HealthOpinionType = Convert.ToInt32(row["CatagoryType"]);

                        verificationDetails.NeedHealthOpinion.Add(objHO);
                    }
                }
                DataSet dsUD = new DataSet();
                SqlParameter[] sqlparameters =
                {
                    new SqlParameter("@employee_id",empId),
                    new SqlParameter("@application_id",applicationId)
                };
                dsUD = _Conn.ExeccuteDataset(sqlparameters, "sp_kgid_selectEmployeeDocuments");
                if (dsUD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsUD.Tables[0].Rows)
                    {
                        UploadedDocuments objHO = new UploadedDocuments();
                        //verificationDetails.HealthOpinionDocuments.Add(row["HealthOpinionDocument"].ToString());
                        objHO.UploaddocPath = row["DocPath"].ToString();
                        objHO.UploaddocType = row["DocType"].ToString();
                        verificationDetails.listUploadDocuments.Add(objHO);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        #endregion
        #region WorkFlow
        #region DDO
        public VM_DDOVerificationDetails GetEmployeeDetailsForDDOVerification(long empId)
        {
            VM_DDOVerificationDetails verificationDetails = new VM_DDOVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",1),
                    new SqlParameter("@employee_id",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectDepartmentWorkflowDetails");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Status = dataRow.Field<string>("AppStatus"),
                    Priority = dataRow.Field<int>("AppType"),
                    RowNum = dataRow.Field<int>("RowNum"),
                    District = dataRow.Field<string>("District"),
                    Department = dataRow.Field<string>("Department")
                }).ToList();
                var IEmployeeVerification = dsDDO.Tables[4].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Status = dataRow.Field<string>("AppStatus"),
                    Priority = dataRow.Field<int>("AppType"),
                    District = dataRow.Field<string>("District"),
                    Department = dataRow.Field<string>("Department")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    //Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<long>("p_kgid_policy_number").ToString(),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.IEmployeeVerificationDetails = IEmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        #endregion
        #region Case worker
        public VM_VerificationDetails GetEmployeeDetailsForCWVerification(long empId)
        {
            VM_VerificationDetails verificationDetails = new VM_VerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",2),
                    new SqlParameter("@employee_id",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectDepartmentWorkflowDetails");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Status = dataRow.Field<string>("AppStatus"),
                    Priority = dataRow.Field<int>("AppType"),
                    District = dataRow.Field<string>("District"),
                    Department = dataRow.Field<string>("Department")
                }).ToList();
                var IEmployeeVerification = dsDDO.Tables[4].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Status = dataRow.Field<string>("AppStatus"),
                    Priority = dataRow.Field<int>("AppType"),
                    District = dataRow.Field<string>("District"),
                    Department = dataRow.Field<string>("Department")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("kad_application_id")
                }).ToList();
                var NeedsHealthOpinion = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("kad_application_id")
                }).ToList();
                verificationDetails.NeedsHealthOpinionForEmployees = NeedsHealthOpinion;
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.IEmployeeVerificationDetails = IEmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        #endregion

        #region AVG Case worker
        public VM_VerificationDetails GetEmployeeDetailsForAVGCWVerification(long empId)
        {
            VM_VerificationDetails verificationDetails = new VM_VerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",7),
                    new SqlParameter("@employee_id",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectDepartmentWorkflowDetails");

                var NeedsHealthOpinion = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("kad_application_id")
                }).ToList();
                verificationDetails.NeedsHealthOpinionForEmployees = NeedsHealthOpinion;
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        #endregion
        #region S I
        public VM_VerificationDetails GetEmployeeDetailsForSIVerification(long empId)
        {
            VM_VerificationDetails verificationDetails = new VM_VerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",3),
                    new SqlParameter("@employee_id",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectDepartmentWorkflowDetails");
                Task<List<EmployeeVerificationDetail>> task3 = Task<List<EmployeeVerificationDetail>>.Factory.StartNew(() =>
                {
                    var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Status = dataRow.Field<string>("AppStatus"),
                    Priority = dataRow.Field<int>("AppType"),
                    District = dataRow.Field<string>("District"),
                    Department = dataRow.Field<string>("Department")
                }).ToList();
                var IEmployeeVerification = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Status = dataRow.Field<string>("AppStatus"),
                    Priority = dataRow.Field<int>("AppType"),
                    District = dataRow.Field<string>("District"),
                    Department = dataRow.Field<string>("Department")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("kad_application_id")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.IEmployeeVerificationDetails = IEmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
                    return EmployeeVerification;
                });
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        #endregion
        #region DIO
        public VM_VerificationDetails GetEmployeeDetailsForDIOVerification(long empId)
        {
            VM_VerificationDetails verificationDetails = new VM_VerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",4),
                    new SqlParameter("@employee_id",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectDepartmentWorkflowDetails");
                Task<List<EmployeeVerificationDetail>> task3 = Task<List<EmployeeVerificationDetail>>.Factory.StartNew(() =>
                {
                    var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                    {
                        EmployeeCode = dataRow.Field<long?>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                        ApplicationId = dataRow.Field<long>("kad_application_id"),
                        Status = dataRow.Field<string>("AppStatus"),
                        Priority = dataRow.Field<int>("AppType"),
                        District = dataRow.Field<string>("District"),
                        Department = dataRow.Field<string>("Department")
                    }).ToList();
                    var IEmployeeVerification = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                    {
                        EmployeeCode = dataRow.Field<long?>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                        ApplicationId = dataRow.Field<long>("kad_application_id"),
                        Status = dataRow.Field<string>("AppStatus"),
                        Priority = dataRow.Field<int>("AppType"),
                        District = dataRow.Field<string>("District"),
                        Department = dataRow.Field<string>("Department")
                    }).ToList();
                    var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                    {
                        EmployeeCode = dataRow.Field<long>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                        Status = dataRow.Field<string>("asm_status_desc"),
                        LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),
                        ApplicationId = dataRow.Field<long>("kad_application_id")
                    }).ToList();
                    verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                    verificationDetails.IEmployeeVerificationDetails = IEmployeeVerification;
                    verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                   // return EmployeeVerification;
                    if (dsDDO.Tables[2].Rows.Count > 0)
                    {
                        if (dsDDO.Tables[2].Rows.Count == 1)
                        {
                            verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                            verificationDetails.SentBackApplication = 0;
                        }
                        else
                        {
                            verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                            verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        }
                        //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                    }   return EmployeeVerification;
                });
            }







            catch (Exception ex)
            {

            }
            return verificationDetails;
      
        }

        public VM_DeptVerificationDetails GetPolicyCalculations(long empId, long applicationId)
        {
            VM_DeptVerificationDetails verificationDetails = new VM_DeptVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employeeId",empId),
                    new SqlParameter("@applicationId",applicationId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getPolicyCalculations");

                if (dsDDO.Tables.Count > 0 && dsDDO.Tables[0].Rows.Count > 0)
                {
                    verificationDetails = new VM_DeptVerificationDetails();
                    verificationDetails.SumAssured = Convert.ToDecimal(dsDDO.Tables[0].Rows[0]["TotalSumAssured"].ToString());
                    verificationDetails.LoadFactor = dsDDO.Tables[0].Rows[0]["LoadFactor"].ToString();
                    verificationDetails.DeductionLoadFactor = dsDDO.Tables[0].Rows[0]["DLFactor"].ToString();
                }

                if (dsDDO.Tables.Count > 0 && dsDDO.Tables[1].Rows.Count > 0 && verificationDetails != null)
                {
                    foreach (DataRow rowItem in dsDDO.Tables[1].Rows)
                    {
                        verificationDetails.DeductionLoadFactors.Add(new SelectListItem()
                        {
                            Text = rowItem["dlfm_dl_factor_desc"].ToString(),
                            Value = rowItem["dlfm_dl_factor_desc"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        #endregion
        #region D D
        public VM_VerificationDetails GetEmployeeDetailsForDDVerification(long empId)
        {
            VM_VerificationDetails verificationDetails = new VM_VerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",5),
                    new SqlParameter("@employee_id",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectDepartmentWorkflowDetails");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Status = dataRow.Field<string>("AppStatus"),
                    Priority = dataRow.Field<int>("AppType"),
                    District = dataRow.Field<string>("District"),
                    Department = dataRow.Field<string>("Department")
                }).ToList();
                var IEmployeeVerification = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Status = dataRow.Field<string>("AppStatus"),
                    Priority = dataRow.Field<int>("AppType"),
                    District = dataRow.Field<string>("District"),
                    Department = dataRow.Field<string>("Department")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("kad_application_id")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.IEmployeeVerificationDetails = IEmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        #endregion
        #region D
        public VM_VerificationDetails GetEmployeeDetailsForDVerification(long empId)
        {
            VM_VerificationDetails verificationDetails = new VM_VerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",6),
                    new SqlParameter("@employee_id",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectDepartmentWorkflowDetails");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Status = dataRow.Field<string>("AppStatus"),
                    Priority = dataRow.Field<int>("AppType"),
                    District = dataRow.Field<string>("District"),
                    Department = dataRow.Field<string>("Department")
                }).ToList();
                var IEmployeeVerification = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    ApplicationId = dataRow.Field<long>("kad_application_id"),
                    Status = dataRow.Field<string>("AppStatus"),
                    Priority = dataRow.Field<int>("AppType"),
                    District = dataRow.Field<string>("District"),
                    Department = dataRow.Field<string>("Department")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetail
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("kad_application_id")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.IEmployeeVerificationDetails = IEmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        #endregion

        #region SAVE WORKFLOW

        public string SaveVerifiedDetails(VM_DeptVerificationDetails objVerification)
        {
            string returnString = string.Empty;

            try
            {
                if (objVerification.IsHealthOpinion)
                {
                    if (objVerification.HealthUploadDoc != null)
                    {
                        objVerification.HealthReportUploadPath = UploadDocument(objVerification.HealthUploadDoc, objVerification.EmpCode, "Health Opinion");
                    }
                }
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@employee_id",objVerification.EmpCode),
                    new SqlParameter("@kawt_application_id",objVerification.ApplicationId),
                    new SqlParameter("@kawt_verified_by",objVerification.CreatedBy),

                    new SqlParameter("@kawt_number_of_medical_leaves",(objVerification.MedicalLeave == null) ? 0 : objVerification.MedicalLeave),
                    new SqlParameter("@kawt_checklist_verification_status",objVerification.VerifyProposerDetails),
                    new SqlParameter("@kawt_remarks",objVerification.Remarks),
                    new SqlParameter("@kawt_comments",(objVerification.Comments != null) ? objVerification.Comments.Replace(Environment.NewLine, " ") : string.Empty),
                    new SqlParameter("@kawt_application_status",objVerification.ApplicationStatus),
                    new SqlParameter("@kawt_health_opinion_upload_path",objVerification.HealthReportUploadPath),
                    new SqlParameter("@kawt_active_status",true),
                    new SqlParameter("@kawt_created_by",objVerification.CreatedBy),
                    new SqlParameter("@kawt_creation_datetime",DateTime.Now),
                    new SqlParameter("@load_factor",objVerification.LoadFactor),
                    new SqlParameter("@dl_factor",objVerification.DeductionLoadFactor),
                    new SqlParameter("@total_sum_assured",objVerification.SumAssured)
                };

                returnString = _Conn.ExecuteCmd(sqlparam, "sp_kgid_insertDepartmentWorkflowVerification");
                if (objVerification.ApplicationStatus == 15)
                {
                    DataSet details = new DataSet();

                    SqlParameter[] sqlparamNotifDetails =
                    {
                        new SqlParameter("@employeeId", objVerification.EmpCode),
                        new SqlParameter("@applicationId",objVerification.ApplicationId)

                    };

                    details = _Conn.ExeccuteDataset(sqlparamNotifDetails, "sp_kgid_getNotificationDetails");
                    VM_NotificationDetails notificationDetails = new VM_NotificationDetails();

                    if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
                    {
                        notificationDetails.DDOEmailId = details.Tables[0].Rows[0]["DDOEmailId"].ToString();
                        notificationDetails.EmpEmailId = details.Tables[0].Rows[0]["EmpEmailId"].ToString();
                        notificationDetails.EmpMobileNumber = Convert.ToInt64(details.Tables[0].Rows[0]["EmpMobileNumber"].ToString());
                        notificationDetails.EmpName = details.Tables[0].Rows[0]["EmpName"].ToString();
                        notificationDetails.PolicyNumber = details.Tables[0].Rows[0]["PolicyNumber"].ToString();
                    }

                    SendInsurancePolicyNotification(notificationDetails);
                    returnString = notificationDetails.PolicyNumber;
                }

            }
            catch (Exception ex)
            {

            }
            //    var empDetail = (from n in _db.tbl_new_employee_basic_details
            //                 where n.nebd_sys_emp_code == objVerification.EmpCode
            //                 select n).FirstOrDefault();

            //var role = Convert.ToString((int)UserCategories.DDO);
            //var ddoDetail = (from n in _db.tbl_new_employee_basic_details
            //                 where n.nebd_ddo_code == empDetail.nebd_ddo_code && n.nebd_kgid_roles.Contains(role)
            //                 select n).FirstOrDefault();

            //var _VerificationDetails = (from n in _db.tbl_dept_verification_details
            //                            where n.dvd_sys_emp_code == objVerification.EmpCode
            //                            && n.dvd_status
            //                            select n).FirstOrDefault();

            //if (_VerificationDetails != null)
            //{
            //    _VerificationDetails.dvd_status = false;
            //}

            //tbl_dept_verification_details objDV = new tbl_dept_verification_details();
            //objDV.dvd_sys_emp_code = objVerification.EmpCode;
            //objDV.dvd_application_ref_no = _VerificationDetails.dvd_application_ref_no;
            //objDV.dvd_medical_leave = objVerification.MedicalLeave;
            //objDV.dvd_verify_proposer_details = objVerification.VerifyProposerDetails;
            //objDV.dvd_verify_payment_details = objVerification.VerifyPaymentDetails;
            //objDV.dvd_verify_medical_report_details = objVerification.VerifyMedicalDetails;
            //objDV.dvd_verify_medical_condition = objVerification.VerifyMedicalCondition;
            //objDV.dvd_remarks = objVerification.Remarks;
            //objDV.dvd_application_status = objVerification.ApplicationStatus;
            //if (objVerification.IsHealthOpinion)
            //{
            //    if (objVerification.HealthUploadDoc != null)
            //    {
            //        objDV.dvd_health_report_upload_path = UploadDocument(objVerification.HealthUploadDoc, objVerification.EmpCode, "Health Opinion");
            //    }
            //    else if (!string.IsNullOrEmpty(objVerification.HealthReportUploadPath))
            //    {
            //        objDV.dvd_health_report_upload_path = _VerificationDetails.dvd_health_report_upload_path;
            //    }
            //}
            //objDV.dvd_status = true;
            //objDV.dvd_created_by = objVerification.CreatedBy;
            //objDV.dvd_creation_datetime = DateTime.Now;

            //_db.tbl_dept_verification_details.Add(objDV);

            //if (!string.IsNullOrEmpty(objVerification.LoadFactor))
            //{
            //    tbl_medical_report_tran medicalReport = (from med in _db.tbl_medical_report_tran
            //                                             where med.mrt_sys_emp_code == objVerification.EmpCode
            //                                             && med.mrt_status.Value
            //                                             select med).FirstOrDefault();

            //    string loadFactor = objVerification.LoadFactor;
            //    if (!string.IsNullOrEmpty(objVerification.DeductionLoadFactor))
            //    {
            //        loadFactor = string.Format("{0}/{1}", loadFactor, objVerification.DeductionLoadFactor);
            //    }

            //    medicalReport.mrt_load_factor = loadFactor;
            //}

            //string newPolicyNumber = string.Empty;
            ///// APPROVE
            //if (objVerification.ApplicationStatus == 15)
            //{
            //    var presentDate = DateTime.Now;
            //    string date = string.Empty;
            //    string month = string.Empty;
            //    if (presentDate.Day < 10)
            //    {
            //        date = "0" + presentDate.Day.ToString();
            //    }

            //    if (presentDate.Month < 10)
            //    {
            //        month = "0" + presentDate.Month.ToString();
            //    }

            //    newPolicyNumber = "BNG" + date + month + presentDate.Year + empDetail.nebd_mobilenumber;
            //    tbl_kgid_mapping_details newKgidDetail = new tbl_kgid_mapping_details();

            //    var previousKgidDetail = (from prevKgid in _db.tbl_kgid_mapping_details
            //                              where prevKgid.kmd_emp_id == objVerification.EmpCode
            //                              select prevKgid).FirstOrDefault();

            //    if (previousKgidDetail == null)
            //    {
            //        newKgidDetail.kmd_first_policy_no = newPolicyNumber;
            //    }
            //    else
            //    {
            //        newKgidDetail.kmd_first_policy_no = previousKgidDetail.kmd_first_policy_no;
            //    }

            //    newKgidDetail.kmd_subsequent_policy_no = newPolicyNumber;
            //    newKgidDetail.kmd_emp_name = empDetail.nebd_emp_full_name;
            //    newKgidDetail.kmd_emp_id = empDetail.nebd_sys_emp_code;
            //    newKgidDetail.kmd_status = true;
            //    newKgidDetail.kmd_creation_datetime = DateTime.Now;
            //    newKgidDetail.kmd_updation_datetime = DateTime.Now;

            //    _db.tbl_kgid_mapping_details.Add(newKgidDetail);

            //    SendInsurancePolicyNotification(empDetail, newPolicyNumber);
            //}

            //var affectedRows = _db.SaveChanges();

            //string returnString = string.Empty;
            //if (objVerification.ApplicationStatus != 15)
            //{
            //    returnString = affectedRows.ToString();
            //}
            //else
            //{
            //    returnString = newPolicyNumber;
            //}

            return returnString;
        }
        public IList<VM_WorkflowDetail> GetWorkFlowDetails(long applicationId)
        {
            IList<VM_WorkflowDetail> workflowDetails = null;

            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@applicationId",applicationId)
                };

                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getWorkflowDetails");

                if (dsDDO.Tables.Count > 0 && dsDDO.Tables[0].Rows.Count > 0)
                {
                    workflowDetails = new List<VM_WorkflowDetail>();
                    foreach (DataRow dr in dsDDO.Tables[0].Rows)
                    {
                        VM_WorkflowDetail workflowDetail = new VM_WorkflowDetail();
                        workflowDetail.ApplicationRefNo = dr["ApplicationRefNo"].ToString();
                        workflowDetail.From = dr["From"].ToString();
                        workflowDetail.To = dr["To"].ToString();
                        workflowDetail.Remarks = dr["Remarks"].ToString();
                        workflowDetail.Comments = dr["Comments"].ToString();
                        workflowDetail.CreationDateTime = dr["CreationDateTime"].ToString();
                        workflowDetail.ApplicationStatus = dr["ApplicationStatus"].ToString();
                        workflowDetails.Add(workflowDetail);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return workflowDetails.OrderByDescending(t => t.CreationDateTime).ToList();
        }

        public VM_ApplicationDetail GenerateApplicationNumber(long empId)
        {
            VM_ApplicationDetail applicationDetail = null;
            DataSet dsDDO = new DataSet();
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employeeId",empId)
                };

                //dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_generateApplicationNumber");
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_generateApplicationNumber1");

                if (dsDDO.Tables != null && dsDDO.Tables[0].Rows != null)
                {
                    applicationDetail = new VM_ApplicationDetail();
                    applicationDetail.ApplicationId = Convert.ToInt64(dsDDO.Tables[0].Rows[0]["ApplicationId"].ToString());
                    applicationDetail.ApplicationNumber = dsDDO.Tables[0].Rows[0]["ApplicationNumber"].ToString();
                    applicationDetail.ApplicationCount = Convert.ToInt32(dsDDO.Tables[0].Rows[0]["ApplicationCount"]);
                    applicationDetail.SentBackAppliaction = Convert.ToInt32(dsDDO.Tables[0].Rows[0]["SentBackApplication"]);
                    applicationDetail.RestrictApplyingPolicy = (dsDDO.Tables[0].Rows[0]["RestrictApplyingPolicy"].ToString() != "") ? (Convert.ToInt32(dsDDO.Tables[0].Rows[0]["RestrictApplyingPolicy"])) : 0;
                    applicationDetail.Remarks = (dsDDO.Tables[0].Rows[0]["Remarks"].ToString() == "") ? 0 : Convert.ToInt32(dsDDO.Tables[0].Rows[0]["Remarks"]);
                    applicationDetail.PaymentStatus = Convert.ToInt32(dsDDO.Tables[0].Rows[0]["PaymentStatus"]);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return applicationDetail;
        }
        #endregion

        #endregion
        #endregion


        #endregion

        public VM_PaymentDetails NBPaymentDll(long EmpID)
        {
            VM_PaymentDetails obj = new VM_PaymentDetails();
            try
            {

                DataSet dsBD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpID",EmpID)
                };
                dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectchallandetails");
                if (dsBD.Tables[0].Rows.Count > 0)
                {
                    obj.EmpID = Convert.ToInt32(dsBD.Tables[0].Rows[0]["EmpID"]);
                    obj.cd_application_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["cd_application_id"]);
                    obj.ddo_code = dsBD.Tables[0].Rows[0]["ddo_code"].ToString();
                    obj.cd_challan_ref_no = dsBD.Tables[0].Rows[0]["cd_challan_ref_no"].ToString();
                    obj.cd_date_of_generation = dsBD.Tables[0].Rows[0]["cd_date_of_generation"].ToString();
                    obj.hoa = dsBD.Tables[0].Rows[0]["hoa"].ToString();
                    obj.cd_amount = Convert.ToInt32(dsBD.Tables[0].Rows[0]["cd_amount"]);
                    obj.PayStatus = dsBD.Tables[0].Rows[0]["PayStatus"].ToString();
                    //Newly Added 22-02-2021 Ujwal
                    obj.EmpName = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                    obj.cd_purpose_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["purpose_code"]);
                    obj.purpose_desc = dsBD.Tables[0].Rows[0]["purpose_desc"].ToString();
                    obj.cd_sub_purpose_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["sub_purpose_code"]);
                    obj.sub_purpose_desc = dsBD.Tables[0].Rows[0]["sub_purpose_desc"].ToString();
                }

            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        public long SaveNBPaymentDll(VM_PaymentDetails objPaymentDetails)
        {
            long result = 0;
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@Purposeid",objPaymentDetails.cd_purpose_id),
                    new SqlParameter("@Subpurposeid",objPaymentDetails.cd_sub_purpose_id),
                    new SqlParameter("@Amount",objPaymentDetails.cd_amount),
                    new SqlParameter("@AppID",objPaymentDetails.cd_application_id),
                    new SqlParameter("@EmpID",objPaymentDetails.EmpID),
                    new SqlParameter("@GenerationDate",objPaymentDetails.cd_date_of_generation),
                    new SqlParameter("@ChallanRefNo",objPaymentDetails.cd_challan_ref_no)
                };
                result = Convert.ToInt64(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertchallandetails"));
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public long SaveNBChallanStatusDll(VM_PaymentDetails objPaymentDetails)
        {
            long result = 0;
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@Chalanid",objPaymentDetails.cd_challan_ref_no),
                    new SqlParameter("@TranRefNo",objPaymentDetails.cs_transaction_ref_no),
                    new SqlParameter("@Amount",objPaymentDetails.cs_amount),
                    new SqlParameter("@Status",objPaymentDetails.cs_status),
                    new SqlParameter("@TransactionDate",objPaymentDetails.cs_transsaction_date),
                    new SqlParameter("@EmpID",objPaymentDetails.EmpID)
                };
                result = Convert.ToInt64(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertChallanstatus"));
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public VM_PaymentDetails NBChallanDetailsDll(long EmpID)
        {
            VM_PaymentDetails obj = new VM_PaymentDetails();
            try
            {

                DataSet dsBD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpID",EmpID)
                };
                dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectchallanstatus");
                if (dsBD.Tables[0].Rows.Count > 0)
                {
                    obj.EmpID = Convert.ToInt32(dsBD.Tables[0].Rows[0]["EmpID"]);
                    obj.cd_application_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["cd_application_id"]);
                    obj.cd_challan_ref_no = dsBD.Tables[0].Rows[0]["cd_challan_ref_no"].ToString();
                    obj.cd_date_of_generation = dsBD.Tables[0].Rows[0]["cd_date_of_generation"].ToString();
                    obj.cd_amount = Convert.ToInt32(dsBD.Tables[0].Rows[0]["cd_amount"]);
                    obj.cd_challan_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["cd_challan_id"]);
                    obj.cs_transaction_ref_no = dsBD.Tables[0].Rows[0]["cs_transaction_ref_no"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "NBChallanDetailsDll" + ex.Message);
            }
            return obj;
        }

        public IEnumerable<SelectListItem> GetReceiptTypeList(int ModuleType)
        {
            var types = new List<SelectListItem>();

            types = (from t in _db.tbl_Receipt_Type_Masters
                     select (new SelectListItem { Text = t.receipt_type_desc, Value = t.receipt_type_id.ToString() })).ToList();

            return types;
        }
        public IEnumerable<SelectListItem> GetPurposeTypeList(int ReceiptTypeID)
        {
            var types = new List<SelectListItem>();

            types = (from t in _db.tbl_Receipt_Purpose_Masters
                     where t.receipt_type_id == ReceiptTypeID
                     select (new SelectListItem { Text = t.purpose_desc, Value = t.purpose_id.ToString() })).ToList();

            return types;
        }
        public IEnumerable<SelectListItem> GetSubPurposeTypeList(int ReceiptTypeID)
        {
            var types = new List<SelectListItem>();

            types = (from t in _db.tbl_Receipt_Subpurpose_Masters
                     where t.purpose_id == ReceiptTypeID
                     select (new SelectListItem { Text = t.sub_purpose_desc, Value = t.sub_purpose_id.ToString() })).ToList();

            return types;
        }
        public IEnumerable<SelectListItem> GetHOATypeList()
        {
            var types = new List<SelectListItem>();

            types = (from t in _db.tbl_Hoa_Masters
                     select (new SelectListItem { Text = t.hoa_desc, Value = t.hoa_id.ToString() })).ToList();

            return types;
        }
        public VM_PaymentDetails NBPaymentDownloadDll(long EmpID)
        {
            VM_PaymentDetails obj = new VM_PaymentDetails();
            try
            {

                DataSet dsBD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpID",EmpID)
                };
                dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectpaymentdetails");
                if (dsBD.Tables[0].Rows.Count > 0)
                {
                    obj.ddo_code = dsBD.Tables[0].Rows[0]["ddo_code"].ToString();
                    obj.cd_challan_ref_no = dsBD.Tables[0].Rows[0]["cd_challan_ref_no"].ToString();
                    obj.cd_date_of_generation = dsBD.Tables[0].Rows[0]["cd_date_of_generation"].ToString();
                    obj.hoa = dsBD.Tables[0].Rows[0]["hoa"].ToString();
                    obj.cd_amount = Convert.ToInt32(dsBD.Tables[0].Rows[0]["cd_amount"]);
                    obj.purpose_desc = dsBD.Tables[0].Rows[0]["purpose_desc"].ToString();
                    obj.sub_purpose_desc = dsBD.Tables[0].Rows[0]["sub_purpose_desc"].ToString();
                    obj.receipt_type_desc = dsBD.Tables[0].Rows[0]["receipt_type_desc"].ToString();
                }

            }
            catch (Exception ex)
            {

            }
            return obj;
        }


        public IEnumerable<SelectListItem> GetDRTypeList()
        {
            var types = new List<SelectListItem>();

            types = (from t in _db.tbl_dr_master

                     select (new SelectListItem { Text = t.dr_desc, Value = t.dr_id.ToString() })).ToList();

            return types;
        }
        public IEnumerable<SelectListItem> GetFamilyRelationList(long EmpId)
        {
            var types = new List<SelectListItem>();
            bool isMarried = _db.tbl_employee_other_details.Where(a => a.eod_emp_id == EmpId).Select(a => a.eod_emp_married).FirstOrDefault();
            if (isMarried)
            {
                types = (from t in _db.tbl_family_relation_master
                         where t.frm_family_type_id == 1
                         select (new SelectListItem { Text = t.frm_relation_desc, Value = t.frm_relation_id.ToString() })).ToList();
            }
            else
            {
                types = (from t in _db.tbl_family_relation_master
                         where t.frm_family_type_id == 1 && (t.frm_relation_desc != "Spouse" && t.frm_relation_desc != "Daughter" && t.frm_relation_desc != "Son")
                         select (new SelectListItem { Text = t.frm_relation_desc, Value = t.frm_relation_id.ToString() })).ToList();
            }
            return types;
        }
        public string DSCLogin(long kgidno, string publickey)
        {
            string response = string.Empty;
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@publicKey",publickey),
                new SqlParameter("@empid",kgidno)
            };

            response = _Conn.ExecuteCmd(sqlparam, "sp_kgid_getDSCDetails");
            return response;
        }
        public int NBApplicationCancel(long AppId, long EmpId, string Comments)
        {
            int response = 0;
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@AppId",AppId),
                    new SqlParameter("@EmpId",EmpId),
                    new SqlParameter("@Comments",Comments)
                };
                response = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_cancelapplication"));
            }
            catch (Exception ex)
            {
                response = 0;
            }
            return response;
        }

        public int SaveDDOMedicalLeaveDll(MedicalLeaveData objMedicalLeaveDetails)
        {
            int result = 0;
            try
            {
                DataTable dt_MedicalLeaveData = new DataTable();
                MedicalLeaveData objMedicalLeaveData = new MedicalLeaveData();
                dt_MedicalLeaveData = CreateDataTable("MedicalLeaveData");
                dt_MedicalLeaveData.Columns.Remove("startdate");
                dt_MedicalLeaveData.Columns.Remove("enddate");
                dt_MedicalLeaveData.Columns.Remove("supportingdocpath");
                dt_MedicalLeaveData.Columns.Remove("reimbursedocpath");
                dt_MedicalLeaveData.Columns.Remove("doc");
                dt_MedicalLeaveData.Columns.Remove("reimbursedoc");
                dt_MedicalLeaveData.Columns.Remove("Type");
                DataRow dr = dt_MedicalLeaveData.NewRow();
                dr["emld_application_id"] = objMedicalLeaveDetails.emld_application_id ?? 0;
                dr["emld_emp_id"] = objMedicalLeaveDetails.emld_emp_id;
                dr["emld_active_status"] = true;
                dr["emld_end_date"] = (!string.IsNullOrEmpty(objMedicalLeaveDetails.enddate)) ? Convert.ToDateTime(objMedicalLeaveDetails.enddate).ToString("MM/dd/yyyy") : "";
                dr["emld_start_date"] = (!string.IsNullOrEmpty(objMedicalLeaveDetails.startdate)) ? Convert.ToDateTime(objMedicalLeaveDetails.startdate).ToString("MM/dd/yyyy") : "";
                dr["emld_leave_reason"] = objMedicalLeaveDetails.emld_leave_reason;
                dr["emld_medical_reimbursement"] = Convert.ToBoolean(objMedicalLeaveDetails.emld_medical_reimbursement);
                dr["emld_medical_leave_id"] = objMedicalLeaveDetails.emld_medical_leave_id ?? 0;
                dr["emld_upload_document_path"] = objMedicalLeaveDetails.emld_upload_document_path;
                dr["emld_no_of_days"] = objMedicalLeaveDetails.emld_no_of_days;
                dr["emld_medical_reimbursement_doc"] = objMedicalLeaveDetails.emld_medical_reimbursement_doc;
                dt_MedicalLeaveData.Rows.Add(dr);

                SqlParameter[] sqlparam =
                {
                     new SqlParameter("@MedicalLeaveData",dt_MedicalLeaveData),
                     new SqlParameter("@Type",objMedicalLeaveDetails.Type),
                     new SqlParameter("@EmpCode",objMedicalLeaveDetails.emld_emp_id)
                };
                result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertMedicalleavedetails"));
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string GetNomineeList(long AppID, string IsMarried)
        {
            string response = "";
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@AppId",AppID),
                    new SqlParameter("@MaritalStatus",IsMarried)
                };
                response = _Conn.ExecuteCmd(sqlparam, "sp_kgid_checkNomineeWihMaritalStatus");
            }
            catch (Exception ex)
            {
                response = "";
            }
            return response;
        }
        public string CheckEmployeeAge(long EmployeeID)
        {
            string empAgeStatus = "";
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",EmployeeID)
                };
                empAgeStatus = _Conn.ExecuteCmd(sqlparam, "sp_kgid_checkEmployeeAgeStatus");
            }
            catch (Exception ex)
            {
                empAgeStatus = "";
            }
            return empAgeStatus;
        }
        public string SavePolicyGeneration(VM_PolicyGeneration objPG)
        {
            string response = "";
            try
            {
                objPG.anp_dob = UploadDocument(objPG.DOBDoc, objPG.anp_emp_id, "PolicyGeneration");
                objPG.anp_appointment_letter = UploadDocument(objPG.AppointmentLetterDoc, objPG.anp_emp_id, "PolicyGeneration");
                objPG.anp_joining_letter = UploadDocument(objPG.JoiningLetterDoc, objPG.anp_emp_id, "PolicyGeneration");
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",objPG.anp_emp_id),
                    new SqlParameter("@DOB",objPG.anp_dob),
                    new SqlParameter("@AppLetr",objPG.anp_appointment_letter),
                    new SqlParameter("@JoinLetr",objPG.anp_joining_letter)
                };
                response = _Conn.ExecuteCmd(sqlparam, "sp_kgid_savePolicyGeneration");
                
            }
            catch (Exception ex)
            {
                response = "";
            }
            return response;
        }
        public string UpdateNBChallanStatusDll(string ChallanRefNo, long StatusCode, long EmpID)
        {
            string result = "0";
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@ChallanRefNo",ChallanRefNo),
                    new SqlParameter("@StatusCode",StatusCode),
                    new SqlParameter("@EmpID",EmpID)
                };
                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_updateNBChallanStatus");
            }
            catch (Exception ex)
            {
                result = "0";
            }
            return result;
        }

        public List<VM_EmpDashboardData> GetDetailsBasedOnKGIDNo(long KgidNo)
        {
            List<VM_EmpDashboardData> obj = new List<VM_EmpDashboardData>();
            try
            {
                DataSet dsBD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@KgidNo",KgidNo)
                };
                dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_GetPolicyDetByKgidNo");

                if (dsBD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsBD.Tables[0].Rows)
                    {
                        VM_EmpDashboardData objEmp = new VM_EmpDashboardData();
                        objEmp.PolicyID = Convert.ToInt64(dr["POLICY_NO"]);
                        objEmp.KGIDfirstHRMS = Convert.ToInt64(dr["FIRST_KGIDNO_HRMS"]);
                        objEmp.PremiumAmt = Convert.ToDecimal(dr["PREMIUM"].ToString());
                        objEmp.SumAssured = Convert.ToDecimal(dr["SUM_ASSURED"].ToString());
                        objEmp.RiskDate = Convert.ToDateTime(dr["DATE_OF_RISK"]);
                        objEmp.status = dr["STATUS"].ToString();
                        obj.Add(objEmp);
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public string GetEmailSMSTemplate(long templateid)
        {
            string result = "0";
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@templateid",templateid),
                };
                result = _Conn.ExecuteCmdForSMS(sqlparam, "sp_kgid_getMessageTemplate");

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "GetEmailSMSTemplate" + result);
                result = "0";
            }
            return result;

        }

        public string GetUserDetails(long KGIDNumber)
        {
            string result = "0";
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@KGIDNumber",KGIDNumber),
                };
                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_getUserDetails");

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "GetUserDetails - Admin" + ex.Message);
                result = "0";
            }
            return result;

        }
        public string UpdateUserDetails(long KGIDNumber, long MobileNo, string EmailID)
        {
            string result = "0";
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@KGIDNumber",KGIDNumber),
                    new SqlParameter("@MobileNo",MobileNo),
                    new SqlParameter("@EmailID",EmailID)
                };
                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_updateUserDetails");

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "UpdateUserDetails - Admin" + ex.Message);
                result = "0";
            }
            return result;
        }

        public List<VM_PolicyGeneration> GetPolicyList(long EmpID)
        {
            List<VM_PolicyGeneration> objPD = new List<VM_PolicyGeneration>();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",EmpID)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getProposalFormForDIO");

                if (dsPD.Tables[0].Rows.Count > 0)
                {
                    objPD = dsPD.Tables[0].AsEnumerable().Select(dataRow => new VM_PolicyGeneration
                    {
                        anp_id = dataRow.Field<long>("anp_id"),
                        anp_emp_id = dataRow.Field<long>("employee_id"),
                        EmployeeName = dataRow.Field<string>("employee_name"),
                        DateofBirth = dataRow.Field<string>("date_of_birth"),
                        DepartmentName = dataRow.Field<string>("dm_deptname_english"),
                        anp_dob = dataRow.Field<string>("anp_dob"),
                        anp_appointment_letter = dataRow.Field<string>("anp_appointment_letter"),
                        anp_joining_letter = dataRow.Field<string>("anp_joining_letter"),
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }

        public int SavePolicyStatus(long ID)
        {
            try
            {
                SqlParameter[] param =
                {
                     new SqlParameter("@ApplyID",ID)
                };
                int output = Convert.ToInt32(_Conn.ExecuteCmd(param, "sp_kgid_updateProposalStatus"));

                if(output > 8999999)
                {
                    
                        SqlParameter[] sqlparamNotifDetails =
                        {
                        new SqlParameter("@employeeId", ID),
                        new SqlParameter("@applicationId",0)

                    };
                        DataSet details = new DataSet();
                        details = _Conn.ExeccuteDataset(sqlparamNotifDetails, "sp_kgid_getNotificationDetails");
                        VM_NotificationDetails notificationDetails = new VM_NotificationDetails();

                        if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
                        {
                            notificationDetails.DDOEmailId = details.Tables[0].Rows[0]["DDOEmailId"].ToString();
                            notificationDetails.EmpEmailId = details.Tables[0].Rows[0]["EmpEmailId"].ToString();
                            notificationDetails.EmpMobileNumber = Convert.ToInt64(details.Tables[0].Rows[0]["EmpMobileNumber"].ToString());
                            notificationDetails.EmpName = details.Tables[0].Rows[0]["EmpName"].ToString();
                            notificationDetails.PolicyNumber = details.Tables[0].Rows[0]["PolicyNumber"].ToString();
                            notificationDetails.DIOEmailId = details.Tables[0].Rows[0]["DIOEmailId"].ToString();
                        }
                        SendInsurancePolicyNotification(notificationDetails);
                    
                }

                return output;
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
		public string GetDIOOfficeDetails(string DDOCode)
        {
            string DIOOffice = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@DDOCode",DDOCode),
                };
                DIOOffice = _Conn.ExecuteCmd(sqlparam, "sp_kgid_getDIOOfficeDetails");

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "GetDIOOfficeDetails - Admin" + ex.Message);
                DIOOffice = "0";
            }
            
            return DIOOffice;
        }
        public string GetDDODetails(long KGIDNumber)
        {
            string result = "0";
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@KGIDNumber",KGIDNumber),
                };
                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_getDDODetails");
                if (string.IsNullOrEmpty(result))
                {
                    result = "-1~-1";
                }

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "GetDDODetails - Admin" + ex.Message);
                result = "0";
            }
            return result;
        }
        public string CheckDDODetails(long KGIDNumber, string DDOCode, string DIOOffice)
        {
            string result = "0";
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@KGIDNumber",KGIDNumber),
                    new SqlParameter("@DDOCode",DDOCode),
                    new SqlParameter("@DIOOffice",DIOOffice)
                };
                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_checkDDOExists");

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "UpdateDDODetails - Admin" + ex.Message);
                result = "0";
            }
            return result;
        }
        public string UpdateDDODetails(long KGIDNumber, string DDOCode, string DIOOffice)
        {
            string result = "0";
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@KGIDNumber",KGIDNumber),
                    new SqlParameter("@DDOCode",DDOCode),
                    new SqlParameter("@DIOOffice",DIOOffice)
                };
                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_updateDDODetails");

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "UpdateDDODetails - Admin" + ex.Message);
                result = "0";
            }
            return result;
        }

        public List<VM_trackDetails> getProposerTrackDetailsDll(string applicationNo)
        {
            List<VM_trackDetails> obj = new List<VM_trackDetails>();
            //List<VM_trackDetails> objPD = new List<VM_trackDetails>();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@ApplicationNo",applicationNo)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getProposalTrackDetails");
              
                if (dsPD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPD.Tables[0].Rows)
                    {
                        
                        VM_trackDetails objDetails = new VM_trackDetails();
                        objDetails.application_no = Convert.ToInt64(dr["kad_kgid_application_number"]).ToString();
                        if (((dr["first_kgid_policy_no"]) == DBNull.Value))
                            objDetails.kgid_policy_number = string.Empty;
                        else
                        objDetails.kgid_policy_number = Convert.ToInt64(dr["first_kgid_policy_no"]).ToString();
                        if (((dr["dm_name_english"]) == DBNull.Value))
                            objDetails.districtNames = string.Empty;
                        else
                            objDetails.districtNames = (dr["dm_name_english"].ToString());
                        objDetails.assigned_date = (dr["kawt_creation_datetime"].ToString());
                        objDetails.application_status = (dr["asm_status_desc"]).ToString();                       
                        obj.Add(objDetails);
                    }

                    //var objDetails = dsPD.Tables[0].AsEnumerable().Select(dataRow => new VM_trackDetails
                    //{
                    //    application_no = dataRow.Field<string>("kad_kgid_application_number"),                                                
                    //    kgid_policy_number =(dataRow.Field<long?>("first_kgid_policy_no").ToString()),                        
                    //districtNames = dataRow.Field<string>("dm_name_english"),
                    //    assigned_date = dataRow.Field<string>("kawt_creation_datetime"),
                    //    application_status = dataRow.Field<string>("asm_status_desc"),
                    //}).ToList();
                    //obj.listTrackDetails = objDetails;
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
    }
}
