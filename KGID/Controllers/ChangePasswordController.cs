using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using static KGID.FilterConfig;

namespace KGID.Controllers
{
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        private readonly ILoginBll _login;
        private readonly INBApplicationBll _INBApplicationbll;

        private readonly IDeptMasterBLL _deptmaster;
    
        //private CommonMethod objCM = new CommonMethod();

        public ChangePasswordController()
        {
            this._login = new LoginBll();
            this._INBApplicationbll = new NBApplicationBll();



        }

        [NoCache]
        [SessionAuthorize]
        public ActionResult Index()
        {
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(Guid.NewGuid().ToString());
            }
            string id = builder.ToString(0, 16);
            tbl_logindetails loginDetails = new tbl_logindetails(id.ToString());
            ViewBag.code = loginDetails.Dynamc_Code;
            TempData["code"] = loginDetails.Dynamc_Code;
            return View();
        }

        [NoCache]
        [SessionAuthorize]
        public ActionResult ChangePassword()
        {
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(Guid.NewGuid().ToString());
            }
            string id = builder.ToString(0, 16);
            tbl_logindetails loginDetails = new tbl_logindetails(id.ToString());
            ViewBag.code = loginDetails.Dynamc_Code;
            TempData["code"] = loginDetails.Dynamc_Code;

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [NoCache]
        [SessionAuthorize]
        public ActionResult UserChangePassword(tbl_logindetails _KGIDLogin, FormCollection form)
        {
            int user_id = 0;
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", string.Empty));
            string isSuccess = string.Empty;
            string message = string.Empty;
            string pswkey = Request.Form["pswkey"];
            //string EncryptPWD = Encryptdata(_KGIDLogin.um_user_password);
            var _userNewpassword = AESEncrytDecry.DecryptStringAES(_KGIDLogin.um_user_New_password, pswkey);
            var _userconfirmpassword = AESEncrytDecry.DecryptStringAES(_KGIDLogin.um_user_Confirm_password, pswkey);
            var _userPassword = AESEncrytDecry.DecryptStringAES(_KGIDLogin.um_user_password, pswkey);
            _KGIDLogin.um_user_New_password = _userNewpassword;
            //_KGIDLogin.um_user_password = _userPassword;
            var psw = EncryptionLibrary.EncryptText(_userPassword);
            var newpsw = EncryptionLibrary.EncryptText(_userNewpassword);
            var confirmpsw = EncryptionLibrary.EncryptText(_userconfirmpassword);
           
            _KGIDLogin.um_user_password = psw;
            _KGIDLogin.um_user_New_password = newpsw;
            _KGIDLogin.um_user_Confirm_password = confirmpsw;
            if (Session["UID"] != null)
            {

                user_id = Convert.ToInt32(Session["UID"]);
            }
            _KGIDLogin.um_user_id = user_id;
            _KGIDLogin.um_user_category_id = Session["Categories"].ToString();
            //_KGIDLogin.txtEnteredCaptcha = null;



            if (_userNewpassword == _userconfirmpassword)
            {
                var result = _login.FeatchRecord(_KGIDLogin.um_user_password);
                if (result.Count() > 0)
                {
                    _KGIDLogin.nebd_mobilenumber = result[0].ToString();
                    _KGIDLogin.nebd_email= result[1].ToString();
                    Random generator = new Random();
                    String otp = generator.Next(0, 999999).ToString("D6");
                   // Session["OTP"] = otp;
                   // string msg = otp + "is your One Time Password (OTP) for changing password in KGID";
                    try
                    {
                        var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161605342227349);

                        msg = msg.Replace("{#var1#}", otp);
                        Logger.LogMessage(TracingLevel.INFO, "MobileNo " + result[0].ToString());
                        Logger.LogMessage(TracingLevel.INFO, "sendUnicodeSMS 1107161605342227349" + msg);
                        Session["MobileNo"] = result[0].ToString();
                        Session["EmailId"] = result[1].ToString();
                        AllCommon.sendUnicodeSMS(result[0].ToString(), msg, "1107161605342227349");
                        Logger.LogMessage(TracingLevel.INFO, "SMS Sent" + msg);

						string emailcontent = "Dear Proposer," +
                        "You have been registered successfully on Karnataka Government Insurance Department(KGID) portal.Now you can start applying for KGID policy by logging in to https://kgidonline.karnataka.gov.in as a new employee using your registered mobile no/ e-mail."
                        + "The One Time Password(OTP) generated for your KGID login is "+otp+" and do not share this One Time Password with anyone."
                        + "Please note that this OTP is valid for 10 minutes or 1 successful attempt, whichever is earlier."
                        + "In case of queries or assistance, Please call us on 080 - 22536189"

                        + "Warm Regards,"
                        + "KGID, Official Branch";
                        Logger.LogMessage(TracingLevel.INFO, "Email" + result[1].ToString());
                        Logger.LogMessage(TracingLevel.INFO, "SendEmail" + emailcontent);

                        AllCommon objemail = new AllCommon();
                        //objemail.SendEmail(result[1].ToString(), emailcontent, "One Time Password");
                        Logger.LogMessage(TracingLevel.INFO, "Email Sent" + msg);

                    }
                    catch(Exception ex)
                    {
                       
                        Logger.LogMessage(TracingLevel.INFO, "UserChangePassword" + ex.Message.ToString());
                    }
                    isSuccess = "Success";
                    //TempData["_KGIDLogin"] = _KGIDLogin;
                    ViewBag.KGIDLogin= _KGIDLogin;
                }
            }
            else
            {
                isSuccess = "InvalidLogin";
                message = "Confirm Passwor and New Password is not Matching";
            }


            //return Json(new
            //{
            //    IsSuccess = isSuccess,
            //    Message = message
            //}, JsonRequestBehavior.AllowGet); ;
            if (isSuccess == "Success")
            {
                //return RedirectToAction("Dashboard", "Home");
                return Json(new { IsSuccess = isSuccess }, JsonRequestBehavior.AllowGet);
            }
            else
            {
               // TempData["EMessage"] = message;
                ViewBag.EMessage = message;
                //return RedirectToAction("Index", "Login");
                return Json(new { IsSuccess = isSuccess, RedirectUrl = "/Login/Index" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult VerifyOTP (string OTP)
        {
            string result = null;

            if (OTP == Session["OTP"].ToString())
            {
                result = "1";
                if (ViewBag.KGIDLogin != null)
                {
                    var KGIDLogin = (tbl_logindetails)ViewBag.KGIDLogin;
                    var userData = _login.ChangePasswordBLL(KGIDLogin);
                }
            }
            else
            {
                result = "2";
            }
            return Json(new { IsSuccess = result }, JsonRequestBehavior.AllowGet);
        }
    }
}