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

        public TestResultDetail GetTestResult(int? userId, int? testId, int? examId)
        {
            return db.TestResultDetails.SingleOrDefault(x => x.UserID == userId && x.TestID == testId && x.ExamID == examId);
        }
    }
}
