using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_VerifyData
{
    public class tbl_verified_details
    {
        [Key]
        public int VD_ID { get; set; }
        public Nullable<int> EMPID_HRMS { get; set; }
        public int POLICY_NO { get; set; }
        public Nullable<int> FIRST_KGIDNO_HRMS { get; set; }
        public string EMPLOYEE_NAME_HRMS { get; set; }
        public string EMPLOYEE_NAME_KGID { get; set; }
        public string GENDER_HRMS { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DATE_OF_BIRTH_HRMS { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DATE_OF_BIRTH_KGID { get; set; }
        public int EMPLOYEE_AGE { get; set; }
        public string FATHER_NAME_HRMS { get; set; }
        public string FATHER_NAME_KGID { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DATE_OF_RISK { get; set; }
        public int PREMIUM { get; set; }
        public int SUM_ASSURED { get; set; }
        public string NOMINEE_NAME { get; set; }
        public string NOMINEE_RELATION { get; set; }
        public int? NOMINEE_AGE { get; set; }
        public string ACCEPTANCE_TYPE { get; set; }
        public string REMARKS { get; set; }
        public string STATUS { get; set; }   
        public int VERIFIED_BY { get; set; } 
    }
}
