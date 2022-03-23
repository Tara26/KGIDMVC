using DLL.DBConnection;
using KGID_Models.KGID_MB_Claim;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static KGID_Models.KGID_MB_Claim.VM_MIOwnDamageClaimDetails;
using static KGID_Models.KGID_MB_Claim.VM_ODClaimApprovedApplicationDetails;

using static KGID_Models.KGIDMotorInsurance.tbl_vehicle_category_master;
using static KGID_Models.KGIDMotorInsurance.tbl_vehicle_type_master;

namespace DLL.MBClaimsDLL
{
    public class MBClaimsDLL : IMBClaimsDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();

        DataTable dtODClaimImagesDocData = new DataTable();
        DataTable dtODClaimDocData = new DataTable();
        DataTable dtODComponentsData = new DataTable();
        DataTable dtODComponentsDataSurveyor = new DataTable();
        DataTable dtODComponentsDataDepartment = new DataTable();
        public VM_MIOwnDamageClaimDetails GetMIOwnDamageClaimDetailsDLL(long EmployeeCode, int Category)
        {
            VM_MIOwnDamageClaimDetails MIODclaimDetails = new VM_MIOwnDamageClaimDetails();
            try
            {
                DataSet dsRD = new DataSet();
                SqlParameter[] sqlparam =
                 {
                    new SqlParameter("@employeeId",EmployeeCode),
                    new SqlParameter("@category",Category)
                 };
                dsRD = _Conn.ExeccuteDataset(sqlparam, "sp_mbclaims_getMBClaimDetails");
                dsRD.Tables["Table"].Merge(dsRD.Tables["Table1"]);
                if (dsRD.Tables[0].Rows.Count > 0)
                {
                    var MIOwnDamelList = dsRD.Tables[0].AsEnumerable().Select(dataRow => new MotorInsuranceODDetailsMI
                    {
                        MIEmployeeId = dataRow.Field<long>("p_mi_emp_id"),
                        MIPolicyNumber = dataRow.Field<string>("p_mi_policy_number"),
                        MIPolicyId = dataRow.Field<long>("p_mi_policy_id"),
                        MIPremium = dataRow.Field<double>("p_mi_premium"),
                        MIApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                        MIApplicationNumber = dataRow.Field<long>("mia_application_ref_no"),
                        MIPolicyActiveStatus = dataRow.Field<bool>("p_mi_active_status"),
                        MIApplicationActiveStatus = dataRow.Field<bool>("mia_active"),
                        MIUserCategoryId = Convert.ToInt32(dataRow.Field<string>("mia_user_category")),
                        MIPolicyFromDate = dataRow.Field<DateTime?>("p_mi_from_date"),
                        MIPolicyToDate = dataRow.Field<DateTime?>("p_mi_to_date"),

                        MIVehicleMakeName = dataRow.Field<string>("vm_vehicle_make_desc"),
                        //MIVehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        MIVehicleModelName = dataRow.Field<string>("vm_vehicle_model_desc"),
                        MIVehicleManufactureDate = dataRow.Field<DateTime?>("mivd_date_of_manufacturer"),
                        MIVehicleRegistrationNumber = dataRow.Field<string>("mivd_registration_no"),
                        MIRegistrationName = dataRow.Field<string>("mivd_registration_authority_and_location"),
                        MICubicCapacity = Convert.ToString(dataRow.Field<int>("mivd_cubic_capacity")),
                        MINoOfPassengers = Convert.ToString(dataRow.Field<int>("mivd_seating_capacity_including_driver")),
                        //MITypeOfCover = Convert.ToString(dataRow.Field<string>("mitoc_type_cover_name")),
                        //MIPolicyType = Convert.ToString(dataRow.Field<string>("MIPolicyType"))
                        //MIRenewalApplicationId = ((dataRow.Field<long?>("mira_motor_insurance_app_id")).ToString() == "") ? (long?)0 : dataRow.Field<long?>("mira_motor_insurance_app_id"),
                        //MIRenewalApplicationNumber = ((dataRow.Field<long?>("mira_application_ref_no")).ToString() == "") ? (long?)0 : dataRow.Field<long?>("mira_application_ref_no"),

                    }).ToList();
                    MIODclaimDetails.MIOwnDamageClaimDetails = MIOwnDamelList;
                }

            }
            catch (Exception ex)
            {

            }
            return MIODclaimDetails;
        }

        //Save OD Claim Application Details
        public long SaveODClaimApplicationDetailsDLL(VM_ODClaimApplicationDetails objCAD)
        {
            long result = 0;
            string RefNo = "";
            try
            {
                if (objCAD.Odca_claim_app_no != "" && objCAD.Odca_claim_app_no != null && objCAD.Odca_claim_app_no != "0")
                {
                    RefNo = Convert.ToString(objCAD.Odca_claim_app_no);
                }
                else
                {
                    RefNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                }
                if (RefNo != "0" && RefNo != "")
                {
                    dtODClaimImagesDocData.Columns.Add("odi_claim_app_id");
                    dtODClaimImagesDocData.Columns.Add("odi_image_desc");
                    dtODClaimImagesDocData.Columns.Add("odi_image_path");
                    dtODClaimImagesDocData.Columns.Add("odi_active_status");
                    if (objCAD.ClaimVehicleImages != null)
                    {
                        SaveMIODClaimImagesFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), "Claim Vehicle Images", true, objCAD.ClaimVehicleImages, objCAD.ClaimVehicleImagesFileName);
                    }
                    else
                    {
                        SaveMIODClaimImagesFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), "Claim Vehicle Images", true, objCAD.ClaimVehicleImages, objCAD.ClaimVehicleImagesFileName);
                    }
                    ///
                    dtODClaimDocData.Columns.Add("odcdd_claim_app_id");
                    dtODClaimDocData.Columns.Add("odcdd_claim_due_id");
                    dtODClaimDocData.Columns.Add("odcdd_doc_upload_path");
                    dtODClaimDocData.Columns.Add("odcdd_active_status");
                    if (objCAD.Odca_claim_id == 1)
                    {
                        if (objCAD.ClaimFormDoc1 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 1, true, objCAD.ClaimFormDoc1, objCAD.ClaimFormDoc1FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 1, true, objCAD.ClaimFormDoc1, objCAD.ClaimFormDoc1FileName);
                        }
                        if (objCAD.RegistrationCopyDoc1 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 2, true, objCAD.RegistrationCopyDoc1, objCAD.RegistrationCopyDoc1FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 2, true, objCAD.RegistrationCopyDoc1, objCAD.RegistrationCopyDoc1FileName);
                        }
                        if (objCAD.DLDoc1 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 3, true, objCAD.DLDoc1, objCAD.DLDoc1FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 3, true, objCAD.DLDoc1, objCAD.DLDoc1FileName);
                        }
                        if (objCAD.FIRDoc1 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 4, true, objCAD.FIRDoc1, objCAD.FIRDoc1FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 4, true, objCAD.FIRDoc1, objCAD.FIRDoc1FileName);
                        }
                        if (objCAD.EstimationReportDoc1 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 5, true, objCAD.EstimationReportDoc1, objCAD.EstimationReportDoc1FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 5, true, objCAD.EstimationReportDoc1, objCAD.EstimationReportDoc1FileName);
                        }
                    }
                    else if (objCAD.Odca_claim_id == 2)
                    {
                        if (objCAD.ClaimFormDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 6, true, objCAD.ClaimFormDoc2, objCAD.ClaimFormDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 6, true, objCAD.ClaimFormDoc2, objCAD.ClaimFormDoc2FileName);
                        }
                        if (objCAD.RegistrationCopyDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 7, true, objCAD.RegistrationCopyDoc2, objCAD.RegistrationCopyDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 7, true, objCAD.RegistrationCopyDoc2, objCAD.RegistrationCopyDoc2FileName);
                        }
                        if (objCAD.DLDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 8, true, objCAD.DLDoc2, objCAD.DLDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 8, true, objCAD.DLDoc2, objCAD.DLDoc2FileName);
                        }
                        if (objCAD.FIRDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 9, true, objCAD.FIRDoc2, objCAD.FIRDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 9, true, objCAD.FIRDoc2, objCAD.FIRDoc2FileName);
                        }
                        if (objCAD.CReportDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 10, true, objCAD.CReportDoc2, objCAD.CReportDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 10, true, objCAD.CReportDoc2, objCAD.CReportDoc2FileName);
                        }
                        if (objCAD.AffidavitDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 11, true, objCAD.AffidavitDoc2, objCAD.AffidavitDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 11, true, objCAD.AffidavitDoc2, objCAD.AffidavitDoc2FileName);
                        }
                        if (objCAD.ClaimDischargeFormDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 12, true, objCAD.ClaimDischargeFormDoc2, objCAD.ClaimDischargeFormDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 12, true, objCAD.ClaimDischargeFormDoc2, objCAD.ClaimDischargeFormDoc2FileName);
                        }
                        if (objCAD.AdvPayeeRecepit2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 13, true, objCAD.AdvPayeeRecepit2, objCAD.AdvPayeeRecepit2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 13, true, objCAD.AdvPayeeRecepit2, objCAD.AdvPayeeRecepit2FileName);
                        }
                        if (objCAD.RecipientIDDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 14, true, objCAD.RecipientIDDoc2, objCAD.RecipientIDDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 14, true, objCAD.RecipientIDDoc2, objCAD.RecipientIDDoc2FileName);
                        }
                    }
                    else if (objCAD.Odca_claim_id == 3)
                    {
                        if (objCAD.ClaimFormDoc3 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 15, true, objCAD.ClaimFormDoc3, objCAD.ClaimFormDoc3FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 15, true, objCAD.ClaimFormDoc3, objCAD.ClaimFormDoc3FileName);
                        }
                        if (objCAD.RegistrationCopyDoc3 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 16, true, objCAD.RegistrationCopyDoc3, objCAD.RegistrationCopyDoc3FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 16, true, objCAD.RegistrationCopyDoc3, objCAD.RegistrationCopyDoc3FileName);
                        }
                        if (objCAD.DLDoc3 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 17, true, objCAD.DLDoc3, objCAD.DLDoc3FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 17, true, objCAD.DLDoc3, objCAD.DLDoc3FileName);
                        }
                        if (objCAD.FIRDoc3 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 18, true, objCAD.FIRDoc3, objCAD.FIRDoc3FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 18, true, objCAD.FIRDoc3, objCAD.FIRDoc3FileName);
                        }
                        if (objCAD.EstimationReportDoc3 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 19, true, objCAD.EstimationReportDoc3, objCAD.EstimationReportDoc3FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 19, true, objCAD.EstimationReportDoc3, objCAD.EstimationReportDoc3FileName);
                        }
                    }
                    else if (objCAD.Odca_claim_id == 4)
                    {
                        if (objCAD.ClaimFormDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 20, true, objCAD.ClaimFormDoc4, objCAD.ClaimFormDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 20, true, objCAD.ClaimFormDoc4, objCAD.ClaimFormDoc4FileName);
                        }
                        if (objCAD.RegistrationCopyDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 21, true, objCAD.RegistrationCopyDoc4, objCAD.RegistrationCopyDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 21, true, objCAD.RegistrationCopyDoc4, objCAD.RegistrationCopyDoc4FileName);
                        }
                        if (objCAD.DLDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 22, true, objCAD.DLDoc4, objCAD.DLDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 22, true, objCAD.DLDoc4, objCAD.DLDoc4FileName);
                        }
                        if (objCAD.FIRDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 23, true, objCAD.FIRDoc4, objCAD.FIRDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 23, true, objCAD.FIRDoc4, objCAD.FIRDoc4FileName);
                        }
                        if (objCAD.EstimationReportDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 24, true, objCAD.EstimationReportDoc4, objCAD.EstimationReportDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 24, true, objCAD.EstimationReportDoc4, objCAD.EstimationReportDoc4FileName);
                        }
                        if (objCAD.RTOReportDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 25, true, objCAD.RTOReportDoc4, objCAD.RTOReportDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 25, true, objCAD.RTOReportDoc4, objCAD.RTOReportDoc4FileName);
                        }
                    }

                    ///
                    dtODComponentsData.Columns.Add("odccd_component_id");
                    dtODComponentsData.Columns.Add("odccd_component_name");
                    dtODComponentsData.Columns.Add("odccd_component_price");
                    dtODComponentsData.Columns.Add("odccd_od_claim_app_no");
                    //DataRow Ddr = dtODClaimDocData.NewRow();
                    //Ddr["odcdd_claim_app_id"] = ApplicationID ?? 0;
                    //Ddr["odcdd_claim_due_id"] = DocTypeID ?? 0;
                    //Ddr["odcdd_doc_upload_path"] = odcd_upload_document_path;
                    //Ddr["odcdd_active_status"] = true;
                    //dtODClaimDocData.Rows.Add(Ddr);

                    //var dt = new DataTable();
                    //dt.Columns.Add("ID", typeof(Int32));
                    //DataRow Ddr = dtODClaimDocData.NewRow();

                    //string newstr = objCAD.ClaimComponentListDetails.FirstOrDefault(en => en equals "undefined");
                    objCAD.ClaimComponentListDetails = objCAD.ClaimComponentListDetails.Where(p => !objCAD.ClaimComponentListDetails.Any(x => x.ID == p.ID && x.ID == "undefined")).ToList();
                    //objCAD.ClaimComponentListDetails = objCAD.ClaimComponentListDetails.Except("undefined").ToList();
                    if (objCAD.ClaimComponentListDetails != null)
                    {
                        for (int i = 0; i < objCAD.ClaimComponentListDetails.Count; i++)
                        {
                            DataRow Ddr = dtODComponentsData.NewRow();
                            //Ddr["odccd_component_id"] = objCAD.ClaimComponentListDetails[i].ID;
                            Ddr["odccd_component_name"] = objCAD.ClaimComponentListDetails[i].Type;
                            Ddr["odccd_component_price"] = objCAD.ClaimComponentListDetails[i].Value;
                            Ddr["odccd_od_claim_app_no"] = Convert.ToInt64(RefNo);
                            dtODComponentsData.Rows.Add(Ddr);
                        }
                    }

                    SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@reference_no",RefNo),
                    new SqlParameter("@employee_id",objCAD.Odca_proposer_id),
                    new SqlParameter("@category",objCAD.Odca_category_id),
                    //
                    new SqlParameter("@claim_id",objCAD.Odca_claim_id),
                    new SqlParameter("@damage_cost",objCAD.Odca_damage_cost),
                    new SqlParameter("@vehicle_number",objCAD.Odca_vehicle_number),
                    new SqlParameter("@policy_number",objCAD.Odca_policy_number),
                    new SqlParameter("@dateofaccident",objCAD.Odca_date_time_of_accident),
                    new SqlParameter("@accident_case_id",objCAD.Odca_accident_cause_id),
                    new SqlParameter("@place_of_accident",objCAD.Odca_place_of_accident),
                    new SqlParameter("@dist_id",objCAD.Odca_district_id),
                    new SqlParameter("@taluka_id",objCAD.Odca_taluka_id),
                    new SqlParameter("@MIODClaimVehicleImagesData",dtODClaimImagesDocData),
                    new SqlParameter("@MIODClaimDocumentsData",dtODClaimDocData),
                    new SqlParameter("@MIODComponentsData",dtODComponentsData)
                };
                    result = Convert.ToInt64(_Conn.ExecuteCmd(sqlparam, "sp_kgid_Save_MI_ODClaimApplicationDetails"));
                    //result = 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }
        public void SaveMIODClaimImagesFileData(long? EmpCode, long? ApplicationID, string ClaimImages, bool status, HttpPostedFileBase MIODDoc, string odcd_upload_document_path)
        {
            try
            {
                if (string.IsNullOrEmpty(odcd_upload_document_path))
                {
                    odcd_upload_document_path = UploadODClaimImagesDocument(MIODDoc, ApplicationID, ClaimImages);
                }
                DataRow Ddr = dtODClaimImagesDocData.NewRow();
                Ddr["odi_claim_app_id"] = ApplicationID ?? 0;
                Ddr["odi_image_desc"] = ClaimImages;
                Ddr["odi_image_path"] = odcd_upload_document_path;
                Ddr["odi_active_status"] = true;
                dtODClaimImagesDocData.Rows.Add(Ddr);
            }
            catch (Exception ex)
            {

            }
        }
        //OD Claim Accident Vehicle Document Upload
        private string UploadODClaimImagesDocument(HttpPostedFileBase document, long? AppId, string ClaimImages)
        {
            string subPath = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
                subPath = "/OD_Claim_Docs/" + AppId.ToString() + "/" + ClaimImages;
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(subPath));
                if (!exists)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                }

                string path = Path.Combine(HttpContext.Current.Server.MapPath(subPath), fileName);
                document.SaveAs(path);
                subPath = subPath + "/" + fileName;
            }

            return subPath;
        }
        public void SaveMIODClaimFileData(long? EmpCode, long? ApplicationID, long? ClaimID, long? DocTypeID, bool status, HttpPostedFileBase MIODDoc, string odcd_upload_document_path)
        {
            try
            {
                if (string.IsNullOrEmpty(odcd_upload_document_path))
                {
                    odcd_upload_document_path = UploadODClaimDocument(MIODDoc, ApplicationID, ClaimID);
                }
                DataRow Ddr = dtODClaimDocData.NewRow();
                Ddr["odcdd_claim_app_id"] = ApplicationID ?? 0;
                Ddr["odcdd_claim_due_id"] = DocTypeID ?? 0;
                Ddr["odcdd_doc_upload_path"] = odcd_upload_document_path;
                Ddr["odcdd_active_status"] = true;
                dtODClaimDocData.Rows.Add(Ddr);
            }
            catch (Exception ex)
            {

            }
        }
        //OD Claim Document Upload
        private string UploadODClaimDocument(HttpPostedFileBase document, long? AppId, long? ClaimId)
        {
            string subPath = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
                subPath = "/OD_Claim_Docs/" + AppId.ToString() + "/" + ClaimId;
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(subPath));
                if (!exists)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                }

                string path = Path.Combine(HttpContext.Current.Server.MapPath(subPath), fileName);
                document.SaveAs(path);
                subPath = subPath + "/" + fileName;
            }

            return subPath;
        }
        //Get OD Claim Application Details
        public VM_ODClaimApplicationDetails GetODClaimApplicationDetailsDLL(long EmployeeCode, string PolicyNumber)
        {
            VM_ODClaimApplicationDetails objPD = new VM_ODClaimApplicationDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",EmployeeCode),
                 new SqlParameter("@referenceid",PolicyNumber)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_ODClaimApplicationDetails");
                //bool OtherData = false;
                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        objPD.Odca_claim_app_no = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_claim_app_no"]);
                        objPD.Odca_claim_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_claim_id"]);
                        objPD.Odca_proposer_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_proposer_id"]);
                        objPD.Odca_category_id = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_category_id"]);
                        objPD.Odca_vehicle_number = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_vehicle_number"]);
                        objPD.Odca_policy_number = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_policy_number"]);
                        objPD.Odca_date_time_of_accident = Convert.ToDateTime(dsPD.Tables[0].Rows[0]["odca_date_time_of_accident"]);
                        objPD.Odca_accident_cause_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_accident_cause_id"]);
                        objPD.Odca_place_of_accident = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_place_of_accident"]);
                        objPD.Odca_district_id = Convert.ToInt32(dsPD.Tables[0].Rows[0]["odca_district_id"]);
                        objPD.Odca_taluka_id = Convert.ToInt32(dsPD.Tables[0].Rows[0]["odca_taluka_id"]);
                        objPD.Odca_damage_cost = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["odca_damage_cost"]);
                    }
                    if (dsPD.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPD.Tables[1].Rows)
                        {
                            if (Convert.ToInt64(dr["odcd_claim_id"]) == 1)
                            {
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 1)
                                {
                                    objPD.ClaimFormDoc1FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 2)
                                {
                                    objPD.RegistrationCopyDoc1FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 3)
                                {
                                    objPD.DLDoc1FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 4)
                                {
                                    objPD.FIRDoc1FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 5)
                                {
                                    objPD.EstimationReportDoc1FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                            }
                            else if (Convert.ToInt64(dr["odcd_claim_id"]) == 2)
                            {
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 6)
                                {
                                    objPD.ClaimFormDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 7)
                                {
                                    objPD.RegistrationCopyDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 8)
                                {
                                    objPD.DLDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 9)
                                {
                                    objPD.FIRDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 10)
                                {
                                    objPD.CReportDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 11)
                                {
                                    objPD.AffidavitDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 12)
                                {
                                    objPD.ClaimDischargeFormDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 13)
                                {
                                    objPD.AdvPayeeRecepit2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 14)
                                {
                                    objPD.RecipientIDDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                            }
                            else if (Convert.ToInt64(dr["odcd_claim_id"]) == 3)
                            {
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 15)
                                {
                                    objPD.ClaimFormDoc3FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 16)
                                {
                                    objPD.RegistrationCopyDoc3FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 17)
                                {
                                    objPD.DLDoc3FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 18)
                                {
                                    objPD.FIRDoc3FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 19)
                                {
                                    objPD.EstimationReportDoc3FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                            }
                            else if (Convert.ToInt64(dr["odcd_claim_id"]) == 4)
                            {
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 20)
                                {
                                    objPD.ClaimFormDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 21)
                                {
                                    objPD.RegistrationCopyDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 22)
                                {
                                    objPD.DLDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 23)
                                {
                                    objPD.FIRDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 24)
                                {
                                    objPD.EstimationReportDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 25)
                                {
                                    objPD.RTOReportDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                            }
                        }
                    }
                    if (dsPD.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPD.Tables[2].Rows)
                        {
                            objPD.ClaimVehicleImagesFileName = dr["odi_image_path"].ToString();
                        }
                    }
                    if (dsPD.Tables[3].Rows.Count > 0)
                    {
                        var myData = dsPD.Tables[3].AsEnumerable().Select(r => new ClaimComponentList1
                        {
                            //ID = Convert.ToString(r.Field<long>("odccd_component_id")),
                            Type = r.Field<string>("odccd_component_name"),
                            Value = r.Field<string>("odccd_component_price")
                        });
                        var list = myData.ToList();
                        objPD.ClaimComponentListDetails = list;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }
        //Get OD Claim Application Status 
        public VM_ODClaimVerificationDetails GetODClaimApplicationStatusListDLL(long empId, int category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                string description = GetCategoryDescription(category);


                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",empId),
                    new SqlParameter("@Category",category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getMI_ODClaim_ApplicationStatus");
                if (dsDDO.Tables[0].Rows.Count > 0)
                {
                    var CurrentStatusList = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                    {
                        EmployeeCode = dataRow.Field<long?>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                        //VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                        //VehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        //TypeofCover = dataRow.Field<string>("type_of_cover"),
                        //VehicleYear = dataRow.Field<string>("year"),

                        Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", description)) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", description)),
                        //Status = (dataRow.Field<string>("asm_status_desc").Replace("Applicant",description)),
                        VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                        //ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                        //EngineNo = dataRow.Field<string>("mivd_engine_no"),
                        AppStatusID = dataRow.Field<int>("status"),
                        //LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                        CategoryId = dataRow.Field<string>("odca_category_id"),
                        ApplicationId = dataRow.Field<long>("odca_id"),
                        // PolicyPremium = dataRow.Field<double?>("p_mi_premium"),
                        PolicyNumber = dataRow.Field<string>("odca_policy_number"),
                        //UnsignBondDocPath = dataRow.Field<string>("unsignbondpath"),
                        //SignedBondDocPath = dataRow.Field<string>("signedbondpath")
                    }).ToList();
                    verificationDetails.ViewStatusForEmployees = CurrentStatusList.Where(a => a.AppStatusID != 2 && a.AppStatusID != 1).ToList();

                    verificationDetails.LastUpdatedStatusForEmployees = CurrentStatusList.Where(a => a.AppStatusID == 2 || a.AppStatusID == 1).Select(a => a).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        private string GetCategoryDescription(int Category)
        {
            string description = "";
            if (Category == 2)
            {
                description = "Department";
            }
            else if (Category == 11)
            {
                description = "Agency";
            }
            else if (Category == 1)
            {
                description = "Employee";
            }
            return description;
        }
        #region OD Claim Workflow
        // OD Claim Workflow
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForCWVerificationDLL(long empId, string Category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",2),
                    new SqlParameter("@EmpId",empId),
                    new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_Claim_Application_Details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("odca_id")
                }).ToList();
                //var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                //{
                //    EmployeeCode = dataRow.Field<long>("employee_id"),
                //    Name = dataRow.Field<string>("employee_name"),
                //    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                //    Status = dataRow.Field<string>("asm_status_desc"),
                //    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                //    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                //    Premium = dataRow.Field<string>("p_premium")
                //}).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
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
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForSuperintendentVerificationDLL(long empId, string Category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",3),
                    new SqlParameter("@EmpId",empId),
                     new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_Claim_Application_Details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("odca_id")
                }).ToList();
                //var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                //{
                //    EmployeeCode = dataRow.Field<long>("employee_id"),
                //    Name = dataRow.Field<string>("employee_name"),
                //    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                //    Status = dataRow.Field<string>("asm_status_desc"),
                //    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                //    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                //    Premium = dataRow.Field<string>("p_premium")
                //}).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;

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

        public VM_ODClaimVerificationDetails GetEmployeeDetailsForDDVerificationDLL(long empId, string Category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",5),
                    new SqlParameter("@EmpId",empId),
                     new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_Claim_Application_Details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("odca_id")
                }).ToList();
                //var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                //{
                //    EmployeeCode = dataRow.Field<long>("employee_id"),
                //    Name = dataRow.Field<string>("employee_name"),
                //    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                //    Status = dataRow.Field<string>("asm_status_desc"),
                //    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                //    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                //    Premium = dataRow.Field<string>("p_premium")
                //}).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;

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

        public VM_ODClaimVerificationDetails GetEmployeeDetailsForDVerificationDLL(long empId, string Category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",6),
                    new SqlParameter("@EmpId",empId),
                     new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_Claim_Application_Details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("odca_id")
                }).ToList();
                //var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                //{
                //    EmployeeCode = dataRow.Field<long>("employee_id"),
                //    Name = dataRow.Field<string>("employee_name"),
                //    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                //    Status = dataRow.Field<string>("asm_status_desc"),
                //    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                //    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                //    Premium = dataRow.Field<string>("p_premium")
                //}).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;

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

        public VM_MIODClaimDeptVerficationDetails GetWorkFlowDetailsDLL(long applicationId, int category)
        {
            VM_MIODClaimDeptVerficationDetails ResultClaimWFDetails = new VM_MIODClaimDeptVerficationDetails();
            //IList<VM_MIODClaimWorkFlowDetails> workflowDetails = null;
            //IList<VM_ODClaimApplicationDetails> applicationdetails = null;
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@applicationId",applicationId),
                    new SqlParameter("@category",category)
                };

                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getMICliamWorkflowDetails");

                if (dsDDO.Tables.Count > 0 && dsDDO.Tables[0].Rows.Count > 0)
                {
                    //workflowDetails = new List<VM_MIODClaimWorkFlowDetails>();
                    foreach (DataRow dr in dsDDO.Tables[0].Rows)
                    {
                        VM_MIODClaimWorkFlowDetails workflowDetail = new VM_MIODClaimWorkFlowDetails();
                        workflowDetail.ApplicationRefNo = dr["ApplicationRefNo"].ToString();
                        workflowDetail.From = dr["From"].ToString();
                        workflowDetail.To = dr["To"].ToString();
                        workflowDetail.Remarks = dr["Remarks"].ToString();
                        workflowDetail.Comments = dr["Comments"].ToString();
                        workflowDetail.CreationDateTime = dr["CreationDateTime"].ToString();
                        workflowDetail.ApplicationStatus = dr["ApplicationStatus"].ToString();
                        workflowDetail.NameOfApplicant = dr["name"].ToString();
                        workflowDetail.Category = Convert.ToInt32(dr["odca_category_id"]);
                        ResultClaimWFDetails.WorkFlowDetails.Add(workflowDetail);
                    }
                    if (dsDDO.Tables[1].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[1].Rows)
                        {
                            ODClaimApplicationDetails ClaimApplicationDetails = new ODClaimApplicationDetails();
                            ClaimApplicationDetails.OD_Claim_Application_No = Convert.ToInt64(dr["odca_claim_app_no"]);
                            ClaimApplicationDetails.OD_Claim_Proposer_ID = Convert.ToInt64(dr["odca_proposer_id"]);
                            ClaimApplicationDetails.OD_Claim_Vehicle_Number = dr["odca_vehicle_number"].ToString();
                            ClaimApplicationDetails.OD_Claim_Policy_Number = dr["odca_policy_number"].ToString();
                            ClaimApplicationDetails.OD_Claim_ID = Convert.ToInt64(dr["odca_district_id"]);
                            ClaimApplicationDetails.OD_Claim_Damage_Cost = dr["odca_damage_cost"].ToString();
                            ClaimApplicationDetails.OD_Claim_Datetime_of_Accident = dr["odca_date_time_of_accident"].ToString();
                            ClaimApplicationDetails.OD_Claim_Accident_Cause_ID = Convert.ToInt64(dr["odca_accident_cause_id"]);
                            ClaimApplicationDetails.OD_Claim_Place_of_Accident = dr["odca_place_of_accident"].ToString();
                            ClaimApplicationDetails.OD_Claim_District_ID = Convert.ToInt64(dr["odca_district_id"]);
                            ClaimApplicationDetails.OD_Claim_Taluka_ID = Convert.ToInt64(dr["odca_taluka_id"]);
                            ClaimApplicationDetails.OD_Claim_District_Name = dr["dm_name_english"].ToString();
                            ClaimApplicationDetails.OD_Claim_Taluka_Name = dr["tm_englishname"].ToString();
                            ResultClaimWFDetails.ODClaimApplicationDetails.Add(ClaimApplicationDetails);
                        }
                    }
                    if (dsDDO.Tables[2].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[2].Rows)
                        {
                            ODClaimsDocumetsDetails ClaimDocDetails = new ODClaimsDocumetsDetails();
                            ClaimDocDetails.OD_Claim_ID = Convert.ToInt64(dr["odcd_claim_id"]);
                            ClaimDocDetails.OD_Claim_Description = dr["odc_description"].ToString();
                            ClaimDocDetails.OD_Claim_Document_Description = dr["odcd_document_desc"].ToString();
                            ClaimDocDetails.OD_Claim_Doc_id = Convert.ToInt64(dr["odcdd_id"]);
                            ClaimDocDetails.OD_Claim_Application_id = Convert.ToInt64(dr["odcdd_claim_app_id"]);
                            ClaimDocDetails.OD_Claim_Due_id = Convert.ToInt64(dr["odcdd_claim_due_id"]);
                            ClaimDocDetails.OD_Claim_Doc_Upload_Path = dr["odcdd_doc_upload_path"].ToString();
                            ResultClaimWFDetails.ClaimUploadDocumentDetails.Add(ClaimDocDetails);
                        }
                    }
                    if (dsDDO.Tables[3].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[3].Rows)
                        {
                            ODClaimsImageDetails ClaimImageDocDetails = new ODClaimsImageDetails();
                            ClaimImageDocDetails.OD_Claim_App_id = Convert.ToInt64(dr["odi_claim_app_id"]);
                            ClaimImageDocDetails.OD_Claim_Image_Description = dr["odi_image_desc"].ToString();
                            ClaimImageDocDetails.OD_Claim_Doc_Upload_Path = dr["odi_image_path"].ToString();
                            ResultClaimWFDetails.ClaimUploadImageDetails.Add(ClaimImageDocDetails);
                        }
                    }
                    if (dsDDO.Tables[4].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[4].Rows)
                        {
                            ODClaimsComponentDetailsApplicant ClaimComponentDetailsApplicant = new ODClaimsComponentDetailsApplicant();
                            ClaimComponentDetailsApplicant.ID = dr["odccd_component_id"].ToString();
                            ClaimComponentDetailsApplicant.Type = dr["odccd_component_name"].ToString();
                            ClaimComponentDetailsApplicant.Value = dr["odccd_component_price"].ToString();
                            ResultClaimWFDetails.ClaimsComponentDetailsApplicant.Add(ClaimComponentDetailsApplicant);
                        }
                    }
                    if (dsDDO.Tables[5].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[5].Rows)
                        {
                            ODClaimsComponentDetailsSurveyor ClaimComponentDetailsSurveyor = new ODClaimsComponentDetailsSurveyor();
                            ClaimComponentDetailsSurveyor.ID = dr["ssc_od_cost_component_id"].ToString();
                            ClaimComponentDetailsSurveyor.Type = dr["ssc_od_cost_component_name"].ToString();
                            ClaimComponentDetailsSurveyor.Value = dr["ssc_assesed_value"].ToString();
                            ResultClaimWFDetails.ClaimsComponentDetailsSurveyor.Add(ClaimComponentDetailsSurveyor);
                        }
                    }
                    if (dsDDO.Tables[6].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[6].Rows)
                        {
                            ODClaimsComponentDetailsDepartment ClaimComponentDetailsDepartment = new ODClaimsComponentDetailsDepartment();
                            ClaimComponentDetailsDepartment.ID = dr["odcap_cost_component_id"].ToString();
                            ClaimComponentDetailsDepartment.Type = dr["odcap_cost_component_name"].ToString();
                            ClaimComponentDetailsDepartment.Value = dr["odcap_component_cost_approved"].ToString();
                            ResultClaimWFDetails.ClaimsComponentDetailsDepartment.Add(ClaimComponentDetailsDepartment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return ResultClaimWFDetails;
        }
        public string SaveVerifiedDetailsDLL(VM_MIODClaimDeptVerficationDetails objVerification)
        {
            var result = "";
            ///OD Components Data Surveyor
            dtODComponentsDataSurveyor.Columns.Add("odccd_component_id");
            dtODComponentsDataSurveyor.Columns.Add("odccd_component_name");
            dtODComponentsDataSurveyor.Columns.Add("odccd_component_price");
            dtODComponentsDataSurveyor.Columns.Add("odccd_od_claim_app_no");
            ///OD Components Data Department
            dtODComponentsDataDepartment.Columns.Add("odccd_component_id");
            dtODComponentsDataDepartment.Columns.Add("odccd_component_name");
            dtODComponentsDataDepartment.Columns.Add("odccd_component_price");
            dtODComponentsDataDepartment.Columns.Add("odccd_od_claim_app_no");
            if (objVerification.ApplicationStatus == 6)
            {

                ///
                if (objVerification.ClaimsComponentDetailsSurveyor != null)
                {
                    for (int i = 0; i < objVerification.ClaimsComponentDetailsSurveyor.Count; i++)
                    {
                        DataRow Ddr = dtODComponentsDataSurveyor.NewRow();
                        //Ddr["odccd_component_id"] = objVerification.ClaimsComponentDetailsSurveyor[i].ID;
                        Ddr["odccd_component_name"] = objVerification.ClaimsComponentDetailsSurveyor[i].Type;
                        Ddr["odccd_component_price"] = objVerification.ClaimsComponentDetailsSurveyor[i].Value;
                        Ddr["odccd_od_claim_app_no"] = objVerification.ApplicationRefNo;
                        dtODComponentsDataSurveyor.Rows.Add(Ddr);
                    }
                }


            }
            else if (objVerification.ApplicationStatus == 15)
            {
                if (objVerification.ClaimsComponentDetailsDepartment != null)
                {
                    for (int i = 0; i < objVerification.ClaimsComponentDetailsDepartment.Count; i++)
                    {
                        DataRow Ddr = dtODComponentsDataDepartment.NewRow();
                        //Ddr["odccd_component_id"] = objVerification.ClaimsComponentDetailsDepartment[i].ID;
                        Ddr["odccd_component_name"] = objVerification.ClaimsComponentDetailsDepartment[i].Type;
                        Ddr["odccd_component_price"] = objVerification.ClaimsComponentDetailsDepartment[i].Value;
                        Ddr["odccd_od_claim_app_no"] = objVerification.ApplicationRefNo;
                        dtODComponentsDataDepartment.Rows.Add(Ddr);
                    }
                }
            }
            try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@employee_id",objVerification.EmpCode),
                    new SqlParameter("@micw_application_id",objVerification.ApplicationId),
                    new SqlParameter("@micw_application_refno",objVerification.ApplicationRefNo),
                    new SqlParameter("@micw_verified_by",objVerification.CreatedBy),
                    new SqlParameter("@micw_checklist_status",objVerification.VerifyApplicationDetails),
                    new SqlParameter("@micw_remarks",objVerification.Remarks),
                    new SqlParameter("@micw_comments",objVerification.Comments),
                    new SqlParameter("@micw_application_status",objVerification.ApplicationStatus),
                    new SqlParameter("@micw_active_status",true),
                    new SqlParameter("@micw_created_by",objVerification.CreatedBy),
                    new SqlParameter("@micw_creation_datetime",DateTime.Now),
                    new SqlParameter("@surveyor_id",objVerification.SurveyorId),
                    new SqlParameter("@damage_cost",objVerification.DamageCost),
                    new SqlParameter("@date_of_inspection",objVerification.DateOfInspection),
                    new SqlParameter("@MIODComponentsDataSurveyor",dtODComponentsDataSurveyor),
                    new SqlParameter("@MIODComponentsDataDepartment",dtODComponentsDataDepartment)

                };

                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_Insert_CW_ODClaimsWFVerification");
                if (objVerification.ApplicationStatus == 15)
                {
                    DataSet details = new DataSet();

                    SqlParameter[] sqlparamNotifDetails =
                    {
                        new SqlParameter("@employeeId", objVerification.EmpCode),
                        new SqlParameter("@applicationId",objVerification.ApplicationRefNo)

                    };

                    details = _Conn.ExeccuteDataset(sqlparamNotifDetails, "sp_kgid_getNotificationDetails");
                    //VM_NotificationDetailsMI notificationDetails = new VM_NotificationDetailsMI();

                    //if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
                    //{
                    //    notificationDetails.DDOEmailId = details.Tables[0].Rows[0]["DDOEmailId"].ToString();
                    //    notificationDetails.EmpEmailId = details.Tables[0].Rows[0]["EmpEmailId"].ToString();
                    //    notificationDetails.EmpMobileNumber = Convert.ToInt64(details.Tables[0].Rows[0]["EmpMobileNumber"].ToString());
                    //    notificationDetails.EmpName = details.Tables[0].Rows[0]["EmpName"].ToString();

                    //}

                    //SendInsurancePolicyNotification(notificationDetails);
                    //returnString = notificationDetails.PolicyNumber;
                }

            }
            catch (Exception ex)
            {

            }
            return result;


        }

        #endregion

        //public VM_MIOwnDamageClaimDetails GetMIOwnDamageClaimDetailsDLL(long empId, int category)
        //{
        //    VM_MIOwnDamageClaimDetails MIODclaimDetails = null;

        //    try
        //    {
        //        DataSet dsMIODClaims = new DataSet();
        //        SqlParameter[] sqlparam =
        //        {
        //            new SqlParameter("@employeeId", empId),
        //             new SqlParameter("@category", category)
        //        };

        //        dsMIODClaims = _Conn.ExeccuteDataset(sqlparam, "sp_mbclaims_getMBClaimDetails");
        //        if (dsMIODClaims.Tables[0].Rows.Count > 0)
        //        {
        //            MIODclaimDetails = new VM_MIOwnDamageClaimDetails();
        //            foreach (DataRow row in dsMIODClaims.Tables[0].Rows)
        //            {
        //                MotorInsuranceODDetailsMI claimDetail = new MotorInsuranceODDetailsMI();
        //                claimDetail.MIPolicyNumber = row["p_mi_policy_number"].ToString();
        //                claimDetail.MIPremium = Convert.ToDouble(row["p_mi_premium"].ToString());
        //                //claimDetail.IsBondReceived = Convert.ToBoolean(row["IsBondReceived"].ToString());
        //                //claimDetail.MIPremium = Convert.ToDecimal(row["NetAmount"].ToString());
        //                //claimDetail.PayableAmount = Convert.ToDecimal(row["PayableAmount"].ToString());
        //                //claimDetail.UnpaidLoanPremium = Convert.ToDecimal(row["UnpaidLoanPremium"].ToString());
        //                //claimDetail.UnpaidPolicyPremium = Convert.ToDecimal(row["UnpaidPolicyPremium"].ToString());
        //                //claimDetail.BonusAmount = Convert.ToDecimal(row["BonusAmount"].ToString());

        //                ///TODO: Add additional fields for maturity claims

        //                MIODclaimDetails.MIOwnDamageClaimDetails.Add(claimDetail);
        //            }
        //        }

        //        //if (dsClaims.Tables[1].Rows.Count > 0 && claimEmployeeDetail.ClaimDetails.Count > 0)
        //        //{
        //        //    claimEmployeeDetail.EmpName = dsClaims.Tables[1].Rows[0]["EmpName"].ToString();
        //        //    claimEmployeeDetail.EmpDesignation = dsClaims.Tables[1].Rows[0]["EmpDesignation"].ToString();
        //        //    claimEmployeeDetail.EmpDepartment = dsClaims.Tables[1].Rows[0]["EmpDepartment"].ToString();
        //        //}

        //        //if (dsClaims.Tables[2].Rows.Count > 0)
        //        //{
        //        //    foreach (DataRow row in dsClaims.Tables[2].Rows)
        //        //    {
        //        //        VM_ClaimDocument claimDocument = new VM_ClaimDocument();
        //        //        var filePath = row["DocumentPath"].ToString();
        //        //        claimDocument.DocumentFileName = Path.GetFileNameWithoutExtension(filePath);
        //        //        claimDocument.DocumentPath = filePath;
        //        //        claimDocument.DocumentType = row["DocumentType"].ToString();
        //        //        claimEmployeeDetail.ClaimDocuments.Add(claimDocument);
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return MIODclaimDetails;
        //}

        //Surveyor Workflow
        public VM_ODClaimSurveyorVerificationDetails GetEmployeeDetailsForSurveyorVerificationDLL(long EmpId)
        {
            VM_ODClaimSurveyorVerificationDetails verificationDetails = new VM_ODClaimSurveyorVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",EmpId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_ODClaimAppDetails_Surveyor");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new ApplicantVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("odca_proposer_id"),
                    //Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();

                verificationDetails.ApplicantVerificationDetails = EmployeeVerification;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                if (dsDDO.Tables[1].Rows.Count > 0)
                {
                    if (dsDDO.Tables[1].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[1].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[1].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[1].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[1].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[1].Rows[0]["FORWAREDED"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.AssignedApplications = Convert.ToInt64(dsDDO.Tables[1].Rows[0]["ASSIGNED"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        //View Approved Applcations
        public VM_ODClaimApprovedApplicationDetails GetApprovedApplicationListDLL(long EmpID, string Category)
        {
            VM_ODClaimApprovedApplicationDetails ResultAppDetails = new VM_ODClaimApprovedApplicationDetails();
            //IList<VM_MIODClaimWorkFlowDetails> workflowDetails = null;
            //IList<VM_ODClaimApplicationDetails> applicationdetails = null;
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",EmpID),
                    new SqlParameter("@category_id",Category)
                };

                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_ODClaim_Approved_App_List");

                if (dsDDO.Tables.Count > 0 && dsDDO.Tables[0].Rows.Count > 0)
                {
                    //workflowDetails = new List<VM_MIODClaimWorkFlowDetails>();
                    foreach (DataRow dr in dsDDO.Tables[0].Rows)
                    {
                        ApprovedApplicationDetails AprvdAppDetail = new ApprovedApplicationDetails();
                        AprvdAppDetail.ApplicationId = Convert.ToInt64(dr["odca_id"]);
                        AprvdAppDetail.ApplicationRefNo = Convert.ToInt64(dr["odca_claim_app_no"]);
                        AprvdAppDetail.EmpolyeeId = Convert.ToInt64(dr["odca_proposer_id"]);
                        AprvdAppDetail.CategoryId = Convert.ToInt64(dr["odca_category_id"]);
                        AprvdAppDetail.DamageCost = Convert.ToDecimal(dr["odca_damage_cost"]);
                        AprvdAppDetail.ApprovedDamageCost = Convert.ToDecimal(dr["micw_approved_damage_cost"]);
                        AprvdAppDetail.VehicleNo = dr["odca_vehicle_number"].ToString();
                        AprvdAppDetail.MIPolicyNo = dr["odca_policy_number"].ToString();
                        AprvdAppDetail.ApplicationStatus = dr["asm_status_desc"].ToString();
                        ResultAppDetails.ApprovedAppDetails.Add(AprvdAppDetail);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ResultAppDetails;
        }
        public VM_ODClaimWorkOrderDetails GetODClaimAprvdAppDetailsDLL(long EmployeeCode, string PolicyNumber, string Category)
        {
            VM_ODClaimWorkOrderDetails objPD = new VM_ODClaimWorkOrderDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",EmployeeCode),
                 new SqlParameter("@referenceid",PolicyNumber),
                 new SqlParameter("@category",Category)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_ODClaim_WorkOrderDetails");
                //bool OtherData = false;
                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        objPD.Odca_claim_app_no = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_claim_app_no"]);
                        objPD.Odca_claim_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_claim_id"]);
                        objPD.Odca_proposer_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_proposer_id"]);
                        objPD.Odca_category_id = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_category_id"]);
                        objPD.Odca_vehicle_number = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_vehicle_number"]);
                        objPD.Odca_policy_number = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_policy_number"]);
                        objPD.Odca_date_time_of_accident = Convert.ToDateTime(dsPD.Tables[0].Rows[0]["odca_date_time_of_accident"]);
                        objPD.Odca_accident_cause_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_accident_cause_id"]);
                        objPD.Odca_place_of_accident = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_place_of_accident"]);
                        objPD.Odca_district_name = Convert.ToString(dsPD.Tables[0].Rows[0]["dm_name_english"]);
                        objPD.Odca_taluka_name = Convert.ToString(dsPD.Tables[0].Rows[0]["tm_englishname"]);
                        objPD.Odca_damage_cost = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["odca_damage_cost"]);
                        objPD.micw_approved_damage_cost = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["micw_approved_damage_cost"]);
                        objPD.WorkOrderDate = Convert.ToDateTime(dsPD.Tables[0].Rows[0]["WorkOrderDate"]);
                        objPD.vy_vehicle_year = Convert.ToString(dsPD.Tables[0].Rows[0]["vy_vehicle_year"]);
                        objPD.ProposerName = Convert.ToString(dsPD.Tables[0].Rows[0]["ProposerName"]);
                        objPD.ProposerAddress = Convert.ToString(dsPD.Tables[0].Rows[0]["ProposerAddress"]);
                        objPD.DdoOffice = Convert.ToString(dsPD.Tables[0].Rows[0]["DdoOffice"]);
                    }

                    if (dsPD.Tables[1].Rows.Count > 0)
                    {
                        var myData = dsPD.Tables[1].AsEnumerable().Select(r => new ApprovedClaimComponentList
                        {
                            ID = Convert.ToString(r.Field<long>("odcap_cost_component_id")),
                            Type = r.Field<string>("odcc_description"),
                            Value = r.Field<string>("odcap_component_cost_approved")
                        });
                        var list = myData.ToList();
                        objPD.ClaimComponentListDetails = list;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }


        #region Master Data Dist&Taluka&Components
        //Dist List
        public List<tbl_district_master> GetDistListDLL()
        {
            return (from distList in _db.tbl_district_master
                    select distList).ToList();
        }
        //Taluka List
        public List<tbl_taluka_master> GetTalukaListDLL(int DistId)
        {
            return (from TalukaList in _db.tbl_taluka_master where TalukaList.tm_distid == DistId select TalukaList).ToList();
        }
        //Component List
        public List<tbl_od_cost_component_master> GetComponentListDLL()
        {
            return (from ComponentList in _db.tbl_od_cost_component_master
                    select ComponentList).ToList();
        }
        public List<GetVehicleChassisPolicyDetails> GetVehicleAndPolicyDetailsDLL(string txtDetails)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();

            vehicleDetails = (from MIA in _db.tbl_motor_insurance_application
                              join item in _db.tbl_motor_insurance_vehicle_details on MIA.mia_motor_insurance_app_id equals item.mivd_application_id
                              join MPD in _db.tbl_mi_policy_details on MIA.mia_motor_insurance_app_id equals MPD.p_mi_application_id
                              join VPC in _db.tbl_vehicle_type_master on item.mivd_vehicle_type equals VPC.vht_vehicle_type_id
                              join VCTM in _db.tbl_vehicle_category_type_master on item.mivd_cat_type_id equals VCTM.vct_vehicle_category_type_id
                              join VCTMaster in _db.tbl_vehicle_category_master on item.mivd_vehicle_category equals VCTMaster.vc_vehicle_category_id
                              join Cover in _db.tbl_motor_insurance_type_of_cover on MIA.mia_type_of_cover equals Cover.mitoc_type_cover_id
                              join VSCM in _db.tbl_vehicle_subtype_master on item.mivd_vehicle_subtype equals VSCM.vst_vehicle_subtype_id
                              join VMM in _db.tbl_vehicle_make_master on item.mivd_make_of_vehicle equals VMM.vm_vehicle_make_id
                              where item.mivd_registration_no == txtDetails || item.mivd_chasis_no == txtDetails || MPD.p_mi_policy_number == txtDetails
                              select new GetVehicleChassisPolicyDetails
                              {
                                  vehicle_registration_no = item.mivd_registration_no,
                                  vehicle_chasis_no = item.mivd_chasis_no,
                                  Vehicle_Category_Type = VCTM.vct_vehicle_category_type_desc,
                                  vehicle_category_desc = VCTMaster.vc_vehicle_category_desc,
                                  vehicle_model = item.mivd_type_of_model,
                                  Vehicle_Type = VPC.vht_vehicle_type_desc,
                                  type_of_Cover = Cover.mitoc_type_cover_name,
                                  Policy_number = MPD.p_mi_policy_number,
                                  OD_from_date = MPD.p_mi_from_date,
                                  OD_to_date = MPD.p_mi_to_date,
                                  TP_from_date = MPD.p_mi_tpfrom_date,
                                  TP_to_date = MPD.p_mi_tpto_date,
                                  owner_name_vehicle = MIA.mia_owner_of_the_vehicle,
                                  cubic_capacity_vehicle = item.mivd_cubic_capacity,
                                  owner_name_vehicle_address = MIA.mia_address,
                                  seating_capacity_vehicle = item.mivd_seating_capacity_including_driver,
                                  Vehicle_subtype_desc = VSCM.vst_vehicle_subtype_desc,
                                  vehicle_make_desc = VMM.vm_vehicle_make_desc
                              }).ToList();

            return vehicleDetails;
        }
        public SelectList GetDistrictListDLL()
        {
            List<SelectListItem> DistrictList = new List<SelectListItem>();

            DistrictList = (from District in _db.tbl_district_master
                            select new SelectListItem
                            {
                                Text = District.dm_name_english,
                                Value = District.dm_code
                            }).ToList();
            return new SelectList(DistrictList, "Value", "Text");

        }
        public SelectList GetTalukListDLL(int dm_code)
        {
            List<SelectListItem> TalukList = new List<SelectListItem>();

            TalukList = (from taluk in _db.tbl_taluka_master
                         where taluk.tm_distid == dm_code
                         select new SelectListItem
                         {
                             Text = taluk.tm_englishname,
                             Value = taluk.tm_code
                         }).ToList();
            var DistrictIndexlist = new SelectListItem()
            {

                Value = null,
                Text = "--Select Taluk--",

            };
            TalukList.Insert(0, DistrictIndexlist);
            return new SelectList(TalukList, "Value", "Text");

        }
        public SelectList GetRemarksDLL()
        {
            List<SelectListItem> RemarkList = new List<SelectListItem>();

            RemarkList = (from remark in _db.tbl_remarks_master

                          select new SelectListItem
                          {
                              Text = remark.RM_Remarks_Desc,
                              Value = (remark.RM_Remarks_id).ToString()
                          }).ToList();
            //var RemarksIndexlist = new SelectListItem()
            //{

            //    Value = null,
            //    Text = "--Select Taluk--",

            //};
            //RemarkList.Insert(0, RemarksIndexlist);
            return new SelectList(RemarkList, "Value", "Text");

        }
        public long SaveMVCClaimDetailsDLL(GetVehicleChassisPolicyDetails model)
        {
            int abc = 0;
            try
            {
                
                tbl_mvc_application_details mvc_ = _db.tbl_mvc_application_details.Where(x => x.mvc_claim_app_id == model.MVC_claim_app_id).FirstOrDefault();
                if (mvc_ == null)
                {
                    tbl_mvc_application_details tbl_data = new tbl_mvc_application_details();

                    string App_no = DateTime.Now.ToString("yyyy-MM-dd HH:ffff").Replace("-", "").Replace(" ", "").Replace(":", "").Replace(".", "");
                    model.MVC_claim_app_id = Convert.ToInt64(App_no);
                    long Application_no = Convert.ToInt64(App_no);
                    tbl_data.mvc_claim_app_id = Application_no;
                    tbl_data.chassis_no = model.vehicle_chasis_no;
                    tbl_data.policy_no = model.Policy_number;
                    tbl_data.mvc_no = (model.MVC_number).ToString();
                    tbl_data.date_of_petition = Convert.ToDateTime(model.CourtTime);
                    tbl_data.name_of_court = model.Name_of_court;
					if (model.stateID == 29)
                {
                    tbl_data.court_district = model.District_dm_id;
                    tbl_data.court_taluk = model.Taluk_id;
                }
                else
                {
                    tbl_data.court_district = 0;
                    tbl_data.court_taluk = 0;
                    tbl_data.other_state_court_taluk = model.other_state_court_taluk;
                    tbl_data.other_state_court_dist = model.other_state_court_dist;
                }

                    
                    tbl_data.name_of_petitioner = "";
                    tbl_data.petitioner_pincode = 11;
                    tbl_data.petitioner_mobile_no = 99;
                    tbl_data.respondant_name = "";
                    tbl_data.respondant_designation = "";
                    tbl_data.respondant_department = "";
                    tbl_data.respondant_agencyName = "";
                    tbl_data.respondant_address = "";
                    tbl_data.respondant_mobile_no = 90;
                    tbl_data.respondant_pincode = 90;
                    tbl_data.accident_district = 0;
                    tbl_data.accident_taluk = 0;
                    tbl_data.accident_hobli = "";
                    tbl_data.accident_grampanchayat = "";
                    tbl_data.accident_village = "";
                    tbl_data.accident_details = "";
                    tbl_data.claim_amount = Convert.ToDecimal(model.claim_Amount);
                    tbl_data.rc_details = "";
                    tbl_data.dl_details = "";
                    tbl_data.fir_details = "";
                    tbl_data.panchanama_details = "";
                    tbl_data.remarks = model.Remarks_id;
                    tbl_data.status_id = true;
                    tbl_data.verified_document = true;
                    tbl_data.created_by = model.loginId;
                    tbl_data.updated_by = 0;
                    tbl_data.mvc_claim_creation_datetime = DateTime.Now;
                    tbl_data.mvc_claim_updation_datetime = DateTime.Now;
                    tbl_data.acdnt_name_of_injured_person = model.Name_of_injured;
                    tbl_data.acdnt_person_father_name = model.Father_name;
                    tbl_data.acdnt_person_spouse_name = model.Spouse_name;
                    tbl_data.acdnt_person_full_address = model.Address_of_dead_details;
                    tbl_data.acdnt_person_age = model.Age_of_injured;
                    tbl_data.acdnt_person_occupation = model.occupation_of_injured;
                    tbl_data.acdnt_emp_nameadress_deceased = model.employer_deceased_details;
                    tbl_data.acdnt_person_monthly_income = Convert.ToDecimal(model.monthly_income_of_injured);
                    tbl_data.acdnt_comp_claimed_tax = Convert.ToDecimal(model.income_tax_of_injured);
                    tbl_data.acdnt_place_accident = model.place_of_accident;
                    tbl_data.acdnt_date_time_of_accident = Convert.ToDateTime(model.accident_DateTime);
                    tbl_data.acdnt_police_station_details = model.police_station_of_jurisdiction;
                    tbl_data.acdnt_compens_claimed_travelling = model.police_station_of_compensation;
                    tbl_data.acdnt_type_of_injury = model.type_injury;
                    tbl_data.acdnt_nature_of_injury = model.nature_of_injuries_sustained;
                    tbl_data.acdnt_medical_officer_detail = model.medical_officer;
                    tbl_data.acdnt_period_treatment_expend = model.Period_of_treatment_of_details;
                    tbl_data.acdnt_name_of_injury_caused = model.Name_of_injury_caused_of_details;
                    tbl_data.acdnt_applicant_details = model.Name_and_address_of_applicant_details;
                    tbl_data.acdnt_relation_details = model.relation_with_deceased;
                    tbl_data.acdnt_title_to_property = model.title_property_deceased;
                    tbl_data.acdt_any_other_info = model.any_other_information_details;
                    tbl_data.state_id = model.stateID;
                    tbl_data.app_saved_status = model.application_stat;
                    // app_saved_status 1 for draft
                    //                  2 for send to sup
                    _db.tbl_mvc_application_details.Add(tbl_data);
                     abc = _db.SaveChanges();
                }
                
                else
                {
                    mvc_.chassis_no = model.vehicle_chasis_no;
                    mvc_.policy_no = model.Policy_number;
                    mvc_.date_of_petition = Convert.ToDateTime(model.CourtTime);
                    mvc_.name_of_court = model.Name_of_court;
                   if (model.stateID == 29)
                {
                        mvc_.court_district = model.District_dm_id;
                        mvc_.court_taluk = model.Taluk_id;
                        mvc_.other_state_court_taluk = "";
                        mvc_.other_state_court_dist = "";
                }
                else
                {
                        mvc_.court_district = 0;
                        mvc_.court_taluk = 0;
                        mvc_.other_state_court_taluk = model.other_state_court_taluk;
                        mvc_.other_state_court_dist = model.other_state_court_dist;
                }

                    mvc_.name_of_petitioner = "";
                    mvc_.petitioner_pincode = 11;
                    mvc_.petitioner_mobile_no = 99;
                    mvc_.respondant_name = "";
                    mvc_.respondant_designation = "";
                    mvc_.respondant_department = "";
                    mvc_.respondant_agencyName = "";
                    mvc_.respondant_address = "";
                    mvc_.respondant_mobile_no = 90;
                    mvc_.respondant_pincode = 90;
                    mvc_.accident_district = 0;
                    mvc_.accident_taluk = 0;
                    mvc_.accident_hobli = "";
                    mvc_.accident_grampanchayat = "";
                    mvc_.accident_village = "";
                    mvc_.accident_details = "";
                    mvc_.claim_amount = Convert.ToDecimal(model.claim_Amount);
                    mvc_.rc_details = "";
                    mvc_.dl_details = "";
                    mvc_.fir_details = "";
                    mvc_.panchanama_details = "";
                    mvc_.remarks = model.Remarks_id;
                    mvc_.status_id = true;
                    mvc_.verified_document = true;
                    mvc_.created_by = model.loginId;
                    mvc_.updated_by = 0;
                    mvc_.mvc_claim_creation_datetime = DateTime.Now;
                    mvc_.mvc_claim_updation_datetime = DateTime.Now;
                    mvc_.acdnt_name_of_injured_person = model.Name_of_injured;
                    mvc_.acdnt_person_father_name = model.Father_name;
                    mvc_.acdnt_person_spouse_name = model.Spouse_name;
                    mvc_.acdnt_person_full_address = model.Address_of_dead_details;
                    mvc_.acdnt_person_age = model.Age_of_injured;
                    mvc_.acdnt_person_occupation = model.occupation_of_injured;
                    mvc_.acdnt_emp_nameadress_deceased = model.employer_deceased_details;
                    mvc_.acdnt_person_monthly_income = Convert.ToDecimal(model.monthly_income_of_injured);
                    mvc_.acdnt_comp_claimed_tax = Convert.ToDecimal(model.income_tax_of_injured);
                    mvc_.acdnt_place_accident = model.place_of_accident;
                    mvc_.acdnt_date_time_of_accident = Convert.ToDateTime(model.accident_DateTime);
                    mvc_.acdnt_police_station_details = model.police_station_of_jurisdiction;
                    mvc_.acdnt_compens_claimed_travelling = model.police_station_of_compensation;
                    mvc_.acdnt_type_of_injury = model.type_injury;
                    mvc_.acdnt_nature_of_injury = model.nature_of_injuries_sustained;
                    mvc_.acdnt_medical_officer_detail = model.medical_officer;
                    mvc_.acdnt_period_treatment_expend = model.Period_of_treatment_of_details;
                    mvc_.acdnt_name_of_injury_caused = model.Name_of_injury_caused_of_details;
                    mvc_.acdnt_applicant_details = model.Name_and_address_of_applicant_details;
                    mvc_.acdnt_relation_details = model.relation_with_deceased;
                    mvc_.acdnt_title_to_property = model.title_property_deceased;
                    mvc_.acdt_any_other_info = model.any_other_information_details;
                    mvc_.state_id = model.stateID;
                    mvc_.app_saved_status = model.application_stat;
                   abc= _db.SaveChanges();

                    
                
                }
                if (abc == 1)
                {
                    int c = PetitionerRespondantDetailsDLL(model.MVC_claim_app_id, model);
                }


                var returnMessage = UpdateWork_flow_Details(model);
               

                //foreach (var files in )
                //{
                //}

               // return model.MVC_claim_app_id;


            }
            catch (Exception e)
            {

                throw e;
            }

            return model.MVC_claim_app_id;
        }

        public int UpdateWork_flow_Details(GetVehicleChassisPolicyDetails model)
        {
            if (model != null)
            {
                try
                {
                    List<tbl_mvc_claim_workflow> oldFlowData = _db.tbl_mvc_claim_workflow.Where(x => x.mvc_claim_app_id == model.MVC_claim_app_id).ToList();
                    if (oldFlowData.Count() != 0)
                    {
                        foreach (var flow in oldFlowData)
                        {
                            flow.micw_active_status = false;

                        }
                    }
                    tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                    work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                    work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number!=null)?model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number );
                    work_flow.micw_policy_number = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].micw_policy_number);
                    work_flow.micw_remarks = model.Remarks_id;
                    work_flow.micw_comments = model.Comments_details;
                    work_flow.micw_verified_by = model.loginId;
                    work_flow.micw_checklist_status = true;
                    work_flow.micw_application_status = model.Category_id;
                    work_flow.micw_surveyor_registered = true;
                    work_flow.micw_approved_damage_cost = Convert.ToDecimal(model.claim_Amount);
                    work_flow.micw_active_status = true;
                    work_flow.micw_creation_datetime = DateTime.Now;
                    work_flow.micw_updation_datetime = DateTime.Now;
                    work_flow.micw_assigned_to = model.roleID;
                    _db.tbl_mvc_claim_workflow.Add(work_flow);
                    int abc = _db.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw ex;
                    
                }
            }
            else
            {
                return 0;
            }
            return 1;

        }

        public int SavePathDetailsDLL(string path, long App_id)
        {
            int ans = 1;
            int returnpathStatus = 0;
            if (path != null && App_id != 0)
            {
                try
                {
                    List<tbl_mvc_claim_doc_details> pathData = _db.tbl_mvc_claim_doc_details.Where(x => x.mvcdd_claim_app_id == App_id).ToList();

                    if (pathData.Count() == 0)
                    {
                         returnpathStatus = savePathOfFile(path, App_id);
                    }
                    else {
                   
                      //  if (dataP.mvcdd_doc_upload_path.Contains("/PrefilledClaimForm/") && path.Contains("/PrefilledClaimForm/"))
                     //       {
                     //           dataP.mvcdd_doc_upload_path = path;
                     //           break;
                     //       }
                            
                     //       else if (dataP.mvcdd_doc_upload_path.Contains("/DsRc/") && path.Contains("/DsRc/"))
                     //       {
                     //           dataP.mvcdd_doc_upload_path = path;
                     //           break;
                     //       }
                            

                     //       else if (dataP.mvcdd_doc_upload_path.Contains("/Covering_Letter/") && path.Contains("/Covering_Letter/"))
                     //       {
                     //           dataP.mvcdd_doc_upload_path = path;
                     //           break;
                     //       }
                           

                     //       else if (dataP.mvcdd_doc_upload_path.Contains("/InsuranceCopy/") && path.Contains("/InsuranceCopy/"))
                     //       {
                     //           dataP.mvcdd_doc_upload_path = path;
                     //           break;
                     //       }
                           
                     //       else if (dataP.mvcdd_doc_upload_path.Contains("/Court_Notice_details/") && path.Contains("/Court_Notice_details/"))
                     //       {
                     //           dataP.mvcdd_doc_upload_path = path;
                     //           break;
                     //       }
                     //       else if (dataP.mvcdd_doc_upload_path.Contains("/Petitioner_details/") && path.Contains("/Petitioner_details/"))
                     //       {
                     //           dataP.mvcdd_doc_upload_path = path;
                     //           break;
                     //       }
                           
                     //       else if (dataP.mvcdd_doc_upload_path.Contains("/DL/") && path.Contains("/DL/"))
                     //       {
                     //           dataP.mvcdd_doc_upload_path = path;
                     //           break;
                     //       }
                     //       else
                     //       {
                                 returnpathStatus = savePathOfFile(path, App_id);
                            //    break;
                            //}


                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
            else
            {
                ans = 0;
                return ans;
            }

            return ans;
        }

        public int PetitionerRespondantDetailsDLL(long Application_id, GetVehicleChassisPolicyDetails model)
        {
            if (Application_id != 0)
            {
                try
                {
                    if (model.PetitionerList.Count() != 0)
                    {
                       
                        List<tbl_mvc_claim_petitioner_details> mvc_petitioner = _db.tbl_mvc_claim_petitioner_details.Where(x => x.mvc_claim_app_id == Application_id).ToList();

                        if (mvc_petitioner.Count == 0)
                        {

                            foreach (var item in model.PetitionerList)
                            {

                                tbl_mvc_claim_petitioner_details mvc_doc = new tbl_mvc_claim_petitioner_details();
                                mvc_doc.mvc_claim_app_id = model.MVC_claim_app_id;
                                mvc_doc.mvc_petitioner_name = item.name_Of_Petitioner;
                                mvc_doc.mvc_petitioner_addres = item.petitioner_Address;
                                mvc_doc.mvc_petitioner_mobile_no = Convert.ToInt64(item.petitioner_Mobile_no);
                                mvc_doc.mvcp_active_status = true;
                                mvc_doc.mvc_petitioner_pincode_no = Convert.ToInt64(item.pincode_Of_Petitioner);
                                mvc_doc.mvcp_creation_datetime = DateTime.Now;
                                mvc_doc.mvcp_updation_datetime = DateTime.Now;
                                mvc_doc.mvcp_updated_by = 1;
                                mvc_doc.mvcp_created_by = model.loginId;

                                _db.tbl_mvc_claim_petitioner_details.Add(mvc_doc);
                                int abc = _db.SaveChanges();
                            }
                        }
                        else {
                            foreach (var data in mvc_petitioner) {

                                _db.tbl_mvc_claim_petitioner_details.Remove(data);
                            }
                            _db.SaveChanges();
                            foreach (var item in model.PetitionerList)
                            {
                                tbl_mvc_claim_petitioner_details mvc_doc = new tbl_mvc_claim_petitioner_details();
                                mvc_doc.mvc_claim_app_id = model.MVC_claim_app_id;
                                mvc_doc.mvc_petitioner_name = item.name_Of_Petitioner;
                                mvc_doc.mvc_petitioner_addres = item.petitioner_Address;
                                mvc_doc.mvc_petitioner_mobile_no = Convert.ToInt64(item.petitioner_Mobile_no);
                                mvc_doc.mvcp_active_status = true;
                                mvc_doc.mvc_petitioner_pincode_no = Convert.ToInt64(item.pincode_Of_Petitioner);
                                mvc_doc.mvcp_creation_datetime = DateTime.Now;
                                mvc_doc.mvcp_updation_datetime = DateTime.Now;
                                mvc_doc.mvcp_updated_by = 1;
                                mvc_doc.mvcp_created_by = model.loginId;

                                _db.tbl_mvc_claim_petitioner_details.Add(mvc_doc);
                                int abc = _db.SaveChanges();
                            }
                        }
                    }
                    if (model.RespondantList.Count() != 0)
                    {
                        List<tbl_mvc_respondent_details> mvc_respondant = _db.tbl_mvc_respondent_details.Where(x => x.mvc_claim_app_id == Application_id).ToList();

                        if (mvc_respondant.Count == 0)
                        {
                            foreach (var data in model.RespondantList)
                            {
                                tbl_mvc_respondent_details mvc_respond = new tbl_mvc_respondent_details();
                                mvc_respond.mvc_claim_app_id = Application_id;
                                mvc_respond.mvcrd_respondent_name = data.Respondant_name;
                                mvc_respond.mvcrd_designation_name = data.Respondant_designation;
                                mvc_respond.mvcrd_department_name = data.Respondant_department;
                                mvc_respond.mvcrd_agency_name = data.Respondant_Agency_name;
                                mvc_respond.mvcrd_designation_name = data.Respondant_designation;
                                mvc_respond.mvcrd_respondent_addres = data.Respondant_address;
                                mvc_respond.mvcrd_pincode_no = Convert.ToInt64(data.Respondant_pincode);
                                mvc_respond.mvcrd_respondent_mobile_no = Convert.ToInt64(data.Respondant_mobile);
                                mvc_respond.mvcrd_active_status = true;
                                mvc_respond.mvcrd_creation_datetime = DateTime.Now;
                                mvc_respond.mvcrd_updation_datetime = DateTime.Now;
                                mvc_respond.mvcrd_created_by = model.loginId;
                                mvc_respond.mvcrd_updated_by = 1;
                                _db.tbl_mvc_respondent_details.Add(mvc_respond);
                                int abc = _db.SaveChanges();

                            }
                        }
                        else {
                            foreach (var resData in mvc_respondant)
                            {

                                _db.tbl_mvc_respondent_details.Remove(resData);
                            }
                            _db.SaveChanges();
                            foreach (var data in model.RespondantList)
                            {
                                tbl_mvc_respondent_details mvc_respond = new tbl_mvc_respondent_details();
                                mvc_respond.mvc_claim_app_id = Application_id;
                                mvc_respond.mvcrd_respondent_name = data.Respondant_name;
                                mvc_respond.mvcrd_designation_name = data.Respondant_designation;
                                mvc_respond.mvcrd_department_name = data.Respondant_department;
                                mvc_respond.mvcrd_agency_name = data.Respondant_Agency_name;
                                mvc_respond.mvcrd_designation_name = data.Respondant_designation;
                                mvc_respond.mvcrd_respondent_addres = data.Respondant_address;
                                mvc_respond.mvcrd_pincode_no = Convert.ToInt64(data.Respondant_pincode);
                                mvc_respond.mvcrd_respondent_mobile_no = Convert.ToInt64(data.Respondant_mobile);
                                mvc_respond.mvcrd_active_status = true;
                                mvc_respond.mvcrd_creation_datetime = DateTime.Now;
                                mvc_respond.mvcrd_updation_datetime = DateTime.Now;
                                mvc_respond.mvcrd_created_by = model.loginId;
                                mvc_respond.mvcrd_updated_by = 1;
                                _db.tbl_mvc_respondent_details.Add(mvc_respond);
                                int abc = _db.SaveChanges();

                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
            {

                return 0;
            }

            return 1;
        }

        public List<GetVehicleChassisPolicyDetails> GetMVCApplicationFormDataDLL()
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_application_details
                              where data.app_saved_status==2 || data.app_saved_status==3
                              select new GetVehicleChassisPolicyDetails
                              {
                                  Court_MVC_number = data.mvc_no,
                                  vehicle_chasis_no = data.chassis_no,
                                  MVC_claim_app_id = data.mvc_claim_app_id,
                                  application_stat = data.app_saved_status

        }).Distinct().ToList();
          
          

            return vehicleDetails;
        }
         public List<GetVehicleChassisPolicyDetails> GetDraftDetailsDLL()
        {
            List<GetVehicleChassisPolicyDetails> GetDraft = new List<GetVehicleChassisPolicyDetails>();
            GetDraft = (from CMM in _db.tbl_mvc_application_details
                                where CMM.app_saved_status == 1
                                select new GetVehicleChassisPolicyDetails
                                {
                                    Court_MVC_number = CMM.mvc_no,
                                    vehicle_chasis_no = CMM.chassis_no,
                                    MVC_claim_app_id = CMM.mvc_claim_app_id,
                                }).ToList();


            return GetDraft;
        }

        public List<GetVehicleChassisPolicyDetails> GetMVCGetDetailsOnChassisDLL(string ChassisNo)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();

            vehicleDetails = (from MIA in _db.tbl_motor_insurance_application
                              join item in _db.tbl_motor_insurance_vehicle_details on MIA.mia_motor_insurance_app_id equals item.mivd_application_id
                              join MPD in _db.tbl_mi_policy_details on MIA.mia_motor_insurance_app_id equals MPD.p_mi_application_id
                              join VPC in _db.tbl_vehicle_type_master on item.mivd_vehicle_type equals VPC.vht_vehicle_type_id
                              join VCTM in _db.tbl_vehicle_category_type_master on item.mivd_cat_type_id equals VCTM.vct_vehicle_category_type_id
                              join VCTMaster in _db.tbl_vehicle_category_master on item.mivd_vehicle_category equals VCTMaster.vc_vehicle_category_id
                              join Cover in _db.tbl_motor_insurance_type_of_cover on MIA.mia_type_of_cover equals Cover.mitoc_type_cover_id
                              join VSCM in _db.tbl_vehicle_subtype_master on item.mivd_vehicle_subtype equals VSCM.vst_vehicle_subtype_id
                              join VMM in _db.tbl_vehicle_make_master on item.mivd_make_of_vehicle equals VMM.vm_vehicle_make_id
                              where item.mivd_chasis_no ==ChassisNo
                              select new GetVehicleChassisPolicyDetails
                              {
                                  vehicle_registration_no = item.mivd_registration_no,
                                  vehicle_chasis_no = item.mivd_chasis_no,
                                  Vehicle_Category_Type = VCTM.vct_vehicle_category_type_desc,
                                  vehicle_category_desc = VCTMaster.vc_vehicle_category_desc,
                                  vehicle_model = item.mivd_type_of_model,
                                  Vehicle_Type = VPC.vht_vehicle_type_desc,
                                  type_of_Cover = Cover.mitoc_type_cover_name,
                                  Policy_number = MPD.p_mi_policy_number,
                                  OD_from_date = MPD.p_mi_from_date,
                                  OD_to_date = MPD.p_mi_to_date,
                                  TP_from_date = MPD.p_mi_tpfrom_date,
                                  TP_to_date = MPD.p_mi_tpto_date,
                                  owner_name_vehicle = MIA.mia_owner_of_the_vehicle,
                                  cubic_capacity_vehicle = item.mivd_cubic_capacity,
                                  owner_name_vehicle_address = MIA.mia_address,
                                  seating_capacity_vehicle = item.mivd_seating_capacity_including_driver,
                                  Vehicle_subtype_desc = VSCM.vst_vehicle_subtype_desc,
                                  vehicle_make_desc = VMM.vm_vehicle_make_desc
                              }).ToList();

            return vehicleDetails;
        }

        public List<GetVehicleChassisPolicyDetails> GetMVCPetitionerDetailsDLL(long Appno)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_application_details
                               join item in _db.tbl_mvc_claim_petitioner_details on data.mvc_claim_app_id equals item.mvc_claim_app_id
                               where data.mvc_claim_app_id == Appno

                              select new GetVehicleChassisPolicyDetails
                               {
                                   petitioner_name= item.mvc_petitioner_name,
                                   petitioner_Mobile_no= item.mvc_petitioner_mobile_no,
                                   petitioner_Address= item.mvc_petitioner_addres,
                                   pincode_Of_Petitioner=item.mvc_petitioner_pincode_no

                               }).Distinct().ToList();
            return vehicleDetails;
        }
        public List<GetVehicleChassisPolicyDetails> GetMVCRespondantDetailsDLL(long Appno)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_application_details
                               join item in _db.tbl_mvc_respondent_details on data.mvc_claim_app_id equals item.mvc_claim_app_id
                               where data.mvc_claim_app_id == Appno

                              select new GetVehicleChassisPolicyDetails
                               {
                                   Respondant_name= item.mvcrd_respondent_name,
                                   Respondant_mobile= (item.mvcrd_respondent_mobile_no).ToString(),
                                   Respondant_address= item.mvcrd_respondent_addres,
                                   Respondant_pincode=(item.mvcrd_pincode_no).ToString(),
                                   Respondant_designation =item.mvcrd_designation_name,
                                   Respondant_department= item.mvcrd_department_name,
                                   Respondant_Agency_name= item.mvcrd_agency_name
                               }).Distinct().ToList();
            return vehicleDetails;
        }
        public List<GetVehicleChassisPolicyDetails> GetMVCdetailsofCourtDLL(long App_id,int category)
            {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_application_details
                              join item in _db.tbl_district_master on data.court_district equals item.dm_id
                              join acc_dist in _db.tbl_taluka_master on data.court_taluk equals acc_dist.tm_id
                              join injury in _db.tbl_mvc_claim_type_of_injury on data.acdnt_type_of_injury equals injury.injury_type_id
                              where data.mvc_claim_app_id == App_id
                              select new GetVehicleChassisPolicyDetails
                              {
                                  Court_DateTime=data.date_of_petition,
                                  Court_MVC_number = data.mvc_no,
                                  Name_of_court= data.name_of_court,
                                  Court_District_Name= item.dm_name_english,
                                  Court_Taluk_Name = acc_dist.tm_englishname,
                                  MVC_number= (data.mvc_no).ToString(),
                                
                                  Accident_details= data.accident_details,
                                  claim_Amount= (data.claim_amount).ToString(),
                                  MVC_claim_app_id= data.mvc_claim_app_id,
                                  Name_of_injured=data.acdnt_name_of_injured_person,
                                  Father_name=data.acdnt_person_father_name,
                                  Spouse_name= data.acdnt_person_spouse_name,
                                  occupation_of_injured= data.acdnt_person_occupation,
                                  Age_of_injured=data.acdnt_person_age,
                                  Address_of_dead_details=data.acdnt_person_full_address,
                                  monthly_income_of_injured = (data.acdnt_person_monthly_income).ToString(),
                                  income_tax_of_injured = (data.acdnt_comp_claimed_tax).ToString(),
                                  employer_deceased_details = data.acdnt_emp_nameadress_deceased,
                                  place_of_accident = data.acdnt_place_accident,
                                  accident_DateTime = (data.acdnt_date_time_of_accident).ToString(),
                                  police_station_of_jurisdiction = data.acdnt_police_station_details,
                                   police_station_of_compensation= data.acdnt_compens_claimed_travelling,
                                   injury_desc= injury.injury_type_desc,
                                   nature_of_injuries_sustained= data.acdnt_nature_of_injury,
                                   medical_officer= data.acdnt_medical_officer_detail,
                                   Period_of_treatment_of_details= data.acdnt_period_treatment_expend,
                                   Name_of_injury_caused_of_details= data.acdnt_name_of_injury_caused,
                                   Name_and_address_of_applicant_details= data.acdnt_applicant_details,
                                   relation_with_deceased = data.acdnt_relation_details,
                                   title_property_deceased= data.acdnt_title_to_property,
                                   any_other_information_details =data.acdt_any_other_info,
                                   stateID= data.state_id,
                                   court_parawise= data.court_parawise_remarks

                              }).Distinct().ToList();

            if (vehicleDetails.Count() == 0)
            {
                vehicleDetails = (from data in _db.tbl_mvc_application_details
                                  
                                  join injury in _db.tbl_mvc_claim_type_of_injury on data.acdnt_type_of_injury equals injury.injury_type_id
                                  where data.mvc_claim_app_id == App_id
                                  select new GetVehicleChassisPolicyDetails
                                  {
                                      Court_DateTime = data.date_of_petition,
                                      Court_MVC_number = data.mvc_no,
                                      Name_of_court = data.name_of_court,
                                      
                                      MVC_number = (data.mvc_claim_app_id).ToString(),

                                      Accident_details = data.accident_details,
                                      claim_Amount = (data.claim_amount).ToString(),
                                      MVC_claim_app_id = data.mvc_claim_app_id,
                                      Name_of_injured = data.acdnt_name_of_injured_person,
                                      Father_name = data.acdnt_person_father_name,
                                      Spouse_name = data.acdnt_person_spouse_name,
                                      occupation_of_injured = data.acdnt_person_occupation,
                                      Age_of_injured = data.acdnt_person_age,
                                      Address_of_dead_details = data.acdnt_person_full_address,
                                      monthly_income_of_injured = (data.acdnt_person_monthly_income).ToString(),
                                      income_tax_of_injured = (data.acdnt_comp_claimed_tax).ToString(),
                                      employer_deceased_details = data.acdnt_emp_nameadress_deceased,
                                      place_of_accident = data.acdnt_place_accident,
                                      accident_DateTime = (data.acdnt_date_time_of_accident).ToString(),
                                      police_station_of_jurisdiction = data.acdnt_police_station_details,
                                      police_station_of_compensation = data.acdnt_compens_claimed_travelling,
                                      injury_desc = injury.injury_type_desc,
                                      nature_of_injuries_sustained = data.acdnt_nature_of_injury,
                                      medical_officer = data.acdnt_medical_officer_detail,
                                      Period_of_treatment_of_details = data.acdnt_period_treatment_expend,
                                      Name_of_injury_caused_of_details = data.acdnt_name_of_injury_caused,
                                      Name_and_address_of_applicant_details = data.acdnt_applicant_details,
                                      relation_with_deceased = data.acdnt_relation_details,
                                      title_property_deceased = data.acdnt_title_to_property,
                                      any_other_information_details = data.acdt_any_other_info,
                                      stateID = data.state_id,

                                      other_state_court_dist=data.other_state_court_dist,
                                      other_state_court_taluk= data.other_state_court_taluk,
                                      court_parawise = data.court_parawise_remarks

                                  }).Distinct().ToList();

            }

            for (int i = 0; i < vehicleDetails.Count; i++)
            {
                if (vehicleDetails[i].stateID != 29)
                {
                    var ById = vehicleDetails[i].stateID;
                    var stateName = (
                                from iti in _db.tbl_mvc_application_details
                                join state1 in _db.tbl_State_master on iti.state_id equals state1.StateId
                                where iti.state_id == ById
                                select new
                                {
                                    StateName = state1.State

                                }).FirstOrDefault();
                    vehicleDetails[i].Court_state_name = stateName.StateName;
                    long scruStat = GetScrutinyStatus(category, App_id);
                    vehicleDetails[i].scrutinyStatus = scruStat;
                }
                else {

                    var ById = vehicleDetails[i].stateID;
                    var UserName = (
                                    from iti in _db.tbl_mvc_application_details
                                    join item in _db.tbl_district_master on iti.court_district equals item.dm_id
                                    join acc_dist in _db.tbl_taluka_master on iti.court_taluk equals acc_dist.tm_id
                                    where iti.state_id == ById
                                    select new
                                    {
                                        DivisionName = item.dm_name_english,
                                        TlukName = acc_dist.tm_englishname,

                                    }).FirstOrDefault();
                    vehicleDetails[i].other_state_court_dist = UserName.DivisionName;
                    vehicleDetails[i].other_state_court_taluk = UserName.TlukName;
                    var stateName = (
                                 from iti in _db.tbl_mvc_application_details
                                 join state1 in _db.tbl_State_master on iti.state_id equals state1.StateId
                                 where iti.state_id == ById
                                 select new
                                 {
                                     StateName = state1.State

                                 }).FirstOrDefault();
                    vehicleDetails[i].Court_state_name = stateName.StateName;

                    long scruStat = GetScrutinyStatus(category, App_id);
                    vehicleDetails[i].scrutinyStatus = scruStat;
                }
            }
           
             

            return vehicleDetails;
        }
        public List<GetVehicleChassisPolicyDetails> GetMVCDocdetailDLL(long Appno)
        {
            
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_application_details
                              join item in _db.tbl_mvc_claim_doc_details on data.mvc_claim_app_id equals item.mvcdd_claim_app_id
                              where item.mvcdd_claim_app_id == Appno

                              select new GetVehicleChassisPolicyDetails
                              {
                                  Accident_details = item.mvcdd_doc_upload_path,
                                  Doc_ref_id = item.mvc_claim_dd_id,

                              }
                              ).ToList();
            return vehicleDetails;
        }

        public List<GetVehicleChassisPolicyDetails> GetMVCDocfileForSignDLL(long docID)
        {

            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_application_details
                              join item in _db.tbl_mvc_claim_doc_details on data.mvc_claim_app_id equals item.mvcdd_claim_app_id
                              where item.mvc_claim_dd_id == docID

                              select new GetVehicleChassisPolicyDetails
                              {
                                  Accident_details = item.mvcdd_doc_upload_path,
                                  Doc_ref_id = item.mvc_claim_dd_id,
                                  MVC_claim_app_id = data.mvc_claim_app_id

                              }
                              ).ToList();
            return vehicleDetails;
        }
        public List<GetVehicleChassisPolicyDetails> GetOtherDocdetailDLL(long Appno)
        {
            List<GetVehicleChassisPolicyDetails> OtherDocumentsData = new List<GetVehicleChassisPolicyDetails>();
            OtherDocumentsData = (from data1 in _db.tbl_mvc_application_details
                              join item1 in _db.tbl_mvc_claim_doc_details on data1.mvc_claim_app_id equals item1.mvcdd_claim_app_id
                              where data1.mvc_claim_app_id == Appno && item1.mvcdd_doc_upload_path.Contains("/OtherDocuments/")
                              select new GetVehicleChassisPolicyDetails
                              {
                                  OtherDocument = item1.mvcdd_doc_upload_path
                              }).ToList();
            return OtherDocumentsData;
        }
        public SelectList GetInjuryListDLL()
        {
            List<SelectListItem> RemarkList = new List<SelectListItem>();

            RemarkList = (from inj in _db.tbl_mvc_claim_type_of_injury

                          select new SelectListItem
                          {
                              Text = inj.injury_type_desc,
                              Value = (inj.injury_type_id).ToString()
                          }).ToList();
           
            return new SelectList(RemarkList, "Value", "Text");

        }
        public SelectList GetstateListDLL()
        {
            List<SelectListItem> RemarkList = new List<SelectListItem>();

            RemarkList = (from state in _db.tbl_State_master

                          select new SelectListItem
                          {
                              Text = state.State,
                              Value = (state.StateId).ToString()
                          }).ToList();
           
            return new SelectList(RemarkList, "Value", "Text");

        }

        public List<GetVehicleChassisPolicyDetails> MvcClaimWorkFlowDetailsDLL(long appid, string chassis)
        {
           List<GetVehicleChassisPolicyDetails> workFlowDetails = new List<GetVehicleChassisPolicyDetails>();

            workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                               join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                               join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                               join remarks in _db.tbl_remarks_master on work.micw_remarks equals remarks.RM_Remarks_id


                              where work.mvc_claim_app_id == appid 
                               select new GetVehicleChassisPolicyDetails
                              {
                                  SubmissionDate = work.micw_creation_datetime,
                                  ByID = work.micw_verified_by,
                                  TO = work.micw_assigned_to,
                                  Remarks = remarks.RM_Remarks_Desc,
                                  comments = work.micw_comments

                              }).OrderByDescending(x => x.SubmissionDate).ToList();

            for (int i = 0; i < workFlowDetails.Count; i++)
            {
                var ById = workFlowDetails[i].ByID;
                int toID = Convert.ToInt32(workFlowDetails[i].TO);
                var UserName = (from j in _db.tbl_category_master
                                //join e in _db.tbl_employee_basic_details on j.cm_category_id equals e.user_category_id
                                where j.cm_category_id == ById
                                select j.cm_category_desc).FirstOrDefault();
                //workFlowDetails[i].From = UserName;
                workFlowDetails[i].From = UserName;
                //workFlowDetails[i].TO = UserName;

                var TO = (from j in _db.tbl_category_master
                                    //join e in _db.tbl_employee_basic_details on j.cm_category_id equals e.user_category_id
                                where j.cm_category_id == toID
                          select j.cm_category_desc).FirstOrDefault();
                workFlowDetails[i].Tooo = TO.ToString(); //TO.ToString();
            }

            return workFlowDetails;
        }
		  public long SaveAsDraftMvcDetailsDLL(GetVehicleChassisPolicyDetails model)
        {
            try
            {

                tbl_mvc_application_details tbl_data = new tbl_mvc_application_details();
                string App_no = DateTime.Now.ToString("yyyy-MM-dd HH:ffff").Replace("-", "").Replace(" ", "").Replace(":", "").Replace(".", "");
                model.MVC_claim_app_id = Convert.ToInt64(App_no);
                long Application_no = Convert.ToInt64(App_no);
                tbl_data.mvc_claim_app_id = Application_no;
                tbl_data.chassis_no = model.vehicle_chasis_no;
                tbl_data.policy_no = model.Policy_number;
                tbl_data.mvc_no = (((model.MVC_number).ToString() != null) ? ((model.MVC_number).ToString()) : " ");
                tbl_data.date_of_petition =  ((Convert.ToDateTime(model.CourtTime) != null) ? Convert.ToDateTime(model.CourtTime) :Convert.ToDateTime(DateTime.Now));
                tbl_data.name_of_court = ((model.Name_of_court != null) ? model.Name_of_court : "");
                if (model.stateID == 29)
                {
              tbl_data.court_district = ((model.District_dm_id != 0) ? model.District_dm_id : 0);
                tbl_data.court_taluk = ((model.Taluk_id != 0) ? model.Taluk_id : 0);
                tbl_data.other_state_court_taluk = "";
                    tbl_data.other_state_court_dist = "";
			    }
                else
                {
                    tbl_data.court_district = 0;
                    tbl_data.court_taluk = 0;
                    tbl_data.other_state_court_taluk = ((model.other_state_court_taluk != null) ? model.other_state_court_taluk : "");
                    tbl_data.other_state_court_dist = ((model.other_state_court_dist != null) ? model.other_state_court_dist : "");
                }
				
			
                tbl_data.name_of_petitioner = "";
                tbl_data.petitioner_pincode = 11;
                tbl_data.petitioner_mobile_no = 99;
                tbl_data.respondant_name = "";
                tbl_data.respondant_designation = "";
                tbl_data.respondant_department = "";
                tbl_data.respondant_agencyName = "";
                tbl_data.respondant_address = "";
                tbl_data.respondant_mobile_no = 90;
                tbl_data.respondant_pincode = 90;
                tbl_data.accident_district = 0;
                tbl_data.accident_taluk = 0;
                tbl_data.accident_hobli = "";
                tbl_data.accident_grampanchayat = "";
                tbl_data.accident_village = "";
                tbl_data.accident_details = "";
                tbl_data.claim_amount = ((Convert.ToDecimal(model.claim_Amount) != 0) ? (Convert.ToDecimal(model.claim_Amount)) : 0);
                tbl_data.rc_details = "";
                tbl_data.dl_details = "";
                tbl_data.fir_details = "";
                tbl_data.panchanama_details = "";
                tbl_data.remarks = ((model.Remarks_id != 0) ? model.Remarks_id : 0);
                tbl_data.status_id = true;
                tbl_data.verified_document = true;
                tbl_data.created_by = model.loginId;
                tbl_data.updated_by = 0;
                tbl_data.mvc_claim_creation_datetime = DateTime.Now;
                tbl_data.mvc_claim_updation_datetime = DateTime.Now;
                tbl_data.acdnt_name_of_injured_person = ((model.Name_of_injured != null) ? model.Name_of_injured : "");
                tbl_data.acdnt_person_father_name = ((model.Father_name != null) ? model.Father_name : "");
                tbl_data.acdnt_person_spouse_name = ((model.Spouse_name != null) ? model.Spouse_name : "");
                tbl_data.acdnt_person_full_address = ((model.Address_of_dead_details != null) ? model.Address_of_dead_details : "");
                tbl_data.acdnt_person_age = ((model.Age_of_injured != 0) ? model.Age_of_injured : 0);
                tbl_data.acdnt_person_occupation = ((model.occupation_of_injured != null) ? model.occupation_of_injured : "");
                tbl_data.acdnt_emp_nameadress_deceased = ((model.employer_deceased_details != null) ? model.employer_deceased_details : "");
                tbl_data.acdnt_person_monthly_income = ((Convert.ToDecimal(model.monthly_income_of_injured) != 0) ? (Convert.ToDecimal(model.monthly_income_of_injured)) : 0);
                tbl_data.acdnt_comp_claimed_tax = ((Convert.ToDecimal(model.income_tax_of_injured) != 0) ? (Convert.ToDecimal(model.income_tax_of_injured)) : 0);
                tbl_data.acdnt_place_accident = ((model.place_of_accident != null) ? model.place_of_accident : "");
                tbl_data.acdnt_date_time_of_accident = ((Convert.ToDateTime(model.accident_DateTime) != null) ? Convert.ToDateTime(model.accident_DateTime) : Convert.ToDateTime(DateTime.Now));
                tbl_data.acdnt_police_station_details = ((model.police_station_of_jurisdiction != null) ? model.police_station_of_jurisdiction : "");
                tbl_data.acdnt_compens_claimed_travelling = ((model.police_station_of_compensation != null) ? model.police_station_of_compensation : "");
                tbl_data.acdnt_type_of_injury = ((model.type_injury != 0) ? model.type_injury : 0);
                tbl_data.acdnt_nature_of_injury = ((model.nature_of_injuries_sustained != null) ? model.nature_of_injuries_sustained : "");
                tbl_data.acdnt_medical_officer_detail = ((model.medical_officer != null) ? model.medical_officer : "");
                tbl_data.acdnt_period_treatment_expend = ((model.Period_of_treatment_of_details != null) ? model.Period_of_treatment_of_details : "");
                tbl_data.acdnt_name_of_injury_caused = ((model.Name_of_injury_caused_of_details != null) ? model.Name_of_injury_caused_of_details : "");
                tbl_data.acdnt_applicant_details = ((model.Name_and_address_of_applicant_details != null) ? model.Name_and_address_of_applicant_details : "");
                tbl_data.acdnt_relation_details = ((model.relation_with_deceased != null) ? model.relation_with_deceased : "");
                tbl_data.acdnt_title_to_property = ((model.title_property_deceased != null) ? model.title_property_deceased : "");
                tbl_data.acdt_any_other_info = ((model.any_other_information_details != null) ? model.any_other_information_details : "");
                tbl_data.state_id = ((model.stateID != 0) ? model.stateID : 0);
                tbl_data.app_saved_status = model.application_stat;
                // app_saved_status 1 for draft
                //                  2 for send to sup
                _db.tbl_mvc_application_details.Add(tbl_data);
                int abc = _db.SaveChanges();
                if (abc == 1)
                {
                    int c = PetitionerRespondantDetailsDLL(model.MVC_claim_app_id, model);
                }

                if (model.application_stat == 2)
                {
                    var returnMessage = UpdateWork_flow_Details(model);
                }

                //foreach (var files in )
                //{
                //}



            }
            catch (Exception ex)
            {

                throw ex;
            }
            return model.MVC_claim_app_id;
        }

        public int savePathOfFile(string path, long App_id) {

            string[] newNV = path.Split('/');
            string pathConstant = newNV[5];
            string otherPath = newNV[6];

            
           tbl_mvc_claim_doc_details pathData = _db.tbl_mvc_claim_doc_details.Where(x => x.mvcdd_claim_app_id == App_id && x.mvcdd_doc_upload_path.Contains(pathConstant)).FirstOrDefault();
            if (pathData == null)
            {
                tbl_mvc_claim_doc_details mvc_doc = new tbl_mvc_claim_doc_details();
                mvc_doc.mvcdd_claim_app_id = App_id;
                mvc_doc.mvcdd_claim_due_id = 0;
                mvc_doc.mvcdd_doc_upload_path = path;
                mvc_doc.mvcdd_active_status = true;
                mvc_doc.mvcdd_creation_datetime = DateTime.Now;
                mvc_doc.mvcdd_updation_datetime = DateTime.Now;
                mvc_doc.mvcdd_updated_by = 1;

                _db.tbl_mvc_claim_doc_details.Add(mvc_doc);
                int abc = _db.SaveChanges();
                return 1;
            }
            else {
                pathData.mvcdd_claim_due_id = 0;
                pathData.mvcdd_doc_upload_path = path;
                pathData.mvcdd_active_status = true;
                pathData.mvcdd_creation_datetime = DateTime.Now;
                pathData.mvcdd_updation_datetime = DateTime.Now;
                pathData.mvcdd_updated_by = 1;
                _db.SaveChanges();

            }
            return 1;
        }
        public long GetScrutinyStatus(int category,long app_id) {
            var scrut = (from mvc in _db.tbl_mvc_application_details
                         join app in _db.tbl_mvc_claim_workflow on mvc.mvc_claim_app_id equals app.mvc_claim_app_id
                         where app.mvc_claim_app_id == app_id && app.micw_active_status == true
                         select app.micw_assigned_to).FirstOrDefault();
            return scrut;
        }
        public int stopMVCFlowOnLokadhalatSelectDLL(long Appid) {
            List<GetVehicleChassisPolicyDetails> mvc_details = new List<GetVehicleChassisPolicyDetails>();
            int result = 0;
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == Appid select n).FirstOrDefault();
            if (mvc_tbl != null) {
                mvc_tbl.app_saved_status = 3;
                mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
              result = _db.SaveChanges();
            }
          
            return result;
        } public int stopLokadhalatFlowOnSelectDLL(long Appid) {
            List<GetVehicleChassisPolicyDetails> mvc_details = new List<GetVehicleChassisPolicyDetails>();
            int result = 0;
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == Appid select n).FirstOrDefault();
            if (mvc_tbl != null) {
                mvc_tbl.app_saved_status = 2;
                mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
              result = _db.SaveChanges();
            }
          
            return result;
        }
        public int submitParawiseRemarksDLL(GetVehicleChassisPolicyDetails model) {
            List<GetVehicleChassisPolicyDetails> mvc_details = new List<GetVehicleChassisPolicyDetails>();
            int result = 0;
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id ==model.MVC_claim_app_id select n).FirstOrDefault();
            if (mvc_tbl != null) {
                mvc_tbl.court_parawise_remarks = model.court_parawise;
                mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
              result = _db.SaveChanges();
            }
           
          
            return result;
        }

        public string MVCSignedDocUploadDLL(long docID, long appId, string DocPath)
        {

            try
            {
                tbl_mvc_claim_doc_details pathData = _db.tbl_mvc_claim_doc_details.Where(x => x.mvcdd_claim_app_id == appId && x.mvc_claim_dd_id == docID).FirstOrDefault();
                if (pathData != null)
                {
                    tbl_mvc_claim_doc_details mvc_doc = new tbl_mvc_claim_doc_details();
                    mvc_doc.mvcdd_claim_app_id = appId;
                    mvc_doc.mvcdd_claim_due_id = 0;
                    mvc_doc.mvcdd_doc_upload_path = DocPath;
                    mvc_doc.mvcdd_active_status = true;
                    mvc_doc.mvcdd_creation_datetime = DateTime.Now;
                    mvc_doc.mvcdd_updation_datetime = DateTime.Now;
                    mvc_doc.mvcdd_updated_by = 1;

                    _db.tbl_mvc_claim_doc_details.Add(mvc_doc);
                    _db.SaveChanges();

                }

                
            }

            catch(Exception ex)
            {

            }
            return "success";
        }

        public List<GetVehicleChassisPolicyDetails> GetSignedDocDLL(long id)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_application_details
                              join item in _db.tbl_mvc_claim_doc_details on data.mvc_claim_app_id equals item.mvcdd_claim_app_id
                              where item.mvcdd_claim_app_id == id && item.mvcdd_signed_status == 1

                              select new GetVehicleChassisPolicyDetails
                              {
                                  Accident_details = item.mvcdd_doc_upload_path,
                                  Doc_ref_id = item.mvc_claim_dd_id,
                                  MVC_claim_app_id = data.mvc_claim_app_id

                              }
                              ).ToList();
            return vehicleDetails;
        }
        #endregion
    }
}
