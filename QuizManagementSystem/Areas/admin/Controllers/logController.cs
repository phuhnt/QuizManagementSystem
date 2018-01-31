using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class logController : Controller
    {
        // GET: admin/log
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var _dao = new SystemLogDAO();
            var _model = _dao.GetAllSystemLogPageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(_model);
        }

        public static bool SystemLog(string eventName, string performedBy, TimeSpan exTime, DateTime exDate, string clientIp)
        {
            var logDao = new SystemLogDAO();
            logDao.Insert(eventName, performedBy, exTime, exDate, clientIp);
            return true;
        }
    }
}