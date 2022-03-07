using System.Collections.Generic;
using System.Web.Mvc;

namespace KGID_Models.NBApplication
{
    public class VM_FamilyDetail
    {
        public long Id { get; set; }
        public long? EmpId { get; set; }
        public int? RelationId { get; set; }
        public string Relation { get; set; }
        public string NameOfMember { get; set; }
        public string DateOfBirth { get; set; }
        public bool? AliveDead { get; set; }
        public string DateOfDeath { get; set; }
        public int? Age { get; set; }
        public string ReasonOfDeath { get; set; }
        public string HealthCondition { get; set; }
        public bool? IsSiblingMarried { get; set; }
        public long? ApplicationId { get; set; }
        public string EditDeleteStatus { get; set; }
        public bool AppliactionSentBack { get; set; }
        public bool ApplicationInsured { get; set; }
        
    }

    public class VM_FamilyDetails
    {
        public VM_FamilyDetails()
        {
            FamilyDetails = new List<VM_FamilyDetail>();
        }
        public long EmployeeId { get; set; }
        public long ApplicationId { get; set; }
        public IList<VM_FamilyDetail> FamilyDetails { get; set; }
        public int NoOfBrother { get; set; }
        public int NoOfSister { get; set; }
        public int NoOfChildren { get; set; }
        public int RelationID { get; set; }
        public IEnumerable<SelectListItem> FamilyRelationList { get; set; }
        public bool IsOrphan { get; set; }
        public bool IsMarried { get; set; }
    }
}
