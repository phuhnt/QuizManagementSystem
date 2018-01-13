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
                                    x.SchoolYear.NameOfSchoolYear.Contains(searchString));
            }
            return model.OrderByDescending(x => x.Name).ToPagedList(page, pageSize);
        }

        public List<Class>GetAllBySubjectID(int? id)
        {
            return db.Classes.Include(c => c.Subjects).ToList();
        }

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
    }
}
