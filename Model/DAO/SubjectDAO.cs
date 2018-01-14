using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class SubjectDAO
    {
        QuizManagementSystemDbContext db = null;
        public SubjectDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public List<Subject> GetAllSubjects()
        {
            return db.Subjects.ToList();
        }

        public Subject GetSubjectById(int? id)
        {
            return db.Subjects.Find(id);
        }

        public Subject GetSubjectByName(string name)
        {
            return db.Subjects.SingleOrDefault(x => x.Name.Contains(name));
        }

        public List<Subject> GetSubjectByClassID(int? id)
        {
            return db.Subjects.Where(x => x.GradeID == id).ToList();
        }
    }
}
