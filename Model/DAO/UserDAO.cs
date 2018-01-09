using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class UserDAO
    {
        QuizManagementSystemDbContext db = null;
        public UserDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public IEnumerable<User> GetAllUserPageList(int page = 1, int pageSize = 10)
        {
            return db.Users.OrderByDescending(x => x.DateOfParticipation).ToPagedList(page, pageSize);
        }

        public IEnumerable<User> GetAllUserPageList(string searchString, int page = 1, int pageSize = 10)
        {
            IQueryable<User> model = db.Users.OrderByDescending(x => x.DateOfParticipation);
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) ||
                                    x.Role.Name.Contains(searchString) ||
                                    x.Email.Contains(searchString));
            }
            return model.OrderByDescending(x => x.DateOfParticipation).ToPagedList(page, pageSize);
        }

        public List<User> GetAllUser()
        {
            return db.Users.ToList();
        }


        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public bool Update(User user)
        {
            var _user = db.Users.Find(user.Id);

            //_user.RoleID = user.RoleID;
            //_user.Status = user.Status;

            //_user.FullName = user.FullName;
            //_user.Sex = user.Sex;
            //_user.Email = user.Email;
            //_user.DayOfBirth = user.DayOfBirth;
            //_user.Phone = user.Phone;
            //_user.ClassID = user.ClassID;

            db.Entry(_user).CurrentValues.SetValues(user);
            db.SaveChanges();
            return true;

        }

        // Update khi không cập nhật mật khẩu mới
        public bool Update(User user, string password)
        {
            var _user = db.Users.Find(user.Id);

            user.PasswordHash = _user.PasswordHash;
            user.ConfirmPasswordHash = _user.PasswordHash;
            user.NewPasswordHash = _user.NewPasswordHash;

            db.Entry(_user).CurrentValues.SetValues(user);
            db.SaveChanges();
            return true;

        }

        public int Login(string userName, string passWordHash)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0; //Tài khoản không tồn tại
            }
            else
            {
                if (result.PasswordHash == passWordHash)
                {
                    return 1; //Đăng nhập đúng
                }
                else if (result.Status == false)
                    return -1; //Tài khoản đang bị khóa
                else
                    return -2; //Đăng nhập sai
            }
        }

        public User GetUserByUserName(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public User GetUserById(int? id)
        {
            return db.Users.Find(id);
        }

        public User IsUserNameExist(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }

    }
}
