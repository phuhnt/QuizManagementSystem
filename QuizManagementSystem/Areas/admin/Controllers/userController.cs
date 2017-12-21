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
        public ActionResult Create(User user)
        {
            SetClassViewBag();
            SetRolesViewBag();
            var _dao = new UserDAO();

            // Kiểm tra username có tồn tại trong cơ sở dữ liệu chưa?
            if (_dao.IsUserNameExist(user.UserName) != null)
            {
                ModelState.AddModelError("", "Tên tài khoản đã tồn tại.");
            }
            if (ModelState.IsValid)
            {

                var _passWordHash = Encode.MD5Hash(user.PasswordHash);
                user.PasswordHash = _passWordHash;
                user.ConfirmPaswordHash = _passWordHash;
                user.DateOfParticipation = DateTime.Now;
                long _id = _dao.Insert(user);

                if (_id > 0)
                {
                    return RedirectToAction("Index", "user");
                }
                else
                {
                    ModelState.AddModelError("success", "Thêm người dùng thành công");
                }
            }
            return View("create");
        }

        /// <summary>
        /// Chỉnh sửa User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var _user = new UserDAO().GetUserById(id);
            return View(_user);
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

            ViewBag.Roles = new SelectList(_roleDao.GetAll(), "Id", "Name", selectedID);
        }

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            // This will result in MVC rendering each item as:
            //     <option value="State Name">State Name</option>
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }
    }
}