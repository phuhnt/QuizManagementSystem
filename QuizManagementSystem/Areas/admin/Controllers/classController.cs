using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class classController : baseController
    {
        
        // GET: admin/class
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var _classDao = new ClassDAO();
            var _class = _classDao.GetAllClassPageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(_class);
        }

        // GET: admin/class/Details/5
        public ActionResult Details(string searchString, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _classDao = new ClassDAO();
            Class _class = _classDao.GetClassById(id);
            if (_class == null)
            {
                return HttpNotFound();
            }
            var _userDao = new UserDAO();
            ViewBag.UserClass = _userDao.GetAllUserByClass(searchString, _class);
            ViewBag.SearchString = searchString;
            return View(_class);
        }

        // GET: admin/class/Create
        [HttpGet]
        public ActionResult Create()
        {
            SetSchoolYearViewBag();
            SetGradeViewBag();
            return View();
        }

        // POST: admin/class/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Class _class)
        {
            var _classDao = new ClassDAO();
            if (CheckInputClass(_class))
            {
                if (ModelState.IsValid)
                {
                    var _result = _classDao.Insert(_class);
                    if (_result > 0)
                    {
                        SetAlert("Thêm lớp học thành công", "success");
                        return Redirect("/admin/class/details/" + _result);
                    }
                    else
                    {
                        SetAlert("Thêm lớp học không thành công", "danger");
                        return RedirectToAction("Index");
                    }
                }               
            }
        
            SetSchoolYearViewBag();
            SetGradeViewBag();
            return View(_class);
        }

        // GET: admin/class/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class _class = new ClassDAO().GetClassById(id);
            if (_class == null)
            {
                return HttpNotFound();
            }          
            SetGradeViewBag(_class.GradeID);
            SetSchoolYearViewBag(_class.Grade.SchoolYear.Id);
            return View(_class);
        }

        // POST: admin/class/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,GradeID,Status,SchoolYearID")] Class _class)
        {
            if (CheckInputClass(_class))
            {
                if (ModelState.IsValid)
                {
                    var _result = new ClassDAO().Update(_class);
                    if (_result)
                    {
                        SetAlert("Cập nhật thông tin lớp học thành công", "success");
                        return Redirect("/admin/class/details/" + _class.Id);
                    }
                    else
                    {
                        SetAlert("Cập nhật thông tin lớp học không thành công", "warning");
                        return Redirect("/admin/class");
                    }
                }
            }
            
            return View(_class);
        }

        // GET: admin/class/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class _class = new ClassDAO().GetClassById(id);
            if (_class == null)
            {
                return HttpNotFound();
            }
            return View(_class);
        }

        // POST: admin/class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var _result = new ClassDAO().Delete(id);
            if (_result)
            {
                SetAlert("Xóa lớp học thành công", "success");
            }
            else
            {
                SetAlert("Lỗi: lớp học tồn tại học sinh. Không thể xóa lớp học này.", "warning");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            //base.Dispose(disposing);
        }

        private bool CheckInputClass(Class _class)
        {
            var _classDao = new ClassDAO();
            if (_class.GradeID <= 0 || _class.GradeID == null)
            {
                ModelState.AddModelError("", "Vui lòng chọn năm học");
                ModelState.AddModelError("", "Vui lòng chọn khối lớp");
                return false;
            }
            if (String.IsNullOrEmpty(_class.Name))
            {
                ModelState.AddModelError("", "Tên lớp không được để trống");
                return false;
            }
            else
            {
                if (_classDao.IsExistNameClass(_class))
                {
                    ModelState.AddModelError("", "Tên lớp đã tồn tại");
                    return false;
                }
            }
            return true;
        }



        private void SetGradeViewBag(int? selectedID = null)
        {
            ViewBag.GradeID = new SelectList(new GradeDAO().GetAll(), "Id", "GradeName", selectedID);
        }

        private void SetSchoolYearViewBag(int? selectedID = null)
        {
            ViewBag.SchoolYearID = new SelectList(new SchoolYearDAO().GetAll(), "Id", "NameOfSchoolYear", selectedID);
        }

        //public void SetSchoolYearViewBag(Class c)
        //{

        //    var _grade = new GradeDAO().GetByClass(c);
        //    if (_grade == null)
        //    {
        //        ViewBag.SchoolYearID = new SelectList(new SchoolYearDAO().GetAll(), "Id", "NameOfSchoolYear");
        //    }
        //    else
        //    {
        //        ViewBag.SchoolYearID = new SelectList(new SchoolYearDAO().GetAll(), "Id", "NameOfSchoolYear", _grade.SchoolYearID);
        //    }

        //}

        public JsonResult FillGrades(int? schoolyearId)
        {
            var grades = new GradeDAO().GetAllBySchoolYear(schoolyearId);

            return Json(new SelectList(grades.ToArray(), "Id", "GradeName"), JsonRequestBehavior.AllowGet);
        }
    }
}
