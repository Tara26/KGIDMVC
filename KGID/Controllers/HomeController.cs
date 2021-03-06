using BLL.DashboardBLL;
using Common;
using KGID_Models.Dashboard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static KGID.FilterConfig;
using javax.crypto.spec;
using javax.crypto;
using BLL.NewEmployeeBLL;
using KGID_Models.NBApplication;
using System.Net;
using KGID_Models.KGID_Master;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml;
using System.Web.Routing;
using KGID_Models.KGID_VerifyData;
using DLL.DBConnection;
using System.Net.Http;
using System.Net.Http.Headers;
using com.sun.org.apache.xml.@internal.security.c14n;
using javax.xml.stream;
using java.io;
using javax.xml.stream.events;
using Newtonsoft.Json.Linq;
using KGID.Models;

namespace KGID.Controllers
{
    [NoCache]
    public class HomeController : Controller
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private IDashboardBLL dashboardBLL;
        private readonly INBApplicationBll _INBApplicationbll;
        public HomeController()
        {
            dashboardBLL = new DashboardBLL();
            this._INBApplicationbll = new NBApplicationBll();
        }

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BrowserError()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Route("kgid-umv")]
        public ActionResult UserManualVideos()
        {

            return View();
        }
        [Route("kgid-pd")]
        public ActionResult GetDetailsBasedOnKGIDNo()
        {
            var aaplyNewFlag = true;
            VM_ListOfPolicyDetails obj = new VM_ListOfPolicyDetails();
            if (Session["FirstKGIDNo"] != null)
            {
                obj.listDashboardData = _INBApplicationbll.GetDetailsBasedOnKGIDNo(Convert.ToInt64(Session["FirstKGIDNo"]));
                if (obj.listDashboardData.Count > 0)
                {
                    foreach (var item in obj.listDashboardData)
                    {
                        if (item.status != "Verified")
                        {
                            aaplyNewFlag = false;
                            break;
                        }
                    }
                }
                if (aaplyNewFlag == false)
                {
                    Session["ApplyNewFlag"] = aaplyNewFlag;
                }
            }
            else
            {
                Session["ApplyNewFlag"] = true;
            }


            // return Json(listDashboardData, JsonRequestBehavior.AllowGet);
            return View(obj);
        }

        [Route("kgid-home")]
        //[CustomAuthorize("Employee")]
        public ActionResult Dashboard()
        {
            long userId = Convert.ToInt64(Session["UID"]);
            VM_Dashboard dashboard = new VM_Dashboard();
            List<VM_EmpDashboardData> objEmpData = new List<VM_EmpDashboardData>();
            objEmpData = dashboardBLL.GetDashboardInsuredEmpData(userId);
            dashboard.listDashboardData = objEmpData;
            return View(dashboard);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", string.Empty));
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            return RedirectToAction("Index", "Login");
        }

        #region Session
        public ActionResult Keepalive()
        {
            Session.Timeout = Session.Timeout + 10;
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxClick()
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        #endregion
        //Added by Venkatesh
        #region View File 

        [CustomAuthorize("Employee", "DDO", "Caseworker", "Superintendent", "DIO", "Deputy Director", "Director", "Admin", "Super Admin", "AVG Caseworker", "Agency")]
        public ActionResult ViewFilePath(string FilePath)
        {
            //string path = Server.MapPath(FilePath);
            var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(FilePath));
            return new FileStreamResult(memoryStream, "application/pdf");
        }
        public ActionResult ViewFilePath1(string FilePath)
        {
            string path = Server.MapPath(FilePath);
            var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(path));
            return base.File(path, "image/png");
        }
        public ActionResult FileDownload(string FilePath)
        {
            string path = Server.MapPath(FilePath);
            var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(path));
            if (FilePath.Contains(".pdf"))
            {
                return new FileStreamResult(memoryStream, "application/pdf");
            }
            else
            {
                return new FileStreamResult(memoryStream, "application/zip");
            }

        }
        #endregion

        #region KII Return Method
        public void Return(string encdata, string dept_code, string urlId, string connect)
        {
            //encdata = "VhrkkSQ5YXM%2BZJ49L439AKHwoDFvcWL%2FokKk%2FlK83Eu1k2jJymStl2t%2FjWf7ZEDsQLH5B9LREgfC%0D%0A%2BhEOJNysLMx1RxxwRUoP1GvbIHhDg%2FS6z3XEeEPRtTziy3QVHxNV7HMhV%2FpgjJrwW2neteGxnGAD%0D%0AWgzoJnFPeDwYADWFAeC18AARNogWt5DMMdkCgR%2BF8c6PAjO9yGMLqbLqbmPgOUKFaL9g6R9s9WP1%0D%0AW%2BdF0vPN6F9DBBlccG88gsOHN2OtyoSG1R%2BdIAaufRr9oL6Bn2TOaP9jF0IKOpMpgd0A1jFdKber%0D%0AAUnQtXfeL5qzCMsb";
            Logger.LogMessage(TracingLevel.INFO, "KII Return Enter");
            Logger.LogMessage(TracingLevel.INFO, "KII Return encdata: "+ encdata);
            //string testdataenc = "VhrkkSQ5YXM%2BZJ49L439AKCEJZzsN5PNVD1WMGXbOu0XTy60BJSDp8LJDWviKwGTPX0we7NfLgxa%0D%0AN7iabx7f837vvVNeNjp4dfUJTBLRN5wAxuqTLDbXRylnuM%2F2e8hZiGjY4LDeafJ53cab7dai6XIf%0D%0Axcp5gxMg1TmYN4DmpadwzNCOsKF8W8g8A7FUeW05%2F3w35rFXH1XmmWW45AVevd8Y3dDikSZlX1%2BG%0D%0AQpm9ZE%2BGf4gbQgmxP4CKObQ7W6epxpazPTxnTD30FKeMOiRKfAY9ByYLgG48QKRIrBVHZcGvZq58%0D%0A0y7MM5ZEW3rB5EMg";
            //var dec = HttpUtility.UrlDecode(encdata);
            //dec.Replace(" ", "+");
            //Logger.LogMessage(TracingLevel.INFO, "Dec: " + dec);
            var resdecData = SymmetricDecrypt(encdata, "EdZUiBM0d8C46PEZ2Yn9Gg==");
            Logger.LogMessage(TracingLevel.INFO, "KII Return resdecData: " + resdecData);
            VM_PaymentDetails objPaymentDetails = GetTranDataFromString(resdecData);
            Logger.LogMessage(TracingLevel.INFO, "objPaymentDetails :" + objPaymentDetails);
            Logger.LogMessage(TracingLevel.INFO, "KII_Home_Return_UID_Session :" + Convert.ToInt64(Session["UID"]));
            long result = _INBApplicationbll.SaveNBChallanStatusDll(objPaymentDetails);
            Logger.LogMessage(TracingLevel.INFO, "KIIReturn DB Save:" + result);
            //
            //HttpCookie cookieKIIReturn = HttpContext.Request.Cookies.Get("k2return");
            //HttpCookie cookieUID = HttpContext.Request.Cookies.Get("empid");
            //string KIIReturn = cookieKIIReturn.Value;
            //string UID = cookieUID.Value;
            //Logger.LogMessage(TracingLevel.INFO, "KIIReturn Cookie:" + KIIReturn);
            //Logger.LogMessage(TracingLevel.INFO, "UID Cookie:" + UID);
            //if (KIIReturn == "NBPay")
            //{
            //    Response.Redirect(Url.Action("ApplicationForm", "Employee", new { empId = UID, pay = "true" }));
            //}
            //
            string K2ReturnSession = Session["KIIReturn"].ToString();
            Logger.LogMessage(TracingLevel.INFO, "KIIReturn Session:" + K2ReturnSession);
            if (Session["KIIReturn"].ToString() == "NBPay")
            {
                Logger.LogMessage(TracingLevel.INFO, "NBPay Enter");
                Logger.LogMessage(TracingLevel.INFO, "UID Session:" + Session["UID"].ToString());
                //return RedirectToAction("/kgid-app?empId=" + Session["UID"] + "&pay=true", "Employee");
                //return RedirectToAction("kgid-app", new { empId = Session["UID"], pay = "true" });
                Response.Redirect(Url.Action("ApplicationForm", "Employee", new { empId = Session["UID"], pay = "true" }));
            }
            else
            {
                Logger.LogMessage(TracingLevel.INFO, "NBPay Enter");
                //return RedirectToAction("/kgid-app?empId=" + Session["UID"] + "&pay=true", "Employee");
                Response.Redirect(Url.Action("ApplicationForm", "Employee", new { empId = Session["UID"], pay = "true" }));       
                // Response.Redirect(Url.Action("kgid-app", new { empId = Session["UID"], pay = "true" }));
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

        public VM_PaymentDetails GetTranDataFromString(string resdecData)
        {
            string[] dataArr = resdecData.Split('|');
            VM_PaymentDetails obj = new VM_PaymentDetails();
            if (Session["UID"] != null)
            {
                obj.EmpID = Convert.ToInt64(Session["UID"]);
            }
            foreach (var a in dataArr)
            {
                if (a.Split('=')[0] == "Bank_transaction_no")
                    obj.cs_transaction_ref_no = (a.Split('=') != null) ? a.Split('=')[1].ToString() : "";
                if (a.Split('=')[0] == "challan_amount")
                    obj.cs_amount = (a.Split('=') != null) ? Convert.ToInt32(a.Split('=')[1]) : 0;
                if (a.Split('=')[0] == "challan_ref_no")
                    obj.cd_challan_ref_no = (a.Split('=') != null) ? a.Split('=')[1].ToString() : "";
                if (a.Split('=')[0] == "Status")
                    obj.cs_status = (a.Split('=') != null) ? Convert.ToInt64(a.Split('=')[1]) : 0;
                if (a.Split('=')[0] == "trsn_timestamp")
                    obj.cs_transsaction_date = (a.Split('=') != null) ? a.Split('=')[1].ToString() : "";

            }
            Logger.LogMessage(TracingLevel.INFO, "GetTranDataFromString:" + obj.cd_challan_ref_no);
            Logger.LogMessage(TracingLevel.INFO, "KII_Home_Return_UID_Session :" + obj.EmpID);
            return obj;
        }
        #endregion
        #region KII RTC 34
        public ActionResult KIIDoubleVerification(string ReqChallanRefNo)
        {
            Session.Timeout = Session.Timeout + 15;
            string ChallanRefNo = ReqChallanRefNo;
            string URL = "https://khajane2.karnataka.gov.in/KhajaneWs/rct/rrpys/secbc/RctReceivePaymentStatusService?wsdl";
            //string URL = "https://117.239.56.125/KhajaneWs/rct/rrpys/RctReceivePaymentStatusService?wsdl";
            string transactiondate = DateTime.Now.ToString("ddMMyyyy");
            ByteArrayOutputStream fileWriter = null;
            StringBuilder content = null;
            string currPath = string.Empty;
            string SignedresultContent = string.Empty;
            string KIIsignresponse = string.Empty;
            string reqFile = TextFileCreate(ChallanRefNo);
            XMLInputFactory factory = XMLInputFactory.newInstance();
            //File fileLoc = new File(filePath);
            FileReader fileReader = new FileReader(reqFile);
            //XMLStreamReader reader = factory.createXMLStreamReader(fileReader);
            //content = new StringBuilder();
            // Parsing XML using stream reader and writing to a ByteArrayOutputStream
            string AsBase64String = string.Empty;
            byte[] AsBytes = System.IO.File.ReadAllBytes(reqFile);
            fileReader = new FileReader(reqFile);
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
                Logger.LogMessage(TracingLevel.INFO, "RCT34-SignedResultData" + SignedresultContent);
            }
            Logger.LogMessage(TracingLevel.INFO, "10.10.31.231:8080/SignXmlData-After convertion");
            ////////
            if (SignedresultContent != null)
            {
                string xml2 = @"<?xml version='1.0' encoding='utf-8'?>"
    +
    "<soapenv:Envelope xmlns:soapenv=" + "'" + "http://schemas.xmlsoap.org/soap/envelope/" + "'" + " xmlns:ser=" + "'" + "http://service.receivepymntstatus.dept.rct.integration.ifms.gov.in/" + "'" + " xmlns:head=" + "'" + "http://header.ei.integration.ifms.gov.in/" + "'" + ">"
    + "\n"
    + "   <soapenv:Header>"
    + "\n"
    + "      <ser:Header>"
    + "\n"
    + "         <head:agencyCode>EA_KID</head:agencyCode>"
     + "\n"
    + "         <head:integrationCode>RCT034</head:integrationCode>"
     + "\n"
    + "         <head:uirNo>EA_KID-RCT034-" + transactiondate + "-" + ChallanRefNo + "</head:uirNo>"
                + "\n"
    + "      </ser:Header>"
     + "\n"
    + "   </soapenv:Header>"
     + "\n"
    + "   <soapenv:Body>"
     + "\n"
    + "      <ser:envelopedDataReq>"
     + "\n"
    + "         <Signature>" + SignedresultContent + "</Signature>"
    + "\n"
    + beforecanonXmlData
    + "      </ser:envelopedDataReq>"
     + "\n"
    + "   </soapenv:Body>"
     + "\n"
    + "</soapenv:Envelope>";

                string responseStr = string.Empty;
                string jsonText = string.Empty;
                //var _url = "https://preprodkhajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/";
                //var _action = "RctReceiveValidateChlnService?wsdl";

                //XmlDocument soapEnvelopeXml = CreateSoapEnvelope(signeddata, xml2);
                // WebRequest.DefaultCachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                //Log.Debug("Request for Signature Validation");
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

                //Log.Debug("FileStream Rquesting for Pending Payment");
                Logger.LogMessage(TracingLevel.INFO, "RCT34-FileStream Rquesting for Pending Payment");
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                //Log.Debug("FileStream Rquest for Pending Payment Proceed");
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                Logger.LogMessage(TracingLevel.INFO, "RCT34-FileStream Rquest for Pending Payment Proceed");
                //Log.Debug("Response from KII Pending Payment Request:  " + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-HttpWebResponse:OK");
                    Stream responseStream = response.GetResponseStream();
                    responseStr = new StreamReader(responseStream).ReadToEnd();
                    XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(responseStr));
                    var xmlWithoutNs = xmlDocumentWithoutNs.ToString();
                    Logger.LogMessage(TracingLevel.INFO, "RCT34 - XmlResultWithoutNs" + xmlWithoutNs);
                    //Log.Debug("Response KII Pending Request Data:  " + xmlWithoutNs);
                    //return Content(responseStr);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlWithoutNs);
                    //XmlNode node = doc.SelectSingleNode("Basic_vehicle_detailsResult");
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);

                    // To convert JSON text contained in string json into an XML node
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
                    ViewBag.Response = responseStr;
                    ViewBag.Response1 = doc.InnerText;
                    Logger.LogMessage(TracingLevel.INFO, "RCT34 - doc.InnerText" + doc.InnerText);
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-Results" + responseStr);
                    //jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                    try
                    {
                        jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                        Logger.LogMessage(TracingLevel.INFO, "RCT34-ResultJsonConversion" + jsonText);
                    }
                    catch (Exception ex)
                    {
                        string errtext = ex.Message.ToString();
                        Logger.LogMessage(TracingLevel.INFO, "RCT34-ResultJsonConversionError" + jsonText + "& ErrorMsg" + errtext);
                    }

                    //jsonText = JsonConvert.SerializeXmlNode(doc);

                    //Status = "Payment pending at payment gateway";
                    //Status = jsonText;
                    DoubleVerificationDataResponseKII DVDataResponseK2 = new DoubleVerificationDataResponseKII();
                    DVDataResponseK2 = JsonConvert.DeserializeObject<DoubleVerificationDataResponseKII>(jsonText);
                    if (DVDataResponseK2.statusCode == "KII-RCTER-34")
                    {
                        DVDataResponseK2.status = "Department reference number not found";
                    }
                    else if (DVDataResponseK2.statusCode == "KII-RCTER-00")
                    {
                        if (DVDataResponseK2.pymntstatus == "10700066")
                        {
                            DVDataResponseK2.status = "Payment received at payment gateway";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700092")
                        {
                            DVDataResponseK2.status = "Payment pending at payment gateway";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700068")
                        {
                            DVDataResponseK2.status = "Payment failed at payment gateway";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700103")
                        {
                            DVDataResponseK2.status = "Transaction Failed";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700098")
                        {
                            DVDataResponseK2.status = "Challan Expired";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700072")
                        {
                            DVDataResponseK2.status = "Payment received at agency bank";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700070")
                        {
                            DVDataResponseK2.status = "Payment pending with agency bank";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720038")
                        {
                            DVDataResponseK2.status = "Cheque Bounced";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720072")
                        {
                            DVDataResponseK2.status = "Lost in transit";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720074")
                        {
                            DVDataResponseK2.status = "Mutilated";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720076")
                        {
                            DVDataResponseK2.status = "Signature Missing";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720078")
                        {
                            DVDataResponseK2.status = "Overwriting";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720080")
                        {
                            DVDataResponseK2.status = "Amount mismatch";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720082")
                        {
                            DVDataResponseK2.status = "Cheque/DD accepted, clearance awaited";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700220")
                        {
                            DVDataResponseK2.status = "Payment not initiated at payment gateway";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720005")
                        {
                            DVDataResponseK2.status = "Challan Expired Reconcilation status";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720034")
                        {
                            DVDataResponseK2.status = "TTR in Progress";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720036")
                        {
                            DVDataResponseK2.status = "TTR Approved";
                        }
                    }
                    long EmpID = 0;
                    if (Session["UID"] != null)
                    {
                        EmpID = Convert.ToInt64(Session["UID"]);
                        Logger.LogMessage(TracingLevel.INFO, "RCT34-EmpID " + EmpID);
                    }
                    Logger.LogMessage(TracingLevel.INFO, "Payment status" + DVDataResponseK2.pymntstatus);
                    string result = _INBApplicationbll.UpdateNBChallanStatusDll(ChallanRefNo, Convert.ToInt64(DVDataResponseK2.pymntstatus), EmpID);
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-DBResult" + result);
                    //return View(DVDataResponseK2);  
                    if (DVDataResponseK2.pymntstatus == "10700066")
                    {
                        try
                        {
                            Logger.LogMessage(TracingLevel.INFO, "SessionData Mobile&AppRefNo" + Session["MobileNo"].ToString() + Session["AppRefNo"].ToString());

                            var ApprefNo = (from ad in _db.tbl_kgid_application_details where ad.kad_emp_id == EmpID select ad.kad_kgid_application_number).ToList().LastOrDefault();
                            //var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == EmpID select eb).FirstOrDefault();

                            //    string msg = "ನಿಮ್ಮ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ" + ApprefNo + "ರ ಪ್ರಾರಂಭಿಕ ಠೇವಣಿ ರೂ " + DVDataResponseK2.paidAmount + "ಗಳ ಪಾವತಿಯು ದಿನಾಂಕ " + DVDataResponseK2.currentTimeStamp + " ದಂದು ‌ಯಶಸ್ವಿಯಾಗಿದ್ದು, ಚಲನ್ ಸಂಖ್ಯೆ " + ChallanRefNo + " ಆಗಿರುತ್ತದೆ."

                            //+ "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                            var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587390729269);
                            Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700066  1107161587390729269" + msg.ToString());

                            msg = msg.Replace("#var1#", Session["AppRefNo"].ToString());
                            msg = msg.Replace("#var2#", DVDataResponseK2.paidAmount.ToString());
                            msg = msg.Replace("#var3#", DVDataResponseK2.currentTimeStamp.ToString());
                            msg = msg.Replace("#var4#", ChallanRefNo);
                            Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700066 replace of values" + msg.ToString());
                            Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification  10700066 mobile no." + Session["MobileNo"].ToString());

                            AllCommon.sendUnicodeSMS(Session["MobileNo"].ToString(), msg, "1107161587390729269");
                            Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700066 SMS sent");

                            string emailcontent = "Dear Proposer," +
                            "The challan reference number " + ChallanRefNo + " . having your Initial Deposit payment"
                            + "of Rs" + DVDataResponseK2.paidAmount + " . towards the proposal reference number" + Session["AppRefNo"].ToString() + ". has been "
                            + "Successfully received."
                            + "The Payment Receipt of Rs " + DVDataResponseK2.paidAmount + ". Is attached hereto. Please note the proposal reference number and quote the same in all your future correspondence "
                            + "Please note that the Initial deposit payment remitted by you is only for operational convenience and does not amount to automatic acceptance of the risk.Your proposal will be evaluated by the department and Policy bond will be issued once the same is approved."

                                + "In case of queries or assistance, Please call us on 080 - 22536189"

                                + "Looking forward and assuring of our best service."

                                    + "Warm Regards,"
                                    + "KGID, Official Branch";

                            Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700066  emailcontent" + emailcontent.ToString());
                            Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700066  email_id" + Session["EmailId"].ToString());

                            AllCommon objemail = new AllCommon();
                            //objemail.SendEmail(Session["EmailId"].ToString(), emailcontent, "Payment Successful");
                            Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700066 EMail sent");
                        }
                        catch (Exception ex)
                        {
                            Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700066" + ex.Message.ToString());
                        }


                        return RedirectToAction("ApplicationForm", "Employee", new RouteValueDictionary(new { pay = "true" }));
                    }
                    else
                    {
                        if (DVDataResponseK2.pymntstatus == "10700092" || DVDataResponseK2.pymntstatus == "10700070") //pending
                        {


                        }
                        if (DVDataResponseK2.pymntstatus == "10700068" || DVDataResponseK2.pymntstatus == "10700103" || DVDataResponseK2.pymntstatus == "10700098") //failed
                        {
                            try
                            {
                                Logger.LogMessage(TracingLevel.INFO, "SessionData Mobile&AppRefNo"+ Session["MobileNo"].ToString() + Session["AppRefNo"].ToString());
                                //string msg = "ನಿಮ್ಮ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ" + ApprefNo + " ರ ಪ್ರಾರಂಭಿಕ ಠೇವಣಿ ರೂ" + DVDataResponseK2.paidAmount + "ಗಳ ಪಾವತಿ ಪ್ರಯತ್ನ ಯಶಸ್ವಿಯಾಗಿರುವುದಿಲ್ಲ, https://kgidonline.karnataka.gov.in ನಲ್ಲಿ ಲಾಗಿನ್‌ ಆಗಿ ಮತ್ತೆ  ಪ್ರಯತ್ನಿಸಿ, ಹೆಚ್ಚಿನ ಮಾಹಿತಿಗಾಗಿ ಸಹಾಯವಾಣಿ ಸಂಖ್ಯೆ 080-22536189 ನ್ನು ಸಂಪರ್ಕಿಸಿ"
                                //           + "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                                var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587458099452);
                                Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700068  1107161587458099452" + msg.ToString());

                                msg = msg.Replace("#var1#", Session["AppRefNo"].ToString());
                                msg = msg.Replace("#var2#", DVDataResponseK2.paidAmount.ToString());

                                Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700068  replace of values" + msg.ToString());
                                Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700068  replace of values" + Session["MobileNo"].ToString());


                                AllCommon.sendUnicodeSMS(Session["MobileNo"].ToString(), msg, "1107161587458099452");
                                Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700068  SMS sent");
                            }
                            catch (Exception ex)
                            {
                                Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700068" + ex.Message.ToString());
                            }


                        }
                        return RedirectToAction("ApplicationForm", "Employee");
                    }
                }
                else
                {
                    Logger.LogMessage(TracingLevel.INFO, "signed content error");
                    return RedirectToAction("ApplicationForm", "Employee");
                }
            }
            return RedirectToAction("ApplicationForm", "Employee");
        }
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
        public string TextFileCreate(string ChallanRefNo)
        {
            Logger.LogMessage(TracingLevel.INFO, "TextFileCreate()");
            // KD0221801112345678
            //string dd = DateTime.Now.ToString("dd");
            //string MM = DateTime.Now.ToString("MM");
            //string yy = DateTime.Now.ToString("yy");
            //string ddHHmmss = DateTime.Now.ToString("ddHHmmss");
            //deptRefNum = "KD" + MM + yy + "8011" + ddHHmmss;
            //var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            string newFile = Server.MapPath("~/Documents/KIIRequest/DoubleVerification/" + ChallanRefNo + ".txt");
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
                    strmwrtr.WriteLine("            <RctReceivePaymentStatusRq>");
                    strmwrtr.WriteLine("               <deptRefNum>" + ChallanRefNo + "</deptRefNum>");
                    strmwrtr.WriteLine("            </RctReceivePaymentStatusRq>");
                    strmwrtr.WriteLine("         </data>");
                }

                Logger.LogMessage(TracingLevel.INFO, "Line catch" + newFile);
                return newFile;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        #endregion
        #region ContactUs
        [Route("kgid-cs")]
        public ActionResult ContactUs()
        {
            return View();
        }
        #endregion
        public ActionResult KIIDoubleVerificationTest(string ReqChallanRefNo, string Date)
        {
            string ChallanRefNo = ReqChallanRefNo;
            string URL = "https://khajane2.karnataka.gov.in/KhajaneWs/rct/rrpys/secbc/RctReceivePaymentStatusService?wsdl";
            string transactiondate = Date;
            ByteArrayOutputStream fileWriter = null;
            StringBuilder content = null;
            string currPath = string.Empty;
            string SignedresultContent = string.Empty;
            string KIIsignresponse = string.Empty;
            string reqFile = TextFileCreate(ChallanRefNo);
            XMLInputFactory factory = XMLInputFactory.newInstance();
            FileReader fileReader = new FileReader(reqFile);
            string AsBase64String = string.Empty;
            byte[] AsBytes = System.IO.File.ReadAllBytes(reqFile);
            fileReader = new FileReader(reqFile);
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
                Logger.LogMessage(TracingLevel.INFO, "RCT34-SignedResultData" + SignedresultContent);
            }
            Logger.LogMessage(TracingLevel.INFO, "10.10.31.231:8080/SignXmlData-After convertion");
            ////////
            if (SignedresultContent != null)
            {
                string xml2 = @"<?xml version='1.0' encoding='utf-8'?>"
    +
    "<soapenv:Envelope xmlns:soapenv=" + "'" + "http://schemas.xmlsoap.org/soap/envelope/" + "'" + " xmlns:ser=" + "'" + "http://service.receivepymntstatus.dept.rct.integration.ifms.gov.in/" + "'" + " xmlns:head=" + "'" + "http://header.ei.integration.ifms.gov.in/" + "'" + ">"
    + "\n"
    + "   <soapenv:Header>"
    + "\n"
    + "      <ser:Header>"
    + "\n"
    + "         <head:agencyCode>EA_KID</head:agencyCode>"
     + "\n"
    + "         <head:integrationCode>RCT034</head:integrationCode>"
     + "\n"
    + "         <head:uirNo>EA_KID-RCT034-" + transactiondate + "-" + ChallanRefNo + "</head:uirNo>"
                + "\n"
    + "      </ser:Header>"
     + "\n"
    + "   </soapenv:Header>"
     + "\n"
    + "   <soapenv:Body>"
     + "\n"
    + "      <ser:envelopedDataReq>"
     + "\n"
    + "         <Signature>" + SignedresultContent + "</Signature>"
    + "\n"
    + beforecanonXmlData
    + "      </ser:envelopedDataReq>"
     + "\n"
    + "   </soapenv:Body>"
     + "\n"
    + "</soapenv:Envelope>";

                string responseStr = string.Empty;
                string jsonText = string.Empty;
                //Logger.LogMessage(TracingLevel.INFO, "RCT34-Request for Signature Validation");
                //Log.Debug("Request for Signature Validation");
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(URL);
                byte[] bytes;
                bytes = Encoding.UTF8.GetBytes(xml2);
                request.ContentType = "text/xml";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                //Log.Debug("FileStream Rquesting for Pending Payment");
                Logger.LogMessage(TracingLevel.INFO, "RCT34-FileStream Rquesting for Pending Payment");
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                //Log.Debug("FileStream Rquest for Pending Payment Proceed");
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                Logger.LogMessage(TracingLevel.INFO, "RCT34-FileStream Rquest for Pending Payment Proceed");
                //Log.Debug("Response from KII Pending Payment Request:  " + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-HttpWebResponse:OK");
                    Stream responseStream = response.GetResponseStream();
                    responseStr = new StreamReader(responseStream).ReadToEnd();
                    XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(responseStr));
                    var xmlWithoutNs = xmlDocumentWithoutNs.ToString();
                    Logger.LogMessage(TracingLevel.INFO, "RCT34 - XmlResultWithoutNs" + xmlWithoutNs);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlWithoutNs);
                    //XmlNode node = doc.SelectSingleNode("Basic_vehicle_detailsResult");
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);

                    // To convert JSON text contained in string json into an XML node
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
                    ViewBag.Response = responseStr;
                    ViewBag.Response1 = doc.InnerText;
                    Logger.LogMessage(TracingLevel.INFO, "RCT34 - doc.InnerText" + doc.InnerText);
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-Results" + responseStr);
                    //jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                    try
                    {
                        Logger.LogMessage(TracingLevel.INFO, "RCT34 -XML Doc To convert JSON Start");
                        jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                        Logger.LogMessage(TracingLevel.INFO, "RCT34-ResultJsonConversion" + jsonText);
                    }
                    catch (Exception ex)
                    {
                        string errtext = ex.Message.ToString();
                        Logger.LogMessage(TracingLevel.INFO, "RCT34-ResultJsonConversionError" + jsonText + "& ErrorMsg" + errtext);
                    }

                    DoubleVerificationDataResponseKII DVDataResponseK2 = new DoubleVerificationDataResponseKII();
                    DVDataResponseK2 = JsonConvert.DeserializeObject<DoubleVerificationDataResponseKII>(jsonText);
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-AfterJsonConversion" + jsonText + "& StatusCode: " + DVDataResponseK2.statusCode + "&&" + "PaymentStatus: " + DVDataResponseK2.pymntstatus);
                    string TestResult = "StatusCode" + DVDataResponseK2.statusCode + "PaymentStatus:" + DVDataResponseK2.pymntstatus;
                    return Json(TestResult, JsonRequestBehavior.AllowGet);
                    //return View(DVDataResponseK2); 
                }
                else
                {
                    Logger.LogMessage(TracingLevel.INFO, "signed content error");
                    string TestResultError = "signed content error";
                    return Json(TestResultError, JsonRequestBehavior.AllowGet);
                }
            }
            string TestResultHome = "Unable To Verify";
            return Json(TestResultHome, JsonRequestBehavior.AllowGet);
        }

    }
}
