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
    }
}
