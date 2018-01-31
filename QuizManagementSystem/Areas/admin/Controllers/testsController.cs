﻿using System;
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
        public ActionResult Create([Bind(Include = "Id,CodeTest,Title,Note,NumberOfQuestions,Time,ExamID,ScoreLadderID,Status")] Test test)
        {
            var _userSession = Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (CheckInputTest(test))
            {
                if (ModelState.IsValid)
                {
                    if (SelectQuiz(test))
                    {
                        SetAlert("Tạo đề thi thành công", "success");
                        new SystemLogDAO().Insert("Tạo đề thi thành công [Mã đề: " + test.CodeTest + "] [Kỳ thi: " + test.Exam.Titile + "]", _userSession.UserName, DateTime.Now.TimeOfDay, DateTime.Now.Date, GetIPAddress.GetLocalIPAddress());
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
            SetSchoolYearViewBag();
            SetGradeViewBag();
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
            //if (test.SubjectID <= 0 || test.SubjectID == null)
            //{
            //    ModelState.AddModelError("", "Vui lòng chọn môn thi");
            //    return false;
            //}
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
            //if (test.NumberOfTurns <= 0)
            //{
            //    ModelState.AddModelError("", "Số lượt làm bài không hợp lệ");
            //    return false;
            //}
            if (test.Status == true)
            {
                //if (test.FromDate == null)
                //{
                //    ModelState.AddModelError("", "Vui lòng chọn ngày bắt đầu thi");
                //    return false;
                //}
                //else if (test.ToDate == null)
                //{
                //    ModelState.AddModelError("", "Vui lòng chọn ngày kết thúc thi");
                //    return false;
                //}
                //else
                //{
                //    if (test.ToDate < test.FromDate)
                //    {
                //        ModelState.AddModelError("", "Ngày kết thúc thi không thể nhỏ hơn ngày bắt đầu thi");
                //        return false;
                //    }
                //}
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
            // Chọn câu hỏi theo kiểu ngẫu nhiên
            if (test.QuizSelection == Common.ConstantVariable.Random)
            {
                var _quizList = new QuizDAO().GetAllQuizBySubject(test.Exam.SubjectID);
                if (_quizList.Count < test.NumberOfQuestions)
                {
                    ModelState.AddModelError("", "Số câu hỏi trong ngân hàng câu hỏi không đủ để tạo đề thi này.");
                    return false;
                }
                else //Random chọn câu hỏi
                {
                    Random random = new Random();
                    List<int> _quizIdList = new List<int>();
                    for (int i = 0; i < test.NumberOfQuestions; i++)
                    {
                        int quizId;
                        do
                        {
                            quizId = _quizList[random.Next(0, _quizList.Count - 1)].Id;
                        } while (_quizIdList.Contains(quizId));
                        _quizIdList.Add(quizId);
                    }
                    var _session = Session[ConstantVariable.USER_SESSION] as UserLogin;
                    test.CreatedBy = _session.UserID;
                    test.CreatedDate = DateTime.Now;
                    test.ModifiedDate = DateTime.Now;
                    return new TestDAO().Insert(test, _quizIdList);
                }
            }
            // Chọn câu hỏi theo kiểu câu hỏi mới nhất
            if (test.QuizSelection == Common.ConstantVariable.QuizNew)
            {
                var _quizList = new QuizDAO().GetAllQuizNewSubject(test.Exam.SubjectID);
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

