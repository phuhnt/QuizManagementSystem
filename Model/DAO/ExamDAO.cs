using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class ExamDAO
    {
        QuizManagementSystemDbContext db = null;
        public ExamDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public int Insert(Exam exam)
        {
            db.Exams.Add(exam);
            db.SaveChanges();
            return exam.Id;
        }

        public Exam GetExamById(int? id)
        {
            return db.Exams.Find(id);
        }

        public IEnumerable<Exam> GetAllExamPageList(int page = 1, int pageSize = 10)
        {
            return db.Exams.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}
