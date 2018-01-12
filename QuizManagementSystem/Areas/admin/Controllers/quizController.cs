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
using QuizManagementSystem.Common;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class quizController : baseController
    {


        // GET: admin/quiz
        //public ActionResult Index(int page = 1, int pageSize = 10)
        //{
        //    var _quizDao = new QuizDAO();
        //    var _model = _quizDao.GetAllQuizPageList();
        //    return View(_model);
        //}

        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var _quizDao = new QuizDAO();
            var _model = _quizDao.GetAllQuizPageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
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
            SetSubjectViewBag();
            SetClassViewBag();

            return View();
        }

        // POST: admin/quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        //[ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,SubjectsID,CategoryID,KindID,LevelID,ContentQuestion,AnswerText,KeyAnswer,UserID,DateCreated,Status")] Question question)
        {
            var _quizDao = new QuizDAO();

            CheckInputQuiz(question);
            
            if (ModelState.IsValid)
            {
                var _session = Session["USER_SESSION"] as UserLogin;
                if (_session != null)
                {
                    question.UserID = _session.UserID;
                }
                question.ContentQuestionEncode = Encode.StripHTML(question.ContentQuestion);
                question.AnswerTextEncode = Encode.StripHTML(question.AnswerText);
                question.KeyAnswer = question.KeyAnswer.ToUpper();
                question.DateCreated = DateTime.Now;
                question.ModifiedDate = question.DateCreated;
                _quizDao.Insert(question);               
                return RedirectToAction("Index", "quiz");
            }

            SetCategoryViewBag(question.CategoryID);
            SetKindViewBag(question.KindID);
            SetLevelViewBag(question.LevelID);
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

            //var _subDao = new SubjectDAO();
            //var _subj = _subDao.GetSubjectById(_quiz.SubjectsID);

            //var _classDao = new ClassDAO();
            //var _class = _classDao.GetClassById(_subj.ClassID);

            if (_quiz == null)
            {
                return HttpNotFound();
            }

            SetClassViewBag(_quiz);
            SetSubjectViewBag(_quiz.SubjectsID);
            SetCategoryViewBag(_quiz.CategoryID);
            SetKindViewBag(_quiz.KindID);
            SetLevelViewBag(_quiz.LevelID);
                     
            return View(_quiz);
        }

        // POST: admin/quiz/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,SubjectsID,CategoryID,KindID,LevelID,ContentQuestion,AnswerText,KeyAnswer,UserID,DateCreated,Status")] Question question)
        {
            if (ModelState.IsValid)
            {
                var _session = Session["USER_SESSION"] as UserLogin;
                var _quizDao = new QuizDAO();

                //question.ContentQuestionEncode = HttpContext.Server.HtmlEncode(question.ContentQuestion);
                //question.AnswerTextEncode = HttpContext.Server.HtmlEncode(question.AnswerText);
                question.ContentQuestionEncode = Common.Encode.StripHTML(question.ContentQuestion);
                question.AnswerTextEncode = Common.Encode.StripHTML(question.AnswerText);
                question.KeyAnswer = question.KeyAnswer.ToUpper();

                if (_session != null)
                {
                    question.ModifiedBy = _session.UserName;
                }
                
                question.ModifiedDate = DateTime.Now;

                var _result =  _quizDao.Update(question);

                if (_result == true)
                {
                    //return RedirectToAction("Index", "quiz");
                    return RedirectToAction("details/"+question.Id);
                }
                
            }
            SetCategoryViewBag(question.CategoryID);
            SetKindViewBag(question.KindID);
            SetLevelViewBag(question.LevelID);
            SetSubjectViewBag(question.SubjectsID);
            SetClassViewBag(question);
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

        private void CheckInputQuiz(Question question)
        {
            // Kiểm tra có chọn lớp, môn học hay chưa?
            if (question.SubjectsID == 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn môn học");
            }

            //Kiểm tra nội dung câu hỏi, đáp án câu hỏi và đáp án đúng nhập vào có hợp lệ không
            if (String.IsNullOrEmpty(question.ContentQuestion))
            {
                ModelState.AddModelError("", "Nội dung câu hỏi không được để trống.");
            }

            if (String.IsNullOrEmpty(question.AnswerText))
            {
                ModelState.AddModelError("", "Đáp án lựa chọn cho câu hỏi không được để trống.");
            }

            if (String.IsNullOrEmpty(question.KeyAnswer))
            {
                ModelState.AddModelError("", "Vui lòng nhập vào đáp án đúng cho câu hỏi này");
            }
            else
            {
                if (question.KeyAnswer.Length == 1)
                {
                    if (!Regex.IsMatch(question.KeyAnswer, "[A-Za-z0-9]{1}"))
                    {
                        ModelState.AddModelError("", "Đáp án đúng cho câu hỏi này không hợp lệ.");
                    }
                }
                else
                {
                    if (!Regex.IsMatch(question.KeyAnswer, "[A-Za-z0-9]{1}[,]"))
                    {
                        ModelState.AddModelError("", "Đáp án đúng cho câu hỏi này không hợp lệ.");
                    }
                }
            }
        }
    }
}
