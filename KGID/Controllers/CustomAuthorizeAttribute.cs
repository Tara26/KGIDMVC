using System;
using DLL.DBConnection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data;
using System.Linq;
using static KGID.FilterConfig;

namespace KGID.Controllers
{
    [NoCache]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;

            long userId = 0; long selectedCatID = 0; long selectedECatID = 0;
            if (httpContext.Session["UID"] != null)
            {
                userId = Convert.ToInt64(httpContext.Session["UID"]);
            }
            if (httpContext.Session["SelectedCategory"] != null)
            {
                selectedCatID = Convert.ToInt64(httpContext.Session["SelectedCategory"]);
            }
            if (httpContext.Session["SelectedCategory"] == null)
            {
                if (httpContext.Session["Categories"] != null)
                {
                    selectedECatID = Convert.ToInt64(httpContext.Session["Categories"]);
                }
                if (userId != 0 && selectedECatID != 0)
                {
                    using (DbConnectionKGID _db = new DbConnectionKGID())
                    {
                        if (selectedECatID == 11)
                        {
                            var userSelectedRole = (from u in _db.tbl_category_master
                                                    where u.cm_category_id == selectedECatID
                                                    select u.cm_category_desc).FirstOrDefault();

                            var empRoles = (from e in _db.tbl_agency_login
                                            where e.al_agency_login_id == userId
                                            select e.al_user_category_id).FirstOrDefault();
                            if (!empRoles.Contains(selectedECatID.ToString()))
                            {
                                return false;
                            }
                            foreach (var role in allowedroles)
                            {
                                if (role == userSelectedRole) authorize = true;
                            }
                        }
                        else
                        {
                            var userSelectedRole = (from u in _db.tbl_category_master
                                                    where u.cm_category_id == selectedECatID
                                                    select u.cm_category_desc).FirstOrDefault();

                            var empRoles = (from e in _db.tbl_employee_basic_details
                                            where e.employee_id == userId
                                            select e.user_category_id).FirstOrDefault();
                            if (!empRoles.Contains(selectedECatID.ToString()))
                            {
                                return false;
                            }
                            foreach (var role in allowedroles)
                            {
                                if (role == userSelectedRole) authorize = true;
                            }
                        }
                    }
                }
                else
                {
                    authorize = false;
                }
            }
            else
            {
                if (userId != 0 && selectedCatID != 0)
                {
                    using (DbConnectionKGID _db = new DbConnectionKGID())
                    {
                        var userSelectedRole = (from u in _db.tbl_category_master
                                                where u.cm_category_id == selectedCatID
                                                select u.cm_category_desc).FirstOrDefault();

                        var empRoles = (from e in _db.tbl_employee_basic_details
                                        where e.employee_id == userId
                                        select e.user_category_id).FirstOrDefault();
                        if (!empRoles.Contains(selectedCatID.ToString()))
                        {
                            authorize = false;
                        }
                        foreach (var role in allowedroles)
                        {
                            if (role == userSelectedRole) authorize = true;
                        }
                    }
                }

                else
                {
                    authorize = false;
                }
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Login" },
                    { "action", "Index" }
               });
        }
    }
}