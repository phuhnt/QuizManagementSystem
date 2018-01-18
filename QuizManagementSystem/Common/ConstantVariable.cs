using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizManagementSystem.Common
{
    public static class ConstantVariable
    {
        //Session
        public static string USER_SESSION = "USER_SESSION";


        //Account
        public static int NotExist          =  0; // Tài khoản không tồn tại
        public static int IsLocked          = -1; // Tài khoản đang bị khóa
        public static int Incorrect         = -2; // Tài khoản hoặc mật khẩu chưa đúng
        public static int NotHaveAccess     = -3; // Tài khoản không có quyền truy cập

        // Tests
        public static int Random            =  0;
        public static int QuizNew           =  1;
        public static int Manual            =  2;
    }
}