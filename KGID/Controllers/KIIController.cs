
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
/// <KII_Integration_References>
using com.sun.org.apache.xml.@internal.security.c14n;
using com.sun.org.apache.xml.@internal.security.utils;
using java.io;
using java.security;
using javax.crypto;
using javax.crypto.spec;
using javax.xml.stream;
using javax.xml.stream.events;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Net.Cache;
using System.IO;
using System.Net;
using log4net;
using KGID.Models;
using KGID_Models.NBApplication;
using BLL.NewEmployeeBLL;
using Common;
using System.Web.Configuration;

namespace KGID.Controllers
{
    public class KIIController : Controller
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(KIIController));
        private readonly INBApplicationBll _INBApplicationbll;
        public KIIController()
        {
            this._INBApplicationbll = new NBApplicationBll();
        }
        // GET: KII
        public ActionResult Index()
        {
            return View();
        }
        #region KII
        string deptRefNum = string.Empty;
        long amount = 0;
        string transactiondate = string.Empty;
        string chlntransactiondate = string.Empty;
        [HttpPost]
        public void IndexPost(VM_PaymentDetails objPaymentDetails)//KIIRequest request
        {
            Session.Timeout = Session.Timeout + 15;
            Logger.LogMessage(TracingLevel.INFO, "IndexPost-Enter to method");
            Session["KIIReturn"] = "NBPay";
            //Logger.LogMessage(TracingLevel.INFO, "Create Cookie Start:" );
            //HttpCookie k2return = new HttpCookie("k2return");
            //k2return.Value="NBPay";
            ////k2return.Expires = DateTime.Now.AddHours(1);
            //Response.SetCookie(k2return);
            //Response.Flush();
            //string Emp = Session["UID"].ToString();
            //HttpCookie empid = new HttpCookie("empid");
            //empid.Value = Emp;
            ////empid.Expires = DateTime.Now.AddHours(1);
            //Response.SetCookie(empid);
            //Response.Flush();
            //Logger.LogMessage(TracingLevel.INFO, "Create Cookie End:");
            // Get Payment Details
            VM_PaymentDetails objPD = _INBApplicationbll.NBPaymentDll(Convert.ToInt64(Session["UID"]));
            objPaymentDetails.cd_amount = objPD.cd_amount;
            //
            long result = _INBApplicationbll.SaveNBPaymentBll(objPaymentDetails);
            //long result = 0;
            if (result == 1)
            {
                string dd = DateTime.Now.ToString("dd");
                string MM = DateTime.Now.ToString("MM");
                string yy = DateTime.Now.ToString("yy");
                string ddHHmmss = DateTime.Now.ToString("ddHHmmss");
                transactiondate = DateTime.Now.ToString("ddMMyyyy");
                chlntransactiondate = DateTime.Now.ToString("dd/MM/yyyy").Replace('-', '/');

                //deptRefNum = "KD" + MM + yy + "8011" + ddHHmmss;
                //string testdataenc = "VhrkkSQ5YXM%2BZJ49L439AKCEJZzsN5PNVD1WMGXbOu0XTy60BJSDp8LJDWviKwGTPX0we7NfLgxa%0D%0AN7iabx7f837vvVNeNjp4dfUJTBLRN5wAxuqTLDbXRylnuM%2F2e8hZiGjY4LDeafJ53cab7dai6XIf%0D%0Axcp5gxMg1TmYN4DmpadwzNCOsKF8W8g8A7FUeW05%2F3w35rFXH1XmmWW45AVevd8Y3dDikSZlX1%2BG%0D%0AQpm9ZE%2BGf4gbQgmxP4CKObQ7W6epxpazPTxnTD30FKeMOiRKfAY9ByYLgG48QKRIrBVHZcGvZq58%0D%0A0y7MM5ZEW3rB5EMg";
                //var dec = HttpUtility.UrlDecode(testdataenc);
                //dec.Replace(" ", "");
                //var resdecData = SymmetricDecrypt(dec, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                //string SymmetricDecryptData11 = SymmetricDecrypt(testdataenc, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                //objPaymentDetails.cd_amount = 1;
                //objPaymentDetails.cd_challan_ref_no = deptRefNum;
                amount = Convert.ToInt64(objPaymentDetails.cd_amount);
                deptRefNum = objPaymentDetails.cd_challan_ref_no;
                Log.Debug(deptRefNum);
                Log.Debug(amount);
                ///////KII Integration Start//////
                ByteArrayOutputStream fileWriter = null;
                StringBuilder content = null;
                string currPath = string.Empty;
                string SignedresultContent = string.Empty;
                string KIIsignresponse = string.Empty;
                string resFile = TextFileCreate(Convert.ToInt64(amount), deptRefNum, objPaymentDetails.EmpName, objPaymentDetails.hoa, objPaymentDetails.ddo_code, objPaymentDetails.cd_purpose_id, objPaymentDetails.sub_purpose_desc);
                XMLInputFactory factory = XMLInputFactory.newInstance();
                //File fileLoc = new File(filePath);
                FileReader fileReader = new FileReader(resFile);
                //XMLStreamReader reader = factory.createXMLStreamReader(fileReader);
                //content = new StringBuilder();
                // Parsing XML using stream reader and writing to a ByteArrayOutputStream
                string AsBase64String = string.Empty;
                byte[] AsBytes = System.IO.File.ReadAllBytes(resFile);
                fileReader = new FileReader(resFile);
                XMLStreamReader reader = factory.createXMLStreamReader(fileReader);
                content = new StringBuilder();
                // Parsing XML using stream reader and writing to a ByteArrayOutputStream
                while (reader.hasNext())
                {
                    int eventType = reader.next();
                    switch (eventType)
                    {

                    case XMLEvent.START_ELEMENT:

                        currPath = currPath + "/" + reader.getLocalName();
                        //Instead
                        if (currPath.Contains("data"))
                        {
                            String startTag = "";
                            //Instead
                            if (reader.getLocalName().Equals("data"))
                            {
                                fileWriter = new ByteArrayOutputStream();
                                startTag = "<" + reader.getLocalName();
                                for (int k = 0; k < reader.getNamespaceCount(); k++)
                                {
                                    if (reader.getNamespacePrefix(k) != null)
                                        startTag = startTag + " xmlns:" + reader.getNamespacePrefix(k) + "=\"" + reader.getNamespaceURI(k) + "\"";
                                    else
                                        startTag = startTag + " xmlns=\"" + reader.getNamespaceURI(k) + "\"";
                                }
                                startTag = startTag + ">";
                            }
                            else
                            {
                                startTag = "<" + reader.getLocalName() + ">";
                            }

                            if (fileWriter != null)
                                fileWriter.write(Encoding.ASCII.GetBytes(startTag));
                        }
                        break;

                    case XMLStreamConstants.CHARACTERS:
                        if (fileWriter != null)
                        {
                            fileWriter.write(Encoding.ASCII.GetBytes(reader.getText()));
                        }
                        break;

                    case XMLStreamConstants.END_ELEMENT:
                        //Instead
                        if (currPath.Contains("data"))
                        {
                            string endTag = "</" + reader.getLocalName() + ">";

                            if (fileWriter != null)
                            {
                                fileWriter.write(Encoding.ASCII.GetBytes(endTag));
                            }
                        }
                        content = new StringBuilder();
                        //RemoveLasttag(currPath);
                        currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                        break;

                    case XMLEvent.CDATA:
                        break;
                    case XMLEvent.SPACE:
                        break;

                }
            }
            com.sun.org.apache.xml.@internal.security.Init.init();
            Canonicalizer canon = Canonicalizer.getInstance(Canonicalizer.ALGO_ID_C14N_OMIT_COMMENTS);
            byte[] canonXmlBytes = canon.canonicalize(fileWriter.toByteArray());
            string beforesignedData = Convert.ToBase64String(canonXmlBytes);
            string beforecanonXmlData = Encoding.UTF8.GetString(AsBytes);
            string aftercanonXmlData = Encoding.UTF8.GetString(canonXmlBytes);

            //SIGN DATA WITH PFX FILE
            string xml_inBase64 = Convert.ToBase64String(AsBytes);
            string em = Encoding.UTF8.GetString(canonXmlBytes);

            Logger.LogMessage(TracingLevel.INFO, "10.10.31.231:8080/SignXmlData-Before convertion");
            //WebAPI Service Call
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://10.10.31.231:8080/SignXmlData/");

                    object reqdata = new
                    {
                        data = beforesignedData
                    };
                    var myContent = JsonConvert.SerializeObject(reqdata);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    /////////
                    var resultsac = client.PostAsync("getSignforDataByte", byteContent).Result;
                    SignedresultContent = resultsac.Content.ReadAsStringAsync().Result;
                    client.CancelPendingRequests();
                    //Console.WriteLine("about to dispose the client");
                    client.Dispose();
                    //return resultContent;
                    Log.Debug(SignedresultContent);
                }
                Logger.LogMessage(TracingLevel.INFO, "10.10.31.231:8080/SignXmlData-After convertion");
                ////////
                try
                {
                    if (SignedresultContent != null)
                    {
                        string data = GetKIISignDetails(SignedresultContent, Encoding.UTF8.GetString(AsBytes));
                        Logger.LogMessage(TracingLevel.INFO, "GetKIISignDetails Response: " + data);
                        SignedDataResponseKII signedDataResponseK2 = new SignedDataResponseKII();
                        signedDataResponseK2 = JsonConvert.DeserializeObject<SignedDataResponseKII>(data);
                        Logger.LogMessage(TracingLevel.INFO, "GetKIISignDetails Response: " + "StatusCode:" + signedDataResponseK2.statusCode + "&" + "statusDescription:" + signedDataResponseK2.statusDescription);
                        //Log.Debug(data);
                        if (signedDataResponseK2.statusCode == "KII-RCTER-00" && signedDataResponseK2.statusDescription == "Success")
                        {
                            //string transactiondate = DateTime.Now.ToString("ddMMyyyy");
                            string HashChechsumMD5 = "dept_ref_no=" + deptRefNum + "|txn_date=" + transactiondate + "|amount=" + amount + "|dept_pwd=1234";
                            string AfterHashChechsumMD5 = GetMD5Checksum(HashChechsumMD5);
                            string BeforeEncryptedStringData = "dept_ref_no=" + deptRefNum + "|txn_date=" + transactiondate + "|amount=" + amount + "|dept_pwd=1234|checkSum=" + AfterHashChechsumMD5 + "";
                            string EncryptedStringData = SymetricEncrypt(BeforeEncryptedStringData, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                            //Log.Debug("Encrypt Data After Signed Data Success");
                            //Log.Debug(EncryptedStringData);
                            Logger.LogMessage(TracingLevel.INFO, "Encrypt Data After Signed Data Success: " + EncryptedStringData);
                            string SymmetricDecryptData = SymmetricDecrypt(EncryptedStringData, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                            //Log.Debug("Test Decrypt Data");
                            //Log.Debug(SymmetricDecryptData);
                            Logger.LogMessage(TracingLevel.INFO, "Test Decrypt Data: " + EncryptedStringData);
                            string KIIurl = "https://k2.karnataka.gov.in/wps/portal/Home/DepartmentPayment/?uri=receiptsample:com.tcs.departmentpage:departmentportlet";
                            string redirect_url = "" + KIIurl + "" + "&encdata=" + EncryptedStringData + "&dept_code=12C";
                            //Log.Debug("Redirect url here");
                            //Log.Debug(redirect_url);
                            Logger.LogMessage(TracingLevel.INFO, "Redirect url here: " + redirect_url);
                            RemotePost myremotepost = new RemotePost();
                            myremotepost.Url = redirect_url;
                            myremotepost.Add("surl", "https://kgidonline.karnataka.gov.in/Home/Return");//Change the success url here depending upon the port number of your local system.  
                            myremotepost.Add("furl", "https://kgidonline.karnataka.gov.in/Home/Return");//Change the failure url here depending upon the port number of your local system.  
                            myremotepost.Post();
                            //Log.Debug("Redirect Done");
                            Logger.LogMessage(TracingLevel.INFO, "Redirect Done");
                            //string finalresponse = GrtUrl(KIIurl, EncryptedStringData, "12C");
                            //return redirect_url;
                        }
                        else
                        {
                            //Log.Error("redirect error");
                            Logger.LogMessage(TracingLevel.INFO, "redirect error");
                            //return null;
                            //Unable to signed the data
                        }
                    }
                    else
                    {
                        Logger.LogMessage(TracingLevel.INFO, "signed content error");
                        // Log.Error("signed content error");
                        //Signed Data Not Captured
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "Line catch" + ex.Message);
                    //Log.Error("Error Level", ex);
                }
                ///////KII Integration End//////
            }
            else
            {
                RemotePostFalse myremotepostfalse = new RemotePostFalse();
                myremotepostfalse.Url = "https://kgidonline.karnataka.gov.in/kgid-app?pay=false";
                myremotepostfalse.Post();
            }
        }
        public string TextFileCreate(long ChallanAmount, string Refno,string rmtrName,string prpsName,string ddoCode,int deptPrpsId,string subPrpsName)
        {
            Logger.LogMessage(TracingLevel.INFO, "TextFileCreate()");
            // KD0221801112345678
            //string dd = DateTime.Now.ToString("dd");
            //string MM = DateTime.Now.ToString("MM");
            //string yy = DateTime.Now.ToString("yy");
            //string ddHHmmss = DateTime.Now.ToString("ddHHmmss");
            //deptRefNum = "KD" + MM + yy + "8011" + ddHHmmss;
            //var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            //string newFile = Server.MapPath("~/Documents/KIIRequest/" + deptRefNum + ".txt");
            //string newFile = @"F:/Documents/KIIRequest/" + deptRefNum + ".txt";
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"KIIRequest\" + deptRefNum + ".txt";
            }
            //string fileName = @"D:\VenkatTXFITx.txt";
            System.IO.FileInfo fi = new System.IO.FileInfo(newFile);
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (fi.Exists)
                {
                    fi.Delete();
                }

                // Create a new file     
                using (System.IO.StreamWriter strmwrtr = fi.CreateText())
                {
                    //strmwrtr.WriteLine("New file created: {0}", DateTime.Now.ToString());
                    //strmwrtr.WriteLine("Author: Venkatesh);
                    strmwrtr.WriteLine("<data>");
                    strmwrtr.WriteLine("<RctReceiveValidateChlnRq>");
                    strmwrtr.WriteLine("      <chlnDate>"+chlntransactiondate+"</chlnDate>");
                    strmwrtr.WriteLine("      <deptCode>12C</deptCode>");
                    strmwrtr.WriteLine("      <ddoCode>"+ ddoCode + "</ddoCode>");
                    strmwrtr.WriteLine("      <deptRefNum>" + deptRefNum + "</deptRefNum>");
                    strmwrtr.WriteLine("      <rctReceiveValidateChlnDtls>");
                    strmwrtr.WriteLine("        <amount>" + ChallanAmount + "</amount>");
                    strmwrtr.WriteLine("        <deptPrpsId>"+ deptPrpsId + "</deptPrpsId>");
                    strmwrtr.WriteLine("        <prpsName>"+ prpsName + "</prpsName>");//0230~00~104~0~00~000
                    strmwrtr.WriteLine("        <subPrpsName>001</subPrpsName>");//FACT REN 03   "+ subPrpsName + "
                    strmwrtr.WriteLine("        <subDeptRefNum>" + deptRefNum + "</subDeptRefNum>");
                    strmwrtr.WriteLine("      </rctReceiveValidateChlnDtls>");
                    strmwrtr.WriteLine("      <rmtrName>"+ rmtrName + "</rmtrName>");
                    strmwrtr.WriteLine("      <totalAmount>" + ChallanAmount + "</totalAmount>");
                    strmwrtr.WriteLine("      <trsryCode>572A</trsryCode>");
                    strmwrtr.WriteLine("</RctReceiveValidateChlnRq>");
                    strmwrtr.WriteLine("</data>");
                }

                //Write file contents on console.
                //using (StreamReader sr = File.OpenText(fileName))
                //{
                //    string s = "";
                //    while ((s = sr.ReadLine()) != null)
                //    {
                //        //Console.WriteLine(s);
                //    }
                //}
                return newFile;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        public string GetKIISignDetails(string signeddata, string xmldata)
        {
            try
            {
                Logger.LogMessage(TracingLevel.INFO, "GetKIISignDetails: SIGNED DATA-" + signeddata + ",xmldata-"+ xmldata);

                string URL = "https://khajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/RctReceiveValidateChlnService?wsdl";

                string xml2 = @"<?xml version='1.0' encoding='utf-8'?>"
                    +
                    "<soapenv:Envelope xmlns:soapenv=" + "'" + "http://schemas.xmlsoap.org/soap/envelope/" + "'" + " xmlns:ser=" + "'" + "http://service.receivevalidatechallan.dept.rct.integration.ifms.gov.in/" + "'" + " xmlns:head=" + "'" + "http://header.ei.integration.ifms.gov.in/" + "'" + ">"
                    + "\n"
                    + "   <soapenv:Header>"
                    + "\n"
                    + "      <ser:Header>"
                    + "\n"
                    + "         <head:agencyCode>EA_KID</head:agencyCode>"
                     + "\n"
                    + "         <head:integrationCode>RCT033</head:integrationCode>"
                     + "\n"
                    + "         <head:uirNo>EA_KID-RCT033-"+ transactiondate + "-" + deptRefNum + "</head:uirNo>"
                     + "\n"
                    + "      </ser:Header>"
                     + "\n"
                    + "   </soapenv:Header>"
                     + "\n"
                    + "   <soapenv:Body>"
                     + "\n"
                    + "      <ser:envelopedDataReq>"
                     + "\n"
                    + "         <Signature>" + signeddata + "</Signature>"
                    + "\n"
                    + xmldata
                    + "      </ser:envelopedDataReq>"
                     + "\n"
                    + "   </soapenv:Body>"
                     + "\n"
                    + "</soapenv:Envelope>";

                /////////////

                //////////////////////



                string responseStr = string.Empty;
                string jsonText = string.Empty;
                //var _url = "https://preprodkhajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/";
                //var _action = "RctReceiveValidateChlnService?wsdl";

                //XmlDocument soapEnvelopeXml = CreateSoapEnvelope(signeddata, xml2);
                // WebRequest.DefaultCachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                //Log.Debug("Request for Signature Validation");

                Logger.LogMessage(TracingLevel.INFO, "Request for Signature Validation");
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(URL);
                byte[] bytes;
                bytes = Encoding.UTF8.GetBytes(xml2);
                request.ContentType = "text/xml";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                //request.SendChunked = false;
                // request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0, no-cache, no-store");
                //request.KeepAlive = true;
                //request.AllowWriteStreamBuffering = false;
                //request.ServicePoint.ConnectionLimit = 10;    // The default value of 2 within a ConnectionGroup caused me always a "Timeout exception" because a user's 1-3 concurrent WebRequests within a second.
                //request.ServicePoint.MaxIdleTime = 5 * 1000;  // (5 sec) default was 100000 (100 sec).  Max idle time for a connection within a ConnectionGroup for reuse before closing

                //Log.Debug("FileStream Rquesting");
                Logger.LogMessage(TracingLevel.INFO, "FileStream Rquesting");
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                //Log.Debug("FileStream Rquest Done");
                Logger.LogMessage(TracingLevel.INFO, "FileStream Rquest Done");
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                //Log.Debug("Response from KII Signing:  " + response.StatusCode);
                Logger.LogMessage(TracingLevel.INFO, "Response from KII Signing: Status Code" + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    responseStr = new StreamReader(responseStream).ReadToEnd();
                    XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(responseStr));
                    var xmlWithoutNs = xmlDocumentWithoutNs.ToString();
                    Log.Debug("Response Data:  " + xmlWithoutNs);
                    Logger.LogMessage(TracingLevel.INFO, "Response from KII Signing Response Data: " + xmlWithoutNs);
                    //return Content(responseStr);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlWithoutNs);
                    //XmlNode node = doc.SelectSingleNode("Basic_vehicle_detailsResult");
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);

                    // To convert JSON text contained in string json into an XML node
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
                    ViewBag.Response = responseStr;
                    ViewBag.Response1 = doc.InnerText;
                    jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                    //jsonText = JsonConvert.SerializeXmlNode(doc);
                    Log.Debug("After Json Convervation Data: " + jsonText);
                }

                //VM_MotorInsuranceVehicleDetails obj = new VM_MotorInsuranceVehicleDetails();
                //dynamic result = JsonConvert.DeserializeObject(ViewBag.Response);

                //obj = _IMotorInsuranceVehicleDetailsBll.BindVahanResponseDetailstoModel(result);

                return jsonText;
            }
            catch (Exception ex)
            {
                //Log.Error("Signing Data Error Level", ex);
                Logger.LogMessage(TracingLevel.INFO, "Signing Data Error Level" + ex.Message);
                return null;
            }
        }

        public class SignedDataResponseKII
        {
            public string deptRefNum { get; set; }
            public string totalAmount { get; set; }
            public string statusCode { get; set; }
            public string statusDescription { get; set; }
        }
        //Implemented based on requirement--Added by Venkatesh--
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
        //Remote post--Added by Venkatesh
        public class RemotePost
        {
            //Remote post added by--Venkatesh
            private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();


            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            public void Post()
            {
                System.Web.HttpContext.Current.Response.Clear();

                System.Web.HttpContext.Current.Response.Write("<html><head>");

                System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
                System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");

                System.Web.HttpContext.Current.Response.End();
            }
        }
        public class RemotePostFalse
        {
            //Remote post added by--Venkatesh
            private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();


            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            public void Post()
            {
                System.Web.HttpContext.Current.Response.Clear();

                System.Web.HttpContext.Current.Response.Write("<html><head>");

                System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
                System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");

                System.Web.HttpContext.Current.Response.End();
            }
        }

        public static string GetMD5Checksum(string filename)
        {
            byte[] b = CreateChecksum(filename);
            string result = "";
            for (int i = 0; i < b.Length; i++)
            {
                result += java.lang.Integer.toString((b[i] & 0xff) + 0x100, 16).Substring(1);
            }
            return result;
        }
        public static byte[] CreateChecksum(string filename)
        {
            InputStream fis = new ByteArrayInputStream(System.Text.Encoding.UTF8.GetBytes(filename));
            byte[] buffer = new byte[1024];
            MessageDigest complete = MessageDigest.getInstance("MD5");
            int numRead;
            do
            {
                numRead = fis.read(buffer);
                if (numRead > 0)
                {
                    complete.update(buffer, 0, numRead);
                }
            }
            while (numRead != -1);
            fis.close();
            return complete.digest();
        }

        public static string SymetricEncrypt(string text, string secretkey)
        {
            Log.Debug("Request SymetricEncryptData: " + text);
            byte[] raw;
            string encryptedString;
            SecretKeySpec skeyspec;

            Cipher cipher;
            try
            {
                byte[] encryptText = Encoding.UTF8.GetBytes(text);
                //byte[] encryptText = text.getBytes("UTF-8");
                //FileInputStream fileinputstream = new FileInputStream("D://KII//KGID_KHAJANE.key");
                //byte[] abyte = new byte[fileinputstream.available()];
                byte[] abyte = Encoding.UTF8.GetBytes(secretkey);
                //fileinputstream.read(abyte);
                //fileinputstream.close();

                byte[] keyBytes = new byte[16];
                int len = abyte.Length;
                if (len > keyBytes.Length) len = keyBytes.Length;
                //System.arraycopy(abyte, 0, keyBytes, 0, len);
                Array.Copy(abyte, 0, keyBytes, 0, len);
                raw = Base64.decode(secretkey);
                skeyspec = new SecretKeySpec(keyBytes, "AES");
                IvParameterSpec ivSpec = new IvParameterSpec(keyBytes);
                cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
                cipher.init(Cipher.ENCRYPT_MODE, skeyspec, ivSpec);
                //byte[] results = cipher.doFinal(encryptText);
                //string beforesignedData = Convert.ToBase64String(results);
                encryptedString = Base64.encode(cipher.doFinal(encryptText));
                //encryptedString = Convert.ToBase64String(cipher.doFinal(encryptText));
                return encryptedString;
            }
            catch (Exception ex)
            {
                Log.Error("SymetricEncrypt Error Level", ex);
                return null;
            }
        }
        public static string SymmetricDecrypt(string text, string secretkey)
        {
            Cipher cipher;
            string encryptedString;
            byte[] encryptText = null;
            byte[] raw;
            SecretKeySpec skeySpec;
            try
            {
                //FileInputStream fileinputstream = new FileInputStream("D://KII//KGID_KHAJANE.key");
                //byte[] abyte = new byte[fileinputstream.available()];
                byte[] abyte = Encoding.UTF8.GetBytes(secretkey);
                //fileinputstream.read(abyte);
                //fileinputstream.close();
                byte[] keyBytes = new byte[16];
                int len = abyte.Length;
                if (len > keyBytes.Length) len = keyBytes.Length;
                //System.arraycopy(abyte, 0, keyBytes, 0, len);
                Array.Copy(abyte, 0, keyBytes, 0, len);
                //raw = Base64.decode(secretkey);
                skeySpec = new SecretKeySpec(keyBytes, "AES");
                //encryptText = System.Text.Encoding.UTF8.GetBytes(text);
                //encryptText = Base64.decode(text);
                encryptText = Convert.FromBase64String(text);

                IvParameterSpec ivSpec = new IvParameterSpec(keyBytes);
                cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
                cipher.init(Cipher.DECRYPT_MODE, skeySpec, ivSpec);
                //encryptedString = new String(cipher.doFinal(encryptText));
                //encryptedString = Convert.ToBase64String(cipher.doFinal(encryptText));
                encryptedString = Encoding.UTF8.GetString(cipher.doFinal(encryptText));
                return encryptedString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //[HttpGet]
        public ActionResult Return(string encdata, string dept_code, string urlId, string connect)
        {
            Log.Debug("After Payment Data: " + encdata);
            //ViewBag.Message = form.Keys;
            //ViewBag.Test = dept_code;
            //UrlDecode
            //string dec = HttpUtility.UrlDecode(encdata);
            string dec = encdata;
            Log.Debug("URL Decode Data: " + dec);
            dec.Replace(" ", "");
            string resdecData = SymmetricDecrypt(dec, "EdZUiBM0d8C46PEZ2Yn9Gg==");
            string[] resdecDataList = resdecData.Split(new Char[] { '|', '=' });
            string BankTransactionNo = resdecDataList[1];
            string ChallanAmount = resdecDataList[3];
            string ChallanRefNo = resdecDataList[5];
            string Status = resdecDataList[7];
            string BankName = resdecDataList[9];
            string PaymentMode = resdecDataList[11];
            string TransactionTimeStamp = resdecDataList[13];
            string CheckSum = resdecDataList[15];

            KIIPaymentResponse PaymentResponseData = new KIIPaymentResponse();
            PaymentResponseData.BankTransactionNo = BankTransactionNo;
            PaymentResponseData.ChallanAmount = ChallanAmount;
            PaymentResponseData.ChallanRefNo = ChallanRefNo;
            PaymentResponseData.Status = Status;
            PaymentResponseData.BankName = BankName;
            PaymentResponseData.PaymentMode = PaymentMode;
            PaymentResponseData.TransactionTimeStamp = TransactionTimeStamp;
            PaymentResponseData.CheckSum = CheckSum;

            Log.Debug("After Payment Redirected Decrypt Data: " + resdecData);
            ViewBag.Message = encdata;
            ViewBag.Test = resdecData;
            //ViewBag.Test = "test";
            return View(PaymentResponseData);
        }
        //[HttpPost]
        //public void Return(string encdata,string dept_code,string urlId,string connect)
        //{
        //    ViewBag.Message = encdata;
        //    //ViewBag.Message = form.Keys;
        //    ViewBag.Test = dept_code;
        //}
        #endregion
    }
}