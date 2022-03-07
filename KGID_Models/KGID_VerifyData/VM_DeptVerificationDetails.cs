using KGID_Models.Attrebute;
using KGID_Models.NBApplication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGID_Models.KGID_VerifyData
{
    public class VM_DeptVerificationDetails
    {
        public VM_DeptVerificationDetails()
        {
            WorkFlowDetails = new List<VM_WorkflowDetail>();
            DeductionLoadFactors = new List<SelectListItem>();
            NeedHealthOpinion = new List<HealthOpinion>();
            listUploadDocuments = new List<UploadedDocuments>();
        }
        public int VerificationId { get; set; }
        public long EmpCode { get; set; }
        public long ApplicationRefNo { get; set; }
        public long ApplicationId { get; set; }
        public Nullable<int> MedicalLeave { get; set; }
        public bool VerifyProposerDetails { get; set; }
        public bool VerifyPaymentDetails { get; set; }
        public bool VerifyMedicalDetails { get; set; }
        public bool VerifyMedicalCondition { get; set; }
        public string PreviousRemarks { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public int ApplicationStatus { get; set; }
        public string HealthReportUploadPath { get; set; }
        [ValidateFile]
       public HttpPostedFileBase HealthUploadDoc { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public string LoadFactor { get; set; }
        public string DeductionLoadFactor { get; set; }
        public int TotalDocCount { get; set; }
        public IList<SelectListItem> DeductionLoadFactors { get; set; }
        public bool IsHealthOpinion { get; set; }
        public decimal? SumAssured { get; set; }

        public IList<VM_WorkflowDetail> WorkFlowDetails { get; set; }
        //Added to check medical examination report to display or not
        public int Age { get; set; }
        public decimal PremiumAmount { get; set; }
        public bool IsMedicalRequired { get; set; }


        public string ApplicationFormLink { get; set; }
        public string MedicalFormLink { get; set; }

        //public HttpPostedFileBase SecondHealthUploadDoc { get; set; }
        //public string SecondHealthReportUploadPath { get; set; }

        public string KGIDDScNo { get; set; }
        public string PublicKey { get; set; }

        public List<HealthOpinion> NeedHealthOpinion { get; set; }
        public List<UploadedDocuments> listUploadDocuments { get; set; }
    }

    public class HealthOpinion
    {
        public string HealthOpinionDocuments { get; set; }
        public int HealthOpinionType { get; set; }
    }
    public class UploadedDocuments
    {
        public string UploaddocPath { get; set; }
        public string UploaddocType { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileTypeAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _DefaultErrorMessage = "Only the following file types are allowed: {0}";
        private IEnumerable<string> _ValidTypes { get; set; }

        public FileTypeAttribute(string validTypes)
        {
            _ValidTypes = validTypes.Split(',').Select(s => s.Trim().ToLower());
            ErrorMessage = string.Format(_DefaultErrorMessage, string.Join(" or ", _ValidTypes));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IEnumerable<HttpPostedFileBase> files = value as IEnumerable<HttpPostedFileBase>;
            if (files != null)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null && !_ValidTypes.Any(e => file.FileName.EndsWith(e)))
                    {
                        return new ValidationResult(ErrorMessageString);
                    }
                }
            }
            return ValidationResult.Success;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "filetype",
                ErrorMessage = ErrorMessageString
            };
            rule.ValidationParameters.Add("validtypes", string.Join(",", _ValidTypes));
            yield return rule;
        }
    }
}
