using Model.DAO;
using QuizManagementSystem.Areas.admin.Models;
using QuizManagementSystem.Common;
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

        public ActionResult login(AdminLoginModel model)
        {
            //Form Không rỗng
            if (ModelState.IsValid)
            {
                var _userDao = new UserDAO();
                var _result = _userDao.Login(model.UserName, Encode.MD5Hash(model.PassWord));
                if (_result == 1)
                {
                    var _user = _userDao.GetUserByUserName(model.UserName);
                    var _userSession = new UserLogin();
                    _userSession.UserName = _user.UserName;
                    _userSession.UserID = _user.Id;
                    Session.Add(ConstantVariable.USER_SESSION, _userSession);

                    return RedirectToAction("Index", "home");
                }
                else if (_result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                else if (_result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa!");
                }
                else if (_result == -2)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu chưa đúng!");
                }
                else
                {
                    ModelState.AddModelError("", "Thông tin đăng nhập không đúng!");
                }
            }
            return View("Index");
        }

        //Logout
        public ActionResult logout()
        {
            Session[Common.ConstantVariable.USER_SESSION] = null;
            return Redirect("/admin");
        }
    }
}