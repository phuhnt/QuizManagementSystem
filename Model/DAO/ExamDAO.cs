using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class ExamDAO
    {
        QuizManagementSystemDbContext db = null;
        public ExamDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public int Insert(Exam exam)
        {
            db.Exams.Add(exam);            
            db.SaveChanges();
            return exam.Id;
        }
        public bool Insert(Exam exam, int[] selectedID)
        {
            var _classDao = new ClassDAO();
            exam.Classes = new List<Class>();
            
            foreach (var i in selectedID)
            {
                var _class = new Class();
                if (i == 0)
                {
                    //Chọn tất cả lớp
                    int[] allClass = _classDao.GetAllClassID();
                    for (int k = 0; k < allClass.Length; k++)
                    {
                        //var c = _classDao.GetClassById(k);                       
                        var _id = allClass[k];
                        exam.Classes.Add(db.Classes.Single(c => c.Id == _id));
                    }
                    break;
                }
                else
                {
                    _class = _classDao.GetClassById(i);
                   
                    exam.Classes.Add(db.Classes.Single(c => c.Id == i));
                    //exam.Classes.Add(_class);
                }
            }
            db.Exams.Add(exam);
            db.SaveChanges();
            return true;
        }

        public bool Update(Exam exam)
        {
            var _exam = db.Exams.Find(exam.Id);      

            _exam.Titile = exam.Titile;
            _exam.Note = exam.Note;
            _exam.NoteEncode = exam.NoteEncode;
            _exam.SubjectID = exam.SubjectID;
            _exam.NumberOfTurns = exam.NumberOfTurns;
            _exam.FromDate = exam.FromDate;
            _exam.ToDate = exam.ToDate;
            _exam.StartTime = exam.StartTime;
            _exam.EndTime = exam.EndTime;
            _exam.ModifiedBy = exam.ModifiedBy;
            _exam.ModifiedDate = exam.ModifiedDate;
            _exam.Status = exam.Status;

            db.SaveChanges();
            return true;
        }

        public bool UpdateStatus(Exam exam)
        {
            var _exam = db.Exams.Find(exam.Id);

            _exam.Status = exam.Status;

            db.SaveChanges();
            return true;
        }

        public bool Update(Exam exam, int[] selectedID)
        {
            var _exam = db.Exams.Find(exam.Id);
            var _classDao = new ClassDAO();
            
            foreach (var i in _exam.Classes.ToList())
            {
                _exam.Classes.Remove(i);
            }
            db.SaveChanges();
            _exam.Classes = new List<Class>();
            
            foreach (var i in selectedID)
            {
                var _class = new Class();
                if (i == 0) //Chọn tất cả lớp
                {
                    int[] allClass = _classDao.GetAllClassID();
                    for (int k = 0; k < allClass.Length; k++)
                    {                     
                        var _id = allClass[k];
                        _exam.Classes.Add(db.Classes.Single(c => c.Id == _id));
                    }
                    break;
                }
                else
                {
                    _class = _classDao.GetClassById(i);
                    _exam.Classes.Add(db.Classes.Single(c => c.Id == i));
                }
            }
            //db.Entry(_exam).CurrentValues.SetValues(exam);
            _exam.Titile = exam.Titile;
            _exam.Note = exam.Note;
            _exam.NoteEncode = exam.NoteEncode;
            _exam.SubjectID = exam.SubjectID;
            _exam.NumberOfTurns = exam.NumberOfTurns;
            _exam.FromDate = exam.FromDate;
            _exam.ToDate = exam.ToDate;
            _exam.StartTime = exam.StartTime;
            _exam.EndTime = exam.EndTime;
            _exam.ModifiedBy = exam.ModifiedBy;
            _exam.ModifiedDate = exam.ModifiedDate;
            _exam.Status = exam.Status;

            db.SaveChanges();
            return true;
        }

        public bool Delete(Exam exam)
        {
            var _exam = GetExamById(exam.Id);
            foreach (var i in _exam.Classes.ToList())
            {
                _exam.Classes.Remove(i);
            }
            db.Exams.Remove(_exam);
            db.SaveChanges();
            return true;
        }

        public Exam GetExamById(int? id)
        {
            return db.Exams.Find(id);
        }

        public IEnumerable<Exam> GetAllExamPageList(string searchString, int page = 1, int pageSize = 10)
        {
            IQueryable<Exam> model = db.Exams;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Titile.Contains(searchString) ||
                                    x.NoteEncode.Contains(searchString) ||
                                    x.Note.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ThenBy(x => x.Status).ToPagedList(page, pageSize);
        }

        public IEnumerable<Exam> GetAllExamActivePageList(string searchString, int page = 1, int pageSize = 10)
        {
            IQueryable<Exam> model = db.Exams;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Status == true && (x.Titile.Contains(searchString) ||
                                    x.NoteEncode.Contains(searchString) ||
                                    x.Note.Contains(searchString)));
            }
            else
            {
                model = model.Where(x => x.Status == true);
            }
            return model.OrderByDescending(x => x.CreatedDate).ThenBy(x => x.Status).ToPagedList(page, pageSize);
        }

        public IEnumerable<Exam> GetAllExamUserPageList(int? id, string searchString, int page = 1, int pageSize = 10)
        {
            var user = new UserDAO().GetUserById(id);
            var _class = new ClassDAO().GetClassById(user.ClassID);
            IQueryable<Exam> model = db.Exams;
            List<Exam> queryable = new List<Exam>();
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Titile.Contains(searchString) ||
                                    x.NoteEncode.Contains(searchString) ||
                                    x.Note.Contains(searchString));
            }
            
            if (_class.Exams != null)
            {
                foreach (var item in _class.Exams.ToList())
                {
                    var e = model.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (e != null)
                    {
                        queryable.Add(e);
                    }  
                }
            }
            queryable.AsQueryable();
            return queryable.OrderByDescending(x => x.CreatedDate).ThenBy(x => x.Status).ToPagedList(page, pageSize);
        }

        public List<Class> GetClassSelected(Exam exam)
        {
            return exam.Classes.ToList();
        }

        public List<Class> GetClassSelected(int id)
        {
            var exam = GetExamById(id);
            return exam.Classes.ToList();
        }

        public List<Exam> GetAllExamActive()
        {
            var model = db.Exams.Where(x => x.Status == true);
            return model.OrderByDescending(x => x.Id).ToList();
        }

    }
}
