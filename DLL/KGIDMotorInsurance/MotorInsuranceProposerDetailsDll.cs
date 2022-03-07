using DLL.DBConnection;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL.KGIDMotorInsurance
{
    public class MotorInsuranceProposerDetailsDll : IMotorInsuranceProposerDetailsDll
    {

        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();
        
        public VM_MotorInsuranceProposerDetails MIProposerDetailsBll(long employeeCode,string PageType,long RefNo, int Category)
        {
           
            VM_MotorInsuranceProposerDetails objBD = new VM_MotorInsuranceProposerDetails();
            objBD.mipd_pagetype = PageType;
            try
            {
                DataSet dsBD = new DataSet();
                if (PageType == "Emp" || PageType == "EmpRenewal")
                {
                    SqlParameter[] sqlparam =
                 {
                    new SqlParameter("@employee_id",employeeCode),
                    new SqlParameter("@PageType",PageType=="EmpRenewal"?"Renewal":""),
                     new SqlParameter("@RefNo",RefNo)
                 };
                    dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectMIProposerDetailsEMP");
                }
                else
                {
                    SqlParameter[] sqlparam =
               {
                    new SqlParameter("@employee_id",employeeCode),
                    new SqlParameter("@PageType",(PageType=="Renewal"?"Renewal":PageType)),
                     new SqlParameter("@RefNo",RefNo),
                      new SqlParameter("@Category",Category)
                 };
                    dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectMIProposerDetails");
                }

                if (dsBD.Tables[0].Rows.Count > 0)
                {
                    objBD.mipd_employee_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["employee_id"]);
                    objBD.mipd_proposer_fullname = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                    objBD.mipd_Department= dsBD.Tables[0].Rows[0]["Department"].ToString();
                    objBD.mipd_address = dsBD.Tables[0].Rows[0]["ead_address"].ToString();
                    objBD.mipd_pincode = Convert.ToInt32(dsBD.Tables[0].Rows[0]["ead_pincode"]);
                    objBD.mipd_telephone_no = Convert.ToInt64(dsBD.Tables[0].Rows[0]["mobile_number"]);
                    objBD.mipd_occupation = dsBD.Tables[0].Rows[0]["d_designation_desc"].ToString();
                    objBD.mipd_email = dsBD.Tables[0].Rows[0]["email_id"].ToString();

                    objBD.PolicyMonths = dsBD.Tables[0].Rows[0]["PolicyMonths"].ToString();
                    var TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
                                           select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
                                                              ).ToList();
                    objBD.mipd_type_of_cover= dsBD.Tables[0].Rows[0]["type_of_cover"].ToString();
                    //Commented by srividya for testing purpose

                    //if ((PageType == "EditRenewal" || PageType == "ViewRenewal" || PageType == "Renewal"))
                    //{
                    //    if (Convert.ToInt32(objBD.PolicyMonths) >=12)
                    //        objBD.mipd_type_of_cover_list = TypeofCoverList.Where(a => a.Value == "3").Select(a => a).ToList();

                    //    if (Convert.ToInt32(objBD.PolicyMonths) > 36)
                    //        objBD.mipd_type_of_cover_list = TypeofCoverList;
                    //    //if (Convert.ToInt32(objBD.PolicyMonths) > 120)
                    //    //    objBD.mipd_type_of_cover_list = TypeofCoverList;

                    //    objBD.mipd_type_of_cover = dsBD.Tables[0].Rows[0]["r_type_of_cover"].ToString();
                    //    objBD.mipd_type_of_cover_id = (dsBD.Tables[0].Rows[0]["mira_type_of_cover"] == DBNull.Value) ? 0 : Convert.ToInt32(dsBD.Tables[0].Rows[0]["mira_type_of_cover"]);

                    //}
                    //else
                    //{
                    //    objBD.mipd_type_of_cover_list = TypeofCoverList.Where(a => a.Value != "4").Select(a => a).ToList();
                    //    objBD.mipd_type_of_cover = dsBD.Tables[0].Rows[0]["type_of_cover"].ToString();
                    //    objBD.mipd_type_of_cover_id = (dsBD.Tables[0].Rows[0]["mia_type_of_cover"] == DBNull.Value) ? 0 : Convert.ToInt32(dsBD.Tables[0].Rows[0]["mia_type_of_cover"]);

                    //}
                    //objBD.mipd_type_of_cover_list = TypeofCoverList;
                    objBD.mipd_fax_no=Convert.ToInt64((dsBD.Tables[0].Rows[0]["dm_fax_no"]==DBNull.Value)?0: dsBD.Tables[0].Rows[0]["dm_fax_no"]);
                     
                    if (PageType== "EditRenewal" || PageType == "ViewRenewal" ||PageType == "Renewal")
                    {
                        objBD.mipd_kgid_application_number = dsBD.Tables[0].Rows[0]["mira_renewal_application_ref_no"].ToString();
                        objBD.mipd_kgid_renewal_application_number = dsBD.Tables[0].Rows[0]["mira_renewal_application_ref_no"].ToString();
                    }
                    else
                    {
                        objBD.mipd_kgid_application_number = dsBD.Tables[0].Rows[0]["mia_application_ref_no"].ToString();
                    }

                    objBD.mipd_old_application_Ref_number = RefNo;
                    objBD.mipd_pagetype = (PageType=="EmpRenewal"?"Renewal":PageType);

                }

            }
            catch (Exception ex)
            {

            }
            return objBD;
        }
        
        public long SaveMIProposalAppnRefNo(VM_MotorInsuranceProposerDetails objPD)
        {
            long result = 0;
            string RefNo = "";
            try
            {
                if(!String.IsNullOrWhiteSpace(objPD.mipd_kgid_application_number) )
                {
                    RefNo =Convert.ToString(objPD.mipd_kgid_application_number);
                }
                else
                {
                    RefNo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                }
                
                SqlParameter[] sqlparam =
                {
                        new SqlParameter("@reference_no",RefNo),
                        new SqlParameter("@employee_id",objPD.mipd_employee_id),
                        new SqlParameter("@PageType",(objPD.mipd_pagetype==null?"":objPD.mipd_pagetype)),
                        new SqlParameter("@Category",objPD.mipd_category),
                        new SqlParameter("@email",objPD.mipd_email),
                        new SqlParameter("@address",objPD.mipd_address),
                        new SqlParameter("@faxno",objPD.mipd_fax_no),
                        new SqlParameter("@pincode",objPD.mipd_pincode),
                       //new SqlParameter("@TypeofCover",objPD.mipd_type_of_cover_id),
                };
                result = Convert.ToInt64(_Conn.ExecuteCmd(sqlparam, "sp_kgid_saveMIReferenceNo"));

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public long SaveMIRenewalProposalAppnRefNo(VM_MotorInsuranceProposerDetails objPD)
        {
            long result = 0;
            string RefNo = "";
            try
            {
                if (objPD.mipd_kgid_application_number != "")
                {
                    RefNo = Convert.ToString(objPD.mipd_kgid_application_number);
                }
                else
                {
                    RefNo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                }

                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@reference_no",RefNo),
                    new SqlParameter("@employee_id",objPD.mipd_employee_id),
                     new SqlParameter("@PageType",objPD.mipd_pagetype),
                     new SqlParameter("@Category",objPD.mipd_category),
                     new SqlParameter("@OldRefNo",objPD.mipd_old_application_Ref_number),
                      new SqlParameter("@TypeofCover",objPD.mipd_type_of_cover_id),
                };
                result = Convert.ToInt64(_Conn.ExecuteCmd(sqlparam, "sp_kgid_saveMIRenewalReferenceNo"));

            }
            catch (Exception ex)
            {

            }
            return result;
        }


        public string SaveMILoanDetails(VM_MIApplicationDetails obj)
        {
            string result = "";
           
            try
            {
                SqlParameter[] sqlparam =
                {
                        new SqlParameter("@vehicleLoanId",obj.vehicleLoanId),
                        new SqlParameter("@LSD",obj.MI_Loan_start_date),
                        new SqlParameter("@LED",obj.MI_Loan_end_date),
                        new SqlParameter("@LoanAmt",obj.MI_Loan_Amount),
                        new SqlParameter("@EmployeeId",obj.EmployeeId),
                };

                result = _Conn.ExecuteCmd(sqlparam, "sp_mi_SaveEmployeeLoanDetails");

            }
            catch (Exception ex)
            {

            }
            return result;
        }
        
        //DSC Validation
        public string DSCLoginDetails(long kgidno, string publickey)
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
    }
}
