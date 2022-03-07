using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KGID_Models.Claim
{
    public class VM_EmpDetailForPrematureClaimApplication
    {

        public string Name { get; set; }
        public long Id { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public int Age { get; set; }

        [Required(ErrorMessage = "Please upload resignation letter")]
        public HttpPostedFileBase ResignationLetterDoc { get; set; }

        [Required(ErrorMessage = "Please enter date of resignation")]
        public DateTime? DateOfResignation { get; set; }

        [Required(ErrorMessage = "Please upload resignation acceptance letter")]
        public HttpPostedFileBase ResignationAcceptanceLetterDoc { get; set; }

        [Required(ErrorMessage = "Please enter date of acceptance of resignation")]
        public DateTime? DateOfAcceptanceOfResignation { get; set; }

        [Required(ErrorMessage = "Please upload relieving letter")]
        public HttpPostedFileBase RelievingLetterDoc { get; set; }

        [Required(ErrorMessage = "Please enter date of relieving")]
        public DateTime? DateOfRelieving { get; set; }

        [Required(ErrorMessage = "Please upload termination letter")]
        public HttpPostedFileBase TerminationLetterDoc { get; set; }

        [Required(ErrorMessage = "Please enter date of termination")]
        public DateTime? DateOfTermination { get; set; }

        public long DDOUserId { get; set; }
        public IList<SelectListItem> ClaimSubTypes { get; set; }
        public int ClaimTypeId { get; set; }

        [Required(ErrorMessage = "Please select claim sub type")]
        public int ClaimSubTypeId { get; set; }

        public string ResignationLetterDocPath { get; set; }
        public string ResignationAcceptanceLetterDocPath { get; set; }
        public string RelievingLetterDocPath { get; set; }
        public string TerminationLetterDocPath { get; set; }

        public int ResignationLetterDocTypeId { get; set; }
        public int ResignationAcceptanceLetterDocTypeId { get; set; }
        public int RelievingLetterDocTypeId { get; set; }
        public int TerminationLetterDocTypeId { get; set; }
    }

    public class VM_EmpDetailForDeathClaimApplication
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public int Age { get; set; }
        
        public IList<SelectListItem> ClaimSubTypes { get; set; }

        public int ClaimTypeId { get; set; }

        [Required(ErrorMessage = "Please select death claim sub type")]
        public int ClaimSubTypeId { get; set; }

        [Required(ErrorMessage = "Please enter Date of Death")]
        public DateTime? DateOfDeath { get; set; }

        [Required(ErrorMessage = "Please enter Cause of Death")]
        public string CauseOfDeath { get; set; }

        public DateTime? FirstToldIll { get; set; }

        public string DoctorName { get; set; }

        [Required(ErrorMessage = "Please enter name of nominee")]
        public string NameOfNominee { get; set; }

        [Required(ErrorMessage = "Please enter relation of nominee with deceased")]
        public string NomineeRelation { get; set; }
        public string DoctorDetail { get; set; }

        [Required(ErrorMessage = "Please enter if deceased was married at the time of death")]
        public bool? IsMarried { get; set; }

        [Required(ErrorMessage = "Please specify if nominee is alive or not")]
        public bool? IsNomineeAlive { get; set; }

        [Required(ErrorMessage = "Please specify age of nominee")]
        public int? AgeOfNominee { get; set; }

        public string NameOfGaurrdian { get; set; }
        public string DetailsOfGaurdian { get; set; }
        public long VerifiedByEmpId { get; set; }
    }
}
