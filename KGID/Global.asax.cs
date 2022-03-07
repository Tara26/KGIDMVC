using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace KGID
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Common.Logger.Initialize(Server.MapPath("~"));
            //var UID = HttpContext.Current.Session["UID"].ToString();
            //if (HttpContext.Current.Session["UID"] == null)
            //{
            //    Response.Redirect("/Login/Index");
            //}
            MvcHandler.DisableMvcResponseHeader = true; //Added this line is to hide mvc header

            //var session = HttpContext.Current.Session;

            //if (session != null)
            //{
               
            //}
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                //Request.Headers.Set("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng");

                app.Context.Response.Headers.Remove("Server");
            }
        }
        //protected void Session_Start(Object sender, EventArgs e)
        //{
        //    var session = HttpContext.Current.Session;

        //    if (session.Count > 0)
        //    {
        //        var UID = HttpContext.Current.Session["UID"];
        //        if (UID == null)
        //        {
        //            Response.Redirect("/Login/Index");
        //        }
        //    }
        //}
        //protected void Session_End(Object sender, EventArgs e)
        //{
        //    var session = HttpContext.Current.Session;

        //    if (session.Count > 0)
        //    {
        //        var UID = HttpContext.Current.Session["UID"];
        //        if (UID == null)
        //        {
        //            Response.Redirect("/Login/Index");
        //        }
        //    }
        //}


        
        protected void Session_Start(object sender, EventArgs e)
        {
            string guid = Guid.NewGuid().ToString();
            Session["AuthToken"] = guid;
            // now create a new cookie with this guid value
            //Response.Cookies.Add(new HttpCookie("AuthToken", guid));
        }
        protected void Session_End(object sender, EventArgs e)
        {
            //Response.Cookies.Clear();
            //Response.Cookies["AuthToken"].Expires = DateTime.Now.AddDays(-1);
            Session.Abandon();
            Session.Clear();
        }
    }
}
