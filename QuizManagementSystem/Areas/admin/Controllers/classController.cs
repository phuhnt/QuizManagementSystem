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
using ClosedXML.Excel;
using System.IO;
using QuizManagementSystem.Common;

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
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            var _classDao = new ClassDAO();
            if (CheckInputClass(_class))
            {
                if (ModelState.IsValid)
                {
                    var _result = _classDao.Insert(_class);
                    if (_result > 0)
                    {
                        SetAlert("Thêm lớp học thành công", "success");
                        new SystemLogDAO().Insert("Thêm lớp học [" + new GradeDAO().GetByClass(_class).SchoolYear.NameOfSchoolYear + "]" + "[" + _class.Name + "]", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                        return Redirect("/admin/class/details/" + _result);
                    }
                    else
                    {
                        SetAlert("Thêm lớp học không thành công", "warning");
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
        public ActionResult Edit([Bind(Include = "Id,Name,GradeID,Status")] Class _class)
        {
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            Class c = new ClassDAO().GetClassById(_class.Id);
            if (CheckInputClass(_class))
            {
                if (ModelState.IsValid)
                {
                    var _result = new ClassDAO().Update(_class);
                    if (_result)
                    {
                        SetAlert("Cập nhật thông tin lớp học thành công", "success");
                        new SystemLogDAO().Insert("Sửa lớp học [" + c.Grade.SchoolYear.NameOfSchoolYear + "]" + "[" + _class.Name + "]", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                        return Redirect("/admin/class/details/" + _class.Id);
                    }
                    else
                    {
                        SetAlert("Cập nhật thông tin lớp học không thành công", "warning");
                        return Redirect("/admin/class");
                    }
                }
                else
                {
                    SetAlert("Cập nhật thông tin lớp học không thành công", "error");
                    return Redirect("/admin/class");
                }
            }
            SetGradeViewBag(_class.GradeID);
            SetSchoolYearViewBag(c.Grade.SchoolYear.Id);
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
            var _class = new ClassDAO().GetClassById(id);
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            var _result = new ClassDAO().Delete(id);
            if (_result)
            {
                SetAlert("Xóa lớp học thành công", "success");
                new SystemLogDAO().Insert("Xóa lớp học [" + _class.Grade.SchoolYear.NameOfSchoolYear + "]" + "[" + _class.Name + "]", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
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

        [HttpPost]
        public FileResult ExportToExcelFile(int? id)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[6] {
                new DataColumn("STT"),
                new DataColumn("Họ và tên"),
                new DataColumn("Giới tính"),
                new DataColumn("Ngày sinh"),
                new DataColumn("Email"),
                new DataColumn("Số điện thoại")
            });

            var _class = new ClassDAO().GetClassById(id);
            var _student = new UserDAO().GetAllUserByClass(id);
            
            for (int i = 0; i < _student.Count; i++)
            {
                dt.Rows.Add(i + 1, _student[i].FullName, _student[i].Sex, _student[i].DayOfBirth.Value.ToShortDateString(), _student[i].Email, _student[i].Phone);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
              
                wb.Worksheets.Add(dt, "" + _class.Name);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Danh sach lop " + _class.Name + ".xlsx");
                }
            }
        }

        private bool CheckInputClass(Class _class)
        {
            var _classDao = new ClassDAO();
            var c = _classDao.GetClassById(_class.Id);
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
                if (c != null)
                {
                    if (c.Id == _class.Id && c.Name == _class.Name)
                    {
                        return true;
                    }
                }  
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
