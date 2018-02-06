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
                    _userSession.GroupID = _user.GroupID;
                    var _listRole = _userDao.GetListRole(model.UserName);
                    if (_user.Avatar != null)
                    {
                        _userSession.Avatar = _user.Avatar;
                    }
                     else   
                    {
                        _userSession.Avatar = "/Data/images/avatardefault.png";
                    }
                    Session.Add(ConstantVariable.USER_SESSION, _userSession);
                    Session.Add(ConstantVariable.SESSION_CREDENTIAL, _listRole);
                    new SystemLogDAO().Insert("Người dùng [" + _user.UserName + "] đăng nhập thành công", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                    return RedirectToAction("Index", "home");
                }
                else if (_result == ConstantVariable.NotExist)  // 0
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                else if (_result == ConstantVariable.IsLocked)  // -1
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa!");
                }
                else if (_result == ConstantVariable.Incorrect) // -2
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu chưa đúng!");
                }
                else if (_result == ConstantVariable.NotHaveAccess) // -3
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền truy cập!");
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi! Đăng nhập không thành công!");
                }
            }
            return View("Index");
        }

        //Logout
        public ActionResult logout()
        {
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (_userSession != null)
            {
                new SystemLogDAO().Insert("Người dùng [" + _userSession.UserName + "] đăng xuất khỏi hệ thống", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
            }
            
            Session[ConstantVariable.USER_SESSION] = null;
            return Redirect("/admin/login");
        }
    }
}