using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class classController : Controller
    {

        // GET: admin/class
        public ActionResult Index()
        {
            return View();
        }
    }
}