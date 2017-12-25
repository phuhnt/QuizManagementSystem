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
using QuizManagementSystem.Models;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class examsController : Controller
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
            return View(_exam);
        }

        // GET: admin/exams/Create
        [HttpGet]
        public ActionResult Create()
        {        
            return View();
        }

        // POST: admin/exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Titile,Note,Status,Link,UserID")] Exam exam)
        {
            if (String.IsNullOrEmpty(exam.Titile))
            {
                ModelState.AddModelError("", "Tiêu đề kỳ thi không được để trống.");
            }

            if (ModelState.IsValid)
            {
                var _session = Session["USER_SESSION"] as LoginModel;
                var _userDao = new UserDAO();
                var _examDao = new ExamDAO();
                if (_session != null)
                {
                    exam.UserID = _userDao.GetUserByUserName(_session.UserName).Id;
                }
                
                exam.CreatedDate = DateTime.Now;

                var _result = _examDao.Insert(exam);
                if (_result > 0)
                {
                    return RedirectToAction("/details/" + exam.Id);
                }
                else
                {
                    ModelState.AddModelError("", "Thêm kỳ thi không thành công.");
                }
            }

            SetUserViewBag(exam.UserID);
           
            return View(exam);
        }

        // GET: admin/exams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Exam exam = db.Exams.Find(id);
            //if (exam == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.UserID = new SelectList(db.Users, "Id", "UserName", exam.UserID);
            return View("exam");
        }

        // POST: admin/exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Titile,Note,Status,Link,UserID")] Exam exam)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(exam).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.UserID = new SelectList(db.Users, "Id", "UserName", exam.UserID);
            return View(exam);
        }

        // GET: admin/exams/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Exam exam = db.Exams.Find(id);
            //if (exam == null)
            //{
            //    return HttpNotFound();
            //}
            return View("exam");
        }

        // POST: admin/exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Exam exam = db.Exams.Find(id);
            //db.Exams.Remove(exam);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void SetUserViewBag(int? selectedID = null)
        {
            var _userDao = new UserDAO();

            ViewBag.UserID = new SelectList(_userDao.GetAllUser(), "Id", "UserName", selectedID);
        }
    }
}
