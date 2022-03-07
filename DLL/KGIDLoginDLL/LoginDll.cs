using KGID_Models.KGID_User;
using System.Linq;
using DLL.DBConnection;
using KGID_Models.KGID_Master;
using System.Collections.Generic;
using System;
using KGID_Models.KGID_Login;
using System.Data;
using System.Data.SqlClient;

namespace DLL.KGIDLoginDLL
{
    public class LoginDll : ILoginDll
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();

        public List<tbl_category_master> GetUserCategories()
        {
            return (from userCat in _db.tbl_category_master
                    select userCat).ToList();
        }

        private readonly Common_Connection _Conn = new Common_Connection();
        public int AddOTPDetails(int otp, long kgid)
        {
          
        DataSet dsBD = new DataSet();
            SqlParameter[] sqlparam =
            {
                new SqlParameter("@otp",otp),
                 new SqlParameter("@kgid",kgid)
            };

            int result =Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_insert_otp_details"));

            return result;

        }

        public int getotpdetails(int kgid)
        {
            SqlParameter[] sqlparam =
           {
               
                 new SqlParameter("@kgid",kgid)
            };

            int result = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_getOTPDetails"));

            return result;
        }


        //public vm_kgid_user Login(string userName, string encryptedPassword)
        //{
        //    var UserDtls = (from n in _db.tbl_agency_login
        //                    where n.al_agency_user_id == userName && n.al_password == encryptedPassword
        //                    select new vm_kgid_user
        //                    {
        //                        um_agency_id = n.al_agency_login_id,
        //                        um_user_name = n.al_agency_user_id,
        //                        um_user_password = n.al_password,
        //                        um_agency_status = n.al_status,
        //                        um_role=n.al_user_category_id
        //                    }).FirstOrDefault();
        //    return UserDtls;
        //}
		public vm_kgid_user Login(string userName, string encryptedPassword)
        {
            vm_kgid_user obj = new vm_kgid_user();
            var UserDtls = (from n in _db.tbl_agency_login
                            where n.al_agency_user_id == userName && n.al_password == encryptedPassword
                            select n).FirstOrDefault();



         var kgid_user =   new vm_kgid_user()
            {
                um_agency_id = UserDtls.al_agency_login_id,
                um_user_name = UserDtls.al_agency_user_id,
                um_user_password = UserDtls.al_password,
                um_agency_status = UserDtls.al_status,
                um_role = UserDtls.al_user_category_id,
                //um__agency_ddo_id=n.al_agency_ddo_id
            };
            return kgid_user;
        }

        public int ChangePassword(tbl_logindetails _KGIDLogin)
        {

            tbl_agency_login obj = new tbl_agency_login();

            var UserDtls = (from n in _db.tbl_agency_login
                            join agencyddomaster in _db.tbl_agency_ddo_master
                            on n.al_agency_ddo_id equals agencyddomaster.adm_agency_id
                            where n.al_password == _KGIDLogin.um_user_password && agencyddomaster.adm_telephone_no == _KGIDLogin.nebd_mobilenumber && agencyddomaster.adm_email == _KGIDLogin.nebd_email

                            select new vm_kgid_user
                            {
                                um_agency_id = n.al_agency_login_id,
                                um_user_password = n.al_password,
                                um_user_name =  n.al_agency_user_id,
                                um_creation_datetime=n.al_creation_datetime,
                                um_created_by=n.al_created_by


                            }).ToList();


            foreach (var xx in UserDtls)
            {
               
                var result = _db.tbl_agency_login.Where(rx=>rx.al_agency_user_id== xx.um_user_name && rx.al_password== xx.um_user_password && rx.al_creation_datetime==xx.um_creation_datetime && rx.al_created_by==xx.um_created_by).FirstOrDefault();
                result.al_password = _KGIDLogin.um_user_New_password;
            }



            _db.SaveChanges();






            return 1;
        }

        public string[] FeatchRecord(string psw)
        {
            string[] obj = new string[5];



            var UserDtls = (from n in _db.tbl_agency_login
                            join agencyddomaster in _db.tbl_agency_ddo_master
                            on n.al_agency_ddo_id equals agencyddomaster.adm_agency_id
                            where n.al_password == psw
                            select new
                            {
                                adm_telephone_no = agencyddomaster.adm_telephone_no,
                                adm_email = agencyddomaster.adm_email.ToString()

                            }).FirstOrDefault();


            obj[0] = UserDtls.adm_telephone_no;
            obj[1] = UserDtls.adm_email;






            return obj;
        }
    }
}
