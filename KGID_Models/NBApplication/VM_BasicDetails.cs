using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KGID_Models.Attrebute;

namespace KGID_Models.NBApplication
{
    public class VM_BasicDetails
    {
        public long employee_id { get; set; }
        public Nullable<long> hrms_employee_code { get; set; }
        public Nullable<long> dept_employee_code { get; set; }
        public string employee_name { get; set; }
        public string father_name { get; set; }
        public string spouse_name { get; set; }
        public string employee_name_kannada { get; set; }
        public string father_name_kannada { get; set; }
        public string spouse_name_kannada { get; set; }
        public Nullable<int> gender_id { get; set; }
        public string date_of_birth { get; set; }
        public string place_of_birth { get; set; }
        public DateTime emp_date_of_birth { get; set; }
        public string pan_number { get; set; }
        public string date_of_appointment { get; set; }
        public Nullable<long> mobile_number { get; set; }
        public string email_id { get; set; }
        public Nullable<int> dr_status { get; set; }
        public string Current_spouse_name { get; set; }
        public string Current_spouse_name_kannada { get; set; }
        public string recipient_id { get; set; }
        public Nullable<bool> active_status { get; set; }
        public Nullable<System.DateTime> creation_datetime { get; set; }
        public Nullable<System.DateTime> updation_datetime { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<int> updated_by { get; set; }
        public string first_kgid_policy_no { get; set; }
        public string user_category_id { get; set; }
        [ValidateFile]
        public HttpPostedFileBase divorced_upload_doc { get; set; }
        public string divorced_upload_doc_path { get; set; }

        public string mailPanCount { get; set; }
        public string mailEmailCount { get; set; }

        //Work Details
        public int ewd_work_id { get; set; }
        public long ewd_emp_id { get; set; }
        public string ewd_place_of_posting { get; set; }
        public string ewd_date_of_joining { get; set; }
        public int ewd_payscale_id { get; set; }
        public int ewd_employment_type { get; set; }
        public int ewd_group_id { get; set; }
        public int ewd_designation_id { get; set; }
        public int ewd_ddo_id { get; set; }

        //Address Details
        public long ead_address_id { get; set; }
        public long ead_emp_id { get; set; }
        public string ead_address { get; set; }
        public Nullable<int> ead_pincode { get; set; }
        public long ead_application_id { get; set; }

        //Other Details
        public long eod_detail_id { get; set; }
        public long eod_emp_id { get; set; }
        public bool eod_emp_married { get; set; }
        public bool eod_spouse_govt_emp { get; set; }
        public bool eod_emp_orphan { get; set; }
        public string eod_spouse_kgid_number { get; set; }
        public string eod_spouse_pan_number { get; set; }
        public long eod_application_id { get; set; }

        public bool isKGIDPAN { get; set; }
        //Payscale
        public int payscale_id { get; set; }
        public Nullable<decimal> payscale_minimum { get; set; }
        public Nullable<decimal> payscale_maximum { get; set; }
        public Nullable<decimal> payscale_average { get; set; }
        public Nullable<int> payscale_status { get; set; }


        public string payscle_code { get; set; }//for showing EX.-(1000 - 2000)

        //Gender
        public string gender_desc { get; set; }

        //Employeement Type
        public int et_employee_type_id { get; set; }
        public string et_employee_type_desc { get; set; }

        //Employeement Group
        public int eg_group_id { get; set; }
        public string eg_group_desc { get; set; }

        //DDO Master
        public int dm_ddo_id { get; set; }
        public string dm_ddo_code { get; set; }
        public string dm_ddo_office { get; set; }
        public string dm_dept_code { get; set; }
        public Nullable<int> dm_district_id { get; set; }
        public Nullable<int> dm_taluka_id { get; set; }

        //Designation
        public int d_designation_id { get; set; }
        public string d_designation_desc { get; set; }

        //
        public string QRCode { get; set; }

        //Application details
        public long kad_application_id { get; set; }
        public string kad_kgid_application_number { get; set; }
        public string kad_date_of_submission { get; set; }

        //Dropdown fields
        public List<SelectListItem> Genders { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> PayscaleCodes { get; set; }
        public List<SelectListItem> EmploymentTypes { get; set; }
        public List<SelectListItem> Designations { get; set; }
        public List<SelectListItem> Groups { get; set; }
        public List<SelectListItem> DDOCodes { get; set; }
        //Dropdown bound values
        public string gender { get; set; }
        public string department { get; set; }
        public string designation { get; set; }
        public string group { get; set; }
        public string emptype { get; set; }
        public string ddocode { get; set; }
        public string payscalecode { get; set; }
        public bool activestatus { get; set; }

        //Policy Details
        public string p_kgid_policy_number { get; set; }
        public string p_sanction_date { get; set; }
        public DateTime policy_sanction_date { get; set; }
        public Nullable<double> p_premium { get; set; }
        public string p_total_sum_assured { get; set; }
        //workflow dates
        public string CaseWorkerName { get; set; }
        public string CaseWorkerVerifiedDate { get; set; }
        public string SuperintendentName { get; set; }
        public string SuperintendentVerifiedDate { get; set; }
        public string DIO_Office_Address { get; set; }
        public string ApplicationSubmitedDate { get; set; }
        public string LoadFactor { get; set; }
        public string DLFactor { get; set; }
        //Date Data String Conversion
        public string FinalPayment { get; set; }
        public string DueDate { get; set; }
        public string EndMonthYear { get; set; }
        public string ApprovedMonth { get; set; }
        public string ApprovedYear { get; set; }

        //Employee Upload form
        public string App_Creation_Date { get; set; }

        //hrms payscale master
        public int monthid { get; set; }
        public int yearid { get; set; }
        public string grosspay { get; set; }

        //dr master
        public int dr_id { get; set; }
        public string dr_desc { get; set; }
        
        public DateTime? dateofbirth { get; set; }
        public DateTime? dateofappointment { get; set; }
        public DateTime? ewddateofjoining { get; set; }
        //Payment Details
        public string ChallanReferenceNumber { get; set; }
        public string ChallanAmount { get; set; }
        public string ChallanPaymentDate { get; set; }
        public string ProposalSubmissionDate { get; set; }
    }
}
