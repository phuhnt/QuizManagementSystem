using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class LevelQuizDAO
    {
        QuizManagementSystemDbContext db = null;
        public LevelQuizDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public List<Level> GetAllLevelQuiz()
        {
            return db.Levels.ToList();
        }
    }
}
