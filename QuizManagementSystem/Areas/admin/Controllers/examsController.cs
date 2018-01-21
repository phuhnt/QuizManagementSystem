using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;
using QuizManagementSystem.Common;
using QuizManagementSystem.Models;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class examsController : baseController
    {
        

        // GET: admin/exams
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var _dao = new ExamDAO();
            var _model = _dao.GetAllExamPageList(page, pageSize);
            return View(_model);
        }

        // GET: admin/exams/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _examDao = new ExamDAO();
            var _exam = _examDao.GetExamById(id);
            if (_exam == null)
            {
                return HttpNotFound();
            }
            SetUserViewBag(_exam.UserID);
            var _listClass = _examDao.GetClassSelected(_exam);
            int[] _selectedClassID = new int[_listClass.Count];
            for (int i=0; i < _listClass.Count; i++)
            {
                _selectedClassID[i] = _listClass[i].Id;
            }
            SetClassViewBag(_selectedClassID);
            return View(_exam);
        }

        // GET: admin/exams/Create
        [HttpGet]
        public ActionResult Create()
        {
            SetSchoolYearViewBag();
            SetClassViewBag(null);
            return View();
        }

        // POST: admin/exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exam exam)
        {
            if (String.IsNullOrEmpty(exam.Titile))
            {
                ModelState.AddModelError("", "Tiêu đề kỳ thi không được để trống.");
            }

            if (exam.SelectedClassID == null)
            {
                ModelState.AddModelError("", "Vui lòng chọn thí sinh cho kỳ thi này.");
            }

            if (ModelState.IsValid)
            {
                var _session = Session[Common.ConstantVariable.USER_SESSION] as Common.UserLogin;
                var _userDao = new UserDAO();
                var _examDao = new ExamDAO();
               
                if (_session != null)
                {
                    exam.UserID = _userDao.GetUserByUserName(_session.UserName).Id;
                }
                if (!String.IsNullOrEmpty(exam.Note))
                {
                    exam.NoteEncode = Common.Encode.StripHTML(exam.Note);
                }     
                exam.CreatedDate = DateTime.Now;

                var _result = _examDao.Insert(exam, exam.SelectedClassID);
                if (_result)
                {
                    SetAlert("Thêm kỳ thi thành công", "success");
                    return RedirectToAction("/details/" + exam.Id);
                }
                else
                {
                    ModelState.AddModelError("", "Thêm kỳ thi không thành công.");
                }
            }
            SetSchoolYearViewBag();
            SetUserViewBag(exam.UserID);
            SetClassViewBag(exam.SelectedClassID);
            return View(exam);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _examDao = new ExamDAO();
            var _exam = _examDao.GetExamById(id);
            if (_exam == null)
            {
                return HttpNotFound();
            }
            var _listClass = _examDao.GetClassSelected(_exam);
            int[] _selectedClassID = new int[_listClass.Count];
            for (int i = 0; i < _listClass.Count; i++)
            {
                _selectedClassID[i] = _listClass[i].Id;
            }
            _exam.SelectedClassID = _selectedClassID;
            SetClassViewBag(_exam.SelectedClassID);
            return View(_exam);
        }

        // POST: admin/exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Titile,Note,NoteEncode,SelectedClassID,Link,ModifiedBy,ModifiedDate,Status")] Exam exam)
        {
            if (String.IsNullOrEmpty(exam.Titile))
            {
                ModelState.AddModelError("", "Tiêu đề kỳ thi không được để trống.");
            }

            if (exam.SelectedClassID == null)
            {
                ModelState.AddModelError("", "Vui lòng chọn thí sinh cho kỳ thi này.");
            }
            if (ModelState.IsValid)
            {
                var _session = Session[Common.ConstantVariable.USER_SESSION] as Common.UserLogin;
                var _userDao = new UserDAO();
                var _examDao = new ExamDAO();

                if (_session != null)
                {
                    exam.ModifiedBy = _session.UserName;
                }

                exam.ModifiedDate = DateTime.Now;

                var _result = _examDao.Update(exam, exam.SelectedClassID);
                if (_result)
                {
                    SetAlert("Cập nhật thông tin kỳ thi thành công", "success");
                    return RedirectToAction("/details/" + exam.Id);
                }
                else
                {
                    SetAlert("Sửa kỳ thi không thành công", "warning");
                    return RedirectToAction("Index");
                }
            }
            SetSchoolYearViewBag();
            SetUserViewBag(exam.UserID);
            SetClassViewBag(exam.SelectedClassID);
            return View(exam);
        }

        // GET: admin/exams/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = new ExamDAO().GetExamById(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // POST: admin/exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exam exam = new ExamDAO().GetExamById(id);
            var _session = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (_session != null)
            {
                if (_session.UserID != exam.UserID && _session.UserID != 1)
                {
                    ModelState.AddModelError("", "Xóa kỳ thi không thành công. Lỗi: bạn không có quyền xóa kỳ thi này");
                }
            }
            if (exam.TestResultDetails.Count > 0)
            {
                ModelState.AddModelError("", "Xóa kỳ thi không thành công. Lỗi: kỳ thi này đã có người thi không thể xóa");
            }
            if (exam.Tests.Count > 0)
            {
                ModelState.AddModelError("", "Xóa kỳ thi không thành công. Lỗi: kỳ thi này có đề thi tham chiếu đến");
            }
            if (ModelState.IsValid)
            {
                var result = new ExamDAO().Delete(exam);
                if (result)
                {
                    SetAlert("Xóa kỳ thi thành công", "success");
                    RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Xóa kỳ thi không thành công", "warning");
                    RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public void SetUserViewBag(int? selectedID = null)
        {
            var _userDao = new UserDAO();

            ViewBag.UserID = new SelectList(_userDao.GetAllUser(), "Id", "UserName", selectedID);
        }

        public void SetClassViewBag(int[] selectedID)
        {
            var _classDao = new ClassDAO();
            ViewBag.SelectedClassID = new SelectList(_classDao.GetAllClass(), "Id", "Name", selectedID);
        }

        private void SetSchoolYearViewBag(int? selectedID = null)
        {
            ViewBag.SchoolYearID = new SelectList(new SchoolYearDAO().GetAll(), "Id", "NameOfSchoolYear", selectedID);
        }

        public JsonResult GetClassSelected(int? id)
        {
            var _examDao = new ExamDAO();
            var _classDao = new ClassDAO();

            var _exam = _examDao.GetExamById(id);
            var _listClass = _examDao.GetClassSelected(_exam);
            int[] _selectedClassID = new int[_listClass.Count];
            for (int i = 0; i < _listClass.Count; i++)
            {
                _selectedClassID[i] = _listClass[i].Id;
            }
            _exam.SelectedClassID = _selectedClassID;
            return Json(_exam.SelectedClassID, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillClasses(int? schoolyearId)
        {
            var classes = new ClassDAO().GetAllBySchoolYear(schoolyearId);            
            return Json(new SelectList(classes.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }
    }
}
