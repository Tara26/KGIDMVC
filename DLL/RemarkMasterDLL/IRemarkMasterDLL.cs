using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL.RemarkMasterDLL
{
    public interface IRemarkMasterDLL
    {
        IEnumerable<SelectListItem> GetRemarkListDLL(int ModuleType);
        string GetRemarkCommentsDLL(int RemarkID);
    }
}
