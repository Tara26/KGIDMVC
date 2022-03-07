using KGID_Models.KGID_VerifyData;
using System.Linq;
using DLL.DBConnection;
using System.Collections.Generic;
using KGID_Models.KGID_User;
using System;
using KGID_Models.KGIDNBApplication;
using System.Web.Mvc;
using KGID_Models.KGID_Verification;
using System.Data;
using System.Data.SqlClient;
using KGID_Models.NBApplication;
using Common;

namespace DLL.VerifyDataDLL
{
    public class Verificationdetailsdll : IVerificationdetailsdll
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();
        public IEnumerable<tbl_verification_details> UserDetailsdll(tbl_verification_details _users)
        {
            var VerificationDetails = (from n in _db.tbl_verification_details
                                       where (n.STATUS != "F") && (n.STATUS != "V") && (n.FIRST_KGIDNO_HRMS != null)
                                       select n).ToList();
            return VerificationDetails;
        }

        public tbl_verification_details EmployeeDetailsdll(int KGIDNO)
        {
            var EmpDetails = (from n in _db.tbl_verification_details
                              where n.FIRST_KGIDNO_HRMS == KGIDNO.ToString() && (n.STATUS != "V" || n.STATUS != "F")
                              select n).FirstOrDefault();
            return EmpDetails;
        }
        public tbl_verification_details UpdateStatusdll(string Status, VM_VerifiedBy _VBY, int UID)
        {
            var EmpDetails = (from n in _db.tbl_verification_details
                              where n.POLICY_NO == _VBY.Verification_details.POLICY_NO
                              select n).FirstOrDefault();
            if (EmpDetails != null)
            {
                EmpDetails.EMPLOYEE_NAME_KGID = _VBY.Verification_details.EMPLOYEE_NAME_KGID;
                EmpDetails.FATHER_NAME_KGID = _VBY.Verification_details.FATHER_NAME_KGID;
                EmpDetails.DATE_OF_BIRTH_KGID = _VBY.Verification_details.DATE_OF_BIRTH_KGID;
                EmpDetails.EMPID_HRMS = _VBY.Verification_details.EMPID_HRMS;
                EmpDetails.FIRST_KGIDNO_HRMS = _VBY.Verification_details.FIRST_KGIDNO_HRMS;
                EmpDetails.EMPLOYEE_AGE = _VBY.Verification_details.EMPLOYEE_AGE;
                EmpDetails.NOMINEE_NAME = _VBY.Verification_details.NOMINEE_NAME;
                EmpDetails.NOMINEE_RELATION = _VBY.Verification_details.NOMINEE_RELATION;
                EmpDetails.NOMINEE_AGE = _VBY.Verification_details.NOMINEE_AGE;
                EmpDetails.DATE_OF_RISK = _VBY.Verification_details.DATE_OF_RISK;
                EmpDetails.PREMIUM = _VBY.Verification_details.PREMIUM;
                EmpDetails.SUM_ASSURED = _VBY.Verification_details.SUM_ASSURED;
                EmpDetails.STATUS = Status;
                EmpDetails.REMARKS = _VBY.Verification_details.REMARKS;
                EmpDetails.VERIFIED_DATE = DateTime.Now;
                EmpDetails.VERIFIED_BY = UID;
                _db.SaveChanges();
            }
            return EmpDetails;
        }

        public tbl_verified_details AddVerifiedDetailsbll(tbl_verified_details _Verified_Details)
        {
            if (_Verified_Details != null)
            {
                _db.tbl_verified_details.Add(_Verified_Details);
                _db.SaveChanges();
            }
            return _Verified_Details;
        }

        public IEnumerable<tbl_verification_details> getVerificationDatadll(int _VerificationStatus, string policyNo, int districtId)
        {
            var VerificationDetails = new List<tbl_verification_details>();
            if (_VerificationStatus == 1)
            {
                VerificationDetails = (from n in _db.tbl_verification_details
                                       join _KGIDD in _db.tbl_kgid_policy_district_mapping on n.POLICY_NO equals _KGIDD.policy_no
                                       where (n.STATUS == "V" && n.POLICY_NO == policyNo && _KGIDD.district_id == districtId)
                                       select n).ToList();
            }
            else if (_VerificationStatus == 2)
            {
                VerificationDetails = (from n in _db.tbl_verification_details
                                       join _KGIDD in _db.tbl_kgid_policy_district_mapping on n.POLICY_NO equals _KGIDD.policy_no
                                       where (n.STATUS == "F" && n.POLICY_NO == policyNo && _KGIDD.district_id == districtId)
                                       select n).ToList();
            }
            else
            {
                VerificationDetails = (from n in _db.tbl_verification_details
                                       join _KGIDD in _db.tbl_kgid_policy_district_mapping on n.POLICY_NO equals _KGIDD.policy_no
                                       where (n.STATUS == "E" && n.POLICY_NO == policyNo && _KGIDD.district_id == districtId)
                                       select n).ToList();
            }

            return VerificationDetails;
        }
        //public IEnumerable<tbl_verification_details> getVerificationDatadll(int _VerificationStatus, int _startNo, int _endNo)
        //{
        //    var VerificationDetails = new List<tbl_verification_details>();
        //    if (_VerificationStatus == 1)
        //    {
        //        VerificationDetails = (from n in _db.tbl_verification_details
        //                               where (n.STATUS == "V") && (n.FIRST_KGIDNO_HRMS >= _startNo && n.FIRST_KGIDNO_HRMS <= _endNo)
        //                               select n).ToList();
        //    }
        //    else if (_VerificationStatus == 2)
        //    {
        //        VerificationDetails = (from n in _db.tbl_verification_details
        //                               where (n.STATUS == "R") && (n.FIRST_KGIDNO_HRMS >= _startNo && n.FIRST_KGIDNO_HRMS <= _endNo)
        //                               select n).ToList();
        //    }
        //    else
        //    {
        //        VerificationDetails = (from n in _db.tbl_verification_details
        //                               where (n.STATUS == "E") && (n.FIRST_KGIDNO_HRMS >= _startNo && n.FIRST_KGIDNO_HRMS <= _endNo)
        //                               select n).ToList();
        //    }

        //    return VerificationDetails;
        //}

        public VM_VerifiedBy GetVerificationDetailById(int _vdId)
        {
            var EmpDetails = (from n in _db.tbl_verification_details
                              where n.VD_ID == _vdId
                              select n).FirstOrDefault();
            VM_VerifiedBy objV = new VM_VerifiedBy();
            if (EmpDetails.STATUS == "V")
            {
                var LogDetails = (from n in _db.tbl_login_master
                                  where n.um_user_id == EmpDetails.VERIFIED_BY
                                  select new vm_kgid_user
                                  {
                                      um_user_id = n.um_user_id,
                                      um_user_name = n.um_user_name,
                                      um_kgid_number = n.um_kgid_number
                                  }).FirstOrDefault();
                objV.Verification_details = EmpDetails;
                if (LogDetails != null)
                {
                    //objV.Verification_details = EmpDetails;
                    objV.UserName = LogDetails.um_user_name;
                    objV.KGIDNumber = LogDetails.um_kgid_number;
                }
                else
                {
                    objV.UserName = "";
                    objV.KGIDNumber = "";
                }
            }
            else
            {
                objV.Verification_details = EmpDetails;
            }
            return objV;
        }
        public int SaveEmployeeFormDll(tbl_upload_employeeform objEmpForm)//To Be Changed
        {
            string saveddetails = string.Empty;
            if(saveddetails.Contains("objEmpForm"))
            {
                saveddetails.Trim('|');
            }
            int Result = 0;
            var EmpRefNo = (from n in _db.tbl_kgid_application_details
                            where n.kad_emp_id == objEmpForm.App_Employee_Code
                            && n.kad_active_status == true
                            select n).FirstOrDefault();
            var EmpUpload = (from n in _db.tbl_upload_employeeform
                             where n.App_Employee_Code == objEmpForm.App_Employee_Code 
                             && n.App_ApplicationID == EmpRefNo.kad_application_id
                             && n.App_Active == true
                             select n).FirstOrDefault();
            var ddoid = (from n in _db.tbl_employee_work_details
                           join _KGIDD in _db.tbl_employee_basic_details on n.ewd_emp_id equals _KGIDD.employee_id
                           where (n.ewd_emp_id == EmpRefNo.kad_emp_id)
                           select n).FirstOrDefault();

            var ddocode = (from n in _db.tbl_employee_work_details
                join _KGIDD in _db.tbl_employee_basic_details on n.ewd_emp_id equals _KGIDD.employee_id
                where (n.ewd_ddo_id ==ddoid.ewd_ddo_id  && _KGIDD.user_category_id.Contains ("2")) select n).FirstOrDefault();

            if (ddocode != null)
            {
                if (EmpRefNo != null)
                {
                    var EmpDeptVer = (from n in _db.tbl_kgid_application_workflow_details
                                          //where n.kawt_verified_by == objEmpForm.App_Employee_Code
                                      where n.kawt_application_id == EmpRefNo.kad_application_id && n.kawt_active_status == true
                                      select n).FirstOrDefault();

                if (EmpUpload != null && EmpDeptVer != null)
                {
                    if (EmpDeptVer != null)
                    {
                        EmpDeptVer.kawt_active_status = false;
                        EmpDeptVer.kawt_updated_by = objEmpForm.App_Employee_Code;
                        EmpDeptVer.kawt_updation_datetime = DateTime.Now;
                        _db.SaveChanges();
                    }
                    EmpUpload.App_Employee_Code = objEmpForm.App_Employee_Code;
                    if(EmpUpload.App_Application_Form  != string.Empty && objEmpForm.App_Application_Form == string.Empty)
                    {
                        objEmpForm.App_Application_Form = EmpUpload.App_Application_Form;
                    }
                    else if(EmpUpload.App_Medical_Form != string.Empty && objEmpForm.App_Medical_Form == string.Empty)
                    {
                        objEmpForm.App_Medical_Form = EmpUpload.App_Medical_Form;
                    }
                    EmpUpload.App_Application_Form = objEmpForm.App_Application_Form;
                    EmpUpload.App_Medical_Form = objEmpForm.App_Medical_Form;
                    EmpUpload.App_Creation_Date = DateTime.Now;

                        tbl_kgid_application_workflow_details objDeptVerification = new tbl_kgid_application_workflow_details();
                        objDeptVerification.kawt_verified_by = objEmpForm.App_Employee_Code;
                        objDeptVerification.kawt_application_id = EmpDeptVer.kawt_application_id;
                        objDeptVerification.kawt_application_status = 3;
                        objDeptVerification.kawt_active_status = true;
                        objDeptVerification.kawt_created_by = objEmpForm.App_Employee_Code;
                        objDeptVerification.kawt_creation_datetime = DateTime.Now;
                        objDeptVerification.kawt_updated_by = objEmpForm.App_Employee_Code;
                        objDeptVerification.kawt_updation_datetime = DateTime.Now;
                        _db.tbl_kgid_application_workflow_details.Add(objDeptVerification);

                    _db.SaveChanges();


                    Result = 2;
                }
                else
                {
                    var EmpInsuredUpload = (from n in _db.tbl_upload_employeeform
                                            where n.App_Employee_Code == objEmpForm.App_Employee_Code
                                            && n.App_ApplicationID != EmpRefNo.kad_application_id
                                            && n.App_Active == true
                                            select n).FirstOrDefault();
                    if (EmpInsuredUpload != null)
                    {
                        EmpInsuredUpload.App_Active = false;
                        _db.SaveChanges();
                    }

                        objEmpForm.App_Active = true;
                        objEmpForm.App_Created_By = Convert.ToInt64(objEmpForm.App_Employee_Code);
                        objEmpForm.App_Creation_Date = DateTime.Now;
                        objEmpForm.App_ApplicationID = EmpRefNo.kad_application_id;
                        _db.tbl_upload_employeeform.Add(objEmpForm);

                        tbl_kgid_application_workflow_details objDeptVerification = new tbl_kgid_application_workflow_details();
                        objDeptVerification.kawt_verified_by = objEmpForm.App_Employee_Code;
                        objDeptVerification.kawt_application_id = EmpRefNo.kad_application_id;
                        objDeptVerification.kawt_application_status = 3;
                        objDeptVerification.kawt_active_status = true;
                        objDeptVerification.kawt_created_by = objEmpForm.App_Employee_Code;
                        objDeptVerification.kawt_creation_datetime = DateTime.Now;
                        objDeptVerification.kawt_updated_by = objEmpForm.App_Employee_Code;
                        objDeptVerification.kawt_updation_datetime = DateTime.Now;
                        _db.tbl_kgid_application_workflow_details.Add(objDeptVerification);
                        _db.SaveChanges();
                        Result = 1;
                    }
                }
            }
            else Result = 0;
            return Result;
        }
        public VM_Upload_EmployeeForm GetUploadDocDll(long _EmpId) // To Be Changed
        {
            var EmpUpload = (from n in _db.tbl_upload_employeeform
                             where n.App_Employee_Code == _EmpId
                             select n).FirstOrDefault();
            VM_Upload_EmployeeForm objUD = new VM_Upload_EmployeeForm();
            if (EmpUpload != null)
            {
                objUD.App_Employee_Code = EmpUpload.App_Employee_Code;
                objUD.ApplicationFormDocName = EmpUpload.App_Application_Form;
                objUD.MedicalFormDocName = EmpUpload.App_Medical_Form;
            }
            return objUD;
        }
        
        public VM_NBBond getNBBondDetailsdll(long EmployeeCode)
        {
            var NBBondDetails = new VM_NBBond();
            
            try
            {
                DataSet dsNBBOND = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",EmployeeCode)
                };

                dsNBBOND = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectNBBond");
                if (dsNBBOND.Tables[0].Rows.Count > 0)
                {
                    VM_BasicDetails objBD = new VM_BasicDetails();
                    
                    objBD.employee_id = Convert.ToInt32(dsNBBOND.Tables[0].Rows[0]["employee_id"]);
                    objBD.employee_name = dsNBBOND.Tables[0].Rows[0]["employee_name"].ToString();
                    objBD.employee_name_kannada = dsNBBOND.Tables[0].Rows[0]["employee_name_kannada"].ToString();
                    objBD.spouse_name = dsNBBOND.Tables[0].Rows[0]["spouse_name"].ToString();
                    objBD.father_name = dsNBBOND.Tables[0].Rows[0]["father_name"].ToString();
                    objBD.father_name_kannada = dsNBBOND.Tables[0].Rows[0]["father_name_kannada"].ToString();
                    objBD.date_of_birth = dsNBBOND.Tables[0].Rows[0]["date_of_birth"].ToString();
                    //
                    //objBD.emp_date_of_birth = Convert.ToDateTime(dsNBBOND.Tables[0].Rows[0]["date_of_birth"]);
                    //Logger.LogMessage(TracingLevel.INFO, "dll objBD.emp_date_of_birth:-" + objBD.emp_date_of_birth);
                    objBD.p_premium = Convert.ToDouble(dsNBBOND.Tables[0].Rows[0]["p_premium"]);
                    objBD.designation = dsNBBOND.Tables[0].Rows[0]["d_designation_desc"].ToString();
                    objBD.dm_ddo_office = dsNBBOND.Tables[0].Rows[0]["dm_ddo_office"].ToString();

                    objBD.ewd_place_of_posting = dsNBBOND.Tables[0].Rows[0]["ewd_place_of_posting"].ToString();
                    objBD.place_of_birth = dsNBBOND.Tables[0].Rows[0]["place_of_birth"].ToString();

                    objBD.gender_desc = dsNBBOND.Tables[0].Rows[0]["gender_desc"].ToString();
                    objBD.ead_address = dsNBBOND.Tables[0].Rows[0]["ead_address"].ToString();

                    objBD.ead_pincode = Convert.ToInt32(dsNBBOND.Tables[0].Rows[0]["ead_pincode"]);
                    objBD.mobile_number = Convert.ToInt64(dsNBBOND.Tables[0].Rows[0]["mobile_number"]);

                    objBD.ewd_date_of_joining = dsNBBOND.Tables[0].Rows[0]["ewd_date_of_joining"].ToString();
                    objBD.et_employee_type_desc = dsNBBOND.Tables[0].Rows[0]["et_employee_type_desc"].ToString();

                    objBD.d_designation_desc = dsNBBOND.Tables[0].Rows[0]["d_designation_desc"].ToString();
                    objBD.payscle_code = dsNBBOND.Tables[0].Rows[0]["payscale_code"].ToString();
                    objBD.kad_kgid_application_number = dsNBBOND.Tables[0].Rows[0]["kad_kgid_application_number"].ToString();
                    objBD.p_kgid_policy_number = dsNBBOND.Tables[0].Rows[0]["p_kgid_policy_number"].ToString();
                    objBD.p_total_sum_assured = dsNBBOND.Tables[0].Rows[0]["p_sum_assured"].ToString();
                    objBD.p_sanction_date = dsNBBOND.Tables[0].Rows[0]["p_sanction_date"].ToString();
                    ///
                    //objBD.policy_sanction_date = Convert.ToDateTime(dsNBBOND.Tables[0].Rows[0]["p_sanction_date"]);
                    objBD.App_Creation_Date = dsNBBOND.Tables[0].Rows[0]["App_Creation_Date"].ToString();
                    /////////
                    objBD.CaseWorkerName = dsNBBOND.Tables[0].Rows[0]["CaseWorkerName"].ToString();
                    objBD.CaseWorkerVerifiedDate = dsNBBOND.Tables[0].Rows[0]["CaseWorkerVerifiedDate"].ToString();
                    objBD.SuperintendentName = dsNBBOND.Tables[0].Rows[0]["SuperintendentName"].ToString();
                    objBD.SuperintendentVerifiedDate = dsNBBOND.Tables[0].Rows[0]["SuperintendentVerifiedDate"].ToString();
                    objBD.ApplicationSubmitedDate = dsNBBOND.Tables[0].Rows[0]["ApplicationSubmitedDate"].ToString();
                    objBD.DIO_Office_Address = dsNBBOND.Tables[0].Rows[0]["DIO_Office_Address"].ToString();
                    objBD.LoadFactor = dsNBBOND.Tables[0].Rows[0]["LoadFactor"].ToString();
                    objBD.DLFactor = dsNBBOND.Tables[0].Rows[0]["DLFactor"].ToString();
                    //
                    //objBD.employee_name_kannada = dsNBBOND.Tables[0].Rows[0]["employee_name_kannada"].ToString();
                    //objBD.father_name_kannada = dsNBBOND.Tables[0].Rows[0]["father_name_kannada"].ToString();
                    //Date Data String Conversion
                    objBD.FinalPayment = dsNBBOND.Tables[0].Rows[0]["FinalPayment"].ToString();
                    objBD.DueDate = dsNBBOND.Tables[0].Rows[0]["DueDate"].ToString();
                    objBD.EndMonthYear = dsNBBOND.Tables[0].Rows[0]["EndMonthYear"].ToString();
                    objBD.ApprovedMonth = dsNBBOND.Tables[0].Rows[0]["ApprovedMonth"].ToString();
                    objBD.ApprovedYear = dsNBBOND.Tables[0].Rows[0]["ApprovedYear"].ToString();
                    NBBondDetails.EmployeeBasicDetails = objBD;
                }
                if (dsNBBOND.Tables[1].Rows.Count > 0)
                {
                    var NomineeDetails = dsNBBOND.Tables[1].AsEnumerable().Select(dataRow => new VM_NomineeDetail
                    {
                        NameOfNominee = dataRow.Field<string>("efd_name_of_member"),
                        Relation = dataRow.Field<string>("frm_relation_desc"),
                        NomineeAge = dataRow.Field<string>("efd_age"),
                        PercentageShare= dataRow.Field<int>("end_percentage_of_share"),
                        NameOfGaurdian= dataRow.Field<string>("end_name_of_guardian"),
                        GaurdianRelation= dataRow.Field<string>("gaurdian_relation")
                    }).ToList();
                    NBBondDetails.NomineeDetailsList = NomineeDetails;
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "sp_kgid_NBBond Error:-" + ex.Message.ToString() );
            }
            Logger.LogMessage(TracingLevel.INFO, "sp_kgid_NBBond Success:-" + NBBondDetails.EmployeeBasicDetails.p_sanction_date);
            return NBBondDetails;
        }
        //New
        public VM_DDOVerificationDetails GetEmployeeApplicationStatusDll(long empId)
        {
            VM_DDOVerificationDetails verificationDetails = new VM_DDOVerificationDetails();
            try
            {
                DataSet dsEFD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",empId)
                };

                dsEFD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getEmployeeFormDetails");
                if (dsEFD.Tables[0].Rows.Count > 0)
                {
                    var EmployeeVerification = dsEFD.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetail
                    {
                        EmployeeCode = dataRow.Field<long?>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                        PolicyNumber = dataRow.Field<long?>("first_kgid_policy_no").ToString(),
                        ApplicationId = dataRow.Field<long>("kad_application_id"),
                        Status = dataRow.Field<string>("asm_status_desc"),
                        Remarks = dataRow.Field<string>("kawt_comments"),
                        NBBondDocPath = dataRow.Field<string>("p_bond_upload_path"),
                        NBFSDocPath = dataRow.Field<string>("p_fs_upload_path"),
                        NBSignBondDocPath = dataRow.Field<string>("p_sign_bond_upload_path")
                    }).ToList();

                    verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                }
            }
            catch(Exception ex)
            {

            }
            return verificationDetails;
        }
        public int SaveEmployeeWorkFlowBll(long EmpID)
        {
            int Result = 0;
            var EmpRefNo = (from n in _db.tbl_kgid_application_details
                            where n.kad_emp_id == EmpID
                            && n.kad_active_status == true
                            select n).FirstOrDefault();
            var EmpUpload = (from n in _db.tbl_upload_employeeform
                             where n.App_Employee_Code == EmpID
                             && n.App_ApplicationID == EmpRefNo.kad_application_id
                             && n.App_Active == true
                             select n).FirstOrDefault();
            if (EmpRefNo != null)
            {
                var EmpDeptVer = (from n in _db.tbl_kgid_application_workflow_details
                                      //where n.kawt_verified_by == objEmpForm.App_Employee_Code
                                  where n.kawt_application_id == EmpRefNo.kad_application_id && n.kawt_active_status == true
                                  select n).FirstOrDefault();

                if (EmpUpload != null && EmpDeptVer != null)
                {
                    if (EmpDeptVer != null)
                    {
                        EmpDeptVer.kawt_active_status = false;
                        EmpDeptVer.kawt_updated_by = EmpID;
                        EmpDeptVer.kawt_updation_datetime = DateTime.Now;
                        _db.SaveChanges();
                    }
                    EmpUpload.App_Employee_Code = EmpID;
                    EmpUpload.App_Application_Form = string.Empty;
                    EmpUpload.App_Medical_Form = string.Empty;
                    EmpUpload.App_Creation_Date = DateTime.Now;

                    tbl_kgid_application_workflow_details objDeptVerification = new tbl_kgid_application_workflow_details();
                    objDeptVerification.kawt_verified_by = EmpID;
                    objDeptVerification.kawt_application_id = EmpDeptVer.kawt_application_id;
                    objDeptVerification.kawt_application_status = 3;
                    objDeptVerification.kawt_active_status = true;
                    objDeptVerification.kawt_created_by = EmpID;
                    objDeptVerification.kawt_creation_datetime = DateTime.Now;
                    _db.tbl_kgid_application_workflow_details.Add(objDeptVerification);

                    _db.SaveChanges();


                    Result = 2;
                }
                else
                {
                    var EmpInsuredUpload = (from n in _db.tbl_upload_employeeform
                                            where n.App_Employee_Code == EmpID
                                            && n.App_ApplicationID != EmpRefNo.kad_application_id
                                            && n.App_Active == true
                                            select n).FirstOrDefault();
                    if (EmpInsuredUpload != null)
                    {
                        EmpInsuredUpload.App_Active = false;
                        _db.SaveChanges();
                    }
                    tbl_upload_employeeform objEmpForm = new tbl_upload_employeeform();
                    objEmpForm.App_Active = true;
                    objEmpForm.App_Created_By = Convert.ToInt64(EmpID);
                    objEmpForm.App_Creation_Date = DateTime.Now;
                    objEmpForm.App_ApplicationID = EmpRefNo.kad_application_id;
                    _db.tbl_upload_employeeform.Add(objEmpForm);

                    tbl_kgid_application_workflow_details objDeptVerification = new tbl_kgid_application_workflow_details();
                    objDeptVerification.kawt_verified_by = EmpID;
                    objDeptVerification.kawt_application_id = EmpRefNo.kad_application_id;
                    objDeptVerification.kawt_application_status = 3;
                    objDeptVerification.kawt_active_status = true;
                    objDeptVerification.kawt_created_by = EmpID;
                    objDeptVerification.kawt_creation_datetime = DateTime.Now;
                    _db.tbl_kgid_application_workflow_details.Add(objDeptVerification);
                    _db.SaveChanges();
                    Result = 1;
                }
            }
            return Result;
        }
        public VM_DDOVerificationDetails GetEmployeeNBBondFacingSheetDll(long empId)
        {
            VM_DDOVerificationDetails verificationDetails = new VM_DDOVerificationDetails();
            try
            {
                DataSet dsEFD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",empId)
                };

                dsEFD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getGetNBBondFacingSheet");
                if (dsEFD.Tables[0].Rows.Count > 0)
                {
                    var EmployeeVerification = dsEFD.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetail
                    {
                        EmployeeCode = dataRow.Field<long?>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                        PolicyNumber = dataRow.Field<long>("first_kgid_policy_no").ToString(),
                        ApplicationId = dataRow.Field<long>("kad_application_id"),
                        Status = dataRow.Field<string>("asm_status_desc"),
                        Remarks = dataRow.Field<string>("kawt_comments"),
                        SanctionedDate = dataRow.Field<string>("p_sanction_date"),
                        NBBondDocPath = dataRow.Field<string>("p_bond_upload_path"),
                        NBFSDocPath = dataRow.Field<string>("p_fs_upload_path"),
                        NBSignBondDocPath= dataRow.Field<string>("p_sign_bond_upload_path")
                    }).ToList();

                    verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        public VM_DDOVerificationDetails GetEmployeeIntimationLetterDll(long empId)
        {
            VM_DDOVerificationDetails verificationDetails = new VM_DDOVerificationDetails();
            try
            {
                DataSet dsEFD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",empId)
                };

                dsEFD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getIntimationLetterToDDO");
                if (dsEFD.Tables[0].Rows.Count > 0)
                {
                    var EmployeeVerification = dsEFD.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetail
                    {
                        EmployeeCode = dataRow.Field<long?>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = dataRow.Field<string>("kad_kgid_application_number"),
                        PolicyNumber = dataRow.Field<long>("first_kgid_policy_no").ToString(),
                        ApplicationId = dataRow.Field<long>("kad_application_id"),
                        Status = dataRow.Field<string>("asm_status_desc"),
                        Remarks = dataRow.Field<string>("kawt_comments"),
                        SanctionedDate = dataRow.Field<string>("p_sanction_date"),
                        NBBondDocPath = dataRow.Field<string>("p_bond_upload_path"),
                        NBFSDocPath = dataRow.Field<string>("p_fs_upload_path"),
                        NBSignBondDocPath = dataRow.Field<string>("p_sign_bond_upload_path")
                    }).ToList();

                    verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_PolicyCancellationDetails GetPolicyCancellationDetails(long EmpId, string Type)
        {
            VM_PolicyCancellationDetails obj = new VM_PolicyCancellationDetails();
            try
            {
                DataSet dsEFD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",EmpId),
                    new SqlParameter("@Type",Type)
                };

                dsEFD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getPolicyCancellationDetails");
                if (dsEFD.Tables[0].Rows.Count > 0)
                {
                    var EmployeeVerification = dsEFD.Tables[0].AsEnumerable().Select(dataRow => new VM_PolicyCancellationData
                    {
                        EmployeeID = dataRow.Field<long>("employee_id"),
                        EmployeeName = dataRow.Field<string>("employee_name"),
                        ApplicationID = dataRow.Field<long>("kad_application_id"),
                        ApplicationReferenceNo = dataRow.Field<string>("kad_kgid_application_number"),
                        EmployeeType = dataRow.Field<string>("EmpStatus"),
                        Status = dataRow.Field<string>("AppStatus"),
                        CreatedDate = dataRow.Field<string>("CreationDateTime"),
                        Type = Type
                    }).ToList();
                    obj.listPolicyCancelDetails = EmployeeVerification;
                }
            }
            catch(Exception ex)
            {

            }
            return obj;
        }
        public int NBAppCancelRequestAction(long AppId, long EmpId, string action)
        {
            int response = 0;
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@AppId",AppId),
                    new SqlParameter("@EmpId",EmpId),
                    new SqlParameter("@Action",action)
                };
                response = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_cancelrequestaction"));
            }
            catch (Exception ex)
            {
                response = 0;
            }
            return response;
        }
    }
}
