using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class KindQuizDAO
    {
        QuizManagementSystemDbContext db = null;
        public KindQuizDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public List<Kind> GetAllKindQuiz()
        {
            return db.Kinds.ToList();
        }
    }
}
