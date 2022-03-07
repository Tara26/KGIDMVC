using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace KGID_Models.KGID_Login
{
    public  class tbl_agency_ddo_master
    {
        [Key]
        
        public long adm_agency_ddo_id { get; set; }

        [StringLength(300)]
        public string adm_agency_ddo_office { get; set; }

        public long adm_agency_id { get; set; }

        [StringLength(50)]
        public string adm_hod_desc { get; set; }

        [StringLength(15)]
        public string adm_designation { get; set; }

        public int? adm_pincode { get; set; }

        [StringLength(50)]
        public string adm_fax_no { get; set; }

        [StringLength(15)]
        public string adm_telephone_no { get; set; }

        [StringLength(50)]
        public string adm_email { get; set; }

        [StringLength(50)]
        public string adm_occupation { get; set; }

        [StringLength(500)]
        public string adm_address { get; set; }

        public bool? adm_status { get; set; }

        public DateTime? adm_creation_datetime { get; set; }

        public DateTime? adm_updation_datetime { get; set; }

        public int? adm_created_by { get; set; }

        public int? adm_updated_by { get; set; }

        public int? adm_district_id { get; set; }
    }
}
