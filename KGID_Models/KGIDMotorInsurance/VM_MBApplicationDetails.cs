using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class VM_MBApplicationDetails
    {
        public VM_MBApplicationDetails()
        {
            MBApplicationDetails = new List<MBApplicationDetailsList>();
        }
        public IList<MBApplicationDetailsList> MBApplicationDetails { get; set; }
    }
    public class MBApplicationDetailsList
    {
        public long RowNumber { get; set; }

        public string Name { get; set; }

        public long EmployeeCode { get; set; }

        public string ApplicationNumber { get; set; }

        public long? ApplicationId { get; set; }

        public string VehicleMakeName { get; set; }

        public string VehicleManufactureName { get; set; }

        public string VehicleModelName { get; set; }

        public string RegisrationNumber { get; set; }

        public string RegistrationAuthorityandLocation { get; set; }

        public long? PolicyId { get; set; }

        public string PolicyNumber { get; set; }

        public Nullable<double> PolicyPremiumAmount { get; set; }

        public DateTime? From_Date { get; set; }

        public DateTime? To_Date { get; set; }
    }
}
