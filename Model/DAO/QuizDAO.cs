﻿using System;
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

        public IEnumerable<Question> GetAllQuizPageList(int page = 1, int pageSize = 10)
        {
            return db.Questions.OrderByDescending(x => x.DateCreated).ToPagedList(page, pageSize);
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
