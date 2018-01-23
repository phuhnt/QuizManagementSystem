using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class UserGroupDAO
    {
        QuizManagementSystemDbContext db = null;

        public UserGroupDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public List<UserGroup> GetAll()
        {
            return db.UserGroups.ToList();
        }
    }
}
