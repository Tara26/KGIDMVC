using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGID_Models.KGIDMotorInsurance
{
    public class VM_MotorInsuranceProposerDetails
    {
        public VM_MotorInsuranceProposerDetails()
        {
            mipd_type_of_cover_list = new List<SelectListItem>();
        }
        public string mipd_kgid_application_number { get; set; }
        public long mipd_employee_id { get; set; }
        public string mipd_proposer_fullname { get; set; }
        public string mipd_address { get; set; }
        public int mipd_pincode { get; set; }
        public Nullable<long> mipd_telephone_no { get; set; }
        public Nullable<long> mipd_fax_no { get; set; }
        public string mipd_email { get; set; }
        public string mipd_occupation { get; set; }
        public string mipd_type_of_cover { get; set; }
        public int mipd_type_of_cover_id { get; set; }
        public string mipd_Department { get; set; }
        public string mipd_pagetype { get; set; }
        public string mipd_policystartdate { get; set; }
        public string mipd_policyenddate { get; set; }
        public string mipd_policypremium { get; set; }
        public string mipd_policynumber { get; set; }
        public long mipd_old_application_Ref_number { get; set; }
        public string mipd_kgid_renewal_application_number { get; set; }
        public string mipd_category { get; set; }
        public int mipd_selectedCategory { get; set; }
        //
        public string QRCode { get; set; }

        //Application details
        public long mipd_application_id { get; set; }
        //public string kad_kgid_application_number { get; set; }
        public string mipd_date_of_submission { get; set; }

        //
        public string PolicyMonths { get; set; }


        public List<SelectListItem> mipd_type_of_cover_list { get; set; }
    }

}
