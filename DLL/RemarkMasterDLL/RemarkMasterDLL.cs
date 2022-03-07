using DLL.DBConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL.RemarkMasterDLL
{
    public class RemarkMasterDLL : IRemarkMasterDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        public IEnumerable<SelectListItem> GetRemarkListDLL(int ModuleType)
        {
            var types = new List<SelectListItem>();

            types = (from t in _db.tbl_remarks_master
                     where t.RM_Active_Status == true && t.RM_Module_Type == ModuleType
                     select (new SelectListItem { Text = t.RM_Remarks_Desc, Value = t.RM_Remarks_id.ToString() })).ToList();
            
            return types;
        }
        public string GetRemarkCommentsDLL(int RemarkID)
        {
            var comments = (from r in _db.tbl_remarks_master
                            where r.RM_Remarks_id == RemarkID
                            select r).FirstOrDefault();
            return comments.RM_Comments;
        }
    }
}
