using BLL.ClaimsBLL;
using Common;
using KGID.Models;
using KGID_Models.KGID_Bonus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static KGID.FilterConfig;

namespace KGID.Controllers
{
    public class BonusController : Controller
    {
        private readonly IBonusBLL bonusBLL;

        public BonusController()
        {
            bonusBLL = new BonusBLL();
        }

        [HttpGet]
        [NoCache]
        [SessionAuthorize]
        public ActionResult GetBonusArrearList()
        {
            try
            {
                VM_BonusArrears bonusArrears = new VM_BonusArrears();
                long loggedInUserId = Convert.ToInt64(Session["UID"]);

                bonusArrears = bonusBLL.GetBonusArrearList(loggedInUserId);
                return View("BonusArrearList", bonusArrears);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.ERROR, ex.StackTrace);
                return null;
            }
        }

        [HttpGet]
        [NoCache]
        [SessionAuthorize]
        public ActionResult GetBonusArrearDetails(long claimId)
        {
            VM_BonusDetails bonusDetail = new VM_BonusDetails();

            bonusDetail = bonusBLL.GetBonusDetails(claimId);

            return View("BonusArrearDetails", bonusDetail);
        }
    }
}