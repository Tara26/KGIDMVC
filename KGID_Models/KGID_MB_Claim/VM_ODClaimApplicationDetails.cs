using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGID_MB_Claim
{
    public class VM_ODClaimApplicationDetails
    {
        public IList<ClaimComponentList1> ClaimComponentListDetails { get; set; }
        //public List<ClaimComponentList1> ClaimComponentsList { get; set; }
        public long Odca_id { get; set; }
        public string Odca_claim_app_no { get; set; }
        public long Odca_claim_id { get; set; }
        public long Odca_proposer_id { get; set; }
        public string Odca_category_id { get; set; }
        public string Odca_vehicle_number { get; set; }
        //public long Odca_policy_id { get; set; }
        public string Odca_policy_number { get; set; }
        public DateTime? Odca_date_time_of_accident { get; set; }
        public long Odca_accident_cause_id { get; set; }
        public string Odca_place_of_accident { get; set; }
        public int Odca_district_id { get; set; }
        public int Odca_taluka_id { get; set; }
        public decimal Odca_damage_cost { get; set; }
        public bool Odca_active_status { get; set; }

        //Claim Vehicle Images
        public HttpPostedFileBase ClaimVehicleImages { get; set; }
        public string ClaimVehicleImagesFileName { get; set; }

        //Fire Accident Cliam Type
        public HttpPostedFileBase ClaimFormDoc1 { get; set; }
        public string ClaimFormDoc1FileName { get; set; }

        public HttpPostedFileBase RegistrationCopyDoc1 { get; set; }
        public string RegistrationCopyDoc1FileName { get; set; }

        public HttpPostedFileBase DLDoc1 { get; set; }
        public string DLDoc1FileName { get; set; }

        public HttpPostedFileBase FIRDoc1 { get; set; }
        public string FIRDoc1FileName { get; set; }

        public HttpPostedFileBase EstimationReportDoc1 { get; set; }
        public string EstimationReportDoc1FileName { get; set; }

        //Theft Cases Cliam Type
        public HttpPostedFileBase ClaimFormDoc2 { get; set; }
        public string ClaimFormDoc2FileName { get; set; }

        public HttpPostedFileBase RegistrationCopyDoc2 { get; set; }
        public string RegistrationCopyDoc2FileName { get; set; }

        public HttpPostedFileBase DLDoc2 { get; set; }
        public string DLDoc2FileName { get; set; }

        public HttpPostedFileBase FIRDoc2 { get; set; }
        public string FIRDoc2FileName { get; set; }

        public HttpPostedFileBase CReportDoc2 { get; set; }
        public string CReportDoc2FileName { get; set; }

        public HttpPostedFileBase AffidavitDoc2 { get; set; }
        public string AffidavitDoc2FileName { get; set; }

        public HttpPostedFileBase ClaimDischargeFormDoc2 { get; set; }
        public string ClaimDischargeFormDoc2FileName { get; set; }

        public HttpPostedFileBase AdvPayeeRecepit2 { get; set; }
        public string AdvPayeeRecepit2FileName { get; set; }

        public HttpPostedFileBase RecipientIDDoc2 { get; set; }
        public string RecipientIDDoc2FileName { get; set; }

        //Natural Calamities Cliam Type
        public HttpPostedFileBase ClaimFormDoc3 { get; set; }
        public string ClaimFormDoc3FileName { get; set; }

        public HttpPostedFileBase RegistrationCopyDoc3 { get; set; }
        public string RegistrationCopyDoc3FileName { get; set; }

        public HttpPostedFileBase DLDoc3 { get; set; }
        public string DLDoc3FileName { get; set; }

        public HttpPostedFileBase FIRDoc3 { get; set; }
        public string FIRDoc3FileName { get; set; }

        public HttpPostedFileBase EstimationReportDoc3 { get; set; }
        public string EstimationReportDoc3FileName { get; set; }

        //Accident Cliam Type
        public HttpPostedFileBase ClaimFormDoc4 { get; set; }
        public string ClaimFormDoc4FileName { get; set; }

        public HttpPostedFileBase RegistrationCopyDoc4 { get; set; }
        public string RegistrationCopyDoc4FileName { get; set; }

        public HttpPostedFileBase DLDoc4 { get; set; }
        public string DLDoc4FileName { get; set; }

        public HttpPostedFileBase FIRDoc4 { get; set; }
        public string FIRDoc4FileName { get; set; }

        public HttpPostedFileBase EstimationReportDoc4 { get; set; }
        public string EstimationReportDoc4FileName { get; set; }

        public HttpPostedFileBase RTOReportDoc4 { get; set; }
        public string RTOReportDoc4FileName { get; set; }
    }
    public class MIODClaimDocumentsData
    {
        public long odcdd_claim_app_id { get; set; }
        public long odcdd_claim_due_id { get; set; }
        public string odcdd_doc_upload_path { get; set; }
        public bool odcdd_active_status { get; set; }
        //public DateTime vod_creation_datetime { get; set; }
        //public DateTime vod_updation_datetime { get; set; }
        //public int vod_created_by { get; set; }
        //public int vod_updated_by { get; set; }
    }
    public class ClaimComponentList1
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
