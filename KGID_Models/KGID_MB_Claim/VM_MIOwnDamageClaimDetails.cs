using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_MB_Claim
{
    public class VM_MIOwnDamageClaimDetails
    {
        public VM_MIOwnDamageClaimDetails()
        {
            MIOwnDamageClaimDetails = new List<MotorInsuranceODDetailsMI>();
        }
        public IList<MotorInsuranceODDetailsMI> MIOwnDamageClaimDetails { get; set; }
        public class MotorInsuranceODDetailsMI
        {
            public long MIEmployeeId { get; set; }
            public string MIPolicyNumber { get; set; }
            public long MIPolicyId { get; set; }
            public double MIPremium { get; set; }
            //public float MIPremium { get; set; }
            public long MIApplicationId { get; set; }
            public long MIApplicationNumber { get; set; }
            public bool MIPolicyActiveStatus { get; set; }
            public bool MIApplicationActiveStatus { get; set; }
            public int? MIUserCategoryId { get; set; }

            public DateTime? MIPolicyFromDate { get; set; }
            public DateTime? MIPolicyToDate { get; set; }

            public string MIVehicleMakeName { get; set; }
            public string MIVehicleManufactureName { get; set; }
            public string MIVehicleModelName { get; set; }
            public DateTime? MIVehicleManufactureDate { get; set; }
            public string MIVehicleRegistrationNumber { get; set; }
            public string MIRegistrationName { get; set; }
            public string MICubicCapacity { get; set; }
            public string MINoOfPassengers { get; set; }


            public long? MIRenewalApplicationId { get; set; }
            public long? MIRenewalApplicationNumber { get; set; }
            //
            public string MITypeOfCover { get; set; }
            public string MIPolicyType { get; set; }
        }


        //public Nullable<int> mipd_pincode { get; set; }
        //public Nullable<long> mipd_telephone_no { get; set; }
        //public Nullable<long> mipd_fax_no { get; set; }
        //public string mipd_email { get; set; }
        //public string mipd_occupation { get; set; }
        //public string mipd_type_of_cover { get; set; }
        //public string mipd_Department { get; set; }
    }
}
