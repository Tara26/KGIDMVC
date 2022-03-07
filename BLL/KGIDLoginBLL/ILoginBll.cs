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
    public interface ILoginBll
    {
        vm_kgid_user LoginBLL(string userName, string encryptedPassword);
        List<tbl_category_master> GetUserCategories();
        int ChangePasswordBLL(tbl_logindetails obj);
        string [] FeatchRecord(string psw);
        int AddOTPDetails(int otp, long kgid);
        int getOTPDetails(int kgid);

        //VM_Employeelogin NewEmployeeLoginBLL(VM_Employeelogin _newEUsers);

        //VM_Employeelogin InsuredEmployeeLoginBLL(VM_Employeelogin _insuredEUsers);
    }
}
