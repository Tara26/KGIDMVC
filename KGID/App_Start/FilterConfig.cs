using System;
using System.Web;
using System.Web.Mvc;

namespace KGID
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new OutputCacheAttribute
            {
                NoStore = true,
                Duration = 0,
                VaryByParam = "*",
                Location = System.Web.UI.OutputCacheLocation.None
            });
        }
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
        public sealed class NoCacheAttribute : FilterAttribute, IResultFilter
        {
            public void OnResultExecuting(ResultExecutingContext filterContext)
            {
            }

            public void OnResultExecuted(ResultExecutedContext filterContext)
            {
                var cache = filterContext.HttpContext.Response.Cache;
                cache.SetCacheability(HttpCacheability.NoCache);
                cache.SetRevalidation(HttpCacheRevalidation.ProxyCaches);
                cache.SetExpires(DateTime.Now.AddYears(-5));
                cache.AppendCacheExtension("private");
                cache.AppendCacheExtension("no-cache=Set-Cookie");
                cache.SetProxyMaxAge(TimeSpan.Zero);
            }
        }
    }
}
