using DLL.RemarkMasterDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.RemarkMasterBLL
{
    public class RemarkMasterBLL : IRemarkMasterBLL
    {
        private readonly IRemarkMasterDLL _IRemarkMasterdll;
        public RemarkMasterBLL()
        {
            this._IRemarkMasterdll = new RemarkMasterDLL();
        }
        public IEnumerable<SelectListItem> GetRemarkList(int ModuleType)
        {
            return _IRemarkMasterdll.GetRemarkListDLL(ModuleType);
        }
        public string GetRemarkComments(int RemarkID)
        {
            return _IRemarkMasterdll.GetRemarkCommentsDLL(RemarkID);
        }
    }
}
