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

namespace Model.DAO
{
    public class ClassDAO
    {
        QuizManagementSystemDbContext db = null;
        public ClassDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public List<Class> GetAll()
        {
            return db.Classes.Where(x => x.Status == true).ToList();
        }

        public List<Class>GetAllBySubjectID(int? id)
        {
            return db.Classes.Include(c => c.Subjects).ToList();
        }

        public Class GetClassBySubjectID(Subject sub)
        {
            return db.Classes.FirstOrDefault(x => x.Id == sub.Id);
        }
    }
}
