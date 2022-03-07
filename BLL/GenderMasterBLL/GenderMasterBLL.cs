using DLL.GenderMasterDLL;

namespace BLL.GenderMasterBLL
{
    public class GenderMasterBLL : IGenderMasterBLL
    {
        private IGenderMasterDLL genderDLL;

        public GenderMasterBLL()
        {
            genderDLL = new GenderMasterDLL();
        }
        public string GetGender(int genderid)
        {
            string gender = genderDLL.GetGender(genderid);
            return gender;
        }
    }
}
