using Model.DAO;
using QuizManagementSystem.Common;
using QuizManagementSystem.Models;
using System.Net;
using System.Web.Mvc;
using Model.EF;
using System;

namespace QuizManagementSystem.Controllers
{
    public class userController : BaseAlert
    {
        // GET: user
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string userLink)
        {
            ViewBag.userLink = userLink;
            return View();
        }


        //[HttpPost]
        //public ActionResult Login(LoginModel loginModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var _userDao = new UserDAO();
        //        var _result = _userDao.Login(loginModel.UserName, Common.Encode.MD5Hash(loginModel.Password));

        //        if (_result == 1)
        //        {
        //            var _user = _userDao.GetUserByUserName(loginModel.UserName);
        //            var _userSession = new UserLogin();
        //            _userSession.UserName = _user.UserName;
        //            _userSession.UserID = _user.Id;
        //            _userSession.Avatar = _user.Avatar;
        //            _userSession.GroupID = _user.GroupID;
        //            var _listRole = _userDao.GetListRole(loginModel.UserName);
        //            if (_user.Avatar != null)
        //            {
        //                _userSession.Avatar = _user.Avatar;
        //            }
        //            else
        //            {
        //                _userSession.Avatar = "/Data/images/avatardefault.png";
        //            }
        //            Session.Add(ConstantVariable.USER_SESSION, _userSession);
        //            Session.Add(ConstantVariable.SESSION_CREDENTIAL, _listRole);
        //            return Redirect("/");
        //        }
        //        else if (_result == ConstantVariable.NotExist)  // 0
        //        {
        //            ModelState.AddModelError("", "Tài khoản không tồn tại!");
        //        }
        //        else if (_result == ConstantVariable.IsLocked)  // -1
        //        {
        //            ModelState.AddModelError("", "Tài khoản đang bị khóa!");
        //        }
        //        else if (_result == ConstantVariable.Incorrect) // -2
        //        {
        //            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu chưa đúng!");
        //        }
        //        else if (_result == ConstantVariable.NotHaveAccess) // -3
        //        {
        //            ModelState.AddModelError("", "Tài khoản của bạn không có quyền truy cập!");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Có lỗi! Đăng nhập không thành công!");
        //        }
        //    }
        //    return View(loginModel);
        //}

        [HttpPost]
        public ActionResult Login(LoginModel loginModel, string userLink)
        {
            if (ModelState.IsValid)
            {
                var _userDao = new UserDAO();
                var _result = _userDao.Login(loginModel.UserName, Encode.MD5Hash(loginModel.Password));

                if (_result == 1)
                {
                    var _user = _userDao.GetUserByUserName(loginModel.UserName);
                    var _userSession = new UserLogin();
                    _userSession.UserName = _user.UserName;
                    _userSession.UserID = _user.Id;
                    _userSession.Avatar = _user.Avatar;
                    _userSession.GroupID = _user.GroupID;
                    var _listRole = _userDao.GetListRole(loginModel.UserName);
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
                    if (userLink == null)
                        return Redirect("/");
                    else
                        return Redirect("" + userLink + "");
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
            return View(loginModel);
        }

        //Logout
        public ActionResult Logout()
        {
            Session[ConstantVariable.USER_SESSION] = null;
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult Info(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (CheckSession(id) == false)
            {
                return RedirectToAction("AccessDenied", "error");
            }
            var _user = new UserDAO().GetUserById(id);
            if (_user == null)
            {
                return HttpNotFound();
            }
            _user.ConfirmPasswordHash = null;

            return View(_user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Info([Bind(Include = "Id,PasswordHash,NewPasswordHash,ConfirmPasswordHash,Email,DayOfBirth,Phone,FullName,Sex,Avatar,ModifiedBy")]User user)
        {
            if (CheckSession(user.Id) == false)
            {
                return RedirectToAction("AccessDenied", "error");
            }
            if (user == null)
            {
                return HttpNotFound();
            }


            var _userDao = new UserDAO();
            var _userCurr = _userDao.GetUserById(user.Id);
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (!String.IsNullOrEmpty(user.NewPasswordHash) && !String.IsNullOrEmpty(user.ConfirmPasswordHash))
            {
                if (String.Compare(user.NewPasswordHash, user.ConfirmPasswordHash, true) != 0)
                {
                    ModelState.AddModelError("", "Mật khẩu chưa trùng khớp.");
                }
                else // Update user có cập nhật mật khẩu mới
                {
                    var _newPassWordHash = Encode.MD5Hash(user.NewPasswordHash);

                    user.PasswordHash = _newPassWordHash; //Gán mật khẩu mới vào cột PasswordHash
                    user.NewPasswordHash = null; 
                    user.ConfirmPasswordHash = null;
                    user.ModifiedBy = _userSession.UserName;

                    // Giữ lại các trường không đổi
                    user.UserName = _userCurr.UserName;
                    user.ClassID = _userCurr.ClassID;
                    user.Class = _userCurr.Class;
                    user.CreateBy = _userCurr.CreateBy;
                    user.GroupID = _userCurr.GroupID;
                    user.UserGroup = _userCurr.UserGroup;
                    user.Status = _userCurr.Status;
                    user.DateOfParticipation = _userCurr.DateOfParticipation;
                    
                    var _result = _userDao.Update(user);


                    if (_result == true)
                    {
                        _userSession.Avatar = null;
                        _userSession.Avatar = user.Avatar;
                        SetAlert("Cập nhật người dùng thành công.", "success");
                        return View(user);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật người dùng không thành công.");
                    }
                }
            }
            else if (String.IsNullOrEmpty(user.NewPasswordHash) && String.IsNullOrEmpty(user.ConfirmPasswordHash)) // Ko cập nhật mật khẩu
            {
                user.ModifiedBy = _userSession.UserName;
                // Giữ lại các trường không đổi
                user.UserName = _userCurr.UserName;
                user.ClassID = _userCurr.ClassID;
                user.Class = _userCurr.Class;
                user.CreateBy = _userCurr.CreateBy;
                user.GroupID = _userCurr.GroupID;
                user.UserGroup = _userCurr.UserGroup;
                user.Status = _userCurr.Status;
                user.DateOfParticipation = _userCurr.DateOfParticipation;

                var _result = _userDao.Update(user, null);
                if (_result == true)
                {
                    SetAlert("Sửa thông tin người dùng thành công.", "success");
                    return View(user);
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật người dùng không thành công.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập vào mật khẩu mới và xác nhận mật khẩu vào ô bên dưới.");
            }

            return View(user);
        }

        public ActionResult HistoryExams(int? id, string searchString, int page = 1, int pageSize = 10)
        {
            if (CheckSession(id) == false)
            {
                SetAlert("Truy cập không hợp lệ.", "error");
                return Redirect("/");
            }

            var _user = new UserDAO().GetUserById(id);
            var listTestResult = new TestResultDetailDAO().GetAllTestResultPageList(_user, searchString, page = 1, pageSize = 10);
            return View(listTestResult);
        }

        private bool CheckSession(int? id)
        {
            var session = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (session == null)
            {
                return false;
            }
            else
            {
                if (session.UserID != id)
                {
                    return false;
                }
            }
            return true;
        }
    }
}