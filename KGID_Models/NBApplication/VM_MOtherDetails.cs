using System.Web;

namespace KGID_Models.NBApplication
{
    public class VM_MOtherDetails
    {
        public long employee_id { get; set; }
        public long application_id { get; set; }
        public int tab_number { get; set; }
        public int gender_id { get; set; }

        public bool AdmittedToHospital { get; set; }
        public string AdmittedToHospitalDesc { get; set; }
        public bool Accident { get; set; }
        public string AccidentDesc { get; set; }
        public bool UndergoneTest { get; set; }
        public string UndergoneTestDesc { get; set; }
        public bool UndergoneAnyTreatment { get; set; }
        public string UndergoneAnyTreatmentDesc { get; set; }

        public HttpPostedFileBase AccidentDoc { get; set; }
        public string AccidentDocFileName { get; set; }
        public HttpPostedFileBase AdmittedToHospitalDoc { get; set; }
        public string AdmittedToHospitalDocFileName { get; set; }
        public HttpPostedFileBase UndergoneTestDoc { get; set; }
        public string UndergoneTestDocFileName { get; set; }
        public HttpPostedFileBase UndergoneAnyTreatmentDoc { get; set; }
        public string UndergoneAnyTreatmentDocFileName { get; set; }


        public long emd_application_id { get; set; }
        public long emd_emp_id { get; set; }
        public int emd_medical_health_id { get; set; }
        public bool emd_status { get; set; }
        public string emd_remarks { get; set; }
        public string emd_upload_document_path { get; set; }


        public bool HavingIllnessinChest { get; set; }
        public string HavingIllnessinChestDesc { get; set; }
        public bool HavingIllnessinTeeth { get; set; }
        public string HavingIllnessinTeethDesc { get; set; }
        public bool Disability { get; set; }
        public string DisabilityDesc { get; set; }
        public bool HaveThyroid { get; set; }
        public string HaveThyroidDesc { get; set; }
        public bool EnlargementSpleenLiver { get; set; }
        public string EnlargementSpleenLiverDesc { get; set; }
        public bool GastroIntestinateTrack { get; set; }
        public string GastroIntestinateTrackDesc { get; set; }
        public bool SufferFromHernia { get; set; }
        public string SufferFromHerniaDesc { get; set; }

        public HttpPostedFileBase HavingIllnessinChestDoc { get; set; }
        public HttpPostedFileBase HavingIllnessinTeethDoc { get; set; }
        public HttpPostedFileBase DisabilityDoc { get; set; }
        public HttpPostedFileBase HaveThyroidDoc { get; set; }
        public HttpPostedFileBase EnlargementSpleenLiverDoc { get; set; }
        public HttpPostedFileBase GastroIntestinateTrackDoc { get; set; }
        public HttpPostedFileBase SufferFromHerniaDoc { get; set; }
        public string HavingIllnessinChestDocFileName { get; set; }
        public string HavingIllnessinTeethDocFileName { get; set; }
        public string DisabilityDocFileName { get; set; }
        public string HaveThyroidDocFileName { get; set; }
        public string EnlargementSpleenLiverDocFileName { get; set; }
        public string GastroIntestinateTrackDocFileName { get; set;  }
        public string SufferFromHerniaDocFileName { get; set; }


        public bool SufferFromUrinaryTract { get; set; }
        public string SufferFromUrinaryTractDesc { get; set; }
        public HttpPostedFileBase SufferFromUrinaryTractDoc { get; set; }
        public string SufferFromUrinaryTractDocFileName { get; set; }

        public bool NervousSystemDisease { get; set; }
        public string NervousSystemDiseaseDesc { get; set; }
        public HttpPostedFileBase NervousSystemDiseaseDoc { get; set; }
        public string NervousSystemDiseaseDocFileName { get; set; }

        public bool UndergoneSurgery { get; set; }
        public string UndergoneSurgeryDesc { get; set; }
        public HttpPostedFileBase UndergoneSurgeryDoc { get; set; }
        public string UndergoneSurgeryDocFileName { get; set; }

        public bool AccidentWoundMarks { get; set; }
        public string AccidentWoundMarksDesc { get; set; }
        public HttpPostedFileBase AccidentWoundMarksDoc { get; set; }
        public string AccidentWoundMarksDocFileName { get; set; }

        public bool AdverseSymptomInHealth { get; set; }
        public string AdverseSymptomInHealthDesc { get; set; }
        public HttpPostedFileBase AdverseSymptomInHealthDoc { get; set; }
        public string AdverseSymptomInHealthDocFileName { get; set; }

        public bool BreastIllness { get; set; }
        public string BreastIllnessDesc { get; set; }
        public HttpPostedFileBase BreastIllnessDoc { get; set; }
        public string BreastIllnessDocFileName { get; set; }

        public bool BreastCancer { get; set; }
        public string BreastCancerDesc { get; set; }
        public HttpPostedFileBase BreastCancerDoc { get; set; }
        public string BreastCancerDocFileName { get; set; }

        public bool ClueInPregancy { get; set; }
        public string ClueInPregancyDesc { get; set; }
        public HttpPostedFileBase ClueInPregancyDoc { get; set; }
        public string ClueInPregancyDocFileName { get; set; }


        public bool BiologicalIllness { get; set; }
        public string BiologicalIllnessDesc { get; set; }
        public HttpPostedFileBase BiologicalIllnessDoc { get; set; }
        public string BiologicalIllnessDocFileName { get; set; }

        public bool GoodLifeCycle { get; set; }
        public string GoodLifeCycleDesc { get; set; }
        public HttpPostedFileBase GoodLifeCycleDoc { get; set; }
        public string GoodLifeCycleDocFileName { get; set; }

       
    }
}
