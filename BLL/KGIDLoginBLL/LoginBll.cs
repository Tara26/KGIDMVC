using DLL.KGIDLoginDLL;
using KGID_Models.KGID_Login;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_User;
using KGID_Models.KGIDEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDLoginBLL
{
    public class LoginBll : ILoginBll
    {
        private readonly ILoginDll _Ilogin;
        public LoginBll()
        {
            this._Ilogin = new LoginDll();
        }

        public List<tbl_category_master> GetUserCategories()
        {
            return _Ilogin.GetUserCategories();
        }

        public int AddOTPDetails(int otp, long kgid)
        {
            return _Ilogin.AddOTPDetails(otp, kgid);
        }
        public int getOTPDetails(int kgid)
        {
            return _Ilogin.getotpdetails(kgid);
        }

    public vm_kgid_user LoginBLL(string userName, string encryptedPassword)
        {
            vm_kgid_user _login = new vm_kgid_user();
            try
            {

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(encryptedPassword))
                {
                    var x = _Ilogin.Login(userName, encryptedPassword);
                    return x;
                }
                else
                {
                    _login.Message = "Please Enter YourName and password";
                    return _login;
                }
            }
            catch (Exception ex)
            {
                //ExceptionLogging.SendErrorToText(ex);
                //throw new HttpException("not an integer");
                _login.Message = "Please Enter YourName and password";
                return _login;
            }
        }

        public int ChangePasswordBLL(tbl_logindetails _KGIDLogin)
        {
            vm_kgid_user _login = new vm_kgid_user();
            
                var x = _Ilogin.ChangePassword(_KGIDLogin);
                return x;
           
            
        }
        public string[] FeatchRecord(string psw)
        {
            return _Ilogin.FeatchRecord(psw);
        }

        ////public vm_kgid_user LoginBLL1(tbl_login_master _users)
        ////{
        ////    vm_kgid_user _login = new vm_kgid_user();
        ////    try
        ////    {

        ////        if (_users.um_user_password != null && _users.um_user_name != null)
        ////        {
        ////            var x = _Ilogin.Login(_users);
        ////            return x;
        ////        }
        ////        else
        ////        {
        ////            _login.Message = "Please Enter YourName and password";
        ////            return _login;
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        //ExceptionLogging.SendErrorToText(ex);
        ////        //throw new HttpException("not an integer");
        ////        _login.Message = "Please Enter YourName and password";
        ////        return _login;
        ////    }
        ////}

        //public VM_Employeelogin InsuredEmployeeLoginBLL(VM_Employeelogin _insuredEUsers)
        //{
        //    VM_Employeelogin _login = new VM_Employeelogin();
        //    try
        //    {

        //        if (_insuredEUsers.um_mobile_email != null)
        //        {
        //            var x = _Ilogin.Login(_users);
        //            return x;
        //        }
        //        else
        //        {
        //            _login.Message = "Please Enter YourName and password";
        //            return _login;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionLogging.SendErrorToText(ex);
        //        //throw new HttpException("not an integer");
        //        _login.Message = "Please Enter YourName and password";
        //        return _login;
        //    }
        //}
    }
}

