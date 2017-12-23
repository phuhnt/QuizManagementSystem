using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class CategoryQuizDAO
    {
        QuizManagementSystemDbContext db = null;
        public CategoryQuizDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public List<CategoryQuiz> GetAllCategoryQuiz()
        {
            return db.CategoryQuizs.ToList();
        }
    }
}
