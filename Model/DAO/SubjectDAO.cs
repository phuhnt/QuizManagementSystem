using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class SubjectDAO
    {
        QuizManagementSystemDbContext db = null;
        public SubjectDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        // Insert
        public int Insert(Subject s)
        {
            db.Subjects.Add(s);
            db.SaveChanges();
            return s.Id;
        }

        // Update
        public bool Update(Subject s)
        {
            var _subj = db.Subjects.Find(s.Id);
            db.Entry(_subj).CurrentValues.SetValues(s);
            db.SaveChanges();
            return true;
        }

        // Delete
        public int Delete(int id)
        {
            var _subj = GetSubjectById(id);
            if (_subj.Exams.Count > 0)
            {
                return 1; // Môn học có chứa kỳ thi
            }
            if (db.Questions.Where(x => x.SubjectsID == id).FirstOrDefault().Id > 0)
            {
                return 2; //Môn học có chứa coau6 hỏi
            }
            db.Subjects.Remove(_subj);
            db.SaveChanges();
            return 0;
        }


        public List<Subject> GetAllSubjects()
        {
            return db.Subjects.ToList();
        }

        public Subject GetSubjectById(int? id)
        {
            return db.Subjects.Find(id);
        }

        public bool IsExistNameSubject(Subject s)
        {
            if (db.Subjects.Where(x => x.Name == s.Name).FirstOrDefault() != null)
                return true;
            return false;
        }

        public List<Subject> GetSubjectByClassID(int? id)
        {
            var _class = new ClassDAO().GetClassById(id);
            if (_class != null)
            {
                return db.Subjects.Where(x => x.GradeID == _class.GradeID).ToList();
            }
            return null;
        }

        public List<Subject> GetAllSubjectsByGrade(int? id)
        {
            return db.Subjects.Where(x => x.GradeID == id).ToList();
        }

        public IEnumerable<Subject> GetAllSubjectsPageList(string searchString, int page = 1, int pageSize = 10)
        {
            IQueryable<Subject> model = db.Subjects;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Id.ToString().Contains(searchString) ||
                                    x.Grade.GradeName.Contains(searchString) ||
                                    x.Name.Contains(searchString) ||
                                    x.Grade.SchoolYear.NameOfSchoolYear.Contains(searchString) ||
                                    x.Note.Contains(searchString) ||
                                    x.Name.Contains(searchString == "Hoạt động" ? "True" : "False"));
            }
            return model.OrderByDescending(x => x.Grade.SchoolYear.StartYear).ToPagedList(page, pageSize);
        }
    }
}
