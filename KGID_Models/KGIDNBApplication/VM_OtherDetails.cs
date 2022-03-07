using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGIDNBApplication
{
    public class VM_OtherDetails
    {
        public long EmpCode { get; set; }
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
    }
}
