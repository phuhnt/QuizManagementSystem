using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
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
            SetExamViewBag();
            SetExamineeViewBag();
            SetSchoolYearViewBag();
            SetGradeViewBag();
            SetSubjectViewBag();
            SetScoreLadderViewBag();
            return View();
        }

        // POST: admin/tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CodeTestID,Title,SubjectID,NumberOfQuestions,Time,NumberOfTurns,ExamID,ScoreLadderID,TestDay,StartTime,EndTime,UserID,Status")] Test test)
        {
            if (CheckInputTest(test))
            {
                if (ModelState.IsValid)
                {
                    db.Tests.Add(test);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
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

        private void SetExamViewBag(int? selectedID = null)
        {
            ViewBag.ExamID = new SelectList(new ExamDAO().GetAllExamActive(), "Id", "Titile", selectedID);
        }

        private void SetSchoolYearViewBag(int? selectedID = null)
        {
            ViewBag.SchoolYearID = new SelectList(new SchoolYearDAO().GetAll(), "Id", "NameOfSchoolYear", selectedID);
        }

        private void SetGradeViewBag(int? selectedID = null)
        {
            ViewBag.GradeID = new SelectList(new GradeDAO().GetAll(), "Id", "GradeName", selectedID);
        }

        private void SetExamineeViewBag(int? selectedID = null)
        {
            ViewBag.ExamineeID = new SelectList(new ClassDAO().GetAllClassActive(), "Id", "Name", selectedID);
        }

        private void SetSubjectViewBag(int? selectedID = null)
        {
            ViewBag.SubjectID = new SelectList(new SubjectDAO().GetAllSubjects(), "Id", "Name", selectedID);
        }

        private void SetScoreLadderViewBag(int? selectedID = null)
        {
            ViewBag.ScoreLadderID = new SelectList(new ScoreLadderDAO().GetAll(), "Id", "Title", selectedID);
        }

        public JsonResult FillExaminee(int? examsId)
        {
            var classes = new ClassDAO().GetAllByExams(examsId);
            return Json(new SelectList(classes.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillGrades(int? schoolyearId)
        {
            var grades = new GradeDAO().GetAllBySchoolYear(schoolyearId);
            return Json(new SelectList(grades.ToArray(), "Id", "GradeName"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillSubjects(int? gradeId)
        {
            var subj = new SubjectDAO().GetAllSubjectsByGrade(gradeId);
            return Json(new SelectList(subj.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        private bool CheckInputTest(Test test)
        {
            if (test.ExamID <= 0 || test.ExamID == null)
            {
                ModelState.AddModelError("", "Vui lòng chọn kỳ thi");
                return false;
            }
            if (test.SubjectID <= 0 || test.SubjectID == null)
            {
                ModelState.AddModelError("", "Vui lòng chọn môn thi");
                return false;
            }
            if (test.NumberOfQuestions <= 0)
            {
                ModelState.AddModelError("", "Số câu hỏi không hợp lệ");
                return false;
            }
            if (test.Time <= 0)
            {
                ModelState.AddModelError("", "Thời gian làm bài không hợp lệ");
                return false;
            }
            if (test.NumberOfTurns <= 0)
            {
                ModelState.AddModelError("", "Số lượt làm bài không hợp lệ");
                return false;
            }
            if (test.Status == true)
            {
                if (test.FromDate == null)
                {
                    ModelState.AddModelError("", "Vui lòng chọn ngày bắt đầu thi");
                    return false;
                }
                else if (test.ToDate == null)
                {
                    ModelState.AddModelError("", "Vui lòng chọn ngày kết thúc thi");
                    return false;
                }
                else
                {
                    if (test.ToDate < test.FromDate)
                    {
                        ModelState.AddModelError("", "Ngày kết thúc thi không thể nhỏ hơn ngày bắt đầu thi");
                        return false;
                    }
                }
                if (test.StartTime == null)
                {
                    ModelState.AddModelError("", "Vui lòng chọn giờ bắt đầu thi");
                    return false;
                }
                if (test.EndTime == null)
                {
                    ModelState.AddModelError("", "Vui lòng chọn giờ kết thúc thi");
                    return false;
                }
            }
            return true;
        }

        private void CheckCodeTest(Test test)
        {
            if (!String.IsNullOrEmpty(test.CodeTest))
            {
                List<int> _list = new List<int>();
                if (!Regex.IsMatch(test.CodeTest, "[A-Za-z0-9]") || !Regex.IsMatch(test.CodeTest, "[A-Za-z0-9][,]"))
                {
                    ModelState.AddModelError("", "Mã đề không hợp lệ");
                }

            }
        }

        // Chọn câu hỏi
        //private bool SelectQuiz(Test test)
        //{
        //    if (test.)
        //    return true;
        //}
    }
}

