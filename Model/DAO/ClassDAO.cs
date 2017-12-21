using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

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

    }
}
