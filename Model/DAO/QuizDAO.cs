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

        public List<Question> GetAllQuiz()
        {
            var questions = db.Questions.Include(q => q.CategoryQuiz).Include(q => q.Kind).Include(q => q.Level).Include(q => q.User);
            return questions.ToList();
        }

        public List<Question> GetAllQuizActive()
        {
            return db.Questions.Where(x => x.Status == true).ToList();
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
                var _subjDao = new SubjectDAO();
                var _subj = new Subject();
                _subj = _subjDao.GetSubjectByName(searchString);
                if (_subj != null)
                {                   
                    model = model.Where(x => x.ContentQuestionEncode.Contains(searchString) ||
                                             x.ContentQuestion.Contains(searchString) ||
                                             x.Level.Name.Contains(searchString) ||
                                             x.SubjectsID.ToString().Contains(_subj.Id.ToString()));
                }
                else
                {
                    model = model.Where(x => x.ContentQuestionEncode.Contains(searchString) ||
                                             x.ContentQuestion.Contains(searchString) ||
                                             x.Level.Name.Contains(searchString));
                }
               
            }
            return model.OrderByDescending(x => x.ModifiedDate).ToPagedList(page, pageSize);
        }

        public Question FindQuizById(int? id)
        {
            return db.Questions.Find(id);
        }

        //Quiz: Insert
        public int Insert(Question quiz)
        {
            db.Questions.Add(quiz);
            db.SaveChanges();
            return quiz.Id;
        }

        public bool Update(Question quiz)
        {
            var _quizCurrent = FindQuizById(quiz.Id);
            db.Entry(_quizCurrent).CurrentValues.SetValues(quiz);
            db.SaveChanges();
            return true;
        }

    }
}
