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

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class quizController : Controller
    {
        

        // GET: admin/quiz
        public ActionResult Index()
        {
            var _quizDao = new QuizDAO();
            var _model = _quizDao.GetAllQuiz();
            return View(_model);
        }

        // GET: admin/quiz/Details/10
        public ActionResult Details(int? id)
        {
            var _quizDao = new QuizDAO();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question quiz = _quizDao.FindQuizById(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // GET: admin/quiz/Create
        public ActionResult Create()
        {
            SetCategoryViewBag();
            SetKindViewBag();
            SetLevelViewBag();
            SetUserViewBag();
            SetSubjectViewBag();
            SetClassViewBag();

            return View();
        }

        // POST: admin/quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubjectsID,CategoryID,KindID,LevelID,ContentQuestion,AnswerID,UserID,DateCreated,Status")] Question question)
        {
            var _quizDao = new QuizDAO();
            if (ModelState.IsValid)
            {
                _quizDao.Insert(question);               
                return RedirectToAction("Index", "quiz");
            }

            SetCategoryViewBag(question.CategoryID);
            SetKindViewBag(question.KindID);
            SetLevelViewBag(question.LevelID);
            SetUserViewBag(question.UserID);
            SetSubjectViewBag(question.SubjectsID);
            SetClassViewBag(question);
            return View(question);
        }

        // GET: admin/quiz/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _quizDao = new QuizDAO();
            var _quiz = _quizDao.FindQuizById(id);

            if (_quiz == null)
            {
                return HttpNotFound();
            }

            SetCategoryViewBag(_quiz.CategoryID);
            SetKindViewBag(_quiz.KindID);
            SetLevelViewBag(_quiz.LevelID);
            SetUserViewBag(_quiz.UserID);
            return View(_quiz);
        }

        // POST: admin/quiz/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubjectsID,CategoryID,KindID,LevelID,ContentQuestion,AnswerID,UserID,DateCreated,Status")] Question question)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(question).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.CategoryID = new SelectList(db.CategoryQuizs, "Id", "Name", question.CategoryID);
            //ViewBag.KindID = new SelectList(db.Kinds, "Id", "Name", question.KindID);
            //ViewBag.LevelID = new SelectList(db.Levels, "Id", "Name", question.LevelID);
            //ViewBag.UserID = new SelectList(db.Users, "Id", "UserName", question.UserID);
            return View(question);
        }

        // GET: admin/quiz/Delete
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Question question = db.Questions.Find(id);
            //if (question == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // POST: admin/quiz/Delete/10
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Question question = db.Questions.Find(id);
            //db.Questions.Remove(question);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }
  

        //Viewbag Catgeory
        public void SetCategoryViewBag(int? selectedID = null)
        {
            var _category = new CategoryQuizDAO();

            ViewBag.CategoryID = new SelectList(_category.GetAllCategoryQuiz(), "Id", "Name", selectedID);
        }

        //Viewbag Kind
        public void SetKindViewBag(int? selectedID = null)
        {
            var _kind = new KindQuizDAO();

            ViewBag.KindID = new SelectList(_kind.GetAllKindQuiz(), "Id", "Name", selectedID);
        }

        //Viewbag Level
        public void SetLevelViewBag(int? selectedID = null)
        {
            var _level = new LevelQuizDAO();

            ViewBag.LevelID = new SelectList(_level.GetAllLevelQuiz(), "Id", "Name", selectedID);
        }

        //Viewbag User
        public void SetUserViewBag(int? selectedID = null)
        {
            var _user = new UserDAO();

            ViewBag.UserID = new SelectList(_user.GetAllUser(), "Id", "UserName", selectedID);
        }

        //Viewbag Subjects
        public void SetSubjectViewBag(int? selectedID = null)
        {
            var _sub = new SubjectDAO();

            ViewBag.SubjectsID = new SelectList(_sub.GetAllSubjects(), "Id", "Name", selectedID);
        }

        //Lấy danh sách lớp
        public void SetClassViewBag(Question quiz)
        {
            var _classDao = new ClassDAO();
            var _subjectDao = new SubjectDAO();
            var _sub = _subjectDao.GetSubjectById(quiz.SubjectsID);

            ViewBag.ClassID = new SelectList(_classDao.GetAll(), "Id", "Name", _sub.ClassID);


        }

        public void SetClassViewBag()
        {
            var _classDao = new ClassDAO();

            ViewBag.ClassID = new SelectList(_classDao.GetAll(), "Id", "Name");

        }

        //Lấy danh sách môn học theo lớp đã chọn
        //Output: Json
        public JsonResult FillSubjects(int? classId)
        {
            var _subDao = new SubjectDAO();

            var subjects = _subDao.GetSubjectByClassID(classId);

            return Json(new SelectList(subjects.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

    }
}
