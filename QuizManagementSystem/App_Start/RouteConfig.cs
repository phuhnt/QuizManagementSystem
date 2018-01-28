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

            //Admin
            routes.MapRoute(
              name: "Start The Test",
              url: "ky-thi-{id}/bat-dau-thi",
              defaults: new { controller = "exams", action = "StartTheTest", id = UrlParameter.Optional },
              namespaces: new[] { "QuizManagementSystem.Areas.admin.Controllers" }
            );

            routes.MapRoute(
              name: "Admin Login",
              url: "admin/dang-nhap",
              defaults: new { controller = "login", action = "login"},
              namespaces: new[] { "QuizManagementSystem.Areas.admin.Controllers" }
            );

            //User
            routes.MapRoute(
                name: "User Login",
                url: "dang-nhap",
                defaults: new { controller = "user", action = "Login"},
                namespaces: new[] { "QuizManagementSystem.Controllers" }
            );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "QuizManagementSystem.Controllers" }
            );
        }
    }
}
