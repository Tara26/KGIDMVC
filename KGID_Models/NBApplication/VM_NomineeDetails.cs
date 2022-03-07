using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGID_Models.NBApplication
{
    public class VM_NomineeDetails
    {
        public VM_NomineeDetails()
        {
            NomineeDetails = new List<VM_NomineeDetail>();
        }

        public long EmployeeId { get; set; }
        public long ApplicationId { get; set; }
        public IList<VM_NomineeDetail> NomineeDetails { get; set; }
    }

    public class VM_NomineeDetail
    {
        public long Id { get; set; }
        public long? EmpId { get; set; }
        public long? ApplicationId { get; set; }
        public int Age { get; set; }
        public int? RelationId { get; set; }
        public string Relation { get; set; }
        public string NameOfNominee { get; set; }
        public string NameOfGaurdian { get; set; }
        public int? GaurdianRelationId { get; set; }
        public string GaurdianRelation { get; set; }
        public int? PercentageShare { get; set; }
        public long? FamilyMemberId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDateTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int UpdatedBy { get; set; }
        public string NomineeAge { get; set; }
    }
    public class VM_DropDownList
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class BindDropDownModel
    {
        public List<VM_DropDownList> GuardianList { get; set; }
        public List<VM_DropDownList> NomineeList { get; set; }
    }
}
