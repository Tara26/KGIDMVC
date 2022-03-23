using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;
using BLL.MBClaimsBLL;
using KGID_Models.KGIDMotorInsurance;

namespace KGID.Controllers
{
    public class DSCController : Controller
    {
        // GET: DSC
        private readonly IMBClaimsBLL _IMBClaimsBLL;

       // private readonly IMBClaimsBLL _IMBClaimsBLL;

        public DSCController()
        {
            this._IMBClaimsBLL = new MBClaimsBLL();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PDFSign()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PDFSign(string certClient, SelCertAttribs SelCertAttribs)
        {
            //Get Cert Chain
            //IList<X509Certificate> chain = new List<X509Certificate>();
            //X509Chain x509Chain = new X509Chain();

            //var abc = JsonConvert.DeserializeObject(certClient);

            ////X509Certificate2 abc = JsonConvert.DeserializeObject<X509Certificate2>(certClient);

            //byte[] bytes = Encoding.ASCII.GetBytes(certClient);
            //X509Certificate2 x509Certificate21 = new X509Certificate2(bytes);

            //X509Certificate2 x509Certificate2 = new X509Certificate2(bytes);



            ////byte[] bytes1 = Encoding.ASCII.GetBytes(SelCertAttribs.PrivateKey);
            ////x509Certificate2.PrivateKey = new AsymmetricAlgorithm(bytes1);

            //x509Chain.Build(x509Certificate2);

            //foreach (X509ChainElement x509ChainElement in x509Chain.ChainElements)
            //{
            //    chain.Add(DotNetUtilities.FromX509Certificate(x509ChainElement.Certificate));
            //}

            //string filename1 = @"C:\\Venkat\sample.pdf";

            //PdfReader inputPdf = new PdfReader(filename1);

            //FileStream signedPdf = new FileStream(@"C:\\Venkat\Sample_signed.pdf", FileMode.Create);
            //PdfStamper pdfStamper = PdfStamper.CreateSignature(inputPdf, signedPdf, '\0');


            //IExternalSignature externalSignature = new X509Certificate2Signature(x509Certificate2, "SHA-256");

            //PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;

            //signatureAppearance.Reason = "My Signature";
            //signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(0, 00, 200, 100), inputPdf.NumberOfPages, "Signature");
            //signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
            //MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0,
            //    CryptoStandard.CMS);

            //inputPdf.Close();
            //pdfStamper.Close();
            //var abc = PDFSignAsync(SelCertSubject, SDHubConnectionId, "C:\\Venkat\\sample.pdf");
            return View();
        }

        private byte[] StreamFile(string filename)
        {

            byte[] bytes = System.IO.File.ReadAllBytes(filename);
            return bytes; //return the byte data
        }

        //public string GetFileForSigning(RequestFile requestFile)
        //{
        //    Image_convert_model _file_obj = new Image_convert_model();
        //    byte[] binFile = null;
        //    try
        //    {
        //        string RefID = "123";
        //        string AppID = requestFile.RefID;
        //        string EmpID = requestFile.RefType;
        //        //GetDetails.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocdetailBLL(appid);
        //        string filename = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/UnSignedNBBOND.pdf");
        //        string pdfFilePath = filename;
        //        byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);

        //        string strBytes = Convert.ToBase64String(bytes);

        //        _file_obj = new Image_convert_model
        //        {
        //            File_Name = "UnSignedNBBOND.pdf",
        //            File_bytes = strBytes,
        //            File_token = "",
        //            RefID = RefID,
        //            RefType = "1",
        //            DSC_user_name = ""
        //        };

        //        return _file_obj.File_bytes;

        //        ////string filename = "~/UploadedFiles/sample.pdf";
        //        ////BinaryReader binReader = new BinaryReader(System.IO.File.Open(System.Web.Hosting.HostingEnvironment.MapPath(filename), FileMode.Open, FileAccess.Read));
        //        ////binReader.BaseStream.Position = 0;
        //        ////binFile = binReader.ReadBytes(Convert.ToInt32(binReader.BaseStream.Length));
        //        ////binReader.Close();

        //        //System.IO.FileStream _FileStream = new System.IO.FileStream(System.Web.Hosting.HostingEnvironment.MapPath(filename), System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //        //System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(_FileStream);
        //        //long _TotalBytes = new System.IO.FileInfo(System.Web.Hosting.HostingEnvironment.MapPath(filename)).Length;
        //        //binFile = _BinaryReader.ReadBytes((Int32)_TotalBytes);
        //        //_FileStream.Close();
        //        //_FileStream.Dispose();
        //        //_BinaryReader.Close();


        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //        int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
        //        //return Request.CreateResponse(HttpStatusCode.OK, _responce_model, Configuration.Formatters.JsonFormatter);
        //    }
        //    return "";
        //}

        //public string UploadSignedFile(RequestFile requestFile)
        //{
        //    string RefID = "123";
        //    Image_convert_model _Model = new Image_convert_model();
        //    string filename = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/UnSignedNBBOND.pdf");
        //    string pdfFilePath = filename;
        //    byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);

        //    string strBytes = Convert.ToBase64String(bytes);

        //    _Model = new Image_convert_model
        //    {
        //        File_Name = "UnSignedNB.pdf",
        //        File_bytes = strBytes,
        //        File_token = "",
        //        RefID = RefID,
        //        RefType = "1",
        //        DSC_user_name = ""
        //    };


        //    _Model.File_Name = "UnSignedNB.pdf";
        //    File_Responce_model _responce_model = new File_Responce_model();
        //    try
        //    {
        //        _Model.File_Path = Server.MapPath("~/TTDocuments/SignedDocuments/");

        //        if (_Model.File_Name != "" && _Model.File_bytes != "")
        //        {
        //            string serverFileName = GenerateUniqueCode(5);
        //            string filePathSigned = "/TTDocuments/SignedDocuments/" + serverFileName + "_Signed.pdf";
        //            byte[] imageBytes = Convert.FromBase64String(_Model.File_bytes);
        //            string FileName = serverFileName + "_Signed.pdf";
        //            string path = _Model.File_Path;
        //            string imgPath = Path.Combine(path, FileName);
        //            //Check if directory exist
        //            if (!System.IO.Directory.Exists(path))
        //            {
        //                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
        //            }
        //            System.IO.File.WriteAllBytes(imgPath, imageBytes);


        //            _responce_model.Status = true;
        //            _responce_model.Message = "success"; ;
        //            _responce_model.return_reponce = "File Upload successfully.";
        //        }
        //        else
        //        {
        //            _responce_model.Status = false;
        //            _responce_model.Message = "failed"; ;
        //            _responce_model.return_reponce = "File unble to upload.";
        //        }

        //        return "true";
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //        int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
        //        return "false";
        //    }
        //}

        public string GetFileForSigning(RequestFile requestFile)
        {
            GetVehicleChassisPolicyDetails model = new GetVehicleChassisPolicyDetails();

            Image_convert_model _file_obj = new Image_convert_model();
            byte[] binFile = null;
            try
            {
                string AppID = requestFile.RefID;
                long appid = Convert.ToInt64(AppID);
                string AppnID = "";
                //string Doc_ref_ID = requestFile.EmpID1;

                string filename = "";// System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/courtnoticecopy.pdf");

                //string EmpID = requestFile.RefType;
                model.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocfileForSignBLL(appid);
                string path = "";
                string pdfFilePath = "";
                for (int i = 0; i <= model.MVCAppDocDetails.Count-1; i++)
                {
                    path = model.MVCAppDocDetails[i].Accident_details;
                    pdfFilePath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                    AppnID = (model.MVCAppDocDetails[0].MVC_claim_app_id).ToString();

                    //filename = "courtnoticecopy.pdf";

                }
                    //string name = Server.MapPath(path);
                    ////string filename = System.Web.Hosting.HostingEnvironment.MapPath(path);
                    // filename = Path.GetFileName(filename);
                    //string pdfFilePath = filename;
                   
                   filename = Path.GetFileName(path);
                byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);

                    string strBytes = Convert.ToBase64String(bytes);

                    _file_obj = new Image_convert_model
                    {
                        File_Name = filename,
                        File_bytes = strBytes,
                        File_token = "",
                        RefID = AppID,
                        RefType = "1",
                        DSC_user_name = ""
                        
                    };

                //    UploadSignedFile(_file_obj);
                
                return _file_obj.File_bytes;


            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                //return Request.CreateResponse(HttpStatusCode.OK, _responce_model, Configuration.Formatters.JsonFormatter);
            }
            return "";
        }

        public string UploadSignedFile(Image_convert_model _Model)
        {
            GetVehicleChassisPolicyDetails model = new GetVehicleChassisPolicyDetails();
            File_Responce_model _responce_model = new File_Responce_model();
            long docID = Convert.ToInt64(_Model.RefID);
            string AppnID = "";
            model.MVCAppDocDetails = _IMBClaimsBLL.GetMVCDocfileForSignBLL(docID);
           
            for (int i = 0; i <= model.MVCAppDocDetails.Count - 1; i++)
            {
                
                AppnID = (model.MVCAppDocDetails[0].MVC_claim_app_id).ToString();

                //filename = "courtnoticecopy.pdf";

            }

            long appId = Convert.ToInt64(AppnID);

            try
            {
                _Model.File_Path = "/Content//MVC_Claim_files/" + AppnID + "";

                if (_Model.File_Name != "" && _Model.File_bytes != "")
                {
                    string serverFileName = GenerateUniqueCode(5);
                    string filePathSigned = "/TTDocuments/SignedDocuments/" + serverFileName + "_Signed.pdf";
                    byte[] imageBytes = Convert.FromBase64String(_Model.File_bytes);
                    string FileName = serverFileName + "_Signed.pdf";
                    string path = _Model.File_Path;

                    string imgPath = Path.Combine(path, FileName);
                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }
                    System.IO.File.WriteAllBytes(imgPath, imageBytes);


                    _responce_model.Status = true;
                    _responce_model.Message = "success"; ;
                    _responce_model.return_reponce = "File Upload successfully.";
                    string result = string.Empty;
                    //string signedfilepath = Server.MapPath(filePathSigned);
                    string signedfilepath = imgPath;
                    
                    result = _IMBClaimsBLL.MVCSignedDocUploadBLL(docID, appId,signedfilepath);
                }
                else
                {
                    _responce_model.Status = false;
                    _responce_model.Message = "failed"; ;
                    _responce_model.return_reponce = "File unble to upload.";
                }

                return "true";
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                return "false";
            }
        }


        public string GetFileName(GetVehicleChassisPolicyDetails model)
        {
           
            for (int i = 0; i < model.MVCAppDocDetails.Count; i++)
            {
                string path = model.MVCAppDocDetails[i].Accident_details;
                string filename = "";

                if (path.Contains("/Dl_details/"))
                {
                    model.MVCAppDocDetails[0].Accident_dl_details = model.MVCAppDocDetails[i].Accident_details;
                    filename = Path.GetFileName(filename);
                }
                if (path.Contains("/Fir_details/"))
                {
                    model.MVCAppDocDetails[0].Accident_fir_details = model.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/ObjectStatement_details/"))
                {
                    model.MVCAppDocDetails[0].Accident_object_statement_details = model.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Panchanama_details/"))
                {
                    model.MVCAppDocDetails[0].Accident_panchnama_details = model.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Rc_details/"))
                {
                    model.MVCAppDocDetails[0].Accident_dl_details = model.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Summons_details/"))
                {
                    model.MVCAppDocDetails[0].summons_detals = model.MVCAppDocDetails[i].Accident_details;
                }
                if (path.Contains("/Petitioner_details/"))
                {
                    model.MVCAppDocDetails[0].petitioner_details = model.MVCAppDocDetails[i].Accident_details;
                    filename = Path.GetFileName(model.MVCAppDocDetails[i].Accident_details);
                }

            }

            return "filename";
        }

        public string GenerateUniqueCode(int num)
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            characters += alphabets + small_alphabets + numbers;
            string otp = string.Empty;
            for (int i = 0; i < num; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }

        public JsonResult ViewSignedMVCdocs(long id, HttpPostedFileBase file)
        {
            GetVehicleChassisPolicyDetails  model = new GetVehicleChassisPolicyDetails();
            
            model.SignedDocList = _IMBClaimsBLL.GetSignedDocBLL(id);
            

            string fileName = "";
            foreach (var files in model.SignedDocList)
            {
                string PdfFileName = Path.GetFileNameWithoutExtension(files.Accident_details);
                
                files.filename = PdfFileName;
                model.MVC_claim_app_id = files.MVC_claim_app_id;


            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
    public class SelCertAttribs
    {
        public string CertThumbPrint
        {
            get;
            set;
        }

        public string eMail
        {
            get;
            set;
        }

        public DateTime ExpDate
        {
            get;
            set;
        }

        public string PublicKey
        {
            get;
            set;
        }
        public string PrivateKey { get; set; }

        public string SelCertSubject
        {
            get;
            set;
        }

        public DateTime ValidFrom
        {
            get;
            set;
        }
        public object issuerName { get; internal set; }

        public SelCertAttribs()
        {
        }
    }

    public class Image_convert_model
    {
        public string File_Name { get; set; }
        public string File_bytes { get; set; }
        public string File_Path { get; set; }
        public string File_token { get; set; }
        public string RefID { get; set; }
        public string RefType { get; set; }
        public string DSC_user_name { get; set; }

        
    }

    public class RequestFile
    {
        public string RefID { get; set; }
        public string RefType { get; set; }
    }
    public class File_Responce_model
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string return_reponce { get; set; }
    }
}