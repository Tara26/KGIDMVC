using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGID_Master
{
    public class ValidPdfFile
    {
        public HttpPostedFileBase PdfFileDoc { get; set; }
        public string PdfFileDocName { get; set; }
    }
}
