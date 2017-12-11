using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class loginController : Controller
    {
        // GET: admin/login
        public ActionResult Index()
        {
            return View();
        }
    }
}