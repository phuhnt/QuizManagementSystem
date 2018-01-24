using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;
using QuizManagementSystem.Controllers;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class homeController : baseController
    {
        // GET: admin/home
        [HasCredential(RoleID = "ADMIN_HOME")]
        public ActionResult Index()
        {
            var _userDao = new UserDAO();
            var _examDao = new ExamDAO();
            var _testDao = new TestDAO();
            var _quizDao = new QuizDAO();
            ViewBag.User = _userDao.GetAllUserActive().Count.ToString();
            ViewBag.Exam = _examDao.GetAllExamActive().Count.ToString();
            ViewBag.Test = _testDao.GetAllTest().Count.ToString();
            ViewBag.Quiz = _quizDao.GetAllQuizActive().Count.ToString();
            return View();
        }
    }
}