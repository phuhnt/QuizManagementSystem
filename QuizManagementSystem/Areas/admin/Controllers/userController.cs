using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using QuizManagementSystem.Common;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class userController : baseController
    {
        // GET: admin/user
        //public ActionResult Index(int page = 1, int pageSize = 10)
        //{
        //    var _dao = new UserDAO();
        //    var _model = _dao.GetAllUserPageList(page, pageSize);
        //    return View(_model);
        //}

        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var _dao = new UserDAO();
            var _model = _dao.GetAllUserPageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(_model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetClassViewBag();
            SetRolesViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            SetClassViewBag();
            var _userDao = new UserDAO();

            // Kiểm tra username có tồn tại trong cơ sở dữ liệu chưa?
            if (_userDao.IsUserNameExist(user.UserName) != null)
            {
                ModelState.AddModelError("", "Tên tài khoản đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
                var _passWordHash = Encode.MD5Hash(user.PasswordHash);
                int _id = 0;

                user.PasswordHash = _passWordHash;
                user.ConfirmPasswordHash = _passWordHash;
                user.DateOfParticipation = DateTime.Now;
                if (_userSession != null)
                {
                    user.CreateBy = _userSession.UserName;
                }
                _id = _userDao.Insert(user);

                if (_id > 0)
                {
                    SetAlert("Thêm người dùng thành công.", "success");
                    return RedirectToAction("Index", "user");
                }
                else
                {
                    ModelState.AddModelError("success", "Thêm người không dùng thành công.");
                }
            }
            SetRolesViewBag();
            return View(user);
        }

        /// <summary>
        /// Chỉnh sửa User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _user = new UserDAO().GetUserById(id);
            if (_user == null)
            {
                return HttpNotFound();
            }
            _user.ConfirmPasswordHash = null;
            SetClassViewBag(_user.ClassID);
            SetRolesViewBag(_user.RoleID);

            return View(_user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,PasswordHash,NewPasswordHash,ConfirmPasswordHash,Email,DayOfBirth,Phone,DateOfParticipation,FullName,Sex,ClassID,Avatar,Status,RoleID,CreateBy,ModifiedBy")]User user)
        {
            var _userDao = new UserDAO();
            var _userCurr = _userDao.GetUserById(user.Id);

            if (!String.IsNullOrEmpty(user.NewPasswordHash) && !String.IsNullOrEmpty(user.ConfirmPasswordHash))
            {
                if (String.Compare(user.NewPasswordHash, user.ConfirmPasswordHash, true) != 0)
                {
                    ModelState.AddModelError("", "Mật khẩu chưa trùng khớp.");
                }
                else // Update user có cập nhật mật khẩu mới
                {
                    if (ModelState.IsValid)
                    {
                        var _newPassWordHash = Encode.MD5Hash(user.NewPasswordHash);

                        user.PasswordHash = _newPassWordHash; //Gán mật khẩu mới vào cột PasswordHash
                        user.NewPasswordHash = _userCurr.PasswordHash; //Gán mật khẩu cũ vào NewPasswordHash
                        user.ConfirmPasswordHash = null; 

                        var _result = _userDao.Update(user);
                        if (_result == true)
                        {
                            SetAlert("Cập nhật người dùng thành công.", "success");
                            //return RedirectToAction("Index", "user");
                            return Redirect("/admin/user/detail/" + user.Id);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cập nhật người dùng không thành công.");
                        }
                    }
                }
            }
            else if (String.IsNullOrEmpty(user.NewPasswordHash) && String.IsNullOrEmpty(user.ConfirmPasswordHash)) // Ko cập nhật mật khẩu
            {
                if (ModelState.IsValid)
                {
                    var _result = _userDao.Update(user, null);
                    if (_result == true)
                    {
                        SetAlert("Sửa thông tin người dùng thành công.", "success");
                        //return RedirectToAction("Detail", "user");
                        return Redirect("/admin/user/detail/" + user.Id);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật người dùng không thành công.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập vào mật khẩu mới và xác nhận mật khẩu vào ô bên dưới.");
            }

            SetClassViewBag(user.ClassID);
            SetRolesViewBag(user.RoleID);
            return View(user);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var _user = new UserDAO().GetUserById(id);
            //SetClassViewBag(_user.ClassID);
            //SetRolesViewBag(_user.RoleID);
            return View(_user);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _user = new UserDAO().GetUserById(id);
            if (_user == null)
            {
                return HttpNotFound();
            }
            return View(_user);
        }

        // POST: Example/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
            
        //}

        //Lấy danh sách lớp
        public void SetClassViewBag(int? selectedID = null)
        {
            var _classDao = new ClassDAO();

            ViewBag.ClassID = new SelectList(_classDao.GetAllClass(), "Id", "Name", selectedID);

        }

        //Lấy danh sách quyền
        public void SetRolesViewBag(int? selectedID = null)
        {
            var _roleDao = new RolesDAO();

            ViewBag.RoleID = new SelectList(_roleDao.GetAll(), "Id", "Name", selectedID);
        }


    }
}