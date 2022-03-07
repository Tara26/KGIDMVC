using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGIDMotorInsurance
{
    public class VM_MI_Upload_Documents
    {
        public int IsDocType { get; set; }
        public long App_Proposer_ID { get; set; }
        public long MI_App_Reference_ID { get; set; }
        public string MIPAgetype { get; set; }
        #region NewPurchasedVehicle
        public HttpPostedFileBase ProposalDocNewPurchase { get; set; }
        public string ProposalDocNewPurchase_filename { get; set; }
        public HttpPostedFileBase GovtSanctionDoc { get; set; }
        public string GovtSanctionDoc_filename { get; set; }
        public HttpPostedFileBase ProformaInvoiceDoc { get; set; }
        public string ProformaInvoiceDoc_filename { get; set; }

        public HttpPostedFileBase PSaleCertificateDoc { get; set; }
        public string PSaleCertificateDoc_filename { get; set; }
        #endregion
        #region PreviouslyInsuredVehicle
        public HttpPostedFileBase OldInsuranceCertificate { get; set; }
        public string OldInsuranceCertificate_filename { get; set; }
        public HttpPostedFileBase PIVGovtSanctionDoc { get; set; }
        public string PIVGovtSanctionDoc_filename { get; set; }
        public HttpPostedFileBase PIVProformaInvoiceDoc { get; set; }
        public string PIVProformaInvoiceDoc_filename { get; set; }

        public HttpPostedFileBase PIVPSaleCertificateDoc { get; set; }
        public string PIVPSaleCertificateDoc_filename { get; set; }
        #endregion
        #region DonatedVehicle
        public HttpPostedFileBase ProposalDocDonatedVehicle { get; set; }
        public string ProposalDocDonatedVehicle_filename { get; set; }
        public HttpPostedFileBase DonationDoc { get; set; }
        public string DonationDoc_filename { get; set; }
        public HttpPostedFileBase SaleCertificateDoc { get; set; }
        public string SaleCertificateDoc_filename { get; set; }
        public HttpPostedFileBase TaxInvoiceDoc { get; set; }
        public string TaxInvoiceDoc_filename { get; set; }
        public HttpPostedFileBase DonatedEmissionCertificate { get; set; }
        public string DonatedEmissionCertificate_filename { get; set; }
        #endregion
        #region SeizedVehicle

        public HttpPostedFileBase SeizedEmissionCertificate { get; set; }
        public string SeizedEmissionCertificate_filename { get; set; }
        public HttpPostedFileBase ProposalDocSeizedVehicle { get; set; }
        public string ProposalDocSeizedVehicle_filename { get; set; }
        public HttpPostedFileBase cCertificateDoc { get; set; }
        public string cCertificateDoc_filename { get; set; }
        public HttpPostedFileBase RTOcertificateDoc { get; set; }
        public string RTOcertificateDoc_filename { get; set; }
        public HttpPostedFileBase FitnessCertificateRTODoc { get; set; }
        public string FitnessCertificateRTODoc_filename { get; set; }

  		public HttpPostedFileBase AuctionDoc { get; set; }
        public string AuctionDoc_filename { get; set; }
        public HttpPostedFileBase MIVIDoc { get; set; }
        public string MIVIDoc_filename { get; set; }
        public HttpPostedFileBase MINOCDoc { get; set; }
        public string MINOCDoc_filename { get; set; }
        #endregion

        public int OtherDocumentCount { get; set; }
        // public List<OtherDocumentsFileUpload> OtherDocumentsFile { get; set; }

        public HttpPostedFileBase OtherDocument1 { get; set; }
        public string OtherDocument1_filename { get; set; }

        public HttpPostedFileBase OtherDocument2 { get; set; }
        public string OtherDocument2_filename { get; set; }

        public HttpPostedFileBase OtherDocument3 { get; set; }
        public string OtherDocument3_filename { get; set; }

        public HttpPostedFileBase OtherDocument4 { get; set; }
        public string OtherDocument4_filename { get; set; }

        public HttpPostedFileBase OtherDocument5 { get; set; }
        public string OtherDocument5_filename { get; set; }

        // -----------------Renewal----------------------------------------------------

        public HttpPostedFileBase RenewalProposalDoc { get; set; }
        public string RenewalProposalDoc_filename { get; set; }
        public HttpPostedFileBase RenewalFitnessCertificateRTODoc { get; set; }
        public string RenewalFitnessCertificateRTODoc_filename { get; set; }
    }
}
