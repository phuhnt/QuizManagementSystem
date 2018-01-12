using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class testsController : baseController
    {
        private QuizManagementSystemDbContext db = new QuizManagementSystemDbContext();

        // GET: admin/tests
        public ActionResult Index()
        {
            var tests = db.Tests.Include(t => t.Exam).Include(t => t.ScoreLadder).Include(t => t.Subject);
            return View(tests.ToList());
        }

        // GET: admin/tests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        // GET: admin/tests/Create
        public ActionResult Create()
        {
            ViewBag.ExamID = new SelectList(db.Exams, "Id", "Titile");
            ViewBag.ScoreLadderID = new SelectList(db.ScoreLadders, "Id", "Title");
            ViewBag.SubjectID = new SelectList(db.Subjects, "Id", "Name");
            return View();
        }

        // POST: admin/tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CodeTestID,Title,SubjectID,NumberOfQuestions,Time,NumberOfTurns,ExamID,ScoreLadderID,TestDay,StartTime,EndTime,UserID,Status")] Test test)
        {
            if (ModelState.IsValid)
            {
                db.Tests.Add(test);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExamID = new SelectList(db.Exams, "Id", "Titile", test.ExamID);
            ViewBag.ScoreLadderID = new SelectList(db.ScoreLadders, "Id", "Title", test.ScoreLadderID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "Id", "Name", test.SubjectID);
            return View(test);
        }

        // GET: admin/tests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExamID = new SelectList(db.Exams, "Id", "Titile", test.ExamID);
            ViewBag.ScoreLadderID = new SelectList(db.ScoreLadders, "Id", "Title", test.ScoreLadderID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "Id", "Name", test.SubjectID);
            return View(test);
        }

        // POST: admin/tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CodeTestID,Title,SubjectID,NumberOfQuestions,Time,NumberOfTurns,ExamID,ScoreLadderID,TestDay,StartTime,EndTime,UserID,Status")] Test test)
        {
            if (ModelState.IsValid)
            {
                db.Entry(test).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExamID = new SelectList(db.Exams, "Id", "Titile", test.ExamID);
            ViewBag.ScoreLadderID = new SelectList(db.ScoreLadders, "Id", "Title", test.ScoreLadderID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "Id", "Name", test.SubjectID);
            return View(test);
        }

        // GET: admin/tests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        // POST: admin/tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Test test = db.Tests.Find(id);
            db.Tests.Remove(test);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
