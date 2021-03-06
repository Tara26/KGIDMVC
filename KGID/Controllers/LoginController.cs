using BLL.KGIDLoginBLL;
using KGID_Models.KGID_User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL.NewEmployeeBLL;
using BLL.DDOMasterBLL;
using BLL.DeptMasterBLL;
using BLL.InsuredEmployeeBll;
using KGID_Models.KGID_Login;
using System.Text.RegularExpressions;
using KGID_Models.KGIDNBApplication;
using KGID_Models.KGID_VerifyData;
using Common;
using KGID_Models.KGID_Master;
using KGID_Models.NBApplication;
using KGID.Models;
using BLL.AES_Encription;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Web.SessionState;
using KGID_Models.Dashboard;
using System.Web.Configuration;

namespace KGID.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginBll _login;
        private readonly INewEmployeeDetailsBLL _newemp;
          private readonly INBApplicationBll _INBApplicationbll;
        private readonly IDDOMasterBLL _ddomaster;
        private readonly IDeptMasterBLL _deptmaster;
        private readonly IInsuredEmployeeBll _insuredEmployee;
        public static string captcha_val = null;
        public static  string CaptchaNew = string.Empty;
        private CommonMethod objCM = new CommonMethod();
        public LoginController()
        {
            this._login = new LoginBll();
            this._newemp = new NewEmployeeDetailsBLL();
            this._ddomaster = new DDOMasterBLL();
            this._deptmaster = new DeptMasterBLL();
            this._insuredEmployee = new InsuredEmployeeBll();
            this._INBApplicationbll = new NBApplicationBll();
        }
		public LoginController(string cap)
        {
            
            tbl_logindetails obj = new tbl_logindetails();
            captcha_val = cap;
            obj.image_captcha = captcha_val;

        }
        // GET: Login
        public ActionResult Index()
        {
            string User_Name = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                User_Name = reqCookies["UserName"].ToString();
            }
            else
            {
                HttpCookie userInfo = new HttpCookie("userInfo");
                userInfo["UserName"] = "KGID";
                userInfo.Expires.Add(new TimeSpan(1, 1, 0));
                Response.Cookies.Add(userInfo);
            }
            
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", string.Empty));
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
			tbl_logindetails obj = new tbl_logindetails();
            
                obj.image_captcha = captcha_val;
            //ViewBag.Data = _ddomaster.DDOMasterbll();
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(Guid.NewGuid().ToString());
            }
            string id = builder.ToString(0, 16);
            tbl_logindetails loginDetails = new tbl_logindetails(id.ToString());
            ViewBag.code = loginDetails.Dynamc_Code;
            TempData["code"] = loginDetails.Dynamc_Code;
            return View(obj);
        }
        private string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        private string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }
        private const int SaltSize = 32;
        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[SaltSize];

                rng.GetBytes(randomNumber);

                return randomNumber;

            }
        }
        public static byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public JsonResult GetCaptcha()
        {
            Random rnd = new Random();
            StringBuilder captcha = new StringBuilder();
            //for (int i = 0; i < 6; i++)
            //{
            //    // captcha.Append(rnd.Next(1, 9).ToString() + " ");
            //    captcha.Append(RandomString(6));

            //}
            var _res = RandomString(6);
            //Session.Remove("GenCaptcha");
            //Session["GenCaptcha"] = _res;
            // return 
            return Json(_res, JsonRequestBehavior.AllowGet);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(tbl_logindetails _KGIDLogin, FormCollection form)
        {
            string isSuccess = string.Empty;
            string message = string.Empty;
            string pswkey = Request.Form["pswkey"];
            string _GenCaptcha = String.Empty;
            
            //string EncryptPWD = Encryptdata(_KGIDLogin.um_user_password);
            var _userName = AESEncrytDecry.DecryptStringAES(_KGIDLogin.um_user_name, pswkey);
            var _userPassword = AESEncrytDecry.DecryptStringAES(_KGIDLogin.um_user_password, pswkey);
            _KGIDLogin.um_user_name = _userName;
            _KGIDLogin.um_user_password = _userPassword;
            var psw = EncryptionLibrary.EncryptText(_userPassword);
            //var psw = EncryptionLibrary.EncryptText(_KGIDLogin.um_user_password);
            _KGIDLogin.um_user_password = psw;
            //_KGIDLogin.txtEnteredCaptcha = null;
            // _GenCaptcha = Session["GenCaptcha"].ToString();
            if (ModelState.IsValid)
            {
                //if (_GenCaptcha != _KGIDLogin.txtEnteredCaptcha)
                if (_KGIDLogin.txtInputCaptcha.Replace(" ", String.Empty) != _KGIDLogin.txtEnteredCaptcha)
                {
                    isSuccess = "WrongCaptcha";
                    TempData["Captcha_validation"] = "Invalid Captcha";
                    //return RedirectToAction("Index", "Login");
                    return Json(new { IsSuccess = isSuccess, RedirectUrl = "/Login/Index/" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var userData = _login.LoginBLL(_KGIDLogin.um_user_name, psw);
                    if (userData != null && (userData.um_status == 1 || userData.um_agency_status == true))
                    {
                        if (userData.um_role == "8")
                        {
                            if (_KGIDLogin.um_user_name.Equals(userData.um_user_name, StringComparison.OrdinalIgnoreCase) && (psw.Equals(userData.um_user_password)))
                            {
                                Session["UserName"] = userData.um_user_name;
                                Session["UID"] = userData.um_agency_id;
                                //TempData["UserData"] = userData;
                                Session["Categories"] = userData.um_role;
                                Session["LoginType"] = "Insured Employee";
                                //Session["LoginType"] = LoginTypes.AGENCY.ToString();
                                isSuccess = "SuccessLogin";
                                message = "Login successful";
                                ViewBag.isrepayment = false;
                            }
                            else
                            {
                                isSuccess = "InvalidLogin";
                                message = "Invalid username or password";
                            }
                        }
                        else if (userData.um_role == "14")
                        {
                            if (_KGIDLogin.um_user_name.Equals(userData.um_user_name, StringComparison.OrdinalIgnoreCase) && (psw.Equals(userData.um_user_password)))
                            {
                                Session["UserName"] = userData.um_user_name;
                                Session["UID"] = userData.um_agency_id;
                                //TempData["UserData"] = userData;
                                Session["Categories"] = userData.um_role;
                                //Session["LoginType"] = LoginTypes.AGENCY.ToString();
                                isSuccess = "SuccessLogin";
                                message = "Login successful";
                                ViewBag.isrepayment = false;
                            }
                            else
                            {
                                isSuccess = "InvalidLogin";
                                message = "Invalid username or password";
                            }
                        }
                        else if (userData.um_role == "9")
                        {
                            if (_KGIDLogin.um_user_name.Equals(userData.um_user_name, StringComparison.OrdinalIgnoreCase) && (psw.Equals(userData.um_user_password)))
                            {
                                Session["UserName"] = userData.um_user_name;
                                Session["UID"] = userData.um_agency_id;
                                //TempData["UserData"] = userData;
                                Session["Categories"] = userData.um_role;
                                //Session["LoginType"] = LoginTypes.AGENCY.ToString();
                                isSuccess = "SuccessLogin";
                                message = "Login successful";
                            }
                            else
                            {
                                isSuccess = "InvalidLogin";
                                message = "Invalid username or password";
                            }
                        }
                        else if (userData.um_role == "11")
                        {
                            if (_KGIDLogin.um_user_name.Equals(userData.um_user_name, StringComparison.OrdinalIgnoreCase) && (psw.Equals(userData.um_user_password)))
                            {
                                Session["UserName"] = userData.um_user_name;
                                Session["UID"] = userData.um_agency_id;
                                //TempData["UserData"] = userData;
                                Session["Categories"] = userData.um_role;
                                //Session["LoginType"] = LoginTypes.AGENCY.ToString();
                                isSuccess = "SuccessLogin";
                                message = "Login successful";
                            }
                            else
                            {
                                isSuccess = "InvalidLogin";
                                message = "Invalid username or password";
                            }
                        }
                        else if (userData.um_role == "13")
                        {
                            if (_KGIDLogin.um_user_name.Equals(userData.um_user_name, StringComparison.OrdinalIgnoreCase) && (psw.Equals(userData.um_user_password)))
                            {
                                Session["UserName"] = userData.um_user_name;
                                Session["UID"] = userData.um_agency_id;
                                //TempData["UserData"] = userData;
                                Session["Categories"] = userData.um_role;
                                //Session["LoginType"] = LoginTypes.AGENCY.ToString();
                                isSuccess = "SuccessLogin";
                                message = "Login successful";
                            }
                            else
                            {
                                isSuccess = "InvalidLogin";
                                message = "Invalid username or password";
                            }
                        }
                    }
                    else
                    {                        
                        string[] loginkgidao = WebConfigurationManager.AppSettings["RePayment"].ToString().Split(',');
                        string   loginkgidaopass = WebConfigurationManager.AppSettings["RePaymentPass"].ToString();
                        
                        if (loginkgidao.Contains(_KGIDLogin.um_user_name) && loginkgidaopass == _userPassword)
                        {
                            Session["UserName"] = _KGIDLogin.um_user_name;
                            isSuccess = "SuccessLogin";
                            message = "Login successful";
                            Session["UID"] = 0;
                            ViewBag.isrepayment = true;
                            Session["Categories"] = 50;
                        }
                        else
                        {
                            isSuccess = "InvalidLogin";
                            message = "Invalid username or password";
                        }
                    }
                }
            }
            else
            {
                isSuccess = "InvalidLogin";
                message = "Invalid username or password";
            }

            //return Json(new
            //{
            //    IsSuccess = isSuccess,
            //    Message = message
            //}, JsonRequestBehavior.AllowGet); ;
            if (isSuccess == "SuccessLogin")
            {
                //return RedirectToAction("Dashboard", "Home");
                return Json(new { IsSuccess = isSuccess, RedirectUrl = "/kgid-home/" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //TempData["EMessage"] = message;
                ViewBag.EMessage = message;
                //return RedirectToAction("Index", "Login");
                return Json(new { IsSuccess = isSuccess, RedirectUrl = "/Login/Index" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Employee()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeLogin(tbl_login_master _KGIDLogin)
        {
            string isSuccess = string.Empty;
            string message = string.Empty;
            var userData = _login.LoginBLL(string.Empty, string.Empty);
            if (userData != null && userData.um_status == 1)
            {
                if (_KGIDLogin.um_user_name.ToUpper() == userData.um_user_name && _KGIDLogin.um_user_password == userData.um_user_password)
                {
                    Session["UserProfile"] = userData.um_user_name;
                    Session["UserName"] = userData.um_user_name;
                    Session["UID"] = userData.um_user_id;
                   // TempData["UserData"] = userData;
                    //Session["LoginType"] = LoginTypes.KGID.ToString();
                    FormsAuthentication.SetAuthCookie(userData.um_user_name, true);
                    Session["LoggedInUserName"] = userData.um_user_name;
                    TempData["LoginSuccess"] = 1;
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    TempData["Captcha_validation"] = "Invalid User Name or Password";
                    TempData["LoginFailure"] = 0;
                    return RedirectToAction("Index", "Login");
                }
            }


            return RedirectToAction("Employee", "Login");
        }
        [HttpGet]
        public ActionResult VerifyKGIDAndMobileNo(string kgId, long mobileNumber)
        {
            try
            {
                if (Session["AuthToken"] == null)
                {
                    return Json(new { IsSuccess = false,IsRedirect = true, RedirectUrl = "/Login/Index" }, JsonRequestBehavior.AllowGet);
                }
                string isSuccess = string.Empty;
                string message = string.Empty;
                objCM.ErrorHandler("Login", 410);
                string redirectUrl = string.Empty;
                var insuredEmployee = _newemp.VerifyKGIDAndMobileNo(kgId, mobileNumber);
                if (insuredEmployee != null)
                {
                    Session["EID"] = insuredEmployee.EmployeeId;
                    //Session["UserName"] = insuredEmployee.EmployeeName;
                    //Session["UID"] = insuredEmployee.EmployeeId;
                    //Session["Categories"] = insuredEmployee.UserCategory;
                    //Session["Department"] = insuredEmployee.Department;
                    //Session["Designation"] = insuredEmployee.Designation;
                    //Session["InsuredUser"] = true;
                    Session["FirstKGIDNo"] = insuredEmployee.FirstKGIDNo;
                    //Session["LoginType"] = "Insured Employee";
                    if (insuredEmployee.UserCategory.Split(',').Count() > 1)
                    {
                        ViewBag.IsMultipleCategories = true;
                    }
                    else
                    {
                        ViewBag.IsMultipleCategories = false;
                    }

                    Random generator = new Random();
                    String otp = "2121"; //generator.Next(0, 999999).ToString("D6");
                    //Session["OTP"] = otp;
                    int result = _login.AddOTPDetails(Convert.ToInt32(otp), Convert.ToInt64(insuredEmployee.EmployeeId));
                    //string msg = otp + " is your One Time Password (OTP) for changing password in KGID";

                    try
                    {
                        var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161605342227349);
                        msg = msg.Replace("{#var#}", otp);
                        Session["MobileNo"] = mobileNumber.ToString();
                        AllCommon.sendUnicodeSMS(mobileNumber.ToString(), msg, "1107161605342227349");

                        Session["EmailId"] = insuredEmployee.Email;

                    string emailcontent = "Dear Proposer," +
                    "You have been registered successfully on Karnataka Government Insurance Department(KGID) portal.Now you can start applying for KGID policy by logging in to https://kgidonline.karnataka.gov.in as a new employee using your registered mobile no/ e-mail."
                    + "The One Time Password(OTP) generated for your KGID login is " + otp + " and do not share this One Time Password with anyone."
                    + "Please note that this OTP is valid for 10 minutes or 1 successful attempt, whichever is earlier."
                    + "In case of queries or assistance, Please call us on 080 - 22536189"

                        + "Warm Regards,"
                        + "KGID, Official Branch";
                        AllCommon objemail = new AllCommon();
                        objemail.SendEmail(insuredEmployee.Email, emailcontent, "One Time Password");
                        
                    }
                    catch(Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "VerifyKGIDAndMobileNo "+ex.Message.ToLower());
                    }
                    

                    List<VM_EmpDashboardData> list = null;
                    if (Session["FirstKGIDNo"] != null)
                    {
                        list = _INBApplicationbll.GetDetailsBasedOnKGIDNo(Convert.ToInt64(Session["FirstKGIDNo"]));
                    }
                    
                    if (list.Count > 0)
                    {
                        redirectUrl = Url.Action("GetDetailsBasedOnKGIDNo", "Home", new { area = "" });
                    }
                    else
                    {
                        redirectUrl = Url.Action("Dashboard", "Home", new { area = "" });
                    }
                    
                    return Json(new { IsSuccess = true, IsRedirect = false, RedirectUrl = redirectUrl }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "VerifyKGIDAndMobileNo-Exception occured " + ex.Message);
            }
            return Json(new { IsSuccess = false, IsRedirect = false, RedirectUrl = "/Login/Index" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VerifyNewEmployeeLogin(VM_NewEmployeeLogin newEmployeeDetails)
        {
            if(Session["AuthToken"] == null)
            {
                return Json(new { IsSuccess = false, EmpId = 0, RedirectUrl = "/Login/Index", IsKGIDPresent = false }, JsonRequestBehavior.AllowGet);
            }
            
            string success = string.Empty;
            string message = string.Empty;
            
            try
            {
                bool isSuccess = false;
                bool isKGIDPresent = false;
                VM_NewEmployeeLoginDetail employeeDetails = null;
                string redirectUrl = string.Empty;
                if (newEmployeeDetails.IsMobileLogin)
                {
                    var mobilePattern = @"^\d{10}$";
                    if (Regex.IsMatch(newEmployeeDetails.LoginValue, mobilePattern))
                    {
                        Session["MobileNo"] = newEmployeeDetails.LoginValue;
                        employeeDetails = _newemp.GetEmployeeByMobileNumber(Convert.ToInt64(newEmployeeDetails.LoginValue));
                        if (employeeDetails != null && (employeeDetails.FirstKGIDNumber == null || employeeDetails.FirstKGIDNumber == "" || employeeDetails.FirstKGIDNumber == "0"))
                        {
                            if (string.IsNullOrEmpty(employeeDetails.FirstKGIDNumber) || employeeDetails.FirstKGIDNumber == "0")
                            {
                                Session["EID"] = employeeDetails.EmployeeId;
                                //Session["UserName"] = employeeDetails.EmployeeName;
                                //Session["UID"] = employeeDetails.EmployeeId;
                                //Session["Categories"] = employeeDetails.UserCategory;
                                //Session["Department"] = employeeDetails.Department;
                                //Session["Designation"] = employeeDetails.Designation;
                                //Session["LoginType"] = "New Employee";

                                if (employeeDetails.UserCategory.Split(',').Count() > 1)
                                {
                                    ViewBag.IsMultipleCategories = true;
                                }
                                else
                                {
                                    ViewBag.IsMultipleCategories = false;
                                }
                               // Logger.LogMessage(TracingLevel.INFO, "employeeDetails.EmployeeId " + employeeDetails.EmployeeId);
                                Random generator = new Random();
                                String otp = "2121"; //generator.Next(0, 999999).ToString("D6");
                                                     // Session["OTP"] = otp;
                                                     // Logger.LogMessage(TracingLevel.INFO, "AddOTPDetails " + otp);
                                int result = _login.AddOTPDetails(Convert.ToInt32(otp), Convert.ToInt64(employeeDetails.EmployeeId));
                               // Logger.LogMessage(TracingLevel.INFO, "AddOTPDetails " + result);
                                //string msg = otp + " is your One Time Password (OTP) for changing password in KGID";
                               // Logger.LogMessage(TracingLevel.INFO, "sendOTPMSG start ");
                                try
                                {
                                    var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161605342227349);
                                    msg = msg.Replace("{#var#}", otp);
                                    AllCommon.sendUnicodeSMS(newEmployeeDetails.LoginValue.ToString(), msg, "1107161605342227349");
                                    
                                    string emailcontent = "Dear Proposer," +
                                    "You have been registered successfully on Karnataka Government Insurance Department(KGID) portal.Now you can start applying for KGID policy by logging in to https://kgidonline.karnataka.gov.in as a new employee using your registered mobile no/ e-mail."
                                    + "The One Time Password(OTP) generated for your KGID login is " + otp + " and do not share this One Time Password with anyone."
                                    + "Please note that this OTP is valid for 10 minutes or 1 successful attempt, whichever is earlier."
                                    + "In case of queries or assistance, Please call us on 080 - 22536189"

                                    + "Warm Regards,"
                                    + "KGID, Official Branch";
                                    AllCommon objemail = new AllCommon();
                                    Session["EmailId"] = employeeDetails.Email;
                                    objemail.SendEmail(employeeDetails.Email, emailcontent, "One Time Password");
                                   

                                }
                                catch(Exception ex)
                                {
                                    Logger.LogMessage(TracingLevel.INFO, "VerifyNewEmployeeLogin " + ex.Message.ToString());
                                }
                                
                                isSuccess = true;
                            }
                            else
                            {
                                isSuccess = false;
                                isKGIDPresent = true;
                            }
                        }
                        else
                        {
                            isSuccess = false;
                            isKGIDPresent = true;
                        }
                    }
                }
                else
                {
                    var emailRgexPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                    if (Regex.IsMatch(newEmployeeDetails.LoginValue, emailRgexPattern))
                    {
                        employeeDetails = _newemp.GetEmployeeByEmail(newEmployeeDetails.LoginValue);
                        if (employeeDetails != null && (employeeDetails.FirstKGIDNumber == null || employeeDetails.FirstKGIDNumber == "" || employeeDetails.FirstKGIDNumber == "0"))
                        {
                            if (string.IsNullOrEmpty(employeeDetails.FirstKGIDNumber) || employeeDetails.FirstKGIDNumber == "0")
                            {
                                Session["EID"] = employeeDetails.EmployeeId;
                                //Session["UserName"] = employeeDetails.EmployeeName;
                                //Session["UID"] = employeeDetails.EmployeeId;
                                //Session["Categories"] = employeeDetails.UserCategory;
                                //Session["Department"] = employeeDetails.Department;
                                //Session["Designation"] = employeeDetails.Designation;

                                if (employeeDetails.UserCategory.Split(',').Count() > 1)
                                {
                                    ViewBag.IsMultipleCategories = true;
                                }
                                else
                                {
                                    ViewBag.IsMultipleCategories = false;
                                }
                                Random generator = new Random();
                                String otp = "2121"; //generator.Next(0, 999999).ToString("D6");
                                                     //  Session["OTP"] = otp;
                                int result = _login.AddOTPDetails(Convert.ToInt32(otp), Convert.ToInt64(employeeDetails.EmployeeId));
                                try
                                {
                                    var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161605342227349);
                                    msg = msg.Replace("{#var#}", otp);
                                    Session["MobileNo"] = employeeDetails.MobileNumber.ToString();

                                    AllCommon.sendUnicodeSMS(employeeDetails.MobileNumber.ToString(), msg, "1107161605342227349");
                                    
                                    string emailcontent = "Dear Proposer," +
                                    "You have been registered successfully on Karnataka Government Insurance Department(KGID) portal.Now you can start applying for KGID policy by logging in to https://kgidonline.karnataka.gov.in as a new employee using your registered mobile no/ e-mail."
                                    + "The One Time Password(OTP) generated for your KGID login is " + otp + " and do not share this One Time Password with anyone."
                                    + "Please note that this OTP is valid for 10 minutes or 1 successful attempt, whichever is earlier."
                                    + "In case of queries or assistance, Please call us on 080 - 22536189"

                                    + "Warm Regards,"
                                    + "KGID, Official Branch";
                                    Session["EmailId"] = employeeDetails.Email.ToString();
                                    AllCommon objemail = new AllCommon();
                                    objemail.SendEmail(employeeDetails.Email, emailcontent, "One Time Password");
                                }
                                catch(Exception ex)
                                {
                                    Logger.LogMessage(TracingLevel.INFO, "VerifyNewEmployeeLogin "+ex.Message.ToString());
                                }
                                
                                isSuccess = true;
                            }
                            else
                            {
                                isSuccess = false;
                                isKGIDPresent = true;
                            }
                        }
                        else
                        {   
                            isSuccess = false;
                            isKGIDPresent = true;
                        }
                    }
                }

                //bool EmpSts = _newemp.GetEmployeeStatusBll(employeeDetails.EmployeeId);
                //if (EmpSts.ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                //{
                //    redirectUrl = Url.Action("ViewApplicationDetails", "VerifyDetails", new { area = "" });
                //}
                //else
                //{
                //    redirectUrl = Url.Action("ApplicationForm", "Employee", new { area = "", empId = employeeDetails.EmployeeId });
                //}
                if (employeeDetails != null && (employeeDetails.FirstKGIDNumber != null || employeeDetails.FirstKGIDNumber != "" || employeeDetails.FirstKGIDNumber == "0"))
                {
                    ViewBag.isKgidAvailable = 1;
                }
                redirectUrl = Url.Action("Dashboard", "Home", new { area = "" });

                return Json(new { IsSuccess = isSuccess, EmpId = employeeDetails != null ? employeeDetails.EmployeeId : 0, RedirectUrl = redirectUrl, IsKGIDPresent = isKGIDPresent }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult SelectCategory()
        {
            List<KeyValuePair<int, string>> categories = new List<KeyValuePair<int, string>>();

            List<tbl_category_master> userCategories = _login.GetUserCategories();

            foreach (var item in userCategories)
            {
                categories.Add(new KeyValuePair<int, string>(item.cm_category_id, item.cm_category_desc));
            }

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SwitchCategory(string selectedCategory)
        {
            string redirectUrl = "";
            Session["SelectedCategory"] = selectedCategory;
            if (selectedCategory == "1")
            {

                List<VM_EmpDashboardData> list = null;
                if (Session["FirstKGIDNo"] != null)
                {
                    list = _INBApplicationbll.GetDetailsBasedOnKGIDNo(Convert.ToInt64(Session["FirstKGIDNo"]));
                }

                if (list.Count > 0)
                {
                    redirectUrl = Url.Action("GetDetailsBasedOnKGIDNo", "Home", new { area = "" });
                }
                else
                {
                    redirectUrl = Url.Action("Dashboard", "Home", new { area = "" });
                }

            }
            else 
            {
                redirectUrl = Url.Action("Dashboard", "Home", new { area = "" });
            }

            return Json(new { IsSuccess = true ,RedirectUrl= redirectUrl }, JsonRequestBehavior.AllowGet);
        }
public static String captcha_new
        {
            get { return CaptchaNew; }
            set { CaptchaNew = value; }
        }
        public FileContentResult CaptchaImage(bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            //generate new question
            //int a = rand.Next(10, 99);
            //int b = rand.Next(0, 9);
            //var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            // Session["Captcha"] = a + b;
            //string captcha = GetCaptchaString(6);

            tbl_logindetails obj = new tbl_logindetails();
            

                CaptchaNew = GetCaptchaString(6);
                LoginController obj_login=new LoginController(CaptchaNew);
                
               
                obj.image_captcha = captcha_val;
            
            string captcha = captcha_val;
            Session["GenCaptcha"] = null;
            Session["GenCaptcha"] = captcha;
           
           
            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                //if (noisy)
                //{
                //    int i, r, x, y;
                //    var pen = new Pen(Color.Yellow);
                //    for (i = 1; i < 10; i++)
                //    {
                //        pen.Color = Color.FromArgb(
                //        (rand.Next(0, 255)),
                //        (rand.Next(0, 255)),
                //        (rand.Next(0, 255)));

                //        r = rand.Next(0, (130 / 3));
                //        x = rand.Next(0, 130);
                //        y = rand.Next(0, 30);

                //        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                //    }
                //}

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Png
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                img = this.File(mem.GetBuffer(), "image/png");
            }

            return File(img.FileContents, img.ContentType);
        }


        public string GetCaptchaString(int length)
        {
            int intZero = '0';
            int intNine = '9';
            int intA = 'A';
            int intZ = 'Z';
            int intCount = 0;
            int intRandomNumber = 0;
            string strCaptchaString = "";

            Random random = new Random(System.DateTime.Now.Millisecond);

            while (intCount < length)
            {
                intRandomNumber = random.Next(intZero, intZ);
                if (((intRandomNumber >= intZero) && (intRandomNumber <= intNine) || (intRandomNumber >= intA) && (intRandomNumber <= intZ)))
                {
                    strCaptchaString = strCaptchaString + (char)intRandomNumber;
                    intCount = intCount + 1;
                }
            }
            return strCaptchaString;
        }


        public ActionResult OTPAuthentication(int kgid, long MNO, string EMail, int OTP, int LType)
        {
            int ValidOTP = 0;
            int result = _login.getOTPDetails(Convert.ToInt32(Session["EID"]));
          // if (result == OTP)
          if (2121 == OTP)
            {
                if (LType == 1)
                {
                    var insuredEmployee = _newemp.VerifyKGIDAndMobileNo(kgid.ToString(), MNO);
                    Session["UserName"] = insuredEmployee.EmployeeName;
                    Session["UID"] = insuredEmployee.EmployeeId;
                    Session["Categories"] = insuredEmployee.UserCategory;
                    Session["Department"] = insuredEmployee.Department;
                    Session["Designation"] = insuredEmployee.Designation;
                    Session["InsuredUser"] = true;
                    Session["FirstKGIDNo"] = insuredEmployee.FirstKGIDNo;
                    Session["LoginType"] = "Insured Employee";
                }
                else if (LType == 2)
                {
                    VM_NewEmployeeLoginDetail employeeDetails = null;

                    employeeDetails = _newemp.GetEmployeeByMobileNumber(Convert.ToInt64(MNO));

                    Session["UserName"] = employeeDetails.EmployeeName;
                    Session["UID"] = employeeDetails.EmployeeId;
                    Session["Categories"] = employeeDetails.UserCategory;
                    Session["Department"] = employeeDetails.Department;
                    Session["Designation"] = employeeDetails.Designation;
                    Session["LoginType"] = "New Employee";
                }
                else if (LType == 3)
                {
                    VM_NewEmployeeLoginDetail employeeDetails = null;
                    employeeDetails = _newemp.GetEmployeeByEmail(EMail);

                    Session["UserName"] = employeeDetails.EmployeeName;
                    Session["UID"] = employeeDetails.EmployeeId;
                    Session["Categories"] = employeeDetails.UserCategory;
                    Session["Department"] = employeeDetails.Department;
                    Session["Designation"] = employeeDetails.Designation;
                }
                ValidOTP = 1;
            }
            return Json(ValidOTP, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult KGIDRoles()
        //{
        //    VM_DDOVerificationDetails verificationDetails = _login.GetEmployeeDetailsForDDOVerification(Convert.ToInt64(Session["UID"]));
        //    return View("DDO", verificationDetails);
        //}

        public int SendSMSAfterLogin()
        {
            try
            {
                var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161623523856523);
                AllCommon.sendUnicodeSMS(Session["MobileNo"].ToString(), msg, "1107161623523856523");
                return 1;
            }
            catch(Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "SendSMSAfterLogin 1107161623523856523 " + ex.Message.ToString());
                return 0;
            }  
        }
        public ActionResult KGID()
        {
            return View();
        }
#region SessionTimeout

        public ActionResult SessionTimeout()
        {
            return View();
        }

        #endregion
    }
}