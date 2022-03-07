using BLL.AdminBLL;
using KGID.Models;
using KGID_Models.Admin;
using KGID_Models.KGID_Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static KGID.FilterConfig;

namespace KGID.Controllers
{
    [NoCache]
    [SessionAuthorize]
    public class AdminController : Controller
    {
        private readonly IAdminBLL admin;

        public AdminController()
        {
            admin = new AdminBLL();
        }

        [HttpGet]
        public ActionResult SaveDSCDetails()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveDSCDetails(VM_DSCDetails dscDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            dscDetails.LoggedInUser = Convert.ToInt64(Session["UID"]);

            isSuccess = admin.SaveDSCDetails(dscDetails);

            if (isSuccess)
            {
                message = "DSC details saved successfully";
            }
            else
            {
                message = "Could not save DSC details";
            }

            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmpDscDetails(string kgidnum)
        {
            VM_EmployeeDscDetails empDetail = admin.GetEmployeeDSCData(kgidnum);
            return Json(new { EmpDetail = empDetail }, JsonRequestBehavior.AllowGet);
        }
    }
}