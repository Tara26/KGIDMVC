using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Common
{
    public class AllCommon
    {
        public string SendEmail(string to_Address, string Body, string Subject)
        {
            try
            {
                string emailID = WebConfigurationManager.AppSettings["EmailID"];
                string Password = WebConfigurationManager.AppSettings["MailPassword"];
                string AppHost = WebConfigurationManager.AppSettings["Host"];
                var AppPort = (WebConfigurationManager.AppSettings["Port"]);
                string subject = Subject;
                string body = Body;
                var smtp = new SmtpClient
                {

                    Host = AppHost,
                    Port = int.Parse(AppPort),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailID, Password)
                };
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                using (var mess = new MailMessage(emailID, to_Address)
                {
                    Subject = subject,
                    Body = body

                })
                {
                    smtp.Send(mess);
                }
                return body;
            }

            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                return message;

            }
        }

        public string SendEmailWithAttachment(string to_Address, string Body, string Subject,MemoryStream AttachmentFilePath)
        {
            try
            {
                string emailID = WebConfigurationManager.AppSettings["EmailID"];
                string Password = WebConfigurationManager.AppSettings["MailPassword"];
                string AppHost = WebConfigurationManager.AppSettings["Host"];
                var AppPort = (WebConfigurationManager.AppSettings["Port"]);
                string subject = Subject;
                string body = Body;

                var smtp = new SmtpClient
                {

                    Host = AppHost,
                    Port = int.Parse(AppPort),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailID, Password)
                };

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                MailMessage message = new MailMessage(emailID, to_Address);
                message.Subject = subject;
                message.Body = body;
                message.Attachments.Add(new Attachment(AttachmentFilePath,"NB Bond", "application/pdf"));

                //using (var mess = new MailMessage(emailID, to_Address)
                //{
                //    Subject = subject,
                //    Body = body,
                //    Attachments.Add(new Attachment())
                //})
                {

                    smtp.Send(message);
                }
                return body;
            }

            catch (Exception ex)
            {                
                string message = ex.Message.ToString();
                Logger.LogMessage(TracingLevel.INFO, "Mail with attachment  - " + message);
                return message;

            }
        }
        public static String sendUnicodeSMS(String mobileNos, String Unicodemessage, String templateid)

        {
            string username = WebConfigurationManager.AppSettings["Username"];
            string password = WebConfigurationManager.AppSettings["Password"];
            string senderid = WebConfigurationManager.AppSettings["Sender_id"];
            string secureKey = WebConfigurationManager.AppSettings["API_service_key"];

            Stream dataStream;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT");
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;

            //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";

            request.Method = "POST";

            System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
            String U_Convertedmessage = "";

            foreach (char c in Unicodemessage)
            {
                int j = (int)c;
                String sss = "&#" + j + ";";
                U_Convertedmessage = U_Convertedmessage + sss;
            }
            String encryptedPassword = encryptedPasswod(password);
            String NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), U_Convertedmessage.Trim(), secureKey.Trim());


            String smsservicetype = "unicodemsg"; // for unicode msg
            String query = "username=" + HttpUtility.UrlEncode(username.Trim()) +
                "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
                "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                "&content=" + HttpUtility.UrlEncode(U_Convertedmessage.Trim()) +
                "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos.Trim()) +
                "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
                "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
                "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());


            byte[] byteArray = Encoding.ASCII.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            String Status = ((HttpWebResponse)response).StatusDescription;
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
        public static String sendOTPMSG(String mobileNo, String message, String templateid)
        {
            string username = WebConfigurationManager.AppSettings["Username"];
            string password = WebConfigurationManager.AppSettings["Password"];
            string senderid = WebConfigurationManager.AppSettings["Sender_id"];
            string secureKey = WebConfigurationManager.AppSettings["API_service_key"];


            Stream dataStream;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT");
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;

            //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
            
            request.Method = "POST";
            System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();

            String encryptPassword = encryptedPasswod(password);
            String NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
            String smsservicetype = "singlemsg"; //For single message. //"9008750107"

            String query = "username=" + HttpUtility.UrlEncode(username.Trim()) +
                "&password=" + HttpUtility.UrlEncode(encryptPassword) +

                "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +

                "&content=" + HttpUtility.UrlEncode(message.Trim()) +

                "&mobileno=" + HttpUtility.UrlEncode(mobileNo.Trim()) +

                "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
              "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
              "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());



            byte[] byteArray = Encoding.ASCII.GetBytes(query);

            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;



            dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);

            dataStream.Close();

            WebResponse response = request.GetResponse();

            String Status = ((HttpWebResponse)response).StatusDescription;

            dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            String responseFromServer = reader.ReadToEnd();

            reader.Close();

            dataStream.Close();

            response.Close();
            return responseFromServer;

        }

        protected static String encryptedPasswod(String password)
        {

            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
            byte[] pp = sha1.ComputeHash(encPwd);
            // static string result = System.Text.Encoding.UTF8.GetString(pp);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pp)
            {

                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();

        }

        protected static String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);

            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }
        public static string CheckExtension(HttpPostedFileBase file)
        {
            try
            {
                var supportedTypes = new[] { "pdf" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    return "File Extension Is InValid - Only Upload PDF File";

                }
                //else if (file.ContentLength > (filesize * 1024))
                //{
                //    ErrorMessage = "File size Should Be UpTo " + filesize + "KB";
                //    return ErrorMessage;
                //}
                else
                {
                    return "1";
                    //return "File Is Successfully Uploaded";

                }
            }
            catch (Exception ex)
            {
                return "Upload Container Should Not Be Empty or Contact Admin";

            }
        }
        public string DateConversion(string Edate)
        {
            string NewDate = string.Empty;
            string[] OldDate = Edate.Split('-');

            DateTime dt = Convert.ToDateTime(OldDate[2] + "-" + OldDate[1] + "-" + OldDate[0]);
            string _date = dt.Day.ToString();
            string _months = dt.Month.ToString();
            string _year = dt.Year.ToString();
            NewDate = _date + "/" + _months + "/" + _year;
            return NewDate;
        }
        public string DateConversion2(string Edate)
        {
            string NewDate = string.Empty;
            string[] OldDate = Edate.Split('-');

            DateTime dt = Convert.ToDateTime(OldDate[2] + "-" + OldDate[1] + "-" + OldDate[0]);
            string _date = dt.Day.ToString();
            string _months = dt.Month.ToString();
            string _year = dt.Year.ToString();
            NewDate = _year + "/" + _months + "/" + _date   ;
            return NewDate;
        }
    }
    public class MyPolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
        {
            return true;
        }
    }
}
