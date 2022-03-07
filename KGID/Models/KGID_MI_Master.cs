using BLL.KGIDMIMasterBLL;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_MB_Claim;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KGID.Models
{
    public class KGID_MI_Master
    {
        public static IEnumerable<tbl_motor_insurance_type_of_cover> GetTypeofCoverList()
        {
            KGIDMIMasterBLL _TypeofCoverList = new KGIDMIMasterBLL();
            return _TypeofCoverList.GetTypeofCoverList();
        }
        public static IEnumerable<tbl_geographical_extension> GetGeoDetails()
        {
            KGIDMIMasterBLL _Dist = new KGIDMIMasterBLL();
            return _Dist.GetGeoDetails();
        }
        public static IEnumerable<tbl_vehicle_type_master> GetVehicleTypeList()
        {
            KGIDMIMasterBLL _Dist = new KGIDMIMasterBLL();
            return _Dist.GetVehicleTypeList();
        }
        public static IEnumerable<tbl_vehicle_subtype_master> GetVehicleSubTypeList()
        {
            KGIDMIMasterBLL _Dist = new KGIDMIMasterBLL();
            return _Dist.GetVehicleSubTypeList();
        }
        public static IEnumerable<tbl_vehicle_category_master> GetVehicleCategoryTypeList()
        {
            KGIDMIMasterBLL _Dist = new KGIDMIMasterBLL();
            return _Dist.GetVehicleCategoryTypeList();
        }


        public static IEnumerable<tbl_month_master> GetMonthList()
        {
            KGIDMIMasterBLL _Dist = new KGIDMIMasterBLL();
            return _Dist.GetMonthList();
        }

        //OD Claims
        public static IEnumerable<tbl_surveyor_master> GetSurveyorList()
        {
            KGIDMIMasterBLL _Surveyor_List = new KGIDMIMasterBLL();
            return _Surveyor_List.GetSurveyorList();
        }
        public static IEnumerable<tbl_repairer_master> GetRepairerList()
        {
            KGIDMIMasterBLL _Repairer_List = new KGIDMIMasterBLL();
            return _Repairer_List.GetRepairerList();
        }
    }
}