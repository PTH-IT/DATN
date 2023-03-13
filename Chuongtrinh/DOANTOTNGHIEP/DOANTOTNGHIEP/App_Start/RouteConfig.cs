using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DOANTOTNGHIEP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Register",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "kiemtra",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "checkaccount", id = UrlParameter.Optional }
            ); routes.MapRoute(
                 name: "home",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "TrangChu", action = "Index", id = UrlParameter.Optional }
             );
        }
    }
}
