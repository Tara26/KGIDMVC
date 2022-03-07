using DLL.DBConnection;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KGID_Models.KGIDMotorInsurance.VM_MotorInsuranceRenewalDetails;

namespace DLL.KGIDMotorInsurance
{
    public class MotorInsuranceRenewalDetailsDll : IMotorInsuranceRenewalDetailsDll
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();

        public VM_MotorInsuranceRenewalDetails MIRenwalDetailsDll(long EmployeeCode, int Category)
        {
            VM_MotorInsuranceRenewalDetails MIRD = new VM_MotorInsuranceRenewalDetails();
            try
            {
                DataSet dsRD = new DataSet();
                SqlParameter[] sqlparam =
                 {
                    new SqlParameter("@employee_id",EmployeeCode),
                    new SqlParameter("@category",Category)
                 };
                dsRD = _Conn.ExeccuteDataset(sqlparam, "[sp_kgid_mi_renewalApplication]");
                dsRD.Tables["Table"].Merge(dsRD.Tables["Table1"]);
                if (dsRD.Tables[0].Rows.Count > 0)
                {
                    var MIApplicationRenewalList = dsRD.Tables[0].AsEnumerable().Select(dataRow => new MotorInsuranceRenewalDetailsMI
                    {
                        MIEmployeeId = dataRow.Field<long>("p_mi_emp_id"),
                        MIPolicyNumber = dataRow.Field<string>("p_mi_policy_number"),
                        MIPolicyId = dataRow.Field<long>("p_mi_policy_id"),
                        MIPremium = dataRow.Field<double>("p_mi_premium"),
                        MIApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                        MIApplicationNumber = dataRow.Field<long>("mia_application_ref_no"),
                        MIPolicyActiveStatus = dataRow.Field<bool>("p_mi_active_status"),
                        MIApplicationActiveStatus = dataRow.Field<bool>("mia_active"),
                        MIUserCategoryId = Convert.ToInt32(dataRow.Field<string>("mia_user_category")),
                        MIPolicyFromDate = dataRow.Field<DateTime?>("p_mi_from_date"),
                        MIPolicyToDate = dataRow.Field<DateTime?>("p_mi_to_date"),
                        MIRenewalStatus = dataRow.Field<string>("p_mi_RenewalStatus"),
                        MIChassisNo = dataRow.Field<string>("mivd_chasis_no"),

                        MIVehicleMakeName = dataRow.Field<string>("vm_vehicle_make_desc"),
                        //MIVehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        MIVehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                        MIVehicleManufactureDate = dataRow.Field<DateTime?>("mivd_date_of_manufacturer"),
                        MIVehicleRegistrationNumber = dataRow.Field<string>("mivd_registration_no"),
                        MIRegistrationName = dataRow.Field<string>("mivd_registration_authority_and_location"),
                        MIPrevApplicationNumber= dataRow.Field<long>("prevRefNo"),
                        MIWorkflowStatus= dataRow.Field<int?>("mirw_application_status"),
                        //MIRenewalApplicationId = ((dataRow.Field<long?>("mira_motor_insurance_app_id")).ToString() =="")?(long?)0: dataRow.Field<long?>("mira_motor_insurance_app_id"),
                        //MIRenewalApplicationNumber = ((dataRow.Field<long?>("mira_application_ref_no")).ToString() == "") ? (long?)0 : dataRow.Field<long?>("mira_application_ref_no"),

                    }).ToList();
                    MIRD.MotorInsuranceRenewalDetails = MIApplicationRenewalList;
                }
                
            }
            catch (Exception ex)
            {

            }
            return MIRD;
        }
    }
}
