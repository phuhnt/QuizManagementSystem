using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class roleController : Controller
    {
        // GET: admin/role
        public ActionResult Index()
        {
            return View();
        }


    }
}