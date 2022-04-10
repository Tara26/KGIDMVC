using DLL.DBConnection;
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

using static KGID_Models.KGIDMotorInsurance.tbl_vehicle_category_master;
using static KGID_Models.KGIDMotorInsurance.tbl_vehicle_type_master;

namespace DLL.MBClaimsDLL
{
    public class MBClaimsDLL : IMBClaimsDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();

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
        public GetVehicleChassisPolicyDetails GetVehicleAndPolicyDetailsDLL(string txtDetails)
        {
            GetVehicleChassisPolicyDetails vehicleDetails = new GetVehicleChassisPolicyDetails();

            vehicleDetails = (from MIA in _db.tbl_motor_insurance_application
                              join item in _db.tbl_motor_insurance_vehicle_details on MIA.mia_motor_insurance_app_id equals item.mivd_application_id
                              join MPD in _db.mi_policy_details on MIA.mia_motor_insurance_app_id equals MPD.p_mi_application_id
                           
                              where item.mivd_registration_no == txtDetails || item.mivd_chasis_no == txtDetails || MPD.p_mi_policy_number == txtDetails
                              select new GetVehicleChassisPolicyDetails
                              {
                                  vehicle_registration_no = item.mivd_registration_no,
                                  vehicle_chasis_no = item.mivd_chasis_no,
                                  
                                  vehicle_model = (item.mivd_type_of_model).ToString(),
                                  Policy_number = MPD.p_mi_policy_number,
                                  OD_from_date = MPD.p_mi_from_date,
                                  OD_to_date = MPD.p_mi_to_date,
                                  TP_from_date = MPD.p_mi_tpfrom_date,
                                  TP_to_date = MPD.p_mi_tpto_date,
                                  owner_name_vehicle = MIA.mia_owner_of_the_vehicle,
                                  cubic_capacity_vehicle = item.mivd_cubic_capacity,
                                  owner_name_vehicle_address = MIA.mia_address,
                                  seating_capacity_vehicle = item.mivd_seating_capacity_including_driver,
                              
                              }).FirstOrDefault();
            if (vehicleDetails.Policy_number != null)
            {

                vehicleDetails = (from MIA in _db.tbl_motor_insurance_application
                                  join item in _db.tbl_motor_insurance_vehicle_details on MIA.mia_motor_insurance_app_id equals item.mivd_application_id
                                  join MPD in _db.mi_policy_details on MIA.mia_motor_insurance_app_id equals MPD.p_mi_application_id
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
                                      vehicle_model = (item.mivd_type_of_model).ToString(),
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
                                  }).FirstOrDefault();
            }
            else
            {

            }
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

            RemarkList = (from remark in _db.tbl_mvc_claim_remarks
                          where remark.moduleType==1
                          select new SelectListItem
                          {
                              Text = remark.remark_desc,
                              Value = (remark.remark_id).ToString()
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
            int abc = 0,c;
            using (var transaction = _db.Database.BeginTransaction())
            {
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
                         c = PetitionerRespondantDetailsDLL(model.MVC_claim_app_id, model);
                    }
                    

                var returnMessage = UpdateWork_flow_Details(model);


                //foreach (var files in )
                //{
                //}

                    // return model.MVC_claim_app_id;
                    transaction.Commit();

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
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
                    work_flow.mvc_main_flow = 1;
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
                    else
                    {

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
                        else
                        {
                            foreach (var data in mvc_petitioner)
                            {

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
                        else
                        {
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
                              where data.app_saved_status == 2 || data.app_saved_status == 3 || data.app_saved_status == 4
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

        public GetVehicleChassisPolicyDetails GetMVCGetDetailsOnChassisDLL(string ChassisNo)
        {
            GetVehicleChassisPolicyDetails vehicleDetails = new GetVehicleChassisPolicyDetails();

            vehicleDetails = (from MIA in _db.tbl_motor_insurance_application
                              join item in _db.tbl_motor_insurance_vehicle_details on MIA.mia_motor_insurance_app_id equals item.mivd_application_id
                              join MPD in _db.mi_policy_details on MIA.mia_motor_insurance_app_id equals MPD.p_mi_application_id
                              join VPC in _db.tbl_vehicle_type_master on item.mivd_vehicle_type equals VPC.vht_vehicle_type_id
                              join VCTM in _db.tbl_vehicle_category_type_master on item.mivd_cat_type_id equals VCTM.vct_vehicle_category_type_id
                              join VCTMaster in _db.tbl_vehicle_category_master on item.mivd_vehicle_category equals VCTMaster.vc_vehicle_category_id
                              join Cover in _db.tbl_motor_insurance_type_of_cover on MIA.mia_type_of_cover equals Cover.mitoc_type_cover_id
                              join VSCM in _db.tbl_vehicle_subtype_master on item.mivd_vehicle_subtype equals VSCM.vst_vehicle_subtype_id
                              join VMM in _db.tbl_vehicle_make_master on item.mivd_make_of_vehicle equals VMM.vm_vehicle_make_id
                              where item.mivd_chasis_no == ChassisNo
                              select new GetVehicleChassisPolicyDetails
                              {
                                  vehicle_registration_no = item.mivd_registration_no,
                                  vehicle_chasis_no = item.mivd_chasis_no,
                                  Vehicle_Category_Type = VCTM.vct_vehicle_category_type_desc,
                                  vehicle_category_desc = VCTMaster.vc_vehicle_category_desc,
                                  vehicle_model = item.mivd_type_of_model.ToString(),
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
                              }).Distinct().FirstOrDefault();

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
                                   court_parawise= data.court_parawise_remarks,
                                   CourtTime2 = (data.Ratification_hearingDate).ToString(),
                                  CourtTime3 = (data.Ratification_hearingNextHearingDate).ToString(),
                                  Comments_details=data.Ratification_hearingComments,
                                  OpinionId= data.opinionStatusLowerCourt??0,
                                  LowerCourtJudgementDate = (data.lowerCourtJudgementDate).ToString(),
                                  awardedAmntLowCourt = (data.awardedAmount_lowerCourt).ToString(),
                                  claim_settle_awardedAmt = (data.mvc_claimSettleAwardedAmount).ToString(),
                                  Claim_settle_awardedInterest = (data.mvc_claimSettleAwardedInterest).ToString(),
                                  claim_petitionDate = (data.mvc_claimSettlePetitionDate).ToString(),
                                  Claim_settle_courtcost = (data.mvc_claimSettlecourtCost).ToString(),
                                  Claim_settle_AwardedTotalAmnt = (data.mvc_claimSettleAwardedTotalAmount).ToString(),
                                  type_injury = data.acdnt_type_of_injury,
                                  DelayNoteHighCourt = data.mvc_InputForDelayNoteHighCourt,
                                  AmtDepositHC = (data.mvc_amntDepoHighCourt.ToString()),
                                  AmtDepositLC = (data.mvc_amntDepoLowCourt.ToString()),
                                  HighCourtOpinionID = data.opinionStatusHighCourt,
                                  HighCourtjudgementDate = (data.HighCourtJudgementDate).ToString(),
                                  HighCourtAwardedAmount = (data.awardedAmount_highCourt).ToString(),
                                  HighCourstatutoryAmount = (data.HighCourt_Statuatory_Amount).ToString(),
                                  HighCourtstatementRemittedDate = (data.HighCourt_statAmnt_RemittedDate).ToString(),
                                  HighCourtDepositAmnt = (data.HighCourtDeposit_Amount).ToString(),
                                  HighCourtDepositAmntRemittedDate = (data.HighCourt_depositAmnt_RemittedDate).ToString(),
                                  HighCourtClaimAwardedAmnt = (data.HighCourtClaimAwarded_Amount).ToString(),
                                  HighCourtClaimAwardedInterest = (data.HighCourtClaimAwarded_Interest).ToString(),
                                  HighCourtClaimSettleCost = (data.HighCourtClaimSettle_cost).ToString(),
                                  HighCourtClaimSettleTotalAmnt = (data.HighCourtClaimSettle_TotalAmnt).ToString(),
                                  OpinionId2 = data.opinionStatusHighCourtKGID,
                                  HighCourtJudgementDateKGID = (data.HighCourtJudgementDateKGID).ToString(),
                                  HighCourtAwardedAmountKGID = (data.awardedAmount_highCourtKGID).ToString(),
                                  awardedAmount_highCourtClaimSttleKGID = (data.awardedAmount_highCourtClaimSttleKGID).ToString(),
                                  awardedInterest_highCourtClaimSttleKGID = (data.awardedInterest_highCourtClaimSttleKGID).ToString(),
                                  courtCost_highCourtClaimSttleKGID = (data.courtCost_highCourtClaimSttleKGID).ToString(),
                                  totalAmnt_highCourtClaimSttleKGID = (data.totalAmnt_highCourtClaimSttleKGID).ToString(),
                                  InputDelaySupremeCourt = data.inputDelaysupremeCourt,
                                  SupremeJudgementOpiniondate = (data.Supreme_judgement_Date).ToString(),
                                  SupremeAwardedAmnt = (data.SupremeopinionAwardAmnt).ToString(),
                                  SupremeOpinionId = data.OpinionSupremeCourt,
                                  Supreme_Statuatory_Amount = (data.Supreme_Statuatory_Amount).ToString(),
                                  Statuatory_Amount_Remitted = (data.Statuatory_Amount_Remitted).ToString(),
                                  Supreme_DepositAmount = (data.Supreme_DepositAmount).ToString(),
                                  Supreme_Deposit_Amount_Remitted = (data.Supreme_Deposit_Amount_Remitted).ToString(),
                                  Supreme_Awarded_Amount = (data.Supreme_Awarded_Amount).ToString(),
                                  Supreme_Awarded_Interest = (data.Supreme_Awarded_Interest).ToString(),
                                  Supreme_Court_Cost = (data.Supreme_Court_Cost).ToString(),
                                  Supreme_Total_Amount = (data.Supreme_Total_Amount).ToString(),
                                  statutorypaidStatus = data.statutorypaidStatus,
                                  mvc_JudgementSupreme_date2 = (data.mvc_JudgementSupreme_date2).ToString(),
                                  mvc_opinionSupremeStatusID2 = data.mvc_opinionSupremeStatusID2,
                                  mvc_awardedSupreme_amount2 = (data.mvc_awardedSupreme_amount2).ToString(),
                                  awardedAmount_supremeCourtKGID = (data.awardedAmount_supremeCourtKGID).ToString(),
                                  awardedInterest_supremeCourtKGID = (data.awardedInterest_supremeCourtKGID).ToString(),
                                  courtcost_supremeCourtKGID = (data.courtcost_supremeCourtKGID).ToString(),
                                  TotalAmount_supremeCourtKGID = (data.TotalAmount_supremeCourtKGID).ToString(),
                                  dist_id12 = item.dm_code,
                                  Taluk_id12= acc_dist.tm_code,
                                  application_stat = data.app_saved_status
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
                                      
                                      MVC_number = (data.mvc_no).ToString(),

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
                                      type_injury = data.acdnt_type_of_injury,
                                      other_state_court_dist=data.other_state_court_dist,
                                      other_state_court_taluk= data.other_state_court_taluk,
                                      court_parawise = data.court_parawise_remarks,
                                      CourtTime2 = (data.Ratification_hearingDate).ToString(),
                                      CourtTime3 = (data.Ratification_hearingNextHearingDate).ToString(),
                                      Comments_details = data.Ratification_hearingComments,
                                      OpinionId = data.opinionStatusLowerCourt ?? 0,
                                      LowerCourtJudgementDate = (data.lowerCourtJudgementDate).ToString(),
                                      awardedAmntLowCourt = (data.awardedAmount_lowerCourt).ToString(),
                                      claim_settle_awardedAmt = (data.mvc_claimSettleAwardedAmount).ToString(),
                                      Claim_settle_awardedInterest = (data.mvc_claimSettleAwardedInterest).ToString(),
                                      claim_petitionDate = (data.mvc_claimSettlePetitionDate).ToString(),
                                      Claim_settle_courtcost = (data.mvc_claimSettlecourtCost).ToString(),
                                      Claim_settle_AwardedTotalAmnt = (data.mvc_claimSettleAwardedTotalAmount).ToString(),
                                      DelayNoteHighCourt = data.mvc_InputForDelayNoteHighCourt,
                                      AmtDepositHC = (data.mvc_amntDepoHighCourt.ToString()),
                                      AmtDepositLC = (data.mvc_amntDepoLowCourt.ToString()),
                                      HighCourtOpinionID = data.opinionStatusHighCourt,
                                      HighCourtjudgementDate = (data.HighCourtJudgementDate).ToString(),
                                      HighCourtAwardedAmount = (data.awardedAmount_highCourt).ToString(),
                                      HighCourstatutoryAmount = (data.HighCourt_Statuatory_Amount).ToString(),
                                      HighCourtstatementRemittedDate = (data.HighCourt_statAmnt_RemittedDate).ToString(),
                                      HighCourtDepositAmnt = (data.HighCourtDeposit_Amount).ToString(),
                                      HighCourtDepositAmntRemittedDate = (data.HighCourt_depositAmnt_RemittedDate).ToString(),
                                      HighCourtClaimAwardedAmnt = (data.HighCourtClaimAwarded_Amount).ToString(),
                                      HighCourtClaimAwardedInterest = (data.HighCourtClaimAwarded_Interest).ToString(),
                                      HighCourtClaimSettleCost = (data.HighCourtClaimSettle_cost).ToString(),
                                      HighCourtClaimSettleTotalAmnt = (data.HighCourtClaimSettle_TotalAmnt).ToString(),
                                      OpinionId2 = data.opinionStatusHighCourtKGID,
                                      HighCourtJudgementDateKGID = (data.HighCourtJudgementDateKGID).ToString(),
                                      HighCourtAwardedAmountKGID = (data.awardedAmount_highCourtKGID).ToString(),
                                      awardedAmount_highCourtClaimSttleKGID = (data.awardedAmount_highCourtClaimSttleKGID).ToString(),
                                      awardedInterest_highCourtClaimSttleKGID = (data.awardedInterest_highCourtClaimSttleKGID).ToString(),
                                      courtCost_highCourtClaimSttleKGID = (data.courtCost_highCourtClaimSttleKGID).ToString(),
                                      totalAmnt_highCourtClaimSttleKGID = (data.totalAmnt_highCourtClaimSttleKGID).ToString(),
                                      InputDelaySupremeCourt = data.inputDelaysupremeCourt,
                                      SupremeJudgementOpiniondate = (data.Supreme_judgement_Date).ToString(),
                                      SupremeAwardedAmnt = (data.SupremeopinionAwardAmnt).ToString(),
                                      SupremeOpinionId = data.OpinionSupremeCourt,
                                      Supreme_Statuatory_Amount = (data.Supreme_Statuatory_Amount).ToString(),
                                      Statuatory_Amount_Remitted = (data.Statuatory_Amount_Remitted).ToString(),
                                      Supreme_DepositAmount = (data.Supreme_DepositAmount).ToString(),
                                      Supreme_Deposit_Amount_Remitted = (data.Supreme_Deposit_Amount_Remitted).ToString(),
                                      Supreme_Awarded_Amount = (data.Supreme_Awarded_Amount).ToString(),
                                      Supreme_Awarded_Interest = (data.Supreme_Awarded_Interest).ToString(),
                                      Supreme_Court_Cost = (data.Supreme_Court_Cost).ToString(),
                                      Supreme_Total_Amount = (data.Supreme_Total_Amount).ToString(),
                                      statutorypaidStatus = data.statutorypaidStatus,
                                      mvc_JudgementSupreme_date2 = (data.mvc_JudgementSupreme_date2).ToString(),
                                      mvc_opinionSupremeStatusID2 = data.mvc_opinionSupremeStatusID2,
                                      mvc_awardedSupreme_amount2 = (data.mvc_awardedSupreme_amount2).ToString(),
                                      awardedAmount_supremeCourtKGID = (data.awardedAmount_supremeCourtKGID).ToString(),
                                      awardedInterest_supremeCourtKGID = (data.awardedInterest_supremeCourtKGID).ToString(),
                                      courtcost_supremeCourtKGID = (data.courtcost_supremeCourtKGID).ToString(),
                                      TotalAmount_supremeCourtKGID = (data.TotalAmount_supremeCourtKGID).ToString(),
                                 application_stat = data.app_saved_status
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
                else
                {

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
                if (vehicleDetails[i].OpinionId == 6)
                {
                    vehicleDetails[i].opinionDesc = "APPEAL";

                }
                else if (vehicleDetails[i].OpinionId == 7)
                {
                    vehicleDetails[i].opinionDesc = "NO APPEAL";
                }
                else
                {
                    vehicleDetails[i].opinionDesc = " ";

                }
                if (vehicleDetails[i].HighCourtOpinionID == 6)
                {
                    vehicleDetails[i].HighCourtOpinionDesc = "APPEAL";

                }
                else if (vehicleDetails[i].HighCourtOpinionID == 7)
                {
                    vehicleDetails[i].HighCourtOpinionDesc = "NO APPEAL";
                }
                else
                {
                    vehicleDetails[i].HighCourtOpinionDesc = " ";

                }
                if (vehicleDetails[i].OpinionId2 == 6)
                {
                    vehicleDetails[i].HighCourtOpinionDesc2 = "APPEAL";

                }
                else if (vehicleDetails[i].OpinionId2 == 7)
                {
                    vehicleDetails[i].HighCourtOpinionDesc2 = "NO APPEAL";
                }
                else
                {
                    vehicleDetails[i].HighCourtOpinionDesc2 = " ";

                }
                if (vehicleDetails[i].SupremeOpinionId == 6)
                {
                    vehicleDetails[i].SupremeOpinionDesc = "APPEAL";

                }
                else if (vehicleDetails[i].SupremeOpinionId == 7)
                {
                    vehicleDetails[i].SupremeOpinionDesc = "NO APPEAL";
                }
                else
                {
                    vehicleDetails[i].SupremeOpinionDesc = " ";

                }
                if (vehicleDetails[i].mvc_opinionSupremeStatusID2 == 6)
                {
                    vehicleDetails[i].mvc_opinionSupremeStatusID2Desc = "APPEAL";

                }
                else if (vehicleDetails[i].mvc_opinionSupremeStatusID2 == 7)
                {
                    vehicleDetails[i].mvc_opinionSupremeStatusID2Desc = "NO APPEAL";
                }
                else
                {
                    vehicleDetails[i].SupremeOpinionDesc = " ";

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
                               join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                              where work.mvc_claim_app_id == appid && work.mvc_main_flow ==1
                               select new GetVehicleChassisPolicyDetails
                              {
                                  SubmissionDate = work.micw_creation_datetime,
                                  ByID = work.micw_verified_by,
                                  TO = work.micw_assigned_to,
                                  Remarks = remarks.remark_desc,
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
            using (var transaction = _db.Database.BeginTransaction())
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


                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw ex;
                }
            }
            return model.MVC_claim_app_id;
        }

        public int savePathOfFile(string path, long App_id)
        {

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
            else
            {
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
        public long GetScrutinyStatus(int category, long app_id)
        {
            var scrut = (from mvc in _db.tbl_mvc_application_details
                         join app in _db.tbl_mvc_claim_workflow on mvc.mvc_claim_app_id equals app.mvc_claim_app_id
                         where app.mvc_claim_app_id == app_id && app.micw_active_status == true
                         select app.micw_assigned_to).FirstOrDefault();
            return scrut;
        }
        public int stopMVCFlowOnLokadhalatSelectDLL(long Appid)
        {
            List<GetVehicleChassisPolicyDetails> mvc_details = new List<GetVehicleChassisPolicyDetails>();
            int result = 0;
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == Appid select n).FirstOrDefault();
            if (mvc_tbl != null)
            {
                mvc_tbl.app_saved_status = 3;
                mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
              result = _db.SaveChanges();
            }
          
            return result;
        }
        public int stopLokadhalatFlowOnSelectDLL(long Appid)
        {
            List<GetVehicleChassisPolicyDetails> mvc_details = new List<GetVehicleChassisPolicyDetails>();
            int result = 0;
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == Appid select n).FirstOrDefault();
            if (mvc_tbl != null)
            {
                mvc_tbl.app_saved_status = 2;
                mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
              result = _db.SaveChanges();
            }
          
            return result;
        }
        public int submitParawiseRemarksDLL(GetVehicleChassisPolicyDetails model)
        {
            List<GetVehicleChassisPolicyDetails> mvc_details = new List<GetVehicleChassisPolicyDetails>();
            int result = 0;
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            if (mvc_tbl != null)
            {
                if (model.court_parawise != null)
                {
                    mvc_tbl.court_parawise_remarks = model.court_parawise;
                    mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
                    result = _db.SaveChanges();
                }
                if (model.Accident_object_statement_details!=null)
                {
                    mvc_tbl.court_parawise_remarks = model.court_parawise;
                    mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
                    result = _db.SaveChanges();

                }
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
        public int UpdateDocumentWork_flow_detailsDLL(GetVehicleChassisPolicyDetails model)
        {
            List<tbl_mvc_claim_workflow> oldFlowData = _db.tbl_mvc_claim_workflow.Where(x => x.mvc_claim_app_id == model.MVC_claim_app_id).ToList();
            int result = 0;
            if (oldFlowData.Count() != 0)
            {
                foreach (var flow in oldFlowData)
                {
                    if(model.DocFileVariable == "ParawiseLawyer")
                    {
                        //flow.mvc_parawiseRemarkLawyer = 1;
                        flow.mvc_parawiseRemarkLawyerStatus = false;
                    }
                    if (model.DocFileVariable == "ObjectStatement")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_objecttionStatementStatus = false;
                    }
                    if (model.DocFileVariable == "RatificationLawDept")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_ratificationLawDeptStatus = false;
                    }
                    if (model.DocFileVariable == "LowerCourtJudgementCopy")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_lower_Court_judgementCopyStatus = false;
                    }
                    if (model.DocFileVariable == "opinionFromLawDept") 
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_OpinionFromLawDepartmentStatus = false;
                    }
                    if (model.DocFileVariable == "ClaimApprovalSettleLowerCourt")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_ClaimApprovalSettleLowerCourtStatus = false;
                    }
                    if (model.DocFileVariable == "DelayNotetoGovtAdvocateHighCourt")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_DraftForDelayNoteHighCourtStatus = false;
                    }
                    if (model.DocFileVariable == "amountDepositToHighCourt")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_amntDepositToHighCourtStatus = false;
                    }
                    if (model.DocFileVariable == "AmountDepostionLetterToLowerCourt")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_amntDepositToLowCourtStatus = false;
                    }
                    if (model.DocFileVariable == "HighCourtJudgementOpinionFormLawDeptDetails")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_HighCourtJudgementOpinionStatus = false;
                    }
                    if (model.DocFileVariable == "HighCourtClaimSettlement")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_claimsettleHighCourtJudgementStatus = false;
                    }
                    if (model.DocFileVariable == "HighCourtNoticePetition")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_HighCourtNoticePetitionStatus = false;
                    }
                    if (model.DocFileVariable == "HighCourtCoveringLetter")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_HighCourtCoveringLetterStatus = false;
                    }
                    if (model.DocFileVariable == "HighCourtJudgementOpinionFormLawDeptDetails2")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_opinionStatusHighCourtKGIDStatus = false;
                    }
                    if (model.DocFileVariable == "HighCourtClaimSettlement2")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_ClaimSettleLowerHighStatus = false;
                    }
                    if (model.DocFileVariable == "DelayNotetoGovtAdvocateSupremeCourt")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_inputDelaysupremeCourtDraftStatus = false;
                    }
                    if (model.DocFileVariable == "AmountDepositiontoSupreme_Court")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_amntDepositToSupremeCStatus = false;
                    }
                    if (model.DocFileVariable == "Amount_Deposition_supremeCourtTOLowercourt")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_amntDepositSupremeCToLCStatus = false;
                    }
                    if (model.DocFileVariable == "SupremeCourtJudgementCopyandopinionfromLawDeptReceivedatKGID")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_supremeOpinionJudgementStatus = false;
                    }
                    if (model.DocFileVariable == "SupremeCourtClaimSettlement")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_supremeClaimSettleStatus = false;
                    }
                    if (model.DocFileVariable == "SupremeCourtNoticePetition")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_SupremeCourtNoticePetitionStatus = false;
                    }
                    if (model.DocFileVariable == "SupremeCourtCoveringLetter")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_SupremeCourtCoveringLetterStatus = false;
                    }
                    if (model.DocFileVariable == "SupremeOpinionNoticeFromLawDept2")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_SupremeOpinionclaimJudgmntstatus = false;
                    }
                    if (model.DocFileVariable == "SupremeClaimSettlement2")
                    {
                        //flow.mvc_objecttionStatement = 1;
                        flow.mvc_claimSettleSupremeCourtKGIDStatus = false;
                    }

                }
            }
            if (model.DocFileVariable == "ParawiseLawyer")
            {
                
                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_parawiseRemarkLawyer = 1;
                work_flow.mvc_parawiseRemarkLawyerStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                 result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "ObjectStatement")
            {
                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_objecttionStatement = 1;
                work_flow.mvc_objecttionStatementStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();

            }
            if (model.DocFileVariable == "RatificationLawDept")
            {
                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_ratificationLawDept = 1;
                work_flow.mvc_ratificationLawDeptStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();

            }
            if (model.DocFileVariable == "LowerCourtJudgementCopy")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_lower_Court_judgementCopy = 1;
                work_flow.mvc_lower_Court_judgementCopyStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "OpinionFromLawDepartment")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_OpinionFromLawDepartment = 1;
                work_flow.mvc_OpinionFromLawDepartmentStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "ClaimApprovalSettleLowerCourt")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_ClaimApprovalSettleLowerCourt = 1;
                work_flow.mvc_ClaimApprovalSettleLowerCourtStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "DelayNotetoGovtAdvocateHighCourt")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_DraftForDelayNoteHighCourt = 1;
                work_flow.mvc_DraftForDelayNoteHighCourtStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "amountDepositToHighCourt")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_amntDepositToHighCourt = 1;
                work_flow.mvc_amntDepositToHighCourtStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "AmountDepostionLetterToLowerCourt")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_amntDepositToLowCourt = 1;
                work_flow.mvc_amntDepositToLowCourtStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "HighCourtJudgementOpinionFormLawDeptDetails")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_HighCourtJudgementOpinion = 1;
                work_flow.mvc_HighCourtJudgementOpinionStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "HighCourtClaimSettlement")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_claimsettleHighCourtJudgement = 1;
                work_flow.mvc_claimsettleHighCourtJudgementStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "HighCourtNoticePetition")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_HighCourtNoticePetition = 1;
                work_flow.mvc_HighCourtNoticePetitionStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "HighCourtCoveringLetter")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_HighCourtCoveringLetter = 1;
                work_flow.mvc_HighCourtCoveringLetterStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "HighCourtJudgementOpinionFormLawDeptDetails2")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_opinionStatusHighCourtKGID = 1;
                work_flow.mvc_opinionStatusHighCourtKGIDStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "HighCourtClaimSettlement2")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_ClaimSettleHighCourt = 1;
                work_flow.mvc_ClaimSettleLowerHighStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "DelayNotetoGovtAdvocateSupremeCourt")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_inputDelaysupremeCourtDraft = 1;
                work_flow.mvc_inputDelaysupremeCourtDraftStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "AmountDepositiontoSupreme_Court")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_amntDepositToSupremeC = 1;
                work_flow.mvc_amntDepositToSupremeCStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "Amount_Deposition_supremeCourtTOLowercourt")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_amntDepositSupremeCToLC = 1;
                work_flow.mvc_amntDepositSupremeCToLCStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "SupremeCourtJudgementCopyandopinionfromLawDeptReceivedatKGID")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_supremeOpinionJudgement = 1;
                work_flow.mvc_supremeOpinionJudgementStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "SupremeCourtClaimSettlement")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_supremeClaimSettle = 1;
                work_flow.mvc_supremeClaimSettleStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "SupremeCourtNoticePetition")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_SupremeCourtNoticePetition = 1;
                work_flow.mvc_SupremeCourtNoticePetitionStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "SupremeCourtCoveringLetter")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_SupremeCourtCoveringLetter = 1;
                work_flow.mvc_SupremeCourtCoveringLetterStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "SupremeOpinionNoticeFromLawDept2")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_SupremeOpinionclaimJudgmnt = 1;
                work_flow.mvc_SupremeOpinionclaimJudgmntstatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }
            if (model.DocFileVariable == "SupremeClaimSettlement2")
            {

                tbl_mvc_claim_workflow work_flow = new tbl_mvc_claim_workflow();
                work_flow.mvc_claim_app_id = model.MVC_claim_app_id;
                work_flow.micw_vehicle_number = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].micw_vehicle_number);
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
                work_flow.mvc_main_flow = 0;
                work_flow.mvc_claimSettlesupremeCourtKGID = 1;
                work_flow.mvc_claimSettleSupremeCourtKGIDStatus = true;
                _db.tbl_mvc_claim_workflow.Add(work_flow);
                result = _db.SaveChanges();
            }

            return result;
        }

        public List<GetVehicleChassisPolicyDetails> GetDocumentDetailsStatusDLL(string FetchDetails, long appid)
        {
            List<GetVehicleChassisPolicyDetails> workFlowDetails = new List<GetVehicleChassisPolicyDetails>();

            if (FetchDetails == "ParawiseLawyer")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_parawiseRemarkLawyer == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "ObjectStatement")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_objecttionStatement == 1 && work.mvc_main_flow!=1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "RatificationToLawDept")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_ratificationLawDept == 1 && work.mvc_main_flow!=1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "LowerCourtJudgementCopy")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_lower_Court_judgementCopy == 1 && work.mvc_main_flow!=1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "opinionFromLawDept")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_OpinionFromLawDepartment == 1 && work.mvc_main_flow!=1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "ClaimApprovalSettleLowerCourt")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_ClaimApprovalSettleLowerCourt == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "DelayNotetoGovtAdvocateHighCourt")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_DraftForDelayNoteHighCourt == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "amountDepositToHighCourt")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_amntDepositToHighCourt == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "AmountDepostionLetterToLowerCourt")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_amntDepositToLowCourt == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "HighCourtJudgementOpinionFormLawDeptDetails")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_HighCourtJudgementOpinion == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "HighCourtClaimSettlement")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_claimsettleHighCourtJudgement == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "HighCourtNoticePetition")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_HighCourtNoticePetition == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "HighCourtCoveringLetter")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_HighCourtCoveringLetter == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "HighCourtJudgementOpinionFormLawDeptDetails2")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_opinionStatusHighCourtKGID == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "HighCourtClaimSettlement2")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_ClaimSettleHighCourt == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "DelayNotetoGovtAdvocateSupremeCourt")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_inputDelaysupremeCourtDraft == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "AmountDepositiontoSupreme_Court")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_amntDepositToSupremeC == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "Amount_Deposition_supremeCourtTOLowercourt")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_amntDepositSupremeCToLC == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "SupremeCourtJudgementCopyandopinionfromLawDeptReceivedatKGID")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_supremeOpinionJudgement == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "SupremeCourtClaimSettlement")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_supremeClaimSettle == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "SupremeCourtNoticePetition")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_SupremeCourtNoticePetition == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "SupremeCourtCoveringLetter")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_SupremeCourtCoveringLetter == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "SupremeOpinionNoticeFromLawDept2")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_SupremeOpinionclaimJudgmnt == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "SupremeClaimSettlement2")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id


                                   where work.mvc_claim_app_id == appid && work.mvc_claimSettlesupremeCourtKGID == 1 && work.mvc_main_flow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "viewSentBackWorkflow")
            {
                workFlowDetails = (from work in _db.tbl_mvc_claim_workflow
                                   join app in _db.tbl_employee_basic_details on work.micw_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.micw_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.micw_remarks equals remarks.remark_id

                                   where work.mvc_claim_app_id == appid && work.mvc_main_flow == 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.micw_creation_datetime,
                                       ByID = work.micw_verified_by,
                                       TO = work.micw_assigned_to,
                                       Remarks = remarks.remark_desc,
                                       comments = work.micw_comments

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
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
        public int saveHearingDatesAndCommentsDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {
               
                
                    mvc_tbl.Ratification_hearingDate = Convert.ToDateTime(model.CourtTime2);
                    mvc_tbl.Ratification_hearingNextHearingDate = Convert.ToDateTime(model.CourtTime3);
                    mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
                    mvc_tbl.Ratification_hearingComments = model.Comments_details;

                    result = _db.SaveChanges();

              
            }


            return result;


        }
        public SelectList GetRemarksUpperCourtDLL()
        {
            List<SelectListItem> RemarkList = new List<SelectListItem>();

            RemarkList = (from remark in _db.tbl_mvc_claim_remarks
                          where remark.moduleType == 2
                          select new SelectListItem
                          {
                              Text = remark.remark_desc,
                              Value = (remark.remark_id).ToString()
                          }).ToList();
            return new SelectList(RemarkList, "Value", "Text"); 

        }
        public List<SelectListItem> GetRemarksLokadhalatCourtDLL()
        {
            List<SelectListItem> RemarkList = new List<SelectListItem>();

            RemarkList = (from remark in _db.tbl_mvc_claim_remarks
                          where remark.moduleType == 2
                          select new SelectListItem
                          {
                              Text = remark.remark_desc,
                              Value = (remark.remark_id).ToString()
                          }).ToList();
            //var LokRemList = new SelectListItem()
            //{

            //    Value = "0",
            //    Text = "Select Trade",

            //};
            //RemarkList.Insert(0, LokRemList);

            //return new SelectList(RemarkList, "Value", "Text");
            return RemarkList;
        }
        public int saveLowerCourtOpinionDetailsDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.lowerCourtJudgementDate = Convert.ToDateTime(model.CourtTime2);
                mvc_tbl.awardedAmount_lowerCourt = Convert.ToDecimal(model.claim_Amount);
                mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
                mvc_tbl.opinionStatusLowerCourt = model.Remarks_id;

                result = _db.SaveChanges();


            }


            return result;


        }
        public int saveClaimApprovalSettleLowerCourtJudgementDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.mvc_claimSettleAwardedAmount = Convert.ToDecimal(model.claim_Amount);
                mvc_tbl.mvc_claimSettleAwardedInterest = Convert.ToDecimal(model.opinionDesc);
                mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
                mvc_tbl.mvc_claimSettlePetitionDate = Convert.ToDateTime(model.CourtTime2);

                mvc_tbl.mvc_claimSettlecourtCost = Convert.ToDecimal(model.injury_desc);
                mvc_tbl.mvc_claimSettleAwardedTotalAmount = Convert.ToDecimal(model.awardedAmntLowCourt);

                result = _db.SaveChanges();


            }


            return result;


        }
        public int SaveDelayNoteToAdvocateHighCourtDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.mvc_InputForDelayNoteHighCourt = model.opinionDesc;

                result = _db.SaveChanges();


            }


            return result;


        }
        public int saveAmountToDeposittedToHighCourtDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.mvc_amntDepoHighCourt = Convert.ToDecimal(model.awardedAmntLowCourt);

                result = _db.SaveChanges();


            }


            return result;


        }
        public int UploadofAmountDepositionLetterLCDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.mvc_amntDepoLowCourt = Convert.ToDecimal(model.awardedAmntLowCourt);

                result = _db.SaveChanges();
                result = 1;

            }


            return result;


        }
        public int HighCourtJudgementOpinionDetailsDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.opinionStatusHighCourt = model.Remarks_id;
                mvc_tbl.HighCourtJudgementDate = Convert.ToDateTime(model.CourtTime2);
                mvc_tbl.awardedAmount_highCourt = Convert.ToDecimal(model.claim_Amount);
                result = _db.SaveChanges();
                result = 1;

            }


            return result;

        }
        public int HighCourtClaimSettlementDetailsDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.HighCourt_Statuatory_Amount = Convert.ToDecimal(model.claim_Amount);
                mvc_tbl.HighCourt_statAmnt_RemittedDate = Convert.ToDateTime(model.CourtTime2);
                mvc_tbl.HighCourtDeposit_Amount = Convert.ToDecimal(model.awardedAmntLowCourt);
                mvc_tbl.HighCourt_depositAmnt_RemittedDate = Convert.ToDateTime(model.CourtTime3);
                mvc_tbl.HighCourtClaimAwarded_Amount = Convert.ToDecimal(model.Claim_settle_courtcost);

                mvc_tbl.HighCourtClaimAwarded_Interest = Convert.ToDecimal(model.income_tax_of_injured);
                mvc_tbl.HighCourtClaimSettle_cost = Convert.ToDecimal(model.Claim_settle_courtcost);
                mvc_tbl.HighCourtClaimSettle_TotalAmnt = Convert.ToDecimal(model.HighCourtAwardedAmount);
                result = _db.SaveChanges();
                result = 1;

            }


            return result;

        }
        public int HighCourtJudgementOpinionDetailsKGIDDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.opinionStatusHighCourtKGID = model.Remarks_id;
                mvc_tbl.HighCourtJudgementDateKGID = Convert.ToDateTime(model.CourtTime2);
                mvc_tbl.awardedAmount_highCourtKGID = Convert.ToDecimal(model.claim_Amount);
                result = _db.SaveChanges();
                result = 1;

            }


            return result;
        }
        public int HighCourtClaimAndSettlementKGIDDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.awardedAmount_highCourtClaimSttleKGID = Convert.ToDecimal(model.claim_Amount);
                mvc_tbl.awardedInterest_highCourtClaimSttleKGID = Convert.ToDecimal(model.income_tax_of_injured);
                mvc_tbl.courtCost_highCourtClaimSttleKGID = Convert.ToDecimal(model.Claim_settle_courtcost);
                mvc_tbl.totalAmnt_highCourtClaimSttleKGID = Convert.ToDecimal(model.HighCourtAwardedAmount);
                result = _db.SaveChanges();
                result = 1;

            }


            return result;
        }

        //Chethan

        public List<GetVehicleChassisPolicyDetails> GetMVCCourtExecutionDLL(long App_id, int category)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_application_details
                              join item in _db.tbl_district_master on data.court_district equals item.dm_id
                              join acc_dist in _db.tbl_taluka_master on data.court_taluk equals acc_dist.tm_id
                              join injury in _db.tbl_mvc_claim_type_of_injury on data.acdnt_type_of_injury equals injury.injury_type_id
                              where data.mvc_claim_app_id == App_id
                              select new GetVehicleChassisPolicyDetails
                              {
                                  Court_DateTime = data.date_of_petition,
                                  Court_MVC_number = data.mvc_no,
                                  Name_of_court = data.name_of_court,
                                  Court_District_Name = item.dm_name_english,
                                  Court_Taluk_Name = acc_dist.tm_englishname,
                                  MVC_number = (data.mvc_no).ToString(),

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
                                  // 21-03-2022
                                  dist_id12 = item.dm_code,
                                  Taluk_id12 = acc_dist.tm_code,
                                  type_injury = injury.injury_type_id,


                                  //

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

                                      MVC_number = (data.mvc_no).ToString(),

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

                                      other_state_court_dist = data.other_state_court_dist,
                                      other_state_court_taluk = data.other_state_court_taluk,
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
                    long scruStat = GetScrutinyStatusCourtExecution(category, App_id);
                    vehicleDetails[i].scrutinyStatus = scruStat;
                }
                else
                {

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

                    long scruStat = GetScrutinyStatusCourtExecution(category, App_id);
                    vehicleDetails[i].scrutinyStatus = scruStat;
                }
            }



            return vehicleDetails;
        }

        public List<GetVehicleChassisPolicyDetails> GetCourtExecutionDocDetailsDLL(long Appno)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_court_execution
                              join item in _db.tbl_court_execution_document on data.court_mvc_ref_no equals item.mvc_ref_no
                              where item.mvc_ref_no == Appno

                              select new GetVehicleChassisPolicyDetails
                              {
                                  Court_ExecutionDetails = item.doc_path,

                              }
                              ).ToList();
            return vehicleDetails;
        }

        public long GetScrutinyStatusCourtExecution(int category, long app_id)
        {
            var scrut = (from mvc in _db.tbl_mvc_court_execution
                         join app in _db.tbl_mvc_court_exeution_workflow on mvc.court_mvc_ref_no equals app.mvc_court_exe_app_id
                         where app.mvc_court_exe_app_id == app_id && app.court_active_status == true
                         select app.assigned_To_category).FirstOrDefault();
            if (scrut == 0)
            {
                scrut = 3;
            }
            return scrut;
        }

        public int stopMVCandlokadhalathOnCourtExecutionSelectDLL(long Appid)
        {
            List<GetVehicleChassisPolicyDetails> mvc_details = new List<GetVehicleChassisPolicyDetails>();
            int result = 0;
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == Appid select n).FirstOrDefault();
            if (mvc_tbl != null)
            {
                mvc_tbl.app_saved_status = 4;
                mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
                result = _db.SaveChanges();
            }

            return result;
        }
        public int StopCourtExecutionProcessDLL(long Appid)
        {
            List<GetVehicleChassisPolicyDetails> mvc_details = new List<GetVehicleChassisPolicyDetails>();
            int result = 0;
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == Appid select n).FirstOrDefault();
            if (mvc_tbl != null)
            {
                mvc_tbl.app_saved_status = 2;
                mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
                result = _db.SaveChanges();
            }

            return result;
        }

        public long SaveMVCCourtExecutionDLL(GetVehicleChassisPolicyDetails model)
        {
            long mvc_ref = Convert.ToInt64(model.court_ref_no);


            tbl_mvc_court_execution court_ = _db.tbl_mvc_court_execution.Where(x => x.court_mvc_ref_no == mvc_ref).FirstOrDefault();
            if (court_ == null)
            {
                try
                {
                    tbl_mvc_court_execution court_data = new tbl_mvc_court_execution();

                    court_data.court_mvc_ref_no = model.court_ref_no;
                    court_data.court_chassis_no = model.vehicle_chasis_no;
                    court_data.court_policy_no = model.Policy_number;
                    court_data.created_by = model.created_by;
                    court_data.verified_by = Convert.ToInt32(model.loginId);
                    court_data.mvc_court_creation_datetime = DateTime.Now;
                    court_data.mvc_court_updation_datetime = DateTime.Now;
                    court_data.status_id = true;


                    _db.tbl_mvc_court_execution.Add(court_data);
                    _db.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {


                    court_.court_mvc_ref_no = model.court_ref_no;
                    court_.court_chassis_no = model.vehicle_chasis_no;
                    court_.court_policy_no = model.Policy_number;
                    court_.created_by = model.created_by;
                    court_.verified_by = Convert.ToInt32(model.loginId);
                    court_.mvc_court_updation_datetime = DateTime.Now;
                    court_.status_id = true;

                    _db.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            var returnMessage = Update_Court_execution_Work_flow_Details(model);
            return model.court_ref_no;

        }

        public int saveCourtDocDLL(string path, long Application_id, string filename)
        {

            int returnpathStatus = 0;
            if (path != null && Application_id != 0)
            {
                try
                {
                    List<tbl_court_execution_document> pathData = _db.tbl_court_execution_document.Where(x => x.mvc_ref_no == Application_id).ToList();

                    if (pathData.Count() == 0)
                    {
                        returnpathStatus = saveCEPathOfFile(path, Application_id);
                    }
                    else
                    {
                        returnpathStatus = saveCEPathOfFile(path, Application_id);
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
        public int saveCEPathOfFile(string path, long App_id)
        {

            string[] newNV = path.Split('/');
            string pathConstant = newNV[6];
            string otherPath = newNV[6];
            int abc = 0;

            tbl_court_execution_document pathData = _db.tbl_court_execution_document.Where(x => x.mvc_ref_no == App_id && x.doc_path.Contains(pathConstant)).FirstOrDefault();
            if (pathData == null)
            {
                tbl_court_execution_document court_doc = new tbl_court_execution_document();
                court_doc.mvc_ref_no = App_id;
                court_doc.doc_path = path;
                court_doc.active_status = true;
                court_doc.court_doc_creation_datetime = DateTime.Now;
                court_doc.court_doc_updation_datetime = DateTime.Now;

                _db.tbl_court_execution_document.Add(court_doc);
                abc = _db.SaveChanges();


                return 1;
            }
            else
            {
                pathData.doc_path = path;
                pathData.court_doc_creation_datetime = DateTime.Now;
                pathData.court_doc_updation_datetime = DateTime.Now;


                abc = _db.SaveChanges();
                return abc;

            }
            return 1;
        }





        public int Update_Court_execution_Work_flow_Details(GetVehicleChassisPolicyDetails model)
        {
            if (model != null)
            {
                try
                {
                    List<tbl_mvc_court_exeution_workflow> oldFlowData = _db.tbl_mvc_court_exeution_workflow.Where(x => x.mvc_court_exe_app_id == model.court_ref_no).ToList();
                    if (oldFlowData.Count() != 0)
                    {
                        foreach (var flow in oldFlowData)
                        {
                            flow.court_active_status = false;
                        }
                    }
                    tbl_mvc_court_exeution_workflow work_flow = new tbl_mvc_court_exeution_workflow();
                    work_flow.mvc_court_exe_app_id = model.court_ref_no;
                    work_flow.court_veh_chassis_no = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].court_veh_chassis_no);
                    work_flow.court_veh_policy_no = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].court_veh_policy_no);
                    work_flow.court_remarks = model.Remarks_id;
                    work_flow.court_comment = model.Comments_details;
                    work_flow.court_verified_by = model.created_by;
                    work_flow.court_checklist_status = true;
                    work_flow.court_application_status = model.Category_id;
                    work_flow.court_active_status = true;
                    work_flow.court_creation_datetime = DateTime.Now;
                    work_flow.court_updation_datetime = DateTime.Now;
                    work_flow.court_assigned_to = model.roleID;
                    work_flow.verified_by_category = model.loginId;
                    work_flow.assigned_To_category = model.Category_id;
                    work_flow.CE_Mainflow = 1;
                    _db.tbl_mvc_court_exeution_workflow.Add(work_flow);
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

        public List<GetVehicleChassisPolicyDetails> GetWorkFlowCOurtExecutionDLL(long App_id, string chassis)
        {
            List<GetVehicleChassisPolicyDetails> workFlowDetails = new List<GetVehicleChassisPolicyDetails>();

            workFlowDetails = (from work in _db.tbl_mvc_court_exeution_workflow
                               join app in _db.tbl_employee_basic_details on work.court_verified_by equals app.employee_id
                               join cat in _db.tbl_category_master on work.court_application_status equals cat.cm_category_id
                               join remarks in _db.tbl_remarks_master on work.court_remarks equals remarks.RM_Remarks_id


                               where work.mvc_court_exe_app_id == App_id
                               select new GetVehicleChassisPolicyDetails
                               {
                                   SubmissionDate = work.court_creation_datetime,
                                   ByID = work.verified_by_category,
                                   TO = work.assigned_To_category,
                                   Remarks = remarks.RM_Remarks_Desc,
                                   comments = work.court_comment

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
        public int SaveDelayNoteToAdvocateSupremeCourtDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.inputDelaysupremeCourt = model.opinionDesc;

                result = _db.SaveChanges();


            }


            return result;


        }
        public int SupremeCourtOpinionAndJudegementCopyDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {

                mvc_tbl.OpinionSupremeCourt = model.Remarks_id;
                mvc_tbl.Supreme_judgement_Date = Convert.ToDateTime(model.CourtTime2);
                mvc_tbl.SupremeopinionAwardAmnt = Convert.ToDecimal(model.claim_Amount);
                result = _db.SaveChanges();
                result = 1;


                result = _db.SaveChanges();


            }


            return result;


        }
        public int SupremeCourtClaimAndSettlementKGIDDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.Supreme_Statuatory_Amount = Convert.ToDecimal(model.claim_Amount);
                mvc_tbl.Statuatory_Amount_Remitted = Convert.ToDateTime(model.CourtTime2);
                mvc_tbl.Supreme_DepositAmount = Convert.ToDecimal(model.awardedAmntLowCourt);
                mvc_tbl.Supreme_Deposit_Amount_Remitted = Convert.ToDateTime(model.CourtTime3);
                mvc_tbl.Supreme_Awarded_Amount = Convert.ToDecimal(model.claim_settle_awardedAmt);
                mvc_tbl.Supreme_Awarded_Interest = Convert.ToDecimal(model.income_tax_of_injured);
                mvc_tbl.Supreme_Court_Cost = Convert.ToDecimal(model.Claim_settle_courtcost);
                mvc_tbl.Supreme_Total_Amount = Convert.ToDecimal(model.HighCourtAwardedAmount);
                result = _db.SaveChanges();
                result = 1;

            }


            return result;
        }
        public int SupremeCourtOpinionAndJudegementCopyKGIDDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {

                mvc_tbl.mvc_opinionSupremeStatusID2 = model.Remarks_id;
                mvc_tbl.mvc_JudgementSupreme_date2 = Convert.ToDateTime(model.CourtTime2);
                mvc_tbl.mvc_awardedSupreme_amount2 = Convert.ToDecimal(model.claim_Amount);
                result = _db.SaveChanges();
                result = 1;


                result = _db.SaveChanges();


            }


            return result;


        }
        public int saveSupremeClaimApprovalSettlementDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {


                mvc_tbl.awardedAmount_supremeCourtKGID = Convert.ToDecimal(model.claim_Amount);
                mvc_tbl.awardedInterest_supremeCourtKGID = Convert.ToDecimal(model.awardedAmntLowCourt);
                mvc_tbl.courtcost_supremeCourtKGID = Convert.ToDecimal(model.Claim_settle_courtcost);
                mvc_tbl.TotalAmount_supremeCourtKGID = Convert.ToDecimal(model.HighCourtAwardedAmount);

                result = _db.SaveChanges();
                result = 1;

            }


            return result;


        }
        public int CEUpdateDocumentWork_flow_detailsDLL(GetVehicleChassisPolicyDetails model)
        {
            List<tbl_mvc_court_exeution_workflow> oldFlowData = _db.tbl_mvc_court_exeution_workflow.Where(x => x.mvc_court_exe_app_id == model.MVC_claim_app_id).ToList();
            int result = 0;
            if (oldFlowData.Count() != 0)
            {
                foreach (var flow in oldFlowData)
                {
                    if (model.DocFileVariable == "CEOpinionFromLawDepartment")
                    {
                        //flow.mvc_parawiseRemarkLawyer = 1;
                        flow.CE_OpinionLawDeptStatus = false;
                    }
                    if (model.DocFileVariable == "CEClaimApprovalandSettlementDepartment")
                    {
                        //flow.mvc_parawiseRemarkLawyer = 1;
                        flow.CE_ClaimSettleDeptStatus = false;
                    }
                }
                if (model.DocFileVariable == "CEOpinionFromLawDepartment")
                {

                    tbl_mvc_court_exeution_workflow work_flow = new tbl_mvc_court_exeution_workflow();
                    work_flow.mvc_court_exe_app_id = model.MVC_claim_app_id;
                    work_flow.court_veh_chassis_no = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].court_veh_chassis_no);
                    work_flow.court_veh_policy_no = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].court_veh_policy_no);
                    work_flow.court_remarks = model.Remarks_id;
                    work_flow.court_comment = model.Comments_details;
                    work_flow.court_verified_by = model.created_by;
                    work_flow.court_checklist_status = true;
                    work_flow.court_application_status = model.Category_id;
                    work_flow.court_active_status = true;
                    work_flow.court_creation_datetime = DateTime.Now;
                    work_flow.court_updation_datetime = DateTime.Now;
                    work_flow.court_assigned_to = model.roleID;
                    work_flow.verified_by_category = model.loginId;
                    work_flow.assigned_To_category = model.roleID;
                    work_flow.CE_Mainflow = 0;
                    work_flow.CE_OpinionLawDept = 1;
                    work_flow.CE_OpinionLawDeptStatus = true;
                    _db.tbl_mvc_court_exeution_workflow.Add(work_flow);

                    result = _db.SaveChanges();
                    result = 1;
                }
                if (model.DocFileVariable == "CEClaimApprovalandSettlementDepartment")
                {

                    tbl_mvc_court_exeution_workflow work_flow = new tbl_mvc_court_exeution_workflow();
                    work_flow.mvc_court_exe_app_id = model.MVC_claim_app_id;
                    work_flow.court_veh_chassis_no = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].court_veh_chassis_no);
                    work_flow.court_veh_policy_no = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].court_veh_policy_no);
                    work_flow.court_remarks = model.Remarks_id;
                    work_flow.court_comment = model.Comments_details;
                    work_flow.court_verified_by = model.created_by;
                    work_flow.court_checklist_status = true;
                    work_flow.court_application_status = model.Category_id;
                    work_flow.court_active_status = true;
                    work_flow.court_creation_datetime = DateTime.Now;
                    work_flow.court_updation_datetime = DateTime.Now;
                    work_flow.court_assigned_to = model.roleID;
                    work_flow.verified_by_category = model.loginId;
                    work_flow.assigned_To_category = model.roleID;
                    work_flow.CE_Mainflow = 0;

                    work_flow.CE_ClaimSettleDept = 1;
                    work_flow.CE_ClaimSettleDeptStatus = true;
                    _db.tbl_mvc_court_exeution_workflow.Add(work_flow);

                    result = _db.SaveChanges();
                    result = 1;
                }
                
            }
            return result;
        }
        public int CEUpdateOpinionLawDeptDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_court_execution mvc_tbl = (from n in _db.tbl_mvc_court_execution where n.court_mvc_ref_no == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {
                mvc_tbl.CE_Opinion_Status = model.OpinionId;
                mvc_tbl.CE_JudgementDate = Convert.ToDateTime(model.CourtTime2);
                mvc_tbl.CE_ClaimAwarded_Amount = Convert.ToDecimal(model.claim_Amount);
                result = _db.SaveChanges();
                result = 1;
            }
            else
            {
                result = 0;
            }
            return result;
        }
        public int CEClaimsettleLawDeptDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_court_execution mvc_tbl = (from n in _db.tbl_mvc_court_execution where n.court_mvc_ref_no == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {
                mvc_tbl.CE_AwardedAmount = Convert.ToDecimal(model.claim_Amount);
                mvc_tbl.CE_AwardedInterest = Convert.ToDecimal(model.income_tax_of_injured);
                mvc_tbl.CE_courtcost = Convert.ToDecimal(model.Claim_settle_courtcost);
                mvc_tbl.CE_totalAmount = Convert.ToDecimal(model.HighCourtAwardedAmount);
                result = _db.SaveChanges();
                result = 1;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public List<GetVehicleChassisPolicyDetails> CourtExecutionMasterDetailsDLL(long appid)
        {
            var vehicleDetails = (from data in _db.tbl_mvc_application_details
                                  join item in _db.tbl_mvc_court_execution on data.mvc_claim_app_id equals item.court_mvc_ref_no

                                  where data.mvc_claim_app_id == appid
                                  select new GetVehicleChassisPolicyDetails
                                  {
                                      HighCourtjudgementDate = (item.CE_JudgementDate).ToString(),
                                      HighCourtAwardedAmount = (item.CE_AwardedAmount).ToString(),
                                      HighCourtOpinionID = item.CE_Opinion_Status,
                                      HighCourtClaimAwardedInterest = (item.CE_AwardedInterest).ToString(),
                                      HighCourtClaimAwardedAmnt = (item.CE_ClaimAwarded_Amount).ToString(),
                                      Claim_settle_courtcost = (item.CE_courtcost).ToString(),
                                      HighCourtClaimSettleTotalAmnt = (item.CE_totalAmount).ToString(),
                                  }).Distinct().ToList();
            for (int i = 0; i < vehicleDetails.Count; i++)
            {
                if (vehicleDetails[i].HighCourtOpinionID == 6)
                {
                    vehicleDetails[i].opinionDesc = "APPEAL";

                }
                else if (vehicleDetails[i].HighCourtOpinionID == 7)
                {
                    vehicleDetails[i].opinionDesc = "NO APPEAL";
                }
                else
                {
                    vehicleDetails[i].opinionDesc = " ";

                }
            }
            return vehicleDetails;
        }
        public List<GetVehicleChassisPolicyDetails> GetCourtExecutiveDocumentDetailsStatusDLL(string FetchDetails, long appid)
        {
            List<GetVehicleChassisPolicyDetails> workFlowDetails = new List<GetVehicleChassisPolicyDetails>();

            if (FetchDetails == "CEOpinionFromLawDepartment")
            {
                workFlowDetails = (from work in _db.tbl_mvc_court_exeution_workflow
                                   join app in _db.tbl_employee_basic_details on work.verified_by_category equals app.employee_id
                                   join cat in _db.tbl_category_master on work.court_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.court_remarks equals remarks.remark_id


                                   where work.mvc_court_exe_app_id == appid && work.CE_OpinionLawDept == 1 && work.CE_Mainflow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.court_creation_datetime,
                                       ByID = work.verified_by_category,
                                       TO = work.assigned_To_category,
                                       Remarks = remarks.remark_desc,
                                       comments = work.court_comment

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "CEClaimApprovalandSettlementDepartment")
            {
                workFlowDetails = (from work in _db.tbl_mvc_court_exeution_workflow
                                   join app in _db.tbl_employee_basic_details on work.verified_by_category equals app.employee_id
                                   join cat in _db.tbl_category_master on work.court_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.court_remarks equals remarks.remark_id


                                   where work.mvc_court_exe_app_id == appid && work.CE_ClaimSettleDept == 1 && work.CE_Mainflow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.court_creation_datetime,
                                       ByID = work.verified_by_category,
                                       TO = work.assigned_To_category,
                                       Remarks = remarks.remark_desc,
                                       comments = work.court_comment

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
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
        public SelectList RemarksJudgementDLL()
        {
            List<SelectListItem> JudgementtList = new List<SelectListItem>();

            JudgementtList = (from remark in _db.tbl_mvc_claim_remarks
                              where remark.moduleType==4
                            select new SelectListItem
                            {
                                Text = remark.remark_desc,
                                Value = (remark.remark_id).ToString(),
                            }).ToList();
            return new SelectList(JudgementtList, "Value", "Text");
        }
        public int  SendBackMvcToCWDLL(GetVehicleChassisPolicyDetails model) {
            tbl_mvc_application_details mvc_tbl = (from n in _db.tbl_mvc_application_details where n.mvc_claim_app_id == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {

                mvc_tbl.app_saved_status = 10;
                mvc_tbl.mvc_claim_updation_datetime = DateTime.Now;
                
                result = _db.SaveChanges();
                result = 1;




            }


            return result;

        }
        public List<GetVehicleChassisPolicyDetails> GetSentBackMVCDetailsDLL()
        {
            List<GetVehicleChassisPolicyDetails> GetDraft = new List<GetVehicleChassisPolicyDetails>();
            GetDraft = (from CMM in _db.tbl_mvc_application_details
                        where CMM.app_saved_status == 10
                        select new GetVehicleChassisPolicyDetails
                        {
                            Court_MVC_number = CMM.mvc_no,
                            vehicle_chasis_no = CMM.chassis_no,
                            MVC_claim_app_id = CMM.mvc_claim_app_id,
                        }).ToList();


            return GetDraft;
        }
        public List<GetVehicleChassisPolicyDetails> GetLokadhalathdetailsofCourtDLL(long App_id, int category)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_application_details
                              join item in _db.tbl_district_master on data.court_district equals item.dm_id
                              join acc_dist in _db.tbl_taluka_master on data.court_taluk equals acc_dist.tm_id
                              join injury in _db.tbl_mvc_claim_type_of_injury on data.acdnt_type_of_injury equals injury.injury_type_id
                              where data.mvc_claim_app_id == App_id
                              select new GetVehicleChassisPolicyDetails
                              {
                                  Court_DateTime = data.date_of_petition,
                                  Court_MVC_number = data.mvc_no,
                                  Name_of_court = data.name_of_court,
                                  Court_District_Name = item.dm_name_english,
                                  Court_Taluk_Name = acc_dist.tm_englishname,
                                  MVC_number = (data.mvc_no).ToString(),

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
                                  // 21-03-2022
                                  dist_id12 = item.dm_code,
                                  Taluk_id12 = acc_dist.tm_code,
                                  type_injury = injury.injury_type_id,


                                  //

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

                                      MVC_number = (data.mvc_no).ToString(),

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

                                      other_state_court_dist = data.other_state_court_dist,
                                      other_state_court_taluk = data.other_state_court_taluk,
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
                    long scruStat = GetLokScrutinyStatus(category, App_id);
                    vehicleDetails[i].scrutinyStatus = scruStat;
                }
                else
                {

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

                    long scruStat = GetLokScrutinyStatus(category, App_id);
                    vehicleDetails[i].scrutinyStatus = scruStat;
                }
            }



            return vehicleDetails;
        }

        public long GetLokScrutinyStatus(int category, long app_id)
        {
            var scrut = (from lok in _db.tbl_mvc_lokadalath_details
                         join app in _db.tbl_mvc_lokadalath_workflow on lok.lok_mvc_ref_no equals app.mvc_lok_exe_app_id
                         where app.mvc_lok_exe_app_id == app_id && app.lok_active_status == true
                         select app.assigned_To_category).FirstOrDefault();
            if (scrut == 0)
            {
                scrut = 3;
            }
            return scrut;
        }

        public long SaveMVCLokadalathDetailsDLL(GetVehicleChassisPolicyDetails model)
        {
            long mvc_ref = Convert.ToInt64(model.lok_mvc_ref_no);
            int abc;

            tbl_mvc_lokadalath_details lok_ = _db.tbl_mvc_lokadalath_details.Where(x => x.lok_mvc_ref_no == mvc_ref).FirstOrDefault();
            if (lok_ == null)
            {
                try
                {
                    tbl_mvc_lokadalath_details tbl_data = new tbl_mvc_lokadalath_details();

                    tbl_data.lok_mvc_ref_no = mvc_ref;
                    tbl_data.lok_chassis_no = model.vehicle_chasis_no;
                    tbl_data.lok_policy_no = model.Policy_number;
                    tbl_data.lok_created_by = model.created_by;
                    tbl_data.lok_creation_datetime = DateTime.Now;
                    tbl_data.lok_updation_datetime = DateTime.Now;
                    tbl_data.lok_verified_by = Convert.ToInt32(model.loginId);
                    tbl_data.lok_status_id = true;
                    tbl_data.lokadalath_date = Convert.ToDateTime(model.lok_date);

                    _db.tbl_mvc_lokadalath_details.Add(tbl_data);
                    abc = _db.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
            {
                try
                {
                    lok_.lok_mvc_ref_no = mvc_ref;
                    lok_.lok_chassis_no = model.vehicle_chasis_no;
                    lok_.lok_policy_no = model.Policy_number;
                    lok_.lok_created_by = model.created_by;
                    lok_.lok_updated_by = model.created_by;
                    lok_.lok_updation_datetime = DateTime.Now;
                    lok_.lok_verified_by = Convert.ToInt32(model.loginId);
                    lok_.lok_status_id = true;

                    abc = _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (abc == 1)
            {
                Update_Lokadalath_Work_flow_Details(model);
            }
            return mvc_ref;
            #endregion
        }

        public int saveLokDocDLL(string path, long Application_id, string filename)
        {

            int returnpathStatus = 0;
            if (path != null && Application_id != 0)
            {
                try
                {
                    List<tbl_mvc_lokadalath_document> pathData = _db.tbl_mvc_lokadalath_document.Where(x => x.mvc_ref_no == Application_id).ToList();



                    if (pathData.Count() == 0)
                    {
                        returnpathStatus = saveLokPathOfFile(path, Application_id);
                    }
                    else
                    {
                        returnpathStatus = saveLokPathOfFile(path, Application_id);
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
        public int Update_Lokadalath_Work_flow_Details(GetVehicleChassisPolicyDetails model)
        {
            if (model != null)
            {
                try
                {
                    long mvc_ref = Convert.ToInt64(model.lok_mvc_ref_no);
                    List<tbl_mvc_lokadalath_workflow> oldFlowData = _db.tbl_mvc_lokadalath_workflow.Where(x => x.mvc_lok_exe_app_id == mvc_ref).ToList();
                    if (oldFlowData.Count() != 0)
                    {
                        foreach (var flow in oldFlowData)
                        {
                            flow.lok_active_status = false;
                        }
                    }
                    tbl_mvc_lokadalath_workflow work_flow = new tbl_mvc_lokadalath_workflow();
                    work_flow.mvc_lok_exe_app_id = Convert.ToInt64(model.lok_mvc_ref_no);
                    work_flow.lok_veh_no = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].lok_veh_no);
                    work_flow.lok_veh_policy_no = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].lok_veh_policy_no);
                    work_flow.lok_remarks = model.Remarks_id;
                    work_flow.lok_comment = model.Comments_details;
                    work_flow.lok_verified_by = model.created_by;
                    work_flow.lok_checklist_status = true;
                    work_flow.lok_application_status = model.Category_id;
                    work_flow.lok_active_status = true;
                    work_flow.lok_creation_datetime = DateTime.Now;
                    work_flow.lok_updation_datetime = DateTime.Now;
                    work_flow.lok_assigned_to = model.roleID;
                    work_flow.verified_by_category = model.loginId;
                    work_flow.assigned_To_category = model.Category_id;
                    work_flow.Lok_Mainflow = 1;
                    _db.tbl_mvc_lokadalath_workflow.Add(work_flow);
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

        public int saveLokPathOfFile(string path, long App_id)
        {
            string[] newNV = path.Split('/');
            string pathConstant = newNV[6];
            string otherPath = newNV[6];
            int abc = 0;

            tbl_mvc_lokadalath_document pathData = _db.tbl_mvc_lokadalath_document.Where(x => x.mvc_ref_no == App_id && x.lok_doc_path.Contains(pathConstant)).FirstOrDefault();
            if (pathData == null)
            {
                tbl_mvc_lokadalath_document lok_doc = new tbl_mvc_lokadalath_document();
                lok_doc.mvc_ref_no = App_id;
                lok_doc.lok_doc_path = path;
                lok_doc.lok_active_status = true;
                lok_doc.lok_doc_creation_datetime = DateTime.Now;
                lok_doc.lok_doc_updation_datetime = DateTime.Now;

                _db.tbl_mvc_lokadalath_document.Add(lok_doc);
                abc = _db.SaveChanges();

                return abc;
            }
            else
            {
                pathData.lok_doc_path = path;
                pathData.lok_doc_creation_datetime = DateTime.Now;
                pathData.lok_doc_updation_datetime = DateTime.Now;

                abc = _db.SaveChanges();
                return abc;
            }
        }

        public List<GetVehicleChassisPolicyDetails> GetWorkFlowLokDLL(long App_id, string chassis)
        {
            List<GetVehicleChassisPolicyDetails> workFlowDetails = new List<GetVehicleChassisPolicyDetails>();

            workFlowDetails = (from work in _db.tbl_mvc_lokadalath_workflow
                               join app in _db.tbl_employee_basic_details on work.lok_verified_by equals app.employee_id
                               join cat in _db.tbl_category_master on work.lok_application_status equals cat.cm_category_id
                               join remarks in _db.tbl_remarks_master on work.lok_remarks equals remarks.RM_Remarks_id


                               where work.mvc_lok_exe_app_id == App_id && work.Lok_Mainflow==1
                               select new GetVehicleChassisPolicyDetails
                               {
                                   SubmissionDate = work.lok_creation_datetime,
                                   ByID = work.verified_by_category,
                                   TO = work.assigned_To_category,
                                   Remarks = remarks.RM_Remarks_Desc,
                                   comments = work.lok_comment

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

        public List<GetVehicleChassisPolicyDetails> GetLokDocDetailsDLL(long Appno)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_lokadalath_details
                              join item in _db.tbl_mvc_lokadalath_document on data.lok_mvc_ref_no equals item.mvc_ref_no
                              where item.mvc_ref_no == Appno

                              select new GetVehicleChassisPolicyDetails
                              {
                                  Lok_doc_Details = item.lok_doc_path,

                              }
                              ).ToList();
            return vehicleDetails;
        }
        public List<GetVehicleChassisPolicyDetails> GetLokadalathDetailsDLL(long Appno)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_lokadalath_details

                              where data.lok_mvc_ref_no == Appno

                              select new GetVehicleChassisPolicyDetails
                              {
                                  Lokadalath_view_date = data.lokadalath_date,

                              }
                              ).ToList();
            return vehicleDetails;
        }
        public int saveJudgementCopyDetailsDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_lokadalath_details mvc_tbl = (from n in _db.tbl_mvc_lokadalath_details where n.lok_mvc_ref_no == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {
                if (model.DocFileVariable == "opinionJudgementCopyFromLawDept")
                {
                    mvc_tbl.mvc_lok_OpinionId = model.Remarks_id;
                    mvc_tbl.mvc_lok_judgementDate = Convert.ToDateTime(model.CourtTime2);
                    mvc_tbl.mvc_lok_awarded_amount = Convert.ToDecimal(model.claim_Amount);
                    result = _db.SaveChanges();
                    result = 1;
                }
                if(model.DocFileVariable == "JudgementOpininonSupremeCopyFromLawDept")
                {
                    mvc_tbl.lok_supreme_opinionId = model.Remarks_id;
                    mvc_tbl.lok_supreme_judgementDate = Convert.ToDateTime(model.CourtTime2);
                    mvc_tbl.lok_supreme_awarded_amount = Convert.ToDecimal(model.claim_Amount);
                    result = _db.SaveChanges();
                    result = 1;
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }


        public int saveLokadhalatDocDLL(string path, long Application_id)
        {

            int returnpathStatus = 0;
            if (path != null && Application_id != 0)
            {
                try
                {
                    List<tbl_court_execution_document> pathData = _db.tbl_court_execution_document.Where(x => x.mvc_ref_no == Application_id).ToList();

                    if (pathData.Count() == 0)
                    {
                        returnpathStatus = saveLokadhalatPathOfFile(path, Application_id);
                    }
                    else
                    {
                        returnpathStatus = saveLokadhalatPathOfFile(path, Application_id);
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
        public int saveLokadhalatPathOfFile(string path, long App_id)
        {

            string[] newNV = path.Split('/');
            string pathConstant = newNV[6];
            string otherPath = newNV[6];
            int abc = 0;

            tbl_mvc_lokadalath_document pathData = _db.tbl_mvc_lokadalath_document.Where(x => x.mvc_ref_no == App_id && x.lok_doc_path.Contains(pathConstant)).FirstOrDefault();
            if (pathData == null)
            {
                tbl_mvc_lokadalath_document court_doc = new tbl_mvc_lokadalath_document();
                court_doc.mvc_ref_no = App_id;
                court_doc.lok_doc_path = path;
                court_doc.lok_active_status = true;
                court_doc.lok_doc_creation_datetime = DateTime.Now;
                court_doc.lok_doc_updation_datetime = DateTime.Now;

                _db.tbl_mvc_lokadalath_document.Add(court_doc);
                abc = _db.SaveChanges();


                return 1;
            }
            else
            {
                pathData.lok_doc_path = path;
                pathData.lok_doc_creation_datetime = DateTime.Now;
                pathData.lok_doc_updation_datetime = DateTime.Now;


                abc = _db.SaveChanges();
                return abc;

            }
            return 1;
        }
        public int UpdateLokadhalatDocumentWork_flow_detailsDLL(GetVehicleChassisPolicyDetails model)
        {
            List<tbl_mvc_lokadalath_workflow> oldFlowData = _db.tbl_mvc_lokadalath_workflow.Where(x => x.mvc_lok_exe_app_id == model.MVC_claim_app_id).ToList();
            int result = 0;
            if (oldFlowData.Count() != 0)
            {
                foreach (var flow in oldFlowData)
                {
                    if (model.DocFileVariable == "OpinionFromLawDepartmentLokadhalat")
                    {
                        //flow.mvc_parawiseRemarkLawyer = 1;
                        flow.loc_opinionJudgementCopyStatus = false;
                    }
                    if (model.DocFileVariable == "JudgementOpininonSupremeCopyFromLawDept")
                    {
                        flow.lok_supreme_opinionJudgementCopyStatus = false;

                    }
                    if (model.DocFileVariable == "LokClaimApprovalandSettlementLowerCourt2")
                    {
                        //flow.mvc_parawiseRemarkLawyer = 1;
                        flow.Lok_ClaimSettleOnLowerCourt2Status = false;
                    }if (model.DocFileVariable == "LokadhalatHighCourtSettlement")
                    {
                        //flow.mvc_parawiseRemarkLawyer = 1;
                        flow.lok_claimSettle_HighStatus = false;
                    }
                    if (model.DocFileVariable == "LokJudgementSupreme")
                    {
                        //flow.mvc_parawiseRemarkLawyer = 1;
                        flow.lok_supreme_opinionJudgementCopyStatus2 = false;
                    }
                    if (model.DocFileVariable == "LokSupremeClaimApprovalandSettlement")
                    {
                        //flow.mvc_parawiseRemarkLawyer = 1;
                        flow.Lok_supreme_ClaimSettleOnLowerCourtStatus = false;
                    }
                }
                if (model.DocFileVariable == "OpinionFromLawDepartmentLokadhalat")
                {

                    tbl_mvc_lokadalath_workflow work_flow = new tbl_mvc_lokadalath_workflow();
                    work_flow.mvc_lok_exe_app_id = model.MVC_claim_app_id;
                    work_flow.lok_veh_no = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].lok_veh_no);
                    work_flow.lok_veh_policy_no = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].lok_veh_policy_no);
                    work_flow.lok_remarks = model.Remarks_id;
                    work_flow.lok_comment = model.Comments_details;
                    work_flow.lok_verified_by = model.Category_id;
                    work_flow.lok_checklist_status = true;
                    work_flow.lok_application_status = model.Category_id;
                    work_flow.lok_active_status = true;
                    work_flow.lok_creation_datetime = DateTime.Now;
                    work_flow.lok_updation_datetime = DateTime.Now;
                    work_flow.lok_assigned_to = model.roleID;
                    work_flow.lok_created_by = model.Category_id;
                    work_flow.lok_updated_by = model.Category_id;
                    work_flow.verified_by_category = model.loginId;
                    work_flow.assigned_To_category = model.roleID;
                    work_flow.Lok_Mainflow = 0;
                    work_flow.loc_opinionJudgementCopy = 1;
                    work_flow.loc_opinionJudgementCopyStatus = true;
                    _db.tbl_mvc_lokadalath_workflow.Add(work_flow);

                    result = _db.SaveChanges();
                    result = 1;
                }
                if (model.DocFileVariable == "JudgementOpininonSupremeCopyFromLawDept")
                {

                    tbl_mvc_lokadalath_workflow work_flow = new tbl_mvc_lokadalath_workflow();
                    work_flow.mvc_lok_exe_app_id = model.MVC_claim_app_id;
                    work_flow.lok_veh_no = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].lok_veh_no);
                    work_flow.lok_veh_policy_no = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].lok_veh_policy_no);
                    work_flow.lok_remarks = model.Remarks_id;
                    work_flow.lok_comment = model.Comments_details;
                    work_flow.lok_verified_by = model.Category_id;
                    work_flow.lok_checklist_status = true;
                    work_flow.lok_application_status = model.Category_id;
                    work_flow.lok_active_status = true;
                    work_flow.lok_creation_datetime = DateTime.Now;
                    work_flow.lok_updation_datetime = DateTime.Now;
                    work_flow.lok_assigned_to = model.roleID;
                    work_flow.lok_created_by = model.Category_id;
                    work_flow.lok_updated_by = model.Category_id;
                    work_flow.verified_by_category = model.loginId;
                    work_flow.assigned_To_category = model.roleID;
                    work_flow.Lok_Mainflow = 0;
                    work_flow.lok_supreme_opinionJudgementCopy = 1;
                    work_flow.lok_supreme_opinionJudgementCopyStatus = true;
                    _db.tbl_mvc_lokadalath_workflow.Add(work_flow);

                    result = _db.SaveChanges();
                    result = 1;
                }
                if (model.DocFileVariable == "LokClaimApprovalandSettlementLowerCourt2")
                {

                    tbl_mvc_lokadalath_workflow work_flow = new tbl_mvc_lokadalath_workflow();
                    work_flow.mvc_lok_exe_app_id = model.MVC_claim_app_id;
                    work_flow.lok_veh_no = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].lok_veh_no);
                    work_flow.lok_veh_policy_no = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].lok_veh_policy_no);
                    work_flow.lok_remarks = model.Remarks_id;
                    work_flow.lok_comment = model.Comments_details;
                    work_flow.lok_verified_by = model.created_by;
                    work_flow.lok_checklist_status = true;
                    work_flow.lok_application_status = model.Category_id;
                    work_flow.lok_active_status = true;
                    work_flow.lok_creation_datetime = DateTime.Now;
                    work_flow.lok_updation_datetime = DateTime.Now;
                    work_flow.lok_assigned_to = model.roleID;
                    work_flow.verified_by_category = model.loginId;
                    work_flow.assigned_To_category = model.roleID;
                    work_flow.Lok_Mainflow = 0;
                    work_flow.Lok_ClaimSettleOnLowerCourt2 = 1;
                    work_flow.Lok_ClaimSettleOnLowerCourt2Status = true;
                    _db.tbl_mvc_lokadalath_workflow.Add(work_flow);

                    result = _db.SaveChanges();
                    result = 1;
                } if (model.DocFileVariable == "LokadhalatHighCourtSettlement")
                {

                    tbl_mvc_lokadalath_workflow work_flow = new tbl_mvc_lokadalath_workflow();
                    work_flow.mvc_lok_exe_app_id = model.MVC_claim_app_id;
                    work_flow.lok_veh_no = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].lok_veh_no);
                    work_flow.lok_veh_policy_no = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].lok_veh_policy_no);
                    work_flow.lok_remarks = model.Remarks_id;
                    work_flow.lok_comment = model.Comments_details;
                    work_flow.lok_verified_by = model.created_by;
                    work_flow.lok_checklist_status = true;
                    work_flow.lok_application_status = model.Category_id;
                    work_flow.lok_active_status = true;
                    work_flow.lok_creation_datetime = DateTime.Now;
                    work_flow.lok_updation_datetime = DateTime.Now;
                    work_flow.lok_assigned_to = model.roleID;
                    work_flow.verified_by_category = model.loginId;
                    work_flow.assigned_To_category = model.roleID;
                    work_flow.Lok_Mainflow = 0;
                    work_flow.lok_claimSettle_Highcourt = 1;
                    work_flow.lok_claimSettle_HighStatus = true;
                    _db.tbl_mvc_lokadalath_workflow.Add(work_flow);

                    result = _db.SaveChanges();
                    result = 1;
                }
                if (model.DocFileVariable == "LokJudgementSupreme")
                {

                    tbl_mvc_lokadalath_workflow work_flow = new tbl_mvc_lokadalath_workflow();
                    work_flow.mvc_lok_exe_app_id = model.MVC_claim_app_id;
                    work_flow.lok_veh_no = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].lok_veh_no);
                    work_flow.lok_veh_policy_no = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].lok_veh_policy_no);
                    work_flow.lok_remarks = model.Remarks_id;
                    work_flow.lok_comment = model.Comments_details;
                    work_flow.lok_verified_by = model.created_by;
                    work_flow.lok_checklist_status = true;
                    work_flow.lok_application_status = model.Category_id;
                    work_flow.lok_active_status = true;
                    work_flow.lok_creation_datetime = DateTime.Now;
                    work_flow.lok_updation_datetime = DateTime.Now;
                    work_flow.lok_assigned_to = model.roleID;
                    work_flow.verified_by_category = model.loginId;
                    work_flow.assigned_To_category = model.roleID;
                    work_flow.Lok_Mainflow = 0;
                    work_flow.lok_supreme_opinionJudgementCopy2 = 1;
                    work_flow.lok_supreme_opinionJudgementCopyStatus2 = true;
                    _db.tbl_mvc_lokadalath_workflow.Add(work_flow);

                    result = _db.SaveChanges();
                    result = 1;
                }

                if (model.DocFileVariable == "LokSupremeClaimApprovalandSettlement")
                {

                    tbl_mvc_lokadalath_workflow work_flow = new tbl_mvc_lokadalath_workflow();
                    work_flow.mvc_lok_exe_app_id = model.MVC_claim_app_id;
                    work_flow.lok_veh_no = ((model.Vehicle_Registration_Number != null) ? model.Vehicle_Registration_Number : oldFlowData[0].lok_veh_no);
                    work_flow.lok_veh_policy_no = ((model.Policy_number != null) ? model.Policy_number : oldFlowData[0].lok_veh_policy_no);
                    work_flow.lok_remarks = model.Remarks_id;
                    work_flow.lok_comment = model.Comments_details;
                    work_flow.lok_verified_by = model.created_by;
                    work_flow.lok_checklist_status = true;
                    work_flow.lok_application_status = model.Category_id;
                    work_flow.lok_active_status = true;
                    work_flow.lok_creation_datetime = DateTime.Now;
                    work_flow.lok_updation_datetime = DateTime.Now;
                    work_flow.lok_assigned_to = model.roleID;
                    work_flow.verified_by_category = model.loginId;
                    work_flow.assigned_To_category = model.roleID;
                    work_flow.Lok_Mainflow = 0;
                    work_flow.Lok_supreme_ClaimSettleOnLowerCourt = 1;
                    work_flow.Lok_supreme_ClaimSettleOnLowerCourtStatus = true;
                    _db.tbl_mvc_lokadalath_workflow.Add(work_flow);

                    result = _db.SaveChanges();
                    result = 1;
                }



            }
            return result;
        }

        public List<GetVehicleChassisPolicyDetails> GetLokadhalatMasterDetailsDLL(long Application_id)
        {
            List<GetVehicleChassisPolicyDetails> vehicleDetails = new List<GetVehicleChassisPolicyDetails>();
            vehicleDetails = (from data in _db.tbl_mvc_lokadalath_details
                              where data.lok_mvc_ref_no == Application_id
                              select new GetVehicleChassisPolicyDetails
                              {
                                  mvc_opinionSupremeStatusID2 = data.mvc_lok_OpinionId,
                                  mvc_awardedSupreme_amount2 = (data.mvc_lok_awarded_amount).ToString(),
                                  SupremeJudgementOpiniondate = (data.mvc_lok_judgementDate).ToString(),
                                  claim_petitionDate = (data.lok_supreme_judgementDate).ToString(),
                                  OpinionId2 = data.lok_supreme_opinionId,
                                  awardedAmount_supremeCourtKGID=(data.lok_supreme_awarded_amount).ToString(),
                                  HighCourtAwardedAmount = (data.lok_claimSettle_awarded_amount).ToString(),
                                  HighCourtClaimAwardedInterest  =(data.lok_claimSettle_awarded_Intrest_amount).ToString(),
                                   Claim_settle_courtcost  =(data.lok_claimSettle_courtcost).ToString(),
                                   HighCourtClaimSettleTotalAmnt  = (data.lok_claimSettle_totalAmount).ToString(),
                                  Supreme_Awarded_Amount= (data.lok_claimSettle_Highawarded_amount).ToString(),
                                   Supreme_Awarded_Interest =(data.lok_claimSettle_Highawarded_Intrest_amount).ToString(),
                                   Supreme_Court_Cost =(data.lok_claimSettle_Highcourtcost).ToString(),
                                   Supreme_Total_Amount =(data.lok_claimSettle_HightotalAmount).ToString(),
                                  Supreme_judgement_date2 = (data.lok_supreme_judgementDate2).ToString(),
                                  Supreme_Awarded_Amount2 = (data.lok_claimSettle_HightotalAmount).ToString(),
                                  OpinionIdSupreme = (data.lok_supreme_opinionId2),
                                  Supreme_Awarded_Amount_Claims = (data.lok_Suprm_claimSettle_awarded_amount).ToString(),
                                  Supreme_Awarded_intrest_Claims = (data.lok__suprm_claimSettle_awarded_Intrest_amount).ToString(),
                                  Supreme_Awarded_court_Claims = (data.lok_suprm_claimSettle_courtcost).ToString(),
                                  Supreme_Awarded_totalAmount_Claims = (data.lok_sprm_claimSettle_totalAmount).ToString(),
                              }).ToList();
            for (int i = 0; i < vehicleDetails.Count; i++)
            {
                if (vehicleDetails[i].mvc_opinionSupremeStatusID2 == 6)
                {
                    vehicleDetails[i].opinionDesc = "APPEAL";

                }
                else if (vehicleDetails[i].mvc_opinionSupremeStatusID2 == 7)
                {
                    vehicleDetails[i].opinionDesc = "NO APPEAL";
                }
                else
                {
                    vehicleDetails[i].opinionDesc = " ";

                }if (vehicleDetails[i].OpinionId2 == 6)
                {
                    vehicleDetails[i].HighCourtOpinionDesc = "APPEAL";

                }
                else if (vehicleDetails[i].OpinionId2 == 7)
                {
                    vehicleDetails[i].HighCourtOpinionDesc = "NO APPEAL";
                }
                else
                {
                    vehicleDetails[i].HighCourtOpinionDesc = " ";

                }
                if (vehicleDetails[i].OpinionIdSupreme == 6)
                {
                    vehicleDetails[i].OpinionIdSupremeDesc = "APPEAL";

                }
                else if (vehicleDetails[i].OpinionIdSupreme == 7)
                {
                    vehicleDetails[i].OpinionIdSupremeDesc = "NO APPEAL";
                }
                else
                {
                    vehicleDetails[i].OpinionIdSupremeDesc = " ";

                }
            }
            return vehicleDetails;
        }
        public List<GetVehicleChassisPolicyDetails> GetLokadhalatDocumentDetailsStatusDLL(string FetchDetails, long appid)
        {
            List<GetVehicleChassisPolicyDetails> workFlowDetails = new List<GetVehicleChassisPolicyDetails>();

            if (FetchDetails == "OpinionFromLawDepartmentLokadhalat")
            {
                workFlowDetails = (from work in _db.tbl_mvc_lokadalath_workflow
                                   join app in _db.tbl_employee_basic_details on work.lok_verified_by equals app.employee_id
                                   join cat in _db.tbl_category_master on work.lok_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.lok_remarks equals remarks.remark_id


                                   where work.mvc_lok_exe_app_id == appid && work.loc_opinionJudgementCopy == 1 && work.Lok_Mainflow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.lok_creation_datetime,
                                       ByID = work.lok_verified_by,
                                       TO = work.assigned_To_category,
                                       Remarks = remarks.remark_desc,
                                       comments = work.lok_comment



                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
                if (FetchDetails == "JudgementOpininonSupremeCopyFromLawDept") 
                {
                    workFlowDetails = (from work in _db.tbl_mvc_lokadalath_workflow
                                       join app in _db.tbl_employee_basic_details on work.lok_verified_by equals app.employee_id
                                       join cat in _db.tbl_category_master on work.lok_application_status equals cat.cm_category_id
                                       join remarks in _db.tbl_mvc_claim_remarks on work.lok_remarks equals remarks.remark_id


                                       where work.mvc_lok_exe_app_id == appid && work.lok_supreme_opinionJudgementCopy == 1 && work.Lok_Mainflow != 1
                                       select new GetVehicleChassisPolicyDetails
                                       {
                                           SubmissionDate = work.lok_creation_datetime,
                                           ByID = work.lok_verified_by,
                                           TO = work.assigned_To_category,
                                           Remarks = remarks.remark_desc,
                                           comments = work.lok_comment



                                       }).OrderByDescending(x => x.SubmissionDate).ToList();
                }
            if (FetchDetails == "LokClaimApprovalandSettlementLowerCourt2")
            {
                workFlowDetails = (from work in _db.tbl_mvc_lokadalath_workflow
                                   join app in _db.tbl_employee_basic_details on work.verified_by_category equals app.employee_id
                                   join cat in _db.tbl_category_master on work.lok_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.lok_remarks equals remarks.remark_id


                                   where work.mvc_lok_exe_app_id == appid && work.Lok_ClaimSettleOnLowerCourt2 == 1 && work.Lok_Mainflow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.lok_creation_datetime,
                                       ByID = work.verified_by_category,
                                       TO = work.assigned_To_category,
                                       Remarks = remarks.remark_desc,
                                       comments = work.lok_comment

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }     if (FetchDetails == "LokadhalatHighCourtSettlement")
            {
                workFlowDetails = (from work in _db.tbl_mvc_lokadalath_workflow
                                   join app in _db.tbl_employee_basic_details on work.verified_by_category equals app.employee_id
                                   join cat in _db.tbl_category_master on work.lok_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.lok_remarks equals remarks.remark_id


                                   where work.mvc_lok_exe_app_id == appid && work.lok_claimSettle_Highcourt == 1 && work.Lok_Mainflow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.lok_creation_datetime,
                                       ByID = work.verified_by_category,
                                       TO = work.assigned_To_category,
                                       Remarks = remarks.remark_desc,
                                       comments = work.lok_comment

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }
            if (FetchDetails == "LokJudgementSupreme")
            {
                workFlowDetails = (from work in _db.tbl_mvc_lokadalath_workflow
                                   join app in _db.tbl_employee_basic_details on work.verified_by_category equals app.employee_id
                                   join cat in _db.tbl_category_master on work.lok_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.lok_remarks equals remarks.remark_id


                                   where work.mvc_lok_exe_app_id == appid && work.lok_supreme_opinionJudgementCopy2 == 1 && work.Lok_Mainflow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.lok_creation_datetime,
                                       ByID = work.verified_by_category,
                                       TO = work.assigned_To_category,
                                       Remarks = remarks.remark_desc,
                                       comments = work.lok_comment

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }

            if (FetchDetails == "LokSupremeClaimApprovalAndSettelement")
            {
                workFlowDetails = (from work in _db.tbl_mvc_lokadalath_workflow
                                   join app in _db.tbl_employee_basic_details on work.verified_by_category equals app.employee_id
                                   join cat in _db.tbl_category_master on work.lok_application_status equals cat.cm_category_id
                                   join remarks in _db.tbl_mvc_claim_remarks on work.lok_remarks equals remarks.remark_id


                                   where work.mvc_lok_exe_app_id == appid && work.Lok_supreme_ClaimSettleOnLowerCourt == 1 && work.Lok_Mainflow != 1
                                   select new GetVehicleChassisPolicyDetails
                                   {
                                       SubmissionDate = work.lok_creation_datetime,
                                       ByID = work.verified_by_category,
                                       TO = work.assigned_To_category,
                                       Remarks = remarks.remark_desc,
                                       comments = work.lok_comment

                                   }).OrderByDescending(x => x.SubmissionDate).ToList();
            }

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
        public int LokClaimsettleLawDeptDLL(GetVehicleChassisPolicyDetails model)
        {
            tbl_mvc_lokadalath_details mvc_tbl = (from n in _db.tbl_mvc_lokadalath_details where n.lok_mvc_ref_no == model.MVC_claim_app_id select n).FirstOrDefault();
            int result = 0;
            if (mvc_tbl != null)
            {
                if (model.DocFileVariable == "LokClaimApprovalandSettlementLowerCourt2")
                {
                    mvc_tbl.lok_claimSettle_awarded_amount = Convert.ToDecimal(model.claim_Amount);
                    mvc_tbl.lok_claimSettle_awarded_Intrest_amount = Convert.ToDecimal(model.income_tax_of_injured);
                    mvc_tbl.lok_claimSettle_courtcost = Convert.ToDecimal(model.Claim_settle_courtcost);
                    mvc_tbl.lok_claimSettle_totalAmount = Convert.ToDecimal(model.HighCourtAwardedAmount);
                    result = _db.SaveChanges();
                    result = 1;
                }
                if (model.DocFileVariable == "LokadhalatHighCourtSettlement")
                {
                    mvc_tbl.lok_claimSettle_Highawarded_amount = Convert.ToDecimal(model.claim_Amount);
                    mvc_tbl.lok_claimSettle_Highawarded_Intrest_amount = Convert.ToDecimal(model.income_tax_of_injured);
                    mvc_tbl.lok_claimSettle_Highcourtcost = Convert.ToDecimal(model.Claim_settle_courtcost);
                    mvc_tbl.lok_claimSettle_HightotalAmount = Convert.ToDecimal(model.HighCourtAwardedAmount);
                    result = _db.SaveChanges();
                    result = 1;
                }
                if (model.DocFileVariable == "LokJudgementSupreme")
                {
                    mvc_tbl.lok_supreme_awarded_amount2 = Convert.ToDecimal(model.claim_Amount);
                    mvc_tbl.lok_supreme_opinionId2 = model.OpinionId;
                    mvc_tbl.lok_supreme_judgementDate2 = Convert.ToDateTime(model.LowerCourtJudgementDate);
                    result = _db.SaveChanges();
                    result = 1;
                }
                if (model.DocFileVariable == "LokSupremeClaimApprovalandSettlement")
                {
                    mvc_tbl.lok_Suprm_claimSettle_awarded_amount = Convert.ToDecimal(model.claim_Amount);
                    mvc_tbl.lok__suprm_claimSettle_awarded_Intrest_amount = Convert.ToDecimal(model.income_tax_of_injured);
                    mvc_tbl.lok_suprm_claimSettle_courtcost = Convert.ToDecimal(model.Claim_settle_courtcost);
                    mvc_tbl.lok_sprm_claimSettle_totalAmount = Convert.ToDecimal(model.HighCourtAwardedAmount);
                    result = _db.SaveChanges();
                    result = 1;
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }
        public SelectList RemarksObjectionStatementDLL(int category)
        {
            List<SelectListItem> JudgementtList = new List<SelectListItem>();
            if (category == 7)
            {
                JudgementtList = (from remark in _db.tbl_mvc_claim_remarks
                                  where remark.moduleType == 3 &&  remark.remark_id!=8
                                  select new SelectListItem
                                  {
                                      Text = remark.remark_desc,
                                      Value = (remark.remark_id).ToString(),
                                  }).ToList();
            }
            else {

                JudgementtList = (from remark in _db.tbl_mvc_claim_remarks
                                  where remark.moduleType == 3  && remark.remark_id!=9
                                  select new SelectListItem
                                  {
                                      Text = remark.remark_desc,
                                      Value = (remark.remark_id).ToString(),
                                  }).ToList();

            }
            return new SelectList(JudgementtList, "Value", "Text");
        }
        public SelectList RemarksPaymentStatementDLL(int category)
        {
            List<SelectListItem> JudgementtList = new List<SelectListItem>();
            if (category == 7)
            {
                JudgementtList = (from remark in _db.tbl_mvc_claim_remarks
                                  where remark.moduleType == 5 && remark.remark_id != 15 && remark.remark_id != 16 && remark.remark_id != 17
                                  select new SelectListItem
                                  {
                                      Text = remark.remark_desc,
                                      Value = (remark.remark_id).ToString(),
                                  }).ToList();
            }
            else {

                JudgementtList = (from remark in _db.tbl_mvc_claim_remarks
                                  where remark.moduleType == 5 && remark.remark_id != 18 && remark.remark_id != 19
                                  select new SelectListItem
                                  {
                                      Text = remark.remark_desc,
                                      Value = (remark.remark_id).ToString(),
                                  }).ToList();

            }
            return new SelectList(JudgementtList, "Value", "Text");
        }
        public int saveVehicleNumberDLL(string vehicle_registration_no, string chassisNo)
        {
            tbl_motor_insurance_vehicle_details mvc_tbl = (from n in _db.tbl_motor_insurance_vehicle_details where n.mivd_chasis_no == chassisNo select n).FirstOrDefault();
            int result = 1;
            if(mvc_tbl != null)
            {

                mvc_tbl.mivd_registration_no = vehicle_registration_no;
                result = _db.SaveChanges();
            }

            return result;

        }
    }


}
