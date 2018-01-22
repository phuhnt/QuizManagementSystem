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
using Model.Common;

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
            IQueryable<User> model = db.Users;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) ||
                                    x.UserGroup.Name.Contains(searchString) ||
                                    x.Email.Contains(searchString));
            }
            return model.OrderByDescending(x => x.DateOfParticipation).ToPagedList(page, pageSize);
        }

        public List<User> GetAllUser()
        {
            return db.Users.ToList();
        }

        public List<User> GetAllUserActive()
        {
            return db.Users.Where(x => x.Status == true).ToList();
        }

        public List<User> GetAllUserByClass (string searchString, Class c)
        {
            IQueryable<User> model = db.Users;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) ||
                                    x.FullName.Contains(searchString)      ||
                                    x.Sex.Contains(searchString)           ||                                    
                                    x.Email.Contains(searchString)         ||
                                    x.Phone.Contains(searchString));
            }
            return model.Where(x => x.ClassID == c.Id).OrderByDescending(x => x.UserName).ToList();
        }
        public List<User> GetAllUserByClass(int? id)
        {
            return db.Users.Where(x => x.ClassID == id).ToList();
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

        public bool Delete(int? id)
        {
            try
            {
                var _user = db.Users.Find(id);
                db.Users.Remove(_user);
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
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

        public int Login(string userName, string passWordHash, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0; //Tài khoản không tồn tại
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupID == ConstantVariable.ADMIN_GROUP || result.GroupID == ConstantVariable.TEACHER_GROUP)
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
                    else
                    {
                        return -3;  // Không phải là Admin và Teacher đang nhập
                    }
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
        }

        public List<string> GetListCredential(string userName)
        {
            var _user = db.Users.Single(x => x.UserName == userName);
            return null;
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
