﻿using System;
using System.Collections.Generic;
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

        public int Insert(User user, Role role)
        {
            db.Users.Add(user);
            db.SaveChanges();

            Role _role = db.Roles.SingleOrDefault(x => x.Id == role.Id);
            
            user.Roles = new List<Role>();
            user.Roles.Add(_role);

            db.SaveChanges();
            return user.Id;
        }

        public bool Update(User user, Role role)
        {
            try
            {
                var _user = db.Users.Find(user.Id);
                _user.PasswordHash = user.PasswordHash;

                var _userRole = _user.Roles.FirstOrDefault(x => x.Id == _user.Id); //Role của user hiện tại
                Role _role = db.Roles.SingleOrDefault(x => x.Id == role.Id); //Role mới
                user.Roles = new List<Role>();
                user.Roles.Remove(_userRole);
                user.Roles.Add(_role);

                _user.Status = user.Status;
                _user.FullName = user.FullName;
                _user.Sex = user.Sex;
                _user.Email = user.Email;
                _user.DayOfBirth = user.DayOfBirth;
                _user.Phone = user.Phone;
                _user.ClassID = user.ClassID;
                db.SaveChanges();
                return true;  
            }
            catch
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
