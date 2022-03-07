using DLL.DBConnection;
using System.Linq;

namespace DLL.GenderMasterDLL
{
    public class GenderMasterDLL : IGenderMasterDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        public string GetGender(int genderid)
        {
            string gender = (from g in _db.tbl_gender_master
                             where g.gender_id == genderid
                             select g.gender_desc).FirstOrDefault();
            return gender;
        }
    }
}
