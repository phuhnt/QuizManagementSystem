using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using QuizManagementSystem.Common;
using System.Data.Entity.Infrastructure;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class userController : Controller
    {
        // GET: admin/user
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var _dao = new UserDAO();
            var _model = _dao.GetAllUserPageList(page, pageSize);
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
        public ActionResult Create(User user, int RoleID)
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

                var _roleDao = new RolesDAO();
                var _passWordHash = Encode.MD5Hash(user.PasswordHash);
                int _id = 0;

                user.PasswordHash = _passWordHash;
                user.ConfirmPaswordHash = _passWordHash;
                user.DateOfParticipation = DateTime.Now;

                var _role = _roleDao.FindById(RoleID);

                if (_role != null)
                {
                    _id = _userDao.Insert(user, _role);
                }
                else
                {
                    _id = _userDao.Insert(user);
                }

                if (_id > 0)
                {
                    return RedirectToAction("Index", "user");
                }
                else
                {
                    ModelState.AddModelError("success", "Thêm người dùng thành công.");
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
        public ActionResult Edit(int id)
        {
            var _user = new UserDAO().GetUserById(id);
            var _role = _user.Roles.FirstOrDefault(x => x.Id == _user.Id);
            

            SetClassViewBag(_user.ClassID);
            if (_role == null)
            {
                SetRolesViewBag();
            }
            else
            {
                SetRolesViewBag(_role.Id);
            }
            
            return View(_user);
        }


        [HttpPost]
        public ActionResult Edit(User user, int id, int? RoleID)
        {
            var _role = new RolesDAO().FindById(RoleID);
            var _userDao = new UserDAO();
            var _user = _userDao.GetUserById(id);

            if (ModelState.IsValid)
            {
                var _passWordHash = Encode.MD5Hash(user.PasswordHash);

                _user.PasswordHash = _passWordHash;
                _user.ConfirmPaswordHash = _passWordHash;
                
                var _result = _userDao.Update(_user, _role);
                if (_result == true)
                {
                    return RedirectToAction("Index", "user");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật người dùng không thành công.");
                }
            }
            else
            {
                SetClassViewBag(user.ClassID);
                SetRolesViewBag(RoleID);
            }
            return View("Edit");
        }

        //Lấy danh sách lớp
        public void SetClassViewBag(int? selectedID = null)
        {
            var _classDao = new ClassDAO();

            ViewBag.ClassID = new SelectList(_classDao.GetAll(), "Id", "Name", selectedID);

        }

        //Lấy danh sách quyền
        public void SetRolesViewBag(int? selectedID = null)
        {
            var _roleDao = new RolesDAO();

            ViewBag.RoleID = new SelectList(_roleDao.GetAll(), "Id", "Name", selectedID);
        }


    }
}