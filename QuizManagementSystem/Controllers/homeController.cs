using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;

namespace QuizManagementSystem.Controllers
{
    public class homeController : Controller
    {
        // GET: home
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var testModel = new ExamDAO().GetAllExamPageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(testModel);
        }
    }
}