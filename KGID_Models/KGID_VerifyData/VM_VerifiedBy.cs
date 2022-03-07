using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGID_Models.KGID_VerifyData
{
    public class VM_VerifiedBy
    {
        public tbl_verification_details Verification_details { get; set; }
        public string UserName { get; set; }
        public string KGIDNumber { get; set; }


    }
    public class VM_Remarks
    {
        public static IEnumerable<SelectListItem> GetRemarkList()
        {
            var types = new List<SelectListItem >();
            types.Add(new SelectListItem() { Text = "FILE NOT FOUND", Value = "FILE NOT FOUND" });
            types.Add(new SelectListItem() { Text = "DATA CORRECTED AS PER KGID", Value = "DATA CORRECTED AS PER KGID" });
            types.Add(new SelectListItem() { Text = "DATA CORRECTED AS PER HRMS", Value = "DATA CORRECTED AS PER HRMS" });
            types.Add(new SelectListItem() { Text = "NO CORRECTION FOUND", Value = "NO CORRECTION FOUND" });
            types.Add(new SelectListItem() { Text = "DETAILS MISSING IN THE FILE", Value = "DETAILS MISSING IN THE FILE" });
            types.Add(new SelectListItem() { Text = "FILE DISCHARGED", Value = "FILE DISCHARGED" });

            return types;
        }
    }
}
