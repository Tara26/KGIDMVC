using DLL.DBConnection;
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
    public class KGIDMIMasterDLL: IKGIDMIMasterDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        tbl_geographical_extension distmaster;
        public IEnumerable<tbl_motor_insurance_type_of_cover> GetTypeofCoverList()
        {
            var TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
                              select n).ToList();
            return TypeofCoverList;
        }
        public IEnumerable<tbl_geographical_extension> GetGeoDetails()
        {
            var distMaster = (from n in _db.tbl_geographical_extension
                              select n).ToList();
            return distMaster;
        }

        public IEnumerable<tbl_vehicle_type_master> GetVehicleTypeList()
        {
            var distMaster = (from n in _db.tbl_vehicle_type_master
                              select n).ToList();
            return distMaster;
        }
        public IEnumerable<tbl_vehicle_subtype_master> GetVehicleSubTypeList()
        {
            var distMaster = (from n in _db.tbl_vehicle_subtype_master
                              select n).ToList();
            return distMaster;
        }
        public IEnumerable<tbl_vehicle_category_master> GetVehicleCategoryTypeList()
        {
            var distMaster = (from n in _db.tbl_vehicle_category_master
                              select n).ToList();
            return distMaster;
        }
        public IEnumerable<tbl_month_master> GetMonthList()
        {
            var distMaster = (from n in _db.tbl_month_master
                              select n).ToList();
            return distMaster;
        }

        //MI OD Claims
        public IEnumerable<tbl_surveyor_master> GetSurveyorList()
        {
            var surveyorMaster = (from n in _db.tbl_surveyor_master
                                  select n).ToList();
            return surveyorMaster;
        }
        public IEnumerable<tbl_repairer_master> GetRepairerList()
        {
            var repairerMaster = (from n in _db.tbl_repairer_master
                                  select n).ToList();
            return repairerMaster;
        }
    }
}
