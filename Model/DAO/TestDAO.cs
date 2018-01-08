using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class TestDAO
    {
        QuizManagementSystemDbContext db = null;
        public TestDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

    }
}
