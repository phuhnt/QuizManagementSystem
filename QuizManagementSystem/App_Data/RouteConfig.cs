using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuizManagementSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Login",
            //    url: "dang-nhap",
            //    defaults: new { controller = "user", action = "Login", id = UrlParameter.Optional },
            //    namespaces: new[] { "QuizManagementSystem.Controllers" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "user", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "QuizManagementSystem.Controllers" }
            );
        }
    }
}
