using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizManagementSystem.Common;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class baseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var _session = Session[ConstantVariable.USER_SESSION];
            if(_session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(
                    new { controller = "login", action = "Index", Area = "admin" }));
            }
            base.OnActionExecuted(filterContext);
        }
    }
}