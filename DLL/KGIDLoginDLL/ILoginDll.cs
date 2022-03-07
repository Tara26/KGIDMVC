using KGID_Models.KGID_Login;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_User;
using System.Collections.Generic;

namespace DLL.KGIDLoginDLL
{
    public interface ILoginDll
    {
        vm_kgid_user Login(string userName, string encryptedPassword);
        List<tbl_category_master> GetUserCategories();
        int ChangePassword(tbl_logindetails _KGIDLogin);
        string[] FeatchRecord(string psw);
        int AddOTPDetails(int otp, long kgid);
        int getotpdetails(int kgid);
    }
}
