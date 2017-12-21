using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.DAO;

namespace Model.DAO
{
    public class UserRoleDAO
    {
        QuizManagementSystemDbContext db = null;

        public UserRoleDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

    }
}
