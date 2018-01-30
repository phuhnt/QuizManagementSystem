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
using QuizManagementSystem.Controllers;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class examsController : baseController
    {
        

        // GET: admin/exams
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var _dao = new ExamDAO();
            var _model = _dao.GetAllExamPageList(searchString, page, pageSize);
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

        [HttpGet]
        public ActionResult StartTheTest(int? id)
        {
            var _exam = new ExamDAO().GetExamById(id);
            var _session = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (_session == null)
            {
                SetAlert("Vui lòng đăng nhập để bắt đầu thi", "error");
                return Redirect("/");
            }
            else
            {
                var _user = new UserDAO().GetUserById(_session.UserID);
                var listClass = new ClassDAO().GetAllByExams(_exam.Id);
                if (!listClass.Exists(x => x.Id == _user.ClassID))
                {
                    SetAlert("Bạn không phải là thí sinh của kỳ thi này.", "error");
                    return Redirect("/");
                }
            }
            // Kiểm tra số lượt làm bài
            //var _testResult = new TestResultDetailDAO().GetTestResult(_session.UserID, )

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (_exam == null)
            {
                return HttpNotFound();
            }
            int[] _codeTestID = new int[_exam.Tests.Count]; // Danh sách các mã đề thi của kỳ thi
            var _tests = _exam.Tests.ToList();
            for (int i = 0; i < _exam.Tests.Count; i++)
            {
                _codeTestID[i] = _tests[i].CodeTest;
            }
            if (_codeTestID != null)
            {
                Random random = new Random();
                int selectCodeTest = random.Next(0, _codeTestID.Length);    // Chọn ngẫu nhiên một mã đề
                return View(_tests[selectCodeTest]);
            }

            return View(_exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StartTheTest(Test test)
        {
            var _test = new TestDAO().GetTestByCodeTest(test.CodeTest, test.ExamID);
            var _testResultDAO = new TestResultDetailDAO();
            var _testResult = new TestResultDetail();
            if (_test != null)
            {
                _test.UserAnswer = test.UserAnswer;
                _test.ExamID = test.ExamID;
                _test.CodeTest = test.CodeTest;
            }
            var _session = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (_session != null)
            {
                _testResult.UserID = _session.UserID;
            }
            _testResult.TestID = _test.Id;
            _testResult.ExamID = _test.ExamID;
            int _answerCorrectNum = 0;
            int _answerWrongNum = 0;
            int _answerIgnoredNum = 0;
            var listQuiz = new TestDAO().GetAllQuiz(_test.Id);
            for (int i = 0; i < listQuiz.Count; i++)
            {
                if (listQuiz[i].KeyAnswer == _test.UserAnswer[i])
                {
                    _answerCorrectNum++;
                }
                else if (listQuiz[i].KeyAnswer != _test.UserAnswer[i])
                {
                    _answerWrongNum++;
                }
                else
                {
                    _answerIgnoredNum++;
                }
            }
            _testResult.Score = (double)_answerCorrectNum * (_test.ScoreLadder.Score / _test.NumberOfQuestions);
            _testResult.NumberOfWrong = _answerWrongNum;
            _testResult.NumberOfCorrect = _answerCorrectNum;
            _testResult.NumberOfIgnored = _answerIgnoredNum;
            if (_testResult.TimeToTake == null)
            {
                _testResult.TimeToTake = 1;
            }
            else
            {
                _testResult.TimeToTake = _testResult.TimeToTake + 1;
            }
            _testResult.ActualTestDate = DateTime.Now;
            _testResult.ActualStartTime = null;
            _testResult.ActualEndTime = DateTime.Now.TimeOfDay;
            for(int i = 0; i < listQuiz.Count; i++)
            {
                if (i != listQuiz.Count - 1)
                {
                    _testResult.UserAnswer += _test.UserAnswer[i] + ",";                    
                }
                else
                {
                    _testResult.UserAnswer += _test.UserAnswer[i];
                }
            }
            int _id = _testResultDAO.Insert(_testResult);
            // Insert
            if (_id > 0)
            {
                return ResultDetail(_id);
            }
            return Redirect("/");
        }

        
        public ActionResult ResultDetail(int? id)
        {
            var _result = new TestResultDetailDAO().GetById(id);
            return View(_result);
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
