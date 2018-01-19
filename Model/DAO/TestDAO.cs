using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class TestDAO
    {
        QuizManagementSystemDbContext db = null;
        public TestDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public List<Test> GetAllTestActive()
        {
            return null;
            //return db.Tests.Where(x => x.Status == true).ToList();
        }

        public bool Insert(Test test, List<int> quizIdList)
        {
            test.Quizs = new List<Question>();
            for (int i = 0; i < quizIdList.Count; i++)
            {
                var _id = quizIdList[i];              
                test.Quizs.Add(db.Questions.Single(c => c.Id == _id));
            }           
            db.Tests.Add(test);
            db.SaveChanges();
            return true;
        }
    }
}
