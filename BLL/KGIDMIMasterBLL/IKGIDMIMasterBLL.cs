using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDMIMasterBLL
{
    public interface IKGIDMIMasterBLL
    {
        IEnumerable<tbl_geographical_extension> GetGeoDetails();

        IEnumerable<tbl_vehicle_type_master> GetVehicleTypeList();
        IEnumerable<tbl_vehicle_subtype_master> GetVehicleSubTypeList();
        IEnumerable<tbl_vehicle_category_master> GetVehicleCategoryTypeList();
    }
}
