using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class TestDAO
    {
        QuizManagementSystemDbContext db = null;
        public TestDAO()
        {
            db = new QuizManagementSystemDbContext();
        } 

        public bool Insert(Test test, List<int> quizIdList)
        {
            test.Questions = new List<Question>();
            for (int i = 0; i < quizIdList.Count; i++)
            {
                var _id = quizIdList[i];              
                test.Questions.Add(db.Questions.Single(c => c.Id == _id));
            }           
            db.Tests.Add(test);
            db.SaveChanges();
            return true;
        }

        public Test GetTestById (int? id)
        {
            return db.Tests.Find(id);
        }
        public List<Test> GetAllTest()
        {
            return db.Tests.ToList();
        }

        public List<Test> GetAllTestActive()
        {
            return db.Tests.Where(x => x.Status == true).ToList();
        }

        public List<Question> GetAllQuiz(int? testsId)
        {
            var test = new TestDAO().GetTestById(testsId);
            return test.Questions.ToList();
        }

        // Lấy danh sách đề thi và phân trang
        public IEnumerable<Test> GetAllTestPageList(string searchString, int page = 1, int pageSize = 10)
        {

            IQueryable<Test> model = db.Tests;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Exam.Titile.Contains(searchString) ||
                                    x.CodeTest.ToString().Contains(searchString) ||
                                    x.Subject.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ThenBy(x => x.Status).ToPagedList(page, pageSize);
        }
    }
}
