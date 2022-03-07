using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KGID_Models.KGID_Loan
{
    public class VM_LoanApplication
    {
        public VM_LoanApplication()
        {
            Purposes = new List<SelectListItem>();
            FamilyMembers = new List<SelectListItem>();
        }

        public long PolicyNumber { get; set; }

        [Required(ErrorMessage = "Please select purpose of loan")]
        public string Purpose { get; set; }

        public List<SelectListItem> Purposes { get; set; }

        [Required(ErrorMessage = "Please select member for whom you are taking loan")]
        public string MemberRelation { get; set; }

        public List<SelectListItem> FamilyMembers { get; set; }

        [Required(ErrorMessage = "Please emter number of installments of loan")]
        public int? Installment { get; set; }

        public long? SpouseKGIDNumber { get; set; }

        [Required(ErrorMessage = "Please upload your policy document")]
        public HttpPostedFileBase Document { get; set; }

        public string DocumentFileName { get; set; }

        public string DocumentFilePath { get; set; }
    }
}
