using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.NBApplication
{
    public class VM_DoctorDetails
    {
        public long Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? ApplicationId { get; set; }
        public long? DoctorId { get; set; }

        [Display(Name = "Doctor Name")]
        public string NameOfDoctor { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Display(Name = "Name of Office")]
        public string NameOfHospital { get; set; }

        [Display(Name = "KMC / IMC code")]
        public string KMCCode { get; set; }
        public string IMCCode { get; set; }

        [Display(Name = "KGID")]
        public long? PolicyId { get; set; }
        public string KGIDOfDoctor { get; set; }
        public bool? IsActive { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdationDateTime { get; set; }
        public bool emdd_is_kmc_doctor { get; set; }
        [Display(Name = "Bank Account Number")]
        public string BankAccNumber { get; set; }
        [Display(Name = "IFSC code")]
        public string IFSCCode { get; set; }
        [Display(Name = "MICR code")]
        public string MICRCode { get; set; }
        [Display(Name = "Place of Office")]
        public string PlaceOfHospital { get; set; }
    }

    public class VM_DoctorDetail
    {
        public string IMCCode { get; set; }
        public string KMCCode { get; set; }
        public string DoctorName { get; set; }
        public string KGIDOfDoctor { get; set; }
        public string Designation { get; set; }
        public string NameOfHospital { get; set; }
        public long DoctorId { get; set; }
        public bool emdd_is_kmc_doctor { get; set; }
        public string BankAccNumber { get; set; }
        public string IFSCCode { get; set; }
        public string MICRCode { get; set; }
        public string PlaceOfHospital { get; set; }
    }
}
