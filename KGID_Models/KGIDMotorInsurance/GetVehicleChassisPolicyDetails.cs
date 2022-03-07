using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KGID_Models.KGIDMotorInsurance
{
    public class GetVehicleChassisPolicyDetails
    {
        public int policy_motor_insurace_emp_id { get; set; }
        public int mia_application_ref_no { get; set; }
        public bool mia_active { get; set; }
        public int mia_user_category { get; set; }
        public DateTime? OD_from_date { get; set; }
        public string OD_from_date1 { get; set; }
        public DateTime? OD_to_date { get; set; }
        public string OD_to_date1 { get; set; }
        public DateTime? TP_from_date { get; set; }
        public string TP_from_date1 { get; set; }
        public DateTime? TP_to_date { get; set; }
        public string TP_to_date1 { get; set; }
        public string vehicle_model { get; set; }
        public string type_of_Cover { get; set; }
        public string vehicle_registration_no { get; set; }
        public DateTime mivd_Date_Of_Manufacturer { get; set; }
        public int cubic_capacity { get; set; }
        public int seating_capacity_including_driver { get; set; }
        public string vehicle_chasis_no { get; set; }
        public string Vehicle_Type { get; set; }
        public string Vehicle_Category_Type { get; set; }
        public string vehicle_category_desc { get; set; }
        public string Policy_number { get; set; }

        public List<GetVehicleChassisPolicyDetails> GetVehicleChassisPolicyDetailsList { get; set; }
        public SelectList TalukaList { get; set; }
        public SelectList DistrictList { get; set; }
        public int District_dm_id { get; set; }
        public string Court_District_Name { get; set; }
        public string Court_Taluk_Name { get; set; }
        public int District_dm_id1 { get; set; }
        public int Taluk_id { get; set; }
        public int Taluk_id1 { get; set; }
         public SelectList RemarksList { get; set; }
        public int Remarks_id { get; set; }
        public string MVC_number { get; set; }
        public DateTime Court_DateTime { get; set; }
        public string  Name_of_court { get; set; }
        public long pincode_Of_Petitioner { get; set; }
        public string name_Of_Petitioner { get; set; }
        public string petitioner_Address { get; set; }
        public long petitioner_Mobile_no { get; set; }
        public string Respondant_name { get; set; }
        public string Respondant_designation { get; set; }
        public string Respondant_department { get; set; }
        public string Respondant_Agency_name { get; set; }
        public string Respondant_address { get; set; }
        public string Respondant_mobile { get; set; }
        public string Respondant_pincode { get; set; }
        public int Accident_district { get; set; }
        public string Accident_district_name { get; set; }
        public int Accident_taluk { get; set; }
        public string Accident_taluk_name { get; set; }
        public string Accident_hobli { get; set; }
        public string Accident_gramPanchayat { get; set; }
        public string Accident_village { get; set; }
        public string Accident_details { get; set; }
        public decimal Accident_claim_amnt { get; set; }
        public string Accident_rc_details { get; set; }
        public string Accident_fir_details { get; set; }
        public string Accident_dl_details { get; set; }
        public string Accident_panchnama_details { get; set; }
        public string Accident_object_statement_details { get; set; }
        public string summons_detals { get; set; }
        public string petitioner_details { get; set; }
        public string petitioner_name { get; set; }

        public string CourtTime { get; set; }
        public string claim_Amount { get; set; }
        public long loginId { get; set; }
        public long MVC_claim_app_id { get; set; }

        public long roleID { get; set; }
        public string Vehicle_Registration_Number { get; set; }
        public string Comments_details { get; set; }
        public int Category_id { get; set; }
        public HttpPostedFileBase UploadPdf { get; set; }
        public HttpPostedFileBase UploadDoc { get; set; }

        public List<GetVehicleChassisPolicyDetails> PetitionerAndRspondantDetailsList { get; set; }
        public List<GetVehicleChassisPolicyDetails> PetitionerList { get; set; }
        public List<GetVehicleChassisPolicyDetails> RespondantList { get; set; }
        public List<GetVehicleChassisPolicyDetails> otherDocuments { get; set; }
    
        public List<GetVehicleChassisPolicyDetails> MVCAppDocDetails { get; set; }
        public List<GetVehicleChassisPolicyDetails> CourtDetailsList { get; set; }
        public List<GetVehicleChassisPolicyDetails> otherDetailsData { get; set; }
        public List<GetVehicleChassisPolicyDetails> MvcClaimWorkFlowDetails { get; set; }
        public string Court_MVC_number { get; set; }
        public string OtherDocument { get; set; }
        public DateTime Accident_Time { get; set; }
        public string owner_name_vehicle { get; set; }
        public int cubic_capacity_vehicle { get; set; }
        public string owner_name_vehicle_address { get; set; }
        public int seating_capacity_vehicle { get; set; }

        public string Vehicle_subtype_desc { get; set; }

        public string vehicle_make_desc { get; set; }

        public string Name_of_injured { get; set; }
        public string Father_name { get; set; }
        public string Spouse_name { get; set; }
        public string Address_of_dead_details { get; set; }
        public int Age_of_injured { get; set; }
        public string occupation_of_injured { get; set; }
        public string employer_deceased_details { get; set; }
        public string monthly_income_of_injured { get; set; }
        public string income_tax_of_injured { get; set; }
        public string place_of_accident { get; set; }
        public string accident_DateTime { get; set; }
        public string police_station_of_jurisdiction { get; set; }
       public string police_station_of_compensation { get; set; }
       public int type_injury{ get; set; }
        public string nature_of_injuries_sustained { get; set; }
        public string medical_officer { get; set; }
        public string Period_of_treatment_of_details { get; set; }
        public string Name_of_injury_caused_of_details { get; set; }
        public string Name_and_address_of_applicant_details { get; set; }
        public string relation_with_deceased { get; set; }
        public string title_property_deceased { get; set; }

        public string any_other_information_details { get; set; }

        public string injury_desc { get; set; }
        public int stateID { get; set; }
        public int injury_id { get; set; }
        public SelectList InjuryList { get; set; }
        public SelectList StateList { get; set; }

        public DateTime SubmissionDate { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public string Remarks { get; set; }

        public string comments { get; set; }
        public  long ByID { get; set; }
 public int application_stat { get; set; }
    }
    public class OtherDocumentS {
        public string OtherDocument { get; set; }

    }
    public class PetitionerData {
        public string name_Of_Petitioner { get; set; }
        public string pincode_Of_Petitioner { get; set; }
        public string petitioner_Address { get; set; }
        public string petitioner_Mobile_no { get; set; }

     
    } public class RespondData {
        public string Respondant_name { get; set; }
        public string Respondant_designation { get; set; }
        public string Respondant_department { get; set; }
        public string Respondant_Agency_name { get; set; }
        public string Respondant_address { get; set; }
        public string Respondant_mobile { get; set; }
        public string Respondant_pincode { get; set; }


    }
}
