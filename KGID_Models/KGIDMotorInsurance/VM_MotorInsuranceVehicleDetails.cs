using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KGID_Models.KGIDMotorInsurance
{
    public class VM_MotorInsuranceVehicleDetails
    {
        public VM_MotorInsuranceVehicleDetails()
        {
            VehicleTypeOfClassList = new List<SelectListItem>();
            VehicleFuelList = new List<SelectListItem>();
            VehicleCategoryTypeList = new List<SelectListItem>();
            VehicleTypeOfMakeList = new List<SelectListItem>();
            VehicleSubTypeList = new List<SelectListItem>();
            VehicleCategoryList = new List<SelectListItem>();
            mipd_type_of_cover_list = new List<SelectListItem>();
        }
        /// <summary>
        public string mipd_type_of_cover { get; set; }
        public int mipd_type_of_cover_id { get; set; }
        public List<SelectListItem> mipd_type_of_cover_list { get; set; }
        /// </summary>
        public long mi_referenceno { get; set; }
        public long mivd_employee_id { get; set; }
        public string mivd_registration_no { get; set; }
        public string mivd_date_of_registration { get; set; }
        public string mivd_registration_authority_and_location { get; set; }
        public int mivd_year_of_manufacturer { get; set; }
        public string mivd_engine_no { get; set; }
      
        public string UserName { get; set; }
        public string mivd_chasis_no { get; set; }
        public int mivd_manufacture_of_vehicle { get; set; }
        public int mivd_make_of_vehicle { get; set; }
        public int mivd_vehicle_category_type_id { get; set; }
        public int mivd_vehicle_class_id { get; set; }
        public string mivd_type_of_model { get; set; }
        public Nullable<int> mivd_cubic_capacity { get; set; }
        public Nullable<int> mivd_seating_capacity_including_driver { get; set; }
        public int mivd_vehicle_fuel_type { get; set; }
        public string mivd_vehicle_reg_no { get; set; }
        public string mivd_date_of_manufacture { get; set; }
        //public DateTime? mivd_date_of_manufacture1 { get; set; }
        public string mivd_vehicle_type_id { get; set; }
        public string mivd_vehicle_subtype_id { get; set; }
        public string mivd_vehicle_category_id { get; set; }
        public string mivd_manufacturer_month { get; set; }
        public string mivd_manufacturer_month_desc { get; set; }
        public long mivd_vehicle_rto_id { get; set; }
        public int mivd_vehicle_weight { get; set; }
        public decimal mivd_own_damage_value { get; set; }
        public Nullable<decimal> mivd_premium_liability_value { get; set; }
		public int mivd_vehicle_min_value { get; set; }
        public string mivd_pagetype { get; set; }
        public int mivd_malus_value { get; set; }
        public int mivd_ncb_value { get; set; }
        public int mivd_Depreciation_value { get; set; }
        public int VehiclePolicyMonth { get; set; }
        public decimal mivd_own_damage_id { get; set; }
        public Nullable<decimal> mivd_premium_liability_id { get; set; }
        public int mivd_vehicle_min_id { get; set; }
        public int mivd_malus_id { get; set; }
        public int mivd_ncb_id { get; set; }
        public int mivd_Depreciation_id { get; set; }
        public string mivd_Zone { get; set; }
        public decimal mivd_Additionalamt { get; set; }
        public decimal mivd_govDiscount { get; set; }

        public decimal mivd_PLgovDiscount { get; set; }
        public decimal mivd_PLDriverAmt { get; set; }
        public decimal mivd_PLPassengerAmt { get; set; }

        //Drop Down List
        public List<SelectListItem> VehicleManufacturerYearList { get; set; }
        public List<SelectListItem> VehicleFuelList { get; set; }
        public List<SelectListItem> VehicleCategoryTypeList { get; set; }
        public List<SelectListItem> VehicleTypeOfMakeList { get; set; }
        public List<SelectListItem> VehicleTypeOfClassList { get; set; }
        public List<SelectListItem> VehicleTypeList { get; set; }
        public List<SelectListItem> VehicleSubTypeList { get; set; }
        public List<SelectListItem> VehicleCategoryList { get; set; }
        public List<SelectListItem> VehicleRTOList { get; set; }

        //Dropdown bound values
        public string year { get; set; }
        public string VehicleFuel { get; set; }
        public string VehicleClass { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleCategoryType { get; set; }
        public string VehicleRTOdesc { get; set; }


        public string yeardesc { get; set; }
        public string VehicleFueldesc { get; set; }
        public string VehicleClassdesc { get; set; }
        public string VehicleMakedesc { get; set; }
        public string VehicleCategoryTypedesc { get; set; }
        public string VehicleTypedesc { get; set; }
        public string VehicleSubTypedesc { get; set; }
        public string VehicleCategorydesc { get; set; }
        public string VehicleIDVAmount { get; set; }
        public string VehicleSaleAmount { get; set; }
    }

    public class VM_MotorInsurancePreviousHistoryDetails
    {
        public long ph_EmployeeCode { get; set; }
        public long ph_reference { get; set; }
        public DateTime? ph_DateOfPurchaseOfVehicle { get; set; }
        public bool ph_PurchaseType { get; set; }
        public bool ph_VehicleUsedPurposeA { get; set; }
        public bool ph_VehicleUsedPurposeB { get; set; }
        public bool ph_vehicleCondition { get; set; }
        public string ph_VehicleConditionReason { get; set; }
        public string ph_previousinsurerDetails { get; set; }
        public string ph_previousinsurerNo { get; set; }
        public DateTime? ph_insuranceFromDt { get; set; }
        public DateTime? ph_insuranceToDt { get; set; }
        public string ph_TypeOfCover { get; set; }
        public bool ph_InsuranceDeclined { get; set; }
        public bool ph_InsuranceCancelled { get; set; }
        public bool ph_InsuranceImposed { get; set; }
        public string ph_CancelledReason { get; set; }
        public string ph_ImposedReason { get; set; }
        public bool ph_Hire { get; set; }
        public bool ph_Lease { get; set; }
        public bool ph_Hypothecation { get; set; }
        public string ph_HReason { get; set; }
        public string ph_OtherInfo { get; set; }
        public string mivd_pagetype { get; set; }
        public long ph_app_id { get; set; }

        public Nullable<int> ph_vehile_type_id { get; set; }
        public Nullable<decimal> premium_amount { get; set; }
        public Nullable<int> previous_vehicle_malus { get; set; }
        public Nullable<int> previous_vehicle_ncb { get; set; }

        public Nullable<int> previous_vehicle_malus_id { get; set; }
        public Nullable<int> previous_vehicle_ncb_id { get; set; }
    }

    public class VehicleMIResponseDetails
        {

        public int vehicleResponseID { get; set; }
        public bool vehicleResponse { get; set; }
        public string vehicleResponse1 { get; set; }
        public bool IsPHSave { get; set; }


    }
    public class VehicleMIHistoryDetails
    {
        public int vehicleHistoryID { get; set; }
        public bool vehicleHResponse { get; set; }
        public Nullable<DateTime> vehicleHDate1 { get; set; }
        public Nullable<DateTime> vehicleHDate2 { get; set; }
        public Nullable<DateTime> vehicleHDate3 { get; set; }


    }

    public class VM_MIApplicationDetails
    {
        public string Type { get; set; }  //new,Edit,ListOfEmployeeStatus
        public long previousRefNo { get; set; }
        public long RenewalRefNo { get; set; }

        //Loan details
        public string vehicleLoanId { get; set; }
        public long EmployeeId { get; set; }
        public Nullable<DateTime> MI_Loan_start_date { get; set; }
        public Nullable<DateTime> MI_Loan_end_date { get; set; }
        public decimal MI_Loan_Amount { get; set; }

        public int SentBackAppliaction { get; set; }

    }
    public class VM_MotorInsuranceCancellation
    {
        public VM_MotorInsuranceCancellation()
        {
            listVehicleDetails = new List<VM_VehicleDetailsForCancellation>();
        }
        public List<VM_VehicleDetailsForCancellation> listVehicleDetails { get; set; }

        public int reasonID { get; set; }
        public string reasonDesc { get; set; }
        public string miVehicleNo { get; set; }
        public string CancellationDocpload { get; set; }
        public List<SelectListItem> cancelReasonList { get; set; }

        public int IsDocType { get; set; }
        public long App_Proposer_ID { get; set; }
        public long MI_App_Reference_ID { get; set; }
       
        public HttpPostedFileBase AuctionDetailsDoc { get; set; }
        public string AuctionDetailsDoc_filename { get; set; }
        public HttpPostedFileBase VIReportDoc { get; set; }
        public string VIReportDoc_filename { get; set; }
        public HttpPostedFileBase NOCDoc { get; set; }
        public string NOCDoc_filename { get; set; }
       
    }
    public class VM_VehicleDetailsForCancellation
    {
        public string vehicle_chassis_no { get; set; }
        public int vehicle_details_id { get; set; }
        public string vehicle_registration_no { get; set; }
        public long vehicle_application_id { get; set; }
        public string status { get; set; }
        public string policy_from_date { get; set; }
        public string policy_to_date { get; set; }
    }

}
