using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDLoan
{
    public class tbl_loan_documents
    {
        [Key]
        public long ld_loan_document_id { get; set; }
        public long? ld_loan_application_id { get; set; }
        public string ld_upload_document_path { get; set; }
        public bool ld_active { get; set; }
        public DateTime ld_creation_datetime { get; set; }
        public int ld_created_by { get; set; }
        public DateTime ld_updation_datetime { get; set; }
        public int ld_updated_by { get; set; }
    }
}
