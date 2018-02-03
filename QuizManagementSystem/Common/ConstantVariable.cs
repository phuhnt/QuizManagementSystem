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
        public static string SESSION_CREDENTIAL = "SESSION_CREDENTIAL";


        //Encode
        public static string htmlTagPattern = "<[^>]*(>|$)";
        public static string pTagPattern = "<p>|</p>";

        //Account
        public static int NotExist          =  0; // Tài khoản không tồn tại
        public static int IsLocked          = -1; // Tài khoản đang bị khóa
        public static int Incorrect         = -2; // Tài khoản hoặc mật khẩu chưa đúng
        public static int NotHaveAccess     = -3; // Tài khoản không có quyền truy cập

        // Tests
        public static int RandomQuiz        =  1; // Ngẫu nhiên
        public static int NewQuiz           =  2; // Câu hỏi mới nhất
        public static int ManualQuiz        =  3; // Thủ công
        public static int FixedQuiz         =  4; // Cố định câu hỏi
        public static int ChangedQuiz       =  5; // Câu hỏi có thể khác nhau
        public static int NoMix             =  6; // Không trộn
        public static int MixQuiz           =  7; // Trộn câu hỏi
        public static int MixAnswer         =  8; // Trộn đáp án
        public static int MixAll            =  9; // Trộn câu hỏi và đáp án


        // UserGroup
        public static string STUDENT_GROUP = "STUDENT";
        public static string TEACHER_GROUP = "TEACHER";
        public static string ADMIN_GROUP   = "ADMIN";
    }
}