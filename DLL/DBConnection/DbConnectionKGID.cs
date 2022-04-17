using System.Data.Entity;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGID_User;
using KGID_Models.KGID_Master;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using KGID_Models.KGID_Loan;
using KGID_Models.KGID_Verification;
using KGID_Models.KGIDLoan;
using KGID_Models.KGIDMotorInsurance;
using KGID_Models.KGID_Login;
using System.Configuration;
using KGID_Models.KGID_MB_Claim;
//using tbl_vehicle_type_master = KGID_Models.KGIDMotorInsurance.tbl_vehicle_type_master;

namespace DLL.DBConnection
{
    public class DbConnectionKGID : DbContext
    {

        //public DbConnectionKGID() : base(Common_Connection.decriptConnection(ConfigurationManager.ConnectionStrings["DbconnectionKGID"].ConnectionString))
        //{

        //}
        public DbSet<tbl_verified_details> tbl_verified_details { get; set; }
        public DbSet<tbl_verification_details> tbl_verification_details { get; set; }
        public DbSet<tbl_district_master> tbl_district_master { get; set; }
        public DbSet<tbl_taluka_master> tbl_taluka_master { get; set; }
        public DbSet<tbl_od_cost_component_master> tbl_od_cost_component_master { get; set; }

        public DbSet<tbl_ranges_master> tbl_ranges_master { get; set; }

        public DbSet<tbl_user_master> tbl_user_master { get; set; }
        public DbSet<tbl_login_master> tbl_login_master { get; set; }
        public DbSet<tbl_ddo_master> tbl_ddo_master { get; set; }
        
        public DbSet<tbl_designation> tbl_designation { get; set; }
        public DbSet<tbl_post> tbl_post { get; set; }
        public DbSet<tbl_dept_master> tbl_department_master { get; set; }


        public DbSet<tbl_insured_details> tbl_insured_details { get; set; }
        public DbSet<tbl_insured_details_new> tbl_insured_details_new { get; set; }


        public DbSet<tbl_challan_details> tbl_challan_details { get; set; }
        public DbSet<tbl_payment_status_details> tbl_payment_status_details { get; set; }

        public DbSet<tbl_challan_status> tbl_challan_status { get; set; }


        public DbSet<tbl_new_employee> tbl_new_employee { get; set; }
        public DbSet<tbl_nb_nominee_details> tbl_nb_nominee_details { get; set; }
        public DbSet<tbl_payscale_details> tbl_payscale_details { get; set; }
        public DbSet<tbl_nb_family_details> tbl_nb_family_details { get; set; }
        public DbSet<tbl_nb_application_details> tbl_nb_application_details { get; set; }
        public DbSet<tbl_nb_personal_health> tbl_nb_personal_health { get; set; }
        public DbSet<tbl_nb_personal_health_master> tbl_nb_personal_health_master { get; set; }
        public DbSet<tbl_nb_declaration_master> tbl_nb_declaration_master { get; set; }


        public DbSet<tbl_new_employee_basic_details> tbl_new_employee_basic_details { get; set; }
        public DbSet<tbl_employee_address_details> tbl_employee_address_details { get; set; }
        public DbSet<tbl_employee_work_details> tbl_employee_work_details { get; set; }
        public DbSet<tbl_family_details> tbl_family_details { get; set; }
        public DbSet<tbl_nominee_details> tbl_nominee_details { get; set; }
        public DbSet<tbl_nominee_number_details> tbl_nominee_number_details { get; set; }
        public DbSet<tbl_personal_disease_details> tbl_personal_disease_details { get; set; }
        public DbSet<tbl_personal_health_details> tbl_personal_health_details { get; set; }
        public DbSet<tbl_personal_health_master> tbl_personal_health_master { get; set; }

        public DbSet<tbl_medical_report_tran> tbl_medical_report_tran { get; set; }
        public DbSet<tbl_medical_unit_code_master> tbl_medical_unit_code_master { get; set; }
        public DbSet<tbl_medical_report_master> tbl_medical_report_master { get; set; }

        public DbSet<tbl_doctor_master> tbl_doctor_master { get; set; }
        public DbSet<tbl_emp_doctor_details> tbl_emp_doctor_details { get; set; }
        public DbSet<tbl_application_referenceno_details> tbl_application_referenceno_details { get; set; }
        public DbSet<tbl_dept_verification_details> tbl_dept_verification_details { get; set; }
        public DbSet<tbl_payscales_master> tbl_payscales_master { get; set; }



        //public DbSet<tbl_employee_loan_details> tbl_employee_loan_details { get; set; }
        public DbSet<tbl_employee_loan_transaction> tbl_employee_loan_transaction { get; set; }
        public DbSet<tbl_upload_employeeform> tbl_upload_employeeform { get; set; }
        public DbSet<tbl_height_age_master> tbl_height_age_master { get; set; }
        public DbSet<tbl_weight_mapping_master> tbl_weight_mapping_master { get; set; }
        public DbSet<tbl_weight_load_master> tbl_weight_load_master { get; set; }
        public DbSet<tbl_load_deduction_master> tbl_load_deduction_master { get; set; }
        public DbSet<tbl_medical_declaration> tbl_medical_declaration { get; set; }
		public DbSet<tbl_kgid_roles> tbl_kgid_roles { get; set; }
		public DbSet<tbl_kgid_policy_district_mapping> tbl_kgid_policy_district_mapping { get; set; }
        public DbSet<tbl_kgid_mapping_details> tbl_kgid_mapping_details { get; set; }
        public DbSet<tbl_medical_leave> tbl_medical_leave { get; set; }
        public DbSet<tbl_status_master> tbl_status_master { get; set; }

        public DbSet<tbl_employee_basic_details> tbl_employee_basic_details { get; set; }
        public DbSet<tbl_employee_family_details> tbl_employee_family_details { get; set; }
        public DbSet<tbl_employee_nominee_details> tbl_employee_nominee_details { get; set; }
        public DbSet<tbl_employee_medical_doctor_details> tbl_employee_medical_doctor_details { get; set; }

        ///Venkatesh:- <DSCSignIn> 
        public DbSet<tbl_dsc_master> tbl_dsc_master { get; set; }
        public DbSet<tbl_policy_details> tbl_policy_details { get; set; }
        public DbSet<tbl_dsc_details> tbl_dsc_details { get; set; }
        public DbSet<tbl_dl_factor_master> tbl_dl_factor_master { get; set; }
        public DbSet<tbl_load_factor_master> tbl_load_factor_master { get; set; }
        /// </DSCSignIn>

        //Masters
        public DbSet<tbl_gender_master> tbl_gender_master { get; set; }
        public DbSet<tbl_employee_group_master> tbl_employee_group_master { get; set; }
        public DbSet<tbl_designation_master> tbl_designation_master { get; set; }
        public DbSet<tbl_employment_type_master> tbl_employment_type_master { get; set; }
        public DbSet<tbl_kgid_application_details> tbl_kgid_application_details { get; set; }
        public DbSet<tbl_kgid_application_workflow_details> tbl_kgid_application_workflow_details { get; set; }
        public DbSet<tbl_family_relation_master> tbl_family_relation_master { get; set; }
        public DbSet<tbl_category_master> tbl_category_master { get; set; }
		///Loan Tables
        public DbSet<tbl_loan_application_workflow> tbl_loan_application_workflow { get; set; }
        public DbSet<tbl_remarks_master> tbl_remarks_master { get; set; }
        //Master table
        public DbSet<tbl_receipt_type_master> tbl_Receipt_Type_Masters { get; set; }
        public DbSet<tbl_receipt_purpose_master> tbl_Receipt_Purpose_Masters { get; set; }
        public DbSet<tbl_receipt_subpurpose_master> tbl_Receipt_Subpurpose_Masters { get; set; }
        public DbSet<tbl_hoa_master> tbl_Hoa_Masters { get; set; }
        public DbSet<tbl_dr_master> tbl_dr_master { get; set; }
        public DbSet<tbl_month_master> tbl_month_master { get; set; }
        //Srividya :- Motor Insurance Proposer Details,Vehicle Details

        public DbSet<tbl_motor_insurance_proposer_details> tbl_motor_insurance_proposer_details { get; set; }
        public DbSet<tbl_motor_insurance_type_of_cover> tbl_motor_insurance_type_of_cover { get; set; }
        public DbSet<tbl_motor_insurance_vehicle_details> tbl_motor_insurance_vehicle_details { get; set; }
       // public DbSet<tbl_mi_renewal_policy_details> tbl_mi_policy_details { get; set; }

        public DbSet<tbl_mi_policy_details> mi_policy_details { get; set; }

        public DbSet<tbl_motor_insurance_application> tbl_motor_insurance_application { get; set; }
        public DbSet<tbl_geographical_extension> tbl_geographical_extension { get; set; }

public DbSet<tbl_vehicle_type_master> tbl_vehicle_type_master { get; set; }
        public DbSet<tbl_vehicle_subtype_master> tbl_vehicle_subtype_master { get; set; }
        public DbSet<tbl_vehicle_category_master> tbl_vehicle_category_master { get; set; }

        public DbSet<tbl_agency_login> tbl_agency_login { get; set; }
        public DbSet<tbl_agency_ddo_master> tbl_agency_ddo_master { get; set; }

        //Loan
        public DbSet<tbl_loan_purpose_master> tbl_loan_purpose_master { get; set; }
        public DbSet<tbl_loan_family_relation_master> tbl_loan_family_relation_master { get; set; }
        public DbSet<tbl_hrms_pay_details_master> tbl_hrms_pay_details_master { get; set; }

        public DbSet<tbl_employee_other_details> tbl_employee_other_details { get; set; }

        //Ticketing Tool 
        public DbSet<tbl_module_type_master> tbl_module_type_master { get; set; }
        public DbSet<tbl_problem_type_master> tbl_problem_type_master { get; set; }
        //MI OD Claims
        public DbSet<tbl_surveyor_master> tbl_surveyor_master { get; set; }
        public DbSet<tbl_repairer_master> tbl_repairer_master { get; set; }
        public DbSet<tbl_mi_kii_challan_request> tbl_mi_kii_challan_request { get; set; }
        //public DbSet<tbl_mi_policy_details> tbl_mi_policy_details1 { get; set; }
        //public DbSet<tbl_vehicle_type_master> tbl_vehicle_type_master { get; set; }
        public DbSet<tbl_vehicle_category_type_master> tbl_vehicle_category_type_master { get; set; }
        public DbSet<tbl_mvc_application_details> tbl_mvc_application_details{ get; set; }
        public DbSet<tbl_mvc_claim_workflow> tbl_mvc_claim_workflow { get; set; }

        public DbSet<tbl_mvc_claim_doc_details> tbl_mvc_claim_doc_details { get; set; }

        public DbSet<tbl_mvc_respondent_details> tbl_mvc_respondent_details { get; set; }
        public DbSet<tbl_mvc_claim_petitioner_details> tbl_mvc_claim_petitioner_details { get; set; }
        public DbSet<tbl_vehicle_make_master> tbl_vehicle_make_master { get; set; }
        public DbSet<tbl_mvc_claim_type_of_injury> tbl_mvc_claim_type_of_injury { get; set; }
        public DbSet<tbl_State_master>tbl_State_master { get; set; }
        public DbSet<tbl_mvc_claim_remarks> tbl_mvc_claim_remarks { get; set; }
        public DbSet<tbl_mvc_court_execution> tbl_mvc_court_execution { get; set; }
        public DbSet<tbl_court_execution_document> tbl_court_execution_document { get; set; }
        public DbSet<tbl_mvc_court_exeution_workflow> tbl_mvc_court_exeution_workflow { get; set; }
        public DbSet<tbl_mvc_lokadalath_workflow> tbl_mvc_lokadalath_workflow { get; set; }
        public DbSet<tbl_mvc_lokadalath_document> tbl_mvc_lokadalath_document { get; set; }
        public DbSet<tbl_mvc_lokadalath_details> tbl_mvc_lokadalath_details { get; set; }

    }
}
