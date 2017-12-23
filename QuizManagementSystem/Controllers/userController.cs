using Model.DAO;
using QuizManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizManagementSystem.Common;

namespace QuizManagementSystem.Controllers
{
    public class userController : Controller
    {
        // GET: user
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var _userDao = new UserDAO();
                var _result = _userDao.Login(loginModel.UserName, Common.Encode.MD5Hash(loginModel.Password));

                if (_result == 1)
                {
                    var _user = _userDao.GetUserByUserName(loginModel.UserName);
                    var _userSession = new UserLogin();
                    _userSession.UserName = _user.UserName;
                    _userSession.UserID = _user.Id;
                    Session.Add(ConstantVariable.USER_SESSION, _userSession);
                    return Redirect("/admin/home");
                }
                else if (_result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
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
            return View(loginModel);
        }

        //Logout
        public ActionResult Logout()
        {
            Session[Common.ConstantVariable.USER_SESSION] = null;
            return Redirect("/");
        }
    }
}