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


        [HasCredential(RoleID = "EXAM_HOME")]
        // GET: admin/exams
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var _dao = new ExamDAO();
            var _model = _dao.GetAllExamPageList(searchString, page, pageSize);
            return View(_model);
        }

        [HasCredential(RoleID = "EXAM_DETAIL")]
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
            for (int i = 0; i < _listClass.Count; i++)
            {
                _selectedClassID[i] = _listClass[i].Id;
            }
            SetClassViewBag(_selectedClassID);
            return View(_exam);
        }


        // GET: admin/exams/Create
        [HttpGet]
        [HasCredential(RoleID = "EXAM_CREATE")]
        public ActionResult Create()
        {
            SetGradeViewBag();
            SetSubjectViewBag();
            SetSchoolYearViewBag();
            SetClassViewBag(null);
            return View();
        }

        // POST: admin/exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "EXAM_CREATE")]
        public ActionResult Create(Exam exam)
        {
            if (CheckInputExam(exam))
            {
                if (ModelState.IsValid)
                {
                    var _session = Session[ConstantVariable.USER_SESSION] as Common.UserLogin;
                    var _userDao = new UserDAO();
                    var _examDao = new ExamDAO();

                    if (_session != null)
                    {
                        exam.UserID = _userDao.GetUserByUserName(_session.UserName).Id;
                    }
                    if (!String.IsNullOrEmpty(exam.Note))
                    {
                        exam.NoteEncode = Encode.StripHTML(exam.Note);
                    }
                    exam.CreatedDate = DateTime.Now;

                    var _result = _examDao.Insert(exam, exam.SelectedClassID);
                    if (_result)
                    {
                        SetAlert("Thêm kỳ thi thành công", "success");
                        new SystemLogDAO().Insert("Thêm kỳ thi [" + exam.Titile + "]", _session.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                        return Redirect("/admin/exams/details/" + exam.Id);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm kỳ thi không thành công.");
                    }
                }
            }
            SetGradeViewBag();
            SetSubjectViewBag(exam.SubjectID);
            SetSchoolYearViewBag();
            SetUserViewBag(exam.UserID);
            SetClassViewBag(exam.SelectedClassID);
            return View(exam);
        }

        [HttpGet]
        [HasCredential(RoleID = "EXAM_EDIT")]
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
            SetGradeViewBag(_exam.Subject.GradeID);
            SetSubjectViewBag(_exam.SubjectID);
            SetSchoolYearViewBag(_exam.Subject.Grade.SchoolYearID);
            SetUserViewBag(_exam.UserID);

            return View(_exam);
        }

        // POST: admin/exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "EXAM_EDIT")]
        public ActionResult Edit(Exam exam)
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
                var _session = Session[ConstantVariable.USER_SESSION] as UserLogin;
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
                    new SystemLogDAO().Insert("Cập nhật kỳ thi [" + exam.Titile + "]", _session.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                    return Redirect("/admin/exams/details/" + exam.Id);
                }
                else
                {
                    SetAlert("Sửa kỳ thi không thành công", "warning");
                    return RedirectToAction("Index");
                }
            }

            SetGradeViewBag();
            SetSubjectViewBag(exam.SubjectID);
            SetSchoolYearViewBag();
            SetUserViewBag(exam.UserID);
            SetClassViewBag(exam.SelectedClassID);
            return View(exam);
        }

        // GET: admin/exams/Delete/5
        [HttpGet]
        [HasCredential(RoleID = "EXAM_DELETE")]
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
        [HasCredential(RoleID = "EXAM_DELETE")]
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
                    new SystemLogDAO().Insert("Xóa kỳ thi [" + exam.Titile + "]", _session.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                    RedirectToAction("Index", "exams");
                }
                else
                {
                    SetAlert("Xóa kỳ thi không thành công", "warning");
                    RedirectToAction("Index", "exams");
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
            var _timeToTakeUser = new TestResultDetailDAO().GetTimeToTake(_session.UserID, _exam.Id);
            if (_timeToTakeUser >= _exam.NumberOfTurns)
            {
                SetAlert("Bạn đã hết lượt làm bài của kỳ thi này.", "error");
                return Redirect("/");
            }

            if (_exam.ToDate < DateTime.Now)
            {
                SetAlert("Kỳ thi đã hết hạn.", "error");
                _exam.Status = false;
                new ExamDAO().UpdateStatus(_exam);

                return Redirect("/");
            }

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
                _codeTestID[i] = (int)_tests[i].CodeTest;
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

            if (_test.UserAnswer == null || _test.UserAnswer.Count <= 0)
            {
                _answerIgnoredNum = listQuiz.Count;
            }
            else
            {
                for (int i = 0; i < _test.UserAnswer.Count; i++)
                {

                    if (listQuiz[i].KeyAnswer == _test.UserAnswer[i])
                    {
                        _answerCorrectNum++;
                    }
                    else
                    {
                        _answerWrongNum++;
                    }              
                }
                _answerIgnoredNum = listQuiz.Count - (_answerCorrectNum + _answerWrongNum);
            }

            _testResult.Score = _answerCorrectNum * (_test.ScoreLadder.Score / _test.NumberOfQuestions);
            _testResult.NumberOfWrong = _answerWrongNum;
            _testResult.NumberOfCorrect = _answerCorrectNum;
            _testResult.NumberOfIgnored = _answerIgnoredNum;
            var _countTimeToTake = new TestResultDetailDAO().GetAll(_session.UserID, _test.ExamID);
            if (_countTimeToTake == null)
            {
                _testResult.TimeToTake = 0;
            }
            else
            {
                _testResult.TimeToTake =_countTimeToTake.Count + 1;
            }
            _testResult.ActualTestDate = DateTime.Now;
            _testResult.ActualStartTime = null;
            _testResult.ActualEndTime = DateTime.Now.TimeOfDay;
            for (int i = 0; i < listQuiz.Count; i++)
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
                var _testDb = new TestDAO().GetTestById(_testResult.TestID);
                new SystemLogDAO().Insert("Người dùng thi online [Kỳ thi: " + _testDb.Exam.Titile + "] [Môn thi: " + _testDb.Exam.Subject.Name + "] [Mã đề: " + _testDb.CodeTest + "]", _session.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                return Redirect("/admin/exams/resultdetail/" + _id);
            }
            return Redirect("/");
        }


        public ActionResult ResultDetail(int? id)
        {
            if (CheckSession() == false)
            {
                SetAlert("Truy cập không hợp lệ.", "error");
                return Redirect("/");
            }
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

        private void SetGradeViewBag(int? selectedID = null)
        {
            ViewBag.GradeID = new SelectList(new GradeDAO().GetAll(), "Id", "GradeName", selectedID);
        }

        private void SetSubjectViewBag(int? selectedID = null)
        {
            ViewBag.SubjectID = new SelectList(new SubjectDAO().GetAllSubjects(), "Id", "Name", selectedID);
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

        public bool CheckSession()
        {
            var session = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (session == null)
            {
                return false;
            }
            return true;
        }

        private bool CheckInputExam(Exam exam)
        {
            if (String.IsNullOrEmpty(exam.Titile))
            {
                ModelState.AddModelError("", "Tiêu đề kỳ thi không được để trống.");
                return false;
            }

            if (exam.SelectedClassID == null)
            {
                ModelState.AddModelError("", "Vui lòng chọn thí sinh cho kỳ thi này.");
                return false;
            }
            if (exam.SubjectID <= 0 || exam.SubjectID == null)
            {
                ModelState.AddModelError("", "Vui lòng chọn môn thi");
                return false;
            }
            if (exam.NumberOfTurns <= 0)
            {
                ModelState.AddModelError("", "Số lượt làm bài không hợp lệ");
                return false;
            }
            if (exam.Status == true)
            {
                if (exam.FromDate == null)
                {
                    ModelState.AddModelError("", "Vui lòng chọn ngày bắt đầu thi");
                    return false;
                }
                else if (exam.ToDate == null)
                {
                    ModelState.AddModelError("", "Vui lòng chọn ngày kết thúc thi");
                    return false;
                }
                else
                {
                    if (exam.ToDate < exam.FromDate)
                    {
                        ModelState.AddModelError("", "Ngày kết thúc thi không thể nhỏ hơn ngày bắt đầu thi");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
