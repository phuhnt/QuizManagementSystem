using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class QuizDAO
    {
        private QuizManagementSystemDbContext db = null;
        public QuizDAO()
        {
            db = new QuizManagementSystemDbContext();
            
        }

        //Quiz: Insert
        public int Insert(Question quiz)
        {
            db.Questions.Add(quiz);
            db.SaveChanges();
            return quiz.Id;
        }

        // Update
        public bool Update(Question quiz)
        {
            var _quizCurrent = FindQuizById(quiz.Id);

            _quizCurrent.SubjectsID = quiz.SubjectsID;
            _quizCurrent.CategoryID = quiz.CategoryID;
            _quizCurrent.KindID = quiz.KindID;
            _quizCurrent.LevelID = quiz.LevelID;
            _quizCurrent.Note = quiz.Note;
            _quizCurrent.ContentQuestion = quiz.ContentQuestion;
            _quizCurrent.ContentQuestionEncode = quiz.ContentQuestionEncode;
            _quizCurrent.AnswerChoiceNum = quiz.AnswerChoiceNum;
            _quizCurrent.AnswerText = quiz.AnswerText;
            _quizCurrent.AnswerTextEncode = quiz.AnswerTextEncode;
            _quizCurrent.KeyAnswer = quiz.KeyAnswer;
            _quizCurrent.ModifiedBy = quiz.ModifiedBy;
            _quizCurrent.ModifiedDate = quiz.ModifiedDate;
            _quizCurrent.Status = quiz.Status;

            db.SaveChanges();
            return true;
        }

        public bool UpdateMixAnswer(Question question)
        {
            var _quizCurrent = GetById(question.Id);

            _quizCurrent.MixAnswer = question.MixAnswer;
            _quizCurrent.MixKeyAnswer = question.MixKeyAnswer;

            db.SaveChanges();
            return true;
        }

        // Delete
        public bool Delete(Question quiz)
        {
            var _quiz = new QuizDAO().FindQuizById(quiz.Id);
            
            if (_quiz.Tests.Count > 0)
            {
                return false;
            }
            else
            {
                db.Questions.Remove(quiz);
            }
            db.SaveChanges();
            return true;
        }

        public Question GetById(int? id)
        {
            return db.Questions.Find(id);
        }

        public List<Question> GetAllQuiz()
        {
            var questions = db.Questions.Include(q => q.CategoryQuiz).Include(q => q.Kind).Include(q => q.Level).Include(q => q.User);
            return questions.ToList();
        }

        public List<Question> GetAllQuizActive()
        {
            return db.Questions.Where(x => x.Status == true).ToList();
        }


        public List<Question> GetAllQuizBySubject(int? id)
        {
            return db.Questions.Where(x => x.SubjectsID == id && x.Status == true).ToList();
        }

        public List<Question> GetAllQuizNewSubject(int? id)
        {
            return db.Questions.Where(x => x.SubjectsID == id && x.Status == true).OrderByDescending(x => x.ModifiedDate).ToList();
        }

        public IEnumerable<Question> GetAllQuizPageList(int page = 1, int pageSize = 10)
        {
            return db.Questions.OrderByDescending(x => x.ModifiedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Question> GetAllQuizPageList(string searchString, int page = 1, int pageSize = 10)
        {
            IQueryable<Question> model = db.Questions;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ContentQuestionEncode.Contains(searchString) ||
                                             x.ContentQuestion.Contains(searchString) ||
                                             x.AnswerText.Contains(searchString) ||
                                             x.AnswerTextEncode.Contains(searchString) ||
                                             x.Level.Name.Contains(searchString));

            }
            return model.OrderByDescending(x => x.ModifiedDate).ToPagedList(page, pageSize);
        }

        public Question FindQuizById(int? id)
        {
            return db.Questions.Find(id);
        }      
    }
}
