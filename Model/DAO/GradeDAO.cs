using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class GradeDAO
    {
        QuizManagementSystemDbContext db;
        public GradeDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public List<Grade> GetAll()
        {
            return db.Grades.ToList();
        }

        public Grade GetByClass(Class c)
        {
            return db.Grades.Where(x => x.Id == c.GradeID).FirstOrDefault();
        }
    }
}
