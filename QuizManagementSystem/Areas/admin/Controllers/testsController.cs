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
    public class testsController : baseController
    {

        // GET: admin/tests
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var testModel = new TestDAO().GetAllTestPageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(testModel);
        }

        // GET: admin/tests/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = new TestDAO().GetTestById(id);
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
            //SetSchoolYearViewBag();
            //SetGradeViewBag();
            SetSubjectViewBag();
            SetScoreLadderViewBag();
            return View();
        }

        // POST: admin/tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Test test)
        {
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (CheckInputTest(test))
            {
                if (ModelState.IsValid)
                {
                    if (SelectQuiz(test))
                    {
                        SetAlert("Tạo đề thi thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Tạo đề thi không thành công", "error");
                        return RedirectToAction("Index");
                    }
                }
            }

            SetExamViewBag();
            SetExamineeViewBag();
            //SetSchoolYearViewBag();
            //SetGradeViewBag();
            SetSubjectViewBag();
            SetScoreLadderViewBag();
            return View(test);
        }

        // GET: admin/tests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = new TestDAO().GetTestById(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            SetExamViewBag(test.ExamID);
            SetExamineeViewBag();
            //SetSchoolYearViewBag(test.Subject.Grade.SchoolYearID);
            //SetGradeViewBag(test.Subject.GradeID);
            //SetSubjectViewBag(test.SubjectID);
            SetScoreLadderViewBag(test.ScoreLadderID);
            return View(test);
        }

        // POST: admin/tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CodeTest,Title,Note,NumberOfQuestions,Time,ExamID,ScoreLadderID,Status")] Test test)
        {
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (CheckInputTest(test))
            {
                if (ModelState.IsValid)
                {
                    if (SelectQuiz(test))
                    {
                        SetAlert("Cập nhật đề thi thành công", "success");
                        new SystemLogDAO().Insert("Cập nhật đề thi thành công [Mã đề: " + test.CodeTest + "] [Kỳ thi: " + test.Exam.Titile + "]", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Cập nhật đề thi không thành công", "error");
                        return RedirectToAction("Index");
                    }
                }
            }
            SetExamViewBag(test.ExamID);
            SetExamineeViewBag();
            //SetSchoolYearViewBag(test.Subject.Grade.SchoolYearID);
            //SetGradeViewBag(test.Subject.GradeID);
            //SetSubjectViewBag(test.SubjectID);
            //SetScoreLadderViewBag(test.ScoreLadderID);
            return View();
        }

        // GET: admin/tests/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Test test = db.Tests.Find(id);
            //if (test == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // POST: admin/tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Test test = db.Tests.Find(id);
            //db.Tests.Remove(test);
            //db.SaveChanges();
            return RedirectToAction("Index");
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

        private void GetExamineeViewBag(int? examsId)
        {
            var classes = new ClassDAO().GetAllByExams(examsId);
            ViewBag.Examinee = classes;
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
            if (String.IsNullOrEmpty(test.CodeTestArr))
            {
                ModelState.AddModelError("", "Vui lòng nhập vào mã đề của đề thi. Nếu muốn tạo nhiều đề thi cùng một lúc, mỗi mã đề cách nhau dấu phẩy.");
                return false;
            }
            else
            {
                var _regex = new Regex(@"[0-9,]");
                var _codeTest = test.CodeTestArr.Replace(" ", string.Empty);
                if (_regex.IsMatch(_codeTest) == false)
                {
                    ModelState.AddModelError("", "Mã đề không hợp lệ. Chú ý: Nếu muốn tạo nhiều đề thi cùng một lúc, mỗi mã đề cách nhau dấu phẩy.");
                    return false;
                }
                else
                {
                    var _codeTestArr = Regex.Split(_codeTest, ",");
                    var _exam = new ExamDAO().GetExamById(test.ExamID);
                    var _testList = new TestDAO().GetAllTestByExam(_exam);
                    string temp = null;
                    for (int i = 0; i < _codeTestArr.Length; i++)
                    {
                        if (_codeTestArr[i] == temp)
                        {
                            ModelState.AddModelError("", "Mã đề không hợp lệ. Chú ý: Nếu muốn tạo nhiều đề thi cùng một lúc, mỗi mã đề cách nhau dấu phẩy, mã đề phải khác nhau.");
                            return false;
                        }
                        else
                        {
                            for (int j = 0; j < _testList.Count; j++)
                            {
                                if (_testList[j].CodeTest.ToString() == _codeTestArr[i])
                                {
                                    ModelState.AddModelError("", "Mã đề không hợp lệ. Mã đề phải khác nhau.");
                                    return false;
                                }
                            }
                        }
                        temp = _codeTestArr[i];
                    }
                    test.CodeTestArr = _codeTest;
                }
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
            
            
            return true;
        }

        private void CheckCodeTest(Test test)
        {
            if (!String.IsNullOrEmpty(test.CodeTestArr))
            {
                List<int> _list = new List<int>();
                if (!Regex.IsMatch(test.CodeTestArr, "[A-Za-z0-9]") || !Regex.IsMatch(test.CodeTestArr, "[A-Za-z0-9][,]"))
                {
                    ModelState.AddModelError("", "Mã đề không hợp lệ");
                }

            }
        }

        // Chọn câu hỏi
        private bool SelectQuiz(Test test)
        {
            var _exam = new ExamDAO().GetExamById(test.ExamID);
            // Chọn câu hỏi theo kiểu ngẫu nhiên (random)
            if (test.QuizSelection == ConstantVariable.RandomQuiz)
            {
                //Lấy tất cả câu hỏi theo môn học từ kỳ thi đã chọn
                var _quizList = new QuizDAO().GetAllQuizBySubject(_exam.SubjectID);
                var _codeTestArr = Regex.Split(test.CodeTestArr, ",");  //Trả về mảng mã đề
                var _session = Session[ConstantVariable.USER_SESSION] as UserLogin; // Thông tin user đăng nhập

                //Hình thức: không cố định câu hỏi / khác nhau ở mỗi đề
                if (test.FixedOrChanged == ConstantVariable.ChangedQuiz)
                {
                    //Kiểm tra số câu hỏi có đủ hay không
                    if ((test.NumberOfQuestions * _codeTestArr.Length) > _quizList.Count)
                    {
                        ModelState.AddModelError("", "Số câu hỏi trong ngân hàng câu hỏi không đủ để tạo đề thi này.");
                        return false;
                    }
                    List<int> _quizIdList = new List<int>();
                    for (int i = 0; i < _codeTestArr.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(_codeTestArr[i]))
                        {
                            test.CodeTest = int.Parse(_codeTestArr[i]); //Mã đề
                            Random random = new Random();
                            List<int> _quizIdListByCodeTest = new List<int>();
                            for (int j = 0; j < test.NumberOfQuestions; j++)
                            {
                                int quizId;
                                do
                                {
                                    quizId = _quizList[random.Next(0, _quizList.Count - 1)].Id;
                                } while (_quizIdList.Contains(quizId));
                                _quizIdListByCodeTest.Add(quizId);
                            }

                            test.CreatedBy = _session.UserID;   //Người tạo
                            test.CreatedDate = DateTime.Now;
                            test.ModifiedDate = DateTime.Now;

                            var r = new TestDAO().Insert(test, _quizIdListByCodeTest);
                            if (r == false)
                            {
                                return r;
                            }

                            new SystemLogDAO().Insert("Tạo đề thi thành công [Mã đề: " + test.CodeTest + "] [Kỳ thi: " + _exam.Titile + "]", _session.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                        }
                    }//end:For                    
                }
                //Hình thức: cố định câu hỏi
                else if (test.FixedOrChanged == ConstantVariable.FixedQuiz)
                {
                    //Kiểm tra số câu hỏi có đủ hay không
                    if (test.NumberOfQuestions > _quizList.Count)
                    {
                        ModelState.AddModelError("", "Số câu hỏi trong ngân hàng câu hỏi không đủ để tạo đề thi này.");
                        return false;
                    }
                    //Trộn câu hỏi
                    if (test.Mix == ConstantVariable.MixQuiz)
                    {
                        List<int> _quizIdList = new List<int>();
                        bool r = false;
                        for (int i = 0; i < _codeTestArr.Length; i++)
                        {
                            if (!String.IsNullOrEmpty(_codeTestArr[i]))
                            {
                                test.CodeTest = int.Parse(_codeTestArr[i]); //Mã đề
                                Random random = new Random();

                                if (i == 0)
                                {
                                    for (int j = 0; j < test.NumberOfQuestions; j++)
                                    {
                                        int quizId;
                                        do
                                        {
                                            quizId = _quizList[random.Next(0, _quizList.Count - 1)].Id;
                                        } while (_quizIdList.Contains(quizId));
                                        _quizIdList.Add(quizId);
                                    }

                                    test.CreatedBy = _session.UserID;   //Người tạo
                                    test.CreatedDate = DateTime.Now;
                                    test.ModifiedDate = DateTime.Now;

                                    r = new TestDAO().Insert(test, _quizIdList);
                                    if (r == false)
                                    {
                                        return r;
                                    }
                                    new SystemLogDAO().Insert("Tạo đề thi thành công [Mã đề: " + test.CodeTest + "] [Kỳ thi: " + _exam.Titile + "]", _session.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                                }
                                else
                                {
                                    List<int> _quizIdListMixQuiz = new List<int>();
                                    for (int j = 0; j < test.NumberOfQuestions; j++)
                                    {
                                        int quizId;
                                        do
                                        {
                                            int index = random.Next(0, _quizIdList.Count - 1);
                                            quizId = _quizIdList[index];

                                        } while (_quizIdListMixQuiz.Contains(quizId));

                                        _quizIdListMixQuiz.Add(quizId);                                       
                                    }
                                    test.CreatedBy = _session.UserID;   //Người tạo
                                    test.CreatedDate = DateTime.Now;
                                    test.ModifiedDate = DateTime.Now;
                                    r = new TestDAO().Insert(test, _quizIdList);
                                    if (r == false)
                                    {
                                        return r;
                                    }
                                    new SystemLogDAO().Insert("Tạo đề thi thành công [Mã đề: " + test.CodeTest + "] [Kỳ thi: " + _exam.Titile + "]", _session.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
                                }
                            }
                        }//end:For
                    }//end-if: trộn câu hỏi
                }

            }
            // Chọn câu hỏi theo kiểu câu hỏi mới nhất
            if (test.QuizSelection == ConstantVariable.NewQuiz)
            {
                var _quizList = new QuizDAO().GetAllQuizNewSubject(_exam.SubjectID);
                if (_quizList.Count < test.NumberOfQuestions)
                {
                    ModelState.AddModelError("", "Số câu hỏi trong ngân hàng câu hỏi không đủ để tạo đề thi này.");
                    return false;
                }
                else
                {
                    List<int> _quizIdList = new List<int>();
                    for (int i = 0; i < test.NumberOfQuestions; i++)
                    {
                        _quizIdList.Add(_quizList[i].Id);
                    }
                    var _session = Session[ConstantVariable.USER_SESSION] as UserLogin;
                    test.CreatedBy = _session.UserID;
                    test.CreatedDate = DateTime.Now;
                    test.ModifiedDate = DateTime.Now;
                    return new TestDAO().Insert(test, _quizIdList);
                }
            }
            return true;
        }
    }
}

