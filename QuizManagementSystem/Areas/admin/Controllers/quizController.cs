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
using QuizManagementSystem.Controllers;

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

        [HasCredential(RoleID = "QUIZ_HOME")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var _quizDao = new QuizDAO();
            var _model = _quizDao.GetAllQuizPageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(_model);
        }

        // GET: admin/quiz/Details/10
        [HasCredential(RoleID = "QUIZ_DETAIL")]
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
        [HasCredential(RoleID = "QUIZ_CREATE")]
        public ActionResult Create()
        {
            SetSubjectViewBag();
            SetGradeViewBag();
            SetCategoryViewBag();
            SetKindViewBag();
            SetLevelViewBag();
            SetAnswerChoiceNumViewBag();

            return View();
        }

        // POST: admin/quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HasCredential(RoleID = "QUIZ_CREATE")]
        public ActionResult Create([Bind(Include = "Id,SubjectsID,CategoryID,KindID,LevelID,ContentQuestion,AnswerText,KeyAnswer,UserID,DateCreated,Status,AnswerList,AnswerKey")] Question question)
        {
            var _quizDao = new QuizDAO();
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (CheckInputQuiz(question))
            {
                if (ModelState.IsValid)
                {
                    var _session = Session["USER_SESSION"] as UserLogin;
                    if (_session != null)
                    {
                        question.UserID = _session.UserID;
                    }
                    // Nội dung câu hỏi
                    if (!String.IsNullOrEmpty(question.ContentQuestion))
                    {
                        question.ContentQuestionEncode = WebUtility.HtmlDecode(Encode.StripHTML(question.ContentQuestion));
                    }
                 
                    if (!String.IsNullOrEmpty(question.AnswerText))
                    {
                        question.AnswerTextEncode = WebUtility.HtmlDecode(Encode.StripHTML(question.AnswerText));
                    }                  

                    question.DateCreated = DateTime.Now;    //Ngày tạo
                    question.ModifiedDate = question.DateCreated;   // Ngày chỉnh sửa
                    _quizDao.Insert(question);      // Gọi phương thức để tạo câu hỏi
                    new SystemLogDAO().Insert("Tạo câu hỏi thành công [ID = " + question.Id + "] [ID môn học: " + question.SubjectsID + "]", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                    return RedirectToAction("Index", "quiz");
                }
            }
            SetGradeViewBag(question);
            SetCategoryViewBag(question.CategoryID);
            SetKindViewBag(question.KindID);
            SetLevelViewBag(question.LevelID);
            SetAnswerChoiceNumViewBag(question.AnswerChoiceNum);
            SetSubjectViewBag(question.SubjectsID);
            
            return View();
        }

        // GET: admin/quiz/Edit
        [HttpGet]
        [HasCredential(RoleID = "QUIZ_EDIT")]
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

            SetGradeViewBag(_quiz);
            SetSubjectViewBag(_quiz.SubjectsID);
            SetCategoryViewBag(_quiz.CategoryID);
            SetKindViewBag(_quiz.KindID);
            SetLevelViewBag(_quiz.LevelID);
            SetAnswerChoiceNumViewBag(_quiz.AnswerChoiceNum);

            return View(_quiz);
        }

        // POST: admin/quiz/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HasCredential(RoleID = "QUIZ_EDIT")]
        public ActionResult Edit([Bind(Include = "Id,SubjectsID,CategoryID,KindID,LevelID,ContentQuestion,AnswerText,KeyAnswer,Status")] Question question)
        {
            if (ModelState.IsValid)
            {
                var _session = Session["USER_SESSION"] as UserLogin;
                var _quizDao = new QuizDAO();
                question.ContentQuestionEncode = Encode.StripHTML(question.ContentQuestion);
                question.AnswerTextEncode = Encode.StripHTML(question.AnswerText);
                question.KeyAnswer = question.KeyAnswer.ToUpper();
                
                if (_session != null)
                {
                    question.ModifiedBy = _session.UserName;
                }

                question.ModifiedDate = DateTime.Now;

                var _result = _quizDao.Update(question);

                if (_result == true)
                {
                    new SystemLogDAO().Insert("Sửa câu hỏi thành công [ID = " + question.Id + "] [ID môn học: " + question.SubjectsID + "]", _session.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                    return Redirect("/admin/quiz/details/" + question.Id);
                }

            }
            SetSubjectViewBag(question.SubjectsID);
            SetGradeViewBag(question);
            SetCategoryViewBag(question.CategoryID);
            SetKindViewBag(question.KindID);
            SetLevelViewBag(question.LevelID);
            SetAnswerChoiceNumViewBag(question.AnswerChoiceNum);

            return View(question);
        }

        // GET: admin/quiz/Delete
        [HttpGet]
        [HasCredential(RoleID = "QUIZ_DELETE")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = new QuizDAO().GetById(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: admin/quiz/Delete/10
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "QUIZ_DELETE")]
        public ActionResult DeleteConfirmed(int id)
        {
            var _quizDao = new QuizDAO();
            var _quiz = _quizDao.GetById(id);
            var _session = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (_session != null)
            {
                if (_quiz.UserID == _session.UserID)
                {
                    var _result = _quizDao.Delete(_quiz);
                    if (_result)
                    {
                        SetAlert("Xóa câu hỏi thành công", "success");
                        new SystemLogDAO().Insert("Xóa câu hỏi thành công [ID = " + _quiz.Id + "] [ID môn học: " + _quiz.SubjectsID + "]", _session.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                        return Redirect("/admin/quiz");
                    }
                    else
                    {
                        SetAlert("Xóa câu hỏi không thành công | Lỗi: Câu hỏi này đã được dùng trong đề thi.", "warning");
                        return Redirect("/admin/quiz");
                    }
                }
                else
                {
                    SetAlert("Xóa câu hỏi không thành công | Lỗi: Bạn không có quyền xóa câu hỏi này", "warning");
                    return Redirect("/admin/quiz");
                }
            }
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

        public void SetGradeViewBag(Question quiz)
        {
            var _sub = new SubjectDAO().GetSubjectById(quiz.SubjectsID);
            if (_sub == null)
            {
                ViewBag.GradeID = new SelectList(new GradeDAO().GetAll(), "Id", "GradeName", null);
            }
            else
            {
                ViewBag.GradeID = new SelectList(new GradeDAO().GetAll(), "Id", "GradeName", _sub.GradeID);
            }
        }

        public void SetGradeViewBag(int? selectedID = null)
        {
            ViewBag.GradeID = new SelectList(new GradeDAO().GetAll(), "Id", "GradeName", selectedID);
        }

        public void SetAnswerChoiceNumViewBag(int? selectedID = null)
        {
            ViewBag.AnswerChoiceNum = new SelectList(new List<SelectListItem>
            {
                    new SelectListItem{ Text = "2", Value = "2"},
                    new SelectListItem{ Text = "3", Value = "3"},
                    new SelectListItem{ Text = "4", Value = "4"},
                    new SelectListItem{ Text = "5", Value = "5"},
                    new SelectListItem{ Text = "6", Value = "6"},
                    new SelectListItem{ Text = "7", Value = "7"},
                    new SelectListItem{ Text = "8", Value = "8"},
                    new SelectListItem{ Text = "9", Value = "9"},
                    new SelectListItem{ Text = "10", Value = "10"}
            }, "Value", "Text", selectedID);
        }

        //Lấy danh sách môn học theo lớp đã chọn
        //Output: Json
        public JsonResult FillSubjects(int? classId)
        {
            var _subDao = new SubjectDAO();
            var subjects = _subDao.GetSubjectByClassID(classId);
            if (subjects == null)
            {
                return Json(new SelectList(null, "Id", "Name"), JsonRequestBehavior.AllowGet);
            }
            return Json(new SelectList(subjects.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        private bool CheckInputQuiz(Question question)
        {
            char[] Alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' }; // 10 đáp án

            // Kiểm tra có chọn lớp, môn học hay chưa?
            if (question.SubjectsID <= 0 || question.SubjectsID == null)
            {
                ModelState.AddModelError("", "Vui lòng chọn môn học");
                return false;
            }

            //Kiểm tra nội dung câu hỏi, đáp án câu hỏi và đáp án đúng nhập vào có hợp lệ không
            if (String.IsNullOrEmpty(question.ContentQuestion))
            {
                ModelState.AddModelError("", "Nội dung câu hỏi không được để trống.");
                return false;
            }

            if (question.AnswerList == null)
            {
                ModelState.AddModelError("", "Lỗi: Số đáp án lựa chọn tối thiểu là 2 đáp án");
                return false;
            }
            else if (question.AnswerList.Count < 2)
            {
                ModelState.AddModelError("", "Lỗi: Số đáp án lựa chọn tối thiểu là 2 đáp án");
                return false;
            }
            else
            {
                // Đáp án lựa chọn
                for (int i = 0; i < question.AnswerList.Count; i++)
                {
                    if (String.IsNullOrEmpty(question.AnswerList[i]))
                    {
                        ModelState.AddModelError("", "Vui lòng nhập đầy đủ nội dung của " + i + 1 + "đáp án.");
                        return false;
                    }
                    question.AnswerText += Alphabet[i] + ". " + question.AnswerList[i] + "\r\n";
                }
            }
           
            if (question.AnswerKey == null)
            {
                ModelState.AddModelError("", "Vui lòng nhập vào đáp án đúng cho câu hỏi này");
                return false;
            }
            else if (question.AnswerKey.Count < 1)
            {
                ModelState.AddModelError("", "Vui lòng nhập vào đáp án đúng cho câu hỏi này");
                return false;
            }
            else
            {
                // Đáp án đúng (Key)
                for (int i = 0; i < question.AnswerKey.Count; i++)
                {
                    if (Int32.Parse(question.AnswerKey[i]) > 0)
                    {
                        question.KeyAnswer += Alphabet[Int32.Parse(question.AnswerKey[i])];
                    }
                }
            }

            return true;
        }
    }
}
