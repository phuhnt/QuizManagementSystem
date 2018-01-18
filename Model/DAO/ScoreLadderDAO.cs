using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class ScoreLadderDAO
    {
        QuizManagementSystemDbContext db = null;
        public ScoreLadderDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public List<ScoreLadder> GetAll()
        {
            return db.ScoreLadders.ToList();
        }
    }
}
