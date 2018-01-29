using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using QuizManagementSystem.Common;

namespace QuizManagementSystem.Controllers
{
    public class homeController : BaseAlert
    {
        // GET: home
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var testModel = new ExamDAO().GetAllExamPageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(testModel);
        }

        public ActionResult Active(string searchString, int page = 1, int pageSize = 10)
        {
            var testModel = new ExamDAO().GetAllExamActivePageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(testModel);
        }

        public ActionResult MyTest(string searchString, int page = 1, int pageSize = 10)
        {
            var session = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (session == null)
            {
                return Redirect("/user/login");
            }
            var testModel = new ExamDAO().GetAllExamUserPageList(session.UserID, searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            if (testModel == null)
            {
                SetAlert("Vui lòng đăng nhập.", "error");
                return Redirect("/");
            }
            return View(testModel);
        }
    }
}