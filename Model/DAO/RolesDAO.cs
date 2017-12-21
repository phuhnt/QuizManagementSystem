using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class RolesDAO
    {
        QuizManagementSystemDbContext db = null;

        public RolesDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public List<Role>GetAll()
        {
            return db.Roles.ToList();
        }

        public int FindIdByName(string rolename)
        {
            return db.Roles.SingleOrDefault(x => x.Name == rolename).Id;
        }
    }
}
