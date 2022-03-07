using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.RemarkMasterBLL
{
    public interface IRemarkMasterBLL
    {
        IEnumerable<SelectListItem> GetRemarkList(int ModuleType);
        string GetRemarkComments(int RemarkID);
    }
}
