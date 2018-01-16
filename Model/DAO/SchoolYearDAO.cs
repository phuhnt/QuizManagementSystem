using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using PagedList;

namespace Model.DAO
{
    public class SchoolYearDAO
    {
        QuizManagementSystemDbContext db;
        public SchoolYearDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public SchoolYear GetById(int id)
        {
            return db.SchoolYears.Find(id);
        }

        public List<SchoolYear> GetAll()
        {
            return db.SchoolYears.ToList();
        }
    }
}
