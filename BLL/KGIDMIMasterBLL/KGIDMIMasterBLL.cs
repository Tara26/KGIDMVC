using DLL.KGIDMIMasterDLL;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_MB_Claim;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDMIMasterBLL
{
    public class KGIDMIMasterBLL: IKGIDMIMasterBLL
    {
        private readonly IKGIDMIMasterDLL _IKGIDMIMasterDLL;
        public KGIDMIMasterBLL()
        {
            this._IKGIDMIMasterDLL = new KGIDMIMasterDLL();
        }
        public IEnumerable<tbl_motor_insurance_type_of_cover> GetTypeofCoverList()
        {
            var result = _IKGIDMIMasterDLL.GetTypeofCoverList();
            return result.OrderBy(t => t.mitoc_type_cover_name);
        }
        public IEnumerable<tbl_geographical_extension> GetGeoDetails()
        {
            var result = _IKGIDMIMasterDLL.GetGeoDetails();
            return result.OrderBy(t => t.ge_name_of_country);
        }


        public IEnumerable<tbl_vehicle_type_master> GetVehicleTypeList()
        {
            var result = _IKGIDMIMasterDLL.GetVehicleTypeList();
            return result.OrderBy(t => t.vht_vehicle_type_desc);
        }
        public IEnumerable<tbl_vehicle_subtype_master> GetVehicleSubTypeList()
        {
            var result = _IKGIDMIMasterDLL.GetVehicleSubTypeList();
            return result.OrderBy(t => t.vst_vehicle_subtype_desc);
        }
        public IEnumerable<tbl_vehicle_category_master> GetVehicleCategoryTypeList()
        {
            var result = _IKGIDMIMasterDLL.GetVehicleCategoryTypeList();
            return result.OrderBy(t => t.vc_vehicle_category_desc);
        }
        public IEnumerable<tbl_month_master> GetMonthList()
        {
            var result = _IKGIDMIMasterDLL.GetMonthList();
            return result.OrderBy(t => t.mm_month_id);
        }
        //MI OD Claims
        public IEnumerable<tbl_surveyor_master> GetSurveyorList()
        {
            var result = _IKGIDMIMasterDLL.GetSurveyorList();
            return result.OrderBy(t => t.svm_name);
        }
        public IEnumerable<tbl_repairer_master> GetRepairerList()
        {
            var result = _IKGIDMIMasterDLL.GetRepairerList();
            return result.OrderBy(t => t.rep_name);
        }
    }
}
