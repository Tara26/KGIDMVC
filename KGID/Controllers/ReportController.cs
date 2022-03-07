using BLL.KGIDReportsBLL;
using Common;
using KGID.Models;
using KGID_Models.KGID_Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static KGID.FilterConfig;

namespace KGID.Controllers
{
    [NoCache]
    public class ReportController : Controller
    {
        private readonly IKGIDReportsBLL reportsBLL;

        public ReportController()
        {
            reportsBLL = new KGIDReportsBLL();
        }

        [Route("APP-REP")]
        public ActionResult GetKGIDApplicationReport()
        {
            return View("ApplicationReports");
        }

        [HttpPost]
        [Route("APP-REP-DET")]
        public ActionResult GetKGIDApplications(VM_KGIDApplicationReportDetails applicationReportDetails)
        {
            VM_KGIDApplicationReports applicationReports = new VM_KGIDApplicationReports();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            int empType = 1;
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DIO)))
            {
                empType = 2;
            }
            else if (Session["SelectedCategory"] != null && (Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DEPUTYDIRECTOR)) || Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DIRECTOR))))
            {
                empType = 3;
            }
            else if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DDO)))
            {
                empType = 4;
            }
            applicationReports = reportsBLL.GetKGIDApplicationsReport(loggedInUserId, applicationReportDetails, empType);

            return PartialView("_KGIDApplications", applicationReports);
        }
    }
}