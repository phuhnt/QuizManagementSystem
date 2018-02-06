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

        public bool Update(Test test, List<int> quizIdList)
        {

            return true;
        }

        public bool UpdateMixQuiz(Test test)
        {
            var _testCurrent = GetTestById(test.Id);

            _testCurrent.MixQuiz = test.MixQuiz;

            db.SaveChanges();
            return true;
        }

        public bool UpdateMixAnswer(Test test)
        {
            var _testCurrent = GetTestById(test.Id);

            _testCurrent.MixAnswer = test.MixAnswer;

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

        public List<Test> GetAllTestByExam(Exam exam)
        {
            return db.Tests.Where(x => x.ExamID == exam.Id).ToList();
        }

        public List<Test> GetAllTestByExam(int? examId)
        {
            return db.Tests.Where(x => x.ExamID == examId).ToList();
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

        public Test GetTestByCodeTest(int? codeTest, int? examId)
        {
            return db.Tests.SingleOrDefault(x => x.CodeTest == codeTest && x.ExamID == examId);
        }

        // Lấy danh sách đề thi và phân trang
        public IEnumerable<Test> GetAllTestPageList(string searchString, int page = 1, int pageSize = 10)
        {

            IQueryable<Test> model = db.Tests;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Exam.Titile.Contains(searchString) ||
                                    x.CodeTest.ToString().Contains(searchString) ||
                                    x.Exam.Subject.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ThenBy(x => x.Status).ToPagedList(page, pageSize);
        }
    }
}
