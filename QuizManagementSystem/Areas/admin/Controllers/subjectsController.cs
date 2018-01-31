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
using QuizManagementSystem.Common;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class subjectsController : baseController
    {

        // GET: admin/subjects
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var subjects = new SubjectDAO().GetAllSubjectsPageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(subjects);
        }

        // GET: admin/subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = new SubjectDAO().GetSubjectById(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: admin/subjects/Create
        public ActionResult Create()
        {
            SetSchoolYearViewBag();
            SetGradeViewBag();
            return View();
        }

        // POST: admin/subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Note,Status,GradeID")] Subject subject)
        {
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (CheckInputSubject(subject))
            {
                var _subjectDao = new SubjectDAO();
                if (ModelState.IsValid)
                {
                    var _result = _subjectDao.Insert(subject);
                    if (_result > 0)
                    {
                        SetAlert("Thêm môn học thành công", "success");
                        new SystemLogDAO().Insert("Tạo môn học thành công [" + subject.Name + "] [Năm học: " + subject.Grade.SchoolYear.NameOfSchoolYear + "]", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                        return Redirect("/admin/subjects/details/" + _result);
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
            return View(subject);
        }

        // GET: admin/subjects/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = new SubjectDAO().GetSubjectById(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            SetGradeViewBag(subject.GradeID);
            SetSchoolYearViewBag(subject.Grade.SchoolYear.Id);
            return View(subject);
        }

        // POST: admin/subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Note,Status,GradeID")] Subject subject)
        {
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (CheckInputSubject(subject))
            {
                if (ModelState.IsValid)
                {
                    var _result = new SubjectDAO().Update(subject);
                    if (_result)
                    {
                        SetAlert("Cập nhật thông tin môn học thành công", "success");
                        new SystemLogDAO().Insert("Cập nhật môn học thành công [" + subject.Name + "] [Năm học: " + subject.Grade.SchoolYear.NameOfSchoolYear + "]", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                        return Redirect("/admin/subjects/details/" + subject.Id);
                    }
                    else
                    {
                        SetAlert("Cập nhật thông tin môn học không thành công", "warning");
                        return Redirect("/admin/subjects");
                    }
                }
            }          
            return View(subject);
        }

        // GET: admin/subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = new SubjectDAO().GetSubjectById(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: admin/subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var subject = new SubjectDAO().GetSubjectById(id);
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            var _result = new SubjectDAO().Delete(id);
            if (_result == 0)
            {
                SetAlert("Xóa môn học thành công", "success");
                new SystemLogDAO().Insert("Xóa môn học thành công [" + subject.Name + "] [Năm học: " + subject.Grade.SchoolYear.NameOfSchoolYear + "]", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
            }
            else if (_result == 1)
            {
                SetAlert("Lỗi: môn học tồn tại đề thi. Không thể xóa môn học này.", "warning");
            }
            else if (_result == 2)
            {
                SetAlert("Lỗi: môn học tồn tại câu hỏi. Không thể xóa môn học này.", "warning");
            }
            return RedirectToAction("Index");
        }

        private bool CheckInputSubject(Subject subject)
        {
            if (subject.GradeID <= 0 || subject.GradeID == null)
            {
                ModelState.AddModelError("", "Vui lòng chọn khối lớp");
                return false;
            }
            if (String.IsNullOrEmpty(subject.Name))
            {
                ModelState.AddModelError("", "Tên môn học không được để trống");
                return false;
            }
            else
            {
                var rs = new SubjectDAO().IsExistNameSubject(subject);
                if (rs)
                {
                    ModelState.AddModelError("", "Tên môn học đã tồn tại, vui lòng kiểm tra lại.");
                    return false;
                }
            }

            return true;
        }

        private void SetSchoolYearViewBag(int? selectedID = null)
        {
            ViewBag.SchoolYearID = new SelectList(new SchoolYearDAO().GetAll(), "Id", "NameOfSchoolYear", selectedID);
        }

        private void SetGradeViewBag(int? selectedID = null)
        {
            ViewBag.GradeID = new SelectList(new GradeDAO().GetAll(), "Id", "GradeName", selectedID);
        }

        public JsonResult FillGrades(int? schoolyearId)
        {
            var grades = new GradeDAO().GetAllBySchoolYear(schoolyearId);

            return Json(new SelectList(grades.ToArray(), "Id", "GradeName"), JsonRequestBehavior.AllowGet);
        }

    }
}
