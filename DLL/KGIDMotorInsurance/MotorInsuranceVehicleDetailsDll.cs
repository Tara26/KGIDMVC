using DLL.DBConnection;
using KGID_Models.KGID_Policy;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace DLL.KGIDMotorInsurance
{
    public class MotorInsuranceVehicleDetailsDll : IMotorInsuranceVehicleDetailsDll
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();
        DataTable dtOtherData = new DataTable();
        DataTable dtIDVData = new DataTable();
        DataTable dtDocumentsData = new DataTable();

        VM_MotorInsuranceVehicleDetails _vehicleDetails = new VM_MotorInsuranceVehicleDetails();
        public DataTable CreateDataTable(string className)
        {
            DataTable dt = new System.Data.DataTable();
            Type classtype = GetType();
            if (className == "MIOtherDetailsData")
            {
                classtype = typeof(MIOtherDetailsData);
            }
            else if (className == "MIOtherDetailsResponseData")
            {
                classtype = typeof(MIOtherDetailsResponseData);
            }
            else if (className == "VehicleMIResponseDetails")
            {
                classtype = typeof(VehicleMIResponseDetails);
            }
            else if (className == "VehicleMIHistoryDetails")
            {
                classtype = typeof(VehicleMIHistoryDetails);
            }

            PropertyInfo[] properties = classtype.GetProperties();

            foreach (System.Reflection.PropertyInfo pi in properties)
            {

                dt.Columns.Add(pi.Name);

            }
            return dt;
        }

        //=========================Vehicle Details related functions ================================
        public VM_MotorInsuranceVehicleDetails GetMIVehicleDetails(long Empid, long RefID)
        {
            VM_MotorInsuranceVehicleDetails _vehicleDetails = new VM_MotorInsuranceVehicleDetails();
            DataSet dsVD = new DataSet();
            DataSet dsVD1 = new DataSet();

            SqlParameter[] parms = {
             new SqlParameter("@EmpId",Empid),
                    new SqlParameter("@RefID",RefID)
            };
            SqlParameter[] parms1 = {

            };
            dsVD = _Conn.ExeccuteDataset(parms, "sp_kgid_selectMIVehicleDetails");
            dsVD1 = _Conn.ExeccuteDataset(parms1, "sp_kgid_getMultipleVehicleDetailsList");

            if (dsVD.Tables.Count > 0)
                _vehicleDetails = BindVehiceDetails(dsVD);




            var VehicleCategoryTypeList = dsVD1.Tables[0].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("vct_vehicle_category_type_desc"),
                Value = dataRow.Field<int>("vct_vehicle_category_type_id").ToString()
            }).OrderBy(a => a.Text).ToList();

            var VehicleMakeList = dsVD1.Tables[1].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("vm_vehicle_make_desc"),
                Value = dataRow.Field<int>("vm_vehicle_make_id").ToString()
            }).Where(a =>a.Text!=null).OrderBy(a => a.Text).ToList();

            var FuelList = dsVD1.Tables[3].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("vf_vehicle_fuel_type_desc"),
                Value = dataRow.Field<int>("vf_vehicle_fuel_type_id").ToString()
            }).OrderBy(a => a.Text).ToList();
            var ManufacturerYearList = dsVD1.Tables[2].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("vy_vehicle_year"),
                Value = dataRow.Field<int>("vy_vehicle_year_id").ToString()
            }).OrderByDescending(a => a.Text).ToList();
            var RTOList = dsVD1.Tables[7].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("rto_desc"),
                Value = dataRow.Field<int>("rto_id").ToString()
            }).OrderBy(a => a.Text).ToList();
            var ClassList = dsVD1.Tables[8].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("vcm_desc"),
                Value = dataRow.Field<int>("vcm_id").ToString()
            }).OrderBy(a => a.Text).ToList();
            var TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
                                   select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
                                                              ).ToList();
            
            
            List<SelectListItem> subtypelist = null;
            List<SelectListItem> catlist = null;
            
            subtypelist = (from n in _db.tbl_vehicle_subtype_master
                                                  select new SelectListItem { Value = n.vst_vehicle_subtype_id.ToString(), Text = n.vst_vehicle_subtype_desc }
                                             ).ToList();
            
            catlist = (from n in _db.tbl_vehicle_category_master
                       select new SelectListItem { Value = n.vc_vehicle_category_id.ToString(), Text = n.vc_vehicle_category_desc }).ToList();



            //if (_vehicleDetails.mivd_registration_no == "" && (_vehicleDetails.mivd_vehicle_category_id == "12" || _vehicleDetails.mivd_vehicle_category_id == "2"))
            //{
            //    TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
            //                       select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
            //                                                 ).ToList().Where(a => a.Value == "3").Select(a => a).ToList();

            //    //_vehicleDetails.mipd_type_of_cover_id = Convert.ToInt32(TypeofCoverList[0].Value);
            //}
            //else /*if (_vehicleDetails.mivd_registration_no == "" && (_vehicleDetails.mivd_vehicle_category_id == "12" || _vehicleDetails.mivd_vehicle_category_id == "2"))*/
            //{
            //    TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
            //                       select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
            //                                                  ).ToList().Where(a => a.Value != "3").Select(a => a).ToList();
            //    //_vehicleDetails.mipd_type_of_cover_id = Convert.ToInt32(_vehicleDetails.mipd_type_of_cover_id);
            //}
            //else
            //{
            //    TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
            //                       select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
            //                                                 ).ToList();
            //    _vehicleDetails.mipd_type_of_cover_id = Convert.ToInt32(0);
            //}Where(a =>a.Text!="").OrderBy(a =>a.Text).ToList();

            _vehicleDetails.mipd_type_of_cover_list = TypeofCoverList;
            var i = _vehicleDetails.mipd_type_of_cover_id;
            _vehicleDetails.VehicleCategoryTypeList = VehicleCategoryTypeList;
            _vehicleDetails.VehicleFuelList = FuelList;
            _vehicleDetails.VehicleTypeOfMakeList = VehicleMakeList;
            //_vehicleDetails.VehicleTypeOfManufactureList = VehicleManufactureList;
            //_vehicleDetails.VehicleTypeOfModelList = VehicleModelList;
            _vehicleDetails.VehicleManufacturerYearList = ManufacturerYearList;
            _vehicleDetails.VehicleRTOList = RTOList;
            _vehicleDetails.VehicleSubTypeList = subtypelist;
            _vehicleDetails.VehicleCategoryList = catlist;
            _vehicleDetails.VehicleTypeOfClassList = ClassList;
            
            return _vehicleDetails;
        }

        public string CheckVehicleExists(string chassisNo, string engineNo)
        {
            string result;
            VM_MotorInsuranceVehicleDetails _vehicleDetails = new VM_MotorInsuranceVehicleDetails();
            SqlParameter[] parms = {
             new SqlParameter("@chassisno",chassisNo),
             new SqlParameter("@EngineNo",engineNo)
            };
            result = _Conn.ExecuteCmd(parms, "sp_kgid_CheckVwhicleExists");
            return result;
        }

            public VM_MotorInsuranceVehicleDetails GetRTODetailsBll(string chasisNo, string EngineNo)
        {
            VM_MotorInsuranceVehicleDetails _vehicleDetails = new VM_MotorInsuranceVehicleDetails();
            DataSet dsVD = new DataSet();

            SqlParameter[] parms = {
             new SqlParameter("@chassisno",chasisNo),
                    new SqlParameter("@engineno",EngineNo)
            };
            dsVD = _Conn.ExeccuteDataset(parms, "sp_kgid_getrtodetailsbasedonid");

            if (dsVD.Tables[0].Rows.Count > 0)
            {
                _vehicleDetails.mivd_date_of_registration = dsVD.Tables[0].Rows[0]["mbi_date_of_reg"] == DBNull.Value ? "" : Convert.ToString(dsVD.Tables[0].Rows[0]["mbi_date_of_reg"]);
                _vehicleDetails.mivd_registration_no = dsVD.Tables[0].Rows[0]["mbi_reg_no"].ToString();
                _vehicleDetails.mivd_vehicle_reg_no = dsVD.Tables[0].Rows[0]["mbi_vehicle_reg_no"].ToString();
                _vehicleDetails.mivd_cubic_capacity = (dsVD.Tables[0].Rows[0]["mbi_cubic_capacity"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mbi_cubic_capacity"]);
                _vehicleDetails.mivd_seating_capacity_including_driver = (dsVD.Tables[0].Rows[0]["mbi_seatcapacity"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mbi_seatcapacity"]);


            }

            return _vehicleDetails;

        }

        public VM_MotorInsuranceVehicleDetails GetModelListBasedonMake(int makeid)
        {

            VM_MotorInsuranceVehicleDetails _vehicleDetails = new VM_MotorInsuranceVehicleDetails();
            //DataSet dsVD = new DataSet();

            //SqlParameter[] parms = {
            // new SqlParameter("@makeid",makeid),
            //};
            //dsVD = _Conn.ExeccuteDataset(parms, "sp_kgid_getVehicleModelList");

            //if (dsVD.Tables[0].Rows.Count > 0)
            //{
            //    var VehicleModelList = dsVD.Tables[0].AsEnumerable().Select(dataRow => new SelectListItem()
            //    {
            //        Text = dataRow.Field<string>("vm_vehicle_model_desc"),
            //        Value = dataRow.Field<int>("vm_vehicle_model_id").ToString()
            //    }).ToList();
            //    _vehicleDetails.VehicleTypeOfModelList = VehicleModelList;
            //}

            return _vehicleDetails;
        }

        public VM_MotorInsuranceVehicleDetails GetManufactureListBasedonMake(int makeid)
        {

            VM_MotorInsuranceVehicleDetails _vehicleDetails = new VM_MotorInsuranceVehicleDetails();
            //DataSet dsVD = new DataSet();

            //SqlParameter[] parms = {
            // new SqlParameter("@makeid",makeid),
            //};
            //dsVD = _Conn.ExeccuteDataset(parms, "sp_kgid_getVehicleManufactureList");

            //if (dsVD.Tables[0].Rows.Count > 0)
            //{
            //    var VehicleModelList = dsVD.Tables[0].AsEnumerable().Select(dataRow => new SelectListItem()
            //    {
            //        Text = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
            //        Value = dataRow.Field<int>("vm_vehicle_manufacture_id").ToString()
            //    }).ToList();
            //    _vehicleDetails.VehicleTypeOfModelList = VehicleModelList;
            //}
            var list = (from n in _db.tbl_vehicle_subtype_master
                        select new SelectListItem { Value = n.vst_vehicle_subtype_id.ToString(), Text = n.vst_vehicle_subtype_desc }
                                              ).ToList();

            if (makeid == 1 || makeid == 2 || makeid == 3)
            {
                _vehicleDetails.VehicleSubTypeList = list.Where(n => n.Value == "1").ToList();
            }
            else if( makeid == 3)
            {
                _vehicleDetails.VehicleSubTypeList = list.Where(n => n.Value != "1").ToList();
            }
           else if (makeid == 4 )
            {
                _vehicleDetails.VehicleSubTypeList = list.Where(n => n.Value == "1").ToList();
            }
            else if (makeid == 11)
            {

                _vehicleDetails.VehicleSubTypeList = list.Where(n => n.Value != "1").ToList();
            }
            else if (makeid == 16)
            {
                _vehicleDetails.VehicleSubTypeList = list.ToList();
            }
            else
            {
                _vehicleDetails.VehicleSubTypeList = list.Where(n => n.Value == "2").ToList();
            }


            return _vehicleDetails;
        }

        public VM_MotorInsuranceVehicleDetails GetTypeOfVehiclebasedonCategory(int categoryid)
        {

            VM_MotorInsuranceVehicleDetails _vehicleDetails = new VM_MotorInsuranceVehicleDetails();

            DataSet dsVD = new DataSet();

            SqlParameter[] parms = {
             new SqlParameter("@categoryid",categoryid),
            };
            dsVD = _Conn.ExeccuteDataset(parms, "sp_kgid_getTypeofVehicleList");

            if (dsVD.Tables[0].Rows.Count > 0)
            {
                var VehicleTypeOfClassList = dsVD.Tables[0].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vcm_desc"),
                    Value = dataRow.Field<int>("vcm_id").ToString()
                }).OrderBy(a => a.Text).ToList();
                _vehicleDetails.VehicleTypeOfClassList = VehicleTypeOfClassList;
            }
            var TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
                                   select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
                                                              ).OrderBy(a => a.Text).ToList();
            _vehicleDetails.mipd_type_of_cover_list = TypeofCoverList;
            return _vehicleDetails;
        }

        public IEnumerable<tbl_vehicle_category_master> GetVehCatergoryList(string TypeId, long SubTypeID)
        {
            DbConnectionKGID _db = new DbConnectionKGID();
            var distMaster = _db.tbl_vehicle_category_master.Where(a => a.vc_vehicle_type_id==TypeId && a.vc_vehicle_subtype_id == SubTypeID).Select(a => a).OrderBy(a => a.vc_vehicle_category_desc).ToList();
            return distMaster;
        }

        public string SaveMIVehicleDetailsData(VM_MotorInsuranceVehicleDetails vmVehicleDetails)
        {
            string result = string.Empty;
            try
            {


                if (vmVehicleDetails.mivd_pagetype == "Renewal" || vmVehicleDetails.mivd_pagetype == "EditRenewal")
                {
                    SqlParameter[] sqlparam =
                {
                    new SqlParameter("@referenceno",vmVehicleDetails.mi_referenceno),
                    new SqlParameter("@employee_id",vmVehicleDetails.mivd_employee_id),
                    new SqlParameter("@owndamage",vmVehicleDetails.mivd_own_damage_id),
                    new SqlParameter("@liability",vmVehicleDetails.mivd_premium_liability_id),
                    new SqlParameter("@depreciation",vmVehicleDetails.mivd_Depreciation_id),
                    new SqlParameter("@malus",vmVehicleDetails.mivd_malus_id),
                    new SqlParameter("@minvalue",vmVehicleDetails.mivd_vehicle_min_id),
                    new SqlParameter("@ncb",vmVehicleDetails.mivd_ncb_id ),
                    new SqlParameter("@TypeofCoverID", vmVehicleDetails.mipd_type_of_cover_id),

                };
                    result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_SaveMIRenewalVehicleDetails"));
                }
                else
                {
                    SqlParameter[] sqlparam1 =
              {
                    new SqlParameter("@referenceno", vmVehicleDetails.mi_referenceno),
                    new SqlParameter("@employee_id", vmVehicleDetails.mivd_employee_id),
                    new SqlParameter("@registrationno", vmVehicleDetails.mivd_registration_no),
                    new SqlParameter("@regauthorityandLocation", vmVehicleDetails.mivd_registration_authority_and_location),
                    new SqlParameter("@dateofregistration",vmVehicleDetails.mivd_date_of_registration==""?"": vmVehicleDetails.mivd_date_of_registration),
                    new SqlParameter("@Rtoid", vmVehicleDetails.mivd_vehicle_rto_id),
                    new SqlParameter("@chassisno", vmVehicleDetails.mivd_chasis_no),
                    new SqlParameter("@engineno", vmVehicleDetails.mivd_engine_no),
                    new SqlParameter("@cubiccapacity", vmVehicleDetails.mivd_cubic_capacity),
                    new SqlParameter("@seatingcapacity", vmVehicleDetails.mivd_seating_capacity_including_driver),
                    new SqlParameter("@vehicleweight", vmVehicleDetails.mivd_vehicle_weight),
                    new SqlParameter("@vehiclecategorytype", vmVehicleDetails.mivd_vehicle_category_type_id),
                    new SqlParameter("@makeofvehicle", vmVehicleDetails.mivd_make_of_vehicle),
                    new SqlParameter("@vehiclemodel", vmVehicleDetails.mivd_type_of_model),
                    new SqlParameter("@year", vmVehicleDetails.mivd_year_of_manufacturer),
                    new SqlParameter("@dateofmanufacture",vmVehicleDetails.mivd_manufacturer_month),                                    
                    new SqlParameter("@fueltype", vmVehicleDetails.mivd_vehicle_fuel_type),
                    new SqlParameter("@vehicletype", vmVehicleDetails.mivd_vehicle_type_id),
                    new SqlParameter("@vehiclesubtype", vmVehicleDetails.mivd_vehicle_subtype_id),
                    new SqlParameter("@vehiclecategory", vmVehicleDetails.mivd_vehicle_category_id),
                    new SqlParameter("@vehicleclass", vmVehicleDetails.mivd_vehicle_class_id),
                    new SqlParameter("@Vaahanidv", vmVehicleDetails.VehicleIDVAmount==null?"": vmVehicleDetails.VehicleIDVAmount), 
                    new SqlParameter("@TypeofCoverID", vmVehicleDetails.mipd_type_of_cover_id),

                    };
                    result = Convert.ToString(_Conn.ExecuteCmd(sqlparam1, "sp_kgid_SaveMIVehicleDetails"));
                }


            }
            catch (Exception ex)
            {

            }
            return result;

        }


        public VM_MotorInsuranceVehicleDetails GetMIRenewalVehicleDetails(long Empid, long RefID,long RenewalRefNo)
        {

            DataSet dsVD = new DataSet();
            DataSet dsVD1 = new DataSet();
            SqlParameter[] parms = {
             new SqlParameter("@EmpId",Empid),
                    new SqlParameter("@RefID",RefID),
                     new SqlParameter("@RenewalRefNo",RenewalRefNo)
            };
            SqlParameter[] parms1 = {

            };
            dsVD = _Conn.ExeccuteDataset(parms, "sp_getMIRenewalVehicleDetails");
            dsVD1 = _Conn.ExeccuteDataset(parms1, "sp_kgid_getMultipleVehicleDetailsList");
            _vehicleDetails = BindVehiceDetails(dsVD);

            /////////////////////////////////////////
            var VehicleCategoryTypeList = dsVD1.Tables[0].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("vct_vehicle_category_type_desc"),
                Value = dataRow.Field<int>("vct_vehicle_category_type_id").ToString()
            }).ToList();

            var VehicleMakeList = dsVD1.Tables[1].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("vm_vehicle_make_desc"),
                Value = dataRow.Field<int>("vm_vehicle_make_id").ToString()
            }).ToList();

            var FuelList = dsVD1.Tables[3].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("vf_vehicle_fuel_type_desc"),
                Value = dataRow.Field<int>("vf_vehicle_fuel_type_id").ToString()
            }).ToList();
            var ManufacturerYearList = dsVD1.Tables[2].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("vy_vehicle_year"),
                Value = dataRow.Field<int>("vy_vehicle_year_id").ToString()
            }).OrderByDescending(a => a.Text).ToList();
            var RTOList = dsVD1.Tables[7].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("rto_desc"),
                Value = dataRow.Field<int>("rto_id").ToString()
            }).ToList();
            var ClassList = dsVD1.Tables[8].AsEnumerable().Select(dataRow => new SelectListItem()
            {
                Text = dataRow.Field<string>("vcm_desc"),
                Value = dataRow.Field<int>("vcm_id").ToString()
            }).ToList();
            var TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
                                   select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
                                                              ).ToList();


            List<SelectListItem> subtypelist = null;
            List<SelectListItem> catlist = null;

            subtypelist = (from n in _db.tbl_vehicle_subtype_master
                           select new SelectListItem { Value = n.vst_vehicle_subtype_id.ToString(), Text = n.vst_vehicle_subtype_desc }
                                             ).ToList();

            catlist = (from n in _db.tbl_vehicle_category_master
                       select new SelectListItem { Value = n.vc_vehicle_category_id.ToString(), Text = n.vc_vehicle_category_desc }).ToList();

            
            _vehicleDetails.mipd_type_of_cover_list = TypeofCoverList;
            
            _vehicleDetails.VehicleCategoryTypeList = VehicleCategoryTypeList;
            _vehicleDetails.VehicleFuelList = FuelList;
            _vehicleDetails.VehicleTypeOfMakeList = VehicleMakeList;
            //_vehicleDetails.VehicleTypeOfManufactureList = VehicleManufactureList;
            //_vehicleDetails.VehicleTypeOfModelList = VehicleModelList;
            _vehicleDetails.VehicleManufacturerYearList = ManufacturerYearList;
            _vehicleDetails.VehicleRTOList = RTOList;
            _vehicleDetails.VehicleSubTypeList = subtypelist;
            _vehicleDetails.VehicleCategoryList = catlist;
            _vehicleDetails.VehicleTypeOfClassList = ClassList;
            /////////////////////////////////////////

             return _vehicleDetails;
        }

        private VM_MotorInsuranceVehicleDetails BindVehiceDetails(DataSet dsVD)
        {
            if (dsVD.Tables[0].Rows.Count > 0)
            {
                _vehicleDetails.mivd_year_of_manufacturer = dsVD.Tables[0].Rows[0]["mivd_year_of_manufacturer"] == DBNull.Value ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_year_of_manufacturer"]);
                _vehicleDetails.mi_referenceno = Convert.ToInt64(dsVD.Tables[0].Rows[0]["mia_application_ref_no"]);
                _vehicleDetails.mivd_chasis_no = dsVD.Tables[0].Rows[0]["mivd_chasis_no"].ToString().ToUpper();
                _vehicleDetails.mivd_engine_no = dsVD.Tables[0].Rows[0]["mivd_engine_no"].ToString().ToUpper();
                _vehicleDetails.mivd_registration_no = dsVD.Tables[0].Rows[0]["mivd_registration_no"].ToString();
                _vehicleDetails.mivd_date_of_registration = dsVD.Tables[0].Rows[0]["mivd_date_of_registration"] == DBNull.Value ? "" : Convert.ToString(dsVD.Tables[0].Rows[0]["mivd_date_of_registration"]);
                //_vehicleDetails.mivd_date_of_manufacture = dsVD.Tables[0].Rows[0]["mivd_date_of_manufacturer"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dsVD.Tables[0].Rows[0]["mivd_date_of_manufacturer"]);
                _vehicleDetails.mivd_date_of_manufacture = dsVD.Tables[0].Rows[0]["mivd_date_of_manufacturer"].ToString(); 
                if(dsVD.Tables[0].Rows[0]["mivd_date_of_manufacturer"].ToString()!="")
                {
                    DateTime date=Convert.ToDateTime(dsVD.Tables[0].Rows[0]["mivd_date_of_manufacturer"].ToString());
                    _vehicleDetails.mivd_manufacturer_month=Convert.ToString(date.Month);
                    _vehicleDetails.mivd_manufacturer_month_desc = Convert.ToString(date.ToString("MMMM"));
                }
                _vehicleDetails.mivd_seating_capacity_including_driver = dsVD.Tables[0].Rows[0]["mivd_seating_capacity_including_driver"] == DBNull.Value ? (int?)null : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_seating_capacity_including_driver"]);
                _vehicleDetails.mivd_cubic_capacity = dsVD.Tables[0].Rows[0]["mivd_cubic_capacity"] == DBNull.Value ? (int?)null : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_cubic_capacity"]);
                _vehicleDetails.mivd_vehicle_weight = dsVD.Tables[0].Rows[0]["mivd_vehicle_weight"] == null ? (int)0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_vehicle_weight"]);
                _vehicleDetails.mivd_vehicle_fuel_type = dsVD.Tables[0].Rows[0]["mivd_vehicle_fuel_type"] == DBNull.Value ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_vehicle_fuel_type"]);
               // _vehicleDetails.mivd_type_of_model = dsVD.Tables[0].Rows[0]["mivd_type_of_model"] == DBNull.Value ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_type_of_model"]);
                _vehicleDetails.mivd_make_of_vehicle = dsVD.Tables[0].Rows[0]["mivd_make_of_vehicle"] == DBNull.Value ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_make_of_vehicle"]);
                //_vehicleDetails.mivd_manufacture_of_vehicle = dsVD.Tables[0].Rows[0]["mivd_mancfacturer_id"] == DBNull.Value ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_mancfacturer_id"]);
                _vehicleDetails.mivd_vehicle_type_id = Convert.ToString(dsVD.Tables[0].Rows[0]["mivd_vehicle_type"]);
                _vehicleDetails.mivd_vehicle_subtype_id = Convert.ToString(dsVD.Tables[0].Rows[0]["mivd_vehicle_subtype"]);
                _vehicleDetails.mivd_vehicle_category_id = Convert.ToString(dsVD.Tables[0].Rows[0]["mivd_vehicle_category"]);

                _vehicleDetails.year = dsVD.Tables[0].Rows[0]["mivd_year_of_manufacturer"].ToString();
                _vehicleDetails.VehicleMake = dsVD.Tables[0].Rows[0]["mivd_make_of_vehicle"].ToString();
               // _vehicleDetails.VehicleManufacture = dsVD.Tables[0].Rows[0]["mivd_mancfacturer_id"].ToString();
                _vehicleDetails.VehicleFuel = dsVD.Tables[0].Rows[0]["mivd_vehicle_fuel_type"].ToString();
                _vehicleDetails.mivd_type_of_model = dsVD.Tables[0].Rows[0]["mivd_type_of_model"].ToString();
                _vehicleDetails.mivd_Zone= dsVD.Tables[0].Rows[0]["p_mi_Zone"].ToString();
                _vehicleDetails.yeardesc = (dsVD.Tables[0].Rows[0]["vy_vehicle_year"] == "") ? "" : dsVD.Tables[0].Rows[0]["vy_vehicle_year"].ToString();
                _vehicleDetails.VehicleMakedesc = (dsVD.Tables[0].Rows[0]["vm_vehicle_make_desc"] == "") ? "" : dsVD.Tables[0].Rows[0]["vm_vehicle_make_desc"].ToString();
                _vehicleDetails.VehicleFueldesc = (dsVD.Tables[0].Rows[0]["vf_vehicle_fuel_type_desc"] == "") ? "" : dsVD.Tables[0].Rows[0]["vf_vehicle_fuel_type_desc"].ToString();
               // _vehicleDetails.VehicleManufacturedesc = (dsVD.Tables[0].Rows[0]["vm_vehicle_manufacture_desc"] == "") ? "" : dsVD.Tables[0].Rows[0]["vm_vehicle_manufacture_desc"].ToString();
               // _vehicleDetails.VehicleModeldesc = (dsVD.Tables[0].Rows[0]["vm_vehicle_model_desc"] == "") ? "" : dsVD.Tables[0].Rows[0]["vm_vehicle_model_desc"].ToString();
                _vehicleDetails.VehicleTypedesc = (dsVD.Tables[0].Rows[0]["vht_vehicle_type_desc"] == "") ? "" : dsVD.Tables[0].Rows[0]["vht_vehicle_type_desc"].ToString();
                _vehicleDetails.VehicleSubTypedesc = (dsVD.Tables[0].Rows[0]["vst_vehicle_subtype_desc"] == "") ? "" : dsVD.Tables[0].Rows[0]["vst_vehicle_subtype_desc"].ToString();
                _vehicleDetails.VehicleCategorydesc = (dsVD.Tables[0].Rows[0]["vc_vehicle_category_desc"] == "") ? "" : dsVD.Tables[0].Rows[0]["vc_vehicle_category_desc"].ToString();
                _vehicleDetails.VehicleRTOdesc = (dsVD.Tables[0].Rows[0]["rto_desc"] == "") ? "" : dsVD.Tables[0].Rows[0]["rto_desc"].ToString();
                _vehicleDetails.mivd_own_damage_value = (dsVD.Tables[0].Rows[0]["mivd_own_damage_value"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsVD.Tables[0].Rows[0]["mivd_own_damage_value"]);
                _vehicleDetails.mivd_premium_liability_value = (dsVD.Tables[0].Rows[0]["mivd_premium_liability_value"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsVD.Tables[0].Rows[0]["mivd_premium_liability_value"]);
                _vehicleDetails.mivd_vehicle_min_value = (dsVD.Tables[0].Rows[0]["mivd_vehicle_min_value"] == DBNull.Value) ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_vehicle_min_value"]);
                _vehicleDetails.mivd_malus_value = (dsVD.Tables[0].Rows[0]["mivd_malus_value"] == DBNull.Value) ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_malus_value"]);
                if(_vehicleDetails.mivd_malus_value==0)
                {
                    _vehicleDetails.mivd_ncb_value = (dsVD.Tables[0].Rows[0]["mivd_no_claim_value"] == DBNull.Value) ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_no_claim_value"]);
                    _vehicleDetails.mivd_ncb_id = (dsVD.Tables[0].Rows[0]["mivd_no_claim_id"] == DBNull.Value) ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_no_claim_id"]);


                }

                _vehicleDetails.mivd_Depreciation_value = (dsVD.Tables[0].Rows[0]["Depreciation"] == DBNull.Value) ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["Depreciation"]);

                _vehicleDetails.mivd_own_damage_id = (dsVD.Tables[0].Rows[0]["mivd_own_damage_id"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsVD.Tables[0].Rows[0]["mivd_own_damage_id"]);
                _vehicleDetails.mivd_vehicle_rto_id = (dsVD.Tables[0].Rows[0]["mivd_rto_id"] == DBNull.Value) ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_rto_id"]);
                _vehicleDetails.mivd_premium_liability_id = (dsVD.Tables[0].Rows[0]["mivd_premium_liability_id"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsVD.Tables[0].Rows[0]["mivd_premium_liability_id"]);
                _vehicleDetails.mivd_vehicle_min_id = (dsVD.Tables[0].Rows[0]["mivd_vehicle_min_id"] == DBNull.Value) ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_vehicle_min_id"]);
                _vehicleDetails.mivd_malus_id = (dsVD.Tables[0].Rows[0]["mivd_malus_id"] == DBNull.Value) ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["mivd_malus_id"]);
                _vehicleDetails.mivd_Depreciation_id = (dsVD.Tables[0].Rows[0]["Depreciation_id"] == DBNull.Value) ? 0 : Convert.ToInt32(dsVD.Tables[0].Rows[0]["Depreciation_id"]);
                _vehicleDetails.mivd_Additionalamt = (dsVD.Tables[0].Rows[0]["mivd_OD_additional_Amount"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsVD.Tables[0].Rows[0]["mivd_OD_additional_Amount"]);
                _vehicleDetails.mivd_govDiscount = (dsVD.Tables[0].Rows[0]["mivd_gov_discount"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsVD.Tables[0].Rows[0]["mivd_gov_discount"]);
                _vehicleDetails.mivd_PLgovDiscount = (dsVD.Tables[0].Rows[0]["mivd_PLgovDiscount"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsVD.Tables[0].Rows[0]["mivd_PLgovDiscount"]);
                _vehicleDetails.mivd_PLDriverAmt = (dsVD.Tables[0].Rows[0]["mivd_PLDriverAmt"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsVD.Tables[0].Rows[0]["mivd_PLDriverAmt"]);
                _vehicleDetails.mivd_PLPassengerAmt = (dsVD.Tables[0].Rows[0]["mivd_PLPassengerAmt"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsVD.Tables[0].Rows[0]["mivd_PLPassengerAmt"]);
                _vehicleDetails.mivd_vehicle_category_type_id = Convert.ToInt32(dsVD.Tables[0].Rows[0]["vct_vehicle_category_type_id"]);
                _vehicleDetails.VehicleCategoryTypedesc =Convert.ToString(dsVD.Tables[0].Rows[0]["vct_vehicle_category_type_desc"]);
                _vehicleDetails.mivd_vehicle_class_id = Convert.ToInt32(dsVD.Tables[0].Rows[0]["vcm_id"]);
                _vehicleDetails.VehicleClassdesc =Convert.ToString(dsVD.Tables[0].Rows[0]["vcm_desc"]);
                _vehicleDetails.mipd_type_of_cover_id = Convert.ToInt32(dsVD.Tables[0].Rows[0]["mia_type_of_cover"]);
                _vehicleDetails.mipd_type_of_cover = Convert.ToString(dsVD.Tables[0].Rows[0]["type_of_cover"]);
                //_vehicleDetails.VehiclePolicyMonth= Convert.ToInt32(dsVD.Tables[0].Rows[0]["PolicyMonths"]);



                _vehicleDetails.VehicleIDVAmount = dsVD.Tables[0].Rows[0]["mivd_vahanidvamount"].ToString();
            }

            return _vehicleDetails;
        }

       public VM_MotorInsuranceVehicleDetails BindVahanResponseDetailstoModel(dynamic responseStr)
        {
            DataSet dsVD1 = new DataSet();
            VM_MotorInsuranceVehicleDetails obj = new VM_MotorInsuranceVehicleDetails();
            SqlParameter[] parms1 = {

            };
           dsVD1 = _Conn.ExeccuteDataset(parms1, "sp_kgid_getMultipleVehicleDetailsList");
         
            if (obj.VehicleCategoryTypeList.Count==0)
            {
                var VehicleTypeOfCategoryList = dsVD1.Tables[0].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vct_vehicle_category_type_desc"),
                    Value = dataRow.Field<int>("vct_vehicle_category_type_id").ToString()
                }).ToList();
                obj.VehicleCategoryTypeList = VehicleTypeOfCategoryList;
            }
            if (obj.VehicleTypeOfMakeList.Count == 0)
            {
                var VehicleMakeList = dsVD1.Tables[1].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vm_vehicle_make_desc"),
                    Value = dataRow.Field<int>("vm_vehicle_make_id").ToString()
                }).ToList();
                obj.VehicleTypeOfMakeList = VehicleMakeList;
            }
            if (obj.VehicleManufacturerYearList== null)
            {
                var VehicleyearList = dsVD1.Tables[2].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vy_vehicle_year"),
                    Value = dataRow.Field<int>("vy_vehicle_year_id").ToString()
                }).OrderByDescending(a=>a.Text).ToList();
                obj.VehicleManufacturerYearList = VehicleyearList;
            }
            if (obj.VehicleFuelList.Count==0)
            {
                var VehicleFuelList = dsVD1.Tables[3].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vf_vehicle_fuel_type_desc"),
                    Value = dataRow.Field<int>("vf_vehicle_fuel_type_id").ToString()
                }).ToList();
                obj.VehicleFuelList = VehicleFuelList;
            }

            if (obj.VehicleTypeList == null)
            {
                var VehicleTypeList = dsVD1.Tables[4].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vht_vehicle_type_desc"),
                    Value = dataRow.Field<long>("vht_vehicle_type_id").ToString()
                }).ToList();
                obj.VehicleTypeList = VehicleTypeList;
            }

            if (obj.VehicleSubTypeList.Count == 0)
            {
                var VehicleSubTypeList = dsVD1.Tables[5].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vst_vehicle_subtype_desc"),
                    Value = dataRow.Field<long>("vst_vehicle_subtype_id").ToString()
                }).ToList();
                obj.VehicleSubTypeList = VehicleSubTypeList;
            }

            if (obj.VehicleCategoryList == null || obj.VehicleCategoryList.Count==0)
            {
                var VehicleCategoryList = dsVD1.Tables[6].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vc_vehicle_category_desc"),
                    Value = dataRow.Field<int>("vc_vehicle_category_id").ToString()
                }).ToList();
                obj.VehicleCategoryList = VehicleCategoryList;
            }
            if (obj.VehicleTypeOfClassList.Count==0)
            {
                var VehicleTypeOfClassList = dsVD1.Tables[8].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vcm_desc"),
                    Value = dataRow.Field<int>("vcm_id").ToString()
                }).ToList();
                obj.VehicleTypeOfClassList = VehicleTypeOfClassList;
            }
            var TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
                                   select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
                                                              ).ToList();
            


            foreach (var item in responseStr)
            {
                if(item.Value[0].ContainsKey("chasis_no"))
                {
                    obj.mivd_chasis_no = item.Value[0].chasis_no.Value;
                }
                else
                {

                    obj.VehicleCategoryTypedesc = item.Value[0].Vehicle_category_type.Value;
                    obj.VehicleMakedesc = item.Value[0].MAKE_OF_VEHICLE.Value;
                    obj.mivd_type_of_model = item.Value[0].MODEL_OF_VEHICLE.Value;
                    obj.mivd_manufacturer_month = item.Value[0].MONTH_OF_MANUFACTURE.Value;
                    obj.yeardesc = item.Value[0].YEAR_OF_MANUFACTURE.Value;
                    obj.VehicleFueldesc = item.Value[0].Veh_Fuel_Type.Value;
                    obj.VehicleTypedesc = item.Value[0].Vehicle_Type.Value;
                    string VaahanVehicleCategoryType= item.Value[0].Vehicle_category_type.Value;
                    if (VaahanVehicleCategoryType == "TWO WHEELER(NT)" || VaahanVehicleCategoryType == "TWO WHEELER(T)"|| VaahanVehicleCategoryType == "TWO WHEELER (Invalid Carriage)")
                    {
                        obj.VehicleSubTypedesc = "Two wheeler";
                    }
                    else
                    {
                        obj.VehicleSubTypedesc = "Non-two wheeler";
                    }
                    string VaahanTypeofvehicle = item.Value[0].TYPE_OF_VEHICLE.Value;
                    if (VaahanTypeofvehicle == "Motor Cycle/Scooter-Used For Hire")
                    {
                        obj.VehicleCategorydesc = "Two wheeler(Public)";
                    }
                    else if (VaahanTypeofvehicle == "M-Cycle/Scooter" || VaahanTypeofvehicle == "Moped" || VaahanTypeofvehicle == "Motorised Cycle (CC > 25cc)" || VaahanTypeofvehicle == "Motor Cycle/Scooter-With Trailer" || VaahanTypeofvehicle == "Invalid Carriage")
                    {
                        if(obj.VehicleSubTypedesc== "Non-two wheeler")
                        {
                            obj.VehicleCategorydesc = "Car/Jeep";
                        }
                        else
                        {
                            obj.VehicleCategorydesc = "Two wheeler(Private)";
                        }
                                             
                    }
                    else if (VaahanTypeofvehicle == "M-Cycle/Scooter-With Side Car" || VaahanTypeofvehicle == "Motor Cycle/Scooter-SideCar(T)")
                    {
                        obj.VehicleCategorydesc = "Two wheeler + side car";
                    }
                    else if (VaahanTypeofvehicle == "Goods Carrier")
                    {
                        obj.VehicleCategorydesc = "Goods carrying vehicle (Public)-A1";
                    }
                    else if (VaahanTypeofvehicle == "e-Rickshaw with Cart (G)" || VaahanTypeofvehicle == "Three Wheeler (Goods)")
                    {
                        obj.VehicleCategorydesc = "Goods carrying vehicle(3-wheeler)(Public)-A3";
                    }
                    else if (VaahanTypeofvehicle == "e-Rickshaw(P)")
                    {
                        obj.VehicleCategorydesc = "Goods carrying vehicle(3-wheeler)(Private)-A4";
                    }
                    else if ( VaahanTypeofvehicle == "Three Wheeler (Personal)" || VaahanTypeofvehicle == "Three Wheeler (Passenger)")
                    {
                        obj.VehicleCategorydesc = "3 wheeler passenger";
                    }
                    else if (VaahanTypeofvehicle == "Quadricycle (Commercial)" )
                    {
                        obj.VehicleCategorydesc = "4 wheeler upto 6 passengers(Public)";
                       
                    }
                    else if(VaahanTypeofvehicle == "Motor Cab"  || VaahanTypeofvehicle == "Maxi Cab" || VaahanTypeofvehicle == "Luxury Cab")
                    {
                        if (Convert.ToInt32(item.Value[0].SEATING_CAPACITY.Value) <= 6)
                        {
                            obj.VehicleCategorydesc = "4 wheeler upto 6 passengers(Public)";
                        }
                        else
                        {
                            obj.VehicleCategorydesc = "4 wheeler and higher(more than 6 passengers)-C2";
                        }
                    }

                    else if (VaahanTypeofvehicle == "Omni Bus (Private Use)" || VaahanTypeofvehicle == "Omni Bus(Private Use)"|| VaahanTypeofvehicle == "Bus"||VaahanTypeofvehicle == "Educational Institution Bus"||VaahanTypeofvehicle == "Omni Bus")
                    {
                        obj.VehicleCategorydesc = "4 wheeler and higher(more than 6 passengers)-C2";
                    }
                    else if (VaahanTypeofvehicle == "Motor Car" || VaahanTypeofvehicle == "Private Service Vehicle (Individual Use)" || VaahanTypeofvehicle == "Quadricycle (Private)" || VaahanTypeofvehicle == "Cash Van")
                    {
                        obj.VehicleCategorydesc = "Car/Jeep";
                        
                    }
                   else if(VaahanTypeofvehicle== "Trailer (Agricultural)" || VaahanTypeofvehicle == "Trailer For Personal Use" || VaahanTypeofvehicle == "Camper Van / Trailer"||VaahanTypeofvehicle == "Auxiliary Trailer"||VaahanTypeofvehicle == "Trailer(Commercial)"||VaahanTypeofvehicle == "Tractor - Trolley(Commercial)"||VaahanTypeofvehicle == "Semi - Trailer(Commercial)" || VaahanTypeofvehicle == "Trailer (Commercial)")
                    {
                        obj.VehicleCategorydesc = "Trailers";
                    }
                    else
                    {
                        obj.VehicleCategorydesc = "Special type of vehicles (Class D)";
                    }
                    //obj.VehicleSubTypedesc = item.Value[0].Veh_Fuel_Type.Value;
                    //obj.VehicleCategorydesc = item.Value[0].Vehicle_category_type.Value;
                    obj.VehicleClassdesc = item.Value[0].TYPE_OF_VEHICLE.Value;

                    obj.mivd_vehicle_rto_id = Convert.ToInt64(item.Value[0].OFFICE_CODE.Value);
                    obj.mivd_chasis_no = item.Value[0].CHASI_NUMBER.Value;
                    obj.mivd_engine_no = item.Value[0].ENGINE_NUMBER.Value;
                    
                    
                    obj.mivd_cubic_capacity = Convert.ToInt32(Math.Round(Convert.ToDecimal(item.Value[0].CUBIC_CAPACITY.Value)));
                    obj.mivd_seating_capacity_including_driver = Convert.ToInt32(item.Value[0].SEATING_CAPACITY.Value);
                    obj.mivd_vehicle_weight = Convert.ToInt32(item.Value[0].LADEN_WEIGHT.Value);
                    
                    obj.mivd_date_of_registration = (item.Value[0].Registration_date.Value);
                    obj.VehicleIDVAmount = item.Value[0].IDV_Details.Value;
                    obj.VehicleSaleAmount = item.Value[0].Sale_Amount.Value;
                    obj.mivd_vehicle_reg_no = item.Value[0].Registration_Number.Value;
                    obj.mipd_type_of_cover_list = TypeofCoverList;

                    //if (obj.mivd_vehicle_reg_no != "" && (obj.VehicleCategorydesc == "4 wheeler upto 6 passengers(Private)" || obj.VehicleCategorydesc == "Two wheeler(Private)"))
                    //{
                    //    obj.mipd_type_of_cover_list = (from n in _db.tbl_motor_insurance_type_of_cover
                    //                                   select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
                    //                                      ).ToList().Where(a => a.Value == "3").Select(a => a).ToList();
                    //    obj.mipd_type_of_cover_id = 3;
                    //    obj.mipd_type_of_cover = "Bundle Policy";
                    //}
                    //else /*if (obj.mivd_vehicle_reg_no == "" && (obj.VehicleCategorydesc == "4 wheeler upto 6 passengers(Private)" || obj.VehicleCategorydesc == "Two wheeler(Private)"))*/
                    //{
                    //    obj.mipd_type_of_cover_list = (from n in _db.tbl_motor_insurance_type_of_cover
                    //                                   select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
                    //                                          ).ToList().Where(a => a.Value != "3").Select(a => a).ToList();
                    //    obj.mipd_type_of_cover_id = 2;
                    //    obj.mipd_type_of_cover = "Package Policy";
                    //}
                    //else
                    //{
                    //    obj.mipd_type_of_cover_list = TypeofCoverList.Select(a => a).ToList();
                    //    obj.mipd_type_of_cover_id = 0;
                    //}



                }
            }
            return obj;
        }

        public string GetVehicleDetailsId(string VehicleMakedesc,string VehicleModeldesc,string VehicleManufacture,string yeardesc)
        {
            SqlParameter[] sqlparam =
                       {
             new SqlParameter("@VehicleMakedesc",VehicleMakedesc),
             new SqlParameter("@VehicleModeldesc",VehicleModeldesc),
             new SqlParameter("@VehicleManufacture",VehicleManufacture),
             new SqlParameter("@yeardesc",yeardesc)
            };
           // result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertMIIDVdetails"));

            return "";
        }
        //=========================Others Details related functions ================================
        public VM_MotorInsuranceOtherDetails OtherDetailsDll(long EmployeeCode, long ReferenceId)
        {
            VM_MotorInsuranceOtherDetails objPD = new VM_MotorInsuranceOtherDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",EmployeeCode),
                 new SqlParameter("@referenceid",ReferenceId)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectMIOtherDetails");
                bool OtherData = false;
                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPD.Tables[0].Rows)
                        {
                            if (!OtherData)
                            {
                                OtherData = true;
                                objPD.miod_emp_id = EmployeeCode;
                                objPD.miod_application_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["vdr_application_id"]);
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 101 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_vehicle_type_id = Convert.ToInt32(dr["vdr_response_details1"].ToString());
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 1 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_non_conventioanal_source = true;
                                objPD.miod_is_non_conventioanal_source_details = dr["vdr_response_details1"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 2 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_driving_tuitions = true;
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 3 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_geographical = true;
                                objPD.miod_geographical_ext1 = dr["vdr_response_details1"].ToString();
                                objPD.miod_geographical_ext2 = dr["vdr_response_details2"].ToString();
                                objPD.miod_geographical_ext3 = dr["vdr_response_details3"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 4 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_own_premises = true;
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 5 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_commercial_purpose = true;
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 6 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_foreign_embasy = true;
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 7 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_vintage_car = true;
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 8 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_for_blind_or_ph = true;
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 9 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_fibre_glass_tank = true;
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 10 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_bi_fuel_system = true;
                                objPD.miod_bi_fuel_amount = dr["vdr_response_details1"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 11 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_higher_deductible = true;
                                objPD.miod_higher_deductible_amount = dr["vdr_response_details1"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 12 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_automobile_association_of_india = true;
                                objPD.miod_name_of_association = dr["vdr_response_details1"].ToString();
                                objPD.miod_membership_no = dr["vdr_response_details2"].ToString();
                                objPD.miod_date_of_expiry = dr["vdr_response_details3"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 13 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_cover_legal_liability = true;
                                objPD.miod_cll_driver_conductor_count = dr["vdr_response_details1"].ToString();
                                objPD.miod_cll_other_emp_count = dr["vdr_response_details2"].ToString();
                                objPD.miod_cll_non_fare_passengers_count = dr["vdr_response_details3"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 14 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_no_claim_bonus = true;
                                objPD.Miod_is_no_claim_bonus_doc_filename = dr["vdr_response_details1"].ToString();
                                //objPD.miod_is_no_claim_bonus_doc = dr["vdr_response_details1"].ToString();
                                //objPD.miod_cll_other_emp_count = dr["vdr_response_details2"].ToString();
                                //objPD.miod_cll_non_fare_passengers_count = dr["vdr_response_details3"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 15 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_liability_third_parties = true;
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 16 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_higher_towing_charges = true;
                                objPD.miod_is_higher_towing_charges_amount = dr["vdr_response_details1"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 17 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_include_personal_accident = true;
                                objPD.miod_pa_driver_conductor_count = dr["vdr_response_details1"].ToString();
                                objPD.miod_pa_other_emp_count = dr["vdr_response_details2"].ToString();
                                objPD.miod_pa_unnamed_passengers_count = dr["vdr_response_details3"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 18 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_include_personal_accident_for_persons = true;
                                objPD.miod_ipap_name1 = dr["vdr_response_details1"].ToString();
                                objPD.miod_ipap_name1_amount = dr["vdr_response_details2"].ToString();
                                objPD.miod_ipap_name2 = dr["vdr_response_details3"].ToString();
                                objPD.miod_ipap_name2_amount = dr["vdr_response_details4"].ToString();
                                objPD.miod_ipap_name3 = dr["vdr_response_details5"].ToString();
                                objPD.miod_ipap_name3_amount = dr["vdr_response_details6"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 19 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_include_pa_cover_for_unnamed_persons = true;
                                objPD.miod_ipaun_name1 = dr["vdr_response_details1"].ToString();
                                objPD.miod_ipaun_name1_amount = dr["vdr_response_details2"].ToString();
                                objPD.miod_ipaun_name2 = dr["vdr_response_details3"].ToString();
                                objPD.miod_ipaun_name2_amount = dr["vdr_response_details4"].ToString();
                                objPD.miod_ipaun_name3 = dr["vdr_response_details5"].ToString();
                                objPD.miod_ipaun_name3_amount = dr["vdr_response_details6"].ToString();
                            }
                            if (Convert.ToInt64(dr["vdr_other_details_id"]) == 20 && Convert.ToBoolean(dr["vdr_response"]) == true)
                            {
                                objPD.miod_is_anti_theft = true;
                                objPD.Miod_is_anti_theft_doc_filename = dr["vdr_response_details1"].ToString();
                                //objPD.miod_is_no_claim_bonus_doc = dr["vdr_response_details1"].ToString();
                                //objPD.miod_cll_other_emp_count = dr["vdr_response_details2"].ToString();
                                //objPD.miod_cll_non_fare_passengers_count = dr["vdr_response_details3"].ToString();
                            }



                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }

        public int SaveMIOtherDetailsDll(VM_MotorInsuranceOtherDetails objPersonal)
        {
            int result = 0;
            dtOtherData.Columns.Add("vdr_application_id");
            dtOtherData.Columns.Add("vdr_emp_id");
            dtOtherData.Columns.Add("vdr_other_details_id");
            dtOtherData.Columns.Add("vdr_response");
            dtOtherData.Columns.Add("vdr_response_details1");
            dtOtherData.Columns.Add("vdr_response_details2");
            dtOtherData.Columns.Add("vdr_response_details3");
            dtOtherData.Columns.Add("vdr_response_details4");
            dtOtherData.Columns.Add("vdr_response_details5");
            dtOtherData.Columns.Add("vdr_response_details6");
            dtOtherData.Columns.Add("vdr_status");
            dtOtherData.Columns.Add("vdr_active");
            DataTable dt_otherdata = CreateDataTable("MIOtherDetailsResponseData");
            if (objPersonal.miod_is_non_conventioanal_source == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 1, objPersonal.miod_is_non_conventioanal_source, objPersonal.miod_is_non_conventioanal_source_details, "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 1, objPersonal.miod_is_non_conventioanal_source, objPersonal.miod_is_non_conventioanal_source_details, "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_driving_tuitions == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 2, objPersonal.miod_is_driving_tuitions, "", "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 2, objPersonal.miod_is_driving_tuitions, "", "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_geographical == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 3, objPersonal.miod_is_geographical, objPersonal.miod_geographical_ext1, objPersonal.miod_geographical_ext2, objPersonal.miod_geographical_ext3, "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 3, objPersonal.miod_is_geographical, objPersonal.miod_geographical_ext1, objPersonal.miod_geographical_ext2, objPersonal.miod_geographical_ext3, "", "", "", true, true);
            }
            if (objPersonal.miod_is_own_premises == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 4, objPersonal.miod_is_own_premises, "", "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 4, objPersonal.miod_is_own_premises, "", "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_commercial_purpose == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 5, objPersonal.miod_is_commercial_purpose, "", "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 5, objPersonal.miod_is_commercial_purpose, "", "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_foreign_embasy == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 6, objPersonal.miod_is_foreign_embasy, "", "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 6, objPersonal.miod_is_foreign_embasy, "", "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_vintage_car == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 7, objPersonal.miod_is_vintage_car, "", "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 7, objPersonal.miod_is_vintage_car, "", "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_for_blind_or_ph == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 8, objPersonal.miod_is_for_blind_or_ph, "", "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 8, objPersonal.miod_is_for_blind_or_ph, "", "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_fibre_glass_tank == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 9, objPersonal.miod_is_fibre_glass_tank, "", "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 9, objPersonal.miod_is_fibre_glass_tank, "", "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_bi_fuel_system == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 10, objPersonal.miod_is_bi_fuel_system, objPersonal.miod_bi_fuel_amount, "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 10, objPersonal.miod_is_bi_fuel_system, objPersonal.miod_bi_fuel_amount, "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_higher_deductible == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 11, objPersonal.miod_is_higher_deductible, objPersonal.miod_higher_deductible_amount, "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 11, objPersonal.miod_is_higher_deductible, objPersonal.miod_higher_deductible_amount, "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_automobile_association_of_india == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 12, objPersonal.miod_is_automobile_association_of_india, objPersonal.miod_name_of_association, objPersonal.miod_membership_no, objPersonal.miod_date_of_expiry, "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 12, objPersonal.miod_is_automobile_association_of_india, objPersonal.miod_name_of_association, objPersonal.miod_membership_no, objPersonal.miod_date_of_expiry, "", "", "", true, true);
            }
            if (objPersonal.miod_is_cover_legal_liability == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 13, objPersonal.miod_is_cover_legal_liability, objPersonal.miod_cll_driver_conductor_count, objPersonal.miod_cll_other_emp_count, objPersonal.miod_cll_non_fare_passengers_count, "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 13, objPersonal.miod_is_cover_legal_liability, objPersonal.miod_cll_driver_conductor_count, objPersonal.miod_cll_other_emp_count, objPersonal.miod_cll_non_fare_passengers_count, "", "", "", true, true);
            }
            if (objPersonal.miod_is_no_claim_bonus == true)
            {
                //UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 14, objPersonal.miod_is_no_claim_bonus, "", "", "", "", "", "", true, true);
                SaveMIODFileData(objPersonal.miod_emp_id, objPersonal.miod_application_id, 14, objPersonal.miod_is_no_claim_bonus, "", "", "", "", "", "", true, true, objPersonal.miod_is_no_claim_bonus_doc, objPersonal.Miod_is_no_claim_bonus_doc_filename, "MI NCB");
            }
            else
            {
                SaveMIODFileData(objPersonal.miod_emp_id, objPersonal.miod_application_id, 14, objPersonal.miod_is_no_claim_bonus, "", "", "", "", "", "", true, true, objPersonal.miod_is_no_claim_bonus_doc, objPersonal.Miod_is_no_claim_bonus_doc_filename, "MI NCB");
            }
            if (objPersonal.miod_is_liability_third_parties == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 15, objPersonal.miod_is_liability_third_parties, "", "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 15, objPersonal.miod_is_liability_third_parties, "", "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_higher_towing_charges == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 16, objPersonal.miod_is_higher_towing_charges, objPersonal.miod_is_higher_towing_charges_amount, "", "", "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 16, objPersonal.miod_is_higher_towing_charges, objPersonal.miod_is_higher_towing_charges_amount, "", "", "", "", "", true, true);
            }
            if (objPersonal.miod_is_include_personal_accident == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 17, objPersonal.miod_is_include_personal_accident, objPersonal.miod_pa_driver_conductor_count, objPersonal.miod_pa_other_emp_count, objPersonal.miod_pa_unnamed_passengers_count, "", "", "", true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 17, objPersonal.miod_is_include_personal_accident, objPersonal.miod_pa_driver_conductor_count, objPersonal.miod_pa_other_emp_count, objPersonal.miod_pa_unnamed_passengers_count, "", "", "", true, true);
            }
            if (objPersonal.miod_is_include_personal_accident_for_persons == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 18, objPersonal.miod_is_include_personal_accident_for_persons, objPersonal.miod_ipap_name1, objPersonal.miod_ipap_name1_amount, objPersonal.miod_ipap_name2, objPersonal.miod_ipap_name2_amount, objPersonal.miod_ipap_name3, objPersonal.miod_ipap_name3_amount, true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 18, objPersonal.miod_is_include_personal_accident_for_persons, objPersonal.miod_ipap_name1, objPersonal.miod_ipap_name1_amount, objPersonal.miod_ipap_name2, objPersonal.miod_ipap_name2_amount, objPersonal.miod_ipap_name3, objPersonal.miod_ipap_name3_amount, true, true);
            }
            if (objPersonal.miod_is_include_pa_cover_for_unnamed_persons == true)
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 19, objPersonal.miod_is_include_pa_cover_for_unnamed_persons, objPersonal.miod_ipaun_name1, objPersonal.miod_ipaun_name1_amount, objPersonal.miod_ipaun_name2, objPersonal.miod_ipaun_name2_amount, objPersonal.miod_ipaun_name3, objPersonal.miod_ipaun_name3_amount, true, true);
            }
            else
            {
                UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 19, objPersonal.miod_is_include_pa_cover_for_unnamed_persons, objPersonal.miod_ipaun_name1, objPersonal.miod_ipaun_name1_amount, objPersonal.miod_ipaun_name2, objPersonal.miod_ipaun_name2_amount, objPersonal.miod_ipaun_name3, objPersonal.miod_ipaun_name3_amount, true, true);
            }
            if (objPersonal.miod_is_anti_theft == true)
            {
                //UpdateDeatils(objPersonal.miod_emp_id, objPersonal.miod_application_id, 14, objPersonal.miod_is_no_claim_bonus, "", "", "", "", "", "", true, true);
                SaveMIODFileData(objPersonal.miod_emp_id, objPersonal.miod_application_id, 20, objPersonal.miod_is_anti_theft, "", "", "", "", "", "", true, true, objPersonal.miod_is_anti_theft_doc, objPersonal.Miod_is_anti_theft_doc_filename, "Antitheft");
            }
            else
            {
                SaveMIODFileData(objPersonal.miod_emp_id, objPersonal.miod_application_id, 20, objPersonal.miod_is_anti_theft, "", "", "", "", "", "", true, true, objPersonal.miod_is_anti_theft_doc, objPersonal.Miod_is_anti_theft_doc_filename, "Antitheft");
            }

            SqlParameter[] sqlparam =
            {
             new SqlParameter("@EmpID",objPersonal.miod_emp_id),
             new SqlParameter("@RefNo",objPersonal.miod_application_id),
             new SqlParameter("@MIOtherDetailsResponseData",dtOtherData)
            };
            result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertMIOtherdetails"));
            return result;
        }

        public void UpdateDeatils(long? EmpCode, long? ApplicationID, long OtherDetailsID, bool response, string response1, string response2, string response3, string response4, string response5, string response6, bool status, bool active)
        {
            DataRow Ddr = dtOtherData.NewRow();
            Ddr["vdr_application_id"] = ApplicationID ?? 0;
            Ddr["vdr_emp_id"] = EmpCode ?? 0;
            Ddr["vdr_other_details_id"] = OtherDetailsID;
            Ddr["vdr_response"] = response;
            Ddr["vdr_response_details1"] = response1;
            Ddr["vdr_response_details2"] = response2;
            Ddr["vdr_response_details3"] = response3;
            Ddr["vdr_response_details4"] = response4;
            Ddr["vdr_response_details5"] = response5;
            Ddr["vdr_response_details6"] = response6;
            Ddr["vdr_status"] = true;
            Ddr["vdr_active"] = true;
            dtOtherData.Rows.Add(Ddr);
            //return dt_otherdata;
        }

        public void SaveMIODFileData(long? EmpCode, long? ApplicationID, long OtherDetailsID, bool? response, string response1, string response2, string response3, string response4, string response5, string response6, bool status, bool active, HttpPostedFileBase MIODDoc, string emd_upload_document_path, string miodDocType)
        {
            try
            {
                string path = "";
                //if (string.IsNullOrEmpty(emd_upload_document_path))
                //{
                //    emd_upload_document_path = UploadDocument(MIODDoc, EmpCode, miodDocType);
                //}
                DataRow Ddr = dtOtherData.NewRow();
                Ddr["vdr_application_id"] = ApplicationID ?? 0;
                Ddr["vdr_emp_id"] = EmpCode ?? 0;
                Ddr["vdr_other_details_id"] = OtherDetailsID;
                Ddr["vdr_response"] = response;
                if (MIODDoc != null)
                    path = MIUploadDocument(MIODDoc, EmpCode, miodDocType);
                else
                    path = emd_upload_document_path;
                Ddr["vdr_response_details1"] = path;
                Ddr["vdr_response_details2"] = response2;
                Ddr["vdr_response_details3"] = response3;
                Ddr["vdr_response_details4"] = response4;
                Ddr["vdr_response_details5"] = response5;
                Ddr["vdr_response_details6"] = response6;
                Ddr["vdr_status"] = true;
                Ddr["vdr_active"] = true;
                dtOtherData.Rows.Add(Ddr);
            }
            catch (Exception ex)
            {

            }
        }

        private string UploadDocument(HttpPostedFileBase document, long? empId, string docType)
        {
            string subPath = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
                subPath = "/Documents/" + empId.ToString() + "/" + docType;
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(subPath));
                if (!exists)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                }

                string path = Path.Combine(HttpContext.Current.Server.MapPath(subPath), fileName);
                document.SaveAs(path);
                subPath = subPath + "/" + fileName;
            }

            return subPath;
        }

        //=========================IDV  related functions ================================
        public VM_MotorInsuranceIDVDetails IDVDetailsDll(long EmployeeCode, long ReferenceId)
        {
            VM_MotorInsuranceIDVDetails objPD = new VM_MotorInsuranceIDVDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",EmployeeCode),
                new SqlParameter("@referenceid",ReferenceId)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectMIIDVDetails");
                bool OtherData = false;
                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPD.Tables[0].Rows)
                        {
                            if (!OtherData)
                            {
                                OtherData = true;
                                objPD.miidv_emp_id = EmployeeCode;
                                objPD.miidv_application_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["vid_motor_insurance_app_id"]);
                                objPD.miidv_idv_premium_amount= Convert.ToInt64(dsPD.Tables[0].Rows[0]["premium"]);

                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 1)
                            {
                                objPD.miidv_vaahanidvamount = Convert.ToInt64(dr["vid_amount"]).ToString();
                                //objPD.midv_own_damage_value = Convert.ToDecimal(dr["owndamage"]);
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 2)
                            {
                                objPD.miidv_insured_redeclared_value_amount = Convert.ToInt64(dr["vid_amount"]).ToString();
                                //objPD.midv_own_damage_value = Convert.ToDecimal(dr["owndamage"]);
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 2)
                            {
                                objPD.miidv_insured_declared_value_amount = Convert.ToInt64(dr["vid_amount"]).ToString();
                                //objPD.midv_own_damage_value = Convert.ToDecimal(dr["owndamage"]);
                            }
                            
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 3)
                            {
                                objPD.miidv_non_electrical_accessories_amount = Convert.ToInt64(dr["vid_amount"]).ToString();
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 4)
                            {
                                objPD.miidv_electrical_accessories_amount = Convert.ToInt64(dr["vid_amount"]).ToString();
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 5)
                            {
                                objPD.miidv_side_car_trailer_amount = Convert.ToInt64(dr["vid_amount"]).ToString();
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 6)
                            {
                                objPD.miidv_value_of_cng_lpg_amount = Convert.ToInt64(dr["vid_amount"]).ToString();
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 7)
                            {
                                objPD.miidv_total_amount = Convert.ToInt64(dr["vid_amount"]).ToString();
                                //objPD.midv_own_damage_value =Convert.ToDecimal(dr["owndamage"]);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }

        public int SaveMIIDVDetailsDll(VM_MotorInsuranceIDVDetails objIDV)
        {
            int result = 0;
            dtIDVData.Columns.Add("vid_motor_insurance_app_id");
            dtIDVData.Columns.Add("vid_emp_id");
            dtIDVData.Columns.Add("vid_idv_id");
            dtIDVData.Columns.Add("vid_amount");
            dtIDVData.Columns.Add("vid_status");
            dtIDVData.Columns.Add("vid_active");

            if (objIDV.miidv_vaahanidvamount != null)
            {
                UpdateIDVDeatils(objIDV.miidv_emp_id, objIDV.miidv_application_id, 1, objIDV.miidv_vaahanidvamount, true, true);
            }
            if (objIDV.miidv_insured_declared_value_amount != null)
            {
                UpdateIDVDeatils(objIDV.miidv_emp_id, objIDV.miidv_application_id, 2, objIDV.miidv_insured_declared_value_amount, true, true);
            }
            if (objIDV.miidv_non_electrical_accessories_amount != null)
            {
                UpdateIDVDeatils(objIDV.miidv_emp_id, objIDV.miidv_application_id, 3, objIDV.miidv_non_electrical_accessories_amount, true, true);
            }
            if (objIDV.miidv_electrical_accessories_amount != null)
            {
                UpdateIDVDeatils(objIDV.miidv_emp_id, objIDV.miidv_application_id, 4, objIDV.miidv_electrical_accessories_amount, true, true);
            }
            if (objIDV.miidv_side_car_trailer_amount != null)
            {
                UpdateIDVDeatils(objIDV.miidv_emp_id, objIDV.miidv_application_id, 5, objIDV.miidv_side_car_trailer_amount, true, true);
            }
            if (objIDV.miidv_value_of_cng_lpg_amount != null)
            {
                UpdateIDVDeatils(objIDV.miidv_emp_id, objIDV.miidv_application_id, 6, objIDV.miidv_value_of_cng_lpg_amount, true, true);
            }
            if (objIDV.miidv_total_amount != null)
            {
                UpdateIDVDeatils(objIDV.miidv_emp_id, objIDV.miidv_application_id, 7, objIDV.miidv_total_amount, true, true);
            }

            SqlParameter[] sqlparam =
            {
             new SqlParameter("@EmpID",objIDV.miidv_emp_id),
             new SqlParameter("@RefNo",objIDV.miidv_application_id),
             new SqlParameter("@MIIDVDetailsResponseData",dtIDVData),
             new SqlParameter("@PremiumAmount",objIDV.premium_amount),
             new SqlParameter("@PageType",(objIDV.miidv_pagetype==null?"":objIDV.miidv_pagetype))
            };
            result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertMIIDVdetails"));
            return result;
        }

        public void UpdateIDVDeatils(long? EmpCode, long? ApplicationID, long IDVDetailsID, string amount, bool status, bool active)
        {
            DataRow Ddr = dtIDVData.NewRow();
            Ddr["vid_motor_insurance_app_id"] = ApplicationID ?? 0;
            Ddr["vid_emp_id"] = EmpCode ?? 0;
            Ddr["vid_idv_id"] = IDVDetailsID;
            Ddr["vid_amount"] = Convert.ToDecimal(amount);
            Ddr["vid_status"] = true;
            Ddr["vid_active"] = true;
            dtIDVData.Rows.Add(Ddr);
            //return dt_otherdata;
        }

        //=========================Others Details related functions ================================

        public VM_MotorInsurancePreviousHistoryDetails PreviousHistoryDetails(long EmployeeCode, long ReferenceId)
        {
            VM_MotorInsurancePreviousHistoryDetails objPD = new VM_MotorInsurancePreviousHistoryDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employeecode",EmployeeCode),
                 new SqlParameter("@referenceid",ReferenceId)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getprevioushistorydetails");
               
                if (dsPD.Tables.Count > 0)
                {

                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        objPD.ph_EmployeeCode = EmployeeCode;
                        objPD.ph_reference = ReferenceId;
                        foreach (DataRow dr in dsPD.Tables[0].Rows)
                        {
                            if(dr["typeofcover"].ToString() != "")
                            {
                                objPD.ph_TypeOfCover = dr["typeofcover"].ToString();
                            }

                            if (Convert.ToInt16(dr["vehicle_details_id"]) == 21)
                            {

                                objPD.ph_DateOfPurchaseOfVehicle = (dr["response1"].ToString() == "") ? (DateTime?)null : Convert.ToDateTime(dr["response1"]);

                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 22)
                            {
                                objPD.ph_PurchaseType = Convert.ToBoolean(dr["response"]);

                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 23)
                            {
                                objPD.ph_VehicleUsedPurposeA = Convert.ToBoolean(dr["response"]);

                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 24)
                            {
                                objPD.ph_VehicleUsedPurposeB = Convert.ToBoolean(dr["response"]);

                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 25)
                            {
                                objPD.ph_vehicleCondition = Convert.ToBoolean(dr["response"]);
                                if (Convert.ToBoolean(dr["response"]) == true)
                                    objPD.ph_VehicleConditionReason = dr["response1"].ToString();


                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 26)
                            {

                                objPD.ph_previousinsurerDetails = dr["response1"].ToString();

                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 27)
                            {

                                objPD.ph_previousinsurerNo = dr["response1"].ToString();

                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 28)
                            {

                                objPD.ph_insuranceFromDt = (dr["response2"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["response2"]);
                                objPD.ph_insuranceToDt = (dr["response3"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["response3"]);
                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 29)
                            {
                                objPD.ph_TypeOfCover = (dr["response1"] == "") ? "Package Policy" : dr["response1"].ToString();
                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 30)
                            {
                                objPD.ph_InsuranceDeclined = Convert.ToBoolean(dr["response"]);

                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 31)
                            {
                                objPD.ph_InsuranceCancelled = Convert.ToBoolean(dr["response"]);
                                if (Convert.ToBoolean(dr["response"]) == true)
                                    objPD.ph_CancelledReason = dr["response1"].ToString();
                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 32)
                            {
                                objPD.ph_InsuranceImposed = Convert.ToBoolean(dr["response"]);
                                if (Convert.ToBoolean(dr["response"]) == true)
                                    objPD.ph_ImposedReason = dr["response1"].ToString();
                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 33)
                            {
                                objPD.ph_Hire = Convert.ToBoolean(dr["response"]);

                            }
                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 34)
                            {
                                objPD.ph_Lease = Convert.ToBoolean(dr["response"]);
                            }

                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 35)
                            {
                                objPD.ph_Hypothecation = Convert.ToBoolean(dr["response"]);
                            }

                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 36)
                            {
                                if (objPD.ph_Hire == true || objPD.ph_Lease == true || objPD.ph_Hypothecation == true)
                                    objPD.ph_HReason = dr["response1"].ToString();
                            }

                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 37)
                            {

                                objPD.ph_OtherInfo = dr["response1"].ToString();

                            }
                            else if(Convert.ToInt16(dr["vehicle_details_id"]) == 38)
                            {
                                objPD.previous_vehicle_malus = Convert.ToInt32(dr["response1"]);
                                if(objPD.previous_vehicle_malus==0)
                                objPD.previous_vehicle_ncb = Convert.ToInt32(dr["response2"]);
                            }

                            else if (Convert.ToInt16(dr["vehicle_details_id"]) == 39)
                            {
                                objPD.ph_vehile_type_id = Convert.ToInt32(dr["response1"]);
                               
                            }


                        }


                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }

        public string SaveMIPreviousHistoryDetails(VM_MotorInsurancePreviousHistoryDetails vmPreviousHistoryDetails)
        {

            string result = string.Empty;

            DataTable dt_VehicleResponseDetails = new DataTable();
            DataTable dt_VehicleHistoryDetails = new DataTable();
            VehicleMIResponseDetails objPersonalData = new VehicleMIResponseDetails();
            dt_VehicleResponseDetails = CreateDataTable("VehicleMIResponseDetails");
            dt_VehicleHistoryDetails = CreateDataTable("VehicleMIHistoryDetails");
            DataRow dr = dt_VehicleResponseDetails.NewRow();
            DataRow drHD = dt_VehicleHistoryDetails.NewRow();
            try
            {

                //if (!String.IsNullOrEmpty(vmPreviousHistoryDetails.ph_DateOfPurchaseOfVehicle.ToString()))
                //{

                UpdateHDeatils(21, true, vmPreviousHistoryDetails.ph_DateOfPurchaseOfVehicle, null, null, dt_VehicleHistoryDetails);

                //}
                //if (!String.IsNullOrEmpty(vmPreviousHistoryDetails.ph_PurchaseType.ToString()))

                //{
                if (vmPreviousHistoryDetails.ph_PurchaseType == true)
                {
                    UpdateDeatils(22, vmPreviousHistoryDetails.ph_PurchaseType, "Second", dt_VehicleResponseDetails);
                }
                else
                {
                    UpdateDeatils(22, vmPreviousHistoryDetails.ph_PurchaseType, "New", dt_VehicleResponseDetails);
                }
                //=========
                if (vmPreviousHistoryDetails.ph_VehicleUsedPurposeA == true)
                {
                    UpdateDeatils(23, vmPreviousHistoryDetails.ph_VehicleUsedPurposeA, "", dt_VehicleResponseDetails);
                }
                else
                {
                    UpdateDeatils(23, vmPreviousHistoryDetails.ph_VehicleUsedPurposeA, "", dt_VehicleResponseDetails);
                }
                //=============
                if (vmPreviousHistoryDetails.ph_VehicleUsedPurposeB == true)
                {
                    UpdateDeatils(24, vmPreviousHistoryDetails.ph_VehicleUsedPurposeB, "", dt_VehicleResponseDetails);

                }
                else
                {
                    UpdateDeatils(24, vmPreviousHistoryDetails.ph_VehicleUsedPurposeB, "", dt_VehicleResponseDetails);
                }
                //==================
                if (vmPreviousHistoryDetails.ph_vehicleCondition == false)
                {

                    UpdateDeatils(25, vmPreviousHistoryDetails.ph_vehicleCondition, "", dt_VehicleResponseDetails);
                }
                else
                {

                    UpdateDeatils(25, vmPreviousHistoryDetails.ph_vehicleCondition, vmPreviousHistoryDetails.ph_VehicleConditionReason, dt_VehicleResponseDetails);
                }
                //=====================
                //if (!String.IsNullOrEmpty(vmPreviousHistoryDetails.ph_previousinsurerDetails))
                //{
                UpdateDeatils(26, true, vmPreviousHistoryDetails.ph_previousinsurerDetails, dt_VehicleResponseDetails);

                //}

                //if (!String.IsNullOrEmpty(vmPreviousHistoryDetails.ph_previousinsurerNo))
                //{

                UpdateDeatils(27, true, vmPreviousHistoryDetails.ph_previousinsurerNo, dt_VehicleResponseDetails);

                //}
                //if (!String.IsNullOrEmpty(vmPreviousHistoryDetails.ph_insuranceFromDt.ToString()) || !String.IsNullOrEmpty(vmPreviousHistoryDetails.ph_insuranceToDt.ToString()))
                //{
                UpdateHDeatils(28, true, null, vmPreviousHistoryDetails.ph_insuranceFromDt, vmPreviousHistoryDetails.ph_insuranceToDt, dt_VehicleHistoryDetails);
                //}
                if (vmPreviousHistoryDetails.ph_TypeOfCover == null)
                {
                    vmPreviousHistoryDetails.ph_TypeOfCover = "Package Policy";
                }
                //if (!String.IsNullOrEmpty(vmPreviousHistoryDetails.ph_TypeOfCover.ToString()))
                //{

                UpdateDeatils(29, true, vmPreviousHistoryDetails.ph_TypeOfCover, dt_VehicleResponseDetails);
                //}
                if (vmPreviousHistoryDetails.ph_InsuranceDeclined == true)
                {
                    UpdateDeatils(30, vmPreviousHistoryDetails.ph_InsuranceDeclined, "", dt_VehicleResponseDetails);
                }
                else
                {
                    UpdateDeatils(30, vmPreviousHistoryDetails.ph_InsuranceDeclined, "", dt_VehicleResponseDetails);
                }
                if (vmPreviousHistoryDetails.ph_InsuranceCancelled == true)
                {
                    UpdateDeatils(31, vmPreviousHistoryDetails.ph_InsuranceCancelled, vmPreviousHistoryDetails.ph_CancelledReason, dt_VehicleResponseDetails);
                }
                else
                {
                    UpdateDeatils(31, vmPreviousHistoryDetails.ph_InsuranceCancelled, "", dt_VehicleResponseDetails);
                }

                if (vmPreviousHistoryDetails.ph_InsuranceImposed == true)
                {
                    UpdateDeatils(32, vmPreviousHistoryDetails.ph_InsuranceImposed, vmPreviousHistoryDetails.ph_ImposedReason, dt_VehicleResponseDetails);
                }
                else
                {
                    UpdateDeatils(32, vmPreviousHistoryDetails.ph_InsuranceImposed, "", dt_VehicleResponseDetails);
                }
                if (vmPreviousHistoryDetails.ph_Hire == true)
                {
                    UpdateDeatils(33, vmPreviousHistoryDetails.ph_Hire, "", dt_VehicleResponseDetails);
                }
                else
                {
                    UpdateDeatils(33, vmPreviousHistoryDetails.ph_Hire, "", dt_VehicleResponseDetails);
                }
                if (vmPreviousHistoryDetails.ph_Lease == true)
                {
                    UpdateDeatils(34, vmPreviousHistoryDetails.ph_Lease, "", dt_VehicleResponseDetails);
                }
                else
                {
                    UpdateDeatils(34, vmPreviousHistoryDetails.ph_Lease, "", dt_VehicleResponseDetails);
                }
                if (vmPreviousHistoryDetails.ph_Hypothecation == true)
                {
                    UpdateDeatils(35, vmPreviousHistoryDetails.ph_Hypothecation, "", dt_VehicleResponseDetails);

                }
                else
                {
                    UpdateDeatils(35, vmPreviousHistoryDetails.ph_Hypothecation, "", dt_VehicleResponseDetails);
                }
                if (vmPreviousHistoryDetails.ph_Hypothecation == true || vmPreviousHistoryDetails.ph_Lease == true || vmPreviousHistoryDetails.ph_Hire == true)
                {
                    UpdateDeatils(36, true, vmPreviousHistoryDetails.ph_HReason, dt_VehicleResponseDetails);

                }
                else
                {
                    UpdateDeatils(36, true, vmPreviousHistoryDetails.ph_HReason, dt_VehicleResponseDetails);
                }
                //if (!String.IsNullOrEmpty(vmPreviousHistoryDetails.ph_OtherInfo))
                //{
                UpdateDeatils(37, true, vmPreviousHistoryDetails.ph_OtherInfo, dt_VehicleResponseDetails);

                //}

                SqlParameter[] sqlparam =
                {
             new SqlParameter("@EmpID",vmPreviousHistoryDetails.ph_EmployeeCode),
             new SqlParameter("@ReferenceId",vmPreviousHistoryDetails.ph_reference),
             new SqlParameter("@VResponseDetails",dt_VehicleResponseDetails),
             new SqlParameter("@VHistoryDetails",dt_VehicleHistoryDetails),
             //new SqlParameter("@PremiumAmount",vmPreviousHistoryDetails.premium_amount),
             new SqlParameter("@PageType",(vmPreviousHistoryDetails.mivd_pagetype==null?"":vmPreviousHistoryDetails.mivd_pagetype))
            };
                result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_saveprevioushistorydetails"));
            }
            catch (Exception ex)
            {


            }
            return result;

        }

        public DataTable UpdateDeatils(int id, bool response, string response1, DataTable dt_VehicleResponseDetails)
        {

            DataRow Ddr = dt_VehicleResponseDetails.NewRow();
            Ddr["vehicleResponseID"] = id;
            Ddr["vehicleResponse"] = response;
            Ddr["vehicleResponse1"] = response1;
            Ddr["IsPHSave"] = true;
            dt_VehicleResponseDetails.Rows.Add(Ddr);
            return dt_VehicleResponseDetails;
        }

        public DataTable UpdateHDeatils(int id, bool response, Nullable<DateTime> response1, Nullable<DateTime> response2, Nullable<DateTime> response3, DataTable dt_VehicleHistoryDetails)
        {


            DataRow Ddr = dt_VehicleHistoryDetails.NewRow();
            Ddr["vehicleHistoryID"] = id;
            Ddr["vehicleHResponse"] = response;
            Ddr["vehicleHDate1"] = (response1 != null) ? Convert.ToDateTime(response1).ToString("MM/dd/yyyy") : null;
            Ddr["vehicleHDate2"] = (response2 != null) ? Convert.ToDateTime(response2).ToString("MM/dd/yyyy") : null; 
            Ddr["vehicleHDate3"] = (response3 != null) ? Convert.ToDateTime(response3).ToString("MM/dd/yyyy") : null;

            dt_VehicleHistoryDetails.Rows.Add(Ddr);
            return dt_VehicleHistoryDetails;
        }

        //=========================MI Document Details related functions ================================
        public VM_MI_Upload_Documents MIDocumentDetailsDll(long EmployeeCode, long ReferenceId)
        {
            VM_MI_Upload_Documents objPD = new VM_MI_Upload_Documents();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",EmployeeCode),
                new SqlParameter("@referenceid",ReferenceId)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectMIDocumentDetails");
                bool DocumentData = false;
                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPD.Tables[0].Rows)
                        {
                            if (!DocumentData)
                            {
                                DocumentData = true;
                                objPD.App_Proposer_ID = EmployeeCode;
                                objPD.MI_App_Reference_ID = Convert.ToInt64(dsPD.Tables[0].Rows[0]["mid_motor_insurance_application_id"]);
                            }

                            if(Convert.ToInt64(dr["mid_motor_document_type_id"])<=22)
                            {
                              //------------------------------------------------------------------------
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 1)
                                {
                                    objPD.ProposalDocNewPurchase_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 2)
                                {
                                    objPD.GovtSanctionDoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 3)
                                {
                                    objPD.ProformaInvoiceDoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 4)
                                {
                                    objPD.PSaleCertificateDoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 5)
                                {
                                    objPD.OtherDocument1_filename = dr["mid_document_path"].ToString();
                                }
                                //------------------------------------------------------------------------
                                
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 6)
                                {
                                    objPD.ProposalDocDonatedVehicle_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 7)
                                {
                                    objPD.DonationDoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 8)
                                {
                                    objPD.SaleCertificateDoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 9)
                                {
                                    objPD.TaxInvoiceDoc_filename = dr["mid_document_path"].ToString();
                                }

                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 10)
                                {
                                    objPD.DonatedEmissionCertificate_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 11)
                                {
                                    objPD.OtherDocument2_filename = dr["mid_document_path"].ToString();
                                }
                                //---------------------------------------------------------------------------
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 12)
                                {
                                    objPD.ProposalDocSeizedVehicle_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 13)
                                {
                                    objPD.cCertificateDoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 14)
                                {
                                    objPD.RTOcertificateDoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 15)
                                {
                                    objPD.FitnessCertificateRTODoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 16)
                                {
                                    objPD.SeizedEmissionCertificate_filename = dr["mid_document_path"].ToString();
                                }
                                
                                
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 17)
                                {
                                    objPD.OtherDocument3_filename = dr["mid_document_path"].ToString();
                                }
                                //----------------------------------------------------------------------------
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 18)
                                {
                                    objPD.PIVGovtSanctionDoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 19)
                                {
                                    objPD.PIVProformaInvoiceDoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 20)
                                {
                                    objPD.PIVPSaleCertificateDoc_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 21)
                                {
                                    objPD.OldInsuranceCertificate_filename = dr["mid_document_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 22)
                                {
                                    objPD.OtherDocument4_filename = dr["mid_document_path"].ToString();
                                }
                                //------------------------------------------------------------------------
                            }
                            else
                            {
                                if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 23)
                                {
                                    objPD.RenewalFitnessCertificateRTODoc_filename = dr["mid_document_path"].ToString();
                                }
                                //if (Convert.ToInt64(dr["mid_motor_document_type_id"]) == 16)
                                //{
                                //    objPD.AuctionDoc_filename = dr["mid_document_path"].ToString();
                                //}
                            }
                            

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }

        public int SaveMIDocumentDetailsDll(VM_MI_Upload_Documents objMIDocDetails)
        {
            int result = 0;
            dtDocumentsData.Columns.Add("mid_motor_insurance_application_id");
            dtDocumentsData.Columns.Add("mid_emp_id");
            dtDocumentsData.Columns.Add("mid_motor_document_type_id");
            dtDocumentsData.Columns.Add("mid_document_path");
            dtDocumentsData.Columns.Add("mid_status");
            DataTable dt_otherdata = CreateDataTable("MIDocumentDetailsResponseData");
            if (objMIDocDetails.IsDocType == 1)
            {
                if (objMIDocDetails.ProposalDocNewPurchase != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 1, true, objMIDocDetails.ProposalDocNewPurchase, objMIDocDetails.ProposalDocNewPurchase_filename, "ProposalDocNewPurchase");
                }
                else if (objMIDocDetails.ProposalDocNewPurchase_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 1, true, objMIDocDetails.ProposalDocNewPurchase, objMIDocDetails.ProposalDocNewPurchase_filename, "ProposalDocNewPurchase");
                }
                if (objMIDocDetails.GovtSanctionDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 2, true, objMIDocDetails.GovtSanctionDoc, objMIDocDetails.GovtSanctionDoc_filename, "GovtSanctionDoc");
                }
                else if (objMIDocDetails.GovtSanctionDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 2, true, objMIDocDetails.GovtSanctionDoc, objMIDocDetails.GovtSanctionDoc_filename, "GovtSanctionDoc");
                }
                if (objMIDocDetails.ProformaInvoiceDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 3, true, objMIDocDetails.ProformaInvoiceDoc, objMIDocDetails.ProformaInvoiceDoc_filename, "ProformaInvoiceDoc");
                }
                else if (objMIDocDetails.ProformaInvoiceDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 3, true, objMIDocDetails.ProformaInvoiceDoc, objMIDocDetails.ProformaInvoiceDoc_filename, "ProformaInvoiceDoc");
                }

                if (objMIDocDetails.PSaleCertificateDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 4, true, objMIDocDetails.PSaleCertificateDoc, objMIDocDetails.PSaleCertificateDoc_filename, "PSaleCertificate");
                }
                else if (objMIDocDetails.PSaleCertificateDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 4, true, objMIDocDetails.PSaleCertificateDoc, objMIDocDetails.PSaleCertificateDoc_filename, "PSaleCertificate");
                }

                if (objMIDocDetails.OtherDocument1 != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 5, true, objMIDocDetails.OtherDocument1, objMIDocDetails.OtherDocument1_filename, "OtherDocument1");
                }
                else if (objMIDocDetails.OtherDocument1_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 5, true, objMIDocDetails.OtherDocument1, objMIDocDetails.OtherDocument1_filename, "OtherDocument1");
                }

                //if (objMIDocDetails.OtherDocumentCount != 0)
                //{
                //    for (int i = 1; i <= objMIDocDetails.OtherDocumentCount; i++)
                //    {
                //        var OD = "objMIDocDetails.OtherDocument" + i;


                //    }
                //}


            }
            else if (objMIDocDetails.IsDocType == 2)
            {
                if (objMIDocDetails.ProposalDocDonatedVehicle != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 6, true, objMIDocDetails.ProposalDocDonatedVehicle, objMIDocDetails.ProposalDocDonatedVehicle_filename, "ProposalDocDonatedVehicle");
                }
                else if (objMIDocDetails.ProposalDocDonatedVehicle_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID,6, true, objMIDocDetails.ProposalDocDonatedVehicle, objMIDocDetails.ProposalDocDonatedVehicle_filename, "ProposalDocDonatedVehicle");
                }
                if (objMIDocDetails.DonationDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 7, true, objMIDocDetails.DonationDoc, objMIDocDetails.DonationDoc_filename, "DonationDoc");
                }
                else if (objMIDocDetails.DonationDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 7, true, objMIDocDetails.DonationDoc, objMIDocDetails.DonationDoc_filename, "DonationDoc");
                }
                if (objMIDocDetails.SaleCertificateDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 8, true, objMIDocDetails.SaleCertificateDoc, objMIDocDetails.SaleCertificateDoc_filename, "SaleCertificateDoc");
                }
                else if (objMIDocDetails.SaleCertificateDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 8, true, objMIDocDetails.SaleCertificateDoc, objMIDocDetails.SaleCertificateDoc_filename, "SaleCertificateDoc");
                }
                if (objMIDocDetails.TaxInvoiceDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 9, true, objMIDocDetails.TaxInvoiceDoc, objMIDocDetails.TaxInvoiceDoc_filename, "TaxInvoiceDoc");
                }
                else if (objMIDocDetails.TaxInvoiceDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 9, true, objMIDocDetails.TaxInvoiceDoc, objMIDocDetails.TaxInvoiceDoc_filename, "TaxInvoiceDoc");
                }
                if (objMIDocDetails.DonatedEmissionCertificate != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 10, true, objMIDocDetails.DonatedEmissionCertificate, objMIDocDetails.DonatedEmissionCertificate_filename, "DonateEmission");
                }
                else if (objMIDocDetails.DonatedEmissionCertificate_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 10, true, objMIDocDetails.DonatedEmissionCertificate, objMIDocDetails.DonatedEmissionCertificate_filename, "DonateEmission");
                }

                if (objMIDocDetails.OtherDocument2 != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 11, true, objMIDocDetails.OtherDocument2, objMIDocDetails.OtherDocument2_filename, "OtherDocument2");
                }
                else if (objMIDocDetails.OtherDocument2_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 11, true, objMIDocDetails.OtherDocument2, objMIDocDetails.OtherDocument2_filename, "OtherDocument2");
                }
            }
            else if (objMIDocDetails.IsDocType == 3)
            {
                if (objMIDocDetails.ProposalDocSeizedVehicle != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID,12, true, objMIDocDetails.ProposalDocSeizedVehicle, objMIDocDetails.ProposalDocSeizedVehicle_filename, "ProposalDocSeizedVehicle");
                }
                else if (objMIDocDetails.ProposalDocSeizedVehicle_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 12, true, objMIDocDetails.ProposalDocSeizedVehicle, objMIDocDetails.ProposalDocSeizedVehicle_filename, "ProposalDocSeizedVehicle");
                }
                if (objMIDocDetails.cCertificateDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 13, true, objMIDocDetails.cCertificateDoc, objMIDocDetails.cCertificateDoc_filename, "cCertificateDoc");
                }
                else if (objMIDocDetails.cCertificateDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 13, true, objMIDocDetails.cCertificateDoc, objMIDocDetails.cCertificateDoc_filename, "cCertificateDoc");
                }
                if (objMIDocDetails.RTOcertificateDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 14, true, objMIDocDetails.RTOcertificateDoc, objMIDocDetails.RTOcertificateDoc_filename, "RTOcertificateDoc");
                }
                else if (objMIDocDetails.RTOcertificateDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 14, true, objMIDocDetails.RTOcertificateDoc, objMIDocDetails.RTOcertificateDoc_filename, "RTOcertificateDoc");
                }
                if (objMIDocDetails.FitnessCertificateRTODoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 15, true, objMIDocDetails.FitnessCertificateRTODoc, objMIDocDetails.FitnessCertificateRTODoc_filename, "FitnessCertificate");
                }
                else if (objMIDocDetails.FitnessCertificateRTODoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 15, true, objMIDocDetails.FitnessCertificateRTODoc, objMIDocDetails.FitnessCertificateRTODoc_filename, "FitnessCertificate");
                }
                if (objMIDocDetails.SeizedEmissionCertificate != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 16, true, objMIDocDetails.SeizedEmissionCertificate, objMIDocDetails.SeizedEmissionCertificate_filename, "SeizedEmissionCertificate");
                }
                else if (objMIDocDetails.SeizedEmissionCertificate_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 16, true, objMIDocDetails.SeizedEmissionCertificate, objMIDocDetails.SeizedEmissionCertificate_filename, "SeizedEmissionCertificate");
                }

                if (objMIDocDetails.OtherDocument3 != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 17, true, objMIDocDetails.OtherDocument3, objMIDocDetails.OtherDocument3_filename, "OtherDocument3");
                }
                else if (objMIDocDetails.OtherDocument3_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 17, true, objMIDocDetails.OtherDocument3, objMIDocDetails.OtherDocument3_filename, "OtherDocument3");
                }
            }
            if (objMIDocDetails.IsDocType == 4)
            {
                if (objMIDocDetails.PIVGovtSanctionDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 18, true, objMIDocDetails.PIVGovtSanctionDoc, objMIDocDetails.PIVGovtSanctionDoc_filename, "PIVGovtSanctionDoc");
                }
                else if (objMIDocDetails.PIVGovtSanctionDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 18, true, objMIDocDetails.PIVGovtSanctionDoc, objMIDocDetails.PIVGovtSanctionDoc_filename, "PIVGovtSanctionDoc");
                }
                
                if (objMIDocDetails.PIVProformaInvoiceDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 19, true, objMIDocDetails.PIVProformaInvoiceDoc, objMIDocDetails.PIVProformaInvoiceDoc_filename, "PIVProformaInvoiceDoc");
                }
                else if (objMIDocDetails.PIVProformaInvoiceDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 19, true, objMIDocDetails.PIVProformaInvoiceDoc, objMIDocDetails.PIVProformaInvoiceDoc_filename, "PIVProformaInvoiceDoc");
                }

                if (objMIDocDetails.PIVPSaleCertificateDoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 20, true, objMIDocDetails.PIVPSaleCertificateDoc, objMIDocDetails.PIVPSaleCertificateDoc_filename, "PIVPSaleCertificateDoc");
                }
                else if (objMIDocDetails.PIVPSaleCertificateDoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 20, true, objMIDocDetails.PIVPSaleCertificateDoc, objMIDocDetails.PIVPSaleCertificateDoc_filename, "PIVPSaleCertificateDoc");
                }

                if (objMIDocDetails.OldInsuranceCertificate != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 21, true, objMIDocDetails.OldInsuranceCertificate, objMIDocDetails.OldInsuranceCertificate_filename, "OldInsuranceCertificate");
                }
                else if (objMIDocDetails.OldInsuranceCertificate_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 21, true, objMIDocDetails.OldInsuranceCertificate, objMIDocDetails.OldInsuranceCertificate_filename, "OldInsuranceCertificate");
                }

                if (objMIDocDetails.OtherDocument4 != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 22, true, objMIDocDetails.OtherDocument4, objMIDocDetails.OtherDocument4_filename, "OtherDocument4");
                }
                else if (objMIDocDetails.OtherDocument4_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 22, true, objMIDocDetails.OtherDocument4, objMIDocDetails.OtherDocument4_filename, "OtherDocument4");
                }

            }
            else if (objMIDocDetails.IsDocType == 5)
            {
                if (objMIDocDetails.RenewalFitnessCertificateRTODoc != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 23, true, objMIDocDetails.RenewalFitnessCertificateRTODoc, objMIDocDetails.RenewalFitnessCertificateRTODoc_filename, "RenewalEmissionDoc");
                }
                else if (objMIDocDetails.RenewalFitnessCertificateRTODoc_filename != null)
                {
                    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 23, true, objMIDocDetails.RenewalFitnessCertificateRTODoc, objMIDocDetails.RenewalFitnessCertificateRTODoc_filename, "RenewalEmissionDoc");
                }
                //else if (objMIDocDetails.RTOcertificateDoc_filename != null)
                //{
                //    SaveMIDocFileData(objMIDocDetails.App_Proposer_ID, objMIDocDetails.MI_App_Reference_ID, 23, true, objMIDocDetails.RenewalFitnessCertificateRTODoc, objMIDocDetails.RenewalFitnessCertificateRTODoc_filename, "RTOcertificateDoc");
                //}
            }

            SqlParameter[] sqlparam =
            {
            new SqlParameter("@EmpID",objMIDocDetails.App_Proposer_ID),
             new SqlParameter("@RefNo",objMIDocDetails.MI_App_Reference_ID),
             new SqlParameter("@MIDocumentDetailsResponseData",dtDocumentsData)
            };
            result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insertMIDocumentdetails"));
            return result;
        }

        public void SaveMIDocFileData(long? EmpCode, long? ApplicationID, long MIDocTypeID, bool status, HttpPostedFileBase MIODDoc, string miad_upload_document_path, string miadDocType)
        {
            try
            {
                //if (string.IsNullOrEmpty(miad_upload_document_path))
                //{
                //    miad_upload_document_path = MIUploadDocument(MIODDoc, ApplicationID, miadDocType);
                //}
                DataRow Ddr = dtDocumentsData.NewRow();
                Ddr["mid_motor_insurance_application_id"] = ApplicationID ?? 0;
                Ddr["mid_emp_id"] = EmpCode ?? 0;
                Ddr["mid_motor_document_type_id"] = MIDocTypeID;

                string path= MIUploadDocument(MIODDoc, ApplicationID, miadDocType);
                if(path!="")
                {
                    Ddr["mid_document_path"] = path;
                }
                else
                {
                    Ddr["mid_document_path"] = miad_upload_document_path;
                }
              //  Ddr["mid_document_path"] = MIUploadDocument(MIODDoc, ApplicationID, miadDocType);//miad_upload_document_path;
                Ddr["mid_status"] = true;
                dtDocumentsData.Rows.Add(Ddr);
            }
            catch (Exception ex)
            {

            }
        }

        private string MIUploadDocument(HttpPostedFileBase document, long? ApplicationID, string docType)
        {
            string subPath = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
                subPath = "/MIDocuments/" + ApplicationID.ToString() + "/" + docType;
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(subPath));
                if (!exists)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                }

                string path = Path.Combine(HttpContext.Current.Server.MapPath(subPath), fileName);
                document.SaveAs(path);
                subPath = subPath + "/" + fileName;
            }

            return subPath;
        }
        //===============Challan Details=================
        public string SaveMIChallanDetailsDll(long EmpID, int Category, string RefNos, int ChallanAmount, string Type)
        {
            string result = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@Empid",EmpID),
                    new SqlParameter("@Category",Category),
                    new SqlParameter("@RefNos",RefNos),
                    new SqlParameter("@ChallanAmount",ChallanAmount),
                    new SqlParameter("@Type",Type)
                };
                result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_SaveMIChallanDetails"));
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        //Print MI Challan Print Details 
        public VM_ChallanPrintDetails PrintMIChallanDetailsDll(long EmpID, int Category, string RefNos, string Type)
        {
            VM_ChallanPrintDetails NBChallanDetails = new VM_ChallanPrintDetails();
            DataSet details = new DataSet();
            SqlParameter[] parms = {
              new SqlParameter("@empId",EmpID),
              new SqlParameter("@applicationId",RefNos),
              new SqlParameter("@category",Convert.ToString(Category)),
              new SqlParameter("@type",Type)
            };
            details = _Conn.ExeccuteDataset(parms, "sp_kgid_MB_Print_PaymentDetails");
            if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
            {
                NBChallanDetails.dm_ddo_code = details.Tables[0].Rows[0]["dm_ddo_code"].ToString();
                NBChallanDetails.dm_ddo_office = details.Tables[0].Rows[0]["dm_ddo_office"].ToString();
                NBChallanDetails.dm_deptname_english = details.Tables[0].Rows[0]["dm_deptname_english"].ToString();
                NBChallanDetails.dm_name_english = details.Tables[0].Rows[0]["dm_name_english"].ToString();
                NBChallanDetails.employee_name = details.Tables[0].Rows[0]["employee_name"].ToString();
                NBChallanDetails.ead_address = details.Tables[0].Rows[0]["ead_address"].ToString();
                NBChallanDetails.mobile_number = details.Tables[0].Rows[0]["mobile_number"].ToString();
                NBChallanDetails.hoa_desc = details.Tables[0].Rows[0]["hoa_desc"].ToString();
                NBChallanDetails.purpose_id = details.Tables[0].Rows[0]["purpose_id"].ToString();
                NBChallanDetails.purpose_desc = details.Tables[0].Rows[0]["purpose_desc"].ToString();
                NBChallanDetails.sub_purpose_desc = details.Tables[0].Rows[0]["sub_purpose_desc"].ToString();
                NBChallanDetails.p_premium = (details.Tables[0].Rows[0]["p_premium"] == DBNull.Value) ? (double?)0 : Convert.ToDouble((details.Tables[0].Rows[0]["p_premium"]));
                //NBChallanDetails.LastUpdatedDateTime = Convert.ToDateTime(details.Tables[0].Rows[0]["p_updation_datetime"].ToString());
            }
            return NBChallanDetails;
        }
        //=========================Verify Data related functions ================================
        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForDDOVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",1),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForCWVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                //string description = GetCategoryDescription(Convert.ToInt32(Category));
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",2),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                    EngineNo = dataRow.Field<string>("mivd_engine_no"),
                    VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                    VehicleYear = dataRow.Field<string>("vy_vehicle_year"),
                    TypeofCover = dataRow.Field<string>("mitoc_type_cover_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mia_user_category"),
                    //Status = dataRow.Field<string>("AppStatus")
                    Status = (dataRow.Field<string>("AppStatus") == "Send Back to Employee") ? (dataRow.Field<string>("AppStatus").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("AppStatus").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    //Status = dataRow.Field<string>("asm_status_desc"),
                    Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                    //Status = dataRow.Field<string>("asm_status_desc"),
                    Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                var CancelStatus = dsDDO.Tables[4].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mia_user_category"),
                    Status = (dataRow.Field<string>("AppStatus")=="Reverted")?"Cancelled":dataRow.Field<string>("AppStatus"),
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                verificationDetails.CancelEmployeeStatus = CancelStatus;
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);
                    verificationDetails.CancelApplication = Convert.ToInt64(dsDDO.Tables[5].Rows[0]["Cancelled"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForSuperintendentVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                //string description = GetCategoryDescription(Convert.ToInt32(Category));
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",3),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                    EngineNo = dataRow.Field<string>("mivd_engine_no"),
                    VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                    VehicleYear = dataRow.Field<string>("vy_vehicle_year"),
                    TypeofCover = dataRow.Field<string>("mitoc_type_cover_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mia_user_category"),
                    //Status = dataRow.Field<string>("AppStatus")
                    Status = (dataRow.Field<string>("AppStatus") == "Send Back to Employee") ? (dataRow.Field<string>("AppStatus").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("AppStatus").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    //Status = dataRow.Field<string>("asm_status_desc"),
                    Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                    //Status = dataRow.Field<string>("asm_status_desc"),
                    Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                var CancelStatus = dsDDO.Tables[4].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mia_user_category"),
                    Status = (dataRow.Field<string>("AppStatus") == "Reverted") ? "Cancelled" : dataRow.Field<string>("AppStatus"),
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                verificationDetails.CancelEmployeeStatus = CancelStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);
                    verificationDetails.CancelApplication = Convert.ToInt64(dsDDO.Tables[5].Rows[0]["Cancelled"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForDIOVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",4),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mia_user_category"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForADVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                //string description = GetCategoryDescription(Convert.ToInt32(Category));
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",15),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                    EngineNo = dataRow.Field<string>("mivd_engine_no"),
                    VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                    VehicleYear = dataRow.Field<string>("vy_vehicle_year"),
                    TypeofCover = dataRow.Field<string>("mitoc_type_cover_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mia_user_category"),
                    //Status = dataRow.Field<string>("AppStatus")
                    Status = (dataRow.Field<string>("AppStatus") == "Send Back to Employee") ? (dataRow.Field<string>("AppStatus").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("AppStatus").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    //Status = dataRow.Field<string>("asm_status_desc"),
                      Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                   // Status = dataRow.Field<string>("asm_status_desc"),
                    Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                var CancelStatus = dsDDO.Tables[4].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mia_user_category"),
                    Status = dataRow.Field<string>("AppStatus"),
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                verificationDetails.CancelEmployeeStatus = CancelStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(ApprovedStatus.Count());
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);
                    verificationDetails.CancelApplication = Convert.ToInt64(dsDDO.Tables[5].Rows[0]["Cancelled"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForDDVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                //string description = GetCategoryDescription(Convert.ToInt32(Category));
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",5),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                    EngineNo = dataRow.Field<string>("mivd_engine_no"),
                    VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                    VehicleYear = dataRow.Field<string>("vy_vehicle_year"),
                    TypeofCover = dataRow.Field<string>("mitoc_type_cover_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mia_user_category"),
                    //Status = dataRow.Field<string>("AppStatus")
                    Status = (dataRow.Field<string>("AppStatus") == "Send Back to Employee") ? (dataRow.Field<string>("AppStatus").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("AppStatus").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    //Status = dataRow.Field<string>("asm_status_desc"),
                    Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                    //Status = dataRow.Field<string>("asm_status_desc"),
                    Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mia_user_category"))))),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                var CancelStatus = dsDDO.Tables[4].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mia_user_category"),
                    Status = dataRow.Field<string>("AppStatus"),
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                verificationDetails.CancelEmployeeStatus = CancelStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);
                     verificationDetails.CancelApplication = Convert.ToInt64(dsDDO.Tables[5].Rows[0]["Cancelled"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_DDOVerificationDetailsMI GetEmployeeDetailsForDVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",6),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mia_user_category"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public IList<VM_MIWorkFlowDetails> GetWorkFlowDetails(long applicationId, int category)
        {
            IList<VM_MIWorkFlowDetails> workflowDetails = null;

            try
            {
                string description = GetCategoryDescription(category);
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@applicationId",applicationId),
                    new SqlParameter("@category",category)
                };

                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getWorkflowDetailsMI");

                if (dsDDO.Tables.Count > 0 && dsDDO.Tables[0].Rows.Count > 0)
                {
                    workflowDetails = new List<VM_MIWorkFlowDetails>();
                    foreach (DataRow dr in dsDDO.Tables[0].Rows)
                    {
                        VM_MIWorkFlowDetails workflowDetail = new VM_MIWorkFlowDetails();
                        workflowDetail.ApplicationRefNo = dr["ApplicationRefNo"].ToString();
                        workflowDetail.From = dr["From"].ToString();
                        workflowDetail.To = dr["To"].ToString();
                        workflowDetail.Remarks = dr["Remarks"].ToString();
                        workflowDetail.Comments = dr["Comments"].ToString();
                        workflowDetail.CreationDateTime = dr["CreationDateTime"].ToString();
                        if(dr["ApplicationStatus"].ToString()== "Send Back to Employee")
                        {
                            workflowDetail.ApplicationStatus = dr["ApplicationStatus"].ToString().Replace("Employee", description);
                        }
                        else
                        {
                            workflowDetail.ApplicationStatus = dr["ApplicationStatus"].ToString().Replace("Applicant", description);
                        }
                       
                        workflowDetail.NameOfApplicant = dr["name"].ToString();
                        workflowDetail.Category = Convert.ToInt32(dr["mia_user_category"]);
                        workflowDetails.Add(workflowDetail);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return workflowDetails.ToList();
        }

        public string SaveVerifiedDetailsBll(VM_MotorInsuranceDeptVerficationDetails objVerification)
        {
            var result = "";


            try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@employee_id",objVerification.EmpCode),
                    new SqlParameter("@miwd_application_id",objVerification.ApplicationRefNo),
                    new SqlParameter("@miwd_verified_by",objVerification.CreatedBy),

                    new SqlParameter("@miwd_checklist_verification_status",objVerification.VerifyProposerDetails),
                    new SqlParameter("@miwd_remarks",objVerification.Remarks),
                    new SqlParameter("@miwd_comments",objVerification.Comments),
                    new SqlParameter("@miwd_application_status",objVerification.ApplicationStatus),

                    new SqlParameter("@miwd_active_status",true),
                    new SqlParameter("@miwd_created_by",objVerification.CreatedBy),
                    new SqlParameter("@miwd_creation_datetime",DateTime.Now),


                };

                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_insertDepartmentWorkflowVerificationMI");
                if (objVerification.ApplicationStatus == 15)
                {
                    DataSet details = new DataSet();

                    SqlParameter[] sqlparamNotifDetails =
                    {
                        new SqlParameter("@employeeId", objVerification.EmpCode),
                        new SqlParameter("@applicationId",objVerification.ApplicationRefNo)

                    };

                    details = _Conn.ExeccuteDataset(sqlparamNotifDetails, "sp_kgid_getNotificationDetails");
                    VM_NotificationDetailsMI notificationDetails = new VM_NotificationDetailsMI();

                    if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
                    {
                        notificationDetails.DDOEmailId = details.Tables[0].Rows[0]["DDOEmailId"].ToString();
                        notificationDetails.EmpEmailId = details.Tables[0].Rows[0]["EmpEmailId"].ToString();
                        notificationDetails.EmpMobileNumber = Convert.ToInt64(details.Tables[0].Rows[0]["EmpMobileNumber"].ToString());
                        notificationDetails.EmpName = details.Tables[0].Rows[0]["EmpName"].ToString();

                    }

                    //SendInsurancePolicyNotification(notificationDetails);
                    //returnString = notificationDetails.PolicyNumber;
                }

            }
            catch (Exception ex)
            {

            }
            return result;


        }

        private string GetCategoryDescription(int Category)
        {
            string description = "";
            if (Category == 2)
            {
                description = "Department";
            }
            else if (Category == 11)
            {
                description = "Agency";
            }
            else if (Category == 1)
            {
                description = "Employee";
            }
            return description;
        }
        //ICT
        public VM_DDOVerificationDetailsMI getMIApplicationEmployeeList(long empId, int Category)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                string description = GetCategoryDescription(Category);
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",empId),
                    new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getMotorInsuranceApplicationStatus");
                if (dsDDO.Tables[0].Rows.Count > 0)
                {
                    var CurrentStatusList = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                    {
                        EmployeeCode = dataRow.Field<long?>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                        VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                        //VehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        TypeofCover = dataRow.Field<string>("type_of_cover"),
                        VehicleYear = dataRow.Field<string>("year"),
                        Status=(dataRow.Field<string>("asm_status_desc") == "Send Back to Employee")?("Reverted back from KGID due to objection") :dataRow.Field<string>("asm_status_desc").ToString(),
                        //Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", description)) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", description)),
                        //Status = (dataRow.Field<string>("asm_status_desc").Replace("Applicant",description)),
                        registrationNo = dataRow.Field<string>("mivd_registration_no"),
                        ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                        EngineNo = dataRow.Field<string>("mivd_engine_no"),
                        AppStatusID = dataRow.Field<int>("status"),
                        LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                        CategoryId = dataRow.Field<string>("mia_user_category"),
                        ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                        PolicyPremium = dataRow.Field<double?>("p_mi_premium"),
                        PolicyNumber = dataRow.Field<string>("p_mi_policy_number"),
                        UnsignBondDocPath = dataRow.Field<string>("unsignbondpath"),
                        SignedBondDocPath = dataRow.Field<string>("signedbondpath")
                    }).ToList();
                    verificationDetails.ViewStatusForEmployees = CurrentStatusList.Where(a => a.AppStatusID != 2 && a.AppStatusID != 1).ToList();

                    verificationDetails.LastUpdatedStatusForEmployees = CurrentStatusList.Where(a => a.AppStatusID == 2 || a.AppStatusID == 1).Select(a => a).ToList();
                }
                if (dsDDO.Tables[1].Rows.Count > 0)
                {
                    var MIPremiumDetailsList = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new PolicyPremiumDetailMI
                    {
                        EmployeeCode = dataRow.Field<long>("mia_proposer_id"),
                        ApplicationNumber = dataRow.Field<long>("mia_application_ref_no"),
                        VehicleMakeName = dataRow.Field<string>("vm_vehicle_make_desc"),
                        //VehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                        //registrationNo = dataRow.Field<string>("mivd_registration_no"),
                        registrationNo = dataRow.Field<string>("mivd_registration_no"),
                        ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                        EngineNo = dataRow.Field<string>("mivd_engine_no"),
                        VehicleYear = dataRow.Field<string>("year"),
                        TypeofCover = dataRow.Field<string>("type_of_cover"),
                        PolicyId = dataRow.Field<long?>("p_mi_policy_id"),
                        PolicyPremiumAmount = dataRow.Field<double?>("p_mi_premium"),
                        ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id")
                    }).ToList();
                    verificationDetails.PolicyPremiumDetailMI = MIPremiumDetailsList.Where(a => a.PolicyPremiumAmount != (double?)null).ToList();

                }
                // ICT CHANGE
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    verificationDetails.DDOCODE = Convert.ToString(dsDDO.Tables[2].Rows[0]["dm_ddo_code"]);
                }
                verificationDetails.Category = Category;
                if (dsDDO.Tables[3].Rows.Count > 0)
                {
                    var HOAList = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new SelectListItem()
                    {
                        Text = dataRow.Field<string>("HOA_TEXT"),
                        Value = dataRow.Field<string>("HOA_ID").ToString()
                    }).ToList();

                    verificationDetails.HOAList = HOAList;
                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        public VM_DDOVerificationDetailsMI getMIApplicationEmployeeStatusList(long empId, int Category)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                string description = GetCategoryDescription(Category);
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",empId),
                    new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getMotorInsuranceApplicationStatusDDO");
                if (dsDDO.Tables[0].Rows.Count > 0)
                {
                    var CurrentStatusList = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                    {
                        EmployeeCode = dataRow.Field<long?>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                        VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                        //VehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        TypeofCover = dataRow.Field<string>("type_of_cover"),
                        VehicleYear = dataRow.Field<string>("year"),
                        Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? ("Objection/Issue in Application") : dataRow.Field<string>("asm_status_desc").ToString(),
                        //Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", description)) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", description)),
                        //Status = (dataRow.Field<string>("asm_status_desc").Replace("Applicant",description)),
                        registrationNo = dataRow.Field<string>("mivd_registration_no"),
                        ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                        EngineNo = dataRow.Field<string>("mivd_engine_no"),
                        AppStatusID = dataRow.Field<int>("status"),
                        LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                        CategoryId = dataRow.Field<string>("mia_user_category"),
                        ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                        PolicyPremium = dataRow.Field<double?>("p_mi_premium"),
                        PolicyNumber = dataRow.Field<string>("p_mi_policy_number"),
                        UnsignBondDocPath = dataRow.Field<string>("unsignbondpath"),
                        SignedBondDocPath = dataRow.Field<string>("signedbondpath")
                    }).ToList();
                    verificationDetails.ViewStatusForEmployees = CurrentStatusList.Where(a => a.AppStatusID != 1).ToList();

                    verificationDetails.LastUpdatedStatusForEmployees = CurrentStatusList.Where(a => a.AppStatusID == 2 || a.AppStatusID == 1).Select(a => a).ToList();
                }
                if (dsDDO.Tables[1].Rows.Count > 0)
                {
                    var MIPremiumDetailsList = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new PolicyPremiumDetailMI
                    {
                        EmployeeCode = dataRow.Field<long>("mia_proposer_id"),
                        ApplicationNumber = dataRow.Field<long>("mia_application_ref_no"),
                        VehicleMakeName = dataRow.Field<string>("vm_vehicle_make_desc"),
                        //VehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                        //registrationNo = dataRow.Field<string>("mivd_registration_no"),
                        registrationNo = dataRow.Field<string>("mivd_registration_no"),
                        ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                        EngineNo = dataRow.Field<string>("mivd_engine_no"),
                        VehicleYear = dataRow.Field<string>("year"),
                        TypeofCover = dataRow.Field<string>("type_of_cover"),
                        PolicyId = dataRow.Field<long?>("p_mi_policy_id"),
                        PolicyPremiumAmount = dataRow.Field<double?>("p_mi_premium"),
                        ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id")
                    }).ToList();
                    verificationDetails.PolicyPremiumDetailMI = MIPremiumDetailsList.Where(a => a.PolicyPremiumAmount != (double?)null).ToList();

                }
                // ICT CHANGE
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    verificationDetails.DDOCODE = Convert.ToString(dsDDO.Tables[2].Rows[0]["dm_ddo_code"]);
                }
                //verificationDetails.Category = Category;
                if (dsDDO.Tables[3].Rows.Count > 0)
                {
                    var HOAList = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new SelectListItem()
                    {
                        Text = dataRow.Field<string>("HOA_TEXT"),
                        Value = dataRow.Field<string>("HOA_ID").ToString()
                    }).ToList();

                    verificationDetails.HOAList = HOAList;
                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }


        public PolicyPremiumDetailMI selectPaymentDetailsMI(string pagetype, long empId, int applicationId)
        {
            PolicyPremiumDetailMI obj = new PolicyPremiumDetailMI();
            DataSet details = new DataSet();

            SqlParameter[] parms = {
              new SqlParameter("@PageType",(pagetype==null?"":pagetype)),
              new SqlParameter("@empId",empId),
              new SqlParameter("@applicationId",applicationId),
            };
            details = _Conn.ExeccuteDataset(parms, "sp_kgid_selectMIPaymentdetails");
            if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
            {
                obj.DDOCode = details.Tables[0].Rows[0]["dm_ddo_code"].ToString();
                obj.PolicyPremiumAmount = (details.Tables[0].Rows[0]["p_mi_premium"] == DBNull.Value) ? (double?)0 : Convert.ToDouble((details.Tables[0].Rows[0]["p_mi_premium"]));
                obj.LastUpdatedDateTime = Convert.ToDateTime(details.Tables[0].Rows[0]["p_mi_updation_datetime"].ToString());


            }
            return obj;

        }
        public VM_MBApplicationDetails GetMBApplicationListDll(long EmpID, int Category)
        {
            VM_MBApplicationDetails ApplicationDetails = new VM_MBApplicationDetails();
            try
            {
                DataSet dsMBList = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",EmpID),
                    new SqlParameter("@Category",Category)
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_AllApplication");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new MBApplicationDetailsList
                    {
                        RowNumber = dataRow.Field<long>("RowNumber"),
                        EmployeeCode = dataRow.Field<long>("mia_proposer_id"),
                        //Name = dataRow.Field<string>("employee_name"),
                        ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                        ApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                        VehicleMakeName = dataRow.Field<string>("vm_vehicle_make_desc"),
                        VehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        VehicleModelName = dataRow.Field<string>("vm_vehicle_model_desc"),
                        RegisrationNumber = dataRow.Field<string>("mivd_registration_no"),
                        RegistrationAuthorityandLocation = dataRow.Field<string>("mivd_registration_authority_and_location"),
                        PolicyNumber = dataRow.Field<string>("p_mi_policy_number"),
                        PolicyId = dataRow.Field<long>("p_mi_policy_id"),
                        PolicyPremiumAmount = dataRow.Field<double?>("p_mi_premium"),
                        From_Date = dataRow.Field<DateTime?>("p_mi_from_date"),
                        To_Date = dataRow.Field<DateTime?>("p_mi_to_date")
                    }).ToList();
                    ApplicationDetails.MBApplicationDetails = MBList;
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }
        //=========================MI Policy Print related functions ================================
        public VM_MotorInsurancePolicyPrintDetails MIPolicyPrintDetailsDll(string Type, long EmployeeCode, long ReferenceId)
        {
            VM_MotorInsurancePolicyPrintDetails objPD = new VM_MotorInsurancePolicyPrintDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",EmployeeCode),
                new SqlParameter("@applicationid",ReferenceId),
                new SqlParameter("@type",Type)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getMotorInsurancePolicyDetails");
                if (dsPD.Tables[0].Rows.Count > 0)
                {
                    objPD.nameandaddress= dsPD.Tables[0].Rows[0]["nameaddress"].ToString();
                    objPD.motor_insurance_app_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["mia_motor_insurance_app_id"]);
                    objPD.application_ref_no = Convert.ToInt64(dsPD.Tables[0].Rows[0]["mia_application_ref_no"]);
                    objPD.type_of_cover = Convert.ToInt32(dsPD.Tables[0].Rows[0]["mia_type_of_cover"]);
                    objPD.zone = dsPD.Tables[0].Rows[0]["p_mi_Zone"].ToString();
                    objPD.vehiclecategory = Convert.ToInt32(dsPD.Tables[0].Rows[0]["mivd_vehicle_category"]);
                    objPD.chasis_no = dsPD.Tables[0].Rows[0]["mia_chasis_no"].ToString();
                    objPD.engine_no = dsPD.Tables[0].Rows[0]["mia_engine_no"].ToString();
                    objPD.regisration_no = dsPD.Tables[0].Rows[0]["mia_regisration_no"].ToString();
                    objPD.vehicle_make = dsPD.Tables[0].Rows[0]["mia_vehicle_make"].ToString();
                    objPD.vehicleClass= dsPD.Tables[0].Rows[0]["typeofclass"].ToString();

                    objPD.year_of_manufacturer = dsPD.Tables[0].Rows[0]["vy_vehicle_year"] == DBNull.Value ? 0 : Convert.ToInt32(dsPD.Tables[0].Rows[0]["vy_vehicle_year"]);
                    objPD.seating_capacity_including_driver = dsPD.Tables[0].Rows[0]["mia_seating_capacity_including_driver"] == DBNull.Value ? (int?)null : Convert.ToInt32(dsPD.Tables[0].Rows[0]["mia_seating_capacity_including_driver"]);
                    objPD.cubic_capacity = dsPD.Tables[0].Rows[0]["mia_cubic_capacity"] == DBNull.Value ? (int?)null : Convert.ToInt32(dsPD.Tables[0].Rows[0]["mia_cubic_capacity"]);
                    objPD.vehicle_weight = dsPD.Tables[0].Rows[0]["mia_vehicle_weight"] == null ? (decimal)0 : Convert.ToDecimal(dsPD.Tables[0].Rows[0]["mia_vehicle_weight"]);
                    //
                    objPD.vehicletype = Convert.ToInt32(dsPD.Tables[0].Rows[0]["mivd_vehicle_type"]);
                    objPD.isfiberglassfitted = Convert.ToBoolean(dsPD.Tables[0].Rows[0]["isfiberglassfitted"]);
                    objPD.isautomobileassociation = Convert.ToBoolean(dsPD.Tables[0].Rows[0]["isautomobileassociation"]);
                    objPD.isdrivinginstitution = Convert.ToBoolean(dsPD.Tables[0].Rows[0]["isdrivinginstitution"]);
                    //
                    objPD.own_damage_value = (dsPD.Tables[0].Rows[0]["mia_own_damage_value"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsPD.Tables[0].Rows[0]["mia_own_damage_value"]);
                    objPD.premium_liability_value = (dsPD.Tables[0].Rows[0]["mia_premium_liability_value"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsPD.Tables[0].Rows[0]["mia_premium_liability_value"]);
                    objPD.vehicle_min_value = (dsPD.Tables[0].Rows[0]["mia_vehicle_min_value"] == DBNull.Value) ? 0 : Convert.ToInt32(dsPD.Tables[0].Rows[0]["mia_vehicle_min_value"]);
                    objPD.depreciation_value = (dsPD.Tables[0].Rows[0]["mia_depreciation_value"] == DBNull.Value) ? 0 : Convert.ToInt32(dsPD.Tables[0].Rows[0]["mia_depreciation_value"]);
                    objPD.ph_malus_value= (dsPD.Tables[0].Rows[0]["mia_ph_malus_value"] == DBNull.Value) ? 0 : Convert.ToInt32(dsPD.Tables[0].Rows[0]["mia_ph_malus_value"]);
                    if(objPD.ph_malus_value==0)
                    objPD.ph_ncb_value = (dsPD.Tables[0].Rows[0]["mia_ph_ncb_value"] == DBNull.Value) ? 0 : Convert.ToInt32(dsPD.Tables[0].Rows[0]["mia_ph_ncb_value"]);

                    objPD.policy_number = dsPD.Tables[0].Rows[0]["p_kgid_policy_number"].ToString();
                    objPD.premium = (dsPD.Tables[0].Rows[0]["p_premium"] == DBNull.Value) ? 0 : Convert.ToDouble(dsPD.Tables[0].Rows[0]["p_premium"]);
                    objPD.sanction_date = dsPD.Tables[0].Rows[0]["p_sanction_date"] == DBNull.Value ? "" : Convert.ToString(dsPD.Tables[0].Rows[0]["p_sanction_date"]);
                    objPD.from_date = dsPD.Tables[0].Rows[0]["from_date"] == DBNull.Value ? "" : Convert.ToString(dsPD.Tables[0].Rows[0]["from_date"]);
                    objPD.to_date = dsPD.Tables[0].Rows[0]["to_date"] == DBNull.Value ? "" : Convert.ToString(dsPD.Tables[0].Rows[0]["to_date"]);
                    objPD.tp_from_date = dsPD.Tables[0].Rows[0]["tp_from_date"] == DBNull.Value ? "" : Convert.ToString(dsPD.Tables[0].Rows[0]["tp_from_date"]);
                    objPD.tp_to_date = dsPD.Tables[0].Rows[0]["tp_to_date"] == DBNull.Value ? "" : Convert.ToString(dsPD.Tables[0].Rows[0]["tp_to_date"]);
                    ///
                    objPD.od_gov_discount = (dsPD.Tables[0].Rows[0]["od_gov_discount"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsPD.Tables[0].Rows[0]["od_gov_discount"]);
                    objPD.mia_own_damage_additional_value = (dsPD.Tables[0].Rows[0]["mia_own_damage_additional_value"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsPD.Tables[0].Rows[0]["mia_own_damage_additional_value"]);
                    objPD.mia_premium_liability_additional_value = (dsPD.Tables[0].Rows[0]["mia_premium_liability_additional_value"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsPD.Tables[0].Rows[0]["mia_premium_liability_additional_value"]);
                    objPD.liability_gov_discount = (dsPD.Tables[0].Rows[0]["liability_gov_discount"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsPD.Tables[0].Rows[0]["liability_gov_discount"]);
                    objPD.driver_amount = (dsPD.Tables[0].Rows[0]["driver_amount"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsPD.Tables[0].Rows[0]["driver_amount"]);
                    objPD.passenger_amount = (dsPD.Tables[0].Rows[0]["passenger_amount"] == DBNull.Value) ? 0 : Convert.ToDecimal(dsPD.Tables[0].Rows[0]["passenger_amount"]);
                    objPD.Endorsements= dsPD.Tables[0].Rows[0]["em_endorsement_value"].ToString();
                    if (Type == "Renewal")
                    {
                        //objPD.PolicyMonths = Convert.ToInt32(dsPD.Tables[0].Rows[0]["PolicyMonths"]);
                        objPD.PolicyMonths = dsPD.Tables[0].Rows[0]["PolicyMonths"] == DBNull.Value ? (int?)null : Convert.ToInt32(dsPD.Tables[0].Rows[0]["PolicyMonths"]);

                    }
                }

                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPD.Tables[1].Rows)
                        {
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 2)
                            {
                                objPD.insured_declared_value_amount = dr["vid_amount"].ToString();
                                //objPD.midv_own_damage_value = Convert.ToDecimal(dr["owndamage"]);
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 3)
                            {
                                objPD.non_electrical_accessories_amount = dr["vid_amount"].ToString();
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 4)
                            {
                                objPD.electrical_accessories_amount = dr["vid_amount"].ToString();
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 5)
                            {
                                objPD.side_car_trailer_amount = dr["vid_amount"].ToString();
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 6)
                            {
                                objPD.value_of_cng_lpg_amount = dr["vid_amount"].ToString();
                            }
                            if (Convert.ToInt64(dr["vid_idv_id"]) == 7)
                            {
                                objPD.total_amount = dr["vid_amount"].ToString();
                                //objPD.midv_own_damage_value =Convert.ToDecimal(dr["owndamage"]);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }
        //=========================MI Policy Bond Print related functions ================================
        public VM_DDOVerificationDetailsMI GetMBBondDetailsDll(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                DataSet dsEFD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",empId)
                };

                dsEFD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getGetMBBondDetails");
                if (dsEFD.Tables[0].Rows.Count > 0)
                {
                    var PolicyVerification = dsEFD.Tables[0].AsEnumerable().Select(dataRow => new PolicyPremiumDetailMI
                    {
                        EmployeeCode = dataRow.Field<long>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = dataRow.Field<long>("mia_application_ref_no"),
                        PolicyNumber = dataRow.Field<string>("policy_no"),
                        ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                        Status = dataRow.Field<string>("asm_status_desc"),
                        Remarks = dataRow.Field<string>("miwd_comments"),
                        FromDate = dataRow.Field<string>("p_from_date"),
                        ToDate = dataRow.Field<string>("p_to_date"),
                        MBBondDocPath = dataRow.Field<string>("p_mi_bond_upload_path"),
                        MBSignedBondDocPath = dataRow.Field<string>("p_mi_sign_bond_upload_path")
                    }).ToList();

                    verificationDetails.PolicyPremiumDetailMI = PolicyVerification;
                }
                if (dsEFD.Tables[1].Rows.Count > 0)
                {
                    var RenewalPolicyVerification = dsEFD.Tables[1].AsEnumerable().Select(dataRow => new RenewalPolicyPremiumDetailMI
                    {
                        EmployeeCode = dataRow.Field<long>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = dataRow.Field<long>("mira_application_ref_no"),
                        PolicyNumber = dataRow.Field<string>("policy_no"),
                        ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id"),
                        Status = dataRow.Field<string>("asm_status_desc"),
                        Remarks = dataRow.Field<string>("mirw_comments"),
                        FromDate = dataRow.Field<string>("rp_from_date"),
                        ToDate = dataRow.Field<string>("rp_to_date"),
                        MBBondDocPath = dataRow.Field<string>("rp_mi_bond_upload_path"),
                        MBSignedBondDocPath = dataRow.Field<string>("rp_mi_sign_bond_upload_path")
                    }).ToList();

                    verificationDetails.RenewalPolicyPremiumDetailMI = RenewalPolicyVerification;
                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        //=========================Sign MB Bond related functions ================================
        public string GetMBBondDocFileDLL(long AppId, long EmpId)
        {
            string returnString = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@AppId",AppId),
                    new SqlParameter("@EmpId",EmpId)
                };
                returnString = _Conn.ExecuteCmd(sqlparam, "sp_kgid_Get_MI_BOND_Doc");
            }
            catch (Exception ex)
            {

            }
            return returnString;
        }
        public string MBSignBondUploadDLL(long AppId, long EmpId, string DocPath)
        {
            string returnString = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@AppId",AppId),
                    new SqlParameter("@EmpId",EmpId),
                    new SqlParameter("@DocPath",DocPath)
                };
                returnString = _Conn.ExecuteCmd(sqlparam, "sp_kgid_MI_Sign_BOND_Doc_Upload");
            }
            catch (Exception ex)
            {

            }
            return returnString;
        }

        //=========================Save MB Bond related functions ================================
        public string MBBondDocUploadDll(long AppId, long EmpId, string DocPath, string DocType)
        {
            string returnString = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@AppId",AppId),
                    new SqlParameter("@EmpId",EmpId),
                    new SqlParameter("@DocPath",DocPath),
                    new SqlParameter("@DocType",DocType)
                };
                returnString = _Conn.ExecuteCmd(sqlparam, "sp_kgid_MB_BOND_Doc_Upload");
            }
            catch (Exception ex)
            {

            }
            return returnString;
        }
        //=========================Verify Data related functions ================================
        public VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForCWVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {

                //string description = GetCategoryDescription(Convert.ToInt32(Category));
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",2),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details_renewal");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {

                   
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                    EngineNo = dataRow.Field<string>("mivd_engine_no"),
                    VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                    VehicleYear =dataRow.Field<string>("year"),
                    TypeofCover = dataRow.Field<string>("type_of_cover"),
                   
                    Status = (dataRow.Field<string>("AppStatus") == "Send Back to Employee") ? (dataRow.Field<string>("AppStatus").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mira_user_category"))))) : (dataRow.Field<string>("AppStatus").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mira_user_category"))))),



                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                    PrevApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mira_user_category"),
                    //Status = dataRow.Field<string>("AppStatus")

                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("rp_mi_policy_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mira_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        public VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForSuperintendentVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                //string description = GetCategoryDescription(Convert.ToInt32(Category));
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",3),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details_renewal");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {

                    ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                    EngineNo = dataRow.Field<string>("mivd_engine_no"),
                    VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                    VehicleYear = dataRow.Field<string>("year"),
                    TypeofCover = dataRow.Field<string>("type_of_cover"),

                    Status = (dataRow.Field<string>("AppStatus") == "Send Back to Employee") ? (dataRow.Field<string>("AppStatus").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mira_user_category"))))) : (dataRow.Field<string>("AppStatus").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mira_user_category"))))),

                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                    PrevApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mira_user_category")
                    //Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("rp_mi_policy_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mira_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForADVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                //string description = GetCategoryDescription(Convert.ToInt32(Category));
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",15),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details_renewal");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                    EngineNo = dataRow.Field<string>("mivd_engine_no"),
                    VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                    VehicleYear = dataRow.Field<string>("year"),
                    TypeofCover = dataRow.Field<string>("type_of_cover"),

                    Status = (dataRow.Field<string>("AppStatus") == "Send Back to Employee") ? (dataRow.Field<string>("AppStatus").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mira_user_category"))))) : (dataRow.Field<string>("AppStatus").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mira_user_category"))))),

                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                    PrevApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mira_user_category")
                    //Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("rp_mi_policy_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mira_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForDDVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                //string description = GetCategoryDescription(Convert.ToInt32(Category));
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",5),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details_renewal");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                    EngineNo = dataRow.Field<string>("mivd_engine_no"),
                    VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                    VehicleYear = dataRow.Field<string>("year"),
                    TypeofCover = dataRow.Field<string>("type_of_cover"),

                    Status = (dataRow.Field<string>("AppStatus") == "Send Back to Employee") ? (dataRow.Field<string>("AppStatus").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mira_user_category"))))) : (dataRow.Field<string>("AppStatus").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mira_user_category"))))),

                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                    PrevApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mira_user_category")
                    //Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("rp_mi_policy_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mira_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        public VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForDVerification(long empId)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                //string description = GetCategoryDescription(Convert.ToInt32(Category));
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",6),
                    new SqlParameter("@EmpId",empId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_mi_select_ddo_details_renewal");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                    EngineNo = dataRow.Field<string>("mivd_engine_no"),
                    VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                    VehicleYear = dataRow.Field<string>("year"),
                    TypeofCover = dataRow.Field<string>("type_of_cover"),

                    Status = (dataRow.Field<string>("AppStatus") == "Send Back to Employee") ? (dataRow.Field<string>("AppStatus").ToString().Replace("Employee", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mira_user_category"))))) : (dataRow.Field<string>("AppStatus").ToString().Replace("Applicant", GetCategoryDescription(Convert.ToInt32(dataRow.Field<string>("mira_user_category"))))),

                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    District = dataRow.Field<string>("district"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                    PrevApplicationNumber = Convert.ToString(dataRow.Field<long>("mia_application_ref_no")),
                    ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id"),
                    CategoryId = dataRow.Field<string>("mira_user_category")
                    //Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id")
                }).ToList();
                var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = dataRow.Field<string>("rp_mi_policy_number"),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("mira_application_ref_no"),
                    Premium = dataRow.Field<string>("p_premium")
                }).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public IList<VM_MIWorkFlowDetails> GetRenewalWorkFlowDetails(long applicationId, int category)
        {
            IList<VM_MIWorkFlowDetails> workflowDetails = null;

            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@applicationId",applicationId),
                    new SqlParameter("@category",category)
                };

                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getWorkflowDetailsMI_Renewal");

                if (dsDDO.Tables.Count > 0 && dsDDO.Tables[0].Rows.Count > 0)
                {
                    workflowDetails = new List<VM_MIWorkFlowDetails>();
                    foreach (DataRow dr in dsDDO.Tables[0].Rows)
                    {
                        VM_MIWorkFlowDetails workflowDetail = new VM_MIWorkFlowDetails();
                        workflowDetail.ApplicationRefNo = dr["ApplicationRefNo"].ToString();
                        workflowDetail.From = dr["From"].ToString();
                        workflowDetail.To = dr["To"].ToString();
                        workflowDetail.Remarks = dr["Remarks"].ToString();
                        workflowDetail.Comments = dr["Comments"].ToString();
                        workflowDetail.CreationDateTime = dr["CreationDateTime"].ToString();
                        workflowDetail.ApplicationStatus = dr["ApplicationStatus"].ToString();
                        workflowDetail.NameOfApplicant = dr["name"].ToString();
                        workflowDetail.Category = Convert.ToInt32(dr["mira_user_category"]);
                        workflowDetails.Add(workflowDetail);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return workflowDetails.OrderByDescending(t => t.CreationDateTime).ToList();
        }
        public string SaveRenewalVerifiedDetailsDll(VM_MotorInsuranceDeptVerficationDetails objVerification)
        {
            var result = "";


            try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@employee_id",objVerification.EmpCode),
                    new SqlParameter("@miwd_application_id",objVerification.ApplicationRefNo),
                    new SqlParameter("@miwd_verified_by",objVerification.CreatedBy),

                    new SqlParameter("@miwd_checklist_verification_status",objVerification.VerifyProposerDetails),
                    new SqlParameter("@miwd_remarks",objVerification.Remarks),
                    new SqlParameter("@miwd_comments",objVerification.Comments),
                    new SqlParameter("@miwd_application_status",objVerification.ApplicationStatus),

                    new SqlParameter("@miwd_active_status",true),
                    new SqlParameter("@miwd_created_by",objVerification.CreatedBy),
                    new SqlParameter("@miwd_creation_datetime",DateTime.Now),


                };

                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_insertDepartmentWorkflowVerificationMI_renewal_test");
                if (objVerification.ApplicationStatus == 15)
                {
                    DataSet details = new DataSet();

                    SqlParameter[] sqlparamNotifDetails =
                    {
                        new SqlParameter("@employeeId", objVerification.EmpCode),
                        new SqlParameter("@applicationId",objVerification.ApplicationRefNo)

                    };

                    details = _Conn.ExeccuteDataset(sqlparamNotifDetails, "sp_kgid_getNotificationDetails");
                    VM_NotificationDetailsMI notificationDetails = new VM_NotificationDetailsMI();

                    if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
                    {
                        notificationDetails.DDOEmailId = details.Tables[0].Rows[0]["DDOEmailId"].ToString();
                        notificationDetails.EmpEmailId = details.Tables[0].Rows[0]["EmpEmailId"].ToString();
                        notificationDetails.EmpMobileNumber = Convert.ToInt64(details.Tables[0].Rows[0]["EmpMobileNumber"].ToString());
                        notificationDetails.EmpName = details.Tables[0].Rows[0]["EmpName"].ToString();

                    }

                    //SendInsurancePolicyNotification(notificationDetails);
                    //returnString = notificationDetails.PolicyNumber;
                }

            }
            catch (Exception ex)
            {

            }
            return result;


        }
        public VM_DDOVerificationDetailsMI getMIRenewalApplicationList(long empId, int Category)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            try
            {
                string description = GetCategoryDescription(Convert.ToInt32(Category));
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",empId),
                    new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getMIRenewalApplicationStatus");
                if (dsDDO.Tables[0].Rows.Count > 0)
                {
                    var CurrentStatusList = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeDDOVerificationDetailMI
                    {

                        ApplicationNumber = Convert.ToString(dataRow.Field<long>("mira_application_ref_no")),
                        PrevApplicationNumber = Convert.ToString(dataRow.Field<long>("prevRefNo")),
                        TypeofCover = dataRow.Field<string>("type_of_cover"),
                        VehicleYear = dataRow.Field<string>("year"),
                        VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                        Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", description)) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", description)),
                        //Status = (dataRow.Field<string>("asm_status_desc").Replace("Applicant",description)),
                        registrationNo = dataRow.Field<string>("mivd_registration_no"),
                        ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                        EngineNo = dataRow.Field<string>("mivd_engine_no"),
                        AppStatusID = dataRow.Field<int>("status"),
                        LastUpdatedDate = dataRow.Field<string>("mirw_creation_datetime"),
                        CategoryId = dataRow.Field<string>("mira_user_category"),
                        ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id"),
                        //PolicyPremium = (dataRow.Field<double?>("rp_mi_policy_number")==null)?0: dataRow.Field<double?>("rp_mi_policy_number"),
                        PolicyPremium = dataRow.Field<double?>("p_mi_premium"),
                        PolicyNumber = dataRow.Field<string>("rp_mi_policy_number"),
                        UnsignBondDocPath = dataRow.Field<string>("unsignbondpath"),
                        SignedBondDocPath = dataRow.Field<string>("signedbondpath"),
                        EmployeeCode = dataRow.Field<long?>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                    }).ToList();
                    //verificationDetails.LastUpdatedStatusForEmployees = CurrentStatusList;
                    verificationDetails.ViewStatusForEmployees = CurrentStatusList.Where(a => a.AppStatusID != 2 && a.AppStatusID != 1).ToList();

                    verificationDetails.LastUpdatedStatusForEmployees = CurrentStatusList.Where(a => a.AppStatusID == 2 || a.AppStatusID == 1).Select(a => a).ToList();
                }
                if (dsDDO.Tables[1].Rows.Count > 0)
                {
                    var MIPremiumDetailsList = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new PolicyPremiumDetailMI
                    {
                       
                        
                        
                        registrationNo = dataRow.Field<string>("mivd_registration_no"),
                        ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                        EngineNo = dataRow.Field<string>("mivd_engine_no"),
                        //VehicleYear = dataRow.Field<string>("year"),
                       
                        EmployeeCode = dataRow.Field<long>("mira_proposer_id"),
                        ApplicationNumber = dataRow.Field<long>("mira_application_ref_no"),
                        //VehicleMakeName = dataRow.Field<string>("vm_vehicle_make_desc"),
                        //VehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                        TypeofCover= dataRow.Field<string>("type_of_cover"),
                        PolicyId = dataRow.Field<long?>("rp_mi_renewal_policy_id"),
                        PolicyPremiumAmount = dataRow.Field<double?>("p_mi_premium"),
                        ApplicationId = dataRow.Field<long>("mira_motor_insurance_app_id")
                    }).ToList();
                    verificationDetails.PolicyPremiumDetailMI = MIPremiumDetailsList.Where(a => a.PolicyPremiumAmount != (double?)null).ToList(); ;
                }



            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public string GetEmployeeLoanDetails(long UserID)
        {
            string result = "";
            DataSet dsDDO = new DataSet();
            SqlParameter[] sqlparam =
            {
                    new SqlParameter("@EmpId",UserID),
                };
            result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_MI_GetEmployeeLoanDetails");
            return result;
        }


        #region Motor Insurance Cancellation Policy
        public VM_MotorInsuranceCancellation GetCancelReasonList()
        {
            VM_MotorInsuranceCancellation obj = new VM_MotorInsuranceCancellation();
            DataSet dsCD = new DataSet();

            SqlParameter[] parms = {

            };
            dsCD = _Conn.ExeccuteDataset(parms, "sp_kgid_mi_PolicyCancelReasonList");

            if (dsCD.Tables[0].Rows.Count > 0)
            {
                var list = dsCD.Tables[0].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("cr_reason_desc"),
                    Value = dataRow.Field<int>("cr_reason_id").ToString()
                }).ToList();
                obj.cancelReasonList = list;
            }
            return obj;

        }

        public string CheckVehicleNo(string vehicleNo, int Category)
        {
            SqlParameter[] parms = {
            new SqlParameter("@vehicleNo",vehicleNo),
            new SqlParameter("@category",Category),
            };

            return _Conn.ExecuteCmd(parms, "sp_CheckVehicleNoValid");

        }

        public int MIAppCancelRequestAction(VM_MotorInsuranceCancellation objMIcancellation)
        {
            int response = 0;

            dtDocumentsData.Columns.Add("mid_motor_insurance_application_id");
            dtDocumentsData.Columns.Add("mid_emp_id");
            dtDocumentsData.Columns.Add("mid_motor_document_type_id");
            dtDocumentsData.Columns.Add("mid_document_path");
            dtDocumentsData.Columns.Add("mid_status");
            DataTable dt_otherdata = CreateDataTable("MIDocumentDetailsResponseData");

            if (objMIcancellation.AuctionDetailsDoc != null)
            {
                SaveMIDocFileData(objMIcancellation.App_Proposer_ID, objMIcancellation.MI_App_Reference_ID, 16, true, objMIcancellation.AuctionDetailsDoc, objMIcancellation.AuctionDetailsDoc_filename, "AuctionDetails");
            }
            else if (objMIcancellation.AuctionDetailsDoc_filename != null)
            {
                SaveMIDocFileData(objMIcancellation.App_Proposer_ID, objMIcancellation.MI_App_Reference_ID, 16, true, objMIcancellation.AuctionDetailsDoc, objMIcancellation.AuctionDetailsDoc_filename, "AuctionDetails");
            }
            if (objMIcancellation.VIReportDoc != null)
            {
                SaveMIDocFileData(objMIcancellation.App_Proposer_ID, objMIcancellation.MI_App_Reference_ID, 17, true, objMIcancellation.VIReportDoc, objMIcancellation.VIReportDoc_filename, "VIReports");
            }
            else if (objMIcancellation.VIReportDoc_filename != null)
            {
                SaveMIDocFileData(objMIcancellation.App_Proposer_ID, objMIcancellation.MI_App_Reference_ID, 17, true, objMIcancellation.VIReportDoc, objMIcancellation.VIReportDoc_filename, "VIReports");
            }
            if (objMIcancellation.NOCDoc != null)
            {
                SaveMIDocFileData(objMIcancellation.App_Proposer_ID, objMIcancellation.MI_App_Reference_ID, 18, true, objMIcancellation.NOCDoc, objMIcancellation.NOCDoc_filename, "NOCDoc");
            }
            else if (objMIcancellation.NOCDoc_filename != null)
            {
                SaveMIDocFileData(objMIcancellation.App_Proposer_ID, objMIcancellation.MI_App_Reference_ID, 18, true, objMIcancellation.NOCDoc, objMIcancellation.NOCDoc_filename, "NOCDoc");
            }
            try
            {
                SqlParameter[] sqlparam =
                {
                  new SqlParameter("@EmpID",objMIcancellation.App_Proposer_ID),
                  new SqlParameter("@RefNo",objMIcancellation.MI_App_Reference_ID),
                  new SqlParameter("@MIDocumentDetailsResponseData",dtDocumentsData),
                  new SqlParameter("@Vehicleno",objMIcancellation.miVehicleNo),
                  new SqlParameter("@CancellationReason",objMIcancellation.reasonID)
                };
                response = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_mi_cancelrequestaction"));
            }
            catch (Exception ex)
            {
                response = 0;
            }
            return response;
        }

        public VM_MotorInsuranceCancellation VehicleDetailsForCancellationDll(string vehicleNo)
        {
            VM_MotorInsuranceCancellation obj = new VM_MotorInsuranceCancellation();
            DataSet dsPD = new DataSet();

            SqlParameter[] parms = {
            new SqlParameter("@vehicleNo",vehicleNo),
            };

            dsPD = _Conn.ExeccuteDataset(parms, "sp_getVehicleDetailsForCancellation");
            if (dsPD.Tables.Count == 1)
            {
                if (dsPD.Tables[0].Rows.Count > 0)
                {
                    var list = dsPD.Tables[0].AsEnumerable().Select(dataRow => new VM_VehicleDetailsForCancellation
                    {
                        vehicle_chassis_no = dataRow.Field<string>("mivd_chasis_no"),
                        vehicle_details_id = dataRow.Field<int>("mivd_vehicle_details_id"),
                        vehicle_application_id = dataRow.Field<long>("p_mi_application_id"),
                        vehicle_registration_no = dataRow.Field<string>("mivd_registration_no"),
                        policy_from_date = dataRow.Field<string>("p_mi_from_date"),
                        policy_to_date = dataRow.Field<string>("p_mi_to_date"),
                        status = dataRow.Field<string>("status"),

                    }).ToList();
                    obj.listVehicleDetails = list;
                }
            }
            return obj;
        }

        //ICT===============18-09-2021=================
     
        public string InsertChallanDetailsDll(long EmpID, int Category, string Applications, int ChallanAmount, string sanno, DateTime sandate, string voucherno, string hoa, string ddocode, string Type)
        {
            string result = string.Empty;
            try
            {
                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@Purposeid",2),
                    new SqlParameter("@Subpurposeid",13),
                    new SqlParameter("@Amount",ChallanAmount),
                    new SqlParameter("@AppID",Applications),
                    new SqlParameter("@Empid",EmpID),
                    //new SqlParameter("@GenerationDate",""),
                    new SqlParameter("@ChallanRefNo",""),
                    new SqlParameter("@cd_user_category",Category),
                    //new SqlParameter("@sanno",sanno),
                    //new SqlParameter("@sandate",sandate),
                    //new SqlParameter("@voucherno",voucherno),
                    new SqlParameter("@hoa_code",hoa),
                    new SqlParameter("@ddo_code",ddocode)
                };
                result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insert_mi_ttr_Challandetails"));
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    
        public VM_DDOVerificationDetailsMI GetChallanDetailsDll(string ChallanNo, long EmpID)
        {
            VM_DDOVerificationDetailsMI verificationDetails = new VM_DDOVerificationDetailsMI();
            DataSet dsCD = new DataSet();

            SqlParameter[] sqlparam =
              {
                    new SqlParameter("@ChallanRefNo",ChallanNo) ,
                    new SqlParameter("@EmpID",EmpID)
                };

            dsCD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_GetChallandetails");


            if (dsCD.Tables.Count > 0)
            {
                if (dsCD.Tables[1].Rows.Count > 0)
                {
                    var list = dsCD.Tables[1].AsEnumerable().Select(dataRow => new PolicyPremiumDetailMI
                    {
                        EmployeeCode = dataRow.Field<long>("mia_proposer_id"),
                        ApplicationNumber = dataRow.Field<long>("mia_application_ref_no"),
                        VehicleMakeName = dataRow.Field<string>("vm_vehicle_make_desc"),
                        VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                        registrationNo = dataRow.Field<string>("mivd_registration_no"),
                        ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                        EngineNo = dataRow.Field<string>("mivd_engine_no"),
                        VehicleYear = dataRow.Field<string>("year"),
                        TypeofCover = dataRow.Field<string>("type_of_cover"),
                        PolicyId = dataRow.Field<long?>("p_mi_policy_id"),
                        PolicyPremiumAmount = dataRow.Field<double?>("p_mi_premium"),
                        ApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id")
                    }).ToList();

                    verificationDetails.PolicyPremiumDetailMI = list;
                    verificationDetails.RefNos = Convert.ToString(dsCD.Tables[0].Rows[0]["CHALLANREFNO"]);
                    verificationDetails.ChallanAmount = Convert.ToString(dsCD.Tables[0].Rows[0]["CHALLANAMT"]);
                    verificationDetails.XML_FILE_NAME = Convert.ToString(dsCD.Tables[0].Rows[0]["XML_FILE_NAME"]);
                    verificationDetails.SAN_ORDER_NO = Convert.ToString(dsCD.Tables[0].Rows[0]["SAN_ORDER_NO"]);
                    verificationDetails.SAN_DATE = Convert.ToString(dsCD.Tables[0].Rows[0]["SAN_DATE"]);
                    verificationDetails.VOUCHERNO = Convert.ToString(dsCD.Tables[0].Rows[0]["VOUCHERNO"]);
                    verificationDetails.DDOCODE = Convert.ToString(dsCD.Tables[0].Rows[0]["dm_ddo_code"]);
                    verificationDetails.HOACODE = Convert.ToString(dsCD.Tables[0].Rows[0]["hoa_code"]);
                    verificationDetails.AMT_IN_WORD = Convert.ToString(dsCD.Tables[0].Rows[0]["AMT_IN_WORD"]);
                    verificationDetails.SAN_DATE_DISP = Convert.ToString(dsCD.Tables[0].Rows[0]["sandate_disp"]);
                    verificationDetails.CHALLAN_DATE = Convert.ToString(dsCD.Tables[0].Rows[0]["challan_date"]);
                    verificationDetails.RO_ADDRES = Convert.ToString(dsCD.Tables[0].Rows[0]["RO_ADDRES"]);
                }
            }
            return verificationDetails;
        }
        
        public string UpdateBPS025Ack(string AckFilePath, string ChallanRefno)
        {
            string result = string.Empty;
            try
            {

                XmlDocument xm = new XmlDocument();
                xm.Load(AckFilePath);

                XmlReader xmlReader = new XmlNodeReader(xm);
                DataSet ds = new DataSet();
                ds.ReadXml(xmlReader);

                DataRow dr = ds.Tables["status"].Rows[0];
                var statusflag = dr["statusFlag"].ToString();
                var statusMessage = dr["statusMessage"].ToString();

                if (statusflag != "success")
                {
                    statusMessage = ds.Tables["Fault"].Rows[0]["faultcode"].ToString() + "$" + ds.Tables["Fault"].Rows[0]["faultstring"].ToString();
                }


                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@p_cd_challan_ref_no",ChallanRefno),
                    new SqlParameter("@p_cd_ack_status_flag",statusflag),
                    new SqlParameter("@p_cd_ack_status_msg",statusMessage)

                };
                result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "Sp_KGID_Update_BPS025_Ack"));

            }
            catch (Exception ex)
            {
            }
            return result;
        }


        public string UpdateBPS025Request(string Upload, string ChallanRefno)
        {
            string result = string.Empty;
            try
            {


                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@p_cd_challan_ref_no",ChallanRefno),
                    new SqlParameter("p_cd_request_sent",Upload)
                };
                result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "Sp_KGID_Update_BPS025_Request"));

            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public VM_MotorInsurancePaymentStatus MotorInsurancePaymentStatusDll(string EmpId)
        {
            VM_MotorInsurancePaymentStatus obj = new VM_MotorInsurancePaymentStatus();
            DataSet dsPS = new DataSet();

            SqlParameter[] parms = {
            new SqlParameter("@P_EMP_ID",EmpId),
            };

            dsPS = _Conn.ExeccuteDataset(parms, "sp_get_motorinsurance_challan_status");
            if (dsPS.Tables[0].Rows.Count > 0)
            {
                var list = dsPS.Tables[0].AsEnumerable().Select(dataRow => new VM_MotorInsurancePaymentStatus
                {
                    cd_challan_ref_no = dataRow.Field<string>("cd_challan_ref_no"),
                    cd_amount = dataRow.Field<int>("cd_amount"),
                    NO_OF_APPL = dataRow.Field<int>("NO_OF_APPL"),
                    Uploded = dataRow.Field<string>("Uploded"),
                    cd_ack_status_flag = dataRow.Field<string>("cd_ack_status_flag"),
                    cs_status = dataRow.Field<string>("cs_status"),
                    miso_sanction_order_numner = dataRow.Field<string>("miso_sanction_order_numner"),
                    miso_sanction_order_date = dataRow.Field<string>("miso_sanction_order_date"),
                    cd_file_name_xml = dataRow.Field<string>("cd_file_name_xml"),
                }).ToList();
                obj.listChallanDetails = list;

            }


            if (dsPS.Tables[1].Rows.Count > 0)
            {
                var list = dsPS.Tables[1].AsEnumerable().Select(dataRow => new VM_MotorInsurancePaymentStatus
                {
                    cd_challan_ref_no = dataRow.Field<string>("cd_challan_ref_no"),
                    cd_amount = dataRow.Field<int>("cd_amount"),
                    NO_OF_APPL = dataRow.Field<int>("NO_OF_APPL"),
                    Uploded = dataRow.Field<string>("Uploded"),
                    cd_ack_status_flag = dataRow.Field<string>("cd_ack_status_flag"),
                    cs_status = dataRow.Field<string>("cs_status"),
                    miso_sanction_order_numner = dataRow.Field<string>("miso_sanction_order_numner"),
                    miso_sanction_order_date = dataRow.Field<string>("miso_sanction_order_date"),
                    cd_file_name_xml = dataRow.Field<string>("cd_file_name_xml"),
                }).ToList();

                obj.listChallanRejected = list;
            }


            if (dsPS.Tables[2].Rows.Count > 0)
            {
                var list = dsPS.Tables[2].AsEnumerable().Select(dataRow => new VM_MotorInsurancePaymentStatus
                {
                    cd_challan_ref_no = dataRow.Field<string>("cd_challan_ref_no"),
                    cd_amount = dataRow.Field<int>("cd_amount"),
                    NO_OF_APPL = dataRow.Field<int>("NO_OF_APPL"),
                    Uploded = dataRow.Field<string>("Uploded"),
                    cd_ack_status_flag = dataRow.Field<string>("cd_ack_status_flag"),
                    cs_status = dataRow.Field<string>("cs_status"),
                    miso_sanction_order_numner = dataRow.Field<string>("miso_sanction_order_numner"),
                    miso_sanction_order_date = dataRow.Field<string>("miso_sanction_order_date"),
                    cd_file_name_xml = dataRow.Field<string>("cd_file_name_xml"),
                }).ToList();

                obj.listChallanApprove = list;
            }

            if (dsPS.Tables[3].Rows.Count > 0)
            {
                var list = dsPS.Tables[3].AsEnumerable().Select(dataRow => new VM_MotorInsurancePaymentStatus
                {
                    cd_challan_ref_no = dataRow.Field<string>("cd_challan_ref_no"),
                    cd_amount = dataRow.Field<int>("cd_amount"),
                    NO_OF_APPL = dataRow.Field<int>("NO_OF_APPL"),
                    Uploded = dataRow.Field<string>("Uploded"),
                    cd_ack_status_flag = dataRow.Field<string>("cd_ack_status_flag"),
                    cs_status = dataRow.Field<string>("cs_status"),
                    miso_sanction_order_numner = dataRow.Field<string>("miso_sanction_order_numner"),
                    miso_sanction_order_date = dataRow.Field<string>("miso_sanction_order_date"),
                    cd_file_name_xml = dataRow.Field<string>("cd_file_name_xml"),
                }).ToList();

                obj.listChallanPendingAtDDO = list;
            }
            return obj;
        }

        #endregion

        

    }
}
