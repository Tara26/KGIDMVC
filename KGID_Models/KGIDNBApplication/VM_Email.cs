using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGIDNBApplication
{
    public class VM_Email
    {
        public VM_Email()
        {
            FromEmail = "";
            CredPassword = "";
        }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
        public string FromEmail { get; set; }
        public string CredPassword { get; set; }

    }
}
