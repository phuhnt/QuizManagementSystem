using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using PagedList;

namespace Model.DAO
{
    public class ClassDAO
    {
        QuizManagementSystemDbContext db = null;
        public ClassDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public int Insert(Class c)
        {
            db.Classes.Add(c);
            db.SaveChanges();
            return c.Id;
        }

        public bool Update(Class c)
        {
            var _class = db.Classes.Find(c.Id);
            db.Entry(_class).CurrentValues.SetValues(c);
            db.SaveChanges();
            return true;
        }

        public bool Delete(Class c)
        {
            if (c.Users.Count > 0)
            {
                return false;
            }
            db.Classes.Remove(c);
            db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var _class = GetClassById(id);
            if (_class.Users.Count > 0)
            {
                return false;
            }
            db.Classes.Remove(_class);
            db.SaveChanges();
            return true;
        }

        public List<Class> GetAllClass()
        {
            return db.Classes.ToList();
        }

        public List<Class> GetAllClassActive()
        {
            return db.Classes.Where(x => x.Status == true).ToList();
        }

        public IEnumerable<Class> GetAllClassPageList(string searchString, int page = 1, int pageSize = 10)
        {

            IQueryable<Class> model = db.Classes;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) ||                                    
                                    x.Grade.SchoolYear.NameOfSchoolYear.Contains(searchString));
            }
            return model.OrderByDescending(x => x.Name).ToPagedList(page, pageSize);
        }

        //public List<Class>GetAllBySubjectID(int? id)
        //{
        //    return db.Classes.Include(c => c.Subjects).ToList();
        //}

        public Class GetClassById(int? id)
        {
            return db.Classes.Find(id);
        }

        public int[] GetAllClassID()
        {
            List<Class> listClass = GetAllClass();
            int[] result = new int[listClass.Count];
            int i = 0;
            foreach (var item in listClass)
            {               
                result[i] = item.Id;
                i++;
            }
            return result;
        }

        public List<Class> GetAllClassByGradeId(int? id)
        {
            return db.Classes.Where(x => x.GradeID == id).ToList();
        }

        public bool IsExistNameClass(Class c)
        {
            if (db.Classes.Where(x => x.Name == c.Name).FirstOrDefault() != null)
                return true;
            return false;
        }

        public List<Class> GetAllBySchoolYear(int? schoolyearID)
        {
            return db.Classes.Where(x => x.Grade.SchoolYearID == schoolyearID).ToList();
        }

        public List<Class> GetAllByExams(int? examsID)
        {
            return db.Classes.Where(x => x.Exams.Any(e => e.Id == examsID)).ToList();
        }

    }
}
