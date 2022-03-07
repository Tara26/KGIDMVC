using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGID_Upload
{
    public class ExcelValidation
    {
        public string column { get; set; }
        public string validation { get; set; }
        public string errorMsg { get; set; }
    }

    public class Validation
    {
        public List<ExcelValidation> ValidationData { get; set; }
    }
    
    public class UploadModel
    {
        public HttpPostedFileBase FileUpload { get; set; }
        public string fileName { get; set; }
        public int error { get; set; }
        public string rowNum { get; set; }
        public string errorDesc { get; set; }
    }

    public class ValidationModel
    {
        public List<long> lstMblNbr { get; set; }
        public List<string> lstPanNbr { get; set; }
    }
}
