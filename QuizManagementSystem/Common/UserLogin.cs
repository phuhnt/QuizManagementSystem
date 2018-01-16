using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizManagementSystem.Common
{
    [Serializable]
    public class UserLogin
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public int? RoleID { get; set; }

        public string Avatar { get; set; }
    }
}