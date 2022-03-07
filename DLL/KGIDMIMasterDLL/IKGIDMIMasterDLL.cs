using KGID_Models.KGID_Master;
using KGID_Models.KGID_MB_Claim;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.KGIDMIMasterDLL
{
    public interface IKGIDMIMasterDLL
    {
        IEnumerable<tbl_motor_insurance_type_of_cover> GetTypeofCoverList();
        IEnumerable<tbl_geographical_extension> GetGeoDetails();

        IEnumerable<tbl_vehicle_type_master> GetVehicleTypeList();
        IEnumerable<tbl_vehicle_subtype_master> GetVehicleSubTypeList();
        IEnumerable<tbl_vehicle_category_master> GetVehicleCategoryTypeList();
        IEnumerable<tbl_month_master> GetMonthList();

        //MI OD Claims
        IEnumerable<tbl_surveyor_master> GetSurveyorList();
        IEnumerable<tbl_repairer_master> GetRepairerList();
    }
}
