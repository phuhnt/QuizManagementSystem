using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class TestResultDetailDAO
    {
        QuizManagementSystemDbContext db = null;
        public TestResultDetailDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public int Insert(TestResultDetail testResultDetail)
        {
            db.TestResultDetails.Add(testResultDetail);
            db.SaveChanges();
            return testResultDetail.Id;
        }

        public TestResultDetail GetById(int? id)
        {
            return db.TestResultDetails.Find(id);
        }

        public TestResultDetail GetTestResult(int? userId, int? examId)
        {
            var model = db.TestResultDetails.Where(x => x.UserID == userId && x.ExamID == examId);
            return model.OrderByDescending(x => x.TimeToTake).First();
        }

        public List<TestResultDetail> GetAll(int? userId, int? examId)
        {
            return db.TestResultDetails.Where(x => x.UserID == userId && x.ExamID == examId).ToList();
        }

        public IEnumerable<TestResultDetail> GetAllTestResultPageList(User user, string searchString, int page = 1, int pageSize = 10)
        {
            IEnumerable<TestResultDetail> model = db.TestResultDetails;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Exam.Titile.Contains(searchString) ||
                                    x.Exam.Subject.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.ActualTestDate).ToPagedList(page, pageSize);
        }
    }
}
