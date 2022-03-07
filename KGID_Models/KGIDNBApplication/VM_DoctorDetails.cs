using KGID_Models.KGID_Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGID_Models.KGIDNBApplication
{
    public class VM_DoctorDetails
    {
        public VM_DoctorDetails()
        {
            DoctorDetail = new VM_DoctorDetail();
            //Doctors = Enumerable.Empty<SelectListItem>();
        }

        //public IEnumerable<SelectListItem> KMCCodes { get; set; }

        //[Required(ErrorMessage = "Please select KMC code of doctor")]
        //[Display(Name = "KMC Codes")]
        //public string KMCCodeId { get; set; }

        [Required(ErrorMessage = "Please enter KMC code of doctor")]
        [Display(Name = "KMC Code")]
        public string KMCCode { get; set; }

        //public IEnumerable<SelectListItem> Doctors { get; set; }

        //[Required(ErrorMessage = "Please select doctor")]
        //[Display(Name = "Doctor Name")]
        //public int DoctorId { get; set; }

        public long EmpId { get; set; }

        public VM_DoctorDetail DoctorDetail { get; set; }
    }

    public class VM_DoctorDetail
    {
        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Display(Name = "KGID")]
        public string KGID { get; set; }

        [Display(Name = "Name of Hospital")]
        public string NameOfOffice { get; set; }
    }
}
