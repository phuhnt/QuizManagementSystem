using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using QuizManagementSystem.Common;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class userController : Controller
    {
        // GET: admin/user
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var _dao = new UserDAO();
                var _passWordHash = Encode.MD5Hash(user.PaswordHash);
                long _id = _dao.Insert(user);

                if (_id > 0)
                {
                    return RedirectToAction("Index", "user");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm người dùng thành công!");
                }
            }
            return View("Index");
        }
    }
}